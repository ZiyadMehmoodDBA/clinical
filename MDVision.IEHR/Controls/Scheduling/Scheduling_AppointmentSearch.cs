using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Globalization;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_AppointmentSearch
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_AppointmentSearch()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_AppointmentSearch _obj = null;
        public static Scheduling_AppointmentSearch Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_AppointmentSearch();
            return _obj;
        }
        #endregion

        #region "Private Functions"

        private string SearchAppointment(string fieldsJSON, int PageNumber, int RowspPage, string AppDate,string AppointmentStatusIds)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                var status = "";
                var action = "";
                // Start Checks For Action dropdown Irfan 25/11/2015 for Bug # EMR-24

                if (SearchedfieldsJSON.ContainsKey("ddlAction_text"))
                {
                    if ((SearchedfieldsJSON["ddlAction_text"]) == "All")
                    {
                        action = "";
                    }
                    else
                    {
                        action = (SearchedfieldsJSON["ddlAction_text"]);
                    }

                }

                // End Checks For Action dropdown Irfan 25/11/2015 for Bug # EMR-24

                if (SearchedfieldsJSON.ContainsKey("ddlStatus_text"))
                {
                    if ((SearchedfieldsJSON["ddlStatus_text"]) == "- Select -")
                    {
                        status = "";
                    }
                    else
                    {
                        status = (SearchedfieldsJSON["ddlStatus_text"]);
                    }
                }



                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLScheduleObj.LoadAppointmentsVisits(0, 0, 0, AppDate, "", "", "", null, "0", PageNumber, RowspPage, "");
                else
                    obj = BLLScheduleObj.LoadAppointmentsVisits(MDVUtility.ToInt64(SearchedfieldsJSON["ddlprovider"]), MDVUtility.ToInt64(SearchedfieldsJSON["ddlfacility"]), 0, AppDate, SearchedfieldsJSON["txtSearchLastName"], SearchedfieldsJSON["txtSearchFirstName"], SearchedfieldsJSON["txtAccount"], status, "0", PageNumber, RowspPage, action,0,"0", AppointmentStatusIds);

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.AppointmentsVisits.RecordCountColumn.ColumnName],
                            SchAppStatus_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_APPOINTMENTS":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string AppDate = MDVUtility.ToStr(context.Request["AppDate"]);
                        string AppointmentStatusIds = MDVUtility.ToStr(context.Request["AppointmentStatusIds"]);
                        string strJSONData = SearchAppointment(fieldsJSON, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage), AppDate, AppointmentStatusIds);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
    }
}