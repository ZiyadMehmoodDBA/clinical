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
    public class Admin_EDIEligibilityInsurance
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIEligibilityInsurance()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIEligibilityInsurance _obj = null;
        public static Admin_EDIEligibilityInsurance Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIEligibilityInsurance();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the edi eligibility insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EligibilityInsuranceID">The eligibility insurance identifier.</param>
        /// <returns></returns>
        private string LoadEDIEligibilityInsurance(string fieldsJSON, Int64 EligibilityInsuranceID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Eligibility Insurance", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = null;
                    BLObject<DSEDI> objEDI;
                    if (SearchedfieldsJSON == null)
                        objEDI = BLLAdminEDIObj.LoadEDIEligibilityInsurance(EligibilityInsuranceID, null, null, null, null, pageNo, rpp);
                    else
                        objEDI = BLLAdminEDIObj.LoadEDIEligibilityInsurance(EligibilityInsuranceID, SearchedfieldsJSON["ddlClearingHouse"], SearchedfieldsJSON["txtEDIEligibilityInsurance"], SearchedfieldsJSON["txtPayorId"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);

                    dsEDI = objEDI.Data;
                    if (objEDI.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            EDIEligibilityInsuranceCount = dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDI.EDIEligibilityInsurance.Rows.Count > 0) ? dsEDI.EDIEligibilityInsurance.Rows[0][dsEDI.EDIEligibilityInsurance.RecordCountColumn.ColumnName] : 0,
                            EDIEligibilityInsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EDIEligibilityInsuranceCount = 0,
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
        /// Handle the EDI Eligibility Insurance Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_EDI_ELIGIBILITY_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDIEligibilityInsuranceData"];
                        Int64 EDIEligibilityInsuranceID = MDVUtility.ToInt64(context.Request["EDIEligibilityInsuranceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadEDIEligibilityInsurance(fieldsJSON, EDIEligibilityInsuranceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}