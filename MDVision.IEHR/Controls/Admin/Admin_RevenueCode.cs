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
    public class Admin_RevenueCode
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_RevenueCode()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_RevenueCode _obj = null;
        public static Admin_RevenueCode Instance()
        {
            if (_obj == null)
                _obj = new Admin_RevenueCode();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the RevenueCode for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The RevenueCode identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadRevenueCode(string fieldsJSON, Int64 RevenueCodeID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsRevenueCode = null;
                    BLObject<DSCodes> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminCodesObj.LoadRevenueCode(RevenueCodeID, null, null, null, null);
                    else
                        obj = BLLAdminCodesObj.LoadRevenueCode(RevenueCodeID, SearchedfieldsJSON["txtRevenueCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsRevenueCode = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            RevenueCodeCount = dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsRevenueCode.RevenueCode.Rows.Count > 0) ? dsRevenueCode.RevenueCode.Rows[0][dsRevenueCode.RevenueCode.RecordCountColumn.ColumnName] : 0,
                            RevenueCodeLoad_JSON = MDVUtility.JSON_DataTable(dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            RevenueCodeCount = 0,
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
        /// Handle the Revenue Code Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_REVENUE_CODE":
                    {
                        string fieldsJSON = context.Request["RevenueCodeData"];
                        Int64 RevenueCodeID = MDVUtility.ToInt64(context.Request["RevenueCodeID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadRevenueCode(fieldsJSON, RevenueCodeID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}