using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Batch.PatientList
{
    public class BatchPatientListModel
    {
        public long PatientId { get; set; }
        public string AccountNumber { get; set; }
        public string PatientFullName { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOB { get; set; }
        public string SmokingStatus { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string Language { get; set; }
        public string Communication { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ProblemName { get; set; }
        public System.DateTime ProblemDate { get; set; }
        public string Medication { get; set; }
        public System.DateTime MedicationDate { get; set; }
        public string Allergy { get; set; }
        public System.DateTime AllergyDate { get; set; }
        public string LabResults { get; set; }
        public System.DateTime LabResultsDate { get; set; }
        public long RecordCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class BatchPatientListModelSearch
    {
        public string commandType { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public string ageFrom { get; set; }
        public string ageTo { get; set; }
        public string gender { get; set; }
        public string SmokingStatusId { get; set; }
        public string RaceId { get; set; }
        public string EthnicityId { get; set; }
        public string PrefLanguageId { get; set; }
        public string PrefCommunicationId { get; set; }
        public string Pt_CreationFrom { get; set; }
        public string Pt_CreationTo { get; set; }
        public string Problems { get; set; }
        public string ProblemsFrom { get; set; }
        public string ProblemsTo { get; set; }
        public string Medications { get; set; }
        public string MedicationsFrom { get; set; }
        public string MedicationsTo { get; set; }
        public string Allergies { get; set; }
        public string AllergiesFrom { get; set; }
        public string AllergiesTo { get; set; }
        public string LabResults { get; set; }
        public string LabResultsFrom { get; set; }
        public string LabResultsTo { get; set; }
        public long EntityId { get; set; }
    }
}