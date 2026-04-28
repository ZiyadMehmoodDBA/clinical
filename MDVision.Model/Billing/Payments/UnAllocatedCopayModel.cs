using System;
namespace MDVision.Model.Billing.Payments
{
    public class UnAllocatedCopayModel
    {
        public string UnAllocatedCopayId { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptDate { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string FacilityId { get; set; }
        public string AppointmentId { get; set; }
        public string CopayAmount { get; set; }
        public string CardTypeId { get; set; }
        public string CheckNo { get; set; }
        public string CheckDate { get; set; }
        public string ExpiryDate { get; set; }
        public string AdvPmtId { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string PmtTypeId { get; set; }
        public string LedgerAccId { get; set; }
        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PracticeName { get; set; }
        public string PracticeAddress { get; set; }
        public string PracticeCity { get; set; }
        public string PracticePhoneNo { get; set; }
        public string PracticeFax { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDate { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityPhoneNo { get; set; }
        public string FacilityFax { get; set; }
        public string PaymentType { get; set; }
        public string PaidAccountType { get; set; }
        public string ProviderName { get; set; }
        public DateTime? ReceiptDateFrom { get; set; }
        public DateTime? ReceiptDateTo { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }

    }
}
