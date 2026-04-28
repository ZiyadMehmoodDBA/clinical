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
    public class Admin_Holidays
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_Holidays()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_Holidays _obj = null;
        public static Admin_Holidays Instance()
        {
            if (_obj == null)
                _obj = new Admin_Holidays();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads the holidays.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="HolidaysID">The holidays identifier.</param>
        /// <returns>System.String.</returns>
        private string LoadHolidays(string fieldsJSON, Int64 HolidaysID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Holidays", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLScheduleObj.LoadHolidays(HolidaysID, null, null, null, null);
                    else
                        obj = BLLScheduleObj.LoadHolidays(HolidaysID, SearchedfieldsJSON["holidayDate"], SearchedfieldsJSON["txtHoliday"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            HolidaysCount = dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsSchedule.ScheduleHolidays.Rows.Count > 0) ? dsSchedule.ScheduleHolidays.Rows[0][dsSchedule.ScheduleHolidays.RecordCountColumn.ColumnName] : 0,
                            HolidaysLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            HolidaysCount = 0,
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
                case "SEARCH_HOLIDAYS":
                    {
                        string fieldsJSON = context.Request["HolidaysData"];
                        Int64 HolidaysID = MDVUtility.ToInt64(context.Request["HolidaysID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadHolidays(fieldsJSON, HolidaysID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}