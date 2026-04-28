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
    public class Admin_ProviderSchedule
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_ProviderSchedule()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_ProviderSchedule _obj = null;
        public static Admin_ProviderSchedule Instance()
        {
            if (_obj == null)
                _obj = new Admin_ProviderSchedule();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads the provider schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <returns>System.String.</returns>
        private string LoadProviderSchedule(string fieldsJSON, Int64 ScheduleId, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider Schedule", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLScheduleObj.LoadProviderSchedule(ScheduleId, null, null);
                    else
                        obj = BLLScheduleObj.LoadProviderSchedule(ScheduleId, SearchedfieldsJSON["ddlprovider"], SearchedfieldsJSON["ddlfacility"], PageNumber, RowsPerPage);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ProviderScheduleCount = dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsSchedule.ProviderSchedule.Rows.Count > 0) ? dsSchedule.ProviderSchedule.Rows[0][dsSchedule.ProviderSchedule.RecordCountColumn.ColumnName] : 0,
                            ProviderScheduleLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProviderScheduleCount = 0,
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
                case "SEARCH_PROVIDERSCHEDULE":
                    {
                        string fieldsJSON = context.Request["ProviderScheduleData"];
                        Int64 ScheduleID = MDVUtility.ToInt64(context.Request["ScheduleId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadProviderSchedule(fieldsJSON, ScheduleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
        #endregion
    }
}