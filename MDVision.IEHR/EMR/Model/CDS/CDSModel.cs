using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CDS
{
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: CDS Model
    public class CDSModel
    {
        public string CDSId { get; set; }
        public string NoteId { get; set; }

        public string CDSTitle { get; set; }
        public string CDSDeveloper { get; set; }
        public string CDSFundingSource { get; set; }
        public string CDSReferenceURL { get; set; }
        public string CDSRelease { get; set; }
        public string CDSRevisionDate { get; set; }
        public string CDSTriggerLocation { get; set; }
        public string CDSUserRole { get; set; }
        public string CDSRuleType { get; set; }
        public string CDSSex { get; set; }
        public string CDSEthnicity { get; set; }
        public string CDSRace { get; set; }
        public string CDSLanguage { get; set; }
        public string CDSReminderLength { get; set; }
        public string CDSReminderPeriod { get; set; }
        public bool CDSRecursive { get; set; }
        public string CDSProblemList { get; set; }
        public string CDSAllergies { get; set; }
        public string CDSMedications { get; set; }
        public string CDSLabResults { get; set; }
        public string CDSVitals { get; set; }
        public string CDSAlertNote { get; set; }

        public string CDSInsurance { get; set; }
        public string ModifiedOn { get; set; }
        public bool CDSActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string CDSQuery { get; set; }
        public string commandType { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string PatientId { get; set; }
        public string CDSIDs { get; set; }
        public string isPopup { get; set; }
        public string CDSAgeCondition { get; set; }
        public string CDSAgeValue { get; set; }
        public string CDSAgeFrom { get; set; }
        public string CDSAgeTo { get; set; }
        public string CDSStatus { get; set; }
        public string EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<CDSMedicationModel> MedicationData { get; set; }
        public List<CDSAllergyModel> AllergyData { get; set; }
        public List<CDSProblemListModel> ProblemData { get; set; }
        public List<CDSLabResultModel> LabResultData { get; set; }
        public List<CDSVitalsModel> VitalData { get; set; }
        public List<CDSInsuranceModel> InsuranceData { get; set; }
        public bool IsVitalInsertUpdate { get; set; }
        public string OrderSetIds { get; set; }
        public List<CDSQuestionnaire> QuestionnaireData { get; set; }
        public string CDSRecursiveLength { get; set; }
        public string CDSRecursivePeriod { get; set; }
        public string QuestionnaireHTML { get; set; }
        public string CDSPatientStatusId { get; set; }
        public string ProviderIds { get; set; }
        public string PrivilegeData { get; set; }
        public bool IsLoadCDS { get; set; }
    }

    //Start//06-03-2016//Ahmad Raza//Addded classes to get Race and Ethnicity Names
    public class Ethnicity
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
    public class Race
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
    //End//06-03-2016//Ahmad Raza//Addded classes to get Race and Ethnicity Names
    public class CDSQuestionnaire
    {
        public long CDSQuestionnaireId { get; set; }
        public string CDSId { get; set; }
        public string Description { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string QuestionnaireControlTypeId { get; set; }
        public string dropDownValues { get; set; }
    }
}