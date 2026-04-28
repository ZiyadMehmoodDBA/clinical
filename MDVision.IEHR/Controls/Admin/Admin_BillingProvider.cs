using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BillingProvider
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_BillingProvider()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        #region Singleton
        private static Admin_BillingProvider _obj = null;
        public static Admin_BillingProvider Instance()
        {
            if (_obj == null)
                _obj = new Admin_BillingProvider();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string LoadBillingProvider(string fieldsJSON, Int64 BillingProviderID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Billing Provider", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSBillingProviderSettings dsBillingProviderSettings = null;
                BLObject<DSBillingProviderSettings> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLAdminProfileObj.LoadBillingProvider(BillingProviderID, null, null, null, null,null);
                else
                    obj = BLLAdminProfileObj.LoadBillingProvider(BillingProviderID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkIsBillToEIN"], SearchedfieldsJSON["chkIsActice"],null, PageNumber, RowsPerPage);

                    dsBillingProviderSettings = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            BillingProviderCount = dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProvider.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsBillingProviderSettings.BillingProvider.Rows.Count > 0) ? dsBillingProviderSettings.BillingProvider.Rows[0][dsBillingProviderSettings.BillingProvider.RecordCountColumn.ColumnName] : 0,
                            BillingProviderLoad_JSON = MDVUtility.JSON_DataTable(dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProvider.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            BillingProviderSettingsCount = 0,
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
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_BILLING_PROVIDER":
                    {
                        string fieldsJSON = context.Request["BillingProviderData"];
                        Int64 BillingProviderID = MDVUtility.ToInt64(context.Request["BillingProviderID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadBillingProvider(fieldsJSON, BillingProviderID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion

    }
}