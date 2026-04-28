using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Helpers.Clinical.Summary;
using MDVision.IEHR.EMR.Model.Clinical;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

using System.Threading.Tasks;
using MDVision.IEHR.EMR.HL7Folder.Summary;
using MDVision.Model.CCDA;
using MDVision.Datasets;
using System.Configuration;
using System.Xml.Xsl;
using System.Net;
using System.Xml;
using System.Text;
using System.Web.Configuration;

namespace MDVision.IEHR.EMR.Services
{
    public class ClinicalSummaryController : ApiController
    {

        /// <summary>
        /// Module Name: PlanOfCare
        /// Author: Ahmad Raza
        /// Created Date: 04-04-2016
        /// Description: Handles different PlanOfCare methods
        /// </summary> 
        /// <param name="AllData" type="JObject">contains PlanOfCareData</param>
        [HttpPost]
        public string PlanOfCare(JObject AllData)
        {
            string response = null;
            List<object> lstDiseaseModel = new List<object>();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            ClinicalSummaryModel PlanModel = null;
            ClinicalSummaryModel model = JsonConvert.DeserializeObject<ClinicalSummaryModel>(MDVUtility.ToStr(AllData["data"]));
            PlanOfCareGoalModel modelDisease = ser.Deserialize<PlanOfCareGoalModel>(MDVUtility.ToStr(AllData["data"]));
            ClinicalSummaryHelper helperPlan = new ClinicalSummaryHelper();


            if (model.commandType.ToLower() == "save_planofcare" || model.commandType.ToLower() == "update_planofcare")
            {
                if (model.PlanOfCareType.ToLower() == "goal")
                {
                    lstDiseaseModel.Add(modelDisease);
                    response = helperPlan.insertUpdatePlanOfCare(model, lstDiseaseModel);
                }
            }

            else if (model.commandType.ToLower() == "getlatest_planofcareby_patientid")
            {
                response = null;
                response = helperPlan.fillPlanOfCare(model, MDVUtility.ToInt64(model.PlanOfCareId));
            }
            else if (model.commandType.ToLower() == "attach_planofcare_from_notes")
            {
                response = null;
                response = helperPlan.attachPlanOfCareWithNotes(model);
            }
            else if (model.commandType.ToLower() == "detach_planofcare_from_notes")
            {
                response = null;
                response = helperPlan.detachPlanOfCareFromNotes(model.PlanOfCareId, MDVUtility.ToInt64(model.NoteId));
            }

            else if (model.commandType.ToLower() == "fill_planofcare")
            {
                response = null;
                response = helperPlan.fillPlanOfCare(model, MDVUtility.ToInt64(model.PlanOfCareId));
            }
            else if (model.commandType.ToLower() == "delete_planofcaregoal")
            {
                response = null;
                response = helperPlan.deletPlanOfCareGoal(model.GoalId, model.PlanOfCareId);
            }
            return response;
        }

        [HttpPost]
        public string Cognitive(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            CognitiveModel model = JsonConvert.DeserializeObject<CognitiveModel>(MDVUtility.ToStr(AllData["data"]));
            CognitiveHelper helperCognitive = new CognitiveHelper();


            if (model.commandType.ToLower() == "save_cognitive" || model.commandType.ToLower() == "update_cognitive")
            {
                response = null;
                response = helperCognitive.insertUpdateCognitive(model);
            }

            else if (model.commandType.ToLower() == "fill_cognitive")
            {
                response = null;
                response = helperCognitive.fillCognitive(model, MDVUtility.ToInt64(model.CognitiveId));
            }

            else if (model.commandType.ToLower() == "delete_cognitivestatus")
            {
                response = null;
                response = helperCognitive.deletCognitiveStatus(model.CognitiveStatusId, model.CognitiveId);
            }
            else if (model.commandType.ToUpper() == "DELETE_FUNCTIONALSTATUS")
            {
                response = null;
                response = helperCognitive.deletFunctionalStatus(model.FunctionalStatusId, model.CognitiveId);
            }

            else if (model.commandType.ToUpper() == "DELETE_MENTALSTATUS")
            {
                response = null;
                response = helperCognitive.deletMentalStatus(model.MentalStatusId, model.CognitiveId);
            }
            else if (model.commandType.ToUpper() == "ATTACH_COGNITIVE_WITH_NOTES")
            {
                response = null;
                response = helperCognitive.attachCognitiveWithNotes(model);
            }

            else if (model.commandType.ToLower() == "detach_cognitive_from_notes")
            {
                response = null;
                response = helperCognitive.detachCognitiveFromNotes(model.CognitiveId, MDVUtility.ToInt64(model.NoteId));
            }

            else if (model.commandType.ToLower() == "getlatest_cognitiveby_patientid")
            {
                response = null;
                response = helperCognitive.fillCognitive(model, MDVUtility.ToInt64(model.CognitiveId));
            }


            return response;
        }

