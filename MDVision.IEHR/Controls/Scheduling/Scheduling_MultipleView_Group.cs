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
    public class Scheduling_MultipleView_Group
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_MultipleView_Group()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_MultipleView_Group _obj = null;
        public static Scheduling_MultipleView_Group Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_MultipleView_Group();
            return _obj;
        }
        #endregion

        #region "Private Functions"
        private string LoadScheduleGroup(string fieldsJSON, Int32 MSGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSScheduleGroup dsViewGroup = null;
                BLObject<DSScheduleGroup> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLScheduleObj.LoadScheduleGroups(MSGroupId, null, null, null, null);
                else
                    obj = BLLScheduleObj.LoadScheduleGroups(MSGroupId, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["chkIsActice"], SearchedfieldsJSON["ddlEntity"]);
                dsViewGroup = obj.Data;
                var response = new
                {
                    status = true,
                    MultipleViewGroupCount = dsViewGroup.Tables[dsViewGroup.MultipleScheduleGroups.TableName].Rows.Count,
                    MultipleViewGroupLoad_JSON = MDVUtility.JSON_DataTable(dsViewGroup.Tables[dsViewGroup.MultipleScheduleGroups.TableName]),
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

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_SCHEDULE_GROUP":
                    {
                        string fieldsJSON = context.Request["MultipleViewGroupData"];
                        Int32 MSGroupId = MDVUtility.ToInt32(context.Request["MSGroupId"]);
                        string strJSONData = LoadScheduleGroup(fieldsJSON, MSGroupId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}