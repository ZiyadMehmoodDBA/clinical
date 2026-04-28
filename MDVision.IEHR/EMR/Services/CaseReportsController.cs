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


namespace MDVision.IEHR.EMR.Services
{
    public class CaseReportsController : ApiController
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

            CaseReportsModel PlanModel = null;
            CaseReportsModel model = JsonConvert.DeserializeObject<CaseReportsModel>(MDVUtility.ToStr(AllData["data"]));
            PlanOfCareGoalModel modelDisease = ser.Deserialize<PlanOfCareGoalModel>(MDVUtility.ToStr(AllData["data"]));
            CaseReportsHelper helperPlan = new CaseReportsHelper();


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
            List<object> lstCognitiveStatusModel = new List<object>();
            List<object> lstFunctionalStatusModel = new List<object>();
            List<object> lstMentalStatusModel = new List<object>();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            CaseReportsModel CognitiveModel = null;
            CaseReportsModel model = JsonConvert.DeserializeObject<CaseReportsModel>(MDVUtility.ToStr(AllData["data"]));
            CognitiveStatusModel CognitiveStatusModel = ser.Deserialize<CognitiveStatusModel>(MDVUtility.ToStr(AllData["data"]));
            FunctionalStatusModel FunctionalStatusModel = ser.Deserialize<FunctionalStatusModel>(MDVUtility.ToStr(AllData["data"]));
            MentalStatusModel MentalStatusModel = ser.Deserialize<MentalStatusModel>(MDVUtility.ToStr(AllData["data"]));

            CognitiveHelper helperCognitive = new CognitiveHelper(); 


            //if (model.commandType.ToLower() == "save_cognitive" || model.commandType.ToLower() == "update_cognitive")
            //{
            //    //  if (model.PlanOfCareType.ToLower() == "goal")
            //    //  {
            //    lstCognitiveStatusModel.Add(CognitiveStatusModel);
            //    lstFunctionalStatusModel.Add(FunctionalStatusModel);
            //    lstMentalStatusModel.Add(MentalStatusModel);
            //    response = helperCognitive.insertUpdateCognitive(model, lstCognitiveStatusModel, lstFunctionalStatusModel, lstMentalStatusModel);
            //    //   }
            //}

            //else if (model.commandType.ToLower() == "fill_cognitive")
            //{
            //    response = null;
            //    response = helperCognitive.fillCognitive(model, MDVUtility.ToInt64(model.CognitiveId));
            //}

             if (model.commandType.ToLower() == "delete_cognitivestatus")
            {
                response = null;
                response = helperCognitive.deletCognitiveStatus(CognitiveStatusModel.CognitiveStatusId, model.CognitiveId);
            }
            else if (model.commandType.ToUpper() == "DELETE_FUNCTIONALSTATUS")
            {
                response = null;
                response = helperCognitive.deletFunctionalStatus(FunctionalStatusModel.FunctionalStatusId, model.CognitiveId);
            }

            else if (model.commandType.ToUpper() == "DELETE_MENTALSTATUS")
            {
                response = null;
                response = helperCognitive.deletMentalStatus(MentalStatusModel.MentalStatusId, model.CognitiveId);
            }
            //else if (model.commandType.ToUpper() == "ATTACH_COGNITIVE_WITH_NOTES")
            //{
            //    response = null;
            //    response = helperCognitive.attachCognitiveWithNotes(model);
            //}

            else if (model.commandType.ToLower() == "detach_cognitive_from_notes")
            {
                response = null;
                response = helperCognitive.detachCognitiveFromNotes(model.CognitiveId, MDVUtility.ToInt64(model.NoteId));
            }

            //else if (model.commandType.ToLower() == "getlatest_cognitiveby_patientid")
            //{
            //    response = null;
            //    response = helperCognitive.fillCognitive(model, MDVUtility.ToInt64(model.CognitiveId));
            //}


