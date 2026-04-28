using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Scheduler;
using MDVision.Model.Native.Scheduler;
using MDVision.Model.PMSSchedule;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Scheduler
{
    public class SchedulerController : ApiController
    {
        [HttpPost]
        public string PMSScheduler(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                AppointmentModel model = ser.Deserialize<AppointmentModel>(MDVUtility.ToStr(objData["data"]));
                string response = null;
                switch (model.CommandType.ToLower())
                {
                    case "search_appointment_schdule":
                        response = PMSSchedulerHelper.Instance().SearchAppointmentSchedule(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "get_provider_schdule":
                        response = PMSSchedulerHelper.Instance().GetProviderSchedule(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "fill_tooltip_data":
                        response = PMSSchedulerHelper.Instance().FillToolTipData(model.AppointmentId);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "load_appointment_status":
                        response = PMSSchedulerHelper.Instance().LoadAppointmentStatusOptions(MDVUtility.ToLong(model.AppointmentStatusId), model.AppointmentStatus);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "search_waitlist_schedule":
                        response = PMSSchedulerHelper.Instance().LoadWaitListSchedule(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "copy_paste_appointment":
                        response = PMSSchedulerHelper.Instance().CopyPasteAppointment(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "cut_paste_appointment":
                        response = PMSSchedulerHelper.Instance().CutPasteAppointment(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "load_sch_blockhours":
                        response = PMSSchedulerHelper.Instance().LoadSchBlockHours(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                    case "load_eligibilityid":
                        response = PMSSchedulerHelper.Instance().EDIEligibilityIdSelect(model);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                        break;
                   
                    default:
                        break;
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string PMSSchedulerNative(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            string response = "";
            JavaScriptSerializer ser = new JavaScriptSerializer();
            EmptySlotModel model = ser.Deserialize<EmptySlotModel>(MDVUtility.ToStr(objData["data"]));
            response =   PMSSchedulerHelper.Instance().SaveAppointmentNative(model);
            ResponseList.Add(MDVisionConstants.ResponseModel, response);
            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}