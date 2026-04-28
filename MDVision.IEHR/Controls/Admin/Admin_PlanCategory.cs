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
    public class Admin_PlanCategory
    {
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Admin_PlanCategory()
        {
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }
        #region Singleton
        private static Admin_PlanCategory _obj = null;
        public static Admin_PlanCategory Instance()
        {
            if (_obj == null)
                _obj = new Admin_PlanCategory();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the plan category.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PlanCategoryID">The plan category identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadPlanCategory(string fieldsJSON, Int64 PlanCategoryID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Category", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminInsuranceObj.LoadPlanCategory(PlanCategoryID, null, null, null, null);
                    else
                        obj = BLLAdminInsuranceObj.LoadPlanCategory(PlanCategoryID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsInsurance = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PlanCategoryCount = dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsInsurance.PlanCategory.Rows.Count > 0) ? dsInsurance.PlanCategory.Rows[0][dsInsurance.PlanCategory.RecordCountColumn.ColumnName] : 0,
                            PlanCategoryLoad_JSON = MDVUtility.JSON_DataTable(dsInsurance.Tables[dsInsurance.PlanCategory.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PlanCategoryCount = 0,
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
        /// Handle the Plan Category Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PLAN_CATEGORY":
                    {
                        string fieldsJSON = context.Request["PlanCategoryData"];
                        Int64 PlanCategoryID = MDVUtility.ToInt64(context.Request["PlanCategoryID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadPlanCategory(fieldsJSON, PlanCategoryID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}