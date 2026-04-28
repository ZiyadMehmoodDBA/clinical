using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Model;
using MDVision.Common.Logging;
using MDVision.Model.Patient;
using MDVision.Common.Utilities;
using MDVision.Model.Native.Patient;
using System.Diagnostics;
using MDVision.Model.Native;
using MDVision.Model.Dashboard;
using MDVision.Model.Native.Clinical;
using MDVision.Model.Native.Scheduler;

namespace MDVision.DataAccess.DAL.Admin.MobileApp
{
 public   class DALMobileApp
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALMobileApp()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALMobileApp(bool isNative)
        {


        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALMobileApp(SharedVariable SharedVariable)
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
        //private const string PROC_ETHNICITY_LOOKUP = "Patient.sp_EthnicityLookup";
        //private const string PROC_RACE_LOOKUP = "Patient.sp_RaceLookup";
        //private const string PROC_COMMUNICATION_LOOKUP = "Patient.sp_CommunicationLookup";
        //private const string PROC_SCHOOL_LOOKUP = "Patient.sp_SchoolLookup";
        //private const string PROC_SCHOOL_STATUS_LOOKUP = "Patient.sp_SchoolStatusLookup";
        //private const string PROC_LANGUAGES_LOOKUP = "Patient.sp_LanguagesLookup";
        //private const string PROC_GUARANTOR_LOOKUP = "Patient.sp_GuarantorLookup";
        //private const string PROC_MARITAL_STATUS_LOOKUP = "Patient.sp_MaritalStatusLookup";
        //private const string PROC_SUFFIX_LOOKUP = "Patient.sp_SuffixLookup";
        //private const string PROC_PREFIX_LOOKUP = "Patient.sp_PrefixLookup";
        //private const string PROC_SMOKERS_STATUS_LOOKUP = "Patient.sp_SmokersStatusLookup";
        //private const string PROC_PATIENT_DELETE = "Patient.sp_PatientsDelete";
        //private const string PROC_PATIENT_INSERT = "Patient.sp_PatientsInsert";
        //private const string PROC_PATIENT_SELECT = "Patient.sp_PatientsSelect";
        //private const string PROC_PATIENT_SEARCH = "Patient.sp_PatientsSearch";

        //private const string PROC_PATIENT_UPDATE = "Patient.sp_PatientsUpdate";
        //private const string PROC_PATIENT_UPDATE_PIC = "Patient.sp_PatientsImgUpdate";
        //private const string PROC_PATIENT_Delete_PIC_Native = "Patient.sp_PatientsUpdateImage_Native";
        //private const string PROC_PATIENT_FILL = "Patient.sp_PatientFill";
        //private const string PROC_PATIENT_FILL_Native = "Patient.sp_PatientFillNative";
        //private const string PROC_PATIENT_LOOKUP = "Patient.sp_PatientsLookup";
        //private const string PROC_PATIENT_LOOKUP_BY_NAME = "Patient.sp_PatientsLookupByName";
        //private const string PROC_PATIENT_RCOPIAID = "Patient.InsertPatientsRcopialID";
        //private const string PROC_RECENT_PATIENT = "System.sp_RecentAccessedPatientsInsert";
        //private const string PROC_PATIENT_PREF_UPDATE = "Patient.sp_PatientPreferencesUpdate";
        //private const string PROC_PATIENT_PREF_UPDATE_Native = "Patient.sp_PatientPreferencesUpdateNative";
        //private const string PROC_REFERRAL_STATUS_LOOKUP = "Patient.sp_ReferralStatusLookup";

        //private const string PROC_RELATIONS_LOOKUP = "Patient.sp_RelationShipLookup";
        //private const string PROC_PATIENT_UPDATE_NATIVE = "Patient.sp_PatientUpdateNative";
        private const string PROC_PROCEDURE_CPTLOOKUP_INSERT = "System.sp_CPTLookupInsert";
        private const string PROC_ICDLookup_INSERT = "System.sp_ICDLookupInsert";
        private const string PROC_CHECK_EXISTING_RECORD_NATIVE = "system.sp_CheckExistingRecordNative";
        private const string PROC_CHECK_EXISTING_PATIENT_RECORD = "Patient.sp_PatientsLookupMultiFilter";
        private const string PROC_Get_PatientBy_Insurance_SubscriberId = "Patient.sp_GetPatientByInsuranceSubscriberId";
        private const string PROC_CHECK_INSURANCE_PLAN_PRIORITY = "system.sp_CheckInsurancePlanPriorityNative";
        private const string PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_NATIVE = "system.sp_GetMaxInsurancePlanPriorityNative";
        private const string PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_DBAUDIT_NATIVE = "system.sp_GetMaxInsurancePlanPriorityDBAuditNative";
        private const string PROC_CHECK_EXISTING_RECORD_INSURANCE_NATIVE = "Patient.sp_CheckExistingRecordForInsuranceNative";
        private const string PROC_INSERT_UPDATE_DBAUDIT_NATIVE = "system.sp_InsertUpdateDBAuditNative";
        private const string PROC_DISCARD_RECORD_DBAUDIT_NATIVE = "system.sp_DiscardRecordInDbAuditNative";
        private const string PROC_DISCARD_ALL_RECORD_DBAUDIT_NATIVE = "system.sp_DiscardAllDBAuditNative";
        private const string PROC_APPROVE_ALL_RECORD_DBAUDIT_NATIVE = "system.sp_ApproveAllDBAuditNative";
        private const string PROC_PATIENT_EMERGENCYCONTACTS_SELECT_NATIVE = "Patient.sp_EmergencyContactsSearchNative";
        private const string PROC_PATIENT_INSURANCES_SELECT_NATIVE = "Mobile.sp_PatientInsuranceSearchNative";
        private const string PROC_PATIENT_EMERGENCYCONTACTS_FILL_NATIVE = "Patient.sp_EmergencyContactFillNative";
        private const string PROC_PATIENT_INSURANCE_FILL_NATIVE = "Patient.sp_PatientInsuranceFillNative";
        private const string PROC_PATIENT_Scheduler_FILL_NATIVE = "Patient.sp_PatientSchedulerFillNative";
        private const string PROC_PATIENT_EMERGENCYCONTACT_INSERT = "Patient.sp_EmergencyContactInsertNative";
        private const string PROC_PATIENT_EMERGENCYCONTACT_UPDATE = "Patient.sp_EmergencyContactUpdateNative";
        private const string PROC_PATIENT_PREF_UPDATE_Native = "Patient.sp_PatientPreferencesUpdateNative";
        private const string PROC_PATIENT_PREF_FILL_Native = "Patient.sp_PatientPreferencesFillNative";
        private const string PROC_DASHBOARD_CHECKINPATIENTS = "System.sp_Dashboard_CHECKINPatientsNative";
        private const string PROC_DASHBOARD_CHECKINPATIENTS_REQUEST = "System.sp_Dashboard_CHECKINPatientsRequestsNative";
        private const string PROC_GetLatest_Slot_ForAppointment = "Patient.sp_GetLatestSlotForAppointment";
        //    private const string PROC_DASHBOARD_CHECKINPATIENTS = "bkp.sp_Dashboard_CHECKINPatientsNative_1";
        
        private const string PROC_INSURANCE_PLAN_UPDATE_NATIVE = "Patient.sp_PatientInsuranceUpdateNative";
        private const string PROC_INSURANCE_PLAN_INSERT_NATIVE = "Patient.sp_PatientInsuranceInsertNative";

        private const string PROC_HOSPITALIZATIONHX_Disease_SELECT_Native = "Clinical.sp_HospitalizationHx_DiseaseSelectNative";
        private const string PROC_SurgicalHx_Disease_SELECT_Native = "Clinical.sp_SurgicalHx_DiseaseSelectNative";
        private const string PROC_BirthHx_Newborn_SelectNative = "Clinical.sp_BirthHx_Newborn_SelectNative";
        private const string PROC_BirthHx_General_SelectNative = "Clinical.sp_BirthHx_General_SelectNative";
        private const string PROC_BirthHx_Maternal_SelectNative = "Clinical.sp_BirthHx_MaternalDelivery_SelectNative";

        //  private const string PROC_INSERT_DBAUDIT_DETAIL_NATIVE = "system.sp_InsertDBAuditDetailNative";
        //private const string PROC_PATIENT_CONSENT_INSERT = "Patient.sp_PatientConsentInsert";
        //private const string PROC_PATIENT_CONSENT_UPDATE = "Patient.sp_PatientConsentUpdate";
        //private const string PROC_PATIENT_CONSENT_DELETE = "Patient.sp_PatientConsentDelete";
        //private const string PROC_PATIENT_CONSENT_SELECT = "Patient.sp_PatientConsentSelect";
        private const string PROC_FAMILYHX_NATIVESELECT = "Clinical.sp_FamilyHxNativeSelect";
        private const string PROC_FAMILYHX_DISEASE_NATIV_ESELECT = "Clinical.sp_FamilyHx_DiseaseNativeSelect";

        #region NATIVE SPs

        private const string PROC_PATIENT_LOOKUP_NATIVE = "Patient.sp_PatientsLookupNative";
        private const string PROC_PATIENT_MOST_VIEWED_LOOKUP_NATIVE = "Patient.sp_MostViewedPatients";

        #endregion

        #endregion

        #region Parameters
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_DIMMY_PATIENT_ID = "@DimmyPatientId";
        private const string PARM_CONTACT_ID = "@ContactId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_PAT_SEARCH_STRING = "@SearchString";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_PREFIX = "@Prefix";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_SUFFIX = "@Suffix";
        private const string PARM_PREVIOUS_NAME = "@PreviousName";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_DOB = "@DOB";
        private const string PARM_DOD = "@DOD";
        private const string PARM_CAUSE_OF_DEATH = "@CauseOfDeath";
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
        private const string PARM_IS_TCM = "@IsTCM";
        private const string PARM_DODISCHARGE = "@DischargeDate";

        
        private const string PARM_ELIGIBILITY_DATE = "@EligibilityDate";
        private const string PARM_CELL_NO = "@CellNo";
        private const string PARM_FAX_NO = "@FaxNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";


        private const string PARM_REFFERING_PROVIDER_ID = "@ReferringProviderId";
        private const string PARM_PCP_ID = "@PCPId";
        private const string PARM_GUARANTOR_ID = "@GuarantorId";
        private const string PARM_ETHNICITY_ID = "@EthnicityId";
        private const string PARM_RACE_ID = "@RaceId";
        private const string PARM_PREF_LANGUAGE_ID = "@PrefLanguageId";
        private const string PARM_SMOKING_STATUS_ID = "@SmokingStatusId";
        private const string PARM_PREF_COMMUNICATION_ID = "@PrefCommunicationId";
        private const string PARM_2NDPREF_COMMUNICATION_ID = "@scndPrefCommunicationId";
        private const string PARM_ADV_DIRECTIVE_ID = "@AdvanceDirectiveId";
        private const string PARM_SCHOOL_ID = "@SchoolId";
        private const string PARM_SCHOOL_STATUS = "@SchoolStatus";
        private const string PARM_BAD_ADDRESS = "@BadAddress";
        private const string PARM_PATIENT_STATEMENT = "@PatientStatement";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_RELATION_SHIP_ID = "@RelationShipId";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_Page_NUMBER = "@PageNumber";
        private const string PARM_ROWS_P_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PATIENT_IMAGE = "@PatientImage";
        private const string PARM_PATIENT_DOCUMENT_IMAGE = "@PatientDocumentImage";
        private const string PARM_PATIENT_IMAGE_TYPE = "@ImageType";
        private const string PARM_PATIENT_INACTIVE_REASON = "@InActiveReason";
        
        private const string PARM_ENTITY = "@EntityId";
        private const string PARM_IS_SEARCH = "@IsSearch";
        private const string PARM_COMMENTS = "@Comments";
        // Begin 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
        private const string PARM_COVERAGE_TYPE = "@CoverageType";
        // end 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
        private const string PARM_RCOPIAID = "@RcopiaID";
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
        private const string PARM_CLAIM_NO = "@ClaimNumber";
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018

        private const string PARM_INSURANCEPLAN_ID = "@InsurancePlanId";
        private const string PARM_File_Stream = "@IsFileStream";
        private const string PARM_SELF_PAY = "@SelfPay";

        private const string PARM_PATIENT_PORTAL_STATUS = "@PatientPortalStatus";
        private const string PARM_PATIENT_STRRACEIDS = "@StrRaceIds";
        private const string PARM_IS_RECENT_PATIENT = "@IsRecentPatient";

        private const string PARM_ACCESSED_PATIENTID = "@AccessedPatientId";
        private const string PARM_ACCESSED_ON = "@AccessedOn";
        private const string COMMUNICATE_WIH_GUARANTOR = "@CommunicatewithGuarantor";
        private const string COMMUNICATION_OPTOUT = "@CommunicationOptout";
        private const string PARM_MODULE_NAME = "@Module";
        private const string PARM_HEARFROMID = "@HearFromId";
        private const string PARM_HEARFROM_OTHER = "@HearFromOther";
        private const string PARM_RCOPIA_ID = "@RcopiaID";
        private const string PARM_ACCOUNT_NOTE_COMMENTS = "@AccountNoteComments";
        private const string PARM_PATIENT_IMAGE_THUMBNAIL = "PatientImageThumbnail";

        private const string PARM_PATIENT_CONSENTID = "@PatientConsentId";
        private const string PARM_PATIENT_NAME = "@PatientName";
        private const string PARM_ERX_CONSENT_SIGN = "@ERxConsentSign";
        private const string PARM_ERX_CONSENT_DATE = "@ERxConsentDate";
        private const string PARM_NPP_CONSENT_SIGN = "@NPPConsentSign";
        private const string PARM_NPP_CONSENT_DATE = "@NPPConsentDate";
        private const string PARM_FP_CONSENT_SIGN = "@FPConsentSign";
        private const string PARM_FP_CONSENT_DATE = "@FPConsentDate";
        private const string PARM_ABN_CONSENT_SIGN = "@ABNConsentSign";
        private const string PARM_ABN_CONSENT_DATE = "@ABNConsentDate";
        private const string PARM_NOTIFIER = "@Notifier";
        private const string PARM_IDENTIFICATION_NO = "@IdentificationNo";
        private const string PARM_EKG = "@EKG";
        private const string PARM_HOMECCULT = "@Homeccult";
        private const string PARM_CULTURES = "@Cultures";
        private const string PARM_SUPPLIER_AND_MATERIALS = "@SupplierAndMaterials";
        private const string PARM_LABWORK = "@LabWork";
        private const string PARM_VACCINE = "@Vaccine";
        private const string PARM_PFT = "@PFT";
        private const string PARM_UA = "@UA";
        private const string PARM_OTHERS = "@Others";
        private const string PARM_MEDICARE_REASON_1 = "@MedicareReason1";
        private const string PARM_MEDICARE_REASON_2 = "@MedicareReason2";
        private const string PARM_MEDICARE_REASON_3 = "@MedicareReason3";
        private const string PARM_MEDICARE_REASON_4 = "@MedicareReason4";
        private const string PARM_UPTO50 = "@UpTo50";
        private const string PARM_UPTO50TO100 = "@UpTo50To100";
        private const string PARM_UPTO100TO200 = "@UpTo100To200";
        private const string PARM_UPTO200TO300 = "@UpTo200To300";
        private const string PARM_MORETHAN300 = "@MoreThan300";
        private const string PARM_OPTION_1 = "@Option1";
        private const string PARM_OPTION_2 = "@Option2";
        private const string PARM_OPTION_3 = "@Option3";
        private const string PARM_URL = "@Url";

        private const string PARM_PrefCommModeId = "@PrefCommModeId";
        private const string PARM_PreferredPrimaryContactId = "@PreferredPrimaryContactId";
        private const string PARM_RevokeReminderService = "@RevokeReminderService";

        private const string PARM_COLUMN_KEY_ID = "@ColumnKeyId";
        private const string PARM_COLUMN_KEY_NAME = "@ColumnKeyName";
        private const string PARM_COLUMN_NAME = "@ColumnName";
        private const string PARM_CURRENT_VALUE_DISPLAY = "@CurrentValueDisplay";
        private const string PARM_DB_TABLE_NAME = "@DBTableName";
        private const string PARM_IS_SYNCED = "@IsSynced";
        private const string PARM_ORIGINAL_VALUE_DISPLAY = "@OriginalValueDisplay";
        private const string PARM_DBAUDIT_ID = "@DBAuditId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_STATUS = "@Status";

        private const string PARM_HOSPITALIZATION_HX_ID = "@HospitalizationHxId";
        private const string PARM_SURGICAL_HX_ID = "@SurgicalHxId";
        private const string PARM_MEDICAL_HX_ID = "@MedicalHxId";
        private const string PARM_FAMILY_HX_ID = "@FamilyHxId";
        private const string PARM_BIRTH_HX_ID = "@BirthHxId";
        private const string PARM_SOCIAL_HX_ID = "@SocialHxId";
        private const string PARM_MISC_HX_ID = "@MiscHxId";


        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_SUBSCRIBER_ID = "@SubscriberId";
        private const string PARM_GROUP_ID = "@GroupId";
        private const string PARM_PLAN_PRIORITY = "@PlanPriority";


        private const string PARM_REQUEST_STATUS = "@RequestStatus";


        private const string PARM_ICD_10 = "@ICD10";
        private const string PARM_ICD_9 = "@ICD9";
        private const string PARM_ICD9DESCRIPTION_ = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10DESCRIPTION_ = "@ICD10_DESCRIPTION";
        private const string PARM_SNOMEDID_ = "@SNOMEDID";
        private const string PARM_SNOMEDDESCRIPTION_ = "@SNOMED_DESCRIPTION";
        private const string PARM_ICDLookupId = "@ICDLookupId";
        private const string PARM_FAMILY_MEMBER_ID = "@FamilyMemberId";
        #endregion
        #region Patients


        #endregion
        private void createParametersDemographicsDBAuditNative(IDBManager dbManager, DataChangeRequest model)
        {
            dbManager.CreateParameters(15);

            dbManager.AddParameters(0, PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(1, PARM_COLUMN_KEY_ID, model.ColumnKeyId);
            dbManager.AddParameters(2, PARM_COLUMN_KEY_NAME, model.ColumnKeyName);
            dbManager.AddParameters(3, PARM_COLUMN_NAME, model.columnName);
            dbManager.AddParameters(4, PARM_CREATED_BY, model.CreatedBy);
            dbManager.AddParameters(5, PARM_CREATED_ON, model.CreatedOn);
            dbManager.AddParameters(6, PARM_CURRENT_VALUE_DISPLAY, model.CurrentValueDisplay);
            dbManager.AddParameters(7, PARM_DB_TABLE_NAME, model.DBTableName);
            dbManager.AddParameters(8, PARM_ENTITY_ID, model.EntityId);
            dbManager.AddParameters(9, PARM_IS_SYNCED, model.IsSynced);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(12, PARM_ORIGINAL_VALUE_DISPLAY, model.OriginalValueDisplay);
            dbManager.AddParameters(13, PARM_DIMMY_PATIENT_ID, model.DimmyPatientId);

            //dbManager.AddParameters(13, PARM_DBAUDIT_ID, );
            dbManager.AddParameters(14, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            // History Parameters.
            //dbManager.AddParameters(14, PARM_SURGICAL_HX_ID, model.SurgicalHxId);
            //dbManager.AddParameters(15, PARM_HOSPITALIZATION_HX_ID, model.HospitalizationHxId);
            //dbManager.AddParameters(16, PARM_MEDICAL_HX_ID, model.MedicalHxId);
            //dbManager.AddParameters(17, PARM_BIRTH_HX_ID, model.BirthHxId);
            //dbManager.AddParameters(18, PARM_SOCIAL_HX_ID, model.SocialHxId);
            //dbManager.AddParameters(19, PARM_FAMILY_HX_ID, model.FamilyHxId);
            //dbManager.AddParameters(20, PARM_MISC_HX_ID, model.MiscHxId);
            //------


            //dbManager.AddParameters(24, PARM_SELF_PAY, model.SelfPay);

        }
        private void createParametersNative(IDBManager dbManager, PatientInsuranceModel model, Boolean IsInsert)
        {

            if (IsInsert == true)
            {
                dbManager.CreateParameters(19);

                dbManager.AddParameters(16, "@CoverageTo", model.CoverageTo);
                dbManager.AddParameters(17, PARM_CREATED_BY, model.ModifiedBy);
                dbManager.AddParameters(18, PARM_CREATED_ON, model.ModifiedOn);
            }
            else
            {
                dbManager.CreateParameters(17);
                
                dbManager.AddParameters(16, PARM_COLUMN_NAME, model.listChangedColumns);
            }
            dbManager.AddParameters(0, PARM_INSURANCE_ID, model.InsuranceId);
            dbManager.AddParameters(1, PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, model.InsurancePlanId);
            dbManager.AddParameters(3, PARM_FIRST_NAME, model.SubscriberFirstName);
            dbManager.AddParameters(4, PARM_MI, model.SubscriberMI);
            dbManager.AddParameters(5, PARM_LAST_NAME, model.SubscriberLastName);
            dbManager.AddParameters(6, PARM_RELATION_SHIP_ID, model.RelationShipId);
            dbManager.AddParameters(7, PARM_DOB, model.SubscriberDOB);
            dbManager.AddParameters(8, PARM_SUBSCRIBER_ID, model.SubscriberId);
            dbManager.AddParameters(9, PARM_GROUP_ID, model.GroupId);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(12, PARM_PLAN_PRIORITY, model.PlanPriority);
            dbManager.AddParameters(13, PARM_GENDER, model.Gender);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(15, PARM_COMMENTS, model.Comments);
        }
        private void CreateParameters_ICDLookUp(IDBManager dbManager, DSCodeLookup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICDLookupId, ds.ICDLookup.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICDLookupId, ds.ICDLookup.IdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ICD_9, ds.ICDLookup.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ICD9DESCRIPTION_, ds.ICDLookup.ICD9_DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_ICD_10, ds.ICDLookup.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICD10DESCRIPTION_, ds.ICDLookup.ICD10_DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_SNOMEDID_, ds.ICDLookup.SNOMEDIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMEDDESCRIPTION_, ds.ICDLookup.SNOMED_DescriptionColumn.ColumnName, DbType.String);

        }
        private void CreateParametersEmergencyContact(IDBManager dbManager,PatientEmergencyContactModel Model)
        {

            if (Convert.ToInt64(Model.ContactId) > 0)
            {

                dbManager.CreateParameters(26);
                dbManager.AddParameters(25, PARM_COLUMN_NAME, Model.listChangedColumns);
            }
            else
            {

                dbManager.CreateParameters(25);
            }
            
            if (Model.IsPrimary == "1")
            {
                Model.IsPrimary = "True";
            }
            else
            {
                Model.IsPrimary = "False";
            }
            if (Model.IsActive == "1")
            {
                Model.IsActive = "True";
            }
            else
            {
                Model.IsActive = "False";
            }

            dbManager.AddParameters(0, PARM_CONTACT_ID, Convert.ToInt64( Model.ContactId));
            dbManager.AddParameters(1, PARM_PATIENT_ID, Model.PatientId);
            dbManager.AddParameters(2, PARM_LAST_NAME, Model.LastName);
            dbManager.AddParameters(3, PARM_FIRST_NAME, Model.FirstName );
            dbManager.AddParameters(4, PARM_MI,Model.MI );
            dbManager.AddParameters(5, PARM_DOB,  Model.DOB);
            dbManager.AddParameters(6, PARM_ADDRESS1, Model.Address1);
            dbManager.AddParameters(7, PARM_ADDRESS2, Model.Address2);
            dbManager.AddParameters(8, PARM_CITY, Model.City);
            dbManager.AddParameters(9, PARM_STATE, Model.State);
            dbManager.AddParameters(10, PARM_ZIPCODE, Model.Zip);
            dbManager.AddParameters(11, PARM_ZIPCODE_EXT,Model.ZipExt );
            dbManager.AddParameters(12, PARM_HOME_PHONE_NO,Model.HomePhone);
            dbManager.AddParameters(13, PARM_WORK_PHONE_NO, Model.WorkPhone);
            dbManager.AddParameters(14, PARM_WORK_PHONE_EXT, Model.WorkPhext);
            dbManager.AddParameters(15, PARM_CELL_NO, Model.CellNo);
            dbManager.AddParameters(16, PARM_FAX_NO, Model.FaxNo);
            dbManager.AddParameters(17, PARM_EMAIL_ADDRESS, Model.EmailAddress);
            dbManager.AddParameters(18, PARM_IS_PRIMARY, Convert.ToBoolean( Model.IsPrimary));
            dbManager.AddParameters(19, PARM_IS_ACTIVE, Convert.ToBoolean( Model.IsActive));
            dbManager.AddParameters(20, PARM_CREATED_BY,Model.CreatedBy );
            dbManager.AddParameters(21, PARM_CREATED_ON, Model.CreatedOn);
            dbManager.AddParameters(22, PARM_MODIFIED_BY,Model.ModifiedBy );
            dbManager.AddParameters(23, PARM_MODIFIED_ON,Model.ModifiedOn );
            dbManager.AddParameters(24, PARM_RELATION_SHIP_ID,Convert.ToInt64( Model.RelationShipId));
          
        }
        public string InsertupdateRecordInDBAuditNative(IDBManager dbManager, DataChangeRequest model)
        {
            try
            {
               
                string returnVal = "";
                createParametersDemographicsDBAuditNative(dbManager, model);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSERT_UPDATE_DBAUDIT_NATIVE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();


                return returnVal;




            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_INSERT_UPDATE_DBAUDIT_NATIVE", PROC_INSERT_UPDATE_DBAUDIT_NATIVE, ex);

                throw ex;
            }

        }
        public DSProcedures InsertCPTLookup(ref DSProcedures ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, "@CPTLookupId", ds.CPTLookup.CPTLookupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, "@CPTCode", ds.CPTLookup.CPTCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@CPT_Description", ds.CPTLookup.CPT_DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@SNOMEDId", ds.CPTLookup.SNOMEDIdColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@SNOMED_Description", ds.CPTLookup.SNOMED_DescriptionColumn.ColumnName, DbType.String);
                ds = (DSProcedures)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_CPTLOOKUP_INSERT, ds, ds.CPTLookup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::InsertCPTLookup", PROC_PROCEDURE_CPTLOOKUP_INSERT, ex);
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
        public DSCodeLookup InsertICDLookup(ref DSCodeLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters_ICDLookUp(dbManager, ds, true);
                ds = (DSCodeLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDLookup_INSERT, ds, ds.ICDLookup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::InsertICDLookup", PROC_ICDLookup_INSERT, ex);
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
        public int CheckExistingRecord(IDBManager dbManager,string PatientId,string DbTableName,string ColumnKeyName)
        {
            try
            {
                SqlDataReader reader = null;

                int returnVal = 0;
                dbManager.CreateParameters(4);
              
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_DB_TABLE_NAME, DbTableName);
                dbManager.AddParameters(2, PARM_COLUMN_KEY_NAME, ColumnKeyName);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CHECK_EXISTING_RECORD_NATIVE);

                while (reader.Read())
                {

                returnVal=    Convert.ToInt32(reader["RecordCount"]);
                }

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();


                return returnVal;




            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_INSERT_UPDATE_DBAUDIT_NATIVE", PROC_INSERT_UPDATE_DBAUDIT_NATIVE, ex);

                throw ex;
            }

        }

        public int CheckExistingPatients(IDBManager dbManager, string FirstName, string LastName, string DOB, string Mobile, string gender)
        {
            try
            {
                SqlDataReader reader = null;

                int returnVal = 0;
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(1, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(2, PARM_DOB, DOB);
                dbManager.AddParameters(3, PARM_CELL_NO, Mobile);
                dbManager.AddParameters(4, PARM_GENDER, gender);
                dbManager.AddParameters(5, "@PatientId", "PatientId", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CHECK_EXISTING_PATIENT_RECORD);

                while (reader.Read())
                {

                    returnVal = Convert.ToInt32(reader["PatientId"]);
                }

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_CHECK_EXISTING_PATIENT_RECORD", PROC_INSERT_UPDATE_DBAUDIT_NATIVE, ex);

                throw ex;
            }

        }

        public string CheckExistingPatientByInsuranceId(IDBManager dbManager, string SubscriberId,string expiryDate)
        {
            try
            {
                SqlDataReader reader = null;

                string returnVal="";
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_SUBSCRIBER_ID,Int64.Parse( SubscriberId));
                dbManager.AddParameters(1, "@ExpiryDate", expiryDate);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_Get_PatientBy_Insurance_SubscriberId);

                while (reader.Read())
                {

                    returnVal = reader["PatientId"] !=null ? reader["PatientId"].ToString() :"" ;
                    break;
                }

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::CheckExistingPatientByInsuranceId", PROC_Get_PatientBy_Insurance_SubscriberId, ex);

                throw ex;
            }

        }

        public int loadMaxPatientInsurancesPriority(long PatientId)
        {

            SqlDataReader reader = null;
        

         
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int returnVal = 0;
                dbManager.Open();
                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, "@PlanPriority", "PlanPriority", DbType.Int64, ParamDirection.Output);



                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_NATIVE);
                while (reader.Read())
                {

                    returnVal = Convert.ToInt32(reader["PlanPriority"]);
                }

                return returnVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_NATIVE", PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public int CheckPlanPriority(IDBManager dbManager, string PatientId, string DbTableName, string InsuranceId)
        {
            try
            {
                SqlDataReader reader = null;

                int returnVal = 0;
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_DB_TABLE_NAME, DbTableName);
                dbManager.AddParameters(2, "@InsuranceId", InsuranceId);
                dbManager.AddParameters(3, "@PlanPriority", "PlanPriority", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CHECK_INSURANCE_PLAN_PRIORITY);

                while (reader.Read())
                {

                    returnVal = Convert.ToInt32(reader["PlanPriority"]);
                }

                return returnVal;




            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_CHECK_INSURANCE_PLAN_PRIORITY", PROC_CHECK_INSURANCE_PLAN_PRIORITY, ex);

                throw ex;
            }

        }
        public int GetMaxPlanPriorityFromDbAuditNative(IDBManager dbManager, string PatientId)
        {
            try
            {
                SqlDataReader reader = null;

                int returnVal = 0;
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
      
                dbManager.AddParameters(1, "@PlanPriority", "PlanPriority", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_DBAUDIT_NATIVE);

                while (reader.Read())
                {

                    returnVal = Convert.ToInt32(reader["PlanPriority"]);
                }

                return returnVal;




            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_DBAUDIT_NATIVE", PROC_GET_MAX_INSURANCE_PLAN_PRIORITY_DBAUDIT_NATIVE, ex);

                throw ex;
            }

        }
        public int CheckExistingRecordForInsurance(IDBManager dbManager, string PatientId,long InsurancePlanId, string DbTableName)
        {
            try
            {
                SqlDataReader reader = null;

                int returnVal = 0;
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_DB_TABLE_NAME, DbTableName);
              
                dbManager.AddParameters(2, "@InsurancePlanId", InsurancePlanId);
             
                dbManager.AddParameters(3, "@InsuranceId", "RecordCount", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CHECK_EXISTING_RECORD_INSURANCE_NATIVE);

                while (reader.Read())
                {

                    returnVal = Convert.ToInt32(reader["InsuranceId"]);
                }
             return returnVal;

         }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_INSERT_UPDATE_DBAUDIT_NATIVE", PROC_CHECK_EXISTING_RECORD_INSURANCE_NATIVE, ex);

