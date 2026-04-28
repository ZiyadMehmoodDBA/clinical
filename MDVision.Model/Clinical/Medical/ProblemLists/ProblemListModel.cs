using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical.ProblemLists
{
    public class ProblemListModel
    {
        public string ProblemListId { get; set; }
        public string ProviderIDs { get; set; }
        public string ProviderId { get; set; }
        public string EntityId { get; set; }
        public string ProblemName { get; set; }
        public string Description { get; set; }
        public string ChronicityLevel { get; set; }
        public string Severity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string commandType { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string IsActiveRecord { get; set; }
        public string IsActive { get; set; }
        public string VisitId { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string ICD9_Description { get; set; }
        public string ICD10_Description { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string IsChiefComplaint { get; set; }
        public string UserId { get; set; }
        public string FromElementId { get; set; }
        public string ToElementId { get; set; }
        public string IsActiveGrid { get; set; }
        public string ProblemOrder { get; set; }
        public string CustomFormId { get; set; }
        public string CheckProblemExists { get; set; }
        public string UpdateFavValues { get; set; }
        public string FavListNames { get; set; }
        public string FavListName { get; set; }
        public string FavListVal { get; set; }
        public string ComplaintId { get; set; }
        public string ComplaintDetailId { get; set; }
        public string Diagnosis { get; set; }
        public string DiagnosisDate { get; set; }
      
        public string PrimarySite { get; set; }
        public string PrimarySiteId { get; set; }
      
        public string HistologicType { get; set; }
        public string HistologicTypeId { get; set; }
      
       
        public string NKOClinical { get; set; }
        public string ClinicalDiagnosisDate { get; set; }
       
      
        public string NKOPathologic { get; set; }
        public string EffectiveDate { get; set; }
        public string DiagnosisConfirmation { get; set; }

        public string Laterality { get; set; }

        public string Behavior { get; set; }
        public string Grade { get; set; }
        public string ClinicalStageGroup { get; set; }
        public string ClinicalStageDescriptor { get; set; }
        public string PrimaryClinicalTumor { get; set; }
        public string RLNC { get; set; }
        public string DistanceMestastatases { get; set; }
        public string StagerClinicalCancer { get; set; }
        public string PathologicStageGroup { get; set; }
        public string PathologicStageDescriptor { get; set; }
        public string PrimaryTumorPathologic { get; set; }
        public string RLNP { get; set; }
        public string DistanceMestastatasesPathologic { get; set; }
        public string StagerPathologicCancer { get; set; }
        public string Code { get; set; }
        public string CodeType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 FacilityId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string NegationIndex { get; set; }
        public string NegationReason { get; set; }
        public string Status { get; set; }

        public string ProblemDetailId { get; set; }

        public bool IsNonDiabetic { get; set; } = false;

        public bool IsDiabeticScreening { get; set; } = false;

    }
    
}
