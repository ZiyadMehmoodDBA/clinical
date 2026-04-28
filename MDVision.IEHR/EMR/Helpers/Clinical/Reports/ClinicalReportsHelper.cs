using HtmlAgilityPack;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.Reports;
using MDVision.Model.Clinical.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Reports
{
    public class ClinicalReportsHelper
    {
        private BLLReports BLLReportObj = null;
        private BLLAdminClinical BLLAdminClinicalObj = null;
        public ClinicalReportsHelper()
        {
            BLLReportObj = new BLLReports();
            BLLAdminClinicalObj = new BLLAdminClinical();
        }
        private static ClinicalReportsHelper _instance = null;
        public static ClinicalReportsHelper Instance()
        {
            if (_instance == null)
                _instance = new ClinicalReportsHelper();
            return _instance;
        }

        public string LoadPhoneEncounterReport(PhoneEncounterSearchModel model)
        {
            try
            {
                BLObject<List<CPhoneEncounterModel>> obj = BLLReportObj.LoadPhoneEncounterReport(model.CreateDateFrom, model.CreateDateTo, model.NoteStatus, model.DurationFrom,model.DurationTo, model.ProviderIds, model.RefProviderIds, model.PracticeIds);
                List<CPhoneEncounterModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        phoneEncounterCount = modelList.Count,
                        phoneEncounterList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        phoneEncounterCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadProgressNoteReport(PhoneEncounterSearchModel model)
        {
            try
            {
                BLObject<List<CProgressNoteReportModel>> obj = BLLReportObj.LoadProgressNotesReport(model.CreateDateFrom, model.CreateDateTo, model.NoteStatus, model.NoteType, model.ProviderIds, model.FacilityIds, model.RefProviderIds, model.PracticeIds,model.IsAmendedNote);
                List<CProgressNoteReportModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        progressNoteCount = modelList.Count,
                        progressNoteList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        progressNoteCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadAllergiesReport(CAllergiesModel model)
        {
            try
            {
                BLObject<List<CAllergiesFillModel>> obj = BLLReportObj.LoadAllergiesReport(model);
                List<CAllergiesFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        allergiesCount = modelList.Count,
                        allergiesList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        allergiesCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadVitalsReport(CVitalsModel model)
        {
            try
            {
                BLObject<List<CVitalsFillModel>> obj = BLLReportObj.LoadVitalsReport(model);
                List<CVitalsFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        vitalsCount = modelList.Count,
                        vitalsList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        vitalsCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadOrdersReport(COrdersModel model)
        {
            try
            {
                BLObject<List<COrdersFillModel>> obj = BLLReportObj.LoadOrdersReport(model);
                List<COrdersFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = 2147483647;
                    var response = new
                    {
                        status = true,
                        ordersCount = modelList.Count,
                        ordersList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ordersCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadProblemsReport(CProblemsModel model)
        {
            try
            {
                BLObject<List<CProblemsFillModel>> obj = BLLReportObj.LoadProblemsReport(model);
                List<CProblemsFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = Int32.MaxValue;
                    var response = new
                    {
                        status = true,
                        ProblemsCount = modelList.Count,
                        ProblemsList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProblemsCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }



        internal string LoadProceduresReport(CProceduresModelcs model)
        {
            try
            {
                BLObject<List<CProceduresFillModelcs>> obj = BLLReportObj.LoadProceduresReport(model);
                List<CProceduresFillModelcs> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        proceduresCount = modelList.Count,
                        proceduresList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        proceduresCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadImmunizationReport(CImmunizationModel model)
        {
            try
            {
                BLObject<List<CImmunizationFillModel>> obj = BLLReportObj.LoadImmunizationReport(model);
                List<CImmunizationFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = 1000000000;
                    var response = new
                    {
                        status = true,
                        immunizationCount = modelList.Count,
                        immunizationList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        immunizationCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadResultsReport(CResultsModel model)
        {
            try
            {
                BLObject<List<CResultsFillModel>> obj = BLLReportObj.LoadResultsReport(model);
                List<CResultsFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        resultsCount = modelList.Count,
                        resultsList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        resultsCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        internal string LoadMedicationReport(CMedicationModel model)
        {
            try
            {
                BLObject<List<CMedicationFillModel>> obj = BLLReportObj.LoadMedicationReport(model);
                List<CMedicationFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        medicationsCount = modelList.Count,
                        medicationsList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        medicationsCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        // Start || 1 September, 2016 || Talha Tanweer

        internal string LoadConsultationOrdersReport(COrdersModel model)
        {
            try
            {
                BLObject<List<COrdersFillModel>> obj = BLLReportObj.LoadConsultationOrdersReport(model);
                List<COrdersFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ordersCount = modelList.Count,
                        ordersList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ordersCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        // End   || 1 September, 2016 || Talha Tanweer


        // Start || 6 September, 2016 || Talha Tanweer

        internal string GetHeaderFooterForClinicalReports(string DefaultProviderId)
        {

            Int64 providerId;
            bool IsProviderExist = Int64.TryParse(DefaultProviderId, out providerId);
            if (IsProviderExist)
            {

                try
                {
                    //HtmlNode tblHeading = document.CreateElement("table");
                    //tblHeading.Attributes.Add("style", "width:100%");

                    //HtmlNode footerNode = document.CreateElement("div");
                    //footerNode.Attributes.Add("style", "float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:5px 25px");


                    DSReportHeader dsreportHeader = null;
                    BLObject<DSReportHeader> obj = BLLAdminClinicalObj.getReportHeaderTagsValue(-1, providerId, -1, "Clinical Reports");
                    dsreportHeader = obj.Data;
                    if (dsreportHeader.ReportHeaderTags.Rows.Count > 0)
                    {
                        DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];



                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            HeaderLogo = dr["HeaderLogo"],
                            FooterText = dr["FooterText"]

                            // reportHeaderCount = DSReportHeader.Tables[DSReportHeader.ReportHeaderList.TableName].Rows.Count,
                            // iTotalDisplayRecords = DSReportHeader.ReportHeaderList.Rows[0][DSReportHeader.ReportHeaderList.RecordCountColumn.ColumnName],
                            // reportHeaderList_JSON = js.Serialize(modelList),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        //if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.HeaderLogoColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]))
                        //    && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName])))
                        //{
                        //    HtmlNode tblBody = document.CreateElement("tbody");
                        //    HtmlNode tbltr = document.CreateElement("tr");
                        //    HtmlNode tbltd = document.CreateElement("td");
                        //    tbltd.SetAttributeValue("style", "width:70%;padding-left:0px;");
                        //    HtmlNode logoImg = document.CreateElement("img");
                        //    logoImg.SetAttributeValue("src", MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.HeaderLogoColumn.ColumnName]));
                        //    logoImg.SetAttributeValue("style", "max-width: 350px;max-height:140px;");
                        //    tbltd.AppendChild(logoImg);

                        //    HtmlNode tbltdH = document.CreateElement("td");
                        //    tbltdH.SetAttributeValue("style", "width:30%;vertical-align:top;");
                        //    tbltdH.InnerHtml = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName]);
                        //    tbltr.AppendChild(tbltd);
                        //    tbltr.AppendChild(tbltdH);
                        //    tblBody.AppendChild(tbltr);
                        //    tblHeading.AppendChild(tblBody);

                        //    patientTable = "<table width='100%' style='padding-left:0px;'>" +
                        //                   "<tbody>" +
                        //                   " <tr>" +
                        //                "<td width='70%' style='line-height:1.5;padding-left:0px;'>" + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]) +
                        //      "</td><td width='30%'>" + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName]) +
                        //        "<br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                        //     "<button class='btn btn-default btn-sm' type='button' id='btnDownload' onclick='Clinical_InfoButtonView.DownloadInfo();'><i class='fa fa-download'></i> Download</button></td></tr></tbody></table>";

                        //    //footerNode.InnerHtml = "Generated by: " + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName]) + "<span style='float:right;'>Page 1 of 1</span></div><div class='clearfix'></div>";
                        //    footerNode.InnerHtml = "Generated by: " + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName]) + "</div><div class='clearfix'></div>";
                        //}
                        //else
                        //{
                        //    HtmlNode tblBody = document.CreateElement("tbody");
                        //    HtmlNode tbltr = document.CreateElement("tr");
                        //    HtmlNode tbltd = document.CreateElement("td");
                        //    tbltd.SetAttributeValue("style", "width:70%;padding-left:25px;");
                        //    HtmlNode logoImg = document.CreateElement("img");
                        //    logoImg.SetAttributeValue("src", VirtualPathUtility.ToAbsolute(@"~\content\images\SHS-nav-logo.png"));
                        //    logoImg.SetAttributeValue("height", "65px");
                        //    tbltd.AppendChild(logoImg);


                        //    tbltr.AppendChild(tbltd);
                        //    tbltr.AppendChild(tbltdH);
                        //    tblBody.AppendChild(tbltr);
                        //    tblHeading.AppendChild(tblBody);

                        //    PatientEducationHelper ptEdu = PatientEducationHelper.Instance();
                        //    DSPatient dspatient = ptEdu.loadDataforPdf(patientId);
                        //    DSProfile dsProfile = new DSProfile();

                        //    patientTable = "<table width='100%' style='padding-left:15px;'><tbody><tr><td width='70%' style='line-height:1.5;padding-left:30px;'><span>" +
                        //       MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.AddressColumn.ColumnName]) + "</span><br/>" +
                        //      "<span>" + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.CityColumn.ColumnName]) + ", " +
                        //      MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.StateColumn.ColumnName]) + " " +
                        //    MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.ZIPCodeColumn.ColumnName]) + "</span><br/><br/>" +
                        //    (MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) != "" ? "<span>Phone: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) + "</span>" : "") + "<br/>" +
                        //    (MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) != "" ? "<span>Fax: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) + "</span>" : "") + "</td><td width='30%'>" +
                        //     "<b>Patient Name: </b> " + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.FullNameColumn.ColumnName]).Replace(",", "") + "</span><br/>" +
                        //     "<span><b>DOB: </b>" + (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]) == string.Empty ? "" : Convert.ToDateTime(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]).ToShortDateString()) + "</span><br/>" +
                        //     (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) != "" ? "<span><b>MRN: </b>" + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) + "</span>" : "") + " <br/><br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                        //     "<button class='btn btn-default btn-sm' type='button' id='btnDownload' onclick='Clinical_InfoButtonView.DownloadInfo();'><i class='fa fa-download'></i> Download</button></td></tr></tbody></table>";

                        //    //footerNode.InnerHtml = "Generated by: MDVISION PM EMR <span style='float:right;'>Page 1 of 1</span></div><div class='clearfix'></div>";
                        //    footerNode.InnerHtml = "Generated by: MDVISION PM EMR</div><div class='clearfix'></div>";
                        //}
                    }
                    else
                    {
                        //////HtmlNode tblBody = document.CreateElement("tbody");
                        //////HtmlNode tbltr = document.CreateElement("tr");
                        //////HtmlNode tbltd = document.CreateElement("td");
                        //////tbltd.SetAttributeValue("style", "width:70%;padding-left:25px;");
                        //////HtmlNode logoImg = document.CreateElement("img");
                        //////logoImg.SetAttributeValue("src", VirtualPathUtility.ToAbsolute(@"~\content\images\SHS-nav-logo.png"));
                        //////logoImg.SetAttributeValue("height", "65px");
                        //////tbltd.AppendChild(logoImg);

                        //////HtmlNode tbltdH = document.CreateElement("td");
                        //////tbltdH.SetAttributeValue("style", "width:30%;");

                        //////HtmlNode h3Heading = document.CreateElement("h4");
                        //////h3Heading.Attributes.Add("class", "text-bold");
                        //////h3Heading.InnerHtml = "Educational Material";
                        //////tbltdH.AppendChild(h3Heading);
                        //////tbltr.AppendChild(tbltd);
                        //////tbltr.AppendChild(tbltdH);
                        //////tblBody.AppendChild(tbltr);
                        //////tblHeading.AppendChild(tblBody);

                        //////PatientEducationHelper ptEdu = PatientEducationHelper.Instance();
                        //////DSPatient dspatient = ptEdu.loadDataforPdf(patientId);
                        //////DSProfile dsProfile = new DSProfile();

                        //////patientTable = "<table width='100%' style='padding-left:15px;'><tbody><tr><td width='70%' style='line-height:1.5;padding-left:30px;'><span>" +
                        //////   MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.AddressColumn.ColumnName]) + "</span><br/>" +
                        //////  "<span>" + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.CityColumn.ColumnName]) + ", " +
                        //////  MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.StateColumn.ColumnName]) + " " +
                        //////Utility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.ZIPCodeColumn.ColumnName]) + "</span><br/><br/>" +
                        //////(MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) != "" ? "<span>Phone: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName]) + "</span>" : "") + "<br />" +
                        //////(MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) != "" ? "<span>Fax: " + MDVUtility.ToStr(dspatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName]) + "</span>" : "") + "</td><td width='30%'>" +
                        ////// "<b>Patient Name: </b> " + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.FullNameColumn.ColumnName]).Replace(",", "") + "</span><br/>" +
                        ////// "<span><b>DOB: </b>" + (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]) == string.Empty ? "" : Convert.ToDateTime(dspatient.Patients.Rows[0][dspatient.Patients.DOBColumn.ColumnName]).ToShortDateString()) + "</span><br/>" +
                        //////    //(MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) != "" ? "<span><b>MRN: </b>" + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) + "</span>" : "") + " <br /><br /><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter'><i class='fa fa-print'></i> print</button>" +
                        ////// (MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) != "" ? "<span><b>MRN: </b>" + MDVUtility.ToStr(dspatient.Patients.Rows[0][dspatient.Patients.MRNumberColumn.ColumnName]) + "</span>" : "") + " <br/><br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                        ////// "<button class='btn btn-default btn-sm' type='button' id='btnDownload' onclick='Clinical_InfoButtonView.downloadInfo(this);'><i class='fa fa-download'></i> Download</button></td></tr></tbody></table>";

                        //////footerNode.InnerHtml = "Generated by: MDVISION PM EMR <span style='float:right;'></span></div><div class='clearfix'></div>";

                    }

                }
                catch (Exception ex)
                {
                    var response = new
                    {
                        status = false,
                        Message =MDVCustomException.HumanReadableMessage(ex.Message),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = true,
                    HeaderLogo = "",
                    FooterText = ""

                    // reportHeaderCount = DSReportHeader.Tables[DSReportHeader.ReportHeaderList.TableName].Rows.Count,
                    // iTotalDisplayRecords = DSReportHeader.ReportHeaderList.Rows[0][DSReportHeader.ReportHeaderList.RecordCountColumn.ColumnName],
                    // reportHeaderList_JSON = js.Serialize(modelList),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

            try
            {
                COrdersModel model = new COrdersModel();
                BLObject<List<COrdersFillModel>> obj = BLLReportObj.LoadConsultationOrdersReport(model);
                List<COrdersFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ordersCount = modelList.Count,
                        ordersList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ordersCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        // End   || 6 September, 2016 || Talha Tanweer


        internal string LoadProcedureOrdersReport(COrdersModel model)
        {
            try
            {
                BLObject<List<COrdersFillModel>> obj = BLLReportObj.LoadProcedureOrdersReport(model);
                List<COrdersFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ordersCount = modelList.Count,
                        ordersList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ordersCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string LoadPrescriptionOrdersReport(CPrescriptionOrderModel model)
        {
            try
            {
                BLObject<List<CPrescriptionOrderFillModel>> obj = BLLReportObj.LoadPrescriptionOrdersReport(model);
                List<CPrescriptionOrderFillModel> modelList = obj.Data;

                if (modelList != null && modelList.Count > 0)
                {

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ordersCount = modelList.Count,
                        ordersList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ordersCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
    }
}