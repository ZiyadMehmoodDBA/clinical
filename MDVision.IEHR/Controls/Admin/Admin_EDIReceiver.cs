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
    public class Admin_EDIReceiver
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIReceiver()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIReceiver _instance = null;
        public static Admin_EDIReceiver Instance()
        {
            if (_instance == null)
                _instance = new Admin_EDIReceiver();
            return _instance;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the receiver setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ReceiverSetupID">The receiver setup identifier.</param>
        /// <returns></returns>
        private string LoadReceiverSetup(string fieldsJSON, Int64 ReceiverSetupID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "SEARCH")).ToString();

                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminEDIObj.LoadReceiverSetup(ReceiverSetupID, null, null, null, null, null);
                    else
                        obj = BLLAdminEDIObj.LoadReceiverSetup(ReceiverSetupID, SearchedfieldsJSON["txtSubmitterId"], SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["ddlClearingHouse"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);

                    dsEDI = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ReceiverSetupCount = dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDI.EDIReceiverSetup.Rows.Count > 0) ? dsEDI.EDIReceiverSetup.Rows[0][dsEDI.EDIReceiverSetup.RecordCountColumn.ColumnName] : 0,
                            ReceiverSetupLoad_JSON = MDVUtility.JSON_DataTable(dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ReceiverSetupCount = 0,
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
        /// Handle the Receiver Setup Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_RECEIVER_SETUP":
                    {
                        string fieldsJSON = context.Request["ReceiverSetupData"];
                        Int64 ReceiverSetupID = MDVUtility.ToInt64(context.Request["ReceiverSetupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadReceiverSetup(fieldsJSON, ReceiverSetupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}