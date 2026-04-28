using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BasicFeeSchedule
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_BasicFeeSchedule()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_BasicFeeSchedule _obj = null;
        public static Admin_BasicFeeSchedule Instance()
        {
            if (_obj == null)
                _obj = new Admin_BasicFeeSchedule();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the basic fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BasicFeeSchID">The basic fee SCH identifier.</param>
        /// <returns></returns>
        private string LoadBasicFeeSchedule(string fieldsJSON, Int64 BasicFeeSchID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsCode = null;
                    BLObject<DSFeeSchedule> objBFS;
                    if (SearchedfieldsJSON == null)
                        objBFS = BLLFeeScheduleObj.LoadBasicFeeSchedule(BasicFeeSchID, null, null, null);
                    else
                        objBFS = BLLFeeScheduleObj.LoadBasicFeeSchedule(BasicFeeSchID, SearchedfieldsJSON["ddlBasicFeeGroup"], SearchedfieldsJSON["txtCPTCode"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);

                    dsCode = objBFS.Data;
                    if (objBFS.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            BasicFeeScheduleCount = dsCode.Tables[dsCode.BasicFeeSchedule.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsCode.BasicFeeSchedule.Rows.Count > 0) ? dsCode.BasicFeeSchedule.Rows[0][dsCode.BasicFeeSchedule.RecordCountColumn.ColumnName] : 0,
                            BasicFeeScheduleLoad_JSON = MDVUtility.JSON_DataTable(dsCode.Tables[dsCode.BasicFeeSchedule.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            BasicFeeScheduleCount = 0,
                            Message = objBFS.Message
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
        /// Handle the Basic Fee Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_BASIC_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["BasicFeeScheduleData"];
                        Int64 BasicFeeScheduleID = MDVUtility.ToInt64(context.Request["BasicFeeScheduleID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadBasicFeeSchedule(fieldsJSON, BasicFeeScheduleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}