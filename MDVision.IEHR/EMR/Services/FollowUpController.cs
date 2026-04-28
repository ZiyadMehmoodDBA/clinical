using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Helpers.Clinical.FollowUp;
using MDVision.IEHR.EMR.Model.Clinical;
using MDVision.IEHR.EMR.Model.FollowUp;
using Newtonsoft.Json;
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

namespace MDVision.IEHR.EMR.Services
{
    public class FollowUpController : ApiController
    {
        [HttpPost]

        public string ClinicalFollowUp(JObject AllData)
        {
            string response = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            FollowUpAppointmentModel model = new FollowUpAppointmentModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<FollowUpAppointmentModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new FollowUpAppointmentModel();
                }

            }


            FollowUpAppointmentHelper helperFollowUp = new FollowUpAppointmentHelper();

            if (model.commandType.ToLower() == "save_followup_appointment")
            {
                //Int64 PatientID = MDVUtility.ToInt64(model.PatientId);
                //Int64 AppointmentID = MDVUtility.ToInt64(model.AppointmentID);
                response = helperFollowUp.SaveFollowUpAppointment(model);
            }
            if (model.commandType.ToLower() == "update_followup_appointment")
            {
                //Int64 PatientID = MDVUtility.ToInt64(model.PatientId);
                //Int64 AppointmentID = MDVUtility.ToInt64(model.AppointmentID);
                response = helperFollowUp.UpdateAppointment(model);
            }
            if (model.commandType.ToLower() == "update_patient_visit")
            {
                response = helperFollowUp.UpdateVisit(model);
            }
            if (model.commandType.ToLower() == "fill_followup_appointment")
            {
                //Int64 PatientID = MDVUtility.ToInt64(model.PatientId);
                //Int64 AppointmentID = MDVUtility.ToInt64(model.AppointmentID);
                response = helperFollowUp.FillAppointment(model);
            }
            if (model.commandType.ToLower() == "load_patient_appointment")
            {
                response = helperFollowUp.LoadPatientAppointment(model);
            }
            if (model.commandType.ToLower() == "load_provider_available_slots")
            {
                response = helperFollowUp.LoadAvailableSlots(model);
            }
            if (model.commandType.ToLower() == "get_appointment_slotinfo")
            {
                response = helperFollowUp.GetAppointmentSlotInfo(model);
            }
            return response;

        }
    }
}
