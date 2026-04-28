using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using MDVision.Model.Dashboard;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
//using static System.String;

namespace MDVision.DataAccess.DAL.Message
{
    public class DALMessage
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALMessage()
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

        #region "Stored Procedure Names"
        private const string PROC_MESSAGETYPES_LOOKUP = "Patient.sp_MessageTypesLookup";
        private const string PROC_MESSAGESTATUS_LOOKUP = "Patient.sp_MessageStatusLookup";
        private const string PROC_MSGALERTTYPES_LOOKUP = "Patient.sp_MsgAlertTypesLookup";
        private const string PROC_MSGPRIORITY_LOOKUP = "Patient.sp_MsgPriorityLookup";
        private const string PROC_AMENDMENTSOURCE_LOOKUP = "Patient.sp_AmendmentSourceLookup";
        private const string PROC_PAT_MESSAGES_INSERT = "Patient.sp_PatMessagesInsert";
        private const string PROC_PAT_MESSAGES_DELETE = "Patient.sp_PatMessagesDelete";
        private const string PROC_PAT_MESSAGES_SELECT = "Patient.sp_PatMessagesSelect";
        private const string PROC_PAT_MESSAGES_SELECT_COUNT = "Patient.sp_PatMessagesSelectCount";
        private const string PROC_PAT_MESSAGES_SEARCH = "Patient.sp_PatMessagesSearch_New";
        private const string PROC_PAT_MESSAGES_UPDATE = "Patient.sp_PatMessagesUpdate";
        private const string PROC_MESSAE_REPLY_INSERT = "Patient.sp_MsgReplyInsert";
        private const string PROC_MESSAE_REPLY_UPDATE = "Patient.sp_MsgReplyUpdate";
        private const string PROC_MESSAE_REPLY_DELETE = "Patient.sp_MsgReplyDelete";
        private const string PROC_MESSAE_REPLY_SELECT = "Patient.sp_MsgReplySelect";
        private const string PROC_MESSAGE_PRIORITY_LOOKUP = "Patient.sp_MessagePriorityLookup";
        private const string PROC_MESSAGE_COUNT = "Clinical.sp_UsersMessagesCount";

        private const string PROC_USER_MESSAGES_INSERT = "Patient.sp_UserMessagesInsert";
        private const string PROC_DIRECT_MESSAGES_INSERT = "Clinical.sp_DirectMessagesInsert";

        private const string PROC_USER_MESSAGES_DELETE = "Patient.sp_UserMessagesDelete";
        private const string PROC_USER_MESSAGES_SELECT = "Patient.sp_UserMessagesSelect";
        private const string PROC_LOG_MESSAGES_SELECT = "Patient.sp_LogMessagesSelect";

        private const string PROC_DIRECT_MESSAGES_SELECT = "Clinical.sp_DirectMessagesSelect";
        private const string PROC_OUTGOING_DIRECT_MESSAGES_SELECT = "Patient.sp_OutgoingDirectMessagesSelect";
        private const string PROC_OUTGOING_DIRECT_MESSAGES_STATUS_UPDATE = "Patient.sp_OutgoingDirectMessagesStatusUpdate";

        private const string PROC_USER_MESSAGES_UPDATE = "Patient.sp_UserMessagesUpdate";

        private const string PROC_USER_TASK_SELECT = "Patient.sp_UserTaskSelect";

        private const string PROC_MESSAGE_DOCUMENTSSTREAM_INSERT = "Patient.sp_DocumentsStreamInsert";
        private const string PROC_MESSAGE_DOCUMENTSSTREAM_SELECT = "Patient.sp_DocumentsStreamSelect";
        private const string PROC_DASHBOARD_MESSAGES_SELECT = "Patient.sp_D_UserMessageSelect";
        private const string PROC_DASHBOARD_TASKS_SELECT = "Patient.sp_D_MessagesSelect";

        private const string PROC_PRAC_MESSAGES_SELECT = "Clinical.sp_PracticeMessages";
        private const string PROC_PRAC_MESSAGES_FILL = "Clinical.sp_PracticeMessagesFill";
        private const string PROC_PRAC_PATIENT_CHAT = "Clinical.sp_patientchatrecord";
        private const string PROC_PRAC_MESSAGES_FILL_Test = "Clinical.sp_PracticeMessagesFill_test";
        private const string PROC_PRAC_MESSAGES_INSERT = "Clinical.sp_PracticeMessagesInsert";
        private const string PROC_OUTGOING_DIRECT_MESSAGES_INSERT = "Patient.sp_OutgoingDirectMessagesInsert";

        private const string PROC_PAT_MESSAGES_SECRETKEY_INSERT = "Clinical.sp_InsertSecretKey";
        private const string PROC_PAT_MESSAGES_SECRETKEY_UPDATE = "Clinical.sp_UpdateSecretKey";
        private const string PROC_PAT_MESSAGES_SECRETKEY_SELECT = "Clinical.sp_SelectSecretKey";
        #endregion

