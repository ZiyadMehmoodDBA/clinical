using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Drug_Code_Cost
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_Drug_Code_Cost()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_Drug_Code_Cost _obj = null;
        public static Admin_Drug_Code_Cost Instance()
        {
            if (_obj == null)
                _obj = new Admin_Drug_Code_Cost();
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
        private string LoadDrugCodeCost(string fieldsJSON, Int64 DrugCodeCostID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DrugCodeCost", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj;
                    if (SearchedfieldsJSON == null)
                    {
                        obj = BLLAdminCodesObj.LoadDrugCodeCostAll(DrugCodeCostID, null, null, null);

                    }
                    else if (!SearchedfieldsJSON.ContainsKey("ddlEntity"))
                    {
                        obj = BLLAdminCodesObj.LoadDrugCodeCostAll(DrugCodeCostID, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDrugCost"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                    }
                    else
                    {
                        obj = BLLAdminCodesObj.LoadDrugCodeCostAll(DrugCodeCostID, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDrugCost"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                        //obj = BLLAdminCodesObj.LoadDrugCodeCostAll(DrugCodeCostID, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDrugCost"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                    }
                    dsCodes = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            DrugCodeCostCount = dsCodes.Tables[dsCodes.CPTCodeCost.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsCodes.CPTCodeCost.Rows.Count > 0) ? dsCodes.CPTCodeCost.Rows[0][dsCodes.CPTCodeCost.RecordCountColumn.ColumnName] : 0,
                            DrugCodeCostCountLoad_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.CPTCodeCost.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SpecialtyCount = 0,
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
        /// Handle the Specialty Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_DRUG_CODE_COST":
                    {
                        string fieldsJSON = context.Request["DrugCodeCostData"];
                        Int64 DrugCodeCostID = MDVUtility.ToInt64(context.Request["DrugCodeCostID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadDrugCodeCost(fieldsJSON, DrugCodeCostID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                //case "APPLY_SECURITY":
                //    {
                //        string formname = "Specialty";
                //        string strJSONData = AppPrivileges.GetFormSecurity(formname);

                //        context.Response.ContentType = "text/plain";
                //        context.Response.Write(strJSONData);
                //    }
                //break;
            }
        }
        #endregion
    }
}