using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_SlotBlockUnblock
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_SlotBlockUnblock()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_SlotBlockUnblock _obj = null;
        public static Scheduling_SlotBlockUnblock Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_SlotBlockUnblock();
            return _obj;
        }
        #endregion

        #region "Update Slot Status"

        private string UpdateSlotStatus(string SlotId, long BlockReasonId, string BlockStatus, string Comments)
        {
            try
            {

                {
                    SlotId.TrimEnd(',');
                    BLObject<string> obj = BLLScheduleObj.UpdateSlotStatus(SlotId, BlockReasonId, BlockStatus, Comments);
                    if (obj.Data == "Updated successfully")
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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

        private string FillFromTimeDDL(Int64 facilityID, Int64 providerID, Int64 resourceID, string scheduleDate)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(facilityID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSAppointment dsApp = null;
                    BLObject<DSAppointment> obj = BLLScheduleObj.LookupSchedulingFromTime(providerID, facilityID, resourceID, scheduleDate);
                    if (obj.Data != null)
                    {
                        dsApp = obj.Data;

                        if (dsApp.Tables[dsApp.SchedulingFromTimeLookUp.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                DDLLoad_JSON = MDVUtility.JSON_DataTable(dsApp.Tables[dsApp.SchedulingFromTimeLookUp.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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

        private string GetScheduleReasons(string SchReason)
        {
            try
            {
                DSScheduleSetup dsApp = null;

                string entityId = "";

                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == "MDVISION")
                {
                    entityId = "";
                }
                else
                {
                    entityId = MDVSession.Current.EntityId;
                }

                BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadReasons(0, SchReason, "", "1", entityId, 1, 1000);
                if (obj.Data != null)
                {
                    dsApp = obj.Data;

                    if (dsApp.Tables[dsApp.ScheduleReasons.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SchReasonCount = dsApp.Tables[dsApp.ScheduleReasons.TableName].Rows.Count,
                            ScheduleReasonsLoad_JSON = MDVUtility.JSON_DataTable(dsApp.Tables[dsApp.ScheduleReasons.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
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

        #region "Reschedule Appointment Search"

        private string ScheduleSearch(string fieldsJSON, int PageNumber, int RowsPerPage)
        {

            string chkSunday = "0";
            string chkMonday = "0";
            string chkTuesday = "0";
            string chkWednesday = "0";
            string chkThursday = "0";
            string chkFriday = "0";
            string chkSaturday = "0";

            string chkDaysString = "";

            string chkBlocked = "";
            string chkBooked = "";
            string chkOverBook = "";
            string chkFull = "";
            string chkOpen = "";
            string proresbit = "";

            string chkStatusString = "";

            Int64 providerID = 0;
            Int64 resourceID = 0;


            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLScheduleObj.SchedulingSearch(0, 0, 0, 0, 0, "", "", "", "", "", "", "", "", PageNumber, RowsPerPage);

                else
                {


                    if (SearchedfieldsJSON["hfResourceId"] == "null")
                        proresbit = "0";
                    if (SearchedfieldsJSON["hfProviderId"] == "null")
                        proresbit = "1";


                    if (SearchedfieldsJSON["hfResourceId"] == "null")
                        resourceID = 0;
                    else
                        resourceID = MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceId"]);

                    if (SearchedfieldsJSON["hfProviderId"] == "null")
                        providerID = 0;
                    else
                        providerID = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);

                    chkDaysString = (chkSunday + "," + chkMonday + "," + chkTuesday + "," + chkWednesday + "," + chkThursday + "," + chkFriday + "," + chkSaturday);



                    if (SearchedfieldsJSON["chkBlocked"] == true)
                        chkBlocked = "1";
                    else
                        chkBlocked = "0";

                    if (SearchedfieldsJSON["chkBooked"] == true)
                        chkBooked = "2";
                    else
                        chkBooked = "0";

                    if (SearchedfieldsJSON["chkOverBook"] == true)
                        chkOverBook = "3";
                    else
                        chkOverBook = "0";

                    if (SearchedfieldsJSON["chkFull"] == true)
                        chkFull = "4";
                    else
                        chkFull = "0";

                    if (SearchedfieldsJSON["chkOpen"] == true)
                        chkOpen = "5";
                    else
                        chkOpen = "0";

                    chkStatusString = (chkBlocked + "," + chkBooked + "," + chkOverBook + "," + chkFull + "," + chkOpen);


                    obj = BLLScheduleObj.SchedulingSearch(MDVUtility.ToInt64(0), MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]), 0, providerID, resourceID, SearchedfieldsJSON["dpfromDate"], SearchedfieldsJSON["dptoDate"], "", "", "ALL", chkStatusString, chkDaysString, proresbit, PageNumber, RowsPerPage);
                }

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleSearchCount = dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.TimeSlotsDetail.Rows[0][dsSchedule.TimeSlotsDetail.RecordCountColumn.ColumnName],
                            ScheduleSearch_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleSearchCount = 0,
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
                        ScheduleSearchCount = 0,
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

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "UPDATE_SLOT_STATUS":
                    {

                        string SlotId = MDVUtility.ToStr(context.Request["SlotId"]);
                        string BlockReasonId = MDVUtility.ToStr(context.Request["BlockReasonId"]);
                        string BlockStatus = MDVUtility.ToStr(context.Request["BlockStatus"]);
                        string Comments = MDVUtility.ToStr(context.Request["Comments"]);
                        string strJSONData = UpdateSlotStatus(SlotId, MDVUtility.ToInt64(BlockReasonId), BlockStatus, Comments);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_FROMTIME_DDL":
                    {

                        Int64 facilityID = MDVUtility.ToInt64(context.Request["facilityID"]);
                        Int64 providerID = MDVUtility.ToInt64(context.Request["providerID"]);
                        Int64 resourceID = MDVUtility.ToInt64(context.Request["resourceID"]);
                        string scheduleDate = MDVUtility.ToStr(context.Request["scheduleDate"]);
                        string strJSONData = FillFromTimeDDL(facilityID, providerID, resourceID, scheduleDate);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_SCHEDULE_REASONS":
                    {
                        string SchReason = MDVUtility.ToStr(context.Request["SchReason"]);
                        string strJSONData = GetScheduleReasons(SchReason);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "RESCHEDULE_SEARCH":
                    {

                        string fieldsJSON = context.Request["SchedulingSearchData"];
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);

                        string strJSONData = ScheduleSearch(fieldsJSON, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}