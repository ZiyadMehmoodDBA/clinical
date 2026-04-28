// Created By:  Muhammad Ahmad Imran
// Created Date: 07/03/2016
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

using System.Data.SqlClient;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;


namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPatientTemplateLetter
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"


        private const string PROC_TEMPLATE_LETTER_NAME = "Clinical.sp_Template_LetterNameLookup";
        private const string PROC_PATIENT_LETTER_SELECT = "Clinical.sp_Patient_LetterSelect";
        private const string PROC_GET_LETTER_CONTENT = "Clinical.sp_PopulateTemplateLetterData";
        private const string PROC_PATIENT_LETTER_DELETE = "Clinical.sp_Patient_LetterDelete";
        private const string PROC_PATIENT_LETTER_UPDATE = "Clinical.sp_Patient_LetterUpdate";
        private const string PROC_PATIENT_LETTER_INSERT = "Clinical.sp_Patient_LetterInsert";
        private const string PROC_PATIENT_LETTER_NOTE_SELECT = "Clinical.sp_PatientLetterNoteSelect";






        #endregion

        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_CATEGORY_ID = "@CategoryId";
        private const string PARM_TAG_CATEGORY_ID = "@TagCategoryId";
        private const string PARM_TAG_CATEGORYNAME_ID = "@TagCategoryNameId";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_pPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_TEMPLATE_CONTENT = "@TemplateContent";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_TAG_IDS = "@TagIds";
        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_TEMPLATE_LETTER_ID = "@TemplateLetterId";
        private const string PARM_PATIENT_LETTER_ID = "@PatientLetterId";
        private const string PARM_MODE = "@Mode";
        private const string PARM_STATUS = "@Status";
        private const string PARM_LETTER_CONTENT = "@LetterContent";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_Base64_Content = "@Base64_Content";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_URL = "@Url";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_VISIT_DATE = "@VisitDate";
        private const string PARM_VISIT_TIME = "@VisitTime";
        private const string PARM_VISIT_DATEFROM = "@VisitDateFrom";
        private const string PARM_VISIT_DATETO = "@VisitDateTo";
        private const string PARM_NOTES_ID = "@NotesId";

        #endregion

        #region Constructors
        public DALPatientTemplateLetter()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        public DALPatientTemplateLetter(SharedVariable SharedVariable)
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : To Create Params for insert and update Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParametersForInsertPatientLetter(IDBManager dbManager, DSLetter ds, Boolean IsInsert)
        {
            
            if (IsInsert == true)
            {
                dbManager.CreateParameters(16);
                dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, ds.PatientTemplateLetter.Patient_Letter_IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(13);
                dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, ds.PatientTemplateLetter.Patient_Letter_IdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARM_TEMPLATE_LETTER_ID, ds.PatientTemplateLetter.Template_Letter_IdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientTemplateLetter.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_STATUS, ds.PatientTemplateLetter.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PatientTemplateLetter.IsactiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_LETTER_CONTENT, ds.PatientTemplateLetter.PatientLetterContentColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_CREATED_BY, ds.PatientTemplateLetter.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.PatientTemplateLetter.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.PatientTemplateLetter.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFY_ON, ds.PatientTemplateLetter.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_Base64_Content, ds.PatientTemplateLetter.Base64ContentColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(11, PARM_URL, ds.PatientTemplateLetter.UrlColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_FILE_PATH, ds.PatientTemplateLetter.FilePathColumn.ColumnName, DbType.String);
            if (IsInsert)
            {
                dbManager.AddParameters(13, PARM_VISIT_DATE, ds.PatientTemplateLetter.VisitDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(14, PARM_VISIT_TIME, ds.PatientTemplateLetter.VisitTimeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(15, PARM_NOTES_ID, ds.PatientTemplateLetter.NotesIdColumn.ColumnName, DbType.Int64);
            }
        }


        #endregion

        #region "Patient Template Letter LookUps"
        public DSClinicalLetterTemplateLookup GetLetterTemplatesName(long providerId)
        {
            DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //Start 11-10-2017 Edit By Humaira Yousaf IMP-1189
                dbManager.CreateParameters(3);
                if (providerId > 0)
                {
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, DBNull.Value);
                }
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                //End 11-10-2017 Edit By Humaira Yousaf IMP-1189
                ds = (DSClinicalLetterTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_LETTER_NAME, ds, ds.Template_Letter_Name.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientTemplateLetter::GetLetterTemplatesName", PROC_TEMPLATE_LETTER_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region CRUD Operations
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To Load Patient Letter
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="IsActive"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        public DSLetter loadPatientTemplateLetter(long Patient_Letter_Id,long patientId, bool IsActive, string name, string description, int categoryId, int pageNo, int rpp, string visitDateFrom , string visitDateTo, long NotesId = 0)
        {
            DSLetter ds = new DSLetter();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                /*if (IsActive == "")
                    IsActive = null;*/
                dbManager.Open();
                dbManager.CreateParameters(12);
                if (Patient_Letter_Id <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, Patient_Letter_Id);
                if (patientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                //----------------
                if (string.IsNullOrEmpty(name))
                    dbManager.AddParameters(3, PARM_NAME, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_NAME, name);

                if (string.IsNullOrEmpty(description))
                    dbManager.AddParameters(4, PARM_DESCRIPTION, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_DESCRIPTION, description);

                if (categoryId <= 0)
                    dbManager.AddParameters(5, PARM_CATEGORY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_CATEGORY_ID, categoryId);
                //----------------
                if (pageNo <= 0) { dbManager.AddParameters(6, PARM_PAGE_NUMBER, DBNull.Value); } else { dbManager.AddParameters(6, PARM_PAGE_NUMBER, pageNo); }
                if (rpp <= 0) { dbManager.AddParameters(7, PARM_ROWS_pPAGE, DBNull.Value); } else { dbManager.AddParameters(7, PARM_ROWS_pPAGE, rpp); }

                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.PatientTemplateLetter.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (!string.IsNullOrEmpty(visitDateFrom))
                {
                    dbManager.AddParameters(9, PARM_VISIT_DATEFROM, visitDateFrom);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_VISIT_DATEFROM, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(visitDateTo))
                {
                    dbManager.AddParameters(10, PARM_VISIT_DATETO, visitDateTo);
                }
                else
                {
                    dbManager.AddParameters(10, PARM_VISIT_DATETO, DBNull.Value);
                }
                if (NotesId > 0)
                    dbManager.AddParameters(11, PARM_NOTES_ID, NotesId);
                else
                    dbManager.AddParameters(11, PARM_NOTES_ID, DBNull.Value);

                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_LETTER_SELECT, ds, ds.PatientTemplateLetter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientTemplateLetter::loadPatientTemplateLetter", PROC_PATIENT_LETTER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientLetter> NotesPatientLetterSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<PatientLetter> objList = new List<PatientLetter>();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_NOTES_ID, objCommonSearch.NotesId));
                reader = (SqlDataReader)dbManager.ExecuteReader(PROC_PATIENT_LETTER_NOTE_SELECT, parameters);
                PatientLetter model = null;
                while (reader.Read())
                {
                    model = new PatientLetter();
                    model.LetterId = !string.IsNullOrEmpty(reader["LetterId"].ToString()) ? reader["LetterId"].ToString() : "";
                    model.Name = !string.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.NotesId = !string.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    model.SoapText = !string.IsNullOrEmpty(reader["SoapText"].ToString()) ? reader["SoapText"].ToString() : "";
                    objList.Add(model);
                }
                return objList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientTemplateLetter::NotesPatientLetterSelect", PROC_PATIENT_LETTER_NOTE_SELECT, ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
            return objList;
        }
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 08/03/2016
        //OverView: Methods "GetPatientLetterContent" for Get Patient specific Letter content 

        public DSLetter GetPatientLetterContent(long patientId, long TemplateLetterId, string Mode, long PatientLetterId, long ProviderId, long LabOrderResultId = 0, string LOINC= null)
        {
            DSLetter ds = new DSLetter();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                /*if (IsActive == "")
                    IsActive = null;*/
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (patientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (TemplateLetterId <= 0)
                    dbManager.AddParameters(1, PARM_TEMPLATE_LETTER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_LETTER_ID, TemplateLetterId);
                if (PatientLetterId <= 0)
                    dbManager.AddParameters(2, PARM_PATIENT_LETTER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_LETTER_ID, PatientLetterId);

                dbManager.AddParameters(3, PARM_MODE, Mode == "Add" ? 0 : 1);
                var UserId = MDVSession.Current.AppUserId;
                dbManager.AddParameters(4, PARM_USER_ID, UserId);
  
                if (ProviderId <= 0)
                    dbManager.AddParameters(5, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_PROVIDER_ID, ProviderId);

                if (LabOrderResultId != 0)
                {
                    dbManager.AddParameters(6, "@LabOrderResultId", LabOrderResultId);
                }
                else
                {
                    dbManager.AddParameters(6, "@LabOrderResultId", null);
                }

                    dbManager.AddParameters(7, "@LOINC", LOINC);
  
                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LETTER_CONTENT, ds, ds.PatientTemplateLetter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientTemplateLetter::GetPatientLetterContent", PROC_GET_LETTER_CONTENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To Delete Patient Letter
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="PatientLetterId"></param>
        /// <returns></returns>
        public string deletePatientLetter(string PatientLetterId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, PatientLetterId);
                if (string.IsNullOrEmpty(PatientLetterId))
                    dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_LETTER_ID, PatientLetterId);

                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_LETTER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientTemplateLetter::deletePatientLetter", PROC_PATIENT_LETTER_DELETE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : To Update Patient Letter
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSLetter UpdatePatientLetter(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertPatientLetter(dbManager, ds, false);
                ds = (DSLetter)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_LETTER_UPDATE, ds, ds.PatientTemplateLetter.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientTemplateLetter::UpdatePatientLetter", PROC_PATIENT_LETTER_UPDATE, ex);
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
        // Created Date: 08/03/2016
        //OverView: Methods "InsertPatientLetter" for save Patient letter 
        public DSLetter InsertPatientLetter(DSLetter ds) 
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertPatientLetter(dbManager, ds, true);
                ds = (DSLetter)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_LETTER_INSERT, ds, ds.PatientTemplateLetter.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLRcopia::InsertTemplateLetter", PROC_PATIENT_LETTER_INSERT, ex);
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
    }
}
