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
    public class Admin_EDITaxIDSetup
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDITaxIDSetup()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDITaxIDSetup _obj = null;
        public static Admin_EDITaxIDSetup Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDITaxIDSetup();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the EDITaxIDSetup for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The EDITaxIDSetup identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadEDITaxIDSetup(string fieldsJSON, Int64 EDITaxIDSetupID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Tax ID Setup", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDITaxIDSetup = null;
                    BLObject<DSEDI> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminEDIObj.LoadEDITaxIDSetup(EDITaxIDSetupID, null, null, null, null, pageNo, rpp);
                    else
                        obj = BLLAdminEDIObj.LoadEDITaxIDSetup(EDITaxIDSetupID, SearchedfieldsJSON["txtTaxID"], SearchedfieldsJSON["ddlClearinghouse"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);
                    dsEDITaxIDSetup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            EDITaxIDSetupCount = dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDITaxIDSetup.EDITaxIDSetup.Rows.Count > 0) ? dsEDITaxIDSetup.EDITaxIDSetup.Rows[0][dsEDITaxIDSetup.EDITaxIDSetup.RecordCountColumn.ColumnName] : 0,
                            EDITaxIDSetupLoad_JSON = MDVUtility.JSON_DataTable(dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EDITaxIDSetupCount = 0,
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
        /// Handle the EDI Taxx ID Setup Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_EDI_TAX_ID_SETUP":
                    {
                        string fieldsJSON = context.Request["EDITaxIDSetupData"];
                        Int64 EDITaxIDSetupID = MDVUtility.ToInt64(context.Request["EDITaxIDSetupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadEDITaxIDSetup(fieldsJSON, EDITaxIDSetupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
