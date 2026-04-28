using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using System.Collections;
using System.Text.RegularExpressions;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_MuliView
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_MuliView()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_MuliView _obj = null;
        public static Scheduling_MuliView Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_MuliView();
            return _obj;
        }
        #endregion

        #region "Search Day Slot Schedule"

        private string SearchDaySlotSchedule(Int64 ProviderId, Int64 Resourceid, Int64 Facilityid, string SlotDate, string StatusId)
        {
            try
            {
                string responseid = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Facilityid)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SlotDate)))
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

                    if (ProviderId != 0)
                        responseid = "Provider" + ProviderId + "Facility" + Facilityid;
                    if (Resourceid != 0)
                        responseid = "Resource" + Resourceid + "Facility" + Facilityid;
                    DSAppointment dsSchedule = null;

                    BLObject<DSAppointment> obj = BLLScheduleObj.SearchDailySlots(ProviderId, Resourceid, Facilityid, SlotDate, StatusId);

                    // BLObject<DSAppointment> obj =BLLScheduleObj.SearchDailySlots(42, Resourceid, 32, "2015-01-01");
                    dsSchedule = obj.Data;


                    BLLScheduleObj.ScheduleAppSort(ref dsSchedule);


                    if (dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count > 0)
                    {

                        var response = new
                        {
                            status = true,
                            id = responseid,
                            ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.DaySlots.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    if (dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count <= 0)
                    {
                        var response = new
                        {
                            status = false,
                            id = responseid,
                            ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.DaySlots.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    var response1 = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response1);

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

        private string SearchMultipleView(string ProviderIds, string Resourceids, string Facilityids, string criteriadate, string StatusId)
        {
            List<object> Allrecord = new List<object>();
            string StatusId1 = StatusId;
            StatusId1 = null;
            string[] Providers = new string[] { };
            string[] Resources = new string[] { };
            string value = Facilityids;
            string value1 = ProviderIds;
            string value2 = Resourceids;
            string a = "0";
            string[] Facilities = Regex.Split(value, ",");
            if (ProviderIds != "" && ProviderIds != "null")
            {
                Providers = Regex.Split(value1, ",");
            }
            else
            {
                Providers = new string[] { };
            }
            if (Resourceids != "" && Resourceids != "null")
            {
                Resources = Regex.Split(value2, ",");
            }

            else
            {
                Resources = new string[] { };
            }
            foreach (string facility in Facilities) // start facility loop
            {
                if (Providers.Length != 0)
                {
                    foreach (string provider in Providers) // start facility loop
                    {

                        string response = SearchDaySlotSchedule(MDVUtility.ToInt64(provider), MDVUtility.ToInt64(a), MDVUtility.ToInt64(facility), criteriadate, StatusId);
                        Allrecord.Add(response);

                    }
                }
                if (Resources.Length != 0)
                {

                    foreach (string resource in Resources) // start resource loop
                    {

                        string response = SearchDaySlotSchedule(MDVUtility.ToInt64(a), MDVUtility.ToInt64(resource), MDVUtility.ToInt64(facility), criteriadate, StatusId);
                        Allrecord.Add(response);
                    }
                }

            }

            return (Newtonsoft.Json.JsonConvert.SerializeObject(Allrecord));
        }

        private string LoadSchGroupProvResByID(Int64 MSGroupId, string criteriadate, string StatusId)
        {
            List<object> Allrecord = new List<object>();
            List<object> Grouprecord = new List<object>();
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MSGroupId)))
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

                    DSScheduleGroup dsSchedule = null;
                    BLObject<DSScheduleGroup> obj = BLLScheduleObj.LoadSchGroupProvResByID(MSGroupId);
                    dsSchedule = obj.Data;
                    int totalcount = 0;
                    if (dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows.Count; i++)
                        {
                            string facilityid = dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows[i][dsSchedule.ScheduleGroupsProRes.FacilityIdColumn.ColumnName].ToString();
                            string[] Provider = dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows[i][dsSchedule.ScheduleGroupsProRes.ProviderColumn.ColumnName].ToString().Split(',');
                            string[] Resource = dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows[i][dsSchedule.ScheduleGroupsProRes.ResourceColumn.ColumnName].ToString().Split(',');

                            string providercount = dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows[i][dsSchedule.ScheduleGroupsProRes.ProCountColumn.ColumnName].ToString();
                            string resourcecount = dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName].Rows[i][dsSchedule.ScheduleGroupsProRes.ResCountColumn.ColumnName].ToString();

                            totalcount += int.Parse(providercount) + int.Parse(resourcecount);
                            string a = "0";
                            for (int j = 0; j < Provider.Length; j++)
                            {

                                string providerid = Provider[j];
                                string response = SearchDaySlotSchedule(MDVUtility.ToInt64(providerid), MDVUtility.ToInt64(a), MDVUtility.ToInt64(facilityid), criteriadate, StatusId);
                                Allrecord.Add(response);

                            }
                            for (int k = 0; k < Resource.Length; k++)
                            {

                                string resourceid = Resource[k];
                                string response = SearchDaySlotSchedule(MDVUtility.ToInt64(a), MDVUtility.ToInt64(resourceid), MDVUtility.ToInt64(facilityid), criteriadate, StatusId);
                                Allrecord.Add(response);

                            }

                        }
                    }


                    var response1 = new
                    {
                        status = true,
                        totalrecord = totalcount,
                        ScheduleRecord_JSON = Allrecord,
                        ScheduleGroupsProRes_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ScheduleGroupsProRes.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));

                }
                // return (Newtonsoft.Json.JsonConvert.SerializeObject(Allrecord));
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

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_MULTIPLEVIEW":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strResourceId = context.Request["ResourceID"];
                        string strFacilityd = context.Request["FacilityID"];
                        string SlotDate = context.Request["SlotDate"];
                        string StatusId = context.Request["StatusId"];
                        string strJSONData = SearchMultipleView(strProviderId, strResourceId, strFacilityd, MDVUtility.ToStr(SlotDate), MDVUtility.ToStr(StatusId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "LOAD_SCHEDULE_GROUP_PRORES":
                    {
                        string MSGroupId = context.Request["MSGroupId"];
                        string SlotDate = context.Request["SlotDate"];
                        string StatusId = context.Request["StatusId"];
                        string strJSONData = LoadSchGroupProvResByID(MDVUtility.ToInt64(MSGroupId), MDVUtility.ToStr(SlotDate), MDVUtility.ToStr(StatusId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


            }
        }
        #endregion

    }
}