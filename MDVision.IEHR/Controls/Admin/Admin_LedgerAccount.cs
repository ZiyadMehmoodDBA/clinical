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
    public class Admin_LedgerAccount
    {
        private BLLBilling BLLBillingObj = null;
        public Admin_LedgerAccount()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Admin_LedgerAccount _obj = null;
        public static Admin_LedgerAccount Instance()
        {
            if (_obj == null)
                _obj = new Admin_LedgerAccount();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the Ledger Account for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadLedgerAccount(string fieldsJSON, Int64 LedgerAccountID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Ledger Account", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPaymentSetup dsLedgerAccount = null;
                    BLObject<DSPaymentSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLBillingObj.LoadLedgerAccount(LedgerAccountID, null, null, null, null);
                    else
                        obj = BLLBillingObj.LoadLedgerAccount(LedgerAccountID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsLedgerAccount = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            LedgerAccountCount = dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsLedgerAccount.LedgerAccount.Rows.Count > 0) ? dsLedgerAccount.LedgerAccount.Rows[0][dsLedgerAccount.LedgerAccount.RecordCountColumn.ColumnName] : 0,
                            LedgerAccountLoad_JSON = MDVUtility.JSON_DataTable(dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            LedgerAccountCount = 0,
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
                case "SEARCH_LEDGER_ACCOUNT":
                    {
                        string fieldsJSON = context.Request["LedgerAccountData"];
                        Int64 LedgerAccountID = MDVUtility.ToInt64(context.Request["LedgerAccountID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadLedgerAccount(fieldsJSON, LedgerAccountID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}