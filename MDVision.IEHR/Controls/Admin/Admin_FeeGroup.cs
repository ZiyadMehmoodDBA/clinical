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
    public class Admin_FeeGroup
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_FeeGroup()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_FeeGroup _obj = null;
        public static Admin_FeeGroup Instance()
        {
            if (_obj == null)
                _obj = new Admin_FeeGroup();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the FeeGroup for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The FeeGroup identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadFeeGroup(string fieldsJSON, Int64 FeeGroupID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeGroup = null;
                    BLObject<DSFeeSchedule> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLFeeScheduleObj.LoadFeeGroup(FeeGroupID, null, null, null, null);
                    else
                        obj = BLLFeeScheduleObj.LoadFeeGroup(FeeGroupID, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsFeeGroup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            FeeGroupCount = dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsFeeGroup.FeeGroup.Rows.Count > 0) ? dsFeeGroup.FeeGroup.Rows[0][dsFeeGroup.FeeGroup.RecordCountColumn.ColumnName] : 0,
                            FeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FeeGroupCount = 0,
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
        /// Handle the Procedure Category Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_FEE_GROUP":
                    {
                        string fieldsJSON = context.Request["FeeGroupData"];
                        Int64 FeeGroupID = MDVUtility.ToInt64(context.Request["FeeGroupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadFeeGroup(fieldsJSON, FeeGroupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
