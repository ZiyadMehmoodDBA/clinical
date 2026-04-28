using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing.FollowUp
{
    public class Bill_FollowUpARHistory
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_FollowUpARHistory()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_FollowUpARHistory _obj = null;
        public static Bill_FollowUpARHistory Instance()
        {
            if (_obj == null)
                _obj = new Bill_FollowUpARHistory();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string LoadFollowUpARHistory(string FollowUpARType, Int64 FollowUpARID, Int32 PageNumber, Int32 RowsPerPage, string VisitId)
        {

            try
            {
                DataSet dsFollowUp = null;
                BLObject<DataSet> obj = BLLBillingObj.LoadInsuranceARActivityLogs("", "", "", "", "", "", VisitId, null, "FollowUpARDetail");
                dsFollowUp = obj.Data;

                if (obj.Data != null && obj.Data.Tables.Count > 0)
                {

                    var response = new
                    {
                        status = true,
                        GroupHistoryCount = dsFollowUp.Tables["GroupHistory"].Rows.Count,
                        GroupHistory_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables["GroupHistory"]),
                        ActionHistoryCount = dsFollowUp.Tables["ActionHistory"].Rows.Count,
                        ActionHistory_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables["ActionHistory"]),
                        ReasonHistoryCount = dsFollowUp.Tables["ReasonHistory"].Rows.Count,
                        ReasonHistory_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables["ReasonHistory"]),
                        RemitCodeHistoryCount = dsFollowUp.Tables["RemitCodeHistory"].Rows.Count,
                        RemitCodeHistory_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables["RemitCodeHistory"])
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ARVisitCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Patient Message Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_FOLLOWUP_HISTORY":
                    {
                        // string fieldsJSON = context.Request["CLaimChargeData"];
                        string FollowUpARType = MDVUtility.ToStr(context.Request["FollowUpARType"]);
                        Int64 FollowUpARID = MDVUtility.ToInt64(context.Request["FollowUpARID"]);
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string VisitId = MDVUtility.ToStr(context.Request["VisitId"]);

                        string strJSONData = LoadFollowUpARHistory(FollowUpARType, FollowUpARID, PageNumber, RowsPerPage, VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}