                throw ex;
            }

        }
        public string InsertupdateRecordInDBAuditNative( List< DataChangeRequest> lstmodel)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();

            try
            {
             //   dbManager.BeginTransaction();
                string returnVal = "";
                foreach (DataChangeRequest model in lstmodel)
                {
                    string Result = "";

                    createParametersDemographicsDBAuditNative(dbManager, model);
                    Result = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSERT_UPDATE_DBAUDIT_NATIVE).ToString();
                    returnVal = returnVal + Result;
                }
                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

             //   dbManager.CommitTransaction();
                return returnVal;




            }
            catch (Exception ex)
            {
             //   dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::PROC_INSERT_UPDATE_DBAUDIT_NATIVE", PROC_INSERT_UPDATE_DBAUDIT_NATIVE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public List<PatientEmergencyContactModel> loadPatientEmergencyContacts(long PatientId, string RequestStatus, int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSPatient ds = new DSPatient();
            SqlDataReader reader = null;
            PatientEmergencyContactModel model = null;

            List<PatientEmergencyContactModel> lstModel = new List<PatientEmergencyContactModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);

                //  dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                //if (pageNumber == 0)
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                //else
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                //if (rowsPerPage == 0)
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                //else
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);
                //dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACTS_SELECT_NATIVE);
                while (reader.Read())
                {

                    model = new PatientEmergencyContactModel();
                    model.ContactId = reader.IsDBNull(reader.GetOrdinal("ContactId")) ? "" : Convert.ToString(reader["ContactId"]);
                   // model.RecordCount= reader.IsDBNull(reader.GetOrdinal("RecordCount")) ? "" : Convert.ToString(reader["RecordCount"]);



                    lstModel.Add(model);





                }

                return lstModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::loadPatientEmergencyContacts", PROC_PATIENT_EMERGENCYCONTACTS_SELECT_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<PatientInsuranceModel> loadPatientInsurances(long PatientId, string RequestStatus)
        {
       
            SqlDataReader reader = null;
            PatientInsuranceModel model = null;

            List<PatientInsuranceModel> lstModel = new List<PatientInsuranceModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_INSURANCES_SELECT_NATIVE);
                while (reader.Read())
                {
                    model = new PatientInsuranceModel();
                    model.InsurancePlanId = reader.IsDBNull(reader.GetOrdinal("insurancePlanId")) ? "" : Convert.ToString(reader["insurancePlanId"]);
                    model.ColumnKeyId = reader.IsDBNull(reader.GetOrdinal("ColumnkeyId")) ? "" : Convert.ToString(reader["ColumnkeyId"]);
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.PlanPriority= reader.IsDBNull(reader.GetOrdinal("PlanPriority")) ? "" : Convert.ToString(reader["PlanPriority"]);
                    model.SubscriberId = reader.IsDBNull(reader.GetOrdinal("SubscriberId")) ? "" : Convert.ToString(reader["SubscriberId"]);
                    model.GroupId = reader.IsDBNull(reader.GetOrdinal("GroupId")) ? "" : Convert.ToString(reader["GroupId"]);
                    model.SubscriberDOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? "" : Convert.ToString(reader["DOB"]);
                    model.SubscriberFirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? "" : Convert.ToString(reader["FirstName"]);
                    model.SubscriberMI = reader.IsDBNull(reader.GetOrdinal("MI")) ? "" : Convert.ToString(reader["MI"]);
                    model.SubscriberLastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? "" : Convert.ToString(reader["LastName"]);
                    model.RelationShipId = reader.IsDBNull(reader.GetOrdinal("RelationShipId")) ? "" : Convert.ToString(reader["RelationShipId"]);
                    model.Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? "" : Convert.ToString(reader["Gender"]);
                    model.Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? "" : Convert.ToString(reader["Comments"]);
                    lstModel.Add(model);
                }

                return lstModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_PATIENT_INSURANCES_SELECT_NATIVE", PROC_PATIENT_INSURANCES_SELECT_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
       
        public List<MobileAppRequestModel> LoadDashboardCheckInPatients(string Status, long ProviderId, long PatientId, long PageNumber, long RowsPerPage)
        {
            List<MobileAppRequestModel> listModel = new List<MobileAppRequestModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY, MDVSession.Current.EntityId);


                dbManager.AddParameters(2, PARM_STATUS, Status);

                if (PatientId==0)
                    dbManager.AddParameters(3, PARM_PATIENT_ID, null);
                else
                 dbManager.AddParameters(3, PARM_PATIENT_ID, PatientId);

                if(ProviderId==0)
                dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_CHECKINPATIENTS);
                MobileAppRequestModel model = null;
                while (reader.Read())
                {
                    
                    model = new MobileAppRequestModel();
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? "" : Convert.ToString(reader["PatientName"]);
                    model.AccountNumber = reader.IsDBNull(reader.GetOrdinal("AccountNumber")) ? "" : Convert.ToString(reader["AccountNumber"]);
                    model.Provider = reader.IsDBNull(reader.GetOrdinal("PatientProviderName")) ? "" : Convert.ToString(reader["PatientProviderName"]);
                    model.Status= reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : Convert.ToString(reader["Status"]);
                    model.RequestReceivedAt= reader.IsDBNull(reader.GetOrdinal("RequestReceivedAt")) ? "" : Convert.ToString(reader["RequestReceivedAt"]);
                    model.RequestReceivedFor= reader.IsDBNull(reader.GetOrdinal("RequestReceivedFor")) ? "" : Convert.ToString(reader["RequestReceivedFor"]);
                    model.AppointmentDate= reader.IsDBNull(reader.GetOrdinal("AppointmentDate")) ? "" : Convert.ToString(reader["AppointmentDate"]);
                    model.AppointmentReason= reader.IsDBNull(reader.GetOrdinal("AppointmentReason")) ? "" : Convert.ToString(reader["AppointmentReason"]);
                    model.DBTableName= reader.IsDBNull(reader.GetOrdinal("TableName")) ? "" : Convert.ToString(reader["TableName"]);
                    model.RecordCount = reader.IsDBNull(reader.GetOrdinal("RecordCount")) ? "" : Convert.ToString(reader["RecordCount"]);
                    model.DimmyPatientId = reader.IsDBNull(reader.GetOrdinal("DimmyPatientId")) ? "" : Convert.ToString(reader["DimmyPatientId"]);
                    model.Facility = reader.IsDBNull(reader.GetOrdinal("FacilityName")) ? "" : Convert.ToString(reader["FacilityName"]);
                    //   model.Status = Convert.ToString(reader["Status"]);
                    //    model.DateOfAppointment = model.Status == "Signed" ? Convert.ToString(reader["AppointmentDate"]) : "Does not apply";
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::PROC_DASHBOARD_CHECKINPATIENTS", PROC_DASHBOARD_CHECKINPATIENTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MobileAppRequestModel> LoadDashboardCheckInPatientsRequest(string Status, long ProviderId, long PatientId, long PageNumber, long RowsPerPage)
        {
            List<MobileAppRequestModel> listModel = new List<MobileAppRequestModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY, MDVSession.Current.EntityId);


                dbManager.AddParameters(2, PARM_STATUS, Status);

                if (PatientId == 0)
                    dbManager.AddParameters(3, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PATIENT_ID, PatientId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_CHECKINPATIENTS_REQUEST);
                MobileAppRequestModel model = null;
                while (reader.Read())
                {

                    model = new MobileAppRequestModel();
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.AppointmentId = reader.IsDBNull(reader.GetOrdinal("AppointmentId")) ? "" : Convert.ToString(reader["AppointmentId"]);
                    model.PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? "" : Convert.ToString(reader["PatientName"]);
                    model.AccountNumber = reader.IsDBNull(reader.GetOrdinal("AccountNumber")) ? "" : Convert.ToString(reader["AccountNumber"]);
                    model.Provider = reader.IsDBNull(reader.GetOrdinal("PatientProviderName")) ? "" : Convert.ToString(reader["PatientProviderName"]);
                    model.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : Convert.ToString(reader["Status"]);
                    model.RequestReceivedAt = reader.IsDBNull(reader.GetOrdinal("RequestReceivedAt")) ? "" : Convert.ToString(reader["RequestReceivedAt"]);
                    model.RequestReceivedFor = reader.IsDBNull(reader.GetOrdinal("RequestReceivedFor")) ? "" : Convert.ToString(reader["RequestReceivedFor"]);
                    model.AppointmentDate = reader.IsDBNull(reader.GetOrdinal("AppointmentDate")) ? "" : Convert.ToString(reader["AppointmentDate"]);
                    model.AppointmentReason = reader.IsDBNull(reader.GetOrdinal("AppointmentReason")) ? "" : Convert.ToString(reader["AppointmentReason"]);
                    model.DBTableName = reader.IsDBNull(reader.GetOrdinal("TableName")) ? "" : Convert.ToString(reader["TableName"]);
                    //model.RecordCount = reader.IsDBNull(reader.GetOrdinal("RecordCount")) ? "" : Convert.ToString(reader["RecordCount"]);
                    model.DimmyPatientId = reader.IsDBNull(reader.GetOrdinal("DimmyPatientId")) ? "" : Convert.ToString(reader["DimmyPatientId"]);
                    model.Facility = reader.IsDBNull(reader.GetOrdinal("FacilityName")) ? "" : Convert.ToString(reader["FacilityName"]);
                    model.TimeFrom = reader.IsDBNull(reader.GetOrdinal("TimeFrom")) ? "" : Convert.ToString(reader["TimeFrom"]);
                    model.RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? "" : Convert.ToString(reader["RejectionReason"]);
                    model.AppointmentDate = MDVUtility.GetDateMMDDYYY(model.AppointmentDate);
                    //model.RejectReason = reader.IsDBNull(reader.GetOrdinal("RejectReason")) ? "" : Convert.ToString(reader["RejectReason"]);
                    //   model.Status = Convert.ToString(reader["Status"]);
                    //    model.DateOfAppointment = model.Status == "Signed" ? Convert.ToString(reader["AppointmentDate"]) : "Does not apply";
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::PROC_DASHBOARD_CHECKINPATIENTS", PROC_DASHBOARD_CHECKINPATIENTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public PatientEmergencyContactModel FillPatientEmergencyContact(long PatientId, long EmergencyContactId, string RequestStatus)
        {
          
            SqlDataReader reader = null;
            PatientEmergencyContactModel model = null;
            ChangedColumnsNative columnsModel = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

              
       
                    dbManager.AddParameters(1, PARM_CONTACT_ID, EmergencyContactId);
                dbManager.AddParameters(2, PARM_REQUEST_STATUS, RequestStatus);

                //  dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                //if (pageNumber == 0)
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                //else
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                //if (rowsPerPage == 0)
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                //else
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);
                //dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACTS_FILL_NATIVE);
                while (reader.Read())
                {

                    model = new PatientEmergencyContactModel();
                    model.ContactId = reader.IsDBNull(reader.GetOrdinal("ContactId")) ? "" : Convert.ToString(reader["ContactId"]);
                    model.PatientId= reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);

                    model.Address1 = reader.IsDBNull(reader.GetOrdinal("Address1")) ? "" : Convert.ToString(reader["Address1"]);
                    model.CellNo = reader.IsDBNull(reader.GetOrdinal("CellNo")) ? "" : Convert.ToString(reader["CellNo"]);
                    model.FaxNo = reader.IsDBNull(reader.GetOrdinal("FaxNo")) ? "" : Convert.ToString(reader["FaxNo"]);
                    model.City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : Convert.ToString(reader["City"]);

                    model.DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? "" : Convert.ToDateTime( (reader["DOB"])).ToShortDateString();

                    model.EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : Convert.ToString(reader["EmailAddress"]);


                   
                    model.FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : Convert.ToString(reader["FirstName"]);
                   
                  
                 
                    model.LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : Convert.ToString(reader["LastName"]);
                  
                    model.MI = reader.IsDBNull(reader.GetOrdinal("MI")) ? null : Convert.ToString(reader["MI"]);
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : Convert.ToString(reader["ModifiedBy"]);
                    model.ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? "" : Convert.ToString((reader["ModifiedOn"])); 

                    if (Convert.ToInt64(model.ContactId) < 1)
                    {
                        model.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : Convert.ToString(reader["CreatedBy"]);
                        model.CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? "" : Convert.ToString((reader["CreatedOn"]));

                    }
                    model.HomePhone= reader.IsDBNull(reader.GetOrdinal("HomePhone")) ? null : Convert.ToString(reader["HomePhone"]);
                    model.WorkPhone = reader.IsDBNull(reader.GetOrdinal("WorkPhone")) ? null : Convert.ToString(reader["WorkPhone"]);
                    model.WorkPhext = reader.IsDBNull(reader.GetOrdinal("WorkPhext")) ? null : Convert.ToString(reader["WorkPhext"]);


                    model.State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : Convert.ToString(reader["State"]);
                   
                    model.Zip = reader.IsDBNull(reader.GetOrdinal("ZipCode")) ? null : Convert.ToString(reader["ZipCode"]);
                    model.ZipExt = reader.IsDBNull(reader.GetOrdinal("ZipExt")) ? null : Convert.ToString(reader["ZipExt"]);
                    model.IsActive= Convert.ToString(reader["isActive"]);
                    model.IsPrimary = Convert.ToString(reader["IsPrimary"]);
                    model.RelationShipId = reader.IsDBNull(reader.GetOrdinal("RelationShipId")) ? null : Convert.ToString(reader["RelationShipId"]);
                    model.Address2= reader.IsDBNull(reader.GetOrdinal("Address2")) ? null : Convert.ToString(reader["Address2"]);


                }
                reader.NextResult();
                while (reader.Read())
                {
                    columnsModel = new ChangedColumnsNative();
                    columnsModel.columnName = reader.IsDBNull(reader.GetOrdinal("columnName")) ? "" : Convert.ToString(reader["columnName"]);
                    columnsModel.CurrentValueDisplay = reader.IsDBNull(reader.GetOrdinal("CurrentValueDisplay")) ? "" : Convert.ToString(reader["CurrentValueDisplay"]);
                    model.lstChangedColumns.Add(columnsModel);





                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::loadPatientEmergencyContacts", PROC_PATIENT_EMERGENCYCONTACTS_SELECT_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public PatientInsuranceModel FillPatientInsurance(long PatientId, long InsuranceId, string RequestStatus)
        {

            SqlDataReader reader = null;
            PatientInsuranceModel model = null;
            ChangedColumnsNative columnsModel = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);



                dbManager.AddParameters(1, PARM_INSURANCE_ID, InsuranceId);

                dbManager.AddParameters(2, PARM_REQUEST_STATUS, RequestStatus);

                //  dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                //if (pageNumber == 0)
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                //else
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                //if (rowsPerPage == 0)
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                //else
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);
                //dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_FILL_NATIVE);
                while (reader.Read())
                {

                    model = new PatientInsuranceModel();
                    model.InsuranceId = !String.IsNullOrEmpty(reader["InsuranceId"].ToString()) ? reader["InsuranceId"].ToString() : "";
                    model.SubscriberId = !String.IsNullOrEmpty(reader["SubscriberId"].ToString()) ? reader["SubscriberId"].ToString() : "";
                    model.GroupId = !String.IsNullOrEmpty(reader["GroupId"].ToString()) ? reader["GroupId"].ToString() : "";
                    model.SubscriberDOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? Convert.ToDateTime( reader["DOB"]).ToShortDateString() : "";
                    model.SubscriberFirstName = !String.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "";
                    model.SubscriberMI = !String.IsNullOrEmpty(reader["MI"].ToString()) ? reader["MI"].ToString() : "";
                    model.SubscriberLastName = !String.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "";
                    model.PlanPriority = !String.IsNullOrEmpty(reader["PlanPriority"].ToString()) ? reader["PlanPriority"].ToString() : "";
                    model.InsurancePlanId = !String.IsNullOrEmpty(reader["InsurancePlanId"].ToString()) ? reader["InsurancePlanId"].ToString() : "";
                    model.InsurancePlanName = !String.IsNullOrEmpty(reader["InsurancePlanName"].ToString()) ? reader["InsurancePlanName"].ToString() : "";
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.RelationShipId = !String.IsNullOrEmpty(reader["RelationShipId"].ToString()) ? reader["RelationShipId"].ToString() : "";
                    model.RelationName = !String.IsNullOrEmpty(reader["RelationName"].ToString()) ? reader["RelationName"].ToString() : "";
                    model.Gender = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    model.CoverageTo = !String.IsNullOrEmpty(reader["CoverageTo"].ToString()) ? reader["CoverageTo"].ToString() : "";
                    model.ModifiedBy= reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : Convert.ToString(reader["CreatedBy"]);
                    model.ModifiedOn= reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? null : Convert.ToString(reader["ModifiedOn"]);
                    model.Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : Convert.ToString(reader["Comments"]);

                    if (Convert.ToString(reader["IsActive"])=="1")
                    model.IsActive= true;
                    else
                        model.IsActive = false;


                    if (Convert.ToInt64(model.InsuranceId) < 1)
                    {
                        model.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : Convert.ToString(reader["CreatedBy"]);
                        model.CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? null : Convert.ToString(reader["CreatedOn"]);

                    }
                   
                  











                }
                reader.NextResult();
                while (reader.Read())
                {
                    columnsModel = new ChangedColumnsNative();
                    columnsModel.columnName = reader.IsDBNull(reader.GetOrdinal("columnName")) ? "" : Convert.ToString(reader["columnName"]);
                    columnsModel.CurrentValueDisplay = reader.IsDBNull(reader.GetOrdinal("CurrentValueDisplay")) ? "" : Convert.ToString(reader["CurrentValueDisplay"]);
                    model.lstChangedColumns.Add(columnsModel);





                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::PROC_PATIENT_INSURANCE_FILL_NATIVE", PROC_PATIENT_INSURANCE_FILL_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public EmptySlotModel FillPatientAppointment(long PatientId, string RequestStatus)
        {

            SqlDataReader reader = null;
            EmptySlotModel model = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_Scheduler_FILL_NATIVE);
                while (reader.Read())
                {
                    model = new EmptySlotModel();
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";
                    model.FacilityId = !String.IsNullOrEmpty(reader["FacilityId"].ToString()) ? reader["FacilityId"].ToString() : "";
                    model.AppointmentDate = !String.IsNullOrEmpty(reader["AppointmentDate"].ToString()) ?reader["AppointmentDate"].ToString().Replace("12: 00: 00AM", ""): "";
                    model.TimeFrom = !String.IsNullOrEmpty(reader["TimeFrom"].ToString()) ? reader["TimeFrom"].ToString() : "";
                    model.TimeTo = !String.IsNullOrEmpty(reader["TimeTo"].ToString()) ? reader["TimeTo"].ToString() : "";
                    model.PatientTypeId = !String.IsNullOrEmpty(reader["PatientTypeId"].ToString()) ? reader["PatientTypeId"].ToString() : "";
                    model.MRNumber = !String.IsNullOrEmpty(reader["MRNumber"].ToString()) ? reader["MRNumber"].ToString() : "";
                    model.Duration = !String.IsNullOrEmpty(reader["Duration"].ToString()) ? reader["Duration"].ToString() : "";
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : Convert.ToString(reader["CreatedBy"]);
                    model.ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? null : Convert.ToString(reader["ModifiedOn"]);
                    model.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : Convert.ToString(reader["CreatedBy"]);
                    model.CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? null : Convert.ToString(reader["CreatedOn"]);
                }
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMOBILEAPP::PROC_PATIENT_SCHEDULER_FILL_NATIVE", PROC_PATIENT_Scheduler_FILL_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public PatientPreferenceModel FillPatientPreferences(long PatientId, string RequestStatus)
        {

            SqlDataReader reader = null;
            PatientPreferenceModel model = null;
            ChangedColumnsNative columnsModel = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);



                //  dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                //if (pageNumber == 0)
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                //else
                //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                //if (rowsPerPage == 0)
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                //else
                //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);
                //dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_PREF_FILL_Native);
                while (reader.Read())
                {

                    model = new PatientPreferenceModel();
                   
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);

                    model.CommunicationOptout = reader.IsDBNull(reader.GetOrdinal("CommunicationOptout")) ? "0" : Convert.ToString(reader["CommunicationOptout"]);
                    model.PatientStatement = reader.IsDBNull(reader.GetOrdinal("PatientStatement")) ? "0" : Convert.ToString(reader["PatientStatement"]);
                    model.ScndPrefCommunicationId = reader.IsDBNull(reader.GetOrdinal("ScndPrefCommunicationId")) ? "" : Convert.ToString(reader["ScndPrefCommunicationId"]);
                    model.PrefCommunicationId = reader.IsDBNull(reader.GetOrdinal("PrefCommunicationId")) ? null : Convert.ToString(reader["PrefCommunicationId"]);
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : Convert.ToString(reader["ModifiedBy"]);
                    model.ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"]); 

                }
                reader.NextResult();
                while (reader.Read())
                {
                    columnsModel = new ChangedColumnsNative();
                    columnsModel.columnName = reader.IsDBNull(reader.GetOrdinal("columnName")) ? "" : Convert.ToString(reader["columnName"]);
                    columnsModel.CurrentValueDisplay = reader.IsDBNull(reader.GetOrdinal("CurrentValueDisplay")) ? "" : Convert.ToString(reader["CurrentValueDisplay"]);
                    model.lstChangedColumns.Add(columnsModel);
                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::PROC_PATIENT_PREF_FILL_Native", PROC_PATIENT_PREF_FILL_Native, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertPatientEmergencyContact(PatientEmergencyContactModel PEC)
        {

           
          


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";
                CreateParametersEmergencyContact(dbManager, PEC);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACT_INSERT).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;

               

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::PROC_PATIENT_EMERGENCYCONTACT_INSERT", PROC_PATIENT_EMERGENCYCONTACT_INSERT, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertPatientInsurance(PatientInsuranceModel PEC)
        {





            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";
                createParametersNative(dbManager, PEC, true);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_INSERT_NATIVE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;



            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::PROC_INSURANCE_PLAN_INSERT_NATIVE", PROC_INSURANCE_PLAN_INSERT_NATIVE, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdatePatientInsurance(PatientInsuranceModel PEC)
        {





            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";
                
                createParametersNative(dbManager, PEC,false);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_UPDATE_NATIVE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;



            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::PROC_INSURANCE_PLAN_UPDATE_NATIVE", PROC_INSURANCE_PLAN_UPDATE_NATIVE, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DiscardRecord(long PatientID, long ColumnkeyId, string DBTableName,string changedColumnsString)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";

                dbManager.CreateParameters(4);
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);
                    dbManager.AddParameters(1, PARM_COLUMN_KEY_ID, ColumnkeyId);
                    dbManager.AddParameters(2, PARM_DB_TABLE_NAME, DBTableName);
                if(changedColumnsString!="")
                    dbManager.AddParameters(3, PARM_COLUMN_NAME, changedColumnsString);
                else
                    dbManager.AddParameters(3, PARM_COLUMN_NAME, null);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DISCARD_RECORD_DBAUDIT_NATIVE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_DISCARD_RECORD_DBAUDIT_NATIVE", PROC_DISCARD_RECORD_DBAUDIT_NATIVE, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DiscardAllRecord(long PatientID) 
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);
                dbManager.AddParameters(1, "ReturnMessage", "", DbType.String, ParamDirection.Output, null, 500);



                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DISCARD_ALL_RECORD_DBAUDIT_NATIVE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_DISCARD_ALL_RECORD_DBAUDIT_NATIVE", PROC_DISCARD_ALL_RECORD_DBAUDIT_NATIVE, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string ApproveAllRecord(long PatientID)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);
                dbManager.AddParameters(1, "ReturnMessage", "", DbType.String, ParamDirection.Output, null, 500);



                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_APPROVE_ALL_RECORD_DBAUDIT_NATIVE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_APPROVE_ALL_RECORD_DBAUDIT_NATIVE", PROC_APPROVE_ALL_RECORD_DBAUDIT_NATIVE, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdatePatientEmergencyContact(PatientEmergencyContactModel PEC)
        {


        


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string returnVal = "";
                CreateParametersEmergencyContact(dbManager, PEC);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACT_UPDATE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;



            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::PROC_PATIENT_EMERGENCYCONTACT_UPDATE", PROC_PATIENT_EMERGENCYCONTACT_UPDATE, ex);

                dbManager.Dispose();
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdatePatientPreferences(PatientPreferenceModel Model)
        {
            // DSPatient ds = new DSPatient();
            var dbManager = ClientConfiguration.GetDBManager();
            var dsDBAudit = new DSDBAudit();
            try
            {
                if(Model.PrefCommunicationId == "Select")
                {
                    Model.PrefCommunicationId = "";
                }
                if (Model.ScndPrefCommunicationId == "Select")
                {
                    Model.ScndPrefCommunicationId = "";
                }
                dbManager.Open();
                dbManager.CreateParameters(8);

                dbManager.AddParameters(0, PARM_PATIENT_ID, Model.PatientId);
                dbManager.AddParameters(1, PARM_PREF_COMMUNICATION_ID, Model.PrefCommunicationId);
                dbManager.AddParameters(2, PARM_2NDPREF_COMMUNICATION_ID, Model.ScndPrefCommunicationId);
                dbManager.AddParameters(3, COMMUNICATION_OPTOUT, Model.CommunicationOptout);
                dbManager.AddParameters(4, PARM_PATIENT_STATEMENT, Model.PatientStatement);
                  dbManager.AddParameters(5, PARM_MODIFIED_BY, Model.ModifiedBy);
                dbManager.AddParameters(6, PARM_MODIFIED_ON, Model.ModifiedOn);
                dbManager.AddParameters(7, PARM_COLUMN_NAME, Model.listChangedColumns);





                string Result = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_PREF_UPDATE_Native).ToString();

                

             //   dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientIdColumn].ToString());
             //   dsDBAudit.AcceptChanges();

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::UpdatePatientPreferencesNative", PROC_PATIENT_PREF_UPDATE_Native, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region HistoryRegion
        //public List<PatientEmergencyContactModel> LoadHospitalizationHxDiseases(long PatientId, long HospitalizationHxId, int pageNumber = 1, int rowsPerPage = 1000)
        //{
        //    DSPatient ds = new DSPatient();
        //    SqlDataReader reader = null;
        //    PatientEmergencyContactModel model = null;

        //    List<PatientEmergencyContactModel> lstModel = new List<PatientEmergencyContactModel>();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {

        //        dbManager.Open();
        //        dbManager.CreateParameters(2);

        //        if (PatientId <= 0)
        //            dbManager.AddParameters(0, PARM_PATIENT_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

        //        dbManager.AddParameters(1, PARM_REQUEST_STATUS, HospitalizationHxId);

        //        //  dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

        //        //if (pageNumber == 0)
        //        //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
        //        //else
        //        //    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
        //        //if (rowsPerPage == 0)
        //        //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
        //        //else
        //        //    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);
        //        //dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

        //        reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACTS_SELECT_NATIVE);
        //        while (reader.Read())
        //        {

        //            model = new PatientEmergencyContactModel();
        //            model.ContactId = reader.IsDBNull(reader.GetOrdinal("ContactId")) ? "" : Convert.ToString(reader["ContactId"]);
        //            // model.RecordCount= reader.IsDBNull(reader.GetOrdinal("RecordCount")) ? "" : Convert.ToString(reader["RecordCount"]);



        //            lstModel.Add(model);





        //        }

        //        return lstModel;

        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALPatientEmergencyContact::loadPatientEmergencyContacts", PROC_PATIENT_EMERGENCYCONTACTS_SELECT_NATIVE, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        public DSHospitalizationHx LoadHospitalizationHx_Disease( long PatientId, string RequestStatus, Int64 HospitalizationHxDiseaseId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSHospitalizationHx ds = new DSHospitalizationHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

              
                    
      
                    dbManager.AddParameters(0, PARM_REQUEST_STATUS, RequestStatus);

             
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if(HospitalizationHxDiseaseId==0)
                dbManager.AddParameters(2, "@DiseaseId", null);
                else
                    dbManager.AddParameters(2, "@DiseaseId", HospitalizationHxDiseaseId);
                ds = (DSHospitalizationHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_Disease_SELECT_Native, ds, ds.HospitalizationHx_Disease.TableName);
               
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_HOSPITALIZATIONHX_Disease_SELECT_Native", PROC_HOSPITALIZATIONHX_Disease_SELECT_Native, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSurgicalHx LoadSurgicalHx_Disease(long PatientId, string RequestStatus, Int64 SurgicalHxDiseaseId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSurgicalHx ds = new DSSurgicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_REQUEST_STATUS, RequestStatus);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (SurgicalHxDiseaseId == 0)
                    dbManager.AddParameters(2, "@DiseaseId", null);
                else
                    dbManager.AddParameters(2, "@DiseaseId", SurgicalHxDiseaseId);
                ds = (DSSurgicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SurgicalHx_Disease_SELECT_Native, ds, ds.SurgicalHx_Disease.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_SurgicalHx_Disease_SELECT_Native", PROC_SurgicalHx_Disease_SELECT_Native, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public BirthHxNewbornModelNative FillBirthHx_NewBorn(long PatientId,  string RequestStatus)
        {
            
            SqlDataReader reader = null;
            BirthHxNewbornModelNative model = null;
            ChangedColumnsNative columnsModel = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);

                
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_BirthHx_Newborn_SelectNative);
                while (reader.Read())
                {

                    model = new BirthHxNewbornModelNative();
                    //model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? null : Convert.ToString(reader["null"]);
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.NewbornId = reader.IsDBNull(reader.GetOrdinal("NewbornId")) ? "" : Convert.ToString(reader["NewbornId"]);
                    model.BirthHxId = reader.IsDBNull(reader.GetOrdinal("BirthHxId")) ? "" : Convert.ToString(reader["BirthHxId"]);
                    model.HeadCircumference = reader.IsDBNull(reader.GetOrdinal("HeadCircumference")) ? "" : Convert.ToString(reader["HeadCircumference"]);
                    model.ChestCircumference = reader.IsDBNull(reader.GetOrdinal("ChestCircumference")) ? "" : Convert.ToString(reader["ChestCircumference"]);

                    model.WeightAtBirth = reader.IsDBNull(reader.GetOrdinal("WeightAtBirth")) ? "" : Convert.ToString(reader["WeightAtBirth"]);
                    model.LengthAtBirth = reader.IsDBNull(reader.GetOrdinal("LengthAtBirth")) ? "" : Convert.ToString(reader["LengthAtBirth"]);
                    model.ApgarAtBirth = reader.IsDBNull(reader.GetOrdinal("ApgarAtBirth")) ? "" : Convert.ToString(reader["ApgarAtBirth"]);
                    model.ApgarAt5Minutes = reader.IsDBNull(reader.GetOrdinal("ApgarAt5Minutes")) ? "" : Convert.ToString(reader["ApgarAt5Minutes"]);
                    model.WeightReleased = reader.IsDBNull(reader.GetOrdinal("WeightReleased")) ? "" : Convert.ToString(reader["WeightReleased"]);
                    model.PatientBloodTypeId = reader.IsDBNull(reader.GetOrdinal("PatientBloodTypeId")) ? "" : Convert.ToString(reader["PatientBloodTypeId"]);
                    model.ProblemsAtBirthId = reader.IsDBNull(reader.GetOrdinal("ProblemsAtBirthId")) ? "" : Convert.ToString(reader["ProblemsAtBirthId"]);
                    model.bFetalDistress = reader.IsDBNull(reader.GetOrdinal("bFetalDistress")) ? "" : Convert.ToString(reader["bFetalDistress"]);

                    model.NewbornComments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? "" : Convert.ToString(reader["Comments"]);
                    model.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? "" : Convert.ToString(reader["CreatedBy"]);
                    model.CreatedOn =  Convert.ToString(reader["CreatedOn"]);
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? "" : Convert.ToString(reader["ModifiedBy"]);
                    model.ModifiedOn =  Convert.ToString(reader["ModifiedOn"]);
               }
                reader.NextResult();
                while (reader.Read())
                {
                    columnsModel = new ChangedColumnsNative();
                    columnsModel.columnName = reader.IsDBNull(reader.GetOrdinal("columnName")) ? "" : Convert.ToString(reader["columnName"]);
                    columnsModel.CurrentValueDisplay = reader.IsDBNull(reader.GetOrdinal("CurrentValueDisplay")) ? "" : Convert.ToString(reader["CurrentValueDisplay"]);
                    model.lstChangedColumns.Add(columnsModel);

                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::FillBirthHx_NewBorn", PROC_BirthHx_Newborn_SelectNative, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public BirthHxGeneralModelNative FillBirthHx_General(long PatientId, string RequestStatus)
        {

            SqlDataReader reader = null;
            BirthHxGeneralModelNative model = null;
            ChangedColumnsNative columnsModel = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_BirthHx_General_SelectNative);
                while (reader.Read())
                {

                    model = new BirthHxGeneralModelNative();
                    //model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? null : Convert.ToString(reader["null"]);
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.GeneralId = reader.IsDBNull(reader.GetOrdinal("GeneralId")) ? "" : Convert.ToString(reader["GeneralId"]);
                    model.BirthHxId = reader.IsDBNull(reader.GetOrdinal("BirthHxId")) ? "" : Convert.ToString(reader["BirthHxId"]);
                    model.HospitalName = reader.IsDBNull(reader.GetOrdinal("HospitalName")) ? "" : Convert.ToString(reader["HospitalName"]);
                    model.PatientDOB = reader.IsDBNull(reader.GetOrdinal("PatientDOB")) ? "" : Convert.ToString(reader["PatientDOB"]);

                    model.LengthStayatHospital = reader.IsDBNull(reader.GetOrdinal("LengthStayatHospital")) ? "" : Convert.ToString(reader["LengthStayatHospital"]);
                    model.DateAdmitted = reader.IsDBNull(reader.GetOrdinal("DateAdmitted")) ? "" : Convert.ToString(reader["DateAdmitted"]);
                    model.ObstetricianName = reader.IsDBNull(reader.GetOrdinal("ObstetricianName")) ? "" : Convert.ToString(reader["ObstetricianName"]);
                    model.PediatricianName = reader.IsDBNull(reader.GetOrdinal("PediatricianName")) ? "" : Convert.ToString(reader["PediatricianName"]);
                    model.ResponsiblePhysicianId = reader.IsDBNull(reader.GetOrdinal("ResponsiblePhysicianId")) ? "" : Convert.ToString(reader["ResponsiblePhysicianId"]);
                    model.ResponsiblePhysicianId_text = reader.IsDBNull(reader.GetOrdinal("ResponsiblePhysicianId_text")) ? "" : Convert.ToString(reader["ResponsiblePhysicianId_text"]);
                    model.GeneralComments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? "" : Convert.ToString(reader["Comments"]);
                  

                  
                    model.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? "" : Convert.ToString(reader["CreatedBy"]);
                    model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? "" : Convert.ToString(reader["ModifiedBy"]);
                    model.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    columnsModel = new ChangedColumnsNative();
                    columnsModel.columnName = reader.IsDBNull(reader.GetOrdinal("columnName")) ? "" : Convert.ToString(reader["columnName"]);
                    columnsModel.CurrentValueDisplay = reader.IsDBNull(reader.GetOrdinal("CurrentValueDisplay")) ? "" : Convert.ToString(reader["CurrentValueDisplay"]);
                    model.lstChangedColumns.Add(columnsModel);

                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::FillBirthHx_General", PROC_BirthHx_General_SelectNative, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public BirthHxMaternalDeliveryModelNative FillBirthHx_Maternal(long PatientId, string RequestStatus)
        {

            SqlDataReader reader = null;
            BirthHxMaternalDeliveryModelNative model = null;
            ChangedColumnsNative columnsModel = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_BirthHx_Maternal_SelectNative);
                while (reader.Read())
                {

                    model = new BirthHxMaternalDeliveryModelNative();
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.MaternalDeliveryId = reader.IsDBNull(reader.GetOrdinal("MaternalDeliveryId")) ? "" : Convert.ToString(reader["MaternalDeliveryId"]);
                    model.BirthHxId = reader.IsDBNull(reader.GetOrdinal("BirthHxId")) ? "" : Convert.ToString(reader["BirthHxId"]);
                    model.Gestation = reader.IsDBNull(reader.GetOrdinal("Gestation")) ? "" : Convert.ToString(reader["Gestation"]);
                    model.NumberOfFetuses = reader.IsDBNull(reader.GetOrdinal("NumberOfFetuses")) ? "" : Convert.ToString(reader["NumberOfFetuses"]);

                    model.NumberOfLivingFetuses = reader.IsDBNull(reader.GetOrdinal("NumberOfLivingFetuses")) ? "" : Convert.ToString(reader["NumberOfLivingFetuses"]);
                    model.LaborLength = reader.IsDBNull(reader.GetOrdinal("LaborLength")) ? "" : Convert.ToString(reader["LaborLength"]);
                    model.DeliveryMethodId = reader.IsDBNull(reader.GetOrdinal("DeliveryMethodId")) ? "" : Convert.ToString(reader["DeliveryMethodId"]);
                    model.DeliveryPresentationId = reader.IsDBNull(reader.GetOrdinal("DeliveryPresentationId")) ? "" : Convert.ToString(reader["DeliveryPresentationId"]);
                    model.MaternalHistoryId = reader.IsDBNull(reader.GetOrdinal("MaternalHistoryId")) ? "" : Convert.ToString(reader["MaternalHistoryId"]);
                    model.MaternalHistoryId_text = reader.IsDBNull(reader.GetOrdinal("MaternalHistoryId_text")) ? "" : Convert.ToString(reader["MaternalHistoryId_text"]);


                    model.DeliveryPresentationId_text = reader.IsDBNull(reader.GetOrdinal("DeliveryPresentationId_text")) ? "" : Convert.ToString(reader["DeliveryPresentationId_text"]);
                    model.DeliveryMethodId_text = reader.IsDBNull(reader.GetOrdinal("DeliveryMethodId_text")) ? "" : Convert.ToString(reader["DeliveryMethodId_text"]);
                    model.MaternalHistoryId = reader.IsDBNull(reader.GetOrdinal("MaternalHistoryId")) ? "" : Convert.ToString(reader["MaternalHistoryId"]);
                    model.MaternalDeliveryComments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? "" : Convert.ToString(reader["Comments"]);


                    model.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? "" : Convert.ToString(reader["CreatedBy"]);
                    model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? "" : Convert.ToString(reader["ModifiedBy"]);
                    model.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    columnsModel = new ChangedColumnsNative();
                    columnsModel.columnName = reader.IsDBNull(reader.GetOrdinal("columnName")) ? "" : Convert.ToString(reader["columnName"]);
                    columnsModel.CurrentValueDisplay = reader.IsDBNull(reader.GetOrdinal("CurrentValueDisplay")) ? "" : Convert.ToString(reader["CurrentValueDisplay"]);
                    model.lstChangedColumns.Add(columnsModel);

                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::FillBirthHx_Maternal", PROC_BirthHx_Maternal_SelectNative, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        public DataTable LoadFamilyHx_Disease(long PatientId, string RequestStatus, long familyMemberId, long familyHxDiseaseId=0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string ProcName = PROC_FAMILYHX_DISEASE_NATIV_ESELECT;
            try
            {

                DataTable dtFamilyMemberDiseases = new DataTable();
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_REQUEST_STATUS,RequestStatus);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_FAMILY_MEMBER_ID,familyMemberId);

                if(familyHxDiseaseId==0)
                    dbManager.AddParameters(3, "DiseaseId", null);
                else
                    dbManager.AddParameters(3, "DiseaseId", familyHxDiseaseId);

                using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcName, dbManager.Parameters))
                {
                    dtFamilyMemberDiseases.Load(reader);
                }
                return dtFamilyMemberDiseases;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_FAMILYHX_NATIVESELECT", ProcName, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DataTable LoadFamilyHx_Members(long PatientId, string RequestStatus)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string ProcName = PROC_FAMILYHX_NATIVESELECT;
            try
            {

                DataTable dtFamilyMember= new DataTable();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REQUEST_STATUS, RequestStatus);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcName, dbManager.Parameters))
                {
                    dtFamilyMember.Load(reader);
                }
                return dtFamilyMember;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::PROC_FAMILYHX_NATIVESELECT", ProcName, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public EmptySlotModel GetNearestEmptySlot( string providerId,string facilityId)
        {

            SqlDataReader reader = null;
            EmptySlotModel model = null;
            #region facilityIds Table

            DataTable dtFacilityids = new DataTable();
            DataColumn COLUMN = new DataColumn();
            COLUMN.ColumnName = "Id";
            COLUMN.DataType = typeof(long);
            dtFacilityids.Columns.Add(COLUMN);
            if (!string.IsNullOrWhiteSpace(facilityId))
            {
                string[] strArry = facilityId.Split(',');
                for (int i = 0; i < strArry.Length; i++)
                {
                    DataRow Dr = dtFacilityids.NewRow();
                    Dr[0] = strArry[i];
                    dtFacilityids.Rows.Add(Dr);
                }
            }
            else
            {
                DataRow Dr = dtFacilityids.NewRow();
                Dr[0] = 0;
                dtFacilityids.Rows.Add(Dr);
            }

            #endregion  ProviderIds Table



            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);

                string cDate = DateTime.Now.ToString();
                    dbManager.AddParameters(0, "@DateFrom", cDate);
                    dbManager.AddParameters(1, "@DateTo", cDate);
                    dbManager.AddParameters(2, "@providerId", providerId);
                    dbManager.AddParameters(3, "@Facilityids", dtFacilityids);
                    dbManager.AddParameters(4, "@FORAll", 0);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GetLatest_Slot_ForAppointment);
                while (reader.Read())
                {
                                 
                model = new EmptySlotModel();
                    model.AppointmentDate = reader.IsDBNull(reader.GetOrdinal("AppointmentDate")) ? "" : Convert.ToString(reader["AppointmentDate"]);
                    model.TimeFrom = reader.IsDBNull(reader.GetOrdinal("TimeFrom")) ? "" : Convert.ToString(reader["TimeFrom"]);
                    model.TimeTo = reader.IsDBNull(reader.GetOrdinal("TimeTo")) ? "" : Convert.ToString(reader["TimeTo"]);
                    model.GapMinutes = reader.IsDBNull(reader.GetOrdinal("GapMinutes")) ? "" : Convert.ToString(reader["GapMinutes"]);
                    model.NoOfAppointments = reader.IsDBNull(reader.GetOrdinal("NoOfAppointments")) ? "" : Convert.ToString(reader["NoOfAppointments"]);
                    model.Duration = reader.IsDBNull(reader.GetOrdinal("Duration")) ? "" : Convert.ToString(reader["Duration"]);
                    model.ExpectedDuration = reader.IsDBNull(reader.GetOrdinal("ExpectedDuration")) ? "" : Convert.ToString(reader["ExpectedDuration"]);
                  
                }
               

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMobileApp::GetNearestEmptySlot", PROC_GetLatest_Slot_ForAppointment, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


    }
}
        
       

