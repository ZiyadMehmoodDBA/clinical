using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing.Claims;
using MDVision.IEHR.Model.Billing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing
{
    public class BillingController : ApiController
    {
        private Bill_ClaimSubmission claimSubmissionHelperObj = null;

        public BillingController() {
            claimSubmissionHelperObj = new Bill_ClaimSubmission();
        }

        [HttpPost]
        public string SearchPatientClaim(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = claimSubmissionHelperObj.SearchPatientClaim(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string SearchSubmittedBatch(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = claimSubmissionHelperObj.SearchSubmittedBatch(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string SearchClaimSubmitionHistory(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = claimSubmissionHelperObj.SearchClaimSubmitHistory(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string ViewEDIFile(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.ViewEDIFile(MDVUtility.ToInt64(model._837BatchId));
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string CreateEDIFile(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.CreateEDIFile(model.Visits, model.ClearingHouse);
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string ScrubClaims(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.ScrubClaims(model.Visits);
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string PrintPatientClaim(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.PrintClaim(model.Visits, MDVUtility.ToInt64(model.ClearingHouse), Convert.ToBoolean(model.isSubmit), Convert.ToBoolean(model.MarkSubmitted), model.UserBrowser, Convert.ToBoolean(model.ViewOnly));
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        } 

        [HttpPost]
        public string PrintClaimHistory(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.PrintClaimHistory(model.VisitId, model.PatientId);
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }


        [HttpPost]
        public string UpdatePatientClaim(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.UpdatePrintClaim(model.Visits, MDVUtility.ToInt64(model.ClearingHouse), Convert.ToBoolean(model.isSubmit), Convert.ToBoolean(model.MarkSubmitted), model.UserBrowser);
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string TransmitPatientClaim(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.TransmitClaim(model.Visits, model.ClearingHouse, Convert.ToBoolean(model.MarkSubmitted));
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string DeleteClaimSubmissionErrors(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ClaimSubmissionModel model = ser.Deserialize<ClaimSubmissionModel>(MDVUtility.ToStr(objData["data"]));

                string response = null;
                response = claimSubmissionHelperObj.DeleteClaimSubmissionErrors(model.Visits);
                ResponseList.Add(MDVisionConstants.ResponseModel, response);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