        #region Parameters
        private const string PARM_PAT_MSG_ID = "@PatMsgId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_USER_MESSAGE_ID = "@UserMessageId";
        private const string PARM_MESSAGE_DETAIL = "@MsgDetail";
        private const string PARM_MSG_TYPE_ID = "@MsgTypeId";
        private const string PARM_CALL_DATE = "@CallDate";
        private const string PARM_DOS = "@DOS";
        private const string PARM_MSG_STATUS_ID = "@MsgStatusId";
        private const string PARM_ALERT_TYPE_ID = "@AlertTypeId";
        private const string PARM_ASSIGNED_TO_ID = "@AssignedToId";
        private const string PARM_DATE_TO = "@DateTo";
        private const string PARM_DATE_FROM = "@Datefrom";
        private const string PARM_PRIORITY_ID = "@PriorityId";
        private const string PARM_ENTRY_DATE = "@EntryDate";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_MEDICATION_ID = "@MedicationId";
        private const string PARM_PHARMACY_ID = "@PharmacyId";
        private const string PARM_LAB_ORDER_ID = "@LabOrderId";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_AMDT_SOURCE_ID = "@AmdtSourceId";
        private const string PARM_VIS_TO_PATIENT = "@VisToPatient";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_MSG_REPLY_ID = "@MsgrId";
        private const string PARM_SPK_PATINET = "@SpkPatinet";
        private const string PARM_MSG_TYPE_SHORT_NAME = "@MsgTypeShortName";
        private const string PARM_MSG_TYPE_NOT = "@MsgTypeNot";
        private const string PARM_IDs = "@IDs";
        private const string PARM_ISDELIVERED = "@isdelivered"; 
        private const string PARM_DIRECT_ADD = "@DirectAddress";
        private const string PARM_MSG_TYP = "@MsgType";
        private const string PARM_UNIQUE_NO = "@UniqueMsgNo";
        private const string PARM_PATIENT_LETTER_ID = "@PatientLetterId";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        //private const string PARM_PATIENT_IMAGE = "@PatientImage";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_USER_ID_MESSAGE = "@UserId";

        private const string PARM_ASSIGNEDCHAT_FROM = "@AssignedFrom";
        private const string PARM_ASSIGNEDCHAT_TO = "@AssignedTo";

        private const string PARM_USER_MESSAGES_ID = "@UserMessagesId";
        private const string PARM_IS_MUALERT_UPDATED = "@IsMUAlertUpdated";
        private const string PARM_DIRECT_MESSAGES_ID = "@UserMessagesId";
        private const string PARM_ASSIGNEDFROM_ID = "@AssignedFromId";
        private const string PARM_ASSIGNEDFROM_EMAIL = "@AssignedFromEmail";

        private const string PARM_ASSIGNEDTO_ID = "@AssignedToId";
        private const string PARM_ATTATCHEDPATIENT_ID = "@AttatchedPatientId";
        private const string PARM_SUBJECT = "@Subject";
        private const string PARM_USERMESSAGE_DETAIL = "@MessageDetail";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARM_IS_READ = "@IsRead";

        private const string PARM_MESSAGE_DATE = "@MessageDate";
        private const string PARM_NAME = "@Name";
        private const string PARM_FILE_TYPE = "@FileType";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_FILE_STREAM = "@FileStream";

        private const string PARM_DOCUMENTS_STR_Id = "@DocumentsStrId";
        private const string PARM_PAGES = "@Pages";

        private const string PARM_MSG_SUBJECT = "@Subject";

        private const string PARM_MSG_TYPE = "@MsgType";
        private const string PARM_USER_NAME_FROM = "@usernamewithpracticefrom";

        private const string PARM_MESSAGES_ID = "@MessageId";
        private const string PARM_MESSAGE_HASH = "@MessageHash";
        private const string PARM_ENCRYPTION_KEY = "@Encryptionkey";
        private const string PARM_ENCRYPTION_IV = "@EncryptionIV";

        private const string PARM_SECRET_KEY = "@SecretKey";
        private const string PARM_SECRET_KEY_TIME = "@SecretKeyTime";


        private const string PARM_EMAIL_FROM = "@EmailFrom";
        private const string PARM_EMAIL_TO = "@EmailTo";
        private const string PARM_EMAIL_SUBJECT = "@Subj";
        private const string PARM_DIRECT_MSG_ID = "@DirectMsgId";
        private const string PARM_MESSAGE_STATUS = "@MessageStatus";
        private const string PARM_DIRECT_MESSAGE_DETAIL = "@MessageDetail";
        private const string PARM_DIRECT_MESSAGE_TIME = "@DateTime";

