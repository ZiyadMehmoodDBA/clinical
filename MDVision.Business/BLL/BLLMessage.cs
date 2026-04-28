using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using MDVision.DataAccess.DAL.Message;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Dashboard;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{
    public class BLLMessage
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLPatient"/> class.
        /// </summary>
        public BLLMessage()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
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

        #region Variable

        #endregion

        #region PatientMessages


        /// <summary>
        /// Loads the patient Messages.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatMsgId">The patient Message identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSMessage> LoadPatientMessage(long PatientId, long PatMsgId, string MsgTypeId, int MsgStatusId, DateTime? CallDate, DateTime? EntryDate, long AssignedToId = 0, string MsgTypeShortName = "", Int32 PageNumber = 1, Int32 RowsPerPage = 15, string MsgTypeNot = "", long UserMessagesId = 0)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadMessage(PatientId, PatMsgId, MsgTypeId, MsgStatusId, CallDate, EntryDate, AssignedToId, MsgTypeShortName, PageNumber, RowsPerPage, MsgTypeNot, UserMessagesId);
                dsReply = new DALMessage().LoadMessageReply(PatMsgId, 0);
                ds.Merge(dsReply);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadPatientMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<List<Model.User.UserMessagesCount>> LoadPatientMessageCount(long PatientId, long PatMsgId, string MsgTypeId, int MsgStatusId, DateTime? CallDate, DateTime? EntryDate, long AssignedToId = 0, string MsgTypeShortName = "", Int32 PageNumber = 1, Int32 RowsPerPage = 15, string MsgTypeNot = "", long UserMessagesId = 0)
        {
            try
            {
                List<Model.User.UserMessagesCount> listUserMessages = new DALMessage().LoadMessage_(PatientId, PatMsgId, MsgTypeId, MsgStatusId, CallDate, EntryDate, AssignedToId, MsgTypeShortName, PageNumber, RowsPerPage, MsgTypeNot, UserMessagesId);
                return new BLObject<List<Model.User.UserMessagesCount>>(listUserMessages);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadPatientMessageCount", ex);
                return new BLObject<List<Model.User.UserMessagesCount>>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> searchPatientMessage(long PatientId, long PatMsgId, string MsgTypeId, int MsgStatusId, DateTime? CallDate, DateTime? EntryDate, string IsActive, Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().searchMessage(PatientId, MsgTypeId, MsgStatusId, CallDate, EntryDate, IsActive, PageNumber, RowsPerPage);
                // dsReply = new DALMessage().LoadMessageReply(PatMsgId, 0);
                //  ds.Merge(dsReply);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::searchPatientMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the patient Message.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSMessage> InsertPatientMessage(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().InsertMessage(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertPatientMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the patient Message.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSMessage> UpdatePatientMessage(DSMessage ds, IDBManager dbManager = null)
        {
            try
            {
                ds = new DALMessage().UpdateMessage(ds, dbManager);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::UpdatePatientMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient Message.
        /// </summary>
        /// <param name="PatMsgId">The patient Message identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientMessage(string PatMsgId)
        {
            try
            {
                PatMsgId = new DALMessage().DeleteMessage(PatMsgId);
                return new BLObject<string>(PatMsgId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::DeletePatientMessage", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public void InsertSecretKey(string messageID, string secretKey, DateTime time, string userId)
        {
            try
            {
                new DALMessage().InsertSecretKey(messageID, secretKey, time, userId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertSecretKey", ex);
            }
        }
        public void UpdateSecretKey(string messageID, string secretKey, DateTime time, string userId)
        {
            try
            {
                new DALMessage().UpdateSecretKey(messageID, secretKey, time, userId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::UpdateSecretKey", ex);
            }
        }
        public BLObject<DSMessage> CheckSteSecretKey(string UserMessagesId, string userId)
        {
            DSMessage ds = new DSMessage();
            try
            {
                ds = new DALMessage().CheckSteSecretKey(UserMessagesId, userId);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::CheckSteSecretKey", ex);
                //return new BLObject<DSMessage>(null, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region PatientMessagesReply


        /// <summary>
        /// Loads the patient Messages Reply.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatMsgId">The patient Message identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSMessage> LoadPatientMessageReply(long PatMsgId, long MsgrId)
        {
            try
            {
                DSMessage ds = new DSMessage();
                ds = new DALMessage().LoadMessageReply(PatMsgId, MsgrId);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadPatientMessageReply", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the patient Message Reply.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSMessage> InsertPatientMessageReply(DSMessage ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                ds = new DALMessage().InsertMessageReply(ds, dbManager);

                foreach (DataRow drReply in ds.Tables[ds.MsgReply.TableName].Rows)
                {
                    DSMessage dsMsgs = new DSMessage();
                    dsMsgs = new DALMessage().LoadMessage(0, MDVUtility.ToInt64(drReply[ds.MsgReply.PatMsgIdColumn]), "", 0, null, null, 0, "", 1, 1000);
                    foreach (DataRow drMessage in dsMsgs.PatMessages.Rows)
                    {
                        drMessage[dsMsgs.PatMessages.AssignedToIdColumn] = drReply[ds.MsgReply.AssignedToIdColumn];
                        drMessage[dsMsgs.PatMessages.MsgStatusIdColumn] = drReply[ds.MsgReply.MsgStatusIdColumn];
                    }

                    BLObject<DSMessage> dsMessages = UpdatePatientMessage(dsMsgs, dbManager);

                }

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLMessage::InsertPatientMessageReply", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the patient Message Reply.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSMessage> UpdatePatientMessageReply(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().UpdateMessageReply(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::UpdatePatientMessageReply", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient Message Reply.
        /// </summary>
        /// <param name="PatMsgId">The patient Message identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientMessageReply(string MsgrId)
        {
            try
            {
                MsgrId = new DALMessage().DeleteMessageReply(MsgrId);
                return new BLObject<string>(MsgrId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::DeletePatientMessageReply", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the MessageTypes.
        /// </summary>
        /// <returns>BLObject&lt;DSMessageLookup&gt;.</returns>
        public BLObject<DSMessageLookup> LookupMessageTypes()
        {
            try
            {
                DSMessageLookup ds = new DSMessageLookup();
                ds = new DALMessage().LookupMessageTypes();

                return new BLObject<DSMessageLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LookupMessageTypes", ex);
                return new BLObject<DSMessageLookup>(null, ex.Message);
            }

        }


        /// <summary>
        ///  the LookupMessageStatus.
        /// </summary>
        /// <returns>BLObject&lt;DSMessageLookup&gt;.</returns>
        public BLObject<DSMessageLookup> LookupMessageStatus()
        {
            try
            {
                DSMessageLookup ds = new DSMessageLookup();
                ds = new DALMessage().LookupMessageStatus();

                return new BLObject<DSMessageLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LookupMessageStatus", ex);
                return new BLObject<DSMessageLookup>(null, ex.Message);
            }

        }

        /// <summary>
        ///  the LookupMsgAlertTypes.
        /// </summary>
        /// <returns>BLObject&lt;DSMessageLookup&gt;.</returns>
        public BLObject<DSMessageLookup> LookupMsgAlertTypes()
        {
            try
            {
                DSMessageLookup ds = new DSMessageLookup();
                ds = new DALMessage().LookupMsgAlertTypes();

                return new BLObject<DSMessageLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LookupMsgAlertTypes", ex);
                return new BLObject<DSMessageLookup>(null, ex.Message);
            }

        }

        /// <summary>
        ///  the LookupMsgPriority.
        /// </summary>
        /// <returns>BLObject&lt;DSMessageLookup&gt;.</returns>
        public BLObject<DSMessageLookup> LookupMsgPriority()
        {
            try
            {
                DSMessageLookup ds = new DSMessageLookup();
                ds = new DALMessage().LookupMsgPriority();

                return new BLObject<DSMessageLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LookupMsgPriority", ex);
                return new BLObject<DSMessageLookup>(null, ex.Message);
            }

        }

        /// <summary>
        ///  the LookupAmendmentSource.
        /// </summary>
        /// <returns>BLObject&lt;DSMessageLookup&gt;.</returns>
        public BLObject<DSMessageLookup> LookupAmendmentSource()
        {
            try
            {
                DSMessageLookup ds = new DSMessageLookup();
                ds = new DALMessage().LookupAmendmentSource();

                return new BLObject<DSMessageLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LookupAmendmentSource", ex);
                return new BLObject<DSMessageLookup>(null, ex.Message);
            }

        }

        public BLObject<DSMessageLookup> LookupMessagesPriority()
        {
            try
            {
                DSMessageLookup ds = new DSMessageLookup();
                ds = new DALMessage().LookupMessagesPriority();

                return new BLObject<DSMessageLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LookupMessagesPriority", ex);
                return new BLObject<DSMessageLookup>(null, ex.Message);
            }

        }

        #endregion

        #region"User Messages"

        public BLObject<DSMessage> InsertUserMessage(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().InsertUserMessage(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> InsertPracticeMessage(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().InsertPracticeMessage(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertPracticeMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> UpdateUserMessage(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().UpdateUserMessage(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::UpdateUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteUserMessage(string UserMessagesId)
        {
            try
            {
                UserMessagesId = new DALMessage().DeleteUserMessage(UserMessagesId);
                return new BLObject<string>(UserMessagesId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::DeleteUserMessage", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> LoadUserMessage(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadUserMessage(UserMessagesId, PriorityId, Name, MessageDate, AttatchedPatientId, UserId, PageNumber, RowsPerPage);
                if (AttatchedPatientId > 0)
                {
                    ds.Merge(new DALMessage().LoadPatientMessageLog(UserMessagesId, PriorityId, Name, null, AttatchedPatientId, 0, PageNumber, RowsPerPage));
                }
                if (UserMessagesId > 0)
                {
                    ds.Merge(new DALMessage().LoadUserMessageDocumentStream(0, UserMessagesId));
                }

                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }


        public BLObject<DSMessage> LoadMessageLog(long UserId, long PatientId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadMessageLog(UserId, PatientId, PageNumber, RowsPerPage);

                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> FillPracticeMessage(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, string Messagetype, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().FillPracticeMessage(UserMessagesId, PriorityId, Name, MessageDate, AttatchedPatientId, UserId, Messagetype, PageNumber, RowsPerPage);
                if (AttatchedPatientId > 0)
                {
                    ds.Merge(new DALMessage().LoadPatientMessageLog(UserMessagesId, PriorityId, Name, null, AttatchedPatientId, 0, PageNumber, RowsPerPage));
                }
                if (UserMessagesId > 0)
                {
                    ds.Merge(new DALMessage().LoadUserMessageDocumentStream(0, UserMessagesId));
                }

                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> FillChatThread(long UserMessagesId, long UserId, string Messagetype)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().FillChatThread(UserMessagesId, UserId, Messagetype);


                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }
        public BLObject<DSMessage> LoadPracticeMessage(long UserMessagesId, Int32 PriorityId, string Name, string MessageDate, long AttatchedPatientId, long UserId, string Messagetype, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadPracticeMessage(UserMessagesId, PriorityId, Name, MessageDate, AttatchedPatientId, UserId, Messagetype, PageNumber, RowsPerPage);
                //if (AttatchedPatientId > 0)
                //{
                //    ds.Merge(new DALMessage().LoadPatientMessageLog(UserMessagesId, PriorityId, Name, null, AttatchedPatientId, 0, PageNumber, RowsPerPage));
                //}
                if (UserMessagesId > 0)
                {
                    ds.Merge(new DALMessage().LoadUserMessageDocumentStream(0, UserMessagesId));
                }

                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> LoadUserMessagesCount(long UserId, long PatientId)
        {
            try
            {
                DSMessage ds = new DSMessage();
                ds = new DALMessage().LoadUserMessagesCount(UserId, PatientId);


                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessagesCount", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }
        public BLObject<DSMessage> LoadDirectMessage(string DirectAddress, string MessageDate, string MessageType, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadDirectMessage(DirectAddress, MessageDate, MessageType, UserId, PageNumber, RowsPerPage);
                //if (AttatchedPatientId > 0)
                //{
                //    ds.Merge(new DALMessage().LoadPatientMessageLog(UserMessagesId, PriorityId, Name, null, AttatchedPatientId, 0, PageNumber, RowsPerPage));
                //}
                //if (UserMessagesId > 0)
                //{
                //    ds.Merge(new DALMessage().LoadUserMessageDocumentStream(0, UserMessagesId));
                //}

                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> LoadUserTask(long UserMessagesId)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadUserTask(UserMessagesId);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadUserTask", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> InsertUserMessageDocumentStream(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().InsertUserMessageDocumentStream(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertUserMessageDocumentStream", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        #endregion
        #region Dashboard Messages
        public BLObject<List<DUserMessageModel>> LoadDashboardMessage(Int32 PriorityId, string Name, string MessageDate, long UserId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                var result = new DALMessage().LoadDashboardMessage(PriorityId, Name, MessageDate, UserId, PageNumber, RowsPerPage);

                return new BLObject<List<DUserMessageModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadDashboardMessage", ex);
                return new BLObject<List<DUserMessageModel>>(null, ex.Message);
            }
        }

        #endregion
        #region Dashboard User Tasks
        public BLObject<List<DUserTaskModel>> LoadDashboardUserTasks(int MsgStatusId, long AssignedToId = 0, string MsgTypeShortName = "", Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            try
            {
                var result = new DALMessage().LoadDashboardUserTasks(MsgStatusId, AssignedToId, MsgTypeShortName, PageNumber, RowsPerPage);

                return new BLObject<List<DUserTaskModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadDashboardUserTasks", ex);
                return new BLObject<List<DUserTaskModel>>(null, ex.Message);
            }
        }

        #endregion


        #region Direct Messaging
        public BLObject<DSMessage> InsertDirectMessage(DSMessage ds)
        {
            try
            {
                ds = new DALMessage().InsertDirectMessage(ds);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertDirectMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<DSMessage> LoadOutgoingDirectMessage(Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            try
            {
                DSMessage ds, dsReply = new DSMessage();
                ds = new DALMessage().LoadOutgoingDirectMessage(PageNumber, RowsPerPage);
                return new BLObject<DSMessage>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::LoadOutgoingDirectMessage", ex);
                return new BLObject<DSMessage>(null, ex.Message);
            }
        }

        public BLObject<string> UpdateOutgoingDirectMessageStatus(string IDs, bool isdelivered)
        {
            try
            {
                return new BLObject<string>(new DALMessage().UpdateOutgoingDirectMessageStatus(IDs, isdelivered));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::UpdateOutgoingDirectMessageStatus", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion
    }
}
