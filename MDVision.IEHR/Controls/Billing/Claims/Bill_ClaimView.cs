using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing.Claims
{
    public class Bill_ClaimView
    {

        private BLLBilling BLLBillingObj = null;
        public Bill_ClaimView()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_ClaimView _obj = null;
        public static Bill_ClaimView Instance()
        {
            if (_obj == null)
                _obj = new Bill_ClaimView();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string LoadClaimsViewDetail(Int64 BatchNumber, Int64 BatchId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSBatchCharge dsClaim = null;
                BLObject<DSBatchCharge> obj;
                obj = BLLBillingObj.LoadBatchClaimDetail(BatchId);
                dsClaim = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ClaimDetailCount = dsClaim.Tables[dsClaim.BatchClaimCharge.TableName].Rows.Count > 0 ? dsClaim.Tables[dsClaim.BatchClaimCharge.TableName].Rows.Count : 0,
                        ClaimDetail_JSON = MDVUtility.JSON_DataTable(dsClaim.Tables[dsClaim.BatchClaimCharge.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ClaimDetailCount = 0,
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
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_CLAIMVIEW_DETAIL":
                    {
                        string BatchNumber = MDVUtility.ToStr(context.Request["BatchNumber"]);
                        string BatchId = MDVUtility.ToStr(context.Request["BatchId"]);
                        string strJSONData = LoadClaimsViewDetail(MDVUtility.ToInt64(BatchNumber), MDVUtility.ToInt64(BatchId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}