        private const string PARM_DIRECT_MSG_DOS = "@DOS";
        private const string PARM_DIRECT_MSG_DOCTYPE = "@DocType";
        private const string PARM_DIRECT_MSG_ISXML = "@isHtml";
        private const string PARM_DIRECT_MSG_ISHTML = "@isXml";
        private const string PARM_DIRECT_MSG_PAT_ACCNT_NUMBER = "@AccountNumber";


        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSMessage ds, Boolean IsInsert, string Type = "Message")
        {
            if (Type == "Message")
            {
                dbManager.CreateParameters(28);

                if (IsInsert == true)
                    dbManager.AddParameters(0, PARM_PAT_MSG_ID, ds.PatMessages.PatMsgIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                else
                    dbManager.AddParameters(0, PARM_PAT_MSG_ID, ds.PatMessages.PatMsgIdColumn.ColumnName, DbType.Int64);

                if (MDVUtility.ToInt64(ds.Tables[ds.PatMessages.TableName].Rows[0][ds.PatMessages.PatientIdColumn.ColumnName]) > 0)
                {

                    dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatMessages.PatientIdColumn.ColumnName, DbType.Int64);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null, DbType.Int64);

                }

                dbManager.AddParameters(2, PARM_MESSAGE_DETAIL, ds.PatMessages.MsgDetailColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_MSG_TYPE_ID, ds.PatMessages.MsgTypeIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(4, PARM_CALL_DATE, ds.PatMessages.CallDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, PARM_DOS, ds.PatMessages.DOSColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(6, PARM_MSG_STATUS_ID, ds.PatMessages.MsgStatusIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(7, PARM_ALERT_TYPE_ID, ds.PatMessages.AlertTypeIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(8, PARM_ASSIGNED_TO_ID, ds.PatMessages.AssignedToIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(9, PARM_PRIORITY_ID, ds.PatMessages.PriorityIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(10, PARM_ENTRY_DATE, ds.PatMessages.EntryDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(11, PARM_USER_NAME, ds.PatMessages.UserNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(12, PARM_MEDICATION_ID, ds.PatMessages.MedicationIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(13, PARM_PHARMACY_ID, ds.PatMessages.PharmacyIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(14, PARM_LAB_ORDER_ID, ds.PatMessages.LabOrderIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(15, PARM_LAB_ID, ds.PatMessages.LabIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(16, PARM_AMDT_SOURCE_ID, ds.PatMessages.AmdtSourceIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(17, PARM_VIS_TO_PATIENT, ds.PatMessages.VisToPatientColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(18, PARM_IS_ACTIVE, ds.PatMessages.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(19, PARM_CREATED_BY, ds.PatMessages.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(20, PARM_CREATED_ON, ds.PatMessages.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(21, PARM_MODIFIED_BY, ds.PatMessages.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(22, PARM_MODIFIED_ON, ds.PatMessages.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(23, PARM_ENTITY_ID, ds.PatMessages.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(24, PARM_USER_ID_MESSAGE, ds.PatMessages.UserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(25, PARM_USER_MESSAGES_ID, ds.PatMessages.UserMessagesIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(26, PARM_MSG_SUBJECT, ds.PatMessages.SubjectColumn.ColumnName, DbType.String);
                dbManager.AddParameters(27, PARM_IS_READ, ds.PatMessages.IsReadColumn.ColumnName, DbType.Byte);
            }
            else if (Type == "MessageReply")
            {
                dbManager.CreateParameters(12);
                if (IsInsert == true)
                    dbManager.AddParameters(0, PARM_MSG_REPLY_ID, ds.MsgReply.MsgrIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                else
                    dbManager.AddParameters(0, PARM_MSG_REPLY_ID, ds.MsgReply.MsgrIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_PAT_MSG_ID, ds.MsgReply.PatMsgIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_MESSAGE_DETAIL, ds.MsgReply.MsgDetailColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_ASSIGNED_TO_ID, ds.MsgReply.AssignedToIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_MSG_STATUS_ID, ds.MsgReply.MsgStatusIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(5, PARM_SPK_PATINET, ds.MsgReply.SpkPatinetColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.MsgReply.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(7, PARM_CREATED_BY, ds.MsgReply.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_CREATED_ON, ds.MsgReply.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.MsgReply.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.MsgReply.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(11, PARM_USER_NAME, ds.MsgReply.UserNameColumn.ColumnName, DbType.String);
                //dbManager.AddParameters(12, PARM_USER_ID_MESSAGE, ds.PatMessages.UserIdColumn.ColumnName, DbType.Int64);
            }


        }
        private void CreateParametersUserMsgs(IDBManager dbManager, DSMessage ds, Boolean IsInsert, Boolean isphiMail)
        {
            //dbManager.CreateParameters(ds.Tables[ds.AppointmentStatus.TableName].Columns.Count);
            dbManager.CreateParameters(18);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, ds.UserMessages.UserMessagesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, ds.UserMessages.UserMessagesIdColumn.ColumnName, DbType.Int64);

            if (isphiMail == true)
            {
                dbManager.AddParameters(1, PARM_ASSIGNEDFROM_EMAIL, ds.UserMessages.AssignedFromEmailColumn.ColumnName, DbType.String);

            }
            else
            {
                dbManager.AddParameters(1, PARM_ASSIGNEDFROM_ID, ds.UserMessages.AssignedFromIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(2, PARM_ASSIGNEDTO_ID, ds.UserMessages.AssignedToIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ATTATCHEDPATIENT_ID, ds.UserMessages.AttatchedPatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_SUBJECT, ds.UserMessages.SubjectColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_USERMESSAGE_DETAIL, ds.UserMessages.MessageDetailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_USER_ID, ds.UserMessages.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_PRIORITY_ID, ds.UserMessages.PriorityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_IS_DELETED, ds.UserMessages.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_IS_READ, ds.UserMessages.IsReadColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.UserMessages.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.UserMessages.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.UserMessages.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.UserMessages.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.UserMessages.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(15, PARM_FILE_TYPE, ds.UserMessages.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_FILE_PATH, ds.UserMessages.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_FILE_STREAM, ds.UserMessages.FileStreamColumn.ColumnName, DbType.Binary);

        }

        private void CreateParametersPracticeMsgs(IDBManager dbManager, DSMessage ds, Boolean IsInsert)
        {
            //dbManager.CreateParameters(ds.Tables[ds.AppointmentStatus.TableName].Columns.Count);


            if (IsInsert == true)
            {
                dbManager.CreateParameters(28);
                dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, ds.UserMessages.UserMessagesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(27);
                dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, ds.UserMessages.UserMessagesIdColumn.ColumnName, DbType.Int64);
            }


            dbManager.AddParameters(1, PARM_ASSIGNEDFROM_ID, ds.UserMessages.AssignedFromIdColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(2, PARM_ASSIGNEDTO_ID, ds.UserMessages.AssignedToIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ATTATCHEDPATIENT_ID, ds.UserMessages.AttatchedPatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_SUBJECT, ds.UserMessages.SubjectColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_USERMESSAGE_DETAIL, ds.UserMessages.MessageDetailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_USER_ID, ds.UserMessages.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_PRIORITY_ID, ds.UserMessages.PriorityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_IS_DELETED, ds.UserMessages.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_IS_READ, ds.UserMessages.IsReadColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.UserMessages.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.UserMessages.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.UserMessages.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.UserMessages.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.UserMessages.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(15, PARM_FILE_TYPE, ds.UserMessages.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_FILE_PATH, ds.UserMessages.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_FILE_STREAM, ds.UserMessages.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(18, PARM_MSG_TYPE, ds.UserMessages.MessagerTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ENTITY_ID, ds.UserMessages.EntityidColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(20, PARM_UNIQUE_NO, ds.UserMessages.UniqueNumberColumn.ColumnName, DbType.String);
            if (MDVUtility.ToInt64(ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.PatientLetterIdColumn.ColumnName]) > 0)
            {
                dbManager.AddParameters(21, PARM_PATIENT_LETTER_ID, ds.UserMessages.PatientLetterIdColumn.ColumnName, DbType.Int64);
            }
            else
            {
                dbManager.AddParameters(21, PARM_PATIENT_LETTER_ID, null, DbType.Int64);

            }
            dbManager.AddParameters(22, PARM_USER_NAME, ds.UserMessages.UserNameWithPracticeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_USER_NAME_FROM, ds.UserMessages.usernamewithpracticefromColumn.ColumnName, DbType.String);

            dbManager.AddParameters(24, PARM_MESSAGE_HASH, ds.UserMessages.MessageHashColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(25, PARM_ENCRYPTION_KEY, ds.UserMessages.EncryptionKeyColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(26, PARM_ENCRYPTION_IV, ds.UserMessages.EncryptionIVColumn.ColumnName, DbType.Binary);

            if (IsInsert == true)
                dbManager.AddParameters(27, PARM_IS_MUALERT_UPDATED, ds.UserMessages.IsMUAlertUpdatedColumn.ColumnName, DbType.Byte, ParamDirection.Output);

        }
        private void CreateParametersDocumentStream(IDBManager dbManager, DSMessage ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_DOCUMENTS_STR_Id, ds.DocumentsStream.DocumentsStrIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DOCUMENTS_STR_Id, ds.DocumentsStream.DocumentsStrIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(1, PARM_USER_MESSAGES_ID, ds.DocumentsStream.UserMessagesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FILE_TYPE, ds.DocumentsStream.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_FILE_PATH, ds.DocumentsStream.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_FILE_STREAM, ds.DocumentsStream.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.DocumentsStream.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.DocumentsStream.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.DocumentsStream.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.DocumentsStream.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.DocumentsStream.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateParametersDirectMsgs(IDBManager dbManager, DSMessage ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            //if (IsInsert == true)
            //    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, ds.DirectMessages.IDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            //else
            //    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, ds.DirectMessages.IDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(0, PARM_MESSAGE_STATUS, ds.DirectMessages.MessageStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(1, PARM_EMAIL_SUBJECT, ds.DirectMessages.SubjColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DIRECT_MESSAGE_DETAIL, ds.DirectMessages.MessageDetailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_EMAIL_FROM, ds.DirectMessages.EmailFromColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_EMAIL_TO, ds.DirectMessages.EmailToColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DIRECT_MSG_ID, ds.DirectMessages.DirectMsgIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_DIRECT_MESSAGE_TIME, ds.DirectMessages.DateTimeColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_DIRECT_MSG_PAT_ACCNT_NUMBER, ds.DirectMessages.AccountNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_DIRECT_MSG_DOS, ds.DirectMessages.DOSColumn .ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_DIRECT_MSG_DOCTYPE, ds.DirectMessages.DocTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_DIRECT_MSG_ISXML, ds.DirectMessages.isXmlColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(11, PARM_DIRECT_MSG_ISHTML, ds.DirectMessages.isHtmlColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_PATIENT_ID, ds.DirectMessages.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_NOTE_ID, ds.DirectMessages.NoteIdColumn.ColumnName, DbType.Int64);


        }


    #endregion

    #region "Insert, delete, update and get Message using dataset Functions"
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
    public DSMessage LoadMessage(long PatientId, long PatMsgId, string MsgTypeId, int MsgStatusId, DateTime? CallDate, DateTime? EntryDate, long AssignedToId, string MsgTypeShortName = "", Int32 PageNumber = 1, Int32 RowsPerPage = 15, string MsgTypeNot = "", long UserMessagesId = 0)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(15);
                if (PatMsgId == 0)
                    dbManager.AddParameters(0, PARM_PAT_MSG_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAT_MSG_ID, PatMsgId);
                if (MsgTypeId == "")
                    dbManager.AddParameters(1, PARM_MSG_TYPE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MSG_TYPE_ID, MsgTypeId);
                if (CallDate == null)
                    dbManager.AddParameters(2, PARM_CALL_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_CALL_DATE, CallDate);
                if (EntryDate == null)
                    dbManager.AddParameters(3, PARM_ENTRY_DATE, null);
                else
                    dbManager.AddParameters(3, PARM_ENTRY_DATE, EntryDate);
                if (PatientId == 0)
                    dbManager.AddParameters(4, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PATIENT_ID, PatientId);
                if (MsgStatusId == 0)
                    dbManager.AddParameters(5, PARM_MSG_STATUS_ID, null);
                else
                    dbManager.AddParameters(5, PARM_MSG_STATUS_ID, MsgStatusId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                //if (SharedObj.IsAdmin)
                //    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                //else
                //    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (AssignedToId == 0)
                    dbManager.AddParameters(7, PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ASSIGNED_TO_ID, AssignedToId);
                if (MsgTypeShortName == "")
                    dbManager.AddParameters(8, PARM_MSG_TYPE_SHORT_NAME, null);
                else
                    dbManager.AddParameters(8, PARM_MSG_TYPE_SHORT_NAME, MsgTypeShortName);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.PatMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (MsgTypeNot == "")
                    dbManager.AddParameters(13, PARM_MSG_TYPE_NOT, null);
                else
                    dbManager.AddParameters(13, PARM_MSG_TYPE_NOT, MsgTypeNot);

                if (UserMessagesId == 0)
                    dbManager.AddParameters(14, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(14, PARM_USER_MESSAGES_ID, UserMessagesId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SELECT, ds, ds.PatMessages.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadMessage", PROC_PAT_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<Model.User.UserMessagesCount> LoadMessage_(long patientId, long patMsgId, string msgTypeId, int msgStatusId, DateTime? callDate, DateTime? entryDate, long assignedToId, string msgTypeShortName = "", int pageNumber = 1, int rowsPerPage = 15, string msgTypeNot = "", long userMessagesId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (patMsgId == 0)
                    dbManager.AddParameters(PARM_PAT_MSG_ID, null);
                else
                    dbManager.AddParameters(PARM_PAT_MSG_ID, patMsgId);

                dbManager.AddParameters(PARM_MSG_TYPE_ID, msgTypeId == "" ? null : msgTypeId);
                dbManager.AddParameters(PARM_CALL_DATE, callDate);
                dbManager.AddParameters(PARM_ENTRY_DATE, entryDate);
                if (patientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, patientId);
                if (msgStatusId == 0)
                    dbManager.AddParameters(PARM_MSG_STATUS_ID, null);
                else
                    dbManager.AddParameters(PARM_MSG_STATUS_ID, msgStatusId);

                dbManager.AddParameters(PARM_ENTITY_ID, objValue: string.Equals(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DefaultUser, StringComparison.CurrentCultureIgnoreCase)
                        ? null
                        : MDVSession.Current.EntityId);

                if (assignedToId == 0)
                    dbManager.AddParameters(PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(PARM_ASSIGNED_TO_ID, assignedToId);

                dbManager.AddParameters(PARM_MSG_TYPE_SHORT_NAME, msgTypeShortName == "" ? null : msgTypeShortName);

                if (pageNumber == 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage == 0)
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, rowsPerPage);

                //dbManager.AddParameters/*(PARM_RECORD_COUNT, ds.PatMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);*/
                dbManager.AddParameters(PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(PARM_MSG_TYPE_NOT, msgTypeNot == "" ? null : msgTypeNot);

                if (userMessagesId == 0)
                    dbManager.AddParameters(PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(PARM_USER_MESSAGES_ID, userMessagesId);

                return dbManager.ExecuteReaders<Model.User.UserMessagesCount>(PROC_PAT_MESSAGES_SELECT_COUNT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadMessage", PROC_PAT_MESSAGES_SELECT_COUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage searchMessage(long PatientId, string MsgTypeId, int MsgStatusId, DateTime? CallDate, DateTime? EntryDate, string IsActive, Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);

                if (MsgTypeId == "")
                    dbManager.AddParameters(0, PARM_MSG_TYPE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MSG_TYPE_ID, MsgTypeId);
                if (CallDate == null)
                    dbManager.AddParameters(1, PARM_CALL_DATE, null);
                else
                    dbManager.AddParameters(1, PARM_CALL_DATE, CallDate);
                if (EntryDate == null)
                    dbManager.AddParameters(2, PARM_ENTRY_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_ENTRY_DATE, EntryDate);
                if (PatientId == 0)
                    dbManager.AddParameters(3, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (MsgStatusId == 0)
                    dbManager.AddParameters(5, PARM_MSG_STATUS_ID, null);
                else
                    dbManager.AddParameters(5, PARM_MSG_STATUS_ID, MsgStatusId);
                if (IsActive == "")
                    dbManager.AddParameters(6, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(8, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.PatMessagesSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SEARCH, ds, ds.PatMessagesSearch.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::searchMessage", PROC_PAT_MESSAGES_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage FillMessage(long PatMsgId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(4);

                if (PatMsgId == 0)
                    dbManager.AddParameters(0, PARM_PAT_MSG_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAT_MSG_ID, PatMsgId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SELECT, ds, ds.PatMessages.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::FillMessage", PROC_PAT_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the Message.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSMessage UpdateMessage(DSMessage ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    this.CreateParameters(dbManager, ds, false);
                    ds = (DSMessage)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_UPDATE, ds, ds.PatMessages.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALMessage::UpdateMessage", PROC_PAT_MESSAGES_UPDATE, ex);
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
                    // dbManagerPrevTrans.Open();
                    this.CreateParameters(dbManager, ds, false);
                    ds = (DSMessage)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_UPDATE, ds, ds.PatMessages.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALMessage::UpdateMessage", PROC_PAT_MESSAGES_UPDATE, ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Deletes the Message.
        /// </summary>
        /// <param name="PatMsgId">The Message identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteMessage(string PatMsgId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PAT_MSG_ID, PatMsgId);
                // dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PAT_MESSAGES_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::DeleteMessage", PROC_PAT_MESSAGES_DELETE, ex);
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
        /// Inserts the Message.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSMessage InsertMessage(DSMessage ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {

                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    CreateParameters(dbManager, ds, true);
                    ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_INSERT, ds, ds.PatMessages.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALMessage::InsertMessage", PROC_PAT_MESSAGES_INSERT, ex);
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
                    CreateParameters(dbManager, ds, true);
                    ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_INSERT, ds, ds.PatMessages.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALMessage::InsertMessage", PROC_PAT_MESSAGES_INSERT, ex);
                    throw ex;
                }

            }
        }
        #endregion

        #region Message Reply

        #region "Insert, delete, update and get Message Reply using dataset Functions"
        /// <summary>
        /// Loads the patient Message reply.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        public DSMessage LoadMessageReply(long PatMsgId, long MsgrId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (MsgrId == 0)
                    dbManager.AddParameters(0, PARM_MSG_REPLY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MSG_REPLY_ID, MsgrId);
                if (PatMsgId == 0)
                    dbManager.AddParameters(1, PARM_PAT_MSG_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PAT_MSG_ID, PatMsgId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAE_REPLY_SELECT, ds, ds.MsgReply.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadMessageReply", PROC_MESSAE_REPLY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage FillMessageReply(long PatMsgId, long MsgrId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);

                if (MsgrId == 0)
                    dbManager.AddParameters(0, PARM_MSG_REPLY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MSG_REPLY_ID, MsgrId);
                if (PatMsgId == 0)
                    dbManager.AddParameters(1, PARM_PAT_MSG_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PAT_MSG_ID, PatMsgId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAE_REPLY_SELECT, ds, ds.MsgReply.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::FillMessageReply", PROC_MESSAE_REPLY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the MessageReply.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSMessage UpdateMessageReply(DSMessage ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSMessage)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MESSAE_REPLY_UPDATE, ds, ds.MsgReply.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::UpdateMessageReply", PROC_MESSAE_REPLY_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the MessageReply.
        /// </summary>
        /// <param name="MsgrId">The MessageReply identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteMessageReply(string MsgrId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MSG_REPLY_ID, MsgrId);
                // dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MESSAE_REPLY_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::DeleteMessageReply", PROC_MESSAE_REPLY_DELETE, ex);
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
        /// Inserts the MessageReply.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSMessage InsertMessageReply(DSMessage ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    CreateParameters(dbManager, ds, true, "MessageReply");
                    ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MESSAE_REPLY_INSERT, ds, ds.MsgReply.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALMessage::InsertMessageReply", PROC_MESSAE_REPLY_INSERT, ex);
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
                    // dbManagerPrevTrans.Open();
                    CreateParameters(dbManager, ds, true, "MessageReply");
                    ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MESSAE_REPLY_INSERT, ds, ds.MsgReply.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALMessage::InsertMessageReply", PROC_MESSAE_REPLY_INSERT, ex);
                    throw ex;
                }
            }
        }
        #endregion

        #endregion

        #region "Lookups"
        //<summary>
        // the LookupMessageTypes.
        //</summary>
        //<returns>DSMessageLookup.</returns>
        public DSMessageLookup LookupMessageTypes()
        {
            DSMessageLookup ds = new DSMessageLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMessageLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAGETYPES_LOOKUP, ds, ds.MessageTypes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LookupMessageTypes", PROC_MESSAGETYPES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //<summary>
        // the LookupMessageStatus.
        //</summary>
        //<returns>DSMessageLookup.</returns>
        public DSMessageLookup LookupMessageStatus()
        {
            DSMessageLookup ds = new DSMessageLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMessageLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAGESTATUS_LOOKUP, ds, ds.MessageStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LookupMessageStatus", PROC_MESSAGESTATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //<summary>
        //Lookups the AlertTypes.
        //</summary>
        //<returns>DSMessageLookup.</returns>

        public DSMessageLookup LookupMsgAlertTypes()
        {
            DSMessageLookup ds = new DSMessageLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMessageLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MSGALERTTYPES_LOOKUP, ds, ds.MsgAlertTypes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LookupMsgAlertTypes", PROC_MSGALERTTYPES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //<summary>
        //Lookups the Priority.
        //</summary>
        //<returns>DSMessageLookup.</returns>

        public DSMessageLookup LookupMsgPriority()
        {
            DSMessageLookup ds = new DSMessageLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMessageLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MSGPRIORITY_LOOKUP, ds, ds.MsgPriority.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LookupMsgPriority", PROC_MSGPRIORITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //<summary>
        //Lookups the AmendmentSource.
        //</summary>
        //<returns>DSMessageLookup.</returns>

        public DSMessageLookup LookupAmendmentSource()
        {
            DSMessageLookup ds = new DSMessageLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMessageLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AMENDMENTSOURCE_LOOKUP, ds, ds.AmendmentSource.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LookupAmendmentSource", PROC_AMENDMENTSOURCE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessageLookup LookupMessagesPriority()
        {
            DSMessageLookup ds = new DSMessageLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMessageLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAGE_PRIORITY_LOOKUP, ds, ds.MessagesPriority.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LookupMessagesPriority", PROC_MESSAGE_PRIORITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region"User Messages"

        public DSMessage UpdateUserMessage(DSMessage ds, Boolean isphiMail = false)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersUserMsgs(dbManager, ds, false, isphiMail);
                ds = (DSMessage)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_USER_MESSAGES_UPDATE, ds, ds.UserMessages.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::UpdateUserMessage", PROC_USER_MESSAGES_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteUserMessage(string UserMessagesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, UserMessagesId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal =MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_USER_MESSAGES_DELETE));

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::DeleteUserMessage", PROC_USER_MESSAGES_DELETE, ex);
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

        public DSMessage InsertUserMessage(DSMessage ds, Boolean isphiMail = false)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersUserMsgs(dbManager, ds, true, isphiMail);
                ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_MESSAGES_INSERT, ds, ds.UserMessages.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertUserMessage", PROC_USER_MESSAGES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage InsertDirectMessage(DSMessage ds, Boolean isphiMail)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersUserMsgs(dbManager, ds, true, isphiMail);
                ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DIRECT_MESSAGES_INSERT, ds, ds.UserMessages.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertUserMessage", PROC_DIRECT_MESSAGES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSMessage InsertPracticeMessage(DSMessage ds, Boolean isphiMail = false)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersPracticeMsgs(dbManager, ds, true);
                ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PRAC_MESSAGES_INSERT, ds, ds.UserMessages.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertPracticeMessage", PROC_PRAC_MESSAGES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMessage FillPracticeMessage(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, string Messagetype, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Name == "")
                    Name = null;
                if (MessageDate == "")
                    MessageDate = null;

                dbManager.Open();
                dbManager.CreateParameters(11);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, UserMessagesId);

                dbManager.AddParameters(1, PARM_USER_ID, UserId);

                if (MessageDate == null)
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, MessageDate);

                if (PriorityId == 0)
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, PriorityId);

                dbManager.AddParameters(4, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.UserMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (AttatchedPatientId == 0)
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, AttatchedPatientId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (Messagetype == "")
                    dbManager.AddParameters(10, PARM_MSG_TYPE, null);
                else
                    dbManager.AddParameters(10, PARM_MSG_TYPE, Messagetype);


                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRAC_MESSAGES_FILL, ds, ds.UserMessages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::FillPracticeMessage", PROC_PRAC_MESSAGES_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage FillChatThread(long UserMessagesId, long UserId, string Messagetype)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(8);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(0, PARM_USER_MESSAGE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_MESSAGE_ID, UserMessagesId);

                //dbManager.AddParameters(1, PARM_USER_ID, UserId);


                dbManager.AddParameters(1, PARM_DATE_TO, null);



                dbManager.AddParameters(2, PARM_DATE_FROM, null);

                dbManager.AddParameters(3, PARM_SUBJECT, null);


                dbManager.AddParameters(4, PARM_ASSIGNEDCHAT_TO, null);

                dbManager.AddParameters(5, PARM_ASSIGNEDCHAT_FROM, null);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (Messagetype == "")
                    dbManager.AddParameters(7, PARM_MSG_TYPE, null);
                else
                    dbManager.AddParameters(7, PARM_MSG_TYPE, Messagetype);


                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRAC_PATIENT_CHAT, ds, ds.UserMessageReply.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::FillChatThread", PROC_PRAC_PATIENT_CHAT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMessage LoadPracticeMessage(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, string Messagetype, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Name == "")
                    Name = null;
                if (MessageDate == "")
                    MessageDate = null;

                dbManager.Open();
                dbManager.CreateParameters(11);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, UserMessagesId);

                dbManager.AddParameters(1, PARM_USER_ID, UserId);

                if (MessageDate == null)
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, MessageDate);

                if (PriorityId == 0)
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, PriorityId);

                dbManager.AddParameters(4, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.UserMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (AttatchedPatientId == 0)
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, AttatchedPatientId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (Messagetype == "")
                    dbManager.AddParameters(10, PARM_MSG_TYPE, null);
                else
                    dbManager.AddParameters(10, PARM_MSG_TYPE, Messagetype);


                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRAC_MESSAGES_SELECT, ds, ds.UserMessages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadPracticeMessage", PROC_PRAC_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMessage LoadUserMessage(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Name == "")
                    Name = null;
                if (MessageDate == "")
                    MessageDate = null;

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, UserMessagesId);

                dbManager.AddParameters(1, PARM_USER_ID, UserId);

                if (MessageDate == null)
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, MessageDate);

                if (PriorityId == 0)
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, PriorityId);

                dbManager.AddParameters(4, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.UserMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (AttatchedPatientId == 0)
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, AttatchedPatientId);
                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_MESSAGES_SELECT, ds, ds.UserMessages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadUserMessage", PROC_USER_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSMessage LoadMessageLog(long UserId, long PatientId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_USER_ID, UserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);
                if (PatientId == 0)
                    dbManager.AddParameters(3, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.UserMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOG_MESSAGES_SELECT, ds, ds.UserMessages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadUserMessage", PROC_LOG_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage LoadUserMessagesCount(long UserId, long PatientId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID_MESSAGE, UserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAGE_COUNT, ds, ds.MessagesCount.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadUserMessage", PROC_MESSAGE_COUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMessage LoadDirectMessage(string DirectAddress, string MessageDate, string MessageType, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (MessageDate == "")
                    MessageDate = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                //dbManager.AddParameters(0, PARM_USER_ID, UserId);

                if (MessageDate == null)
                    dbManager.AddParameters(1, PARM_MESSAGE_DATE, null);
                else
                    dbManager.AddParameters(1, PARM_MESSAGE_DATE, MessageDate);

                if (PageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                if (DirectAddress == "")
                    dbManager.AddParameters(4, PARM_DIRECT_ADD, null);
                else
                    dbManager.AddParameters(4, PARM_DIRECT_ADD, DirectAddress);
                if (MessageType == "")
                    dbManager.AddParameters(5, PARM_MSG_TYP, null);
                else
                    dbManager.AddParameters(5, PARM_MSG_TYP, MessageType);

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.UserMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DIRECT_MESSAGES_SELECT, ds, ds.UserMessages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadDirectMessage", PROC_DIRECT_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSMessage LoadPatientMessageLog(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Name == "")
                    Name = null;
                if (MessageDate == "")
                    MessageDate = null;

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, UserMessagesId);
                if (UserId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, UserId);
                if (MessageDate == null)
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_MESSAGE_DATE, MessageDate);

                if (PriorityId == 0)
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PRIORITY_ID, PriorityId);

                dbManager.AddParameters(4, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.UserMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (AttatchedPatientId == 0)
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ATTATCHEDPATIENT_ID, AttatchedPatientId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_MESSAGES_SELECT, ds, ds.UserMessages.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadPatientMessageLog", PROC_USER_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage LoadUserTask(long UserMessagesId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_MESSAGES_ID, UserMessagesId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_TASK_SELECT, ds, ds.PatMessages.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadUserTask", PROC_USER_TASK_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public void InsertSecretKey(string messageID, string secretKey, DateTime time, string userId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (messageID == "")
                    dbManager.AddParameters(0, PARM_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MESSAGES_ID, messageID);
                if (secretKey == "")
                    dbManager.AddParameters(1, PARM_SECRET_KEY, null);
                else
                    dbManager.AddParameters(1, PARM_SECRET_KEY, secretKey);
                if (secretKey == "")
                    dbManager.AddParameters(2, PARM_SECRET_KEY_TIME, null);
                else
                    dbManager.AddParameters(2, PARM_SECRET_KEY_TIME, time);

                if (userId == "")
                    dbManager.AddParameters(3, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_USER_ID, userId);


                var xyz = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SECRETKEY_INSERT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertSecretKey", PROC_PAT_MESSAGES_SECRETKEY_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public void UpdateSecretKey(string messageID, string secretKey, DateTime time, string userId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (messageID == "")
                    dbManager.AddParameters(0, PARM_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MESSAGES_ID, messageID);
                if (secretKey == "")
                    dbManager.AddParameters(1, PARM_SECRET_KEY, null);
                else
                    dbManager.AddParameters(1, PARM_SECRET_KEY, secretKey);
                if (secretKey == "")
                    dbManager.AddParameters(2, PARM_SECRET_KEY_TIME, null);
                else
                    dbManager.AddParameters(2, PARM_SECRET_KEY_TIME, time);
                if (userId == "")
                    dbManager.AddParameters(3, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_USER_ID, userId);

                var xyz = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SECRETKEY_UPDATE);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::UpdateSecretKey", PROC_PAT_MESSAGES_SECRETKEY_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSMessage CheckSteSecretKey(string UserMessagesId, string userId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (UserMessagesId == "")
                    dbManager.AddParameters(0, PARM_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MESSAGES_ID, UserMessagesId);
                if (userId == "")
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, userId);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SECRETKEY_SELECT, ds, ds.SecretKey.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::CheckSteSecretKey", PROC_PAT_MESSAGES_SECRETKEY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region "User Messages Document Stream"

        public DSMessage InsertUserMessageDocumentStream(DSMessage ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersDocumentStream(dbManager, ds, true);
                ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MESSAGE_DOCUMENTSSTREAM_INSERT, ds, ds.DocumentsStream.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertUserMessageDocumentStream", PROC_MESSAGE_DOCUMENTSSTREAM_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSMessage LoadUserMessageDocumentStream(Int32 DocumentsStrId, long UserMessagesId)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (DocumentsStrId == 0)
                    dbManager.AddParameters(0, PARM_DOCUMENTS_STR_Id, null);
                else
                    dbManager.AddParameters(0, PARM_DOCUMENTS_STR_Id, DocumentsStrId);
                if (UserMessagesId == 0)
                    dbManager.AddParameters(1, PARM_USER_MESSAGES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_MESSAGES_ID, UserMessagesId);
                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MESSAGE_DOCUMENTSSTREAM_SELECT, ds, ds.DocumentsStream.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadUserMessageDocumentStream", PROC_MESSAGE_DOCUMENTSSTREAM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Dashboard Messages

        public List<DUserMessageModel> LoadDashboardMessage(Int32 PriorityId, string Name, string MessageDate, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            List<DUserMessageModel> listModel = new List<DUserMessageModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                if (Name == "")
                    Name = null;
                if (MessageDate == "")
                    MessageDate = null;

                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_USER_ID, UserId);

                if (MessageDate == null)
                    dbManager.AddParameters(1, PARM_MESSAGE_DATE, null);
                else
                    dbManager.AddParameters(1, PARM_MESSAGE_DATE, MessageDate);

                if (PriorityId == 0)
                    dbManager.AddParameters(2, PARM_PRIORITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PRIORITY_ID, PriorityId);

                dbManager.AddParameters(3, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_MESSAGES_SELECT);
                DUserMessageModel model = null;
                while (reader.Read())
                {
                    model = new DUserMessageModel();
                    model.UserMessagesId = Convert.ToString(reader["UserMessagesId"]);
                    model.AssignedToId = Convert.ToString(reader["AssignedToId"]);
                    model.Subject = Convert.ToString(reader["Subject"]);
                    model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    model.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    model.PriorityName = Convert.ToString(reader["PriorityName"]);
                    listModel.Add(model);
                }

                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadDashboardMessage", PROC_DASHBOARD_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
        #region Dashboard User Tasks
        public List<DUserTaskModel> LoadDashboardUserTasks(int MsgStatusId, long AssignedToId = 0, string MsgTypeShortName = "", Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            List<DUserTaskModel> listModel = new List<DUserTaskModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (MsgStatusId == 0)
                    dbManager.AddParameters(0, PARM_MSG_STATUS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MSG_STATUS_ID, MsgStatusId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (AssignedToId == 0)
                    dbManager.AddParameters(2, PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ASSIGNED_TO_ID, AssignedToId);
                if (MsgTypeShortName == "")
                    dbManager.AddParameters(3, PARM_MSG_TYPE_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_MSG_TYPE_SHORT_NAME, MsgTypeShortName);

                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PAT_MESSAGES_SELECT);
                DUserTaskModel model = null;
                while (reader.Read())
                {
                    model = new DUserTaskModel();
                    model.PatMsgId = Convert.ToString(reader["PatMsgId"]);
                    model.PatientId = Convert.ToString(reader["PatientId"]);
                    model.MsgDetail = Convert.ToString(reader["MsgDetail"]);
                    model.MsgStatusId = Convert.ToString(reader["MsgStatusId"]);
                    model.MessageStatus = Convert.ToString(reader["MessageStatus"]);
                    model.EntryDate = Convert.ToString(reader["CreatedOn"]);
                    model.AssignedToId = Convert.ToString(reader["AssignedToId"]);
                    model.AssigneeName = Convert.ToString(reader["AssigneeName"]);
                    model.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    model.FacilityName = Convert.ToString(reader["FacilityName"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    listModel.Add(model);
                }

                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadDashboardUserTasks", PROC_PAT_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        #region Direct Messaging

        public DSMessage InsertDirectMessage(DSMessage ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersDirectMsgs(dbManager, ds, true);
                ds = (DSMessage)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_OUTGOING_DIRECT_MESSAGES_INSERT, ds, ds.DirectMessages.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::InsertDirectMessage", PROC_OUTGOING_DIRECT_MESSAGES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMessage LoadOutgoingDirectMessage( Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            DSMessage ds = new DSMessage();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (PageNumber == 0)
                    dbManager.AddParameters(0, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(1, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(1, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(2, PARM_RECORD_COUNT, ds.DirectMessages.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSMessage)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OUTGOING_DIRECT_MESSAGES_SELECT, ds, ds.DirectMessages.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::LoadOutgoingDirectMessage", PROC_OUTGOING_DIRECT_MESSAGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Direct Message Status.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="isdelivered">Delivery Status.</param>
        /// <returns></returns>
        public string UpdateOutgoingDirectMessageStatus(string IDs, bool isdelivered)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_IDs, IDs);
                dbManager.AddParameters(1, PARM_ISDELIVERED, isdelivered);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_OUTGOING_DIRECT_MESSAGES_STATUS_UPDATE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMessage::UpdateOutgoingDirectMessageStatus", PROC_OUTGOING_DIRECT_MESSAGES_STATUS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
    }
}
