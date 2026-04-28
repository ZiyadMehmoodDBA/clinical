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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALCaseReports
    {
        #region Variable

        #endregion

        #region Constructors

        public DALCaseReports()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALCaseReports(SharedVariable SharedVariable)
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

        //End 27-04-2016 Muhammad Arshad Cognitive

        //Start Farooq Ahmad 28-04-2016

        private const string PROC_CCDA_PATIENT_LOOKUPS = "Clinical.sp_CCDAPatientLookUps";
        private const string PROC_CCDA_GET_DRUGID_FROM_RXNORMID = "Clinical.sp_CCDAGetDrugIDFromRxnormID";
        private const string PROC_ALLERGY_SELECT = "Clinical.sp_AllergySelect";
        private const string PROC_VDTAUDITINSERT = "Patient.sp_VDTAuditInsert";
        private const string PROC_PROBLEMLIST_SELECT = "Clinical.sp_ProblemListSelect_CaseReports";
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
        private const string PROC_Lab_ORDER_RESULT_DETAIL_SELECT = "Clinical.sp_LabOrderResultDetailSelect_CaseReporting";
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
        private const string PROC_Lab_ORDER_TEST_SELECT_CaseReporting = "Clinical.sp_LabOrderTestSelect_CaseReporting";
        private const string PROC_CARETEAM_SELECT_CCDA = "CCM.sp_CareTeamSelect_CCDA";

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

        #endregion

        public DSClinicalMedication loadMedicationsCaseReporting(long patientId, long noteId)
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
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Clinical.sp_MedicationSelect_CaseReporting", ds, ds.Medication.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsCCDA", "Clinical.sp_MedicationSelect_CaseReporting", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LoadProblemListsCaseReports(long PatientId, long NoteId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemList.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadProblemListsCCDA", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSLabResult loadLabResultDetail(long PatientId , long NoteId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);

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

        public DSClinicalComplaint LoadComplaintsCaseReports(long PatientId, long NoteId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Clinical.sp_NoteComplaint_CaseReporting", ds, ds.ComplaintDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadProblemListsCCDA", "Clinical.sp_NoteComplaint_CaseReporting", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLabOrder LoadLabOrdersForCaseReports(long PatientId, long NoteId )
        {
            DSLabOrder ds = new DSLabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);



                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_SELECT_CaseReporting, ds, ds.LabOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalSummary::LoadLabOrdersForCCDA", PROC_Lab_ORDER_TEST_SELECT_CaseReporting, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LoadSocialHxCaseReports(long PatientId, long NoteId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NoteId == 0)
                    dbManager.AddParameters(1, "@NoteId", null);
                else
                    dbManager.AddParameters(1, "@NoteId", NoteId);

                List<string> tableNames = new List<string>
                {
                ds.SocialHx_MiscHx_OccupationHx.TableName,
                ds.SocialHx_MiscHx_TravelHx.TableName,
                ds.SocialHx_SexualHx.TableName
                };

                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[Clinical].[sp_SocialHx_CaseReporting]", ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALSocialHistory::LoadSocialHx", "[Clinical].[sp_SocialHx_CaseReporting]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

    }
}
