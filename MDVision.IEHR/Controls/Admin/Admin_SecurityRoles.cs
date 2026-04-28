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
    public class Admin_SecurityRoles
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_SecurityRoles()
        {
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Admin_SecurityRoles _obj = null;
        public static Admin_SecurityRoles Instance()
        {
            if (_obj == null)
                _obj = new Admin_SecurityRoles();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the security roles for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The security role identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadSecurityRole(string fieldsJSON, Int64 SecurityRoleID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSRoles dsRoles = null;
                    BLObject<DSRoles> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminSecurityObj.LoadRoles(SecurityRoleID, null, null, null);
                    else
                        obj = BLLAdminSecurityObj.LoadRoles(SecurityRoleID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["chkIsActice"], PageNumber, RowsPerPage);
                    dsRoles = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            SecurityRoleCount = dsRoles.Roles.Rows.Count,
                            iTotalDisplayRecords = (dsRoles.Roles.Rows.Count > 0) ? dsRoles.Roles.Rows[0][dsRoles.Roles.RecordCountColumn.ColumnName] : 0,
                            SecurityRoleLoad_JSON = MDVUtility.JSON_DataTable(dsRoles.Roles),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SecurityRoleCount = 0,
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
        /// Handle the Security Roles Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_SECURITY_ROLE":
                    {
                        string fieldsJSON = context.Request["SecurityRoleData"];
                        Int64 SecurityRoleID = MDVUtility.ToInt64(context.Request["SecurityRoleID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);

                        string strJSONData = LoadSecurityRole(fieldsJSON, SecurityRoleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}