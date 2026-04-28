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
    public class Admin_ResourceSchedule
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_ResourceSchedule()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_ResourceSchedule _obj = null;
        public static Admin_ResourceSchedule Instance()
        {
            if (_obj == null)
                _obj = new Admin_ResourceSchedule();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string LoadResourceSchedule(string fieldsJSON, Int64 ResScheduleId, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resource Schedule", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLScheduleObj.LoadResourceSchedule(ResScheduleId, null, null);
                    else
                        obj = BLLScheduleObj.LoadResourceSchedule(ResScheduleId, SearchedfieldsJSON["ddlresource"], SearchedfieldsJSON["ddlfacility"], PageNumber, RowsPerPage);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ResourceScheduleCount = dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsSchedule.ResourceSchedule.Rows.Count > 0) ? dsSchedule.ResourceSchedule.Rows[0][dsSchedule.ResourceSchedule.RecordCountColumn.ColumnName] : 0,
                            ResourceScheduleLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResourceScheduleCount = 0,
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
        /// Handle the Specialty Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_RESOURCESCHEDULE":
                    {
                        string fieldsJSON = context.Request["ResourceScheduleData"];
                        Int64 ResScheduleID = MDVUtility.ToInt64(context.Request["ResScheduleId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadResourceSchedule(fieldsJSON, ResScheduleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
        #endregion
    }
}