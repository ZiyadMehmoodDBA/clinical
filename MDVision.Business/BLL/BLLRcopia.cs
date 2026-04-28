using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DAL.Rcopia;
using MDVision.Datasets;
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using iTextSharp.text.pdf.draw;
using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using MDVision.Common.Utilities;
using System.Net;
using MDVision.Model.Clinical.Medical;
using System.Data;
using System.Web;
using System.Threading;
using Newtonsoft.Json.Linq;
using MDVision.Model.Clinical.Orderset;
using MDVision.Model.Clinical.Medical.ProblemLists;

namespace MDVision.Business.BLL
{
    public class BLLRcopia
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLPatient"/> class.
        /// </summary>
        public BLLRcopia()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }

        public BLLRcopia(SharedVariable SharedVariable)
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Variable

        #endregion

        #region Common Function

        public BLObject<DSRcopia> UpdatePtientLastUpdateDate(DSRcopia ds, SharedVariable sharedVariable = null)
        {
            try
            {
                if (sharedVariable == null)
                {
                    ds = new DLLRcopia().UpdatePtientLastUpdateDate(ds);
                }
                else
                {
                    ds = new DLLRcopia(sharedVariable).UpdatePtientLastUpdateDate(ds);
                }
                
                return new BLObject<DSRcopia>(ds);

            }
            catch (Exception ex)
            {
                //MDVLogger.BLLErrorLog("BLLRcopia::UpdatePtientLastUpdateDate", ex);
                MDVLogger.SendExcepToDB(ex, "BLLRcopia::UpdatePtientLastUpdateDate", null);
                return new BLObject<DSRcopia>(null, ex.Message);
            }

        }

        public BLObject<DSRcopia> UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(DSRcopia ds)
        {
            try
            {
                ds = new DLLRcopia().UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(ds);
                return new BLObject<DSRcopia>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLRcopia::UpdateMedicationAndPrescriptionLastUpdateDateForLIMP", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }

        }

        public BLObject<DSRcopia> SelectPatientLastUpdateInfo(DSRcopia ds)
        {
            try
            {
                DSRcopia PatientLastUpdateInfoData = new DSRcopia();
                PatientLastUpdateInfoData = new DLLRcopia().SelectPatientLastUpdateInfo(ds);
                return new BLObject<DSRcopia>(PatientLastUpdateInfoData);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLRcopia::UpdateLastAllergyUpdateDate", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }

        }

        public BLObject<DSRcopia> SelectPatientLastUpdateInfoOp(long PatientID)
        {
            try
            {
                DSRcopia PatientLastUpdateInfoData = new DSRcopia();
                PatientLastUpdateInfoData = new DLLRcopia().SelectPatientLastUpdateInfoOp(PatientID);
                return new BLObject<DSRcopia>(PatientLastUpdateInfoData);

            }
            catch (Exception ex)
            {
                //MDVLogger.BLLErrorLog("BLLRcopia::UpdateLastAllergyUpdateDate", ex);
                MDVLogger.SendExcepToDB(ex, "BLLRcopia::UpdateLastAllergyUpdateDate", null);
                return new BLObject<DSRcopia>(null, ex.Message);
            }

        }

        public BLObject<DSRcopia> SelectGetUrls()
        {
            try
            {
                DSRcopia GetUrlData = new DSRcopia();
                GetUrlData = new DLLRcopia().SelectGetUrls();
                return new BLObject<DSRcopia>(GetUrlData);

            }
            catch (Exception ex)
            {
                //MDVLogger.BLLErrorLog("BLLRcopia::UpdateLastAllergyUpdateDate", ex);
                MDVLogger.SendExcepToDB(ex, "BLLRcopia::UpdateLastAllergyUpdateDate", null);
                return new BLObject<DSRcopia>(null, ex.Message);
            }

        }


        public BLObject<DSRcopia> SelectGetUrls(SharedVariable SharedVariable)
        {
            try
            {
                DSRcopia GetUrlData = new DSRcopia();
                if (SharedVariable != null)
                {
                    GetUrlData = new DLLRcopia(SharedVariable).SelectGetUrls(SharedVariable);
                }
                else
                {
                    GetUrlData = new DLLRcopia().SelectGetUrls(SharedVariable);
                }
                
                return new BLObject<DSRcopia>(GetUrlData);

            }
            catch (Exception ex)
            {
                //MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::UpdateLastAllergyUpdateDate", ex);
                MDVLogger.SendExcepToDB(ex, "BLLRcopia::UpdateLastAllergyUpdateDate", null);
                return new BLObject<DSRcopia>(null, ex.Message);
            }

        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to Select Software Customer Info.
        /// Date : 20 january 2016
        /// </summary>
        /// <param name="customerRegCode"></param>
        /// <returns></returns>
        public BLObject<DSRcopia> SelectSoftwareCustomerInfo(string customerRegCode,SharedVariable sharedVariable=null)
        {
            try
            {
                DSRcopia ds = new DSRcopia();
                if (sharedVariable != null)
                {
                    ds = new DLLRcopia(sharedVariable).SelectSoftwareCustomerInfo(customerRegCode);
                }
                else
                {
                    ds = new DLLRcopia().SelectSoftwareCustomerInfo(customerRegCode);
                }
                return new BLObject<DSRcopia>(ds);
            }
            catch (Exception ex)
            {
                //MDVLogger.BLLErrorLog("BLLRcopia::SelectSoftwareCustomerInfo", ex);
                MDVLogger.SendExcepToDB(ex, "BLLRcopia::SelectSoftwareCustomerInfo", null);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }

        public BLObject<DSRcopia> SelectSoftwareCustomerInfo(SharedVariable SharedVariable, string customerRegCode)
        {
            try
            {
                DSRcopia ds = new DSRcopia();
                ds = new DLLRcopia(SharedVariable).SelectSoftwareCustomerInfo(SharedVariable, customerRegCode);
                return new BLObject<DSRcopia>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::SelectSoftwareCustomerInfo", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }
        public BLObject<DSRcopia> UpDateGetUrl(DSRcopia ds,SharedVariable sharedVariable=null)
        {
            try
            {
                DSRcopia UpdateGetUrl = new DSRcopia();
                if (sharedVariable != null)
                {
                    UpdateGetUrl = new DLLRcopia(sharedVariable).UpDateGetUrl(ds);
                }
                else
                {
                    UpdateGetUrl = new DLLRcopia().UpDateGetUrl(ds);
                }
               
                return new BLObject<DSRcopia>(UpdateGetUrl);

            }
            catch (Exception ex)
            {
                //MDVLogger.BLLErrorLog("BLLRcopia::UpdateGetUrl", ex);
                MDVLogger.SendExcepToDB(ex, "BLLRcopia::UpdateGetUrl", null);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }

        //Start//13/7/2016//M Ahmad Ahmad//Delete existing Allergies

        public BLObject<string> IsPatientRegisteredOnDrFirs(Int64 PatientId)
        {
            try
            {
                string IsPatientRegisteredOnDrFirs;
                IsPatientRegisteredOnDrFirs = new DLLRcopia().IsPatientRegisteredOnDrFirs(PatientId);
                return new BLObject<string>(IsPatientRegisteredOnDrFirs);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteAllergies", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public BLObject<string> GetRcopiaUserName()
        {
            try
            {
                string UserName;
                UserName = new DLLRcopia().GetRcopiaUserName();
                return new BLObject<string>(UserName);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::GetRcopiaUserName", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> GetProviderRcopiaUserName(long NotesId)
        {
            try
            {
                string UserName;
                UserName = new DLLRcopia().GetProviderRcopiaUserName(NotesId);
                return new BLObject<string>(UserName);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::GetProviderRcopiaUserName", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        
        //End//13/7/2016//M Ahmad Ahmad//Delete existing Allergies


        //Start//13/7/2016//M Ahmad Ahmad//Delete existing Allergies

        public BLObject<string> CheckUserHaveRcopiaRights(Int64 UserId)
        {
            try
            {
                string IsUserHaveRcopiaRights;
                IsUserHaveRcopiaRights = new DLLRcopia().CheckUserHaveRcopiaRights(UserId);
                return new BLObject<string>(IsUserHaveRcopiaRights);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteAllergies", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSRcopia> GetPatientsForDrfirstRegisteration(SharedVariable SharedVariable)
        {
            try
            {
                DSRcopia PatientList = new DSRcopia();
                PatientList = new DLLRcopia(SharedVariable).GetPatientsForDrfirstRegisteration(SharedVariable);
                return new BLObject<DSRcopia>(PatientList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::GetPatientsForDrfirstRegisteration", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }
        public BLObject<DSRcopia> SaveResponse(SharedVariable SharedVariable, string RequestType, string Response, string status)
        {
            try
            {
                DSRcopia ds = new DSRcopia();
                ds = new DLLRcopia(SharedVariable).SaveResponse(SharedVariable, RequestType, Response, status);
                return new BLObject<DSRcopia>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::SaveResponse", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }


        public BLObject<DSRcopia> UpdatePatientRcopia(SharedVariable SharedVariable, DSRcopia ds)
        {
            try
            {
                DSRcopia PatientList = new DSRcopia();
                PatientList = new DLLRcopia(SharedVariable).UpdatePatientRcopia(SharedVariable, ds);
                return new BLObject<DSRcopia>(PatientList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::UpdatePatientRcopia", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }
        public BLObject<DSRcopia> UpdateRcopiaGetUrl(SharedVariable SharedVariable, DSRcopia ds)
        {
            try
            {
                DSRcopia RcopiaGetUrlList = new DSRcopia();
                RcopiaGetUrlList = new DLLRcopia(SharedVariable).UpdateRcopiaGetUrl(SharedVariable, ds);
                return new BLObject<DSRcopia>(RcopiaGetUrlList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::UpdateRcopiaGetUrl", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }

        public BLObject<DSRcopia> GetProblemsForAddOnDrfirst(SharedVariable SharedVariable, string PatientIds)
        {
            try
            {
                DSRcopia ProblemList = new DSRcopia();
                ProblemList = new DLLRcopia(SharedVariable).GetProblemsForAddOnDrfirst(SharedVariable, PatientIds);
                return new BLObject<DSRcopia>(ProblemList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::GetProblemsForAddOnDrfirst", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }

        public BLObject<DSRcopia> UpdateProblemRcopia(SharedVariable SharedVariable, DSRcopia ds)
        {
            try
            {
                DSRcopia ProblemList = new DSRcopia();
                ProblemList = new DLLRcopia(SharedVariable).UpdateProblemRcopia(SharedVariable, ds);
                return new BLObject<DSRcopia>(ProblemList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLRcopia::UpdateProblemRcopia", ex);
                return new BLObject<DSRcopia>(null, ex.Message);
            }
        }
        //End//13/7/2016//M Ahmad Ahmad//Delete existing Allergies
        public string setUploadUrl(SharedVariable SharedVariable, DateTime Modified, string RcopiaANS, string RcopiaANSbackup, string RcopiaVendorUsername, string RcopiaVendorPassword, string RcopiaPracticeUserName)
        {
            string exception = "DrFirst ANS cannot be called within 10 minutes,Please retry later";
            try
            {
                int minutes = Convert.ToInt32(DateTime.Now.Subtract(Modified).TotalMinutes);
                if (minutes > 10 || RcopiaANSbackup != "")
                {
                    HttpClient client = new HttpClient();
                    string error = string.Empty;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";


                    var ANS1url = RcopiaANS + "?xml=" + inputdata;
                    if (RcopiaANS == "" && RcopiaANSbackup != "")
                    {
                        ANS1url = RcopiaANSbackup + "?xml=" + inputdata;
                    }
                    HttpResponseMessage ResponseUploadPatient = client.GetAsync(ANS1url).Result;

                    if (ResponseUploadPatient != null)
                    {
                        var GetdataANS1 = ResponseUploadPatient.Content.ReadAsStringAsync().Result;
                        XmlDocument DocANS1 = new XmlDocument();
                        DocANS1.LoadXml(GetdataANS1);
                        XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                        XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                        XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                        string UploadUrlANS1 = string.Empty;
                        string downloadUrlANS1 = string.Empty;
                        string WebBrowserURLANS1 = string.Empty;
                        foreach (XmlNode node in nodelistWebBrowserURLANS1)
                        {
                            WebBrowserURLANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodeListuploadurlANS1)
                        {
                            UploadUrlANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodelistDownloadurlANS1)
                        {
                            downloadUrlANS1 = node.InnerText;
                        }
                        DSRcopia dsRc = new DSRcopia();
                        DSRcopia.Rcopia_GetUrlRow dr = dsRc.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                        dr.EngineUploadURL = UploadUrlANS1;
                        dr.EngineDownloadURL = downloadUrlANS1;
                        dr.WebBrowserURL = WebBrowserURLANS1;
                        dsRc.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);
                        BLObject<DSRcopia> UpdateRcopiaGetUrlObj = new BLLRcopia(SharedVariable).UpdateRcopiaGetUrl(SharedVariable, dsRc);
                        dsRc = UpdateRcopiaGetUrlObj.Data;
                        if (UpdateRcopiaGetUrlObj.Data != null)
                        {
                            exception = string.Empty;
                        }
                        else
                        {
                            throw new Exception("problistHelper:setUploadUrl", null);
                        }
                    }
                }
                else
                {
                    throw new Exception("problistHelper:setUploadUrl", null);
                }
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                throw new Exception(exception,ex);
            }

            return exception;
        }

        #endregion

        #region " Dr First Rcopia "

        public string getRcopiaResponseUrl(string ModuleName, DataRow RowtData, string ID, SharedVariable sharedVariable = null, HttpContext hCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        {
            // DSPatient.PatientsRow PatientData
            int count = 0;
            int ANS1count = 0;
            int ANS2count = 0;

            dynamic Errorresponse = "";

            for (int i = 0; i < 3; i++)
            {

                count++;
                Errorresponse = JObject.Parse(UploadURLS(ModuleName, RowtData, ID, count, sharedVariable, hCtrl, MedicationModel, ProviderUsername, PatientId));
                if (Errorresponse.Rcopia != "")
                {

                    break;
                }
                else if (Errorresponse.Error == "error")
                {
                    MDVLogger.RcopiaLogMessage("Error: Send_Patient", "", "", "", MDVUtility.ToStr(Errorresponse.Error), count);
                    break;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Error: Send_Patient", "", "", "", MDVUtility.ToStr(Errorresponse.exception), count);
                }
                int milliseconds = 30000;
                Thread.Sleep(milliseconds);
            }
            if (count > 2)
            {
                for (int j = 0; j < 3; j++)
                {
                    ANS1count++;
                    Errorresponse = JObject.Parse(ANS1response(ModuleName, RowtData, ID, ANS1count, sharedVariable, hCtrl, MedicationModel, ProviderUsername, PatientId));
                    if (Errorresponse.Rcopia != "")
                    {
                        break;
                    }
                    if (Errorresponse.exception == "DrFirst ANS cannot be called within 10 minutes,Please retry later")
                    {
                        MDVLogger.RcopiaLogMessage("Error", "", "", "Using ANS1", MDVUtility.ToStr(Errorresponse.exception), ANS1count);
                        break;
                    }
                    else if (Errorresponse.Error == "error")
                    {
                        MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", MDVUtility.ToStr(Errorresponse.Error), ANS1count);

                        break;
                    }
                    else
                    {
                        MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", MDVUtility.ToStr(Errorresponse.exception), ANS1count);
                    }
                    int milliseconds = 30000;
                    Thread.Sleep(milliseconds);
                }
            }
            if (ANS1count > 2)
            {

                Errorresponse = JObject.Parse(ANS2response(ModuleName, RowtData, ID, sharedVariable, hCtrl, MedicationModel, ProviderUsername, PatientId));
                if (Errorresponse.Rcopia != "")
                {

                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS2", MDVUtility.ToStr(Errorresponse.exception));
                }

            }
            return (Newtonsoft.Json.JsonConvert.SerializeObject(Errorresponse));


        }

        public string UploadURLS(string ModuleName, DataRow RowData, string ID, int count, SharedVariable sharedVariable = null, HttpContext httpCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        {
            string ErrorUpDownloadUrls = string.Empty;
            try
            {
                if (httpCtrl == null)
                {
                    httpCtrl = HttpContext.Current;
                }
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel model = new RcopiaModel();
                List<RcopiaModel> ListRcopia = GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:8080/");
                string error = string.Empty;
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                BLObject<DSRcopia> obj = SelectGetUrls(sharedVariable);
                dsRcopia = obj.Data;
                model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);
                model.EngineUploadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]);
                string UploadUrl = model.EngineUploadURL;
                string DownloadURL = model.EngineDownloadURL;
                var upload = string.Empty;
                var xml = string.Empty;
                var DownloadUrl = string.Empty;
                if (ModuleName == "Patient")
                {
                    upload = MDVUtility.GetXmlForAddPatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
                    xml = httpCtrl.Server.UrlEncode(upload);
                }
                else if (ModuleName == "Problem")
                {
                    upload = MDVUtility.GetXmlForAddProblemList(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
                    xml = httpCtrl.Server.UrlEncode(upload);
                }
                else if (ModuleName == "UpdatePatient")
                {
                    upload = MDVUtility.GetXmlForUpdatePatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
                    xml = httpCtrl.Server.UrlEncode(upload);
                }
                else if (ModuleName == "UpdateProblem")
                {
                    upload = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
                    xml = upload;
                }
                else if (ModuleName == "Medication")
                {
                    upload = MDVUtility.GetXmlForUpdateMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
                    xml = httpCtrl.Server.UrlEncode(upload);
                }
                else if (ModuleName == "MedicationDelete")
                {
                    upload = MDVUtility.GetXmlForUpdateMedicationDelete(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
                    xml = httpCtrl.Server.UrlEncode(upload);
                }

                MDVLogger.RcopiaLogMessage("Request: Send_" + ModuleName, ID, "", UploadUrl + "?xml=" + xml, "", count);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadUrl);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Accept = "application/xml";
                xml = "xml=" + xml;
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                request.ContentLength = bytes.Length;
                using (Stream putStream = request.GetRequestStream())
                {
                    putStream.Write(bytes, 0, bytes.Length);

                }

                var GetPatientDataANS1 = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        GetPatientDataANS1 = reader.ReadToEnd();
                    }
                }

                //HttpResponseMessage ResponseUploadPatientANS1 = client.GetAsync(Uploadurl).Result;
                //var GetPatientDataANS1 = ResponseUploadPatientANS1.Content.ReadAsStringAsync().Result;
                if (GetPatientDataANS1 != string.Empty)
                {
                    XmlDocument Xmldoc = new XmlDocument();
                    Xmldoc.LoadXml(GetPatientDataANS1);


                    string status = "";
                    string Problemstatus = "";
                    XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                    XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");


                    foreach (XmlNode node in statusNode)
                    {
                        status = node.InnerText;
                    }
                    foreach (XmlNode noderror in statusNodeerror)
                    {
                        error = noderror.InnerText;
                    }
                    XmlNodeList statusNodeInProblem = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/Status");
                    foreach (XmlNode node in statusNodeInProblem)
                    {
                        Problemstatus = node.InnerText;
                    }
                    if (status == "ok" && Problemstatus != "error")
                    {
                        string RcopiaID = "";
                        XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                        foreach (XmlNode node in RcopialNode)
                        {
                            RcopiaID = node.InnerText;
                        }
                        MDVLogger.RcopiaLogMessage("Response: Send_" + ModuleName, ID, status, GetPatientDataANS1, "", count);
                        var response = new
                        {
                            Rcopia = RcopiaID,
                            Error = "",
                            exception = ""
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else if (status == "error" || Problemstatus == "error")
                    {
                        var response = new
                        {
                            Rcopia = "",
                            Error = "error",
                            exception = ""
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                }
            }
            catch (Exception ex)
            {
                ErrorUpDownloadUrls = ex.Message;
            }
            var response1 = new
            {
                Rcopia = "",
                Error = "",
                exception = ErrorUpDownloadUrls
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
        }
        public string ANS1response(string ModuleName, DataRow RowData, string ID, int count, SharedVariable sharedVariable = null, HttpContext httpCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        {
            DateTime Modified;
            string exception = "DrFirst ANS cannot be called within 10 minutes,Please retry later";
            try
            {
                if (httpCtrl == null)
                {
                    httpCtrl = HttpContext.Current;
                }
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel model = new RcopiaModel();
                List<RcopiaModel> ListRcopia = GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> objGetUrl = SelectGetUrls(sharedVariable);
                dsRcopia = objGetUrl.Data;
                Modified = MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.ModifiedOnColumn.ColumnName]);
                int minutes = Convert.ToInt32(DateTime.Now.Subtract(Modified).TotalMinutes);
                if (minutes > 10)
                {

                    BLObject<DSRcopia> obj = SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                    HttpClient client = new HttpClient();
                    dsRcopia = obj.Data;
                    model.RcopiaANS = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSColumn.ColumnName]);
                    string RcopiaANS = model.RcopiaANS;


                    string error = string.Empty;
                    client.DefaultRequestHeaders.Accept.Add(
                           new MediaTypeWithQualityHeaderValue("application/xml"));
                    var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                    var ANS1url = RcopiaANS + "?xml=" + inputdata;
                    HttpResponseMessage ResponseUploadPatient = client.GetAsync(ANS1url).Result;

                    if (ResponseUploadPatient != null)
                    {
                        var GetdataANS1 = ResponseUploadPatient.Content.ReadAsStringAsync().Result;
                        XmlDocument DocANS1 = new XmlDocument();
                        DocANS1.LoadXml(GetdataANS1);
                        XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                        XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                        XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                        string UploadUrlANS1 = string.Empty;
                        string downloadUrlANS1 = string.Empty;
                        string WebBrowserURLANS1 = string.Empty;
                        foreach (XmlNode node in nodelistWebBrowserURLANS1)
                        {
                            WebBrowserURLANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodeListuploadurlANS1)
                        {
                            UploadUrlANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodelistDownloadurlANS1)
                        {
                            downloadUrlANS1 = node.InnerText;
                        }

                        var uploadPatientANS1 = string.Empty;
                        var UploadANS1URL = string.Empty;
                        var xml = string.Empty;
                        if (ModuleName == "Patient")
                        {
                            uploadPatientANS1 = MDVUtility.GetXmlForAddPatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
                            xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
                        }
                        else if (ModuleName == "Problem")
                        {
                            uploadPatientANS1 = MDVUtility.GetXmlForAddProblemList(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
                            xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
                        }
                        else if (ModuleName == "UpdatePatient")
                        {
                            uploadPatientANS1 = MDVUtility.GetXmlForUpdatePatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
                            xml = uploadPatientANS1;
                        }
                        else if (ModuleName == "UpdateProblem")
                        {
                            uploadPatientANS1 = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
                            xml = uploadPatientANS1;
                        }
                        else if (ModuleName == "Medication")
                        {
                            uploadPatientANS1 = MDVUtility.GetXmlForUpdateMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
                            xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
                        }
                        else if (ModuleName == "MedicationDelete")
                        {
                            uploadPatientANS1 = MDVUtility.GetXmlForUpdateMedicationDelete(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
                            xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
                        }
                        //UploadUrlANS1
                        //uploadPatientANS1 = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", RowData,ID);
                        //UploadANS1 = UploadUrlANS1 + "/getURL?xml=" + httpCtrl.Server.UrlEncode(uploadPatientANS1);
                        MDVLogger.RcopiaLogMessage("Request: Send_Patient", ID, "", UploadUrlANS1 + "?xml=" + xml, "", count);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadUrlANS1);
                        request.Method = "POST";
                        request.ContentType = "application/xml";
                        request.Accept = "application/xml";
                        xml = "xml=" + xml;
                        byte[] bytes = Encoding.UTF8.GetBytes(xml);
                        request.ContentLength = bytes.Length;
                        using (Stream putStream = request.GetRequestStream())
                        {
                            putStream.Write(bytes, 0, bytes.Length);

                        }

                        var GetPatientDataANS1 = string.Empty;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                                GetPatientDataANS1 = reader.ReadToEnd();
                            }
                        }

                        //HttpResponseMessage ResponseUploadPatientANS1 = client.GetAsync(UploadANS1URL).Result;
                        //var GetPatientDataANS1 = ResponseUploadPatientANS1.Content.ReadAsStringAsync().Result;
                        if (GetPatientDataANS1 != string.Empty)
                        {
                            model.URLID = 1;
                            model.EngineDownloadURL = downloadUrlANS1;
                            model.EngineUploadURL = UploadUrlANS1;
                            model.WebBrowserURL = WebBrowserURLANS1;
                            model.CreatedOn = DateTime.Now;
                            model.ModifiedOn = DateTime.Now;
                            model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dsRcopia = new DSRcopia();
                            DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                            dr.UrlID = model.URLID;
                            dr.EngineDownloadURL = model.EngineDownloadURL;
                            dr.EngineUploadURL = model.EngineUploadURL;
                            dr.WebBrowserURL = model.WebBrowserURL;
                            dr.IsActive = true;
                            dr.CreatedOn = model.CreatedOn;
                            dr.ModifiedOn = model.ModifiedOn;
                            dr.CreatedBy = model.CreatedBy;
                            dr.ModifiedBy = model.ModifiedBy;
                            dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                            BLObject<DSRcopia> objMofieddate = UpDateGetUrl(dsRcopia, sharedVariable);
                            dsRcopia = objMofieddate.Data;


                            XmlDocument Xmldoc = new XmlDocument();
                            Xmldoc.LoadXml(GetPatientDataANS1);
                            exception = string.Empty;

                            string status = "";
                            XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                            XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

                            foreach (XmlNode node in statusNode)
                            {
                                status = node.InnerText;
                            }
                            foreach (XmlNode noderror in statusNodeerror)
                            {
                                error = noderror.InnerText;
                            }
                            string Problemstatus = "";
                            XmlNodeList statusNodeInProblem = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/Status");
                            foreach (XmlNode node in statusNodeInProblem)
                            {
                                Problemstatus = node.InnerText;
                            }
                            if (status == "ok" && Problemstatus != "error")
                            {
                                string RcopiaID = "";
                                XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                                foreach (XmlNode node in RcopialNode)
                                {
                                    RcopiaID = node.InnerText;
                                }
                                MDVLogger.RcopiaLogMessage("Response: Send_Patient", ID, status, GetPatientDataANS1, "", count);
                                var response = new
                                {
                                    Rcopia = RcopiaID,
                                    Error = "",
                                    exception = ""
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (status == "error" || Problemstatus == "error")
                            {

                                var response = new
                                {
                                    Rcopia = "",
                                    Error = "error",
                                    exception = ""
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                            }


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }
            var response1 = new
            {
                Rcopia = "",
                Error = "",
                exception = exception
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
        }
        public string ANS2response(string ModuleName, DataRow RowData, string ID, SharedVariable sharedVariable = null, HttpContext httpCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        {
            string exception = string.Empty;
            try
            {
                if (httpCtrl == null)
                {
                    httpCtrl = HttpContext.Current;
                }
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                dsRcopia = obj.Data;
                HttpClient client = new HttpClient();
                RcopiaModel model = new RcopiaModel();
                List<RcopiaModel> ListRcopia = GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
                //string RcopiaANS = RcopiaANSbackup;

                string error = string.Empty;
                client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/xml"));
                var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                var ANS1url = RcopiaANSbackup + "?xml=" + inputdata;
                MDVLogger.RcopiaLogMessage("Request: GET URL from ANS2", ID, "", ANS1url);
                HttpResponseMessage ResponseUploadPatient = client.GetAsync(ANS1url).Result;
                MDVLogger.RcopiaLogMessage("Response: GET URL  from ANS2", ID, "", ResponseUploadPatient.ToString());
                if (ResponseUploadPatient != null)
                {
                    var GetdataANS1 = ResponseUploadPatient.Content.ReadAsStringAsync().Result;
                    XmlDocument DocANS1 = new XmlDocument();
                    DocANS1.LoadXml(GetdataANS1);
                    XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                    XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                    XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                    string UploadUrlANS2 = string.Empty;
                    string downloadUrlANS2 = string.Empty;
                    string WebBrowserURLANS2 = string.Empty;
                    foreach (XmlNode node in nodelistWebBrowserURLANS1)
                    {
                        WebBrowserURLANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodeListuploadurlANS1)
                    {
                        UploadUrlANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodelistDownloadurlANS1)
                    {
                        downloadUrlANS2 = node.InnerText;
                    }


                    var uploadPatientANS2 = string.Empty;
                    var UploadANS1URL = string.Empty;
                    var xml = string.Empty;
                    if (ModuleName == "Patient")
                    {
                        uploadPatientANS2 = MDVUtility.GetXmlForAddPatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
                        xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
                    }
                    else if (ModuleName == "Problem")
                    {
                        uploadPatientANS2 = MDVUtility.GetXmlForAddProblemList(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
                        xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
                    }
                    else if (ModuleName == "UpdatePatient")
                    {
                        uploadPatientANS2 = MDVUtility.GetXmlForUpdatePatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
                        xml = uploadPatientANS2;
                    }
                    else if (ModuleName == "UpdateProblem")
                    {
                        uploadPatientANS2 = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
                        xml = uploadPatientANS2;
                    }
                    else if (ModuleName == "Medication")
                    {
                        uploadPatientANS2 = MDVUtility.GetXmlForUpdateMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
                        xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
                    }
                    else if (ModuleName == "MedicationDelete")
                    {
                        uploadPatientANS2 = MDVUtility.GetXmlForUpdateMedicationDelete(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
                        xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
                    }
                    //
                    //var uploadPatientANS1 = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", RowData,ID);
                    //var UploadANS2 = UploadUrlANS2 + "/getURL?xml=" + HttpContext.Current.Server.UrlEncode(uploadPatientANS1);
                    MDVLogger.RcopiaLogMessage("Request:" + ModuleName, ID, "", UploadUrlANS2 + "?xml=" + xml);


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadUrlANS2);
                    request.Method = "POST";
                    request.ContentType = "application/xml";
                    request.Accept = "application/xml";
                    xml = "xml=" + xml;
                    byte[] bytes = Encoding.UTF8.GetBytes(xml);
                    request.ContentLength = bytes.Length;
                    using (Stream putStream = request.GetRequestStream())
                    {
                        putStream.Write(bytes, 0, bytes.Length);

                    }

                    var GetPatientDataANS1 = string.Empty;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                            GetPatientDataANS1 = reader.ReadToEnd();
                        }
                    }
                    if (GetPatientDataANS1 != string.Empty)
                    {
                        model.URLID = 1;
                        model.EngineDownloadURL = downloadUrlANS2;
                        model.EngineUploadURL = UploadUrlANS2;
                        model.WebBrowserURL = WebBrowserURLANS2;
                        model.CreatedOn = DateTime.Now;
                        model.ModifiedOn = DateTime.Now;
                        model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dsRcopia = new DSRcopia();
                        DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                        dr.UrlID = model.URLID;
                        dr.EngineDownloadURL = model.EngineDownloadURL;
                        dr.EngineUploadURL = model.EngineUploadURL;
                        dr.WebBrowserURL = model.WebBrowserURL;
                        dr.IsActive = true;
                        dr.CreatedOn = model.CreatedOn;
                        dr.ModifiedOn = model.ModifiedOn;
                        dr.CreatedBy = model.CreatedBy;
                        dr.ModifiedBy = model.ModifiedBy;
                        dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                        BLObject<DSRcopia> objMofieddate = UpDateGetUrl(dsRcopia, sharedVariable);

                        XmlDocument Xmldoc = new XmlDocument();
                        Xmldoc.LoadXml(GetPatientDataANS1);


                        string status = "";
                        XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                        XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

                        foreach (XmlNode node in statusNode)
                        {
                            status = node.InnerText;
                        }
                        foreach (XmlNode noderror in statusNodeerror)
                        {
                            error = noderror.InnerText;
                        }
                        string Problemstatus = "";
                        XmlNodeList statusNodeInProblem = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/Status");
                        foreach (XmlNode node in statusNodeInProblem)
                        {
                            Problemstatus = node.InnerText;
                        }
                        if (status == "ok" && Problemstatus != "error")
                        {
                            string RcopiaID = "";
                            XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                            foreach (XmlNode node in RcopialNode)
                            {
                                RcopiaID = node.InnerText;
                            }
                            MDVLogger.RcopiaLogMessage("Response:" + ModuleName, ID, status, GetPatientDataANS1, "");
                            var response = new
                            {
                                Rcopia = RcopiaID,
                                Error = "",
                                exception = ""
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else if (status == "error" || Problemstatus == "error")
                        {

                            var response = new
                            {
                                Rcopia = "",
                                Error = "error",
                                exception = ""
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        }


                    }
                }


            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }
            var response1 = new
            {
                Rcopia = "",
                Error = "",
                exception = exception
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
        }

        public List<RcopiaModel> GetRcopiaInfo(SharedVariable sharedVariable = null)
        {
            List<RcopiaModel> RcopiaInfo = new List<RcopiaModel>();
            try
            {
                RcopiaModel model = new RcopiaModel();
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                dsRcopia = obj.Data;
                HttpClient client = new HttpClient();
                model.RcopiaANSbackup = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
                model.RcopiaScretkey = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
                model.RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
                model.RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
                model.RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
                model.RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);
                RcopiaInfo.Add(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RcopiaInfo;
        }

        public string DeleteDrFirstProblem(ProblemListModel ProblemModel)
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:8080/");

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml"));
                DSRcopia dsRcopia1 = new DSRcopia();
                BLObject<DSRcopia> obj1 = SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode);
                dsRcopia1 = obj1.Data;
                if (obj1.Data != null)
                {
                    if (dsRcopia1.SoftwareCustomersInfo.Rows.Count > 0)
                    {
                        string RcopiaANSbackup = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
                        string RcopiaScretkey = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
                        string RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
                        string RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
                        string RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
                        string RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);

                        var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                        DSRcopia dsRcopia = new DSRcopia();
                        BLObject<DSRcopia> obj = SelectGetUrls();
                        dsRcopia = obj.Data;
                        if (obj.Data != null)
                        {
                            if (dsRcopia.Rcopia_GetUrl.Rows.Count > 0)
                            {

                                var url = string.Concat(MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]), "/getURL?xml=", inputdata);

                                HttpResponseMessage response = client.GetAsync(url).Result;

                                if (response != null)
                                {
                                    var getdata = response.Content.ReadAsStringAsync().Result;
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(getdata);
                                    //string jsonText = JsonConvert.SerializeXmlNode(doc);
                                    XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
                                    XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");

                                    string UploadUrl = string.Empty;
                                    string downloadUrl = string.Empty;
                                    foreach (XmlNode node in nodeListuploadurl)
                                    {
                                        UploadUrl = node.InnerText;
                                    }
                                    foreach (XmlNode node in nodelistDownloadurl)
                                    {
                                        downloadUrl = node.InnerText;
                                    }




                                    //string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername,DSPatient.PatientsRow patient
                                    var DeleteProblemXml = MDVUtility.GetXmlForDeleteProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, ProblemModel.PatientId, ProblemModel.ProblemListId, ProblemModel.Description, ProblemModel.StartDate);
                                    var Uploadurl = UploadUrl + "?xml=" + DeleteProblemXml;
                                    HttpResponseMessage ResponseDeleteProblem = client.GetAsync(Uploadurl).Result;
                                    var GetProblemdDeleteData = ResponseDeleteProblem.Content.ReadAsStringAsync().Result;

                                    XmlDocument Xmldoc = new XmlDocument();
                                    Xmldoc.LoadXml(GetProblemdDeleteData);

                                    string status = "";
                                    XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");


                                    foreach (XmlNode node in statusNode)
                                    {
                                        status = node.InnerText;
                                    }
                                    if (status == "ok")
                                    {
                                        return "Ok";
                                    }
                                    else if (status == "error")
                                    {
                                        return "error";
                                    }
                                }
                                return "";
                            }
                            else
                            {
                                throw new Exception("Error In Delete Problem On DrFirst");
                            }
                        }
                        else
                        {
                            throw new Exception("Error In Delete Problem On DrFirst");
                        }

                    }
                    else
                    {
                        throw new Exception("Error In Delete Problem On DrFirst");
                    }
                }
                else
                {
                    throw new Exception("Error In Delete Problem On DrFirst");
                }


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        #endregion
    }
}