            return response;
        }

        /// <summary>
        /// Module Name: CaseReports
        /// Author: Farooq AHmad
        /// Created Date: 25-04-2016
        /// Description: Handles different CaseReports methods
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        public string CaseReports(JObject AllData)
        {

            CaseReportsHelper helperPlan = new CaseReportsHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            CaseReportsModel model = JsonConvert.DeserializeObject<CaseReportsModel>(MDVUtility.ToStr(AllData["data"]));
            if (!string.IsNullOrEmpty(model.Patients))
            {
                model.PatientInfo = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(model.Patients);
            }
            string Encrypted = string.Empty;
            if (model.commandType.ToLower() == "xml")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadCaseReportsXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CaseReportsCCDAGenerator.DocumentTemplateType.ClinicalSummary);
                var response = new
                {
                    status = true,
                    data = data,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlreferral")
            {
                var type = model.commandType.ToLower();

                string data = helperPlan.loadCaseReportsXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CaseReportsCCDAGenerator.DocumentTemplateType.ReferralSummary, MDVUtility.ToInt64(model.referralProvider), model.raferralReason);
                var response = new
                {
                    status = true,
                    data = data,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            if (model.commandType.ToLower() == "xmlcontinuityofcaredocument")
            {
                var type = model.commandType.ToLower();
                string data = helperPlan.loadCaseReportsXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CaseReportsCCDAGenerator.DocumentTemplateType.ContinutyofCaredocument);
                var response = new
                {
                    status = true,
                    data = data,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "xmlreferralnote")
            {
                var type = model.commandType.ToLower();

                string data = helperPlan.loadCaseReportsXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, type, CaseReportsCCDAGenerator.DocumentTemplateType.ReferralNote, MDVUtility.ToInt64(model.referralProvider), model.raferralReason);
                var response = new
                {
                    status = true,
                    data = data,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else if (model.commandType.ToLower() == "html")
            {
                string data = CaseReportsCCDAGenerator.DecryptRawXml(model.XMLData, Convert.ToBoolean(model.DataIsEncrypted), model.Password);
                data = new string(data.Where(c => !char.IsControl(c)).ToArray());
                var output = CaseReportsCCDAGenerator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), data, true, "", model.Template);
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
            CaseReportsHelper helperPlan = new CaseReportsHelper();

            CaseReportsModel model = JsonConvert.DeserializeObject<CaseReportsModel>(MDVUtility.ToStr(AllData["data"]));

            var xmlData = CaseReportsCCDAGenerator.DecryptRawXml(model.XMLData, Convert.ToBoolean(model.DataIsEncrypted), model.Password);
            var data = helperPlan.HashingAndEncryption(xmlData, Convert.ToBoolean(model.IncludeHashCode), Convert.ToBoolean(model.Encryption), model.Password);
            string HashCode = string.Empty;
            if (Convert.ToBoolean(model.IncludeHashCode))
                HashCode = data["hashCode"];
            CaseReportsCCDAGenerator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), data["xmlData"], true, xmlData, model.Template);
            var folderPath = System.Web.HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
            var xmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.xml", model.NoteId, model.ProviderId, model.PatientId, MDVSession.Current.AppUserId);
            var htmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.html", model.NoteId, model.ProviderId, model.PatientId, MDVSession.Current.AppUserId);
            string[] xmlfilesPath = Directory.GetFiles(folderPath, xmlfileName, SearchOption.AllDirectories);
          //  string[] htmlfilesPath = Directory.GetFiles(folderPath, htmlfileName, SearchOption.AllDirectories);

            List<string> xmlFiles = xmlfilesPath.ToList();
           // List<string> htmlFiles = htmlfilesPath.ToList();

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("HashCode", HashCode);
            int counter = 0;
            foreach (var xmlFile in xmlFiles)
            {
                byte[] bcode = File.ReadAllBytes(xmlFile);
                result.Add(string.Format("XMLByte{0}", (counter == 0 ? string.Empty : Convert.ToString(counter))), Convert.ToBase64String(bcode));
            }
            counter = 0;
            //foreach (var htmlFile in htmlFiles)
            //{
            //    byte[] bcode = File.ReadAllBytes(htmlFile);
            //    result.Add(string.Format("HTMLByte{0}", (counter == 0 ? string.Empty : Convert.ToString(counter))), Convert.ToBase64String(bcode));
            //}
            //fixme why the BLL function is directly being called from controller ? {Ali Awan}
            DataTable dtDBAudit = new BLLClinical().insertClinicalSummaryCopyAudit(model.PatientId, model.SummaryType);

            return Newtonsoft.Json.JsonConvert.SerializeObject(result); ;
        }

        [HttpPost]
        public string SendEmail(JObject AllData)
        {
            CaseReportsHelper helperPlan = new CaseReportsHelper();

            CaseReportsModel model = JsonConvert.DeserializeObject<CaseReportsModel>(MDVUtility.ToStr(AllData["data"]));

            var xmlData = CaseReportsCCDAGenerator.DecryptRawXml(model.XMLData, Convert.ToBoolean(model.DataIsEncrypted), model.Password);
            var data = helperPlan.HashingAndEncryption(xmlData, Convert.ToBoolean(model.IncludeHashCode), Convert.ToBoolean(model.Encryption), model.Password);
            string HashCode = string.Empty;
            if (Convert.ToBoolean(model.IncludeHashCode))
                HashCode = data["hashCode"];
            CaseReportsCCDAGenerator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), data["xmlData"], true, xmlData);
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
                var message = helperPlan.SendEmailThroughPhiMailConnector(model.toEmail, XmlString, htmlString);

                var result = new
                {
                    status = true,
                    data = message,
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

    }

}