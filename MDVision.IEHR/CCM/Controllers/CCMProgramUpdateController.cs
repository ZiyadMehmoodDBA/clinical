using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.CCM.Herlpers;
using MDVision.Model.CCM;
using MDVision.Model.CCM.CCMHub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.CCM.Controllers
{

    public class CCMProgramUpdateController : ApiController
    {


        [HttpPost]
        public string SaveCCMTaskTime(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                TaskAmalgamatedModel model = ser.Deserialize<TaskAmalgamatedModel>(MDVUtility.ToStr(objData["data"]));

                var response = new CCMProgramUpdateHelper().SaveCCMTaskTime(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string SaveCCMTaskTimeFromDashBoard(JObject objData)
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

                var DurationUnit = MDVUtility.ToStr(SearchedfieldsJSON["DurationUnit"]);
                var Duration = MDVUtility.Tofloat(SearchedfieldsJSON["Duration"]);

                if (DurationUnit == "seconds")
                    Duration = Duration / 60;
                else if (DurationUnit == "hours")
                    Duration = Duration * 60;
                

                CCMTaskTimerModel model = ser.Deserialize<CCMTaskTimerModel>(MDVUtility.ToStr(objData["data"]));
                model.TaskDuration = MDVUtility.ToStr(Duration);

                var response = new CCMProgramUpdateHelper().SaveCCMTaskTimeFromDashBoard(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string LoadCCMTaskTimer(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMTaskTimerModel model = ser.Deserialize<CCMTaskTimerModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().LoadCCMTaskTimer(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }


        [HttpPost]
        public string SaveCCMCallDetails(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                TaskAmalgamatedModel model = ser.Deserialize<TaskAmalgamatedModel>(MDVUtility.ToStr(objData["data"]));

                var response = new CCMProgramUpdateHelper().SaveCCMCallDetails(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }


        [HttpPost]
        public string UpdateCCMCallDetails(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                TaskAmalgamatedModel model = ser.Deserialize<TaskAmalgamatedModel>(MDVUtility.ToStr(objData["data"]));

                var response = new CCMProgramUpdateHelper().UpdateCCMTaskTimeDetails(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string DeleteCCMCallDetails(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMCallDetailsModel model = ser.Deserialize<CCMCallDetailsModel>(MDVUtility.ToStr(objData["data"]));

                var response = new CCMProgramUpdateHelper().DeleteCCMCallDetails(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string LoadCCMCallDetails(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMCallDetailsModel model = ser.Deserialize<CCMCallDetailsModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().LoadCCMCallDetails(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string LoadCCMTaskTimerDetails(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                TaskAmalgamatedModel model = ser.Deserialize<TaskAmalgamatedModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().LoadCCMTaskTimerDetails(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string SaveProgramUpdate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMProgramUpdateModel model = ser.Deserialize<CCMProgramUpdateModel>(MDVUtility.ToStr(objData["data"]));

                var response = new CCMProgramUpdateHelper().SaveProgramUpdate(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());

            }
            return JsonConvert.SerializeObject(ResponseList);
        }


        [HttpPost]
        public string LoadProgressUpdate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMProgramUpdateModel model = ser.Deserialize<CCMProgramUpdateModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().LoadProgressUpdate(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }


        [HttpPost]
        public string FillCCMProgramUpdate(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMProgramUpdateModel model = ser.Deserialize<CCMProgramUpdateModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().LoadProgressUpdateDetail(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }


        [HttpPost]
        public string DeleteCCMTaskTimerHistory(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMTaskTimerModel model = ser.Deserialize<CCMTaskTimerModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().DeleteCCMTaskTime(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string UpdateCCMTaskTimerHistory(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                CCMTaskTimerModel model = ser.Deserialize<CCMTaskTimerModel>(MDVUtility.ToStr(objData["data"]));
                var response = new CCMProgramUpdateHelper().UpdateCCMTaskTime(model);
                ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
            }
            return JsonConvert.SerializeObject(ResponseList);
        }


    }
}