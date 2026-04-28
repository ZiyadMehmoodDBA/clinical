using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Model.Dashboard;
using System.Data.SqlClient;
using MDVision.Common.Logging;
using MDVision.Model.FaceSheet;
using MDVision.Common.Utilities;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Patient;
using MDVision.Model.Document;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientDocument
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientDocument()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALPatientDocument(SharedVariable sharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(sharedVariable);

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
        private const string PROC_PATIENT_DOCUMENT_DELETE = "Patient.sp_PatientDocumentDelete";
        private const string PROC_PATIENT_DEATTACH_CLAIM_DOCUMENT = "Patient.sp_PatientDetachClaimDocument";
        private const string PROC_PATIENT_DOCUMENT_SELECT_BY_FOLDER = "Patient.sp_PatientDocumentSelectByFolder";
        private const string PROC_PATIENT_INSURANCE_DOCUMENT_DELETE = "Patient.sp_PatientInsuranceDocumentDelete";
        private const string PROC_PATIENT_DOCUMENT_INSERT = "Patient.sp_PatientDocumentInsert";
        private const string PROC_PATIENT_DOCUMENT_SELECT = "Patient.sp_PatientDocumentSelect";
        private const string PROC_PATIENT_DOCUMENT_SEARCH = "Patient.sp_PatientDocumentSearch";
        private const string PROC_PATIENT_DOCUMENT_AUDIOFILE_SEARCH = "Patient.sp_PatientAudioFileSearch";
        private const string PROC_PATIENT_DOCUMENT_UPDATE = "Patient.sp_PatientDocumentUpdate";
        private const string PROC_MOVE_PATIENT_FOLDER = "Patient.sp_MovePatientFolders";
        private const string PROC_PATIENT_INSURANCE_DOCUMENT_UPDATE = "[Patient].[sp_PatientInsuranceImgUpdate]";
        private const string PROC_PATIENT_DOCUMENT_UPDATE_FROM_NOTES = "Clinical.sp_UpdateCustomFormFromNotes";
        private const string PROC_DASHBOARD_DOCUMENT_SELECT = "Patient.sp_D_DocumentSelect";
        private const string PROC_PATIENT_DOCUMENT_SELECT_For_FaceSheet = "Clinical.sp_FaceSheetPatientDocumentSelect";
        private const string PROC_BILLING_UNALLOCATED_COPAY_DOCUMENT_DELETE = "Billing.sp_Delete_Unallocated_Copay_Document";

        private const string PROC_NOTE_DOCUMENT_SELECT = "Clinical.sp_NoteDocumentSelect";
        private const string PROC_NOTE_DOCUMENT_INSERT = "Clinical.sp_NoteDocumentInsert";
        private const string PROC_NOTE_DOCUMENT_UPDATE = "Clinical.sp_NoteDocumentUpdate";
        private const string PROC_NOTE_DOCUMENT_DELETE = "Clinical.sp_NoteDocumentDelete";
        private const string PROC_NOTE_DOCUMENT_DETACH = "Clinical.sp_DetachDocumentsFromNote";
        private const string PROC_PATIENT_ATTACHED_DOCUMENT_SELECT = "Patient.sp_PatientAttachedDocumentSelect";
        private const string PROC_PATIENT_PORTAL_DOCUMENT_SELECT = "Patient.sp_PatientPortalDocumentSelect";
        private const string PROC_PATIENT_PORTAL_DOCUMENT_UPDATE = "Patient.sp_PatientPortalDocumentUpdate";
        private const string PROC_PATIENT_DOCUMENT_PRIORITY_SELECT = "Patient.DocumentPrioritySelect";
        private const string PROC_PATIENT_VISIT_DOS = "Patient.sp_PatientVisitDOS";
        private const string PROC_PATIENT_DOCUMENTLINK_SELECT = "[Patient].[sp_PatientDocumentLinkSelect]";
        private const string PROC_PATIENT_DOCUMENT_EXPIRY_ALERT_PROMPT = "[Patient].[sp_DocumentExpiryAlertPrompt]";
        private const string PROC_USER_DOCUMENT_EXPIRY_ALERT_INSERT = "[Patient].[sp_UserDocumentExpiryAlertInsert]";
        private const string PROC_CHECK_FOR_PRIVACY = "Patient.sp_DocumentPrivacy";
        private const string PROC_DOC_PASSWORD_MATCH = "Patient.sp_DocPasswordMatch";
        private const string PROC_DOC_PASSWORD_SAVE = "Patient.sp_DocPasswordSave";
        private const string PROC_DOC_USER_ACCESS_INSERT = "Patient.sp_UserDocumentAccessInsert";
        private const string PROC_PATIENT_DOCUMENT_TAGS = "Patient.sp_PatientDocumentTagsInsert";
        private const string PROC_DOC_CHANGE_REMOVE_PASSWORD = "Patient.sp_DocChangeRemovePassword";
        private const string PROC_PATIENT_DOCUMENT_UPDATE_FROM_CLAIM = "Patient.sp_UpdateDocumentFromClaim";
        #endregion

        #region Parameters
        private const string PARM_PATIENT_DOC_ID = "@PatDocId";
        private const string PARM_FOLDER_IDS = "@FolderIDs";
        private const string PARM_FOLDER_XML = "@Xml";
        private const string PARM_CHILD_PAT_DOC_ID = "@ChildPatDocId";
        private const string PARM_LINK_DOC_ID = "@LinkDocumentId";
        private const string PARM_MEDICAL_DOC_ID = "@MedicalDocId";
        private const string PARM_DOCUMENT_ID = "@Documentid";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_DOS = "@DOS";
        private const string PARM_CLAIM_ID = "@ClaimId";
        private const string PARM_CASE_ID = "@CaseId";
        private const string PARM_VISIT_ID = "@VisitId";
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
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ATTACHED = "@IsAttached";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_FROM_DOS = "@FromDOS";
        private const string PARM_TO_DOS = "@ToDOS";
        private const string PARM_User_ID = "@UserId";      
        private const string PARM_User_CURRENT_ID = "@CurrentUserId";
        private const string PARM_FROM_ENTRY_DATE = "@FromEntryDate";
        private const string PARM_TO_ENTRY_DATE = "@ToEntryDate";
        private const string PARM_ENTERED_BY = "@EnteredBy";
        private const string PARM_ASSIGNED_BY_ID = "@AssignedById";
        private const string PARM_REVIEWED_BY_ID = "@ReviewedById";
        private const string PARM_IS_REVIEWED = "@IsReviewed";
        private const string PARM_ASSIGNED_TO_ID = "@AssignedToId";
        private const string PARM_MESSAGE_ID = "@MessageId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SIGN_BY = "@SignBy";
        private const string PARM_SIGN_DATE = "@SignDate";
        private const string PARM_B_UPDATE_STREAM = "@BUpdateStream";
        private const string PARM_ADVANCE_PAYMENT_ID = "@AdvPaymentId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_IS_FIRST_LOAD = "@IsFirstLoad";
        private const string PARM_REF_MODULE_NAME = "@RefModuleName";
        private const string PARM_IS_SCAN = "@IsScan";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_URL = "@url";
        private const string PARM_FILE_URL = "@FileURL";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ORDER_SET_REFERRAL_ID = "@OrderSetReferralId";
        private const string PARM_ORDER_SET_EDUCATION_ID = "@OrderSetPatEducationId";
        private const string PARM_ENC_PHONE_FOLDER = "@PhoneEncounterFolder";
        private const string PARM_INSURANCE_IMAGE = "@InsuranceCardImage";
        private const string PARM_NOTE_HTML = "@DocumentHtml";
        private const string PARM_NOTE_DOCUMENT_ID = "@NoteDocumentId";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_PATIENT_DOCUMENT_ID = "@PatientDocumentId";
        private const string PARM_DOCUMENT_SOURCE_ID = "@DocumentSourceId";
        private const string PARM_OTHER_DOCUMENT_SOURCE = "@OtherDocumentSource";
        private const string PARM_RECEIVED_ON = "@ReceivedOn";
        private const string PARM_NARRATIVE_REFERENCE = "@NarrativeReference";
        private const string PARM_REFERENCE_LINK = "@ReferenceLink";
        private const string PARM_DOCUMENT_PROVIDER_ID = "@DocumentProviderId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_REASON = "@Reason";
        private const string PARM_PAT_PORTAL_DOC_ID = "@Id";
        private const string PARM_PAT_DOC_ID = "@PatDocId";
        private const string PARM_DOCUMENT_REVIEWBY_ID = "@ReviewById";
        private const string PARM_CHK_REVIEWED = "@ChkReviewed";
        private const string PARM_LOOKUP_NAME = "@LookupName";

        private const string PARM_DOC_PRIORITY_ID = "@DocPriorityID";
        private const string PARM_DOC_ASSIGN_TO_REVIEW = "@DocAssignToReviewId";
        private const string PARM_MEDICAL_DOC_IDS = "@MedicalDocIds";
        private const string PARM_PARENT_DOCUMENT_ID = "@ParentDocumentID";
        private const string PARM_DOC_IS_CALENDAR = "@IsCalendar";
        public const string PARM_PATIENT_NAME = "@PatientName";
        private const string PARM_DOC_PASSWORD = "@Password";
        private const string PARM_DOC_EXPIRTY_DATE = "@ExpiryDate";
        private const string PARM_DOC_TAG_ID = "@TagId";
        private const string PARM_FROM_EXPIRY = "@FromExpiry";
        private const string PARM_TO_EXPIRY = "@ToExpiry";
        private const string PARM_IS_RECENT_DOC = "@IsRecentDocument";
        private const string PARM_OLD_PASSWORD = "@OldPassword";
        private const string PARM_NEW_PASSWORD = "@NewPassword";
        private const string PARM_CONFIRM_NEW_PASSWORD = "@ConfirmPassword";
        private const string PARM_PASSWORD_CREATED_BY_ID = "@PasswordCreatedById";
        private const string PARM_NEXT_PENDING_DOC = "@NextPendingDoc";
        private const string PARM_EOB_MANUAL_POSTING_ID = "@EOBManualPostingId";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert, string FoldersXML)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(53);
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
                dbManager.AddParameters(37, PARM_DOCUMENT_SOURCE_ID, ds.PatientDocument.DocumentSourceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(38, PARM_OTHER_DOCUMENT_SOURCE, ds.PatientDocument.OtherDocumentSourceColumn.ColumnName, DbType.String);
                dbManager.AddParameters(39, PARM_RECEIVED_ON, ds.PatientDocument.ReceivedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(40, PARM_NARRATIVE_REFERENCE, ds.PatientDocument.NarrativeReferenceColumn.ColumnName, DbType.String);
                dbManager.AddParameters(41, PARM_REFERENCE_LINK, ds.PatientDocument.ReferenceLinkColumn.ColumnName, DbType.String);
                dbManager.AddParameters(42, PARM_DOCUMENT_PROVIDER_ID, ds.PatientDocument.DocumentProviderIdColumn.ColumnName, DbType.String);
                dbManager.AddParameters(43, PARM_DOCUMENT_REVIEWBY_ID, ds.PatientDocument.ReviewedByIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(44, PARM_DOC_PRIORITY_ID, ds.PatientDocument.DocPriorityIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(45, PARM_DOC_IS_CALENDAR, ds.PatientDocument.IsCalendarColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(46, PARM_MEDICAL_DOC_IDS, ds.PatientDocument.AttachedDocsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(47, PARM_DOC_EXPIRTY_DATE, ds.PatientDocument.ExpiryDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(48, PARM_DOC_TAG_ID, ds.PatientDocument.TagIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(49, PARM_DOC_PASSWORD, ds.PatientDocument.DocPasswordColumn.ColumnName, DbType.String);
                dbManager.AddParameters(50, PARM_User_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(51, PARM_PASSWORD_CREATED_BY_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(52, PARM_EOB_MANUAL_POSTING_ID, ds.PatientDocument.EOBManualPostingIdColumn.ColumnName,DbType.Int64);


            }
            else
            {

                dbManager.CreateParameters(47);
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
                dbManager.AddParameters(34, PARM_DOCUMENT_SOURCE_ID, ds.PatientDocument.DocumentSourceIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(35, PARM_OTHER_DOCUMENT_SOURCE, ds.PatientDocument.OtherDocumentSourceColumn.ColumnName, DbType.String);
                dbManager.AddParameters(36, PARM_RECEIVED_ON, ds.PatientDocument.ReceivedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(37, PARM_DOCUMENT_PROVIDER_ID, ds.PatientDocument.DocumentProviderIdColumn.ColumnName, DbType.String);
                dbManager.AddParameters(38, PARM_NARRATIVE_REFERENCE, ds.PatientDocument.NarrativeReferenceColumn.ColumnName, DbType.String);
                dbManager.AddParameters(39, PARM_REFERENCE_LINK, ds.PatientDocument.ReferenceLinkColumn.ColumnName, DbType.String);
                dbManager.AddParameters(40, PARM_DOCUMENT_REVIEWBY_ID, ds.PatientDocument.ReviewedByIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(41, PARM_DOC_PRIORITY_ID, ds.PatientDocument.DocPriorityIDColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(42, PARM_DOC_IS_CALENDAR, ds.PatientDocument.IsCalendarColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(43, PARM_DOC_EXPIRTY_DATE, ds.PatientDocument.ExpiryDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(44, PARM_DOC_TAG_ID, ds.PatientDocument.TagIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(45, PARM_User_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(46, PARM_FOLDER_XML, FoldersXML);
            }
        }

        private void CreateParametersForNotesUpdate(IDBManager dbManager, DSPatient ds)
        {
            dbManager.CreateParameters(2);

            dbManager.AddParameters(0, PARM_FILE_PATH, ds.PatientDocument.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(1, PARM_FILE_STREAM, ds.PatientDocument.FileStreamColumn.ColumnName, DbType.Binary);
        }
        private void CreateParametersForClaimUpdate(IDBManager dbManager, DSPatient ds)
        {
            dbManager.CreateParameters(4);

            dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, ds.PatientDocument.PatDocIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(1, PARM_VISIT_ID, ds.PatientDocument.VisitIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_MODIFIED_BY, ds.PatientPortalDocument.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MODIFIED_ON, ds.PatientPortalDocument.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateParametersForPatientPortalDocument(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {

            if (IsInsert == true)
            {
                dbManager.CreateParameters(12);

                dbManager.AddParameters(0, PARM_PAT_PORTAL_DOC_ID, ds.PatientPortalDocument.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(10);

                dbManager.AddParameters(0, PARM_PAT_PORTAL_DOC_ID, ds.PatientPortalDocument.IdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientPortalDocument.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FILE_NAME, ds.PatientPortalDocument.FileNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_FILE_TYPE, ds.PatientPortalDocument.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_URL, ds.PatientPortalDocument.UrlColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_COMMENTS, ds.PatientPortalDocument.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_STATUS, ds.PatientPortalDocument.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_REASON, ds.PatientPortalDocument.ReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.PatientPortalDocument.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.PatientPortalDocument.ModifiedOnColumn.ColumnName, DbType.DateTime);
            if (IsInsert == true)
            {
                dbManager.AddParameters(10, PARM_CREATED_BY, ds.PatientPortalDocument.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, PARM_CREATED_ON, ds.PatientPortalDocument.CreatedOnColumn.ColumnName, DbType.DateTime);
            }

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the patient Documents.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        public DSPatient LoadPatientDocument(string PatientDocId, long PatientId, string PatientAccountNumber, string PatientLastName
                                    , string PatientFirstName, DateTime? FromDOS, DateTime? ToDOS, DateTime? FromEntryDate
                                    , DateTime? ToEntryDate, string EnteredBy, long AssignedById, long ReviewedById, string IsReviewed, int DocumentId, string IsActive, string FileStream, Int64 AdvancePaymentId, int PageNumber = 1, int RowspPage = 15, Int32 AssignedToId = 0, Int64 TransitionId = 0, string RefModuleName = "", long OrderSetReferralId = 0, long OrderSetPatEducationId = 0, SharedVariable sharedVariable = null)
        {

            string username = string.Empty;
            string EntityId = string.Empty;

            if (sharedVariable != null)
            {
                username = sharedVariable.UserName;
                EntityId = sharedVariable.EntityId;
            }
            else
            {
                username = MDVSession.Current.AppUserName;
                EntityId = MDVSession.Current.EntityId;
            }

            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (PatientDocId == "")
                    PatientDocId = null;

                if (PatientAccountNumber == "")
                    PatientAccountNumber = null;

                if (PatientLastName == "")
                    PatientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;

                if (EnteredBy == "")
                    EnteredBy = null;

                if (IsReviewed == "")
                    IsReviewed = null;

                if (IsActive == "")
                    IsActive = null;
                if (RefModuleName == "")
                    RefModuleName = null;

                dbManager.Open();
                dbManager.CreateParameters(28);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, PatientDocId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, PatientAccountNumber);
                dbManager.AddParameters(3, PARM_LAST_NAME, PatientLastName);
                dbManager.AddParameters(4, PARM_FIRST_NAME, PatientFirstName);
                dbManager.AddParameters(5, PARM_FROM_DOS, FromDOS);
                dbManager.AddParameters(6, PARM_TO_DOS, ToDOS);
                dbManager.AddParameters(7, PARM_FROM_ENTRY_DATE, FromEntryDate);
                dbManager.AddParameters(8, PARM_TO_ENTRY_DATE, ToEntryDate);
                dbManager.AddParameters(9, PARM_ENTERED_BY, EnteredBy);

                if (AssignedById == 0)
                    dbManager.AddParameters(10, PARM_ASSIGNED_BY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_ASSIGNED_BY_ID, AssignedById);

                if (ReviewedById == 0)
                    dbManager.AddParameters(11, PARM_REVIEWED_BY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_REVIEWED_BY_ID, ReviewedById);

                dbManager.AddParameters(12, PARM_IS_REVIEWED, IsReviewed);

                if (DocumentId == 0)
                    dbManager.AddParameters(13, PARM_DOCUMENT_ID, null);
                else
                    dbManager.AddParameters(13, PARM_DOCUMENT_ID, DocumentId);

                dbManager.AddParameters(14, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(15, PARM_FILE_STREAM, FileStream);

                if (AdvancePaymentId == 0)
                    dbManager.AddParameters(16, PARM_ADVANCE_PAYMENT_ID, null);
                else
                    dbManager.AddParameters(16, PARM_ADVANCE_PAYMENT_ID, AdvancePaymentId);

                if (PageNumber == 0)
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(19, PARM_RECORD_COUNT, ds.PatientDocument.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(username).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(20, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(20, PARM_ENTITY_ID, EntityId);

                if (AssignedToId == 0)
                    dbManager.AddParameters(21, PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(21, PARM_ASSIGNED_TO_ID, AssignedToId);
                // For time being, since IsFirstLoad is not being sent, so it is sent as 0 for now
                dbManager.AddParameters(22, PARM_IS_FIRST_LOAD, 0);
                if (TransitionId <= 0)
                    dbManager.AddParameters(23, PARM_TRANSITION_ID, null);
                else
                    dbManager.AddParameters(23, PARM_TRANSITION_ID, TransitionId);

                dbManager.AddParameters(24, PARM_REF_MODULE_NAME, RefModuleName);

                if (OrderSetReferralId == 0)
                    dbManager.AddParameters(25, PARM_ORDER_SET_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(25, PARM_ORDER_SET_REFERRAL_ID, OrderSetReferralId);

                if (OrderSetPatEducationId == 0)
                    dbManager.AddParameters(26, PARM_ORDER_SET_EDUCATION_ID, null);
                else
                    dbManager.AddParameters(26, PARM_ORDER_SET_EDUCATION_ID, OrderSetPatEducationId);

                dbManager.AddParameters(27, PARM_User_ID, MDVSession.Current.AppUserId);


                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_SELECT, ds, ds.PatientDocument.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadPatientDocument", PROC_PATIENT_DOCUMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSPatient loadPatientDocumentForFaceSheet(long patientId, long pageNumber, long rowsPerPage)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            dbManager.CreateParameters(4);
            if (patientId > 0)
                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
            else
                dbManager.AddParameters(0, PARM_PATIENT_ID, null);

            if (pageNumber > 0)
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
            else
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
            if (rowsPerPage > 0)
                dbManager.AddParameters(2, PARM_ROWS_PERPAGE, rowsPerPage);
            else
                dbManager.AddParameters(2, PARM_ROWS_PERPAGE, null);

            dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.PatientDocument.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_SELECT_For_FaceSheet, ds, ds.PatientDocument.TableName);

            dbManager.Dispose();
            return ds;
        }
        public DSPatient searchPatientDocument(string PatientDocId, long PatientId, string PatientAccountNumber, DateTime? FromDOS, DateTime? ToDOS, DateTime? FromEntryDate, DateTime? ToEntryDate, string EnteredBy, long AssignedById, string IsReviewed, int DocumentId, string FileStream, Int64 advancePaymentId = 0, int PageNumber = 1, int RowspPage = 15, Int32 AssignedToId = 0, string PatientLastName = null, string PatientFirstName = null, long NoteId = 0, string DocPriority = "", DateTime? FromExpiry = null, DateTime? ToExpiry = null, Int64 TagId = 0, bool IsRecentDocs = false)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (PatientDocId == "")
                    PatientDocId = null;

                if (PatientAccountNumber == "")
                    PatientAccountNumber = null;

                if (EnteredBy == "")
                    EnteredBy = null;

                if (IsReviewed == "")
                    IsReviewed = null;

                //if (RecentDocument == "")
                //    RecentDocument = null;

                if (PatientLastName == "")
                    PatientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;
                if (DocPriority == "")
                    DocPriority = null;
                dbManager.Open();
                dbManager.CreateParameters(28);

                if (PatientId == 0 || PatientId == -1)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_ACCOUNT_NUMBER, PatientAccountNumber);
                dbManager.AddParameters(2, PARM_FROM_DOS, FromDOS);
                dbManager.AddParameters(3, PARM_TO_DOS, ToDOS);
                dbManager.AddParameters(4, PARM_FROM_ENTRY_DATE, FromEntryDate);
                dbManager.AddParameters(5, PARM_TO_ENTRY_DATE, ToEntryDate);
                dbManager.AddParameters(6, PARM_ENTERED_BY, EnteredBy);

                if (AssignedById == 0)
                    dbManager.AddParameters(7, PARM_ASSIGNED_BY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ASSIGNED_BY_ID, AssignedById);

                dbManager.AddParameters(8, PARM_IS_REVIEWED, IsReviewed);

                dbManager.AddParameters(9, PARM_FILE_STREAM, FileStream);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.PatientDocumentSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (AssignedToId == 0)
                    dbManager.AddParameters(14, PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(14, PARM_ASSIGNED_TO_ID, AssignedToId);

                dbManager.AddParameters(15, PARM_IS_FIRST_LOAD, 0);
                dbManager.AddParameters(16, PARM_PATIENT_DOC_ID, PatientDocId);
                if (DocumentId == 0)
                    dbManager.AddParameters(17, PARM_DOCUMENT_ID, null);
                else
                    dbManager.AddParameters(17, PARM_DOCUMENT_ID, DocumentId);
                dbManager.AddParameters(18, PARM_LAST_NAME, PatientLastName);
                dbManager.AddParameters(19, PARM_FIRST_NAME, PatientFirstName);
                if (advancePaymentId > 0)
                    dbManager.AddParameters(20, PARM_ADVANCE_PAYMENT_ID, advancePaymentId);
                else
                    dbManager.AddParameters(20, PARM_ADVANCE_PAYMENT_ID, null);

                if (NoteId > 0)
                    dbManager.AddParameters(21, PARM_NOTE_ID, NoteId);
                else
                    dbManager.AddParameters(21, PARM_NOTE_ID, null);

                dbManager.AddParameters(22, PARM_DOC_PRIORITY_ID, DocPriority);
                dbManager.AddParameters(23, PARM_FROM_EXPIRY, FromExpiry);
                dbManager.AddParameters(24, PARM_TO_EXPIRY, ToExpiry);
                dbManager.AddParameters(25, PARM_IS_RECENT_DOC, IsRecentDocs);
                if (TagId > 0)
                    dbManager.AddParameters(26, PARM_DOC_TAG_ID, TagId);
                else
                    dbManager.AddParameters(26, PARM_DOC_TAG_ID, null);
                dbManager.AddParameters(27, PARM_User_ID, MDVSession.Current.AppUserId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_SEARCH, ds, ds.PatientDocumentSearch.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::searchPatientDocument", PROC_PATIENT_DOCUMENT_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient searchPatientLinkedDocument(Int64 MedicalParentDocId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_PARENT_DOCUMENT_ID, MedicalParentDocId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENTLINK_SELECT, ds, ds.PatientDocumentSearch.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::searchPatientLinkedDocument", PROC_PATIENT_DOCUMENTLINK_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public FileData GetAudioFile(int VisitId)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                FileData model = new FileData();

                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_VISIT_ID, VisitId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_PATIENT_DOCUMENT_AUDIOFILE_SEARCH, parameters))
                {
                    while (reader.Read())
                    {
                        model.PatDocId = MDVUtility.ToStr(reader["PatDocId"]);
                        model.FilePath = MDVUtility.ToStr(reader["FilePath"]);
                        if (reader["FileStream"] != null && reader["FileStream"] != DBNull.Value)
                        {
                            model.FileStream = (byte[])reader["FileStream"];
                        }
                        model.FileType = MDVUtility.ToStr(reader["FileType"]);
                        model.Url = MDVUtility.ToStr(reader["Url"]);

                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::searchPatientDocument", PROC_PATIENT_DOCUMENT_AUDIOFILE_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the Patient Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatient UpdatePatientDocument(DSPatient ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {

                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    CreateParameters(dbManager, ds, false, null);
                    ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_UPDATE, ds, ds.PatientDocument.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientDocument", PROC_PATIENT_DOCUMENT_UPDATE, ex);
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
                    CreateParameters(dbManager, ds, false, null);
                    ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_UPDATE, ds, ds.PatientDocument.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientDocument", PROC_PATIENT_DOCUMENT_UPDATE, ex);
                    throw ex;
                }
            }
        }


        public DSPatient UpdatePatientDocumentMUltiple(DSPatient ds, string  foldersXml, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {

                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    CreateParameters(dbManager, ds, false, foldersXml);
                    // dbManager.AddParameters(0, PARM_ERADTL_ID, ERADtlId);
                    ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure,PROC_MOVE_PATIENT_FOLDER, ds, ds.PatientDocument.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientDocument", PROC_PATIENT_DOCUMENT_UPDATE, ex);
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
                    CreateParameters(dbManager, ds, false, foldersXml);
                    ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MOVE_PATIENT_FOLDER, ds, ds.PatientDocument.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientDocument", PROC_PATIENT_DOCUMENT_UPDATE, ex);
                    throw ex;
                }
            }
        }

        public DSPatient UpdatePatientInsuranceDocument(DSPatient ds, int OperationID, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    ds = UpdatePatientInsuranceDocumentConcrete(ds, OperationID, dbManager);
                    ds.AcceptChanges();
                    return ds;

                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientInsuranceDocument", PROC_PATIENT_INSURANCE_DOCUMENT_UPDATE, ex);
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
                    ds = UpdatePatientInsuranceDocumentConcrete(ds, OperationID, dbManager);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientInsuranceDocument", PROC_PATIENT_INSURANCE_DOCUMENT_UPDATE, ex);
                    throw ex;
                }

            }
        }

        private DSPatient UpdatePatientInsuranceDocumentConcrete(DSPatient ds, int OperationID, IDBManager dbManager = null)
        {
            dbManager.CreateParameters(9);

            dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientDocument.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_INSURANCE_ID, ds.PatientDocument.InsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_MODIFIED_BY, ds.PatientDocument.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_MODIFIED_ON, ds.PatientDocument.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(5, PARM_FILE_PATH, ds.PatientDocument.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_FILE_TYPE, ds.PatientDocument.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_INSURANCE_IMAGE, ds.PatientDocument.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(8, PARM_URL, ds.PatientDocument.UrlColumn.ColumnName, DbType.String);
            if (OperationID == 1)
            {
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_DOCUMENT_UPDATE, ds, ds.PatientDocument.TableName);
            }
            else
            {
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_DOCUMENT_UPDATE, ds, ds.PatientDocument.TableName);
            }
            return ds;

        }

        /// <summary>
        /// Deletes the Patient Document.
        /// </summary>
        /// <param name="DocId">The Document identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePatientDocument(string PatientDocId, string ChildPatientDocId, string LinkDocumentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, PatientDocId);

                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                if (MDVUtility.ToInt64(ChildPatientDocId) > 0)
                {
                    dbManager.AddParameters(2, PARM_CHILD_PAT_DOC_ID, ChildPatientDocId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_CHILD_PAT_DOC_ID, DBNull.Value);
                }

                if (MDVUtility.ToInt64(LinkDocumentId) > 0)
                {
                    dbManager.AddParameters(3, PARM_LINK_DOC_ID, LinkDocumentId);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_LINK_DOC_ID, DBNull.Value);
                }
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_DELETE).ToString();

                return returnVal;

                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_DELETE);

                //return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DeletePatientDocument", PROC_PATIENT_DOCUMENT_DELETE, ex);
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
        public string DeAttachClaimDocument(string PatientDocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, PatientDocId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_DEATTACH_CLAIM_DOCUMENT));
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DeAttachClaimDocument", PROC_PATIENT_DEATTACH_CLAIM_DOCUMENT, ex);
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

        public string DeleteInsuranceDocument(Int64 PatientInsuranceId, Int64 PatientID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_INSURANCE_ID, PatientInsuranceId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientID);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_DOCUMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";

                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_DELETE);

                //return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DeletePatientDocument", PROC_PATIENT_DOCUMENT_DELETE, ex);
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
        /// Fill the Patient Document.
        /// </summary>
        /// <param name="PatientDocId">The Document identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public DSPatient FillPatientDocument(int PatientDocId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (PatientDocId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, PatientDocId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_SELECT, ds, ds.PatientDocument.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::FillPatientDocument", PROC_PATIENT_DOCUMENT_SELECT, ex);
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
        public DSPatient InsertPatientDocument(DSPatient ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    CreateParameters(dbManager, ds, true, null);
                    ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_INSERT, ds, ds.PatientDocument.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocument", PROC_PATIENT_DOCUMENT_INSERT, ex);
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
                    CreateParameters(dbManager, ds, true, null);
                    ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_INSERT, ds, ds.PatientDocument.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocument", PROC_PATIENT_DOCUMENT_INSERT, ex);
                    throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }
        }

        public DSPatient LoadPatientDocumentByFolder(long PatientId, string FolderIDs)
        {


            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_FOLDER_IDS, FolderIDs);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_SELECT_BY_FOLDER, ds, ds.PatientDocument.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadPatientDocumentByFolder", PROC_PATIENT_DOCUMENT_SELECT_BY_FOLDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatient InsertDocumentUserAccess(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, ds.UserDocumentAlert.PatDOcIdColumn.ColumnName, DbType.String);
                dbManager.AddParameters(1, PARM_User_ID, ds.UserDocumentAlert.UseridColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_User_CURRENT_ID, MDVSession.Current.AppUserId);

                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DOC_USER_ACCESS_INSERT, ds, ds.UserDocumentAlert.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertDocumentUserAccess", PROC_DOC_USER_ACCESS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteUnallocatedCopayDocument(Int64 PatientID, string FileName, string dtpDOS, Int64 VisitId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                SqlDataReader reader = null;
                string returnFileURL = "";

                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);
                dbManager.AddParameters(1, PARM_FILE_NAME, FileName);
                dbManager.AddParameters(2, PARM_DOS, dtpDOS);
                if (VisitId == 0)
                    dbManager.AddParameters(3, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_VISIT_ID, VisitId);


                dbManager.AddParameters(4, PARM_FILE_URL, null, DbType.String, ParamDirection.Output, null, int.MaxValue);
                dbManager.AddParameters(5, PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, null, int.MaxValue);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_BILLING_UNALLOCATED_COPAY_DOCUMENT_DELETE);
                while (reader.Read())
                {
                    returnFileURL = reader["FileURL"].ToString();
                }
                return returnFileURL;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DeleteUnallocatedCopayDocument", PROC_BILLING_UNALLOCATED_COPAY_DOCUMENT_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient UpdatePatientDocumentFromNotes(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForNotesUpdate(dbManager, ds);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_UPDATE_FROM_NOTES, ds, ds.PatientDocument.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocument", PROC_PATIENT_DOCUMENT_UPDATE_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatient UpdatePatientDocumentFromClaim(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForClaimUpdate(dbManager, ds);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_UPDATE_FROM_CLAIM, ds, ds.PatientDocument.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientDocumentFromClaim", PROC_PATIENT_DOCUMENT_UPDATE_FROM_CLAIM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientDocumentModel> LoadAttachedPatientDocument(string PatientDocId, long PatientId, string PatientAccountNumber, string PatientLastName
                                   , string PatientFirstName, DateTime? FromDOS, DateTime? ToDOS, DateTime? FromEntryDate
                                   , DateTime? ToEntryDate, string EnteredBy, long AssignedById, long ReviewedById, string IsReviewed, int DocumentId, string IsActive, string FileStream, Int64 AdvancePaymentId, int PageNumber = 1, int RowspPage = 15, Int32 AssignedToId = 0, Int64 TransitionId = 0, string RefModuleName = "", long OrderSetReferralId = 0, long OrderSetPatEducationId = 0, SharedVariable sharedVariable = null)
        {
            List<PatientDocumentModel> documentList = new List<PatientDocumentModel>();
            SqlDataReader reader = null;

            string username = string.Empty;
            string EntityId = string.Empty;

            if (sharedVariable != null)
            {
                username = sharedVariable.UserName;
                EntityId = sharedVariable.EntityId;
            }
            else
            {
                username = MDVSession.Current.AppUserName;
                EntityId = MDVSession.Current.EntityId;
            }

            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (PatientDocId == "")
                    PatientDocId = null;

                if (PatientAccountNumber == "")
                    PatientAccountNumber = null;

                if (PatientLastName == "")
                    PatientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;

                if (EnteredBy == "")
                    EnteredBy = null;

                if (IsReviewed == "")
                    IsReviewed = null;

                if (IsActive == "")
                    IsActive = null;
                if (RefModuleName == "")
                    RefModuleName = null;

                dbManager.Open();
                dbManager.CreateParameters(28);
                dbManager.AddParameters(0, PARM_PATIENT_DOC_ID, PatientDocId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, PatientAccountNumber);
                dbManager.AddParameters(3, PARM_LAST_NAME, PatientLastName);
                dbManager.AddParameters(4, PARM_FIRST_NAME, PatientFirstName);
                dbManager.AddParameters(5, PARM_FROM_DOS, FromDOS);
                dbManager.AddParameters(6, PARM_TO_DOS, ToDOS);
                dbManager.AddParameters(7, PARM_FROM_ENTRY_DATE, FromEntryDate);
                dbManager.AddParameters(8, PARM_TO_ENTRY_DATE, ToEntryDate);
                dbManager.AddParameters(9, PARM_ENTERED_BY, EnteredBy);

                if (AssignedById == 0)
                    dbManager.AddParameters(10, PARM_ASSIGNED_BY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_ASSIGNED_BY_ID, AssignedById);

                if (ReviewedById == 0)
                    dbManager.AddParameters(11, PARM_REVIEWED_BY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_REVIEWED_BY_ID, ReviewedById);

                dbManager.AddParameters(12, PARM_IS_REVIEWED, IsReviewed);

                if (DocumentId == 0)
                    dbManager.AddParameters(13, PARM_DOCUMENT_ID, null);
                else
                    dbManager.AddParameters(13, PARM_DOCUMENT_ID, DocumentId);

                dbManager.AddParameters(14, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(15, PARM_FILE_STREAM, FileStream);

                if (AdvancePaymentId == 0)
                    dbManager.AddParameters(16, PARM_ADVANCE_PAYMENT_ID, null);
                else
                    dbManager.AddParameters(16, PARM_ADVANCE_PAYMENT_ID, AdvancePaymentId);

                if (PageNumber == 0)
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(19, PARM_RECORD_COUNT, ds.PatientDocument.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(username).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(20, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(20, PARM_ENTITY_ID, EntityId);

                if (AssignedToId == 0)
                    dbManager.AddParameters(21, PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(21, PARM_ASSIGNED_TO_ID, AssignedToId);
                // For time being, since IsFirstLoad is not being sent, so it is sent as 0 for now
                dbManager.AddParameters(22, PARM_IS_FIRST_LOAD, 0);
                if (TransitionId <= 0)
                    dbManager.AddParameters(23, PARM_TRANSITION_ID, null);
                else
                    dbManager.AddParameters(23, PARM_TRANSITION_ID, TransitionId);

                dbManager.AddParameters(24, PARM_REF_MODULE_NAME, RefModuleName);

                if (OrderSetReferralId == 0)
                    dbManager.AddParameters(25, PARM_ORDER_SET_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(25, PARM_ORDER_SET_REFERRAL_ID, OrderSetReferralId);

                if (OrderSetPatEducationId == 0)
                    dbManager.AddParameters(26, PARM_ORDER_SET_EDUCATION_ID, null);
                else
                    dbManager.AddParameters(26, PARM_ORDER_SET_EDUCATION_ID, OrderSetPatEducationId);

                dbManager.AddParameters(27, PARM_User_ID, MDVSession.Current.AppUserId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_ATTACHED_DOCUMENT_SELECT);
                PatientDocumentModel model = null;
                while (reader.Read())
                {
                    model = new PatientDocumentModel();
                    model.PatDocId = !String.IsNullOrEmpty(reader["PatDocId"].ToString()) ? reader["PatDocId"].ToString() : "";
                    model.DocumentType = !String.IsNullOrEmpty(reader["DocumentType"].ToString()) ? reader["DocumentType"].ToString() : "";
                    model.Documentid = !String.IsNullOrEmpty(reader["Documentid"].ToString()) ? reader["Documentid"].ToString() : "";
                    model.DocumentName = !String.IsNullOrEmpty(reader["DocumentName"].ToString()) ? reader["DocumentName"].ToString() : "";
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.FileType = !String.IsNullOrEmpty(reader["FileType"].ToString()) ? reader["FileType"].ToString() : "";
                    model.FilePath = !String.IsNullOrEmpty(reader["FilePath"].ToString()) ? reader["FilePath"].ToString() : "";
                    model.FileStream = !String.IsNullOrEmpty(reader["FileStream"].ToString()) ? reader["FileStream"] as byte[] : null;
                    model.Url = !String.IsNullOrEmpty(reader["Url"].ToString()) ? reader["Url"].ToString() : "";
                    documentList.Add(model);
                }

                return documentList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadAttachedPatientDocument", PROC_PATIENT_ATTACHED_DOCUMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient LoadPatientInformationSubmission(long Id, long PatientId, string Status = "", int PageNumber = 1, int RowspPage = 15)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(6);

                if (Id <= 0)
                    dbManager.AddParameters(0, PARM_PAT_PORTAL_DOC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAT_PORTAL_DOC_ID, Id);

                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (string.IsNullOrEmpty(Status))
                    dbManager.AddParameters(2, PARM_STATUS, null);
                else
                    dbManager.AddParameters(2, PARM_STATUS, Status);

                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PatientPortalDocument.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PORTAL_DOCUMENT_SELECT, ds, ds.PatientPortalDocument.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadPatientInformationSubmission", PROC_PATIENT_PORTAL_DOCUMENT_SELECT, ex);
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

        public DSPatient UpdatePatientInformationSubmission(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForPatientPortalDocument(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PORTAL_DOCUMENT_UPDATE, ds, ds.PatientPortalDocument.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::UpdatePatientInformationSubmission", PROC_PATIENT_PORTAL_DOCUMENT_UPDATE, ex);
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
        public List<PatientDocumentPriorityModel> LoadPatientDocumentPriority()
        {
            List<PatientDocumentPriorityModel> docPriorityModel = new List<PatientDocumentPriorityModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_PRIORITY_SELECT);
                PatientDocumentPriorityModel model = null;
                while (reader.Read())
                {
                    model = new PatientDocumentPriorityModel();
                    model.DocumentPriorityId = reader["DocumentPriorityId"].ToString();
                    model.Name = reader["Name"].ToString();
                    docPriorityModel.Add(model);
                }
                return docPriorityModel;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALDocument::LoadPatientDocumentPriority", PROC_PATIENT_DOCUMENT_PRIORITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Get ALL Patient Visit DOS 
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public List<PatientVisitDOS> GetPatientVisitDOS(Int64 PatientId)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<PatientVisitDOS> dosmodel = new List<PatientVisitDOS>();
                PatientVisitDOS model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_PATIENT_VISIT_DOS, parameters))
                {
                    while (reader.Read())
                    {
                        model = new PatientVisitDOS();
                        //model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                        model.DOSFrom = MDVUtility.ToStr(reader["DOSFrom"]);
                        model.TotalVisit = MDVUtility.ToInt32(reader["TotalVisit"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::GetPatientVisitDOS", PROC_PATIENT_VISIT_DOS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<DocumentPrivacyModel> CheckForPrivacy(Int64 PatientId, Int64 PatDocId)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<DocumentPrivacyModel> dosmodel = new List<DocumentPrivacyModel>();
                DocumentPrivacyModel model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_DOC_ID, PatDocId));
                parameters.Add(new SqlParameter(PARM_User_ID, MDVSession.Current.AppUserId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_CHECK_FOR_PRIVACY, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentPrivacyModel();
                        model.ShowPasswordAlert = MDVUtility.ToStr(reader["ShowPasswordAlert"]);
                        model.Password = MDVUtility.ToStr(reader["Password"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::CheckForPrivacy", PROC_CHECK_FOR_PRIVACY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientDocumentModel> LoadPatientDocumentExpiryAlert(long UserId, long PatientId)
        {
            List<PatientDocumentModel> documentList = new List<PatientDocumentModel>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);

                if (UserId > 0)
                {
                    dbManager.AddParameters(0, PARM_User_ID, UserId);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_User_ID, null);
                }
                if (PatientId > 0)
                {
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                }
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_EXPIRY_ALERT_PROMPT);
                PatientDocumentModel model = null;
                while (reader.Read())
                {
                    model = new PatientDocumentModel();
                    model.PatDocId = !String.IsNullOrEmpty(reader["PatDocId"].ToString()) ? reader["PatDocId"].ToString() : "";
                    model.FilePath = !String.IsNullOrEmpty(reader["FilePath"].ToString()) ? reader["FilePath"].ToString() : "";
                    model.ExpiryDate = !String.IsNullOrEmpty(reader["ExpiryDate"].ToString()) ? reader["ExpiryDate"].ToString() : "";
                    documentList.Add(model);
                }

                return documentList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::FillPatientDocumentExpiryAlert", PROC_PATIENT_DOCUMENT_EXPIRY_ALERT_PROMPT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string InsertPatientDocumentExpiryAlert(long UserId, ref DataTable PatDocIds, string LookupName, DateTime CreatedOn)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnValue = "";
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_User_ID, UserId);
                dbManager.AddParameters(1, PARM_PAT_DOC_ID, PatDocIds);
                dbManager.AddParameters(2, PARM_LOOKUP_NAME, LookupName);
                dbManager.AddParameters(3, PARM_CREATED_ON, CreatedOn);
                returnValue = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USER_DOCUMENT_EXPIRY_ALERT_INSERT).ToString();
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocumentExpiryAlert", PROC_USER_DOCUMENT_EXPIRY_ALERT_INSERT, ex);
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

        public List<DocumentPrivacyModel> DocPasswordMatch(string Password, Int64 PatDocId)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<DocumentPrivacyModel> dosmodel = new List<DocumentPrivacyModel>();
                DocumentPrivacyModel model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_DOC_ID, PatDocId));
                parameters.Add(new SqlParameter(PARM_DOC_PASSWORD, Password));
                parameters.Add(new SqlParameter(PARM_User_ID, MDVSession.Current.AppUserId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_DOC_PASSWORD_MATCH, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentPrivacyModel();
                        //model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                        //model.Password = MDVUtility.ToStr(reader["Password"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DocPasswordMatch", PROC_DOC_PASSWORD_MATCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<DocumentPrivacyModel> DOCPasswordSave(string Password, Int64 PatDocId)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<DocumentPrivacyModel> dosmodel = new List<DocumentPrivacyModel>();
                DocumentPrivacyModel model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_DOC_ID, PatDocId));
                parameters.Add(new SqlParameter(PARM_DOC_PASSWORD, Password));
                parameters.Add(new SqlParameter(PARM_User_ID, MDVSession.Current.AppUserId));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_DOC_PASSWORD_SAVE, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentPrivacyModel();
                        //model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                        //model.Password = MDVUtility.ToStr(reader["Password"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DOCPasswordSave", PROC_DOC_PASSWORD_SAVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<DocumentPrivacyModel> ChangeOrRemovePassword(Int64 PatDocID, string OldPassword, string NewPassword)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string NewPass = NewPassword == "" ? null : NewPassword;
                List<DocumentPrivacyModel> dosmodel = new List<DocumentPrivacyModel>();
                DocumentPrivacyModel model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_DOC_ID, PatDocID));
                parameters.Add(new SqlParameter(PARM_User_ID, MDVSession.Current.AppUserId));
                parameters.Add(new SqlParameter(PARM_OLD_PASSWORD, OldPassword));
                parameters.Add(new SqlParameter(PARM_NEW_PASSWORD, NewPass));

                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_DOC_CHANGE_REMOVE_PASSWORD, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentPrivacyModel();
                        //model.VisitId = MDVUtility.ToStr(reader["VisitId"]);
                        //model.Password = MDVUtility.ToStr(reader["Password"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::ChangeOrRemovePassword", PROC_DOC_CHANGE_REMOVE_PASSWORD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Insert Pateint Document Tags
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string InsertPatientDocumentTags(PatientDocumentTag model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PAT_DOC_ID, model.PatDocId));
                parameters.Add(new SqlParameter(PARM_DOC_TAG_ID, model.TagId));
                parameters.Add(new SqlParameter(PARM_IS_ACTIVE, model.IsActive));
                parameters.Add(new SqlParameter(PARM_CREATED_BY, model.CreatedBy));
                parameters.Add(new SqlParameter(PARM_CREATED_ON, model.CreatedOn));
                parameters.Add(new SqlParameter(PARM_MODIFIED_BY, model.ModifiedBy));
                parameters.Add(new SqlParameter(PARM_MODIFIED_ON, model.ModifiedOn));
                int result = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_TAGS);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocumentTags", PROC_PATIENT_DOCUMENT_TAGS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Dashboard Document
        public DPatientDoucmnetModel LoadDashboardDocument(DateTime? DOSFrom, DateTime? DOSTo, int PageNumber = 1, int RowspPage = 15, string DocPriority = "", string DocAssignToReview = "", string PatientId = "", string Status = "", bool NextPendingDoc = false)
        {
            DPatientDoucmnetModel docModel = new DPatientDoucmnetModel();
            docModel.ListDDocumentModel = new List<DDocumentModel>();
            docModel.ListDPandingPatientDoc = new List<DPandingPatientDoc>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);

                if (PageNumber == 0)
                    dbManager.AddParameters(0, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(1, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(1, PARM_ROWS_PERPAGE, RowspPage);
                if (DocPriority == "" || DocPriority == null)
                    DocPriority = null;
                if (Status == "")
                    Status = null;

                dbManager.AddParameters(2, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(4, PARM_FROM_DOS, DOSFrom);
                dbManager.AddParameters(5, PARM_TO_DOS, DOSTo);

                dbManager.AddParameters(6, PARM_User_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(7, PARM_DOC_PRIORITY_ID, DocPriority);
                dbManager.AddParameters(8, PARM_DOC_ASSIGN_TO_REVIEW, DocAssignToReview);
                dbManager.AddParameters(9, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(10, PARM_STATUS, Status);
                dbManager.AddParameters(11, PARM_NEXT_PENDING_DOC, NextPendingDoc);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_DOCUMENT_SELECT);
                DDocumentModel model = null;
                while (reader.Read())
                {
                    model = new DDocumentModel();
                    model.PatDocId = Convert.ToString(reader["PatDocId"]);
                    model.DocumentName = Convert.ToString(reader["DocumentName"]);
                    model.DOS = Convert.ToString(reader["DOS"]);
                    model.Comments = Convert.ToString(reader["Comments"]);
                    model.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.Pending = Convert.ToString(reader["Pending"]);
                    model.Reviewed = Convert.ToString(reader["Reviewed"]);
                    model.PatientId = Convert.ToString(reader["PatientId"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.DocPriority = Convert.ToString(reader["DocPriority"]);
                    model.DocAssignToReview = Convert.ToString(reader["AssignedToName"]);
                    model.FacilityName = Convert.ToString(reader["FacilityName"]);
                    model.isReviewed = Convert.ToString(reader["isReviewed"]);
                    docModel.ListDDocumentModel.Add(model);
                }
                if (NextPendingDoc)
                {
                    reader.NextResult();
                    while (reader.Read())
                    {
                        DPandingPatientDoc PatDocModel = new DPandingPatientDoc();
                        PatDocModel.PatDocId = Convert.ToString(reader["PatDocId"]);
                        PatDocModel.PatientId = Convert.ToString(reader["PatientId"]);

                        docModel.ListDPandingPatientDoc.Add(PatDocModel);
                    }

                }

                return docModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadDashboardDocument", PROC_DASHBOARD_DOCUMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region Native
        public DSPatient InsertPatientDocumentNative(DSPatient ds, IDBManager dbManager)
        {
            try
            {
                CreateParameters(dbManager, ds, true,null);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_INSERT, ds, ds.PatientDocument.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertPatientDocumentNative", PROC_PATIENT_DOCUMENT_INSERT, ex);
                throw ex;
            }
        }
        #endregion

        #region " Note Document " 

        public void CreateParametersForNoteDocument(IDBManager dbManager, NoteDocumentModel model, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                int i = 0;
                dbManager.CreateParameters(8);

                dbManager.AddParameters(i++, PARM_NOTE_DOCUMENT_ID, model.NoteDocumentId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(i++, PARM_NOTE_ID, model.NoteId);
                dbManager.AddParameters(i++, PARM_PATIENT_DOC_ID, model.PatDocId);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(i++, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(i++, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, model.ModifiedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_NOTE_DOCUMENT_ID, model.NoteDocumentId);
                dbManager.AddParameters(PARM_NOTE_ID, model.NoteId);
                dbManager.AddParameters(PARM_PATIENT_DOC_ID, model.PatDocId);
                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(PARM_MODIFIED_ON, model.ModifiedOn);
            }



        }

        public List<NoteDocumentModel> LoadNoteDocument(long NoteDocumentId, int PageNumber, int RowspPage)
        {
            List<NoteDocumentModel> listDDocuments = new List<NoteDocumentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (NoteDocumentId == 0)
                    dbManager.AddParameters(0, PARM_NOTE_DOCUMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_DOCUMENT_ID, NoteDocumentId);

                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_DOCUMENT_SELECT);
                NoteDocumentModel model = null;
                while (reader.Read())
                {
                    model = new NoteDocumentModel();
                    model.PatDocId = Convert.ToString(reader["PatDocId"]);
                    model.DocumentName = Convert.ToString(reader["DocumentName"]);
                    model.Comments = Convert.ToString(reader["Comments"]);
                    model.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    listDDocuments.Add(model);
                }

                return listDDocuments;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadNoteDocument", PROC_NOTE_DOCUMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public NoteDocumentModel InsertNoteDocument(NoteDocumentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                CreateParametersForNoteDocument(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_DOCUMENT_INSERT);
                while (reader.Read())
                {
                    model.NoteDocumentId = Convert.ToString(reader["NoteDocumentId"]);
                }
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::InsertNoteDocument", PROC_NOTE_DOCUMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public string UpdateNoteDocument(NoteDocumentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForNoteDocument(dbManager, model, false);
                string returnVal = Convert.ToString(dbManager.ExecuteScalar(PROC_NOTE_DOCUMENT_UPDATE));
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::UpdateNoteDocument", PROC_NOTE_DOCUMENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteNoteDocument(long NoteDocumentId, long NoteId, long PatDocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (NoteDocumentId > 0)
                    dbManager.AddParameters(0, PARM_NOTE_DOCUMENT_ID, NoteDocumentId);
                else
                    dbManager.AddParameters(0, PARM_NOTE_DOCUMENT_ID, null);

                if (NoteId > 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);

                if (PatDocId > 0)
                    dbManager.AddParameters(2, PARM_PATIENT_DOC_ID, PatDocId);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_DOC_ID, null);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTE_DOCUMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DeleteNoteDocument", PROC_NOTE_DOCUMENT_DELETE, ex);
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

        public string DetachDocumentsFromNote(string PatientDocumentIds, long NoteId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(PatientDocumentIds))
                {
                    dbManager.AddParameters(0, PARM_PATIENT_DOCUMENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_DOCUMENT_ID, PatientDocumentIds);
                }

                if (NoteId > 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);

                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTE_DOCUMENT_DETACH));

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::DetachDocumentsFromNote", PROC_NOTE_DOCUMENT_DETACH, ex);
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

    }

}
