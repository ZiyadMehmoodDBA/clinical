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
    public class Admin_EDIClaimStatusInsurance
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIClaimStatusInsurance()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIClaimStatusInsurance _obj = null;
        public static Admin_EDIClaimStatusInsurance Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIClaimStatusInsurance();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the edi submit insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ClaimStatusInsuranceID">The claim status insurance identifier.</param>
        /// <returns></returns>
        private string LoadEDIClaimStatusInsurance(string fieldsJSON, Int64 ClaimStatusInsuranceID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Claim Status Insurance", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = null;
                    BLObject<DSEDI> objEDI;
                    if (SearchedfieldsJSON == null)
                        objEDI = BLLAdminEDIObj.LoadEDIClaimStatusInsurance(ClaimStatusInsuranceID, null, null, null, null);
                    else
                        objEDI = BLLAdminEDIObj.LoadEDIClaimStatusInsurance(ClaimStatusInsuranceID, SearchedfieldsJSON["ddlClearingHouse"], SearchedfieldsJSON["txtEDIStatusInsurance"], SearchedfieldsJSON["txtPayorId"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);

                    dsEDI = objEDI.Data;
                    if (objEDI.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            EDIClaimStatusInsuranceCount = dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDI.EDIClaimStatusInsurance.Rows.Count > 0) ? dsEDI.EDIClaimStatusInsurance.Rows[0][dsEDI.EDIClaimStatusInsurance.RecordCountColumn.ColumnName] : 0,
                            EDIClaimStatusInsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EDIClaimStatusInsuranceCount = 0,
                            Message = objEDI.Message
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
        /// Handle the EDI Claim Status Insurance Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_EDI_CLAIM_STATUS_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDIClaimStatusInsuranceData"];
                        Int64 EDIClaimStatusInsuranceID = MDVUtility.ToInt64(context.Request["EDIClaimStatusInsuranceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadEDIClaimStatusInsurance(fieldsJSON, EDIClaimStatusInsuranceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}