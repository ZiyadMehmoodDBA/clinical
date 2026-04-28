using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Billing.Charges
{
    public class ChargeModel
    {
        public string CommandType { get; set; }
        public string PatientAccount { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Facility { get; set; }
        public string Provider { get; set; }
        public string ClaimNumber { get; set; }
        public string IncludeSecondaryClaim { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string CreatedBy { get; set; }
        public string InsurancePlan { get; set; }
        public string ResourceProvider { get; set; }
        public string Paid { get; set; }
        public string ClaimType { get; set; }
        public string SubmitStatus { get; set; }
        public string ChargeStatus { get; set; }
        public string DateClaimCreatedFrom { get; set; }
        public string DateClaimCreatedTo { get; set; }
        public string EncounterSignOffDate { get; set; }
        public string ImportedOnFrom { get; set; }
        public string ImportedOnTo { get; set; }
        public string InsurancePlanId { get; set; }
        public string FacilityId { get; set; }
        public string ProviderId { get; set; }
        public string PatientId { get; set; }
        public string ResourceProviderId { get; set; }
        public string ChargeCapId { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string CreatedBy_Text { get; set; }
        public string Paid_Text { get; set; }
        public string SubmittedDateTo { get; set; }
        public string SubmittedDateFrom { get; set; }
        public string Hold { get; set; }
        public string Hold_Text { get; set; }
        public string CPTCode { get; set; }
        public string IncludeVoidedClaims { get; set; }
        public string ClaimErroredId { get; set; }
        public string EncounterTypeId { get; set; }
        public string EncounterTypeName { get; set; }
    }
}