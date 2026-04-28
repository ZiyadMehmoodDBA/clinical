using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model;
using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALUser
    {

        #region " Stored Procedure Names"
        private const string PROC_COWORKERSGROUP_INSERT = "system.sp_CoWorkersGroupInsert";
        private const string PROC_COWORKERSGROUP_UPDATE = "system.sp_CoWorkersGroupUpdate";
        private const string PROC_COWORKERSGROUP_DELETE = "system.sp_CoWorkersGroupDelete";
        private const string PROC_USER_INSERT = "system.sp_UsersInsert";
        private const string PROC_USER_UPDATE = "system.sp_UsersUpdate";
        private const string PROC_USER_PASSWORD_UPDATE = "system.sp_UserPasswordUpdate";
        private const string PROC_USER_DELETE = "system.sp_UsersDelete";
        private const string PROC_USER_SELECT = "system.sp_UsersSelect";
        private const string PROC_USER_LOOKUP = "system.sp_UsersLookup";
        private const string PROC_USERS_COWORKER_LOOKUP = "system.sp_UsersCoWorkerLookup";
        private const string PROC_USERNAME_LOOKUP = "Patient.sp_Usernamesearch";
        private const string PROC_USERSROLESELECT_LOOKUP = "system.sp_UsersRoleSelect";
        private const string PROC_Co_WORKER_GROUP_USER_LOOKUP = "system.sp_CoWorkerGroupUsersLookup";
        private const string PROC_COWORKERGROUP_LOOKUP = "system.sp_CoWorkerGroupLookup";
        private const string PROC_LOGGEDIN_USERS_CHECK = "system.sp_LoggedInUsersCheck";
        private const string PROC_LOGGEDIN_USERS_INSERT = "system.sp_LoggedInUsersInsert";
        private const string PROC_LOGGEDOUT_OTHER_USERS_SESSION = "system.sp_LoggedOutOtherUsersSession";
        private const string PROC_THEMES_LOOKUP = "system.sp_ThemesLookup";

        private const string PROC_IS_USER_EXIST = "system.sp_UserExists";

        private const string PROC_ENTITY_USER_OPTION_SELECT = "system.sp_EntityUserOptionSelect";
        private const string PROC_ENTITY_USER_OPTION_SELECT_SHOW_ICD10 = "system.sp_EntityUserOptionSelectShowICD10";
        private const string PROC_ENTITY_USER_SELECT = "system.sp_UsersSelect";
        private const string PROC_USER_EMAIL = "Clinical.sp_UserEmailAddress";

        private const string PROC_ENTITY_USER_LOGIN_SELECT = "System.[sp_UsersLoginSelect]";
        private const string PROC_USER_PRIVILEGES = "system.sp_UsersPrivileges";

        private const string PROC_MODULE_FORM_USERS_INSERT = "System.sp_ModuleFormUsersInsert";
        private const string PROC_MODULE_FORM_USERS_DELETE = "System.sp_ModuleFormUsersDelete";
        private const string PROC_MODULE_FORMS_DELETE = "system.sp_ModuleFormsDelete";
        private const string PROC_MODULE_FORM_USER_SELECT = "System.sp_ModuleFormUsersSelect";

        private const string PROC_MODULE_FORM_USERS_PRIVILEGES_INSERT = "System.sp_ModuleFormUsersPrivilegesInsert";
        private const string PROC_MODULE_FORM_USERS_PRIVILEGES_DELETE = "System.sp_ModuleFormUsersPrivilegesDelete";
        private const string PROC_MODULE_FORM_USERS_PRIVILEGE_SELECT = "System.sp_ModuleFormUsersPrivilegesSelect";


        private const string PROC_USER_ENTITY_GROUP_INSERT = "System.sp_UsersEntityGroupInsert";
        private const string PROC_USER_ENTITY_GROUP_DELETE = "System.sp_UsersEntityGroupDelete";
        private const string PROC_USER_ENTITY_GROUP_SELECT = "System.sp_UsersEntityGroupSelect";
        private const string PROC_SECURITY_ENTITY_GROUP_LOOK_UP = "System.sp_SecurityGroupLookUp";

        private const string PROC_FORM_PRIV_SELECT = "dbo.sp_FormPrivilegesZD";

        private const string PROC_INSERT_DEFAULTSETTING = "System.sp_EntityUserOptionInsert";
        private const string PROC_UPDATE_DEFAULTSETTING = "System.sp_EntityUserOptionUpdate";

        private const string PROC_NOTIFICATIONS_COUNT_SELECT = "System.sp_NotificationsCountSelect";
        private const string PROC_PASSWORD_CONFIGURATION_INSERT_UPDATE = "System.sp_PasswordConfigurationInsertUpdate";
        private const string PROC_PASSWORD_CONFIGURATION_SELECT = "System.sp_PasswordConfigurationSelect";
        private const string PROC_GET_PASSWORD_REGEX = "System.sp_GetPasswordRegex";
        private const string PROC_CO_WORKERS_GROUP_SELECT = "system.sp_CoWorkersGroupSelect";
        private const string PROC_ACTIVE_INACTIVE_CO_WORKERS_GROUP = "system.sp_ActiveInActiveCoWorkersGroup";
        private const string PROC_USER_TYPE_LOOKUP = "System.sp_UserTypeLookup";

        #endregion

        #region "Query "

        #endregion


        #region "Parameters"

        private const string PARM_DEFAULT_TAB_MESSAGES = "@DefaultTabMessages";
        private const string PARM_ID = "@Id";
        private const string PARM_NAME = "@Name";
        private const string PARM_USER_IDs = "@UserIds";

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_USER_PASSWORD = "@UserPassword";
        private const string PARM_RESET_PASSWORD = "@ResetPassword";
        private const string PARM_PASSWORD_EXPIRATION = "@PasswordExpiration";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_BILLING_PROVIDER_ID = "@BillingProviderId";
        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PHONE_EXT = "@PhoneExt";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_SYSTEM_PRIVILEGES = "@SystemPrivileges";
        private const string PARM_PRIVILEGES_GROUP = "@PrivilegesGroup";
        private const string PARM_USER_ROLE_ID = "@UserRoleId";

        private const string PARM_RCOPIA_USER_NAME = "@RcUserName";
        private const string PARM_RCOPIA_PASSWORD = "@RcPassword";
        private const string PARM_RCOPIA_SIG_PASSWORD = "@RcSigPassword";
        private const string PARM_EMERGENCY_ROLE_ID = "@EmergencyRoleId";
        private const string PARM_DIRECT_ADDRESS = "@DirectAddress";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_ALL_USERS = "@AllUsers";
        private const string PARM_IS_ADMIN = "@IsAdmin";
        private const string PARM_ADDITIONAL_PRIVILEGES = "@AdditionalPrivileges";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_IS_FIRST_TIME_LOGGED_IN = "@isFirstTimeloggedIn";
        private const string PARM_IS_EXPITY_ALERT = "@IsExpiryAlert";
        private const string PARM_IS_MobileLogin = "@IsMobileLogin";
        private const string PARM_IS_MedText = "@IsMedText";
        private const string PARM_Mob_SessionExpTime = "@MobSessionExpTime";
        private const string PARM_DAYS_BEFORE_EXPIRY = "@DaysBeforeExpiry";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_ENTITY_REG_CODE = "@EntityRegCode";
        private const string PARM_MFUID = "@MFUId";
        private const string PARM_MODULE_FORM_ID = "@ModuleFormId";

        private const string PARM_MODULE_ID = "@ModuleId";
        private const string PARM_MODULE_USER_ID = "@Userid";
        private const string PARM_ROLE_ID = "@RoleId";

        private const string PARM_MODULE_FORM_USER_PRIVILIGES_ID = "@ModuleFormUserPriviligesId";
        private const string PARM_MODULE_FORM_USERS_ID = "@ModuleFormUsersId";
        private const string PARM_PRIVILEGE_SELECTION_ID = "@PrivilegeSelectionId";
        private const string PARM_IS_PRIVILEGED = "@IsPrivileged";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARM_SEC_GROUP_ID = "@SecGroupId";

        private const string PARM_USER_ENTITY_GROUP_ID = "@UserEntityGroupId";
        //private const string PARM_USER_ID = "@UserId";
        //private const string PARM_ENTITY_ID = "@Entityid";
        private const string PARM_SECURITY_GROUP_ID = "@SecurityGroupId";

        private const string PARM_SECURITY_USER_ID = "@SecurityUserId";

        private const string PARM_IS_MDVISION = "@IsMDVisionRequired";
        private const string PARM_MESSAGE = "@Message";
        private const string PARM_ENTITY_USER_OPTON_ID = "@EntityUserOptionId";
        private const string PARM_IDLE_TIME = "@IdleTime";
        private const string PARM_MAX_PATIENT_OPEN = "@MaxPatientOpen";
        private const string PARM_MAX_ATTEMPT = "@MaxAttempt";
        private const string PARM_ACCOUNT_LOCK_ACTION_TYPE = "@AccountLockActionType";
        private const string PARM_ACCOUNT_LOCK_UnLOCK_TIME = "@AccountLockUnlockTime";
        private const string PARM_ALLOW_MULTIPLE_LOGINS = "@AllowMultipleLogins";
        private const string PARM_SCH_WEEKDAYS = "@SchWeekDay";
        private const string PARM_PREFFERED_SCREEN = "@PreferredScreen";
        private const string PARM_PREFFERED_SCH_SCREEN = "@PreferredSchScreen";
        private const string PARM_DEFAULT_TEMPLATE = "@DefaultTemplate";
        private const string PARM_DEFAULT_SUPER_BILL = "@DefaultSuperBill";
        private const string PARM_PB_PHONE_NUMBER1 = "@PBPhoneNumber1";
        private const string PARM_PB_PHONE_NUMBER2 = "@PBPhoneNumber2";
        private const string PARM_PBPCP = "@PBPCP";
        private const string PARM_PB_REF_PROVIDER = "@PBRefProvider";
        private const string PARM_PB_PLAN_BALANCE = "@PBPlanBalance";
        private const string PARM_PB_PATIENT_BALANCE = "@PBPatientBalance";
        private const string PARM_CLAIM_PRINTER = "@ClaimPrinter";
        private const string PARM_CLAIM_TRAY = "@ClaimTray";
        private const string PARM_ATTACH_PRINTER = "@AttachmentPrinter";
        private const string PARM_ATTACH_TRAY = "@AttachmentTray";
        private const string PARM_PREFFERED_SCH_SCREEN_NAME = "@PreferredSchScreenName";
        private const string PARM_REFRESH_TIME = "@RefreshTime";
        private const string PARM_THEME_ID = "@ThemeId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_SCHEDULE_PATTERN = "@SchedulePattern";

        private const string PARM_IS_SEARCH_PATIENT = "@IsSearchPatient";
        private const string PARM_IS_QUICK_PATIENT = "@IsQuickAddPatient";
        private const string PARM_IS_APPOINTMENT = "@IsAppointment";
        private const string PARM_IS_NOTE = "@IsNote";
        private const string PARM_IS_TASK = "@IsTask";
        private const string PARM_IS_FAX = "@IsFax";
        private const string PARM_IS_MESSAGE = "@IsMessage";

        private const string PARM_IS_PATIENT_NAME = "@IsPatientName";
        private const string PARM_IS_DOB = "@IsPatientDOB";
        private const string PARM_IS_ADDRESS = "@IsPatientAddress";
        private const string PARM_IS_INSURANCE_PLAN = "@IsInsurancePlan";
        private const string PARM_IS_SUBSCRIBER_ID = "@IsSubscriberID";
        private const string PARM_IS_PATIENT_ACCOUNTNO = "@IsPatientAccountNo";
        private const string PARM_IS_PROVIDER = "@IsProvider";


        private const string PARM_IS_PRESCRIPTIONS_REFILL = "@IsPrescriptionsRefill";
        private const string PARM_IS_PENDING_PRESCRIPTIONS = "@IsPendingPrescriptions";
        private const string PARM_USER_LOGIN_ID = "@UserLoginId";
        private const string PARM_SESSION_ID = "@SessionID";
        private const string PARM_IS_SIMULTANEOUS_LOGIN_ALLOWED = "@IsSimultaneousLoginAllowed";
        private const string PARM_IS_LOGOUT = "@IsLogout";
        private const string PARM_IS_SUSPEAND = "@IsSuspand";
        private const string PARM_ENTITY_ID_HISTORY = "@EntityID";
        private const string PARM_IS_LOGIN = "@IsLogin";
        private const string PARM_IS_CHANGE_REFLECTED = "@IsChangeReflected";
        private const string PARM_CHANGE_SET = "@ChangeSet";
        private const string PARM_LOGIN_TIME = "@LogInTime";
        private const string PARM_LOGOUT_TIME = "@LogOutTime";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_AUTOLOGOFF = "@AutoLogOff";
        private const string PARM_PROCEDURE_FAVLIST = "@ProcedureFavList";
        private const string PARM_FORM_NAME = "@FormName";
        private const string PARM_PRIVILEGE_NAME = "@PrivilegeName";
        private const string PARM_ASSIGNED_TO = "@AssignedTo";
        private const string PARM_LAB_FAVLIST = "@LabFavList";
        private const string PARM_FAV_LIST_NAMES = "@FavListNames";

        private const string PARM_RECENT_PATIENT = "@RecentPatient";
        private const string PARM_IS_EMR = "@IsEMR";
        private const string PARM_IS_NOTE_UNSIGN = "@IsNoteUnSign";
        private const string PARM_IS_LOCKED = "@IsLocked";
        private const string PARM_ENM_CODES_TIME = "@ENMCodesTime";
        private const string PARM_NOTE_FONT_SIZE = "@NoteFontSize";
        private const string PARM_FAV_PROCEDURE_VAL = "@FavProceduresVal";
        private const string PARM_FAV_PROBLEM_VAL = "@FavProblemsVal";
        private const string PARM_FAV_MEDICALHX_VAL = "@FavMedicalHxVal";
        private const string PARM_FAV_FAMILYHX_VAL = "@FavFamilyHxVal";
        private const string PARM_FAV_SURGICALHX_VAL = "@FavSurgicalHxVal";
        private const string PARM_FAV_LABORDER_VAL = "@FavLabOrderVal";
        private const string PARM_FAV_PROCEDREORDER_VAL = "@FavProcedureOrderVal";
        private const string PARM_FAV_RADIOLOGYORDER_VAL = "@FavRadiologyOrderVal";
        private const string PARM_FAV_CONSULTATION_VAL = "@FavConsultationVal";
        private const string PARM_FAV_COMPLAINT_VAL = "@FavComplaintVal";
        private const string PARM_FAV_HOSPITALIZATIONHX_VAL = "@FavHospitalizationHxVal";
        private const string PARM_APPOINTMENT_STATUS = "@Appointmentstatus";
        private const string PARM_ISIMMUNIZATION_ALERT = "@IsImmunizationAlert";
        private const string PARM_ISDOCUMENTS_ALERT = "@IsDocumentsAlert";

        private const string PARM_IS_DEFAULT_HPI = "@IsDefaultHPI";

        private const string PARM_IS_CURRENT_MEDICATION = "@IsCurrentMedications";
        private const string PARM_IS_PAST_MEDICATION = "@IsPastMedications";
        private const string PARM_IS_ACTIVE_ALLERGIES = "@IsActiveAllergies";
        private const string PARM_IS_INACTIVE_ALLERGIES = "@IsInactiveAllergies";
        private const string PARM_IS_ACTIVE_PROBLEMS = "@IsActiveProblems";
        private const string PARM_IS_INACTIVEPROBLEMS = "@IsInactiveProblems";
        private const string PARM_IS_VITALS = "@IsVitals";
        private const string PARM_IS_IMMUNIZATIONS = "@IsImmunizations";
        private const string PARM_IS_FAMILY_HISTORY = "@IsFamilyHistory";
        private const string PARM_IS_SOCIAL_HISTORY = "@IsSocialHistory";
        private const string PARM_IS_MEDICAL_HISTORY = "@IsMedicalHistory";
        private const string PARM_IS_SURGICAL_HISTORY = "@IsSurgicalHistory";
        private const string PARM_IS_BIRTH_HISTORY = "@IsBirthHistory";
        private const string PARM_IS_ACTIVE_PROCEDURES = "@IsActiveProcedures";
        private const string PARM_IS_INACTIVE_PROCEDURES = "@IsInActiveProcedures";
        private const string PARM_IS_HOSPITAL_HISTORY = "@IsHospitalizationHistory";
        private const string PARM_IS_FULL_SSN = "@IsFullSSN";
        private const string PARM_IS_DATA_PRIVACY = "@IsDataPrivacy";
        private const string PARM_IS_COLLECTION = "@IsCollection";
        private const string PARM_User_Selected_Documents = "@Documentid";
        #region Password Configuration Parameters
        private const string PARM_PASSWORD_CONFIGURATION_ID = "@PasswordConfigurationId";
        private const string PARM_MIN_PASSWORD_LENGTH = "@MinPasswordLength";
        private const string PARM_MIN_SPECIAL_CHARACTER = "@MinSpecialCharacter";
        private const string PARM_MIN_ALPHA_CHARACTER = "@MinAlphaCharacter";
        private const string PARM_MIN_NUMERIC_CHARACTER = "@MinNumericCharacter";
        private const string PARM_MIN_UPPERCASE_CHARACTER = "@MinUppercaseCharacter";
        private const string PARM_MAX_PASSWORD_AGE = "@MaxPasswordAge";
        private const string PARM_PASSWORD_HISTORY = "@PasswordHistory";
        private const string PARM_ACCOUNT_LOCKOUT_THRESHOLD = "@AccountLockoutThreshold";
        private const string PARM_IDLE_SESSION_TIMEOUT = "@IdleSessionTimeout";
        private const string PARM_PASSWORD_REGEX = "@PasswordRegex";

        #endregion

        private const string PARM_PB_PRIMARY_INS = "@PBPrimaryInsurance";
        private const string PARM_PB_PAT_ADV_BAL = "@PBPatientAdvanceBalance";

        private const string PARM_RCOPIA_USER = "@RcopiaUser";
        private const string PARM_PREFERRED_PHONE = "@PreferredPhone";
        private const string PARM_FREE_TEXT_ICD = "@FreeTextICD";
        private const string PARM_CO_WORKERS_GROUP_ID = "@CoWorkersGroupId";

        private const string PARM_IS_SELECT_NOTE_COMPONENTS = "@IsSelectNoteComponents";
        private const string PARM_IS_SHOW_ICD10 = "@IsShowICD10";
        private const string PARM_IS_SHOW_FACILITY_SHORTNAME = "@IsShowFacilityShortName";
        private const string PARM_IS_SEND_BILLING_INQUIRY = "@isSendBillingInquiry ";
        private const string PARM_SCHEDULER_TIME_INTERVAL = "@SchedulerTimeInterval";

        private const string PARM_NOTE_PREVIE_STYLE = "@NotePrevieStyle";

        private const string PARM_PREFFERED_BILLING_SCREEN_NAME = "@PreferredBillingScreenName";
        private const string PARM_PREFFERED_BILLING_SCREEN = "@PreferredBillingScreen";
        private const string PARM_FAV_IMMUNIZATION_VAL = "@FavImmunizationVal";
        private const string PARM_FAV_THERAPEUTIC_VAL = "@FavTherapeuticVal";
        private const string PARM_EM_CODE_TYPE_IDs = "@EMCodeTypeIds";
        private const string PARM_AUDIT_USER_ID = "@AuditUserId";
        private const string PARM_IS_EXPAND = "@IsExpand";
        private const string PARM_IS_SOCPSYANDBEHAVIORHX = "@IsSocPsyandBehaviorHx";
        private const string PARM_IS_SEARCH_CRITERIA_EXPAND = "@IsSearchCriteriaExpand";
        private const string PARM_DOC_PRIORITY_ID = "@DocumentPriorityId";
        private const string PARM_RACE_IDS = "@RaceIds";
        private const string PARM_IS_PREVNOTECOMPLAINT = "@IsPrevNoteComplaints";
        private const string PARM_IS_SHOW_SUCCESS_MESSAGES = "@IsShowSuccessMessages";
        private const string PARM_IS_ORDERSEXPAND = "@IsOrdersExpand";
        private const string PARM_IS_RESULTSEXPAND = "@IsResultsExpand ";
        private const string PARM_IS_REFFERAL = "@isRefferal";
        private const string PARM_WORK_WEEK_DAYS = "@WorkWeekDays";
        private const string PARM_IS_PREV_NOTE_PE = "@IsPreviousNotePE";
        private const string PARM_IS_PREV_NOTE_ROS = "@IsPreviousNoteROS";

        private const string PARM_IS_CARE_PLAN = "@IsCarePlan";
        private const string PARM_IS_DEMOGRAPHICS = "@IsDemographics";
        private const string PARM_IS_MU3_FAMILY_HISTORY = "@IsMu3FamilyHistory";
        private const string PARM_IS_HEALTH_CARE_SURVEYS = "@IsHealthcareSurveys";
        private const string PARM_IS_PAT_HEALTH_INFO_CAPTURE = "@IsPatientHealthInformationCapture";
        private const string PARM_IS_TRANSIMISSION_CASE_REPORTING = "@IsTransimissionCaseReporting";
        private const string PARM_IS_TRANSITION_DIRECT_PROJECT = "@IsTransitionDirectProject";
        private const string PARM_IS_DATA_SEGMENTATION_PRIVACY = "@IsDataSegmentationPrivacy";
        private const string PARM_IS_IMPLANTABLE_DEVICES = "@IsImplantableDevices";
        private const string PARM_IS_CANCER_REGISTRIES = "@IsTransitCancerRegistries";
        private const string PARM_IS_CONSOLIDATED_CDA = "@IsConsolidatedCDA";
        private const string PARM_IS_TRANSIMISSION_ANTIMICOBIAL = "@IsTransimissionAntimicobial";
        private const string PARM_IS_SOCPSY_BEHAVIOURHX = "@IsSocPsyBehaviourHx";
        private const string PARM_IS_DATA_EXPORT = "@IsDataExport";
        private const string PARM_IS_IMMUNIZATION_REGISTRIES = "@IsImmunizationRegistries";
        private const string PARM_IS_SELECT_COMP_ON_COPY_NOTE = "@IsSelectCompOnCopyNote";
        private const string PARM_USER_TYPE_ID = "@UserTypeID";
        private const string PARM_IS_PE_TEMPLATENAME_REQUIRED = "@ISPETemplateNameRequired";
        private const string PARM_IS_CONFIGURE_ALERTS = "@IsConfigureAlerts";
        //PRD-31 by:MAHMAD
        private const string PARM_IS_EXPAND_FOLDER_TREE = "@IsExpandFolderTree";
        //PRD-31 by:MAHMAD

        private const string PARM_iTRACK_DASHBOARD = "@iTrackDashboardIds";
        private const string PARM_IS_PREV_NOTE_PROBLEMS = "@IsPrevNoteProblems";
        private const string PARM_PB_PAT_CCMTIMER = "@PBPatientCCMTimer";
        private const string PARM_IS_PREV_NOTE_TREATMENT_COMENTS = "@IsPrevNoteTreatmentComents";

        private const string PARM_IS_E_ACCESS ="@IseAccess";
        private const string PARM_IS_E_PRESCRIBING ="@IsePrescribing";
        private const string PARM_IS_IN_CORPORATE_SUMMARY_OF_CARE ="@IsInCorporateSummaryOfCare";
        private const string PARM_IS_PAT_DOCUMENT ="@IsPatientDocument";
        private const string PARM_IS_PAT_EDUCATION ="@IsPatientEducation" ;
        private const string PARM_IS_RECONCILIATION ="@IsReconciliation";
        private const string PARM_IS_SECURE_MESSAGING ="@IsSecureMessaging" ;
        private const string PARM_IS_TRANSITION_OF_CARE ="@IsTransitionOfCare";
        private const string PARM_IS_VIEW_DOWNLOAD_TRANSMIT ="@IsViewDownloadTransmit";

        private const string PARM_IS_CMS65v7 = "@IsCMS65v7";
        private const string PARM_IS_CMS69v6 = "@IsCMS69v6";
        private const string PARM_IS_CMS68v7 = "@IsCMS68v7";
        private const string PARM_IS_CMS138v6 = "@IsCMS138v6";
        private const string PARM_IS_CMS165v6 = "@IsCMS165v6";
        private const string PARM_IS_CMS22v6 = "@IsCMS22v6";

        private const string PARM_IS_CDS = "@IsCDS";
        private const string PARM_IS_DEPRESSION = "@IsDepression";
        private const string PARM_IS_TOBACCO = "@IsTobacco";
        private const string PARM_IS_PATIENT_PORTAL_DOCUMENT = "@IsPatientPortalDocument";
        private const string PARM_IS_LAND_ON_NOTE_COMPONENT = "@BlueButtonLandOnNoteComponent";
        private const string PARM_DEFAULT_TAB_MEDICATIONS = "@DefaultTabMedications";
        private const string PARM_IS_ASSIGNED_RESULTS = "@IsAssignedResults";
        private const string PARM_IS_NOTE_COMP_EXPANDED = "@IsNoteCompExpanded";

        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }

        #endregion

        #region Constructors

        public DALUser()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

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

        #region "Support Functions"
        private void CreateParameters(IDBManager dbManager, DSUsers ds, Boolean IsInsert)
        {
            //Start || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
            dbManager.CreateParameters(42);
            //End   || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail

            dbManager.AddParameters(0, PARM_USER_NAME, ds.Users.UserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(1, PARM_USER_PASSWORD, ds.Users.UserPasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_RESET_PASSWORD, ds.Users.ResetPasswordColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(3, PARM_PASSWORD_EXPIRATION, ds.Users.PasswordExpirationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.Users.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_FIRST_NAME, ds.Users.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_LAST_NAME, ds.Users.LastNameColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(7, PARM_IS_DEFAULT, ds.Users.IsDefaultColumn.ColumnName, DbType.Byte);
            //dbManager.AddParameters(8, PARM_FACILITY_ID, ds.Users.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_PROVIDER_ID, ds.Users.ProviderIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(10, PARM_RESOURCE_ID, ds.Users.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_PHONE_NO, ds.Users.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PHONE_EXT, ds.Users.PhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_EMAIL_ADDRESS, ds.Users.EmailAddressColumn.ColumnName, DbType.String);
            // dbManager.AddParameters(14, PARM_SYSTEM_PRIVILEGES, ds.Users.SystemPrivilegesColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(15, PARM_PRIVILEGES_GROUP, ds.Users.PrivilegesGroupColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_USER_ROLE_ID, ds.Users.UserRoleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.Users.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_IS_ADMIN, ds.Users.IsAdminColumn.ColumnName, DbType.Byte);
            // dbManager.AddParameters(19, PARM_ADDITIONAL_PRIVILEGES, ds.Users.AdditionalPrivilegesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_BY, ds.Users.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_ON, ds.Users.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.Users.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.Users.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_ERROR_MESSAGE, ds.Users.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(18, PARM_AUTOLOGOFF, ds.Users.AutoLogOffColumn.ColumnName, DbType.Int32);

            if (IsInsert == true)
                dbManager.AddParameters(19, PARM_USER_ID, ds.Users.UserIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(19, PARM_USER_ID, ds.Users.UserIdColumn.ColumnName, DbType.Int64);

            //Start || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access
            dbManager.AddParameters(20, PARM_EMERGENCY_ROLE_ID, ds.Users.EmergencyRoleIdColumn.ColumnName, DbType.Int64);
            //End   || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access


            //Start || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
            dbManager.AddParameters(21, PARM_DIRECT_ADDRESS, ds.Users.DirectAddressColumn.ColumnName, DbType.String);
            //End   || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail


            dbManager.AddParameters(22, PARM_IS_EMR, ds.Users.IsEMRColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(23, PARM_IS_LOCKED, ds.Users.IsLockedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(24, PARM_RCOPIA_USER, ds.Users.RCopialUserColumn.ColumnName, DbType.Byte);

            dbManager.AddParameters(25, PARM_RCOPIA_USER_NAME, ds.Users.RcUserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_RCOPIA_PASSWORD, ds.Users.RcPasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_RCOPIA_SIG_PASSWORD, ds.Users.RcSigPasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_CO_WORKERS_GROUP_ID, ds.Users.CoWorkersGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(29, PARM_IS_NOTE_UNSIGN, ds.Users.IsNoteUnSignColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(30, PARM_IS_FULL_SSN, ds.Users.IsFullSSNColumn.ColumnName, DbType.Byte);
            // Faizan Ameen MU3-16 
            dbManager.AddParameters(31, PARM_IS_DATA_PRIVACY, ds.Users.IsDataPrivacyColumn.ColumnName, DbType.Byte);
            // Faizan Ameen MU3-16 
            dbManager.AddParameters(32, PARM_IS_COLLECTION, ds.Users.IsCollectionColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, "@MiddleInitial", ds.Users.MiddleInitialColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_User_Selected_Documents, ds.Users.UserSelectedDocumentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARM_IS_EXPITY_ALERT, ds.Users.IsExpiryAlertColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(36, PARM_DAYS_BEFORE_EXPIRY, ds.Users.DaysBeforeExpiryColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(37, PARM_IS_MobileLogin, ds.Users.IsMobileLoginColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(38, PARM_Mob_SessionExpTime, ds.Users.MobSessionExpTimeColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(39, PARM_IS_MedText, ds.Users.IsMedTextColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(40, PARM_USER_TYPE_ID, ds.Users.UserTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(41, PARM_IS_CONFIGURE_ALERTS, ds.Users.IsConfigureAlertsColumn.ColumnName, DbType.Byte);
        }
        private void CreateCoWorkersGroupParameters(IDBManager dbManager, DSCoWorkersGroup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, ds.Group.CoWorkersGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, ds.Group.CoWorkersGroupIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NAME, ds.Group.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.Group.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(3, PARM_USER_IDs, ds.Group.UserIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.Group.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.Group.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.Group.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.Group.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        private void CreateParametersForUserDefaultSettings(IDBManager dbManager, DSUsers ds, Boolean IsInsert, DataTable dtRaceIds = null)
        {
            if (dtRaceIds != null)
                dbManager.CreateParameters(166);
            else
                dbManager.CreateParameters(165);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ENTITY_USER_OPTON_ID, ds.EntityUserOption.EntityUserOptionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ENTITY_USER_OPTON_ID, ds.EntityUserOption.EntityUserOptionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_MODULE_USER_ID, ds.EntityUserOption.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ENTITY_ID, ds.EntityUserOption.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_REG_CODE, ds.EntityUserOption.EntityRegCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IDLE_TIME, ds.EntityUserOption.IdleTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MAX_PATIENT_OPEN, ds.EntityUserOption.MaxPatientOpenColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_MAX_ATTEMPT, ds.EntityUserOption.MaxAttemptColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_ACCOUNT_LOCK_ACTION_TYPE, ds.EntityUserOption.AccountLockActionTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ACCOUNT_LOCK_UnLOCK_TIME, ds.EntityUserOption.AccountLockUnlockTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_ALLOW_MULTIPLE_LOGINS, ds.EntityUserOption.AllowMultipleLoginsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SCH_WEEKDAYS, ds.EntityUserOption.SchWeekDayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_IS_DEFAULT, ds.EntityUserOption.IsDefaultColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_FACILITY_ID, ds.EntityUserOption.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_PROVIDER_ID, ds.EntityUserOption.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_BILLING_PROVIDER_ID, ds.EntityUserOption.BillingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_RESOURCE_ID, ds.EntityUserOption.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_IS_ACTIVE, ds.EntityUserOption.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARM_IS_DELETED, ds.EntityUserOption.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(18, PARM_CREATED_BY, ds.EntityUserOption.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CREATED_ON, ds.EntityUserOption.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_MODIFIED_BY, ds.EntityUserOption.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_MODIFIED_ON, ds.EntityUserOption.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_PREFFERED_SCREEN, ds.EntityUserOption.PreferredScreenColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(23, PARM_PREFFERED_SCH_SCREEN, ds.EntityUserOption.PreferredSchScreenColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(24, PARM_DEFAULT_TEMPLATE, ds.EntityUserOption.DefaultTemplateColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(25, PARM_DEFAULT_SUPER_BILL, ds.EntityUserOption.DefaultSuperBillColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_PB_PHONE_NUMBER1, ds.EntityUserOption.PBPhoneNumber1Column.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARM_PB_PHONE_NUMBER2, ds.EntityUserOption.PBPhoneNumber2Column.ColumnName, DbType.Byte);
            dbManager.AddParameters(28, PARM_PBPCP, ds.EntityUserOption.PBPCPColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(29, PARM_PB_REF_PROVIDER, ds.EntityUserOption.PBRefProviderColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(30, PARM_PB_PLAN_BALANCE, ds.EntityUserOption.PBPlanBalanceColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(31, PARM_PB_PATIENT_BALANCE, ds.EntityUserOption.PBPatientBalanceColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(32, PARM_CLAIM_PRINTER, ds.EntityUserOption.ClaimPrinterColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_CLAIM_TRAY, ds.EntityUserOption.ClaimTrayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_ATTACH_PRINTER, ds.EntityUserOption.AttachmentPrinterColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARM_ATTACH_TRAY, ds.EntityUserOption.AttachmentTrayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_PREFFERED_SCH_SCREEN_NAME, ds.EntityUserOption.PreferredSchScreenNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(37, PARM_REFRESH_TIME, ds.EntityUserOption.RefreshTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_THEME_ID, ds.EntityUserOption.ThemeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(39, PARM_SCHEDULE_PATTERN, ds.EntityUserOption.SchedulePatternColumn.ColumnName, DbType.String);
            dbManager.AddParameters(40, PARM_IS_SEARCH_PATIENT, ds.EntityUserOption.IsSearchPatientColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(41, PARM_IS_QUICK_PATIENT, ds.EntityUserOption.IsQuickAddPatientColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(42, PARM_IS_APPOINTMENT, ds.EntityUserOption.IsAppointmentColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(43, PARM_IS_NOTE, ds.EntityUserOption.IsNoteColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(44, PARM_IS_TASK, ds.EntityUserOption.IsTaskColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(45, PARM_IS_MESSAGE, ds.EntityUserOption.IsMessageColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(46, PARM_IS_PRESCRIPTIONS_REFILL, ds.EntityUserOption.IsPrescriptionsRefillColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(47, PARM_IS_PENDING_PRESCRIPTIONS, ds.EntityUserOption.IsPendingPrescriptionsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(48, PARM_FAV_LIST_NAMES, ds.EntityUserOption.FavListNamesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(49, PARM_RECENT_PATIENT, ds.EntityUserOption.RecentPatientColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(50, PARM_PB_PRIMARY_INS, ds.EntityUserOption.PBPrimaryInsuranceColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(51, PARM_PB_PAT_ADV_BAL, ds.EntityUserOption.PBPatientAdvanceBalanceColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(52, PARM_IS_CURRENT_MEDICATION, ds.EntityUserOption.IsCurrentMedicationsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(53, PARM_IS_PAST_MEDICATION, ds.EntityUserOption.IsPastMedicationsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(54, PARM_IS_ACTIVE_ALLERGIES, ds.EntityUserOption.IsActiveAllergiesColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(55, PARM_IS_INACTIVE_ALLERGIES, ds.EntityUserOption.IsInactiveAllergiesColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(56, PARM_IS_ACTIVE_PROBLEMS, ds.EntityUserOption.IsActiveProblemsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(57, PARM_IS_INACTIVEPROBLEMS, ds.EntityUserOption.IsInactiveProblemsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(58, PARM_IS_VITALS, ds.EntityUserOption.IsVitalsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(59, PARM_IS_IMMUNIZATIONS, ds.EntityUserOption.IsImmunizationsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(60, PARM_IS_FAMILY_HISTORY, ds.EntityUserOption.IsFamilyHistoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(61, PARM_IS_SOCIAL_HISTORY, ds.EntityUserOption.IsSocialHistoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(62, PARM_IS_MEDICAL_HISTORY, ds.EntityUserOption.IsMedicalHistoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(63, PARM_IS_SURGICAL_HISTORY, ds.EntityUserOption.IsSurgicalHistoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(64, PARM_IS_BIRTH_HISTORY, ds.EntityUserOption.IsBirthHistoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(65, PARM_IS_HOSPITAL_HISTORY, ds.EntityUserOption.IsHospitalizationHistoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(66, PARM_PREFERRED_PHONE, ds.EntityUserOption.PreferredPhoneColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(67, PARM_FREE_TEXT_ICD, ds.EntityUserOption.FreeTextICDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(68, PARM_ENM_CODES_TIME, ds.EntityUserOption.ENMCodesTimeColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(69, PARM_APPOINTMENT_STATUS, ds.EntityUserOption.AppointmentStatusColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(70, PARM_FAV_PROCEDURE_VAL, ds.EntityUserOption.FavProceduresValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(71, PARM_FAV_PROBLEM_VAL, ds.EntityUserOption.FavProblemsValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(72, PARM_FAV_MEDICALHX_VAL, ds.EntityUserOption.FavMedicalHxValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(73, PARM_FAV_FAMILYHX_VAL, ds.EntityUserOption.FavFamilyHxValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(74, PARM_FAV_SURGICALHX_VAL, ds.EntityUserOption.FavSurgicalHxValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(75, PARM_FAV_LABORDER_VAL, ds.EntityUserOption.FavLabOrderValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(76, PARM_FAV_PROCEDREORDER_VAL, ds.EntityUserOption.FavProcedureOrderValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(77, PARM_FAV_RADIOLOGYORDER_VAL, ds.EntityUserOption.FavRadiologyOrderValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(78, PARM_FAV_CONSULTATION_VAL, ds.EntityUserOption.FavConsultationValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(79, PARM_FAV_COMPLAINT_VAL, ds.EntityUserOption.FavComplaintValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(80, PARM_ISIMMUNIZATION_ALERT, ds.EntityUserOption.IsImmunizationAlertColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(81, PARM_IS_SELECT_NOTE_COMPONENTS, ds.EntityUserOption.IsSelectNoteComponentsColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(82, PARM_NOTE_FONT_SIZE, ds.EntityUserOption.NoteFontSizeColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(83, PARM_IS_ACTIVE_PROCEDURES, ds.EntityUserOption.IsActiveProceduresColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(84, PARM_IS_INACTIVE_PROCEDURES, ds.EntityUserOption.IsInActiveProceduresColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(85, PARM_IS_SHOW_ICD10, ds.EntityUserOption.IsShowICD10Column.ColumnName, DbType.Byte);
            dbManager.AddParameters(86, PARM_FAV_HOSPITALIZATIONHX_VAL, ds.EntityUserOption.FavHospitalizationHxValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(87, PARM_NOTE_PREVIE_STYLE, ds.EntityUserOption.NotePrevieStyleColumn.ColumnName, DbType.String);
            dbManager.AddParameters(88, PARM_PREFFERED_BILLING_SCREEN, ds.EntityUserOption.PreferredBillingScreenColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(89, PARM_PREFFERED_BILLING_SCREEN_NAME, ds.EntityUserOption.PreferredBillingScreenNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(90, PARM_IS_DEFAULT_HPI, ds.EntityUserOption.IsDefaultHPIColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(91, PARM_FAV_IMMUNIZATION_VAL, ds.EntityUserOption.FavImmunizationValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(92, PARM_FAV_THERAPEUTIC_VAL, ds.EntityUserOption.FavTherapeuticValColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(93, PARM_EM_CODE_TYPE_IDs, ds.EntityUserOption.EMCodesTypeIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(94, PARM_IS_EXPAND, ds.EntityUserOption.IsExpandColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(95, PARM_IS_SOCPSYANDBEHAVIORHX, ds.EntityUserOption.IsSocPsyandBehaviorHxColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(96, PARM_IS_SEARCH_CRITERIA_EXPAND, ds.EntityUserOption.IsSearchCriteriaExpandColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(97, PARM_IS_COLLECTION, ds.EntityUserOption.IsCollectionColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(98, PARM_DOC_PRIORITY_ID, ds.EntityUserOption.DefaultDocumentPriorityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(99, PARM_IS_SHOW_FACILITY_SHORTNAME, ds.EntityUserOption.IsShowFacilityShortNameColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(100, PARM_SCHEDULER_TIME_INTERVAL, ds.EntityUserOption.SchedulerTimeIntervalColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(101, PARM_IS_FAX, ds.EntityUserOption.IsFaxColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(102, PARM_ISDOCUMENTS_ALERT, ds.EntityUserOption.IsDocumentsAlertColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(103, PARM_IS_PATIENT_NAME, ds.EntityUserOption.IsPatientNameColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(104, PARM_IS_DOB, ds.EntityUserOption.IsDOBColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(105, PARM_IS_ADDRESS, ds.EntityUserOption.IsAddressColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(106, PARM_IS_INSURANCE_PLAN, ds.EntityUserOption.IsInsurancePlanColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(107, PARM_IS_SUBSCRIBER_ID, ds.EntityUserOption.IsSubscriberIDColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(108, PARM_IS_PATIENT_ACCOUNTNO, ds.EntityUserOption.IsPatientAccountNoColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(109, PARM_IS_PROVIDER, ds.EntityUserOption.IsProviderColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(110, PARM_IS_SEND_BILLING_INQUIRY, ds.EntityUserOption.isSendBillingInquiryColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(111, PARM_IS_PREVNOTECOMPLAINT, ds.EntityUserOption.IsPrevNoteComplaintsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(112, PARM_WORK_WEEK_DAYS, ds.EntityUserOption.WorkWeekDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(113, PARM_IS_SHOW_SUCCESS_MESSAGES, ds.EntityUserOption.IsShowSuccessMessagesColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(114, PARM_IS_ORDERSEXPAND, ds.EntityUserOption.IsOrdersExpandColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(115, PARM_IS_RESULTSEXPAND, ds.EntityUserOption.IsResultsExpandColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(116, PARM_IS_REFFERAL, ds.EntityUserOption.IsReferralRequiredColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(117, PARM_IS_PREV_NOTE_PE, ds.EntityUserOption.IsPreviousNotePEColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(118, PARM_IS_PREV_NOTE_ROS, ds.EntityUserOption.IsPreviousNoteROSColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(119, PARM_IS_DEMOGRAPHICS, ds.EntityUserOption.IsDemographicsColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(120, PARM_IS_MU3_FAMILY_HISTORY, ds.EntityUserOption.IsMu3FamilyHistoryColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(121, PARM_IS_HEALTH_CARE_SURVEYS, ds.EntityUserOption.IsHealthcareSurveysColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(122, PARM_IS_PAT_HEALTH_INFO_CAPTURE, ds.EntityUserOption.IsPatientHealthInformationCaptureColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(123, PARM_IS_TRANSIMISSION_CASE_REPORTING, ds.EntityUserOption.IsTransimissionCaseReportingColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(124, PARM_IS_TRANSITION_DIRECT_PROJECT, ds.EntityUserOption.IsTransitionDirectProjectColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(125, PARM_IS_DATA_SEGMENTATION_PRIVACY, ds.EntityUserOption.IsDataSegmentationPrivacyColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(126, PARM_IS_IMPLANTABLE_DEVICES, ds.EntityUserOption.IsImplantableDevicesColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(127, PARM_IS_CANCER_REGISTRIES, ds.EntityUserOption.IsTransitCancerRegistriesColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(128, PARM_IS_CONSOLIDATED_CDA, ds.EntityUserOption.IsConsolidatedCDAColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(129, PARM_IS_TRANSIMISSION_ANTIMICOBIAL, ds.EntityUserOption.IsTransimissionAntimicobialColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(130, PARM_IS_SOCPSY_BEHAVIOURHX, ds.EntityUserOption.IsMU3SocPsyBehaviourHxColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(131, PARM_IS_DATA_EXPORT, ds.EntityUserOption.IsDataExportColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(132, PARM_IS_IMMUNIZATION_REGISTRIES, ds.EntityUserOption.IsImmunizationRegistriesColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(133, PARM_IS_CARE_PLAN, ds.EntityUserOption.IsCarePlanColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(134, PARM_IS_SELECT_COMP_ON_COPY_NOTE, ds.EntityUserOption.IsSelectCompOnCopyNoteColumn.ColumnName, DbType.Byte);
            //PRD-31 by:MAHMAD
            dbManager.AddParameters(135, PARM_IS_EXPAND_FOLDER_TREE, ds.EntityUserOption.IsExpandFolderTreeColumn.ColumnName, DbType.Byte);
            //PRD-31 by:MAHMAD
            dbManager.AddParameters(136, PARM_IS_PE_TEMPLATENAME_REQUIRED, ds.EntityUserOption.ISPETemplateNameRequiredColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(137, PARM_iTRACK_DASHBOARD, ds.EntityUserOption.iTrackDashboardIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(138, PARM_IS_PREV_NOTE_PROBLEMS, ds.EntityUserOption.IsPrevNoteProblemsColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(139, PARM_PB_PAT_CCMTIMER, ds.EntityUserOption.PBPatientCCMTimerColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(140, PARM_IS_PREV_NOTE_TREATMENT_COMENTS, ds.EntityUserOption.IsPrevNoteTreatmentComentsColumn.ColumnName, DbType.Boolean);

            dbManager.AddParameters(141, PARM_IS_E_ACCESS, ds.EntityUserOption.IseAccessColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(142, PARM_IS_E_PRESCRIBING, ds.EntityUserOption.IsePrescribingColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(143, PARM_IS_IN_CORPORATE_SUMMARY_OF_CARE, ds.EntityUserOption.IsInCorporateSummaryOfCareColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(144, PARM_IS_PAT_DOCUMENT, ds.EntityUserOption.IsPatientDocumentColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(145, PARM_IS_PAT_EDUCATION, ds.EntityUserOption.IsPatientEducationColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(146, PARM_IS_RECONCILIATION, ds.EntityUserOption.IsReconciliationColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(147, PARM_IS_SECURE_MESSAGING, ds.EntityUserOption.IsSecureMessagingColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(148, PARM_IS_TRANSITION_OF_CARE, ds.EntityUserOption.IsTransitionOfCareColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(149, PARM_IS_VIEW_DOWNLOAD_TRANSMIT, ds.EntityUserOption.IsViewDownloadTransmitColumn.ColumnName, DbType.Boolean);

            dbManager.AddParameters(150, PARM_IS_CMS65v7, ds.EntityUserOption.IsCMS65v7Column.ColumnName, DbType.Boolean);
            dbManager.AddParameters(151, PARM_IS_CMS69v6, ds.EntityUserOption.IsCMS69v6Column.ColumnName, DbType.Boolean);
            dbManager.AddParameters(152, PARM_IS_CMS68v7, ds.EntityUserOption.IsCMS68v7Column.ColumnName, DbType.Boolean);
            dbManager.AddParameters(153, PARM_IS_CMS138v6, ds.EntityUserOption.IsCMS138v6Column.ColumnName, DbType.Boolean);
            dbManager.AddParameters(154, PARM_IS_CMS165v6, ds.EntityUserOption.IsCMS165v6Column.ColumnName, DbType.Boolean);
            dbManager.AddParameters(155, PARM_IS_CMS22v6, ds.EntityUserOption.IsCMS22v6Column.ColumnName, DbType.Boolean);

            dbManager.AddParameters(156, PARM_IS_CDS, ds.EntityUserOption.IsCDSColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(157, PARM_IS_DEPRESSION, ds.EntityUserOption.IsDepressionColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(158, PARM_IS_TOBACCO, ds.EntityUserOption.IsTobaccoColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(159, PARM_IS_PATIENT_PORTAL_DOCUMENT, ds.EntityUserOption.IsPatientPortalDocumentColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(160, PARM_IS_LAND_ON_NOTE_COMPONENT, ds.EntityUserOption.IsLandOnComponentColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(161, PARM_DEFAULT_TAB_MEDICATIONS, ds.EntityUserOption.DefaultTabMedicationsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(162, PARM_IS_ASSIGNED_RESULTS, ds.EntityUserOption.IsAssignedResultsColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(163, PARM_DEFAULT_TAB_MESSAGES, ds.EntityUserOption.DefaultTabMessagesColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(164, PARM_IS_NOTE_COMP_EXPANDED, ds.EntityUserOption.IsNoteCompExpandedColumn.ColumnName, DbType.Boolean);
            if (dtRaceIds != null)
                dbManager.AddParameters(165, PARM_RACE_IDS, dtRaceIds);
        }
        private void CreateModuleFormUsersParameters(IDBManager dbManager, DSUsers ds, Boolean IsInsert)
        {
            // dbManager.CreateParameters(ds.Tables[ds.ModuleFormUsers.TableName].Columns.Count);
            dbManager.CreateParameters(4);

            dbManager.AddParameters(0, PARM_MODULE_FORM_ID, ds.ModuleFormUsers.ModuleFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.ModuleFormUsers.UserIdColumn.ColumnName, DbType.Int64);

            if (IsInsert == true)
                dbManager.AddParameters(2, PARM_MFUID, ds.ModuleFormUsers.MFUIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(2, PARM_MFUID, ds.ModuleFormUsers.MFUIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_AUDIT_USER_ID, ds.ModuleFormUsers.AuditUserIdColumn.ColumnName, DbType.String);
        }

        private void CreateUserEntityGroupParameters(IDBManager dbManager, DSUsers ds, Boolean IsInsert)
        {
            // dbManager.CreateParameters(ds.Tables[ds.ModuleFormUsers.TableName].Columns.Count);
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_USER_ENTITY_GROUP_ID, ds.UsersEntityGroup.UserEntityGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_USER_ENTITY_GROUP_ID, ds.UsersEntityGroup.UserEntityGroupIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_USER_ID, ds.UsersEntityGroup.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SECURITY_GROUP_ID, ds.UsersEntityGroup.SecurityGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_AUDIT_USER_ID, ds.UsersEntityGroup.AuditUserIdColumn.ColumnName, DbType.Int64);

        }

        private void CreateModuleFormUsersPrivilegesParameters(IDBManager dbManager, DSUsers ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.ModuleFormUsersPrivileges.TableName].Columns.Count);


            dbManager.AddParameters(0, PARM_MODULE_FORM_USERS_ID, ds.ModuleFormUsersPrivileges.ModuleFormUsersIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PRIVILEGE_SELECTION_ID, ds.ModuleFormUsersPrivileges.PrivilegeSelectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_IS_PRIVILEGED, ds.ModuleFormUsersPrivileges.IsPrivilegedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ModuleFormUsersPrivileges.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_IS_DELETED, ds.ModuleFormUsersPrivileges.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.ModuleFormUsersPrivileges.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.ModuleFormUsersPrivileges.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.ModuleFormUsersPrivileges.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.ModuleFormUsersPrivileges.ModifiedOnColumn.ColumnName, DbType.DateTime);
            if (IsInsert == true)
                dbManager.AddParameters(9, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ds.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(9, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ds.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_FORM_NAME, ds.ModuleFormUsersPrivileges.FormNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_PRIVILEGE_NAME, ds.ModuleFormUsersPrivileges.PrivilegeNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ASSIGNED_TO, ds.ModuleFormUsersPrivileges.AssignedToColumn.ColumnName, DbType.String);
        }

        private void CreateUserLoginHistoryParameters(IDBManager dbManager, DSUsers ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(9);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_USER_LOGIN_ID, ds.UserLoginHistory.UserLoginIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_USER_LOGIN_ID, ds.UserLoginHistory.UserLoginIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_USER_ID, ds.UserLoginHistory.UserIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SESSION_ID, ds.UserLoginHistory.SessionIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ENTITY_ID_HISTORY, ds.UserLoginHistory.EntityIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_LOGIN, ds.UserLoginHistory.IsLoginColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_IS_CHANGE_REFLECTED, ds.UserLoginHistory.IsChangeReflectedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CHANGE_SET, ds.UserLoginHistory.ChangeSetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_LOGIN_TIME, ds.UserLoginHistory.LogInTimeColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_LOGOUT_TIME, ds.UserLoginHistory.LogOutTimeColumn.ColumnName, DbType.DateTime);

        }

        private void CreatePasswordConfigurationParameters(IDBManager dbManager, DSUsers ds)
        {

            dbManager.CreateParameters(17);

            dbManager.AddParameters(0, PARM_PASSWORD_CONFIGURATION_ID, ds.PasswordConfiguration.PasswordConfigurationIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_MIN_PASSWORD_LENGTH, ds.PasswordConfiguration.MinPasswordLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_MIN_SPECIAL_CHARACTER, ds.PasswordConfiguration.MinSpecialCharacterColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_MIN_ALPHA_CHARACTER, ds.PasswordConfiguration.MinAlphaCharacterColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_MIN_NUMERIC_CHARACTER, ds.PasswordConfiguration.MinNumericCharacterColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_MIN_UPPERCASE_CHARACTER, ds.PasswordConfiguration.MinUppercaseCharacterColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_MAX_PASSWORD_AGE, ds.PasswordConfiguration.MaxPasswordAgeColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_PASSWORD_HISTORY, ds.PasswordConfiguration.PasswordHistoryColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_ACCOUNT_LOCKOUT_THRESHOLD, ds.PasswordConfiguration.AccountLockoutThresholdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_IDLE_SESSION_TIMEOUT, ds.PasswordConfiguration.IdleSessionTimeoutColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_PASSWORD_REGEX, ds.PasswordConfiguration.PasswordRegexColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.PasswordConfiguration.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_CREATED_BY, ds.PasswordConfiguration.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_ON, ds.PasswordConfiguration.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.PasswordConfiguration.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.PasswordConfiguration.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_ERROR_MESSAGE, ds.PasswordConfiguration.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);

        }


        #endregion

        #region "Insert, delete, update and get using dataset Functions users"
        //        public DSUsers LoadUser(long UserId, string UserName, string EntityId, string Active)
        //        {
        //            DSUsers ds = new DSUsers();
        //            IDBManager dbManager =ClientConfiguration.GetDBManager();
        //            try
        //            {
        //                if (UserName == "")
        //                    UserName = null;

        //                if (EntityId == "")
        //                    EntityId = null;

        //                if (Active == "")
        //                    Active = null;

        //                //if (IsAdmin == "")
        //                //    IsAdmin = null;

        //                //dbManager.Open();
        //                //dbManager.CreateParameters(4);

        //                //if (UserId <= 0)
        //                //    dbManager.AddParameters(0, PARM_USER_ID, null);
        //                //else
        //                //dbManager.AddParameters(0, PARM_USER_ID, UserId);
        //                //dbManager.AddParameters(1, PARM_USER_NAME, UserName);
        //                //dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);
        //                //dbManager.AddParameters(3, PARM_LAST_NAME, LastName);

        //                ////dbManager.AddParameters(4, PARM_IS_ADMIN, IsAdmin);
        //                //ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_SELECT, ds, ds.Users.TableName);
        //                //return ds;
        //                dbManager.Open();
        //                dbManager.CreateParameters(4);

        //                if (UserId <= 0)
        //                    dbManager.AddParameters(0, PARM_USER_ID, null);
        //                else
        //                    dbManager.AddParameters(0, PARM_USER_ID, UserId);

        //                dbManager.AddParameters(1, PARM_USER_NAME, UserName);
        //                dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);
        //                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
        //                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_SELECT, ds,

        //ds.Users.TableName);

        //                return ds;
        //            }
        //            catch (Exception ex)
        //            {
        //                MDVLogger.LogErrorMessage("DALUser::LoadUser", PROC_USER_SELECT, ex);
        //                throw ex;
        //                //Usual code
        //            }
        //            finally
        //            {
        //                dbManager.Dispose();
        //            }



        //        }

        public DSUsers LoadUser(ref DSUsers ds, long UserId, string UserName, string EntityId, string FirstName, string LastName, string IsActive, string IsAdmin, int PageNumber = 1, int RowsPerPage = 1000)
        {

            if (ds == null)
                ds = new DSUsers();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (UserName == "")
                    UserName = null;

                if (EntityId == "")
                    EntityId = null;

                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (IsActive == "")
                    IsActive = null;

                if (IsAdmin == "")
                    IsAdmin = null;

                dbManager.Open();
                dbManager.CreateParameters(11);

                if (UserId == 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, UserId);

                dbManager.AddParameters(1, PARM_USER_NAME, UserName);
                //dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);

                if (EntityId == null)
                {
                    //if the user is MDVISION or userId is being passed then entityId can be sent null
                    if ((ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper()) || UserId > 0)
                        dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(3, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(4, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(5, PARM_IS_ACTIVE, IsActive);
                //dbManager.AddParameters(6, PARM_IS_ADMIN, IsAdmin);

                dbManager.AddParameters(6, PARM_SECURITY_USER_ID, MDVSession.Current.AppUserId);

                string CreatedBy = null;
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != ClientConfiguration.DefaultUser.ToUpper())
                {
                    CreatedBy = ClientConfiguration.DefaultUser.ToUpper();
                }
                dbManager.AddParameters(7, PARM_CREATED_BY, CreatedBy);
                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.Users.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ENTITY_USER_SELECT, ds, ds.Users.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadUser", PROC_ENTITY_USER_SELECT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public List<Model.User.UserLoginModel> LoginUser_(long userId, string userName, string entityId, string isActive)
        {
            List<Model.User.UserLoginModel> userLoginModel = new List<Model.User.UserLoginModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            dbManager.CreateParameters(4);
            try
            {
                if (userName == "") userName = null;
                if (entityId == "") entityId = null;
                if (isActive == "") isActive = null;

                if (userId == 0)
                    dbManager.AddParameters(PARM_USER_ID, null);
                else
                    dbManager.AddParameters(PARM_USER_ID, userId);

                dbManager.AddParameters(PARM_USER_NAME, userName);
                dbManager.AddParameters(PARM_ENTITY_ID, entityId);
                dbManager.AddParameters(PARM_IS_ACTIVE, isActive);
                dbManager.AddParameters(PARM_SECURITY_USER_ID, MDVSession.Current.AppUserId);

                //var reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ENTITY_USER_LOGIN_SELECT);
                //while (reader.Read())
                //{
                //    var model = new Model.User.UserLoginModel
                //    {
                //        UserId = !string.IsNullOrEmpty(reader["UserId"].ToString()) ? reader["UserId"].ToString() : "",
                //        UserName = !string.IsNullOrEmpty(reader["UserName"].ToString()) ? reader["UserName"].ToString() : "",
                //        UserPassword = !string.IsNullOrEmpty(reader["UserPassword"].ToString()) ? reader["UserPassword"].ToString() : "",
                //        ResetPassword = !string.IsNullOrEmpty(reader["ResetPassword"].ToString()) ? reader["ResetPassword"].ToString() : "",
                //        EntityId = !string.IsNullOrEmpty(reader["EntityId"].ToString()) ? reader["EntityId"].ToString() : "",
                //        FirstName = !string.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "",
                //        LastName = !string.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "",
                //        ProviderId = !string.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "",
                //        PhoneNo = !string.IsNullOrEmpty(reader["PhoneNo"].ToString()) ? reader["PhoneNo"].ToString() : "",
                //        PhoneExt = !string.IsNullOrEmpty(reader["PhoneExt"].ToString()) ? reader["PhoneExt"].ToString() : "",
                //        EmailAddress = !string.IsNullOrEmpty(reader["EmailAddress"].ToString()) ? reader["EmailAddress"].ToString() : "",
                //        UserRoleId = !string.IsNullOrEmpty(reader["UserRoleId"].ToString()) ? reader["UserRoleId"].ToString() : "",
                //        IsActive = !string.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "",
                //        IsAdmin = !string.IsNullOrEmpty(reader["IsAdmin"].ToString()) ? reader["IsAdmin"].ToString() : "",
                //        CreatedBy = !string.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "",
                //        CreatedOn = !string.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "",
                //        ModifiedBy = !string.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "",
                //        ModifiedOn = !string.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "",
                //        AutoLogOff = !string.IsNullOrEmpty(reader["AutoLogOff"].ToString()) ? reader["AutoLogOff"].ToString() : "",
                //        IsEMR = !string.IsNullOrEmpty(reader["IsEMR"].ToString()) ? reader["IsEMR"].ToString() : "",
                //        DirectAddress = !string.IsNullOrEmpty(reader["DirectAddress"].ToString()) ? reader["DirectAddress"].ToString() : "",
                //        IsDirectAddress = !string.IsNullOrEmpty(reader["IsDirectAddress"].ToString()) ? reader["IsDirectAddress"].ToString() : ""
                //    };

                //    userLoginModel.Add(model);
                //}
                //return userLoginModel;
                return dbManager.ExecuteReaders<Model.User.UserLoginModel>(PROC_ENTITY_USER_LOGIN_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoginUser_", PROC_ENTITY_USER_LOGIN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSUsers LoginUser(long UserId, string UserName, string EntityId, string IsActive)
        {

            //if (ds == null)
            DSUsers ds = new DSUsers();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (UserName == "")
                    UserName = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(5);

                if (UserId == 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, UserId);

                dbManager.AddParameters(1, PARM_USER_NAME, UserName);
                dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);

                dbManager.AddParameters(4, PARM_SECURITY_USER_ID, MDVSession.Current.AppUserId);


                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ENTITY_USER_LOGIN_SELECT, ds, ds.Users.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoginUser", PROC_ENTITY_USER_LOGIN_SELECT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public DSUsers LoadModuleFormUser(Int64 ModuleFormId, Int64 UserId)
        {
            DSUsers dsUser = new DSUsers();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ModuleFormId == 0)
                    dbManager.AddParameters(0, PARM_MODULE_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_FORM_ID, ModuleFormId);

                if (UserId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, UserId);

                dsUser = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_USER_SELECT, dsUser, dsUser.ModuleFormUsers.TableName);
                return dsUser;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUsers::LoadModuleFormUsers", PROC_MODULE_FORM_USER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSUsers LoadModuleFormUsersPrivileges(Int64 ModuleFormRolePrivilegesId, Int64 UserId, Int64 ModuleFormId)
        {
            DSUsers dsUser = new DSUsers();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(3);

                if (ModuleFormRolePrivilegesId == 0)
                    dbManager.AddParameters(0, PARM_MODULE_FORM_USER_PRIVILIGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ModuleFormRolePrivilegesId);


                if (UserId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, UserId);

                if (ModuleFormId == 0)
                    dbManager.AddParameters(2, PARM_MODULE_FORM_ID, null);
                else
                    dbManager.AddParameters(2, PARM_MODULE_FORM_ID, ModuleFormId);

                dsUser = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_PRIVILEGE_SELECT, dsUser, dsUser.ModuleFormUsersPrivileges.TableName);

                return dsUser;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::LoadModuleFormRolesPrivileges", PROC_MODULE_FORM_USERS_PRIVILEGE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSUserLookup LookupFullUserName(string Active, string UserName = "")
        {


            DSUserLookup ds = new DSUserLookup();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                string CreatedBy = null;
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != ClientConfiguration.DefaultUser.ToUpper())
                {
                    CreatedBy = ClientConfiguration.DefaultUser.ToUpper();
                }

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(1, PARM_USER_NAME, UserName);
                ds = (DSUserLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USERNAME_LOOKUP, ds, ds.UsersPractices.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LookupFullUserName", PROC_USERNAME_LOOKUP, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public DSUserLookup LookupUser(string Active, string AllUsers = "")
        {


            DSUserLookup ds = new DSUserLookup();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                string CreatedBy = null;
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != ClientConfiguration.DefaultUser.ToUpper())
                {
                    CreatedBy = ClientConfiguration.DefaultUser.ToUpper();
                }

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(2, PARM_CREATED_BY, CreatedBy);

                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(4, PARM_ALL_USERS, string.IsNullOrEmpty(AllUsers) ? "0" : "1");
                ds = (DSUserLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_LOOKUP, ds, ds.Users.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LookupUser", PROC_USER_LOOKUP, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public DSUserLookup LookupUsersForCoWorker()
        {


            DSUserLookup ds = new DSUserLookup();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSUserLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USERS_COWORKER_LOOKUP, ds, ds.Users.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LookupUsersForCoWorker", PROC_USERS_COWORKER_LOOKUP, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        public DSUserLookup LookupCoWorkerGroupUser()
        {


            DSUserLookup ds = new DSUserLookup();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSUserLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Co_WORKER_GROUP_USER_LOOKUP, ds, ds.Users.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LookupCoWorkerGroupUser", PROC_Co_WORKER_GROUP_USER_LOOKUP, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public DSUsers UpdateUser(ref DSUsers ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Users.GetChanges().Copy();

                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSUsers)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_USER_UPDATE, ds, ds.Users.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    var idColumn = ds.Users.Rows[0][ds.Users.UserIdColumn].ToString();
                    new DBActivityAudit().InsertDBAuditAsync(dtTemp, idColumn);

                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Update User", ds.Tables[ds.Users.TableName].Rows[0][ds.Users.UserIdColumn.ColumnName].ToString());

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::UpdateUser", PROC_USER_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public DSUsers UpdateUserPassword(ref DSUsers ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Users.GetChanges().Copy();
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_USER_ID, ds.Users.UserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_USER_PASSWORD, ds.Users.UserPasswordColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ds.Users.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_MODIFIED_ON, ds.Users.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, PARM_IS_FIRST_TIME_LOGGED_IN, ds.Users.isFirstTimeloggedInColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, ds.Users.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);

                ds = (DSUsers)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_USER_PASSWORD_UPDATE, ds, ds.Users.TableName);

                ds.AcceptChanges();

                if (dtTemp != null)
                {
                    var idColumn = ds.Users.Rows[0][ds.Users.UserIdColumn].ToString();
                    new DBActivityAudit().InsertDBAuditAsync(dtTemp, idColumn);

                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Update User", ds.Tables[ds.Users.TableName].Rows[0][ds.Users.UserIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::UpdateUserPassword", PROC_USER_PASSWORD_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        public string GetPasswordRegex(int userId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string passwordRegex = "";
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_ID, userId);
                passwordRegex = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PASSWORD_REGEX).ToString();

                return passwordRegex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }

        }


        public string UpdateUserPassword(int UserId, string Username, string UserPassword)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    //  MDVUtility.ValidateStringAgainstRegex(Password, GetPasswordRegex(UserId));

                }


                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_USER_ID, UserId);
                dbManager.AddParameters(1, PARM_USER_PASSWORD, UserPassword);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, Username);
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(5, PARM_IS_FIRST_TIME_LOGGED_IN, true);

                //  dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_PASSWORD_UPDATE);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_USER_PASSWORD_UPDATE).ToString();
                //if (returnValue != "")
                //    throw new Exception(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::UpdateUserPassword", PROC_USER_PASSWORD_UPDATE, ex);
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

        public DSUsers DeleteUser(ref DSUsers ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_ID, ds.Users.UserIdColumn.ColumnName, DbType.Int64);
                ds = (DSUsers)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_USER_DELETE, ds, ds.Users.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteUser", PROC_USER_DELETE, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteUser(string UserIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, UserIds);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USER_DELETE);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_USER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteUser", PROC_USER_DELETE, ex);
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

        public DSUsers InsertUser(ref DSUsers ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Users.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_INSERT, ds, ds.Users.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Users.Rows[0][ds.Users.UserIdColumn].ToString(), "");
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Users.TableName].Rows[0][ds.Users.UserIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::InsertUser", PROC_USER_INSERT, ex);
                //throw ex;
                //Usual code
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

        public string LoggedInUserInsert(long UserId, string SessionId)
        {
            object returnValue = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ID, null);
                dbManager.AddParameters(1, PARM_USER_ID, UserId.ToString());
                dbManager.AddParameters(2, PARM_SESSION_ID, SessionId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_LOGGEDIN_USERS_INSERT);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoggedInUserInsert", PROC_LOGGEDIN_USERS_INSERT, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public MDVision.Model.User.LoggedInUserModel LoggedInUserCheck(long UserId, string SessionId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (UserId > 0)
                    dbManager.AddParameters(PARM_USER_ID, UserId);
                else
                    dbManager.AddParameters(PARM_USER_ID, null);

                if (!string.IsNullOrEmpty(SessionId))
                    dbManager.AddParameters(PARM_SESSION_ID, SessionId);
                else
                    dbManager.AddParameters(PARM_SESSION_ID, null);

                return dbManager.ExecuteReader<MDVision.Model.User.LoggedInUserModel>(PROC_LOGGEDIN_USERS_CHECK);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoggedInUserCheck", PROC_LOGGEDIN_USERS_CHECK, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public bool LoggedOutOtherUsersSession(long UserId, string SessionId, bool IsSuspand, bool IsLogOut)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (UserId > 0)
                    dbManager.AddParameters(0, PARM_USER_ID, UserId);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, null);

                if (!string.IsNullOrEmpty(SessionId))
                    dbManager.AddParameters(1, PARM_SESSION_ID, SessionId);
                else
                    dbManager.AddParameters(1, PARM_SESSION_ID, null);

                dbManager.AddParameters(2, PARM_IS_SUSPEAND, IsSuspand);
                dbManager.AddParameters(3, PARM_IS_LOGIN, IsLogOut);
                dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LOGGEDOUT_OTHER_USERS_SESSION);
                return true;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoggedOutOtherUsersSession", PROC_LOGGEDOUT_OTHER_USERS_SESSION, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public DSCoWorkersGroup InsertCoWorkersGroup(ref DSCoWorkersGroup ds)
        {
            //DALUsersActivity obj = new DALUsersActivity();
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.Group.GetChanges();
                dbManager.Open();
                CreateCoWorkersGroupParameters(dbManager, ds, true);
                ds = (DSCoWorkersGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_COWORKERSGROUP_INSERT, ds, ds.Group.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Group.Rows[0][ds.Group.CoWorkersGroupIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Group.TableName].Rows[0][ds.Group.CoWorkersGroupIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::InsertCoWorkersGroup", PROC_COWORKERSGROUP_INSERT, ex);
                //throw ex;
                //Usual code
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
        public DSCoWorkersGroup UpdateCoWorkersGroup(ref DSCoWorkersGroup ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Group.GetChanges();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, ds.Group.CoWorkersGroupIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_NAME, ds.Group.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.Group.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(3, PARM_USER_IDs, ds.Group.UserIdsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, ds.Group.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_MODIFIED_ON, ds.Group.ModifiedOnColumn.ColumnName, DbType.DateTime);
                ds = (DSCoWorkersGroup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_COWORKERSGROUP_UPDATE, ds, ds.Group.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Group.Rows[0][ds.Group.CoWorkersGroupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::UpdateCoWorkersGroup", PROC_COWORKERSGROUP_UPDATE, ex);
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


        //public DSProfile InsertAndUpdateUser(ref DSProfile ds)
        //{
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        this.CreateInsertUpdateParameters(dbManager, ds);
        //        ds = (DSProfile)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_USER_INSERT, PROC_USER_UPDATE, PROC_USER_DELETE, ds, ds.Users.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALUser::InsertAndUpdateUser", PROC_USER_INSERT, ex);
        //        throw ex;
        //        //Usual code
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        public string IsUserExist(string UserName)
        {
            object returnValue;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_NAME, UserName);
                dbManager.AddParameters(1, PARM_MESSAGE, "", DbType.String, ParamDirection.Output, null, 15);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_USER_EXIST);
                if (returnValue == null)
                    return "1";
                else
                    return "0";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::IsUserExist", PROC_IS_USER_EXIST, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSUserLookup LookupUserRolesUser(string Active, String UserId, String UserRoleId, String EmergencyRoleId)
        {
            DSUserLookup ds = new DSUserLookup();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;



                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_ID, string.IsNullOrEmpty(UserId) ? null : UserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ROLE_ID, string.IsNullOrEmpty(UserRoleId) ? null : UserRoleId);

                dbManager.AddParameters(3, PARM_IS_ACTIVE, string.IsNullOrEmpty(Active) ? null : Active);
                dbManager.AddParameters(4, PARM_EMERGENCY_ROLE_ID, string.IsNullOrEmpty(EmergencyRoleId) ? null : EmergencyRoleId);


                ds = (DSUserLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USERSROLESELECT_LOOKUP, ds, ds.UsersRoleSelect.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LookupUserRolesUser", PROC_USERSROLESELECT_LOOKUP, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }

        }



        #region "use for transaction with dataset"
        //public DSProfile InsertUser(ref DSProfile ds, IDBManager dbManager)
        //{

        //    try
        //    {
        //        CreateParameters(dbManager, ds, true);
        //        ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_INSERT, ds, ds.Users.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALUser::InsertUser", PROC_USER_INSERT, ex);
        //        throw ex;
        //        //Usual code
        //    }


        //}

        //public DSProfile InsertAndUpdateUser(ref DSProfile ds, IDBManager dbManager)
        //{
        //    try
        //    {
        //        this.CreateInsertUpdateParameters(dbManager, ds);
        //        ds = (DSProfile)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_USER_INSERT, PROC_USER_UPDATE, PROC_USER_DELETE, ds, ds.USER.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALUser::InsertAndUpdateUser", PROC_USER_INSERT, ex);
        //        throw ex;
        //        //Usual code
        //    }

        //}
        #endregion
        #endregion

        #region "Insert, delete, update and get Functions For MODULES, FORMS, PRIVILEGES"

        public DSUsers LoadUserEntityGroup(long UserEntityGroupId, long UserId)
        {

            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (UserEntityGroupId == 0)
                    dbManager.AddParameters(0, PARM_USER_ENTITY_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ENTITY_GROUP_ID, UserEntityGroupId);

                if (UserId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, UserId);



                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_ENTITY_GROUP_SELECT, ds, ds.UsersEntityGroup.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadUserEntityGroup", PROC_USER_ENTITY_GROUP_SELECT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        public DSUsers UserSecurityGroupLookUp(string IsActive = null)
        {

            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                if (!string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                else
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_ENTITY_GROUP_LOOK_UP, ds, ds.SecurityGroup.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadSecurityEntityGroup", PROC_SECURITY_ENTITY_GROUP_LOOK_UP, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public DSUsers InsertUserEntityGroup(ref DSUsers ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // DataTable dtTemp = ds.UsersEntityGroup.GetChanges();
                dbManager.Open();
                CreateUserEntityGroupParameters(dbManager, ds, true);
                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_ENTITY_GROUP_INSERT, ds, ds.UsersEntityGroup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.UsersEntityGroup.Rows[0][ds.UsersEntityGroup.UserIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER_ENTITY_GROUP, true, "Insert User Entity Group", ds.Tables[ds.UsersEntityGroup.TableName].Rows[0][ds.UsersEntityGroup.UserEntityGroupIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::InsertUserEntityGroup", PROC_USER_ENTITY_GROUP_INSERT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public string DeleteUserEntityGroup(string UserEntityGroupId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ENTITY_GROUP_ID, UserEntityGroupId);
                dbManager.AddParameters(1, PARM_AUDIT_USER_ID, MDVSession.Current.AppUserId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USER_ENTITY_GROUP_DELETE);
                //dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
                //ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                //ds.AcceptChanges();
                //return ds;
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteModuleFormUsers", PROC_USER_ENTITY_GROUP_DELETE, ex);
                return ex.Message;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSUsers InsertModuleFormUsers(ref DSUsers ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateModuleFormUsersParameters(dbManager, ds, true);
                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_INSERT, ds, ds.ModuleFormUsers.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::InsertModuleFormUsers", PROC_MODULE_FORM_USERS_INSERT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public string DeleteModuleFormUsers(string ModuleFormUsersIds, long AuditUserId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MFUID, ModuleFormUsersIds);
                dbManager.AddParameters(1, PARM_AUDIT_USER_ID, AuditUserId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_DELETE);
                //dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
                //ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                //ds.AcceptChanges();
                //return ds;
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteModuleFormUsers", PROC_MODULE_FORM_USERS_DELETE, ex);
                return ex.Message;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSUsers InsertModuleFormUsersPrivileges(ref DSUsers ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ModuleFormUsersPrivileges.GetChanges();
                CreateModuleFormUsersPrivilegesParameters(dbManager, ds, true);
                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_PRIVILEGES_INSERT, ds, ds.ModuleFormUsersPrivileges.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertPrivilegeDBAudit(dtTemp, dbManager, "True", null, null, null);
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModuleFormUsersPrivileges::InsertModuleFormUsersPrivileges", PROC_MODULE_FORM_USERS_PRIVILEGES_INSERT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public string DeleteModuleFormUsersPrivileges(string ModuleFormUsersPrivilegesIds, string PrivilegeName, string FormName, string AssignedTo, long AuditUserId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                DSUsers ds = new DSUsers();
                //DataTable dtTemp = ds.ModuleFormUsersPrivileges.GetChanges();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ModuleFormUsersPrivilegesIds);
                dbManager.AddParameters(1, PARM_AUDIT_USER_ID, AuditUserId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_PRIVILEGES_DELETE);

                //dsDBAudit = new DBActivityAudit().InsertPrivilegeDBAudit(dtTemp, dbManager, "False", PrivilegeName, FormName, AssignedTo);
                //dsDBAudit.AcceptChanges();
                //dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
                //ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                //ds.AcceptChanges();
                //return ds;
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModuleFormUsersPrivileges::DeleteModuleFormUsersPrivileges", PROC_MODULE_FORM_USERS_PRIVILEGES_DELETE, ex);
                return ex.Message;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteCoWorkerGroup(string CoWorkerGroupId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, MDVUtility.ToInt64(CoWorkerGroupId));
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COWORKERSGROUP_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteCoWorkerGroup", PROC_COWORKERSGROUP_DELETE, ex);
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
        public string DeleteModuleUsers(long UserIds, long ModuleIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_MODULE_ID, ModuleIds);
                dbManager.AddParameters(1, PARM_ROLE_ID, null);
                dbManager.AddParameters(2, PARM_MODULE_USER_ID, UserIds);
                dbManager.AddParameters(3, PARM_AUDIT_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORMS_DELETE);
                //dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
                //ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                //ds.AcceptChanges();
                //return ds;
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser:: DeleteModuleUsers", PROC_MODULE_FORMS_DELETE, ex);
                return ex.Message;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        #endregion

        #region "Login"
        public bool LoadEntityUserOptionShowICD10(string UserName, string EntityId, string EntityRegCode = null)
        {
            // DSUsers ds = new DSUsers();

            System.Data.SqlClient.SqlDataReader reader;
            bool result = false;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(1, PARM_USER_NAME, UserName);

                dbManager.AddParameters(2, PARM_ENTITY_REG_CODE, EntityRegCode);

                reader = (System.Data.SqlClient.SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ENTITY_USER_OPTION_SELECT_SHOW_ICD10);

                // int r = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ENTITY_USER_OPTION_SELECT);


                while (reader.Read())
                {



                    result = reader["IsShowICD10"] != DBNull.Value ? Convert.ToBoolean(reader["IsShowICD10"]) : false;



                }

                return result;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadEntityUserOptionShowICD10", PROC_ENTITY_USER_OPTION_SELECT_SHOW_ICD10, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public List<Model.User.EntityUserOptions> LoadEntityUserOption_(List<Model.User.UserLoginModel> ds, string userName, string entityId, string entityRegCode = null)
        {
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_ENTITY_ID, entityId);
                dbManager.AddParameters(PARM_USER_NAME, userName);
                dbManager.AddParameters(PARM_ENTITY_REG_CODE, entityRegCode);
                return dbManager.ExecuteReaders<Model.User.EntityUserOptions>(PROC_ENTITY_USER_OPTION_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadEntityUserOption", PROC_ENTITY_USER_OPTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSUsers LoadEntityUserOption(ref DSUsers ds, string UserName, string EntityId, string EntityRegCode = null)
        {
            // DSUsers ds = new DSUsers();


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(1, PARM_USER_NAME, UserName);

                dbManager.AddParameters(2, PARM_ENTITY_REG_CODE, EntityRegCode);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ENTITY_USER_OPTION_SELECT, ds, ds.EntityUserOption.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadEntityUserOption", PROC_ENTITY_USER_OPTION_SELECT, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        public DSUsers LoadUserPrivileges_(ref DSUsers ds, string UserName)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_NAME, UserName);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_PRIVILEGES, ds, ds.UsersPrivileges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadUserPrivileges", PROC_USER_PRIVILEGES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public Model.Patient.DemographicLabelModel getDemographicData(long PatientId)
        {
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                var model = new Model.Patient.DemographicLabelModel();

                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                // dbManager.CreateParameters(2);
                parameters.Add(new SqlParameter("@PatientId", PatientId));
                parameters.Add(new SqlParameter("@UserId", MDVSession.Current.AppUserId));
                parameters.Add(new SqlParameter("@EntityId", MDVSession.Current.EntityId));
                var reader = dbManager.ExecuteReader("[Patient].[sp_GetDemographicLabelData]", parameters);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model.Settings.IsPatientName = reader["IsPatientName"].ToString();
                        model.Settings.IsPatientDOB = reader["IsPatientDOB"].ToString();
                        model.Settings.IsPatientAccountNo = reader["IsPatientAccountNo"].ToString();
                        model.Settings.IsPatientAddress = reader["IsPatientAddress"].ToString();
                        model.Settings.IsInsurancePlan = reader["IsInsurancePlan"].ToString();
                        model.Settings.IsSubscriberID = reader["IsSubscriberID"].ToString();
                        model.Settings.IsProvider = reader["IsProvider"].ToString();
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        model.PatientName = reader["PatientName"].ToString();
                        model.PatientDOB = reader["PatientDOB"].ToString();
                        model.PatientAddress = reader["PatientAddress"].ToString();
                        model.InsurancePlan = reader["InsurancePlan"].ToString();
                        model.SubscriberID = reader["SubscriberID"].ToString();
                        model.ProviderName = reader["ProviderName"].ToString();
                        model.AccountNumber = reader["AccountNumber"].ToString();
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadEntityUserOption", "[Patient].[sp_GetDemographicLabelData]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<Model.User.UserPrivileges> LoadUserPrivileges_(string userName)
        {
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_USER_NAME, userName);
                return dbManager.ExecuteReaders<Model.User.UserPrivileges>(PROC_USER_PRIVILEGES);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadEntityUserOption", PROC_USER_PRIVILEGES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSUsers LoadUserPrivileges(ref DSUsers ds, string UserName)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_NAME, UserName);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_PRIVILEGES, ds, ds.UsersPrivileges.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadUserPrivileges", PROC_USER_PRIVILEGES, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        #endregion

        public DSUsers LoadReportFormPrivliges(string UserName, long ModuleId)
        {
            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (UserName == "")
                    UserName = null;


                dbManager.Open();
                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_USER_NAME, UserName);
                if (ModuleId <= 0)
                    dbManager.AddParameters(1, PARM_MODULE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MODULE_ID, ModuleId);
                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FORM_PRIV_SELECT, ds, ds.ReportFormPrivliges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoadReportFormPrivliges", PROC_FORM_PRIV_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "Default User Settings"

        public DSUsers InsertDefaultSettings(DSUsers ds, DataTable dtRaceIds = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                CreateParametersForUserDefaultSettings(dbManager, ds, true, dtRaceIds);
                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_DEFAULTSETTING, ds, ds.EntityUserOption.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::InsertDefaultSettings", PROC_INSERT_DEFAULTSETTING, ex);
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

        public DSUsers UpdatDefaultSettings(DSUsers ds, DataTable dtRaceIds = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //if(dtRaceIds==null)
                //{
                //    dtRaceIds = new DataTable();
                //    DataColumn COLUMN = new DataColumn();
                //    COLUMN.ColumnName = "Id";
                //    COLUMN.DataType = typeof(int);
                //    dtRaceIds.Columns.Add(COLUMN);
                //    DataRow Dr = dtRaceIds.NewRow();
                //    Dr[0] = 0;
                //    dtRaceIds.Rows.Add(Dr);
                //}
                dbManager.Open();
                this.CreateParametersForUserDefaultSettings(dbManager, ds, false, dtRaceIds);
                ds = (DSUsers)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_UPDATE_DEFAULTSETTING, ds, ds.EntityUserOption.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDefaultSetting::UpdatDefaultSettings", PROC_UPDATE_DEFAULTSETTING, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSUsers LoadNotificationsCounts(Int64 PatientID)
        {

            DSUsers ds = new DSUsers();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (PatientID <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);


                if (MDVSession.Current.IsAdmin)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTIFICATIONS_COUNT_SELECT, ds, ds.NotificationsCounts.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoadNotificationsCounts", PROC_NOTIFICATIONS_COUNT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }


        }


        #endregion

        #region Lookup Themes

        public DSUserLookup LookupThemes(string Active)
        {
            DSUserLookup ds = new DSUserLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);
                ds = (DSUserLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_THEMES_LOOKUP, ds, ds.Themes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LookupThemes", PROC_THEMES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Lookup

        public List<LookUpModel> LookupUserType()
        {
            List<LookUpModel> list = new List<LookUpModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_USER_TYPE_LOOKUP);
                LookUpModel model = null;
                while (reader.Read())
                {
                    model = new LookUpModel();
                    model.Id = reader["UserTypeId"].ToString();
                    model.Name = reader["ShortName"].ToString();

                    list.Add(model);
                }

                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LookupUserType", PROC_USER_TYPE_LOOKUP, ex);
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


        public DSUsers SaveUpdatePasswordConfiguration(ref DSUsers ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePasswordConfigurationParameters(dbManager, ds);
                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PASSWORD_CONFIGURATION_INSERT_UPDATE, ds, ds.PasswordConfiguration.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::SaveUpdatePasswordConfiguration", PROC_PASSWORD_CONFIGURATION_INSERT_UPDATE, ex);
                throw ex;
                //Usual code
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        public DSUsers LoadPasswordConfiguration(Int64 PasswordConfigurationId)
        {

            DSUsers ds = new DSUsers();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                //if (PasswordConfigurationId <= 0)
                //    dbManager.AddParameters(0, PARM_PASSWORD_CONFIGURATION_ID, null);
                //else
                dbManager.AddParameters(0, PARM_PASSWORD_CONFIGURATION_ID, PasswordConfigurationId);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PASSWORD_CONFIGURATION_SELECT, ds, ds.PasswordConfiguration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoadPasswordConfiguration", PROC_PASSWORD_CONFIGURATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        #region Co-Worker Group
        public DSUsers LoadCoWorkerGroup(long CoWorkersGroupId, string Name, String IsActive, int PageNumber = 1, int RowsPerPage = 15)
        {
            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (CoWorkersGroupId > 0)
                {
                    dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, CoWorkersGroupId);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, null);
                }
                if (string.IsNullOrEmpty(Name))
                    dbManager.AddParameters(1, PARM_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_NAME, Name);
                if (IsActive == "")
                    dbManager.AddParameters(2, "@IsActive", null);
                else
                    dbManager.AddParameters(2, "@IsActive", IsActive);
                dbManager.AddParameters(3, "@PageNumber", PageNumber);
                dbManager.AddParameters(4, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(5, "@RecordCount", ds.CoWorkersGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CO_WORKERS_GROUP_SELECT, ds, ds.CoWorkersGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMTemplate", PROC_CO_WORKERS_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCoWorkersGroup FillCoWorkerGroup(long CoWorkersGroupId, string Name, String IsActive, int PageNumber = 1, int RowsPerPage = 15)
        {
            DSCoWorkersGroup ds = new DSCoWorkersGroup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (CoWorkersGroupId > 0)
                {
                    dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, CoWorkersGroupId);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, null);
                }
                if (string.IsNullOrEmpty(Name))
                    dbManager.AddParameters(1, PARM_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_NAME, Name);
                if (IsActive == "")
                    dbManager.AddParameters(2, "@IsActive", null);
                else
                    dbManager.AddParameters(2, "@IsActive", IsActive);
                dbManager.AddParameters(3, "@PageNumber", PageNumber);
                dbManager.AddParameters(4, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(5, "@RecordCount", ds.Group.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCoWorkersGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CO_WORKERS_GROUP_SELECT, ds, ds.Group.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMTemplate", PROC_CO_WORKERS_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string ActiveInActiveCoWorkerGroup(string CoWorkersGroupId, bool isActive)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_CO_WORKERS_GROUP_ID, MDVUtility.ToInt64(CoWorkersGroupId));
                dbManager.AddParameters(1, PARM_IS_ACTIVE, (isActive));
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, ParamDirection.Output);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ACTIVE_INACTIVE_CO_WORKERS_GROUP).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::ActiveInActiveCoWorkerGroup", PROC_ACTIVE_INACTIVE_CO_WORKERS_GROUP, ex);
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
        public DSUsers LookupCoWorker()
        {
            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COWORKERGROUP_LOOKUP, ds, ds.CoWorkersGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LookupCoWorker", PROC_COWORKERGROUP_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion



        #region Native 
        public List<UserPrivileges> LoadUserPrivilegesNative(string UserName)
        {
            List<UserPrivileges> UserPrivilegesList = new List<UserPrivileges>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_USER_NAME, UserName);


                UserPrivilegesList = dbManager.ExecuteReaders<UserPrivileges>(PROC_USER_PRIVILEGES);
                return UserPrivilegesList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::LoadCCMCallDetails", PROC_USER_PRIVILEGES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        public DSUsers LoadUserEmail(long userId)
        {
            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_ID, userId);

                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_EMAIL, ds, ds.UserEmail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::LoadUserEmail", PROC_USER_EMAIL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}

