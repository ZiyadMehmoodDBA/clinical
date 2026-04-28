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
    public class Admin_SubmitterSetup
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_SubmitterSetup()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_SubmitterSetup _instance = null;
        public static Admin_SubmitterSetup Instance()
        {
            if (_instance == null)
                _instance = new Admin_SubmitterSetup();
            return _instance;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the submitter setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SubmitterSetupID">The submitter setup identifier.</param>
        /// <returns></returns>
        private string LoadSubmitterSetup(string fieldsJSON, Int64 SubmitterSetupID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Submitter Setup", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminEDIObj.LoadSubmitterSetup(SubmitterSetupID, null, null, null, null);
                    else
                        obj = BLLAdminEDIObj.LoadSubmitterSetup(SubmitterSetupID, SearchedfieldsJSON["txtOrganizzationLastName"], SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlIsActice"], pageNo, rpp);

                    dsEDI = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            SubmitterSetupCount = dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDI.SubmitterSetup.Rows.Count > 0) ? dsEDI.SubmitterSetup.Rows[0][dsEDI.SubmitterSetup.RecordCountColumn.ColumnName] : 0,
                            SubmitterSetupLoad_JSON = MDVUtility.JSON_DataTable(dsEDI.Tables[dsEDI.SubmitterSetup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SubmitterSetupCount = 0,
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
        /// Handle the Submitter Setup Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_SUBMITTER_SETUP":
                    {
                        string fieldsJSON = context.Request["SubmitterSetupData"];
                        Int64 SubmitterSetupID = MDVUtility.ToInt64(context.Request["SubmitterSetupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadSubmitterSetup(fieldsJSON, SubmitterSetupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}