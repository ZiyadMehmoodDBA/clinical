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
    public class Admin_ProcedureCategory
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_ProcedureCategory()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_ProcedureCategory _obj = null;
        public static Admin_ProcedureCategory Instance()
        {
            if (_obj == null)
                _obj = new Admin_ProcedureCategory();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the specialities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadProcedureCategory(string fieldsJSON, Int64 ProcedureCategoryID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Procedure Category", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsProcedureCategory = null;
                    BLObject<DSCodes> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminCodesObj.LoadProcedureCategory(ProcedureCategoryID, null, null, null);
                    else
                        obj = BLLAdminCodesObj.LoadProcedureCategory(ProcedureCategoryID, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsProcedureCategory = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureCategoryCount = dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsProcedureCategory.ProcedureCategory.Rows.Count > 0) ? dsProcedureCategory.ProcedureCategory.Rows[0][dsProcedureCategory.ProcedureCategory.RecordCountColumn.ColumnName] : 0,
                            ProcedureCategoryLoad_JSON = MDVUtility.JSON_DataTable(dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureCategoryCount = 0,
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
                case "SEARCH_PROCEDURE_CATEGORY":
                    {
                        string fieldsJSON = context.Request["ProcedureCategoryData"];
                        Int64 ProcedureCategoryID = MDVUtility.ToInt64(context.Request["ProcedureCategoryID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadProcedureCategory(fieldsJSON, ProcedureCategoryID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}