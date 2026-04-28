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
    public class Admin_BasicFeeGroup
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_BasicFeeGroup()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_BasicFeeGroup _obj = null;
        public static Admin_BasicFeeGroup Instance()
        {
            if (_obj == null)
                _obj = new Admin_BasicFeeGroup();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the BasicFeeGroup for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The BasicFeeGroup identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadBasicFeeGroup(string fieldsJSON, Int64 BasicFeeGroupID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Group", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsBasicFeeGroup = null;
                    BLObject<DSFeeSchedule> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLFeeScheduleObj.LoadBasicFeeGroup(BasicFeeGroupID, null, null, null, null, PageNumber, RowsPerPage);
                    else
                        obj = BLLFeeScheduleObj.LoadBasicFeeGroup(BasicFeeGroupID, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsBasicFeeGroup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            BasicFeeGroupCount = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsBasicFeeGroup.BasicFeeGroup.Rows.Count > 0) ? dsBasicFeeGroup.BasicFeeGroup.Rows[0][dsBasicFeeGroup.BasicFeeGroup.RecordCountColumn.ColumnName] : 0,
                            BasicFeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            BasicFeeGroupCount = 0,
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
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_BASIC_FEE_GROUP":
                    {
                        string fieldsJSON = context.Request["BasicFeeGroupData"];
                        Int64 BasicFeeGroupID = MDVUtility.ToInt64(context.Request["BasicFeeGroupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadBasicFeeGroup(fieldsJSON, BasicFeeGroupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
