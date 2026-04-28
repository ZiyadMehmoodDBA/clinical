using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Model.Billing.Charges;
using MDVision.IEHR.Model.Billing.ERA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.ERA
{
    public class Bill_ERA_Charge
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_ERA_Charge()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_ERA_Charge _obj = null;
        public static Bill_ERA_Charge Instance()
        {
            if (_obj == null)
                _obj = new Bill_ERA_Charge();
            return _obj;
        }
        #endregion
        #region ChargeModelSearch
        public string SearchCharge(ChargeModel model)
        {
            string ClaimCreatedBy = null;
            string ClaimType = null;
            bool IncludeSecondaryClaim = true;
            bool IncludeVoidedClaims = false;
            try
            {
                if (string.IsNullOrEmpty(model.Facility))
                    model.FacilityId = "0";

                if (string.IsNullOrEmpty(model.Provider))
                    model.ProviderId = "0";

                if (string.IsNullOrEmpty(model.ResourceProvider))
                    model.ResourceProviderId = "0";

                if (string.IsNullOrEmpty(model.InsurancePlan))
                    model.InsurancePlanId = "0";

                IncludeSecondaryClaim = Convert.ToBoolean(model.IncludeSecondaryClaim);
                IncludeVoidedClaims = Convert.ToBoolean(model.IncludeVoidedClaims);

                if (model.Hold_Text == "All")
                {
                    model.Hold = null;
                }

                if (model.CreatedBy_Text == "- Select -")
                    ClaimCreatedBy = null;
                else
                    ClaimCreatedBy = model.CreatedBy;

                if (model.Paid_Text == "All")
                {
                    model.Paid = null;
                }

                DateTime? dtfrom = String.IsNullOrEmpty(model.DOSFrom) ? (DateTime?)null : DateTime.Parse(model.DOSFrom);
                DateTime? dtto = String.IsNullOrEmpty(model.DOSTo) ? (DateTime?)null : DateTime.Parse(model.DOSTo);

                DateTime? dtSubmittedTo = String.IsNullOrEmpty(model.SubmittedDateTo) ? (DateTime?)null : DateTime.Parse(model.SubmittedDateTo);
                DateTime? dtSubmittedFrom = String.IsNullOrEmpty(model.SubmittedDateFrom) ? (DateTime?)null : DateTime.Parse(model.SubmittedDateFrom);

                DateTime? ImportedDateFrom = null;
                DateTime? ImportedDateTo = null;

                DateTime? ClaimCreatedFrom = String.IsNullOrEmpty(model.DateClaimCreatedFrom) ? (DateTime?)null : DateTime.Parse(model.DateClaimCreatedFrom);
                DateTime? ClaimCreatedTo = String.IsNullOrEmpty(model.DateClaimCreatedTo) ? (DateTime?)null : DateTime.Parse(model.DateClaimCreatedTo);
                DateTime? EncounterSignOffDate = String.IsNullOrEmpty(model.EncounterSignOffDate) ? (DateTime?)null : DateTime.Parse(model.EncounterSignOffDate);


                if (dtto == null)
                    dtto = dtfrom;
                if (dtfrom == null)
                    dtfrom = dtto;

                ClaimType = !string.IsNullOrEmpty(model.ClaimType) ? MDVUtility.ToStr(model.ClaimType) : "";
                model.CPTCode = !string.IsNullOrEmpty(model.CPTCode) ? MDVUtility.ToStr(model.CPTCode) : null;

                ImportedDateFrom = String.IsNullOrEmpty(model.ImportedOnFrom) ? (DateTime?)null : DateTime.Parse(model.ImportedOnFrom);

                ImportedDateTo = String.IsNullOrEmpty(model.ImportedOnTo) ? (DateTime?)null : DateTime.Parse(model.ImportedOnTo);

                DSCharge dsCharge = null;
                BLObject<DSCharge> obj;

                if (model == null)
                    obj = BLLBillingObj.LoadPatientCharges(MDVUtility.ToInt64(model.ChargeCapId), "", "", 0, 0, "", 0, "", "", null, null, 0, 1, "");
                else
                    obj = BLLBillingObj.LoadPatientCharges(MDVUtility.ToInt64(model.ChargeCapId), model.LastName, model.FirstName, MDVUtility.ToInt64(model.FacilityId), MDVUtility.ToInt64(model.ProviderId), model.ChargeStatus, MDVUtility.ToInt64(model.InsurancePlanId), model.ClaimNumber, model.Paid, dtfrom, dtto, 0, 1, model.PatientAccount, 0, MDVUtility.ToInt16(model.PageNumber), MDVUtility.ToInt16(model.RowsPerPage), model.CPTCode, model.ClaimType, null, IncludeVoidedClaims);

                if (obj.Data != null)
                {
                    dsCharge = obj.Data;

                    if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            BillChargeCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCharge.PatientCharges.Rows[0][dsCharge.PatientCharges.RecordCountColumn.ColumnName],
                            BillChargeLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            BillChargeCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        BillChargeCount = 0,
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


    }
}