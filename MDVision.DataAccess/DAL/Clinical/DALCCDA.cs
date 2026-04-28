using System;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.CCDA;
using MDVision.Common.Utilities;
using MDVision.Model.Common;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALCCDA
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALCCDA()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALCCDA(bool isNative)
        {


        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALCCDA(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PATIENT_SELECT = "Patient.sp_PatientsSelect";
        private const string PROC_ENCOUNTER_INSERT_UPDATE = "Clinical.CQM_EncounterInsertUpdate";
        private const string PROC_INSERT_MU_SETTING = "Clinical.sp_InsertMUSetting";
        private const string PROC_PROCEDURE_INSERT_UPDATE = "Clinical.CQM_ProcedureInsertUpdate";
        private const string PROC_DIAGNOSIS_INSERT_UPDATE = "Clinical.CQM_DiagnosisInsertUpdate";
        private const string PROC_INTERVENTION_INSERT_UPDATE = "Clinical.CQM_InterventionsInsertUpdate";
        private const string PROC_COMM_PROVIDER_TO_PROVIDER_INSERT_UPDATE = "Clinical.CQM_CommProviderToProviderInsertUpdate";
        private const string PROC_COMM_PATIENT_TO_PROVIDER_INSERT_UPDATE = "Clinical.CQM_CommPatientToProviderInsertUpdate";
        private const string PROC_PATIENT_CHARACTERISTICS_INSERT_UPDATE = "Clinical.CQM_PatientCharacteristicsInsertUpdate";
        private const string PROC_DIAGNOSTIC_STUDY_INSERT_UPDATE = "Clinical.CQM_DiagnosticStudyInsertUpdate";
        private const string PROC_IMMUNIZATION_INSERT_UPDATE = "Clinical.CQM_ImmunizationInsertUpdate";
        private const string PROC_MEDICATION_INSERT_UPDATE = "Clinical.CQM_MedicationInsertUpdate";
        private const string PROC_PRIVACY_SEGMENTED_DATA_INSERT = "Clinical.sp_CCDA_PrivacySegmentedDataInsert";
        private const string PROC_PRIVACY_SEGMENTED_DATA_SELECT = "Clinical.sp_CCDA_PrivacySegmentedDataSelect";
        private const string PROC_USER_ISDATAPRIVACY_SELECT = "Patient.Sp_GetUserIsDataPrivacy";

        #endregion

        #region Parameters
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_VALUE_SETTING_ID = "@ValueSettingId";
        private const string PARM_IS_MEDICATION = "@IsMedication";
        private const string PARM_IS_ALLERGY = "@IsAllergy";
        private const string PARM_IS_PROBLEMLIST = "@IsProblemList";
        private const string PARM_IS_PATIENT_EDUCATION = "@IsPatientEducation";
        private const string PARM_IS_TOC = "@IsTOC";
        private const string PARM_IS_TOC_DELIVERED = "@IsTOCDelivered";
        private const string PARM_TOC_ID = "@TOCId";
        private const string PARM_IS_SUMMARY_OF_CARE = "@IsSummaryOfCare";
        private const string PARM_FILE_NAME = "@FileName";
        private const string PARM_URL = "@URL";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_DOB = "@DOB";
        private const string PARM_SSN = "@SSN";
        private const string PARM_MR_NUMBER = "@MRNumber";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_HOME_PHONE_NO = "@HomePhoneNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_BAD_ADDRESS = "@BadAddress";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_Page_NUMBER = "@PageNumber";
        private const string PARM_ROWS_P_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_IS_SEARCH = "@IsSearch";
        private const string PARM_COVERAGE_TYPE = "@CoverageType";
        private const string PARM_File_Stream = "@IsFileStream";
        private const string PARM_VISIT_DATE = "@VisitDate";
        private const string PARM_VISIT_TIME = "@VisitTime";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_B_MED_RECONCILED = "@bMedReconciled";
        private const string PARM_MED_RECONCILED_ID = "@MedReconciledId";
        private const string PARM_IS_PHONE_ENCOUNTER = "@IsPhoneEncounter";
        private const string PARM_TEMPLATE_TYPE_ID = "@TemplateTypeId";
        private const string PARM_NOTE_TEXT = "@NoteText";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_PROCEDURE_ID = "@ProcedureId";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_ACTION_RESULT = "@ActionResult";
        private const string PARM_NEGATION_VALUESET = "@NegationValueset";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_PROBLEMLIST_ID = "@ProblemListId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_DESCRIPTION = "@CPT_DESCRIPTION";
        private const string PARM_MODIFIER = "@Modifier";
        private const string PARM_UNIT = "@Unit";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_CPT_ID = "@CPTId";
        private const string PARM_VACCINEHX_ID = "@VaccineHxId";
        private const string PARM_TYPE = "@Type";
        private const string PARM_ADMINISTRATION_DATE = "@AdministrationDate";
        private const string PARM_VACCINE_GROUP_CATEGORY_ID = "@VaccineGroupCategory";
        private const string PARM_VACCINE = "@Vaccine";
        private const string PARM_IMMUNIZATION_THER_INJECTION_ID = "@ImmTherInjectionId";
        private const string PARM_IS_FROM_SUPPERBILL = "@IsFromSupperbill";
        private const string PARM_CUSTOM_FORM_ID = "@CustomFormId";
        private const string PARM_PROBLEM_NAME = "@ProblemName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_CHRONICITY_LEVEL = "@ChronicityLevel";
        private const string PARM_SEVERITY = "@Severity";
        private const string PARM_INACTIVE_CHKBOX_VALUE = "@InActiveChkBoxValue";
        private const string PARM_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_RCOPIA_ID = "@RcopiaID";
        private const string PARM_CODE = "@Code";
        private const string PARM_SOCIAL_HX_ID = "@SocialHxId";
        private const string PARM_CODE_TYPE = "@Codetype";
        private const string PARM_CODETYPE = "@CodeType";
        private const string PARM_ICD9 = "@ICD9";
        private const string PARM_ICD10 = "@ICD10";
        private const string PARM_ICD9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10_DESCRIPTION = "@ICD10_DESCRIPTION";
        private const string PARM_IS_CHIEFCOMPLAINT = "@IsChiefComplaint";
        private const string PARM_PROBLEM_ORDER = "@ProblemOrder";
        private const string PARM_CHKPROBLEM_EXISTS = "@CheckProblemExists";
        private const string PARM_PROB_COMPLAINT_ID = "@ProbComplaintId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_COMPLAINT_DETAIL_ID = "@ComplaintDetailId";
        private const string PARM_INTERVENTION_ID = "@InterventionId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_DATETIME = "@DateTime";
        private const string PARM_REASON = "@Reason";
        private const string PARM_NEGATION_CODE = "@NegationCode";
        private const string PARM_NEGATION_CODE_SYSTEM = "@NegationCodeSystem";
        private const string PARM_CODE_SYSTEM = "@CodeSystem";
        private const string PARM_ICD9_CODE = "@ICD9Code";
        private const string PARM_ICD10_CODE = "@ICD10Code";
        private const string PARM_LOINC_CODE = "@LOINCCode";
        private const string PARM_SNOMED_CODE = "@SNOMEDCode";
        private const string PARM_HCPCSCODE_CODE = "@HCPCSCode";
        private const string PARM_CODE_DESCRIPTION = "@CodeDescription";
        private const string PARM_SNOMEDDESCRIPTION = "@SNOMEDDescription";
        private const string PARM_ID = "@Id";
        private const string PARM_DIRECTION = "@Direction";
        private const string PARM_REFERENCES = "@References";
        private const string PARM_TIME = "@Time";
        private const string PARM_MEDICATION_ID = "@MedicationId";
        private const string PARM_RXNORM_CODE = "@RXNormCode";
        private const string PARM_DOSE_UNIT = "@DoseUnit";
        private const string PARM_DOSE_VALUE = "@DoseValue";
        private const string PARM_STOP_DATE = "@StopDate";
        private const string PARM_NEGATION_INDEX = "@NegationIndex";
        private const string PARM_NEGATION_REASON = "@NegationReason";
        private const string PARM_RADIOLOGY_ORDER_ID = "@RadiologyOrderId";
        private const string PARM_LOINC = "@LOINC";
        private const string PARM_ACTION_PERFORMED = "@ActionPerformed";
        public const string PARM_PROBLEM_ID = "@ProblemId";
        private const string PARM_CONFIDENTIALITY_CODE = "@ConfidentialityCode";
        private const string PARM_FILE_PATH = "@FilePath";

        #endregion

        #region _CCDS Stored Procedure Names

        private const string PROC_CCDA_PROBLEMLIST_INSERT = "[Clinical].[CCDA_ProblemListInsert]";
        private const string PROC_CCDA_VITALSIGNS_INSERT = "[Clinical].[CCDA__VitalSignsInsert]";
        private const string PROC_CCDA_PROCEDURES_INSERT = "[Clinical].[CCDA_ProceduresInsert]";
        private const string PROC_CCDA_ALLERGY_INSERT = "[Clinical].[sp_CCDA_AllergyInsert]";
        private const string PROC_CCDA_IMMUNIZATION_INSERT = "[Clinical].[sp_CCDA_ImmunizationInsert]";
        private const string PROC_CCDA_SOCIALHX_TOBACCO_INSERT = "[Clinical].[sp_CCDA_SocialHx_TobaccoInsert]";
        private const string PROC_CCDA_MEDICATION = "[Clinical].[SP_CCDA_Medication]";
        private const string PROC_CCDA_RESULTS = "[Clinical].[sp_CCDA_Results]";
        private const string PROC_CCDA_LABTEST_INSERT = "[Clinical].[sp_CCDA_LabTestInsert]";
        private const string PROC_CCDA_PATIENT_IMPLANTABLE_DEVICE = "[Clinical].[Sp_CCDA_PatientImplantableDevice]";
        private const string PROC_CCDA_GOAL = "[Clinical].[Sp_CCDA_Goal]";
        private const string PROC_CCDA_CAREPLAN_HEALTHCONCERNS_INSERT = "[Clinical].[sp_CCDA_CarePlan_HealthConcernsInsert]";
        private const string PROC_CCDA_PLANNED_ENCOUNTER = "[Clinical].[Sp_CCDA_PlannedEncounter]";

        private const string PROC_CCDA_COGNITIVE_FCMSTATUSINSERT = "[Clinical].[sp_CCDA_Cognitive_FCMStatusInsert]";
        private const string PROC_CCDA_REFERRALS_INSERT = "[Patient].[CCDA_sp_ReferralsInsert]";
        private const string PROC_CCDA_PATIENT_INSURANCE_INSERT = "[Patient].[CCDA_sp_PatientInsuranceInsert]";
        private const string PROC_CCDA_CAREPLANGOAL_INSERT = "[Clinical].[sp_CCDA_CarePlanGoalInsert]";
        private const string PROC_CCDA_CAREPLAN_OUTCOMESINSERT = "[Clinical].[sp_CCDA_CarePlan_OutcomesInsert]";
        private const string PROC_CCDA_CAREPLAN_INTERVENTIONS = "[Clinical].[sp_CCDA_CarePlanInterventions]";

        private const string PROC_CCDA_CARETEAMMEMBERS_CAREGIVERS = "[Clinical].[Sp_CCDA_CareTeamMembers_CareGivers]";

        #endregion _CCDS Stored Procedure Names

        #region Parameters CCDA

        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_COMPLAINT_COMMENTS = "@ComplaintComments";
        private const string PARM_IS_PROBLEM = "@IsProblem";

        #region Vital Signs

        private const string PARM_VITALSIGN_ID = "@VitalSignId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_HEIGHT = "@Height";
        private const string PARM_WEIGHT = "@Weight";
        private const string PARM_COPYPARENT_ID = "@CopyParentId";
        private const string PARM_SPO2 = "@SPO2";
        private const string PARM_OXYGEN_SOURCE = "@OxygenSource";
        private const string PARM_PEAKFLOW = "@PeakFlow";
        private const string PARM_PAIN_ID = "@PainId";
        private const string PARM_SMOKESTATUS_ID = "@SmokeStatusId";
        private const string PARM_VITALSIGN_DATE = "@VitalSignDate";
        private const string PARM_VITALSIGN_TIME = "@VitalSignTime";
        private const string PARM_BMI = "@BMI";
        private const string PARM_BSA = "@BSA";
        private const string PARM_HEADCR = "@HeadCr";
        private const string PARM_BLOOD_TYPE = "@BloodType";
        private const string PARM_DELETE_COMMENTS = "@DeleteComments";
        private const string PARM_RISK_ASSESSMENT_ID = "@RiskAssessmentId";
        private const string PARM_IS_FROMNOTE = "@IsFromNote";
        private const string PARM_DIASTOLIC = "@Diastolic";
        private const string PARM_DIASTOLIC_DATETIME = "@DiastolicDatetime";
        private const string PARM_SYSTOLIC = "@Systolic";
        private const string PARM_SYSTOLIC_DATETIME = "@SystolicDatetime";
        private const string PARM_PULSE = "@Pulse";
        private const string PARM_PULSE_DATETIME = "@PulseDatetime";
        private const string PARM_TEMPERATURE = "@Temperature";
        private const string PARM_TEMPERATURE_DATETIME = "@TemperatureDatetime";
        private const string PARM_RESPIRATION = "@Respiration";
        private const string PARM_RESPIRATION_DATETIME = "@RespirationDatetime";
        private const string PARM_LOINIC_SYSTOLIC = "@LOINICSystolic";
        private const string PARM_LOINIC_DIASTOLIC = "@LOINICDiastolic";

        #endregion Vital Signs

        #region Allergies

        private const string PARM_ALLERGY_ID = "@AllergyId";
        private const string PARM_ALLERGEN = "@Allergen";
        private const string PARM_REACTION = "@Reaction";
        private const string PARM_ONSET_DATE = "@OnSetDate";
        private const string PARM_LAST_MODIFIED = "@LastModified";
        private const string PARM_INACTIVE_CHECKBOXVALUE = "@InActiveCheckBoxValue";
        private const string PARM_RXNORM_ID = "@RxnormID";
        private const string PARM_RXNORMID_TYPE = "@RxnormType";
        private const string PARM_ISNEW_ROW = "@IsNewRow";
        private const string PARM_ISDELETED = "@IsDeleted";
        private const string PARM_TYPE_SNOMEDCODE = "@TypeSNOMEDCode";
        private const string PARM_ALLERGIES_ID = "@AllergiesId";

        #endregion Allergies

        #region Immunization

        private const string PARM_CVX_CODE = "@CVXCode";
        private const string PARM_CVXCODE_DESCRIPTION = "@CVXCodeDescription";
        private const string PARM_LOT_NUMBER = "@LotNumber";
        private const string PARM_MANUFACTURER = "@Manufacturer";
        private const string PARM_ROUTE_CODE = "@RouteCode";
        private const string PARM_ROUTE_DESCRIPTION = "@RouteDescription";
        private const string PARM_DOSE = "@Dose";
        private const string PARM_AMOUNT = "@Amount";
        private const string PARM_VACCINE_STATUS = "@VaccineStatus";
        private const string PARM_AMOUNT_DESCRIPTION = "@AmountDescription";

        #endregion Immunization

        #region Social History

        private const string PARM_TOBACCO_ID = "@TobaccoId";
        private const string PARM_USAGE_PERIOD = "@UsagePeriod";
        private const string PARM_FREQUENCY = "@Frequency";
        private const string PARM_COUNSELLING = "@Counselling";
        private const string PARM_COUNSELLING_TOPIC = "@CounsellingTopic";
        private const string PARM_CESSATION_LENGTH = "@CessationLength";
        private const string PARM_CESSATION_PERIOD = "@CessationPeriod";
        private const string PARM_RECENTLY_QUIT = "@RecentlyQuit";
        private const string PARM_WOULD_QUIT = "@WouldQuit";
        private const string PARM_SNOMEDCT_CODE = "@SNOMEDCTCode";
        private const string PARM_HISTORY_TYPE = "@HistoryType";
        private const string PARM_BIRTH_SEX = "@BirthSex";

        #endregion Social History

        #region Medications

        private const string PARM_BRAND_NAME = "@BrandName";
        private const string PARM_ACTION = "@Action";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_QUANTITY_UNIT = "@QuantityUnit";
        private const string PARM_ROUTE_BY = "@Routeby";
        private const string PARM_REPEAT_NUMBER = "@RepeatNumber";
        private const string PARM_REFILL = "@Refill";
        private const string PARM_DOSETIMING = "@DoseTiming";
        private const string PARM_SUBSTITUTION = "@Substitution";
        private const string PARM_OTHER_NOTE = "@OtherNote";
        private const string PARM_PATIEN_TNOTES = "@PatientNotes";
        private const string PARM_MEDICATIONS_ID = "@medicationsId";
        private const string PARM_INTENDED_USE = "@IntendedUse";


        #endregion Medications

        #region Result

        private const string PARM_LABORDER_RESULTID = "@LabOrderResultId";
        private const string PARM_OBSERVATION_DATE = "@ObservationDate";
        private const string PARM_LOINC_DESCRIPTION = "@LOINCDescription";
        private const string PARM_CPTCODE = "@CptCode";
        private const string PARM_CPTCODE_DESCRIPTION = "@CptCodeDescription";
        private const string PARM_RESULT_VALUE = "@ResultValue";
        private const string PARM_RANGE = "@Range";
        private const string PARM_CATEGORY_CODE = "@CategoryCode";
        private const string PARM_CATEGORY_NAME = "@CategoryName";
        private const string PARM_CATEGORYCODE_SYSTEM = "@CategoryCodeSystem";
        private const string PARM_UOM = "@UOM";
        private const string PARM_FLAG_CODE = "@FlagCode";
        private const string PARM_FLAG = "@Flag";

        #endregion Result

        #region Lab Test

        private const string PARM_LABTEST_ID = "@LabTestId";
        private const string PARM_LABORDER_TEST_ID = "@LabOrderTestId";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_IS_TEMPLATE = "@IsTemplate";

        #endregion Lab Test

        #region Implantable Device

        private const string PARM_IMPLANTABLEDEVICE_ID = "@ImplantableDeviceId";
        private const string PARM_PLACED_DATE = "@PlacedDate";
        private const string PARM_TARGET_SITE_CODE = "@TargetSiteCode";
        private const string PARM_TARGET_SITE = "@TargetSite";
        private const string PARM_UDI = "@UDI";
        private const string PARM_DEVICE_IDENTIFIER = "@DeviceIdentifier";
        private const string PARM_DEVICE_DESCRIPTION = "@DeviceDescription";
        private const string PARM_ISSUING_AGENCY = "@IssuingAgency";
        private const string PARM_MANUFACTURING_DATE = "@ManufacturingDate";
        private const string PARM_SERIAL_NUMBER = "@SerialNumber";
        private const string PARM_EXPIRATION_DATE = "@ExpirationDate";
        private const string PARM_VERSION_MODELNUMBER = "@VersionModelNumber";
        private const string PARM_COMPANY_NAME = "@CompanyName";
        private const string PARM_MRI_SAFETY_STATUS = "@MRISafetyStatus";
        private const string PARM_LABELED_CONTAINS_NRL = "@LabeledContainsNRL";
        private const string PARM_GMDNP_NAME = "@GMDNPName";

        #endregion Implantable Device

        #region Goals

        private const string PARM_ENROLLEDGOALS_ICDID = "@GoalId";
        private const string PARM_GOAL_DATETIME = "@GoalDateTime";
        private const string PARM_INSTRUCTION = "@Instruction";
        private const string PARM_CPTDESCRIPTION = "@CPTDescription";
        private const string PARM_ICDCODE = "@ICDCode";

        private const string PARM_ICDCODEDESCRIPTION = "@ICDCodeDescription";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        //private const string PARM_SNOMEDDESCRIPTION = "@SNOMEDDescription";
        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_PATIENT_PRIORITY = "@PatientPriority";
        private const string PARM_PATIENT_PRIORITYCODE = "@PatientPriorityCode";
        private const string PARM_PROVIDER_PRIORITY = "@ProviderPriority";
        private const string PARM_PROVIDER_PRIORITYCODE = "@ProviderPriorityCode";

        private const string PARM_VALUE = "@Value";

        #endregion Goals

        #region Health Concerns

        private const string PARM_HEALTHCONCERN_ID = "@HealthConcernId";
        private const string PARM_CONCERNS_DATE = "@Concerns_Date";
        private const string PARM_OBSERVATION_DATETIME = "@Observation_Date";
        private const string PARM_CONCERNS_ICD9_CODE = "@Concerns_ICD9Code";
        private const string PARM_CONCERNS_STATUS = "@Concerns_Status";
        private const string PARM_OBSERVATION_STATUS = "@Observation_Status";
        private const string PARM_CONCERNS_ICD9CODE_DESCRIPTION = "@Concerns_ICD9CodeDescription";
        private const string PARM_CONCERNS_ICD10_CODE = "@Concerns_ICD10Code";
        private const string PARM_CONCERNS_ICD10CODE_DESCRIPTION = "@Concerns_ICD10CodeDescription";
        private const string PARM_CONCERNS_SNOMED_ID = "@Concerns_SNOMEDID";
        private const string PARM_CONCERNS_SNOMED_DESCRIPTION = "@Concerns_SNOMEDDescription";
        private const string PARM_OBSERVATION_ICD9CODE = "@Observation_ICD9Code";
        private const string PARM_OBSERVATION_ICD9CODE_DESCRIPTION = "@Observation_ICD9CodeDescription";
        private const string PARM_OBSERVATION_ICD10CODE = "@Observation_ICD10Code";
        private const string PARM_OBSERVATION_ICD10CODE_DESCRIPTION = "@Observation_ICD10CodeDescription";
        private const string PARM_OBSERVATION_SNOMEDID = "@Observation_SNOMEDID";
        private const string PARM_OBSERVATION_SNOMED_DESCRIPTION = "@Observation_SNOMEDDescription";

        private const string PARM_OBSPATIENT_PRIORITYCODE = "@ObsPatientPriorityCode";
        private const string PARM_OBSPATIENT_PRIORITY = "@ObsPatientPriority";
        private const string PARM_OBSPROVIDER_PRIORITYCODE = "@ObsProviderPriorityCode";
        private const string PARM_OBSPROVIDER_PRIORITY = "@ObsProviderPriority";
        private const string PARM_RISK_ICD9CODE = "@RiskICD9Code";
        private const string PARM_RISK_ICD10CODE = "@RiskICD10Code";
        private const string PARM_RISK_ICD9DESCRIPTION = "@RiskICD9Description";
        private const string PARM_RISK_ICD10DESCRIPTION = "@RiskICD10Description";
        private const string PARM_RISK_DATE = "@RiskDate";
        private const string PARM_RISK_STATUS = "@RiskStatus";
        private const string PARM_RISKPATIENT_PRIORITYCODE = "@RiskPatientPriorityCode";
        private const string PARM_RISKPATIENT_PRIORITY = "@RiskPatientPriority";
        private const string PARM_RISKPROVIDER_PRIORITYCODE = "@RiskProviderPriorityCode";
        private const string PARM_RISKPROVIDER_PRIORITY = "@RiskProviderPriority";
        private const string PARM_RISK_SNOMEDID = "@RiskSNOMEDID";
        private const string PARM_RISK_SNOMEDDESC = "@RiskSNOMEDDesc";

        #endregion Health Concerns

        #region Cognitive, Functional and Mental Status

        private const string PARM_LEXI_CODE = "@LexiCode";
        private const string PARM_LEXICODE_DESCRIPTION = "@LexiCodeDescription";
        private const string PARM_FUNCTIONAL_STATUS_ID = "@FunctionalStatusId";
        private const string PARM_ICD9CODE_DESCRIPTION = "@ICD9CodeDescription";
        private const string PARM_ICD10CODE_DESCRIPTION = "@ICD10CodeDescription";

        #endregion Cognitive, Functional and Mental Status

        #region Referral Reason

        private const string PARM_REFERRAL_ID = "@ReferralId";
        private const string PARM_FACILITY_FROM = "@FacilityFrom";

        #endregion Referral Reason

        #region Insurance Provider

        private const string PARM_Plan_Priority = "@PlanPriority";
        private const string PARM_Suscriber_FirstName = "@SuscriberFirstName";
        private const string PARM_Suscriber_MiddleName = "@SuscriberMiddleName";
        private const string PARM_Suscriber_LastName = "@SuscriberLastName";
        private const string PARM_Suscriber_DOB = "@SuscriberDOB";
        private const string PARM_InsuranceProvider_Name = "@InsuranceProviderName";
        private const string PARM_InsuranceProvider_Phone = "@InsuranceProviderPhone";
        private const string PARM_InsuranceProvider_Street = "@InsuranceProviderStreet";
        private const string PARM_InsuranceProvider_City = "@InsuranceProviderCity";
        private const string PARM_InsuranceProvider_State = "@InsuranceProviderState";
        private const string PARM_InsuranceProvider_PostalCode = "@InsuranceProviderPostalCode";
        private const string PARM_InsuranceProvider_Address1 = "@InsuranceProviderAddress1";
        private const string PARM_Subscriber_RelationshipCode = "@SubscriberRelationshipCode";
        private const string PARM_Insurance_Number = "@InsuranceNumber";
        private const string PARM_Insurance_GroupId = "@InsuranceGroupId";
        private const string PARM_Subscriber_Relationship_DisplayName = "@SubscriberRelationshipDisplayName";


        #endregion Insurance Provider

        #region Care Plan Outcome

        private const string PARM_OUTCOMES_ID = "@OutcomesId";
        private const string PARM_CODESYSTEMNAME = "@CodeSystemName";
        private const string PARM_ICD9DESCRIPTION = "@ICD9Description";
        private const string PARM_ICD10DESCRIPTION = "@ICD10Description";


        #endregion  Care Plan Outcome

        #region Care team members

        private const string PARM_CARETEAM_ID = "@CareTeamId";
        private const string PARM_CAREPROVIDER_TYPECODE = "@CareProviderTypeCode";
        private const string PARM_CAREPROVIDER_TYPE = "@CareProviderType";
        private const string PARM_MEMBER_CODE = "@MemberCode";
        private const string PARM_MEMBER_CODENAME = "@MemberCodeName";
        private const string PARM_PREFIX = "@Prefix";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_CITY = "@City";
        private const string PARM_State = "@State";
        private const string PARM_ZIPCODE = "@ZIPCode";
        private const string PARM_ORGANIZATION = "@Organization";
        private const string PARM_ORGANIZATION_TELEPHONE = "@OrganizationTelephone";
        private const string PARM_ORGANIZATION_CITY = "@OrganizationCity";
        private const string PARM_ORGANIZATION_STATE = "@OrganizationState";
        private const string PARM_ORGANIZATION_ZIPCODE = "@OrganizationZIPCode";
        private const string PARM_ORGANIZATION_ADDRESS1 = "@OrganizationAddress1";


        #endregion Care team members

        #endregion Parameters CCDA

        #region General
        private void CreateSelectParameters(IDBManager dbManager, DSPatient ds)
        {
            if (ds.Patients.Rows.Count > 0)
            {
                dbManager.CreateParameters(23);

                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString() == "-1" ? null : ds.Patients.Rows[0][ds.Patients.PatientIdColumn.ColumnName]);
                dbManager.AddParameters(1, PARM_ACCOUNT_NUMBER, ds.Patients.Rows[0][ds.Patients.AccountNumberColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.AccountNumberColumn.ColumnName]);
                dbManager.AddParameters(2, PARM_LAST_NAME, ds.Patients.Rows[0][ds.Patients.LastNameColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.LastNameColumn.ColumnName]);
                dbManager.AddParameters(3, PARM_FIRST_NAME, ds.Patients.Rows[0][ds.Patients.FirstNameColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.FirstNameColumn.ColumnName]);
                dbManager.AddParameters(4, PARM_SSN, ds.Patients.Rows[0][ds.Patients.SSNColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.SSNColumn.ColumnName]);
                dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.Patients.Rows[0][ds.Patients.IsActiveColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.IsActiveColumn.ColumnName]);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (ds.Patients.Rows[0][ds.Patients.DOBColumn.ColumnName].ToString() != "" && Convert.ToDateTime(ds.Patients.Rows[0][ds.Patients.DOBColumn.ColumnName]) != DateTime.MinValue)
                    dbManager.AddParameters(7, PARM_DOB, ds.Patients.Rows[0][ds.Patients.DOBColumn.ColumnName]);
                else
                    dbManager.AddParameters(7, PARM_DOB, null);
                dbManager.AddParameters(8, PARM_MR_NUMBER, ds.Patients.Rows[0][ds.Patients.MRNumberColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.MRNumberColumn.ColumnName]);
                dbManager.AddParameters(9, PARM_PROVIDER_ID, ds.Patients.Rows[0][ds.Patients.ProviderIdColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.ProviderIdColumn.ColumnName]);
                dbManager.AddParameters(10, PARM_FACILITY_ID, ds.Patients.Rows[0][ds.Patients.FacilityIdColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.FacilityIdColumn.ColumnName]);
                dbManager.AddParameters(11, PARM_PRACTICE_ID, ds.Patients.Rows[0][ds.Patients.PracticeIdColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.PracticeIdColumn.ColumnName]);
                dbManager.AddParameters(12, PARM_HOME_PHONE_NO, ds.Patients.Rows[0][ds.Patients.HomePhoneNoColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.HomePhoneNoColumn.ColumnName]);
                dbManager.AddParameters(13, PARM_EMAIL_ADDRESS, ds.Patients.Rows[0][ds.Patients.EmailAddressColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.EmailAddressColumn.ColumnName]);
                dbManager.AddParameters(14, PARM_BAD_ADDRESS, ds.Patients.Rows[0][ds.Patients.BadAddressColumn.ColumnName]);
                dbManager.AddParameters(15, PARM_GENDER, ds.Patients.Rows[0][ds.Patients.GenderColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.GenderColumn.ColumnName]);
                dbManager.AddParameters(16, PARM_Page_NUMBER, ds.Patients.Rows[0][ds.Patients.PageNumberColumn.ColumnName].ToString() == "" ? 1 : ds.Patients.Rows[0][ds.Patients.PageNumberColumn.ColumnName]);
                dbManager.AddParameters(17, PARM_ROWS_P_PAGE, ds.Patients.Rows[0][ds.Patients.RowspPageColumn.ColumnName].ToString() == "" ? 1 : ds.Patients.Rows[0][ds.Patients.RowspPageColumn.ColumnName]);
                dbManager.AddParameters(18, PARM_RECORD_COUNT, ds.Patients.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(19, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(20, PARM_IS_SEARCH, ds.Patients.Rows[0][ds.Patients.IsSearchColumn.ColumnName].ToString() == "" ? false : ds.Patients.Rows[0][ds.Patients.IsSearchColumn.ColumnName]);
                dbManager.AddParameters(21, PARM_COVERAGE_TYPE, ds.Patients.Rows[0][ds.Patients.CoverageTypeColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.CoverageTypeColumn.ColumnName]);
                dbManager.AddParameters(22, PARM_File_Stream, ds.Patients.Rows[0][ds.Patients.IsFileStreamColumn.ColumnName].ToString() == "" ? false : ds.Patients.Rows[0][ds.Patients.IsFileStreamColumn.ColumnName]);
            }

        }
        private void CreateEncounterInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(22);

            dbManager.AddParameters(0, PARM_NOTES_ID, ds.NotesEncounter.NotesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_NOTE_TEXT, ds.NotesEncounter.NoteTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_TEMPLATE_TYPE_ID, ds.NotesEncounter.TemplateTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_VISIT_DATE, ds.NotesEncounter.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_VISIT_TIME, ds.NotesEncounter.VisitTimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.NotesEncounter.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.NotesEncounter.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.NotesEncounter.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.NotesEncounter.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.NotesEncounter.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_PROVIDER_ID, ds.NotesEncounter.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_FACILITY_ID, ds.NotesEncounter.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_ENTITY_ID, ds.NotesEncounter.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_PATIENT_ID, ds.NotesEncounter.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_IS_PHONE_ENCOUNTER, ds.NotesEncounter.IsPhoneEncounterColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(15, PARM_CODE, ds.NotesEncounter.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CODE_TYPE, ds.NotesEncounter.CodeTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_B_MED_RECONCILED, ds.NotesEncounter.bMedReconciledColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(18, PARM_MED_RECONCILED_ID, ds.NotesEncounter.MedReconciledIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_START_DATE, ds.NotesEncounter.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_END_DATE, ds.NotesEncounter.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_ACTION_RESULT, ds.NotesEncounter.ActionResultColumn.ColumnName, DbType.String);
        }

        
        private void CreateProcedureInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(28);

            dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.NotesProcedures.ProcedureIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_START_DATE, ds.NotesProcedures.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(2, PARM_END_DATE, ds.NotesProcedures.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_COMMENTS, ds.NotesProcedures.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.NotesProcedures.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_NOTES_ID, ds.NotesProcedures.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.NotesProcedures.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(7, PARM_PROBLEMLIST_ID, ds.NotesProcedures.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.NotesProcedures.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.NotesProcedures.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.NotesProcedures.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.NotesProcedures.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_CPT_CODE, ds.NotesProcedures.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CPT_DESCRIPTION, ds.NotesProcedures.CPT_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIER, ds.NotesProcedures.ModifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_UNIT, ds.NotesProcedures.UnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SNOMED_ID, ds.NotesProcedures.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_SNOMED_DESCRIPTION, ds.NotesProcedures.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CPT_ID, ds.NotesProcedures.CPTIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_VACCINEHX_ID, ds.NotesProcedures.VaccineHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(20, PARM_IMMUNIZATION_THER_INJECTION_ID, ds.NotesProcedures.ImmTherInjectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(21, PARM_IS_FROM_SUPPERBILL, ds.NotesProcedures.IsFromSupperbillColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(22, PARM_CUSTOM_FORM_ID, ds.NotesProcedures.CustomFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(23, PARM_PROVIDER_ID, ds.NotesProcedures.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(24, PARM_FACILITY_ID, ds.NotesProcedures.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(25, PARM_NEGATION_INDEX, ds.NotesProcedures.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(26, PARM_NEGATION_REASON, ds.NotesProcedures.NegationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_NEGATION_VALUESET, ds.NotesProcedures.NegationValuesetColumn.ColumnName, DbType.String);

        }

        private void CreateDiagnosisInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(35);

            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.Diagnosis.ProblemListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROBLEM_NAME, ds.Diagnosis.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Diagnosis.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CHRONICITY_LEVEL, ds.Diagnosis.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.Diagnosis.SeverityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_START_DATE, ds.Diagnosis.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_END_DATE, ds.Diagnosis.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.Diagnosis.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PATIENT_ID, ds.Diagnosis.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_NOTE_ID, ds.Diagnosis.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.Diagnosis.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.Diagnosis.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.Diagnosis.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.Diagnosis.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.Diagnosis.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_INACTIVE_CHKBOX_VALUE, ds.Diagnosis.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_INACTIVE_REASON, ds.Diagnosis.InActiveReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_RCOPIA_ID, ds.Diagnosis.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CODE, ds.Diagnosis.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CODE_TYPE, ds.Diagnosis.CodetypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ICD9, ds.Diagnosis.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_ICD10, ds.Diagnosis.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_SNOMED_ID, ds.Diagnosis.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_SNOMED_DESCRIPTION, ds.Diagnosis.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_ICD9_DESCRIPTION, ds.Diagnosis.ICD9_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_ICD10_DESCRIPTION, ds.Diagnosis.ICD10_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_IS_CHIEFCOMPLAINT, ds.Diagnosis.IsChiefComplaintColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(27, PARM_PROBLEM_ORDER, ds.Diagnosis.ProblemOrderColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(28, PARM_CUSTOM_FORM_ID, ds.Diagnosis.CustomFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(29, PARM_CHKPROBLEM_EXISTS, ds.Diagnosis.CheckProblemExistsColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(30, PARM_PROB_COMPLAINT_ID, ds.Diagnosis.ProbComplaintIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(31, PARM_COMPLAINT_DETAIL_ID, ds.Diagnosis.ComplaintDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(32, PARM_PROVIDER_ID, ds.Diagnosis.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(33, PARM_FACILITY_ID, ds.Diagnosis.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(34, PARM_ACTION_PERFORMED, ds.Diagnosis.ActionPerformedColumn.ColumnName, DbType.String);
        }

        private void CreateInterventionInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(18);


            dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.Interventions.ProcedureIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_START_DATE, ds.Interventions.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(2, PARM_END_DATE, ds.Interventions.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_PATIENT_ID, ds.Interventions.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.Interventions.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.Interventions.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.Interventions.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.Interventions.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.Interventions.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_CPT_CODE, ds.Interventions.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SNOMED_ID, ds.Interventions.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_PROVIDER_ID, ds.Interventions.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_FACILITY_ID, ds.Interventions.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_NEGATION_INDEX, ds.Interventions.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(14, PARM_NEGATION_REASON, ds.Interventions.NegationCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_STATUS, ds.Interventions.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ACTION_RESULT, ds.Interventions.ActionResultColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_NEGATION_VALUESET, ds.Interventions.NegationValuesetColumn.ColumnName, DbType.String);
        }

        private void CreateImmunizationInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(17);

            dbManager.AddParameters(0, PARM_VACCINEHX_ID, ds.Immunization.VaccineHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_VACCINE_GROUP_CATEGORY_ID, ds.Immunization.VaccineGroupCategoryColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_VACCINE, ds.Immunization.VaccineColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PATIENT_ID, ds.Immunization.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_ADMINISTRATION_DATE, ds.Immunization.AdministrationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.Immunization.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_TYPE, ds.Immunization.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PROVIDER_ID, ds.Immunization.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.Immunization.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.Immunization.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.Immunization.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.Immunization.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_FACILITY_ID, ds.Immunization.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_NEGATION_INDEX, ds.Immunization.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(14, PARM_NEGATION_REASON, ds.Immunization.NegationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_NEGATION_VALUESET, ds.Immunization.NegationValuesetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_COMMENTS, ds.Immunization.CommentsColumn.ColumnName, DbType.String);
        }

        private void CreateMedicationInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(18);

            dbManager.AddParameters(0, PARM_MEDICATION_ID, ds.Medication.MedicationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.Medication.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_RXNORM_CODE, ds.Medication.RxNormCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_DOSE_UNIT, ds.Medication.DoseUnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_DOSE_VALUE, ds.Medication.DoseValueColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_START_DATE, ds.Medication.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_STOP_DATE, ds.Medication.StopDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.Medication.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.Medication.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.Medication.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.Medication.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_PROVIDER_ID, ds.Medication.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_FACILITY_ID, ds.Medication.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_NEGATION_INDEX, ds.Medication.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(14, PARM_NEGATION_REASON, ds.Medication.NegationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ACTION_RESULT, ds.Medication.ActionResultColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_NEGATION_VALUESET, ds.Medication.NegationValuesetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_DURATION, ds.Medication.DurationColumn.ColumnName, DbType.Int64);
        }

        private void CreateCommProviderToProviderInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(14);

            dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.ComProviderToProvider.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_CPT_CODE, ds.ComProviderToProvider.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_CPT_DESCRIPTION, ds.ComProviderToProvider.CPT_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_SNOMED_ID, ds.ComProviderToProvider.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_START_DATE, ds.ComProviderToProvider.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_END_DATE, ds.ComProviderToProvider.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.ComProviderToProvider.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.ComProviderToProvider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.ComProviderToProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.ComProviderToProvider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.ComProviderToProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_PATIENT_ID, ds.ComProviderToProvider.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_PROVIDER_ID, ds.ComProviderToProvider.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_FACILITY_ID, ds.ComProviderToProvider.FacilityIdColumn.ColumnName, DbType.Int64);

        }

        private void CreateCommPatientToProviderInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(14);

            dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.ComPatientToProvider.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_CPT_CODE, ds.ComPatientToProvider.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_CPT_DESCRIPTION, ds.ComPatientToProvider.CPT_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_SNOMED_ID, ds.ComPatientToProvider.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_START_DATE, ds.ComPatientToProvider.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_END_DATE, ds.ComPatientToProvider.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.ComPatientToProvider.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.ComPatientToProvider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.ComPatientToProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.ComPatientToProvider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.ComPatientToProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_PATIENT_ID, ds.ComPatientToProvider.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_PROVIDER_ID, ds.ComPatientToProvider.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_FACILITY_ID, ds.ComPatientToProvider.FacilityIdColumn.ColumnName, DbType.Int64);

        }


        private void CreatePatientCharacteristicsInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(13);

            dbManager.AddParameters(0, PARM_SOCIAL_HX_ID, ds.PatientCharacteristics.SocialHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_CODE, ds.PatientCharacteristics.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DATETIME, ds.PatientCharacteristics.TimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.PatientCharacteristics.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.PatientCharacteristics.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.PatientCharacteristics.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.PatientCharacteristics.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.PatientCharacteristics.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_PATIENT_ID, ds.PatientCharacteristics.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_PROVIDER_ID, ds.PatientCharacteristics.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_FACILITY_ID, ds.PatientCharacteristics.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_START_DATE, ds.PatientCharacteristics.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_END_DATE, ds.PatientCharacteristics.EndDateColumn.ColumnName, DbType.DateTime);

        }

        private void CreatePatientDiagnosticStudyInsertUpdateParameters(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(12);

            dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, ds.DiagnosticStudy.RadiologyOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.DiagnosticStudy.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.DiagnosticStudy.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FACILITY_ID, ds.DiagnosticStudy.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_START_DATE, ds.DiagnosticStudy.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_END_DATE, ds.DiagnosticStudy.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.DiagnosticStudy.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.DiagnosticStudy.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.DiagnosticStudy.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.DiagnosticStudy.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_LOINC, ds.DiagnosticStudy.LOINCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ACTION_PERFORMED, ds.DiagnosticStudy.ActionPerformedColumn.ColumnName, DbType.String);



        }

        private void CreatePrivacySegmentedDocumentInsertParameters(IDBManager dbManager, DSPatient ds)
        {
            dbManager.CreateParameters(13);

            dbManager.AddParameters(0, PARM_ID, PARM_ID, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_ENTITY_ID, ds.Patients.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FIRST_NAME, ds.Patients.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MI, ds.Patients.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_LAST_NAME, ds.Patients.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DOB, ds.Patients.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_CONFIDENTIALITY_CODE, ds.Patients.ConfidentialityCodeColumn.ColumnName, DbType.StringFixedLength);
            dbManager.AddParameters(7, PARM_FILE_PATH, ds.Patients.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.Patients.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.Patients.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.Patients.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.Patients.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.Patients.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region Private functions
        public DSPatient LoadPatient(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateSelectParameters(dbManager, ds);
                DSPatient dsPatient = new DSPatient();
                dsPatient = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_SELECT, dsPatient, ds.Patients.TableName);

                return dsPatient;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadPatient", PROC_PATIENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region _CCDA

        #region Insurance Provider

        public void InsertUpdateInsuranceProvider_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.InsuranceProviderInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_PATIENT_INSURANCE_INSERT, ds, ds.InsuranceProvider.TableName);
                dbManager.CommitTransaction();
                ds.InsuranceProvider.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateInsuranceProvider_CCDAData", PROC_CCDA_PATIENT_INSURANCE_INSERT, ex);
                throw ex;
            }
        }

        private void InsuranceProviderInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(26);
            dbManager.AddParameters(0, PARM_PROVIDER_ID, ds.InsuranceProvider.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.InsuranceProvider.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.InsuranceProvider.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.InsuranceProvider.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.InsuranceProvider.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.InsuranceProvider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.InsuranceProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.InsuranceProvider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.InsuranceProvider.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_STATUS, ds.InsuranceProvider.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_Plan_Priority, ds.InsuranceProvider.PriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_Suscriber_FirstName, ds.InsuranceProvider.SuscriberFirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_Suscriber_MiddleName, ds.InsuranceProvider.SuscriberMiddleNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_Suscriber_LastName, ds.InsuranceProvider.SuscriberLastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_Suscriber_DOB, ds.InsuranceProvider.SuscriberDOBColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_InsuranceProvider_Name, ds.InsuranceProvider.InsuranceProviderNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_InsuranceProvider_Phone, ds.InsuranceProvider.InsuranceProviderPhoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_InsuranceProvider_Street, ds.InsuranceProvider.InsuranceProviderStreetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_InsuranceProvider_City, ds.InsuranceProvider.InsuranceProviderCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_InsuranceProvider_State, ds.InsuranceProvider.InsuranceProviderStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_InsuranceProvider_PostalCode, ds.InsuranceProvider.InsuranceProviderPostalCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_InsuranceProvider_Address1, ds.InsuranceProvider.InsuranceProviderAddress1Column.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_Subscriber_RelationshipCode, ds.InsuranceProvider.SubscriberRelationshipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_Insurance_Number, ds.InsuranceProvider.InsuranceNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_Insurance_GroupId, ds.InsuranceProvider.InsuranceGroupIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_Subscriber_Relationship_DisplayName, ds.InsuranceProvider.SubscriberRelationshipDisplayNameColumn.ColumnName, DbType.String);
        }

        #endregion Insurance Provider

        #region Referral Reason

        public void InsertUpdateReferralReason_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.ReferralReasonInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_REFERRALS_INSERT, ds, ds.ReasonForReferral.TableName);
                dbManager.CommitTransaction();
                ds.ReasonForReferral.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateReferralReason_CCDAData", PROC_CCDA_REFERRALS_INSERT, ex);
                throw ex;
            }
        }

        private void ReferralReasonInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(10);
            dbManager.AddParameters(0, PARM_REFERRAL_ID, ds.ReasonForReferral.ReferralIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.ReasonForReferral.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.ReasonForReferral.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_REASON, ds.ReasonForReferral.ReferralNoteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.ReasonForReferral.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.ReasonForReferral.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.ReasonForReferral.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.ReasonForReferral.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.ReasonForReferral.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_FACILITY_FROM, ds.ReasonForReferral.FacilityIdColumn.ColumnName, DbType.Int64);
        }

        #endregion Referral Reason

        #region Cognitive Status

        public void InsertUpdateCognitiveStatus_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.CognitiveStatusInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_COGNITIVE_FCMSTATUSINSERT, ds, ds.FunctionalStatus.TableName);
                dbManager.CommitTransaction();
                ds.FunctionalStatus.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCognitiveStatus_CCDAData", PROC_CCDA_COGNITIVE_FCMSTATUSINSERT, ex);
                throw ex;
            }
        }

        private void CognitiveStatusInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(22);
            dbManager.AddParameters(0, PARM_FUNCTIONAL_STATUS_ID, ds.FunctionalStatus.FunctionalStatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_USER_ID, ds.FunctionalStatus.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.FunctionalStatus.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.FunctionalStatus.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.FunctionalStatus.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_TYPE, ds.FunctionalStatus.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ICD9_CODE, ds.FunctionalStatus.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ICD9CODE_DESCRIPTION, ds.FunctionalStatus.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ICD10_CODE, ds.FunctionalStatus.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_ICD10CODE_DESCRIPTION, ds.FunctionalStatus.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SNOMED_ID, ds.FunctionalStatus.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SNOMEDDESCRIPTION, ds.FunctionalStatus.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_LEXI_CODE, ds.FunctionalStatus.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_LEXICODE_DESCRIPTION, ds.FunctionalStatus.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_INSTRUCTION, ds.FunctionalStatus.InstructionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_COMMENTS, ds.FunctionalStatus.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_IS_ACTIVE, ds.FunctionalStatus.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(17, PARM_CREATED_BY, ds.FunctionalStatus.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CREATED_ON, ds.FunctionalStatus.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_MODIFIED_BY, ds.FunctionalStatus.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_MODIFIED_ON, ds.FunctionalStatus.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_SOAP_TEXT, ds.FunctionalStatus.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion Cognitive Status

        #region Encounters

        public void InsertUpdateEncounter_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.EncounterInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_PLANNED_ENCOUNTER, ds, ds.NotesEncounter.TableName);
                dbManager.CommitTransaction();
                ds.NotesEncounter.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateEncounter_CCDAData", PROC_CCDA_PLANNED_ENCOUNTER, ex);
                throw ex;
            }
        }

        private void EncounterInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(17);
            dbManager.AddParameters(0, PARM_VISIT_ID, ds.NotesEncounter.EncounterIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.NotesEncounter.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.NotesEncounter.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FACILITY_ID, ds.NotesEncounter.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.NotesEncounter.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.NotesEncounter.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.NotesEncounter.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.NotesEncounter.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.NotesEncounter.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.NotesEncounter.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_STATUS, ds.NotesEncounter.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_START_DATE, ds.NotesEncounter.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_END_DATE, ds.NotesEncounter.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_VISIT_DATE, ds.NotesEncounter.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_CODE, ds.NotesEncounter.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CODETYPE, ds.NotesEncounter.CodeTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_REASON, ds.NotesEncounter.EncounterColumn.ColumnName, DbType.String);
        }

        #endregion Encounters

        #region Lab Test

        public void InsertUpdateLabTest_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.LabTestInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_LABTEST_INSERT, ds, ds.LabOrderTest.TableName);
                dbManager.CommitTransaction();
                ds.LabOrderTest.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateLabTest_CCDAData", PROC_CCDA_LABTEST_INSERT, ex);
                throw ex;
            }
        }

        private void LabTestInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(18);
            dbManager.AddParameters(0, PARM_LABTEST_ID, ds.LabOrderTest.LabTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.LabOrderTest.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.LabOrderTest.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.LabOrderTest.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_USER_ID, ds.LabOrderTest.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.LabOrderTest.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_LABORDER_TEST_ID, ds.LabOrderTest.LabOrderTestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_LAB_ID, ds.LabOrderTest.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_LOINC, ds.LabOrderTest.LOINCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_LOINC_DESCRIPTION, ds.LabOrderTest.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_TEST_DATE, ds.LabOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_STATUS, ds.LabOrderTest.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_TEMPLATE, ds.LabOrderTest.IsTemplateColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.LabOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.LabOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CREATED_ON, ds.LabOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_MODIFIED_BY, ds.LabOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIED_ON, ds.LabOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion Lab Test

        #region Immunization

        public void InsertUpdateImmunization_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.ImmunizationInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_IMMUNIZATION_INSERT, ds, ds.Immunization.TableName);
                dbManager.CommitTransaction();
                ds.Immunization.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateImmunization_CCDAData", PROC_CCDA_IMMUNIZATION_INSERT, ex);
                throw ex;
            }
        }

        private void ImmunizationInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(25);
            dbManager.AddParameters(0, PARM_VACCINEHX_ID, ds.Immunization.VaccineHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.Immunization.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.Immunization.GivenByColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Immunization.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.Immunization.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_ADMINISTRATION_DATE, ds.Immunization.AdministrationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_CVX_CODE, ds.Immunization.VaccineColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CVXCODE_DESCRIPTION, ds.Immunization.VaccineDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_LOT_NUMBER, ds.Immunization.LotNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MANUFACTURER, ds.Immunization.ManufacturerColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_NEGATION_INDEX, ds.Immunization.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(11, PARM_NEGATION_REASON, ds.Immunization.NegationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_NEGATION_CODE, ds.Immunization.NegationCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_TYPE, ds.Immunization.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ROUTE_CODE, ds.Immunization.RouteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ROUTE_DESCRIPTION, ds.Immunization.RouteDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_DOSE, ds.Immunization.DoseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_AMOUNT, ds.Immunization.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CREATED_BY, ds.Immunization.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CREATED_ON, ds.Immunization.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_MODIFIED_BY, ds.Immunization.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_MODIFIED_ON, ds.Immunization.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_VACCINE_STATUS, ds.Immunization.VaccineStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_CPT_CODE, ds.Immunization.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_AMOUNT_DESCRIPTION, ds.Immunization.AmountDescriptionColumn.ColumnName, DbType.String);
        }

        #endregion Immunization

        #region Allergies

        public void InsertUpdateAllergies_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.AllergiesInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_ALLERGY_INSERT, ds, ds.Allergy.TableName);
                dbManager.CommitTransaction();
                ds.Allergy.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateAllergies_CCDAData", PROC_CCDA_ALLERGY_INSERT, ex);
                throw ex;
            }
        }

        private void AllergiesInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(16);
            dbManager.AddParameters(0, PARM_ALLERGY_ID, ds.Allergy.AllergyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.Allergy.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ONSET_DATE, ds.Allergy.OnSetDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_LAST_MODIFIED, ds.Allergy.LastModifiedColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_TYPE, ds.Allergy.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_TYPE_SNOMEDCODE, ds.Allergy.TypeSNOMEDCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_RXNORM_ID, ds.Allergy.RxnormIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_RXNORMID_TYPE, ds.Allergy.RxnormIDTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ALLERGEN, ds.Allergy.AllergenColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_SEVERITY, ds.Allergy.SeverityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_REACTION, ds.Allergy.ReactionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_STATUS, ds.Allergy.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.Allergy.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.Allergy.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.Allergy.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ALLERGIES_ID, ds.Allergy.AllergiesIdColumn.ColumnName, DbType.Int64);
        }

        #endregion Allergies

        #region Vital Signs

        public void InsertUpdateVitalSigns_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.VitalSignsInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_VITALSIGNS_INSERT, ds, ds.VitalSigns.TableName);
                dbManager.CommitTransaction();
                ds.VitalSigns.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateVitalSigns_CCDAData", PROC_CCDA_VITALSIGNS_INSERT, ex);
                throw ex;
            }
        }

        private void VitalSignsInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(39);
            dbManager.AddParameters(0, PARM_VITALSIGN_ID, ds.VitalSigns.VitalSignIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.VitalSigns.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_VISIT_ID, ds.VitalSigns.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_HEIGHT, ds.VitalSigns.HeightColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_WEIGHT, ds.VitalSigns.WeightColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.VitalSigns.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.VitalSigns.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.VitalSigns.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.VitalSigns.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.VitalSigns.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_NOTES_ID, ds.VitalSigns.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_COPYPARENT_ID, ds.VitalSigns.CopyParentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_SPO2, ds.VitalSigns.SPO2Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_OXYGEN_SOURCE, ds.VitalSigns.OxygenSourceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PEAKFLOW, ds.VitalSigns.PeakFlowColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_PAIN_ID, ds.VitalSigns.PainIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(16, PARM_SMOKESTATUS_ID, ds.VitalSigns.SmokeStatusIdColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(17, PARM_COMMENTS, ds.VitalSigns.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_VITALSIGN_DATE, ds.VitalSigns.VitalSignDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_VITALSIGN_TIME, ds.VitalSigns.VitalSignTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_BMI, ds.VitalSigns.BMIColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(21, PARM_BSA, ds.VitalSigns.BSAColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(22, PARM_HEADCR, ds.VitalSigns.HeadCrColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(23, PARM_BLOOD_TYPE, ds.VitalSigns.BloodTypeColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(24, PARM_DELETE_COMMENTS, ds.VitalSigns.DeleteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_RISK_ASSESSMENT_ID, ds.VitalSigns.RiskAssessmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_IS_FROMNOTE, ds.VitalSigns.IsFromNoteColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(27, PARM_DIASTOLIC, ds.VitalSigns.DiastolicColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(28, PARM_DIASTOLIC_DATETIME, ds.VitalSigns.DiastolicDatetimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_SYSTOLIC, ds.VitalSigns.SystolicColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(30, PARM_SYSTOLIC_DATETIME, ds.VitalSigns.SystolicDatetimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(31, PARM_PULSE, ds.VitalSigns.PulseResultColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(32, PARM_PULSE_DATETIME, ds.VitalSigns.PulseDatetimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(33, PARM_TEMPERATURE, ds.VitalSigns.TemperatureResultColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(34, PARM_TEMPERATURE_DATETIME, ds.VitalSigns.TemperatureDatetimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_RESPIRATION, ds.VitalSigns.RespirationResultColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(36, PARM_RESPIRATION_DATETIME, ds.VitalSigns.RespirationDatetimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(37, PARM_LOINIC_SYSTOLIC, ds.VitalSigns.LOINICSystolicColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_LOINIC_DIASTOLIC, ds.VitalSigns.LOINICDiastolicColumn.ColumnName, DbType.String);
        }

        #endregion Vital Signs

        #region Problems List

        public void InsertUpdateProblemList_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.ProblemListInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_PROBLEMLIST_INSERT, ds, ds.ProblemList.TableName);
                dbManager.CommitTransaction();
                ds.ProblemList.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                ds.ProblemList.RejectChanges();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateProblemList_CCDAData", PROC_CCDA_PROBLEMLIST_INSERT, ex);
                throw ex;
            }
        }

        private void ProblemListInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(42);

            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.ProblemList.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.ProblemList.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.ProblemList.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_USER_ID, ds.ProblemList.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_PROBLEM_NAME, ds.ProblemList.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_DESCRIPTION, ds.ProblemList.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CHRONICITY_LEVEL, ds.ProblemList.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_SEVERITY, ds.ProblemList.SeverityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_START_DATE, ds.ProblemList.StartDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_COMMENTS, ds.ProblemList.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PATIENT_ID, ds.ProblemList.PatientIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_NOTE_ID, ds.ProblemList.NoteIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.ProblemList.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.ProblemList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.ProblemList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_INACTIVE_CHKBOX_VALUE, ds.ProblemList.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_INACTIVE_REASON, ds.ProblemList.InActiveReasonColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(21, PARM_RCOPIA_ID, ds.ProblemList.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CODE, ds.ProblemList.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_CODE_TYPE, ds.ProblemList.CodeTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_ICD9, ds.ProblemList.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_ICD10, ds.ProblemList.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_SNOMED_ID, ds.ProblemList.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_SNOMED_DESCRIPTION, ds.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_ICD9_DESCRIPTION, ds.ProblemList.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_ICD10_DESCRIPTION, ds.ProblemList.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_IS_CHIEFCOMPLAINT, ds.ProblemList.IsChiefComplaintColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(31, PARM_PROBLEM_ORDER, ds.ProblemList.ProblemOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(32, PARM_CUSTOM_FORM_ID, ds.ProblemList.CustomFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(33, PARM_CHKPROBLEM_EXISTS, ds.ProblemList.CheckProblemExistsColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(34, PARM_PROB_COMPLAINT_ID, ds.ProblemList.ComplaintIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(35, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(36, PARM_COMPLAINT_DETAIL_ID, ds.ProblemList.ComplaintDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(37, PARM_NEGATION_INDEX, ds.ProblemList.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(38, PARM_NEGATION_REASON, ds.ProblemList.NegationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(39, PARM_PROBLEM_ID, ds.ProblemList.ProblemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(40, PARM_COMPLAINT_COMMENTS, ds.ProblemList.ComplaintCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(41, PARM_IS_PROBLEM, ds.ProblemList.IsProblemColumn.ColumnName, DbType.Boolean);

        }

        #endregion Problems List

        #region Procedures

        public void InsertUpdateProcedures_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.ProceduresInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_PROCEDURES_INSERT, ds, ds.NotesProcedures.TableName);
                dbManager.CommitTransaction();
                ds.NotesProcedures.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateProcedures_CCDAData", PROC_CCDA_PROCEDURES_INSERT, ex);
                throw ex;
            }
        }

        private void ProceduresInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(28);

            dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.NotesProcedures.ProcedureIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.NotesProcedures.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.NotesProcedures.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.NotesProcedures.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_START_DATE, ds.NotesProcedures.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_END_DATE, ds.NotesProcedures.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_COMMENTS, ds.NotesProcedures.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PATIENT_ID, ds.NotesProcedures.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_NOTES_ID, ds.NotesProcedures.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.NotesProcedures.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(10, PARM_PROBLEMLIST_ID, ds.NotesProcedures.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.NotesProcedures.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.NotesProcedures.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.NotesProcedures.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.NotesProcedures.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_CPT_CODE, ds.NotesProcedures.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CPT_DESCRIPTION, ds.NotesProcedures.CPT_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIER, ds.NotesProcedures.ModifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_UNIT, ds.NotesProcedures.UnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_SNOMED_ID, ds.NotesProcedures.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_SNOMED_DESCRIPTION, ds.NotesProcedures.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_CPT_ID, ds.NotesProcedures.CPTIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(22, PARM_VACCINEHX_ID, ds.NotesProcedures.VaccineHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(23, PARM_IMMUNIZATION_THER_INJECTION_ID, ds.NotesProcedures.ImmTherInjectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(24, PARM_IS_FROM_SUPPERBILL, ds.NotesProcedures.IsFromSupperbillColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(25, PARM_CUSTOM_FORM_ID, ds.NotesProcedures.CustomFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_NEGATION_INDEX, ds.NotesProcedures.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(27, PARM_NEGATION_REASON, ds.NotesProcedures.NegationReasonColumn.ColumnName, DbType.String);

        }

        #endregion Procedures

        #region Social History

        public void InsertUpdateSocialHistory_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.SocialHistoryInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_SOCIALHX_TOBACCO_INSERT, ds, ds.SocialHistory.TableName);
                dbManager.CommitTransaction();
                ds.SocialHistory.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateSocialHistory_CCDAData", PROC_CCDA_SOCIALHX_TOBACCO_INSERT, ex);
                throw ex;
            }
        }

        private void SocialHistoryInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(25);
            dbManager.AddParameters(0, PARM_TOBACCO_ID, ds.SocialHistory.TobaccoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.SocialHistory.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS, ds.SocialHistory.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_TYPE, ds.SocialHistory.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_USAGE_PERIOD, ds.SocialHistory.UsagePeriodColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_FREQUENCY, ds.SocialHistory.FrequencyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_COUNSELLING, ds.SocialHistory.CounsellingColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_COUNSELLING_TOPIC, ds.SocialHistory.CounsellingTopicColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CESSATION_LENGTH, ds.SocialHistory.CessationLengthColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CESSATION_PERIOD, ds.SocialHistory.CessationPeriodColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_RECENTLY_QUIT, ds.SocialHistory.RecentlyQuitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_WOULD_QUIT, ds.SocialHistory.WouldQuitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_START_DATE, ds.SocialHistory.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_END_DATE, ds.SocialHistory.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_SNOMEDCT_CODE, ds.SocialHistory.SNOMEDCTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_DESCRIPTION, ds.SocialHistory.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_IS_ACTIVE, ds.SocialHistory.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(17, PARM_CREATED_BY, ds.SocialHistory.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CREATED_ON, ds.SocialHistory.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_MODIFIED_BY, ds.SocialHistory.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_MODIFIED_ON, ds.SocialHistory.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_SOAP_TEXT, ds.SocialHistory.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_HISTORY_TYPE, ds.SocialHistory.HistoryTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_COMMENTS, ds.SocialHistory.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_BIRTH_SEX, ds.SocialHistory.BirthSexColumn.ColumnName, DbType.String);
        }

        #endregion Social History

        #region Results

        public void InsertUpdateResults_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.ResultsInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_RESULTS, ds, ds.ResultDetail.TableName);
                dbManager.CommitTransaction();
                ds.ResultDetail.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateResults_CCDAData", PROC_CCDA_RESULTS, ex);
                throw ex;
            }
        }

        private void ResultsInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(24);
            dbManager.AddParameters(0, PARM_LABORDER_RESULTID, ds.ResultDetail.ResultDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.ResultDetail.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.ResultDetail.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.ResultDetail.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_USER_ID, ds.ResultDetail.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.ResultDetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_PATIENT_ID, ds.ResultDetail.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_OBSERVATION_DATE, ds.ResultDetail.ObservationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_LOINC_CODE, ds.ResultDetail.LOINCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_LOINC_DESCRIPTION, ds.ResultDetail.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CPTCODE, ds.ResultDetail.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CPTCODE_DESCRIPTION, ds.ResultDetail.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_RESULT_VALUE, ds.ResultDetail.ResultValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_RANGE, ds.ResultDetail.RangeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.ResultDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CREATED_ON, ds.ResultDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_MODIFIED_BY, ds.ResultDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIED_ON, ds.ResultDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_CATEGORY_CODE, ds.ResultDetail.CategoryCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CATEGORY_NAME, ds.ResultDetail.CategoryNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_CATEGORYCODE_SYSTEM, ds.ResultDetail.CategoryCodeSystemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_UOM, ds.ResultDetail.UOMColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_FLAG_CODE, ds.ResultDetail.FlagCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_FLAG, ds.ResultDetail.FlagColumn.ColumnName, DbType.String);
        }

        #endregion Results

        #region Medications

        public void InsertUpdateMedications_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.MedicationsInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_MEDICATION, ds, ds.Medication.TableName);
                dbManager.CommitTransaction();
                ds.Medication.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateMedications_CCDAData", PROC_CCDA_MEDICATION, ex);
                throw ex;
            }
        }

        private void MedicationsInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(33);
            dbManager.AddParameters(0, PARM_MEDICATION_ID, ds.Medication.MedicationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.Medication.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.Medication.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.Medication.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_USER_ID, ds.Medication.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.Medication.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_PATIENT_ID, ds.Medication.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_START_DATE, ds.Medication.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_STOP_DATE, ds.Medication.StopDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_RXNORM_ID, ds.Medication.RxNormCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_BRAND_NAME, ds.Medication.DrugDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ACTION, ds.Medication.ActionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_DOSE, ds.Medication.DoseValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_DOSE_UNIT, ds.Medication.DoseUnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_QUANTITY, ds.Medication.QuantityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_QUANTITY_UNIT, ds.Medication.QuantityUnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ROUTE_CODE, ds.Medication.RouteCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ROUTE_BY, ds.Medication.RouteByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_REPEAT_NUMBER, ds.Medication.RepeatNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_REFILL, ds.Medication.RefillColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_DOSETIMING, ds.Medication.DoseTimingColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_SUBSTITUTION, ds.Medication.SubstitutionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_NEGATION_INDEX, ds.Medication.NegationIndexColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(23, PARM_NEGATION_REASON, ds.Medication.NegationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_OTHER_NOTE, ds.Medication.OtherNoteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_PATIEN_TNOTES, ds.Medication.PatientNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_STATUS, ds.Medication.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_CREATED_BY, ds.Medication.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_CREATED_ON, ds.Medication.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_MODIFIED_BY, ds.Medication.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_MODIFIED_ON, ds.Medication.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(31, PARM_MEDICATIONS_ID, ds.Medication.MedicationsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(32, PARM_INTENDED_USE, ds.Medication.IntendedUseColumn.ColumnName, DbType.String);

        }

        #endregion Medications

        #region Implantable Device

        public void InsertUpdateImplantableDevice_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.ImplantableDeviceInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_PATIENT_IMPLANTABLE_DEVICE, ds, ds.MedicalDeviceEquipment.TableName);
                dbManager.CommitTransaction();
                ds.MedicalDeviceEquipment.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateImplantableDevice_CCDAData", PROC_CCDA_PATIENT_IMPLANTABLE_DEVICE, ex);
                throw ex;
            }
        }

        private void ImplantableDeviceInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(28);
            dbManager.AddParameters(0, PARM_PROVIDER_ID, ds.MedicalDeviceEquipment.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_FACILITY_ID, ds.MedicalDeviceEquipment.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.MedicalDeviceEquipment.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.MedicalDeviceEquipment.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.MedicalDeviceEquipment.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_IMPLANTABLEDEVICE_ID, ds.MedicalDeviceEquipment.MedicalDeviceEquipmentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(6, PARM_PLACED_DATE, ds.MedicalDeviceEquipment.PlacedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_TARGET_SITE_CODE, ds.MedicalDeviceEquipment.TargetSiteCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_TARGET_SITE, ds.MedicalDeviceEquipment.TargetSiteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_UDI, ds.MedicalDeviceEquipment.UDIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_DEVICE_IDENTIFIER, ds.MedicalDeviceEquipment.DeviceIdentifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_DEVICE_DESCRIPTION, ds.MedicalDeviceEquipment.DeviceDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ISSUING_AGENCY, ds.MedicalDeviceEquipment.IssuingAgencyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MANUFACTURING_DATE, ds.MedicalDeviceEquipment.ManufacturingDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_SERIAL_NUMBER, ds.MedicalDeviceEquipment.SerialNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_EXPIRATION_DATE, ds.MedicalDeviceEquipment.ExpirationDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_LOT_NUMBER, ds.MedicalDeviceEquipment.LotNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_BRAND_NAME, ds.MedicalDeviceEquipment.BrandNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_VERSION_MODELNUMBER, ds.MedicalDeviceEquipment.VersionModelNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_COMPANY_NAME, ds.MedicalDeviceEquipment.CompanyNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_MRI_SAFETY_STATUS, ds.MedicalDeviceEquipment.MRISafetyStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_LABELED_CONTAINS_NRL, ds.MedicalDeviceEquipment.LabeledContainsNRLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_GMDNP_NAME, ds.MedicalDeviceEquipment.GMDNPNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_STATUS, ds.MedicalDeviceEquipment.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_CREATED_BY, ds.MedicalDeviceEquipment.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CREATED_ON, ds.MedicalDeviceEquipment.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_MODIFIED_BY, ds.MedicalDeviceEquipment.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_MODIFIED_ON, ds.MedicalDeviceEquipment.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion Implantable Device

        #region Goals

        public void InsertUpdateGoals_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.GoalsInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_GOAL, ds, ds.EnrolledGoals.TableName);
                dbManager.CommitTransaction();
                ds.EnrolledGoals.AcceptChanges();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateGoals_CCDAData", PROC_CCDA_GOAL, ex);
                throw ex;
            }
        }

        private void GoalsInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(19);
            dbManager.AddParameters(0, PARM_PROVIDER_ID, ds.EnrolledGoals.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_FACILITY_ID, ds.EnrolledGoals.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.EnrolledGoals.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.EnrolledGoals.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.EnrolledGoals.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_LOINC_CODE, ds.EnrolledGoals.LOINCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_LOINC_DESCRIPTION, ds.EnrolledGoals.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_STATUS, ds.EnrolledGoals.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_GOAL_DATETIME, ds.EnrolledGoals.GoalDateTimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_COMMENTS, ds.EnrolledGoals.InstructionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CPT_CODE, ds.EnrolledGoals.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CPTDESCRIPTION, ds.EnrolledGoals.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_SNOMED_CODE, ds.EnrolledGoals.SNOMEDCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SNOMEDDESCRIPTION, ds.EnrolledGoals.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.EnrolledGoals.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CREATED_ON, ds.EnrolledGoals.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_MODIFIED_BY, ds.EnrolledGoals.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIED_ON, ds.EnrolledGoals.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_ENROLLEDGOALS_ICDID, ds.EnrolledGoals.EnrolledGoalsIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
        }

        #endregion Goals

        #region Health Concerns

        public void InsertUpdateHealthConcerns_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.HealthConcernsInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_CAREPLAN_HEALTHCONCERNS_INSERT, ds, ds.HealthConcerns.TableName);
                dbManager.CommitTransaction();
                ds.HealthConcerns.AcceptChanges();
            }
            catch (Exception ex)
            {
                ds.HealthConcerns.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateHealthConcerns_CCDAData", PROC_CCDA_CAREPLAN_HEALTHCONCERNS_INSERT, ex);
                throw ex;
            }
        }

        private void HealthConcernsInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(42);
            dbManager.AddParameters(0, PARM_HEALTHCONCERN_ID, ds.HealthConcerns.HealthConcernIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.HealthConcerns.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.HealthConcerns.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_USER_ID, ds.HealthConcerns.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.HealthConcerns.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.HealthConcerns.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_CONCERNS_DATE, ds.HealthConcerns.ConcernsDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_OBSERVATION_DATETIME, ds.HealthConcerns.ObservationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_CONCERNS_ICD9_CODE, ds.HealthConcerns.ConcernsICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CONCERNS_STATUS, ds.HealthConcerns.ConcernsStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_OBSERVATION_STATUS, ds.HealthConcerns.ObservationStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CONCERNS_ICD9CODE_DESCRIPTION, ds.HealthConcerns.ConcernsICD9DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CONCERNS_ICD10_CODE, ds.HealthConcerns.ConcernsICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CONCERNS_ICD10CODE_DESCRIPTION, ds.HealthConcerns.ConcernsICD10DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CONCERNS_SNOMED_ID, ds.HealthConcerns.ConcernsSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CONCERNS_SNOMED_DESCRIPTION, ds.HealthConcerns.ConcernsSNOMEDDescColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_OBSERVATION_ICD9CODE, ds.HealthConcerns.ObservationICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_OBSERVATION_ICD9CODE_DESCRIPTION, ds.HealthConcerns.ObservationICD9DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_OBSERVATION_ICD10CODE, ds.HealthConcerns.ObservationICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_OBSERVATION_ICD10CODE_DESCRIPTION, ds.HealthConcerns.ObservationICD10DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_OBSERVATION_SNOMEDID, ds.HealthConcerns.ObservationSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_OBSERVATION_SNOMED_DESCRIPTION, ds.HealthConcerns.ObservationSNOMEDDescColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_BY, ds.HealthConcerns.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_CREATED_ON, ds.HealthConcerns.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_MODIFIED_BY, ds.HealthConcerns.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_MODIFIED_ON, ds.HealthConcerns.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_OBSPATIENT_PRIORITYCODE, ds.HealthConcerns.ObsPatientPriorityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_OBSPATIENT_PRIORITY, ds.HealthConcerns.ObsPatientPriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_OBSPROVIDER_PRIORITYCODE, ds.HealthConcerns.ObsProviderPriorityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_OBSPROVIDER_PRIORITY, ds.HealthConcerns.ObsProviderPriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_RISK_ICD9CODE, ds.HealthConcerns.RiskICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_RISK_ICD10CODE, ds.HealthConcerns.RiskICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_RISK_ICD9DESCRIPTION, ds.HealthConcerns.RiskICD9DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_RISK_ICD10DESCRIPTION, ds.HealthConcerns.RiskICD10DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_RISK_DATE, ds.HealthConcerns.RiskDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_RISK_STATUS, ds.HealthConcerns.RiskStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_RISKPATIENT_PRIORITYCODE, ds.HealthConcerns.RiskPatientPriorityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(37, PARM_RISKPATIENT_PRIORITY, ds.HealthConcerns.RiskPatientPriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_RISKPROVIDER_PRIORITYCODE, ds.HealthConcerns.RiskProviderPriorityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(39, PARM_RISKPROVIDER_PRIORITY, ds.HealthConcerns.RiskProviderPriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(40, PARM_RISK_SNOMEDID, ds.HealthConcerns.RiskSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(41, PARM_RISK_SNOMEDDESC, ds.HealthConcerns.RiskSNOMEDDescColumn.ColumnName, DbType.String);
        }

        #endregion  Health Concerns

        #region CARE PLAN  Goals

        public void InsertUpdateCarePlanGoals_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.CarePlanGoalsInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_CAREPLANGOAL_INSERT, ds, ds.CarePlanGoals.TableName);
                dbManager.CommitTransaction();
                ds.CarePlanGoals.AcceptChanges();
            }
            catch (Exception ex)
            {
                ds.CarePlanGoals.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCarePlanGoals_CCDAData", PROC_CCDA_CAREPLANGOAL_INSERT, ex);
                throw ex;
            }
        }

        private void CarePlanGoalsInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(27);
            dbManager.AddParameters(0, PARM_ENROLLEDGOALS_ICDID, ds.CarePlanGoals.GoalIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.CarePlanGoals.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.CarePlanGoals.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ICDCODE, ds.CarePlanGoals.ICDCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICDCODEDESCRIPTION, ds.CarePlanGoals.ICDCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_SNOMEDID, ds.CarePlanGoals.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMEDDESCRIPTION, ds.CarePlanGoals.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CPT_CODE, ds.CarePlanGoals.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CPTDESCRIPTION, ds.CarePlanGoals.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CPT_SNOMEDID, ds.CarePlanGoals.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CPT_SNOMEDDESCRIPTION, ds.CarePlanGoals.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_LOINC_CODE, ds.CarePlanGoals.LOINCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_LOINC_DESCRIPTION, ds.CarePlanGoals.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_COMMENTS, ds.CarePlanGoals.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_VALUE, ds.CarePlanGoals.ValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_UNIT, ds.CarePlanGoals.UnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_STATUS, ds.EnrolledGoals.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_START_DATE, ds.CarePlanGoals.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_END_DATE, ds.CarePlanGoals.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_PATIENT_PRIORITY, ds.CarePlanGoals.PatientPriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_PATIENT_PRIORITYCODE, ds.CarePlanGoals.PatientPriorityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_PROVIDER_PRIORITY, ds.CarePlanGoals.ProviderPriorityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_PROVIDER_PRIORITYCODE, ds.CarePlanGoals.ProviderPriorityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_CREATED_BY, ds.EnrolledGoals.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_CREATED_ON, ds.EnrolledGoals.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_MODIFIED_BY, ds.EnrolledGoals.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_MODIFIED_ON, ds.EnrolledGoals.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion CARE PLAN Goals

        #region CARE PLAN  Outcome

        public void InsertUpdateCarePlanOutcome_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.CarePlanOutcomeInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_CAREPLAN_OUTCOMESINSERT, ds, ds.HealthStatus.TableName);
                dbManager.CommitTransaction();
                ds.HealthStatus.AcceptChanges();
            }
            catch (Exception ex)
            {
                ds.HealthStatus.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCarePlanOutcome_CCDAData", PROC_CCDA_CAREPLAN_OUTCOMESINSERT, ex);
                throw ex;
            }
        }

        private void CarePlanOutcomeInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(20);
            dbManager.AddParameters(0, PARM_OUTCOMES_ID, ds.HealthStatus.HealthStatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.HealthStatus.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.HealthStatus.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FACILITY_ID, ds.HealthStatus.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.HealthStatus.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.HealthStatus.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.HealthStatus.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.HealthStatus.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.HealthStatus.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.HealthStatus.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SNOMEDID, ds.HealthStatus.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SNOMEDDESCRIPTION, ds.HealthStatus.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_LOINC_CODE, ds.HealthStatus.LOINCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_LOINC_DESCRIPTION, ds.HealthStatus.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CPT_CODE, ds.HealthStatus.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CPTDESCRIPTION, ds.HealthStatus.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_START_DATE, ds.HealthStatus.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_END_DATE, ds.HealthStatus.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_VALUE, ds.HealthStatus.ValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_UNIT, ds.HealthStatus.UnitColumn.ColumnName, DbType.String);
        }

        #endregion CARE PLAN Outcome

        #region CARE PLAN  Intervention

        public void InsertUpdateCarePlanIntervention_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.CarePlanInterventionInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_CAREPLAN_INTERVENTIONS, ds, ds.Interventions.TableName);
                dbManager.CommitTransaction();
                ds.Interventions.AcceptChanges();
            }
            catch (Exception ex)
            {
                ds.Interventions.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCarePlanIntervention_CCDAData", PROC_CCDA_CAREPLAN_INTERVENTIONS, ex);
                throw ex;
            }
        }

        private void CarePlanInterventionInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(27);
            dbManager.AddParameters(0, PARM_PROVIDER_ID, ds.Interventions.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.Interventions.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.Interventions.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Interventions.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.Interventions.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.Interventions.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.Interventions.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.Interventions.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.Interventions.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_INTERVENTION_ID, ds.Interventions.InterventionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(10, PARM_CODE, ds.Interventions.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CODE_DESCRIPTION, ds.Interventions.CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_START_DATE, ds.Interventions.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_END_DATE, ds.Interventions.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_SNOMEDID, ds.Interventions.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_SNOMEDDESCRIPTION, ds.Interventions.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_LOINC_CODE, ds.Interventions.LOINCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_LOINC_DESCRIPTION, ds.Interventions.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CODE_SYSTEM, ds.Interventions.CodeSystemColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CODESYSTEMNAME, ds.Interventions.CodeSystemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ICD9_CODE, ds.Interventions.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_ICD10_CODE, ds.Interventions.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_ICD9DESCRIPTION, ds.Interventions.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_ICD10DESCRIPTION, ds.Interventions.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_CPT_CODE, ds.Interventions.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CPTDESCRIPTION, ds.Interventions.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_STATUS, ds.Interventions.StatusColumn.ColumnName, DbType.String);
        }

        #endregion CARE PLAN Intervention

        #region CARE Team

        public void InsertUpdate_CareTeamMembers_CCDAData(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                this.CareTeamMembersInsertUpdateParametersForCCDA(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCDA_CARETEAMMEMBERS_CAREGIVERS, ds, ds.CareTeamMembers.TableName);
                dbManager.CommitTransaction();
                ds.CareTeamMembers.AcceptChanges();
            }
            catch (Exception ex)
            {
                ds.CareTeamMembers.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdate_CARE_Team_CCDAData", PROC_CCDA_CARETEAMMEMBERS_CAREGIVERS, ex);
                throw ex;
            }
        }

        private void CareTeamMembersInsertUpdateParametersForCCDA(IDBManager dbManager, DSCCDA ds)
        {
            dbManager.CreateParameters(29);
            dbManager.AddParameters(0, PARM_CARETEAM_ID, ds.CareTeamMembers.CareTeamMemberIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.CareTeamMembers.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.CareTeamMembers.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(3, PARM_PATIENT_ID, ds.CareTeamMembers.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PROVIDER_ID, ds.CareTeamMembers.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_CAREPROVIDER_TYPECODE, ds.CareTeamMembers.CareProviderTypeCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CAREPROVIDER_TYPE, ds.CareTeamMembers.CareProviderTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MEMBER_CODE, ds.CareTeamMembers.MemberCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MEMBER_CODENAME, ds.CareTeamMembers.MemberCodeNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PREFIX, ds.CareTeamMembers.PrefixColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_FIRST_NAME, ds.CareTeamMembers.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MI, ds.CareTeamMembers.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_LAST_NAME, ds.CareTeamMembers.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ADDRESS1, ds.CareTeamMembers.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CITY, ds.CareTeamMembers.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_State, ds.CareTeamMembers.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ZIPCODE, ds.CareTeamMembers.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_HOME_PHONE_NO, ds.CareTeamMembers.HomePhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ORGANIZATION, ds.CareTeamMembers.OrganizationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ORGANIZATION_TELEPHONE, ds.CareTeamMembers.OrganizationTelephoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ORGANIZATION_CITY, ds.CareTeamMembers.OrganizationCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_ORGANIZATION_STATE, ds.CareTeamMembers.OrganizationStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_ORGANIZATION_ZIPCODE, ds.CareTeamMembers.OrganizationZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_ORGANIZATION_ADDRESS1, ds.CareTeamMembers.OrganizationAddress1Column.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_CREATED_BY, ds.CareTeamMembers.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CREATED_ON, ds.CareTeamMembers.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_MODIFIED_BY, ds.CareTeamMembers.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_MODIFIED_ON, ds.CareTeamMembers.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(28, PARM_ENTITY_ID, ds.CareTeamMembers.EntityIdColumn.ColumnName, DbType.Int64);
        }

        #endregion CARE Team

        #endregion _CCDA


        public string InsertLabData(long PatientId, DateTime EffectiveTimeLow, DateTime EffectiveTimeHigh, string StatusCode, string Text, string Code,
                                            string CodeDescripion, string ResultValue, string ResultUnit, string ResultRange, string ActionPerformed)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);
                dbManager.AddParameters(0, "@LabOrderId", ParamDirection.Output);
                dbManager.AddParameters(1, "@PatientId", PatientId);
                dbManager.AddParameters(2, "@FacilityId", MDVSession.Current.DefaultFacilityId);
                dbManager.AddParameters(3, "@ProviderId", MDVSession.Current.DefaultProviderId);
                dbManager.AddParameters(4, "@StartDate", EffectiveTimeLow);
                dbManager.AddParameters(5, "@EndDate", EffectiveTimeHigh);
                dbManager.AddParameters(6, "@IsActive", 1);
                dbManager.AddParameters(7, "@CreatedBy", "Cypress");
                dbManager.AddParameters(8, "@CreatedOn", DateTime.Now);
                dbManager.AddParameters(9, "@ModifiedBy", "Cypress");
                dbManager.AddParameters(10, "@ModifiedOn", DateTime.Now);
                dbManager.AddParameters(11, "@CPTCode", Code);
                dbManager.AddParameters(12, "@CPTCodeDescription", CodeDescripion);
                dbManager.AddParameters(13, "@Result", ResultValue);
                dbManager.AddParameters(14, "@ResultUnit", ResultUnit);
                dbManager.AddParameters(15, "@ActionPerformed", ActionPerformed);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[Clinical].[CQM_LabOrderInsertUpdate]");
                return "Success";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("CCDA::InsertLabData", "[Clinical].[CQM_Result_Entry_Insert]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string insertUpdatePatientInsurance(long insurancePlanId, long PatientId)

        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@PairId", insurancePlanId);
                dbManager.AddParameters(1, "@PatientId", PatientId);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[Clinical].[CQM_PatientInsuranceInsertUpdate]");
                return "Success";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("CCDA::insertUpdatePatientInsurance", "[Clinical].[CQM_PatientInsuranceInsertUpdate]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string InsertPhysicalExam(long PatientId, DateTime EffectiveTimeLow, DateTime EffectiveTimeHigh, string StatusCode, string Text, string Code,
                            string CodeDescripion, string ResultCode, string ResultCodeDescription, string ResultValue, string ResultUnit, string ActionPerformed, string originalText = "", string NegationValueset = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(20);
                dbManager.AddParameters(0, "@VitalSignId", null);
                dbManager.AddParameters(1, "@StartDate", EffectiveTimeLow);
                dbManager.AddParameters(2, "@EndDate", EffectiveTimeHigh);
                dbManager.AddParameters(3, "@LOINC", Code);
                dbManager.AddParameters(4, "@Result", ResultValue);
                dbManager.AddParameters(5, "@Unit", ResultUnit);
                dbManager.AddParameters(6, "@PatientId", PatientId);
                dbManager.AddParameters(7, "@ProviderId", MDVSession.Current.DefaultProviderId);
                dbManager.AddParameters(8, "@FacilityId", MDVSession.Current.DefaultFacilityId);
                dbManager.AddParameters(9, "@IsActive", true);
                dbManager.AddParameters(10, "@CreatedBy", "Cypress");
                dbManager.AddParameters(11, "@CreatedOn", DateTime.Now);
                dbManager.AddParameters(12, "@ModifiedBy", "Cypress");
                dbManager.AddParameters(13, "@ModifiedOn", DateTime.Now);
                dbManager.AddParameters(14, "@NegationIndex", null);
                dbManager.AddParameters(15, "@NegationReason", null);
                dbManager.AddParameters(16, "@UserName", "Cypress");
                dbManager.AddParameters(17, "@ActionPerformed", ActionPerformed == "" ? null : ActionPerformed);
                dbManager.AddParameters(18, "@originalText", originalText == "" ? null : originalText);
                dbManager.AddParameters(19, "@NegationValueset", NegationValueset == null ? null : NegationValueset);

                if (Code == "91161007" || Code == "401191002" || Code == "134388005")
                {
                    dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[Clinical].[CQM_PhysicalExamProceduresInsertUpdate]");
                    return "Success";
                }
                else
                {

                    dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[Clinical].[CQM_PhysicalExamInsertUpdate]");
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("CCDA::InsertPEData", "[Clinical].[CQM_PhysicalExamInsertUpdate]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertUpdateRiskAssessment(long PatientId, DateTime StartDate, DateTime EndDate, string StatusCode, string Text, string CPTCode, string SNOMEDID,
                            string CodeType, string ResultCode, string ResultCodeDescription, bool negationIndex, string negationReason, string negationValueset = null, string PHQScore = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (CodeType == "SNOMED-CT")
                {
                    CodeType = "SNOMED";
                }
                dbManager.Open();
                dbManager.CreateParameters(19);
                dbManager.AddParameters(0, "@ProcedureId", ParamDirection.Output);
                dbManager.AddParameters(1, "@NotesId", null);
                dbManager.AddParameters(2, "@PatientId", PatientId);
                dbManager.AddParameters(3, "@ProviderId", MDVSession.Current.DefaultProviderId);
                dbManager.AddParameters(4, "@FacilityId", MDVSession.Current.DefaultFacilityId);

                dbManager.AddParameters(5, "@StartDate", StartDate);
                dbManager.AddParameters(6, "@EndDate", EndDate);
                dbManager.AddParameters(7, "@CPTCode", CPTCode);
                dbManager.AddParameters(8, "@SNOMEDID", SNOMEDID);
                //dbManager.AddParameters(9, "@CodeType", CodeType);
                dbManager.AddParameters(9, "@CreatedBy", "Cypress");
                dbManager.AddParameters(10, "@CreatedOn", DateTime.Now);
                dbManager.AddParameters(11, "@ModifiedBy", "Cypress");
                dbManager.AddParameters(12, "@ModifiedOn", DateTime.Now);
                dbManager.AddParameters(13, "@IsActive", 1);
                dbManager.AddParameters(14, "@ResultCode", ResultCode);
                dbManager.AddParameters(15, "@NegationIndex", negationIndex);
                dbManager.AddParameters(16, "@NegationReason", negationReason);
                dbManager.AddParameters(17, "@NegationValueset", negationValueset);
                dbManager.AddParameters(18, "@PHQScore", MDVUtility.Tofloat(PHQScore));

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[Clinical].[CQM_RiskAssessmentInsertUpdate]");
                return "Success";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("CCDA::InsertUpdateRiskAssessment", "[Clinical].[CQM_RiskAssessmentInsertUpdate]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCDA InsertUpdateEncounter(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateEncounterInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ENCOUNTER_INSERT_UPDATE, ds, ds.NotesEncounter.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateEncounter", PROC_ENCOUNTER_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertMUSetting(long patientId, long providerId, long facilityId, long noteId, bool lstMedication, bool lstProblems, bool lstAllergies,bool IsPatientEducation,bool IsTOC, long IsTOCDelivered, long TOCId, bool IsSummaryOfCare)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCDA ds = new DSCCDA();
            try
            {

               
                dbManager.Open();
                dbManager.CreateParameters(16);

                if (patientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (providerId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, providerId);
                if (facilityId <= 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, facilityId);
                if (noteId <= 0)
                    dbManager.AddParameters(3, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_NOTE_ID, noteId);

                dbManager.AddParameters(4, PARM_IS_MEDICATION, lstMedication);
                dbManager.AddParameters(5, PARM_IS_ALLERGY, lstAllergies);
                dbManager.AddParameters(6, PARM_IS_PROBLEMLIST, lstProblems);
                dbManager.AddParameters(7, PARM_IS_PATIENT_EDUCATION, IsPatientEducation);
                dbManager.AddParameters(8, PARM_IS_TOC, IsTOC);
                dbManager.AddParameters(9, PARM_IS_TOC_DELIVERED, IsTOCDelivered);
                dbManager.AddParameters(10, PARM_TOC_ID, TOCId);
                dbManager.AddParameters(11, PARM_IS_SUMMARY_OF_CARE,IsSummaryOfCare);
                dbManager.AddParameters(12, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(13, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(14, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(15, PARM_MODIFIED_ON, DateTime.Now);
                ds = (DSCCDA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSERT_MU_SETTING, ds, ds.MUSetting.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateEncounter", PROC_INSERT_MU_SETTING, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertUpdateProcedure(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateProcedureInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_INSERT_UPDATE, ds, ds.NotesProcedures.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateProcedure", PROC_PROCEDURE_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertUpdateDiagnosis(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateDiagnosisInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DIAGNOSIS_INSERT_UPDATE, ds, ds.Diagnosis.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateDiagnosis", PROC_DIAGNOSIS_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCDA InsertUpdateIntervention(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateInterventionInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INTERVENTION_INSERT_UPDATE, ds, ds.Interventions.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateIntervention", PROC_INTERVENTION_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertUpdateImmunization(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateImmunizationInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_IMMUNIZATION_INSERT_UPDATE, ds, ds.Immunization.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateImmunization", PROC_IMMUNIZATION_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCDA InsertUpdateMedication(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateMedicationInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEDICATION_INSERT_UPDATE, ds, ds.Medication.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateMedication", PROC_MEDICATION_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCDA InsertUpdateCommProviderToProvider(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateCommProviderToProviderInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_INSERT_UPDATE, ds, ds.ComProviderToProvider.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCommProviderToProvider", PROC_PROCEDURE_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertUpdateCommPatientToProvider(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreateCommPatientToProviderInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_INSERT_UPDATE, ds, ds.ComPatientToProvider.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCommPatientToProvider", PROC_PROCEDURE_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertUpdateDiagnosticStudy(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreatePatientDiagnosticStudyInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DIAGNOSTIC_STUDY_INSERT_UPDATE, ds, ds.DiagnosticStudy.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateDiagnosticStudy", PROC_DIAGNOSTIC_STUDY_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCDA InsertUpdatePatientCharacteristics(DSCCDA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                this.CreatePatientCharacteristicsInsertUpdateParameters(dbManager, ds);
                ds = (DSCCDA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARACTERISTICS_INSERT_UPDATE, ds, ds.PatientCharacteristics.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertUpdateCommPatientToProvider", PROC_PATIENT_CHARACTERISTICS_INSERT_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient InsertPrivacySegmentedDocument(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();

                CreatePrivacySegmentedDocumentInsertParameters(dbManager, ds);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PRIVACY_SEGMENTED_DATA_INSERT, ds, ds.Patients.TableName);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCCDA::InsertPrivacySegmentedDocument", PROC_PRIVACY_SEGMENTED_DATA_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient LoadPrivacySegmentedDocument(long Id, long EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSPatient ds = new DSPatient();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (Id <= 0)
                    dbManager.AddParameters(0, PARM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ID, Id);

                if (EntityId <= 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_Page_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_Page_NUMBER, PageNumber);

                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_P_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_P_PAGE, RowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Patients.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRIVACY_SEGMENTED_DATA_SELECT, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCDA::LoadPrivacySegmentedDocument", PROC_PRIVACY_SEGMENTED_DATA_SELECT, ex);
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

        public string GetIsDataPrivacy()
        {
            string IsDataPrivacy = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_USER_ISDATAPRIVACY_SELECT);

                while (reader.Read())
                {
                    IsDataPrivacy = reader.IsDBNull(reader.GetOrdinal("IsDataPrivacy")) ? "" : Convert.ToString(reader["IsDataPrivacy"]);
                }
                return IsDataPrivacy;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCDA::GetIsDataPrivacy", PROC_USER_ISDATAPRIVACY_SELECT, ex);
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
    }
}