        /// <summary>
        /// Module Name: ClinicalSummary
        /// Author: Farooq AHmad
        /// Created Date: 25-04-2016
        /// Description: Handles different ClinicalSummary methods
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        public string ClinicalSummary(JObject AllData)
        {
            ClinicalSummaryHelper helperPlan = new ClinicalSummaryHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ClinicalSummaryModel model = JsonConvert.DeserializeObject<ClinicalSummaryModel>(MDVUtility.ToStr(AllData["data"]));
            if (!string.IsNullOrEmpty(model.Patients))
            {
                model.PatientInfo = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(model.Patients);
            }
            string Encrypted = string.Empty;
            if (model.commandType.ToLower() == "xml")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.ClinicalSummary);
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlreferral")
            {
                var type = model.commandType.ToLower();

                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.ReferralSummary, MDVUtility.ToInt64(model.referralProvider), model.raferralReason);
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            if (model.commandType.ToLower() == "xmlcontinuityofcaredocument")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.ContinutyofCaredocument);
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlreferralnote")
            {
                var type = model.commandType.ToLower();

                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.ReferralNote, MDVUtility.ToInt64(model.referralProvider), model.raferralReason);
                var response = new
                {
                    status = true,
                    data = data,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "html")
            {
                string data = CCDAGenrator.DecryptRawXml(model.XMLData, Convert.ToBoolean(model.DataIsEncrypted), model.Password);
                data = new string(data.Where(c => !char.IsControl(c)).ToArray());
                var output = CCDAGenrator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), data, true, "", model.Template);
                var response = new
                {
                    status = true,
                    data = output,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "exportccda")
            {
                string data = helperPlan.loadDataPortabilty(model);
                var response = new
                {
                    status = true,
                    data = data,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlcareplan")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.CarePlan);
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "aroreport")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.AROReport, 0, "", CreateReportsParams(model.QueryString));
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "aupreport")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.AUPReport, 0, "", CreateReportsParams(model.QueryString));
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlhealthcaresurvey")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.HealthCareSurveys);
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlcancerreport")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CCDAGenrator.DocumentTemplateType.CancerReport);
                var resp = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var response = new
                {
                    status = true,
                    data = data,
                    Message = "",
                };
                if (!string.IsNullOrWhiteSpace(resp["status"]))
                {
                    if (resp["status"] == "false")
                        response = new
                        {
                            status = false,
                            data = data,
                            Message = "Mandatory information is missing.",
                        };
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }


            var response2 = new
            {
                status = true,
                data = string.Empty,
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
        }


        [HttpPost]
        public string DownloadFile(JObject AllData)
        {
            var context = HttpContext.Current;
            ClinicalSummaryHelper helperPlan = new ClinicalSummaryHelper();

            ClinicalSummaryModel model = JsonConvert.DeserializeObject<ClinicalSummaryModel>(MDVUtility.ToStr(AllData["data"]));

            var xmlData = CCDAGenrator.DecryptRawXml(model.XMLData, Convert.ToBoolean(model.DataIsEncrypted), model.Password);
            var data = helperPlan.HashingAndEncryption(xmlData, Convert.ToBoolean(model.IncludeHashCode), Convert.ToBoolean(model.Encryption), model.Password);
            string HashCode = string.Empty;
            if (Convert.ToBoolean(model.IncludeHashCode))
                HashCode = data["hashCode"];
            CCDAGenrator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), data["xmlData"], true, xmlData, model.Template);
            var folderPath = System.Web.HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
            var xmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.xml", model.NoteId, model.ProviderId, model.PatientId, MDVSession.Current.AppUserId);
            var htmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.html", model.NoteId, model.ProviderId, model.PatientId, MDVSession.Current.AppUserId);
            string[] xmlfilesPath = Directory.GetFiles(folderPath, xmlfileName, SearchOption.AllDirectories);
            string[] htmlfilesPath = Directory.GetFiles(folderPath, htmlfileName, SearchOption.AllDirectories);

            List<string> xmlFiles = xmlfilesPath.ToList();
            List<string> htmlFiles = htmlfilesPath.ToList();

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("HashCode", HashCode);
            int counter = 0;
            foreach (var xmlFile in xmlFiles)
            {
                byte[] bcode = File.ReadAllBytes(xmlFile);
                result.Add(string.Format("XMLByte{0}", (counter == 0 ? string.Empty : Convert.ToString(counter))), Convert.ToBase64String(bcode));
            }
            counter = 0;
            foreach (var htmlFile in htmlFiles)
            {
                byte[] bcode = File.ReadAllBytes(htmlFile);
                result.Add(string.Format("HTMLByte{0}", (counter == 0 ? string.Empty : Convert.ToString(counter))), Convert.ToBase64String(bcode));
            }
            //fixme why the BLL function is directly being called from controller ? {Ali Awan}
            if (!string.IsNullOrWhiteSpace(model.PatientId) && model.PatientId != "0")
            {
                DataTable dtDBAudit = new BLLClinical().insertClinicalSummaryCopyAudit(model.PatientId, model.SummaryType);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result); ;
        }

        [HttpPost]
        public string SendEmail(JObject AllData)
        {
            ClinicalSummaryHelper helperPlan = new ClinicalSummaryHelper();

            ClinicalSummaryModel model = JsonConvert.DeserializeObject<ClinicalSummaryModel>(MDVUtility.ToStr(AllData["data"]));

            var xmlData = CCDAGenrator.DecryptRawXml(model.XMLData, Convert.ToBoolean(model.DataIsEncrypted), model.Password);
            var data = helperPlan.HashingAndEncryption(xmlData, Convert.ToBoolean(model.IncludeHashCode), Convert.ToBoolean(model.Encryption), model.Password);
            string HashCode = string.Empty;
            if (Convert.ToBoolean(model.IncludeHashCode))
                HashCode = data["hashCode"];
            CCDAGenrator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), data["xmlData"], true, xmlData);
            var folderPath = System.Web.HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
            var xmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.xml", model.NoteId, model.ProviderId, model.PatientId, MDVSession.Current.AppUserId);
            var htmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.html", model.NoteId, model.ProviderId, model.PatientId, MDVSession.Current.AppUserId);
            string[] xmlfilesPath = Directory.GetFiles(folderPath, xmlfileName, SearchOption.AllDirectories);
            string[] htmlfilesPath = Directory.GetFiles(folderPath, htmlfileName, SearchOption.AllDirectories);

            List<string> xmlFiles = xmlfilesPath.ToList();
            List<string> htmlFiles = htmlfilesPath.ToList();



            int counter = 0;
            string XmlString = string.Empty, htmlString = string.Empty;

            if (Convert.ToBoolean(model.IncludeXML))
            {
                foreach (var xmlFile in xmlFiles)
                {
                    XmlString = File.ReadAllText(xmlFile);
                }
            }

            if (Convert.ToBoolean(model.IncludeHTML))
            {
                foreach (var htmlFile in htmlFiles)
                {
                    htmlString = File.ReadAllText(htmlFile);
                }
            }

            try
            {
                string phiMail_RemoteUser = model.msgType == "direct" ? ConfigurationManager.AppSettings["phiMail_DirectUser"] : ConfigurationManager.AppSettings["phiMail_EdgeUser"];
                dynamic message = helperPlan.SendEmailThroughPhiMailConnector(model.toEmail, XmlString, htmlString, phiMail_RemoteUser, model.MessageDetail);

                if (message.status)
                {
                    try
                    {

                        DSMessage dsMessage = new DSMessage();
                        DSMessage.DirectMessagesRow dr = dsMessage.DirectMessages.NewDirectMessagesRow();


                        if (!string.IsNullOrEmpty(phiMail_RemoteUser))
                            dr.EmailFrom = MDVUtility.ToStr(phiMail_RemoteUser);
                        if (!string.IsNullOrEmpty(model.toEmail))
                            dr.EmailTo = MDVUtility.ToStr(model.toEmail);
                        if (!string.IsNullOrEmpty(model.PatientAccountNo))
                            dr.AccountNumber = model.PatientAccountNo;
                        dr.isHtml = Convert.ToBoolean(model.IncludeHTML);
                        dr.isXml = Convert.ToBoolean(model.IncludeXML);
                        if (!string.IsNullOrEmpty(Convert.ToString(model.DOS)))
                            dr.DOS = model.DOS;
                        if (!string.IsNullOrEmpty(model.DocType))
                            dr.DocType = MDVUtility.ToStr(model.DocType);

                        dr.DirectMsgId = MDVUtility.ToStr(message.Message);
                        dr.DateTime = DateTime.Now;
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        dr.NoteId = MDVUtility.ToInt64(model.NoteId);

                        #region Database Insertion
                        dsMessage.DirectMessages.AddDirectMessagesRow(dr);

                        BLObject<DSMessage> obj = new BLLMessage().InsertDirectMessage(dsMessage);

                        BLObject<DSCCDA> dsCCDAReconcilation = new BLObject<DSCCDA>();
                        BLLCCDA _bllCCDAobj = new BLLCCDA();
                        dsCCDAReconcilation = _bllCCDAobj.InsertMUSetting(MDVUtility.ToInt64(model.PatientId), 0, 0, MDVUtility.ToInt64(model.NoteId), false, false, false, false, true, 0, 0, false);


                        #endregion
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

                var result = new
                {
                    status = true,
                    data = "Transmit successfull.",
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    status = false,
                    data = ex.Message,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }

        }

        [HttpPost]
        public async Task<string> ValidateCCDA(ValidateModel objValidateModel)
        {
            string strResult = "";
            try
            {
                List<string> errorImport = new List<string>();
                strResult = await new ClinicalSummary_ImportCCDA().ValidateCCDAData(objValidateModel.XMLContent, new List<string>(), errorImport, objValidateModel.FileType, objValidateModel.DocfileType, objValidateModel.DocfileName, objValidateModel.IsFile);
                //return Result;
            }
            catch (Exception ex)
            {
                var output = new
                {
                    status = false,
                    Message = ex.Message
                };
                strResult = JsonConvert.SerializeObject(output);
            }
            return strResult;
        }

        [HttpPost]
        public string ReconcileCCDABase64(ValidateModel objValidateModel)
        {
            string strResult = "";
            try
            {
                List<string> errorImport = new List<string>();
                DSCCDA dsCCDAReconcile = new DSCCDA();
                DSPatient ds = null;
                if (objValidateModel.FileType != "C32" && objValidateModel.FileType != "CCR")
                {
                    ds = new ClinicalSummary_ImportCCDA().ReconcileCCDABase64(objValidateModel.XMLContent, ref dsCCDAReconcile, new List<string>(), errorImport, objValidateModel.FileType, objValidateModel.DocfileType, objValidateModel.DocfileName, objValidateModel.IsFile);
                }
                string dataHtml = string.Empty;
                byte[] byteArray2 = Encoding.UTF8.GetBytes(objValidateModel.XMLContent);
                byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                string url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                string url2 = FilePath + url;
                XmlDocument doc = new XmlDocument();
                doc.Load(url2);
                string xmlcontents = doc.InnerXml;
                string html = GetHtmlFromXMLContent(xmlcontents, objValidateModel.FileType);
                var htmlfileName = string.Format("{0}.html", Guid.NewGuid());
                var folderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);
                //Save XML Content
                File.WriteAllText(htmlFilePath, html);

                dataHtml = MDVSession.Current.ImagePath.Replace("~/", string.Empty) + "/" + htmlfileName;
                //string html = GetHtmlFromXMLContent(xmlData);
                //var htmlfileName = string.Format("{0}.html", Guid.NewGuid());

                //var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);


                ////Save XML Content
                //File.WriteAllText(htmlFilePath, html);

                //var message = string.Empty;
                //if (error.Count > 0)
                //{
                //    message = error.Aggregate(message, (current, e) => string.Concat(current, e, "<br/>"));
                //}
                var output = new
                {
                    status = true,
                    //data = MDVSession.Current.ImagePath.Replace("~/", string.Empty) + "/" + htmlfileName,
                    PatientInfo = ds != null ? MDVUtility.JSON_DataTable(ds.Tables[ds.Patients.TableName]) : null,
                    AllergiesInfo = dsCCDAReconcile != null ? MDVUtility.JSON_DataTable(dsCCDAReconcile.Tables[dsCCDAReconcile.Allergy.TableName]) : null,
                    ProblemsInfo = dsCCDAReconcile != null ? MDVUtility.JSON_DataTable(dsCCDAReconcile.Tables[dsCCDAReconcile.ProblemList.TableName]) : null,
                    MedicationsInfo = dsCCDAReconcile != null ? MDVUtility.JSON_DataTable(dsCCDAReconcile.Tables[dsCCDAReconcile.Medication.TableName]) : null,
                    dataHtml = dataHtml,
                    url = url
                };
                strResult = JsonConvert.SerializeObject(output);
            }
            catch (Exception ex)
            {
                var output = new
                {
                    status = false,
                    Message = ex.Message
                };
                strResult = JsonConvert.SerializeObject(output);
            }
            return strResult;
        }

        private string GetHtmlFromXMLContent(string xmlData,string fileType = "CCDA")
        {
            xmlData = xmlData.Replace("0x3c", "&lt;").Replace("0x3e", "&gt;");
            var folderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);

            var htmlfileName = "CCDA_{Guid.NewGuid()}.html";
            var xmlfileName = "CCDA_{Guid.NewGuid()}.xml";
            //var htmlFilePath = @"{folderPath}\{htmlfileName}";
            //var xmlFilePath = @"{folderPath}\{xmlfileName}";
            var xmlFilePath = Path.GetTempFileName();
            var htmlFilePath = Path.GetTempFileName();

            XslCompiledTransform transform = new XslCompiledTransform();
            XmlUrlResolver resolver = new XmlUrlResolver { Credentials = CredentialCache.DefaultCredentials };
            XsltSettings settings = new XsltSettings() { EnableDocumentFunction = true };
            // load up the stylesheet
            string stylesheetPath = "";
            if (fileType == "C32")
            {
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\C32.xsl";
            }
            else if (fileType == "CCR")
            {
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCR.xsl";
            }
            else
            {
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCDA.xsl";
            }
            transform.Load(stylesheetPath, settings, resolver);
            // perform the transformation
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            doc.Save(xmlFilePath);

            transform.Transform(xmlFilePath, htmlFilePath);
            string content = File.ReadAllText(htmlFilePath);
            content = content.Replace("�", " ");
            //Delete Xml & Html File

            if (File.Exists(xmlFilePath))
                File.Delete(xmlFilePath);
            if (File.Exists(htmlFilePath))
                File.Delete(htmlFilePath);
            return content;
        }

        private Dictionary<string, object> CreateReportsParams(string reportsParamaters)
        {
            //if (reportsParamaters == null) throw new ArgumentNullException(nameof(reportsParamaters));
            var result = new Dictionary<string, object>();
            foreach (string key in HttpUtility.ParseQueryString(reportsParamaters).AllKeys)
            {
                if (!key.Equals("ReportName"))
                {
                    string currentKey = key;
                    var strings = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    var values = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    if (values == null) continue;
                    string currentValue = strings != null && string.IsNullOrEmpty(strings[0]) ? string.Empty : HttpUtility.UrlDecode(values[0]);
                    if (currentKey.Equals("PaymentDateFrom") || currentKey.Equals("PaymentDateTo") || currentKey.Equals("DOSFrom") || currentKey.Equals("DOSTo") || currentKey.Equals("VisitEntryDateFrom") || currentKey.Equals("VisitEntryDateTo") || currentKey.Equals("DatePaidFrom") || currentKey.Equals("DatePaidTo") || currentKey.Equals("EntryDateFrom") || currentKey.Equals("EntryDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("SubmitDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("AppointmentDateStart") || currentKey.Equals("AppointmentDateEnd") || currentKey.Equals("VisitDateFrom") || currentKey.Equals("VisitDateTo") || currentKey.Equals("FromRegistrationDate") || currentKey.Equals("ToRegistrationDate") || currentKey.Equals("CreateDateFrom") || currentKey.Equals("CreateDateTo") || currentKey.Equals("LastVisitFrom") || currentKey.Equals("LastVisitTo") || currentKey.Equals("DOBFrom") || currentKey.Equals("DOBTo") || currentKey.Equals("DOSStart") || currentKey.Equals("DOSEnd") || currentKey.Equals("ProbGivenDateFrom") || currentKey.Equals("ProbGivenDateTo"))
                    {
                        if (!string.IsNullOrEmpty(currentValue) && MDVUtility.IsDate(currentValue))
                        {
                            result.Add("@" + currentKey, Convert.ToDateTime(currentValue));
                        }
                        else
                        {
                            result.Add("@" + currentKey, DBNull.Value);
                        }
                    }
                    else if (currentKey.Equals("Balance") || currentKey.Equals("CPTCode") || currentKey.Equals("ICDCode"))
                    {
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            result.Add("@" + currentKey,
                                currentKey.Equals("Balance")
                                    ? MDVUtility.ToDouble(currentValue)
                                    : MDVUtility.ToLong(currentValue));
                        }
                        else
                        {
                            result.Add("@" + currentKey, DBNull.Value);
                        }
                    }
                    else
                    {
                        result.Add("@" + key, currentValue);
                    }
                }
            }
            result.Add("@EntityId", MDVSession.Current.EntityId);
            return result;
        }
    }

}