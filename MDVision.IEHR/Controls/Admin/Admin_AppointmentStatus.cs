using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_AppointmentStatus
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_AppointmentStatus()
        {
            BLLScheduleObj = new BLLSchedule();
        }

        #region Singleton
        private static Admin_AppointmentStatus _obj = null;
        public static Admin_AppointmentStatus Instance()
        {
            if (_obj == null)
                _obj = new Admin_AppointmentStatus();
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
        private string LoadAppointmentStatus(string fieldsJSON, Int64 AppointmentID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointment Status", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLScheduleObj.LoadAppointmentStatus(AppointmentID, null, null, null);
                    else
                        obj = BLLScheduleObj.LoadAppointmentStatus(AppointmentID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["chkIsActice"], PageNumber, RowsPerPage);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentStatusCount = dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsSchedule.AppointmentStatus.Rows.Count > 0) ? dsSchedule.AppointmentStatus.Rows[0][dsSchedule.AppointmentStatus.RecordCountColumn.ColumnName] : 0,
                            AppointmentStatusLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentStatusCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
                }
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
                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadAppointmentStatus(fieldsJSON, AppointmentID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}