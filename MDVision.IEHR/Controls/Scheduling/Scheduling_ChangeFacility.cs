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
    public class Scheduling_ChangeFacility
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_ChangeFacility()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_ChangeFacility _obj = null;
        public static Scheduling_ChangeFacility Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_ChangeFacility();
            return _obj;
        }
        #endregion

        #region "Change Facility"


        private string ChangeSchFacility(string SlotDtlIds, long MoveFacilityId)
        {
            try
            {

                {
                    BLObject<string> obj = BLLScheduleObj.ChangeSchFacility(SlotDtlIds, MoveFacilityId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Successfully Updated"
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

        #endregion

        #region "Service Command Handler"


        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "CHANGE_SCH_FACILITY":
                    {

                        string SlotDtlIds = MDVUtility.ToStr(context.Request["SlotDtlIds"]);
                        Int64 MoveFacilityId = MDVUtility.ToInt64(context.Request["MoveFacilityId"]);
                        string strJSONData = ChangeSchFacility(SlotDtlIds, MoveFacilityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

    }
}