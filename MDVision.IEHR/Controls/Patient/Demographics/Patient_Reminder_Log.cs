using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.Patient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_Reminder_Log
    {

        private BLLPatient BLLPatientObj = null;
        public Patient_Reminder_Log()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Reminder_Log _obj = null;
        public static Patient_Reminder_Log Instance()
        {
            if (_obj == null)
                _obj = new Patient_Reminder_Log();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Search the school.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SearchReminderLog(long PatientId)
        {
            try
            {


                #region Database Insertion
                
                List<PatientReminderLog> obj = BLLPatientObj.getReminderLogData(PatientId);

                if (obj != null)
                {
                    var response = new
                    {
                        status = true,
                        ReminderLogData = obj,
                        ReminderLogCount=obj.Count
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "An Error occured.",
                    };
                    return (JsonConvert.SerializeObject(response));
                }
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

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Reminder Log Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "LOAD_REMINDER_LOG":
                    {
                        long PatientId = MDVUtility.ToLong( context.Request["PatientId"]);
                        string strJSONData = SearchReminderLog(PatientId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}