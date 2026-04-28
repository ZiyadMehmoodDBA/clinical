using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.CCM.Herlpers;
using MDVision.Model.CCM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.CCM.Controllers
{

    public class CCMEnrollmentInfoController : ApiController
    {
        [HttpPost]
        public string SaveCCMEnrollmentInfo(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                try
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    CCMEnrollmentInfoModel model = ser.Deserialize<CCMEnrollmentInfoModel>(MDVUtility.ToStr(objData["data"]));

                    var response = new CCMEnrollmentInfoHelper().SaveCCMEnrollmentInfo(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }
                catch (System.Exception ex)
                {
                    var response = new
                    {
                        status = false,
                        Message = "Maximum " + MDVision.Common.Shared.MDVSession.Current.FileSize.ToString() + "MB is allowed",
                    };
                    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }
            }
            return JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string FillCCMEnrollmentInfo(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMEnrollmentInfoModel model = ser.Deserialize<CCMEnrollmentInfoModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMEnrollmentInfoHelper().LoadCCMEnrollmentInfoDetail(MDVUtility.ToInt32(model.EnrollmentInfoId));
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string UpdateCCMEnrollmentInfo(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();

                try
                {
                    CCMEnrollmentInfoModel model = ser.Deserialize<CCMEnrollmentInfoModel>(MDVUtility.ToStr(objData["data"]));
                    var response = new CCMEnrollmentInfoHelper().UpdateCCMEnrollmentInfo(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }
                catch (System.Exception ex)
                {
                    var response = new
                    {
                        status = false,
                        Message = "Maximum " + MDVision.Common.Shared.MDVSession.Current.FileSize.ToString() + "MB is allowed",
                    };
                    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string SaveCCMEnrollmentDecline(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMEnrollmentInfoModel model = ser.Deserialize<CCMEnrollmentInfoModel>(MDVUtility.ToStr(objData["data"]));

                var response = new CCMEnrollmentInfoHelper().SaveCCMEnrollmentDecline(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }



        
             [HttpPost]
        public string ResumeCCMEnrollmentInfo(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMEnrollmentInfoModel model = ser.Deserialize<CCMEnrollmentInfoModel>(MDVUtility.ToStr(objData["data"]));


                var response = new CCMEnrollmentInfoHelper().ResumeCCMEnrollmentInfo(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);

        }

        public string TerminationCCMEnrollmentInfo(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                MDVision.Model.CCM.CCMHub.CCMTermination model = ser.Deserialize<MDVision.Model.CCM.CCMHub.CCMTermination>(MDVUtility.ToStr(objData["data"]));


                var response = new CCMEnrollmentInfoHelper().TerminationCCMEnrollmentInfo(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);

        }

    }
}