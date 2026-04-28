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
    public class Admin_Modifier
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_Modifier()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_Modifier _obj = null;
        public static Admin_Modifier Instance()
        {
            if (_obj == null)
                _obj = new Admin_Modifier();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the modifier.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ModifierID">The modifier identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadModifier(string fieldsJSON, Int64 ModifierID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modifier", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminCodesObj.LoadModifier(ModifierID, null, null, null);
                    else
                        obj = BLLAdminCodesObj.LoadModifier(ModifierID, SearchedfieldsJSON["txtModifierCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsCodes = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ModifierCount = dsCodes.Tables[dsCodes.Modifier.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsCodes.Modifier.Rows.Count > 0) ? dsCodes.Modifier.Rows[0][dsCodes.Modifier.RecordCountColumn.ColumnName] : 0,
                            ModifierLoad_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.Modifier.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            ModifierCount = 0,
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
        /// Handle the Modifier Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_MODIFIER":
                    {
                        string fieldsJSON = context.Request["ModifierData"];
                        Int64 ModifierID = MDVUtility.ToInt64(context.Request["ModifierID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadModifier(fieldsJSON, ModifierID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}