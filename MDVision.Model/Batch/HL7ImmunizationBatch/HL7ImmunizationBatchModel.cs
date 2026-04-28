using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Batch.HL7ImmunizationBatch
{
    public class HL7ImmunizationBatchModel
    {
        public string HL7BatchId { get; set; }
        public string Providerid { get; set; }
        public string ProviderName { get; set; }
        public string GivenByProviderid { get; set; }
        public string GivenByProviderName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientName { get; set; }
        public string PatientFirstName { get; set; }
        public string AccountNumber { get; set; }
        public string PatientId { get; set; }
        public string Type { get; set; }
        public string Hl7MsgText { get; set; }
        public string IsCompleted { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FacilityName { get; set; }
        public string FacilityId { get; set; }
        public string SubmissionTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string RecordCount { get; set; }
        public string Records { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public string ErrorMessage { get; set; }
        public string completionDate { get; set; }
        public string VaccineHxId { get; set; }
        public string AcknowledgementCode { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string years { get; set; }
        public string VaccineScheduleId { get; set; }
        public string VaccineCategory { get; set; }
        public string VaccineCategoryId { get; set; }
        public string TabId { get; set; }
        public string VaccineHxType { get; set; }
    }
    public class HL7ImmunizationBatchMessageTypeModel
    {
        public string TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
