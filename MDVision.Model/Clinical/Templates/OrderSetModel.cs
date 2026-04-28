using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model
{
    public class OrderSetModel
    {

        public string OrderSetId { get; set; }
        public string OrderSetIds { get; set; }
        public string OrderSetName { get; set; }
        public string commandType { get; set; }
        public bool IsProviderAll { get; set; }
        public bool IsSpecialtyAll { get; set; }
        public string ProviderNames { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyNames { get; set; }
        public string SpecialtyIds { get; set; }
        public string RecordCount { get; set; }
        public string IsActive { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public long? EntityId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedOn { get; set; }
        public string Comments { get; set; }
        public string CDSId { get; set; }

        public string NotesId { get; set; }

        public string PatientId { get; set; }
        public string OrderSetComponents { get; set; }
        public bool AddInValidAgeRecordsInHxTab { get; set; }


        public string ProblemListIDs { get; set; }
        public string ProceduresIDs { get; set; }
        public string LabOrderIDs { get; set; }
        public string RadiologyOrderIDs { get; set; }
        public string FollowUpIDs { get; set; }
        public string PatientEducationIDs { get; set; }
        public string ReferralsIDs { get; set; }
        public string ImmunizationIDs { get; set; }
        public string TherapeuticIDs { get; set; }
        public string MedicationIDs { get; set; }
        public string ProcedureOrderIDs { get; set; }
        public List<OrderSetProblemModel> AssociatedProblemData { get; set; }
        public string OrderSetProblemXML { get; set; }
        public string PatientProblemIds { get; set; }
        public string OrderSetAssociatedProblemIds { get; set; }
        public string ProviderId { get; set; }
        public string DefaultFollowUpId { get; set; }
    }

    public class OrderSetResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string OrderSetId { get; set; }
    }

    public class OrderSetProblemModel
    {
        public long OrderSetProblemId { get; set; }
        public string OrderSetId { get; set; }
        public string Problem { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string OrderSetProblemQuery { get; set; }
        public string ProblemOperator { get; set; }
        public string SnomedCode { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string SnomedId { get; set; }
        public string SnomedDescription { get; set; }
        public string Icd9Description { get; set; }
        public string Icd10Description { get; set; }
        public string PatientProblem { get; set; }
        public string ProblemId { get; set; }
        
    }
}