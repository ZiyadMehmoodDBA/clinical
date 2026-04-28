using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Medical;
using System.Linq;
using HtmlAgilityPack;
using System.Text;
using MDVision.Datasets;
using System.Drawing;
using System.Text.RegularExpressions;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using System.Net.Mail;
using System.Xml.Linq;
using System.Xml;
using System.Web.Configuration;

namespace MDVision.IEHR.EMR.Services
{
    public class InfoButtonController : ApiController
    {


        [HttpPost]
        public string GetInfobuttonDetails(JObject allData)
        {
            var datastr = string.Empty;
            var statusstr = true;
            var message = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            dynamic data = ser.Deserialize<dynamic>(MDVUtility.ToStr(allData["data"]));
            var connectPlusResponse = connectMedlinePlus(Convert.ToString(data["searchStr"]), Convert.ToString(data["codeSystem"]));

            string patientId = Convert.ToString(data["PatientId"]);
            
            string noteId = data.ContainsKey("NoteId") == true ? MDVUtility.ToStr(data["NoteId"]) : null;
            
            string parentCtrl = Convert.ToString(data["Caller"]);
            string providerId = Convert.ToString(data["ProviderId"]);
            if (connectPlusResponse.data != null)
            {
                XmlDocument doc = connectPlusResponse.data;
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("feed", "http://www.w3.org/2005/Atom");
                XmlNodeList nodes = doc.SelectNodes("//feed:entry", nsmgr);
                if (nodes != null && nodes.Count > 0)
                {
                    int EntryCounter = 0;
                    var html = "<!DOCTYPE html><html><head></head><body>";
                    html += "<div id='AllContent' style='padding-left: 50px; padding-right: 50px; '>";
                    foreach (XmlNode n in nodes)
                    {
                        EntryCounter++;
                        html += "<div id='entry" + EntryCounter + "'>";
                        foreach (XmlNode subNode in n.ChildNodes)
                        {
                            if (subNode.Name.ToLower() == "title")
                            {
                                html += "<h3>" + subNode.InnerText + "</h3>"; // Title
                            }
                            else if (subNode.Name.ToLower() == "summary")
                            {
                                html += subNode.InnerText;         // HTML      
                            }
                            else if (subNode.Name.ToLower() == "link")
                            {
                                if (parentCtrl == "Clinical_Medications" || parentCtrl == "Clinical_CDSAlertDetail")
                                {
                                    string SummaryUrl = subNode.Attributes["href"].Value;

                                    HtmlWeb web = new HtmlWeb();
                                    HtmlDocument docSummary = web.Load(SummaryUrl);
                                    HtmlNode BreadCrumbs = docSummary.DocumentNode.SelectSingleNode("//div[@id='breadcrumbs']");
                                    if (BreadCrumbs != null)
                                        BreadCrumbs.Remove();

                                    HtmlNode SumNode = docSummary.DocumentNode.SelectSingleNode("//div[@id='mplus-content']");
                                    if (SumNode != null)
                                    {
                                        string SummaryHtmlCode = SumNode.InnerHtml;
                                        html += SummaryHtmlCode;
                                    }
                                }
                            }
                        }
                        html += "</div>";
                    }

                    html += "</div></body></html>";
                    BLObject<byte[]> objLoad = ModifyHtml(html, patientId, parentCtrl, providerId);

                    if (objLoad.Data != null)
                    {
                        var bytearray = objLoad.Data;
                        var base64Str = Convert.ToBase64String(bytearray);
                        message = objLoad.Message;
                        if (!objLoad.Message.Equals("No Information Found"))
                        {

                            datastr = base64Str;// data["FileStream"]; string filename = "";
                            if (message != null && message.IndexOf("/") > -1)
                            {
                                Uri uri = new Uri(message);
                                message = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
                            }
                            //EMR-7189 Now Numerator will be achieved when Patient Education will be attached to Note.
                            //if (noteId != null)
                            //{
                            //    BLObject<DSCCDA> dsCCDAReconcilation = new BLObject<DSCCDA>();
                            //    BLLCCDA _bllCCDAobj = new BLLCCDA();
                            //    dsCCDAReconcilation = _bllCCDAobj.InsertMUSetting(MDVUtility.ToInt64(patientId), 0, 0, MDVUtility.ToInt64(noteId), false, false, false, true, false, 0, 0, false);
                            //}

                        }
                        else
                        {
                            datastr = base64Str;
                        }
                    }
                    else
                    {
                        message = objLoad.Message;
                        statusstr = false;
                    }
                }
                else
                {
                    statusstr = false;
                    message = "No information found !";
                }
            }
            else
            {
                message = connectPlusResponse.Message;
                statusstr = false;
            }

            var response = new
            {
                data = datastr,
                Message = message,
                status = statusstr
            };
            return (JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        public string GetInfobuttonDetail(JObject allData)
        {
            var datastr = string.Empty;
            var statusstr = true;
            var message = string.Empty;
            try
            {


                JavaScriptSerializer ser = new JavaScriptSerializer();
                dynamic data = ser.Deserialize<dynamic>(MDVUtility.ToStr(allData["data"]));
                string noteId = data.ContainsKey("NoteId") == true ? MDVUtility.ToStr(data["NoteId"]) : null;
                PatientEducationModel ptEduModel = new PatientEducationModel
                {
                    PatientId = Convert.ToString(data["PatientId"]),
                    NoteId = noteId,
                    DocType = "1", //Info Data
                    FileType = "application/pdf",
                    DocumentName = Convert.ToString(data["FileName"]),
                    DocumentId = "0",
                    Comments = "",
                    FileStream = Convert.FromBase64String(data["stream"]),
                };

                PatientEducationHelper ptEdu = PatientEducationHelper.Instance();
                var responseptEdu = ptEdu.InsertClinical_PatientEducation(ptEduModel);

                data = ser.Deserialize<dynamic>(responseptEdu);
                statusstr = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                statusstr = false;
            }

            var response = new
            {

                Message = message,
                status = statusstr
            };
            return (JsonConvert.SerializeObject(response));
        }

        private object connectMedlinePlus(string searchStr, string codeSection)
        {
            XmlDocument response = null;
            var error = string.Empty;
            try
            {
                //Web Service Call
                //string url = string.Format("https://apps.nlm.nih.gov/medlineplus/services/mpconnect_service.cfm?mainSearchCriteria.v.c={0}&mainSearchCriteria.v.cs={1}", searchStr, codeSection);
                //Web Application Call
                //  string url = string.Format("https://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.c={0}&mainSearchCriteria.v.cs={1}", searchStr, codeSection);
                string url = string.Format(WebConfigurationManager.AppSettings["MedlineURL"].ToString()
                    + WebConfigurationManager.AppSettings["MedlineParam1"].ToString() + "{1}" + "&"
                    + WebConfigurationManager.AppSettings["MedlineParam2"].ToString() + "{0}", searchStr, codeSection);

                XmlDocument doc = new XmlDocument();
                doc.Load(url);


                if (doc != null)
                {
                    response = doc;
                }
                else
                {
                    error = "No Information Found";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new
            {
                data = response,
                Message = error
            };
        }
        private string B64Encode(string path)
        {
            byte[] imageBytes = null;
            if (path.IndexOf("http", StringComparison.Ordinal) > -1)
            {
                Uri uri = new Uri(path);
                WebClient client = new WebClient();
                try
                {
                    imageBytes = client.DownloadData(uri);
                }
                catch (Exception)
                {
                    //ignore
                }
                finally
                {
                    client.Dispose();
                }
            }
            else
            {
                try
                {
                    using (Image image = Image.FromFile(HttpContext.Current.Server.MapPath(path)))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            imageBytes = m.ToArray();
                        }
                    }
                }
                catch (Exception)
                {
                    //ignore
                }

            }


            if (imageBytes != null)
            {
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }

            return path;
        }

        private BLObject<byte[]> ModifyHtml(string htmlContent, string patientId, string parentCtrl, string providerId)
        {
            try
            {
                htmlContent = RemoveComments(htmlContent);
                string caller = string.Empty;
                var noinfoFound = string.Empty;
                HtmlDocument document = new HtmlDocument();
                HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Empty;
                document.LoadHtml(htmlContent);

                string patientTable = string.Empty;

                //Create Div for Header
                HtmlNode headerDiv = document.CreateElement("div");
                headerDiv.Attributes.Add("id", "mplus-wrap");

                HtmlNode contentDiv = document.CreateElement("div");
                contentDiv.Attributes.Add("id", "mplus-content");

                HtmlNode serviceDiv = document.CreateElement("div");
                serviceDiv.Attributes.Add("id", "serviceContent");

                HtmlNode resultsDiv = document.CreateElement("div");
                resultsDiv.Attributes.Add("class", "results col-xs-12");

                HtmlNode resultDiv = document.CreateElement("div");
                resultDiv.Attributes.Add("class", "result col-xs-12 pl-none pt-sm");

                HtmlNode tblHeading = document.CreateElement("table");
                tblHeading.Attributes.Add("style", "width:100%");

                HtmlNode divStyle = document.CreateElement("div");
                divStyle.Attributes.Add("id", "separatorDiv");
                divStyle.Attributes.Add("style", "float:left; width:100%; border-bottom:22px solid #005da9;margin-top:5px; margin-bottom:5px;");

                HtmlNode bodyNode = document.DocumentNode.SelectSingleNode("html/body");
                HtmlNode contentNode = bodyNode.ChildNodes.FindFirst("div");

                string footerDivBar = "<div id='footerbar' style='float:left;border-top:3px solid #005da9;width:100%;padding-top:3px;'></div>";

                HtmlNode footerNode = document.CreateElement("div");
                footerNode.Attributes.Add("id", "ptEduFooter");
                footerNode.Attributes.Add("style", "float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:5px 25px");
                DSReportHeader dsreportHeader = null;

                BLObject<DSReportHeader> obj = new BLLAdminClinical().getReportHeaderTagsValue(Convert.ToInt64(patientId), Convert.ToInt64(providerId), -1, "Patient Education");
                dsreportHeader = obj.Data;
                if (dsreportHeader != null && dsreportHeader.ReportHeaderTags.Rows.Count > 0)
                {
                    DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];

                    //if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.HeaderLogoColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]))
                    //    && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName])))
                    bool IsCustomHdear = bool.Parse(dsreportHeader.ReportHeaderTags.Rows[0][9].ToString());
                    if (IsCustomHdear == true)
                    {
                        HtmlNode tblBody = document.CreateElement("tbody");
                        HtmlNode tbltr = document.CreateElement("tr");
                        HtmlNode tbltd = document.CreateElement("td");
                        tbltd.SetAttributeValue("style", "width:70%;padding-left:0px;");
                        HtmlNode logoImg = document.CreateElement("img");
                        logoImg.SetAttributeValue("src", MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.HeaderLogoColumn.ColumnName]));
                        logoImg.SetAttributeValue("style", "max-width: 350px;max-height:140px;");
                        tbltd.AppendChild(logoImg);

                        HtmlNode tbltdH = document.CreateElement("td");
                        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        tbltdH.SetAttributeValue("style", "width:30%;vertical-align:top;text-align:right;");
                        //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090

                        var arrPracticeTextColumn = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName]).Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
                        string finalstrPractice = "";
                        foreach (string PracticeColumn in arrPracticeTextColumn)
                        {
                            finalstrPractice += PracticeColumn + "<br/>";
                        }
                        //tbltdH.InnerHtml = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName]);
                        tbltdH.InnerHtml = finalstrPractice;
                        tbltr.AppendChild(tbltd);
                        tbltr.AppendChild(tbltdH);
                        tblBody.AppendChild(tbltr);
                        tblHeading.AppendChild(tblBody);

                        //var arrProvider =  MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName]).Replace("<br/>", "~").Split('~');
                        //var arrPatient = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]).Replace("<br/>", "~").Split('~');
                        //string finalstrProvider = "";
                        //foreach (var item in arrProvider)
                        //{
                        //    var newstr = item.TrimStart().TrimEnd();
                        //    if (newstr != "")
                        //    {
                        //        finalstrProvider += newstr + "<br/>";
                        //    }
                        //}
                        //var finalstrPatient = "";
                        //foreach (var item in arrPatient)
                        //{
                        //    var newstr = item.TrimStart().TrimEnd();
                        //    if (newstr != "")
                        //    {
                        //        finalstrPatient += newstr + "<br/>";
                        //    }
                        //}
                        var arrProvider = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName]).Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
                        var arrPatient = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]).Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
                        string finalstrPatient = "";
                        string finalstrProvider = "";
                        foreach (string ProviderColumn in arrProvider)
                        {
                            finalstrProvider += ProviderColumn + "<br/>";
                        }
                        foreach (string PatientColumn in arrPatient)
                        {
                            finalstrPatient += PatientColumn + "<br/>";
                        }
                        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        patientTable = "<table width='100%' style='padding-left:0px;'>" +
                                      "<tbody>" +
                                      " <tr>" +
                                   "<td width='68%' style='line-height:1.5;padding-left:0px;'>" + finalstrPatient +
                         "</td><td width='32%' style='text-align:right;'>" + finalstrProvider +
                           "<br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                        "<button class='btn btn-default btn-sm mr-xs' type='button' id='btnDownload' onclick='Clinical_InfoButtonView.DownloadInfo();'><i class='fa fa-download'></i> Download</button>" +
                        "<button class='btn btn-default btn-sm' type='button' id='btnEmail' onclick='Clinical_InfoButtonView.SendEmail(" + patientId + ");'><i class='fa fa-envelope-o'></i> Email</button></td></tr></tbody></table>";
                        //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        //footerNode.InnerHtml = "Generated by: " + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName]) + "<span style='float:right;'>Page 1 of 1</span></div><div class='clearfix'></div>";
                        footerNode.InnerHtml = "Generated by: " + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName]) + "</div><div class='clearfix'></div>";
                    }
                    else
                    {
                        HtmlNode tblBody = document.CreateElement("tbody");
                        HtmlNode tbltr = document.CreateElement("tr");
                        HtmlNode tbltd = document.CreateElement("td");
                        tbltd.SetAttributeValue("style", "width:70%;padding-left:25px;");
                        HtmlNode logoImg = document.CreateElement("img");
                        logoImg.SetAttributeValue("src", VirtualPathUtility.ToAbsolute(@"~\content\images\SHS-nav-logo.png"));
                        logoImg.SetAttributeValue("height", "65px");
                        tbltd.AppendChild(logoImg);

                        HtmlNode tbltdH = document.CreateElement("td");
                        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        tbltdH.SetAttributeValue("style", "width:30%; text-align:right;");
                        //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        HtmlNode h3Heading = document.CreateElement("h4");
                        h3Heading.Attributes.Add("class", "text-bold");
                        h3Heading.InnerHtml = "Educational Material";
                        tbltdH.AppendChild(h3Heading);
                        tbltr.AppendChild(tbltd);
                        tbltr.AppendChild(tbltdH);
                        tblBody.AppendChild(tbltr);
                        tblHeading.AppendChild(tblBody);

                        PatientEducationHelper ptEdu = PatientEducationHelper.Instance();
                        DSPatient dspatient = ptEdu.loadDataforPdf(patientId);
                        DSProfile dsProfile = new DSProfile();

                        patientTable = "<table width='100%' style='padding-left:15px;'><tbody><tr><td width='68%' style='line-height:1.5;padding-left:30px;'><span>" +
                           MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.AddressColumn.ColumnName]) + "</span><br/>" +
                          "<span>" + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.CityColumn.ColumnName]) + ", " +
                          MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.StateColumn.ColumnName]) + " " +
                        MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.ZIPCodeColumn.ColumnName]) + "</span><br/><br/>" +
                        (MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) != "" ? "<span>Phone: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) + "</span>" : "") + "<br/>" +
                        (MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) != "" ? "<span>Fax: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) + "</span>" : "") + "</td><td width='32%' style='text-align:right;'>" +
                         "<b>Patient Name: </b> " + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.FullNameColumn.ColumnName]).Replace(",", "") + "</span><br/>" +
                         "<span><b>DOB: </b>" + (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]) == string.Empty ? "" : Convert.ToDateTime(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]).ToShortDateString()) + "</span><br/>" +
                         (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) != "" ? "<span><b>MRN: </b>" + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) + "</span>" : "") + " <br/><br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='parent.Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                         //Start 14-11-2016 Edit By Humaira Yousaf for EMR-1615
                         "<button class='btn btn-default btn-sm mr-xs' type='button' id='btnDownload' onclick='parent.Clinical_InfoButtonView.DownloadInfo();'><i class='fa fa-download'></i> Download</button>" +
                         "<button class='btn btn-default btn-sm' type='button' id='btnEmail' onclick='Clinical_InfoButtonView.SendEmail(" + patientId + ");'><i class='fa fa-envelope-o'></i> Email</button></td></tr></tbody></table>";
                        //End 14-11-2016 Edit By Humaira Yousaf for EMR-1615

                        //footerNode.InnerHtml = "Generated by: MDVISION PM EMR <span style='float:right;'>Page 1 of 1</span></div><div class='clearfix'></div>";
                        footerNode.InnerHtml = "Generated by: MDVISION PM EMR</div><div class='clearfix'></div>";
                    }
                }
                else
                {
                    HtmlNode tblBody = document.CreateElement("tbody");
                    HtmlNode tbltr = document.CreateElement("tr");
                    HtmlNode tbltd = document.CreateElement("td");
                    tbltd.SetAttributeValue("style", "width:70%;padding-left:25px;");
                    HtmlNode logoImg = document.CreateElement("img");
                    logoImg.SetAttributeValue("src", VirtualPathUtility.ToAbsolute(@"~\content\images\SHS-nav-logo.png"));
                    logoImg.SetAttributeValue("height", "65px");
                    tbltd.AppendChild(logoImg);

                    HtmlNode tbltdH = document.CreateElement("td");
                    //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    tbltdH.SetAttributeValue("style", "width:30%; text-align:right;");
                    //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    HtmlNode h3Heading = document.CreateElement("h4");
                    h3Heading.Attributes.Add("class", "text-bold");
                    h3Heading.InnerHtml = "Educational Material";
                    tbltdH.AppendChild(h3Heading);
                    tbltr.AppendChild(tbltd);
                    tbltr.AppendChild(tbltdH);
                    tblBody.AppendChild(tbltr);
                    tblHeading.AppendChild(tblBody);

                    PatientEducationHelper ptEdu = PatientEducationHelper.Instance();
                    DSPatient dspatient = ptEdu.loadDataforPdf(patientId);
                    DSProfile dsProfile = new DSProfile();

                    patientTable = "<table width='100%' style='padding-left:15px;'><tbody><tr><td width='68%' style='line-height:1.5;padding-left:30px;'><span>" +
                       MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.AddressColumn.ColumnName]) + "</span><br/>" +
                      "<span>" + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.CityColumn.ColumnName]) + ", " +
                      MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.StateColumn.ColumnName]) + " " +
                    MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.ZIPCodeColumn.ColumnName]) + "</span><br/><br/>" +
                    (MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) != "" ? "<span>Phone: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) + "</span>" : "") + "<br />" +
                    (MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) != "" ? "<span>Fax: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) + "</span>" : "") + "</td><td width='32%' style='text-align:right;'>" + // //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                     "<b>Patient Name: </b> " + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.FullNameColumn.ColumnName]).Replace(",", "") + "</span><br/>" +
                     "<span><b>DOB: </b>" + (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]) == string.Empty ? "" : Convert.ToDateTime(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]).ToShortDateString()) + "</span><br/>" +
                     //(MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) != "" ? "<span><b>MRN: </b>" + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) + "</span>" : "") + " <br /><br /><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter'><i class='fa fa-print'></i> print</button>" +
                     (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) != "" ? "<span><b>MRN: </b>" + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) + "</span>" : "") + " <br/><br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                     //Start 14-11-2016 Edit By Humaira Yousaf for EMR-1615
                     "<button class='btn btn-default btn-sm mr-xs' type='button' id='btnDownload' onclick='Clinical_InfoButtonView.DownloadInfo(this);'><i class='fa fa-download'></i> Download</button>" +
                     "<button class='btn btn-default btn-sm' type='button' id='btnEmail' onclick='Clinical_InfoButtonView.SendEmail(" + patientId + ");'><i class='fa fa-envelope-o'></i> Email</button></td></tr></tbody></table>";
                    //End 14-11-2016 Edit By Humaira Yousaf for EMR-1615

                    footerNode.InnerHtml = "Generated by: MDVISION PM EMR <span style='float:right;'></span></div><div class='clearfix'></div>";

                }

                var patientNode = HtmlNode.CreateNode(patientTable);

                if (bodyNode != null)
                {
                    headerDiv.AppendChild(contentDiv);
                    contentDiv.AppendChild(serviceDiv);
                    serviceDiv.AppendChild(resultsDiv);
                    resultsDiv.AppendChild(resultDiv);
                    resultDiv.AppendChild(tblHeading);
                    resultDiv.AppendChild(divStyle);
                    resultDiv.AppendChild(patientNode);
                    bodyNode.InsertBefore(headerDiv, contentNode);
                }

                //Remove Scripts from html File
                document.DocumentNode.Descendants()
                                .Where(n => n.Name == "script" || n.Name == "noscript" || n.Name == "meta")
                                .ToList()
                                .ForEach(n => n.Remove());

                //Remove Navigation tags
                document.DocumentNode.Descendants()
                                .Where(d => d.Attributes.Contains("class") && (d.Attributes["class"].Value.Contains("hide-offscreen") || d.Attributes["class"].Value.Contains("return-top") || d.Attributes["class"].Value.Contains("subhead") || d.Attributes["class"].Value.Contains("mplushead") || d.Attributes["class"].Value.Contains("notes")))
                                .ToList()
                                .ForEach(n => n.Remove());

                //Add target=_blank to <a> tags so that the links would open in New Tab
                var anchorNodes = document.DocumentNode.Descendants()
                    .Where(d => d.Name.Equals("a") && (d.Attributes["target"] == null)).ToList();
                foreach (var anode in anchorNodes)
                {
                    anode.Attributes.Add("target", "_blank");
                }

                //Add styles and images
                Dictionary<int, string> cssPath = new Dictionary<int, string>();
                //cssPath.Add(1, "https://apps.nlm.nih.gov/medlineplus/services/css/connect.css");
                //cssPath.Add(2, "~/Content/Default/bootstrap.css");
                //cssPath.Add(3, "~/Content/Blue/default.css");
                //cssPath.Add(4, "~/Content/Blue/theme.css");
                //cssPath.Add(5, "~/Content/Blue/theme-custom.css");

                HtmlNode head = document.DocumentNode.SelectSingleNode("/html/head");
                var newLineNode = HtmlNode.CreateNode("\r\n");
                head.AppendChild(newLineNode);
                foreach (var css in cssPath)
                {
                    HtmlNode link = document.CreateElement("link");
                    head.AppendChild(link);
                    link.SetAttributeValue("rel", "stylesheet");
                    link.SetAttributeValue("href", css.Key > 1 ? VirtualPathUtility.ToAbsolute(css.Value) : css.Value);
                    head.AppendChild(newLineNode);
                }

                var styleNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Name.Equals("link") && (d.Attributes["rel"].Value.Equals("stylesheet")));
                if (styleNode != null)
                {
                    styleNode.Remove();
                }

                var node = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("mplus-nav-info"));

                if (node != null)
                {
                    if (node.InnerText.Trim().Equals("0 results found."))
                        noinfoFound = "No Information Found";
                }

                document.DocumentNode.Descendants()
                           .Where(n => n.Name == "div" && n.Attributes.Contains("id") && (n.Attributes["id"].Value.Contains("mplus-orgs") || n.Attributes["id"].Value.Contains("mplus-logo") || n.Attributes["id"].Value.Contains("mplus-nav") || n.Attributes["id"].Value.Contains("mplus-nav") || n.Attributes["id"].Value.Contains("mplus-footer")))
                           .ToList()
                           .ForEach(n => n.Remove());

                if (parentCtrl.Equals("Clinical_ProblemLists"))
                    caller = "Illness: ";
                else if (parentCtrl.Equals("Clinical_Medications"))
                    caller = "Drug: ";
                else
                    caller = "Lab Test: ";

                HtmlNode resultNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("result-main"));

                //Check if Record found then get FileName else show No Information Found 
                var fileName = string.Empty;
                if (String.IsNullOrWhiteSpace(noinfoFound))
                {
                    //Get Name for Problem List
                    node = document.DocumentNode.Descendants().FirstOrDefault(n => n.Id.Equals("problems_topichead"));
                    if (node != null)
                    {
                        fileName = node.InnerText.Trim();

                        HtmlNode summaryNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("problems_summaryTopic"));

                        HtmlNode summaryImageNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("problems_imagetopic"));

                        HtmlNode summaryImage = summaryImageNode.Descendants().FirstOrDefault(d => d.Name.Equals("a"));

                        string contentTable = "<table width='100%' style='border:1px solid #d8eaf6;  margin-top:10px'><thead><tr style='background:#d8eaf6;'><th style='padding-left:15px; font-weight:normal;'>" +
                                                             "<b>" + caller + "</b>" + fileName + "</th></tr></thead><tbody><tr><td style='padding:15px;'><p>" + summaryNode.InnerHtml + "</p><p>" + summaryImage.InnerHtml + "</p></td></tr></tbody></table>";


                        HtmlNode withAlsoNode = resultNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("with-also"));
                        if (withAlsoNode != null)
                            withAlsoNode.Remove();

                        HtmlNode alsoCalledNode = resultNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("alsoCalled"));
                        if (alsoCalledNode != null)
                            alsoCalledNode.Remove();

                        HtmlNode image = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("result-side"));
                        if (image != null)
                            image.Remove();

                        summaryNode.Remove();
                        HtmlNode content = HtmlNode.CreateNode(contentTable);

                        resultNode.PrependChild(content);

                    }
                    else
                    {
                        //Get Name for Medication & Lab Results
                        node = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("a") && n.Attributes.Any(a => a.Value.Contains("druginfo") || a.Value.Contains("health") || a.Value.Contains("factsheets") || a.Value.Contains("ency")));

                        HtmlNode pageTitle = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("page-title"));
                        string tempFileName = "";
                        if (pageTitle != null)
                        {
                            var header = pageTitle.ChildNodes.FirstOrDefault(n => n.Name.Equals("h1"));
                            if (header != null)
                            {
                                tempFileName = header.InnerText.Trim();
                            }
                        }
                        fileName = node != null ? node.InnerText.Trim() : Guid.NewGuid().ToString();
                        if (node != null)
                        {
                            fileName = node.InnerText.Trim();
                            HtmlNode resultsNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("results"));
                            if (resultNode != null)
                            {
                                resultNode.SetAttributeValue("class", "noBorder p-none");
                            }

                            node.ParentNode.InnerHtml = "<table width='100%' style='border:1px solid #d8eaf6;'><thead><tr style='background:#d8eaf6;'><th style='padding-left:15px; font-weight:normal;'>" +
                            "<b>" + caller + "</b>" + fileName + "</th></tr></thead><tbody><tr><td style='padding:15px;'>" + node.ParentNode.InnerHtml + "</td></tr></tbody></table>";
                        }
                        if (!string.IsNullOrWhiteSpace(tempFileName))
                        {
                            fileName = tempFileName;
                        }
                    }
                }

                HtmlNode footerNodeBar = HtmlNode.CreateNode(footerDivBar);
                bodyNode.AppendChild(footerNodeBar);
                bodyNode.AppendChild(footerNode);

                foreach (HtmlNode item in document.DocumentNode.Descendants()
                             .Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("mplus-wrap")))
                {
                    item.SetAttributeValue("class", "noBoxshadow ");

                }

                foreach (HtmlNode item in document.DocumentNode.Descendants()
                             .Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("mplus-content")))
                {
                    item.SetAttributeValue("class", "noBorder p-none");

                }

                HtmlNode printer = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("button") && (n.Id.Equals("btnPrinter")));
                if (printer != null)
                {
                    printer.Attributes.Remove("background");
                }
                HtmlNode download = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("button") && (n.Id.Equals("btnDownload")));
                if (download != null)
                {
                    download.Attributes.Remove("background");
                }
                node = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("a") && (n.Id.Equals("mplus-lang-toggle") || n.Id.Equals("toggleMenu")));
                if (node != null)
                {
                    //Update Language to English
                    node.InnerHtml = "English";

                    var href = node.Attributes["href"].Value.Replace("sp", "en");
                    href = "https://apps.nlm.nih.gov" + href;
                    node.Attributes["href"].Value = href;
                }

                //Convert images to Base64 to be shown in pdf file
                foreach (var element in document.DocumentNode.SelectNodes("//img[@src]"))
                {
                    HtmlAttribute attr = element.Attributes["src"];
                    attr.Value = B64Encode(attr.Value);
                }

                document.OptionWriteEmptyNodes = true;
                htmlContent = document.DocumentNode.OuterHtml;

                byte[] array = Encoding.ASCII.GetBytes(htmlContent);

                return new BLObject<byte[]>(array, fileName != string.Empty ? fileName : noinfoFound);
            }
            catch (Exception ex)
            {
                return new BLObject<byte[]>(null, ex.Message);
            }
        }

        public static string RemoveComments(string input)
        {
            string tagPattern = @"<!--(.|[\r\n])*?-->";

            MatchCollection matches = Regex.Matches(input, tagPattern);
            foreach (Match match in matches)
            {
                input = input.Replace(match.Value, string.Empty);
            }

            return input;
        }

        [HttpPost]
        public string GetLinksResponse(JObject allData)
        {
            var linkResponse = string.Empty;
            var error = string.Empty;

            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                dynamic data = ser.Deserialize<dynamic>(MDVUtility.ToStr(allData["data"]));

                string linkURL = Convert.ToString(data["LinkURL"]);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(linkURL);
                webRequest.Method = "GET";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    if (responseStream != null)
                    {
                        StreamReader responseStreamReader = new StreamReader(responseStream);
                        linkResponse = responseStreamReader.ReadToEnd();
                    }
                    else
                    {
                        error = "No Information Found";
                    }
                }
                else
                {
                    error = "No Information Found";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            var response = new
            {
                data = linkResponse,
                Message = error
            };

            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public string SendEmail(JObject allData)
        {
            var linkResponse = string.Empty;
            var error = string.Empty;

            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                dynamic data = ser.Deserialize<dynamic>(MDVUtility.ToStr(allData["data"]));

                string patientId = Convert.ToString(data["PatientId"]);
                string body = Convert.ToString(data["Body"]);

                string mailAddress = Convert.ToString(data["EmailAddress"]); ;
                if (mailAddress != "")
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");

                    mail.From = new MailAddress("no_reply@sovms.com");
                    mail.To.Add(mailAddress); //dr.EmailAddress "  ;
                    mail.Subject = "Sovereign Health System Patient Education";
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new NetworkCredential("no_reply@sovms.com", "Naka5116");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                else
                {
                    error = "Email id is missing.";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            var response = new
            {
                error = error
            };

            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        /* PDF Generation
        private BLObject<byte[]> TransformPdf(long patientId, string connectPlusResponse)
        {
            try
            {

                byte[] newByteArr = null;
                var infotitle = string.Empty;
                using (MemoryStream stream = new MemoryStream())
                {

                    Document pdfDocument = new Document(PageSize.LETTER, 10f, 15f, 10f, 10f);

                    // Heading Font Style
                    Document pdfDocument = new Document(PageSize.LETTER, 15f, 15f, 140f, 10f);
                    Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    Font linkFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, new BaseColor(0, 0, 255));
                    Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    Font bodyHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    Font titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD, new BaseColor(102, 178, 255));

                    MDVision.Business.BCommon.Utility.PDFCreator pdf = new MDVision.Business.BCommon.Utility.PDFCreator(ref pdfDocument, stream, false, ClientConfiguration.DecryptFrom64(Patient.MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(Patient.MDVSession.Current.AppUserName));
                    pdf.Document.Open();

                    //Create Header & Footer
                    var _event = new TextEvents();
                    _event.Patientid = patientId;
                    _event.UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    pdf.Writer.PageEvent = _event;
                    pdf.Document.Open();

                    // Body Data
                    PdfPTable summaryTable = new PdfPTable(1);
                    summaryTable.TotalWidth = 575f;
                    summaryTable.LockedWidth = true;
                    summaryTable.SpacingBefore = summaryTable.SpacingAfter = 10f;
                    summaryTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    summaryTable.HorizontalAlignment = Element.ALIGN_LEFT;



                    Paragraph summayTitle = new Paragraph(infotitle, titleFont);
                    summaryTable.AddCell(summayTitle);


                    var helpLink = getInformationLink(connectPlusResponse);
                    Anchor helperanchor = new Anchor(helpLink, linkFont);
                    helperanchor.Reference = helpLink;
                    summaryTable.AddCell(helperanchor);

                    pdfDocument.Add(summaryTable);

                    var summaryDetails = getInformationDetails(connectPlusResponse);
                    List<Tuple<string, string, string>> linklist = GetLinksFromHtml(ref summaryDetails);
                    using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDocument))
                    {
                        using (var sr = new StringReader(summaryDetails))
                        {
                            htmlWorker.Parse(sr);
                        }
                    }

                    if (linklist.Count > 0)
                    {
                        PdfPTable linksTable = new PdfPTable(1);
                        linksTable.TotalWidth = 575f;
                        linksTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        linksTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        linksTable.SpacingAfter = linksTable.SpacingBefore = 7f;
                        linksTable.AddCell(new Phrase("Selected resources", bodyHeadingFont));

                        foreach (var lnk in linklist)
                        {
                            Anchor anchor = new Anchor(lnk.Item2 + " " + lnk.Item3, linkFont);
                            anchor.Reference = lnk.Item1;
                            linksTable.AddCell(anchor);
                        }

                        pdfDocument.Add(linksTable);
                    }

                    pdf.Document.Close();
                    pdf.Writer.Close();

                    newByteArr = stream.GetBuffer();
                }


                return new BLObject<byte[]>(newByteArr, infotitle);
            }
            catch (Exception ex)
            {
                return new BLObject<byte[]>(null, ex.Message);
            }
        }

        private string getInformationLink(string responseXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ab", "http://www.w3.org/2005/Atom");
                doc.LoadXml(responseXml);
                return doc.SelectSingleNode("/ab:feed/ab:entry/ab:link",nsmgr).Attributes["href"].Value;
            }
            catch (Exception ex) 
            {
                throw new Exception("No Information Found");
            }
        }

        private string getInformationTitle(string responseXml)
        {
             try
            {
                XmlDocument doc = new XmlDocument();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ab", "http://www.w3.org/2005/Atom");
                doc.LoadXml(responseXml);
                return doc.SelectSingleNode("/ab:feed/ab:entry/ab:title", nsmgr).InnerText;
            }
             catch (Exception ex)
             {
                 throw new Exception("No Information Found");
             }
        }

        private string getInformationDetails(string responseXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ab", "http://www.w3.org/2005/Atom");
                doc.LoadXml(responseXml);
                return doc.SelectSingleNode("/ab:feed/ab:entry/ab:summary", nsmgr).InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception("No Information Found");
            }
        }

        private List<Tuple<string, string, string>> GetLinksFromHtml(ref string content)
        {
            var links = new List<Tuple<string, string, string>>();
            try
            { 
                string regex = @"<li><a\shref=""(?<url>.*?)"">(?<text>.*?)</a>(?<additionaltext>.*?)</li>";
                var matches = Regex.Matches(content, regex, RegexOptions.IgnoreCase);
                
                foreach (Match m in matches)
                {
                    var replacestring = "<li><a href=\"" + m.Groups["url"].Value + "\">" + m.Groups["text"].Value + "</a>" + m.Groups["additionaltext"].Value + "</li>";
                    content = content.Replace(replacestring, string.Empty);
                    links.Add(new Tuple<string, string, string>(m.Groups["url"].Value, m.Groups["text"].Value, m.Groups["additionaltext"].Value));
                }
           }
            catch (Exception ex)
            {
                // ignored
            }
            return links;
        }
         */
    }

    /* PDF Generation Header & Footer
    public class TextEvents : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte _cb;

        // we will put the final number of pages in a template
        PdfTemplate _footerTemplate, _headerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;


        #region Fields
        private long _patientid;
        private string _userName;
        #endregion

        #region Properties
        public long Patientid
        {
            set { _patientid = value; }
        }

        public string UserName
        {
            set { _userName = value; }
        }
        #endregion

        /// Called when the document is opened.
        ///              @param writer the <CODE>PdfWriter</CODE>for this document
        ///              @param document the document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                _bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                _cb = writer.DirectContent;
                _headerTemplate = _cb.CreateTemplate(100, 100);
                _footerTemplate = _cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                //handle exception here
            }
            catch (System.IO.IOException ioe)
            {
                //handle exception here
            }
        }

        /// Called when a page is finished, just before being written to the document.
        ///              @param writer the <CODE>PdfWriter</CODE>for this document
        ///              @param document the document
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            String text = "Page " + writer.PageNumber + " of ";
            String userName = "Generated By: " + _userName;

            //Add paging to footer
            {
                _cb.BeginText();
                _cb.SetFontAndSize(_bf, 12);
                _cb.SetTextMatrix(document.PageSize.GetRight(105), document.PageSize.GetBottom(20));
                _cb.ShowText(text);
                _cb.EndText();
                float len = _bf.GetWidthPoint(text, 12);
                _cb.AddTemplate(_footerTemplate, document.PageSize.GetRight(105) + len, document.PageSize.GetBottom(20));
            }

            //Add UserName to footer
            {
                _cb.BeginText();
                _cb.SetFontAndSize(_bf, 12);
                _cb.SetTextMatrix(10, document.PageSize.GetBottom(20));
                _cb.ShowText(userName);
                _cb.EndText();
                float len = _bf.GetWidthPoint(userName, 12);
                _cb.AddTemplate(_footerTemplate, document.PageSize.GetRight(10) + len, document.PageSize.GetBottom(20));
            }

            Font headerFont = FontFactory.GetFont("Arial", 13, Font.BOLD, new BaseColor(23, 29, 33));
            Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            DataSet ds = new DataSet();
            ds.Merge(new DALPatient(Patient.SharedObj).FillPatient(_patientid, "", ""));

            DSPatient dsPatient = new DSPatient();
            DSProfile dsProfile = new DSProfile();

            // Start Append Patient's Data
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.TotalWidth = 575f;
            headerTable.LockedWidth = true;
            headerTable.SetWidths(new float[] { 450f, 125f });
            headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

            # region Heading

            
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(@"~\content\images\mdvision-logo.png"));
            logo.ScalePercent(59f);
            PdfPCell cell1 = new PdfPCell();
            cell1.AddElement(logo);
            cell1.Border = Rectangle.NO_BORDER;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            headerTable.AddCell(cell1);

            Paragraph title = new Paragraph("Patient Education", headerFont);
            headerTable.AddCell(title);
          
            # endregion Heading

            foreach (DSPatient.PatientsRow dr in ds.Tables[dsPatient.Patients.TableName].Rows)
            {
                # region PracticeInfo
                //Practice Data
                ds.Merge(new DALPractice(Patient.SharedObj).LoadPractice(Convert.ToInt64(dr[dsPatient.Patients.PracticeIdColumn.ColumnName]), "", "", "", "", "1"));
                var practiceName = string.Empty;
                var practiceAddress = string.Empty;
                var practicePhone = string.Empty;
                var practiceFax = string.Empty;
                foreach (DSProfile.PracticeRow drpractice in ds.Tables[dsProfile.Practice.TableName].Rows)
                {
                    practiceName = Convert.ToString(drpractice[dsProfile.Practice.DescriptionColumn.ColumnName]);
                    practiceAddress = Convert.ToString(drpractice[dsProfile.Practice.Address1Column.ColumnName]);
                    practicePhone = String.Format("{0:(###) ###-####}", Convert.ToString(drpractice[dsProfile.Practice.PhoneNoColumn.ColumnName]));
                    if (!String.IsNullOrEmpty(Convert.ToString(drpractice[dsProfile.Practice.PhoneExtColumn.ColumnName])))
                    {
                        practicePhone = practicePhone + " Ext " + Convert.ToString(drpractice[dsProfile.Practice.PhoneExtColumn.ColumnName]);
                    }
                    practiceFax = String.Format("{0:(###) ###-####}", Convert.ToString(drpractice[dsProfile.Practice.FaxColumn.ColumnName]));
                }
                # endregion PracticeInfo

                # region Header Rows

                //Load Practice Data
                PdfPTable practiceTable = new PdfPTable(1);
                practiceTable.DefaultCell.Border = Rectangle.NO_BORDER;
                practiceTable.HorizontalAlignment = Element.ALIGN_LEFT;
                practiceTable.AddCell(new Phrase(practiceName, bodyFont));
                practiceTable.AddCell(new Phrase(practiceAddress, bodyFont));
                practiceTable.AddCell(new Phrase(string.Format("Practice Phone: {0}", practicePhone), bodyFont));
                practiceTable.AddCell(new Phrase(string.Format("Fax: {0}", practiceFax), bodyFont));
                headerTable.AddCell(practiceTable);

                //Load Patient Data
                PdfPTable patientTable = new PdfPTable(1);
                patientTable.DefaultCell.Border = Rectangle.NO_BORDER;
                patientTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                patientTable.AddCell(new Phrase(string.Format("Patient: {0}", Convert.ToString(dr[dsPatient.Patients.FullNameColumn.ColumnName])), bodyFont));
                patientTable.AddCell(new Phrase(string.Format("DOB: {0}", String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]))), bodyFont));
                patientTable.AddCell(new Phrase(string.Format("MRN: {0}", Convert.ToString(dr[dsPatient.Patients.MRNumberColumn.ColumnName])), bodyFont));
                patientTable.AddCell(new Phrase(string.Format("Phone: {0}", String.Format("{0:(###) ###-####}", Convert.ToString(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName]))), bodyFont));
                headerTable.AddCell(patientTable);
                # endregion
            }

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            headerTable.WriteSelectedRows(0, -1, 8, document.PageSize.Height - 15, writer.DirectContent);
            //UserName.WriteSelectedRows(0, -1, document.PageSize.Width - 8, document.PageSize.GetBottom(50), writer.DirectContent);

            _cb.SetLineWidth(3.0f);
            _cb.SetColorStroke(new BaseColor(88, 143, 212));
            //Move the pointer and draw line to separate header section from rest of page
            _cb.MoveTo(8 , document.PageSize.Height - 135);
            _cb.LineTo(document.PageSize.Width - 8 , document.PageSize.Height - 135);
            _cb.ClosePathStroke();

            //Move the pointer and draw line to separate footer section from rest of page
            _cb.MoveTo(8, document.PageSize.GetBottom(50));
            _cb.LineTo(document.PageSize.Width - 8, document.PageSize.GetBottom(50));
            _cb.ClosePathStroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            _footerTemplate.BeginText();
            _footerTemplate.SetFontAndSize(_bf, 12);
            _footerTemplate.SetTextMatrix(0, 0);
            _footerTemplate.ShowText((writer.PageNumber).ToString());
            _footerTemplate.EndText();


        }
    }
     */
}
