using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Immunization
{
    public class ImmunizationQueryResponseModel
    {
        public string ImmunizationQueryResponseId { get; set; }
        public string File { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string MothersMaidenName { get; set; }
        public string Address { get; set; }
        public string Comments { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string XML { get; set; }
        public string pageNumber { get; set; }
        public string rowsPerPage { get; set; }
        public string PatientId { get; set; }
        public string RecordCount { get; set; }
        public string EvaluatedImmunizationHistoryIds { get; set; }
        public string UserId { get; set; }
        public string ResponseType { get; set; }
        public string VaccineHxId { get; set; }
        public string AcknowledgementCode { get; set; }
        public string QueryId { get; set; }
    }
    public class EvaluatedImmunizationHistoryModel
    {
        public string EvaluatedImmunizationHistoryId { get; set; }
        public string ImmunizationQueryResponseId { get; set; }
        public string VaccineGroupId { get; set; }
        public string VaccineId { get; set; }
        public string AdministrationDate { get; set; }
        public string ValidDose { get; set; }
        public string ValidityReason { get; set; }
        public string CompletionStatus { get; set; }
        public string Reaction { get; set; }
        public string Comments { get; set; }
        public string VaccineDescription { get; set; }
        public string VaccineCVX { get; set; }
        public string VaccineGroupDescription { get; set; }
        public string VaccineGroupCVX { get; set; }
        public string AlreadyExists { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string RecordCount { get; set; }
        public string Dose { get; set; }
    }

    public class ImmunizationForecastModel
    {
        public string ImmunizationForecastId { get; set; }
        public string VaccineGroupDescription { get; set; }
        public string VaccineGroupCVX { get; set; }
        public string DueDate { get; set; }
        public string EarliestDateToGive { get; set; }
        public string LatestDateToGive { get; set; }
        public string Comments { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string RecordCount { get; set; }
    }
   
}
