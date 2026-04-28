using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing.FollowUp;
using MDVision.IEHR.Model.Billing.FollowUp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing.FollowUp
{
    public class FollowUpsController : ApiController
    {
        [HttpPost]
        public string FollowUpPatientAR(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                FollowUpPatientARModel model = ser.Deserialize<FollowUpPatientARModel>(MDVUtility.ToStr(objData["data"]));
      

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = Bill_FollowUpPatientAR.Instance().LoadFollowUpARDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string FollowUpPatientARDetail(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                FollowUpPatientARModel model = ser.Deserialize<FollowUpPatientARModel>(MDVUtility.ToStr(objData["data"]));
        
                if (model.CommandType.ToLower() == "fill_followup_patient_ar")
                {
                    string response = null;
                    response = Bill_FollowUpPatientAR.Instance().FillFollowUpARDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "update_followup_patient_ar")
                {
                    string response = null;
                    response = Bill_FollowUpPatientAR.Instance().UpdateFollowUpARDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string FollowUpInsuranceARDetail(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                FollowUpInsuranceARModel model = ser.Deserialize<FollowUpInsuranceARModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "fill_followup_insurance_ar")
                {
                    string response = null;
                    response = Bill_FollowUpInsuranceAR.Instance().FillFollowUpARDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "update_followup_insurance_ar")
                {
                    string response = null;
                    response = Bill_FollowUpInsuranceAR.Instance().UpdateFollowUpARDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string FollowUpInsuranceAR(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                FollowUpInsuranceARModel model = ser.Deserialize<FollowUpInsuranceARModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = Bill_FollowUpInsuranceAR.Instance().LoadFollowUpARDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
