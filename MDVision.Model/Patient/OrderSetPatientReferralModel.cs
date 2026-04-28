using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
    public class OrderSetPatientReferralModel
    {
        public string OrderSetReferralId { get; set; }
        public string OrderSetId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string AssigneeId { get; set; }
        public string RefProviderId { get; set; }
        public string PAN { get; set; }
        public string Visits { get; set; }
        public string VisitType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string SoapText { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string commandType { get; set; }
        public string Type { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CPTCodeDescription { get; set; }
        public string LoadFor { get; set; }
        public string IsActiveRecord { get; set; }
        public string ReferralProcedureId { get; set; }
        public string Provider { get; set; }
        public string ProviderName { get; set; }
        public string RefProvider { get; set; }
        public string Assignee { get; set; }
        public string PatientInsurance { get; set; }
        public string FacilityFrom { get; set; }
        public string FacilityTo { get; set; }
        public string VisitsUsed { get; set; }
        public string PatientInsuranceName { get; set; }
        public string FacilityFromName { get; set; }
        public string FacilityToName { get; set; }
        public string RefferalTo { get; set; }
        public string RefferalFrom { get; set; }
        public string Comments { get; set; }
        public string SpecialtyFrom { get; set; }
        public string SpecialtyFromName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }
        public List<ReferralProcedureModel> ReferralProcedure { get; set; }
        public string XML { get; set; }
        public string OrderSetReferralProcedureId { get; set; }
        public string Procedures { get; set; }
    }

    public class OrderSetPatientReferralResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string OrderSetReferralId { get; set; }
    }
    public class ReferralProcedureModel
    {
        public string ReferralProcedureId { get; set; }
        public string ReferralId { get; set; }
        public string Procedure { get; set; }
        public string Urgency { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string ProcedureIds { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string ShowCPTCode { get; set; }
        public string OrderSetReferralProcedureId { get; set; }
        public string OrderSetReferralId { get; set; }
    }
}
