using System;
using System.Web.Script.Serialization;
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


namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatient
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatient()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALPatient(bool isNative)
        {


        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALPatient(SharedVariable SharedVariable)
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
        private const string PROC_ETHNICITY_LOOKUP = "Patient.sp_EthnicityLookup";
        private const string PROC_RACE_LOOKUP = "Patient.sp_RaceLookup";
        private const string PROC_RACE_LOOKUP_BY_DESCRIPTION = "Patient.sp_RaceLookupByDescription";
        private const string PROC_COMMUNICATION_LOOKUP = "Patient.sp_CommunicationLookup";
        private const string PROC_PATIENT_RACE_LOOKUP = "Patient.sp_PatientRaceLookup";
        private const string PROC_PATIENT_ETHNICITY_LOOKUP = "Patient.sp_PatientEthnicityLookup";
        private const string PROC_PATIENT_GENDER_IDENTITY_LOOKUP = "Patient.sp_GenderIdentityLookup";
        private const string PROC_PATIENT_SEXUAL_ORIENTATION_LOOKUP = "Patient.sp_SexualOrientationLookup";
        private const string PROC_SCHOOL_LOOKUP = "Patient.sp_SchoolLookup";
        private const string PROC_SCHOOL_STATUS_LOOKUP = "Patient.sp_SchoolStatusLookup";
        private const string PROC_LANGUAGES_LOOKUP = "Patient.sp_LanguagesLookup";
        private const string PROC_GUARANTOR_LOOKUP = "Patient.sp_GuarantorLookup";
        private const string PROC_MARITAL_STATUS_LOOKUP = "Patient.sp_MaritalStatusLookup";
        private const string PROC_SUFFIX_LOOKUP = "Patient.sp_SuffixLookup";
        private const string PROC_PREFIX_LOOKUP = "Patient.sp_PrefixLookup";
        private const string PROC_SMOKERS_STATUS_LOOKUP = "Patient.sp_SmokersStatusLookup";
        private const string PROC_PATIENT_DELETE = "Patient.sp_PatientsDelete";
        private const string PROC_PATIENT_INSERT = "Patient.sp_PatientsInsert";
        private const string PROC_PATIENT_INSERT_CCDA = "Patient.sp_PatientsInsert_CCDA";
        private const string PROC_PATIENT_SELECT = "Patient.sp_PatientsSelect";
        private const string PROC_PATIENT_SEARCH = "Patient.sp_PatientsSearch";
        private const string PROC_USER_ISDATAPRIVACY_SELECT = "Patient.Sp_GetUserIsDataPrivacy";
        private const string PROC_PATIENT_UPDATE = "Patient.sp_PatientsUpdate";
        private const string PROC_PATIENT_UPDATE_PIC = "Patient.sp_PatientsImgUpdate";
        private const string PROC_PATIENT_Delete_PIC_Native = "Patient.sp_PatientsUpdateImage_Native";
        private const string PROC_PATIENT_FILL = "Patient.sp_PatientFill";
        private const string PROC_PATIENT_DETAILS = "Patient.sp_PatientDemographicsDetails";
        private const string PROC_PATIENT_FILL_Native = "Patient.sp_PatientFillNative";
        private const string PROC_PATIENT_LOOKUP = "Patient.sp_PatientsLookup";
        private const string PROC_PATIENT_LOOKUP_BY_NAME = "Patient.sp_PatientsLookupByName";
        private const string PROC_PATIENT_LOOKUP_BY_NAME_FOR_SUPERADMIN = "[Patient].[sp_PatientsLookupForSuperAdmin]";
        private const string PROC_PATIENT_RCOPIAID = "Patient.InsertPatientsRcopialID";
        private const string PROC_RECENT_PATIENT = "System.sp_RecentAccessedPatientsInsert";
        private const string PROC_PATIENT_PREF_UPDATE = "Patient.sp_PatientPreferencesUpdate";
        private const string PROC_PATIENT_PREF_UPDATE_Native = "Patient.sp_PatientPreferencesUpdateNative";
        private const string PROC_REFERRAL_STATUS_LOOKUP = "Patient.sp_ReferralStatusLookup";
        private const string PROC_PATIENT_RACE_CODES = "Patient.sp_GetRaceCodes";
        private const string PROC_PATIENT_ETHNICITY_CODES = "Patient.sp_GetEthnicityCodes";
        private const string PROC_REFERRAL_STATUS_REASON_LOOKUP = "Patient.Sp_ReferralStatusReasonsLookup";
        private const string PROC_RELATIONS_LOOKUP = "Patient.sp_RelationShipLookup";
        private const string PROC_PATIENT_UPDATE_NATIVE = "Patient.sp_PatientUpdateNative";
        private const string PROC_INSERT_UPDATE_DBAUDIT_NATIVE = "system.sp_InsertUpdateDBAuditNative";
        private const string PROC_INSERT_DBAUDIT_DETAIL_NATIVE = "system.sp_InsertDBAuditDetailNative";
        private const string PROC_PATIENT_CONSENT_INSERT = "Patient.sp_PatientConsentInsert";
        private const string PROC_PATIENT_CONSENT_UPDATE = "Patient.sp_PatientConsentUpdate";
        private const string PROC_PATIENT_CONSENT_DELETE = "Patient.sp_PatientConsentDelete";
        private const string PROC_PATIENT_CONSENT_SELECT = "Patient.sp_PatientConsentSelect";
        private const string PROC_DOCUMENT_SOURCE_LOOKUP = "Patient.sp_DocumentSourceLookup";
        private const string PROC_PATIENT_SELECT_CCDA = "Patient.sp_PatientSelect_CCDA";
        private const string PROC_COUNTRIES_LOOKUP = "Patient.sp_CountriesLookup";
        private const string PROC_PREFERRED_ADDRESS_LOOKUP = "Patient.sp_PreferredAddressLookup";
        private const string PROC_PREFERRED_PHONE_LOOKUP = "Patient.sp_PreferredPhoneLookup";

        private const string PROC_BILLING_INQUIRY_EMAIL = "Patient.sp_BillingInquiryEmailInsert";
        private const string PROC_CITY_LOOKUP = "System.CitiesLookup";
        private const string PROC_PATIENT_CAREGIVER_LOOKUP = "Patient.sp_EmergencyContact";


        #region NATIVE SPs

        private const string PROC_PATIENT_LOOKUP_NATIVE = "Patient.sp_PatientsLookupNative";
        private const string PROC_PATIENT_MOST_VIEWED_LOOKUP_NATIVE = "Patient.sp_MostViewedPatients";

        #endregion

        #endregion

        #region Parameters
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_DIMMY_PATIENT_ID = "@DimmyPatientId";
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
        private const string PARM_REQUEST_STATUS = "@RequestStatus";
        private const string PARM_ZIPCODE = "@ZIPCode";
        private const string PARM_ZIPCODE_EXT = "@ZIPCodeExt";
        private const string PARM_HOME_PHONE_NO = "@HomePhoneNo";
        private const string PARM_WORK_PHONE_NO = "@WorkPhoneNo";
        private const string PARM_WORK_PHONE_EXT = "@WorkPhoneExt";
        private const string PARM_IS_TCM = "@IsTCM";
        private const string PARM_DODISCHARGE = "@DischargeDate";

        private const string PARM_CELL_NO = "@CellNo";
        private const string PARM_FAX_NO = "@FaxNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_LANGUAGE_NAME = "@LanguageName";
        private const string PARM_COUNTRY_NAME = "@CountryName";
        private const string PARM_Dummy_PatientId = "@DummyPatientId";
        
        private const string PARM_PATIENT_STR_ETHNICITY_IDS = "@StrEthnicityIds";
        private const string PARM_GENDER_IDENTITY_ID = "@GenderIdentityId";
        private const string PARM_SEXUAL_ORIENTATION_ID = "@SexualOrientationId";
        private const string PARM_CONFIDENTIALITY_CODE = "@ConfidentialityCode";
        private const string PARM_MOTHER_MAIDEN_NAME = "@MotherMaidenName";
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
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_DESCRIPTION = "@Description";
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
        private const string PARM_APPOINTMENT_DATE = "@AppointmentDate";

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
        private const string PARM_PROFILE_IMAGE_PATH = "@PatientProfileImagePath";
        private const string PARM_PROFILE_THUMBNAILPath = "@PatientProfileThumbnailPath";
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

        private const string PARM_BIRTH_SEX = "@BirthSex";
        private const string PARM_PATIENT_PROFILE_IMAGE_PATH = "@PatientProfileImagePath";
        private const string PARM_PATIENT_PROFILE_THUMBNAIL_PATH = "@PatientProfileThumbnailPath";

        private const string PARM_PREFERRED_ADDRESS_ID = "@PreferredAddressId";
        private const string PARM_COUNTRY_ID = "@CountryId";
        private const string PARM_PREFERRED_PHONE_ID = "@PreferredPhoneId";
        private const string PARM_FOR_IMPORT_CQM = "@ForImportCQM";

        private const string PARM_REF_LAST_NAME = "@RefLastName";
        private const string PARM_REF_FIRST_NAME = "@RefFirstName";

        private const string PARM_PATIENT_BALANCE = "@PatientBalance";
        private const string PARM_EMAIL_FROM = "@EmailFrom";
        private const string PARM_EMAIL_TO = "@EmailTo";
        private const string PARM_INCOMPLETE_DEMOGRAPHICS = "@IncompleteDemographics";
        private const string PARM_CITY_Name = "@cityname";
        private const string PARM_CCDA_AVAILABLE = "@IsCCDAAvailable";
        private const string PARM_CAREGIVER_IDS = "@CareGiverIds";
        private const string PARM_STATUS_ID = "@StatusId";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_CCDA(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(76);

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
            dbManager.AddParameters(44, PARM_IS_ACTIVE, ds.Patients.IsActiveColumn.ColumnName, DbType.Int32);
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
            dbManager.AddParameters(56, PARM_HEARFROMID, ds.Patients.HearFromIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(57, PARM_HEARFROM_OTHER, ds.Patients.HearFromOtherColumn.ColumnName, DbType.String);
            dbManager.AddParameters(58, PARM_RCOPIA_ID, ds.Patients.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(59, PARM_ACCOUNT_NOTE_COMMENTS, ds.Patients.AccountNoteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(60, PARM_PATIENT_IMAGE_THUMBNAIL, ds.Patients.PatientImageThumbnailColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(61, PARM_IS_TCM, ds.Patients.IsTCMColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(62, PARM_DODISCHARGE, ds.Patients.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(63, PARM_PATIENT_STR_ETHNICITY_IDS, ds.Patients.strEthnicityIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(64, PARM_GENDER_IDENTITY_ID, ds.Patients.GenderIdentityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(65, PARM_SEXUAL_ORIENTATION_ID, ds.Patients.SexualOrientationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(66, PARM_CONFIDENTIALITY_CODE, ds.Patients.ConfidentialityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(67, PARM_MOTHER_MAIDEN_NAME, ds.Patients.MotherMaidenNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(68, PARM_BIRTH_SEX, ds.Patients.BirthSexColumn.ColumnName, DbType.String);

            dbManager.AddParameters(69, PARM_PATIENT_PROFILE_IMAGE_PATH, ds.Patients.PatientProfileImagePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(70, PARM_PATIENT_PROFILE_THUMBNAIL_PATH, ds.Patients.PatientProfileThumbnailPathColumn.ColumnName, DbType.String);

            dbManager.AddParameters(71, PARM_COUNTRY_ID, ds.Patients.CountryIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(72, PARM_PREFERRED_ADDRESS_ID, ds.Patients.PreferredAddressIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(73, PARM_PREFERRED_PHONE_ID, ds.Patients.PreferredPhoneIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(74, PARM_REF_LAST_NAME, ds.Patients.RefLastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(75, PARM_REF_FIRST_NAME, ds.Patients.RefFirstNameColumn.ColumnName, DbType.String);
        }

        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(75);

            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.PatientIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(74, PARM_CCDA_AVAILABLE, ds.Patients.IsCCDAAvailableColumn.ColumnName, DbType.String);
            }
            else
            {                
                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(74, PARM_CAREGIVER_IDS, ds.Patients.CareGiverIdsColumn.ColumnName, DbType.String);
            }
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
            dbManager.AddParameters(44, PARM_IS_ACTIVE, ds.Patients.IsActiveColumn.ColumnName, DbType.Int32);
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
            dbManager.AddParameters(56, PARM_HEARFROMID, ds.Patients.HearFromIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(57, PARM_HEARFROM_OTHER, ds.Patients.HearFromOtherColumn.ColumnName, DbType.String);
            dbManager.AddParameters(58, PARM_RCOPIA_ID, ds.Patients.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(59, PARM_ACCOUNT_NOTE_COMMENTS, ds.Patients.AccountNoteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(60, PARM_PATIENT_IMAGE_THUMBNAIL, ds.Patients.PatientImageThumbnailColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(61, PARM_IS_TCM, ds.Patients.IsTCMColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(62, PARM_DODISCHARGE, ds.Patients.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(63, PARM_PATIENT_STR_ETHNICITY_IDS, ds.Patients.strEthnicityIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(64, PARM_GENDER_IDENTITY_ID, ds.Patients.GenderIdentityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(65, PARM_SEXUAL_ORIENTATION_ID, ds.Patients.SexualOrientationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(66, PARM_CONFIDENTIALITY_CODE, ds.Patients.ConfidentialityCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(67, PARM_MOTHER_MAIDEN_NAME, ds.Patients.MotherMaidenNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(68, PARM_BIRTH_SEX, ds.Patients.BirthSexColumn.ColumnName, DbType.String);

            dbManager.AddParameters(69, PARM_PATIENT_PROFILE_IMAGE_PATH, ds.Patients.PatientProfileImagePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(70, PARM_PATIENT_PROFILE_THUMBNAIL_PATH, ds.Patients.PatientProfileThumbnailPathColumn.ColumnName, DbType.String);

            dbManager.AddParameters(71, PARM_COUNTRY_ID, ds.Patients.CountryIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(72, PARM_PREFERRED_ADDRESS_ID, ds.Patients.PreferredAddressIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(73, PARM_PREFERRED_PHONE_ID, ds.Patients.PreferredPhoneIDColumn.ColumnName, DbType.Int64);
        }

        private void CreateSelectParameters(IDBManager dbManager, DSPatient ds)
        {
            if (ds.Patients.Rows.Count > 0)
            {
                // Begin 05-Jan-2015  Edited By Azeem Raza Tayyab Bug # PMS-3136( add new parameter-CoverageType)
                dbManager.CreateParameters(24);
                // End 05-Jan-2015  Edit By Azeem Raza Tayyab Bug # PMS-3136

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

                // dbManager.AddParameters(6, PARM_ENTITY_ID, ds.Patients.Rows[0][ds.Patients.EntityIdColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.EntityIdColumn.ColumnName]);
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
                // Begin 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                dbManager.AddParameters(21, PARM_COVERAGE_TYPE, ds.Patients.Rows[0][ds.Patients.CoverageTypeColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.CoverageTypeColumn.ColumnName]);
                // End 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                dbManager.AddParameters(22, PARM_File_Stream, ds.Patients.Rows[0][ds.Patients.IsFileStreamColumn.ColumnName].ToString() == "" ? false : ds.Patients.Rows[0][ds.Patients.IsFileStreamColumn.ColumnName]);
                dbManager.AddParameters(23, PARM_FOR_IMPORT_CQM, ds.Patients.Rows[0][ds.Patients.ForImportCqmColumn.ColumnName].ToString() == "" ? false : ds.Patients.Rows[0][ds.Patients.ForImportCqmColumn.ColumnName]);
                

            }

        }
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
        private void CreateSearchParameters(IDBManager dbManager, DSPatient ds)
        {
            if (ds.Patients.Rows.Count > 0)
            {
                // Begin 05-Jan-2015  Edited By Azeem Raza Tayyab Bug # PMS-3136( add new parameter-CoverageType)
                dbManager.CreateParameters(28);
                // End 05-Jan-2015  Edit By Azeem Raza Tayyab Bug # PMS-3136

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

                // dbManager.AddParameters(6, PARM_ENTITY_ID, ds.Patients.Rows[0][ds.Patients.EntityIdColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.EntityIdColumn.ColumnName]);
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
                // Begin 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                dbManager.AddParameters(21, PARM_COVERAGE_TYPE, ds.Patients.Rows[0][ds.Patients.CoverageTypeColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.CoverageTypeColumn.ColumnName]);
                // End 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                dbManager.AddParameters(22, PARM_CLAIM_NO, ds.Patients.Rows[0][ds.Patients.ClaimNoColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.ClaimNoColumn.ColumnName]);
                dbManager.AddParameters(23, PARM_INSURANCEPLAN_ID, ds.Patients.Rows[0][ds.Patients.InsurancePlanIdColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.InsurancePlanIdColumn.ColumnName]);
                dbManager.AddParameters(24, PARM_IS_RECENT_PATIENT, ds.Patients.Rows[0][ds.Patients.IsRecentPatientColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.IsRecentPatientColumn.ColumnName]);
                dbManager.AddParameters(25, PARM_APPOINTMENT_DATE, ds.Patients.Rows[0][ds.Patients.AppointmentDateColumn.ColumnName].ToString() == "" ? null : ds.Patients.Rows[0][ds.Patients.AppointmentDateColumn.ColumnName]);
                dbManager.AddParameters(26, PARM_INCOMPLETE_DEMOGRAPHICS, ds.Patients.Rows[0][ds.Patients.IncompleteDemographicsColumn.ColumnName]);
                dbManager.AddParameters(27, PARM_GUARANTOR_ID, ds.Patients.Rows[0][ds.Patients.GuarantorIdColumn.ColumnName]);
            }

        }
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
        private void CreateParametersForAddRcopiaID(IDBManager dbManager, DSPatient ds)
        {
            if (ds.Patients.Rows.Count > 0)
            {
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.Rows[0][ds.Patients.PatientIdColumn.ColumnName]);
                dbManager.AddParameters(1, PARM_RCOPIAID, ds.Patients.Rows[0][ds.Patients.RcopiaIDColumn.ColumnName].ToString());
            }
        }

        private void CreateParametersForAddRecentPatients(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ACCESSED_PATIENTID, ds.RecentAccessedPatients.AccessedPatientIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ACCESSED_PATIENTID, ds.RecentAccessedPatients.AccessedPatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.RecentAccessedPatients.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.RecentAccessedPatients.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ACCESSED_ON, ds.RecentAccessedPatients.AccessedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="AccountNumber">The account number.</param>
        /// <param name="SSN">The SSN.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
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

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="PatientId"></param>
        /// <param name="AccountNo"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DSPatient FillPatient(SharedVariable SharedVariable, long PatientId, string AccountNo, string IsActive)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {




                if (AccountNo == "")
                    AccountNo = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();

                dbManager.CreateParameters(5);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, SharedVariable.EntityId);

                dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, AccountNo);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(4, PARM_USER_ID, SharedVariable.AppUserId);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_FILL, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALPatient::FillPatient", PROC_PATIENT_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient FillPatient(long PatientId, string AccountNo, string IsActive, SharedVariable sharedVariable = null)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            long AppUserId = 0;
            string username = string.Empty;
            string EntityId = string.Empty;
            if (sharedVariable != null)
            {
                AppUserId = sharedVariable.AppUserId;
                username = sharedVariable.UserName;
                EntityId = sharedVariable.EntityId;
            }
            else
            {
                AppUserId = MDVSession.Current.AppUserId;
                username = MDVSession.Current.AppUserName;
                EntityId = MDVSession.Current.EntityId;
            }
            try
            {

                if (AccountNo == "")
                    AccountNo = null;

                if (IsActive == "")
                    IsActive = null;



                dbManager.Open();
                dbManager.CreateParameters(5);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (ClientConfiguration.DecryptFrom64(username).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, AccountNo);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(4, PARM_USER_ID, AppUserId);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_FILL, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALPatient::FillPatient", PROC_PATIENT_FILL, ex);
                MDVLogger.SendExcepToDB(ex, "DALPatient::FillPatient", PROC_PATIENT_FILL);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the patient.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatient UpdatePatient(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            //   DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                DataTable dtTemp = ds.Patients.GetChanges().Copy();

                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_UPDATE, ds, ds.Patients.TableName);

                if (dtTemp != null)
                {
                    var idColumn = ds.Patients.Rows[0][ds.Patients.PatientIdColumn].ToString();

                    new DBActivityAudit().InsertDBAuditAsync(dtTemp, idColumn);
                    //  dsDBAudit.AcceptChanges();
                }

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Update Patient", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                //  Trace.WriteLine("Patient Update done at:  " + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond);
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //  dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::UpdatePatient", PROC_PATIENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient UpdatePatientPic(DSPatient ds, string URL)
        {
            DALUsersActivity obj = new DALUsersActivity();
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //DataTable dtTemp = ds.Patients.GetChanges();

                dbManager.BeginTransaction();
                dbManager.CreateParameters(8);

                dbManager.AddParameters(0, PARM_ENTITY_ID, ds.Patients.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_PATIENT_ID, ds.Patients.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ds.Patients.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_MODIFIED_ON, ds.Patients.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(4, PARM_PATIENT_IMAGE_TYPE, ds.Patients.ImageTypeColumn.ColumnName, DbType.String);
                //dbManager.AddParameters(5, PARM_PATIENT_IMAGE_THUMBNAIL, ds.Patients.PatientImageThumbnailColumn.ColumnName, DbType.Byte);
                //dbManager.AddParameters(6, PARM_PATIENT_IMAGE, ds.Patients.PatientImageColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(5, PARM_PATIENT_PROFILE_IMAGE_PATH, ds.Patients.PatientProfileImagePathColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_PATIENT_PROFILE_THUMBNAIL_PATH, ds.Patients.PatientProfileThumbnailPathColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, PARM_URL, URL);

                //DataTable dtTemp = ds.Patients.GetChanges().Copy();

                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_UPDATE_PIC, ds, ds.Patients.TableName);

                //this.CreateParameters(dbManager, ds, false);
                //if (dtTemp != null)
                //{
                //    var idColumn = ds.Patients.Rows[0][ds.Patients.PatientIdColumn].ToString();
                //    new DBActivityAudit().InsertDBAuditAsync(dtTemp, idColumn);
                //}

                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Update Patient Pic", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::UpdatePatientPic", PROC_PATIENT_UPDATE_PIC, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeletePatientPicNative(string ModifiedBy, DateTime ModifiedOn, Int64 PatientId)
        {
            string returnVal = "";
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //DataTable dtTemp = ds.Patients.GetChanges();

                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);


                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(2, PARM_MODIFIED_ON, ModifiedOn);



                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_Delete_PIC_Native).ToString();


                if (returnVal != "-1")
                {
                    dbManager.RollBackTransaction();
                    throw new Exception(returnVal);

                }
                else
                {
                    dbManager.CommitTransaction();

                    return "";
                }
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Update Patient Pic", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());



            }
            catch (Exception ex)
            {


                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::DeletePatientPicNative", PROC_PATIENT_Delete_PIC_Native, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePatient(string PatientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_SELECT).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::DeletePatient", PROC_PATIENT_SELECT, ex);
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
                MDVLogger.DALErrorLog("DALPatient::PROC_USER_ISDATAPRIVACY_SELECT", PROC_USER_ISDATAPRIVACY_SELECT, ex);
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

        public void InsertBillingInquiryEmail(BillingInquiryEmailModel model, string MailFrom, string MailTo)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (model.hfPatientId == "")
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, model.hfPatientId);
                if (model.hfPatientFacilityId == "")
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, model.hfPatientFacilityId);
                if (model.hfPatientProviderId == "")
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, model.hfPatientProviderId);

                if (model.hfPatientBalance == "")
                    dbManager.AddParameters(3, PARM_PATIENT_BALANCE, null);
                else
                    dbManager.AddParameters(3, PARM_PATIENT_BALANCE, MDVUtility.ToStr(model.hfPatientBalance).Replace("$", ""));

                if (MailFrom == "")
                    dbManager.AddParameters(4, PARM_EMAIL_FROM, null);
                else
                    dbManager.AddParameters(4, PARM_EMAIL_FROM, MailFrom);
                if (MailTo == "")
                    dbManager.AddParameters(5, PARM_EMAIL_TO, null);
                else
                    dbManager.AddParameters(5, PARM_EMAIL_TO, MailTo);

                    dbManager.AddParameters(6, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                    dbManager.AddParameters(7, PARM_CREATED_ON, DateTime.Now);
                


                var xyz = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_INQUIRY_EMAIL);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertBillingInquiryEmail", PROC_BILLING_INQUIRY_EMAIL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientReminderLog> getReminderLogData(long PatientId)
        {
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<PatientReminderLog> list = new List<PatientReminderLog>();
               

                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientId", PatientId));
             
                var reader = dbManager.ExecuteReader("[Patient].[sp_ReminderAuditLog]", parameters);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var model = new Model.Patient.PatientReminderLog();
                        model.AppointmentDate = reader["AppointmentDate"].ToString();
                        model.AppointmentStatus = reader["AppointmentStatus"].ToString();
                        model.Duration = reader["Duration"].ToString();
                        model.ReminderResponse = reader["ReminderResponse"].ToString();
                        model.ReminderType = reader["ReminderType"].ToString();
                        model.Status = reader["Status"].ToString();
                        model.Time = reader["Time"].ToString();
                        list.Add(model);
                    }
                    
                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLPatient::getReminderLogData", "[Patient].[sp_ReminderAuditLog]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<PatientFaxLog> getFaxLogData(long PatientId)
        {
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<PatientFaxLog> list = new List<PatientFaxLog>();


                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientId", PatientId));

                var reader = dbManager.ExecuteReader("[Patient].[sp_FaxAuditLog]", parameters);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var model = new Model.Patient.PatientFaxLog();
                        model.DateAndTime = reader["DateAndTime"].ToString();
                        model.Pages = reader["Pages"].ToString();
                        model.SenderName = reader["SenderName"].ToString();
                        model.SentStatus = reader["SentStatus"].ToString();
                        model.Subject = reader["Subject"].ToString();
                        model.ToFaxNumber = reader["ToFaxNumber"].ToString();
                        model.RecipientName = reader["RecipientName"].ToString();
                        list.Add(model);
                    }

                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLPatient::getFaxLogData", "[Patient].[sp_FaxAuditLog]", ex);
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

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Patients.Rows[0][ds.Patients.PatientIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Insert Patient", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
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

        public DSPatient InsertPatient_CCDA(DSPatient ds)
        {

            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Patients.GetChanges();

                // dbManager.Open();
                dbManager.BeginTransaction();

                CreateParameters_CCDA(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSERT_CCDA, ds, ds.Patients.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Patients.Rows[0][ds.Patients.PatientIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Insert Patient", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error while inserting the patient : " + ex.ToString());
                MDVLogger.DALErrorLog("DALPatient::InsertPatient_CCDA", PROC_PATIENT_INSERT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient InsertPatientsRcopialID(DSPatient ds)
        {

            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Patients.GetChanges();

                // dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersForAddRcopiaID(dbManager, ds);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_RCOPIAID, ds, ds.Patients.TableName);

                //      if (dtTemp != null)
                //       {
                //  dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Patients.Rows[0][ds.Patients.PatientIdColumn].ToString());
                //   dsDBAudit.AcceptChanges();
                //      }
                //    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Insert Patient", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error while inserting the patient : " + ex.ToString());
                MDVLogger.DALErrorLog("DALPatient::InsertPatientsRcopialID", PROC_PATIENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient PatientsSearch(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                this.CreateSearchParameters(dbManager, ds);
                // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                DSPatient dsPatient = new DSPatient();
                dsPatient = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_SEARCH, dsPatient, ds.Patients.TableName);

                return dsPatient;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PatientsSearch", PROC_PATIENT_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient InsertRecentPatient(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                CreateParametersForAddRecentPatients(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_RECENT_PATIENT, ds, ds.RecentAccessedPatients.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::InsertRecentPatient", PROC_RECENT_PATIENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient GetRaceCodes(long PatientId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_RACE_CODES, ds, ds.PatientRace.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::GetRaceCodes", PROC_PATIENT_RACE_CODES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient GetEthnicityCodes(long PatientId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_ETHNICITY_CODES, ds, ds.PatientEthnicity.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::GetEthnicityCodes", PROC_PATIENT_ETHNICITY_CODES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient FillPatientPMS(long PatientId, string FormName)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FormName == "")
                    FormName = null;

                dbManager.Open();

                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_MODULE_NAME, FormName);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);
                var tableNames = new List<string>
                {
                    ds.Patients.TableName,
                };
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_FILL, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::FillPatient", PROC_PATIENT_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public PatientDemographicModelNative FillPatientNative(long? PatientId, string DimmyPatientId, string RequestStatus)
        {
            PatientDemographicModelNative model = null;
            ChangedColumnsNative columnsModel = null;



            SqlDataReader reader = null;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();

                dbManager.CreateParameters(3);


                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_DIMMY_PATIENT_ID, DimmyPatientId);
                dbManager.AddParameters(2, PARM_REQUEST_STATUS, RequestStatus);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_FILL_Native);
                //   ds = (DSPatient)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_FILL_Native, ds, ds.Patients.TableName);

                while (reader.Read())
                {
                    model = new PatientDemographicModelNative();
                    model.Address1 = reader.IsDBNull(reader.GetOrdinal("Address1")) ? "" : Convert.ToString(reader["Address1"]);
                    model.Address2 = reader.IsDBNull(reader.GetOrdinal("Address2")) ? "" : Convert.ToString(reader["Address2"]);
                    model.CellNo = reader.IsDBNull(reader.GetOrdinal("CellNo")) ? "" : Convert.ToString(reader["CellNo"]);
                    model.City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : Convert.ToString(reader["City"]);

                    model.DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? null : Convert.ToDateTime(reader["DOB"]).ToShortDateString();

                    model.EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : Convert.ToString(reader["EmailAddress"]);


                    model.EthnicityId = reader.IsDBNull(reader.GetOrdinal("EthnicityId")) ? null : Convert.ToString(reader["EthnicityId"]);
                    model.FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : Convert.ToString(reader["FirstName"]);
                    model.Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : Convert.ToString(reader["Gender"]);
                    model.HomePhoneNo = reader.IsDBNull(reader.GetOrdinal("HomePhoneNo")) ? null : Convert.ToString(reader["HomePhoneNo"]);
                    model.ImageType = reader.IsDBNull(reader.GetOrdinal("ImageType")) ? null : Convert.ToString(reader["ImageType"]);
                    model.PatientProfileImagePath = reader.IsDBNull(reader.GetOrdinal("PatientProfileImagePath")) ? null : Convert.ToString(reader["PatientProfileImagePath"]);
                    model.PatientProfileThumbnailPath = reader.IsDBNull(reader.GetOrdinal("PatientProfileThumbnailPath")) ? null : Convert.ToString(reader["PatientProfileThumbnailPath"]);
                    model.LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : Convert.ToString(reader["LastName"]);
                    model.MaritialStatus = reader.IsDBNull(reader.GetOrdinal("MaritialStatus")) ? null : Convert.ToString(reader["MaritialStatus"]);
                    model.MI = reader.IsDBNull(reader.GetOrdinal("MI")) ? null : Convert.ToString(reader["MI"]);
                    model.ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : Convert.ToString(reader["ModifiedBy"]);
                    model.ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? null : Convert.ToString(reader["ModifiedOn"]);
                    model.PrefLanguageId = reader.IsDBNull(reader.GetOrdinal("PrefLanguageId")) ? null : Convert.ToString(reader["PrefLanguageId"]);
                    model.SelfPay = reader.IsDBNull(reader.GetOrdinal("SelfPay")) ? null : Convert.ToString(reader["SelfPay"]);
                    model.SSN = reader.IsDBNull(reader.GetOrdinal("SSN")) ? null : Convert.ToString(reader["SSN"]);
                    model.State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : Convert.ToString(reader["State"]);
                    model.strRaceIds = reader.IsDBNull(reader.GetOrdinal("strRaceIds")) ? null : Convert.ToString(reader["strRaceIds"]);
                    model.ZipCode = reader.IsDBNull(reader.GetOrdinal("ZipCode")) ? null : Convert.ToString(reader["ZipCode"]);
                    model.ZipCodeExt = reader.IsDBNull(reader.GetOrdinal("ZipCodeExt")) ? null : Convert.ToString(reader["ZipCodeExt"]); 
                    model.AccountNo = reader.IsDBNull(reader.GetOrdinal("AccountNumber")) ? null : Convert.ToString(reader["AccountNumber"]);
                    model.RaceId = reader.IsDBNull(reader.GetOrdinal("RaceId")) ? null : Convert.ToString(reader["RaceId"]);
                    model.PrefLanguage = reader.IsDBNull(reader.GetOrdinal("PrefLanguage")) ? null : Convert.ToString(reader["PrefLanguage"]);
                    model.DimmyPatientId = reader.IsDBNull(reader.GetOrdinal("DimmyPatientId")) ? null : Convert.ToString(reader["DimmyPatientId"]);





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
                MDVLogger.DALErrorLog("DALPatient::FillPatientNative", PROC_PATIENT_FILL_Native, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="PatientId"></param>
        /// <param name="FormName"></param>
        /// <returns></returns>
        public DSPatient FillPatientPMS(SharedVariable SharedVariable, long PatientId, string FormName)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (FormName == "")
                    FormName = null;

                dbManager.Open();

                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_MODULE_NAME, FormName);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_FILL, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALPatient::FillPatient", PROC_PATIENT_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the ethnicity.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupEthnicity()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ETHNICITY_LOOKUP, ds, ds.Ethnicity.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupEthnicity", PROC_ETHNICITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<EthnicityModel> LookupEthnicityDemographic()
        {
            List<EthnicityModel> listAllergies = new List<EthnicityModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ETHNICITY_LOOKUP);
                EthnicityModel model = null;
                while (reader.Read())
                {
                    model = new EthnicityModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupEthnicityDemographic", PROC_ETHNICITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the race.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupRace()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RACE_LOOKUP, ds, ds.Race.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupRace", PROC_RACE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatientLookups LookupRaceByDescription(string Description)
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (string.IsNullOrEmpty(Description))
                    dbManager.AddParameters(0, PARM_DESCRIPTION, null);
                else
                    dbManager.AddParameters(0, PARM_DESCRIPTION, Description);

                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RACE_LOOKUP_BY_DESCRIPTION, ds, ds.Race.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupRaceByDescription", PROC_RACE_LOOKUP_BY_DESCRIPTION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<LookUpModel> LoadGenderIdentityLookUp()
        {
            List<LookUpModel> GenderIdentityModelList = new List<LookUpModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                GenderIdentityModelList = dbManager.ExecuteReaders<LookUpModel>(PROC_PATIENT_GENDER_IDENTITY_LOOKUP);
                return GenderIdentityModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadGenderIdentityLookUp", PROC_PATIENT_GENDER_IDENTITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<LookUpModel> LoadSexualOrientationLookUp()
        {
            List<LookUpModel> SexualOrientationModelList = new List<LookUpModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                SexualOrientationModelList = dbManager.ExecuteReaders<LookUpModel>(PROC_PATIENT_SEXUAL_ORIENTATION_LOOKUP);
                return SexualOrientationModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadSexualOrientationLookUp", PROC_PATIENT_SEXUAL_ORIENTATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CustomModel> LookupPatientRaceDemographics()
        {
            List<CustomModel> listRace = new List<CustomModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_RACE_LOOKUP);
                CustomModel model = null;
                while (reader.Read())
                {
                    model = new CustomModel();
                    model.Id = reader["Id"].ToString();
                    model.Code = reader["Code"].ToString();
                    model.Name = reader["Name"].ToString();
                    model.ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? "" : Convert.ToString(reader["ParentId"]);
                    listRace.Add(model);
                }
                return listRace;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatientRaceDemographic", PROC_PATIENT_RACE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CustomModel> LookupPatientEthnicityDemographics()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                return dbManager.ExecuteReaders<CustomModel>(PROC_PATIENT_ETHNICITY_LOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatientEthnicityDemographic", PROC_PATIENT_ETHNICITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<RaceModel> LookupRaceDemographic()
        {
            List<RaceModel> listAllergies = new List<RaceModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RACE_LOOKUP);
                RaceModel model = null;
                while (reader.Read())
                {
                    model = new RaceModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupRaceDemographic", PROC_RACE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public DSPatientLookups GetStatusReasons(long StatusId)
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_STATUS_ID, StatusId);
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_STATUS_REASON_LOOKUP, ds, ds.ReferralStatusReason.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::GetStatusReasons", PROC_REFERRAL_STATUS_REASON_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the race.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupReferralStatus()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_STATUS_LOOKUP, ds, ds.ReferralStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupReferralStatus", PROC_RACE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Lookups the Communication.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupCommunication()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMMUNICATION_LOOKUP, ds, ds.Communication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupCommunication", PROC_COMMUNICATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the school.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupSchool()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHOOL_LOOKUP, ds, ds.School.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupSchool", PROC_SCHOOL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the school status.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupSchoolStatus()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHOOL_STATUS_LOOKUP, ds, ds.SchoolStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupSchoolStatus", PROC_SCHOOL_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the smokers status.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupSmokersStatus()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SMOKERS_STATUS_LOOKUP, ds, ds.SmokersStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupSmokersStatus", PROC_SMOKERS_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the languages.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupLanguages(string LanguageName = "")
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(LanguageName))
                    dbManager.AddParameters(0, PARM_LANGUAGE_NAME, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_LANGUAGE_NAME, LanguageName);
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LANGUAGES_LOOKUP, ds, ds.Languages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupLanguages", PROC_LANGUAGES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientLookups LookupCountries(string CountryName = "")
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(CountryName))
                    dbManager.AddParameters(0, PARM_COUNTRY_NAME, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_COUNTRY_NAME, CountryName);
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COUNTRIES_LOOKUP, ds, ds.Countries.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupCountries", PROC_COUNTRIES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CityModel> LookupCities(string CityName)
        {
            List<CityModel> listCities = new List<CityModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(CityName))
                    dbManager.AddParameters(0, PARM_CITY_Name, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_CITY_Name, CityName);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CITY_LOOKUP);
                CityModel model = null;
                while (reader.Read())
                {
                    model = new CityModel();                   
                    model.Description = reader["City"].ToString();
                    model.Zip = reader["Zip"].ToString();

                    listCities.Add(model);
                }
                return listCities;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupCities", PROC_CITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
        public DSPatientLookups LookupPreferredAddress()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PREFERRED_ADDRESS_LOOKUP, ds, ds.PreferredAddress.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPreferredAddress", PROC_PREFERRED_ADDRESS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientLookups LookupPreferredPhone()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PREFERRED_PHONE_LOOKUP, ds, ds.PreferredPhone.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPreferredPhone", PROC_PREFERRED_PHONE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the guarantor.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupGuarantor()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GUARANTOR_LOOKUP, ds, ds.Guarantor.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupGuarantor", PROC_GUARANTOR_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the marital status.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupMaritalStatus()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MARITAL_STATUS_LOOKUP, ds, ds.MaritialStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupMaritalStatus", PROC_MARITAL_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MaritalStatusModel> LookupMaritalStatusDemographic()
        {
            List<MaritalStatusModel> listAllergies = new List<MaritalStatusModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MARITAL_STATUS_LOOKUP);
                MaritalStatusModel model = null;
                while (reader.Read())
                {
                    model = new MaritalStatusModel();
                    model.Id = reader["Id"].ToString();
                    model.Status = reader["Status"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupMaritalStatusDemographic", PROC_MARITAL_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the suffix.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupSuffix()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUFFIX_LOOKUP, ds, ds.Suffix.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupSuffix", PROC_SUFFIX_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<SuffixModel> LookupSuffixDemographic()
        {
            List<SuffixModel> listAllergies = new List<SuffixModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_SUFFIX_LOOKUP);
                SuffixModel model = null;
                while (reader.Read())
                {
                    model = new SuffixModel();
                    model.Id = reader["Id"].ToString();
                    model.Title = reader["Title"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupSuffixDemographic", PROC_SUFFIX_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the prefix.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupPrefix()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PREFIX_LOOKUP, ds, ds.PreFix.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPrefix", PROC_PREFIX_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PrefixModel> LookupPrefixDemographic()
        {
            List<PrefixModel> listAllergies = new List<PrefixModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PREFIX_LOOKUP);
                PrefixModel model = null;
                while (reader.Read())
                {
                    model = new PrefixModel();
                    model.Id = reader["Id"].ToString();
                    model.Title = reader["Title"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPrefixDemographic", PROC_PREFIX_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
        public DSPatient LookupPatient(long PatientId, string AccountNo, string IsActive)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (AccountNo == "")
                    AccountNo = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();

                dbManager.CreateParameters(5);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, AccountNo);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_LOOKUP, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatient", PROC_PATIENT_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientRepresentativeLookupModel> LookupDocumentSource()
        {
            List<PatientRepresentativeLookupModel> listDocumentSource = new List<PatientRepresentativeLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DOCUMENT_SOURCE_LOOKUP);
                PatientRepresentativeLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientRepresentativeLookupModel();
                    model.Id = reader["DocumentSourceId"].ToString();
                    model.Name = reader["ShortName"].ToString();

                    listDocumentSource.Add(model);
                }

                return listDocumentSource;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupDocumentSource", PROC_DOCUMENT_SOURCE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public DSPatient LookupPatientByName(long PatientId, string Searchstring, string IsActive)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Searchstring == "")
                    Searchstring = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) != "MDVISION")
                {
                    dbManager.CreateParameters(5);

                    if (PatientId == 0)
                        dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                    else
                        dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                    dbManager.AddParameters(2, PARM_PAT_SEARCH_STRING, Searchstring);
                    dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                    dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                    ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_LOOKUP_BY_NAME, ds, ds.Patients.TableName);
                }
                else
                {
                    dbManager.CreateParameters(1);
                    dbManager.AddParameters(0, PARM_PAT_SEARCH_STRING, Searchstring);
                    // "PROC_PATIENT_LOOKUP_BY_NAME_FOR_SUPERADMIN" sp to fetch all entities patients record for super admin(mdvision)
                    ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_LOOKUP_BY_NAME_FOR_SUPERADMIN, ds, ds.Patients.TableName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatientByName", PROC_PATIENT_LOOKUP_BY_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CustomModel> LookupPatientCareGiver(long PatientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                return dbManager.ExecuteReaders<CustomModel>(PROC_PATIENT_CAREGIVER_LOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatientCareGiver", PROC_PATIENT_CAREGIVER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region PatientPreferences

        public DSPatient UpdatePatientPreferences(DSPatient dsPatient)
        {
            // DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = dsPatient.Patients.GetChanges();
                dbManager.Open();
                dbManager.CreateParameters(10);

                dbManager.AddParameters(0, PARM_PATIENT_ID, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);
                dbManager.AddParameters(1, PARM_PREF_COMMUNICATION_ID, MDVUtility.ToInt32(dsPatient.Patients.Rows[0][dsPatient.Patients.PrefCommunicationIdColumn.ColumnName]) == 0 ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.PrefCommunicationIdColumn.ColumnName]);
                dbManager.AddParameters(2, PARM_SCHOOL_STATUS, MDVUtility.ToStr(dsPatient.Patients.Rows[0][dsPatient.Patients.SchoolStatusColumn.ColumnName]) == "" ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.SchoolStatusColumn.ColumnName]);
                dbManager.AddParameters(3, PARM_SCHOOL_ID, MDVUtility.ToStr(dsPatient.Patients.Rows[0][dsPatient.Patients.SchoolIdColumn.ColumnName]) == "" ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.SchoolIdColumn.ColumnName]);
                dbManager.AddParameters(4, PARM_DOD, MDVUtility.ToStr(dsPatient.Patients.Rows[0][dsPatient.Patients.DODColumn.ColumnName]) == "" ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.DODColumn.ColumnName]);
                dbManager.AddParameters(5, PARM_PATIENT_STATEMENT, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientStatementColumn.ColumnName]);
                dbManager.AddParameters(6, PARM_CAUSE_OF_DEATH, dsPatient.Patients.Rows[0][dsPatient.Patients.CauseOfDeathColumn.ColumnName]);
                dbManager.AddParameters(7, PARM_2NDPREF_COMMUNICATION_ID, MDVUtility.ToInt32(dsPatient.Patients.Rows[0][dsPatient.Patients.ScndPrefCommunicationIdColumn.ColumnName]) == 0 ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.ScndPrefCommunicationIdColumn.ColumnName]);
                dbManager.AddParameters(8, COMMUNICATE_WIH_GUARANTOR, dsPatient.Patients.Rows[0][dsPatient.Patients.CommunicatewithGuarantorColumn.ColumnName]);
                dbManager.AddParameters(9, COMMUNICATION_OPTOUT, dsPatient.Patients.Rows[0][dsPatient.Patients.CommunicationOptoutColumn.ColumnName]);

                dsPatient = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PREF_UPDATE, dsPatient, dsPatient.Patients.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return dsPatient;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::UpdatePatientPreferences", PROC_PATIENT_PREF_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatient UpdatePatientPreferencesNative(DSPatient dsPatient)
        {
            // DSPatient ds = new DSPatient();
            var dbManager = ClientConfiguration.GetDBManager();
            var dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = dsPatient.Patients.GetChanges();
                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_PATIENT_ID, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);
                dbManager.AddParameters(1, PARM_PREF_COMMUNICATION_ID, MDVUtility.ToInt32(dsPatient.Patients.Rows[0][dsPatient.Patients.PrefCommunicationIdColumn.ColumnName]) == 0 ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.PrefCommunicationIdColumn.ColumnName]);
                dbManager.AddParameters(2, PARM_2NDPREF_COMMUNICATION_ID, MDVUtility.ToInt32(dsPatient.Patients.Rows[0][dsPatient.Patients.ScndPrefCommunicationIdColumn.ColumnName]) == 0 ? null : dsPatient.Patients.Rows[0][dsPatient.Patients.ScndPrefCommunicationIdColumn.ColumnName]);
                dbManager.AddParameters(3, COMMUNICATION_OPTOUT, dsPatient.Patients.Rows[0][dsPatient.Patients.CommunicationOptoutColumn.ColumnName]);
                dbManager.AddParameters(4, PARM_PATIENT_STATEMENT, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientStatementColumn.ColumnName]);





                dsPatient = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PREF_UPDATE_Native, dsPatient, dsPatient.Patients.TableName);

                if (dtTemp == null) return dsPatient;

                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsPatient.Patients.Rows[0][dsPatient.Patients.PatientIdColumn].ToString());
                dsDBAudit.AcceptChanges();

                return dsPatient;
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
        #endregion



        #region NATIVE FUNCTIONS

        public List<PatientLookupModel> LookupMostViewedPatientNative(long EntityId, long UserId)
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, UserId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_MOST_VIEWED_LOOKUP_NATIVE);

                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.PatientID = !String.IsNullOrEmpty(reader["PatientID"].ToString()) ? reader["PatientID"].ToString() : "";
                    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.LastName = !String.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "";
                    model.FirstName = !String.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "";
                    model.MiddleInitial = !String.IsNullOrEmpty(reader["MI"].ToString()) ? reader["MI"].ToString() : "";
                    model.DOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? reader["DOB"].ToString() : "";
                    model.Sex = !String.IsNullOrEmpty(reader["Sex"].ToString()) ? reader["Sex"].ToString() : "";
                    model.Age = !String.IsNullOrEmpty(reader["Age"].ToString()) ? reader["Age"].ToString() : "";
                    model.SSN = !String.IsNullOrEmpty(reader["SSN"].ToString()) ? reader["SSN"].ToString() : "";
                    model.imagedata = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? reader["PatientImage"].ToString() : "";
                    model.PatientImageThumbnail = !String.IsNullOrEmpty(reader["PatientImageThumbnail"].ToString()) ? Convert.ToBase64String((byte[])reader["PatientImageThumbnail"]) : "";
                    PatientLookupList.Add(model);
                }


                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatientNative", PROC_PATIENT_LOOKUP_NATIVE, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// this function is used to populate the list of patients for native devices
        /// </summary>
        /// <param name="Searchstring"> patient can be searched by account number, first name or last name
        /// if this string is empty then recently accessed patiens will be fetheced</param>
        /// <param name="EntityId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsActive"></param>
        /// <returns>Lookup list of patients</returns>
        public List<PatientLookupModel> LookupPatientNative(string Searchstring, long EntityId, long UserId, string IsActive, int PageNumber)
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                bool IsRecentPatient = false;
                if (Searchstring == "")
                {
                    Searchstring = null;
                    IsRecentPatient = true;
                }
                if (IsActive == "")
                    IsActive = "true";
                if (PageNumber == 0) //:(
                {
                    PageNumber = 1;
                }

                dbManager.Open();
                dbManager.CreateParameters(6);

                //dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                dbManager.AddParameters(0, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(1, PARM_PAT_SEARCH_STRING, Searchstring);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(3, PARM_USER_ID, UserId);
                dbManager.AddParameters(4, PARM_IS_RECENT_PATIENT, IsRecentPatient);
                dbManager.AddParameters(5, PARM_Page_NUMBER, PageNumber);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_LOOKUP_NATIVE);

                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.PatientID = !String.IsNullOrEmpty(reader["PatientID"].ToString()) ? reader["PatientID"].ToString() : "";
                    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.LastName = !String.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "";
                    model.FirstName = !String.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "";
                    model.MiddleInitial = !String.IsNullOrEmpty(reader["MI"].ToString()) ? reader["MI"].ToString() : "";
                    model.DOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? reader["DOB"].ToString() : "";
                    model.Sex = !String.IsNullOrEmpty(reader["Sex"].ToString()) ? reader["Sex"].ToString() : "";
                    model.Age = !String.IsNullOrEmpty(reader["Age"].ToString()) ? reader["Age"].ToString() : "";
                    model.SSN = !String.IsNullOrEmpty(reader["SSN"].ToString()) ? reader["SSN"].ToString() : "";
                    model.imagedata = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? reader["PatientImage"].ToString() : "";
                    model.PatientImageThumbnail = !String.IsNullOrEmpty(reader["PatientImageThumbnail"].ToString()) ? Convert.ToBase64String((byte[])reader["PatientImageThumbnail"]) : "";
                    PatientLookupList.Add(model);
                }


                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupPatientNative", PROC_PATIENT_LOOKUP_NATIVE, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public PatientDemographicModel GetPatientDemographicsDetailsForCustomForms(long PatientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_DETAILS);
                PatientDemographicModel model = new PatientDemographicModel();
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                while (reader.Read())
                {
                    model.AccountNo = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.MRN = !String.IsNullOrEmpty(reader["MRNumber"].ToString()) ? reader["MRNumber"].ToString() : "";
                    model.SSN = !String.IsNullOrEmpty(reader["SSN"].ToString()) ? reader["SSN"].ToString() : "";
                    model.Prefix = !String.IsNullOrEmpty(reader["Prefix"].ToString()) ? reader["Prefix"].ToString() : "";
                    model.Suffix = !String.IsNullOrEmpty(reader["Suffix"].ToString()) ? reader["Suffix"].ToString() : "";
                    model.LastName = !String.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "";
                    model.FirstName = !String.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "";
                    model.FullName = !String.IsNullOrEmpty(reader["FullName"].ToString()) ? reader["FullName"].ToString() : "";
                    model.MiddleInitial = !String.IsNullOrEmpty(reader["MI"].ToString()) ? reader["MI"].ToString() : "";
                    model.DOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? MDVUtility.ToDateTime(reader["DOB"]).ToShortDateString() : "";
                    model.Sex = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    model.MaritalStatus = !String.IsNullOrEmpty(reader["MaritialStatus"].ToString()) ? reader["MaritialStatus"].ToString() : "";
                    model.Ethnicity = !String.IsNullOrEmpty(reader["Ethnicity"].ToString()) ? reader["Ethnicity"].ToString() : "";
                    model.Race = !String.IsNullOrEmpty(reader["Race"].ToString()) ? reader["Race"].ToString() : "";
                    model.PrefLanguage = !String.IsNullOrEmpty(reader["PrefLanguage"].ToString()) ? reader["PrefLanguage"].ToString() : "";
                    model.Address1 = !String.IsNullOrEmpty(reader["Address1"].ToString()) ? reader["Address1"].ToString() : "";
                    model.Address2 = !String.IsNullOrEmpty(reader["Address2"].ToString()) ? reader["Address2"].ToString() : "";
                    model.City = !String.IsNullOrEmpty(reader["City"].ToString()) ? reader["City"].ToString() : "";
                    model.State = !String.IsNullOrEmpty(reader["State"].ToString()) ? reader["State"].ToString() : "";
                    model.Zip = !String.IsNullOrEmpty(reader["ZipCode"].ToString()) ? reader["ZipCode"].ToString() : "";
                    model.ZipExt = !String.IsNullOrEmpty(reader["ZipCodeExt"].ToString()) ? reader["ZipCodeExt"].ToString() : "";
                    model.HomeTel = !String.IsNullOrEmpty(reader["HomePhoneNo"].ToString()) ? reader["HomePhoneNo"].ToString() : "";
                    model.WorkTel = !String.IsNullOrEmpty(reader["WorkPhoneNo"].ToString()) ? reader["WorkPhoneNo"].ToString() : "";
                    model.Cell = !String.IsNullOrEmpty(reader["CellNo"].ToString()) ? reader["CellNo"].ToString() : "";
                    model.Fax = !String.IsNullOrEmpty(reader["FaxNo"].ToString()) ? reader["FaxNo"].ToString() : "";
                    model.Email = !String.IsNullOrEmpty(reader["EmailAddress"].ToString()) ? reader["EmailAddress"].ToString() : "";
                    model.PatientWorkPhoneExt = !String.IsNullOrEmpty(reader["WorkPhoneExt"].ToString()) ? reader["WorkPhoneExt"].ToString() : "";
                    model.PatientEmergencyContactName = !String.IsNullOrEmpty(reader["PatientEmergencyContactName"].ToString()) ? reader["PatientEmergencyContactName"].ToString() : "";
                    model.PatientEmergencyContactAddress = !String.IsNullOrEmpty(reader["PatientEmergencyContactAddress"].ToString()) ? reader["PatientEmergencyContactAddress"].ToString() : "";
                    model.EmergencyContactRelationship = !String.IsNullOrEmpty(reader["EmergencyContactRelationship"].ToString()) ? reader["EmergencyContactRelationship"].ToString() : "";
                    model.PatientEmergencyContactPhone = !String.IsNullOrEmpty(reader["PatientEmergencyContactPhone"].ToString()) ? reader["PatientEmergencyContactPhone"].ToString() : "";
                    model.PatientEmergencyContactCell = !String.IsNullOrEmpty(reader["PatientEmergencyContactCell"].ToString()) ? reader["PatientEmergencyContactCell"].ToString() : "";
                    string AgeResponse = !String.IsNullOrEmpty(reader["DOB"].ToString())? MDVUtility.GetAge(MDVUtility.ToDateTime(reader["DOB"].ToString())): "";
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(AgeResponse);
                    if (SearchedfieldsJSON["status"])
                    {
                        model.Age= MDVUtility.ToStr(SearchedfieldsJSON["ActualAge"]);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::GetPatientDemographicsDetailsForCustomForms", PROC_PATIENT_DETAILS, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        
        public List<PatientDemographicModel> GetPatientDemographics(int PatientId, string FormName)
        {
            List<PatientDemographicModel> PatientDemographicList = new List<PatientDemographicModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                if (FormName == "")
                    FormName = null;

                dbManager.Open();

                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_MODULE_NAME, FormName);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_FILL);

                PatientDemographicModel model = null;
                while (reader.Read())
                {
                    model = new PatientDemographicModel();
                    model.PatientID = !String.IsNullOrEmpty(reader["PatientID"].ToString()) ? reader["PatientID"].ToString() : "";
                    //model.PatientId = !String.IsNullOrEmpty(reader["PatientID"].ToString()) ? reader["PatientID"].ToString() : "";
                    model.AccountNo = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.MRN = !String.IsNullOrEmpty(reader["MRNumber"].ToString()) ? reader["MRNumber"].ToString() : "";
                    model.SSN = !String.IsNullOrEmpty(reader["SSN"].ToString()) ? reader["SSN"].ToString() : "";
                    model.Prefix = !String.IsNullOrEmpty(reader["Prefix"].ToString()) ? reader["Prefix"].ToString() : "";
                    // model.Prefix_text = !String.IsNullOrEmpty(reader["Prefix_text"].ToString()) ? reader["Prefix_text"].ToString() : "";
                    model.Suffix = !String.IsNullOrEmpty(reader["Suffix"].ToString()) ? reader["Suffix"].ToString() : "";
                    // model.Suffix_text = !String.IsNullOrEmpty(reader["Suffix_text"].ToString()) ? reader["Suffix_text"].ToString() : "";
                    model.LastName = !String.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "";
                    model.FirstName = !String.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "";
                    model.FullName = !String.IsNullOrEmpty(reader["FullName"].ToString()) ? reader["FullName"].ToString() : "";
                    model.MiddleInitial = !String.IsNullOrEmpty(reader["MI"].ToString()) ? reader["MI"].ToString() : "";
                    model.PreviousName = !String.IsNullOrEmpty(reader["PreviousName"].ToString()) ? reader["PreviousName"].ToString() : "";
                    model.DOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? MDVUtility.ToDateTime(reader["DOB"]).ToShortDateString() : "";
                    model.Age = !String.IsNullOrEmpty(reader["Age"].ToString()) ? reader["Age"].ToString() : "";
                    model.Sex = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    // model.Sex_text = !String.IsNullOrEmpty(reader["Sex_text"].ToString()) ? reader["Sex_text"].ToString() : "";
                    model.MaritalStatus = !String.IsNullOrEmpty(reader["MaritialStatus"].ToString()) ? reader["MaritialStatus"].ToString() : "";
                    model.MaritalStatusId = !String.IsNullOrEmpty(reader["MaritialStatusId"].ToString()) ? reader["MaritialStatusId"].ToString() : "";
                    // model.MaritalStatus_text = !String.IsNullOrEmpty(reader["MaritalStatus_text"].ToString()) ? reader["MaritalStatus_text"].ToString() : "";
                    model.Ethnicity = !String.IsNullOrEmpty(reader["EthnicityId"].ToString()) ? reader["EthnicityId"].ToString() : "";
                    model.Race = !String.IsNullOrEmpty(reader["RaceId"].ToString()) ? reader["RaceId"].ToString() : "";
                    model.LanguageID = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["PrefLanguageId"])) ? MDVUtility.ToStr(reader["PrefLanguageId"]) : "";
                    model.PrefLanguage = !String.IsNullOrEmpty(reader["PrefLanguageId"].ToString()) ? reader["PrefLanguageId"].ToString() : "";
                    model.Active = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.InactiveReason = !String.IsNullOrEmpty(reader["InactiveReason"].ToString()) ? reader["InactiveReason"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.Address1 = !String.IsNullOrEmpty(reader["Address1"].ToString()) ? reader["Address1"].ToString() : "";
                    model.Address2 = !String.IsNullOrEmpty(reader["Address2"].ToString()) ? reader["Address2"].ToString() : "";
                    model.City = !String.IsNullOrEmpty(reader["City"].ToString()) ? reader["City"].ToString() : "";
                    model.State = !String.IsNullOrEmpty(reader["State"].ToString()) ? reader["State"].ToString() : "";
                    model.Zip = !String.IsNullOrEmpty(reader["ZipCode"].ToString()) ? reader["ZipCode"].ToString() : "";
                    model.ZipExt = !String.IsNullOrEmpty(reader["ZipCodeExt"].ToString()) ? reader["ZipCodeExt"].ToString() : "";
                    model.HomeTel = !String.IsNullOrEmpty(reader["HomePhoneNo"].ToString()) ? reader["HomePhoneNo"].ToString() : "";
                    //WorkTel Line Uncommented by faizan ameen.
                    model.WorkTel = !String.IsNullOrEmpty(reader["WorkPhoneNo"].ToString()) ? reader["WorkPhoneNo"].ToString() : "";
                    // model.Ext = !String.IsNullOrEmpty(reader["Ext"].ToString()) ? reader["Ext"].ToString() : "";
                    model.Cell = !String.IsNullOrEmpty(reader["CellNo"].ToString()) ? reader["CellNo"].ToString() : "";
                    model.Fax = !String.IsNullOrEmpty(reader["FaxNo"].ToString()) ? reader["FaxNo"].ToString() : "";
                    model.Email = !String.IsNullOrEmpty(reader["EmailAddress"].ToString()) ? reader["EmailAddress"].ToString() : "";
                    model.PatientProfileImagePath = !String.IsNullOrEmpty(reader["PatientProfileImagePath"].ToString()) ? reader["PatientProfileImagePath"].ToString() : "";
                    model.PatientProfileThumbnailPath = !String.IsNullOrEmpty(reader["PatientProfileThumbnailPath"].ToString()) ? reader["PatientProfileThumbnailPath"].ToString() : "";
                    model.BadAddress = !String.IsNullOrEmpty(reader["BadAddress"].ToString()) ? reader["BadAddress"].ToString() : "";
                    model.Provider = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.ProviderID = !String.IsNullOrEmpty(reader["ProviderID"].ToString()) ? reader["ProviderID"].ToString() : "";
                    model.Facility = !String.IsNullOrEmpty(reader["FacilityName"].ToString()) ? reader["FacilityName"].ToString() : "";
                    model.FacilityID = !String.IsNullOrEmpty(reader["FacilityID"].ToString()) ? reader["FacilityID"].ToString() : "";
                    model.Practice = !String.IsNullOrEmpty(reader["PracticeName"].ToString()) ? reader["PracticeName"].ToString() : "";
                    model.PracticeID = !String.IsNullOrEmpty(reader["PracticeID"].ToString()) ? reader["PracticeID"].ToString() : "";
                    model.RefProvider = !String.IsNullOrEmpty(reader["ReferringProviderName"].ToString()) ? reader["ReferringProviderName"].ToString() : "";
                    model.RefProviderID = !String.IsNullOrEmpty(reader["ReferringProviderId"].ToString()) ? reader["ReferringProviderId"].ToString() : "";
                    //model.PCP = !String.IsNullOrEmpty(reader["PCP"].ToString()) ? reader["PCP"].ToString() : "";
                    model.PCPID = !String.IsNullOrEmpty(reader["PCPID"].ToString()) ? reader["PCPID"].ToString() : "";
                    //model.Guarantor = !String.IsNullOrEmpty(reader["Guarantor"].ToString()) ? reader["Guarantor"].ToString() : "";
                    model.GuarantorID = !String.IsNullOrEmpty(reader["GuarantorID"].ToString()) ? reader["GuarantorID"].ToString() : "";
                    model.PatientBalance = !String.IsNullOrEmpty(reader["PatientBalance"].ToString()) ? reader["PatientBalance"].ToString() : "";
                    model.InsuranceBalance = !String.IsNullOrEmpty(reader["InsuranceBalance"].ToString()) ? reader["InsuranceBalance"].ToString() : "";
                    model.AdvanceBalance = !String.IsNullOrEmpty(reader["AdvanceBalance"].ToString()) ? reader["AdvanceBalance"].ToString() : "";
                    model.PatientPortalStatus = !String.IsNullOrEmpty(reader["PatientPortalStatus"].ToString()) ? reader["PatientPortalStatus"].ToString() : "";
                    model.SelfPay = !String.IsNullOrEmpty(reader["SelfPay"].ToString()) ? reader["SelfPay"].ToString() : "";
                    model.InsuranceName = !String.IsNullOrEmpty(reader["PatientID"].ToString()) ? reader["PatientID"].ToString() : "";
                    model.PatDocId = !String.IsNullOrEmpty(reader["PatDocId"].ToString()) ? reader["PatDocId"].ToString() : "";
                    // model.Image_url = !String.IsNullOrEmpty(reader["Image_url"].ToString()) ? reader["Image_url"].ToString() : "";
                    model.strRaceIds = !String.IsNullOrEmpty(reader["strRaceIds"].ToString()) ? reader["strRaceIds"].ToString() : "";
                    model.Scan = !String.IsNullOrEmpty(reader["Scan"].ToString()) ? reader["Scan"].ToString() : "";
                    model.OCR = !String.IsNullOrEmpty(reader["OCR"].ToString()) ? reader["OCR"].ToString() : "";
                    model.PrefCommunicationId = !String.IsNullOrEmpty(reader["PrefCommunicationId"].ToString()) ? reader["PrefCommunicationId"].ToString() : "";
                    model.ScndPrefCommunicationId = !String.IsNullOrEmpty(reader["ScndPrefCommunicationId"].ToString()) ? reader["ScndPrefCommunicationId"].ToString() : "";
                    model.HearFromId = !String.IsNullOrEmpty(reader["HearFromId"].ToString()) ? reader["HearFromId"].ToString() : "";
                    model.HearFromOther = !String.IsNullOrEmpty(reader["HearFromOther"].ToString()) ? reader["HearFromOther"].ToString() : "";
                    //model.CommunicateWithGurantor = !String.IsNullOrEmpty(reader["CommunicateWithGurantor"].ToString()) ? reader["CommunicateWithGurantor"].ToString() : "";
                    model.PatientImage = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? Convert.ToBase64String(reader["PatientImageThumbnail"] as byte[]) : "";
                    PatientDemographicList.Add(model);
                }


                return PatientDemographicList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::GetPatientDemographics", PROC_PATIENT_FILL, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public List<PatientLookupModel> LookupMaritalStatusNative()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {


                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MARITAL_STATUS_LOOKUP);

                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.MaritalStatusId = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.MaritalStatus = !String.IsNullOrEmpty(reader["Status"].ToString()) ? reader["Status"].ToString() : "";
                    PatientLookupList.Add(model);
                }


                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupMaritalStatusNative", PROC_MARITAL_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<PatientLookupModel> LookupEthnicityNative()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {


                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ETHNICITY_LOOKUP);

                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.EthnicityId = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.EthnicityDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    PatientLookupList.Add(model);
                }


                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupEthnicityNative", PROC_ETHNICITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<PatientLookupModel> LookupRaceNative()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {


                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RACE_LOOKUP);

                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.RaceId = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.RaceDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    PatientLookupList.Add(model);
                }


                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupRaceNative", PROC_RACE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<PatientLookupModel> LookupLanguagesNative()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {


                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LANGUAGES_LOOKUP);

                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.LanguagesId = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.LanguagesDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    PatientLookupList.Add(model);
                }


                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupLanguagesNative", PROC_LANGUAGES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<PatientLookupModel> LookupRelationshipNative()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RELATIONS_LOOKUP);
                PatientLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientLookupModel();
                    model.RelationshipId = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.RelationshipDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    PatientLookupList.Add(model);
                }
                return PatientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupRelationshipNative", PROC_RELATIONS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        #region "Save Demographics Native"

        private void createParametersDemographicsNative(IDBManager dbManager, PatientDemographicModelNative model)
        {
            if (model.PatientProfileImagePath != null)
            {
                dbManager.CreateParameters(30);
                dbManager.AddParameters(28, PARM_PATIENT_PROFILE_IMAGE_PATH, model.PatientProfileImagePath);
                dbManager.AddParameters(29, PARM_PATIENT_PROFILE_THUMBNAIL_PATH, model.PatientProfileThumbnailPath);
            }
            else
            {
                dbManager.CreateParameters(28);
            }
            dbManager.AddParameters(0, PARM_PATIENT_ID, (!string.IsNullOrEmpty(model.PatientID) ? MDVUtility.ToInt64(model.PatientID) : (long?)null));
            dbManager.AddParameters(1, PARM_SSN, model.SSN);
            dbManager.AddParameters(2, PARM_FIRST_NAME, model.FirstName);
            dbManager.AddParameters(3, PARM_LAST_NAME, model.LastName);
            dbManager.AddParameters(4, PARM_MI, model.MI);
            dbManager.AddParameters(5, PARM_DOB, model.DOB);
            dbManager.AddParameters(6, PARM_GENDER, model.Gender == "Other" ? "Unknown" : model.Gender);
            dbManager.AddParameters(7, PARM_MARITAL_STATUS, model.MaritialStatus);
            dbManager.AddParameters(8, PARM_ETHNICITY_ID, model.EthnicityId);
            dbManager.AddParameters(9, PARM_RACE_ID, model.RaceId.Split(',')[0]);
            dbManager.AddParameters(10, PARM_PREF_LANGUAGE_ID, MDVUtility.ToInt64(model.PrefLanguageId));
            dbManager.AddParameters(11, PARM_SELF_PAY, model.SelfPay);
            dbManager.AddParameters(12, PARM_ADDRESS1, model.Address1);
            dbManager.AddParameters(13, PARM_CITY, model.City);
            dbManager.AddParameters(14, PARM_STATE, model.State);
            dbManager.AddParameters(15, PARM_ZIPCODE, model.ZipCode);
            dbManager.AddParameters(16, PARM_ZIPCODE_EXT, model.ZipCodeExt);
            dbManager.AddParameters(17, PARM_CELL_NO, model.CellNo);
            dbManager.AddParameters(18, PARM_EMAIL_ADDRESS, model.EmailAddress);
            dbManager.AddParameters(19, PARM_HOME_PHONE_NO, model.HomePhoneNo);
            dbManager.AddParameters(20, PARM_ACCOUNT_NUMBER, null);
            dbManager.AddParameters(21, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(22, PARM_MODIFIED_ON, model.ModifiedOn);

            dbManager.AddParameters(23, PARM_PATIENT_IMAGE_TYPE, model.ImageType);


            dbManager.AddParameters(24, PARM_PATIENT_STRRACEIDS, model.strRaceIds);
            dbManager.AddParameters(25, PARM_COLUMN_NAME, model.listChangedColumns);
            dbManager.AddParameters(26, PARM_ADDRESS2, model.Address2);
            dbManager.AddParameters(27, PARM_DIMMY_PATIENT_ID, model.DimmyPatientId);


        }

        private void createParametersDemographicsDBAuditNative(IDBManager dbManager, DataChangeRequest model)
        {
            dbManager.CreateParameters(14);

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
            //dbManager.AddParameters(13, PARM_DBAUDIT_ID, );
            dbManager.AddParameters(13, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);



            //dbManager.AddParameters(24, PARM_SELF_PAY, model.SelfPay);

        }
        private void createParametersDemographicsDBAuditDetailNative(IDBManager dbManager, DataChangeRequest model)
        {
            dbManager.CreateParameters(11);

            dbManager.AddParameters(0, PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(1, PARM_COLUMN_KEY_ID, model.ColumnKeyId);
            dbManager.AddParameters(2, PARM_COLUMN_KEY_NAME, model.ColumnKeyName);
            dbManager.AddParameters(3, PARM_COLUMN_NAME, model.columnName);
            dbManager.AddParameters(4, PARM_CURRENT_VALUE_DISPLAY, model.CurrentValueDisplay);
            dbManager.AddParameters(5, PARM_DB_TABLE_NAME, model.DBTableName);
            dbManager.AddParameters(6, PARM_ENTITY_ID, model.EntityId);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(9, PARM_ORIGINAL_VALUE_DISPLAY, model.OriginalValueDisplay);
            dbManager.AddParameters(10, PARM_DBAUDIT_ID, model.DbAuditId);



            //dbManager.AddParameters(24, PARM_SELF_PAY, model.SelfPay);

        }
        public string updateDemographicsNative(IDBManager dbManager, PatientDemographicModelNative model)
        {
            try
            {
                SqlDataReader reader = null;
                string returnValue = "";
                createParametersDemographicsNative(dbManager, model);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_UPDATE_NATIVE);
                while (reader.Read())
                {
                    returnValue = reader["PatientId"].ToString();
                }
                reader.Close();
                return Convert.ToString(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_PATIENT_UPDATE_NATIVE", PROC_PATIENT_UPDATE_NATIVE, ex);

                throw ex;
            }

        }
        public string InsertupdateDemographicsInDBAuditNative(IDBManager dbManager, DataChangeRequest model)
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
        public string InsertDemographicsInDBAuditDetailNative(IDBManager dbManager, DataChangeRequest model)
        {
            try
            {

                string returnVal = "";
                createParametersDemographicsDBAuditDetailNative(dbManager, model);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_INSERT_DBAUDIT_DETAIL_NATIVE).ToString();



                if (returnVal == "-1")
                {
                    returnVal = "";
                }

                return returnVal;





            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::PROC_INSERT_DBAUDIT_DETAIL_NATIVE", PROC_INSERT_DBAUDIT_DETAIL_NATIVE, ex);

                throw ex;
            }

        }
        #endregion


        private void createParametersPatientConsentNative(IDBManager dbManager, PatientConsentVM model)
        {
            if (MDVUtility.ToInt64(model.PatientConsentId) > 0)
            {
                dbManager.CreateParameters(38);
                dbManager.AddParameters(37, PARM_PATIENT_CONSENTID, Convert.ToString(model.PatientConsentId));
            }
            else
            {
                dbManager.CreateParameters(40);
                dbManager.AddParameters(37, PARM_PATIENT_CONSENTID, Convert.ToString(model.PatientConsentId));
                dbManager.AddParameters(38, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(39, PARM_CREATED_ON, model.CreatedOn);
            }
            dbManager.AddParameters(0, PARM_PATIENT_ID, MDVUtility.ToInt64(model.PatientId));
            dbManager.AddParameters(1, PARM_PATIENT_NAME, model.PatientName);
            dbManager.AddParameters(2, PARM_ERX_CONSENT_SIGN, model.ERxConsentSign);
            dbManager.AddParameters(3, PARM_ERX_CONSENT_DATE, model.ERxConsentDate);
            dbManager.AddParameters(4, PARM_NPP_CONSENT_SIGN, model.NPPConsentSign);
            dbManager.AddParameters(5, PARM_NPP_CONSENT_DATE, model.NPPConsentDate);
            dbManager.AddParameters(6, PARM_FP_CONSENT_SIGN, model.FPConsentSign);
            dbManager.AddParameters(7, PARM_FP_CONSENT_DATE, model.FPConsentDate);
            dbManager.AddParameters(8, PARM_ABN_CONSENT_SIGN, model.ABNConsentSign);
            dbManager.AddParameters(9, PARM_ABN_CONSENT_DATE, model.ABNConsentDate);
            dbManager.AddParameters(10, PARM_NOTIFIER, model.Notifier);
            dbManager.AddParameters(11, PARM_IDENTIFICATION_NO, model.IdentificationNo);
            dbManager.AddParameters(12, PARM_EKG, model.EKG);
            dbManager.AddParameters(13, PARM_HOMECCULT, model.Homeccult);
            dbManager.AddParameters(14, PARM_CULTURES, model.Cultures);
            dbManager.AddParameters(15, PARM_SUPPLIER_AND_MATERIALS, model.SupplierAndMaterials);
            dbManager.AddParameters(16, PARM_LABWORK, model.LabWork);
            dbManager.AddParameters(17, PARM_VACCINE, model.Vaccine);
            dbManager.AddParameters(18, PARM_PFT, model.PFT);
            dbManager.AddParameters(19, PARM_UA, model.UA);
            dbManager.AddParameters(20, PARM_OTHERS, model.Others);
            dbManager.AddParameters(21, PARM_MEDICARE_REASON_1, model.MedicareReason1);
            dbManager.AddParameters(22, PARM_MEDICARE_REASON_2, model.MedicareReason2);
            dbManager.AddParameters(23, PARM_MEDICARE_REASON_3, model.MedicareReason3);
            dbManager.AddParameters(24, PARM_MEDICARE_REASON_4, model.MedicareReason4);
            dbManager.AddParameters(25, PARM_UPTO50, model.UpTo50);
            dbManager.AddParameters(26, PARM_UPTO50TO100, model.UpTo50To100);
            dbManager.AddParameters(27, PARM_UPTO100TO200, model.UpTo100To200);
            dbManager.AddParameters(28, PARM_UPTO200TO300, model.UpTo200To300);
            dbManager.AddParameters(29, PARM_MORETHAN300, model.MoreThan300);
            dbManager.AddParameters(30, PARM_OPTION_1, model.Option1);
            dbManager.AddParameters(31, PARM_OPTION_2, model.Option2);
            dbManager.AddParameters(32, PARM_OPTION_3, model.Option3);
            dbManager.AddParameters(33, PARM_Dummy_PatientId, model.DimmyPatientId);
         
            dbManager.AddParameters(34, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(35, PARM_IS_ACTIVE, "true");
            dbManager.AddParameters(36, PARM_MODIFIED_BY, model.ModifiedBy);

        }
        public string InsertUpdatePatientConsentNative(PatientConsentVM model, IDBManager dbManager)
        {

            string returnValue = "";

            SqlDataReader reader = null;
            try
            {

                createParametersPatientConsentNative(dbManager, model);
                if (MDVUtility.ToInt64(model.PatientConsentId) > 0)
                {
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_CONSENT_UPDATE);
                }
                else
                {
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_CONSENT_INSERT);
                }
                while (reader.Read())
                {
                    returnValue = reader["PatientConsentId"].ToString();
                }
                reader.Close();
                return Convert.ToString(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::InsertUpdatePatientConsentNative", PROC_PATIENT_CONSENT_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        public PatientConsentVM LoadPatientConsentNative(long patientId)
        {
            PatientConsentVM model = null;
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                dbManager.AddParameters(1, PARM_PATIENT_CONSENTID, null);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_CONSENT_SELECT);

                while (reader.Read())
                {
                    model = new PatientConsentVM();
                    model.PatientConsentId = reader.IsDBNull(reader.GetOrdinal("PatientConsentId")) ? "" : Convert.ToString(reader["PatientConsentId"]);
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.ERxConsentSign = reader.IsDBNull(reader.GetOrdinal("ERxConsentSign")) ? null : Convert.ToString(reader["ERxConsentSign"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("ERxConsentDate")))
                    {
                        model.ERxConsentDate = Convert.ToString(reader["ERxConsentDate"]);
                    }
                    model.NPPConsentSign = reader.IsDBNull(reader.GetOrdinal("NPPConsentSign")) ? null : Convert.ToString(reader["NPPConsentSign"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("NPPConsentDate")))
                    {
                        model.NPPConsentDate = Convert.ToString(reader["NPPConsentDate"]);
                    }
                    model.FPConsentSign = reader.IsDBNull(reader.GetOrdinal("FPConsentSign")) ? null : Convert.ToString(reader["FPConsentSign"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("FPConsentDate")))
                    {
                        model.FPConsentDate = Convert.ToString(reader["FPConsentDate"]);
                    }
                    model.ABNConsentSign = reader.IsDBNull(reader.GetOrdinal("ABNConsentSign")) ? null : Convert.ToString(reader["ABNConsentSign"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("ABNConsentDate")))
                    {
                        model.ABNConsentDate = Convert.ToString(reader["ABNConsentDate"]);
                    }

                    model.Notifier = reader.IsDBNull(reader.GetOrdinal("Notifier")) ? null : Convert.ToString(reader["Notifier"]);
                    model.IdentificationNo = reader.IsDBNull(reader.GetOrdinal("IdentificationNo")) ? null : Convert.ToString(reader["IdentificationNo"]);
                    model.PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : Convert.ToString(reader["PatientName"]);
                    model.EKG = reader.IsDBNull(reader.GetOrdinal("EKG")) ? false : Convert.ToBoolean(reader["EKG"]);
                    model.Homeccult = reader.IsDBNull(reader.GetOrdinal("Homeccult")) ? false : Convert.ToBoolean(reader["Homeccult"]);
                    model.Cultures = reader.IsDBNull(reader.GetOrdinal("Cultures")) ? false : Convert.ToBoolean(reader["Cultures"]);
                    model.SupplierAndMaterials = reader.IsDBNull(reader.GetOrdinal("SupplierAndMaterials")) ? false : Convert.ToBoolean(reader["SupplierAndMaterials"]);
                    model.LabWork = reader.IsDBNull(reader.GetOrdinal("LabWork")) ? false : Convert.ToBoolean(reader["LabWork"]);
                    model.Vaccine = reader.IsDBNull(reader.GetOrdinal("Vaccine")) ? false : Convert.ToBoolean(reader["Vaccine"]);
                    model.PFT = reader.IsDBNull(reader.GetOrdinal("PFT")) ? false : Convert.ToBoolean(reader["PFT"]);
                    model.UA = reader.IsDBNull(reader.GetOrdinal("UA")) ? false : Convert.ToBoolean(reader["UA"]);
                    model.Others = reader.IsDBNull(reader.GetOrdinal("Others")) ? false : Convert.ToBoolean(reader["Others"]);
                    model.MedicareReason1 = reader.IsDBNull(reader.GetOrdinal("MedicareReason1")) ? false : Convert.ToBoolean(reader["MedicareReason1"]);
                    model.MedicareReason2 = reader.IsDBNull(reader.GetOrdinal("MedicareReason2")) ? false : Convert.ToBoolean(reader["MedicareReason2"]);
                    model.MedicareReason3 = reader.IsDBNull(reader.GetOrdinal("MedicareReason2")) ? false : Convert.ToBoolean(reader["MedicareReason3"]);
                    model.MedicareReason4 = reader.IsDBNull(reader.GetOrdinal("MedicareReason3")) ? false : Convert.ToBoolean(reader["MedicareReason4"]);
                    model.UpTo50 = reader.IsDBNull(reader.GetOrdinal("UpTo50")) ? false : Convert.ToBoolean(reader["UpTo50"]);
                    model.UpTo50To100 = reader.IsDBNull(reader.GetOrdinal("UpTo50To100")) ? false : Convert.ToBoolean(reader["UpTo50To100"]);
                    model.UpTo100To200 = reader.IsDBNull(reader.GetOrdinal("UpTo100To200")) ? false : Convert.ToBoolean(reader["UpTo100To200"]);
                    model.UpTo200To300 = reader.IsDBNull(reader.GetOrdinal("UpTo200To300")) ? false : Convert.ToBoolean(reader["UpTo200To300"]);
                    model.MoreThan300 = reader.IsDBNull(reader.GetOrdinal("MoreThan300")) ? false : Convert.ToBoolean(reader["MoreThan300"]);
                    model.Option1 = reader.IsDBNull(reader.GetOrdinal("Option1")) ? false : Convert.ToBoolean(reader["Option1"]);
                    model.Option2 = reader.IsDBNull(reader.GetOrdinal("Option2")) ? false : Convert.ToBoolean(reader["Option2"]);
                    model.Option3 = reader.IsDBNull(reader.GetOrdinal("Option3")) ? false : Convert.ToBoolean(reader["Option3"]);
                }


                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadPatientConsentNative", PROC_PATIENT_CONSENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        #endregion

        public DSPatient SelectPatientCCDA(long PatientId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                List<string> tableNames = new List<string>();
                tableNames.Add(ds.Patients.TableName);
                tableNames.Add(ds.PatientRace.TableName);
                tableNames.Add(ds.PatientEthnicity.TableName);
                tableNames.Add(ds.PatientParticipants.TableName);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_SELECT_CCDA, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALPatient::SelectPatientCCDA", PROC_PATIENT_SELECT_CCDA);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

    }
}
