using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class MedicalHxModel
    {
        public string MedicalHxId { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string MedicalHxDate { get; set; }
        public string MedicalHxUnremarkable { get; set; }
        public string MedicalHxComments { get; set; }
        public string commandType { get; set; }
        public string MedicalHxType { get; set; }        
        public long NotesId { get; set; }
        public long DiseaseId { get; set; }

        public bool? IsRCPneumococcal { get; set; }
        public bool? IsRCInfluenza { get; set; }
        public DateTime? RCPneumococcalDate { get; set; }
        public DateTime? RCInfluenzaDate { get; set; }

        public string JSON { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string RequestStatus { get; set; }

        public string AddedFromMobileApp { get; set; }
        public List<MedicalHxDiseaseModel> MedicalDiseaseList { get; set; }

        public string FromClinicalSide { get; set; }
        public string FreeTextICD { get; set; }
        public string MedicalDiseaseFromDate { get; set; }
        public string MedicalDiseaseToDate { get; set; }
        public string MedicalDiseaseOnset { get; set; }
        public string MedicalDiseaseDurationLength { get; set; }
        public string MedicalDiseaseDurationPeriod { get; set; }
        public string CPTDescription { get; set; }
        public string MedicalDiseaseTestResult { get; set; }
        public string MedicalDiseaseStatus { get; set; }
        public string MedicalDiseaseLocation { get; set; }
        public string MedicalDiseaseSeverity { get; set; }
        public string MedicalDiseasePattern { get; set; }
        public string MedicalDiseaseAgggravatedBy { get; set; }
        public string MedicalDiseaseComments { get; set; }
    }
}