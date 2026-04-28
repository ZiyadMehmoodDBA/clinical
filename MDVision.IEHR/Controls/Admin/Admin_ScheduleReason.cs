using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ScheduleReason
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_ScheduleReason()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_ScheduleReason _obj = null;
        public static Admin_ScheduleReason Instance()
        {
            if (_obj == null)
                _obj = new Admin_ScheduleReason();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the schedule reason.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ScheduleReasonID">The schedule reason identifier.</param>
        /// <returns>System.String.</returns>
        private string LoadScheduleReason(string fieldsJSON, Int64 ScheduleReasonID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Schedule Reason", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLScheduleObj.LoadReasons(ScheduleReasonID, null, null, null, null);
                    else
                        obj = BLLScheduleObj.LoadReasons(ScheduleReasonID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["chkIsActice"], SearchedfieldsJSON["ddlEntity"], PageNumber, RowsPerPage);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleReasonCount = dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsSchedule.ScheduleReasons.Rows.Count > 0) ? dsSchedule.ScheduleReasons.Rows[0][dsSchedule.ScheduleReasons.RecordCountColumn.ColumnName] : 0,
                            ScheduleReasonLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleReasonCount = 0,
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
        /// Handle the ScheduleReason Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_SCHEDULE_REASON":
                    {
                        string fieldsJSON = context.Request["ScheduleReasonData"];
                        Int64 ScheduleReasonID = MDVUtility.ToInt64(context.Request["ScheduleReasonID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadScheduleReason(fieldsJSON, ScheduleReasonID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}