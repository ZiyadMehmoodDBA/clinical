using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Logging;
using System.Data.SqlClient;
using MDVision.Model.Admin.Reminder;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALReminders
    {
        #region Variable
        // public SharedVariable SharedObj ;
        #endregion

        #region Constructors

        public DALReminders()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALReminders(SharedVariable SharedVariable)
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

        #region " Stored Procedure Names"
        //done start
        private const string PROC_REMINDERS_TEMPLATE_SELECT = "Provider.sp_RemindersTemplateSelect";//
        private const string PROC_REMINDERS_TEMPLATE_UPDATE = "Provider.sp_RemindersTemplateUpdate";//
        private const string PROC_REMINDERS_TEMPLATE_INSERT = "Provider.sp_RemindersTemplateInsert";//
        private const string PROC_REMINDERS_TEMPLATE_DELETE = "Provider.sp_RemindersTemplateDelete";//

        private const string PROC_REMINDERS_TEMPLATES_TYPE_INSERT = "Provider.sp_RemindersTemplateTypeInsert";//
        private const string PROC_REMINDERS_TEMPLATE_TYPE_LOOKUP = "Provider.sp_RemindersTemplateLookup";//

        private const string PROC_REMINDER_TYPE_LOOKUP = "[Provider].[sp_RemindersTemplateTypeLookup]";//

        private const string PROC_REMINDERS_EMAIL_SETTINGS_UPDATE = "Provider.sp_ReminderEmailSettingUpdate";//
        private const string PROC_REMINDERS_EMAIL_SETTINGS_INSERT = "Provider.sp_ReminderEmailSettingInsert";//
        private const string PROC_REMINDERS_EMAIL_SETTINGS_DELETE = "Provider.sp_ReminderEmailSettingDelete";//
        private const string PROC_REMINDERS_EMAIL_SETTINGS_SELECT = "Provider.sp_ReminderEmailSettingSelect";//

        private const string PROC_REMINDER_TEXTSETTING_SELECT = "Provider.sp_ReminderTextSettingSelect";
        private const string PROC_REMINDER_TEXTSETTING_UPDATE = "Provider.sp_ReminderTextSettingUpdate";
        private const string PROC_REMINDER_TEXTSETTING_INSERT = "Provider.sp_ReminderTextSettingInsert";
        private const string PROC_REMINDER_TEXTSETTING_DELETE = "Provider.sp_ReminderTextSettingDelete";

        private const string PROC_REMINDER_VOICESETTING_SELECT = "Provider.sp_ReminderVoiceSettingSelect";
        private const string PROC_REMINDER_VOICESETTING_UPDATE = "Provider.sp_ReminderVoiceSettingUpdate";
        private const string PROC_REMINDER_VOICESETTING_INSERT = "Provider.sp_ReminderVoiceSettingInsert";
        private const string PROC_REMINDER_VOICESETTING_DELETE = "Provider.sp_ReminderVoiceSettingDelete";

        private const string PROC_QUICK_VOICE_REMINDER_INSERT = "Provider.sp_QuickVoiceReminderInsert";
        private const string PROC_QUICK_VOICE_REMINDER_UPDATE = "Provider.sp_QuickVoiceReminderUpdate";
        private const string PROC_QUICK_VOICE_REMINDER_SELECT = "Provider.sp_QuickVoiceReminderSelect";
        private const string PROC_QUICK_VOICE_REMINDER_DELETE = "Provider.sp_QuickVoiceReminderDelete";


        private const string PROC_QUICK_SMS_REMINDER_INSERT = "Provider.sp_QuickSMSReminderInsert";
        private const string PROC_QUICK_SMS_REMINDER_UPDATE = "Provider.sp_QuickSMSReminderUpdate";
        private const string PROC_QUICK_SMS_REMINDER_SELECT = "Provider.sp_QuickSMSReminderSelect";
        private const string PROC_QUICK_SMS_REMINDER_DELETE = "Provider.sp_QuickSMSReminderDelete";

        private const string PROC_QUICK_EMAIL_REMINDER_INSERT = "Provider.sp_QuickEmailReminderInsert";
        private const string PROC_QUICK_EMAIL_REMINDER_UPDATE = "Provider.sp_QuickEmailReminderUpdate";
        private const string PROC_QUICK_EMAIL_REMINDER_SELECT = "Provider.sp_QuickEmailReminderSelect";
        private const string PROC_QUICK_EMAIL_REMINDER_DELETE = "Provider.sp_QuickEmailReminderDelete";

        private const string PROC_NOTE_TAG_NAME_LOOKUP = "Clinical.sp_NotesTagNameLookup";//
        private const string PROC_REMINDER_CONFIRMATION_KEY = "Provider.sp_ReminderConfirmationKeyLookup";//
        private const string PROC_REMINDER_TEXT_VOICE_LOOKUP = "[Provider].[sp_RemindersTextVoiceLookup]";//
        private const string PROC_REMINDER_SETTING_SELECT = "[Provider].[sp_SelectReminderSetting]";//
        //done end

        private const string PROC_WEEK_DAYS_LOOKUP = "Provider.sp_WeekDaysLookup";

        private const string PROC_NOTE_TAG_CATEGORY_LOOKUP = "Clinical.sp_NotesTagCategoryLookup";
        private const string PROC_REMINDER_TAG_NAME_LOOKUP = "Provider.sp_NotesTagNameLookup";
        //private const string PROC_REMINDERS_TEMPLATE_TYPE_INSERT = "Clinical.sp_NotesTemplateTypeInsert";

        private const string PROC_NOTE_TEMPLATE_LOOKUP = "Clinical.sp_NotesTemplateLookup";
        private const string PROC_REMINDER_DELIVERY_DATE_TIME_LOOKUP = "Provider.sp_ReminderDeliveryDateTimeLookup";
        private const string PROC_TELEVOXAPPOINTMENTS = "Patient.TeleVoxAppointments";

        private const string PROC_TELEVOXLOGINSERT = "Patient.TeleVoxLogInsert";
        private const string PROC_TELEVOXLOGDETAILINSERT = "Patient.TeleVoxLogDetailInsert";

        private const string PROC_TeleVoxAppointmentsHandle = "Patient.TeleVoxAppointmentsHandle";


        #endregion

        #region "Parameters"
        private const string PARM_REMINDERS_TEMPLATE_ID = "@RemindersTemplateId";
        private const string PARM_REMINDERS_EMAIL_SETTINGS_ID = "@ReminderEmailSettingId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_REMINDERS_TEMPLATE_NAME = "@RemindersTemplateName";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_REMINDERS_NAMES = "@ProviderNames";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_NAMES = "@SpecialtyNames";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_NOTES_TAGNAME_IDS = "@NotesTagNameIds";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_HTML_TEMPLATE = "@HTMLTemplate";
        private const string PARM_TEMPLATE_TYPE_ID = "@TemplateTypeId";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_REMINDERS_TEMPLATE_TYPE_ID = "@NotesTemplateTypeId";
        private const string PARM_SHORTNAME = "@ShortName";
        private const string PARM_NOTE_CATEGORY_ID = "@NotesTagCategoryId";
        private const string PARM_IS_REMINDER = "@IsReminder";
        private const string PARM_REMINDER_TEMPLATE_TYPE = "@ReminderTemplateType";
        private const string PARM_REMINDERS_TEMPLATES_TYPE_ID = "@RemindersTemplateTypeId";
        private const string PARM_SENDER_EMAIL = "@SenderEmail";
        private const string PARM_MESSAGE_TEMPLATE = "@MessageTemplate";

        private const string PARM_REMINDER_TEXTSETTING_ID = "@ReminderTextSettingId";
        private const string PARM_REMINDER_VOICESETTING_ID = "@ReminderVoiceSettingId";
        private const string PARM_CALLEE_NAME = "@CalleeName";
        private const string PARM_CALL_ID_NUMBER = "@CallIDNumber";

        private const string PARM_REPLIES_EMAIL = "@RepliesEmail";
        private const string PARM_IS_REMINDER_FAILOVER = "@IsReminderFailover";
        private const string PARM_STATUS = "@Status";
        private const string PARM_REQUEST_DELIVERY = "@RequestDelivery";
        private const string PARM_RESPONSE_DELIVERY = "@ResponseDelivery";
        private const string PARM_MESSAGE = "@Message";
        private const string PARM_KEY_PRESS = "@KeyPress";
        private const string PARM_RETRIES = "@ReTries";
        private const string PARM_TIME_ZONE = "@TimeZone";
        private const string PARM_TIME_ZONE_ID = "@TimeZoneId";
        private const string PARM_REMINDER_SETTING_ID = "@ReminderSettingId";
        private const string PARM_API_KEY = "@APIKey";
        private const string PARM_IS_LAST_WEEK_RECORDS_ONLY = "@IsLastWeekRecordsOnly";
        private const string PARM_FACILITY_IDs = "@Facilityids";



        private const string PARM_REPEAT_MESSAGE = "@RepeatMessage";
        private const string PARM_APP_CONFIRMATION_KEY = "@AppConfirmationKey";
        private const string PARM_APP_CANCEL_KEY = "@AppCancelKey";
        private const string PARM_DAIL_IN_PIN = "@DialInPin";
        private const string PARM_TEXT_TO_SPEECH = "@TextToSpeech";


        private const string PARM_QUICK_VOICE_REMINDER_ID = "@QuickVoiceReminderId";
        private const string PARM_PHONE_NUMBER = "@PhoneNumber";
        private const string PARM_MESSAGE_VOICE_ID = "@MessageVoiceId";
        private const string PARM_LEADIN_VOICE_ID = "@LeadInVoiceId";
        private const string PARM_LEADOUT_VOICE_ID = "@LeadOutVoiceId";

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_QUICK_SMS_REMINDER_ID = "@QuickSMSReminderId";
        private const string PARM_IS_PROCESSED = "@IsProcessed";
        private const string PARM_HTML_TEMPLATE_WITH_IDS = "@HTMLTemplateWithIds";
        private const string PARM_NOTES_TAG_CATEGORY_ID = "@NotesTagCategoryId";

        private const string PARM_CONFIRMATION_MESSAGE = "@ConfirmationMessage";
        private const string PARM_CANCELLATION_MESSAGE = "@CancellationMessage";
        private const string PARM_DELIVERY_DATE_TIME = "@DeliveryDateTime";
        private const string PARM_GUARANTOR_PH_NUMBER = "@GuarantorPhNumber";
        private const string PARM_VOICE_REMINDER_FAILOVER = "@VoiceReminderFailover";
        private const string PARM_PATIENT_RESPONSE = "@PatientResponse";

        private const string PARM_QUICK_EMAIL_REMINDER_ID = "@QuickEmailReminderId";
        private const string PARM_EMAIL_TO = "@EmailTo";
        private const string PARM_EMAIL_FROM = "@EmailFrom";
        private const string PARM_SUBJECT = "@Subject";

        private const string PARM_APPOINTMENTSDATE = "@AppointmentsDate";

        private const string PARM_TELEVOXLOGDETAILID = "@TeleVoxLogDetailId";
        private const string PARM_TELEVOXLOGID = "@TeleVoxLogId";

        private const string PARM_PATIENTACCOUNTNUMBER = "@PatientAccountNumber";
        private const string PARM_FILENAME = "@FileName";
        private const string PARM_MESSAGEBODY = "@MessageBody";

        private const string PARM_TeleVoxMessageLogId = "@TeleVoxMessageLogId";
        private const string PARM_AppointmentDate = "@AppointmentDate";
        private const string PARM_AppointmentTime = "@AppointmentTime";
        private const string PARM_ClientsLastName = "@ClientsLastName";
        private const string PARM_ClientsFirstName = "@ClientsFirstName";
        private const string PARM_HomePhoneNumber = "@HomePhoneNumber";
        private const string PARM_MessageNumber = "@MessageNumber";
        private const string PARM_CallStatusDescription = "@CallStatusDescription";
        private const string PARM_ContactedDate = "@ContactedDate";
        private const string PARM_ContactedTime = "@ContactedTime";
        private const string PARM_MessageDescription = "@MessageDescription";
        private const string PARM_HouseCallsClientNumber = "@HouseCallsClientNumber";
        private const string PARM_ClientsNickName = "@ClientsNickName";
        private const string PARM_ClientsReferenceNumberfromImport = "@ClientsReferenceNumberfromImport";
        private const string PARM_CallStatusCode = "@CallStatusCode";
        private const string PARM_ProviderNumber = "@ProviderNumber";
        private const string PARM_ProviderName = "@ProviderName";
        private const string PARM_LocationNumber = "@LocationNumber";
        private const string PARM_LocationName = "@LocationName";
        private const string PARM_ReasonNumber = "@ReasonNumber";
        private const string PARM_ReasonName = "@ReasonName";
        private const string PARM_AppointmentId = "@AppointmentId";
        private const string PARM_AppointmentNotes = "@AppointmentNotes";
        private const string PARM_AppointmentColumn = "@AppointmentColumn";
        private const string PARM_ClientsBirthDate = "@ClientsBirthDate";
        private const string PARM_ClientNotes = "@ClientNotes";
        private const string PARM_NumberofCallAttempts = "@NumberofCallAttempts";
        private const string PARM_ClientAddress = "@ClientAddress";
        private const string PARM_ClientCity = "@ClientCity";
        private const string PARM_ClientState = "@ClientState";
        private const string PARM_ClientZip = "@ClientZip";
        private const string PARM_EmailAddress = "@EmailAddress";
        private const string PARM_EmailStatus = "@EmailStatus";
        private const string PARM_EmailedDate = "@EmailedDate";
        private const string PARM_EmailedTime = "@EmailedTime";
        private const string PARM_SMSPhone = "@SMSPhone";
        private const string PARM_SMSStatus = "@SMSStatus ";
        private const string PARM_SMSDeliveredDate = "@SMSDeliveredDate";
        private const string PARM_SMSDeliveredTime = "@SMSDeliveredTime";
        private const string PARM_ReminderSetBy = "@ReminderSetBy";


        #endregion

        #region "Support Functions"

        private void createParameters(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATE_ID, ds.RemindersTemplate.RemindersTemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATE_ID, ds.RemindersTemplate.RemindersTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_REMINDERS_TEMPLATE_NAME, ds.RemindersTemplate.RemindersTemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_TEMPLATE_TYPE_ID, ds.RemindersTemplate.TemplateTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_HTML_TEMPLATE, ds.RemindersTemplate.HTMLTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.RemindersTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.RemindersTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.RemindersTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.RemindersTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.RemindersTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_PROVIDER_IDS, ds.RemindersTemplate.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds.RemindersTemplate.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_REMINDER_TEMPLATE_TYPE, ds.RemindersTemplate.ReminderTemplateTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_HTML_TEMPLATE_WITH_IDS, ds.RemindersTemplate.HTMLTemplateWithIdsColumn.ColumnName, DbType.String);

        }
        public void createParametersForNoteType(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATES_TYPE_ID, ds.RemindersTemplateType.RemindersTemplateTypeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATES_TYPE_ID, ds.RemindersTemplateType.RemindersTemplateTypeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORTNAME, ds.RemindersTemplateType.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.RemindersTemplateType.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_CREATED_BY, ds.RemindersTemplateType.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.RemindersTemplateType.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.RemindersTemplateType.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.RemindersTemplateType.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        public void createParametersforemailsettings(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, ds.ReminderEmailSetting.ReminderEmailSettingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, ds.ReminderEmailSetting.ReminderEmailSettingIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SENDER_EMAIL, ds.ReminderEmailSetting.SenderEmailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.ReminderEmailSetting.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.ReminderEmailSetting.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.ReminderEmailSetting.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.ReminderEmailSetting.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.ReminderEmailSetting.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MESSAGE_TEMPLATE, ds.ReminderEmailSetting.MessageTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.ReminderEmailSetting.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_DELIVERY_DATE_TIME, ds.ReminderEmailSetting.DeliveryDateTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_FACILITY_IDs, ds.ReminderEmailSetting.FacilityidsColumn.ColumnName, DbType.String);
        }
        private void createParametersTextSettings(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(17);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, ds.ReminderTextSetting.ReminderTextSettingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, ds.ReminderTextSetting.ReminderTextSettingIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CALL_ID_NUMBER, ds.ReminderTextSetting.CallIDNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_REMINDER_FAILOVER, ds.ReminderTextSetting.IsReminderFailoverColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_MESSAGE_TEMPLATE, ds.ReminderTextSetting.MessageTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PROVIDER_ID, ds.ReminderTextSetting.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.ReminderTextSetting.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.ReminderTextSetting.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.ReminderTextSetting.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.ReminderTextSetting.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.ReminderTextSetting.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_REPLIES_EMAIL, ds.ReminderTextSetting.RepliesEmailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CONFIRMATION_MESSAGE, ds.ReminderTextSetting.ConfirmationMessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CANCELLATION_MESSAGE, ds.ReminderTextSetting.CancellationMessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_DELIVERY_DATE_TIME, ds.ReminderTextSetting.DeliveryDateTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_TIME_ZONE, ds.ReminderTextSetting.TimeZoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_TIME_ZONE_ID, ds.ReminderTextSetting.TimeZoneIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_FACILITY_IDs, ds.ReminderTextSetting.FacilityidsColumn.ColumnName, DbType.String);

        }
        private void createParametersVoiceSettings(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, ds.ReminderVoiceSetting.ReminderVoiceSettingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, ds.ReminderVoiceSetting.ReminderVoiceSettingIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CALL_ID_NUMBER, ds.ReminderVoiceSetting.CallIDNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_REPEAT_MESSAGE, ds.ReminderVoiceSetting.RepeatMessageColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_APP_CONFIRMATION_KEY, ds.ReminderVoiceSetting.AppConfirmationKeyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_APP_CANCEL_KEY, ds.ReminderVoiceSetting.AppCancelKeyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DAIL_IN_PIN, ds.ReminderVoiceSetting.DialInPinColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_TEXT_TO_SPEECH, ds.ReminderVoiceSetting.TextToSpeechColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MESSAGE_TEMPLATE, ds.ReminderVoiceSetting.MessageTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PROVIDER_ID, ds.ReminderVoiceSetting.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.ReminderVoiceSetting.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.ReminderVoiceSetting.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.ReminderVoiceSetting.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.ReminderVoiceSetting.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.ReminderVoiceSetting.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_DELIVERY_DATE_TIME, ds.ReminderVoiceSetting.DeliveryDateTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_TIME_ZONE, ds.ReminderVoiceSetting.TimeZoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_TIME_ZONE_ID, ds.ReminderVoiceSetting.TimeZoneIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(17, PARM_REMINDERS_TEMPLATE_ID, ds.ReminderVoiceSetting.RemindersTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_FACILITY_IDs, ds.ReminderVoiceSetting.FacilityidsColumn.ColumnName, DbType.String);

        }
        private void CreateParametersQuickVoiceReminder(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(26);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, ds.QuickVoiceReminder.QuickVoiceReminderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, ds.QuickVoiceReminder.QuickVoiceReminderIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_CALLEE_NAME, ds.QuickVoiceReminder.CalleeNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PHONE_NUMBER, ds.QuickVoiceReminder.PhoneNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MESSAGE_VOICE_ID, ds.QuickVoiceReminder.MessageVoiceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_LEADIN_VOICE_ID, ds.QuickVoiceReminder.LeadInVoiceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_LEADOUT_VOICE_ID, ds.QuickVoiceReminder.LeadOutVoiceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_MESSAGE_TEMPLATE, ds.QuickVoiceReminder.MessageTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_APPOINTMENT_ID, ds.QuickVoiceReminder.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_PATIENT_ID, ds.QuickVoiceReminder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_PROVIDER_ID, ds.QuickVoiceReminder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.QuickVoiceReminder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.QuickVoiceReminder.CreatedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(12, PARM_IS_PROCESSED, ds.QuickVoiceReminder.IsProcessedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.QuickVoiceReminder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.QuickVoiceReminder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_STATUS, ds.QuickVoiceReminder.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MESSAGE, ds.QuickVoiceReminder.MessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_RESPONSE_DELIVERY, ds.QuickVoiceReminder.ResponseDeliveryColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_REQUEST_DELIVERY, ds.QuickVoiceReminder.RequestDeliveryColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_KEY_PRESS, ds.QuickVoiceReminder.KeyPressColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(20, PARM_RETRIES, ds.QuickVoiceReminder.ReTriesColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(21, PARM_TIME_ZONE, ds.QuickVoiceReminder.TimeZoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_TIME_ZONE_ID, ds.QuickVoiceReminder.TimeZoneIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(23, PARM_GUARANTOR_PH_NUMBER, ds.QuickVoiceReminder.GuarantorPhNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_REPEAT_MESSAGE, ds.QuickVoiceReminder.RepeatMessageColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(25, PARM_ReminderSetBy, ds.QuickVoiceReminder.ReminderSetByColumn.ColumnName, DbType.String);

        }
        private void CreateParametersQuickEmailReminder(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(21);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUICK_EMAIL_REMINDER_ID, ds.QuickEmailReminder.QuickEmailReminderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUICK_EMAIL_REMINDER_ID, ds.QuickEmailReminder.QuickEmailReminderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_EMAIL_TO, ds.QuickEmailReminder.ToEmailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_EMAIL_FROM, ds.QuickEmailReminder.FromNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MESSAGE_TEMPLATE, ds.QuickEmailReminder.MessageTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_APPOINTMENT_ID, ds.QuickEmailReminder.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.QuickEmailReminder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_PROVIDER_ID, ds.QuickEmailReminder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.QuickEmailReminder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.QuickEmailReminder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.QuickEmailReminder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.QuickEmailReminder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_IS_PROCESSED, ds.QuickEmailReminder.IsProcessedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_STATUS, ds.QuickEmailReminder.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_REQUEST_DELIVERY, ds.QuickEmailReminder.RequestDeliveryColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_RESPONSE_DELIVERY, ds.QuickEmailReminder.ResponseDeliveryColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MESSAGE, ds.QuickEmailReminder.MessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_TIME_ZONE, ds.QuickEmailReminder.TimeZoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_TIME_ZONE_ID, ds.QuickEmailReminder.TimeZoneIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_SUBJECT, ds.QuickEmailReminder.SubjectColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_PATIENT_RESPONSE, ds.QuickEmailReminder.KeyPressColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(20, PARM_ReminderSetBy, ds.QuickEmailReminder.ReminderSetByColumn.ColumnName, DbType.String);

        }
        private void CreateParametersQuickSMSReminder(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(22);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, ds.QuickSMSReminder.QuickSMSReminderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, ds.QuickSMSReminder.QuickSMSReminderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CALLEE_NAME, ds.QuickSMSReminder.CalleeNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PHONE_NUMBER, ds.QuickSMSReminder.PhoneNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MESSAGE_TEMPLATE, ds.QuickSMSReminder.MessageTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_APPOINTMENT_ID, ds.QuickSMSReminder.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.QuickSMSReminder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_PROVIDER_ID, ds.QuickSMSReminder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.QuickSMSReminder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.QuickSMSReminder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.QuickSMSReminder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.QuickSMSReminder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_IS_PROCESSED, ds.QuickSMSReminder.IsProcessedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_STATUS, ds.QuickSMSReminder.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_REQUEST_DELIVERY, ds.QuickSMSReminder.RequestDeliveryColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_RESPONSE_DELIVERY, ds.QuickSMSReminder.ResponseDeliveryColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MESSAGE, ds.QuickSMSReminder.MessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_TIME_ZONE, ds.QuickSMSReminder.TimeZoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_TIME_ZONE_ID, ds.QuickSMSReminder.TimeZoneIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_KEY_PRESS, ds.QuickSMSReminder.KeyPressColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(19, PARM_GUARANTOR_PH_NUMBER, ds.QuickSMSReminder.GuarantorPhNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_VOICE_REMINDER_FAILOVER, ds.QuickSMSReminder.VoiceReminderFailoverColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(21, PARM_ReminderSetBy, ds.QuickSMSReminder.ReminderSetByColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "CRUD Reminders Template"
        public DSReminders loadRemindersTemplate(long reminderstemplateId, string providersIds, string reminderstemplatename, long templatetypeId, int? isActive, int pageNumber, int rowsPerPage, string ReminderTemplateType = "")
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ReminderTemplateType == "")
                    ReminderTemplateType = null;

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (reminderstemplateId <= 0)
                    dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATE_ID, reminderstemplateId);

                if (string.IsNullOrEmpty(reminderstemplatename))
                {
                    dbManager.AddParameters(1, PARM_REMINDERS_TEMPLATE_NAME, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_REMINDERS_TEMPLATE_NAME, reminderstemplatename);
                }

                if (string.IsNullOrEmpty(providersIds))
                {
                    dbManager.AddParameters(2, PARM_PROVIDER_IDS, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PROVIDER_IDS, providersIds);
                }


                //if (entityId <= 0)
                //    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                //else
                //    dbManager.AddParameters(4, PARM_ENTITY_ID, entityId);
                if (templatetypeId <= 0)
                    dbManager.AddParameters(3, PARM_TEMPLATE_TYPE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_TEMPLATE_TYPE_ID, templatetypeId);


                if (isActive == null)
                {
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, isActive);
                }

                if (pageNumber <= 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage <= 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.RemindersTemplate.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(8, PARM_REMINDER_TEMPLATE_TYPE, ReminderTemplateType);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATE_SELECT, ds, ds.RemindersTemplate.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::loadNoteTemplate", PROC_REMINDERS_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders updateRemindersTemplate(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATE_UPDATE, ds, ds.RemindersTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::UpdateNoteTemplate", PROC_REMINDERS_TEMPLATE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteRemindersTemplate(long templateID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REMINDERS_TEMPLATE_ID, templateID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::DeleteNoteTemplate", PROC_REMINDERS_TEMPLATE_DELETE, ex);
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

        public DSReminders insertRemindersTemplate(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATE_INSERT, ds, ds.RemindersTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertRemindersTemplate", PROC_REMINDERS_TEMPLATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //public DSClinicalNoteTemplateLookup InsertNewNoteType(DSClinicalNoteTemplateLookup ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        this.createParametersForNoteType(dbManager, ds, true);
        //        ds = (DSClinicalNoteTemplateLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATE_TYPE_INSERT, ds, ds.NotesTemplateType.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALReminders::InsertNewNoteType", PROC_REMINDERS_TEMPLATE_TYPE_INSERT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        #endregion

        #region "Email Settings"
        public DSReminders loadEmailSettings(long ProviderId, long emailID, string IsActive = "")
        {
            //  , string senderemail, string messagetemplate, long providerId, int? isActive, int pageNumber, int rowsPerPage
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (emailID <= 0)
                    dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, emailID);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.ReminderEmailSetting.TableName);
                tableNames.Add(ds.ReminderSettingsFacility.TableName);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDERS_EMAIL_SETTINGS_SELECT, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::loadEmailSettings", PROC_REMINDERS_EMAIL_SETTINGS_SELECT, ex);
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
        /// <param name="ProviderId"></param>
        /// <param name="emailID"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DSReminders loadEmailSettings(SharedVariable SharedVariable, long ProviderId, long emailID, string IsActive)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (emailID <= 0)
                    dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, emailID);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.ReminderEmailSetting.TableName);
                tableNames.Add(ds.ReminderSettingsFacility.TableName);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDERS_EMAIL_SETTINGS_SELECT, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::loadEmailSettings", PROC_REMINDERS_EMAIL_SETTINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders insertEmailSettings(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersforemailsettings(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMINDERS_EMAIL_SETTINGS_INSERT, ds, ds.ReminderEmailSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertEmailSettings", PROC_REMINDERS_EMAIL_SETTINGS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSReminders updateEmailSettings(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersforemailsettings(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REMINDERS_EMAIL_SETTINGS_UPDATE, ds, ds.ReminderEmailSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::updateEmailSettings", PROC_REMINDERS_EMAIL_SETTINGS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string deleteEmailSettings(long emailsettingID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REMINDERS_EMAIL_SETTINGS_ID, emailsettingID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REMINDERS_EMAIL_SETTINGS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::deleteEmailSettings", PROC_REMINDERS_EMAIL_SETTINGS_DELETE, ex);
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

        #region "Text Settings"

        public DSReminders loadRemindersTextSettings(long ProviderId, long TextId, string IsActive = "")
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (TextId <= 0)
                    dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, TextId);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.ReminderTextSetting.TableName);
                tableNames.Add(ds.ReminderSettingsFacility.TableName);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_TEXTSETTING_SELECT, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::loadNoteTemplate", PROC_REMINDER_TEXTSETTING_SELECT, ex);
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
        /// <param name="ProviderId"></param>
        /// <param name="TextId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DSReminders loadRemindersTextSettings(SharedVariable SharedVariable, long ProviderId, long TextId, string IsActive)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (TextId <= 0)
                    dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, TextId);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.ReminderTextSetting.TableName);
                tableNames.Add(ds.ReminderSettingsFacility.TableName);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_TEXTSETTING_SELECT, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::loadNoteTemplate", PROC_REMINDER_TEXTSETTING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders updateRemindersTextSettings(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersTextSettings(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REMINDER_TEXTSETTING_UPDATE, ds, ds.ReminderTextSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::updateRemindersTextSettings", PROC_REMINDER_TEXTSETTING_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteRemindersTextSettings(long ReminderTextSettingId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_REMINDER_TEXTSETTING_ID, ReminderTextSettingId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REMINDER_TEXTSETTING_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::deleteRemindersTextSettings", PROC_REMINDER_TEXTSETTING_DELETE, ex);
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

        public DSReminders insertRemindersTextSettings(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersTextSettings(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMINDER_TEXTSETTING_INSERT, ds, ds.ReminderTextSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertRemindersTextSettings", PROC_REMINDER_TEXTSETTING_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Voice Settings"

        public DSReminders loadRemindersVoiceSettings(long ProviderId, long VoiceId, string IsActive = "")
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (VoiceId <= 0)
                    dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, VoiceId);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.ReminderVoiceSetting.TableName);
                tableNames.Add(ds.ReminderSettingsFacility.TableName);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_VOICESETTING_SELECT, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::loadRemindersVoiceSettings", PROC_REMINDER_VOICESETTING_SELECT, ex);
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
        /// <param name="ProviderId"></param>
        /// <param name="VoiceId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DSReminders loadRemindersVoiceSettings(SharedVariable SharedVariable, long ProviderId, long VoiceId, string IsActive)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (VoiceId <= 0)
                    dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, VoiceId);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.ReminderVoiceSetting.TableName);
                tableNames.Add(ds.ReminderSettingsFacility.TableName);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_VOICESETTING_SELECT, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::loadRemindersVoiceSettings", PROC_REMINDER_VOICESETTING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders updateRemindersVoiceSettings(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersVoiceSettings(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REMINDER_VOICESETTING_UPDATE, ds, ds.ReminderVoiceSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::updateRemindersVoiceSettings", PROC_REMINDER_VOICESETTING_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteRemindersVoiceSettings(long ReminderVoiceSettingId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REMINDER_VOICESETTING_ID, ReminderVoiceSettingId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REMINDER_VOICESETTING_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::deleteRemindersVoiceSettings", PROC_REMINDER_VOICESETTING_DELETE, ex);
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

        public DSReminders insertRemindersVoiceSettings(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersVoiceSettings(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMINDER_VOICESETTING_INSERT, ds, ds.ReminderVoiceSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertRemindersVoiceSettings", PROC_REMINDER_VOICESETTING_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Lookups
        public DSClinicalNoteTemplateLookup GetRemindersTemplateTagCategory()
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_NOTE_CATEGORY_ID, null);
                dbManager.AddParameters(1, PARM_IS_REMINDER, "1");

                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_TAG_CATEGORY_LOOKUP, ds, ds.NotesTagCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetRemindersTemplateTagCategory", PROC_NOTE_TAG_CATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRemindersLookup GetRemindersTemplateType(string type)
        {
            DSRemindersLookup ds = new DSRemindersLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (type == "")
                    type = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_REMINDER_TEMPLATE_TYPE, type);

                ds = (DSRemindersLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATE_TYPE_LOOKUP, ds, ds.RemindersTemplateType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetRemindersTemplateType", PROC_REMINDERS_TEMPLATE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRemindersLookup GetWeekDays()
        {
            DSRemindersLookup ds = new DSRemindersLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(0);

                //   dbManager.AddParameters(0, PARM_REMINDER_TEMPLATE_TYPE, null);
                // dbManager.AddParameters(1, PARM_IS_REMINDER, "1");

                ds = (DSRemindersLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_WEEK_DAYS_LOOKUP, ds, ds.WeekDays.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetReminderConfirmationKey", PROC_WEEK_DAYS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRemindersLookup GetReminderConfirmationKey()
        {
            DSRemindersLookup ds = new DSRemindersLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(0);

                //   dbManager.AddParameters(0, PARM_REMINDER_TEMPLATE_TYPE, null);
                // dbManager.AddParameters(1, PARM_IS_REMINDER, "1");

                ds = (DSRemindersLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_CONFIRMATION_KEY, ds, ds.ReminderConfirmationKey.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetReminderConfirmationKey", PROC_REMINDER_CONFIRMATION_KEY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRemindersLookup GetRemindersType()
        {
            DSRemindersLookup ds = new DSRemindersLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRemindersLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_TYPE_LOOKUP, ds, ds.TemplateType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetRemindersType", PROC_REMINDER_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRemindersLookup GetRemindersTextVoice()
        {
            DSRemindersLookup ds = new DSRemindersLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRemindersLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_TEXT_VOICE_LOOKUP, ds, ds.RemindersTextVoice.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetRemindersTextVoice", PROC_REMINDER_TEXT_VOICE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders InsertNewNoteType(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                this.createParametersForNoteType(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMINDERS_TEMPLATES_TYPE_INSERT, ds, ds.RemindersTemplateType.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::InsertNewNoteType", PROC_REMINDERS_TEMPLATES_TYPE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRemindersLookup GetReminderDeliveryDateTime()
        {
            DSRemindersLookup ds = new DSRemindersLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRemindersLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_DELIVERY_DATE_TIME_LOOKUP, ds, ds.ReminderDeliveryDateTime.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetReminderDeliveryDateTime", PROC_REMINDER_DELIVERY_DATE_TIME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalNoteTemplateLookup GetReminderTemplateTagName(Int32 NoteTagCategory)
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTES_TAG_CATEGORY_ID, NoteTagCategory);
                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDER_TAG_NAME_LOOKUP, ds, ds.NotesTagName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetReminderTemplateTagName", PROC_REMINDER_TAG_NAME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region " Quick Reminders "
        public DSReminders loadQuickEmail(long ProviderId)
        {

            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                int isActive = 1;
                dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);




                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMINDERS_EMAIL_SETTINGS_SELECT, ds, ds.ReminderEmailSetting.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::loadEmailSettings", PROC_REMINDERS_EMAIL_SETTINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSReminders InsertQuickVoiceReminder(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuickVoiceReminder(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_INSERT, ds, ds.QuickVoiceReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_INSERT, ex);
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
        public DSReminders InsertQuickVoiceReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersQuickVoiceReminder(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_INSERT, ds, ds.QuickVoiceReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::insertQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders UpdateQuickVoiceReminder(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuickVoiceReminder(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_UPDATE, ds, ds.QuickVoiceReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::UpdateQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_UPDATE, ex);
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
        public DSReminders UpdateQuickVoiceReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersQuickVoiceReminder(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_UPDATE, ds, ds.QuickVoiceReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::UpdateQuickVoiceReminder Windows service", PROC_QUICK_VOICE_REMINDER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteQuickVoiceReminder(long QuickVoiceReminderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, QuickVoiceReminderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::DeleteQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_DELETE, ex);
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
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="QuickVoiceReminderId"></param>
        /// <returns></returns>
        public string DeleteQuickVoiceReminder(SharedVariable SharedVariable, long QuickVoiceReminderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, QuickVoiceReminderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::DeleteQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_DELETE, ex);
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
        public DSReminders LoadQuickVoiceReminder(long QuickVoiceReminderId, string IsProcessed, long PatientId, long AppointmentId, long ProviderId,bool IsLastWeekRecordsOnly , long PageNumber, long RowspPage)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (QuickVoiceReminderId == 0)
                    dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, QuickVoiceReminderId);

                if (string.IsNullOrEmpty(IsProcessed))
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, null);
                else
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, IsProcessed);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                if (AppointmentId == 0)
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, AppointmentId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowspPage);
                dbManager.AddParameters(7, PARM_IS_LAST_WEEK_RECORDS_ONLY, IsLastWeekRecordsOnly);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.QuickVoiceReminder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_SELECT, ds, ds.QuickVoiceReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::LoadQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_SELECT, ex);
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
        /// <param name="QuickVoiceReminderId"></param>
        /// <param name="IsProcessed"></param>
        /// <param name="PatientId"></param>
        /// <param name="AppointmentId"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        public DSReminders LoadQuickVoiceReminder(SharedVariable SharedVariable, long QuickVoiceReminderId, string IsProcessed, long PatientId, long AppointmentId, long ProviderId,bool IsLastWeekRecordsOnly , long PageNumber, long RowspPage)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (QuickVoiceReminderId == 0)
                    dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUICK_VOICE_REMINDER_ID, QuickVoiceReminderId);

                if (string.IsNullOrEmpty(IsProcessed))
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, null);
                else
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, IsProcessed);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                if (AppointmentId == 0)
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, AppointmentId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowspPage);
                dbManager.AddParameters(7, PARM_IS_LAST_WEEK_RECORDS_ONLY, IsLastWeekRecordsOnly);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.QuickVoiceReminder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUICK_VOICE_REMINDER_SELECT, ds, ds.QuickVoiceReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::LoadQuickVoiceReminder", PROC_QUICK_VOICE_REMINDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSReminders InsertQuickSMSReminder(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuickSMSReminder(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_INSERT, ds, ds.QuickSMSReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertQuickSMSReminder", PROC_QUICK_SMS_REMINDER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSReminders InsertQuickSMSReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersQuickSMSReminder(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_INSERT, ds, ds.QuickSMSReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::insertQuickSMSReminder", PROC_QUICK_SMS_REMINDER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSReminders InsertQuickEmailReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersQuickEmailReminder(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_INSERT, ds, ds.QuickEmailReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::InsertQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders InsertQuickEmailReminder(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuickEmailReminder(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_INSERT, ds, ds.QuickEmailReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::InsertQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSReminders UpdateQuickEmailReminder(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuickEmailReminder(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_UPDATE, ds, ds.QuickEmailReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::UpdateQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_UPDATE, ex);
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
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSReminders UpdateQuickEmailReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersQuickEmailReminder(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_UPDATE, ds, ds.QuickEmailReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::UpdateQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteQuickEmailReminder(long QuickEmailReminderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUICK_EMAIL_REMINDER_ID, QuickEmailReminderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::DeleteQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_DELETE, ex);
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
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="QuickEmailReminderId"></param>
        public string DeleteQuickEmailReminder(SharedVariable SharedVariable, long QuickEmailReminderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUICK_EMAIL_REMINDER_ID, QuickEmailReminderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::DeleteQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_DELETE, ex);
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
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="QuickEmailReminderId"></param>
        /// <param name="IsProcessed"></param>
        /// <param name="PatientId"></param>
        /// <param name="AppointmentId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSReminders LoadQuickEmailReminder(SharedVariable SharedVariable, long QuickEmailReminderId, string IsProcessed, long PatientId, long AppointmentId, long ProviderId, int PageNumber, int RowsPerPage)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (QuickEmailReminderId == 0)
                    dbManager.AddParameters(0, PARM_QUICK_EMAIL_REMINDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUICK_EMAIL_REMINDER_ID, QuickEmailReminderId);

                if (string.IsNullOrEmpty(IsProcessed))
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, null);
                else
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, IsProcessed);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                if (AppointmentId == 0)
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, AppointmentId);

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

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.QuickEmailReminder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUICK_EMAIL_REMINDER_SELECT, ds, ds.QuickEmailReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::LoadQuickEmailReminder", PROC_QUICK_EMAIL_REMINDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReminders UpdateQuickSMSReminder(DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuickSMSReminder(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_UPDATE, ds, ds.QuickSMSReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::UpdateQuickSMSReminder", PROC_QUICK_SMS_REMINDER_UPDATE, ex);
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
        public DSReminders UpdateQuickSMSReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersQuickSMSReminder(dbManager, ds, false);
                ds = (DSReminders)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_UPDATE, ds, ds.QuickSMSReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::UpdateQuickSMSReminder", PROC_QUICK_SMS_REMINDER_UPDATE, ex);
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
        /// <param name="QuickSMSReminderId"></param>
        /// <returns></returns>
        public string DeleteQuickSMSReminder(SharedVariable SharedVariable, long QuickSMSReminderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, QuickSMSReminderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::DeleteQuickSMSReminder", PROC_QUICK_SMS_REMINDER_DELETE, ex);
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

        public string DeleteQuickSMSReminder(long QuickSMSReminderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, QuickSMSReminderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::DeleteQuickSMSReminder", PROC_QUICK_SMS_REMINDER_DELETE, ex);
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

        public DSReminders LoadQuickSMSReminder(long QuickSMSReminderId, string IsProcessed, long PatientId, long AppointmentId, long ProviderId, int PageNumber, int RowsPerPage)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (QuickSMSReminderId == 0)
                    dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, QuickSMSReminderId);

                if (string.IsNullOrEmpty(IsProcessed))
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, null);
                else
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, IsProcessed);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                if (AppointmentId == 0)
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, AppointmentId);

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

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.QuickSMSReminder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_SELECT, ds, ds.QuickSMSReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::LoadQuickSMSReminder", PROC_QUICK_SMS_REMINDER_SELECT, ex);
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
        /// <param name="QuickSMSReminderId"></param>
        /// <param name="IsProcessed"></param>
        /// <param name="PatientId"></param>
        /// <param name="AppointmentId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSReminders LoadQuickSMSReminder(SharedVariable SharedVariable, long QuickSMSReminderId, string IsProcessed, long PatientId, long AppointmentId, long ProviderId, int PageNumber, int RowsPerPage)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (QuickSMSReminderId == 0)
                    dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUICK_SMS_REMINDER_ID, QuickSMSReminderId);

                if (string.IsNullOrEmpty(IsProcessed))
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, null);
                else
                    dbManager.AddParameters(1, PARM_IS_PROCESSED, IsProcessed);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                if (AppointmentId == 0)
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_APPOINTMENT_ID, AppointmentId);

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

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.QuickSMSReminder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUICK_SMS_REMINDER_SELECT, ds, ds.QuickSMSReminder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::LoadQuickSMSReminder", PROC_QUICK_SMS_REMINDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region " Reminder Setting "

        public ReminderSetting loadReminderSetting(long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            ReminderSetting model = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REMINDER_SETTING_SELECT);

                while (reader.Read())
                {
                    model = new ReminderSetting();
                    model.ReminderSettingId = !String.IsNullOrEmpty(reader["ReminderSettingId"].ToString()) ? reader["ReminderSettingId"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";
                    model.APIKey = !String.IsNullOrEmpty(reader["APIKey"].ToString()) ? reader["APIKey"].ToString() : "";
                    model.TimeZone = !String.IsNullOrEmpty(reader["TimeZone"].ToString()) ? reader["TimeZone"].ToString() : "";
                    model.CalleeName = !String.IsNullOrEmpty(reader["CalleeName"].ToString()) ? reader["CalleeName"].ToString() : "";
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::loadReminderSetting", PROC_REMINDER_SETTING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

            return model;
        }

        public ReminderSetting loadReminderSetting(SharedVariable SharedVariable, long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;
            ReminderSetting model = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REMINDER_SETTING_SELECT);

                while (reader.Read())
                {
                    model = new ReminderSetting();
                    model.ReminderSettingId = !String.IsNullOrEmpty(reader["ReminderSettingId"].ToString()) ? reader["ReminderSettingId"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";
                    model.APIKey = !String.IsNullOrEmpty(reader["APIKey"].ToString()) ? reader["APIKey"].ToString() : "";
                    model.TimeZone = !String.IsNullOrEmpty(reader["TimeZone"].ToString()) ? reader["TimeZone"].ToString() : "";
                    model.CalleeName = !String.IsNullOrEmpty(reader["CalleeName"].ToString()) ? reader["CalleeName"].ToString() : "";
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::loadReminderSetting", PROC_REMINDER_SETTING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

            return model;
        }
        #endregion

        // ----------------------------------------------------------- TeleVox ----------------------------------------------- //

        #region TeleVox

        public DSReminders LoadTeleVoxFutureAppointments(SharedVariable SharedVariable)
        {
            DSReminders ds = new DSReminders();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_APPOINTMENTSDATE, DateTime.Now.AddDays(2));
                ds = (DSReminders)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TELEVOXAPPOINTMENTS, ds, ds.TeleVoxRequestedData.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::LoadTeleVoxFutureAppointments", PROC_TELEVOXAPPOINTMENTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        private void CreateParametersTeleVoxLog(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TELEVOXLOGID, ds.TeleVoxLog.TeleVoxLogIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TELEVOXLOGID, ds.TeleVoxLog.TeleVoxLogIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(2, PARM_FILENAME, ds.TeleVoxLogDetail.FileNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MESSAGEBODY, ds.TeleVoxLogDetail.MessageBodyColumn.ColumnName, DbType.String);
        }

        public DSReminders InsertTeleVoxLog(DSReminders ds, SharedVariable SharedVariable)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateParametersTeleVoxLog(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TELEVOXLOGINSERT, ds, ds.TeleVoxLog.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertTeleVoxLog", PROC_TELEVOXLOGINSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private void CreateParametersTeleVoxLogDetail(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TELEVOXLOGDETAILID, ds.TeleVoxLogDetail.TeleVoxLogDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TELEVOXLOGDETAILID, ds.TeleVoxLogDetail.TeleVoxLogDetailIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENTACCOUNTNUMBER, ds.TeleVoxLogDetail.PatientAccountNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_FILENAME, ds.TeleVoxLogDetail.FileNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MESSAGEBODY, ds.TeleVoxLogDetail.MessageBodyColumn.ColumnName, DbType.String);
        }
        public DSReminders InsertTeleVoxLogDetail(DSReminders ds, SharedVariable SharedVariable)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersTeleVoxLogDetail(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TELEVOXLOGDETAILINSERT, ds, ds.TeleVoxLogDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::insertTeleVoxLogDetail", PROC_TELEVOXLOGDETAILINSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSReminders UpdateTeleVoxAppointments(SharedVariable SharedVariable, DSReminders ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersTeleVoxAppointments(dbManager, ds, true);
                ds = (DSReminders)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TeleVoxAppointmentsHandle, ds, ds.TeleVoxMessageLog.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReminders::UpdateTeleVoxAppointments", PROC_TeleVoxAppointmentsHandle, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public void CreateParametersTeleVoxAppointments(IDBManager dbManager, DSReminders ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(42);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TeleVoxMessageLogId, ds.TeleVoxMessageLog.TeleVoxMessageLogIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TeleVoxMessageLogId, ds.TeleVoxMessageLog.TeleVoxMessageLogIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_AppointmentDate, ds.TeleVoxMessageLog.AppointmentDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_AppointmentTime, ds.TeleVoxMessageLog.AppointmentTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ClientsLastName, ds.TeleVoxMessageLog.ClientsLastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ClientsFirstName, ds.TeleVoxMessageLog.ClientsFirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_HomePhoneNumber, ds.TeleVoxMessageLog.HomePhoneNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MessageNumber, ds.TeleVoxMessageLog.MessageNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CallStatusDescription, ds.TeleVoxMessageLog.CallStatusDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ContactedDate, ds.TeleVoxMessageLog.ContactedDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_ContactedTime, ds.TeleVoxMessageLog.ContactedTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MessageDescription, ds.TeleVoxMessageLog.MessageDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_HouseCallsClientNumber, ds.TeleVoxMessageLog.HouseCallsClientNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ClientsNickName, ds.TeleVoxMessageLog.ClientsNickNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ClientsReferenceNumberfromImport, ds.TeleVoxMessageLog.ClientsReferenceNumberfromImportColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CallStatusCode, ds.TeleVoxMessageLog.CallStatusCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ProviderNumber, ds.TeleVoxMessageLog.ProviderNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ProviderName, ds.TeleVoxMessageLog.ProviderNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_LocationNumber, ds.TeleVoxMessageLog.LocationNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_LocationName, ds.TeleVoxMessageLog.LocationNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ReasonNumber, ds.TeleVoxMessageLog.ReasonNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ReasonName, ds.TeleVoxMessageLog.ReasonNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(21, PARM_AppointmentId, ds.TeleVoxMessageLog.AppointmentIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_AppointmentNotes, ds.TeleVoxMessageLog.AppointmentNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_AppointmentColumn, ds.TeleVoxMessageLog.AppointmentColumnColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_ClientsBirthDate, ds.TeleVoxMessageLog.ClientsBirthDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_ClientNotes, ds.TeleVoxMessageLog.ClientNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_NumberofCallAttempts, ds.TeleVoxMessageLog.NumberofCallAttemptsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_ClientAddress, ds.TeleVoxMessageLog.ClientAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_ClientCity, ds.TeleVoxMessageLog.ClientCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_ClientState, ds.TeleVoxMessageLog.ClientStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_ClientZip, ds.TeleVoxMessageLog.ClientZipColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_EmailAddress, ds.TeleVoxMessageLog.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_EmailStatus, ds.TeleVoxMessageLog.EmailStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_EmailedDate, ds.TeleVoxMessageLog.EmailedDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_EmailedTime, ds.TeleVoxMessageLog.EmailedTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARM_SMSPhone, ds.TeleVoxMessageLog.SMSPhoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_SMSStatus, ds.TeleVoxMessageLog.SMSStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(37, PARM_SMSDeliveredDate, ds.TeleVoxMessageLog.SMSDeliveredDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_SMSDeliveredTime, ds.TeleVoxMessageLog.SMSDeliveredTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(39, PARM_FILENAME, ds.TeleVoxMessageLog.FileNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(40, PARM_MESSAGEBODY, ds.TeleVoxMessageLog.MessageBodyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(41, PARM_CREATED_BY, ds.TeleVoxMessageLog.CreatedByColumn.ColumnName, DbType.String);
        }

        #endregion
    }
}
