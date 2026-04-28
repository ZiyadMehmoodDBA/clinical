using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient.Encounter
{
    public class Encounter_ClaimSummary
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Encounter_ClaimSummary()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
            BLLBillingObj = new BLLBilling();
            BLLVisitsObj = new BLLVisits();
        }
        #region Singleton
        private static Encounter_ClaimSummary _obj = null;
        public static Encounter_ClaimSummary Instance()
        {
            if (_obj == null)
                _obj = new Encounter_ClaimSummary();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadClaimSummary(Int64 VisitID)
        {
            DSClaimSummary dsSummary = null;
            BLObject<DSClaimSummary> ds = new BLObject<DSClaimSummary>();
            try
            {
                 ds = BLLBillingObj.LoadClaimSummary(VisitID);
                 dsSummary = ds.Data;
                var response = new
                {
                    status = false,
                    PatientDetail_JSON = MDVUtility.JSON_DataTable(dsSummary.Tables[dsSummary.PatientDetail.TableName]),
                    ClaimDetail_JSON = MDVUtility.JSON_DataTable(dsSummary.Tables[dsSummary.ClaimDetail.TableName]),
                    ICDDetail_JSON = MDVUtility.JSON_DataTable(dsSummary.Tables[dsSummary.ICDDetail.TableName]),
                    CPTDetail_JSON = MDVUtility.JSON_DataTable(dsSummary.Tables[dsSummary.CPTDescription.TableName]),
                    InsuranceDetail_JSON = MDVUtility.JSON_DataTable(dsSummary.Tables[dsSummary.InsuranceDetail.TableName]),
                    PaymentDetail_JSON = MDVUtility.JSON_DataTable(dsSummary.Tables[dsSummary.PaymentDetail.TableName])
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Handle the Employer Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "GET_CLAIM_SUMMARY":
                    {
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        string strJSONData = LoadClaimSummary(VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

    }

        #endregion
}