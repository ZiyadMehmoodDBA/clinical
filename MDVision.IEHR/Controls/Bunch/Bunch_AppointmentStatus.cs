using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.BusinessWrapper;

namespace MDVision.IEHR.Controls.Bunch
{
    public class Bunch_AppointmentStatus
    {
        #region Singleton
        private static Bunch_AppointmentStatus _obj = null;
        public static Bunch_AppointmentStatus Instance()
        {
            if (_obj == null)
                _obj = new Bunch_AppointmentStatus();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the appointment status.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="AppointmentID">The appointment identifier.</param>
        /// <returns>System.String.</returns>
        private string LoadAppointmentStatus(string fieldsJSON, Int64 AppointmentID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSScheduleSetup dsSchedule = null;
                BLObject<DSScheduleSetup> obj;
                if (SearchedfieldsJSON == null)
                    obj = AdminSchedule.BusinessObj.LoadAppointmentStatus(AppointmentID, null, null, null);
                else
                    obj = AdminSchedule.BusinessObj.LoadAppointmentStatus(AppointmentID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["chkIsActice"]);
                dsSchedule = obj.Data;
                var response = new
                {
                    status = true,
                    AppointmentStatusCount = dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count,
                    AppointmentStatusLoad_JSON = Utility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Service Command Handler

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_APPOINTMENT_STATUS":
                    {
                        string fieldsJSON = context.Request["AppointmentStatusData"];
                        Int64 AppointmentID = Utility.ToInt64(context.Request["AppointmentID"]);
                        string strJSONData = LoadAppointmentStatus(fieldsJSON, AppointmentID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}