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
    public class Admin_Resources
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Resources()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton

        private static Admin_Resources _instance = null;
        public static Admin_Resources Instance()
        {
            if (_instance == null)
                _instance = new Admin_Resources();
            return _instance;
        }

        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the resources.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ResourceID">The resource identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadResources(string fieldsJSON, Int64 ResourceID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resources", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsEntity = null;
                    BLObject<DSProfile> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminProfileObj.LoadResources(ResourceID, null, null);
                    else
                        obj = BLLAdminProfileObj.LoadResources(ResourceID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);

                    dsEntity = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ResourcesCount = dsEntity.Tables[dsEntity.Resources.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.Resources.Rows.Count > 0) ? dsEntity.Resources.Rows[0][dsEntity.Resources.RecordCountColumn.ColumnName] : 0,
                            ResourcesLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.Resources.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResourcesCount = 0,
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
        /// Handle the Resource Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_RESOURCES":
                    {
                        string fieldsJSON = context.Request["ResourcesData"];
                        Int64 ResourceID = MDVUtility.ToInt64(context.Request["ResourceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadResources(fieldsJSON, ResourceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}