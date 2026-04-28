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
    public class Admin_ReferringProvider
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_ReferringProvider()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton

        private static Admin_ReferringProvider _instance = null;
        public static Admin_ReferringProvider Instance()
        {
            if (_instance == null)
                _instance = new Admin_ReferringProvider();
            return _instance;
        }

        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the resources.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ResourceID">The ReferringProvider identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadReferringProvider(string fieldsJSON, Int64 ReferringProviderID, int PageNumber, int RowsPerPage, string ParentCtrl = "", string ComeFromSearchBtn = "")
        {
            //string IsActive = null;
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referring Provider", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    string Specialty = "", IsSovereign = string.Empty;
                    if (SearchedfieldsJSON.ContainsKey("ddlSpecialty"))
                    {
                        Specialty = SearchedfieldsJSON["ddlSpecialty"];
                    }

                    if (!string.IsNullOrEmpty(ComeFromSearchBtn) && MDVUtility.ToBool(ComeFromSearchBtn))
                    {
                        IsSovereign = MDVUtility.ToStr(SearchedfieldsJSON["chkIsSovereign"]);
                    }

                    DSProfile dsEntity = null;
                    BLObject<DSProfile> obj;

                    if (SearchedfieldsJSON == null)
                    {
                        obj = BLLAdminProfileObj.LoadReferringProvider(ReferringProviderID, null, null, null, null, null);
                    }
                    else if (ParentCtrl == "Patient_Referrals_Outgoing_Detail")
                    {
                        obj = BLLAdminProfileObj.LoadReferringProvider(ReferringProviderID, SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["chkActive"], SearchedfieldsJSON["txtNPI"], SearchedfieldsJSON["ddlEntity"], PageNumber, RowsPerPage, ParentCtrl, null, Specialty, SearchedfieldsJSON["txtPhone"], IsSovereign);
                    }
                    else
                    {
                        obj = BLLAdminProfileObj.LoadReferringProvider(ReferringProviderID, SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["chkActive"], SearchedfieldsJSON["txtNPI"], SearchedfieldsJSON["ddlEntity"], PageNumber, RowsPerPage, "", SearchedfieldsJSON["txtFax"], Specialty, SearchedfieldsJSON["txtPhone"], IsSovereign);
                    }

                    dsEntity = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ReferringProviderCount = dsEntity.Tables[dsEntity.ReferringProvider.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.ReferringProvider.Rows.Count > 0) ? dsEntity.ReferringProvider.Rows[0][dsEntity.ReferringProvider.RecordCountColumn.ColumnName] : 0,
                            ReferringProviderLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.ReferringProvider.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ReferringProviderCount = 0,
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
        /// Handle the ReferringProvider Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_REFERRING_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ReferringProviderData"];
                        Int64 ReferringProviderID = MDVUtility.ToInt64(context.Request["ReferringProviderID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string ParentCtrl = MDVUtility.ToStr(context.Request["ParentCtrl"]);
                        string ComeFromSearchBtn = MDVUtility.ToStr(context.Request["ComeFromSearchBtn"]);

                        string strJSONData = LoadReferringProvider(fieldsJSON, ReferringProviderID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage), ParentCtrl, ComeFromSearchBtn);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}