using System;
using System.Collections.Generic;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Model.Batch.Fax;

namespace MDVision.DataAccess.DAL.BatchFax
{
    public class DALBatchFax
    {


        #region " Stored Procedure Names"
        private const string PROC_USER_FAX_SETTING_SELECT = "Provider.sp_FaxSettingsUsersSelect";
        private const string PROC_FAX_CONTACT_SELECT = "Provider.sp_FaxContactsSelect";
        private const string PROC_PROVIDER_FAX_CONTACT_INSERT = "Provider.sp_ProviderFaxContactsInsert";
        private const string PROC_FACILITY_FAX_CONTACT_INSERT = "Provider.sp_FacilityFaxContactsInsert";

        private const string PROC_PROVIDER_FAX_CONTACT_DELETE = "Provider.sp_ProviderFaxContactsDelete";
        private const string PROC_FACILITY_FAX_CONTACT_DELETE = "Provider.sp_FacilityFaxContactsDelete";
        // Fax documents

        private const string PROC_FAX_DOCUMENT_INSERT = "Patient.sp_FaxDocumentsInsert";

        private const string PROC_FAX_CONFIDENTIALITY_SELECT = "Patient.sp_FaxDocumentSelect";
        private const string PROC_CUSTOMFORM_LETTER_NAME = "Clinical.sp_CustomFormLetterLookup";
        private const string PROC_OUTGOING_REFERRALS = "Clinical.SP_NotesOutgoingReferrals";

        private const string PROC_FAX_OUTBOX_DETAIL_DELETE = "Provider.sp_FaxOutboxDetailDelete";
        private const string PROC_FAX_OUTBOX_DETAIL_INSERT = "Provider.sp_FaxOutboxDetailInsert";
        private const string PROC_FAX_OUTBOX_DETAIL_SELECT = "Provider.sp_FaxOutboxDetailSelect";
        private const string PROC_FAX_OUTBOX_DETAIL_UPDATE = "Provider.sp_FaxOutboxDetailUpdate";
        private const string PROC_FAX_OUTBOX_DETAIL_INACTIVE = "Provider.sp_FaxOutboxDetailInactive";

        #endregion

        #region "Parameters"

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_NOTE_ID = "@noteid";
        private const string PARM_PATIENT_ID = "@patientId";
        private const string PARM_FAX_DETAILS_ID = "@FaxDetailsID";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_SUBJECT = "@Subject";
        private const string PARM_SENT_TO_NAME = "@SentToName";
        private const string PARM_TO_FAX_NUMBER = "@ToFaxNumber";
        private const string PARM_FILE_NAME = "@FileName";
        private const string PARM_PAGES = "@Pages";
        private const string PARM_SENT_STATUS = "@SentStatus";
        private const string PARM_ERROR_CODE = "@ErrorCode";
        private const string PARM_CALLER_ID = "@CallerID";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";

        #endregion

        #region Constructors

        public DALBatchFax()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALBatchFax(SharedVariable SharedVariable)
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

        #region "Utility functions"

        public DSProfile LoadProviderFaxSettingsUsers(long UserId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, "@ProviderId", null);

                dbManager.AddParameters(1, "@FacilityId", null);

                dbManager.AddParameters(2, "@UserId", UserId);

                if (PageNumber == 0)
                {
                    dbManager.AddParameters(3, "@PageNumber", null);
                }
                else
                {
                    dbManager.AddParameters(3, "@PageNumber", PageNumber);
                }
                if (RowspPage == 0)
                {
                    dbManager.AddParameters(4, "@RowspPage", null);
                }
                else
                {
                    dbManager.AddParameters(4, "@RowspPage", RowspPage);
                }
                dbManager.AddParameters(5, "@IsProvider", false);
                dbManager.AddParameters(6, "@IsCompose", true);

