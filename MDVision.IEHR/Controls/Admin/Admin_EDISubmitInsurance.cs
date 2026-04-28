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
    public class Admin_EDISubmitInsurance
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDISubmitInsurance()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDISubmitInsurance _obj = null;
        public static Admin_EDISubmitInsurance Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDISubmitInsurance();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the edi submit insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDISubmitInsuranceID">The edi submit insurance identifier.</param>
        /// <returns></returns>
        private string LoadEDISubmitInsurance(string fieldsJSON, Int64 EDISubmitInsuranceID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Submit Insurance", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = null;
                    BLObject<DSEDI> objEDI;
                    if (SearchedfieldsJSON == null)
                        objEDI = BLLAdminEDIObj.LoadEDISubmitInsurance(EDISubmitInsuranceID, null, null, null, null);
                    else
                        objEDI = BLLAdminEDIObj.LoadEDISubmitInsurance(EDISubmitInsuranceID, SearchedfieldsJSON["ddlClearingHouse"], SearchedfieldsJSON["txtEDISubmitInsuranceName"], SearchedfieldsJSON["txtPayorId"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);

                    dsEDI = objEDI.Data;
                    if (objEDI.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            EDISubmitInsuranceCount = dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDI.EDISubmitInsurance.Rows.Count > 0) ? dsEDI.EDISubmitInsurance.Rows[0][dsEDI.EDISubmitInsurance.RecordCountColumn.ColumnName] : 0,
                            EDISubmitInsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EDISubmitInsuranceCount = 0,
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
        /// Handle the EDI Submit Insurance Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_EDI_SUBMIT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDISubmitInsuranceData"];
                        Int64 EDISubmitInsuranceID = MDVUtility.ToInt64(context.Request["EDISubmitInsuranceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadEDISubmitInsurance(fieldsJSON, EDISubmitInsuranceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}