using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Lookups;
using MDVision.Model.Clinical.Notes.Notes;
using MDVision.Model.Common;
using MDVision.Model.Patient;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALNotes
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALNotes"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALNotes()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALNotes(SharedVariable SharedVariable)
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

        private const string PROC_Notes_INSERT = "Clinical.sp_NotesInsert";
        private const string PROC_ORDERSET_COMPONENTS_DELETE = "Clinical.sp_DeleteOrderSetComponents";
        private const string PROC_Notes_DELETE = "Clinical.NotesDeleteWithComponents";
        private const string PROC_CHK_OS_WITH_NOTE = "Clinical.sp_chkNoteattachwithOrderSet";
        private const string PROC_Notes_SELECT = "Clinical.sp_NotesSelect";
        private const string PROC_Notes_SELECT_BY_NOTEID = "Clinical.sp_Note_By_NoteId";
        private const string PROC_NOTES_SELECT = "Clinical.sp_Notes_Select";
        private const string PROC_RECONCILEDMEDICATION_SELECT_FORNOTE = "Clinical.sp_ReconciledMedicationSelectForNote";
        private const string PROC_Notes_UPDATE = "Clinical.sp_NotesUpdate";
        private const string PROC_Notes_UPDATE_VISIT_TYPE = "Clinical.sp_UpdateVisitType";
        private const string PROC_UNSIGN_Note = "clinical.sp_UpdateNotesUnsignedStatus";
        private const string PROC_NOTES_LINKAPPOINTMENT = "Clinical.sp_LinkAppointment";
        private const string PROC_NOTES_COPYFROM_PREVIOUSNOTE = "Clinical.sp_CopyFromPreviousNote";
        private const string PROC_FOLLOWUP_APPOINTMENT_INSERT = "Clinical.sp_FollowUpAppointmentInsert";
        private const string PROC_GET_REFERENCE_DATA = "system.sp_ReferenceData";
        private const string PROC_PATIENT_FILL_NOTES = "Patient.sp_PatientFill_Notes";

        private const string PROC_PATIENT_MUSETTING_INSERT = "Patient.sp_PatientMUSettingInsert";
        private const string PROC_PATIENT_MUSETTING_UPDATE = "Patient.sp_PatientMUSettingUpdate";
        private const string PROC_PATIENT_MU_SETTING_SELECT = "Patient.sp_PatientMUSettingSelect";
        private const string PROC_PATIENT_VALUE_SETTING_LOOKUP = "system.ValueSettingLookup";
        private const string PROC_APPOINTMENTIDBYNOTEID = "Clinical.sp_AppointmentIdByNoteId";
        private const string PROC_PATIENT_MU_SETTING_WITH_MISSING_REFERENCE_INSERT = "Patient.sp_PatientMUSettingWithMissingReferenceInsert";

        private const string PROC_NOTESHXTABORDERUPDATE = "Clinical.sp_NotesHxtabOrderUpdate";
        private const string NOTE_AUTO_POPULATE_OPTIONS = "System.sp_HistoryAutoPopulateSelect";
        private const string PROC_NOTE_AUTO_POPULATE_OPTIONS_SELECT = "System.sp_HistoryAutoPopulateSelect";
        private const string PROC_NEW_INSERT_TABLES = "Clinical.GetNewInsertTables";
        private const string PROC_POP_NOTE_TEMPDATA = "Clinical.sp_PopulateNoteTemplateData";
        private const string PROC_DEATTACH_FROM_NOTE = "Clinical.sp_DetachComponentsFromNote";
        private const string PROC_NOTE_INFO_SELECT = "Clinical.sp_NoteInfoSelect";
        private const string PROC_NOTE_SYNDROMIC_SURV_SELECT = "Clinical.sp_HL7SyndromicSurveillanceSelect";
        private const string PROC_NOTE_WITH_NEW_TEMPLATE = "Clinical.sp_PopulateNoteWithNewTemplate";
        private const string PROC_IS_TODATS_NOTE_CREATED = "Clinical.sp_IsTodays_Note_Created";
        private const string PROC_ModifiedNotes_SELECT = "Clinical.sp_ModifiedNotesSelect";
        private const string PROC_MODIFIED_NOTE_REVIEWED = "Clinical.sp_ModifiedNotesReviewed";
        private const string PROC_GET_AMENDMENT_DATA = "Clinical.GetAmendmentData";
        private const string PROC_GET_AMENDMENT_DATA_REPORT = "Clinical.ProgressNoteAmendment";

        private const string PROC_NOTE_COMPONENT_INSERT = "Clinical.sp_NoteComponentsInsert";
        private const string PROC_NOTE_COMPONENT_UPDATE = "Clinical.sp_NoteComponentsUpdate";
        private const string PROC_NOTE_COMPONENT_SELECT = "Clinical.sp_NoteComponentsSelect";
        private const string PROC_NOTE_COMPONENT_DELETE = "Clinical.sp_NoteComponentsDelete";
        private const string PROC_NOTE_COMPONENTS_LOOKUP = "Clinical.sp_NoteComponentsLookup";
        private const string PROC_NOTE_SECTIONS_LOOKUP = "Clinical.sp_NoteSectionsLookup";
        private const string PROC_NOTE_COMPONENTS_ORDER = "Clinical.sp_SetNotesComponentsOrder";
        private const string PROC_NOTE_COMPONENTS_INSERT_BULK = "Clinical.sp_NoteComponentsInsert_Bulk";
        private const string PROC_NOTE_COMPONENT_AUDIT_SELECT = "Clinical.sp_NoteComponentAuditSelect";
        private const string PROC_NOTE_FOLLOWUP_COMPONENT_SELECT = "Clinical.sp_NoteFollowComponentSelect";


        private const string PROC_DASHBOARD_MODIFIED_NOTES_COUNT = "System.sp_DashBoardModifiedNotesCount";
        private const string PROC_LOAD_NOTES_DATES = "Clinical.sp_LoadVisitDate";

        private const string PROC_TEMPLATE_BY_ID_SELECT = "Clinical.sp_TemplateByIdSelect";

        private const string PROC_FETCH_ROS_TEMPLATE_DATA = "[Clinical].[FetchROSTemplateData]";
        //private const string PROC_FETCH_ROS_TEMPLATE_DATA = " [Clinical].[FetchROSRevampTemplateData]";

        private const string PROC_NOTES_ATTACHMENTS = "[Clinical].[sp_NotesAssociatedAttachments]";
        private const string PROC_NOTES_ATTACHMENTS_EXISTS = "Clinical.sp_NoteAttachmentExists";

        private const string PROC_NOTES_DOCS_CLASSICVIEW = "Clinical.sp_NoteDocsForClassicView";
        private const string PROC_NOTE_COMPONENT_SELECT_FOR_PDF = "Clinical.sp_NoteComponentsSelectForPDF";
        private const string PROC_NOTE_PDF_SELECT_DATA_FIX = "Clinical.IncorrectHeaderProgressNotes";
        private const string PROC_SIGN_NOTES = "Clinical.sp_SignNotes";
        private const string PROC_PATIENT_DOCUMENT_INSERT = "Patient.sp_PatientDocumentInsert";
        private const string PROC_SELECT_PREVIOUS_NOTE_PE_AND_ROS = "Clinical.sp_SelectPrevNotePEAndROS";
        private const string PROC_INSERT_ORDER_SET = "Clinical.sp_OrderSetAttachToNote";
        private const string PROC_SIGN_NOTES_MULTIPLE = "[Clinical].[sp_SignNotesMultiple]";
        private const string PROC_LOAD_NOTEDATA_FORPDF = "[Clinical].[sp_LoadNoteDataForPDF]";
        private const string PROC_NOTE_READYTOSIGN_INSERT = "[Clinical].[sp_NoteReadyToSignInsert]";
        private const string PROC_NOTE_COMPONENT_NAME_SELECT = "Clinical.sp_NoteComponentsNameSelect";
        private const string PROC_UPDATE_ISNONBILLABLE_INFO = "Clinical.sp_UpdateIsNonBillableInfo";
        private const string PROC_GET_ISNONBILLABLE_INFO = "Clinical.sp_getIsNonBillableInfo";
        

        #endregion

        #region Parameters
        private const string PARM_NOTE_ID = "@NotesId";
        private const string PARM_MUAlertsCount = "@MUAlertsCount";
        private const string PARM_COMPONENT_LOOKUP_IDS = "@ComponentLookupIDs";
        private const string PARM_COMPONENT_TYPE = "@ComponentType";
        private const string PARM_USERID = "@UserId";
        private const string PARM_ENTITYID = "@EntityId";
        private const string PARM_HXTABORDER = "@HxtabOrder";
        private const string PARM_CURRENT_NOTE_ID = "@CurrentNoteId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_NOTETEXT = "@NoteText";
        private const string PARM_TEMPLATE_TYPEID = "@TemplateTypeId";
        private const string PARM_TEMPLATEID = "@TemplateId";
        private const string PARM_VISIT_DATE = "@VisitDate";
        private const string PARM_VISIT_TIME = "@VisitTime";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_UNSIGNED_STATUS = "@UnSignedStatus";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_IS_NOTE_UPDATE = "@IsNoteUpdate";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_USER_ID = "@Userid";
        private const string PARM_PATIENTID = "@PatientId";

        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_ASSIGNED_TO_ID = "@AssignedTo";

        private const string PARM_NOTE_STATUS = "@NoteStatus";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_REF_PROVIDER_ID = "@RefProviderId";
        private const string PARM_LINKED_APPOINTMENT = "@LinkedAppointment";
        private const string PARM_VISIT_REASON_ID = "@VisitReasonId";
        private const string PARM_ROOMNO_ID = "@RoomNoId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_PREV_NOTE_DESCRIPTION = "@PrevNoteDescription";
        private const string PARM_PREV_NOTE_ID = "@PrevNoteId";
        private const string PARM_IS_PHONE_ENCOUNTER = "@IsPhoneEncounter";
        private const string PARM_ENCOUNTER_DURATION = "@Duration";
        private const string PARM_CPTCODE = "@CPTCode";

        private const string PARM_B_MEDRECONCILED = "@bMedReconciled";
        private const string PARM_MEDRECONCILEDID = "@MedReconciledId";
        private const string PARM_COME_FROM_COPY_NOTE = "@FromCopyNote";
        private const string PARM_IS_NON_BILABLE = "@IsNonBilable";
        private const string PARM_IS_HPI_COMPLAINT = "@IsHPIComplaint";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_RESOURCE_PROVIDER_ID = "@ResourceProviderId";
        private const string PARM_APPOINTMENT_DATE = "@AppointmentDate";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_SCHREASON_ID = "@SchReasonId";
        private const string PARM_FROM_TIME = "@FromTime";


        private const string PARM_PATIENT_MUSETTING_ID = "@PatientMUSettingId";
        private const string PARM_REFERRENCE_DATA_ID = "@ReferenceDataId";
        private const string PARM_VALUE_SETTING_ID = "@ValueSettingId";
        private const string PARM_TOP = "@Top";

        private const string PARM_VISIT_REASON_COMMENTS = "@VisitReasonComments";
        private const string PARM_SCHEDULE_REASON_COMMENTS = "@ScheduleReasonComments";
        private const string PARM_IS_NOTE_TEXT = "@IsNoteText";
        private const string PARM_MODE = "@mode";
        private const string PARM_NOTES_TEMPID = "@NotesTemplateId";
        private const string PARM_NOTES_ID = "@NoteId";
        private const string PARM_COMP_IDS = "@ComponentList";

        private const string PARM_ENCOUNTERTYPE = "@EncounterType";
        private const string PARM_CALLER = "@Caller";
        private const string PARM_RECEIVER = "@Receiver";
        private const string PARM_USERS_ID = "@UserIds";

        private const string PARM_NOTE_COMPONENT_ID = "@NoteComponentId";
        private const string PARM_NOTE_COMPONENT_LOOKUP_ID = "@NoteComponentsLookupId";
        private const string PARM_SOAP_TEXT = "@SOAPText";
        private const string PARM_ORDER_NO = "@OrderNo";
        private const string PARM_NOTE_SECTION_LOOKUP_ID = "@NoteSectionsLookupId";
        private const string PARM_NOTE_COMPONENT_XML = "@Data";
        private const string PARM_NOTE_COMPONENT_XML_OUTPUT = "@ReturnXML";
        private const string PARM_NOTE_COMPONENTS_IDS = "@NotesComponentIds";
        private const string PARM_VISIT_FROM = "@VisitFrom";
        private const string PARM_VISIT_TO = "@VisitTo";
        private const string PARM_VISIT_TYPE = "@VisitTypeId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_IS_REVIEWED = "@IsReviewed";
        private const string PARM_COMPONENT_NAME = "@ComponentName";

        private const string PARM_NOTE_COMPONENT_AUDIT_ID = "@NoteComponentAuditId";

        private const string PARM_ROS_DATA_TEMPLATE_ID = "@ROSDataTemplateId";
        private const string PARM_SECTION_NAME = "@SectionName";
        private const string PARM_NOTES_IDS = "@NotesIds";
        private const string PARM_FROM_CCM = "@FromCCM";
        private const string PARM_PATIENT_DOC_ID = "@PatDocId";
        private const string PARM_MEDICAL_DOC_ID = "@MedicalDocId";
        private const string PARM_DOCUMENT_ID = "@Documentid";
        private const string PARM_DOS = "@DOS";
        private const string PARM_CLAIM_ID = "@ClaimId";
        private const string PARM_CASE_ID = "@CaseId";
        private const string PARM_TRANSITION_ID = "@TransitionId";
        private const string PARM_FILE_TYPE = "@FileType";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_FILE_NAME = "@FileName";
        private const string PARM_FILE_STREAM = "@FileStream";
        private const string PARM_PAGES = "@Pages";
        private const string PARM_VIEW_BY = "@ViewBy";
        private const string PARM_VIEW_DATE = "@ViewDate";
        private const string PARM_REVIEW_BY = "@ReviewBy";
        private const string PARM_REVIEW_DATE = "@ReviewDate";
        private const string PARM_IS_ATTACHED = "@IsAttached";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_FROM_DOS = "@FromDOS";
        private const string PARM_TO_DOS = "@ToDOS";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_FROM_ENTRY_DATE = "@FromEntryDate";
        private const string PARM_TO_ENTRY_DATE = "@ToEntryDate";
        private const string PARM_ENTERED_BY = "@EnteredBy";
        private const string PARM_ASSIGNED_BY_ID = "@AssignedById";
        private const string PARM_REVIEWED_BY_ID = "@ReviewedById";
        private const string PARM_MESSAGE_ID = "@MessageId";
        private const string PARM_SIGN_BY = "@SignBy";
        private const string PARM_SIGN_DATE = "@SignDate";
        private const string PARM_B_UPDATE_STREAM = "@BUpdateStream";
        private const string PARM_ADVANCE_PAYMENT_ID = "@AdvPaymentId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_IS_FIRST_LOAD = "@IsFirstLoad";
        private const string PARM_REF_MODULE_NAME = "@RefModuleName";
        private const string PARM_IS_SCAN = "@IsScan";
        private const string PARM_URL = "@url";
        private const string PARM_FILE_URL = "@FileURL";
        private const string PARM_ORDER_SET_REFERRAL_ID = "@OrderSetReferralId";
        private const string PARM_ORDER_SET_EDUCATION_ID = "@OrderSetPatEducationId";
        private const string PARM_ENC_PHONE_FOLDER = "@PhoneEncounterFolder";
        private const string PARM_INSURANCE_IMAGE = "@InsuranceCardImage";
        private const string PARM_NOTE_HTML = "@DocumentHtml";
        private const string PARM_NOTE_DOCUMENT_ID = "@NoteDocumentId";
        private const string PARM_NOTEID = "@NoteId";
        private const string PARM_PATIENT_DOCUMENT_ID = "@PatientDocumentId";
        private const string PARM_NOTE_MISSING_DATA_REASON = "@NoteMissingDataReason";
        private const string PARM_CONFIRM_SIGN = "@ConfirmSign";
        private const string PARM_IS_AMENDMENT_FOR_BILLING = "@IsAmendmentForBilling";
        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_COPY_COMPONENTS = "@NoteComponentList";

        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void createParameters(IDBManager dbManager, DSNotes ds, Boolean IsInsert, string Type = "Notes", DataTable dtCopytComponents = null)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(43);
                dbManager.AddParameters(0, PARM_NOTE_ID, ds.Notes.NotesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }

            else
            {
                dbManager.CreateParameters(42);
                dbManager.AddParameters(0, PARM_NOTE_ID, ds.Notes.NotesIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.Notes.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Notes.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_NOTETEXT, ds.Notes.NoteTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_TEMPLATE_TYPEID, ds.Notes.TemplateTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_TEMPLATEID, ds.Notes.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_VISIT_DATE, ds.Notes.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_VISIT_TIME, ds.Notes.VisitTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.Notes.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.Notes.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.Notes.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.Notes.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.Notes.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(13, PARM_NOTE_STATUS, ds.Notes.NoteStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PROVIDER_ID, ds.Notes.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_FACILITY_ID, ds.Notes.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_REF_PROVIDER_ID, ds.Notes.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(17, PARM_LINKED_APPOINTMENT, ds.Notes.LinkedAppointmentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_VISIT_REASON_ID, ds.Notes.VisitReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_ROOMNO_ID, ds.Notes.RoomsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(20, PARM_ENTITY_ID, ds.Notes.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(21, PARM_PATIENT_ID, ds.Notes.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(22, PARM_VISIT_ID, ds.Notes.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(23, PARM_PREV_NOTE_DESCRIPTION, ds.Notes.PrevNoteDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_PREV_NOTE_ID, ds.Notes.PrevNotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(25, PARM_IS_PHONE_ENCOUNTER, ds.Notes.IsPhoneEncounterColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(26, PARM_ENCOUNTER_DURATION, ds.Notes.DurationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_CPTCODE, ds.Notes.CPTCodeColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(28, PARM_B_MEDRECONCILED, ds.Notes.bMedReconciledColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(29, PARM_MEDRECONCILEDID, ds.Notes.MedReconciledIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_CALLER, ds.Notes.CallerColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_RECEIVER, ds.Notes.ReceiverColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_USERS_ID, ds.Notes.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(33, PARM_ENCOUNTERTYPE, ds.Notes.EncounterTypeColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(34, PARM_VISIT_REASON_COMMENTS, ds.Notes.VisitReasonCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARM_RESOURCE_ID, ds.Notes.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(36, PARM_RESOURCE_PROVIDER_ID, ds.Notes.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(37, PARM_IS_NON_BILABLE, ds.Notes.IsNonBilableColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(38, PARM_VISIT_TYPE, ds.Notes.VisitTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(39, PARM_APPOINTMENT_ID, ds.Notes.AppointmentIdColumn.ColumnName, DbType.Int64);
            if (IsInsert == false)
            {

                dbManager.AddParameters(40, PARM_COME_FROM_COPY_NOTE, ds.Notes.ComeFromCopyNoteColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(41, PARM_IS_AMENDMENT_FOR_BILLING, ds.Notes.IsAmendmentForBillingColumn.ColumnName, DbType.Byte);
            }
            else
            {

                dbManager.AddParameters(40, PARM_IS_HPI_COMPLAINT, ds.Notes.IsHPIComplaintColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(41, PARM_MUAlertsCount, ds.Notes.MUAlertsCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (dtCopytComponents != null)
                    dbManager.AddParameters(42, PARM_COPY_COMPONENTS, dtCopytComponents);
            }

        }

        private void createUnSignParameters(IDBManager dbManager, DSNotes ds)
        {
            int i = 0;

            dbManager.CreateParameters(6);
            dbManager.AddParameters(i++, PARM_NOTE_ID, ds.Notes.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_UNSIGNED_STATUS, ds.Notes.UnSignedStatusColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, ds.Notes.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, ds.Notes.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
            dbManager.AddParameters(i++, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);


        }



        private void CreateParametersForInsertPatientMUSetting(IDBManager dbManager, DSNotes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PATIENT_MUSETTING_ID, ds.PatientMUSetting.PatientMUSettingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENT_MUSETTING_ID, ds.PatientMUSetting.PatientMUSettingIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_NOTE_ID, ds.PatientMUSetting.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientMUSetting.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_REFERRENCE_DATA_ID, ds.PatientMUSetting.ReferenceDataIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_VALUE_SETTING_ID, ds.PatientMUSetting.ValueSettingIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.PatientMUSetting.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.PatientMUSetting.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.PatientMUSetting.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.PatientMUSetting.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }


        private void ParamsForSeperateComponentInsert(IDBManager dbManager, NoteComponentModel model, Boolean IsInsert, string UserName = null)
        {
            if (UserName == null)
            {
                UserName = MDVSession.Current.AppUserName;
            }

            if (IsInsert == true)
                dbManager.AddParameters(PARM_NOTE_COMPONENT_ID, model.NoteComponentId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_NOTE_COMPONENT_ID, model.NoteComponentId, DbType.Int64);

            dbManager.AddParameters(PARM_NOTE_COMPONENT_LOOKUP_ID, model.NoteComponentsLookupId);
            dbManager.AddParameters(PARM_NOTE_ID, model.NotesId);
            dbManager.AddParameters(PARM_SOAP_TEXT, model.SOAPText);
            dbManager.AddParameters(PARM_ORDER_NO, model.OrderNo);
            dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(UserName));
            dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(UserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            if (model.NoteSectionsLookupId == 0)
            {
                dbManager.AddParameters(PARM_NOTE_SECTION_LOOKUP_ID, DBNull.Value);
            }
            else
            {
                dbManager.AddParameters(PARM_NOTE_SECTION_LOOKUP_ID, model.NoteSectionsLookupId);
            }

        }


        #endregion

        #region "Insert, delete, update and get Notes using dataset Functions"
        /// <summary>
        /// Loads the Clinical.
        /// </summary>
        /// <param name="ClinicalId">The Clinical identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="AccountNumber">The account number.</param>
        /// <param name="SSN">The SSN.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        ///
        public DSNotes loadClinical_Notes_CaseReports(long PatientId, long NoteId, long AppointmentID, Int32 PageNumber, Int32 RowsPerPage, string isViewNotes = "", string isPrintNotes = "", int isPhoneEncounter = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Notes;
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Notes.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //if (isPhoneEncounter == 1)
                //    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, isPhoneEncounter);
                //else
                //    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, null);
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Clinical.sp_NotesSelect_CaseReporting", ds, ds.Notes.TableName);
                if (dtTemp != null && ds.Notes.Rows.Count > 0)
                {
                    if (isViewNotes == "1" || isPrintNotes == "1")
                    {
                        bool isViewAction = isViewNotes == "1" ? true : false;
                        bool isPrintAcion = isPrintNotes == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", "Clinical.sp_NotesSelect_CaseReporting", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSNotes loadClinical_Notes(long PatientId, long NoteId, long AppointmentID, Int32 PageNumber, Int32 RowsPerPage, string isViewNotes = "", string isPrintNotes = "", int isPhoneEncounter = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Notes;
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Notes.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //if (isPhoneEncounter == 1)
                //    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, isPhoneEncounter);
                //else
                //    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, null);
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Notes_SELECT, ds, ds.Notes.TableName);
                if (dtTemp != null && ds.Notes.Rows.Count > 0)
                {
                    if (isViewNotes == "1" || isPrintNotes == "1")
                    {
                        bool isViewAction = isViewNotes == "1" ? true : false;
                        bool isPrintAcion = isPrintNotes == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_Notes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSNotes loadClinical_Notes(IDBManager dbManager, long PatientId, long NoteId, long AppointmentID, Int32 PageNumber, Int32 RowsPerPage, string isViewNotes = "", string isPrintNotes = "", int isPhoneEncounter = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSNotes ds = new DSNotes();
            try
            {
                DataTable dtTemp = ds.Notes;
                dbManager.CreateParameters(5);

                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Notes.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Notes_SELECT, ds, ds.Notes.TableName);
                if (dtTemp != null && ds.Notes.Rows.Count > 0)
                {
                    if (isViewNotes == "1" || isPrintNotes == "1")
                    {
                        bool isViewAction = isViewNotes == "1" ? true : false;
                        bool isPrintAcion = isPrintNotes == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_Notes_SELECT, ex);
                throw ex;
            }
        }


        public DSAppointment loadModifiedNotes(string VisitFrom, string VisitTo, int Status, Int64 ProviderId, Int32 PageNumber = 0, Int32 RowsPerPage = 0)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            DSAppointment ds = new DSAppointment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.Notes;
                dbManager.Open();
                dbManager.CreateParameters(8);

                if (VisitFrom == "")
                    dbManager.AddParameters(0, PARM_VISIT_FROM, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_VISIT_FROM, VisitFrom);

                if (VisitTo == "")
                    dbManager.AddParameters(1, PARM_VISIT_TO, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_VISIT_TO, VisitTo);


                dbManager.AddParameters(2, PARM_STATUS, Status);
                if (ProviderId <= 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);



                if (PageNumber <= 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.AppointmentsVisits.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSAppointment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ModifiedNotes_SELECT, ds, ds.AppointmentsVisits.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALNotes::loadModifiedNotes", PROC_ModifiedNotes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ClinicalNotesInfo> loadClinical_NoteInfo(long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_NOTES_ID, NoteId);
                return dbManager.ExecuteReaders<ClinicalNotesInfo>(PROC_NOTE_INFO_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::loadClinical_NoteInfo", PROC_NOTE_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public Syndromic GetSyndromicSurveillanceData(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                var resultSet = dbManager.ExecuteReadersForMultiResultSets<Syndromic>(PROC_NOTE_SYNDROMIC_SURV_SELECT, typeof(SyndromicPatientModel), typeof(SyndromicNotesModel), typeof(SyndromicProviderModel), typeof(SyndromicFacilityModel), typeof(SyndromicObservationModel), typeof(SyndromicVitalsModel));
                return resultSet;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetSyndromicSurveillanceData", PROC_NOTE_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public ClinicalNotesModel loadClinical_Note_By_NoteID(long PatientId, long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                ClinicalNotesModel model = new ClinicalNotesModel();
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_Notes_SELECT_BY_NOTEID, parameters))
                {
                    while (reader.Read())
                    {
                        model.NotesId = MDVUtility.ToStr(reader["NotesId"]);
                        model.Duration = MDVUtility.ToStr(reader["Duration"]);
                        model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                        model.NoteText = MDVUtility.ToStr(reader["NoteText"]);
                        model.NoteType = MDVUtility.ToStr(reader["TemplateTypeId"]);
                        model.NoteTemplate = MDVUtility.ToStr(reader["TemplateId"]);
                        model.VisitDate = MDVUtility.ToStr(reader["VisitDate"]) != "" ? MDVUtility.ToDateTime(reader["VisitDate"]).ToShortDateString() : "";
                        model.VisitTime = MDVUtility.ToStr(reader["VisitTime"]);
                        model.NoteStatus = MDVUtility.ToStr(reader["NoteStatus"]);
                        model.SignedBy = MDVUtility.ToStr(reader["SignedBy"]);
                        model.Provider = MDVUtility.ToStr(reader["ProviderName"]);
                        model.ProviderFullName = MDVUtility.ToStr(reader["ProviderFullName"]);
                        model.ProviderId = MDVUtility.ToStr(reader["ProviderId"]);
                        model.FacilityName = model.Facility = MDVUtility.ToStr(reader["FacilityName"]);
                        model.FacilityDescription = MDVUtility.ToStr(reader["FacilityDescription"]);
                        model.FacilityId = MDVUtility.ToStr(reader["FacilityId"]);
                        model.RefProvider = MDVUtility.ToStr(reader["RefProviderName"]);
                        model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                        model.AppointmentID = MDVUtility.ToStr(reader["LinkedAppointmentId"]);
                        model.LinkedAppointment = MDVUtility.ToStr(reader["LinkedAppointment"]);
                        model.IsLinkedAppointment = string.IsNullOrEmpty(MDVUtility.ToStr(reader["LinkedAppointment"])) ? false : true;
                        model.VisitReason = MDVUtility.ToStr(reader["VisitReasonComments"]);
                        model.VisitReasonId = MDVUtility.ToStr(reader["SchReasonId"]);
                        model.RoomNo = MDVUtility.ToStr(reader["RoomsId"]);
                        model.CopayPreviousNote = MDVUtility.ToStr(reader["PrevNoteDescription"]);
                        model.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                        model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                        model.ChiefComplaint = MDVUtility.ToStr(reader["ChiefComplaint"]);
                        model.IsPhoneEncounter = MDVUtility.ToBool(reader["IsPhoneEncounter"]);
                        model.TemplateName = MDVUtility.ToStr(reader["TemplateName"]);
                        model.TemplateTypeName = MDVUtility.ToStr(reader["TemplateTypeName"]);
                        model.VisitType = MDVUtility.ToStr(reader["VisitType"]);
                        model.FacilityPOSCode = MDVUtility.ToStr(reader["FacilityPOSCode"]);
                        model.FacilityPOSDesc = MDVUtility.ToStr(reader["FacilityPOSDesc"]);
                        model.ROSDataTemptId = MDVUtility.ToStr(reader["ROSDataTemptId"]);
                        model.BillingInfoId = MDVUtility.ToStr(reader["BillingInfoId"]);
                        model.bMedReconciled = !string.IsNullOrEmpty(MDVUtility.ToStr(reader["bMedReconciled"])) && MDVUtility.ToStr(reader["bMedReconciled"]).ToLower() == "true" ? "1" : "0";
                        model.MedReconciledId = MDVUtility.ToStr(reader["MedReconciledId"]);
                        model.HxtabOrder = MDVUtility.ToStr(reader["HxtabOrder"]);
                        model.IsNonBilable = MDVUtility.ToBool(reader["IsNonBilable"]);
                        model.PEDataTemptId = MDVUtility.ToStr(reader["PEDataTemptId"]);
                        model.ROSTemplateId = MDVUtility.ToStr(reader["ROSTemplateId"]);
                        model.PETemplateId = MDVUtility.ToStr(reader["PETemplateId"]);
                        model.HPITemplateId = MDVUtility.ToStr(reader["HPITemplateId"]);
                        model.EncounterType = MDVUtility.ToStr(reader["EncounterType"]);
                        model.User = MDVUtility.ToStr(reader["UserName"]);
                        model.Receiver = MDVUtility.ToStr(reader["Receiver"]);
                        model.Caller = MDVUtility.ToStr(reader["Caller"]);
                        model.UserId = MDVUtility.ToStr(reader["UserId"]);
                        model.PatientTypeId = MDVUtility.ToInt32(reader["PatientTypeId"]);
                        model.FilePath = MDVUtility.ToStr(reader["FilePath"]);
                        model.ResourceId = MDVUtility.ToStr(reader["ResourceId"]);
                        model.ResourceProviderId = MDVUtility.ToStr(reader["ResourceProviderId"]);
                        model.ResourceProvider = MDVUtility.ToStr(reader["ResourceProvider"]);
                        model.IsAnyDocumentAttached = MDVUtility.ToStr(reader["IsAnyDocumentAttached"]);
                        model.VisitTypeId = MDVUtility.ToStr(reader["VisitTypeId"]);
                        model.OrderSetId = MDVUtility.ToStr(reader["OrderSetId"]);
                        model.OrderSetName = MDVUtility.ToStr(reader["OrderSetName"]);
                        model.OrderSetComments = MDVUtility.ToStr(reader["OrderSetComments"]);
                        model.IsBodyPart = MDVUtility.StringToBoolean(MDVUtility.ToStr(reader["IsBodyPart"]));
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_Notes_SELECT_BY_NOTEID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<string> loadReconciledMedicationIds(long PatientId, long NoteId)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<string> lstCombined = new List<string>();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, PatientId);


                if (NoteId <= 0)
                    dbManager.AddParameters(1, PARM_CURRENT_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CURRENT_NOTE_ID, NoteId);
                //ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Notes_SELECT, ds, ds.Medication.TableName);

                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RECONCILEDMEDICATION_SELECT_FORNOTE);
                //MedicationLookupModel modelFill = null;
                while (reader.Read())
                {
                    List<string> lstLastVisitMedsId = MDVUtility.CheckStringNull(reader["LastVisitReconciledMedsIds"]).Split(',').ToList<string>();
                    List<string> lstPatAllMedsId = MDVUtility.CheckStringNull(reader["PatAllMedsIds"]).Split(',').ToList<string>();
                    bool inCommon = lstPatAllMedsId.Intersect(lstLastVisitMedsId).Any();
                    List<string> combinedIds = lstPatAllMedsId.Union(lstLastVisitMedsId).ToList();
                    lstCombined.Add(MDVUtility.CheckStringNull(reader["LastVisitReconciledMedsIds"]));
                    lstCombined.Add(MDVUtility.CheckStringNull(reader["PatAllMedsIds"]));
                }

                return lstCombined;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::loadReconciledMedications", PROC_Notes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NotesVisitDate> GetNotesDates(Int64 PatientId)
        {
            List<NotesVisitDate> NotesList = new List<NotesVisitDate>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                NotesList = dbManager.ExecuteReaders<NotesVisitDate>(PROC_LOAD_NOTES_DATES);
                return NotesList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetNotesDates", PROC_LOAD_NOTES_DATES, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public DSNotes load_Clinical_Notes_Obsolete(long PatientId, long NoteId, long AppointmentID, Int32 PageNumber, Int32 RowsPerPage, string isViewNotes = "", string isPrintNotes = "", int isPhoneEncounter = 0, string NoteStatus = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Notes;
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);

                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Notes.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (isPhoneEncounter == 1)
                    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, isPhoneEncounter);
                else
                    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, null);
                dbManager.AddParameters(6, PARM_NOTE_STATUS, NoteStatus);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTES_SELECT, ds, ds.Notes.TableName);
                if (dtTemp != null && ds.Notes.Rows.Count > 0)
                {
                    if (isViewNotes == "1" || isPrintNotes == "1")
                    {
                        bool isViewAction = isViewNotes == "1" ? true : false;
                        bool isPrintAcion = isPrintNotes == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_Notes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSNotes load_Clinical_Notes_Obsolete_CaseReport(long PatientId, long NoteId, long AppointmentID, Int32 PageNumber, Int32 RowsPerPage, string isViewNotes = "", string isPrintNotes = "", int isPhoneEncounter = 0, string NoteStatus = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Notes;
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);

                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Notes.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (isPhoneEncounter == 1)
                    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, isPhoneEncounter);
                else
                    dbManager.AddParameters(5, PARM_IS_PHONE_ENCOUNTER, null);
                dbManager.AddParameters(6, PARM_NOTE_STATUS, NoteStatus);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Clinical.sp_Notes_Select_CaseReporting", ds, ds.Notes.TableName);
                if (dtTemp != null && ds.Notes.Rows.Count > 0)
                {
                    if (isViewNotes == "1" || isPrintNotes == "1")
                    {
                        bool isViewAction = isViewNotes == "1" ? true : false;
                        bool isPrintAcion = isPrintNotes == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", "Clinical.sp_Notes_Select_CaseReporting", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<Notes> load_Clinical_Notes(long PatientId, long NoteId, long AppointmentID, Int32 PageNumber, Int32 RowsPerPage, string isViewNotes = "", string isPrintNotes = "", int isPhoneEncounter = 0, string NoteStatus = null, Int64 PatDocId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.BeginTransaction();

                if (NoteId <= 0)
                    dbManager.AddParameters(PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, NoteId);
                if (PatientId <= 0)
                    dbManager.AddParameters(PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(PARM_PATIENTID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, 0, DbType.Int64, ParamDirection.Output);

                if (isPhoneEncounter == 1)
                    dbManager.AddParameters(PARM_IS_PHONE_ENCOUNTER, isPhoneEncounter);
                else
                    dbManager.AddParameters(PARM_IS_PHONE_ENCOUNTER, null);
                dbManager.AddParameters(PARM_NOTE_STATUS, NoteStatus);
                dbManager.AddParameters(PARM_PATIENT_DOC_ID, PatDocId);
                List<Notes> notesList = dbManager.ExecuteReaderMapper<Notes>(PROC_NOTES_SELECT);
                //if (dtTemp != null && ds.Notes.Rows.Count > 0)
                //{
                //    if (isViewNotes == "1" || isPrintNotes == "1")
                //    {
                //        bool isViewAction = isViewNotes == "1" ? true : false;
                //        bool isPrintAcion = isPrintNotes == "1" ? true : false;
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                dbManager.CommitTransaction();
                return notesList;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_Notes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient FillPatientInfo(long PatientId)
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
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_FILL_NOTES, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::FillPatient", PROC_PATIENT_FILL_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSNotes updateNotes(IDBManager dbManager, DSNotes ds)
        {
            try
            {
                this.createParameters(dbManager, ds, false);
                ds = (DSNotes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_Notes_UPDATE, ds, ds.Notes.TableName);
                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALNotes::UpdateNotes", PROC_Notes_UPDATE, ex);
                throw ex;
            }
        }
        public string updateVisitType(IDBManager dbManager, Int64 NotesId, Int64 VisitTypeId, Int64 AppointmentId = 0)
        {
            try
            {
                dbManager.Open();
                string returnVal;
                if (AppointmentId == 0)
                {
                    dbManager.CreateParameters(2);
                }
                else
                {
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(2, PARM_APPOINTMENT_ID, AppointmentId);
                }

                dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_VISIT_TYPE, VisitTypeId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Notes_UPDATE_VISIT_TYPE).ToString();

                //  string Output = dbManager.Parameters[""];
                //     string Output1 = cmd.Parameters["@name"].Value.ToString();

                if (returnVal == "1" || returnVal == "2")
                {
                    returnVal = "";
                }

                dbManager.Close();
                return returnVal;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALNotes::UpdateNotes", PROC_Notes_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSNotes fillNotes(long NoteId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Notes_SELECT, ds, ds.Notes.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::FillNotes", PROC_Notes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        ///// <summary>
        /// Updates the Notes.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSNotes updateNotes(DSNotes ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                //DSDBAudit dsDBAudit = new DSDBAudit();
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    return UpdateNotesConcrete(ds, dbManager);


                    //if (dtTemp != null && ds.Notes.Rows.Count > 0)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString());
                    //    dsDBAudit.AcceptChanges();
                    //}
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALNotes::UpdateNotes", PROC_Notes_UPDATE, ex);
                    throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }
            else
            {
                try
                {
                    return UpdateNotesConcrete(ds, dbManager);
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALNotes::UpdateNotes", PROC_Notes_UPDATE, ex);
                    throw ex;
                }
            }
        }

        private DSNotes UpdateNotesConcrete(DSNotes ds, IDBManager dbManager)
        {
            this.createParameters(dbManager, ds, false);
            ds = (DSNotes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_Notes_UPDATE, ds, ds.Notes.TableName);
            return ds;
        }


        public string ModifiedNoteReviewed(Int64 NoteId, bool IsReviewed)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(1, PARM_IS_REVIEWED, IsReviewed);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(3, PARM_USERID, MDVSession.Current.AppUserId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODIFIED_NOTE_REVIEWED).ToString();
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::ModifiedNoteReviewed", PROC_MODIFIED_NOTE_REVIEWED, ex);
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

        public DSNotes unsignClinical_Notes(DSNotes ds)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                this.createUnSignParameters(dbManager, ds);
                ds = (DSNotes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_UNSIGN_Note, ds, ds.Notes.TableName);

                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALNotes::unsignClinical_Notes", PROC_UNSIGN_Note, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string IsOrderSetAssociatedWithNote(long NoteId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CHK_OS_WITH_NOTE));
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::IsOrderSetAssociatedWithNote", PROC_CHK_OS_WITH_NOTE, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return returnVal;
        }
        /// <summary>
        /// Deletes the Notes.
        /// </summary>
        /// <param name="MsgId">The Notes identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string deleteNotes(string NoteId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSNotes ds = loadClinical_Notes(0, Convert.ToInt64(NoteId), 0, 1, 1);
                DataTable dtTemp = ds.Notes;
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Notes_DELETE));

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                else
                {
                    if (dtTemp != null && ds.Notes.Rows.Count > 0)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Notes.Rows[0][ds.Notes.NotesIdColumn].ToString(), null, "", false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALNotes::DeleteNotes", PROC_Notes_DELETE, ex);
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
        public string removeComponents(string componenetsIds, long NoteId)
        {
            string returnVal = "";

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_COMP_IDS, componenetsIds);

                if (NoteId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NoteId);



                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DEATTACH_FROM_NOTE).ToString();

                return returnVal;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALNotes::DeleteNotes", PROC_Notes_DELETE, ex);
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

        public ClinicalNotesModel loadNoteWithNewTemplate(long NoteId, long PatientId, long TemplateId, long UserId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                ClinicalNotesModel model = new ClinicalNotesModel();
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_NOTES_TEMPID, TemplateId));
                parameters.Add(new SqlParameter(PARM_USERID, UserId));

                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_NOTE_WITH_NEW_TEMPLATE, parameters))
                {
                    while (reader.Read())
                    {
                        model.NotesId = MDVUtility.ToStr(reader["NotesId"]);
                        model.Duration = MDVUtility.ToStr(reader["Duration"]);
                        model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                        model.NoteText = MDVUtility.ToStr(reader["NoteText"]);
                        model.NoteType = MDVUtility.ToStr(reader["TemplateTypeId"]);
                        model.NoteTemplate = MDVUtility.ToStr(reader["TemplateId"]);
                        model.VisitDate = MDVUtility.ToStr(reader["VisitDate"]) != "" ? MDVUtility.ToDateTime(reader["VisitDate"]).ToShortDateString() : "";
                        model.VisitTime = MDVUtility.ToStr(reader["VisitTime"]);
                        model.NoteStatus = MDVUtility.ToStr(reader["NoteStatus"]);
                        model.SignedBy = MDVUtility.ToStr(reader["SignedBy"]);
                        model.Provider = MDVUtility.ToStr(reader["ProviderName"]);
                        model.ProviderId = MDVUtility.ToStr(reader["ProviderId"]);
                        model.FacilityName = model.Facility = MDVUtility.ToStr(reader["FacilityName"]);
                        model.FacilityId = MDVUtility.ToStr(reader["FacilityId"]);
                        model.RefProvider = MDVUtility.ToStr(reader["RefProviderName"]);
                        model.RefProviderId = MDVUtility.ToStr(reader["RefProviderId"]);
                        model.AppointmentID = MDVUtility.ToStr(reader["LinkedAppointmentId"]);
                        model.LinkedAppointment = MDVUtility.ToStr(reader["LinkedAppointment"]);
                        model.IsLinkedAppointment = string.IsNullOrEmpty(MDVUtility.ToStr(reader["LinkedAppointment"])) ? false : true;
                        model.VisitReason = MDVUtility.ToStr(reader["VisitReasonComments"]);
                        model.VisitReasonId = MDVUtility.ToStr(reader["SchReasonId"]);
                        model.RoomNo = MDVUtility.ToStr(reader["RoomsId"]);
                        model.CopayPreviousNote = MDVUtility.ToStr(reader["PrevNoteDescription"]);
                        model.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                        model.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                        model.ChiefComplaint = MDVUtility.ToStr(reader["ChiefComplaint"]);
                        model.IsPhoneEncounter = MDVUtility.ToBool(reader["IsPhoneEncounter"]);
                        model.TemplateName = MDVUtility.ToStr(reader["TemplateName"]);
                        model.TemplateTypeName = MDVUtility.ToStr(reader["TemplateTypeName"]);
                        model.VisitType = MDVUtility.ToStr(reader["VisitType"]);
                        model.FacilityPOSCode = MDVUtility.ToStr(reader["FacilityPOSCode"]);
                        model.FacilityPOSDesc = MDVUtility.ToStr(reader["FacilityPOSDesc"]);
                        model.ROSDataTemptId = MDVUtility.ToStr(reader["ROSDataTemptId"]);
                        model.BillingInfoId = MDVUtility.ToStr(reader["BillingInfoId"]);
                        model.bMedReconciled = !string.IsNullOrEmpty(MDVUtility.ToStr(reader["bMedReconciled"])) && MDVUtility.ToStr(reader["bMedReconciled"]).ToLower() == "true" ? "1" : "0";
                        model.MedReconciledId = MDVUtility.ToStr(reader["MedReconciledId"]);
                        model.HxtabOrder = MDVUtility.ToStr(reader["HxtabOrder"]);
                        model.IsNonBilable = MDVUtility.ToBool(reader["IsNonBilable"]);
                        model.PEDataTemptId = MDVUtility.ToStr(reader["PEDataTemptId"]);
                        model.ROSTemplateId = MDVUtility.ToStr(reader["ROSTemplateId"]);
                        model.PETemplateId = MDVUtility.ToStr(reader["PETemplateId"]);
                        model.EncounterType = MDVUtility.ToStr(reader["EncounterType"]);
                        model.User = MDVUtility.ToStr(reader["UserName"]);
                        model.Receiver = MDVUtility.ToStr(reader["Receiver"]);
                        model.Caller = MDVUtility.ToStr(reader["Caller"]);
                        model.UserId = MDVUtility.ToStr(reader["UserId"]);
                        model.PatientTypeId = MDVUtility.ToInt32(reader["PatientTypeId"]);
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_NOTE_WITH_NEW_TEMPLATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public bool IsTodaysNoteCreated(long PatientId,string VisitDate)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            bool IsCreated = false;
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_VISIT_DATE, VisitDate));
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_IS_TODATS_NOTE_CREATED, parameters))
                {
                    while (reader.Read())
                    {
                        IsCreated = MDVUtility.StringToBoolean(reader["IsCreated"].ToString());
                    }
                }

                return IsCreated;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::IsTodaysNoteCreated", PROC_IS_TODATS_NOTE_CREATED, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string loadTemplate_HTML(long PatientId, long NoteId, long TemplateId)
        {
            string returnVal = "";

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NoteId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, NoteId);

                if (TemplateId == 0)
                    dbManager.AddParameters(2, PARM_NOTES_TEMPID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTES_TEMPID, TemplateId);

                dbManager.AddParameters(3, PARM_IS_NOTE_TEXT, 1);

                dbManager.AddParameters(4, PARM_MODE, 0);

                dbManager.AddParameters(5, PARM_USERID, MDVUtility.ToInt64(MDVSession.Current.AppUserId));

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_POP_NOTE_TEMPDATA).ToString();

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient:loadTemplate_HTML", PROC_PATIENT_FILL_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Notes.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSNotes insertNotes(DSNotes ds, DataTable dtCopytComponents = null)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, true, "Notes", dtCopytComponents);
                ds = (DSNotes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_Notes_INSERT, ds, ds.Notes.TableName);// ds.Notes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::InsertNotes", PROC_Notes_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string InsertOrderSet(long UserId, string OrderSetId, long PatientID, string NotesID)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string orderSetID = string.Empty;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesID);
                dbManager.AddParameters(1, PARM_ORDER_SET_ID, OrderSetId);
                dbManager.AddParameters(2, PARM_PATIENTID, PatientID);
                dbManager.AddParameters(3, PARM_User_ID, UserId);

                orderSetID = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSERT_ORDER_SET).ToString();
                return orderSetID;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::InsertOrderSet", PROC_INSERT_ORDER_SET, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSNotes GetNewInsertTables(long NoteId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (NoteId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NEW_INSERT_TABLES, ds, ds.NewInsertionTables.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetNewInsertTables", PROC_NEW_INSERT_TABLES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSNotes fillLinkedAppointment_Notes(long PatientId, long ProviderId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, PatientId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTES_LINKAPPOINTMENT, ds, ds.Notes_LinkAppointment.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::FillLinkedAppointment_Notes", PROC_NOTES_LINKAPPOINTMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSNotes copyPreviousNote_Patient(long PatientId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, PatientId);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTES_COPYFROM_PREVIOUSNOTE, ds, ds.Notes_PreviousNote.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::CopyPreviousNote_Patient", PROC_NOTES_COPYFROM_PREVIOUSNOTE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "FollowUp Appointment form Clinical Progress Notes"

        //private void CreateFollowUpParameters(IDBManager dbManager, DSNotes ds, Boolean IsInsert)
        //{
        //    dbManager.CreateParameters(14);

        //    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, ds.FollowUpAppointment.AppointmentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
        //    dbManager.AddParameters(0, PARM_PATIENT_ID, ds.FollowUpAppointment.PatientIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.FollowUpAppointment.ProviderIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(2, PARM_RESOURCE_ID, ds.FollowUpAppointment.ResourceIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(3, PARM_FACILITY_ID, ds.FollowUpAppointment.FacilityIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(4, PARM_APPOINTMENT_DATE, ds.FollowUpAppointment.AppointmentDateColumn.ColumnName, DbType.DateTime);
        //    dbManager.AddParameters(5, PARM_FROM_TIME, ds.FollowUpAppointment.FromTimeColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(6, PARM_DURATION, ds.FollowUpAppointment.DurationColumn.ColumnName, DbType.Int32);
        //    dbManager.AddParameters(7, PARM_COMMENTS, ds.FollowUpAppointment.CommentsColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(8, PARM_SCHREASON_ID, ds.FollowUpAppointment.SchReasonIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.FollowUpAppointment.IsActiveColumn.ColumnName, DbType.Byte);
        //    dbManager.AddParameters(10, PARM_CREATED_BY, ds.FollowUpAppointment.CreatedByColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(11, PARM_CREATED_ON, ds.FollowUpAppointment.CreatedOnColumn.ColumnName, DbType.DateTime);
        //    dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.FollowUpAppointment.ModifiedByColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.FollowUpAppointment.ModifiedOnColumn.ColumnName, DbType.DateTime);

        //}


        public string InsertFollowUpAppointment(long PatientId, long ProviderId, long ResourceId, long FacilityId, DateTime AppointmentDate, string Time, Int32 Duration, string Comments, long SchReasonId, Byte IsActive, string CreatedBy, DateTime CreatedOn, string ModifiedBy, DateTime ModifiedOn, string ReasonComments)
        {


            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (Comments == "")
                    Comments = null;

                dbManager.Open();
                dbManager.CreateParameters(15);

                dbManager.AddParameters(0, PARM_PATIENTID, PatientId);

                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);
                if (ResourceId <= 0)
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_RESOURCE_ID, ResourceId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);


                dbManager.AddParameters(4, PARM_APPOINTMENT_DATE, AppointmentDate);

                dbManager.AddParameters(5, PARM_FROM_TIME, Time);
                dbManager.AddParameters(6, PARM_DURATION, Duration);
                dbManager.AddParameters(7, PARM_COMMENTS, Comments);
                //if (SchReasonId <= 0)
                //    dbManager.AddParameters(8, PARM_SCHREASON_ID, null);
                //else
                dbManager.AddParameters(8, PARM_SCHREASON_ID, null);


                dbManager.AddParameters(9, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(10, PARM_CREATED_BY, CreatedBy);
                dbManager.AddParameters(11, PARM_CREATED_ON, CreatedOn);
                dbManager.AddParameters(12, PARM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(13, PARM_MODIFIED_ON, ModifiedOn);
                dbManager.AddParameters(14, PARM_SCHEDULE_REASON_COMMENTS, ReasonComments);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOWUP_APPOINTMENT_INSERT).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::InsertFollowUpAppointment", PROC_FOLLOWUP_APPOINTMENT_INSERT, ex);
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

        #region Notes Extra Region
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 20/05/2016
        // Purpose : Get Reference Data.

        public DSNotes GetReferenceData()
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_REFERENCE_DATA, ds, ds.ReferenceData.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetReferenceData", PROC_GET_REFERENCE_DATA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 09/02/2016
        //OverView: Methods "InsertPatientMUSetting" for save Patient MUSetting
        public DSNotes InsertPatientMUSetting(DSNotes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertPatientMUSetting(dbManager, ds, true);
                ds = (DSNotes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_MUSETTING_INSERT, ds, ds.PatientMUSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::InsertPatientMUSetting", PROC_PATIENT_MUSETTING_INSERT, ex);
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

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 20/05/2016
        // Purpose : Get Reference Data.

        public DSNotes LoadPatientMUSetting(long PatientId, long NoteId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);

                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_MU_SETTING_SELECT, ds, ds.PatientMUSetting.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadPatientMUSetting", PROC_PATIENT_MU_SETTING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSNotes UpdatePatientMenuSetting(DSNotes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersForInsertPatientMUSetting(dbManager, ds, false);
                ds = (DSNotes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_MUSETTING_UPDATE, ds, ds.PatientMUSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::UpdatePatientMenuSetting", PROC_PATIENT_MUSETTING_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSNotes InsertPatientMUSettingWithMissingReference(DSNotes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertPatientMUSetting(dbManager, ds, true);
                ds = (DSNotes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_MU_SETTING_WITH_MISSING_REFERENCE_INSERT, ds, ds.PatientMUSetting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::InsertPatientMUSettingWithMissingReference", PROC_PATIENT_MU_SETTING_WITH_MISSING_REFERENCE_INSERT, ex);
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

        // Created By:  Farooq Ahmad
        // Created Date: 22/07/2016
        //OverView: Methods "GetAppointmentIdByNoteId"
        public long GetAppointmentIdByNoteId(long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                long AppointmentID = (long)dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_APPOINTMENTIDBYNOTEID);
                return AppointmentID;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetAppointmentIdByNoteId", PROC_APPOINTMENTIDBYNOTEID, ex);
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

        // Created By:  Farooq Ahmad
        // Created Date: 07/09/2016
        //OverView: Methods "UpdateNotesHxtabOrder"
        public int UpdateNotesHxtabOrder(long NoteId, string HxtabOrder)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(1, PARM_HXTABORDER, HxtabOrder);
                int AppointmentID = (int)dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_NOTESHXTABORDERUPDATE);
                return AppointmentID;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::UpdateNotesHxtabOrder", PROC_NOTESHXTABORDERUPDATE, ex);
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


        public DSNotes LoadAutoPopulateOptions(long userId, string componentType, long entityId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USERID, userId);
                dbManager.AddParameters(1, PARM_COMPONENT_TYPE, componentType);
                dbManager.AddParameters(2, PARM_ENTITYID, entityId);
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_AUTO_POPULATE_OPTIONS_SELECT, ds, ds.NoteAutoPopulateOptions.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadAutoPopulateOptions", PROC_NOTE_AUTO_POPULATE_OPTIONS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region LookUp
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Case.

        public DSNotes LookupValueSetting(int Top)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_TOP, Top);
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VALUE_SETTING_LOOKUP, ds, ds.ValueSetting.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::LookupValueSetting", PROC_PATIENT_VALUE_SETTING_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #endregion



        #region Notes Seperate Component Implementation

        public string insertNoteComponent(NoteComponentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ParamsForSeperateComponentInsert(dbManager, model, true);
                var ProgressUpdateId = dbManager.ExecuteScalar(PROC_NOTE_COMPONENT_INSERT);

                return MDVUtility.ToStr(ProgressUpdateId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::insertNoteComponent", PROC_NOTE_COMPONENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateNoteComponent(NoteComponentModel model, string UserName = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ParamsForSeperateComponentInsert(dbManager, model, false, UserName);
                var ProgressUpdateId = dbManager.ExecuteScalar(PROC_NOTE_COMPONENT_UPDATE);

                return MDVUtility.ToStr(ProgressUpdateId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::updateNoteComponent", PROC_NOTE_COMPONENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NoteComponentModel> loadNoteComponents(long NotesId, string ComponentName)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NoteComponentModel> model_ = new List<NoteComponentModel>();
                SqlDataReader reader = null;

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                dbManager.AddParameters(1, PARM_COMPONENT_NAME, ComponentName);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_COMPONENT_SELECT);
                while (reader.Read())
                {
                    NoteComponentModel item = new NoteComponentModel();
                    item.NoteComponentId = MDVUtility.ToLong(reader["NoteComponentId"]);
                    item.NotesId = MDVUtility.ToLong(reader["NotesId"]);
                    item.NoteComponentsLookupId = MDVUtility.ToLong(reader["NoteComponentsLookupId"]);
                    item.SOAPText = MDVUtility.ToStr(reader["SOAPText"]);
                    item.OrderNo = MDVUtility.ToInt16(reader["OrderNo"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    item.SectionName = MDVUtility.ToStr(reader["SectionName"]);
                    item.NoteSectionsLookupId = MDVUtility.ToLong(reader["NoteSectionsLookupId"]);
                    model_.Add(item);
                }

                return model_;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALNotes::loadNoteComponents", PROC_NOTE_COMPONENT_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteComponents", PROC_NOTE_COMPONENT_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NoteComponentModel> loadNoteComponentsName(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NoteComponentModel> model_ = new List<NoteComponentModel>();
                SqlDataReader reader = null;

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_COMPONENT_NAME_SELECT);
                while (reader.Read())
                {
                    NoteComponentModel item = new NoteComponentModel();

                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    item.customComponentName = MDVUtility.ToStr(reader["customComponentName"]);
                    model_.Add(item);
                }

                return model_;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALNotes::loadNoteComponents", PROC_NOTE_COMPONENT_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteComponentsName", PROC_NOTE_COMPONENT_NAME_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public NotesViewModel LoadNotesDataBulkSign(string NotesIds, Int64 UserId, Int64 EntityId, Int64 PracticeId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            NotesViewModel model = new NotesViewModel();
            try
            {
                SqlDataReader reader = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesIds);
                dbManager.AddParameters(1, PARM_USERID, UserId);
                dbManager.AddParameters(2, PARM_ENTITYID, EntityId);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, PracticeId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LOAD_NOTEDATA_FORPDF);
                model.NoteComponentModelList = new List<NoteComponentModel>();
                while (reader.Read())
                {
                    NoteComponentModel item = new NoteComponentModel();
                    item.NoteComponentId = MDVUtility.ToLong(reader["NoteComponentId"]);
                    item.NotesId = MDVUtility.ToLong(reader["NotesId"]);
                    item.NoteComponentsLookupId = MDVUtility.ToLong(reader["NoteComponentsLookupId"]);
                    item.SOAPText = MDVUtility.ToStr(reader["SOAPText"]);
                    item.OrderNo = MDVUtility.ToInt16(reader["OrderNo"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    item.SectionName = MDVUtility.ToStr(reader["SectionName"]);
                    item.NoteSectionsLookupId = MDVUtility.ToLong(reader["NoteSectionsLookupId"]);
                    model.NoteComponentModelList.Add(item);
                }
                reader.NextResult();
                model.NotesModelList = new List<NotesModel>();
                while (reader.Read())
                {
                    NotesModel item = new NotesModel();
                    item.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                    item.ProviderId = MDVUtility.ToStr(reader["ProviderId"]);
                    item.NotesId = MDVUtility.ToStr(reader["NotesId"]);
                    item.Duration = MDVUtility.ToStr(reader["Duration"]);
                    item.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                    item.ShortName = MDVUtility.ToStr(reader["ShortName"]);
                    item.Description = MDVUtility.ToStr(reader["Description"]);
                    item.NoteText = MDVUtility.ToStr(reader["NoteText"]);
                    item.VisitDate = MDVUtility.ToStr(reader["VisitDate"]);
                    item.VisitTime = MDVUtility.ToStr(reader["VisitTime"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToStr(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToStr(reader["ModifiedOn"]);
                    item.NoteStatus = MDVUtility.ToStr(reader["NoteStatus"]);
                    item.SignedBy = MDVUtility.ToStr(reader["SignedBy"]);
                    item.SignedByName = MDVUtility.ToStr(reader["SignedByName"]);
                    item.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    item.Comments = MDVUtility.ToStr(reader["Comments"]);
                    item.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    item.RefProviderName = MDVUtility.ToStr(reader["RefProviderName"]);
                    item.RefProviderAddress = MDVUtility.ToStr(reader["RefProviderAddress"]);
                    item.VisitReason = MDVUtility.ToStr(reader["VisitReason"]);
                    item.RoomName = MDVUtility.ToStr(reader["RoomName"]);
                    item.PrevNoteDescription = MDVUtility.ToStr(reader["PrevNoteDescription"]);
                    item.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                    item.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    item.ChiefComplaint = MDVUtility.ToStr(reader["ChiefComplaint"]);
                    item.TemplateName = MDVUtility.ToStr(reader["TemplateName"]);
                    item.VisitType = MDVUtility.ToStr(reader["VisitType"]);
                    item.TemplateTypeName = MDVUtility.ToStr(reader["TemplateTypeName"]);
                    item.FacilityPOSDesc = MDVUtility.ToStr(reader["FacilityPOSDesc"]);
                    item.BillingInfoId = MDVUtility.ToStr(reader["BillingInfoId"]);
                    item.VisitReasonComments = MDVUtility.ToStr(reader["VisitReasonComments"]);
                    model.NotesModelList.Add(item);
                }

                reader.NextResult();
                model.PatientList = new List<PatientViewModel>();
                while (reader.Read())
                {
                    PatientViewModel item = new PatientViewModel();
                    item.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                    item.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"]);
                    item.EntityId = MDVUtility.ToStr(reader["EntityId"]);
                    item.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    item.MI = MDVUtility.ToStr(reader["MI"]);
                    item.Prefix = MDVUtility.ToStr(reader["Prefix"]);
                    item.LastName = MDVUtility.ToStr(reader["LastName"]);
                    item.CellNo = MDVUtility.ToStr(reader["CellNo"]);
                    item.Suffix = MDVUtility.ToStr(reader["Suffix"]);
                    item.Gender = MDVUtility.ToStr(reader["Gender"]);
                    item.DOB = MDVUtility.ToStr(reader["DOB"]);
                    item.SSN = MDVUtility.ToStr(reader["SSN"]);
                    item.MRNumber = MDVUtility.ToStr(reader["MRNumber"]);
                    item.Age = MDVUtility.ToStr(reader["Age"]);
                    item.EmailAddress = MDVUtility.ToStr(reader["EmailAddress"]);
                    item.Address1 = MDVUtility.ToStr(reader["Address1"]);
                    item.Address2 = MDVUtility.ToStr(reader["Address2"]);
                    item.City = MDVUtility.ToStr(reader["City"]);
                    item.State = MDVUtility.ToStr(reader["State"]);
                    item.ZipCode = MDVUtility.ToStr(reader["ZipCode"]);
                    item.ZipCodeExt = MDVUtility.ToStr(reader["ZipCodeExt"]);
                    item.HomePhoneNo = MDVUtility.ToStr(reader["HomePhoneNo"]);
                    item.WorkPhoneNo = MDVUtility.ToStr(reader["WorkPhoneNo"]);
                    item.WorkPhoneExt = MDVUtility.ToStr(reader["WorkPhoneExt"]);
                    item.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    item.ProviderFirstName = MDVUtility.ToStr(reader["ProviderFirstName"]);
                    item.ProviderLastName = MDVUtility.ToStr(reader["ProviderLastName"]);
                    item.FacilityName = MDVUtility.ToStr(reader["FacilityName"]);
                    item.FacilityAddress = MDVUtility.ToStr(reader["FacilityAddress"]);
                    item.FacilityCity = MDVUtility.ToStr(reader["FacilityCity"]);
                    item.FacilityState = MDVUtility.ToStr(reader["FacilityState"]);
                    item.FacilityPhone = MDVUtility.ToStr(reader["FacilityPhone"]);
                    model.PatientList.Add(item);
                }

                reader.NextResult();
                model.ProviderList = new List<ProviderModel>();
                while (reader.Read())
                {
                    ProviderModel item = new ProviderModel();
                    item.ProviderId = MDVUtility.ToStr(reader["ProviderId"]);
                    item.FirstName = MDVUtility.ToStr(reader["FirstName"]);
                    item.LastName = MDVUtility.ToStr(reader["LastName"]);
                    item.SpecialtyName = MDVUtility.ToStr(reader["SpecialtyName"]);
                    model.ProviderList.Add(item);
                }

                reader.NextResult();
                model.PracticeList = new List<PracticeModel>();
                while (reader.Read())
                {
                    PracticeModel item = new PracticeModel();
                    item.PracticeId = MDVUtility.ToStr(reader["PracticeId"]);
                    item.City = MDVUtility.ToStr(reader["City"]);
                    item.State = MDVUtility.ToStr(reader["State"]);
                    item.ZIPCode = MDVUtility.ToStr(reader["ZIPCode"]);
                    item.ShortName = MDVUtility.ToStr(reader["ShortName"]);
                    item.Address = MDVUtility.ToStr(reader["Address"]);
                    item.PhoneNo = MDVUtility.ToStr(reader["PhoneNo"]);
                    model.PracticeList.Add(item);
                }

                reader.NextResult();
                model.RptHdrTagsList = new List<RptHdrTags>();
                while (reader.Read())
                {
                    RptHdrTags item = new RptHdrTags();
                    item.NotesId = MDVUtility.ToStr(reader["NotesId"]);
                    item.PracticeText = MDVUtility.ToStr(reader["PracticeText"]);
                    item.PatientText = MDVUtility.ToStr(reader["PatientText"]);
                    item.ProviderText = MDVUtility.ToStr(reader["ProviderText"]);
                    item.HeaderLogo = MDVUtility.ToStr(reader["HeaderLogo"]);
                    item.FooterText = MDVUtility.ToStr(reader["FooterText"]);
                    item.PatientName = MDVUtility.ToStr(reader["PatientName"]);
                    item.PatientDOB = MDVUtility.ToStr(reader["PatientDOB"]);
                    item.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    item.DOS = MDVUtility.ToStr(reader["DOS"]);
                    item.IsProviderHeader = MDVUtility.ToBool(reader["IsProviderHeader"]);
                    model.RptHdrTagsList.Add(item);
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadNotesDataBulkSign", PROC_LOAD_NOTEDATA_FORPDF, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NoteComponentModel> loadNoteComponentsDataFixPDF(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NoteComponentModel> model_ = new List<NoteComponentModel>();
                SqlDataReader reader = null;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_COMPONENT_SELECT_FOR_PDF);
                while (reader.Read())
                {
                    NoteComponentModel item = new NoteComponentModel();
                    item.NotesId = MDVUtility.ToLong(reader["NotesId"]);
                    item.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    item.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    item.NoteComponentId = MDVUtility.ToLong(reader["NoteComponentId"]);
                    item.NoteComponentsLookupId = MDVUtility.ToLong(reader["NoteComponentsLookupId"]);
                    item.SOAPText = MDVUtility.ToStr(reader["SOAPText"]);
                    item.OrderNo = MDVUtility.ToInt16(reader["OrderNo"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    item.SectionName = MDVUtility.ToStr(reader["SectionName"]);
                    item.NoteSectionsLookupId = MDVUtility.ToLong(reader["NoteSectionsLookupId"]);
                    model_.Add(item);
                }
                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteComponentsDataFixPDF", PROC_NOTE_COMPONENT_SELECT_FOR_PDF);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<NotesPDFModel> loadNoteFilePaths4DataFixPDF()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NotesPDFModel> model_ = new List<NotesPDFModel>();
                SqlDataReader reader = null;
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_PDF_SELECT_DATA_FIX);
                while (reader.Read())
                {
                    NotesPDFModel item = new NotesPDFModel();
                    item.NotesId = MDVUtility.ToStr(reader["NotesId"]);
                    item.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                    item.NotePDF_FileName = MDVUtility.ToStr(reader["FilePath"]);
                    item.NotePDF_FilePath = MDVUtility.ToStr(reader["url"]);
                    model_.Add(item);
                }
                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteFilePaths4DataFixPDF", PROC_NOTE_COMPONENT_SELECT_FOR_PDF);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NotesPDFModel> loadNoteFilePaths4DataFixPDF(string noteids)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NotesPDFModel> model_ = new List<NotesPDFModel>();
                SqlDataReader reader = null;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTE_ID, noteids);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_PDF_SELECT_DATA_FIX);
                while (reader.Read())
                {
                    NotesPDFModel item = new NotesPDFModel();
                    item.NotesId = MDVUtility.ToStr(reader["NotesId"]);
                    item.PatientId = MDVUtility.ToStr(reader["PatientId"]);
                    item.NotePDF_FileName = MDVUtility.ToStr(reader["FilePath"]);
                    item.NotePDF_FilePath = MDVUtility.ToStr(reader["url"]);
                    model_.Add(item);
                }
                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteFilePaths4DataFixPDF", PROC_NOTE_COMPONENT_SELECT_FOR_PDF);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<NoteComponentModel> loadNoteFollowUpComponent(long AppointmentId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NoteComponentModel> model_ = new List<NoteComponentModel>();
                SqlDataReader reader = null;

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_APPOINTMENT_ID, AppointmentId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_FOLLOWUP_COMPONENT_SELECT);
                while (reader.Read())
                {
                    NoteComponentModel item = new NoteComponentModel();
                    item.NoteComponentId = MDVUtility.ToLong(reader["NoteComponentId"]);
                    item.NotesId = MDVUtility.ToLong(reader["NotesId"]);
                    item.NoteComponentsLookupId = MDVUtility.ToLong(reader["NoteComponentsLookupId"]);
                    item.SOAPText = MDVUtility.ToStr(reader["SOAPText"]);
                    item.OrderNo = MDVUtility.ToInt16(reader["OrderNo"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    model_.Add(item);
                }

                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteFollowUpComponent", PROC_NOTE_FOLLOWUP_COMPONENT_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteNoteComponent(long NoteComponentId, long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_NOTE_COMPONENT_ID, NoteComponentId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTE_COMPONENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::deleteNoteComponent", PROC_NOTE_COMPONENT_DELETE, ex);
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

        public string deleteOrderSetComponents(long NotesId, string ComponentLookupIDs)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                dbManager.AddParameters(1, PARM_COMPONENT_LOOKUP_IDS, ComponentLookupIDs);

                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDERSET_COMPONENTS_DELETE));

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::deleteOrderSetComponents", PROC_ORDERSET_COMPONENTS_DELETE, ex);
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


        public List<NoteComponentsLookupModel> LookupNoteComponents()
        {
            List<NoteComponentsLookupModel> listCustomForm = new List<NoteComponentsLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                //dbManager.CreateParameters(1);
                //dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_COMPONENTS_LOOKUP);
                NoteComponentsLookupModel model = null;
                while (reader.Read())
                {
                    model = new NoteComponentsLookupModel();
                    model.NoteComponentId = reader["NoteComponentsLookupId"].ToString();
                    model.NoteComponentName = reader["ComponentName"].ToString();

                    listCustomForm.Add(model);
                }

                return listCustomForm;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LookupNoteComponents", PROC_NOTE_COMPONENTS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<NoteSectionsLookupModel> LookupNoteSections()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                return dbManager.ExecuteReaders<NoteSectionsLookupModel>(PROC_NOTE_SECTIONS_LOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LookupNoteSections", PROC_NOTE_SECTIONS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<NoteComponentModel> insertNoteComponentsBulk(string NoteComponentsXml, bool IsNoteUpdate, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    List<NoteComponentModel> NoteComponentList = new List<NoteComponentModel>();
                    dbManager.Open();
                    NoteComponentList = insertNoteComponentConcrete(dbManager, NoteComponentsXml, IsNoteUpdate);
                    return NoteComponentList;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALNotes::insertNoteComponentsBulk", PROC_NOTE_COMPONENTS_INSERT_BULK, ex);
                    throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }
            else
            {
                try
                {
                    return insertNoteComponentConcrete(dbManager, NoteComponentsXml, IsNoteUpdate);
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALNotes::insertNoteComponentsBulk", PROC_NOTE_COMPONENTS_INSERT_BULK, ex);
                    throw ex;
                }
            }
        }

        private List<NoteComponentModel> insertNoteComponentConcrete(IDBManager dbManager, string NoteComponentsXml, bool IsNoteUpdate)
        {
            List<NoteComponentModel> NoteComponentList = new List<NoteComponentModel>();
            string ComponentsXML = string.Empty;
            dbManager.CreateParameters(7);
            dbManager.AddParameters(0, PARM_NOTE_COMPONENT_XML, NoteComponentsXml);
            dbManager.AddParameters(1, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(3, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_IS_NOTE_UPDATE, IsNoteUpdate);
            dbManager.AddParameters(6, PARM_NOTE_COMPONENT_XML_OUTPUT, "", DbType.Xml, ParamDirection.Output);

            dbManager.ExecuteNonQueryWithOutputParam(CommandType.StoredProcedure, PROC_NOTE_COMPONENTS_INSERT_BULK);

            if (((SqlParameter)(dbManager.Command.Parameters["@ReturnXML"])).Value != null)
                ComponentsXML = ((SqlParameter)(dbManager.Command.Parameters["@ReturnXML"])).Value.ToString();

            dbManager.ClearCommandParam();
            if (!string.IsNullOrWhiteSpace(ComponentsXML))
            {
                var doc2 = System.Xml.Linq.XDocument.Parse(ComponentsXML);
                NoteComponentList = doc2.Descendants("BulkComponent").Select(d =>
                                    new NoteComponentModel
                                    {
                                        NoteComponentId = MDVUtility.ToInt64(d.Element("NoteComponentId").Value),
                                        UniqueId = d.Element("UniqueId").Value
                                    }).ToList();
            }
            return NoteComponentList;
        }
        public string SetNotesComponentsOrder(string NotesComponentIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_NOTE_COMPONENTS_IDS, NotesComponentIds);
                var ProgressUpdateId = dbManager.ExecuteScalar(PROC_NOTE_COMPONENTS_ORDER);
                return MDVUtility.ToStr(ProgressUpdateId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::SetNotesComponentsOrder", PROC_NOTE_COMPONENTS_ORDER, ex);
                throw ex;
            }
            finally
            {
            }
        }
        #endregion

        #region Notes Component Audit

        public List<NoteComponentAuditModel> loadNoteComponentAudit(long NotesId, long NoteComponentAuditId, int PageNumber, int RowsPerPage)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<NoteComponentAuditModel> model_ = new List<NoteComponentAuditModel>();
                SqlDataReader reader = null;
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (NotesId > 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);

                if (NoteComponentAuditId > 0)
                    dbManager.AddParameters(1, PARM_NOTE_COMPONENT_AUDIT_ID, NoteComponentAuditId);
                else
                    dbManager.AddParameters(1, PARM_NOTE_COMPONENT_AUDIT_ID, null);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_COMPONENT_AUDIT_SELECT);

                while (reader.Read())
                {
                    NoteComponentAuditModel item = new NoteComponentAuditModel();
                    item.NoteComponentAuditId = MDVUtility.ToLong(reader["NoteComponentAuditId"]);
                    item.NotesId = MDVUtility.ToLong(reader["NotesId"]);
                    item.ComponentsLookupId = MDVUtility.ToLong(reader["ComponentsLookupId"]);
                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    item.OldSOAPText = MDVUtility.ToStr(reader["OldSOAPText"]);
                    item.NewSOAPText = MDVUtility.ToStr(reader["NewSOAPText"]);
                    item.DBAction = MDVUtility.ToStr(reader["DBAction"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    item.RecordCount = MDVUtility.ToInt(reader["RecordCount"]);
                    item.ModifiedByName = MDVUtility.ToStr(reader["ModifiedByName"]);
                    item.CreatedByName = MDVUtility.ToStr(reader["CreatedByName"]);
                    model_.Add(item);
                }

                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::loadNoteComponentAudit", PROC_NOTE_COMPONENT_AUDIT_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        #region Amendments
        public string GetModifiedNoteCount(long UserId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USERID, UserId);
                dbManager.AddParameters(1, PARM_ENTITYID, MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dbManager.AddParameters(2, PARM_RECORD_COUNT, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DASHBOARD_MODIFIED_NOTES_COUNT).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetModifiedNoteCount", PROC_DASHBOARD_MODIFIED_NOTES_COUNT, ex);
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
        public List<AmendmentNoteModel> GetAmendmentData(long NotesId)
        {
            List<AmendmentNoteModel> FavoriteListImmunizationModelList = new List<AmendmentNoteModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                FavoriteListImmunizationModelList = dbManager.ExecuteReaders<AmendmentNoteModel>(PROC_GET_AMENDMENT_DATA);

                AmendmentNoteModel dumy = new AmendmentNoteModel();

                return FavoriteListImmunizationModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetAmendmentData", PROC_GET_AMENDMENT_DATA, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public List<AmendmentNoteReportModel> GetAmendmentDataReport(long NotesId)
        {
            List<AmendmentNoteReportModel> AmendmentReportModelList = new List<AmendmentNoteReportModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                AmendmentReportModelList = dbManager.ExecuteReaders<AmendmentNoteReportModel>(PROC_GET_AMENDMENT_DATA_REPORT);

                AmendmentNoteReportModel dumy = new AmendmentNoteReportModel();

                return AmendmentReportModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetAmendmentData", PROC_GET_AMENDMENT_DATA_REPORT, ex);
                throw ex;
            }
            finally
            {
            }
        }
        #endregion


        public List<NoteDocumentModel> GetAssociatedAttachmentsOfNote(long NotesId)
        {
            List<NoteDocumentModel> documentList = new List<NoteDocumentModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                documentList = dbManager.ExecuteReaders<NoteDocumentModel>(PROC_NOTES_ATTACHMENTS);
                return documentList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetAssociatedAttachmentsOfNote", PROC_NOTES_ATTACHMENTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string NoteAttachmentExists(long NotesId)
        {
            string returnVal = "";

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTES_ATTACHMENTS_EXISTS));
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::NoteAttachmentExists", PROC_NOTES_ATTACHMENTS_EXISTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region Template Data Print

        public MDVision.Model.Clinical.Notes.Notes.TemplateData NotesTemplateDataSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MDVision.Model.Clinical.Notes.Notes.TemplateData> objList_TemplateData = new List<MDVision.Model.Clinical.Notes.Notes.TemplateData>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_TEMPLATEID, objCommonSearch.TemplateId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId)));
                using (var reader = dbManager.ExecuteReader(PROC_TEMPLATE_BY_ID_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        MDVision.Model.Clinical.Notes.Notes.TemplateData model = new MDVision.Model.Clinical.Notes.Notes.TemplateData();
                        var properties = typeof(MDVision.Model.Clinical.Notes.Notes.TemplateData).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_TemplateData.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::NotesTemplateDataSelect", PROC_TEMPLATE_BY_ID_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            if (objList_TemplateData == null || objList_TemplateData.Count() == 0)
            {
                return new MDVision.Model.Clinical.Notes.Notes.TemplateData();
            }
            else
            {
                return objList_TemplateData.FirstOrDefault();
            }
        }

        public List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem> ReviewOfSystemDataSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem> objList_ReviewOfSystem = new List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_ROS_DATA_TEMPLATE_ID, objCommonSearch.ROSDataTemplateId));
                using (var reader = dbManager.ExecuteReader(PROC_FETCH_ROS_TEMPLATE_DATA, parameters))
                {
                    while (reader.Read())
                    {
                        MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem model = new MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem();
                        var properties = typeof(MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_ReviewOfSystem.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::ReviewOfSystemDataSelect", PROC_FETCH_ROS_TEMPLATE_DATA, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_ReviewOfSystem;
        }

        public MDVision.Model.Clinical.Notes.Notes.TemplateData NotesTemplateDataWithTagsSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MDVision.Model.Clinical.Notes.Notes.TemplateData> objList_TemplateData = new List<MDVision.Model.Clinical.Notes.Notes.TemplateData>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENTID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTES_TEMPID, objCommonSearch.TemplateId));
                parameters.Add(new SqlParameter(PARM_USERID, objCommonSearch.UserID));
                parameters.Add(new SqlParameter(PARM_NOTES_ID, DBNull.Value));
                parameters.Add(new SqlParameter(PARM_MODE, false));
                parameters.Add(new SqlParameter(PARM_IS_NOTE_TEXT, false));
                using (var reader = dbManager.ExecuteReader(PROC_POP_NOTE_TEMPDATA, parameters))
                {
                    while (reader.Read())
                    {
                        MDVision.Model.Clinical.Notes.Notes.TemplateData model = new MDVision.Model.Clinical.Notes.Notes.TemplateData();
                        var properties = typeof(MDVision.Model.Clinical.Notes.Notes.TemplateData).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_TemplateData.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::NotesTemplateDataWithTagsSelect", PROC_POP_NOTE_TEMPDATA, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            if (objList_TemplateData == null || objList_TemplateData.Count() == 0)
            {
                return new MDVision.Model.Clinical.Notes.Notes.TemplateData();
            }
            else
            {
                return objList_TemplateData.FirstOrDefault();
            }
        }

        #endregion  Template Data Print
        private const string EXCLUDED_IMAGE_IDS = "@ExcludedImageIds";
        public List<NoteDocumentModel> loadNoteDocsForClassicView(long NotesId, string ExcludedImageIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NoteDocumentModel> model_ = new List<NoteDocumentModel>();
                SqlDataReader reader = null;

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                dbManager.AddParameters(1, EXCLUDED_IMAGE_IDS, ExcludedImageIds);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTES_DOCS_CLASSICVIEW);
                while (reader.Read())
                {
                    NoteDocumentModel item = new NoteDocumentModel();
                    item.DocumentURL = MDVUtility.ToStr(reader["Url"]);
                    item.PatDocId = MDVUtility.ToStr(reader["PatDocId"]);
                    item.NoteId = MDVUtility.ToStr(reader["MedicalDocId"]);

                    model_.Add(item);
                }

                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::loadNoteDocsForClassicView", PROC_NOTES_DOCS_CLASSICVIEW, ex);
                // MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteComponents", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NoteComponentModel> loadNoteComponentsForpdf(IDBManager dbManager, long notesId)
        {
            // IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<NoteComponentModel> model_ = new List<NoteComponentModel>();
                SqlDataReader reader = null;

                // dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTES_ID, notesId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_COMPONENT_SELECT_FOR_PDF);
                while (reader.Read())
                {
                    NoteComponentModel item = new NoteComponentModel();
                    item.NoteComponentId = MDVUtility.ToLong(reader["NoteComponentId"]);
                    item.NotesId = MDVUtility.ToLong(reader["NotesId"]);
                    item.NoteComponentsLookupId = MDVUtility.ToLong(reader["NoteComponentsLookupId"]);
                    item.SOAPText = MDVUtility.ToStr(reader["SOAPText"]);
                    item.OrderNo = MDVUtility.ToInt16(reader["OrderNo"]);
                    item.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                    item.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    item.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                    item.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    item.ComponentName = MDVUtility.ToStr(reader["ComponentName"]);
                    item.SectionName = MDVUtility.ToStr(reader["SectionName"]);
                    item.NoteSectionsLookupId = MDVUtility.ToLong(reader["NoteSectionsLookupId"]);
                    item.IsFromTemplate = MDVUtility.ToStr(reader["TemplateId"]) == "" ? false : true;
                    item.VisitId = MDVUtility.ToLong(reader["VisitId"]);
                    item.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                    item.ProviderId = MDVUtility.ToLong(reader["ProviderId"]);
                    model_.Add(item);
                }

                return model_;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALNotes::loadNoteComponents", PROC_NOTE_COMPONENT_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALNotes::loadNoteComponents", null);
                throw ex;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }


        public NoteComponentModel Sign_Note(long NotesId, string FromCCM, bool ConfirmSign,string NoteMissingDataReason)
        {

            NoteComponentModel model_ = null;
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);


                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);
                if (string.IsNullOrEmpty(FromCCM))
                    dbManager.AddParameters(1, PARM_FROM_CCM, null);
                else
                    dbManager.AddParameters(1, PARM_FROM_CCM, FromCCM);

                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_CONFIRM_SIGN, ConfirmSign);
                dbManager.AddParameters(5, PARM_NOTE_MISSING_DATA_REASON, NoteMissingDataReason);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_SIGN_NOTES);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model_ = new NoteComponentModel();
                        model_.ErrorMessage = MDVUtility.ToStr(reader["ErrorMessage"]);
                        if (model_.ErrorMessage != "This note is already signed.")
                        {
                            model_.SOAPText = MDVUtility.ToStr(reader["SOAPText"]);
                            model_.VisitId = MDVUtility.ToLong(reader["VisitId"]);
                            model_.PatientId = MDVUtility.ToLong(reader["PatientId"]);
                            model_.BillingInfoId = MDVUtility.ToLong(reader["BillingInfoId"]);
                            model_.IsProblemMissed = MDVUtility.ToBool(reader["IsProblemMissed"]);
                            model_.IsProcedureMissed = MDVUtility.ToBool(reader["IsProcedureMissed"]);
                            model_.IsNoteSignWOCPTCode = MDVUtility.ToBool(reader["IsNoteSignWOCPTCode"]);
                            model_.IsNoteSignWOICDCode = MDVUtility.ToBool(reader["IsNoteSignWOICDCode"]);
                            model_.IsPhoneEncounter = MDVUtility.ToBool(reader["IsPhoneEncounter"]);
                            model_.IsNonBillable = MDVUtility.ToBool(reader["IsNonBilable"]);
                            model_.MUAlertsCount= MDVUtility.ToLong(reader["MUAlertsCount"]);
                        }
                    }
                }
                return model_;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::Sign_Note", PROC_SIGN_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NoteComponentModel> Sign_Note_Multiple(string FromCCM, bool ConfirmSign, DataTable dtNotesIds,string NoteMissingDataReason)
        {
            List<NoteComponentModel> modellist = new List<NoteComponentModel>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_NOTES_IDS, dtNotesIds);
                if (string.IsNullOrEmpty(FromCCM))
                    dbManager.AddParameters(1, PARM_FROM_CCM, null);
                else
                    dbManager.AddParameters(1, PARM_FROM_CCM, FromCCM);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_CONFIRM_SIGN, ConfirmSign);
                dbManager.AddParameters(5, PARM_NOTE_MISSING_DATA_REASON, NoteMissingDataReason);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_SIGN_NOTES_MULTIPLE);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        modellist.Add(new NoteComponentModel
                        {
                            ErrorMessage = MDVUtility.ToStr(reader["ErrorMessage"]),
                            NotesId = MDVUtility.ToLong(reader["NotesId"]),
                        });
                    }
                }
                return modellist;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::Sign_Note_Multiple", PROC_SIGN_NOTES_MULTIPLE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<NoteComponentModel> NoteReadytoSign_multiple(DataTable dtNotesIds)
        {

            List<NoteComponentModel> modellist = new List<NoteComponentModel>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //string returnVal = string.Empty;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_NOTES_IDS, dtNotesIds);
                dbManager.AddParameters(1, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_READYTOSIGN_INSERT);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        modellist.Add(new NoteComponentModel
                        {
                            NotesId = MDVUtility.ToLong(reader["NotesId"]),
                            ProviderId = MDVUtility.ToLong(reader["ProviderId"]),
                            PatientId = MDVUtility.ToLong(reader["PatientId"]),
                            CPT = MDVUtility.ToBool(reader["CPT"]),
                            ICD = MDVUtility.ToBool(reader["ICD"]),
                            CDSAlerts = MDVUtility.ToBool(reader["CDSAlert"])
                        });
                    }
                }
                return modellist;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::NoteReadytoSign_multiple", PROC_NOTE_READYTOSIGN_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Inserts the Patient Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatient InsertSignedNoteDocument(IDBManager dbManager, DSPatient ds)
        {
            try
            {
                CreateParametersForPatDoc(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_INSERT, ds, ds.PatientDocument.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocument", PROC_PATIENT_DOCUMENT_INSERT, ex);
                throw ex;
            }

        }
        private void CreateParametersForPatDoc(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(37);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, ds.PatientDocument.PatDocIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_MEDICAL_DOC_ID, ds.PatientDocument.MedicalDocIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(2, PARM_DOCUMENT_ID, ds.PatientDocument.DocumentidColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(3, PARM_PATIENT_ID, ds.PatientDocument.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_DOS, ds.PatientDocument.DOSColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, PARM_CLAIM_ID, ds.PatientDocument.ClaimIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, PARM_CASE_ID, ds.PatientDocument.CaseIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(7, PARM_VISIT_ID, ds.PatientDocument.VisitIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, PARM_TRANSITION_ID, ds.PatientDocument.TransitionIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(9, PARM_FILE_TYPE, ds.PatientDocument.FileTypeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, PARM_FILE_PATH, ds.PatientDocument.FilePathColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, PARM_FILE_STREAM, ds.PatientDocument.FileStreamColumn.ColumnName, DbType.Binary);
                dbManager.AddParameters(12, PARM_PAGES, ds.PatientDocument.PagesColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(13, PARM_VIEW_BY, ds.PatientDocument.ViewByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_VIEW_DATE, ds.PatientDocument.ViewDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(15, PARM_REVIEW_BY, ds.PatientDocument.ReviewByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(16, PARM_REVIEW_DATE, ds.PatientDocument.ReviewDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(17, PARM_COMMENTS, ds.PatientDocument.CommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(18, PARM_IS_ATTACHED, ds.PatientDocument.IsAttachedColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(19, PARM_IS_ACTIVE, ds.PatientDocument.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(20, PARM_CREATED_BY, ds.PatientDocument.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(21, PARM_CREATED_ON, ds.PatientDocument.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(22, PARM_MODIFIED_BY, ds.PatientDocument.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(23, PARM_MODIFIED_ON, ds.PatientDocument.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(24, PARM_ASSIGNED_TO_ID, ds.PatientDocument.AssignedToIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(25, PARM_MESSAGE_ID, ds.PatientDocument.MessageIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(26, PARM_SIGN_BY, ds.PatientDocument.SignByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(27, PARM_SIGN_DATE, ds.PatientDocument.SignDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(28, PARM_ADVANCE_PAYMENT_ID, ds.PatientDocument.AdvPaymentIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(29, PARM_INSURANCE_ID, ds.PatientDocument.InsuranceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(30, PARM_REF_MODULE_NAME, ds.PatientDocument.RefModuleNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(31, PARM_IS_SCAN, ds.PatientDocument.IsScanColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(32, PARM_URL, ds.PatientDocument.UrlColumn.ColumnName, DbType.String);
                dbManager.AddParameters(33, PARM_ORDER_SET_REFERRAL_ID, ds.PatientDocument.OrderSetReferralIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(34, PARM_ORDER_SET_EDUCATION_ID, ds.PatientDocument.OrderSetPatEducationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(35, PARM_ENC_PHONE_FOLDER, ds.PatientDocument.PhoneEncounterFolderColumn.ColumnName, DbType.String);
                dbManager.AddParameters(36, PARM_NOTE_HTML, ds.PatientDocument.DocumentHtmlColumn.ColumnName, DbType.String);

            }
            else
            {
                dbManager.CreateParameters(34);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, ds.PatientDocument.PatDocIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(1, PARM_DOCUMENT_ID, ds.PatientDocument.DocumentidColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientDocument.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(3, PARM_DOS, ds.PatientDocument.DOSColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(4, PARM_CLAIM_ID, ds.PatientDocument.ClaimIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(5, PARM_CASE_ID, ds.PatientDocument.CaseIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, PARM_VISIT_ID, ds.PatientDocument.VisitIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(7, PARM_TRANSITION_ID, ds.PatientDocument.TransitionIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, PARM_FILE_TYPE, ds.PatientDocument.FileTypeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, PARM_FILE_PATH, ds.PatientDocument.FilePathColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, PARM_FILE_STREAM, ds.PatientDocument.FileStreamColumn.ColumnName, DbType.Binary);
                dbManager.AddParameters(11, PARM_PAGES, ds.PatientDocument.PagesColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(12, PARM_VIEW_BY, ds.PatientDocument.ViewByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(13, PARM_VIEW_DATE, ds.PatientDocument.ViewDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(14, PARM_REVIEW_BY, ds.PatientDocument.ReviewByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(15, PARM_REVIEW_DATE, ds.PatientDocument.ReviewDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(16, PARM_COMMENTS, ds.PatientDocument.CommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(17, PARM_IS_ATTACHED, ds.PatientDocument.IsAttachedColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(18, PARM_IS_ACTIVE, ds.PatientDocument.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(19, PARM_CREATED_BY, ds.PatientDocument.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(20, PARM_CREATED_ON, ds.PatientDocument.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(21, PARM_MODIFIED_BY, ds.PatientDocument.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(22, PARM_MODIFIED_ON, ds.PatientDocument.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(23, PARM_ASSIGNED_TO_ID, ds.PatientDocument.AssignedToIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(24, PARM_MESSAGE_ID, ds.PatientDocument.MessageIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(25, PARM_SIGN_BY, ds.PatientDocument.SignByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(26, PARM_SIGN_DATE, ds.PatientDocument.SignDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(27, PARM_B_UPDATE_STREAM, ds.PatientDocument.BUpdateStreamColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(28, PARM_ADVANCE_PAYMENT_ID, ds.PatientDocument.AdvPaymentIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(29, PARM_INSURANCE_ID, ds.PatientDocument.InsuranceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(30, PARM_IS_SCAN, ds.PatientDocument.IsScanColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(31, PARM_ORDER_SET_REFERRAL_ID, ds.PatientDocument.OrderSetReferralIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(32, PARM_ORDER_SET_EDUCATION_ID, ds.PatientDocument.OrderSetPatEducationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(33, PARM_URL, ds.PatientDocument.UrlColumn.ColumnName, DbType.String);

            }
        }

        public ClinicalNotesModel LoadPreviouNotePEAndROS(long PatientId, bool IsPhoneEncounter, long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                ClinicalNotesModel model = new ClinicalNotesModel();
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USERID, MDVUtility.ToInt64(MDVSession.Current.AppUserId)));
                parameters.Add(new SqlParameter(PARM_ENTITYID, Convert.ToInt64(MDVSession.Current.EntityId)));
                parameters.Add(new SqlParameter(PARM_IS_PHONE_ENCOUNTER, IsPhoneEncounter));
                parameters.Add(new SqlParameter(PARM_PROVIDER_ID, ProviderId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_SELECT_PREVIOUS_NOTE_PE_AND_ROS, parameters))
                {
                    while (reader.Read())
                    {
                        model.ROSTemplateId = MDVUtility.ToStr(reader["ROSTemplateId"]);
                        model.PETemplateId = MDVUtility.ToStr(reader["PETemplateId"]);
                        model.PrevNotesIdPE = MDVUtility.ToInt64(reader["PrevNotesIdPE"]);
                        model.PrevNotesIdROS = MDVUtility.ToInt64(reader["PrevNotesIdROS"]);
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_SELECT_PREVIOUS_NOTE_PE_AND_ROS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<NotesComponentDataFixModel> loadClinicalSignedNotesForPDFFix()
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                List<NotesComponentDataFixModel> notesList = new List<NotesComponentDataFixModel>();
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, "[Clinical].[SelectSignedNotesPDFFix]"))
                {
                    while (reader.Read())
                    {
                        notesList.Add(new NotesComponentDataFixModel { NotesIds = MDVUtility.ToStr(reader["NotesId"]) });
                    }
                }

                return notesList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::loadClinicalSignedNotesForPDFFix", "[Clinical].[SelectSignedNotesPDFFix]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public void updateClinicalSignedNotesForPDFFix(string noteid, string error)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_NOTES_ID, noteid);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, error);

                dbManager.ExecuteScalar("[Clinical].[UpdateSignedNotesPDFFix]");

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::LoadClinical_Notes", PROC_Notes_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public void UpdateIsNonBillableInfo(Int64 NotesId, bool IsNonBilable)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0,PARM_NOTEID, NotesId);
                dbManager.AddParameters(1,PARM_IS_NON_BILABLE, IsNonBilable);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_ISNONBILLABLE_INFO);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::UpdateIsNonBillableInfo", PROC_UPDATE_ISNONBILLABLE_INFO, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string GetIsNonBillableInfo(Int64 NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_NOTEID, NotesId);
                return dbManager.ExecuteScalar(PROC_GET_ISNONBILLABLE_INFO).ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNotes::GetIsNonBillableInfo", PROC_GET_ISNONBILLABLE_INFO, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