                List<string> tableNames = new List<string>
                {
                ds.ProviderFaxSettingsUsers.TableName,
                ds.FacilityFaxSettingsUsers.TableName
                };


                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_FAX_SETTING_SELECT, ds, tableNames);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_USER_FAX_SETTING_SELECT, ex);
                //throw ex;
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

        #region "Contacts"

        public DSProfile SaveProviderContact(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, "@ProviderId", ds.ProviderFaxContacts.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@ContactName", ds.ProviderFaxContacts.ContactNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@FaxNumber", ds.ProviderFaxContacts.FaxNumberColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@CreatedBy", ds.ProviderFaxContacts.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@CreatedOn", ds.ProviderFaxContacts.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, "@ModifiedBy", ds.ProviderFaxContacts.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, "@ModifiedOn", ds.ProviderFaxContacts.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(7, "@ContactId", ds.ProviderFaxContacts.ContactIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROVIDER_FAX_CONTACT_INSERT, ds, ds.ProviderFaxContacts.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProviderFaxContact", PROC_PROVIDER_FAX_CONTACT_INSERT, ex);
                //throw ex;
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

        public DSDocument SaveFaxDocument(ref DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);
                dbManager.AddParameters(0, "@DocumentId", ds.FaxDocuments.DocIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(1, "@UserId", ds.FaxDocuments.UserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, "@FileType", ds.FaxDocuments.FileTypeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@FilePath", ds.FaxDocuments.FilePathColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@FileStream", ds.FaxDocuments.FileStreamColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@Pages", ds.FaxDocuments.PagesColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, "@Comments", ds.FaxDocuments.CommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, "@IsActive", ds.FaxDocuments.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(8, "@CreatedBy", ds.FaxDocuments.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, "@CreatedOn", ds.FaxDocuments.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(10, "@ModifiedBy", ds.FaxDocuments.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, "@ModifiedOn", ds.FaxDocuments.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(12, "@FaxId", ds.FaxDocuments.FaxIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(13, "@IsConfidential", ds.FaxDocuments.IsConfidentialColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(14, "@AssignedByUserId", ds.FaxDocuments.AssignedByUserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(15, "@FaxDocumentPath", ds.FaxDocuments.FaxDocumentPathColumn.ColumnName, DbType.String);


                ds = (DSDocument)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FAX_DOCUMENT_INSERT, ds, ds.FaxDocuments.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProviderFaxContact", PROC_PROVIDER_FAX_CONTACT_INSERT, ex);
                //throw ex;
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

        public DSDocument LoadFaxConfidentiality(int PageNumber, int RowspPage)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@RowspPage", PageNumber);
                dbManager.AddParameters(1, "@PageNumber", PageNumber);

                //    dbManager.AddParameters(1, "@IsConfidential", null);

                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAX_CONFIDENTIALITY_SELECT, ds, ds.FaxDocuments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_USER_FAX_SETTING_SELECT, ex);
                //throw ex;
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
        public DSProfile SaveFacilityContact(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, "@FacilityId", ds.FacilityFaxContacts.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@ContactName", ds.FacilityFaxContacts.ContactNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@FaxNumber", ds.FacilityFaxContacts.FaxNumberColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@CreatedBy", ds.FacilityFaxContacts.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@CreatedOn", ds.FacilityFaxContacts.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, "@ModifiedBy", ds.FacilityFaxContacts.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, "@ModifiedOn", ds.FacilityFaxContacts.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(7, "@ContactId", ds.ProviderFaxContacts.ContactIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FACILITY_FAX_CONTACT_INSERT, ds, ds.FacilityFaxContacts.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::InsertFacilityFaxContact", PROC_FACILITY_FAX_CONTACT_INSERT, ex);
                //throw ex;
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

        public DSProfile LoadProviderContacts(long ProviderId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, "@ProviderId", ProviderId);

                dbManager.AddParameters(1, "@FacilityId", null);

                dbManager.AddParameters(2, "@IsProvider", true);

                dbManager.AddParameters(3, "@IsSearch", false);

                dbManager.AddParameters(4, "@ContactName", null);

                if (PageNumber == 0)
                {
                    dbManager.AddParameters(5, "@PageNumber", null);
                }
                else
                {
                    dbManager.AddParameters(5, "@PageNumber", PageNumber);
                }
                if (RowspPage == 0)
                {
                    dbManager.AddParameters(6, "@RowspPage", null);
                }
                else
                {
                    dbManager.AddParameters(6, "@RowspPage", RowspPage);
                }





                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAX_CONTACT_SELECT, ds, ds.ProviderFaxContacts.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_FAX_CONTACT_SELECT, ex);
                //throw ex;
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

        public DSProfile LoadFacilityContacts(long FacilityId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, "@ProviderId", null);

                dbManager.AddParameters(1, "@FacilityId", FacilityId);

                dbManager.AddParameters(2, "@IsProvider", false);

                dbManager.AddParameters(3, "@IsSearch", false);

                dbManager.AddParameters(4, "@ContactName", null);

                if (PageNumber == 0)
                {
                    dbManager.AddParameters(5, "@PageNumber", null);
                }
                else
                {
                    dbManager.AddParameters(5, "@PageNumber", PageNumber);
                }
                if (RowspPage == 0)
                {
                    dbManager.AddParameters(6, "@RowspPage", null);
                }
                else
                {
                    dbManager.AddParameters(6, "@RowspPage", RowspPage);
                }



                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAX_CONTACT_SELECT, ds, ds.FacilityFaxContacts.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_FAX_CONTACT_SELECT, ex);
                //throw ex;
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

        public DSProfile SearchContacts(bool IsProvider, long ProviderId, long FacilityId, string contactName, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                if (ProviderId == 0)
                {
                    dbManager.AddParameters(0, "@ProviderId", null);
                }
                else
                {
                    dbManager.AddParameters(0, "@ProviderId", ProviderId);
                }
                if (FacilityId == 0)
                {
                    dbManager.AddParameters(1, "@FacilityId", null);
                }
                else
                {
                    dbManager.AddParameters(1, "@FacilityId", FacilityId);
                }

                dbManager.AddParameters(2, "@IsProvider", IsProvider);

                dbManager.AddParameters(3, "@IsSearch", 1);

                dbManager.AddParameters(4, "@ContactName", contactName);

                if (PageNumber == 0)
                {
                    dbManager.AddParameters(5, "@PageNumber", null);
                }
                else
                {
                    dbManager.AddParameters(5, "@PageNumber", PageNumber);
                }
                if (RowspPage == 0)
                {
                    dbManager.AddParameters(6, "@RowspPage", null);
                }
                else
                {
                    dbManager.AddParameters(6, "@RowspPage", RowspPage);
                }


                if (IsProvider == true)
                {
                    ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAX_CONTACT_SELECT, ds, ds.ProviderFaxContacts.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                else
                {
                    ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAX_CONTACT_SELECT, ds, ds.FacilityFaxContacts.TableName);
                    ds.AcceptChanges();
                    return ds;

                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_FAX_CONTACT_SELECT, ex);
                //throw ex;
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
        public string DeleteProviderFaxContacts(long ProviderId, long ContactId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@ProviderId", ProviderId);
                dbManager.AddParameters(1, "@ContactId", ContactId);
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROVIDER_FAX_CONTACT_DELETE).ToString();

                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::DeleteProviderFaxContact", PROC_PROVIDER_FAX_CONTACT_DELETE, ex);
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

        public string DeleteFacilityFaxContacts(long FacilityId, long ContactId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@FacilityId", FacilityId);
                dbManager.AddParameters(1, "@ContactId", ContactId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FACILITY_FAX_CONTACT_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::DeleteFacilityFaxContact", PROC_FACILITY_FAX_CONTACT_DELETE, ex);
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


        #region "Patient Custom Form LookUps"

        public List<CustomFormLookupModel> GetCustomFormLetters()
        {
            List<CustomFormLookupModel> listCustomForm = new List<CustomFormLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOMFORM_LETTER_NAME);
                CustomFormLookupModel model = null;
                while (reader.Read())
                {
                    model = new CustomFormLookupModel();
                    model.CustomFormId = reader["CustomFormId"].ToString();
                    model.CustomFormName = reader["FormName"].ToString();
                    model.Title = reader["Title"].ToString();

                    listCustomForm.Add(model);
                }

                return listCustomForm;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::GetCustomFormLetters", PROC_CUSTOMFORM_LETTER_NAME, ex);
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


        #region "Outgoing Referrals LookUp"

        public List<OutgoingReferralsLookupModel> GetOutgoingReferrals(Int64 noteId, Int64 patientId)
        {
            List<OutgoingReferralsLookupModel> listOutgoingReferrals = new List<OutgoingReferralsLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_OUTGOING_REFERRALS);
                OutgoingReferralsLookupModel model = null;
                while (reader.Read())
                {
                    model = new OutgoingReferralsLookupModel();
                    model.ProviderName = reader["ProviderName"].ToString();
                    model.FaxNumber = reader["FaxNumber"].ToString();
                    model.RowNumber = reader["RowNumber"].ToString();


                    listOutgoingReferrals.Add(model);
                }

                return listOutgoingReferrals;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::GetOutgoingReferrals", PROC_OUTGOING_REFERRALS, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }


        public List<OutgoingReferralsLookupModel> GetPatientReferrals(Int64 patientId)
        {
            List<OutgoingReferralsLookupModel> listOutgoingReferrals = new List<OutgoingReferralsLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_OUTGOING_REFERRALS);
                OutgoingReferralsLookupModel model = null;
                while (reader.Read())
                {
                    model = new OutgoingReferralsLookupModel();
                    model.ProviderName = reader["ProviderName"].ToString();
                    model.FaxNumber = reader["FaxNumber"].ToString();
                    model.RowNumber = reader["RowNumber"].ToString();


                    listOutgoingReferrals.Add(model);
                }

                return listOutgoingReferrals;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::GetOutgoingReferrals", PROC_OUTGOING_REFERRALS, ex);
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

        #region "Batch Fax"

        private void CreateFaxOutboxDetailParameters(IDBManager dbManager, FaxOutboxDetailModel model, Boolean IsInsert)
        {
            if (IsInsert)
            {
                dbManager.CreateParameters(13);
                dbManager.AddParameters(0, PARM_FAX_DETAILS_ID, model.FaxDetailsID);
                dbManager.AddParameters(1, PARM_USER_ID, model.UserId);
                dbManager.AddParameters(2, PARM_PROVIDER_ID, model.ProviderId);
                dbManager.AddParameters(3, PARM_SUBJECT, model.Subject);
                dbManager.AddParameters(4, PARM_SENT_TO_NAME, model.SentToName);
                dbManager.AddParameters(5, PARM_TO_FAX_NUMBER, model.ToFaxNumber);
                dbManager.AddParameters(6, PARM_SENT_STATUS, model.SentStatus);
                dbManager.AddParameters(7, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(8, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(9, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(10, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(11, PARM_CALLER_ID, model.CallerID);
                if (model.PatientId == "0")
                    dbManager.AddParameters(12, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(12, PARM_PATIENT_ID, model.PatientId);

            }
            else
            {
                dbManager.AddParameters(PARM_FAX_DETAILS_ID, model.FaxDetailsID);
                dbManager.AddParameters(PARM_FILE_NAME, model.FileName);
                dbManager.AddParameters(PARM_PAGES, model.Pages);
                dbManager.AddParameters(PARM_SENT_STATUS, model.SentStatus);
                dbManager.AddParameters(PARM_ERROR_CODE, model.ErrorCode);
                dbManager.AddParameters(PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(PARM_MODIFIED_ON, model.ModifiedOn);
                if (model.PatientId == "0")
                    dbManager.AddParameters( PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, model.PatientId);
            }

        }
        public string SaveFaxOutboxDetail(FaxOutboxDetailModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                CreateFaxOutboxDetailParameters(dbManager, model,true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_FAX_OUTBOX_DETAIL_INSERT);
                return model.FaxDetailsID;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::SaveFaxOutboxDetail", PROC_FAX_OUTBOX_DETAIL_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<FaxOutboxDetailModel> LoadFaxOutboxDetail(long FaxDetailsID, string StartDate, string EndDate, int PageNumber = 1, int RowspPage = 15,long UserId=0, long ProviderId = 0)
        {
            List<FaxOutboxDetailModel> list = new List<FaxOutboxDetailModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (FaxDetailsID <= 0)
                    dbManager.AddParameters(0, PARM_FAX_DETAILS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAX_DETAILS_ID, FaxDetailsID);

                if (StartDate == "")
                    dbManager.AddParameters(1, PARM_START_DATE, null);
                else
                    dbManager.AddParameters(1, PARM_START_DATE, StartDate);

                if (EndDate == "")
                    dbManager.AddParameters(2, PARM_END_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_END_DATE, EndDate);

                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowspPage);
                if (UserId == 0)
                    dbManager.AddParameters(5 ,PARM_USER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_USER_ID, UserId);
                if (ProviderId == 0)
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, ProviderId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_FAX_OUTBOX_DETAIL_SELECT);
                FaxOutboxDetailModel model = null;
                while (reader.Read())
                {
                    model = new FaxOutboxDetailModel();
                    model.CallerID = !String.IsNullOrEmpty(reader["CallerID"].ToString()) ? reader["CallerID"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ErrorCode = !String.IsNullOrEmpty(reader["ErrorCode"].ToString()) ? reader["ErrorCode"].ToString() : "";
                    model.FaxDetailsID = !String.IsNullOrEmpty(reader["FaxDetailsID"].ToString()) ? reader["FaxDetailsID"].ToString() : "";
                    model.FileName = !String.IsNullOrEmpty(reader["FileName"].ToString()) ? reader["FileName"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.Pages = !String.IsNullOrEmpty(reader["Pages"].ToString()) ? reader["Pages"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.SenderName = !String.IsNullOrEmpty(reader["SenderName"].ToString()) ? reader["SenderName"].ToString() : "";
                    model.SentStatus = !String.IsNullOrEmpty(reader["SentStatus"].ToString()) ? reader["SentStatus"].ToString() : "";
                    model.SentToName = !String.IsNullOrEmpty(reader["SentToName"].ToString()) ? reader["SentToName"].ToString() : "";
                    model.Subject = !String.IsNullOrEmpty(reader["Subject"].ToString()) ? reader["Subject"].ToString() : "";
                    model.ToFaxNumber = !String.IsNullOrEmpty(reader["ToFaxNumber"].ToString()) ? reader["ToFaxNumber"].ToString() : "";
                    model.UserId = !String.IsNullOrEmpty(reader["UserId"].ToString()) ? reader["UserId"].ToString() : "";
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::LoadFaxOutboxDetail", PROC_FAX_OUTBOX_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string DeleteFaxOutboxDetail(string FaxDetailsID)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FAX_DETAILS_ID, FaxDetailsID);
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAX_OUTBOX_DETAIL_DELETE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::DeleteFaxOutboxDetail", PROC_FAX_OUTBOX_DETAIL_DELETE, ex);
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
        public string InactiveFaxOutboxDetail(string FaxDetailsID)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FAX_DETAILS_ID, FaxDetailsID);
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAX_OUTBOX_DETAIL_INACTIVE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::InactiveFaxOutboxDetail", PROC_FAX_OUTBOX_DETAIL_INACTIVE, ex);
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
        public string UpdateFaxOutboxDetail(FaxOutboxDetailModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateFaxOutboxDetailParameters(dbManager, model, false);
                dbManager.ExecuteScalar(PROC_FAX_OUTBOX_DETAIL_UPDATE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchFax::UpdateFaxOutboxDetail", PROC_FAX_OUTBOX_DETAIL_UPDATE, ex);
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
