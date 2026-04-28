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
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Model;
using MDVision.Common.Utilities;
using MDVision.Model.Admin.Provider;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPatientCustomForm
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CUSTOM_FORM_NAME = "Clinical.sp_CustomFormLookup";
        private const string PROC_CUSTOM_FORMS_SEARCH = "Clinical.sp_Patient_CustomFormSearch";
        private const string PROC_CUSTOM_FORM_FILL = "Clinical.sp_Patient_CustomFormFill";
        private const string PROC_CUSTOM_FORM_DELETE = "Clinical.sp_Patient_CustomFormDelete";
        private const string PROC_CUSTOM_FORM_UPDATE = "Clinical.sp_Patient_CustomFormUpdate";
        private const string PROC_CUSTOM_FORM_INSERT = "Clinical.sp_Patient_CustomFormInsert";
        private const string PROC_CUSTOM_FORMS_ACTIVE_INACTIVE = "Clinical.sp_Patient_CustomFormActiveInActive";
        private const string PROC_GET_PATIENT_DOCUMENTID = "Clinical.sp_GetPatientDocumentId";
        private const string PROC_GET_PROVIDER_CPTS = "Clinical.sp_GetProviderCPTsForCustomForm";
        #endregion

        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_pPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CUSTOM_FORM_ID = "@CustomFormId";
        private const string PARM_PATIENT_CUSTOM_FORM_ID = "@PatientCustomFormId";
        private const string PARM_MODE = "@Mode";
        private const string PARM_CUSTOM_FORM_CONTENT = "@CustomFormContent";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_Base64_Content = "@Base64_Content";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_CUSTOM_FORM_NAME = "@CustomFormName";
        private const string PARM_CUSTOM_URL = "@Url";
        private const string PARM_IS_SIGNED = "@IsSigned";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        #endregion

        #region Constructors
        public DALPatientCustomForm()
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

        #region "Support Functions"



        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To Create Params for insert and update Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void ParamsCustomFormInsert(IDBManager dbManager, PatientCustomFormModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);
            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, model.PatientCustomFormId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, true);
            }
            else
            {
                dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, model.PatientCustomFormId);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, model.IsActive == "1" ? true : false);
            }

            dbManager.AddParameters(1, PARM_CUSTOM_FORM_ID, model.CustomFormId);
            dbManager.AddParameters(2, PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(4, PARM_CUSTOM_FORM_CONTENT, model.CustomFormHTML);
            dbManager.AddParameters(5, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(6, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(8, PARM_MODIFY_ON, DateTime.Now);
            dbManager.AddParameters(9, PARM_CUSTOM_URL, model.Url);
            dbManager.AddParameters(10, PARM_IS_SIGNED, model.IsSigned);
        }


        #endregion

        #region "Patient Custom Form LookUps"

        public List<CustomFormLookupModel> LookupCustomFormName()
        {
            List<CustomFormLookupModel> listCustomForm = new List<CustomFormLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOM_FORM_NAME);
                CustomFormLookupModel model = null;
                while (reader.Read())
                {
                    model = new CustomFormLookupModel();
                    model.CustomFormId = reader["CustomFormId"].ToString();
                    model.CustomFormName = reader["FormName"].ToString();

                    listCustomForm.Add(model);
                }

                return listCustomForm;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupEthnicityDemographic", PROC_CUSTOM_FORM_NAME, ex);
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

        #region CRUD Operations

        public string InsertPatientCustomForm(PatientCustomFormModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                ParamsCustomFormInsert(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CUSTOM_FORM_INSERT));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLPatientCustomForm::InsertPatientCustomForm", PROC_CUSTOM_FORM_INSERT, ex);
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

        public List<PatientCustomFormModel> GetProviderCPTs(long CustomFormId, long ProviderId)
        {
            PatientCustomFormModel providerCPTsList = new PatientCustomFormModel();
            List<PatientCustomFormModel> listProviderCPTs = new List<PatientCustomFormModel>();
            ProviderCPTs providerCPTsModel = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (CustomFormId == 0)
                    dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, CustomFormId);

                if (ProviderId == 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_PROVIDER_CPTS);

                while (reader.Read())
                {
                    providerCPTsModel = new ProviderCPTs();
                    providerCPTsModel.ProcedureId = !String.IsNullOrEmpty(reader["Procedureid"].ToString()) ? reader["Procedureid"].ToString() : "";
                    providerCPTsModel.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                    providerCPTsModel.CPTCodeDescription = !String.IsNullOrEmpty(reader["CPT_Description"].ToString()) ? reader["CPT_Description"].ToString() : "";
                    providerCPTsModel.ShowCPTCode = !String.IsNullOrEmpty(reader["ShowCPTCode"].ToString()) ? reader["ShowCPTCode"].ToString() : "";

                    providerCPTsList.ProviderCPTsList.Add(providerCPTsModel);
                }
                listProviderCPTs.Add(providerCPTsList);

                return listProviderCPTs;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientCustomForm::GetProviderCPTs", PROC_GET_PROVIDER_CPTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string GetPatientDocumentId(string CustomFormId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, CustomFormId);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PATIENT_DOCUMENTID));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLPatientCustomForm::GetPatientDocumentId", PROC_GET_PATIENT_DOCUMENTID, ex);
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

        public List<PatientCustomFormModel> loadPatientCustomForm(string formName, int? isActive, string patientId, long pageNumber, long rowsPerPage)
        {
            List<PatientCustomFormModel> listCustomForms = new List<PatientCustomFormModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(6);
                if (isActive == null)
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);
                }

                if (pageNumber <= 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage <= 0)
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(3, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(3, PARM_PATIENT_ID, patientId);


                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                if (String.IsNullOrEmpty(formName))
                    dbManager.AddParameters(5, PARM_CUSTOM_FORM_NAME, null);
                else
                    dbManager.AddParameters(5, PARM_CUSTOM_FORM_NAME, formName);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOM_FORMS_SEARCH);
                PatientCustomFormModel model = null;
                while (reader.Read())
                {
                    model = new PatientCustomFormModel();
                    model.PatientCustomFormId = !String.IsNullOrEmpty(reader["PatientCustomFormId"].ToString()) ? reader["PatientCustomFormId"].ToString() : "";
                    model.FormName = !String.IsNullOrEmpty(reader["CustomFormName"].ToString()) ? reader["CustomFormName"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.CustomFormId = !String.IsNullOrEmpty(reader["CustomFormId"].ToString()) ? reader["CustomFormId"].ToString() : "";
                    model.CreatedByName = !String.IsNullOrEmpty(reader["CreatedByName"].ToString()) ? reader["CreatedByName"].ToString() : "";
                    model.ModifiedByName = !String.IsNullOrEmpty(reader["ModifiedByName"].ToString()) ? reader["ModifiedByName"].ToString() : "";
                    model.IsSigned = !String.IsNullOrEmpty(reader["IsSigned"].ToString()) ? reader["IsSigned"].ToString() : "";
                    model.PatDocID = !String.IsNullOrEmpty(reader["PatDocID"].ToString()) ? reader["PatDocID"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadCustomForms", PROC_CUSTOM_FORMS_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deletePatientCustomForm(string formID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, formID);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CUSTOM_FORM_DELETE).ToString();

                if (returnVal != "-1")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::DeleteCustomForm", PROC_CUSTOM_FORM_DELETE, ex);
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

        public string activeInactivePatientCustomForm(string formId, string isActive)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (String.IsNullOrEmpty(formId))
                    dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, formId);
                if (String.IsNullOrEmpty(isActive))
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, isActive);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CUSTOM_FORMS_ACTIVE_INACTIVE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::activeInactiveCustomForm", PROC_CUSTOM_FORMS_ACTIVE_INACTIVE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientCustomFormModel> fillPatientCustomForm(string formId)
        {
            List<PatientCustomFormModel> listCustomForms = new List<PatientCustomFormModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (String.IsNullOrEmpty(formId))
                    dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_CUSTOM_FORM_ID, formId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOM_FORM_FILL);
                PatientCustomFormModel model = null;
                while (reader.Read())
                {
                    model = new PatientCustomFormModel();
                    model.CustomFormHTML = !String.IsNullOrEmpty(reader["CustomFormContent"].ToString()) ? reader["CustomFormContent"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadCustomForms", PROC_CUSTOM_FORM_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdatePatientCustomForm(PatientCustomFormModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                ParamsCustomFormInsert(dbManager, model, false);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CUSTOM_FORM_UPDATE));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLPatientCustomForm::UpdatePatientCustomForm", PROC_CUSTOM_FORM_UPDATE, ex);
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
