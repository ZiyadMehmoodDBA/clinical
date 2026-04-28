/* Author:  Muhammad Arshad
 * Created Date: 31/03/2016
 * OverView: Created for Clinical Summary in Clinical Module
 */

using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Common;
using System.Data.SqlClient;
using MDVision.Model.Clinical.Medical.CarePlan;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALClinicalSummary
    {
        #region Variable

        #endregion

        #region Constructors

        public DALClinicalSummary()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALClinicalSummary(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Stored Procedure Names
        private const string PROC_GET_LATEST_REFERRAL_BY_PATIENTI = "Clinical.sp_CCDAGetLatestReferralByPatientId";
        private const string PROC_ALLERGY_INSERT = "Clinical.sp_CCDAAllergyInsert";
        private const string PROC_PATIENT_INSERT = "Patient.sp_PatientsInsert";
        private const string GET_ALLERGY_BY_RCOPIAID = "Clinical.sp_GetAllergyAgainstRcopiaID";
        private const string PROC_PLAN_OF_CARE_INSERT = "Clinical.sp_PlanofCareInsert";
        private const string PROC_PLAN_OF_CARE_DELETE = "Clinical.sp_PlanofCareDelete";
        private const string PROC_PLAN_OF_CARE_SELECT = "Clinical.sp_PlanofCareSelect";
        private const string PROC_PLAN_OF_CARE_UPDATE = "Clinical.sp_PlanofCareUpdate";
        private const string PROC_PLAN_OF_CARE_GOAL_SELECT = "Clinical.sp_PlanofCare_GoalSelect";
        private const string PROC_UPDATE_SOAPTEXT_FOR_PLAN_OF_CARE = "Clinical.sp_UpdateSoapTextForPlanofCare";
        private const string PROC_PLAN_OF_CARE_GOAL_INSERT = "Clinical.sp_PlanofCare_GoalInsert";
        private const string PROC_PLAN_OF_CARE_GOAL_UPDATE = "Clinical.sp_PlanofCare_GoalUpdate";

        private const string PROC_ATTACH_PLAN_OF_CARE_FROM_NOTES = "Clinical.";
        private const string PROC_DETACH_PLAN_OF_CARE_FROM_NOTES = "Clinical.";
        private const string PROC_PLAN_OF_CARE_GOAL_DELETE = "Clinical.sp_PlanofCare_GoalDelete";

        //Start 27-04-2016 Muhammad Arshad Cognitive
        private const string PROC_COGNITIVE_INSERT = "Clinical.sp_CognitiveInsert";
        private const string PROC_COGNITIVE_DELETE = "Clinical.sp_CognitiveDelete";
        private const string PROC_DEATTATCH_COGNITIVE_STATUS_FROM_NOTE = "Clinical.sp_DeAttatchCognitiveStatusFromNote";
        private const string PROC_ATTATCH_COGNITIVE_STATUS_TO_NOTE = "Clinical.sp_AttatchCognitiveStatusToNote";
        private const string PROC_COGNITIVE_SELECT = "Clinical.sp_CognitiveSelect";
        private const string PROC_COGNITIVE_UPDATE = "Clinical.sp_CognitiveUpdate";

        private const string PROC_UPDATE_SOAPTEXT_FOR_COGNITIVE = "Clinical.sp_UpdateSoapTextForCognitive";

        private const string PROC_COGNITIVE_STATUS_INSERT = "Clinical.sp_Cognitive_StatusInsert";
        private const string PROC_COGNITIVE_STATUS_UPDATE = "Clinical.sp_Cognitive_StatusUpdate";
        private const string PROC_COGNITIVE_STATUS_DELETE = "Clinical.sp_Cognitive_StatusDelete";
        private const string PROC_COGNITIVE_STATUS_SELECT = "Clinical.sp_Cognitive_StatusSelect";

        private const string PROC_COGNITIVE_FUNCTIONALSTATUS_INSERT = "Clinical.sp_Cognitive_FunctionalStatusInsert";
        private const string PROC_COGNITIVE_FUNCTIONALSTATUS_UPDATE = "Clinical.sp_Cognitive_FunctionalStatusUpdate";
        private const string PROC_COGNITIVE_FUNCTIONALSTATUS_DELETE = "Clinical.sp_Cognitive_FunctionalStatusDelete";
        private const string PROC_COGNITIVE_FUNCTIONALSTATUS_SELECT = "Clinical.sp_Cognitive_FunctionalStatusSelect";

        private const string PROC_COGNITIVE_MentalSTATUS_SELECT = "Clinical.sp_Cognitive_MentalStatusSelect";
        private const string PROC_COGNITIVE_MentalSTATUS_INSERT = "Clinical.sp_Cognitive_MentalStatusInsert";
        private const string PROC_COGNITIVE_MentalSTATUS_UPDATE = "Clinical.sp_Cognitive_MentalStatusUpdate";
        private const string PROC_COGNITIVE_MentalSTATUS_DELETE = "Clinical.sp_Cognitive_MentalStatusDelete";

        private const string PROC_MentalSTATUS_SELECT_CCDA = "Clinical.sp_MentalStatusSelect_CCDA";

        private const string PROC_ATTACH_COGNITIVE_FROM_NOTES = "Clinical.";
        private const string PROC_DETACH_COGNITIVE_FROM_NOTES = "Clinical.";


        private const string GET_MEDICATION_BY_RCOPIAID = "Clinical.sp_GetMedicationAgainstRcopiaID";

        private const string PROC_COGNITIVES_STATUS_SELECT = "Clinical.sp_Cognitives_StatusSelect";
        private const string PROC_COGNITIVES_FUNCTIONALSTATUS_SELECT = "Clinical.sp_Cognitives_FunctionalStatusSelect";
        private const string PROC_COGNITIVES_MentalSTATUS_SELECT = "Clinical.sp_Cognitives_MentalStatusSelect";
        //End 27-04-2016 Muhammad Arshad Cognitive

        //Start Farooq Ahmad 28-04-2016

        private const string PROC_CCDA_PATIENT_LOOKUPS = "Clinical.sp_CCDAPatientLookUps";
        private const string PROC_CCDA_GET_DRUGID_FROM_RXNORMID = "Clinical.sp_CCDAGetDrugIDFromRxnormID";
        private const string PROC_ALLERGY_SELECT = "Clinical.sp_AllergySelect";
        private const string PROC_VDTAUDITINSERT = "Patient.sp_VDTAuditInsert";
        private const string PROC_PROBLEMLIST_SELECT = "Clinical.sp_ProblemListSelect";
        private const string PROC_MEDICALHX_SELECT = "Clinical.sp_MedicalHxSelect";
        private const string PROC_SURGICALHX_SELECT = "Clinical.sp_SurgicalHxSelect";
        private const string PROC_HOSPITALIZATIONHX_SELECT = "Clinical.sp_HospitalizationHxSelect";
        private const string PROC_FAMILYHX_SELECT = "Clinical.sp_FamilyHxSelect";
        private const string PROC_BIRTHHX_SEELCT = "Clinical.sp_BirthHxSelect";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SELECT = "Clinical.sp_PatientPhysicalExamSelect";
        private const string PROC_MEDICATION_SELECT = "Clinical.sp_MedicationSelect";
        private const string PROC_PROCEDURES_SELECT = "Clinical.sp_ProceduresSelect";
        private const string PROC_ImmunizationHx_LOAD = "Clinical.sp_VaccineHxLoad";
        private const string PROC_ImmunizationHx_LOAD_Clinical_Summary = "Clinical.sp_CCDA_ImmunizationVaccineInfo";
        private const string PROC_VACCINE_LOAD = "Clinical.sp_GetVaccine";
        private const string PROC_Lab_ORDER_RESULT_DETAIL_SELECT = "Clinical.sp_LabOrderResultDetailSelect";
        private const string PROC_PATIENT_FILL = "Patient.sp_PatientFill";
        private const string PROC_Lab_ORDER_RESULT_SELECT = "Clinical.sp_LabOrderResultSelect";
        private const string PROC_CONSULTATION_ORDER_SELECT = "Clinical.sp_ConsultationOrderSelect";
        private const string PROC_MISCHX_SELECT = "Clinical.sp_SocialHx_MiscHxSelect";
        private const string PROC_SOCIALHX_SEXUALHX_SELECT = "Clinical.sp_SocialHx_SexualHxSelect";
        private const string PROC_SOCIALHX_DRUGABUSE_SELECT = "Clinical.sp_SocialHx_DrugAbuseSelect";
        private const string PROC_SOCIALHX_ALCOHOL_SELECT = "Clinical.sp_SocialHx_AlcoholSelect";
        private const string PROC_SOCIALHX_TOBACCO_SELECT = "Clinical.sp_SocialHx_TobaccoSelect";
        private const string PROC_VITALS_RESPIRATION_SELECT = "Clinical.sp_VitalSignsRespirationSelect";
        private const string PROC_VITALS_TEMPERATURE_SELECT = "Clinical.sp_VitalSignsTempratureSelect";
        private const string PROC_VITALS_PULSE_SELECT = "Clinical.sp_VitalSignsPulseSelect";
        private const string PROC_VITALS_BLOODPRESSURE_SELECT = "Clinical.sp_VitalSignsBloodPressureSelect";
        private const string PROC_VITALS_SELECT = "Clinical.sp_VitalSignsSelect";
        private const string PROC_PRACTICE_SELECT = "Provider.sp_PracticeSelect";
        private const string PROC_PROVIDER_SELECT = "Provider.sp_ProviderSelect";
        private const string PROC_MISCHX_COMPONENT_SELECT = "Clinical.sp_SocialHx_MiscHx_ComponentSelect";
        private const string PROC_COGNITIVE_COGNITIVEFUNCTIONALSTATUS_SELECT = "[Clinical].[sp_Cognitive_CognitiveFunctionalStatusSelect]";
        private const string PROC_CCDAENCOUNTERDIAGNOSTIC = "[Clinical].[sp_CCDAEncounterDiagnostic]";
        private const string PROC_CCDA_PATIENTS_CHECK_EXIST = "[Patient].[sp_CCDAPatientsCheckExist]";
        private const string PROC_MEDICATION_INSERT = "Clinical.sp_CCDAMedicationInsert";
        private const string PROC_PROBLEMLIST_INSERT = "Clinical.sp_CCDAProblemListInsert";
        private const string PROC_CCDA_COMPLAINTS_BY_NOTEID = "[Clinical].[sp_CCDAComplaintsByNoteId]";

        private const string PROC_NOTES_FUNCCOGNITIVE_SELECT = "[Clinical].[sp_NotesFuncCognitiveSelect]";
        private const string PROC_NOTES_COMPONENTS_SELECT = "[Clinical].[sp_NotesComponentsSelect]";
        private const string PROC_NOTES_PRESCRIPTION_SELECT = "[Clinical].[sp_NotesPrescriptionSelect]";
        private const string PROC_NOTES_HEADERDAT_SELECT = "[Clinical].[sp_NotesHeaderDatSelect]";
        private const string PROC_GET_PROVIDER_SIGNATURE = "[Provider].[sp_GetProviderInfoAndSignature]";
        //End Farooq Ahmad 28-04-2016
        private const string PROC_Lab_ORDER_TEST_SELECT_CCDA = "Clinical.sp_LabOrderTestSelect_CCDA";
        private const string PROC_CARETEAM_SELECT_CCDA = "CCM.sp_CareTeamSelect_CCDA";
        private const string PROC_PROBLEMLIST_SELECT_CCDA = "Clinical.sp_ProblemListSelect_CCDA";
        private const string PROC_MEDICATION_SELECT_CCDA = "Clinical.sp_MedicationSelect_CCDA";
        private const string PROC_HEALTHCONCERNS_SELECT_CCDA = "Clinical.sp_HealthConcernsSelect_CCDA";
        private const string PROC_Lab_ORDER_RESULT_DETAIL_SELECT_CCDA = "Clinical.sp_LabOrderResultDetailSelect_CCDA";
        private const string PROC_INTERVENTION_SELECT_CCDA = "Clinical.sp_InterventionsSelect_CCDA";
        private const string PROC_OUTCOMES_SELECT_CCDA = "Clinical.sp_OutcomesSelect_CCDA";
        private const string PROC_ImmunizationHx_SELECT_CCDA = "Clinical.sp_CCDA_ImmunizationVaccineInfo_CCDA";
        private const string PROC_ALLERGY_SELECT_CCDA = "Clinical.sp_AllergySelect_CCDA";
        private const string PROC_ARO_REPORT = "Clinical.sp_AROExport";
        private const string PROC_AUP_REPORT = "Clinical.sp_AUPExport";
        private const string PROC_OUTPATIENT_ENCOUNTER_SELECT = "Clinical.sp_OutPatientEncounterSeclect";
        private const string PROC_CCDA_CANCERREPORT_SELECT = "[Clinical].[sp_CCDA_CancerReportSelect]";
        private const string PROC_CCDA_FAMILYHX_SELECT = "[Clinical].[sp_CCDA_FamilyHxSelect]";
        private const string PROC_SOCIALHX_SELECT_CCDA = "Clinical.sp_SocialHxSelect_CCDA";
        private const string PROC_VITALS_SELECT_CCDA = "Clinical.sp_VitalSignsSelect_CCDA";
        private const string PROC_PROCEDURES_SELECT_CCDA = "Clinical.sp_ProceduresSelect_CCDA";
        private const string PROC_CCDA_EMPLOYMENTHISTORYSELECT = "clinical.sp_CCDA_EmploymentHistorySelect";
        private const string PROC_RADIOLOGY_ORDER_RESULT_DETAIL_SELECT_CCDA = "Clinical.sp_RadiologyOrderResultDetailSelect_CCDA";
        #endregion

        #region Parameters
        private const string PARM_PRESCRIPTIONID = "@PrescriptionID";
        private const string PARM_PREPARER_USERID = "@Preparer_UserID";
        private const string PARM_PROVIDERID = "@ProviderID";
        private const string PARM_ROUTEBY = "@Routeby";
        private const string PARM_DRUGID = "@DrugID";
        private const string PARM_DOSE_TIMING = "@DoseTiming";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_QUANTITYUNIT = "@QuantityUnit";
        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_STOPEDATE = "@StopDate";
        private const string PARM_STOPREASON = "@StopReason";
        private const string PARM_SIGCHNAGEDATE = "@SigChangedDate";
        private const string PARM_LASTMODIFIEDBY = "@LastModifiedBy";
        private const string PARM_LASTMODIFIEDDATE = "@LastModifiedDate";
        private const string PARM_INTENDEDUSE = "@IntendedUse";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_CREATEDBY = "@CreatedBy";
        private const string PARM_CREATEDON = "@CreatedOn";
        private const string PARM_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_MODIFIEDON = "@ModifiedOn";
        private const string PARAM_REFILL = "@Refill";
        private const string PARAM_FILLDATE = "@FillDate";
        private const string PARAM_ISDELETED = "@IsDeleted";

        private const string PARM_DRUGDESCRIPTION = "@DrugDescription";
        private const string PARAM_ISCURRENT = "@isCurrent";
        private const string PARAM_PRESCRIPTION_RCOPIAID = "@PrescriptionRcopiaID";
        private const string PARM_NUMBER = "@Number";
        private const string PARM_SUBSTITUTION = "@Substitution";
        private const string PARM_PATIENTNOTES = "@PatientNotes";
        private const string PARM_OTHERNOTE = "@OtherNote";
        private const string PARM_DOSE_OTHER = "@DoseOther";
        private const string PARM_ACTION = "@Action";
        private const string PARM_RCOPIA = "@RcopiaID";
        private const string PARM_DOSEUNIT = "@DoseUnit";
        private const string PARM_DOSE = "@Dose";
        private const string PARM_COMPONENT_ID = "@ComponentId";
        private const string PARM_EIN = "@EIN";
        private const string PARM_ENTITY = "@EntityId";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_PREFIX = "@Prefix";
        private const string PARM_DOD = "@DOD";
        private const string PARM_SUFFIX = "@Suffix";
        private const string PARM_CAUSE_OF_DEATH = "@CauseOfDeath";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_PREVIOUS_NAME = "@PreviousName";
        private const string PARM_DOB = "@DOB";
        private const string PARM_SSN = "@SSN";
        private const string PARM_MR_NUMBER = "@MRNumber";
        private const string PARM_MARITAL_STATUS = "@MaritialStatus";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIPCODE = "@ZIPCode";
        private const string PARM_ZIPCODE_EXT = "@ZIPCodeExt";
        private const string PARM_HOME_PHONE_NO = "@HomePhoneNo";
        private const string PARM_WORK_PHONE_NO = "@WorkPhoneNo";
        private const string PARM_WORK_PHONE_EXT = "@WorkPhoneExt";
        private const string PARM_CELL_NO = "@CellNo";
        private const string PARM_FAX_NO = "@FaxNo";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PCP_ID = "@PCPId";
        private const string PARM_GUARANTOR_ID = "@GuarantorId";
        private const string PARM_ETHNICITY_ID = "@EthnicityId";
        private const string PARM_RACE_ID = "@RaceId";
        private const string PARM_PREF_LANGUAGE_ID = "@PrefLanguageId";
        private const string PARM_SMOKING_STATUS_ID = "@SmokingStatusId";
        private const string PARM_PREF_COMMUNICATION_ID = "@PrefCommunicationId";
        private const string PARM_ADV_DIRECTIVE_ID = "@AdvanceDirectiveId";
        private const string PARM_SCHOOL_ID = "@SchoolId";
        private const string PARM_SCHOOL_STATUS = "@SchoolStatus";
        private const string PARM_BAD_ADDRESS = "@BadAddress";
        private const string PARM_PATIENT_STATEMENT = "@PatientStatement";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_SELF_PAY = "@SelfPay";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_PATIENT_PORTAL_STATUS = "@PatientPortalStatus";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_PATIENT_IMAGE = "@PatientImage";
        private const string PARM_PATIENT_STRRACEIDS = "@StrRaceIds";
        private const string PARM_PATIENT_IMAGE_TYPE = "@ImageType";
        private const string PARM_PATIENT_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_REFFERING_PROVIDER_ID = "@ReferringProviderId";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_SPECIALITY = "@SpecialtyId";
        private const string PARM_NPI = "@NPI";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_IS_PARENT = "@IsParent";
        private const string PARM_PULSE_ID = "@PulseId";
        private const string PARM_BP_ID = "@BPId";
        private const string PARM_TEMPERATURE_ID = "@TempratureId";
        private const string PARM_VITAL_ID = "@VitalSignId";
        private const string PARM_RESPIRATION_ID = "@RespirationId";
        private const string PARM_TOBACCO_ID = "@TobaccoId";
        private const string PARM_ALCOHOL_ID = "@AlcoholId";
        private const string PARM_SEXUALHX_ID = "@SexualHxId";
        private const string PARM_DRUG_ABUSE_ID = "@DrugAbuseId";
        private const string PARM_LAB_ORDER_RESULT_DETAIL_ID = "@LabOrderResultDetailId";
        private const string PARM_LAB_ORDER_RESULT_ID = "@LabOrderResultId";
        private const string PARM_LAB_ORDER_ID = "@LabOrderId";
        private const string PARM_MISCHX_ID = "@MiscHxId";
        private const string PARM_SOCIALHX_ID = "@SocialHxId";
        private const string PARM_PLAN_OF_CARE_ID = "@PlanofCareId";
        private const string PARM_PLAN_OF_CARE_PATIENT_ID = "@PatientId";
        private const string PARM_PLAN_OF_CARE_CLINICAL_INSTRUCTION = "@ClinicalInstruction";
        private const string PARM_PLAN_OF_CARE_FUTURE_INSTRUCTION = "@FutureInstruction";
        private const string PARM_PLAN_OF_CARE_PATIENT_DECISION_AID = "@PatientDecisionAid";
        private const string PARM_PLAN_OF_CARE_COMMENTS = "@Comments";
        private const string PARM_PLAN_OF_CARE_ISACTIVE = "@IsActive";
        private const string PARM_PLAN_OF_CARE_CREATED_BY = "@CreatedBy";
        private const string PARM_PLAN_OF_CARE_CREATED_ON = "@CreatedOn";
        private const string PARM_PLAN_OF_CARE_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_PLAN_OF_CARE_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PLAN_OF_CARE_SOAPTEXT = "@SoapText";
        private const string PARM_PLAN_OF_CARE_NOTES_ID = "@NoteId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PLAN_OF_CARE_GOAL = "@PlanOfCareGoal";

        private const string PARM_PLAN_OF_CARE_GOALID = "@GoalId";
        private const string PARM_ICD9_CODE = "@ICD9Code";
        private const string PARM_ICD9_CODE_DESCRIPTION = "@ICD9CodeDescription";
        private const string PARM_ICD10_CODE = "@ICD10Code";
        private const string PARM_ICD10_CODE_DESCRIPTION = "@ICD10CodeDescription";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_SNOMEDDESCRIPTION = "@SNOMED_Description";
        private const string PARM_LEXI_CODE = "@LexiCode";
        private const string PARM_LEXI_CODE_DESCRIPTION = "@LexiCodeDescription";
        private const string PARM_PLAN_OF_CARE_INSTRUCTION = "@Instruction";

        //Start 27-04-2016 Muhammad Arshad Cognitive
        private const string PARM_COGNITIVE_ID = "@CognitiveId";
        private const string PARM_COGNITIVE_PATIENT_ID = "@PatientId";
        private const string PARM_COGNITIVE_COMMENTS = "@Comments";
        private const string PARM_COGNITIVE_ISACTIVE = "@IsActive";
        private const string PARM_COGNITIVE_CREATED_BY = "@CreatedBy";
        private const string PARM_COGNITIVE_CREATED_ON = "@CreatedOn";
        private const string PARM_COGNITIVE_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_COGNITIVE_MODIFIED_ON = "@ModifiedOn";
        //End 27-04-2016 Muhammad Arshad Cognitive

        private const string PARM_COGNITIVE_STAUS_ID = "@StatusId";
        private const string PARM_COGNITIVE_SOAPTEXT = "@SoapText";
        private const string PARM_COGNITIVE_FreeTextICD = "@FreeTextICD";

        private const string PARM_COGNITIVE_INSTRUCTION = "@Instruction";

        private const string PARM_COGNITIVE_FUNCTIONAL_STAUS_ID = "@FunctionalStatusId";
        private const string PARM_COGNITIVE_Mental_STAUS_ID = "@MentalStatusId";


        //Start Farooq Ahmad 28-04-2016 Problem List
        private const string PARM_ICD9 = "@ICD9";
        private const string PARM_ICD10 = "@ICD10";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_ICD9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10_DESCRIPTION = "@ICD10_DESCRIPTION";
        private const string PARM_SURGICAL_HX_ID = "@SurgicalHxId";
        private const string PARM_MEDICAL_HX_ID = "@MedicalHxId";
        private const string PARM_PROBLEMLIST_ID = "@ProblemListId";
        private const string PARM_PROBLEM_NAME = "@ProblemName";
        private const string PARM_CHRONICITY_LEVEL = "@ChronicityLevel";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_INACTIVE_VALUE = "@InActiveChkBoxValue";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_ALLERGEN = "@Allergen";
        private const string PARM_TYPE = "@Type";
        private const string PARM_REACTION = "@Reaction";
        private const string PARM_SEVERITY = "@Severity";
        private const string PARM_ISDELETED = "@IsDeleted";
        private const string PARM_RXNORMID = "@RxnormID";
        private const string PARM_RXNORMIDTYPE = "@RxnormIDType";

        private const string PARM_ISNEWROW = "@IsNewRow";
        private const string PARM_INACTIVE_CHECKBOXVALUE = "@InActiveCheckBoxValue";
        private const string PARM_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_ONSET_DATE = "@OnSetDate";
        private const string PARM_LAST_MODIFIED = "@LastModified";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_DRUG_DESCRIPTION = "@DrugDescription";
        private const string PARM_BRAND_NAME = "@BrandName";

        private const string PARM_USER = "@User";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_IS_HISTORY = "@IsHistory";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_TEST = "@Test";
        private const string PARM_ORDER_NO = "@OrderNo";
        private const string PARM_HOSPITALIZATION_HX_ID = "@HospitalizationHxId";
        private const string PARM_FAMILY_HX_ID = "@FamilyHxId";
        private const string PARM_BIRTHHX_ID = "@BirthHxId";
        private const string PARM_PATIENT_PHYSICAL_EXAM_ID = "@PatientPhysicalExamId";
        private const string PARM_MEDICATION_ID = "@MedicationID";
        private const string PARM_PATIENTID = "@PatientID";

        private const string PARAM_VACCINE_HX_ID = "@VaccineHxId";
        private const string PARM_PROCEDURE_ID = "@ProcedureId";//kr
        private const string PARM_VACCINE_GROUP_ID = "@vaccineGroupId";
        private const string PARM_VACCINE_FOR_MODULE = "@forModule";
        private const string PARM_CONSULTATION_ORDER_ID = "@ConsultationOrderId";
        private const string PARM_PROCEDURENAME = "@ProcedureName";
        private const string PARM_ORDERNO = "@OrderNo";
        private const string PARM_ORDER_DATE_TO = "@OrderDateTo";
        private const string PARM_ORDER_DATE_FROM = "@OrderDateFrom";
        private const string PARM_STATUS = "@Status";
        private const string PARM_ORDERTO = "@OrderDateTo";
        private const string PARM_ORDERFROM = "@OrderDateFrom";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_SHOW_CFPROCEDURES = "@showCustomFormProcedure";
        private const string PARM_SHOW_VPROCEDURES = "@showVaccineProcedure";
        private const string PARM_SHOW_EM_CODES = "@ShowEmCodes";
        //End Farooq Ahmad 28-04-2016 Problem List

        //Start Farooq Ahmad 28-04-2016 Allergies

        private const string PARM_ALLERGY_ID = "@AllergyId";
        private const string PARM_RCOPIAID = "@RcopiaID";
        private const string PARM_PATIENT_MISCELLANEOUS_STATUS_ID = "@PatientMiscellaneousStatusId";
        private const string PARM_REFERENCE_DATA_ID = "@ReferenceDataId";
        private const string PARM_VALUE = "@Value";
        private const string PARM_DOCUMENTNAME = "@DocumentName";
        //End Farooq Ahmad 28-04-2016 Allergies
        private const string PARM_RADIOLOGY_ORDER_RESULT_Id = "@RadiologyOrderResultId";
        #endregion

        #region Create Parameter functions

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(56);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.PatientIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_ACCOUNT_NUMBER, ds.Patients.AccountNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ENTITY_ID, ds.Patients.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FIRST_NAME, ds.Patients.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_MI, ds.Patients.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_PREFIX, ds.Patients.PrefixColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_LAST_NAME, ds.Patients.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SUFFIX, ds.Patients.SuffixColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PREVIOUS_NAME, ds.Patients.PreviousNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_GENDER, ds.Patients.GenderColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_DOB, ds.Patients.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_DOD, ds.Patients.DODColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_CAUSE_OF_DEATH, ds.Patients.CauseOfDeathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SSN, ds.Patients.SSNColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MR_NUMBER, ds.Patients.MRNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MARITAL_STATUS, ds.Patients.MaritialStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_EMAIL_ADDRESS, ds.Patients.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ADDRESS1, ds.Patients.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ADDRESS2, ds.Patients.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CITY, ds.Patients.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_STATE, ds.Patients.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_ZIPCODE, ds.Patients.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_ZIPCODE_EXT, ds.Patients.ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_HOME_PHONE_NO, ds.Patients.HomePhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_WORK_PHONE_NO, ds.Patients.WorkPhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_WORK_PHONE_EXT, ds.Patients.WorkPhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_CELL_NO, ds.Patients.CellNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_FAX_NO, ds.Patients.FaxNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_PROVIDER_ID, ds.Patients.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(29, PARM_FACILITY_ID, ds.Patients.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(30, PARM_PRACTICE_ID, ds.Patients.PracticeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(31, PARM_REFFERING_PROVIDER_ID, ds.Patients.ReferringProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(32, PARM_PCP_ID, ds.Patients.PCPIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(33, PARM_GUARANTOR_ID, ds.Patients.GuarantorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(34, PARM_ETHNICITY_ID, ds.Patients.EthnicityIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(35, PARM_RACE_ID, ds.Patients.RaceIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(36, PARM_PREF_LANGUAGE_ID, ds.Patients.PrefLanguageIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(37, PARM_SMOKING_STATUS_ID, ds.Patients.SmokingStatusIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(38, PARM_PREF_COMMUNICATION_ID, ds.Patients.PrefCommunicationIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(39, PARM_ADV_DIRECTIVE_ID, ds.Patients.AdvanceDirectiveIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(40, PARM_SCHOOL_ID, ds.Patients.SchoolIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(41, PARM_SCHOOL_STATUS, ds.Patients.SchoolStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(42, PARM_BAD_ADDRESS, ds.Patients.BadAddressColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(43, PARM_PATIENT_STATEMENT, ds.Patients.PatientStatementColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(44, PARM_IS_ACTIVE, ds.Patients.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(45, PARM_CREATED_BY, ds.Patients.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(46, PARM_CREATED_ON, ds.Patients.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(47, PARM_MODIFIED_BY, ds.Patients.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(48, PARM_MODIFIED_ON, ds.Patients.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(49, PARM_PATIENT_IMAGE, ds.Patients.PatientImageColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(50, PARM_PATIENT_IMAGE_TYPE, ds.Patients.ImageTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(51, PARM_PATIENT_INACTIVE_REASON, ds.Patients.InActiveReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(52, PARM_COMMENTS, ds.Patients.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(53, PARM_SELF_PAY, ds.Patients.SelfPayColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(54, PARM_PATIENT_PORTAL_STATUS, ds.Patients.PatientPortalStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(55, PARM_PATIENT_STRRACEIDS, ds.Patients.strRaceIdsColumn.ColumnName, DbType.String);//kr
            dbManager.AddParameters(55, PARM_PATIENT_STRRACEIDS, ds.Patients.strRaceIdsColumn.ColumnName, DbType.String);

        }

        /// <summary>
        /// Method Name: createPlanOfCareParameters
        /// Author Name: Ahmad Raza
        /// Created Date: 01-04-2016
        /// Description: This function will create parameters for Plan of Care insert/update
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createPlanOfCareParameters(IDBManager dbManager, DSClinicalSummary ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(13);
                dbManager.AddInsertUpdateParameters(0, PARM_PLAN_OF_CARE_ID, ds.PlanofCare.PlanofCareIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(13);
                dbManager.AddInsertUpdateParameters(0, PARM_PLAN_OF_CARE_ID, ds.PlanofCare.PlanofCareIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PLAN_OF_CARE_PATIENT_ID, ds.PlanofCare.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PLAN_OF_CARE_CLINICAL_INSTRUCTION, ds.PlanofCare.ClinicalInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_PLAN_OF_CARE_FUTURE_INSTRUCTION, ds.PlanofCare.FutureInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PLAN_OF_CARE_COMMENTS, ds.PlanofCare.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_PLAN_OF_CARE_ISACTIVE, ds.PlanofCare.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_PLAN_OF_CARE_CREATED_BY, ds.PlanofCare.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_PLAN_OF_CARE_CREATED_ON, ds.PlanofCare.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_PLAN_OF_CARE_MODIFIED_BY, ds.PlanofCare.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_PLAN_OF_CARE_MODIFIED_ON, ds.PlanofCare.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_PLAN_OF_CARE_SOAPTEXT, ds.PlanofCare.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_PLAN_OF_CARE_NOTES_ID, ds.PlanofCare.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_PLAN_OF_CARE_PATIENT_DECISION_AID, ds.PlanofCare.PatientDecisionAidColumn.ColumnName, DbType.String);


        }
        /// <summary>
        /// Method Name: CreatePlanOfCareGoalParameters
        /// Author Name: Ahmad Raza
        /// Created Date: 01-04-2016
        /// Description: This function will create parameters for Plan of Care goal insert/update
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreatePlanOfCareGoalParameters(IDBManager dbManager, DSClinicalSummary ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(18);
                dbManager.AddInsertUpdateParameters(0, PARM_PLAN_OF_CARE_GOALID, ds.PlanOfCareGoal.GoalIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(18);
                dbManager.AddInsertUpdateParameters(0, PARM_PLAN_OF_CARE_GOALID, ds.PlanOfCareGoal.GoalIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PLAN_OF_CARE_ID, ds.PlanOfCareGoal.PlanOfCareIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.PlanOfCareGoal.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.PlanOfCareGoal.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.PlanOfCareGoal.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.PlanOfCareGoal.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.PlanOfCareGoal.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.PlanOfCareGoal.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.PlanOfCareGoal.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.PlanOfCareGoal.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_PLAN_OF_CARE_INSTRUCTION, ds.PlanOfCareGoal.InstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_PLAN_OF_CARE_COMMENTS, ds.PlanOfCareGoal.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_PLAN_OF_CARE_ISACTIVE, ds.PlanOfCareGoal.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_PLAN_OF_CARE_CREATED_BY, ds.PlanOfCareGoal.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_PLAN_OF_CARE_CREATED_ON, ds.PlanOfCareGoal.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_PLAN_OF_CARE_MODIFIED_BY, ds.PlanOfCareGoal.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_PLAN_OF_CARE_MODIFIED_ON, ds.PlanOfCareGoal.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_PLAN_OF_CARE_SOAPTEXT, ds.PlanOfCareGoal.SoapTextColumn.ColumnName, DbType.String);



        }

        /// <summary>
        /// Method Name: createCognitiveParameters
        /// Author Name: Muhammad Arshad
        /// Created Date: 27-04-2016
        /// Description: This function will create parameters for Cognitive insert/update
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createCognitiveParameters(IDBManager dbManager, DSClinicalSummary ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_ID, ds.Cognitive.CognitiveIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_ID, ds.Cognitive.CognitiveIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_COGNITIVE_PATIENT_ID, ds.Cognitive.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_COGNITIVE_COMMENTS, ds.Cognitive.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_COGNITIVE_ISACTIVE, ds.Cognitive.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(4, PARM_COGNITIVE_CREATED_BY, ds.Cognitive.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COGNITIVE_CREATED_ON, ds.Cognitive.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(6, PARM_COGNITIVE_MODIFIED_BY, ds.Cognitive.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_COGNITIVE_MODIFIED_ON, ds.Cognitive.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_PLAN_OF_CARE_SOAPTEXT, ds.Cognitive.SoapTextColumn.ColumnName, DbType.String);


        }

        /// <summary>
        /// Method Name: CreateCognitive_StatusParameters
        /// Author Name: Muhammad Arshad
        /// Created Date: 28-04-2016
        /// Description: This function will create parameters for Cognitive goal insert/update
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateCognitiveStatusParameters(IDBManager dbManager, DSClinicalSummary ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(18);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_STAUS_ID, ds.Cognitive_Status.StatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(18);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_STAUS_ID, ds.Cognitive_Status.StatusIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_COGNITIVE_ID, ds.Cognitive_Status.CognitiveIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.Cognitive_Status.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.Cognitive_Status.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.Cognitive_Status.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.Cognitive_Status.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.Cognitive_Status.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.Cognitive_Status.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.Cognitive_Status.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.Cognitive_Status.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_COGNITIVE_INSTRUCTION, ds.Cognitive_Status.InstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_COGNITIVE_COMMENTS, ds.Cognitive_Status.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_COGNITIVE_ISACTIVE, ds.Cognitive_Status.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_COGNITIVE_CREATED_BY, ds.Cognitive_Status.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_COGNITIVE_CREATED_ON, ds.Cognitive_Status.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_COGNITIVE_MODIFIED_BY, ds.Cognitive_Status.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_COGNITIVE_MODIFIED_ON, ds.Cognitive_Status.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_COGNITIVE_SOAPTEXT, ds.Cognitive_Status.SoapTextColumn.ColumnName, DbType.String);
        }


        /// <summary>
        /// Method Name: CreateCognitive_FunctionalStatusParameters
        /// Author Name: Muhammad Arshad
        /// Created Date: 28-04-2016
        /// Description: This function will create parameters for Cognitive Functional Status insert/update
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateCognitiveFunctionalStatusParameters(IDBManager dbManager, DSClinicalSummary ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(18);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_FUNCTIONAL_STAUS_ID, ds.Cognitive_FunctionalStatus.FunctionalStatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(18);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_FUNCTIONAL_STAUS_ID, ds.Cognitive_FunctionalStatus.FunctionalStatusIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_COGNITIVE_ID, ds.Cognitive_FunctionalStatus.CognitiveIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.Cognitive_FunctionalStatus.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.Cognitive_FunctionalStatus.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.Cognitive_FunctionalStatus.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.Cognitive_FunctionalStatus.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.Cognitive_FunctionalStatus.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.Cognitive_FunctionalStatus.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.Cognitive_FunctionalStatus.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.Cognitive_FunctionalStatus.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_COGNITIVE_INSTRUCTION, ds.Cognitive_FunctionalStatus.InstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_COGNITIVE_COMMENTS, ds.Cognitive_FunctionalStatus.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_COGNITIVE_ISACTIVE, ds.Cognitive_FunctionalStatus.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_COGNITIVE_CREATED_BY, ds.Cognitive_FunctionalStatus.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_COGNITIVE_CREATED_ON, ds.Cognitive_FunctionalStatus.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_COGNITIVE_MODIFIED_BY, ds.Cognitive_FunctionalStatus.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_COGNITIVE_MODIFIED_ON, ds.Cognitive_FunctionalStatus.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_COGNITIVE_SOAPTEXT, ds.Cognitive_FunctionalStatus.SoapTextColumn.ColumnName, DbType.String);
        }


        /// <summary>
        /// Method Name: CreateCognitive_MentalStatusParameters
        /// Author Name: Usman Saeed
        /// Created Date: 11-11-2017
        /// Description: This function will create parameters for Cognitive Mental Status insert/update
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateCognitiveMentalStatusParameters(IDBManager dbManager, DSClinicalSummary ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(19);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_Mental_STAUS_ID, ds.Cognitive_MentalStatus.MentalStatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(19);
                dbManager.AddInsertUpdateParameters(0, PARM_COGNITIVE_Mental_STAUS_ID, ds.Cognitive_MentalStatus.MentalStatusIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_COGNITIVE_ID, ds.Cognitive_MentalStatus.CognitiveIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.Cognitive_MentalStatus.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.Cognitive_MentalStatus.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.Cognitive_MentalStatus.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.Cognitive_MentalStatus.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.Cognitive_MentalStatus.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.Cognitive_MentalStatus.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.Cognitive_MentalStatus.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.Cognitive_MentalStatus.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_COGNITIVE_INSTRUCTION, ds.Cognitive_MentalStatus.InstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_COGNITIVE_COMMENTS, ds.Cognitive_MentalStatus.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_COGNITIVE_ISACTIVE, ds.Cognitive_MentalStatus.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_COGNITIVE_CREATED_BY, ds.Cognitive_MentalStatus.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_COGNITIVE_CREATED_ON, ds.Cognitive_MentalStatus.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_COGNITIVE_MODIFIED_BY, ds.Cognitive_MentalStatus.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_COGNITIVE_MODIFIED_ON, ds.Cognitive_MentalStatus.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_COGNITIVE_SOAPTEXT, ds.Cognitive_MentalStatus.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_COGNITIVE_FreeTextICD, ds.Cognitive_MentalStatus.FreeTextICDColumn.ColumnName, DbType.String);
        }



        private void createReferenceDataParameters(IDBManager dbManager, DSClinicalSummary ds)
        {
            dbManager.CreateParameters(5);
            dbManager.AddParameters(0, PARM_PATIENT_MISCELLANEOUS_STATUS_ID, ds.PatientMiscellaneousStatus.PatientMiscellaneousStatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientMiscellaneousStatus.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_REFERENCE_DATA_ID, ds.PatientMiscellaneousStatus.ReferenceDataIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_VALUE, ds.PatientMiscellaneousStatus.ValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_DOCUMENTNAME, ds.PatientMiscellaneousStatus.DocumentNameColumn.ColumnName, DbType.String);
        }


        private void CreateMedicationParameters(IDBManager dbManager, DSClinicalMedication ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(40);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEDICATION_ID, ds.Medication.MedicationIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEDICATION_ID, ds.Medication.MedicationIDColumn.ColumnName, DbType.Int64);




            dbManager.AddParameters(1, PARM_RCOPIA, ds.Medication.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PATIENTID, ds.Medication.PatientIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PRESCRIPTIONID, ds.Medication.PrescriptionIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PROVIDERID, ds.Medication.ProviderIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_PREPARER_USERID, ds.Medication.Preparer_UserIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_DRUGID, ds.Medication.DrugIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_ACTION, ds.Medication.ActionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_DOSE, ds.Medication.DoseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_DOSEUNIT, ds.Medication.DoseUnitColumn.ColumnName, DbType.String);

            dbManager.AddParameters(10, PARM_ROUTEBY, ds.Medication.RoutebyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_DOSE_TIMING, ds.Medication.DoseTimingColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_DOSE_OTHER, ds.Medication.DoseOtherColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_DURATION, ds.Medication.DurationColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_QUANTITY, ds.Medication.QuantityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_QUANTITYUNIT, ds.Medication.QuantityUnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SUBSTITUTION, ds.Medication.SubstitutionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_OTHERNOTE, ds.Medication.OtherNoteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_PATIENTNOTES, ds.Medication.PatientNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_COMMENTS, ds.Medication.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_STARTDATE, ds.Medication.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_STOPEDATE, ds.Medication.StopDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_STOPREASON, ds.Medication.StopReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_SIGCHNAGEDATE, ds.Medication.SigChangedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_LASTMODIFIEDBY, ds.Medication.LastModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_LASTMODIFIEDDATE, ds.Medication.LastModifiedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_INTENDEDUSE, ds.Medication.IntendedUseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_NUMBER, ds.Medication.NumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(28, PARM_STATUS, ds.Medication.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_ISACTIVE, ds.Medication.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(30, PARM_CREATEDBY, ds.Medication.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_CREATEDON, ds.Medication.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(32, PARM_MODIFIEDBY, ds.Medication.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_MODIFIEDON, ds.Medication.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(34, PARAM_PRESCRIPTION_RCOPIAID, ds.Medication.PrescriptionRcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARAM_REFILL, ds.Medication.RefillColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARAM_FILLDATE, ds.Medication.FillDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(37, PARAM_ISDELETED, ds.Medication.IsDeletedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(38, PARM_ISNEWROW, ds.Medication.IsNewRowColumn.ColumnName, DbType.Byte, ParamDirection.Output);
            dbManager.AddParameters(39, PARM_DRUGDESCRIPTION, ds.Medication.DrugDescriptionColumn.ColumnName, DbType.String);//kr
        }
        #endregion

        #region PlanOfCare

        /// <summary>
        /// Method Name: insertUpdatePlanOfCare
        /// Author Name: Ahmad Raza
        /// Created Date: 01-04-2016
        /// Description: This function will handle insert/update of Plan of Care
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalSummary insertUpdatePlanOfCare(DSClinicalSummary ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.PlanofCare.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                createPlanOfCareParameters(dbManager, ds, true);
                createPlanOfCareParameters(dbManager, ds, false);
                ds = (DSClinicalSummary)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_INSERT, PROC_PLAN_OF_CARE_UPDATE, ds, ds.PlanofCare.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PlanofCare.Rows[0][ds.PlanofCare.PlanofCareIdColumn].ToString(), null, ds.PlanofCare.Rows[0][ds.PlanofCare.PlanofCareIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::insertUpdatePlanOfCare", PROC_PLAN_OF_CARE_INSERT + " " + PROC_PLAN_OF_CARE_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: loadPlanOfCare
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle load of Plan of Care
        /// </summary>
        /// <param name="PlanOfCareId"></param>
        /// <param name="PatientId"></param>
        /// <param name="NoteId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public DSClinicalSummary loadPlanOfCare(long PlanOfCareId, long PatientId, long NoteId, int pageNumber = 1, int rowsPerPage = 1000, string isViewPlanOfCare = "", string isPrintPlanOfCare = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (PlanOfCareId == 0)
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, PlanOfCareId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_PLAN_OF_CARE_NOTES_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PLAN_OF_CARE_NOTES_ID, NoteId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_SELECT, ds, ds.PlanofCare.TableName);
                if (ds.PlanofCare.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.PlanofCare.Rows[0]["PlanOfCareId"]) > 0)
                    {

                        DataTable dtTemp = ds.PlanofCare;
                        if (dtTemp != null)
                        {
                            if (isViewPlanOfCare == "1" || isPrintPlanOfCare == "1")
                            {
                                bool isViewAction = isViewPlanOfCare == "1" ? true : false;
                                bool isPrintAcion = isPrintPlanOfCare == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PlanofCare.Rows[0][ds.PlanofCare.PlanofCareIdColumn].ToString(), null, ds.PlanofCare.Rows[0][ds.PlanofCare.PlanofCareIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::loadPlanOfCare", PROC_PLAN_OF_CARE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: DeletePlanOfCare
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle delete of Plan of Care
        /// </summary>
        /// <param name="planOfCareId"></param>
        /// <returns></returns>

        public DSCaseReports loadPlanOfCareForCaseReports(long PlanOfCareId, long PatientId, long NoteId, int pageNumber = 1, int rowsPerPage = 1000, string isViewPlanOfCare = "", string isPrintPlanOfCare = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSCaseReports ds = new DSCaseReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (PlanOfCareId == 0)
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, PlanOfCareId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_PLAN_OF_CARE_NOTES_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PLAN_OF_CARE_NOTES_ID, NoteId);

                ds = (DSCaseReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_SELECT, ds, ds.PlanofCare.TableName);
                if (ds.PlanofCare.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.PlanofCare.Rows[0]["PlanOfCareId"]) > 0)
                    {

                        DataTable dtTemp = ds.PlanofCare;
                        if (dtTemp != null)
                        {
                            if (isViewPlanOfCare == "1" || isPrintPlanOfCare == "1")
                            {
                                bool isViewAction = isViewPlanOfCare == "1" ? true : false;
                                bool isPrintAcion = isPrintPlanOfCare == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PlanofCare.Rows[0][ds.PlanofCare.PlanofCareIdColumn].ToString(), null, ds.PlanofCare.Rows[0][ds.PlanofCare.PlanofCareIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALCaseReports::loadPlanOfCare", PROC_PLAN_OF_CARE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeletePlanOfCare(string planOfCareId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSClinicalSummary dsCurrentPlanOfCare = loadPlanOfCare(Convert.ToInt64(planOfCareId), 0, 0, 1, 1000, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, planOfCareId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentPlanOfCare.PlanofCare;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, planOfCareId.ToString(), null, planOfCareId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::DeletePlanOfCare", PROC_PLAN_OF_CARE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: updateSoapTextForPlanOfCare
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle update soap text of Plan of Care
        /// </summary>
        /// <param name="planOfCareId"></param>
        /// <returns></returns>
        public string updateSoapTextForPlanOfCare(long planOfCareId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, planOfCareId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_PLAN_OF_CARE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::updateSoapTextForPlanOfCare", PROC_UPDATE_SOAPTEXT_FOR_PLAN_OF_CARE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: loadPlanOfCareGoal
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle load of Plan of Care goal
        /// </summary>
        /// <param name="planOfCareId"></param>
        /// <param name="goalId"></param>
        /// <returns></returns>
        public DSClinicalSummary loadPlanOfCareGoal(long planOfCareId, long goalId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (planOfCareId == 0)
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_ID, planOfCareId);

                if (goalId == 0)
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_GOALID, null);
                else
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_GOALID, goalId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_GOAL_SELECT, ds, ds.PlanOfCareGoal.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadPlanOfCareGoal", PROC_PLAN_OF_CARE_GOAL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: insertUpdatePlanOfCareGoal
        /// Author Name: Ahmad Raza
        /// Created Date: 04-04-2016
        /// Description: This function will handle insert/Update of PlanOfCare Goal
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalSummary insertUpdatePlanOfCareGoal(DSClinicalSummary ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.PlanOfCareGoal.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreatePlanOfCareGoalParameters(dbManager, ds, true);
                CreatePlanOfCareGoalParameters(dbManager, ds, false);
                ds = (DSClinicalSummary)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_GOAL_INSERT, PROC_PLAN_OF_CARE_GOAL_UPDATE, ds, ds.PlanOfCareGoal.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PlanOfCareGoal.Rows[0][ds.PlanOfCareGoal.GoalIdColumn].ToString(), null, ds.PlanOfCareGoal.Rows[0][ds.PlanOfCareGoal.PlanOfCareIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::insertUpdatePlanOfCareGoal", PROC_PLAN_OF_CARE_GOAL_INSERT + " " + PROC_PLAN_OF_CARE_GOAL_UPDATE, ex);
                throw ex;
            }
        }
        /// <summary>
        /// Method Name: detachPlanOfCareFromNotes
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle detach of PlanOfCare from Notes
        /// </summary>
        /// <param name="planOfCareId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        public string detachPlanOfCareFromNotes(string planOfCareId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(planOfCareId))
                {
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, planOfCareId);
                }

                if (notesId == 0)
                {
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_NOTES_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_NOTES_ID, notesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::detachPlanOfCareFromNotes", PROC_PLAN_OF_CARE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string detachCognitiveFromNotes(string cognitiveId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_COGNITIVE_ID, cognitiveId);
                dbManager.AddParameters(1, PARM_NOTES_ID, notesId);
    

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DEATTATCH_COGNITIVE_STATUS_FROM_NOTE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::detachPlanOfCareFromNotes", PROC_DEATTATCH_COGNITIVE_STATUS_FROM_NOTE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string attatchCognitiveWithNotes(string cognitiveId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_COGNITIVE_ID, cognitiveId);
                dbManager.AddParameters(1, PARM_NOTES_ID, notesId);


                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_ATTATCH_COGNITIVE_STATUS_TO_NOTE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::attatchCognitiveWithNotes", PROC_ATTATCH_COGNITIVE_STATUS_TO_NOTE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        ///  Method Name: attachPlanOfCareWithNotes
        ///  Author Name: Ahmad Raza
        ///  Created Date: 04-04-2016
        ///  Description: This function will handle attach of PlanOfCare with Notes
        /// </summary>
        /// <param name="planOfCareId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        public DSClinicalSummary attachPlanOfCareWithNotes(string planOfCareId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSClinicalSummary ds = new DSClinicalSummary();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(planOfCareId))
                {
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PLAN_OF_CARE_ID, planOfCareId);
                }

                if (notesId == 0)
                {
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_NOTES_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PLAN_OF_CARE_NOTES_ID, notesId);
                }


                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PLAN_OF_CARE_FROM_NOTES, ds, ds.PlanofCare.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::attachPlanOfCareWithNotes", PROC_ATTACH_PLAN_OF_CARE_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: deletePlanOfCareGoal
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle delete of PlanOfCare goal
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        public string deletePlanOfCareGoal(string goalId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSClinicalSummary dsPlanOfCareGoal = loadPlanOfCareGoal(0, Convert.ToInt64(goalId));

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PLAN_OF_CARE_GOALID, goalId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PLAN_OF_CARE_GOAL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsPlanOfCareGoal.PlanOfCareGoal;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, goalId, null, dsPlanOfCareGoal.PlanOfCareGoal.Rows[0][dsPlanOfCareGoal.PlanOfCareGoal.PlanOfCareIdColumn].ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::deletePlanOfCareGoal", PROC_PLAN_OF_CARE_GOAL_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Congnitive

        /// <summary>
        /// Method Name: insertUpdateCognitive
        /// Author Name: Ahmad Raza
        /// Created Date: 01-04-2016
        /// Description: This function will handle insert/update of Cognitive
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalSummary insertUpdateCognitive(DSClinicalSummary ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Cognitive.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                createCognitiveParameters(dbManager, ds, true);
                createCognitiveParameters(dbManager, ds, false);
                ds = (DSClinicalSummary)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_INSERT, PROC_COGNITIVE_UPDATE, ds, ds.Cognitive.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Cognitive.Rows[0][ds.Cognitive.CognitiveIdColumn].ToString(), null, ds.Cognitive.Rows[0][ds.Cognitive.CognitiveIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::insertUpdateCognitive", PROC_COGNITIVE_INSERT + " " + PROC_COGNITIVE_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: loadCognitive
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle load of Cognitive
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <param name="PatientId"></param>
        /// <param name="NoteId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public DSClinicalSummary loadCognitive(long CognitiveId, long PatientId, int pageNumber = 1, int rowsPerPage = 1000, string isViewCognitive = "", string isPrintCognitive = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (CognitiveId <= 0)
                    dbManager.AddParameters(0, PARM_COGNITIVE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_COGNITIVE_ID, CognitiveId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_COGNITIVE_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_COGNITIVE_PATIENT_ID, PatientId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_SELECT, ds, ds.Cognitive.TableName);
                if (ds.Cognitive.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.Cognitive.Rows[0]["CognitiveId"]) > 0)
                    {

                        DataTable dtTemp = ds.Cognitive;
                        if (dtTemp != null)
                        {
                            if (isViewCognitive == "1" || isPrintCognitive == "1")
                            {
                                bool isViewAction = isViewCognitive == "1" ? true : false;
                                bool isPrintAcion = isPrintCognitive == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Cognitive.Rows[0][ds.Cognitive.CognitiveIdColumn].ToString(), null, ds.Cognitive.Rows[0][ds.Cognitive.CognitiveIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitive", PROC_COGNITIVE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalSummary loadCognitives(long CognitiveId, long PatientId, long NoteId, int pageNumber = 1, int rowsPerPage = 1000, string isViewCognitive = "", string isPrintCognitive = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (CognitiveId <= 0)
                    dbManager.AddParameters(0, PARM_COGNITIVE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_COGNITIVE_ID, CognitiveId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_COGNITIVE_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_COGNITIVE_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_PLAN_OF_CARE_NOTES_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PLAN_OF_CARE_NOTES_ID, NoteId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_SELECT, ds, ds.Cognitive.TableName);
                if (ds.Cognitive.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.Cognitive.Rows[0]["CognitiveId"]) > 0)
                    {

                        DataTable dtTemp = ds.Cognitive;
                        if (dtTemp != null)
                        {
                            if (isViewCognitive == "1" || isPrintCognitive == "1")
                            {
                                bool isViewAction = isViewCognitive == "1" ? true : false;
                                bool isPrintAcion = isPrintCognitive == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Cognitive.Rows[0][ds.Cognitive.CognitiveIdColumn].ToString(), null, ds.Cognitive.Rows[0][ds.Cognitive.CognitiveIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitive", PROC_COGNITIVE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: DeleteCognitive
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle delete of Cognitive
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <returns></returns>
        public string DeleteCognitive(string CognitiveId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSClinicalSummary dsCurrentCognitive = loadCognitive(Convert.ToInt64(CognitiveId), 0, 1, 1000, "", "");

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_COGNITIVE_ID, CognitiveId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COGNITIVE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentCognitive.Cognitive;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, CognitiveId.ToString(), null, CognitiveId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::DeleteCognitive", PROC_COGNITIVE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: updateSoapTextForCognitive
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle update soap text of Cognitive
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <returns></returns>
        public string updateSoapTextForCognitive(long CognitiveId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_COGNITIVE_ID, CognitiveId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_COGNITIVE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::updateSoapTextForCognitive", PROC_UPDATE_SOAPTEXT_FOR_COGNITIVE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: loadCognitiveStatus
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle load of Cognitive Status
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        public DSClinicalSummary loadCognitiveStatus(long CognitiveId, long StatusId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (CognitiveId == 0)
                    dbManager.AddParameters(1, PARM_COGNITIVE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_COGNITIVE_ID, CognitiveId);

                if (StatusId == 0)
                    dbManager.AddParameters(0, PARM_COGNITIVE_STAUS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_COGNITIVE_STAUS_ID, StatusId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_STATUS_SELECT, ds, ds.Cognitive_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitiveStatus", PROC_COGNITIVE_STATUS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: insertUpdateCognitiveStatus
        /// Author Name: Ahmad Raza
        /// Created Date: 04-04-2016
        /// Description: This function will handle insert/Update of Cognitive Status
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalSummary insertUpdateCognitiveStatus(DSClinicalSummary ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Cognitive_Status.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateCognitiveStatusParameters(dbManager, ds, true);
                CreateCognitiveStatusParameters(dbManager, ds, false);
                ds = (DSClinicalSummary)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_STATUS_INSERT, PROC_COGNITIVE_STATUS_UPDATE, ds, ds.Cognitive_Status.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Cognitive_Status.Rows[0][ds.Cognitive_Status.StatusIdColumn].ToString(), null, ds.Cognitive_Status.Rows[0][ds.Cognitive_Status.CognitiveIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::insertUpdateCognitiveStatus", PROC_COGNITIVE_STATUS_INSERT + " " + PROC_COGNITIVE_STATUS_UPDATE, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method Name: DeleteCognitiveStatus
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle delete of Cognitive
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <returns></returns>
        public string DeleteCognitiveStatus(string StatusId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSClinicalSummary dsCurrentCognitiveStatus = loadCognitiveStatus(0, Convert.ToInt64(StatusId));
                string CognitiveId = "";
                if (dsCurrentCognitiveStatus != null && dsCurrentCognitiveStatus.Cognitive_Status.Rows.Count > 0)
                {
                    CognitiveId = Convert.ToString(dsCurrentCognitiveStatus.Cognitive_Status.Rows[0][dsCurrentCognitiveStatus.Cognitive_Status.CognitiveIdColumn.ColumnName]);
                }

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_COGNITIVE_STAUS_ID, StatusId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COGNITIVE_STATUS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentCognitiveStatus.Cognitive_Status;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, StatusId.ToString(), null, CognitiveId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::PROC_COGNITIVE_STATUS_DELETE", PROC_COGNITIVE_STATUS_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: loadCognitiveFunctionalStatus
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle load of Cognitive FunctionalStatus
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <param name="FunctionalStatusId"></param>
        /// <returns></returns>
        public DSClinicalSummary loadCognitiveFunctionalStatus(long CognitiveId, long FunctionalStatusId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (CognitiveId == 0)
                    dbManager.AddParameters(1, PARM_COGNITIVE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_COGNITIVE_ID, CognitiveId);

                if (FunctionalStatusId == 0)
                    dbManager.AddParameters(0, PARM_COGNITIVE_FUNCTIONAL_STAUS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_COGNITIVE_FUNCTIONAL_STAUS_ID, FunctionalStatusId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_FUNCTIONALSTATUS_SELECT, ds, ds.Cognitive_FunctionalStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitiveFunctionalStatus", PROC_COGNITIVE_FUNCTIONALSTATUS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: insertUpdateCognitiveFunctionalStatus
        /// Author Name: Ahmad Raza
        /// Created Date: 04-04-2016
        /// Description: This function will handle insert/Update of Cognitive FunctionalStatus
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalSummary insertUpdateCognitiveFunctionalStatus(DSClinicalSummary ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Cognitive_FunctionalStatus.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateCognitiveFunctionalStatusParameters(dbManager, ds, true);
                CreateCognitiveFunctionalStatusParameters(dbManager, ds, false);
                ds = (DSClinicalSummary)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_FUNCTIONALSTATUS_INSERT, PROC_COGNITIVE_FUNCTIONALSTATUS_UPDATE, ds, ds.Cognitive_FunctionalStatus.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Cognitive_FunctionalStatus.Rows[0][ds.Cognitive_FunctionalStatus.FunctionalStatusIdColumn].ToString(), null, ds.Cognitive_FunctionalStatus.Rows[0][ds.Cognitive_FunctionalStatus.CognitiveIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::insertUpdateCognitiveFunctionalStatus", PROC_COGNITIVE_FUNCTIONALSTATUS_INSERT + " " + PROC_COGNITIVE_FUNCTIONALSTATUS_UPDATE, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method Name: DeleteCognitiveFunctionalStatus
        /// Author Name: Ahmad Raza
        /// Created Date: 05-04-2016
        /// Description: This function will handle delete of Cognitive
        /// </summary>
        /// <param name="CognitiveId"></param>
        /// <returns></returns>
        public string DeleteCognitiveFunctionalStatus(string FunctionalStatusId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSClinicalSummary dsCurrentCognitiveFunctionalStatus = loadCognitiveFunctionalStatus(0, Convert.ToInt64(FunctionalStatusId));
                string CognitiveId = "";
                if (dsCurrentCognitiveFunctionalStatus != null && dsCurrentCognitiveFunctionalStatus.Cognitive_FunctionalStatus.Rows.Count > 0)
                {
                    CognitiveId = Convert.ToString(dsCurrentCognitiveFunctionalStatus.Cognitive_FunctionalStatus.Rows[0][dsCurrentCognitiveFunctionalStatus.Cognitive_FunctionalStatus.CognitiveIdColumn.ColumnName]);
                }

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_COGNITIVE_FUNCTIONAL_STAUS_ID, FunctionalStatusId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COGNITIVE_FUNCTIONALSTATUS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentCognitiveFunctionalStatus.Cognitive_FunctionalStatus;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, FunctionalStatusId.ToString(), null, CognitiveId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::PROC_COGNITIVE_FunctionalStatus_DELETE", PROC_COGNITIVE_FUNCTIONALSTATUS_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary loadCognitiveMentalStatus(long CognitiveId, long MentalStatusId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (MentalStatusId == 0)
                    dbManager.AddParameters(0, PARM_COGNITIVE_Mental_STAUS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_COGNITIVE_Mental_STAUS_ID, MentalStatusId);

                if (CognitiveId == 0)
                    dbManager.AddParameters(1, PARM_COGNITIVE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_COGNITIVE_ID, CognitiveId);
                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_MentalSTATUS_SELECT, ds, ds.Cognitive_MentalStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitiveMentalStatus", PROC_COGNITIVE_MentalSTATUS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary insertUpdateCognitiveMentalStatus(DSClinicalSummary ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Cognitive_MentalStatus.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateCognitiveMentalStatusParameters(dbManager, ds, true);
                CreateCognitiveMentalStatusParameters(dbManager, ds, false);
                ds = (DSClinicalSummary)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_MentalSTATUS_INSERT, PROC_COGNITIVE_MentalSTATUS_UPDATE, ds, ds.Cognitive_MentalStatus.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Cognitive_MentalStatus.Rows[0][ds.Cognitive_MentalStatus.MentalStatusIdColumn].ToString(), null, ds.Cognitive_MentalStatus.Rows[0][ds.Cognitive_MentalStatus.CognitiveIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::insertUpdateCognitiveMentalStatus", PROC_COGNITIVE_MentalSTATUS_INSERT + " " + PROC_COGNITIVE_MentalSTATUS_UPDATE, ex);
                throw ex;
            }
        }

        public string DeleteCognitiveMentalStatus(string MentalStatusId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSClinicalSummary dsCurrentCognitiveMentalStatus = loadCognitiveMentalStatus(0, Convert.ToInt64(MentalStatusId));
                string CognitiveId = "";
                if (dsCurrentCognitiveMentalStatus != null && dsCurrentCognitiveMentalStatus.Cognitive_MentalStatus.Rows.Count > 0)
                {
                    CognitiveId = Convert.ToString(dsCurrentCognitiveMentalStatus.Cognitive_MentalStatus.Rows[0][dsCurrentCognitiveMentalStatus.Cognitive_MentalStatus.CognitiveIdColumn.ColumnName]);
                }

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_COGNITIVE_Mental_STAUS_ID, MentalStatusId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COGNITIVE_MentalSTATUS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentCognitiveMentalStatus.Cognitive_MentalStatus;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, MentalStatusId.ToString(), null, CognitiveId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::PROC_COGNITIVE_MentalStatus_DELETE", PROC_COGNITIVE_MentalSTATUS_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalSummary loadeMentalStatusCCDA(long NotesId, long PatientId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (NotesId > 0)
                    dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);
                else
                    dbManager.AddParameters(0, PARM_NOTES_ID, null);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MentalSTATUS_SELECT_CCDA, ds, ds.MentalStatus_CCDA.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadeMentalStatusCCDA", PROC_MentalSTATUS_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Clinical Summary Functions

        public DSLabResult LoadLabResult(long labResultId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                string pageNumber = "", rowsPerPage = "", test = "", orderNo = "", orderDateFrom = "", orderDateTo = "", status = "", labId = "", noteId = "", patientId = "", isViewOrder = "", isPrintOrder = "";
                int page;
                int rpp;
                if (string.IsNullOrEmpty(pageNumber))
                {
                    page = 1;
                }
                else
                {
                    page = Convert.ToInt32(pageNumber);
                }

                if (string.IsNullOrEmpty(rowsPerPage))
                {
                    rpp = 2000;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }

                dbManager.BeginTransaction();
                dbManager.CreateParameters(14);

                if (labResultId == 0)
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, labResultId);

                dbManager.AddParameters(1, PARM_LAB_ORDER_ID, null);

                if (page <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.LabOrderResult.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (test == "")
                    dbManager.AddParameters(5, PARM_TEST, null);
                else
                    dbManager.AddParameters(5, PARM_TEST, test);

                if (orderNo == "")
                    dbManager.AddParameters(6, PARM_ORDER_NO, null);
                else
                    dbManager.AddParameters(6, PARM_ORDER_NO, orderNo);


                dbManager.AddParameters(7, PARM_PROVIDER_ID, null);


                if (orderDateFrom == "")
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, null);
                else
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, orderDateFrom);

                if (orderDateTo == "")
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, null);
                else
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, orderDateTo);

                if (status == "")
                    dbManager.AddParameters(10, PARM_STATUS, null);
                else
                    dbManager.AddParameters(10, PARM_STATUS, status);

                if (labId == "")
                    dbManager.AddParameters(11, PARM_LAB_ID, null);
                else
                    dbManager.AddParameters(11, PARM_LAB_ID, labId);


                dbManager.AddParameters(12, PARM_NOTE_ID, null);


                dbManager.AddParameters(13, PARM_PATIENT_ID, null);


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_SELECT, ds, ds.LabOrderResult.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabResult::Lab_ORDER_SELECT", PROC_Lab_ORDER_RESULT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientReferral getLatestReferralByPatientId(long PatientId,long NotesId)
        {
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);

                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_REFERRAL_BY_PATIENTI, ds, ds.Referrals.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::getLatestReferralByPatientId", PROC_GET_LATEST_REFERRAL_BY_PATIENTI, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfile LoadProvider(long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (SpecialityId == "")
                    SpecialityId = null;

                if (NPI == "")
                    NPI = null;

                if (EntityId == "")
                    EntityId = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(12);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(3, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(4, PARM_SPECIALITY, SpecialityId);
                dbManager.AddParameters(5, PARM_NPI, NPI);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);


                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.Provider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_SELECT, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProvider", PROC_PROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadProblemLists(long ProblemListId, long PatientId, long NoteId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DataTable dtTemp = ds.ProblemList;
                dbManager.BeginTransaction();
                dbManager.CreateParameters(8);
                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(3, PARM_IS_HISTORY, "0");
                dbManager.AddParameters(4, PARM_IS_ACTIVE, "1");
                dbManager.AddParameters(5, PARM_PAGE_NUMBER, "1");
                dbManager.AddParameters(6, PARM_ROWSP_PAGE, "20000");
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.ProblemList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemList.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary LoadComplaintsByNoteId(long NoteId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCDA_COMPLAINTS_BY_NOTEID, ds, ds.ComplaintDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAllergies loadAllergies(long allergyId, long patientId, long NoteId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Allergy;
                dbManager.BeginTransaction();
                dbManager.CreateParameters(8);

                if (allergyId == 0)
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, allergyId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");
                if (NoteId == 0)
                    dbManager.AddParameters(3, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(4, PARM_IS_ACTIVE, "1");

                dbManager.AddParameters(5, PARM_PAGE_NUMBER, "1");
                dbManager.AddParameters(6, PARM_ROWSP_PAGE, "20000");
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Allergy.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT, ds, ds.Allergy.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::loadAllergies", PROC_ALLERGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMedicalHx LoadMedicalHx(long PatientId, long MedicalHxId)
        {
            DSMedicalHx ds = new DSMedicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (MedicalHxId == 0)
                    dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, MedicalHxId);
                ds = (DSMedicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_SELECT, ds, ds.MedicalHx.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALMedicalHx::LoadMedicalHx", PROC_MEDICALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSurgicalHx LoadSurgicalHx(long PatientId, long SurgicalHxId)
        {
            DSSurgicalHx ds = new DSSurgicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (SurgicalHxId == 0)
                    dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, SurgicalHxId);
                ds = (DSSurgicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SURGICALHX_SELECT, ds, ds.SurgicalHx.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSurgicalHx::LoadSurgicalHx", PROC_SURGICALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHospitalizationHx LoadHospitalizationHx(long PatientId, long HospitalizationHxId)
        {
            DSHospitalizationHx ds = new DSHospitalizationHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (HospitalizationHxId == 0)
                    dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, HospitalizationHxId);
                ds = (DSHospitalizationHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_SELECT, ds, ds.HospitalizationHx.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALHospitalizationHx::LoadHospitalizationHx", PROC_HOSPITALIZATIONHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFamilyHx LoadFamilyHx(long PatientId, long FamilyHxId, string isViewFamilyHx = "", string isPrintFamilyHx = "")
        {
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (FamilyHxId == 0)
                    dbManager.AddParameters(1, PARM_FAMILY_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAMILY_HX_ID, FamilyHxId);
                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_SELECT, ds, ds.FamilyHx.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHx", PROC_FAMILYHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBirthHistory loadBirthHx(long patientId, long birthHxId)
        {
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (birthHxId == 0)
                    dbManager.AddParameters(1, PARM_BIRTHHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BIRTHHX_ID, birthHxId);
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_SEELCT, ds, ds.BirthHx.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::loadBirthHx", PROC_BIRTHHX_SEELCT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExam loadPatientPhysicalExam(long patientId, long patientPhysicalExamId, long noteId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (patientPhysicalExamId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SELECT, ds, ds.PatientPhysicalExam.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LoadPhysicalExam", PROC_PATIENT_PHYSICAL_EXAM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalMedication loadMedications(long medicationId, long patientId, long noteId, bool isCurrent, int pageNo = 1, int rpp = 15, string isViewAllergy = "", string isPrintAllergy = "")
        {

            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication;
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);

                if (medicationId == 0)
                    dbManager.AddParameters(0, PARM_MEDICATION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEDICATION_ID, medicationId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, patientId);

                dbManager.AddParameters(2, PARAM_ISCURRENT, isCurrent);

                if (pageNo == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNo);
                if (rpp == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, rpp);
                if (noteId == 0)
                    dbManager.AddParameters(5, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.Medication.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT, ds, ds.Medication.TableName);


                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedications", PROC_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedures loadProcedures(long procedureId, long patientId, long noteId, string ShowEMCodes = "", string showCustomFormProcedures = "", string showVaccineProcedures = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProcedures ds = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                DataTable dtTemp = ds.Procedures;
                dbManager.Open();
                dbManager.CreateParameters(11);

                if (procedureId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(2, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTES_ID, noteId);

                dbManager.AddParameters(3, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(4, PARM_IS_ACTIVE, "1");

                dbManager.AddParameters(5, PARM_PAGE_NUMBER, "1");

                dbManager.AddParameters(6, PARM_ROWSP_PAGE, "20000");
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Procedures.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ShowEMCodes == "0")
                    dbManager.AddParameters(8, PARM_SHOW_EM_CODES, false);
                else
                    dbManager.AddParameters(8, PARM_SHOW_EM_CODES, null);

                if (showCustomFormProcedures == "")
                    dbManager.AddParameters(9, PARM_SHOW_CFPROCEDURES, false);
                else
                    dbManager.AddParameters(9, PARM_SHOW_CFPROCEDURES, true);

                if (showVaccineProcedures == "")
                    dbManager.AddParameters(10, PARM_SHOW_VPROCEDURES, false);
                else
                    dbManager.AddParameters(10, PARM_SHOW_VPROCEDURES, true);

                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.Procedures.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadProcedures", PROC_PROCEDURES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary LoadVaccineHxClinicalSummary(long patientId, long vaccineHxId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                DSClinicalSummary ds = new DSClinicalSummary();
                dbManager.CreateParameters(2);
                if (vaccineHxId == 0)
                    dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, null);
                else
                    dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, vaccineHxId);
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_LOAD_Clinical_Summary, ds, ds.Immunization.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DSClinicalSummary::LoadVaccineHx", PROC_ImmunizationHx_LOAD, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSConsultationOrder loadConsultationOrder(long consultationOrderId, long patientId, long NoteId = 0)
        {
            DSConsultationOrder ds = new DSConsultationOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(12);

                if (consultationOrderId == 0)
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);

                dbManager.AddParameters(3, PARM_ROWSP_PAGE, 2000);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.ConsultationOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(5, PARM_PROCEDURENAME, null);

                dbManager.AddParameters(6, PARM_ORDERNO, null);

                dbManager.AddParameters(7, PARM_PROVIDER_ID, null);

                dbManager.AddParameters(8, PARM_ORDERFROM, null);

                dbManager.AddParameters(9, PARM_ORDERTO, null);

                dbManager.AddParameters(10, PARM_STATUS, null);



                if (NoteId == 0)
                    dbManager.AddParameters(11, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(11, PARM_NOTE_ID, NoteId);


                ds = (DSConsultationOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_SELECT, ds, ds.ConsultationOrder.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALConsultationOrder::loadConsultationOrder", PROC_CONSULTATION_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: loadLabResultDetaii
        /// Author: Abid Ali
        /// Created Date: 31-03-2016
        /// Description: loading Lab Result Detail
        /// </summary>
        /// <param name="LabResultId" type="long">LabOrderResultId</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSLabResult loadLabResultDetail(long LabOrderResultDetailId, long LabOrderResultId, long PatientId = 0)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (LabOrderResultDetailId == 0)
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_DETAIL_ID, LabOrderResultDetailId);

                if (LabOrderResultId == 0)
                    dbManager.AddParameters(1, PARM_LAB_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LAB_ORDER_RESULT_ID, LabOrderResultId);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.LabOrderResultDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_SELECT, ds, ds.LabOrderResultDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrderResultDetail::loadLabOrderResultDetail", PROC_Lab_ORDER_RESULT_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary loadCognitive_CognitiveFunctionalStatus(long PatientId, long NotesId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NotesId > 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COGNITIVE_COGNITIVEFUNCTIONALSTATUS_SELECT, ds, ds.FunctionalStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitiveFunctionalStatus", PROC_COGNITIVE_COGNITIVEFUNCTIONALSTATUS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary loadCCDAEncounterDiagnostic(long PatientId, long NoteId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCDAENCOUNTERDIAGNOSTIC, ds, ds.EncounterProblemList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitiveFunctionalStatus", PROC_CCDAENCOUNTERDIAGNOSTIC, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public Int64 CCDAPatientsCheckExist(string LastName, string FirstName, DateTime DOB, string Gender)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(2, PARM_DOB, DOB.ToShortDateString());
                dbManager.AddParameters(3, PARM_GENDER, Gender);
                var result = Convert.ToInt64(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCDA_PATIENTS_CHECK_EXIST));
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadCognitiveFunctionalStatus", PROC_CCDA_PATIENTS_CHECK_EXIST, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSDBAudit insertClinicalSummaryCopyAudit(string PatientId, string SummaryType)
        {

            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();


            dsDBAudit = new DBActivityAudit().InsertDBAudit(new DataTable(), dbManager, "", null, "", false, false, false, PatientId, SummaryType);
            return dsDBAudit;
        }
        public DSLabOrder LoadLabOrdersForCCDA(long PatientId, long NotesId)
        {
            DSLabOrder ds = new DSLabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);


                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_SELECT_CCDA, ds, ds.LabOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadLabOrdersForCCDA", PROC_Lab_ORDER_TEST_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCM loadCareTeamCCDA(long patientId)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                List<string> tableNames = new List<string>();
                tableNames.Add(ds.CareTeamPCP.TableName);
                tableNames.Add(ds.CareTeamProvider.TableName);
                tableNames.Add(ds.CareGivers.TableName);
                tableNames.Add(ds.CareManagers.TableName);
                tableNames.Add(ds.CareCoordinators.TableName);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CARETEAM_SELECT_CCDA, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::loadCareTeamCCDA", PROC_CARETEAM_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LoadProblemListsCCDA(long PatientId, long NoteId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NoteId > 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_CCDA, ds, ds.ProblemList_CCDA.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadProblemListsCCDA", PROC_PROBLEMLIST_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFamilyHx LoadFamilyHistoryCCDA(long PatientId, long NoteId)
        {
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NoteId > 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCDA_FAMILYHX_SELECT, ds, ds.FamilyHx.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadFamilyHistoryCCDA", PROC_CCDA_FAMILYHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadProblemListsCCDACancer(long PatientId, long NoteId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NoteId > 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCDA_CANCERREPORT_SELECT, ds, ds.ProblemList.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadProblemListsCCDACancer", PROC_CCDA_CANCERREPORT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CarePlanHealthConcernsModel> LoadHealthConcernCCDA(long PatientId, long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (NoteId > 0)
                {
                    dbManager.AddParameters(PARM_NOTES_ID, NoteId);
                }
                else
                {
                    dbManager.AddParameters(PARM_NOTES_ID, DBNull.Value);
                }
                List<CarePlanHealthConcernsModel> result = dbManager.ExecuteReaders<CarePlanHealthConcernsModel>(PROC_HEALTHCONCERNS_SELECT_CCDA);

                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadHealthConcernCCDA", PROC_HEALTHCONCERNS_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CarePlanInterventionsModel> LoadInterventionsCCDA(long PatientId, long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (NoteId > 0)
                {
                    dbManager.AddParameters(PARM_NOTES_ID, NoteId);
                }
                else
                {
                    dbManager.AddParameters(PARM_NOTES_ID, DBNull.Value);
                }
                List<CarePlanInterventionsModel> result = dbManager.ExecuteReaders<CarePlanInterventionsModel>(PROC_INTERVENTION_SELECT_CCDA);

                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadInterventionsCCDA", PROC_INTERVENTION_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CarePlanOutcomesModel> LoadOutcomesCCDA(long PatientId, long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (NoteId > 0)
                {
                    dbManager.AddParameters(PARM_NOTES_ID, NoteId);
                }
                else
                {
                    dbManager.AddParameters(PARM_NOTES_ID, DBNull.Value);
                }
                List<CarePlanOutcomesModel> result = dbManager.ExecuteReaders<CarePlanOutcomesModel>(PROC_OUTCOMES_SELECT_CCDA);

                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadOutcomesCCDA", PROC_OUTCOMES_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalSummary LoadVaccineHxCCDA(long patientId, long notesId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSClinicalSummary ds = new DSClinicalSummary();
                dbManager.CreateParameters(2);
                if (notesId == 0)
                    dbManager.AddParameters(0, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTES_ID, notesId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_SELECT_CCDA, ds, ds.Immunization.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSClinicalSummary::LoadVaccineHxCCDA", PROC_ImmunizationHx_SELECT_CCDA, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region ImportCDS Functions

        public DSClinicalSummary InsertImportCDS(DSClinicalSummary dsImportCDS)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();


            try
            {

                dbManager.BeginTransaction();
                createReferenceDataParameters(dbManager, dsImportCDS);

                dsImportCDS = (DSClinicalSummary)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VDTAUDITINSERT, dsImportCDS, dsImportCDS.PatientMiscellaneousStatus.TableName);
                dbManager.CommitTransaction();
                dsImportCDS.AcceptChanges();
                //ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VDTAUDITINSERT, ds, ds.PatientMiscellaneousStatus.TableName);


                return dsImportCDS;
            }
            catch (Exception ex)
            {
                dsImportCDS.RejectChanges();
                MDVLogger.DALErrorLog("DALClinicalSummary::InsertImportCDS", PROC_VDTAUDITINSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Inserts the patient.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatient InsertPatient(DSPatient ds)
        {

            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Patients.GetChanges();

                // dbManager.Open();
                dbManager.BeginTransaction();

                CreateParameters(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSERT, ds, ds.Patients.TableName);


                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Insert Patient", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();

                dbManager.RollBackTransaction();
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error while inserting the patient : " + ex.ToString());
                MDVLogger.DALErrorLog("DALPatient::InsertPatient", PROC_PATIENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSAllergies insertAllergies(DSAllergies ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Allergy.GetChanges();
                DSAllergies dsOldDataAllergy = new DSAllergies();

                for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                {
                    string RcopiaID = ds.Allergy.Rows[i]["RcopiaID"].ToString();
                    if (RcopiaID != "")
                    {
                        var asas = getAllergyByRcopiaID(RcopiaID);
                        dsOldDataAllergy.Merge(asas);
                    }
                }

                DataTable dt = dsOldDataAllergy.Tables["Allergy"];
                dbManager.BeginTransaction();
                CreateParametersAllergies(dbManager, ds, true);
                ds = (DSAllergies)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ALLERGY_INSERT, ds, ds.Allergy.TableName);
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::insertAllergies", PROC_ALLERGY_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalMedication getMedicationByRcopiaID(string RcopiaID)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (RcopiaID == "")
                    dbManager.AddParameters(0, PARM_RCOPIAID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_RCOPIAID, RcopiaID);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, GET_MEDICATION_BY_RCOPIAID, ds, ds.Medication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getMedicationByRcopiaID", GET_MEDICATION_BY_RCOPIAID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalMedication insertMedication(DSClinicalMedication ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication.GetChanges();
                // dbManager.Open();
                DSClinicalMedication dsOldDataMedication = new DSClinicalMedication();

                for (int i = 0; i < ds.Medication.Rows.Count; i++)
                {
                    string RcopiaID = ds.Medication[i]["RcopiaID"].ToString();
                    if (RcopiaID != "")
                    {
                        var asas = getMedicationByRcopiaID(RcopiaID);
                        dsOldDataMedication.Merge(asas);
                    }
                }

                dbManager.BeginTransaction();
                CreateMedicationParameters(dbManager, ds, true);
                ds = (DSClinicalMedication)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEDICATION_INSERT, ds, ds.Medication.TableName);
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALMedications::insertMedication", PROC_MEDICATION_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists insertProblemLists(DSProblemLists ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                dbManager.BeginTransaction();
                CreateProblemListParameters(dbManager, ds, true);
                ds = (DSProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_INSERT, ds, ds.ProblemList.TableName);
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemLists", PROC_PROBLEMLIST_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private void CreateProblemListParameters(IDBManager dbManager, DSProblemLists ds, bool IsInsert)
        {
            dbManager.CreateParameters(23);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PROBLEM_NAME, ds.ProblemList.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ProblemList.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CHRONICITY_LEVEL, ds.ProblemList.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.ProblemList.SeverityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_START_DATE, ds.ProblemList.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.ProblemList.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PATIENT_ID, ds.ProblemList.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_NOTE_ID, ds.ProblemList.NoteIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.ProblemList.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.ProblemList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(15, PARM_INACTIVE_VALUE, ds.ProblemList.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_INACTIVE_REASON, ds.ProblemList.InActiveReasonColumn.ColumnName, DbType.String);

            dbManager.AddParameters(17, PARM_ICD9, ds.ProblemList.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ICD10, ds.ProblemList.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ICD10_DESCRIPTION, ds.ProblemList.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ICD9_DESCRIPTION, ds.ProblemList.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_SNOMEDID, ds.ProblemList.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_SNOMEDDESCRIPTION, ds.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
        }

        private void CreateParametersAllergies(IDBManager dbManager, DSAllergies ds, bool IsInsert)
        {
            dbManager.CreateParameters(20);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ALLERGY_ID, ds.Allergy.AllergyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ALLERGY_ID, ds.Allergy.AllergyIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ALLERGEN, ds.Allergy.AllergenColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_TYPE, ds.Allergy.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_REACTION, ds.Allergy.ReactionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.Allergy.SeverityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_ONSET_DATE, ds.Allergy.OnSetDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_LAST_MODIFIED, ds.Allergy.LastModifiedColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.Allergy.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_COMMENTS, ds.Allergy.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_NOTE_ID, ds.Allergy.NoteIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(10, PARM_INACTIVE_CHECKBOXVALUE, ds.Allergy.InActiveCheckBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_INACTIVE_REASON, ds.Allergy.InActiveReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PATIENT_ID, ds.Allergy.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.Allergy.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.Allergy.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_RCOPIAID, ds.Allergy.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ISDELETED, ds.Allergy.IsDeletedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_RXNORMID, ds.Allergy.RxnormIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_RXNORMIDTYPE, ds.Allergy.RxnormIDTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ISNEWROW, ds.Allergy.IsNewRowColumn.ColumnName, DbType.Byte, ParamDirection.Output);
        }

        public DSAllergies getAllergyByRcopiaID(string RcopiaID)
        {
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (RcopiaID == "")
                    dbManager.AddParameters(0, PARM_RCOPIAID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_RCOPIAID, RcopiaID);

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, GET_ALLERGY_BY_RCOPIAID, ds, ds.Allergy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getAllergyByRcopiaID", GET_ALLERGY_BY_RCOPIAID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// load Patient loopup
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSClinicalSummary PatientLookup()
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSClinicalSummary ds = new DSClinicalSummary();
            try
            {
                dbManager.BeginTransaction();
                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCDA_PATIENT_LOOKUPS, ds, new List<string> { ds.Languages.TableName, ds.Race.TableName, ds.Ethnicity.TableName });
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::PatientLookup", PROC_CCDA_PATIENT_LOOKUPS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public long getDrugIdByRxnormId(long RxnormId, string User, string DrugDescription)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            long DrugId = 0;
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(4);
                if (RxnormId == 0)
                    dbManager.AddParameters(0, PARM_RXNORMID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_RXNORMID, RxnormId);

                dbManager.AddParameters(1, PARM_USER, User);
                dbManager.AddParameters(2, PARM_DRUG_DESCRIPTION, DrugDescription);
                dbManager.AddParameters(3, PARM_BRAND_NAME, DrugDescription);
                DrugId = (long)dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCDA_GET_DRUGID_FROM_RXNORMID);
                dbManager.CommitTransaction();

                return DrugId;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALClinicalSummary::getDrugIdByRxnormId", PROC_CCDA_GET_DRUGID_FROM_RXNORMID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Legacy Notes

        public List<FunctionalCognitive> NotesFunctionalCognitiveSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<FunctionalCognitive> objList_FunctionalCognitive = new List<FunctionalCognitive>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_FUNCCOGNITIVE_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        FunctionalCognitive model = new FunctionalCognitive();
                        var properties = typeof(FunctionalCognitive).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_FunctionalCognitive.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::NotesFunctionalCognitiveSelect", PROC_NOTES_FUNCCOGNITIVE_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_FunctionalCognitive;
        }

        public List<NotesComponent> NotesComponentSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<NotesComponent> objList_NotesComponent = new List<NotesComponent>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_COMPONENTS_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        NotesComponent model = new NotesComponent();
                        var properties = typeof(NotesComponent).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_NotesComponent.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::NotesComponentSelect", PROC_NOTES_COMPONENTS_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_NotesComponent;
        }

        public List<Prescription> NotesPrescriptionSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<Prescription> objList_Prescription = new List<Prescription>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_PRESCRIPTION_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        Prescription model = new Prescription();
                        var properties = typeof(Prescription).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_Prescription.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::NotesPrescriptionSelect", PROC_NOTES_PRESCRIPTION_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_Prescription;
        }

        public List<NoteHeaderData> NotesNoteHeaderDataSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<NoteHeaderData> objList_NoteHeaderData = new List<NoteHeaderData>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                parameters.Add(new SqlParameter(PARM_PROVIDER_ID, objCommonSearch.ProviderId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_HEADERDAT_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        NoteHeaderData model = new NoteHeaderData();
                        var properties = typeof(NoteHeaderData).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_NoteHeaderData.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::NotesNoteHeaderDataSelect", PROC_NOTES_HEADERDAT_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_NoteHeaderData;
        }

        public List<MDVision.Model.Clinical.LegacyNotes.Provider> ProviderDataSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MDVision.Model.Clinical.LegacyNotes.Provider> objList_Provider = new List<MDVision.Model.Clinical.LegacyNotes.Provider>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PROVIDER_ID, objCommonSearch.ProviderId));
                using (var reader = dbManager.ExecuteReader(PROC_GET_PROVIDER_SIGNATURE, parameters))
                {
                    while (reader.Read())
                    {
                        MDVision.Model.Clinical.LegacyNotes.Provider model = new MDVision.Model.Clinical.LegacyNotes.Provider();
                        var properties = typeof(MDVision.Model.Clinical.LegacyNotes.Provider).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_Provider.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::ProviderDataSelect", PROC_GET_PROVIDER_SIGNATURE, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_Provider;
        }

        #endregion Legacy Notes
        public DSClinicalMedication loadMedicationsCCDA(long patientId, long noteId)
        {

            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);

                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, noteId);
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_CCDA, ds, ds.Medication.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsCCDA", PROC_MEDICATION_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSLabResult loadLabResultDetailCCDA(long PatientId, long noteId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, noteId);

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_SELECT_CCDA, ds, ds.LabOrderResultDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadLabResultDetailCCDA", PROC_Lab_ORDER_RESULT_DETAIL_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAllergies loadAllergiesCCDA(long patientId, long NoteId)
        {

            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            try
            {
                dbManager.CreateParameters(2);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);

                if (NoteId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT_CCDA, ds, ds.Allergy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::loadAllergiesCCDA", PROC_ALLERGY_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalSummary LoadAROAUPData(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(ReportsParamaters.Count);
                int counter = 0;

                foreach (var item in ReportsParamaters)
                {
                    dbManager.AddParameters(counter, item.Key, string.IsNullOrEmpty(item.Value.ToString()) ? DBNull.Value : item.Value);
                    counter++;
                }
                if (ReportName.Equals("AROReport".Trim()))
                {
                    List<string> tableNames = new List<string>();
                    tableNames.Add(ds.ARO_FacilitySpecimen.TableName);
                    tableNames.Add(ds.ARO_Antimicrobial.TableName);
                    tableNames.Add(ds.ARO_Observations.TableName);

                    ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ARO_REPORT, ds, tableNames);
                }
                else if (ReportName.Equals("AUPReport".Trim()))
                {
                    ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AUP_REPORT, ds, ds.AUP_Report.TableName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadAROAUPData", "", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary LoadOutPatientEncounter(long PatientId, long NotesId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OUTPATIENT_ENCOUNTER_SELECT, ds, ds.OutPatientEncounter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadOutPatientEncounter", PROC_OUTPATIENT_ENCOUNTER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalSummary LoadOccupationData(long PatientId, long NotesId)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);

                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCDA_EMPLOYMENTHISTORYSELECT, ds, ds.OccupationStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadOccupationData", PROC_CCDA_EMPLOYMENTHISTORYSELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LoadSocialHx(long PatientId, long NotesId)
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SELECT_CCDA, ds, ds.SocialHx.TableName);
              
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::LoadSocialHx", PROC_SOCIALHX_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVitals LoadVitalsForCCDA(long PatientId, long VitalSignId, long NotesId)
        {
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (VitalSignId == 0)
                    dbManager.AddParameters(0, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VITAL_ID, VitalSignId);
                if ( NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID,  NotesId);
            
                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);
                
                if (VitalSignId == 0)
                {
                    // For Child Records IsParent=0
                    dbManager.AddParameters(3, PARM_IS_PARENT, "0");
                    DSVitals dsChildVitals = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_SELECT_CCDA, ds, ds.VitalSignsChild.TableName);
                    if (dsChildVitals != null)
                    {
                        ds.Merge(dsChildVitals);
                    }

                    // For Parent Records IsParent=1
                    dbManager.AddParameters(3, PARM_IS_PARENT, "1");
                    DSVitals dsParentVitals = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_SELECT_CCDA, ds, ds.VitalSignSoap.TableName);
                    if (dsParentVitals != null)
                    {
                        ds.Merge(dsParentVitals);
                    }
                }
                else
                {
                    dbManager.AddParameters(3, PARM_IS_PARENT, "1");
                    ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_SELECT_CCDA, ds, ds.VitalSigns.TableName);
                    
                }
                
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALClinicalSummary::LoadVitalsForCCDA", PROC_VITALS_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedures loadProceduresCCDA( long patientId, long noteId)
        {
            DSProcedures ds = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                
                dbManager.Open();
                dbManager.CreateParameters(2);

           
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, noteId);
                
                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT_CCDA, ds, ds.Procedures.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadProceduresCCDA", PROC_PROCEDURES_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyResult loadRadiologyResultDetail( long patientId,long notesId)
        {
            DSRadiologyResult ds = new DSRadiologyResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (notesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, notesId);

                ds = (DSRadiologyResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_RESULT_DETAIL_SELECT_CCDA, ds, ds.RadiologyOrderResultDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::loadRadiologyResultDetail", PROC_RADIOLOGY_ORDER_RESULT_DETAIL_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

    }
}
