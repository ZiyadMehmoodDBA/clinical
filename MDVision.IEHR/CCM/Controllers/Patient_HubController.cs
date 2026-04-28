using MDVision.Common.Utilities;
using MDVision.IEHR.CCM.Helpers;
using MDVision.IEHR.CCM.Helpers.CCMHub;
using MDVision.IEHR.CCM.Helpers.PatientHub;
using MDVision.IEHR.Common;
using MDVision.Model.CCM;
using MDVision.Model.CCM.CCMHub;
using MDVision.Model.CCM.PatientHub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.CCM.Services
{
    public class Patient_HubController:ApiController
    {
        #region Action plan

        /// <summary>
        /// SaveCCMEnrolledGoals -- This Not Being Used Yet
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveCCMEnrolledGoals(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                MDVision.Model.CCM.CCMHub.EnrolledGoals model = ser.Deserialize<MDVision.Model.CCM.CCMHub.EnrolledGoals>(MDVUtility.ToStr(objData["data"]));


                var response = new MDVision.IEHR.CCM.Helpers.CCMHub.PatientHubHelper().SaveCCMEnrolledGoals(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        /// <summary>
        /// SaveCCMEnrolledGoalsCPT -- This Not Being Used Yet
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveCCMEnrolledGoalsCPT(JObject objData)
        {
            var jsonString = ((JValue)((JProperty)objData.First).Value).Value;

            System.Web.Script.Serialization.JavaScriptSerializer ser11 = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser11.Deserialize<dynamic>(jsonString.ToString());


            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                MDVision.Model.CCM.CCMHub.EnrolledGoalsCPT model = ser.Deserialize<MDVision.Model.CCM.CCMHub.EnrolledGoalsCPT>(MDVUtility.ToStr(objData["data"]));

                var response = new MDVision.IEHR.CCM.Helpers.CCMHub.PatientHubHelper().SaveCCMEnrolledGoalsCPT(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        /// <summary>
        /// SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT -- This Being Used
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT(JObject objData)
        {

            var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
            // var result = JsonConvert.DeserializeObject<List<Dictionary<string, Dictionary<string, string>>>>(jsonString.ToString());

            JavaScriptSerializer ser_ = new JavaScriptSerializer();
            var SearchedfieldsJSON = ser_.Deserialize<dynamic>(jsonString.ToString());
            var SearchedfieldsJSONCount = ((object[])SearchedfieldsJSON).Count();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            for (int i = 0; i < SearchedfieldsJSONCount; i++)
            {
                var CPTCode = SearchedfieldsJSON[i]["cptcode"];
                var CPTDescription = SearchedfieldsJSON[i]["cptdescription"];
                var SNOMEDCode = SearchedfieldsJSON[i]["SNOMEDID"];
                var SNOMEDDescription = SearchedfieldsJSON[i]["SNOMEDDescription"];
                var Instruction = SearchedfieldsJSON[i]["Instruction"];
                var EnrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON[i]["EnrollmentInfoId"]);

                if (i == 0)
                    ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

                if (RequestModel.IsLogIn)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();

                    //var des = (EnrolledGoals_EnrolledGoalsCPT)Newtonsoft.Json.JsonConvert.DeserializeObject(objData["data"], typeof(EnrolledGoals_EnrolledGoalsCPT));

                    List<EnrolledGoals_EnrolledGoalsCPT> model = JsonConvert.DeserializeObject<List<EnrolledGoals_EnrolledGoalsCPT>>(objData["data"].ToString());

                    //EnrolledGoals_EnrolledGoalsCPT model = ser.Deserialize<EnrolledGoals_EnrolledGoalsCPT>(MDVUtility.ToStr(objData["data"][i]));
                    model[i].CPTCodeId = -1;
                    var response = new PatientHubHelper().SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT(model[i], CPTCode, CPTDescription, SNOMEDCode, SNOMEDDescription);
                    if (i == 0)
                        ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string InsertUpdateRiskAssessmentScoreTemplate(JObject objData)
        {
            var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
            // var result = JsonConvert.DeserializeObject<List<Dictionary<string, Dictionary<string, string>>>>(jsonString.ToString());

            JavaScriptSerializer ser_ = new JavaScriptSerializer();
            var SearchedfieldsJSON = ser_.Deserialize<dynamic>(jsonString.ToString());
            var SearchedfieldsJSONCount = ((object[])SearchedfieldsJSON).Count();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            for (int i = 0; i < SearchedfieldsJSONCount; i++)
            {
                var RiskAssessmentId = SearchedfieldsJSON[i]["RiskAssessmentId"];
                var RiskAssessTemptId = SearchedfieldsJSON[i]["RiskAssessTemptId"];
                var EnrollmentInfoId = SearchedfieldsJSON[i]["EnrollmentInfoId"];
                var AssessHTML = SearchedfieldsJSON[i]["AssessHTML"];
                var RiskScore = SearchedfieldsJSON[i]["RiskScore"];

                if (i == 0)
                    ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

                if (RequestModel.IsLogIn)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();

                    //var des = (EnrolledGoals_EnrolledGoalsCPT)Newtonsoft.Json.JsonConvert.DeserializeObject(objData["data"], typeof(EnrolledGoals_EnrolledGoalsCPT));

                    List<RiskAssessment> model = JsonConvert.DeserializeObject<List<RiskAssessment>>(objData["data"].ToString());

                    //EnrolledGoals_EnrolledGoalsCPT model = ser.Deserialize<EnrolledGoals_EnrolledGoalsCPT>(MDVUtility.ToStr(objData["data"][i]));
                    model[i].RiskAssessTemptId = MDVUtility.ToStr(RiskAssessTemptId);
                    model[i].EnrollmentInfoId = MDVUtility.ToStr(EnrollmentInfoId);
                    //model[i].AssessHTML = AssessHTML;
                    model[i].RiskScore = MDVUtility.Tofloat(RiskScore);

                    var response = new PatientHubHelper().InsertUpdateCCMPatientHubRiskAssessmentScore(model[i]);
                    if (i == 0)
                        ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }

            }
            //PreRequestModel RequestModel = new PreRequestModel();
            //RequestModel = new PreRequests().ApplicationServerContent();

            //Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            //ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            //if (RequestModel.IsLogIn)
            //{

            //    JavaScriptSerializer ser = new JavaScriptSerializer();
            //    MDVision.Model.CCM.CCMHub.RiskAssessment model = ser.Deserialize<MDVision.Model.CCM.CCMHub.RiskAssessment>(MDVUtility.ToStr(objData["data"]));

            //    var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().InsertUpdateCCMPatientHubRiskAssessmentScore(model);
            //    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            //}
            //return JsonConvert.SerializeObject(ResponseList);
            return JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string InsertCCMPatientHUBRiskAssessmentTemplate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {

                JavaScriptSerializer ser = new JavaScriptSerializer();
                MDVision.Model.CCM.CCMHub.RiskAssessment model = ser.Deserialize<MDVision.Model.CCM.CCMHub.RiskAssessment>(MDVUtility.ToStr(objData["data"]));

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().InsertCCMPatientHUBRiskAssessmentTemplate(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }


        #endregion

        #region Patient Hub Static

        [HttpPost]
        public string loadPatientHubStatic(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var patientId = MDVUtility.ToLong(SearchedfieldsJSON["PatientId"]);
                var enrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON["EnrollmentInfoId"]);
                
                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubStatic(patientId, enrollmentInfoId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string loadPatientHubProblems(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var patientId = MDVUtility.ToLong(SearchedfieldsJSON["PatientId"]);

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubProblems(patientId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string DeleteChronicProblems(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var ChronicProblemId = MDVUtility.ToLong(SearchedfieldsJSON["ChronicProblemId"]);
                var PatientId = MDVUtility.ToLong(SearchedfieldsJSON["PatientId"]);

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().DeleteChronic(ChronicProblemId, PatientId);

                //var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubProblems(ChronicProblemId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        //[HttpPost]
        //public string LoadRiskAssessmentScoreTemplate(JObject objData)
        //{
        //    PreRequestModel RequestModel = new PreRequestModel();
        //    RequestModel = new PreRequests().ApplicationServerContent();

        //    Dictionary<string, string> ResponseList = new Dictionary<string, string>();
        //    ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

        //    if (RequestModel.IsLogIn)
        //    {
        //        var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
        //        JavaScriptSerializer ser = new JavaScriptSerializer();
        //        var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

        //        var EnrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON["EnrollmentInfoId"]);

        //        var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubRiskAssessmentScore(EnrollmentInfoId);
        //        ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
        //    }
        //    return JsonConvert.SerializeObject(ResponseList);
        //}

        [HttpPost]
        public string LoadRiskAssessmentScoreTemplate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var EnrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON["EnrollmentInfoId"]);

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubRiskAssessmentScore(EnrollmentInfoId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string DeleteRiskAssessmentScoreTemplate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var RiskAssessmentId = SearchedfieldsJSON["RiskAssessmentId"];

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().DeleteRiskAssessmentScoreTemplate(RiskAssessmentId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string DeleteCareTeamProviderTemplate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var EnrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON["EnrollmentInfoId"]);
                var ProviderId = MDVUtility.ToLong(SearchedfieldsJSON["ProviderId"]);

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().DeleteCareTeamProviderTemplate(ProviderId, EnrollmentInfoId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string InsertCCMPatientHUBEnrolledCareTeam(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();
            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                MDVision.Model.CCM.CCMHub.EnrolledCareTeam model = ser.Deserialize<MDVision.Model.CCM.CCMHub.EnrolledCareTeam>(MDVUtility.ToStr(objData["data"]));

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().InsertCCMPatientHUBEnrolledCareTeam(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }
        //public string DeleteRiskAssessmentScoreTemplate(JObject objData)
        //{
        //    PreRequestModel RequestModel = new PreRequestModel();
        //    RequestModel = new PreRequests().ApplicationServerContent();

        //    Dictionary<string, string> ResponseList = new Dictionary<string, string>();
        //    ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

        //    if (RequestModel.IsLogIn)
        //    {
        //        JavaScriptSerializer ser = new JavaScriptSerializer();
        //        MDVision.Model.CCM.CCMHub.RiskAssessment model = ser.Deserialize<MDVision.Model.CCM.CCMHub.RiskAssessment>(MDVUtility.ToStr(objData["data"]));

        //        var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().DeleteRiskAssessmentScore(MDVUtility.ToLong(model.EnrollmentInfoId), MDVUtility.ToLong(model.RiskAssessTemptId));
        //        ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
        //    }
        //    return JsonConvert.SerializeObject(ResponseList);
        //}

        [HttpPost]
        public string loadPatientHubCareTeam(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var patientId = MDVUtility.ToLong(SearchedfieldsJSON["PatientId"]);
                var enrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON["EnrollmentInfoId"]);
                var providerId = MDVUtility.ToLong(SearchedfieldsJSON["ProviderId"]);


                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubCareTeam(providerId, enrollmentInfoId, 0);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string LoadCCMPatientHUBGoals(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());
                var EnrollmentInfoId = MDVUtility.ToLong(SearchedfieldsJSON["EnrollmentInfoId"]);

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHUBGoals(EnrollmentInfoId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string DeletePatientHubEnrolledGoals(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

                var EnrolledGoalsId = MDVUtility.ToLong(SearchedfieldsJSON["EnrolledGoalsId"]);
                var EnrolledGoalsICDId = MDVUtility.ToLong(SearchedfieldsJSON["EnrolledGoalsICDId"]);

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().DeletePatientHubEnrolledGoals(EnrolledGoalsId, EnrolledGoalsICDId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        //[HttpPost]
        //public string DeleteRiskAssessmentScoreTemplate(JObject objData)
        //{
        //    PreRequestModel RequestModel = new PreRequestModel();
        //    RequestModel = new PreRequests().ApplicationServerContent();

        //    Dictionary<string, string> ResponseList = new Dictionary<string, string>();
        //    ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

        //    if (RequestModel.IsLogIn)
        //    {
        //        var jsonString = ((JValue)((JProperty)objData.First).Value).Value;
        //        JavaScriptSerializer ser = new JavaScriptSerializer();
        //        var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonString.ToString());

        //        var TemplateId = SearchedfieldsJSON["TemplateId"];
        //        var EnrollmentInfoId = SearchedfieldsJSON["EnrollmentInfoId"];

        //        var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().LoadCCMPatientHubRiskAssessmentScore(EnrollmentInfoId);
        //        ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
        //    }
        //    return JsonConvert.SerializeObject(ResponseList);
        //}

        #endregion

        #region CCM Termination


        [HttpPost]
        public string SaveCCMTermination(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                MDVision.Model.CCM.CCMHub.EnrolledGoals model = ser.Deserialize<MDVision.Model.CCM.CCMHub.EnrolledGoals>(MDVUtility.ToStr(objData["data"]));

                var response = new IEHR.CCM.Helpers.CCMHub.PatientHubHelper().SaveCCMEnrolledGoals(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        #endregion

        #region CarePlan


        [HttpPost]
        public string CarePlan(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            
            dynamic data = JsonConvert.DeserializeObject<dynamic>(MDVUtility.ToStr(AllData["data"]));

            string commandType = data.commandType.Value.ToLower();

            if (commandType.ToLower() == "search_careplan_list")
            {
                CarePlanSearchModel model = ser.Deserialize<CarePlanSearchModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = CarePlanHelper.Instance().loadCarePlanList(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
           else if (commandType.ToLower() == "delete_ccm_care_plan")
            {
                CarePlanSearchModel model = ser.Deserialize<CarePlanSearchModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = CarePlanHelper.Instance().deleteCarePlanList(model.CarePlanId);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "fill_ccm_care_plan")
            {
                CarePlanModel model = ser.Deserialize<CarePlanModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.CarePlanId > 0)
                    {
                        response = CarePlanHelper.Instance().fillCarePlanList(model.CarePlanId);
                    }
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "save_ccm_care_plan")
            {
                CarePlanModel model = ser.Deserialize<CarePlanModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.CarePlanId<=0)
                    {
                        response = CarePlanHelper.Instance().saveCarePlanList(model);
                    }                    
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "update_ccm_care_plan")
            {
                CarePlanModel model = ser.Deserialize<CarePlanModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.CarePlanId > 0)
                    {
                        response = CarePlanHelper.Instance().updateCarePlanList(model);
                    }
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "update_care_plan_active_inactive")
            {
                CarePlanSearchModel model = ser.Deserialize<CarePlanSearchModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = CarePlanHelper.Instance().updateStatusCarePlanList(model.CarePlanId,model.IsActive);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        #endregion

        #region
        [HttpPost]
        public string HRAssessment(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();

            dynamic data = JsonConvert.DeserializeObject<dynamic>(MDVUtility.ToStr(AllData["data"]));

            string commandType = data.commandType.Value.ToLower();

            if (commandType.ToLower() == "search_hrassessment_list")
            {
                HRAssessmentSearchModel model = ser.Deserialize<HRAssessmentSearchModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = HRAssessmentHelper.Instance().loadHRAssessmentList(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "delete_ccm_hr_assessment")
            {
                HRAssessmentSearchModel model = ser.Deserialize<HRAssessmentSearchModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = HRAssessmentHelper.Instance().deleteHRAssessmentList(model.HRAssessmentId);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "fill_ccm_hr_assessment")
            {
                HRAssessmentModel model = ser.Deserialize<HRAssessmentModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.HRAssessmentId > 0)
                    {
                        response = HRAssessmentHelper.Instance().fillHRAssessmentList(model.HRAssessmentId);
                    }
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "save_ccm_hr_assessment")
            {
                HRAssessmentModel model = ser.Deserialize<HRAssessmentModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.HRAssessmentId <= 0)
                    {
                        response = HRAssessmentHelper.Instance().saveHRAssessmentList(model);
                    }
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "update_ccm_hr_assessment")
            {
                HRAssessmentModel model = ser.Deserialize<HRAssessmentModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.HRAssessmentId > 0)
                    {
                        response = HRAssessmentHelper.Instance().updateHRAssessmentList(model);
                    }
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (commandType.ToLower() == "update_hr_assessment_active_inactive")
            {
                HRAssessmentSearchModel model = ser.Deserialize<HRAssessmentSearchModel>(MDVUtility.ToStr(AllData["data"]));
                string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = HRAssessmentHelper.Instance().updateStatusHRAssessmentList(model.HRAssessmentId, model.IsActive);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }

            }

            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        #endregion
    }
}