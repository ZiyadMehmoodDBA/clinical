using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Document
{
    public class DALMedicalDocument
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALDocument"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALMedicalDocument()
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
       
        private const string PROC_MEDICAL_DOCUMENTS_INSERT = "Patient.sp_MedicalDocumentsInsert";
        private const string PROC_MEDICAL_DOCUMENTS_DELETE = "Patient.sp_MedicalDocumentsDelete";
        private const string PROC_MEDICAL_DOCUMENTS_SELECT = "Patient.sp_MedicalDocumentsSelect";
        private const string PROC_MEDICAL_DOCUMENTS_UPDATE = "Patient.sp_MedicalDocumentsUpdate";
        #endregion

        #region Parameters
        private const string PARM_MEDICAL_DOC_ID = "@MedicalDocId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_TRANSITION_ID = "@TransitionId";
        private const string PARM_FILE_TYPE = "@FileType";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_FILE_STREAM = "@FileStream";
        private const string PARM_PAGES = "@Pages";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
               #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSDocument ds, Boolean IsInsert)
        {
            
                dbManager.CreateParameters(12);

                if (IsInsert == true)
                    dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, ds.MedicalDocuments.MedicalDocIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
                else
                    dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, ds.MedicalDocuments.MedicalDocIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(1, PARM_PATIENT_ID, ds.MedicalDocuments.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_TRANSITION_ID, ds.MedicalDocuments.TransitionIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(3, PARM_FILE_TYPE, ds.MedicalDocuments.FileTypeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_FILE_PATH, ds.MedicalDocuments.FilePathColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_FILE_STREAM, ds.MedicalDocuments.FileStreamColumn.ColumnName, DbType.Binary);
                dbManager.AddParameters(6, PARM_PAGES, ds.MedicalDocuments.PagesColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.Documents.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(8, PARM_CREATED_BY, ds.Documents.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, PARM_CREATED_ON, ds.Documents.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.Documents.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.Documents.ModifiedOnColumn.ColumnName, DbType.DateTime);
               
           


        }
        #endregion

        #region "Insert, delete, update and get MedicalDocuments using dataset Functions"
        /// <summary>
        /// Loads the Medical Documents.
        /// </summary>
        /// <param name="MedicalDocId">The Document identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSDocument LoadMedicalDocument(int MedicalDocId)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
              
                
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (MedicalDocId == 0)
                    dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, MedicalDocId);
             
              
                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICAL_DOCUMENTS_SELECT, ds, ds.MedicalDocuments.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalDocument::LoadMedicalDocument", PROC_MEDICAL_DOCUMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSDocument FillMedicalDocument(int MedicalDocId)
        {
            DSDocument ds = new DSDocument();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (MedicalDocId == 0)
                    dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, MedicalDocId);

                ds = (DSDocument)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICAL_DOCUMENTS_SELECT, ds, ds.MedicalDocuments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalDocument::FillMedicalDocument", PROC_MEDICAL_DOCUMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the Medical Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSDocument UpdateMedicalDocument(DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSDocument)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MEDICAL_DOCUMENTS_UPDATE, ds, ds.MedicalDocuments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalDocument::UpdateMedicalDocument", PROC_MEDICAL_DOCUMENTS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the Medical Document.
        /// </summary>
        /// <param name="DocId">The Document identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteMedicalDocument(string MedicalDocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MEDICAL_DOC_ID, MedicalDocId);
                
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEDICAL_DOCUMENTS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalDocument::DeleteMedicalDocument", PROC_MEDICAL_DOCUMENTS_DELETE, ex);
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
        /// Inserts the Medical Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSDocument InsertMedicalDocument(DSDocument ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSDocument)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEDICAL_DOCUMENTS_INSERT, ds, ds.MedicalDocuments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalDocument::InsertMedicalDocument", PROC_MEDICAL_DOCUMENTS_INSERT, ex);
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
