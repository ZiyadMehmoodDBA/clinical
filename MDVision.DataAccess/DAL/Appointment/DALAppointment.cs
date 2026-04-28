using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Clinical.FollowUp;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Dashboard;
using MDVision.Model.Lookups;
using MDVision.Model.Native.Scheduler;
using MDVision.Model.PMSSchedule;
using MDVision.Model.Schedule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Appointment
{
    public class DALAppointment
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PATIENT_APPOINTMENT_INSERT = "Patient.sp_PatientAppointmentsInsert";
        private const string PROC_PATIENT_APPOINTMENT_UPDATE = "Patient.sp_PatientAppointmentsUpdate_New";
        private const string PROC_PATIENT_APPOINTMENT_UPDATE_NEW = "Patient.sp_PatientAppointmentsUpdate";
        private const string PROC_PATIENT_APPOINTMENT_STATUS_UPDATE = "Patient.sp_AppointmentsStatusUpdate";
        private const string PROC_PATIENT_APPOINTMENT_CANCELLATIONREASON_UPDATE = "Patient.sp_AppointmentCancellationReasonUpdate";
        private const string PROC_PATIENT_VISIT_UPDATE = "Clinical.sp_PatientVisitUpdate";
        private const string PROC_INSURANCE_APPOINTMENT_UPDATE = "Patient.sp_UpdateAppointmentInsurance";
        private const string PROC_PATIENT_APPOINTMENT_SELECT = "Patient.sp_PatientAppointmentsSelect";
        private const string PROC_PATIENT_APPOINTMENT_CHECKOUT_SELECT = "Patient.AppointmentCheckOutSelect";
        private const string PROC_PATIENT_APPOINTMENT_LOAD = "Patient.sp_PatientAppointmentsLoad";
        private const string PROC_APPOINTMENT_STATUS_FROM_REMINDER_UPDATE = "Provider.sp_AppointmentStatusFromReminderUpdate";
        private const string PROC_AVAILABLE_APPOINTMENTS = "Patient.sp_AppointmentsAvailable";
        private const string PROC_AVAILABLE_SCHEDULE_SLOTS = "Provider.sp_AvailableScheduleSlots";
        private const string PROC_SCHEDULE_REASON_DURATION = "Provider.sp_ScheduleReasonsDuration";
        private const string PROC_APPOINTMENT_DAYSLOT_SEARCH = "Patient.sp_DailySlotsWithAppointments";
        private const string PROC_APPOINTMENT_MOVE = "Patient.sp_MoveAppointment";
        private const string PROC_SLOT_STATUS_UPDATE = "Provider.sp_SlotStatusUpdate";
        private const string PROC_MONTHLY_APPOINTMENT = "Patient.sp_MonthlySlotsWithAppointments";
        private const string PROC_WEEKLY_APPOINTMENT = "Patient.sp_WeeklySlotsWithAppointments";
        private const string PROC_PATIENT_APPOINTMENT_DELETE = "Patient.sp_PatientAppointmentsDelete";
        private const string PROC_SCH_SLOT_UPDATE = "Patient.sp_SchSlotUpdate";
        private const string PROC_SCH_APPOINTMENT_SELECT = "Patient.sp_SchAppointmentSelect";
        private const string PROC_SCHEDULING_SEARCH = "Patient.sp_BatchScheduleSelect";
        private const string PROC_LOAD_SCH_SLOT = "Patient.sp_SchSlotSelect";
        private const string PROC_WEEKLY_APP_SELECT = "Patient.sp_WeeklySlotsWithAppointments";
        private const string PROC_MULTIPLE_SLOTS_WITH_APPOINTMENTS = "Patient.sp_MultipleSlotsWithAppointments";
        private const string PROC_CHANGE_SCH_FACILITY = "Provider.SchChangeFacility";
        private const string PROC_WAITLIST_STATUS_LOOKUP = "Patient.sp_WaitListStatusLookUp";
        private const string PROC_PREFERRED_TIME_LOOKUP = "Patient.sp_PreferredTimeLookup";
        private const string PROC_WAITLIST_SELECT = "Patient.sp_WaitListSelect";
        private const string PROC_WAITLIST_INSERT = "Patient.sp_WaitListInsert";
        private const string PROC_WAITLIST_UPDATE = "Patient.sp_WaitListUpdate";
        private const string PROC_WAITLIST_DELETE = "Patient.sp_WaitListDelete";
        private const string PROC_PATIENT_BALANCE_SELECT = "Patient.sp_PatientBalance";
        private const string PROC_DASH_APP_VISIT_SELECT = "Clinical.sp_DashboardVisitSelect";
        private const string PROC_PATIENT_APPOITMENTS_SELECT = "Patient.sp_PatientAppointmentsSelectPatModule";
        private const string PROC_DASH_APP_VISIT_SELECT_COUNT = "Clinical.sp_DashboardVisitSelectCount";
        private const string PROC_APP_VISIT_SELECT = "Clinical.sp_AppointmentNotesSelect";
        private const string PROC_APP_VISIT_SELECT_COUNT = "Clinical.sp_AppointmentNotesSelectCount";
        private const string PROC_APP_VISIT_STATUS_LOOKUP = "Clinical.sp_AppVisitStatusLookup";
        private const string PROC_NOTES_DRAFT_COUNT = "Clinical.sp_NotesDraftCount";
        private const string PROC_PATIENT_TYPE_LOOKUP = "Patient.sp_PatientTypeLookup";
        private const string PROC_PATIENT_VISIT_TYPE_LOOKUP = "Patient.sp_PatientVisitTypeLookup";
        private const string PROC_PATIENT_VISIT_TYPE_WO_CANCER_REGISTRIES_LOOKUP = "Patient.sp_PatientVisitType_WO_CancerRegistriesLookup";
        private const string PROC_VISIT_TYPE_LOOKUP = "Patient.sp_VisitTypeLookup";
        private const string PROC_PATIENT_VISIT_TYPE_DURATION_LOOKUP = "Patient.sp_PatientVisitTypeDurationLookup";
        private const string PROC_APP_SUMMARY = "Patient.sp_AppointmentSummary";
        private const string PROC_WEEK_APP_SELECT = "Patient.sp_DailySlotsWithAppointments_New";
        private const string PROC_BLOCK_APP_SUMMARY = "Patient.sp_BlockedAppointmentSummary";
        private const string PROC_SCHEDULING_FROMTIME_LOOKUP = "Provider.sp_SchedulingFromTimeLookUp";

        private const string PROC_CANCEL_CHECKIN = "Patient.sp_AppointmentCancelCheckIn";
        private const string PROC_EDIELIGIBILITY_ID_SELECT = "Patient.sp_EDIEligibilityIdSelect";
        private const string PROC_DASHBOARD_APP_NOTES_SELECT = "Clinical.sp_D_AppointmentNotesSelect";
        private const string PROC_DASHBOARD_APP_SELECT = "Clinical.sp_D_DashboardVisitSelect";

        private const string PROC_PORTAL_APP_REQUEST_INSERT = "Patient.sp_PortalAppRequestInsert";
        private const string PROC_PORTAL_APP_REQUEST_SELECT = "Patient.sp_PortalAppRequestSelect";
        private const string PROC_PORTAL_APP_REQUEST_UPDATE = "Patient.sp_PortalAppRequestUpdate";

        private const string PROC_PORTAL_APP_REJECT_ACCEPT_MULTIPLE = "Patient.sp_MultiplePortalAppRequestAcceptReject";
        private const string PROC_PATIENT_APPOINTMENT_REFERRAL_UPDATE = "Patient.sp_PatientAppointmentReferralUpdate";
        private const string PROC_PROVIDER_APPOINTMENT_SLOT_INFO = "Patient.ProviderAppointmentSlotInfo";
        private const string PROC_PROVIDER_APPOINTMENT_PRINT = "Patient.sp_ProviderAppointmentPrint";
        private const string PROC_PROVIDER_APPOINTMENT_DAY_VIEW = "Patient.sp_ProvidersAppointmentDayView";
        private const string PROC_PROVIDER_APPOINTMENT_DAY_VIEW_SCHEDULE = "Provider.sp_ProviderBusinessHours";
        private const string PROC_RESOURCE_APPOINTMENT_DAY_VIEW_SCHEDULE = "Provider.sp_ResourcerBusinessHours";
        private const string PROC_RESOURCE_APPOINTMENT_DAY_VIEW = "Patient.sp_ResourceAppointmentDayView";
        private const string PROC_PATIENT_Appointments_InsertNative = "Patient.sp_PatientAppointmentsInsertNative";
        private const string PROC_PATIENT_APPOINTMENT_APPROVE_NATIVE = "mobile.sp_PatientAppointmentsApproveNative";
        private const string PROC_PATIENT_APPOINTMENT_DISCARD_NATIVE = "mobile.sp_PatientAppointmentsDiscardNative";
        private const string PROC_PROVIDER_APPOINTMENT_WEEK_VIEW = "Patient.sp_ProvidersAppointmentWeekView";
        private const string PROC_RESOURCE_APPOINTMENT_WEEK_VIEW = "Patient.sp_ResourceAppointmentWeekView";
        private const string PROC_PROVIDER_APPOINTMENT_WORKWEEK_VIEW = "Patient.sp_ProvidersAppointmentWorkWeekView";
        private const string PROC_PROVIDER_WORK_WEEK_SCHEDULES = "Provider.sp_ProvidersWeeklySchedules";
        private const string PROC_RESOURCE_WORK_WEEK_SCHEDULES = "Provider.sp_ResourcesWeeklySchedules";
        private const string PROC_RESOURCE_APPOINTMENT_WORKWEEK_VIEW = "Patient.sp_ResourceAppointmentWorkWeekView";
        private const string PROC_PROVIDERS_APPOINTMENT_MONTH_VIEW = "[Patient].[sp_ProvidersAppointmentMonthView]";
        private const string PROC_RESOURCES_APPOINTMENT_MONTH_VIEW = "[Patient].[sp_ResourcesAppointmentMonthView]";
        private const string PROC_LOAD_TOOLTIP_DATA = "[Patient].[sp_AppointmentsLoadToolTip]";
        private const string PROC_PATIENT_APPOINTMENTS_COPY_PASTE = "Patient.sp_PatientAppointmentsCopyPaste";
        private const string PROC_PATIENT_APPOINTMENTS_CUT_PASTE = "Patient.sp_PatientAppointmentsCutPaste";
        private const string PROC_APPOINTMENT_STATUS_OPTIONS = "[Patient].[sp_AppointmentStatusOptions]";

        private const string PROC_WAITLIST_SCHEDULESEARCH = "Patient.WaitListAppointmentSelect";
        private const string PROC_APPOINTMENT_HISTORY_UPDATE = "System.sp_DBAuditAppointmentSelect";

        private const string PROC_SCH_BLOCKHOURS_SELECT = "Provider.sp_SchBlockedHours";
        private const string PROC_GETAPPOINTMENT_REFERRALNO = "Patient.sp_GetReferralNoForAppointment";
        private const string PROC_APPOINTMENT_COPAY_ALERT = "Patient.sp_AppointmentCopayAlert";
        private const string PROC_APPOINTMENT_STATUS_LOOKUP = "Provider.sp_AppointmentStatusLookup";

        private const string PROC_GET_BULK_NOTES = "[Clinical].[sp_GetBulkNotes]";
        private const string PROC_UPDATE_APPOINTMENT_COPAY = "Patient.sp_UpdateAppointmentCopayment";
        #endregion

        #region "Parameters"
        private const string PARM_XML_APPOINTMENTS = "@XMLAppointments";
        private const string PARM_PATIENT_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_REJECTION_REASON = "@RejectionReason";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        // private const string PARM_APPOINTMENT_DATE = "@AppointmentDate";
        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_IDs = "@ProviderIds";
        private const string PARM_RESOURCE_IDs = "@ResourceIds";
        private const string PARM_FACILITY_IDs = "@FacilityIds";
        private const string PARM_REFF_PROVIDER_ID = "@RefProviderId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_APPOINTMENT_IDS = "@AppointmentIds";
        private const string PARM_SCHEDULE_REASON_ID = "@SchReasonId";
        private const string PARM_STATUS_ID = "@SchStatusId";
        private const string PARM_TIME_SLOT_ID = "@TimeSlotId";
        private const string PARM_TIME_SLOT_DETAIL_ID = "@TimeSlotDtlId";
        private const string PARM_APP_DETAIL = "@AppDtl";
        private const string PARM_PATIENT_ALLOWED = "@PatientAllowed";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_REFERRAL_NO = "@ReferralNo";
        private const string PARM_PATIENT_BALANCE = "@PatientBalance";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PATTERN_EVERY = "@PatternEvery";
        private const string PARM_VALUE = "@Value";
        private const string PARM_PATTERN_DAYS = "@PatternDays";
        private const string PARM_PATTERN_WEEKS = "@PatternWeeks";
        private const string PARM_PATTERN_MONTHS = "@PatternMonths";
        private const string PARM_END_BY_DATE = "@EndByDate";
        private const string PARM_END_BY_APPOINTMENT = "@EndByAppointment";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SLOT_DATE = "@SlotDate";
        private const string PARM_PASTE_SLOT_DATE = "@PasteTimeSlotDtlId";
        private const string PARM_MONTH_YEAR = "@MonthYear";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_DAYS_OF_WEEK = "@DaysOfWeek";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_FROM_DATE = "@FromDate";
        private const string PARM_TO_DATE = "@ToDate";
        private const string PARM_FROM_TIME = "@FromTime";
        private const string PARM_TO_TIME = "@ToTime";
        private const string PARM_AM_PM = "@AMPM";
        private const string PARM_GET_STATUS = "@GetStatus";
        private const string PARM_GET_DAYS = "@GetDays";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_BLOCK_STATUS = "@BlockStatus";
        private const string PARM_BLOCK_REASON_ID = "@BlockReasonId";
        private const string PARM_SLOT_ID = "@SlotId";
        private const string PARM_COLOR = "@Color";
        private const string PARM_OVER_BOOKED_ALLOWED = "@OverBookAllowed";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_STATUSES_ID = "@StatusId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_EDIELIGIBILITY_ID = "@EDIEligibilityId";
        private const string PARM_MULTIPLE_SLOT_IDS = "@SlotDtlIds";
        private const string PARM_MOVE_FACILITY_ID = "@MoveFacilityId";
        private const string PARM_PROV_RES_BIT = "@PRBit";
        private const string PARM_WAIT_LIST_ID = "@WaitListId";
        private const string PARM_PREFTIME_ID = "@PrfTimeId";
        private const string PARM_PREF_DATE = "@PreferredDate";
        private const string PARM_WAIT_LISTSTATUS_ID = "@WtListStatusId";
        private const string PARM_SCH_REASON_ID = "@ScheduleReasonId";
        private const string PARM_PAN = "@PAN";
        private const string PARM_WTSTATUS_ID = "@WtStatusId";
        private const string PARM_IS_SELECTED = "@IsSelected";
        private const string PARM_IS_PREF_DAY = "@IsPreferredDay";
        private const string PARM_PREF_DAY = "@PreferredDay";
        private const string PARM_ISPAGING = "@IsPaging";
        private const string PARM_IS_SPECIALIST = "@IsSpecialist";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_APPOINTMENT_STATUS = "@AppointmentStatus";
        private const string PARM_APPOINTMENT_DATE = "@AppointmentDate";
        private const string PARM_VIEW_TYPE = "@ViewType";
        private const string PARM_APPOINTMENT_TIME = "@AppointmentTime";
        private const string PARM_IS_CHECKD_IN = "@IsCheckedIn";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_ACCOUNT_NO = "@AccountNumber";
        private const string PARM_REASON_NAME = "@ReasonName";

        private const string PARM_VISIT_FROM = "@VisitFrom";
        private const string PARM_VISIT_TO = "@VisitTo";
        private const string PARM_NOTE_STATUS = "@NoteStatus";
        private const string PARM_IS_DRAFT_NOTE = "@IsDraftNote";

        private const string PARM_NOTE_TYPE = "@NoteType";
        private const string PARM_BILLING_COMMENTS = "@BillingComments";
        private const string PARM_ECW_APPOINTMENT_ID = "@ECWAppointmentID";

        private const string PARM_ACTION = "@Action";
        private const string PARM_IS_REMINDER_SENT = "@IsReminderSent";
        private const string PARM_IS_To_CHECK_REMINDER_SETTING = "@IsToCheckReminderSetting";


        // Start 15/12/2015 Muhammad Irfan For Appointments in FaceSheet
        private const string PARM_IS_FACESHEET = "@IsFaceSheet";
        // End 15/12/2015 Muhammad Irfan For Appointments in FaceSheet
        private const string PARM_PATIENT_TYPE = "@PatientTypeId";
        private const string PARM_PATIENT_VISIT_TYPE = "@VisitTypeId";
        private const string PARM_SCH_DATE = "@ScheduleDate";
        private const string PARM_ISMOVE_NEXTDAY = "@IsMoveToNextDay";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_REFERRAL_ID = "@ReferralId";


        private const string PARM_APP_DATE = "@AppDate";
        private const string PARM_TIMESLOT_ID = "@TimeSlotId";
        private const string PARM_TIMESLOTDTL_ID = "@TimeSlotDtlId";
        private const string PARM_REQUEST_STATUS = "@RequestStatus";
        private const string PARM_SCHREASON_ID = "@SchReasonId";
        private const string PARM_SCHREASON_NAME = "@SchReasonName";
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_PORTAL_APPREQUEST_ID = "@PortalAppRequestId";
        private const string PARM_COPAYMENT = "@Copayment";
        private const string PARM_PROV_RES_SCH_ID = "@ProvResSchId";
        private const string PARM_IS_NON_BILABLE = "@IsNonBilable";
        private const string PARM_IS_FROM_FOLLOW_UP = "@FromFollowUp";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_FOLLOWUP_APPOINTMENT_NOTES_ID = "@FollowUpAppointmentNotesId";
        private const string PARM_VISIT_STATUS = "@VisitStatus";

        private const string PARM_REASON_COMMENTS = "@ReasonComments";
        private const string PARM_CHECKIN_REASON = "@CheckInWOInsReason";
        private const string PARM_CANCELLATION_REASON = "@CancellationReason";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_IS_ALL_VISIT_USED = "@IsAllVisitUsed";

        private const string PARM_REASON_COMMENT_TYPE = "@ReasonCommentType";
        private const string PARM_ICD10_CODE = "@ICD10";
        private const string PARM_ICD10_CODE_DESCRIPTION = "@ICD10_DESCRIPTION";
        private const string PARM_SNOMED_CODE = "@SNOMEDID";
        private const string PARM_SNOMED_CODE_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_PROVIDERS_TABLE = "@Providers";
        private const string PARM_VISITTYPES_TABLE = "@VisitTypes";
        private const string PARM_FACILITYS_TABLE = "@Facilitys";
        private const string PARM_SCHEDULE_DATE = "@ScheduleDate";
        private const string PARM_APPOINTMENT_STATUSES_TABLE = "@AppointmentStatuses";
        private const string PARM_MONTH = "@Month";
        private const string PARM_YEAR = "@Year";
        private const string PARM_RESOURCES_TABLE = "@Resources";
        private const string PARM_PREFERRED_DATE = "@PreferredDate";
        private const string PARM_IS_PROVIDER = "@IsProvider";
        private const string PARM_APPOINTMENT_STATUS_ID = "@AppointmentStatusId";
        private const string PARM_IS_RESCHEDULE = "@IsReschedule";
        private const string PARM_CREATED_DATE = "@CreatedDate";

        private const string PARM_FROM_VIEW_TYPE = "@Type";
        private const string PARM_APPOINTMENT_STATUS_IDS = "@AppointmentStatusIds";
        private const string PARM_SCHEDULING_STATUSES_TABLE = "@SchedulingStatusIds";

        private const string PARM_APPOINTMENT_DATES = "@AppointmentDates";
        private const string PARM_IS_COPAY_ALERT = "@IsCopayAlert";
        private const string PARM_IS_READY_OR_MISSING = "@IsReadyOrMissing";
        private const string PARM_MISSING_INFO = "@BillingInfo";
        private const string PARM_PATIENT_REFFRAL_ID = "@PatientReferralId";

        #endregion

        #region Constructors
        public DALAppointment()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALAppointment(SharedVariable SharedVariable)
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

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSAppointment ds, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(42);
                dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, ds.PatientAppointments.AppointmentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientAppointments.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.PatientAppointments.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(3, PARM_RESOURCE_ID, ds.PatientAppointments.ResourceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_FACILITY_ID, ds.PatientAppointments.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(5, PARM_REFF_PROVIDER_ID, ds.PatientAppointments.RefProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, PARM_PATIENT_INSURANCE_ID, ds.PatientAppointments.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(7, PARM_SCHEDULE_REASON_ID, ds.PatientAppointments.SchReasonIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, PARM_STATUS_ID, ds.PatientAppointments.SchStatusIdColumn.ColumnName, DbType.Int64); ;
                dbManager.AddParameters(9, PARM_DURATION, ds.PatientAppointments.DurationColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(10, PARM_REFERRAL_NO, ds.PatientAppointments.ReferralNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, PARM_PATIENT_BALANCE, ds.PatientAppointments.PatientBalanceColumn.ColumnName, DbType.Double);
                dbManager.AddParameters(12, PARM_COMMENTS, ds.PatientAppointments.CommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.PatientAppointments.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(14, PARM_CREATED_BY, ds.PatientAppointments.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(15, PARM_CREATED_ON, ds.PatientAppointments.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(16, PARM_MODIFIED_BY, ds.PatientAppointments.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(17, PARM_MODIFIED_ON, ds.PatientAppointments.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(18, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(19, PARM_WAIT_LIST_ID, ds.PatientAppointments.WaitListIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(20, PARM_IS_SPECIALIST, ds.PatientAppointments.IsSpecialistColumn.ColumnName, DbType.Byte);
                //dbManager.AddParameters(21, PARM_ECW_APPOINTMENT_ID, ds.PatientAppointments.ECWAppointmentIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(21, PARM_BILLING_COMMENTS, ds.PatientAppointments.BillingCommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(22, PARM_PATIENT_TYPE, ds.PatientAppointments.PatientTypeIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(23, PARM_PATIENT_VISIT_TYPE, ds.PatientAppointments.VisitTypeIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(24, PARM_IS_REMINDER_SENT, ds.PatientAppointments.IsReminderSentColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(25, PARM_REFERRAL_ID, ds.PatientAppointments.ReferralIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(26, PARM_COPAYMENT, ds.PatientAppointments.CopaymentColumn.ColumnName, DbType.Double);
                dbManager.AddParameters(27, PARM_IS_NON_BILABLE, ds.PatientAppointments.IsNonBilableColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(28, PARM_REASON_COMMENTS, ds.PatientAppointments.ReasonCommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(29, PARM_APP_DATE, ds.PatientAppointments.AppointmentDateColumn.ColumnName, DbType.String);
                dbManager.AddParameters(30, PARM_TO_TIME, ds.PatientAppointments.TimeToColumn.ColumnName, DbType.String);
                dbManager.AddParameters(31, PARM_FROM_TIME, ds.PatientAppointments.TimeFromColumn.ColumnName, DbType.String);
                dbManager.AddParameters(32, PARM_REASON_COMMENT_TYPE, ds.PatientAppointments.ReasonCommentsTypeNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(33, PARM_ICD10_CODE, ds.PatientAppointments.ICDCode10Column.ColumnName, DbType.String);
                dbManager.AddParameters(34, PARM_ICD10_CODE_DESCRIPTION, ds.PatientAppointments.ICDCode10DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARM_SNOMED_CODE, ds.PatientAppointments.SnomedCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(36, PARM_SNOMED_CODE_DESCRIPTION, ds.PatientAppointments.SnomedCodeDescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(37, PARM_IS_COPAY_ALERT, ds.PatientAppointments.IsCopayAlertColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(38, PARM_IS_FROM_FOLLOW_UP, ds.PatientAppointments.FromFollowUpColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(39, PARM_NOTES_ID, ds.PatientAppointments.NotesIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(40, "@CancellationReason", ds.PatientAppointments.CancellationReasonColumn.ColumnName, DbType.String);
                dbManager.AddParameters(41, PARM_PATIENT_REFFRAL_ID, ds.PatientAppointments.PatientReferralIdColumn.ColumnName, DbType.Int64);

            }
            else
            {
                dbManager.CreateParameters(42);
                dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, ds.PatientAppointments.AppointmentIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientAppointments.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.PatientAppointments.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(3, PARM_RESOURCE_ID, ds.PatientAppointments.ResourceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_FACILITY_ID, ds.PatientAppointments.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(5, PARM_REFF_PROVIDER_ID, ds.PatientAppointments.RefProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, PARM_PATIENT_INSURANCE_ID, ds.PatientAppointments.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(7, PARM_SCHEDULE_REASON_ID, ds.PatientAppointments.SchReasonIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, PARM_STATUS_ID, ds.PatientAppointments.SchStatusIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(9, PARM_PATIENT_ALLOWED, ds.PatientAppointments.PatientAllowedColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(10, PARM_DURATION, ds.PatientAppointments.DurationColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(11, PARM_REFERRAL_NO, ds.PatientAppointments.ReferralNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(12, PARM_PATIENT_BALANCE, ds.PatientAppointments.PatientBalanceColumn.ColumnName, DbType.Double);
                dbManager.AddParameters(13, PARM_COMMENTS, ds.PatientAppointments.CommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.PatientAppointments.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(15, PARM_CREATED_BY, ds.PatientAppointments.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(16, PARM_CREATED_ON, ds.PatientAppointments.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.PatientAppointments.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.PatientAppointments.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(19, PARM_IS_SPECIALIST, ds.PatientAppointments.IsSpecialistColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(20, PARM_APPOINTMENT_STATUS, ds.PatientAppointments.AppointmentStatusColumn.ColumnName, DbType.String);
                //dbManager.AddParameters(21, PARM_ECW_APPOINTMENT_ID, ds.PatientAppointments.ECWAppointmentIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(21, PARM_BILLING_COMMENTS, ds.PatientAppointments.BillingCommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(22, PARM_PATIENT_TYPE, ds.PatientAppointments.PatientTypeIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(23, PARM_PATIENT_VISIT_TYPE, ds.PatientAppointments.VisitTypeIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(24, PARM_IS_REMINDER_SENT, ds.PatientAppointments.IsReminderSentColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(25, PARM_REFERRAL_ID, ds.PatientAppointments.ReferralIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(26, PARM_COPAYMENT, ds.PatientAppointments.CopaymentColumn.ColumnName, DbType.Double);
                dbManager.AddParameters(27, PARM_IS_NON_BILABLE, ds.PatientAppointments.IsNonBilableColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(28, PARM_REASON_COMMENTS, ds.PatientAppointments.ReasonCommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(29, PARM_CHECKIN_REASON, ds.PatientAppointments.CheckInReasonColumn.ColumnName, DbType.String);
                dbManager.AddParameters(30, PARM_APP_DATE, ds.PatientAppointments.AppointmentDateColumn.ColumnName, DbType.String);
                dbManager.AddParameters(31, PARM_TO_TIME, ds.PatientAppointments.TimeToColumn.ColumnName, DbType.String);
                dbManager.AddParameters(32, PARM_FROM_TIME, ds.PatientAppointments.TimeFromColumn.ColumnName, DbType.String);
                dbManager.AddParameters(33, PARM_REASON_COMMENT_TYPE, ds.PatientAppointments.ReasonCommentsTypeNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(34, PARM_ICD10_CODE, ds.PatientAppointments.ICDCode10Column.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARM_ICD10_CODE_DESCRIPTION, ds.PatientAppointments.ICDCode10DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(36, PARM_SNOMED_CODE, ds.PatientAppointments.SnomedCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(37, PARM_SNOMED_CODE_DESCRIPTION, ds.PatientAppointments.SnomedCodeDescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(38, PARM_IS_RESCHEDULE, ds.PatientAppointments.IsRescheduleColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(39, PARM_IS_COPAY_ALERT, ds.PatientAppointments.IsCopayAlertColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(40, "@CancellationReason", ds.PatientAppointments.CancellationReasonColumn.ColumnName, DbType.String);
                dbManager.AddParameters(41, PARM_PATIENT_REFFRAL_ID, ds.PatientAppointments.PatientReferralIdColumn.ColumnName, DbType.Int64);


            }

        }
        private void CreateVisitParameters(IDBManager dbManager, DSVisits ds, Boolean IsInsert)
        {


            dbManager.CreateParameters(4);
            dbManager.AddParameters(0, PARM_VISIT_ID, ds.PatientVisits.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_APPOINTMENT_ID, ds.PatientVisits.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_VISIT_STATUS, ds.PatientVisits.VisitStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MODIFIED_BY, ds.PatientVisits.ModifiedByColumn.ColumnName, DbType.String);

        }

        private void CreateStatusUpdateParameters(IDBManager dbManager, DSAppointment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(33);
            dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, ds.PatientAppointments.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientAppointments.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.PatientAppointments.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_RESOURCE_ID, ds.PatientAppointments.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_FACILITY_ID, ds.PatientAppointments.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_REFF_PROVIDER_ID, ds.PatientAppointments.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_PATIENT_INSURANCE_ID, ds.PatientAppointments.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_SCHEDULE_REASON_ID, ds.PatientAppointments.SchReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_STATUS_ID, ds.PatientAppointments.SchStatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_PATIENT_ALLOWED, ds.PatientAppointments.PatientAllowedColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_DURATION, ds.PatientAppointments.DurationColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_REFERRAL_NO, ds.PatientAppointments.ReferralNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PATIENT_BALANCE, ds.PatientAppointments.PatientBalanceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(13, PARM_COMMENTS, ds.PatientAppointments.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.PatientAppointments.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.PatientAppointments.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.PatientAppointments.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.PatientAppointments.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.PatientAppointments.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_IS_SPECIALIST, ds.PatientAppointments.IsSpecialistColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(20, PARM_APPOINTMENT_STATUS, ds.PatientAppointments.AppointmentStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_ECW_APPOINTMENT_ID, ds.PatientAppointments.ECWAppointmentIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(22, PARM_BILLING_COMMENTS, ds.PatientAppointments.BillingCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_PATIENT_TYPE, ds.PatientAppointments.PatientTypeIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(24, PARM_PATIENT_VISIT_TYPE, ds.PatientAppointments.VisitTypeIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(25, PARM_IS_REMINDER_SENT, ds.PatientAppointments.IsReminderSentColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(26, PARM_REFERRAL_ID, ds.PatientAppointments.ReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(27, PARM_COPAYMENT, ds.PatientAppointments.CopaymentColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(28, PARM_IS_NON_BILABLE, ds.PatientAppointments.IsNonBilableColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(29, PARM_REASON_COMMENTS, ds.PatientAppointments.ReasonCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_CHECKIN_REASON, ds.PatientAppointments.CheckInReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_CANCELLATION_REASON, ds.PatientAppointments.CancellationReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_IS_ALL_VISIT_USED, ds.PatientAppointments.IsAllVisitUsedColumn.ColumnName, DbType.Boolean);
        }

        private void CreateWaitListParameters(IDBManager dbManager, DSAppointment ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(22);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_WAIT_LIST_ID, ds.WaitList.WaitListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_WAIT_LIST_ID, ds.WaitList.WaitListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.WaitList.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.WaitList.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.WaitList.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_RESOURCE_ID, ds.WaitList.ResourceIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_SCH_REASON_ID, ds.WaitList.ScheduleReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_FROM_DATE, ds.WaitList.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_TO_DATE, ds.WaitList.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_PREF_DATE, ds.WaitList.PreferredDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_REFF_PROVIDER_ID, ds.WaitList.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_PAN, ds.WaitList.PANColumn.ColumnName, DbType.String);

            dbManager.AddParameters(11, PARM_WTSTATUS_ID, ds.WaitList.WtStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARM_PREFTIME_ID, ds.WaitList.PrfTimeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_COMMENTS, ds.WaitList.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.WaitList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.WaitList.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.WaitList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.WaitList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.WaitList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(19, PARM_IS_SELECTED, ds.WaitList.IsSelectedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_IS_PREF_DAY, ds.WaitList.IsPreferredDayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_PREF_DAY, ds.WaitList.PreferredDayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_REASON_NAME, ds.WaitList.ReasonNameColumn.ColumnName, DbType.String);


        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public bool CheckifCancelledAppointmentExists(long PatientId, long FacilityId, string AppDate, string FromTime, string ToTime, long ProviderId, long ResourceId = 0)
        {
            bool IfExists = false;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PatientId", PatientId));
                parameters.Add(new SqlParameter("@ProviderId", ProviderId));
                parameters.Add(new SqlParameter("@FacilityId", FacilityId));
                parameters.Add(new SqlParameter("@AppDate", AppDate));
                parameters.Add(new SqlParameter("@ToTime", ToTime));
                parameters.Add(new SqlParameter("@FromTime", FromTime));
                using (var reader = dbManager.ExecuteReader("[Patient].[sp_CheckifCancelledAppointmentExists]", parameters))
                {
                    while (reader.Read())
                    {
                        IfExists = Convert.ToInt64(reader["AppointmentId"]) == 0 ? false : true;
                    }
                }
                return IfExists;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointments::CheckifCancelledAppointmentExists", "[Clinical].[sp_CheckifCancelledAppointmentExists]", ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return IfExists;
        }
        public DSAppointment InsertAppointment(DSAppointment ds)
        {
            DALUsersActivity obj = new DALUsersActivity();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                ds = (DSAppointment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_INSERT, ds, ds.PatientAppointments.TableName);
                ds.AcceptChanges();

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAuditAppointmentHistory(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_Appointment, true, "Insert Appointment", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                dbManager.CommitTransaction();

                return ds;
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_Appointment, false, "Error during insert Appointment : " + ex.ToString());
                MDVLogger.DALErrorLog("DALAppointment::InsertAppointment", PROC_PATIENT_APPOINTMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LoadProviderAppointmentSlotInfo(long ProviderId, long FacilityId, string AppointmentDate, long Duration)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(2, PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(3, PARM_DURATION, Duration);
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_APPOINTMENT_SLOT_INFO, ds, ds.TimeSlotsDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentSlotInfo", PROC_PROVIDER_APPOINTMENT_SLOT_INFO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSAppointment LoadAppointment(long AppointmentId, string PatientId, string ProviderId, string ResourceId, string FacilityId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (PatientId == "")
                    PatientId = null;

                if (ProviderId == "")
                    ProviderId = null;

                if (ResourceId == "")
                    ResourceId = null;

                if (FacilityId == "")
                    FacilityId = null;

                dbManager.Open();
                dbManager.CreateParameters(5);

                if (AppointmentId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(3, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(4, PARM_FACILITY_ID, FacilityId);


                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_SELECT, ds, ds.PatientAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointment", PROC_PATIENT_APPOINTMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LoadAppointmentCheckOutSelect(long AppointmentId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (AppointmentId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentId);



                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_CHECKOUT_SELECT, ds, ds.AppointmentCheckout.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentCheckOutSelect", PROC_PATIENT_APPOINTMENT_CHECKOUT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSDBAudit LoadAppointmentHistory(long AppointmentId, DateTime? CreatedOn, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSDBAudit ds = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(5);


                dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_CREATED_DATE, CreatedOn);
                if (PageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PERPAGE, RowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.DBAuditAppointment.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_HISTORY_UPDATE, ds, ds.DBAuditAppointment.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointment", PROC_APPOINTMENT_HISTORY_UPDATE, ex);
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
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="AppointmentId"></param>
        /// <param name="PatientId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="ResourceId"></param>
        /// <param name="FacilityId"></param>
        /// <returns></returns>
        public DSAppointment LoadAppointment(SharedVariable SharedVariable, long AppointmentId, string PatientId, string ProviderId, string ResourceId, string FacilityId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (PatientId == "")
                    PatientId = null;

                if (ProviderId == "")
                    ProviderId = null;

                if (ResourceId == "")
                    ResourceId = null;

                if (FacilityId == "")
                    FacilityId = null;

                dbManager.Open();
                dbManager.CreateParameters(5);

                if (AppointmentId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(3, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(4, PARM_FACILITY_ID, FacilityId);


                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_SELECT, ds, ds.PatientAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALAppointment::LoadAppointment", PROC_PATIENT_APPOINTMENT_SELECT, ex);
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
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="AppointmentIds"></param>
        /// <param name="IsReminderSent"></param>
        /// <param name="IsCheckToReminderSetting"></param>
        /// <returns></returns>
        public DSAppointment LoadAppointment(SharedVariable SharedVariable, string AppointmentIds, string IsReminderSent = "", bool IsCheckToReminderSetting = false)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(AppointmentIds))
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentIds);

                if (string.IsNullOrEmpty(IsReminderSent))
                    dbManager.AddParameters(1, PARM_IS_REMINDER_SENT, null);
                else
                    dbManager.AddParameters(1, PARM_IS_REMINDER_SENT, IsReminderSent);

                dbManager.AddParameters(2, PARM_IS_To_CHECK_REMINDER_SETTING, IsCheckToReminderSetting);


                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_LOAD, ds, ds.PatientAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALAppointment::LoadAppointment", PROC_PATIENT_APPOINTMENT_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LoadAppointment(string AppointmentIds, string IsReminderSent = "", bool IsCheckToReminderSetting = false)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(AppointmentIds))
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentIds);

                if (string.IsNullOrEmpty(IsReminderSent))
                    dbManager.AddParameters(1, PARM_IS_REMINDER_SENT, null);
                else
                    dbManager.AddParameters(1, PARM_IS_REMINDER_SENT, IsReminderSent);

                dbManager.AddParameters(2, PARM_IS_To_CHECK_REMINDER_SETTING, IsCheckToReminderSetting);


                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_LOAD, ds, ds.PatientAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointment", PROC_PATIENT_APPOINTMENT_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LoadAppointmentSummary(ref DataTable dtFacilityIds, ref DataTable ProviderIds, string FromDate, string ToDate, ref DataTable ResourceIds)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (FromDate == "")
                    FromDate = null;
                if (ToDate == "")
                    ToDate = null;
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_FACILITY_IDs, dtFacilityIds);
                dbManager.AddParameters(1, PARM_PROVIDER_IDs, ProviderIds);
                dbManager.AddParameters(2, PARM_FROM_DATE, FromDate);
                dbManager.AddParameters(3, PARM_TO_DATE, ToDate);
                dbManager.AddParameters(4, PARM_RESOURCE_IDs, ResourceIds);
                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APP_SUMMARY, ds, ds.AppointmentSummary.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentSummary", PROC_APP_SUMMARY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment UpdateAppointment(DSAppointment ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_UPDATE, ds, ds.PatientAppointments.TableName);
                if (dtTemp != null && ds.PatientAppointments.Rows[0][ds.PatientAppointments.IsRescheduleColumn].ToString() != "True")
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAuditAppointmentHistory(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_Appointment, true, "Update Appointment", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointment", PROC_PATIENT_APPOINTMENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment UpdateAppointmentStatus(DSAppointment ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                dbManager.Open();
                this.CreateStatusUpdateParameters(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_STATUS_UPDATE, ds, ds.PatientAppointments.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAuditAppointmentHistory(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_Appointment, true, "Update Appointment", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointmentStatus", PROC_PATIENT_APPOINTMENT_STATUS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment UpdateAppointmentCancellationReason(DSAppointment ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, ds.PatientAppointments.AppointmentIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@CancellationReason", ds.PatientAppointments.CancellationReasonColumn.ColumnName, DbType.String);

                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_CANCELLATIONREASON_UPDATE, ds, ds.PatientAppointments.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAuditAppointmentHistory(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_Appointment, true, "Update Appointment", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointmentCancellationReason", PROC_PATIENT_APPOINTMENT_CANCELLATIONREASON_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAppointment UpdateAppointmentwithTrns(DSAppointment ds, IDBManager dbManager)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            //IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                //dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_UPDATE, ds, ds.PatientAppointments.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_Appointment, true, "Update Appointment", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                // dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointment", PROC_PATIENT_APPOINTMENT_UPDATE, ex);
                throw ex;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        public DSAppointment UpdateAppointmentStatuswithTrns(DSAppointment ds, IDBManager dbManager)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            //IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                //dbManager.Open();
                this.CreateStatusUpdateParameters(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_STATUS_UPDATE, ds, ds.PatientAppointments.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAuditAppointmentHistory(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_Appointment, true, "Update Appointment", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                // dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointment", PROC_PATIENT_APPOINTMENT_STATUS_UPDATE, ex);
                throw ex;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        public DSVisits UpdateVisit(DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                this.CreateVisitParameters(dbManager, ds, false);
                ds = (DSVisits)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_UPDATE, ds, ds.PatientVisits.TableName);
                //ds.AcceptChanges();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateVisit", PROC_PATIENT_VISIT_UPDATE, ex);
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
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSAppointment UpdateAppointment(SharedVariable SharedVariable, DSAppointment ds)
        {
            DALUsersActivity obj = new DALUsersActivity(SharedVariable);
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                DataTable dtTemp = ds.PatientAppointments.GetChanges();
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_UPDATE, ds, ds.PatientAppointments.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit(SharedVariable).InsertDBAudit(SharedVariable, dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                obj.InsertUsersActivityLog(SharedVariable, DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_Appointment, true, "Update Appointment from Windows service", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //ds.AcceptChanges();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALAppointment::UpdateAppointment from Windows service", PROC_PATIENT_APPOINTMENT_UPDATE, ex);
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
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string UpdateAppointmentStatusFromReminder(SharedVariable SharedVariable, string XMLAppointments)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_XML_APPOINTMENTS, XMLAppointments);
                dbManager.AddParameters(1, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(SharedVariable.UserName));
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_FROM_REMINDER_UPDATE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALAppointment::UpdateAppointmentStatusFromReminder from Windows service", PROC_APPOINTMENT_STATUS_FROM_REMINDER_UPDATE, ex);
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

        public DSScheduleSetup LoadScheduleAppReasonDuration(long ScheduleReasonId)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ScheduleReasonId <= 0)
                    dbManager.AddParameters(0, PARM_SCHEDULE_REASON_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SCHEDULE_REASON_ID, ScheduleReasonId);

                //dbManager.AddParameters(1, PARM_SCHEDULE_SCHEDULE_REASON, ScheduleReasonId);
                //dbManager.AddParameters(2, PARM_SCHEDULE_FACILITY_ID, FacilityId);

                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_REASON_DURATION, ds, ds.ScheduleReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadScheduleAppReasonDuration", PROC_SCHEDULE_REASON_DURATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment UpdateAppointmentReferral(long appointmentId, long referralId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSAppointment ds = new DSAppointment();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (appointmentId <= 0)
                    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, appointmentId);

                if (referralId <= 0)
                    dbManager.AddParameters(1, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_REFERRAL_ID, referralId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_REFERRAL_UPDATE, ds, ds.PatientAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointmentReferral", PROC_PATIENT_APPOINTMENT_REFERRAL_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Load Provider ALL appointment on multiple facilites
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="AppointmentDate"></param>
        /// <returns></returns>
        public List<ProviderAppointmentPrint> LoadProviderAppointmentPrint(string ProviderId, string FacilityId, string AppointmentDate, DataTable dtAppointmentStatus, string ResourceId)
        {
            List<ProviderAppointmentPrint> providerAppointmentList = new List<ProviderAppointmentPrint>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (string.IsNullOrEmpty(ProviderId))
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                if (string.IsNullOrEmpty(FacilityId))
                    dbManager.AddParameters(PARM_FACILITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(PARM_SCHEDULING_STATUSES_TABLE, dtAppointmentStatus);
                if (string.IsNullOrEmpty(ResourceId))
                    dbManager.AddParameters(PARM_RESOURCE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_RESOURCE_ID, ResourceId);

                providerAppointmentList = dbManager.ExecuteReaders<ProviderAppointmentPrint>(PROC_PROVIDER_APPOINTMENT_PRINT);
                return providerAppointmentList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentPrint", PROC_PROVIDER_APPOINTMENT_PRINT, ex);
                throw ex;
            }

        }
        public List<AppointmentCopayAlert> LoadAppointmentAlert(string VisitId)
        {
            List<AppointmentCopayAlert> providerAppointmentList = new List<AppointmentCopayAlert>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_VISIT_ID, VisitId);
                providerAppointmentList = dbManager.ExecuteReaders<AppointmentCopayAlert>(PROC_APPOINTMENT_COPAY_ALERT);
                return providerAppointmentList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentAlert", PROC_APPOINTMENT_COPAY_ALERT, ex);
                throw ex;
            }

        }
        public List<AppointmentModel> GetAppointmentReferralNo(string ProviderId, string FacilityId, string PatientInsuranceId, string PatientId, string AppointmentDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, MDVUtility.ToLong(ProviderId));
                if (string.IsNullOrEmpty(FacilityId))
                {
                    dbManager.AddParameters(1, PARM_FACILITY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_FACILITY_ID, MDVUtility.ToLong(FacilityId));
                }
                dbManager.AddParameters(2, PARM_PATIENT_INSURANCE_ID, MDVUtility.ToLong(PatientInsuranceId));
                dbManager.AddParameters(3, PARM_PATIENT_ID, MDVUtility.ToLong(PatientId));
                dbManager.AddParameters(4, PARM_APPOINTMENT_DATE, MDVUtility.ToDateTime(AppointmentDate));

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GETAPPOINTMENT_REFERRALNO);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.ReferringFromId = !string.IsNullOrEmpty(reader["ReferringFromId"].ToString()) ? MDVUtility.ToLong(reader["ReferringFromId"]) : 0;
                    model.ReferringFromName = !string.IsNullOrEmpty(reader["ReferringFromName"].ToString()) ? MDVUtility.ToStr(reader["ReferringFromName"]) : "";
                    model.ReferralAuthNo = !string.IsNullOrEmpty(reader["ReferralAuthNo"].ToString()) ? MDVUtility.ToStr(reader["ReferralAuthNo"]) : "";
                    model.RecordCount = !string.IsNullOrEmpty(reader["RecordCount"].ToString()) ? MDVUtility.ToStr(reader["RecordCount"]) : "";
                    if (model.ReferralAuthNo != "")
                        model.PatientReferralId = !string.IsNullOrEmpty(reader["PatientReferralId"].ToString()) ? MDVUtility.ToStr(reader["PatientReferralId"]) : "";

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::GetAppointmentReferralNo", PROC_GETAPPOINTMENT_REFERRALNO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Appointment Slots Search"

        public DSAppointment SearchDailySlots(long ProviderId, long ResourceId, long FacilityId, string SlotDate, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //if (StatusId == "")
                //    StatusId = null;
                //if (PatientTypeId == "")
                //    PatientTypeId = null;
                //if (VisitTypeId == "")
                //    VisitTypeId = null;
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (ResourceId <= 0)
                    dbManager.AddParameters(1, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_RESOURCE_ID, ResourceId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                dbManager.AddParameters(3, PARM_SLOT_DATE, SlotDate);

                dbManager.AddParameters(4, PARM_STATUSES_ID, StatusId);
                if (MDVUtility.ToInt64(PatientTypeId) <= 0)
                    dbManager.AddParameters(5, PARM_PATIENT_TYPE, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_TYPE, MDVUtility.ToInt64(PatientTypeId));
                if (MDVUtility.ToInt64(VisitTypeId) <= 0)
                    dbManager.AddParameters(6, PARM_PATIENT_VISIT_TYPE, null);
                else
                    dbManager.AddParameters(6, PARM_PATIENT_VISIT_TYPE, MDVUtility.ToInt64(VisitTypeId));



                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_DAYSLOT_SEARCH, ds, ds.DaySlots.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::SearchDailySlots", PROC_APPOINTMENT_DAYSLOT_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Cut and Paste Appointments"
        public string MoveAppointment(string AppointmentId, string PasteTimeSlotDtlId, string NewDate, string NewTime, string IsMoveToNextDay = "0")
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (PasteTimeSlotDtlId == "")
                {
                    PasteTimeSlotDtlId = null;
                }
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_PASTE_SLOT_DATE, PasteTimeSlotDtlId);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(4, PARM_ISMOVE_NEXTDAY, IsMoveToNextDay);
                dbManager.AddParameters(5, PARM_APPOINTMENT_DATE, NewDate);
                dbManager.AddParameters(6, PARM_APPOINTMENT_TIME, NewTime);

                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_APPOINTMENT_MOVE);

                if (returnVal != null)
                    throw new Exception(returnVal.ToString());

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::MoveAppointment", PROC_APPOINTMENT_MOVE, ex);
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

        #region "Update Slot Status"
        public string UpdateSlotStatus(string SlotId, long BlockReasonId, string BlockStatus, string Comments)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_SLOT_ID, SlotId);
                if (BlockReasonId <= 0)
                    dbManager.AddParameters(1, PARM_BLOCK_REASON_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BLOCK_REASON_ID, BlockReasonId);
                dbManager.AddParameters(2, PARM_BLOCK_STATUS, BlockStatus);
                dbManager.AddParameters(3, PARM_COMMENTS, Comments);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SLOT_STATUS_UPDATE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateSlotStatus", PROC_SLOT_STATUS_UPDATE, ex);
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

        #region "Monthly Appointment"

        public DSAppointment SelectMonthlyAppointment(long ProviderId, long FacilityId, long ResourceId, string MonthYear, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (StatusId == "")
                    StatusId = null;
                if (PatientTypeId == "")
                    PatientTypeId = null;
                if (VisitTypeId == "")
                    VisitTypeId = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId <= 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(3, PARM_MONTH_YEAR, MonthYear);

                dbManager.AddParameters(4, PARM_STATUSES_ID, StatusId);
                dbManager.AddParameters(5, PARM_PATIENT_TYPE, PatientTypeId);
                dbManager.AddParameters(6, PARM_PATIENT_VISIT_TYPE, VisitTypeId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MONTHLY_APPOINTMENT, ds, ds.MonthlyAppointment.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::SelectMonthlyAppointment", PROC_MONTHLY_APPOINTMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Weekly Appointment"

        public DSAppointment SelectWeeklyAppointment(long ProviderId, long FacilityId, long ResourceId, string StartDate, string EndDate, string DaysOfWeek)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(6);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId <= 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(3, PARM_START_DATE, StartDate);
                dbManager.AddParameters(4, PARM_END_DATE, EndDate);
                dbManager.AddParameters(5, PARM_DAYS_OF_WEEK, DaysOfWeek);





                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_WEEKLY_APPOINTMENT, ds, ds.WeeklyAppointment.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::SelectWeeklyAppointment", PROC_WEEKLY_APPOINTMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Batch Scheduling Select"

        public DSAppointment SchedulingSearch(long PatientId, long FacilityId, long PracticeId, long ProviderId, long ResourceId, string FromDate, string ToDate, string FromTime, string ToTime, string AMPM, string GetStatus, string GetDays, string prresbit, int PageNumber, int RowspPage)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FromDate == "")
                    FromDate = null;
                if (ToDate == "")
                    ToDate = null;
                if (FromTime == "")
                    FromTime = null;
                if (ToTime == "")
                    ToTime = null;
                if (AMPM == "")
                    AMPM = null;
                if (GetStatus == "")
                    GetStatus = null;
                if (GetDays == "")
                    GetDays = null;

                dbManager.Open();
                dbManager.CreateParameters(17);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);

                if (PracticeId == 0)
                    dbManager.AddParameters(2, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PRACTICE_ID, PracticeId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                if (ResourceId == 0)
                    dbManager.AddParameters(4, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_RESOURCE_ID, ResourceId);

                dbManager.AddParameters(5, PARM_FROM_DATE, FromDate);
                dbManager.AddParameters(6, PARM_TO_DATE, ToDate);
                dbManager.AddParameters(7, PARM_FROM_TIME, FromTime);
                dbManager.AddParameters(8, PARM_TO_TIME, ToTime);
                dbManager.AddParameters(9, PARM_AM_PM, AMPM);
                dbManager.AddParameters(10, PARM_GET_STATUS, GetStatus);
                dbManager.AddParameters(11, PARM_GET_DAYS, GetDays);

                dbManager.AddParameters(12, PARM_PROV_RES_BIT, prresbit);

                if (PageNumber == 0)
                    dbManager.AddParameters(13, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(13, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(14, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(14, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(15, PARM_RECORD_COUNT, ds.TimeSlotsDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(16, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULING_SEARCH, ds, ds.TimeSlotsDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::SchedulingSearch", PROC_SCHEDULING_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Schedule Appointment Load"

        public DSAppointment LoadSchAppointment(long ProviderId, long FacilityId, string SlotDate, string Color, long TimeSlotDtlId, long ResourceId, int PageNumber = 0, int RowspPage = 0, int IsPaging = 0)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(12);
                DateTime? dt = null;
                if (SlotDate == "")
                    SlotDate = null;
                else
                    dt = DateTime.Parse(SlotDate);

                if (Color == "")
                    Color = null;

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);


                dbManager.AddParameters(2, PARM_SLOT_DATE, dt);

                dbManager.AddParameters(3, PARM_COLOR, Color);

                if (TimeSlotDtlId == 0)
                    dbManager.AddParameters(4, PARM_TIME_SLOT_DETAIL_ID, null);
                else
                    dbManager.AddParameters(4, PARM_TIME_SLOT_DETAIL_ID, TimeSlotDtlId);
                if (ResourceId == 0)
                    dbManager.AddParameters(5, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_RESOURCE_ID, ResourceId);

                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(7, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.SchAppointment.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                dbManager.AddParameters(9, PARM_ISPAGING, IsPaging);

                dbManager.AddParameters(10, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCH_APPOINTMENT_SELECT, ds, ds.SchAppointment.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadSchAppointment", PROC_SCH_APPOINTMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Patient Appointment Delete"
        public string DeletePatAppointment(string AppointmentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DSAppointment ds = LoadAppointment(MDVUtility.ToInt64(AppointmentId), "", "", "", "");
                DataTable dtTemp = ds.PatientAppointments;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    if (dtTemp != null && ds.PatientAppointments.Rows.Count > 0)
                    {
                        dtTemp.Columns.Remove("CreatedBy");
                        dtTemp.Columns.Remove("CreatedOn");
                        dtTemp.Columns.Remove("ModifiedBy");
                        dtTemp.Columns.Remove("ModifiedOn");

                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientAppointments.Rows[0][ds.PatientAppointments.AppointmentIdColumn].ToString(), null, "", false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::DeletePatientAppointment", PROC_PATIENT_APPOINTMENT_DELETE, ex);
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

        #region "Update Slot"
        public DSAppointment UpdateSchSlot(long TimeSlotDtlId, Int32 PatientAllowed, Boolean OverBookAllowed, Int32 BlockReasonId, string Comments)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Comments == "")
                    Comments = null;
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (TimeSlotDtlId == 0)
                    dbManager.AddParameters(0, PARM_TIME_SLOT_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TIME_SLOT_DETAIL_ID, TimeSlotDtlId);
                if (PatientAllowed == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ALLOWED, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ALLOWED, PatientAllowed);

                if (BlockReasonId == 0)
                    dbManager.AddParameters(2, PARM_BLOCK_REASON_ID, null);
                else
                    dbManager.AddParameters(2, PARM_BLOCK_REASON_ID, BlockReasonId);

                dbManager.AddParameters(3, PARM_COMMENTS, Comments);

                dbManager.AddParameters(4, PARM_OVER_BOOKED_ALLOWED, OverBookAllowed);
                //dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCH_SLOT_UPDATE, ds, ds.TimeSlotsDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateSchSlot", PROC_SCH_SLOT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Load Schedule Slot"
        public DSAppointment LoadScheduleSlot(long TimeSlotDtlId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(1);

                if (TimeSlotDtlId <= 0)
                    dbManager.AddParameters(0, PARM_TIME_SLOT_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TIME_SLOT_DETAIL_ID, TimeSlotDtlId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_SCH_SLOT, ds, ds.TimeSlotsDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadScheduleSlot", PROC_LOAD_SCH_SLOT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Weekly Slots With Appointment"

        public DSAppointment LoadWeeklySlotAppointment(long ProviderId, long FacilityId, long ResourceId, string SlotDate)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(4);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId == 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);

                dbManager.AddParameters(3, PARM_SLOT_DATE, SlotDate);





                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_WEEKLY_APP_SELECT, ds, ds.DaySlots.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadWeeklySlotAppointment", PROC_WEEKLY_APP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Multiple Slots With Appointment"

        public DSAppointment LoadMultipleSlotAppointment(long ProviderId, long FacilityId, long ResourceId, string SlotDate)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(4);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId == 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);

                dbManager.AddParameters(3, PARM_SLOT_DATE, SlotDate);





                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MULTIPLE_SLOTS_WITH_APPOINTMENTS, ds, ds.DaySlots.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadMultipleSlotAppointment", PROC_MULTIPLE_SLOTS_WITH_APPOINTMENTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Change Schedule Facility"

        public string ChangeSchFacility(string SlotDtlIds, long MoveFacilityId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MULTIPLE_SLOT_IDS, SlotDtlIds);

                if (MoveFacilityId == 0)
                    dbManager.AddParameters(1, PARM_MOVE_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MOVE_FACILITY_ID, MoveFacilityId);

                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CHANGE_SCH_FACILITY);

                if (returnVal != null)
                    throw new Exception(returnVal.ToString());

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::ChangeSchFacility", PROC_CHANGE_SCH_FACILITY, ex);
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

        #region "Wait List"

        public DSAppointment LookupWaitListStatus()
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_WAITLIST_STATUS_LOOKUP, ds, ds.WaitListStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupWaitListStatus", PROC_WAITLIST_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAppointment LookupPreferredTime()
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PREFERRED_TIME_LOOKUP, ds, ds.PreferredTime.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupPreferredTime", PROC_PREFERRED_TIME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAppointment LoadWaitList(long WaitListId, long PatientId, long FacilityId, long ProviderId, long ResourceId, int PrfTimeId, int WtListStatusId, DateTime? FromDate = null, DateTime? PreferredDate = null, DateTime? ToDate = null, int PageNumber = 1, int RowspPage = 1000)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(14);

                if (WaitListId == 0)
                    dbManager.AddParameters(0, PARM_WAIT_LIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_WAIT_LIST_ID, WaitListId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                if (ResourceId == 0)
                    dbManager.AddParameters(4, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_RESOURCE_ID, ResourceId);

                if (PrfTimeId == 0)
                    dbManager.AddParameters(5, PARM_PREFTIME_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PREFTIME_ID, PrfTimeId);

                if (WtListStatusId == 0)
                    dbManager.AddParameters(6, PARM_WAIT_LISTSTATUS_ID, null);
                else
                    dbManager.AddParameters(6, PARM_WAIT_LISTSTATUS_ID, WtListStatusId);

                dbManager.AddParameters(7, PARM_FROM_DATE, FromDate);
                dbManager.AddParameters(8, PARM_PREF_DATE, PreferredDate);
                dbManager.AddParameters(9, PARM_TO_DATE, ToDate);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.WaitList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(13, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_WAITLIST_SELECT, ds, ds.WaitList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadWaitList", PROC_WAITLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAppointment InsertWaitList(DSAppointment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                CreateWaitListParameters(dbManager, ds, true);
                ds = (DSAppointment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_WAITLIST_INSERT, ds, ds.WaitList.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALAppointment::InsertWaitList", PROC_WAITLIST_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAppointment UpdateWaitList(DSAppointment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                this.CreateWaitListParameters(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_WAITLIST_UPDATE, ds, ds.WaitList.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateWaitList", PROC_WAITLIST_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteWaitList(string WaitListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_WAIT_LIST_ID, WaitListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_WAITLIST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::DeleteWaitList", PROC_WAITLIST_DELETE, ex);
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

        #region "PatientBalance"
        public DSAppointment LoadPatientBalance(long PatientId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_BALANCE_SELECT, ds, ds.PatientBalance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadPatientBalance", PROC_PATIENT_BALANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Dashboard Appointments Visit"
        public DSAppointment LoadAppointmentsVisits(long ProviderId, long FacilityId, long ResourceId, string AppointmentDate, string LastName, string FirstName, string AccountNumber, string Status, string IsCheckedIn, Int32 PageNumber, Int32 RowspPage, string Action, long? PatientId = 0, string IsFaceSheet = "0", string AppointmentStatusIds = "")
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                if (FirstName == "")
                    FirstName = null;
                if (LastName == "")
                    LastName = null;
                if (AccountNumber == "")
                    AccountNumber = null;
                if (IsCheckedIn == "")
                    IsCheckedIn = null;
                if (Status == "")
                    Status = null;
                if (Action == "")
                    Action = null;
                // Start 15/12/2015 Muhammad Irfan For Appointments in FaceSheet
                if (IsFaceSheet == "")
                    IsFaceSheet = null;
                // End 15/12/2015 Muhammad Irfan For Appointments in FaceSheet

                dbManager.Open();
                dbManager.CreateParameters(18);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId == 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(3, PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(4, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(5, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(6, PARM_ACCOUNT_NO, AccountNumber);
                dbManager.AddParameters(7, PARM_STATUSES_ID, Status);
                dbManager.AddParameters(8, PARM_IS_CHECKD_IN, IsCheckedIn);
                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowspPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, RowspPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.AppointmentsVisits.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(14, PARM_ACTION, Action);

                // Start 15/12/2015 Muhammad Irfan For Appointments in FaceSheet

                if (PatientId == 0)
                    dbManager.AddParameters(15, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(15, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(16, PARM_IS_FACESHEET, IsFaceSheet);





                DataTable dtAppointmentStatusIds = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtAppointmentStatusIds.Columns.Add(COLUMN);




                if (AppointmentStatusIds != "")
                {
                    String[] substrings = AppointmentStatusIds.Split(',');
                    foreach (var substring in substrings)
                    {
                        DataRow Dr = dtAppointmentStatusIds.NewRow();
                        Dr[0] = substring;
                        dtAppointmentStatusIds.Rows.Add(Dr);
                    }
                    dbManager.AddParameters(17, PARM_APPOINTMENT_STATUS_IDS, dtAppointmentStatusIds);
                }
                else
                {

                    DataRow Dr = dtAppointmentStatusIds.NewRow();
                    Dr[0] = -1;
                    dtAppointmentStatusIds.Rows.Add(Dr);
                    dbManager.AddParameters(17, PARM_APPOINTMENT_STATUS_IDS, dtAppointmentStatusIds);
                }
                // End 15/12/2015 Muhammad Irfan For Appointments in FaceSheet

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASH_APP_VISIT_SELECT, ds, ds.AppointmentsVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentsVisits", PROC_DASH_APP_VISIT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSAppointment LoadPatientAppointments(long PatientId, long ProviderId, long FacilityId, Int32 PageNumber, Int32 RowspPage, string AppointmentStatusIds = "")
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);


                if (PageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowspPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.AppointmentsVisits.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(7, PARM_PATIENT_ID, PatientId);
                DataTable dtAppointmentStatusIds = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtAppointmentStatusIds.Columns.Add(COLUMN);

                if (AppointmentStatusIds != "")
                {
                    String[] substrings = AppointmentStatusIds.Split(',');
                    foreach (var substring in substrings)
                    {
                        DataRow Dr = dtAppointmentStatusIds.NewRow();
                        Dr[0] = substring;
                        dtAppointmentStatusIds.Rows.Add(Dr);
                    }
                    dbManager.AddParameters(8, PARM_APPOINTMENT_STATUS_IDS, dtAppointmentStatusIds);
                }
                else
                {
                    DataRow Dr = dtAppointmentStatusIds.NewRow();
                    Dr[0] = -1;
                    dtAppointmentStatusIds.Rows.Add(Dr);
                    dbManager.AddParameters(8, PARM_APPOINTMENT_STATUS_IDS, dtAppointmentStatusIds);
                }
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_APPOITMENTS_SELECT, ds, ds.AppointmentsVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadPatientAppointments", PROC_PATIENT_APPOITMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<Model.User.AppointmentVisits> LoadAppointmentsVisits_(long providerId, long facilityId, long resourceId, string appointmentDate, string lastName, string firstName, string accountNumber, string status, string isCheckedIn, int pageNumber, int rowspPage, string action, long? patientId = 0, string isFaceSheet = "0")
        {
            var listAppointmentVisits = new List<Model.User.AppointmentVisits>();
            var dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (firstName == "") firstName = null;
                if (lastName == "") lastName = null;
                if (accountNumber == "") accountNumber = null;
                if (isCheckedIn == "") isCheckedIn = null;
                if (status == "") status = null;
                if (action == "") action = null;
                if (isFaceSheet == "") isFaceSheet = null;

                dbManager.Open();
                dbManager.CreateParameters(17);

                if (providerId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);
                if (facilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, facilityId);
                if (resourceId == 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, resourceId);
                dbManager.AddParameters(3, PARM_APPOINTMENT_DATE, appointmentDate);
                dbManager.AddParameters(4, PARM_LAST_NAME, lastName);
                dbManager.AddParameters(5, PARM_FIRST_NAME, firstName);
                dbManager.AddParameters(6, PARM_ACCOUNT_NO, accountNumber);
                dbManager.AddParameters(7, PARM_STATUSES_ID, status);
                dbManager.AddParameters(8, PARM_IS_CHECKD_IN, isCheckedIn);
                if (pageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, pageNumber);
                if (rowspPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, rowspPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(13, PARM_ENTITY_ID,
                    string.Equals(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DefaultUser, StringComparison.CurrentCultureIgnoreCase)
                        ? null
                        : MDVSession.Current.EntityId);
                dbManager.AddParameters(14, PARM_ACTION, action);

                dbManager.AddParameters(15, PARM_PATIENT_ID, patientId == 0 ? null : patientId);
                dbManager.AddParameters(16, PARM_IS_FACESHEET, isFaceSheet);

                //return dbManager.ExecuteReaders<Model.User.AppointmentVisits>(PROC_DASH_APP_VISIT_SELECT);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASH_APP_VISIT_SELECT_COUNT);

                while (reader.Read())
                {
                    var model = new Model.User.AppointmentVisits
                    {
                        RecordCount = !string.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : ""
                    };
                    listAppointmentVisits.Add(model);
                }
                return listAppointmentVisits;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentsVisits", PROC_DASH_APP_VISIT_SELECT_COUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAppointment LoadAppointmentsVisits(long ProviderId, long FacilityId, long ResourceId, string AppointmentDate, string LastName, string FirstName, string AccountNumber, string Status, string IsCheckedIn, Int32 PageNumber, Int32 RowspPage, string Action, long UserId, long EntityId, long? PatientId = 0, string IsFaceSheet = "0")
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                if (FirstName == "")
                    FirstName = null;
                if (LastName == "")
                    LastName = null;
                if (AccountNumber == "")
                    AccountNumber = null;
                if (IsCheckedIn == "")
                    IsCheckedIn = null;
                if (Status == "")
                    Status = null;
                if (Action == "")
                    Action = null;

                if (IsFaceSheet == "")
                    IsFaceSheet = null;

                dbManager.Open();
                dbManager.CreateParameters(17);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId == 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(3, PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(4, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(5, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(6, PARM_ACCOUNT_NO, AccountNumber);
                dbManager.AddParameters(7, PARM_STATUSES_ID, Status);
                dbManager.AddParameters(8, PARM_IS_CHECKD_IN, IsCheckedIn);
                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowspPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, RowspPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.AppointmentsVisits.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(12, PARM_USER_ID, UserId);
                dbManager.AddParameters(13, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(14, PARM_ACTION, Action);

                if (PatientId == 0)
                    dbManager.AddParameters(15, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(15, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(16, PARM_IS_FACESHEET, IsFaceSheet);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASH_APP_VISIT_SELECT, ds, ds.AppointmentsVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentsVisits", PROC_DASH_APP_VISIT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Appointment Notes"

        public DSAppointment LoadAppointmentsNotes(string VisitFrom, string VisitTo, string NoteStatus, string FName, string LName, string NoteType, long ProviderId,
                                string AccountNo, long? PatientId = 0, Int32 PageNumber = 0, Int32 RowsPerPage = 0, bool IsDraftNote = false)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                if (VisitFrom == "")
                    VisitFrom = null;
                if (VisitTo == "")
                    VisitTo = null;
                if (NoteStatus == "")
                    NoteStatus = null;

                if (FName == "")
                    FName = null;
                if (LName == "")
                    LName = null;
                if (NoteType == "")
                    NoteType = null;
                if (AccountNo == "")
                    AccountNo = null;

                dbManager.Open();
                dbManager.CreateParameters(15);
                dbManager.AddParameters(0, PARM_VISIT_FROM, VisitFrom);
                dbManager.AddParameters(1, PARM_VISIT_TO, VisitTo);
                dbManager.AddParameters(2, PARM_NOTE_STATUS, NoteStatus);

                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                dbManager.AddParameters(5, PARM_LAST_NAME, LName);
                dbManager.AddParameters(6, PARM_FIRST_NAME, FName);
                dbManager.AddParameters(7, PARM_NOTE_TYPE, NoteType);
                if (ProviderId == 0)
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, ProviderId);
                // Start 30/11/2015 Muhammad Irfan Bug # 37
                dbManager.AddParameters(9, PARM_ACCOUNT_NO, AccountNo);
                // End 30/11/2015 Muhammad Irfan Bug # 37
                // Start 22/12/2015 Muhammad Irfan for selection of notes on patient id
                if (PatientId == 0)
                    dbManager.AddParameters(10, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(10, PARM_PATIENT_ID, PatientId);
                // End 22/12/2015 Muhammad Irfan for selection of notes on patient id
                /*Pagination included by Azhar for the bug # EMR-264*/
                if (PageNumber == 0)
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, DBNull.Value);
                else
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, RowsPerPage);
                dbManager.AddParameters(13, PARM_RECORD_COUNT, ds.AppointmentsVisits.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(14, PARM_IS_DRAFT_NOTE, IsDraftNote);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APP_VISIT_SELECT, ds, ds.AppointmentsVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentsNotes", PROC_APP_VISIT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<Model.User.AppointmentNote> LoadAppointmentsNotes_(string visitFrom, string visitTo, string noteStatus, string fName, string lName, string noteType, long providerId, string accountNo, long? patientId = 0, int pageNumber = 0, int rowsPerPage = 0, bool isDraftNote = false)
        {
            var listAppointmentVisits = new List<Model.User.AppointmentNote>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (visitFrom == "") visitFrom = null;
                if (visitTo == "") visitTo = null;
                if (noteStatus == "") noteStatus = null;
                if (fName == "") fName = null;
                if (lName == "") lName = null;
                if (noteType == "") noteType = null;
                if (accountNo == "") accountNo = null;

                dbManager.Open();
                dbManager.CreateParameters(15);

                dbManager.AddParameters(0, PARM_VISIT_FROM, visitFrom);
                dbManager.AddParameters(1, PARM_VISIT_TO, visitTo);
                dbManager.AddParameters(2, PARM_NOTE_STATUS, noteStatus);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, string.Equals(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DefaultUser, StringComparison.CurrentCultureIgnoreCase) ? null : MDVSession.Current.EntityId);
                dbManager.AddParameters(5, PARM_LAST_NAME, lName);
                dbManager.AddParameters(6, PARM_FIRST_NAME, fName);
                dbManager.AddParameters(7, PARM_NOTE_TYPE, noteType);
                if (providerId == 0) dbManager.AddParameters(8, PARM_PROVIDER_ID, null);
                else dbManager.AddParameters(8, PARM_PROVIDER_ID, providerId);
                dbManager.AddParameters(9, PARM_ACCOUNT_NO, accountNo);
                dbManager.AddParameters(10, PARM_PATIENT_ID, patientId == 0 ? null : patientId);
                if (pageNumber == 0) dbManager.AddParameters(11, PARM_PAGE_NUMBER, DBNull.Value);
                else dbManager.AddParameters(11, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0) dbManager.AddParameters(12, PARM_ROWS_PERPAGE, DBNull.Value);
                else dbManager.AddParameters(12, PARM_ROWS_PERPAGE, rowsPerPage);
                dbManager.AddParameters(13, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(14, PARM_IS_DRAFT_NOTE, isDraftNote);

                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_APP_VISIT_SELECT_COUNT);

                while (reader.Read())
                {
                    var model = new Model.User.AppointmentNote
                    {
                        RecordCount = !string.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : ""
                    };
                    listAppointmentVisits.Add(model);
                }
                return listAppointmentVisits;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentsNotes_", PROC_APP_VISIT_SELECT_COUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LookupAppointmentAndVisitStatus()
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_LOOKUP, ds, ds.AppVisitStatusLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupAppointmentAndVisitStatus", PROC_APP_VISIT_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LoadNotesDraftCount()
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTES_DRAFT_COUNT, ds, ds.AppointmentsVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadNotesDraftCount", PROC_NOTES_DRAFT_COUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NotesModel> LoadAppointmentsNotesBulkSign(string VisitFrom, string VisitTo, string NoteStatus, string FName, string LName, string NoteType,
            long ProviderId, string AccountNo, long? PatientId = 0, Int32 PageNumber = 0, Int32 RowsPerPage = 0, int IsReadyOrMissing = 1, string MissingInfo = null)
        {
            List<NotesModel> listModel = new List<NotesModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                if (VisitFrom == "")
                    VisitFrom = null;
                if (VisitTo == "")
                    VisitTo = null;
                if (NoteStatus == "")
                    NoteStatus = null;
                if (FName == "")
                    FName = null;
                if (LName == "")
                    LName = null;
                if (NoteType == "")
                    NoteType = null;
                if (AccountNo == "")
                    AccountNo = null;

                dbManager.Open();
                dbManager.CreateParameters(15);


                dbManager.AddParameters(0, PARM_VISIT_FROM, VisitFrom);
                dbManager.AddParameters(1, PARM_VISIT_TO, VisitTo);
                dbManager.AddParameters(2, PARM_NOTE_STATUS, NoteStatus);

                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(5, PARM_LAST_NAME, LName);
                dbManager.AddParameters(6, PARM_FIRST_NAME, FName);


                if (ProviderId == 0)
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(8, PARM_NOTE_TYPE, NoteType);
                dbManager.AddParameters(9, PARM_ACCOUNT_NO, AccountNo);
                if (PatientId == 0)
                    dbManager.AddParameters(10, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(10, PARM_PATIENT_ID, PatientId);
                if (PageNumber == 0)
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, DBNull.Value);
                else
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, RowsPerPage);
                dbManager.AddParameters(13, PARM_IS_READY_OR_MISSING, IsReadyOrMissing);
                dbManager.AddParameters(14, PARM_MISSING_INFO, MissingInfo);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_BULK_NOTES);
                string RecordType = "True";
                if (NoteType == "Progress Note")
                    RecordType = "False";
                while (reader.Read())
                {
                    if (IsReadyOrMissing == 1)
                    {
                        listModel.Add(new NotesModel
                        {
                            NotesId = ModelUtility.ToStr(reader["NotesId"]),
                            VisitId = ModelUtility.ToStr(reader["VisitId"]),
                            AccountNumber = ModelUtility.ToStr(reader["AccountNumber"]),
                            PatientName = ModelUtility.ToStr(reader["PateintName"]),
                            PatientId = ModelUtility.ToStr(reader["PatientId"]),
                            ProviderName = ModelUtility.ToStr(reader["Provider"]),
                            FacilityName = ModelUtility.ToStr(reader["Facility"]),
                            VisitDate = ModelUtility.ToStr(reader["VisitDate"]),
                            AppReason = ModelUtility.ToStr(reader["Reason"]),
                            NoteType = ModelUtility.ToStr(reader["NoteType"]),
                            NoteStatus = ModelUtility.ToStr(reader["NoteStatus"]),
                            BillingInfoId = ModelUtility.ToStr(reader["BillingInfoId"]),
                            RecordCount = ModelUtility.ToStr(reader["RecordCount"]),
                            IsPhoneEncounter = RecordType,
                            AppointmentDate = ModelUtility.ToStr(reader["LinkedAppointmentDate"]),
                            NoteDate = ModelUtility.ToStr(reader["NoteDate"]),
                            POS = ModelUtility.ToStr(reader["POS"]),
                            ProviderId = ModelUtility.ToStr(reader["CurrentNotesProviderId"]),
                        });
                    }
                    else
                    {
                        listModel.Add(new NotesModel
                        {
                            NotesId = ModelUtility.ToStr(reader["NotesId"]),
                            VisitId = ModelUtility.ToStr(reader["VisitId"]),
                            AccountNumber = ModelUtility.ToStr(reader["AccountNumber"]),
                            PatientName = ModelUtility.ToStr(reader["PateintName"]),
                            PatientId = ModelUtility.ToStr(reader["PatientId"]),
                            ProviderName = ModelUtility.ToStr(reader["Provider"]),
                            FacilityName = ModelUtility.ToStr(reader["Facility"]),
                            VisitDate = ModelUtility.ToStr(reader["VisitDate"]),
                            AppReason = ModelUtility.ToStr(reader["Reason"]),
                            NoteType = ModelUtility.ToStr(reader["NoteType"]),
                            MissingInfo = ModelUtility.ToStr(reader["MissingInfo"]),
                            CDSIds = ModelUtility.ToStr(reader["CDSIds"]),
                            RecordCount = ModelUtility.ToStr(reader["RecordCount"]),
                            IsPhoneEncounter = RecordType,
                            AppointmentDate = ModelUtility.ToStr(reader["LinkedAppointmentDate"]),
                            NoteDate = ModelUtility.ToStr(reader["NoteDate"]),
                            POS = ModelUtility.ToStr(reader["POS"]),
                            ProviderId = ModelUtility.ToStr(reader["CurrentNotesProviderId"]),
                        });
                    }
                }
                return listModel; ;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadAppointmentsNotesBulkSign", PROC_GET_BULK_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region PatientType

        #endregion

        public DSScheduleLookups LookupPatientType()
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_TYPE_LOOKUP, ds, ds.PatientType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupPatientType", PROC_PATIENT_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<FollowUpAppointmentModel> LoadPatientAppointment(long PatientId, long ProviderId, long FacilityId, string Date)
        {
            List<FollowUpAppointmentModel> listModel = new List<FollowUpAppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (ProviderId == 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);
                if (string.IsNullOrEmpty(Date))
                    dbManager.AddParameters(3, PARM_APPOINTMENT_DATE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_APPOINTMENT_DATE, Date);



                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_AVAILABLE_APPOINTMENTS);
                FollowUpAppointmentModel model = null;
                while (reader.Read())
                {
                    model = new FollowUpAppointmentModel();
                    model.AppointmentDate = reader["AppointmentDate"].ToString();
                    model.AppointmentID = reader["AppointmentID"].ToString();
                    model.Date = reader["Date"].ToString();
                    model.FacilityId = reader["FacilityId"].ToString();
                    model.ProviderId = reader["ProviderId"].ToString();
                    model.TimeFrom = reader["TimeFrom"].ToString();
                    model.Reason = reader["Reason"].ToString();
                    model.Duration = reader["Duration"].ToString();
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALAppointment::LoadPatientAppointment", PROC_AVAILABLE_APPOINTMENTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<FollowUpAppointmentModel> LoadAvailableSlots(long ProviderId, long FacilityId, string ScheduleDate)
        {
            List<FollowUpAppointmentModel> PatientAppointment = new List<FollowUpAppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (ProviderId == 0)
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(PARM_FACILITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_FACILITY_ID, FacilityId);
                if (string.IsNullOrEmpty(ScheduleDate))
                    dbManager.AddParameters(PARM_SCH_DATE, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_SCH_DATE, ScheduleDate);

                return dbManager.ExecuteReaders<FollowUpAppointmentModel>(PROC_AVAILABLE_SCHEDULE_SLOTS);
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALAppointment::LoadAvailableSlots", PROC_AVAILABLE_SCHEDULE_SLOTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        public DSScheduleLookups LookupVisitType()
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VISIT_TYPE_LOOKUP, ds, ds.VisitType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupVisitType", PROC_VISIT_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSScheduleLookups LookupPatientVisitType()
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_TYPE_LOOKUP, ds, ds.PatientVisitType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupPatientVisitType", PROC_PATIENT_VISIT_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<PatientVisitTypeLookUpModel> LookupPatientVisitType_WO_CancerRegistries()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<PatientVisitTypeLookUpModel> lstModel = new List<PatientVisitTypeLookUpModel>();
                SqlDataReader reader = null;
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_VISIT_TYPE_WO_CANCER_REGISTRIES_LOOKUP);
                PatientVisitTypeLookUpModel model = null;
                while (reader.Read())
                {
                    model = new PatientVisitTypeLookUpModel();
                    model.PatientTypeID = !String.IsNullOrEmpty(reader["PatientTypeID"].ToString()) ? reader["PatientTypeID"].ToString() : "";
                    model.VisitTypeID = !String.IsNullOrEmpty(reader["VisitTypeID"].ToString()) ? reader["VisitTypeID"].ToString() : "";
                    model.VisitType = !string.IsNullOrEmpty(reader["VisitType"].ToString()) ? reader["VisitType"].ToString() : "";
                    model.PatientType = !string.IsNullOrEmpty(reader["PatientType"].ToString()) ? reader["PatientType"].ToString() : "";

                    lstModel.Add(model);
                }
                return lstModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupPatientVisitType_WO_CancerRegistries", PROC_PATIENT_VISIT_TYPE_WO_CANCER_REGISTRIES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSScheduleLookups LookupPatientVisitType(string ProviderId)
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (ProviderId == "0")
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_TYPE_DURATION_LOOKUP, ds, ds.PatientVisitType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupPatientVisitType", PROC_PATIENT_VISIT_TYPE_DURATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "week select new function"
        /*
         * Date: 02/03/2016
         * Author: Muhammad Irfan
         * Overview: This function will select weekly slots appointments
         */

        public DSAppointment SelectWeeklySlots(long ProviderId, long ResourceId, long FacilityId, string SlotDate, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //if (StatusId == "")
                //    StatusId = null;
                if (PatientTypeId == "")
                    PatientTypeId = null;
                if (VisitTypeId == "")
                    VisitTypeId = null;
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (ResourceId <= 0)
                    dbManager.AddParameters(1, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_RESOURCE_ID, ResourceId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                dbManager.AddParameters(3, PARM_SLOT_DATE, SlotDate);

                dbManager.AddParameters(4, PARM_STATUSES_ID, StatusId);
                dbManager.AddParameters(5, PARM_PATIENT_TYPE, PatientTypeId);
                dbManager.AddParameters(6, PARM_PATIENT_VISIT_TYPE, VisitTypeId);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_WEEK_APP_SELECT, ds, ds.DaySlots.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::SelectWeeklySlots", PROC_WEEK_APP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        #region "Block Appointment Summary"

        public DSAppointment LoadBlockAppointmentSummary(ref DataTable dtFacilityIds, ref DataTable ProviderIds, string FromDate, string ToDate, ref DataTable ResourceIds)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (FromDate == "")
                    FromDate = null;
                if (ToDate == "")
                    ToDate = null;
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_FACILITY_IDs, dtFacilityIds);
                dbManager.AddParameters(1, PARM_PROVIDER_IDs, ProviderIds);
                dbManager.AddParameters(2, PARM_FROM_DATE, FromDate);
                dbManager.AddParameters(3, PARM_TO_DATE, ToDate);
                dbManager.AddParameters(4, PARM_RESOURCE_IDs, ResourceIds);
                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BLOCK_APP_SUMMARY, ds, ds.AppointmentSummary.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadBlockAppointmentSummary", PROC_BLOCK_APP_SUMMARY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment LookupSchedulingFromTime(Int64 ProviderId, Int64 FacilityId, Int64 ResourceId, string SchDate)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (SchDate == "")
                    SchDate = null;
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                if (ResourceId <= 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(3, PARM_SCH_DATE, SchDate);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULING_FROMTIME_LOOKUP, ds, ds.SchedulingFromTimeLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LookupSchedulingFromTime", PROC_SCHEDULING_FROMTIME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Cancel ChackIn"

        public string AppointmentCancelCheckIn(string VisitId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(2, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CANCEL_CHECKIN).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::AppointmentCancelCheckIn", PROC_CANCEL_CHECKIN, ex);
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

        #region Dashboar Appointment Notes
        public List<DAppointmentNoteModel> LoadDashboardAppointmentsNotes(string VisitFrom, string VisitTo, string NoteStatus, Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            List<DAppointmentNoteModel> listModel = new List<DAppointmentNoteModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                if (VisitFrom == "")
                    VisitFrom = null;
                if (VisitTo == "")
                    VisitTo = null;
                if (NoteStatus == "")
                    NoteStatus = null;
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_VISIT_FROM, VisitFrom);
                dbManager.AddParameters(1, PARM_VISIT_TO, VisitTo);
                dbManager.AddParameters(2, PARM_NOTE_STATUS, NoteStatus);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PERPAGE, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PERPAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_APP_NOTES_SELECT);
                DAppointmentNoteModel model = null;
                while (reader.Read())
                {
                    model = new DAppointmentNoteModel();
                    model.PatientId = reader["PatientId"].ToString();
                    model.PatientName = reader["PatientName"].ToString();
                    model.AccountNumber = reader["AccountNumber"].ToString();
                    model.ProviderName = reader["ProviderName"].ToString();
                    model.FacilityName = reader["FacilityName"].ToString();
                    model.NoteStatus = reader["NoteStatus"].ToString();
                    model.AppReason = reader["AppReason"].ToString();
                    model.VisitDate = reader["VisitDate"].ToString();
                    model.CC = reader["CC"].ToString();
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadDashboardAppointmentsNotes", PROC_DASHBOARD_APP_NOTES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Dashboard Appointments
        public List<DAppointmentModel> LoadDashboardAppointments(string AppointmentDate, string IsCheckedIn, Int32 PageNumber, Int32 RowspPage, string IsFaceSheet = "0")
        {
            List<DAppointmentModel> listModel = new List<DAppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                if (IsFaceSheet == "")
                    IsFaceSheet = null;
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(1, PARM_IS_CHECKD_IN, IsCheckedIn);
                dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(3, PARM_ROWS_PERPAGE, RowspPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(7, PARM_IS_FACESHEET, IsFaceSheet);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_APP_SELECT);
                DAppointmentModel model = null;
                while (reader.Read())
                {
                    model = new DAppointmentModel();
                    model.PatientId = Convert.ToString(reader["PatientId"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.FacilityName = Convert.ToString(reader["FacilityName"]);
                    model.Room = Convert.ToString(reader["Room"]);
                    model.AppointmentStatus = Convert.ToString(reader["AppointmentStatus"]);
                    model.VisitDate = Convert.ToString(reader["VisitDate"]);
                    model.minsWait = Convert.ToString(reader["minsWait"]);
                    model.Duration = Convert.ToString(reader["Duration"]);
                    model.AppointmentTime = Convert.ToString(reader["AppointmentTime"]);
                    model.NotesId = Convert.ToString(reader["NotesId"]);
                    model.VisitId = Convert.ToString(reader["VisitId"]);
                    model.Reason = Convert.ToString(reader["Reason"]);
                    model.AppointmentId = Convert.ToString(reader["AppointmentId"]);
                    model.FacilityId = Convert.ToString(reader["FacilityId"]);
                    model.ProviderId = Convert.ToString(reader["ProviderId"]);
                    model.PatientType = Convert.ToString(reader["PatientType"]);
                    model.VisitType = Convert.ToString(reader["VisitType"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadDashboardAppointments", PROC_DASHBOARD_APP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Portal Appointment Request"

        private void CreateParametersPortalRequest(IDBManager dbManager, DSAppointment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(16);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PORTAL_APPREQUEST_ID, ds.PortalAppRequest.PortalAppRequestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PORTAL_APPREQUEST_ID, ds.PortalAppRequest.PortalAppRequestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.PortalAppRequest.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.PortalAppRequest.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PATIENT_ID, ds.PortalAppRequest.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_APP_DATE, ds.PortalAppRequest.AppDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_TIMESLOT_ID, ds.PortalAppRequest.TimeSlotIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_TIMESLOTDTL_ID, ds.PortalAppRequest.TimeSlotDtlIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_REQUEST_STATUS, ds.PortalAppRequest.RequestStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_SCHREASON_ID, ds.PortalAppRequest.SchReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_SCHREASON_NAME, ds.PortalAppRequest.SchReasonNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_APPOINTMENT_ID, ds.PortalAppRequest.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.PortalAppRequest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.PortalAppRequest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_FROM_TIME, ds.PortalAppRequest.FromTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_TO_TIME, ds.PortalAppRequest.ToTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_DURATION, ds.PortalAppRequest.DurationColumn.ColumnName, DbType.String);
        }

        public DSAppointment LoadPortalAppRequest(long PortalAppRequestId, string RequestStatus, string AppDate)
        {
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (RequestStatus == "")
                    RequestStatus = null;
                if (AppDate == "")
                    AppDate = null;

                dbManager.Open();
                dbManager.CreateParameters(4);

                if (PortalAppRequestId <= 0)
                    dbManager.AddParameters(0, PARM_PORTAL_APPREQUEST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PORTAL_APPREQUEST_ID, PortalAppRequestId);
                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);
                dbManager.AddParameters(2, PARM_APP_DATE, AppDate);
                dbManager.AddParameters(3, PARM_User_ID, MDVSession.Current.AppUserId);
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PORTAL_APP_REQUEST_SELECT, ds, ds.PortalAppRequest.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadPortalAppRequest", PROC_PORTAL_APP_REQUEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAppointment UpdatePortalAppRequest(DSAppointment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersPortalRequest(dbManager, ds, false);
                ds = (DSAppointment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PORTAL_APP_REQUEST_UPDATE, ds, ds.PortalAppRequest.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdatePortalAppRequest", PROC_PORTAL_APP_REQUEST_UPDATE, ex);
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

        public DSAppointment InsertPortalAppRequest(DSAppointment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersPortalRequest(dbManager, ds, true);
                ds = (DSAppointment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PORTAL_APP_REQUEST_INSERT, ds, ds.PortalAppRequest.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::InsertPortalAppRequest", PROC_PORTAL_APP_REQUEST_INSERT, ex);
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

        public DSAppointment AcceptRejectMultiplePortalAppRequest(string PortalRequestIds, string RequestStatus)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSAppointment ds = new DSAppointment();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PORTAL_APPREQUEST_ID, PortalRequestIds);
                dbManager.AddParameters(1, PARM_REQUEST_STATUS, RequestStatus);
                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PORTAL_APP_REJECT_ACCEPT_MULTIPLE, ds, ds.PortalAppRequest.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::AcceptRejectMultiplePortalAppRequest", PROC_PORTAL_APP_REJECT_ACCEPT_MULTIPLE, ex);
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
        #region "UpdatePatientInsuranceINFutureAppointments"
        public string UpdateInsuranceInFutureAppointments(Int64 InsuranceId, string AppointmentId)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                //  DataTable dtTemp = ds.PatientAppointments.GetChanges();

                dbManager.AddParameters(PARM_INSURANCE_ID, InsuranceId, DbType.Int64);
                dbManager.AddParameters(PARM_APPOINTMENT_IDS, AppointmentId, DbType.String);
                dbManager.Open();
                var ReturnId = dbManager.ExecuteScalar(PROC_INSURANCE_APPOINTMENT_UPDATE);
                return MDVUtility.ToStr(ReturnId);

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointment", PROC_INSURANCE_APPOINTMENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdateAppointmentCopay(string AppointmentIds, Int64 InsuranceId)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.AddParameters(PARM_APPOINTMENT_IDS, AppointmentIds, DbType.String);
                dbManager.AddParameters(PARM_INSURANCE_ID, InsuranceId, DbType.Int64);
                dbManager.Open();

                dbManager.ExecuteScalar(PROC_UPDATE_APPOINTMENT_COPAY);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::UpdateAppointmentCopay", PROC_UPDATE_APPOINTMENT_COPAY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Schedule Revamp

        public string EDIEligibilityIdSelect(AppointmentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_PATIENT_ID, model.PatientId);
                dbManager.AddParameters(1, PARM_INSURANCE_ID, model.InsuranceId);
                dbManager.AddParameters(2, PARM_STATUS, model.Status);
                dbManager.AddParameters(3, PARM_PATIENT_APPOINTMENT_ID, model.AppointmentId);

                var returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EDIELIGIBILITY_ID_SELECT);

                return returnVal.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::EDIEligibilityIdSelect", PROC_EDIELIGIBILITY_ID_SELECT, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region DayView
        public List<AppointmentModel> LoadProviderAppointmentDayView(DataTable Providers, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, string AppointmentDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, Providers);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROVIDER_APPOINTMENT_DAY_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    model.LastName = MDVUtility.ToStr(reader["LastName"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.InsurancePlanId = MDVUtility.ToInt64(reader["InsurancePlanId"]);
                    model.InsuranceId = MDVUtility.ToInt64(reader["InsuranceId"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    model.NewPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["NewPatientColor"])) ? MDVUtility.ToStr(reader["NewPatientColor"]) : "#0088cc";
                    model.EstablishedPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EstablishedPatientColor"])) ? MDVUtility.ToStr(reader["EstablishedPatientColor"]) : "#0088cc";
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentDayView", PROC_PROVIDER_APPOINTMENT_DAY_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceAppointmentDayView(DataTable Resources, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, string AppointmentDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_RESOURCES_TABLE, Resources);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_APPOINTMENT_DATE, AppointmentDate);
                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RESOURCE_APPOINTMENT_DAY_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ResourceId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ResourceName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    model.LastName = MDVUtility.ToStr(reader["LastName"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.InsurancePlanId = MDVUtility.ToInt64(reader["InsurancePlanId"]);
                    model.InsuranceId = MDVUtility.ToInt64(reader["InsuranceId"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    model.NewPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["NewPatientColor"])) ? MDVUtility.ToStr(reader["NewPatientColor"]) : "#0088cc";
                    model.EstablishedPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EstablishedPatientColor"])) ? MDVUtility.ToStr(reader["EstablishedPatientColor"]) : "#0088cc";
                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadResourceAppointmentDayView", PROC_RESOURCE_APPOINTMENT_DAY_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceAppointmentDayViewSchedule(DataTable Resources, DataTable Facilitys, string AppointmentDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_RESOURCES_TABLE, Resources);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                dbManager.AddParameters(2, PARM_SCHEDULE_DATE, AppointmentDate);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RESOURCE_APPOINTMENT_DAY_VIEW_SCHEDULE);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["ScheduleDate"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["Timefrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["ToTime"]);
                    model.BeginningTime = MDVUtility.ToStr(reader["MinTimeFrom"]);
                    model.EndingTime = MDVUtility.ToStr(reader["MaxToTime"]);

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadResourceAppointmentDayViewSchedule", PROC_RESOURCE_APPOINTMENT_DAY_VIEW_SCHEDULE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadProviderAppointmentDayViewSchedule(DataTable Provider, DataTable Facilitys, string AppointmentDate, string viewType = null)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, Provider);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                dbManager.AddParameters(2, PARM_SCHEDULE_DATE, AppointmentDate);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(5, PARM_VIEW_TYPE, viewType);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROVIDER_APPOINTMENT_DAY_VIEW_SCHEDULE);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["ScheduleDate"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["Timefrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["ToTime"]);
                    model.BeginningTime = MDVUtility.ToStr(reader["MinTimeFrom"]);
                    model.EndingTime = MDVUtility.ToStr(reader["MaxToTime"]);

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentDayViewSchedule", PROC_PROVIDER_APPOINTMENT_DAY_VIEW_SCHEDULE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion DayView

        #region weekView
        public List<AppointmentModel> LoadProviderAppointmentWeekView(DataTable Providers, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, string StartDate, string EndDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, Providers);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_START_DATE, StartDate);
                dbManager.AddParameters(6, PARM_END_DATE, EndDate);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROVIDER_APPOINTMENT_WEEK_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    model.LastName = MDVUtility.ToStr(reader["LastName"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.InsurancePlanId = MDVUtility.ToInt64(reader["InsurancePlanId"]);
                    model.InsuranceId = MDVUtility.ToInt64(reader["InsuranceId"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    model.NewPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["NewPatientColor"])) ? MDVUtility.ToStr(reader["NewPatientColor"]) : "#0088cc";
                    model.EstablishedPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EstablishedPatientColor"])) ? MDVUtility.ToStr(reader["EstablishedPatientColor"]) : "#0088cc";

                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentWeekView", PROC_PROVIDER_APPOINTMENT_WEEK_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceAppointmentWeekView(DataTable Resources, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, string StartDate, string EndDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_RESOURCES_TABLE, Resources);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_START_DATE, StartDate);
                dbManager.AddParameters(6, PARM_END_DATE, EndDate);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RESOURCE_APPOINTMENT_WEEK_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ResourceId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ResourceName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    model.LastName = MDVUtility.ToStr(reader["LastName"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.InsurancePlanId = MDVUtility.ToInt64(reader["InsurancePlanId"]);
                    model.InsuranceId = MDVUtility.ToInt64(reader["InsuranceId"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    model.NewPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["NewPatientColor"])) ? MDVUtility.ToStr(reader["NewPatientColor"]) : "#0088cc";
                    model.EstablishedPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EstablishedPatientColor"])) ? MDVUtility.ToStr(reader["EstablishedPatientColor"]) : "#0088cc";
                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadResourceAppointmentWeekView", PROC_RESOURCE_APPOINTMENT_WEEK_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion weekView

        #region WorkweekView
        public List<AppointmentModel> LoadProviderAppointmentWorkWeekView(DataTable Providers, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, string StartDate, string EndDate, string WorkWeekDates = null)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, Providers);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_START_DATE, StartDate);
                dbManager.AddParameters(6, PARM_END_DATE, EndDate);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (!string.IsNullOrWhiteSpace(WorkWeekDates))
                    dbManager.AddParameters(9, PARM_APPOINTMENT_DATES, WorkWeekDates);
                else
                    dbManager.AddParameters(9, PARM_APPOINTMENT_DATES, DBNull.Value);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROVIDER_APPOINTMENT_WORKWEEK_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    model.LastName = MDVUtility.ToStr(reader["LastName"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.InsurancePlanId = MDVUtility.ToInt64(reader["InsurancePlanId"]);
                    model.InsuranceId = MDVUtility.ToInt64(reader["InsuranceId"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    model.NewPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["NewPatientColor"])) ? MDVUtility.ToStr(reader["NewPatientColor"]) : "#0088cc";
                    model.EstablishedPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EstablishedPatientColor"])) ? MDVUtility.ToStr(reader["EstablishedPatientColor"]) : "#0088cc";
                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}

                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentWorkWeekView", PROC_PROVIDER_APPOINTMENT_WORKWEEK_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadProviderWorkWeekSchedules(DataTable Providers, DataTable Facilitys, string StartDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, Providers);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                dbManager.AddParameters(2, PARM_SCHEDULE_DATE, StartDate);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROVIDER_WORK_WEEK_SCHEDULES);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();

                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);

                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);

                    model.AppointmentDate = MDVUtility.ToDateTime(reader["ScheduleDate"]);

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderWorkWeekSchedules", PROC_PROVIDER_WORK_WEEK_SCHEDULES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceWorkWeekSchedules(DataTable Resourcess, DataTable Facilitys, string StartDate)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_RESOURCES_TABLE, Resourcess);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                dbManager.AddParameters(2, PARM_SCHEDULE_DATE, StartDate);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RESOURCE_WORK_WEEK_SCHEDULES);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();

                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);

                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);

                    model.AppointmentDate = MDVUtility.ToDateTime(reader["ScheduleDate"]);

                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadResourceWorkWeekSchedules", PROC_RESOURCE_WORK_WEEK_SCHEDULES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceAppointmentWorkWeekView(DataTable Resources, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, string StartDate, string EndDate, string WorkWeekDays = null)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);
                dbManager.AddParameters(0, PARM_RESOURCES_TABLE, Resources);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_START_DATE, StartDate);
                dbManager.AddParameters(6, PARM_END_DATE, EndDate);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (!string.IsNullOrWhiteSpace(WorkWeekDays))
                    dbManager.AddParameters(9, PARM_APPOINTMENT_DATES, WorkWeekDays);
                else
                    dbManager.AddParameters(9, PARM_APPOINTMENT_DATES, DBNull.Value);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RESOURCE_APPOINTMENT_WORKWEEK_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ResourceId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ResourceName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    model.LastName = MDVUtility.ToStr(reader["LastName"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.InsurancePlanId = MDVUtility.ToInt64(reader["InsurancePlanId"]);
                    model.InsuranceId = MDVUtility.ToInt64(reader["InsuranceId"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    model.NewPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["NewPatientColor"])) ? MDVUtility.ToStr(reader["NewPatientColor"]) : "#0088cc";
                    model.EstablishedPatientColor = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EstablishedPatientColor"])) ? MDVUtility.ToStr(reader["EstablishedPatientColor"]) : "#0088cc";

                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadResourceAppointmentWorkWeekView", PROC_RESOURCE_APPOINTMENT_WORKWEEK_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion workweekView

        public List<AppointmentModel> LoadProviderAppointmentMonthView(DataTable Providers, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, int Month, int Year)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, Providers);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_MONTH, Month);
                dbManager.AddParameters(6, PARM_YEAR, Year);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROVIDERS_APPOINTMENT_MONTH_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    //model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentMonthView", PROC_PROVIDERS_APPOINTMENT_MONTH_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceAppointmentMonthView(DataTable Resources, DataTable Facilitys, long PatientTypeId, DataTable VisitTypes, DataTable AppointmentStatuses, int Month, int Year)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_RESOURCES_TABLE, Resources);
                dbManager.AddParameters(1, PARM_FACILITYS_TABLE, Facilitys);
                if (PatientTypeId > 0)
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, PatientTypeId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_TYPE, null);
                }
                dbManager.AddParameters(3, PARM_VISITTYPES_TABLE, VisitTypes);
                dbManager.AddParameters(4, PARM_APPOINTMENT_STATUSES_TABLE, AppointmentStatuses);
                dbManager.AddParameters(5, PARM_MONTH, Month);
                dbManager.AddParameters(6, PARM_YEAR, Year);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RESOURCES_APPOINTMENT_MONTH_VIEW);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ResourceId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ResourceName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.GroupType = MDVUtility.ToStr(reader["GroupType"]);
                    //model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    //model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    //model.LastAppointmentStatus = MDVUtility.ToStr(reader["LastAppointmentStatus"]);
                    //model.LastScheduleStatusId = MDVUtility.ToStr(reader["LastScheduleStatusId"]);
                    //model.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    //model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                    //model.isNoteCreated = MDVUtility.ToStr(reader["isNoteCreated"]) == "1" ? true : false;
                    //model.Notesid = MDVUtility.ToStr(reader["Notesid"]);
                    //model.IsNoteSigned = MDVUtility.ToStr(reader["IsNoteSigned"]) == "1" ? true : false;
                    //model.FacilityPhoneNo = MDVUtility.ToStr(reader["FacilityPhoneNo"]);
                    model.PatientAddress1 = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Address1"])) ? MDVUtility.ToStr(reader["Address1"]) : "";
                    model.PatientCity = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["City"])) ? MDVUtility.ToStr(reader["City"]) : "";
                    model.PatientDOB = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["DOB"])) ? MDVUtility.ToStr(reader["DOB"]) : "";
                    model.PatientEthnicityIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["EthnicityId"])) ? MDVUtility.ToStr(reader["EthnicityId"]) : "";
                    model.PatientMaritalStatus = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["MaritialStatus"])) ? MDVUtility.ToStr(reader["MaritialStatus"]) : "";
                    model.PatientRaceIds = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RaceId"])) ? MDVUtility.ToStr(reader["RaceId"]) : "";
                    model.PatientSex = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["Gender"])) ? MDVUtility.ToStr(reader["Gender"]) : "";
                    model.PatientState = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["State"])) ? MDVUtility.ToStr(reader["State"]) : "";
                    model.PatientZip = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ZIPCode"])) ? MDVUtility.ToStr(reader["ZIPCode"]) : "";
                    model.PatientHomeTel = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["HomePhoneNo"])) ? MDVUtility.ToStr(reader["HomePhoneNo"]) : "";
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    //if (model.CopayBal < 0 && model.AmtCopay == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    if (model.CopayBal < 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.AmtCopay > 0 && model.CopayBal == 0)
                    {
                        model.CopayBal = model.AmtCopay;
                    }

                    //if (model.CopayBal == 0)
                    //{
                    //    model.CopayClass = "Green";
                    //}
                    //if (model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Black";
                    //}
                    //if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    //{
                    //    model.CopayClass = "Red";
                    //}
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadProviderAppointmentMonthView", PROC_RESOURCES_APPOINTMENT_MONTH_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentStatusOptionModel> AppointmentStatusOptions(long AppointmentStatusId, string AppointmentStatus)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<AppointmentStatusOptionModel> listModel = new List<AppointmentStatusOptionModel>();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_APPOINTMENT_STATUS_ID, AppointmentStatusId);
                dbManager.AddParameters(1, PARM_APPOINTMENT_STATUS, AppointmentStatus);



                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_OPTIONS);
                AppointmentStatusOptionModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentStatusOptionModel();
                    model.Status = MDVUtility.ToStr(reader["status"]);
                    model.PossibleOption = MDVUtility.ToStr(reader["PossibleOptions"]);
                    model.Color = MDVUtility.ToStr(reader["Color"]);
                    model.DestinationStatusId = MDVUtility.ToStr(reader["DestinationStatusId"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::AppointmentStatusOptions", PROC_APPOINTMENT_STATUS_OPTIONS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region TooltipRegion
        public AppointmentTooltipModel FillToolTipData(string AppointmentId)
        {
            // AppointmentModel TooltipModel = new  AppointmentModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_APPOINTMENT_ID, AppointmentId);



                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LOAD_TOOLTIP_DATA);
                AppointmentTooltipModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentTooltipModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    model.Gender = MDVUtility.ToStr(reader["Gender"]);
                    model.DOB = MDVUtility.ToStr(reader["DOB"]);
                    model.EmailAddress = MDVUtility.ToStr(reader["EmailAddress"]);
                    model.Age = MDVUtility.ToStr(reader["Age"]);
                    model.CellNo = MDVUtility.ToStr(reader["CellNo"]);
                    model.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    model.CreatedOn = MDVUtility.ToStr(reader["CreatedOn"]);
                    model.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    model.ModifiedOn = MDVUtility.ToStr(reader["ModifiedOn"]);
                    model.RescheduleDate = MDVUtility.ToStr(reader["RescheduleDate"]);
                    model.IsReminderSent = MDVUtility.ToStr(reader["IsReminderSent"]) == "1" ? true : false;
                    model.Status = MDVUtility.ToStr(reader["Status"]);
                    model.KeyPress = MDVUtility.ToStr(reader["KeyPress"]);
                    model.ResponseMessage = MDVUtility.ToStr(reader["ResponseMessage"]);
                    model.PatientProfileImagePath = MDVUtility.ToStr(reader["PatientProfileImagePath"]);
                    model.PatientProfileThumbnailPath = MDVUtility.ToStr(reader["PatientProfileThumbnailPath"]);
                    model.Duration = MDVUtility.ToStr(reader["Duration"]);
                    model.PrimaryInsuranceName = MDVUtility.ToStr(reader["PrimaryInsuranceName"]);
                    model.CopayBal = MDVUtility.Tofloat(MDVUtility.ToStr(reader["CopayBal"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.ImageType = MDVUtility.ToStr(reader["ImageType"]);
                    if (!string.IsNullOrEmpty(reader["PatientImage"].ToString()))
                        model.PatientImage = (byte[])(reader["PatientImage"]);

                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.StatusColor = MDVUtility.ToStr(reader["StatusColor"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.VisitTypeColor = MDVUtility.ToStr(reader["VisitTypeColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.CancellationReason = MDVUtility.ToStr(reader["CancellationReason"]);

                    model.CopayClass = "Black";
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Black";
                    }
                    if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    if (model.CopayBal < 0 && model.AmtCopay == 0)
                    {
                        model.CopayClass = "Green";
                    }

                }
                reader.NextResult();
                while (reader.Read())
                {

                    model.RescheduleProvider = MDVUtility.ToStr(reader["RescheduleProvider"]);
                    model.RescheduleFacility = MDVUtility.ToStr(reader["RescheduleFacility"]);
                    model.ScheduleDate = MDVUtility.ToStr(reader["ScheduleDate"]);
                    model.ScheduleFacility = MDVUtility.ToStr(reader["ScheduleFacility"]);
                    model.ScheduleProvider = MDVUtility.ToStr(reader["ScheduleProvider"]);





                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::sp_PatientAppointmentsToolTip", PROC_LOAD_TOOLTIP_DATA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Wait List

        public List<AppointmentModel> LoadWaitListSchedule(AppointmentModel apptModel)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, apptModel.ProviderId);
                dbManager.AddParameters(1, PARM_FACILITY_ID, apptModel.FacilityId);
                dbManager.AddParameters(2, PARM_RESOURCE_ID, apptModel.ResourceId);
                dbManager.AddParameters(3, PARM_FROM_DATE, Convert.ToDateTime(apptModel.TimeFrom));
                if (apptModel.TimeTo != "")
                {
                    dbManager.AddParameters(4, PARM_TO_DATE, Convert.ToDateTime(apptModel.TimeTo));
                }
                else
                {
                    dbManager.AddParameters(4, PARM_TO_DATE, DBNull.Value);
                }
                if (apptModel.PreferredDate != "")
                {
                    dbManager.AddParameters(5, PARM_PREFERRED_DATE, Convert.ToDateTime(apptModel.PreferredDate));
                }
                else
                {
                    dbManager.AddParameters(5, PARM_PREFERRED_DATE, DBNull.Value);
                }
                dbManager.AddParameters(6, PARM_IS_PROVIDER, apptModel.IsProvider);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_WAITLIST_SCHEDULESEARCH);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Black";
                    }
                    if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadWaitListSchedule", PROC_WAITLIST_SCHEDULESEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AppointmentModel> LoadResourceWaitListSchedule(AppointmentModel apptModel)
        {
            List<AppointmentModel> listModel = new List<AppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, apptModel.ProviderId);
                dbManager.AddParameters(1, PARM_FACILITY_ID, apptModel.FacilityId);
                dbManager.AddParameters(2, PARM_RESOURCE_ID, apptModel.ResourceId);
                dbManager.AddParameters(3, PARM_FROM_DATE, Convert.ToDateTime(apptModel.TimeFrom));
                if (apptModel.TimeTo != "")
                {
                    dbManager.AddParameters(4, PARM_TO_DATE, Convert.ToDateTime(apptModel.TimeTo));
                }
                else
                {
                    dbManager.AddParameters(4, PARM_TO_DATE, DBNull.Value);
                }
                if (apptModel.PreferredDate != "")
                {
                    dbManager.AddParameters(5, PARM_PREFERRED_DATE, Convert.ToDateTime(apptModel.PreferredDate));
                }
                else
                {
                    dbManager.AddParameters(5, PARM_PREFERRED_DATE, DBNull.Value);
                }
                dbManager.AddParameters(6, PARM_IS_PROVIDER, apptModel.IsProvider);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_WAITLIST_SCHEDULESEARCH);
                AppointmentModel model = null;
                while (reader.Read())
                {
                    model = new AppointmentModel();
                    model.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    model.AppointmentId = MDVUtility.ToStr(reader["AppointmentId"]);
                    model.id = MDVUtility.ToLong(reader["AppointmentId"]);
                    model.AppointmentDate = MDVUtility.ToDateTime(reader["AppointmentDate"]);
                    model.ProviderId = MDVUtility.ToLong(reader["ResourceId"]);
                    model.ProviderName = MDVUtility.ToStr(reader["ResourceName"]);
                    model.PatientTypeId = MDVUtility.ToLong(reader["PatientTypeId"]);
                    model.PatientType = MDVUtility.ToStr(reader["PatientType"]);
                    model.SchStatusId = MDVUtility.ToStr(reader["SchStatusId"]);
                    model.AppointmentStatus = MDVUtility.ToStr(reader["AppointmentStatus"]);
                    model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                    model.VisitTypeName = MDVUtility.ToStr(reader["VisitTypeName"]);
                    model.FacilityId = MDVUtility.ToLong(reader["FacilityId"]);
                    model.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    model.FacilityColor = MDVUtility.ToStr(reader["FacilityColor"]);
                    model.Comments = MDVUtility.ToStr(reader["Comments"]);
                    model.IsNonBilable = MDVUtility.ToStr(reader["IsNonBilable"]) == "1" ? true : false;
                    model.ReasonComments = MDVUtility.ToStr(reader["ReasonComments"]);
                    model.EligibilityStatus = MDVUtility.ToStr(reader["EligibilityStatus"]);
                    model.CopayBal = MDVUtility.Tofloat(reader["CopayBal"]);
                    model.AppointmentDateFrom = MDVUtility.ToDateTime(reader["AppointmentDateFrom"]);
                    model.AppointmentDateTo = MDVUtility.ToDateTime(reader["AppointmentDateTo"]);
                    model.TimeFrom = MDVUtility.ToStr(reader["TimeFrom"]);
                    model.TimeTo = MDVUtility.ToStr(reader["TimeTo"]);
                    model.start = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateFrom"]));
                    model.end = MDVUtility.ToDateTime(MDVUtility.ToStr(reader["AppointmentDateTo"]));
                    model.StatusColor = MDVUtility.ToStr(MDVUtility.ToStr(reader["StatusColor"]));
                    model.AmtCopay = MDVUtility.Tofloat(MDVUtility.ToStr(reader["AmtCopay"]));
                    model.CopayClass = "Black";
                    if (model.CopayBal == 0)
                    {
                        model.CopayClass = "Green";
                    }
                    if (model.CopayBal > 0)
                    {
                        model.CopayClass = "Black";
                    }
                    if (model.CopayBal == model.AmtCopay && model.CopayBal > 0)
                    {
                        model.CopayClass = "Red";
                    }
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadResourceWaitListSchedule", PROC_RESOURCE_APPOINTMENT_DAY_VIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public string SaveAppointmentNative(EmptySlotModel smodel)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, smodel.PatientId);
                dbManager.AddParameters(1, PARM_PATIENT_APPOINTMENT_ID, smodel.AppointmentId);
                string Message = "";
                if (MDVUtility.ToBool(smodel.Approve) == true)
                {
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, PARM_PATIENT_ID, smodel.PatientId);
                    dbManager.AddParameters(1, PARM_PATIENT_APPOINTMENT_ID, smodel.AppointmentId);
                    dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_APPROVE_NATIVE);
                    Message = "Successfully Approved!";
                }
                else
                {
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_PATIENT_ID, smodel.PatientId);
                    dbManager.AddParameters(1, PARM_PATIENT_APPOINTMENT_ID, smodel.AppointmentId);
                    dbManager.AddParameters(2, PARM_REJECTION_REASON, smodel.RejectionReason);
                    dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENT_DISCARD_NATIVE);
                    Message = "Successfully Discarded!";
                }
                return Message;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::SaveAppointmentNative", PROC_PATIENT_APPOINTMENT_APPROVE_NATIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string CopyPasteAppointment(AppointmentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.CreateParametersForPaste(dbManager, model);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENTS_COPY_PASTE));
                if (retunVal == "Appointment already exists for this patient for given date and time")
                    throw new Exception(retunVal);

                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::CopyPasteAppointment", PROC_PATIENT_APPOINTMENTS_COPY_PASTE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string CutPasteAppointment(AppointmentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.CreateParametersForPaste(dbManager, model);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_APPOINTMENTS_CUT_PASTE));
                if (retunVal == "Appointment already exists for this patient for given date and time")
                    throw new Exception(retunVal);
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::CutPasteAppointment", PROC_PATIENT_APPOINTMENTS_CUT_PASTE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        private void CreateParametersForPaste(IDBManager dbManager, AppointmentModel model)
        {

            dbManager.CreateParameters(13);

            dbManager.AddParameters(0, PARM_PATIENT_APPOINTMENT_ID, model.AppointmentId);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, model.ProviderId);
            dbManager.AddParameters(2, PARM_RESOURCE_ID, model.ResourceId);
            dbManager.AddParameters(3, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(6, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(7, PARM_ERROR_MESSAGE, "");
            dbManager.AddParameters(8, PARM_APP_DATE, model.AppointmentDate);
            dbManager.AddParameters(9, PARM_TO_TIME, model.end);
            dbManager.AddParameters(10, PARM_FROM_TIME, model.start);
            dbManager.AddParameters(11, PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(12, PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }
        public DSScheduleSetup LoadSchBlockHours(DataTable dtProvider, DataTable dtResources, DateTime date, string viewType = "0")
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_PROVIDERS_TABLE, dtProvider);
                dbManager.AddParameters(1, PARM_RESOURCES_TABLE, dtResources);
                dbManager.AddParameters(2, PARM_FROM_DATE, date);
                dbManager.AddParameters(3, PARM_FROM_VIEW_TYPE, viewType);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCH_BLOCKHOURS_SELECT, ds, ds.BlockHoursSlots.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointment::LoadBlockHours", PROC_SCH_BLOCKHOURS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion Schedule Revamp


    }
}
