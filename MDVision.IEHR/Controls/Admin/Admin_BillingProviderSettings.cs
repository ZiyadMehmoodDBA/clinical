
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BillingProviderSettings
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_BillingProviderSettings()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_BillingProviderSettings _obj = null;
        public static Admin_BillingProviderSettings Instance()
        {
            if (_obj == null)
                _obj = new Admin_BillingProviderSettings();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the specialities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns></returns>
        private string LoadBillingProviderSettings(string fieldsJSON, Int64 BillingProviderSettingsID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Additional Billing Provider", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSBillingProviderSettings dsBillingProviderSettings = null;
                    BLObject<DSBillingProviderSettings> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminProfileObj.LoadBillingProviderSettings(BillingProviderSettingsID, null, null, null, null);
                    //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyID, null, null);
                    else
                        obj = BLLAdminProfileObj.LoadBillingProviderSettings(BillingProviderSettingsID, SearchedfieldsJSON["ddlInsurance"], SearchedfieldsJSON["ddlFacility"], SearchedfieldsJSON["ddlProvider"], SearchedfieldsJSON["chkIsActice"], PageNumber, RowsPerPage);
                    //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"]);
                    dsBillingProviderSettings = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            BillingProviderSettingsCount = dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsBillingProviderSettings.BillingProviderSettings.Rows.Count > 0) ? dsBillingProviderSettings.BillingProviderSettings.Rows[0][dsBillingProviderSettings.BillingProviderSettings.RecordCountColumn.ColumnName] : 0,
                            BillingProviderSettingsLoad_JSON = MDVUtility.JSON_DataTable(dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName]),
                            //SpecialtyLoad_JSON = MDVUtility.JSON_DataTable((dsProfile.Tables[dsProfile.Specialty.TableName])(obj.Data)),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
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
        /// <summary>
        /// Handle the Provider Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_BILLING_PROVIDER_SETTINGS":
                    {
                        string fieldsJSON = context.Request["BillingProviderSettingsData"];
                        Int64 BillingProviderSettingsID = MDVUtility.ToInt64(context.Request["BillingProviderSettingsID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadBillingProviderSettings(fieldsJSON, BillingProviderSettingsID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}