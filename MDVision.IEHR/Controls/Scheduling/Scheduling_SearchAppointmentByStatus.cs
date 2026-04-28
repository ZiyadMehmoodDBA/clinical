using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_SearchAppointmentByStatus
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_SearchAppointmentByStatus()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_SearchAppointmentByStatus _obj = null;
        public static Scheduling_SearchAppointmentByStatus Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_SearchAppointmentByStatus();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string SearchAppByStatus(Int64 ProviderId, Int64 FacilityId, string SlotDate, string Color, Int64 ResourceId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                obj = BLLScheduleObj.LoadSchAppointment(ProviderId, FacilityId, SlotDate, Color, 0, ResourceId);
                dsSchedule = obj.Data;

                var response = new
                {
                    status = true,
                    SchAppStatusCount = dsSchedule.Tables[dsSchedule.SchAppointment.TableName].Rows.Count,
                    SchAppStatus_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.SchAppointment.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                case "SEARCH_APP_BY_STATUS":
                    {
                        string ProviderId = MDVUtility.ToStr(context.Request["ProviderId"]);
                        string FacilityId = MDVUtility.ToStr(context.Request["FacilityId"]);
                        string SlotDate = MDVUtility.ToStr(context.Request["SlotDate"]);
                        string color = MDVUtility.ToStr(context.Request["color"]);
                        string ResourceId = MDVUtility.ToStr(context.Request["ResourceId"]);
                        string strJSONData = SearchAppByStatus(MDVUtility.ToInt64(ProviderId), MDVUtility.ToInt64(FacilityId), SlotDate, color, MDVUtility.ToInt64(ResourceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}