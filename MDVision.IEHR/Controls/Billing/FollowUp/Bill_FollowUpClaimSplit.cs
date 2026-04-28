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
    public class Bill_FollowUpClaimSplit
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_FollowUpClaimSplit()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_FollowUpClaimSplit _obj = null;
        public static Bill_FollowUpClaimSplit Instance()
        {
            if (_obj == null)
                _obj = new Bill_FollowUpClaimSplit();
            return _obj;
        }
        #endregion

        #region Private Functions


        private string SplitClaimCharges(Int64 VisitID, string ChargeIDs)
        {

            try
            {
                if (VisitID <= 0 || ChargeIDs == "")
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Split_Claim_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.SplitInsuranceARClaim(VisitID, ChargeIDs);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Split_Claim_Success_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
                case "SPLIT_CLAIM_CHARGES":
                    {
                        string fieldsJSON = context.Request["CLaimChargeData"];
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        string ChargeIDs = context.Request["ChargeIDs"];
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);

                        string strJSONData = SplitClaimCharges(VisitID, ChargeIDs);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}