using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Model.Document;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Document
{
    public class DALDocument
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALDocument"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALDocument()
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
        private const string PROC_DOCUMENT_LOOKUP = "System.sp_Documentslookup";
        private const string PROC_DOCUMENT_FOLDER_DETAIL = "Patient.sp_DocumentFolderDetail";
        private const string PROC_Folders_LOOKUP = "System.sp_Folderslookup";
        private const string PROC_DOCUMENT_TYPE_LOOKUP = "System.sp_DocumentTypeLookup";
        private const string PROC_PATIENT_REPRESENTATIVE_LOOKUP = "Patient.sp_PatientRepresentativeLookup";
        private const string PROC_DOCUMENTS_INSERT = "System.sp_DocumentsInsert";
        private const string PROC_DOCUMENTS_DELETE = "System.sp_DocumentsDelete";
        private const string PROC_DOCUMENTS_SELECT = "System.sp_DocumentsSelect";
        private const string PROC_DOCUMENTS_UPDATE = "System.sp_DocumentsUpdate";
        private const string PROC_DOCUMENT_TYPE_DELETE = "System.sp_DocumentTypeDelete";
        private const string PROC_DOCUMENT_TYPE_INSERT = "System.sp_DocumentTypeInsert";
        private const string PROC_DOCUMENT_TYPE_SELECT = "System.sp_DocumentTypeSelect";
        private const string PROC_DOCUMENT_TYPE_UPDATE = "System.sp_DocumentTypeUpdate";

        private const string PROC_DOCUMENT_TAG_SELECT = "Clinical.sp_TagsSelect";
        private const string PROC_DOCUMENT_TAG_INSERT = "Clinical.sp_TagsInsert";
        private const string PROC_DOCUMENT_TAG_DELETE = "Clinical.sp_TagsDelete";
        private const string PROC_DOCUMENT_TAG_UPDATE = "Clinical.sp_TagsUpdate";
        private const string PROC_DOCUMENT_TAG_BY_NAME = "Patient.sp_GetTagDocumentByName";

        private const string PROC_DOCUMENT_ACTIVITY_LOG = "Patient.sp_DocumentActivityLogSelect";
        #endregion

        #region Parameters
        private const string PARM_DOC_ID = "@DocId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_DOC_TYPE_ID = "@DocTypeId";
        private const string PARM_BAR_CODE_VALUE = "@BarCodeValue";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_TAG_ID = "@TagId";
        private const string PARM_TAG_NAME = "@Name";
        private const string PARM_TAG_IS_ACTIVE = "@IsActive";
        private const string PARM_TAG_CREATE_BY = "@CreatedBy ";
        private const string PARM_TAG_CREATED_ON = "@CreatedOn";
        private const string PARM_TAG_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_TAG_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_ACTIVITY_DOCID = "@PatDocId";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_FROM_DOS = "@FromDOS";
        private const string PARM_TO_DOS = "@ToDOS";
        private const string PARM_FROM_ENTRY_DATE = "@FromEntryDate";
        private const string PARM_TO_ENTRY_DATE = "@ToEntryDate";
        private const string PARM_ENTERED_BY = "@EnteredBy";
        private const string PARM_ASSIGNED_BY_ID = "@AssignedById";
        private const string PARM_REVIEWED_BY_ID = "@ReviewedById";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_ASSIGNED_TO_ID = "@AssignedToId";
        private const string PARM_FROM_EXPIRY = "@FromExpiry";
        private const string PARM_TO_EXPIRY = "@ToExpiry";
        private const string PARM_IS_RECENT_DOC = "@IsRecentDocument";
        private const string PARM_DOC_PRIORITY_ID = "@DocPriorityID";
        //private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        //private const string PARM_Page_NUMBER = "@PageNumber";
        //private const string PARM_ROWS_P_PAGE = "@RowspPage";
        //private const string PARM_RECORD_COUNT = "@RecordCount";
        //private const string PARM_PATIENT_IMAGE = "@PatientImage";

        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSDocument ds, Boolean IsInsert, string Type = "Document")
        {
            if (Type == "Document")
            {
                dbManager.CreateParameters(12);

                if (IsInsert == true)
                    dbManager.AddParameters(0, PARM_DOC_ID, ds.Documents.DocIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
                else
                    dbManager.AddParameters(0, PARM_DOC_ID, ds.Documents.DocIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ds.Documents.ShortNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Documents.DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_DOC_TYPE_ID, ds.Documents.DocTypeIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(4, PARM_BAR_CODE_VALUE, ds.Documents.BarCodeValueColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_ENTITY_ID, ds.Documents.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.Documents.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(7, PARM_CREATED_BY, ds.Documents.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_CREATED_ON, ds.Documents.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.Documents.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.Documents.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(11, PARM_ERROR_MESSAGE, ds.Documents.ErrorMessageColumn.ColumnName, DbType.String);
            }
            else if (Type == "DocumentType")
            {
                dbManager.CreateParameters(10);
                if (IsInsert == true)
                    dbManager.AddParameters(0, PARM_DOC_TYPE_ID, ds.DocumentType.DoctypeIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
                else
                    dbManager.AddParameters(0, PARM_DOC_TYPE_ID, ds.DocumentType.DoctypeIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ds.DocumentType.ShortNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_DESCRIPTION, ds.DocumentType.DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_ENTITY_ID, ds.DocumentType.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.DocumentType.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(5, PARM_CREATED_BY, ds.DocumentType.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_CREATED_ON, ds.DocumentType.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.DocumentType.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.DocumentType.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(9, PARM_ERROR_MESSAGE, ds.DocumentType.ErrorMessageColumn.ColumnName, DbType.String);

            }


        }
        #endregion

        #region Documents

        #region "Insert, delete, update and get Documents using dataset Functions"
        /// <summary>
        /// Loads the Documents.
        /// </summary>
        /// <param name="DocId">The Document identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSDocument LoadDocument(int DocId, string ShortName, int DocTypeId, string IsActive, string EntityId, string Description, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (IsActive == "")
                    IsActive = null;
                if (EntityId == "")
                    EntityId = null;
                dbManager.Open();
                dbManager.CreateParameters(10);
                if (DocId == 0)
                    dbManager.AddParameters(0, PARM_DOC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DOC_ID, DocId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                if (DocTypeId == 0)
                    dbManager.AddParameters(2, PARM_DOC_TYPE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_DOC_TYPE_ID, DocTypeId);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(4, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(5, PARM_DESCRIPTION, Description);

                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.Documents.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENTS_SELECT, ds, ds.Documents.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::LoadDocument", PROC_DOCUMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSDocument FillDocument(int DocId)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (DocId == 0)
                    dbManager.AddParameters(0, PARM_DOC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DOC_ID, DocId);

                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENTS_SELECT, ds, ds.Documents.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::FillDocument", PROC_DOCUMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSDocument UpdateDocument(DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Documents.GetChanges();
                ds = (DSDocument)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_DOCUMENTS_UPDATE, ds, ds.Documents.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Documents.Rows[0][ds.Documents.DocIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::UpdateDocument", PROC_DOCUMENTS_UPDATE, ex);
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

        /// <summary>
        /// Deletes the Document.
        /// </summary>
        /// <param name="DocId">The Document identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteDocument(string DocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSDocument ds = LoadDocument(Convert.ToInt32(DocId), null, 0, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Documents;
                dbManager.AddParameters(0, PARM_DOC_ID, DocId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DOCUMENTS_DELETE).ToString();

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Documents.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Documents.Rows[0][ds.Documents.DocIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::DeleteDocument", PROC_DOCUMENTS_DELETE, ex);
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
        /// Inserts the Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSDocument InsertDocument(DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Documents.GetChanges();
                ds = (DSDocument)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DOCUMENTS_INSERT, ds, ds.Documents.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Documents.Rows[0][ds.Documents.DocIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::InsertDocument", PROC_DOCUMENTS_INSERT, ex);
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


        public List<DocumentActivity> LoadDocumentActivity(Int64 DocId, int PageNumber, int RowsPerPage)
        {
            List<DocumentActivity> obj = new List<DocumentActivity>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<DocumentActivity> dosmodel = new List<DocumentActivity>();
                DocumentActivity model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_ACTIVITY_DOCID, DocId));
                parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, PageNumber));
                parameters.Add(new SqlParameter(PARM_ROWSP_PAGE, RowsPerPage));
                parameters.Add(new SqlParameter(PARM_RECORD_COUNT, 1));


                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_DOCUMENT_ACTIVITY_LOG, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentActivity();
                        model.DocumentActivityLogId = MDVUtility.ToStr(reader["DocumentActivityLogId"]);
                        model.ActionName = MDVUtility.ToStr(reader["ActionName"]);
                        model.UserName = MDVUtility.ToStr(reader["UserName"]);
                        model.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                        model.RecordCount = MDVUtility.ToStr(reader["RecordCount"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadDocumentActivity", PROC_DOCUMENT_ACTIVITY_LOG, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public List<DocumentTag> GetDocumentTags(Int64 TagId, int PageNumber, int RowsPerPage)
        {
            List<DocumentTag> obj = new List<DocumentTag>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<DocumentTag> dosmodel = new List<DocumentTag>();
                DocumentTag model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_TAG_ID, null));
                parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, PageNumber));
                parameters.Add(new SqlParameter(PARM_ROWSP_PAGE, RowsPerPage));
                parameters.Add(new SqlParameter(PARM_RECORD_COUNT, 1));


                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_DOCUMENT_TAG_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentTag();
                        model.TagId = MDVUtility.ToStr(reader["TagId"]);
                        model.Name = MDVUtility.ToStr(reader["Name"]);
                        model.IsActive = MDVUtility.ToBool(reader["IsActive"]);
                        model.RecordCount = MDVUtility.ToStr(reader["RecordCount"]);
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::GetPatientVisitDOS", PROC_DOCUMENT_TAG_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        private void ParamsDocumentsTag(IDBManager dbManager, DocumentTag model, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_TAG_ID, model.TagId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_TAG_NAME, model.Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(3, PARM_TAG_CREATE_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(4, PARM_TAG_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(5, PARM_TAG_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(6, PARM_TAG_MODIFIED_ON, DateTime.Now);
            }
            else
            {
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_TAG_ID, model.TagId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(2, PARM_TAG_NAME, model.Name);
                dbManager.AddParameters(3, PARM_TAG_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(4, PARM_TAG_MODIFIED_ON, DateTime.Now);
            }

        }


        public string InsertNewTagDocument(DocumentTag model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try

            {
                dbManager.Open();
                string retunVal = "";
                dbManager.Open();
                ParamsDocumentsTag(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DOCUMENT_TAG_INSERT));
                return retunVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::Clinical.sp_TagsInsert", PROC_DOCUMENT_TAG_INSERT, ex);
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

        public string DeleteTagDocuments(string TagID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_TAG_ID, TagID);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DOCUMENT_TAG_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::DeleteTagDocuments", PROC_DOCUMENT_TAG_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<DocumentTag> GetTagDocumentsByName(string name)
        {
            List<DocumentTag> obj = new List<DocumentTag>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<DocumentTag> dosmodel = new List<DocumentTag>();
                DocumentTag model = null;
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_TAG_NAME, name));
                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_DOCUMENT_TAG_BY_NAME, parameters))
                {
                    while (reader.Read())
                    {
                        model = new DocumentTag();
                        model.TagId = MDVUtility.ToStr(reader["TagId"]);
                        model.Name = MDVUtility.ToStr(reader["Name"]);
                        
                        
                        dosmodel.Add(model);
                    }
                }

                return dosmodel;
                
               
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::GetTagDocumentsByName", PROC_DOCUMENT_TAG_BY_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string ActiveInActiveTagDocuments(DocumentTag model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string retunVal = "";
                dbManager.Open();
                ParamsDocumentsTag(dbManager, model, false);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DOCUMENT_TAG_UPDATE));
                return retunVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::ActiveInActiveTagDocuments", PROC_DOCUMENT_TAG_UPDATE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #endregion

        #region Document Type

        #region "Insert, delete, update and get Document Type using dataset Functions"
        /// <summary>
        /// Loads the Document Type.
        /// </summary>
        /// <param name="DocId">The Document identifier.</param>
        /// <returns></returns>
        public DSDocument LoadDocumentType(int DocTypeId, string ShortName, string Description, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (IsActive == "")
                    IsActive = null;
                if (EntityId == "")
                    EntityId = null;
                dbManager.Open();
                dbManager.CreateParameters(9);
                if (DocTypeId == 0)
                    dbManager.AddParameters(0, PARM_DOC_TYPE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DOC_TYPE_ID, DocTypeId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(4, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, EntityId);


                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.DocumentType.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_TYPE_SELECT, ds, ds.DocumentType.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::LoadDocumentType", PROC_DOCUMENT_TYPE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Fills the type of the document.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="DocTypeId">The document type identifier.</param>
        /// <returns></returns>
        public DSDocument FillDocumentType(int DocId, int DocTypeId)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);

                if (DocTypeId == 0)
                    dbManager.AddParameters(0, PARM_DOC_TYPE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DOC_TYPE_ID, DocTypeId);
                if (DocId == 0)
                    dbManager.AddParameters(1, PARM_DOC_ID, null);
                else
                    dbManager.AddParameters(1, PARM_DOC_ID, DocId);

                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_TYPE_SELECT, ds, ds.DocumentType.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::FillDocumentType", PROC_DOCUMENT_TYPE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the type of the document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSDocument UpdateDocumentType(DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false, "DocumentType");
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.DocumentType.GetChanges();
                ds = (DSDocument)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_TYPE_UPDATE, ds, ds.DocumentType.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.DocumentType.Rows[0][ds.DocumentType.DoctypeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::UpdateDocumentType", PROC_DOCUMENT_TYPE_UPDATE, ex);
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


        /// <summary>
        /// Deletes the type of the document.
        /// </summary>
        /// <param name="DocTypeId">The document type identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteDocumentType(string DocTypeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //dbManager.CreateParameters(2);
                //DSDocument ds = LoadDocumentType(Convert.ToInt32(DocTypeId),null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.DocumentType;
                dbManager.AddParameters(0, PARM_DOC_TYPE_ID, DocTypeId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                // dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DOCUMENT_TYPE_DELETE).ToString();

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //        if (dtTemp != null && ds.DocumentType.Rows.Count > 0)
                //        {
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.DocumentType.Rows[0][ds.DocumentType.DoctypeIdColumn].ToString(), null, "", false, false, true);
                //            dsDBAudit.AcceptChanges();
                //        }
                //    }


                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::DeleteDocumentType", PROC_DOCUMENT_TYPE_DELETE, ex);
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
        /// Inserts the type of the document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSDocument InsertDocumentType(DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true, "DocumentType");
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.DocumentType.GetChanges();
                ds = (DSDocument)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_TYPE_INSERT, ds, ds.DocumentType.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.DocumentType.Rows[0][ds.DocumentType.DoctypeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::InsertDocumentType", PROC_DOCUMENT_TYPE_INSERT, ex);
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

        #endregion

        #region "Lookups"
        public DSDocumentLookup LookupDocument(string PatientId)
        {
            DSDocumentLookup ds = new DSDocumentLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (!string.IsNullOrWhiteSpace(PatientId))
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, "0");


                ds = (DSDocumentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_LOOKUP, ds, ds.Documents.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::LookupDocument", PROC_DOCUMENT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSDocumentLookup GetPatientDocument(Int64 PatientId, DateTime? FromDOS = null, DateTime? ToDOS = null, DateTime? FromEntry = null, DateTime? ToEntry = null,
            string AccountNumber = null, int AssignedToReviewedID = 0, string EnteredBy = null, string DocPriority = null, DateTime? FromExpiry = null, DateTime? ToExpiry = null,
            long tagId = 0, bool IsrecentCheck = false)
        {
            DSDocumentLookup ds = new DSDocumentLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(15);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PatientId != 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, "0");
                dbManager.AddParameters(3, PARM_FROM_DOS, FromDOS);
                dbManager.AddParameters(4, PARM_TO_DOS, ToDOS);
                dbManager.AddParameters(5, PARM_FROM_ENTRY_DATE, FromEntry);
                dbManager.AddParameters(6, PARM_TO_ENTRY_DATE, ToEntry);
                dbManager.AddParameters(7, PARM_ACCOUNT_NUMBER, AccountNumber);
                if (AssignedToReviewedID == 0)
                    dbManager.AddParameters(8, PARM_ASSIGNED_TO_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ASSIGNED_TO_ID, AssignedToReviewedID);
                if(EnteredBy=="")
                dbManager.AddParameters(9, PARM_ENTERED_BY, null);
                else
                    dbManager.AddParameters(9, PARM_ENTERED_BY, EnteredBy);
                if (string.IsNullOrEmpty(DocPriority))
                dbManager.AddParameters(10, PARM_DOC_PRIORITY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_DOC_PRIORITY_ID, DocPriority);
                dbManager.AddParameters(11, PARM_FROM_EXPIRY, FromExpiry);
                dbManager.AddParameters(12, PARM_TO_EXPIRY, ToExpiry);
                if(tagId==0)
                dbManager.AddParameters(13, PARM_TAG_ID, null);
                else
                    dbManager.AddParameters(13, PARM_TAG_ID, tagId);
                dbManager.AddParameters(14, PARM_IS_RECENT_DOC, IsrecentCheck);
                var tableNames = new List<string>
                {
                    ds.Documents.TableName,
                    ds.FolderDocument.TableName
                };

                ds = (DSDocumentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_FOLDER_DETAIL, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::GetPatientDocument", PROC_DOCUMENT_FOLDER_DETAIL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        

        public DSDocumentLookup LookupFolders(string PatientId)
        {
            DSDocumentLookup ds = new DSDocumentLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (!string.IsNullOrEmpty(PatientId))
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, "0");


                ds = (DSDocumentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Folders_LOOKUP, ds, ds.Documents.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::LookupDocument", PROC_Folders_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //<summary>
        // the LookupMessageTypes.
        //</summary>
        //<returns>DSMessageLookup.</returns>
        public DSDocumentLookup LookupDocumentType()
        {
            DSDocumentLookup ds = new DSDocumentLookup();
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
                ds = (DSDocumentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DOCUMENT_TYPE_LOOKUP, ds, ds.DocumentType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::LookupDocumentType", PROC_DOCUMENT_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientRepresentativeLookupModel> LookupDocumentProvider(long PatientId)
        {
            List<PatientRepresentativeLookupModel> listDocumentProvider = new List<PatientRepresentativeLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_REPRESENTATIVE_LOOKUP);
                PatientRepresentativeLookupModel model = null;
                while (reader.Read())
                {
                    model = new PatientRepresentativeLookupModel();
                    model.Id = reader["Id"].ToString();
                    model.Name = reader["Name"].ToString();

                    listDocumentProvider.Add(model);
                }

                return listDocumentProvider;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDocument::LookupDocumentProvider", PROC_PATIENT_REPRESENTATIVE_LOOKUP, ex);
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
    }
}
