using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.ERA
{
   public class EOBManualPaymentPost
    {
        public string EOBManualPostingId { get; set; }
       
        public string VisitInsuranceId { get; set; }
        public string PatientId { get; set; }
        public string VisitId { get; set; }
        public string DateOfService { get; set; }
        public string ChargeCapId { get; set; }
        public string CPTCode { get; set; }
        public string BilledAmount { get; set; }
        public string ExpectedAmount { get; set; }
        public string InsCharges { get; set; }
        public string AllowedAmount { get; set; } 
        public string PaidAmount { get; set; } 
        public string WriteOffAmount { get; set; } 
        public string Deducables { get; set; } 
        public string CoInsAmount { get; set; } 
        public string EOBCopay { get; set; } 
        public string ChargeCopay { get; set; }
        public string PatientResp { get; set; }
        public string NextResponsibilityId_text { get; set; }
        public string NextResponsibilityId { get; set; } 
       
      
        public string AdjustmentGroupCodeId { get; set; } 
        public string AdjustmentReasonCodeId { get; set; } 
        public string RemarkCode { get; set; }
        public string Posted { get; set; } 
        public string CrossOver { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string InsurancePlanId_text { get; set; }
        public string AdjustmentReasonCodeId_text { get; set; }
        public string AdjustmentReasonCodeId_RefValue { get; set; }
        public string AdjustmentGroupCodeId_RefValue { get; set; }
        public string AdjustmentGroupCodeId_text { get; set; }
        public string RemarkCode_text { get; set; }
        public string RemarkCode_RefValue { get; set; }
        public string Posted_text { get; set; }
        public string CrossOver_text { get; set; }
        public string PatientName { get; set; }
        public string CheckNumber { get; set; }
        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string InsurancePlanId { get; set; }

    }
}
