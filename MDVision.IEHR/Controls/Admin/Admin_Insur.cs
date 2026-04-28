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
    public class Admin_Insur
    {
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Admin_Insur()
        {
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }

        #region Singleton
        private static Admin_Insur _obj = null;
        public static Admin_Insur Instance()
        {
            if (_obj == null)
                _obj = new Admin_Insur();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="InsuranceID">The insurance identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadInsurance(string fieldsJSON, Int64 InsuranceID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminInsuranceObj.LoadInsurance(InsuranceID, null, null, null, null);
                    else
                        obj = BLLAdminInsuranceObj.LoadInsurance(InsuranceID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsInsurance = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            InsuranceCount = dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsInsurance.Insurance.Rows.Count > 0) ? dsInsurance.Insurance.Rows[0][dsInsurance.Insurance.RecordCountColumn.ColumnName] : 0,
                            InsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsInsurance.Tables[dsInsurance.Insurance.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            InsuranceCount = 0,
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
        /// Handle the Insurance Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_INSURANCE":
                    {
                        string fieldsJSON = context.Request["InsuranceData"];
                        Int64 InsuranceID = MDVUtility.ToInt64(context.Request["InsuranceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadInsurance(fieldsJSON, InsuranceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}