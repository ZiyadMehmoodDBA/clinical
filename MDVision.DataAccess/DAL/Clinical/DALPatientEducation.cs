using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Medical.PatientEducation;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Data.SqlClient;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Clinical.Notes;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPatientEducation
    {
        #region Variable
        
        #endregion

        public DALPatientEducation()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        public DALPatientEducation(SharedVariable SharedVariable)
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

        #region "Stored Procedure Names"
        private const string PROC_PATIENT_EDUCATION_SELECT = "[Clinical].[sp_PatientEducation_Select]";
        private const string PROC_PATIENT_EDUCATION_DELETE = "[Clinical].[sp_PatientEducation_Delete]";
        private const string PROC_PATIENT_EDUCATION_INSERT = "[Clinical].[sp_PatientEducation_Insert]";
        private const string PROC_PATIENT_EDUCATION_Lookup = "[Clinical].[sp_PatientEducation_Lookup]";
        private const string PROC_PATIENT_EDUCATION_SOAP = "[Clinical].[sp_GetPatientEducationsoap]";
        private const string PROC_GET_LATEST_PATIENTEDUCATION_BY_PATIENT = "Clinical.sp_GetLatestPatientEducationByPatientId";
        private const string PROC_DETACH_PATIENTEDUCATION_FROM_NOTES = "Clinical.sp_DetachPatientEducationFromNotes";
        private const string PROC_ATTACH_PATIENTEDUCATION_FROM_NOTES = "Clinical.sp_AttachPatientEducationWithNotes";

        private const string PROC_NOTES_PATIENTEDUCATION_SELECT = "[Clinical].[sp_NotesPatientEducationSelect]";


        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_EDUCATION_ID = "@PatientEducationId";
        private const string PARM_PATDOC_ID = "@PatDocId";
        private const string PARM_DOC_TYPE = "@DocType";
        private const string PARM_PATIENT_ID = "@PatientID";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_FILE_TYPE = "@FileType";
        private const string PARM_Document_Name = "@DocumentName";

        private const string PARM_FILE_STREAM = "@FileStream";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_PAGES = "@Pages";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_URL_PATIENT_EDUCATION = "@UrlPatientEducation";

        #endregion

        #region "Insert, delete, Lookup and get Patient Education using dataset Functions"

        public DSPatientEducation LookupClinical_PatientEducation(string DocumentName)
        {
            DSPatientEducation ds = new DSPatientEducation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_Document_Name, DocumentName);

                ds = (DSPatientEducation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_EDUCATION_Lookup, ds, ds.PatientEducation.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::LookupClinical_PatientEducation", PROC_PATIENT_EDUCATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientEducation loadClinical_PatientEducation_Obsolete(long PatientID, string DocType, int PageNumber, int RowsPerPage, long NoteId)
        {
            DSPatientEducation ds = new DSPatientEducation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);
                dbManager.AddParameters(1, PARM_DOC_TYPE, DocType);
                dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PatientEducation.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatientEducation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_EDUCATION_SELECT, ds, ds.PatientEducation.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::loadClinical_PatientEducation", PROC_PATIENT_EDUCATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MDVision.Model.Clinical.Medical.PatientEducation.PatientEducation> loadClinical_PatientEducation(long PatientID, string DocType, int PageNumber, int RowsPerPage, long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_PATIENT_ID, PatientID);
                dbManager.AddParameters(PARM_DOC_TYPE, DocType);
                dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(PARM_RECORD_COUNT, 0, DbType.Int64, ParamDirection.Output);

                List<MDVision.Model.Clinical.Medical.PatientEducation.PatientEducation> patientEducationList = dbManager.ExecuteReaderMapper<MDVision.Model.Clinical.Medical.PatientEducation.PatientEducation>(PROC_PATIENT_EDUCATION_SELECT);

                return patientEducationList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::loadClinical_PatientEducation", PROC_PATIENT_EDUCATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        private void CreateParameters(IDBManager dbManager, DSPatientEducation ds)
        {
            dbManager.CreateParameters(13);

            dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, ds.PatientEducation.PatEducationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PATDOC_ID, ds.PatientEducation.PatDocIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientEducation.PatientIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_DOC_TYPE, ds.PatientEducation.DocTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_FILE_TYPE, ds.PatientEducation.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_Document_Name, ds.PatientEducation.DocumentNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_FILE_STREAM, ds.PatientEducation.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(7, PARM_PAGES, ds.PatientEducation.PagesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_COMMENTS, ds.PatientEducation.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.PatientEducation.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.PatientEducation.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_NOTE_ID, ds.PatientEducation.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_URL_PATIENT_EDUCATION, ds.PatientEducation.URLColumn.ColumnName, DbType.String);
        }

        public DSPatientEducation InsertClinical_PatientEducation(DSPatientEducation ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds);
                ds = (DSPatientEducation)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_EDUCATION_INSERT, ds, ds.PatientEducation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::insertClinical_PatientEducation", PROC_PATIENT_EDUCATION_INSERT, ex);
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
        
        public string DeleteClinical_PatientEducation(long PatientEducationId, int PatDocID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, PatientEducationId);
                dbManager.AddParameters(1, PARM_PATDOC_ID, PatDocID);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_EDUCATION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::DeleteClinical_PatientEducation", PROC_PATIENT_EDUCATION_DELETE, ex);
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

        public DSPatientEducation getLatestPatientEducationByPatientId(long PatientId)
        {
            DSPatientEducation ds = new DSPatientEducation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                ds = (DSPatientEducation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_PATIENTEDUCATION_BY_PATIENT, ds, ds.PatientEducation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::getLatestProblemListByPatientId", PROC_GET_LATEST_PATIENTEDUCATION_BY_PATIENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatientEducation getPatientEducationSOAP(Int64 PatientId, string PatEducationId, Int64 NoteId)
        {
            DSPatientEducation ds = new DSPatientEducation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(PatEducationId))
                {
                    dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, PatEducationId);
                }
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);
                ds = (DSPatientEducation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_EDUCATION_SOAP, ds, ds.PatientEducation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::getLatestProblemListByPatientId", PROC_GET_LATEST_PATIENTEDUCATION_BY_PATIENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }      

        public List<PatientEducationModel> GetPatientEducationByPatientId(long PatientId)
        {
            List<PatientEducationModel> modelList = new List<PatientEducationModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();                
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
              
                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_PATIENTEDUCATION_BY_PATIENT, parameters))
                {
                    while (reader.Read())
                    {
                        PatientEducationModel model = new PatientEducationModel();

                        var properties = typeof(PatientEducationModel).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        modelList.Add(model);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::GetPatientEducationByPatientId", PARM_PATIENT_EDUCATION_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string detachPatientEducationFromNotes(string patientEducationId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(patientEducationId))
                {
                    dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, patientEducationId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PATIENTEDUCATION_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::detachPatientEducationFromNotes", PROC_DETACH_PATIENTEDUCATION_FROM_NOTES, ex);
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

        public DSPatientEducation attachPatientEducationWithNotes(string PatientEducationId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSPatientEducation ds = new DSPatientEducation();

                dbManager.Open();

                dbManager.CreateParameters(6);
                if (string.IsNullOrEmpty(PatientEducationId))
                {
                    dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_EDUCATION_ID, PatientEducationId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(4, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(5, PARM_CREATED_ON, DateTime.Now);


                ds = (DSPatientEducation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PATIENTEDUCATION_FROM_NOTES, ds, ds.PatientEducation.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::attachPatientEducationWithNotes", PROC_ATTACH_PATIENTEDUCATION_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        #endregion

        #region Legacy Notes

        public List<MDVision.Model.Clinical.LegacyNotes.PatientEducation> NotesPatientEducationSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MDVision.Model.Clinical.LegacyNotes.PatientEducation> objList_PatientEducation = new List<MDVision.Model.Clinical.LegacyNotes.PatientEducation>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_PATIENTEDUCATION_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        MDVision.Model.Clinical.LegacyNotes.PatientEducation model = new MDVision.Model.Clinical.LegacyNotes.PatientEducation();
                        var properties = typeof(MDVision.Model.Clinical.LegacyNotes.PatientEducation).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_PatientEducation.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEducation::NotesPatientEducationSelect", PROC_NOTES_PATIENTEDUCATION_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_PatientEducation;
        }

        #endregion Legacy Notes

    }
}