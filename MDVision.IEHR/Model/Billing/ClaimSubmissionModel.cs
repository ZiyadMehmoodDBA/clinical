using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Billing
{
    public class ClaimSubmissionModel
    {
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProviderName { get; set; }
        public string ChargeBatchNumber { get; set; }
        public string FacilityName { get; set; }
        public string PracticeName { get; set; }
        public string InsurancePlanName { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string ClaimNumber { get; set; }
        public string InsurancePlanId { get; set; }
        public string FacilityId { get; set; }
        public string ProviderId { get; set; }
        public string PracticeId { get; set; }
        public string ChargeBatchId { get; set; }
        public string PatientId { get; set; }
        public string VisitId { get; set; }
        public string ClearingHouse { get; set; }
        public string ClearingHouseName { get; set; }
        public string BillerId { get; set; }
        public string BillerName { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string SubmissionMode { get; set; }
        public string ClaimTypeId { get; set; }
        public string ClaimTypeName { get; set; }
        public string BillerTypeId { get; set; }
        public string BillerTypeName { get; set; }
        public string PageNo { get; set; }
        public string rpp { get; set; }
        public string CommandType { get; set; }
        public string batchNumber { get; set; }
        public string SubmittedDate { get; set; }
        public string SubmittedBy { get; set; }
        public string BatchStatus { get; set; }
        public string Completed { get; set; }
        public string _837BatchId { get; set; }
        public string Visits{ get; set; }
        public string isSubmit { get; set; }
        public string MarkSubmitted { get; set; }
        public string UserBrowser { get; set; }
        public string ViewOnly { get; set; }

        public string ClaimErroredId { get; set; }
        public string SubmissionErrorId { get; set; }

        public string submitUserId { get; set; }
        public bool IsLoad { get; set; }



    }
}