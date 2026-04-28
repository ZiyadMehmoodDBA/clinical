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
    public class Admin_PrivilegeGroup
    {
         private BLLAdminSecurity BLLAdminSecurityObj = null;
         public Admin_PrivilegeGroup()
        {
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Admin_PrivilegeGroup _obj = null;
        public static Admin_PrivilegeGroup Instance()
        {
            if (_obj == null)
                _obj = new Admin_PrivilegeGroup();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the privilege group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PrivilegeGroupID">The privilege group identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadPrivilegeGroup(string fieldsJSON, Int64 PrivilegeGroupID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    Common.MDVisionLookups objs = new Common.MDVisionLookups();
                    DSPrivilegeGroup dsPrivGroup = null;
                    BLObject<DSPrivilegeGroup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminSecurityObj.LoadPrivilegeGroup(PrivilegeGroupID, null, null, null);
                    else
                        obj = BLLAdminSecurityObj.LoadPrivilegeGroup(PrivilegeGroupID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsPrivGroup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PrivilegeGroupCount = dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsPrivGroup.SecurityGroup.Rows.Count > 0) ? dsPrivGroup.SecurityGroup.Rows[0][dsPrivGroup.SecurityGroup.RecordCountColumn.ColumnName] : 0,
                            PrivilegeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PrivilegeGroupCount = 0,
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
        /// Handle the Privilege Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PRIVILEGE_GROUP":
                    {
                        string fieldsJSON = context.Request["PrivilegeGroupData"];
                        Int64 PrivilegeGroupID = MDVUtility.ToInt64(context.Request["PrivilegeGroupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadPrivilegeGroup(fieldsJSON, PrivilegeGroupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}