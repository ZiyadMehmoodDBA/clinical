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
using MDVision.Model;
using System.Data.SqlClient;
using MDVision.Common.Utilities;
using MDVision.Model.Lookups;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALCustomForm
    {

        #region "Stored Procedure Names"
        private const string PROC_CUSTOM_FORMS_SELECT = "System.sp_CustomFormSelect";
        private const string PROC_CUSTOM_FORM_FILL = "System.sp_CustomFormFill";
        private const string PROC_CUSTOM_FORMS_INSERT = "System.sp_CustomFormInsert";
        private const string PROC_CUSTOM_FORMS_UPDATE = "System.sp_CustomFormUpdate";
        private const string PROC_CUSTOM_FORMS_DELETE = "System.sp_CustomFormDelete";
        private const string PROC_CUSTOM_FORMS_ACTIVE_INACTIVE = "System.sp_CustomFormActiveInActive";
        private const string PROC_CATEGORY_LOOKUP = "System.sp_AttachCategoryLookUp";
        private const string PROC_CUSTOMFORMSBY_PROVIDER = "System.sp_CustomFormsByProviders";
        private const string PROC_GLOBAL_QUESTION_INSERT = "System.sp_GlobalQuestionInsert";
        private const string PROC_GLOBAL_QUESTION_SELECT = "System.sp_GlobalQuestionSelect";
        private const string PROC_GLOBAL_QUESTION_UPDATE = "System.sp_GlobalQuestionUpdate";
        private const string PROC_GLOBAL_QUESTION_DELETE = "System.sp_GlobalQuestionDelete";
        private const string PROC_GLOBAL_QUESTION_GROUP_INSERT = "System.sp_GlobalQuestionGroupInsert";
        private const string PROC_GLOBAL_QUESTION_GROUP_SELECT = "System.sp_GlobalQuestionGroupSelect";
        private const string PROC_GLOBAL_QUESTION_GROUP_UPDATE = "System.sp_GlobalQuestionGroupUpdate";
        private const string PROC_GLOBAL_QUESTION_GROUP_DELETE = "System.sp_GlobalQuestionGroupDelete";
        private const string PROC_GLOBAL_QUESTION_GROUP_FILL = "System.sp_GlobalQuestionGroupFill";

        private const string PROC_ATTACH_CUSTOM_FORM_WITH_NOTES = "Clinical.sp_AttachCustomFormWithNotes";
        private const string PROC_DETACH_CUSTOM_FORM_FROM_NOTES = "Clinical.sp_DetachCustomFromNotes";
        private const string PROC_CF_PROBLEMLIST_DELETE = "Clinical.sp_MultipleProblemListDelete";
        private const string PROC_CF_PROCEDURES_DELETE = "Clinical.sp_MultipleProcedureListDelete";
        private const string PROC_FAVORITE_CF_SELECT = "Clinical.sp_FavoriteListCustomFormSelect";

        
        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_FILE_PATH = "@FilePath";

        private const string PARM_SYSTEM_NAME = "@SystemName";
        private const string PARM_SORTING_ORDER = "@SortingOrder";

        private const string PARM_CUSTOM_FORM_ID = "@CustomFormId";
        private const string PARM_PROVIDERIDS = "@ProviderIds";
        private const string PARM_CUSTOM_FORM_NAME = "@FormName";
        private const string PARM_CUSTOM_FORM_HEADING = "@FormHeading";
        private const string PARM_CUSTOM_FORM_HTML = "@CustomFormHTML";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_IS_SPECIALTY_ALL = "@IsSpecialtyAll";
        private const string PARM_IS_PROVIDER_ALL = "@IsProviderAll";
        private const string PARM_PROVIDER_NAMES = "@ProviderNames";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_NAMES = "@SpecialtyNames";
        private const string PARM_SPECIALTY_IDS = "@SpecialityIds";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ATTACH_CAT_IDS = "@AttachCatIds";
        private const string PARM_CANVAS_COLS = "@CanvasCols";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_QUESTION_ID = "@QuestionId";
        private const string PARM_QUESTION_NAME = "@QuestionName";
        private const string PARM_QUESTION_HTML = "@QuestionHTML";

        private const string PARM_QUESTION_GROUP_ID = "@QuestionGroupId";
        private const string PARM_QUESTION_GROUP_NAME = "@QuestionGroupName";
        private const string PARM_QUESTION_GROUP_HTML = "@QuestionGroupHTML";
        private const string PARM_QUESTION_GROUP_TITLE = "@QuestionGroupTitle";
        private const string PARM_SAVE_GLOBALLY = "@SaveGlobally";
        private const string PARM_PROBLEMLIST_ID = "@ProblemListIds";
        private const string PARM_PROCEDURE_IDS = "@ProcedureListIds";
        private const string PARM_FAVORITELIST_ID = "@FavoriteListId";
        private const string PARM_FAVORITELIST_CF_ID = "@FavoriteListCustomFormId";
        private const string PARM_PROVIDER_ID = "@Providerid";
        private const string PARM_IS_FROMNOTES = "@IsFromNotes";
        #endregion

        #region "Constructors"
        public DALCustomForm()
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

        private void createParameters(IDBManager dbManager, CustomFormModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, model.CustomFormId);
            else
                dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, model.CustomFormId);
            dbManager.AddParameters(1, PARM_CUSTOM_FORM_NAME, model.FormName);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(3, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(6, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(7, PARM_PROVIDER_IDS, String.IsNullOrEmpty(model.ProviderIds) ? null : model.ProviderIds);
            dbManager.AddParameters(8, PARM_SPECIALTY_IDS, String.IsNullOrEmpty(model.SpecialtyIds) ? null : model.SpecialtyIds);
            dbManager.AddParameters(9, PARM_CANVAS_COLS, model.CanvasCols);
            dbManager.AddParameters(10, PARM_ATTACH_CAT_IDS, String.IsNullOrEmpty(model.AttachCatIds) ? null : model.AttachCatIds);
            dbManager.AddParameters(11, PARM_CUSTOM_FORM_HEADING, String.IsNullOrEmpty(model.FormHeading) ? null : model.FormHeading);
            dbManager.AddParameters(12, PARM_CUSTOM_FORM_HTML, String.IsNullOrEmpty(model.CustomFormHTML) ? null : model.CustomFormHTML);
        }
        private void createParametersGlobalQuestion(IDBManager dbManager, GlobalQuestionModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUESTION_ID, model.QuestionId);
            else
                dbManager.AddParameters(0, PARM_QUESTION_ID, model.QuestionId);
            dbManager.AddParameters(1, PARM_QUESTION_NAME, model.QuestionName);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(3, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(6, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(7, PARM_QUESTION_HTML, String.IsNullOrEmpty(model.QuestionHTML) ? null : model.QuestionHTML);
            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(8, PARM_ENTITY_ID, null);
            else
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }
        private void createParametersGlobalQuestionGroup(IDBManager dbManager, GlobalQuestionGroupModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, model.QuestionGroupId);
            else
                dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, model.QuestionGroupId);
            dbManager.AddParameters(1, PARM_QUESTION_GROUP_NAME, model.QuestionGroupName);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(3, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(6, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(7, PARM_QUESTION_GROUP_HTML, String.IsNullOrEmpty(model.QuestionGroupHTML) ? null : model.QuestionGroupHTML);
            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(8, PARM_ENTITY_ID, null);
            else
                dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
            dbManager.AddParameters(9, PARM_QUESTION_GROUP_TITLE, model.QuestionGroupTitle);
            dbManager.AddParameters(10, PARM_SAVE_GLOBALLY, model.SaveGlobally);
            dbManager.AddParameters(11, PARM_CANVAS_COLS, model.CanvasCols);
        }

        #endregion

        #region "CRUD Custom Form"

        /// Author: ZeeshanAK
        /// Purpose: To Insert Custom Form
        /// Date : September 20, 2016
        public string insertCustomForm(CustomFormModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal="";
                dbManager.Open();
                this.createParameters(dbManager, model, true);
                retunVal =Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CUSTOM_FORMS_INSERT));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::InsertCustomForm", PROC_CUSTOM_FORMS_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Author: ZeeshanAK
        /// Purpose: To load Custom Form
        /// Date : September 21, 2016
        public List<CustomFormModel> loadCustomForm(string formName, int? isActive, string providerIds, string specialityIds, long pageNumber, long rowsPerPage, int isFromNotes)
        {
            List<CustomFormModel> listCustomForms = new List<CustomFormModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(10);
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

                if (String.IsNullOrEmpty(providerIds))
                    dbManager.AddParameters(3, PARM_PROVIDER_IDS, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_IDS, providerIds);

                if (String.IsNullOrEmpty(specialityIds))
                    dbManager.AddParameters(4, PARM_SPECIALTY_IDS, null);
                else
                    dbManager.AddParameters(4, PARM_SPECIALTY_IDS, specialityIds);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                if (String.IsNullOrEmpty(formName))
                    dbManager.AddParameters(6, PARM_CUSTOM_FORM_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_CUSTOM_FORM_NAME, formName);
                //----------------


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(8, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (isFromNotes == 1)
                {
                    dbManager.AddParameters(9, PARM_IS_FROMNOTES, true);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_IS_FROMNOTES, false);
                }

                //----------------
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOM_FORMS_SELECT);
                CustomFormModel model = null;
                while (reader.Read())
                {
                    model = new CustomFormModel();
                    model.CustomFormId = !String.IsNullOrEmpty(reader["CustomFormId"].ToString()) ? reader["CustomFormId"].ToString() : "";
                    model.FormName = !String.IsNullOrEmpty(reader["FormName"].ToString()) ? reader["FormName"].ToString() : "";
                    model.SpecialtyNames = !String.IsNullOrEmpty(reader["SpecialtyNames"].ToString()) ? reader["SpecialtyNames"].ToString() : "";
                    model.ProviderNames = !String.IsNullOrEmpty(reader["ProviderNames"].ToString()) ? reader["ProviderNames"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.NoteId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    model.ModifiedByName = !String.IsNullOrEmpty(reader["ModifiedByName"].ToString()) ? reader["ModifiedByName"].ToString() : "";

                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadCustomForms", PROC_CUSTOM_FORMS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Author: ZeeshanAK
        /// Purpose: To Delete Custom Form
        /// Date : September 22, 2016
        public string deleteCustomForm(string formID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, formID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CUSTOM_FORMS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::DeleteCustomForm", PROC_CUSTOM_FORMS_DELETE, ex);
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

        /// Author: ZeeshanAK
        /// Purpose: To Fill Custom Form
        /// Date : September 23, 2016
        public List<CustomFormModel> fillCustomForm(string formId)
        {
            List<CustomFormModel> listCustomForms = new List<CustomFormModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (String.IsNullOrEmpty(formId))
                    dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, formId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOM_FORM_FILL);
                CustomFormModel model = null;
                while (reader.Read())
                {
                    model = new CustomFormModel();
                    model.CustomFormId = !String.IsNullOrEmpty(reader["CustomFormId"].ToString()) ? reader["CustomFormId"].ToString() : "";
                    model.FormName = !String.IsNullOrEmpty(reader["FormName"].ToString()) ? reader["FormName"].ToString() : "";
                    model.FormHeading = !String.IsNullOrEmpty(reader["FormHeading"].ToString()) ? reader["FormHeading"].ToString() : "";
                    model.CanvasCols = !String.IsNullOrEmpty(reader["CanvasCols"].ToString()) ? reader["CanvasCols"].ToString() : "";
                    model.CustomFormHTML = !String.IsNullOrEmpty(reader["CustomFormHTML"].ToString()) ? reader["CustomFormHTML"].ToString() : "";
                    model.SpecialtyIds = !String.IsNullOrEmpty(reader["SpecialtyIds"].ToString()) ? reader["SpecialtyIds"].ToString() : "";
                    model.ProviderIds = !String.IsNullOrEmpty(reader["ProviderIds"].ToString()) ? reader["ProviderIds"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.AttachCatIds = !String.IsNullOrEmpty(reader["AttachCategoryIds"].ToString()) ? reader["AttachCategoryIds"].ToString() : "";

                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadCustomForms", PROC_CUSTOM_FORMS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Author: ZeeshanAK
        /// Purpose: To Update Custom Form
        /// Date : September 23, 2016
        public string updateCustomForm(CustomFormModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = "";
                dbManager.Open();
                this.createParameters(dbManager, model, false);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CUSTOM_FORMS_UPDATE));
                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                    return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::UpdateCustomForm", PROC_CUSTOM_FORMS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// Author: ZeeshanAK
        /// Purpose: To Update Custom Form
        /// Date : September 23, 2016
        public List<CategoryLookupModel> LookupAttachCategory()
        {
            List<CategoryLookupModel> listModel = new List<CategoryLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CATEGORY_LOOKUP);
                CategoryLookupModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CategoryLookupModel();
                    modelFill.AttachCategoryId = Convert.ToInt64(reader["AttachCategoryId"]);
                    modelFill.ShortName = MDVUtility.CheckStringNull(reader["ShortName"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LookupMedicationsReprot", PROC_CATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CustomFormModel> LookupCustomFormsByProvider(string ProviderIDs)
        {
            List<CustomFormModel> listModel = new List<CustomFormModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PROVIDERIDS, ProviderIDs);
              
                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOMFORMSBY_PROVIDER);
                CustomFormModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CustomFormModel();
                    modelFill.CustomFormId = MDVUtility.CheckStringNull(reader["CustomFormId"]);
                    modelFill.FormName = MDVUtility.CheckStringNull(reader["FormName"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LookupCustomFormsByProvider", PROC_CUSTOMFORMSBY_PROVIDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// Author: ZeeshanAK
        /// Purpose: To Update Custom Form Active Inactive
        /// Date : September 26, 2016
        public string activeInactiveCustomForm(string formId, string isActive)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (String.IsNullOrEmpty(formId))
                    dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, formId);
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


        #endregion

        #region "CRUD Global Question"

        /// <summary>
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Insert Qlobal Question
        /// Date : September 28, 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string insertGlobalQuestion(GlobalQuestionModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                SqlDataReader reader = null;
                string returnValue = "";
                dbManager.Open();
                this.createParametersGlobalQuestion(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_INSERT);
                while (reader.Read())
                {
                    returnValue = reader[reader.GetName(0)].ToString();
                }
                return Convert.ToString(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::insertGlobalQuestion", PROC_GLOBAL_QUESTION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Load Qlobal Question
        /// Date : September 28, 2016
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="questionName"></param>
        /// <param name="isActive"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public List<GlobalQuestionModel> loadGlobalQuestion(string questionId, string questionName, int? isActive, long pageNumber, long rowsPerPage)
        {
            List<GlobalQuestionModel> listCustomForms = new List<GlobalQuestionModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (pageNumber <= 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage <= 0)
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                if (string.IsNullOrEmpty(questionId))
                {
                    dbManager.AddParameters(0, PARM_QUESTION_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_QUESTION_ID, questionId);
                }
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_SELECT);
                GlobalQuestionModel model = null;
                while (reader.Read())
                {
                    model = new GlobalQuestionModel();
                    model.QuestionId = !String.IsNullOrEmpty(reader["QuestionId"].ToString()) ? reader["QuestionId"].ToString() : "";
                    model.QuestionName = !String.IsNullOrEmpty(reader["QuestionName"].ToString()) ? reader["QuestionName"].ToString() : "";
                    model.QuestionHTML = !String.IsNullOrEmpty(reader["QuestionHTML"].ToString()) ? reader["QuestionHTML"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadGlobalQuestion", PROC_GLOBAL_QUESTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Delete Qlobal Question
        /// Date : September 28, 2016
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public string deleteGlobalQuestion(string questionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUESTION_ID, questionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::deleteGlobalQuestion", PROC_GLOBAL_QUESTION_DELETE, ex);
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
        public string updateGlobalQuestion(GlobalQuestionModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                SqlDataReader reader = null;
                string returnValue = "";
                dbManager.Open();
                this.createParametersGlobalQuestion(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_UPDATE);
                while (reader.Read())
                {
                    returnValue = reader["QuestionId"].ToString();
                }
                return Convert.ToString(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::insertGlobalQuestion", PROC_GLOBAL_QUESTION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "CRUD Global Question Group"

        /// <summary>
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Insert Global Question Group Group
        /// Date : September 28, 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string insertGlobalQuestionGroup(GlobalQuestionGroupModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersGlobalQuestionGroup(dbManager, model, true);
                object qstnGrpID = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_GROUP_INSERT);
                return Convert.ToString(qstnGrpID);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::insertGlobalQuestionGroup", PROC_GLOBAL_QUESTION_GROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Load Global Question Group Group
        /// Date : September 28, 2016
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="questionName"></param>
        /// <param name="isActive"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public List<GlobalQuestionGroupModel> loadGlobalQuestionGroup(string questionGroupId, string questionGroupName, long pageNumber, long rowsPerPage, bool? saveGlobally)
        {
            List<GlobalQuestionGroupModel> listCustomForms = new List<GlobalQuestionGroupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(6);
                if (string.IsNullOrEmpty(questionGroupId))
                {
                    dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, questionGroupId);
                }

                if (pageNumber <= 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage <= 0)
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(5, PARM_SAVE_GLOBALLY, saveGlobally);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_GROUP_SELECT);
                GlobalQuestionGroupModel model = null;
                while (reader.Read())
                {
                    model = new GlobalQuestionGroupModel();
                    model.QuestionGroupId = !String.IsNullOrEmpty(reader["QuestionGroupId"].ToString()) ? reader["QuestionGroupId"].ToString() : "";
                    model.QuestionGroupName = !String.IsNullOrEmpty(reader["QuestionGroupName"].ToString()) ? reader["QuestionGroupName"].ToString() : "";
                    model.QuestionGroupHTML = !String.IsNullOrEmpty(reader["QuestionGroupHTML"].ToString()) ? reader["QuestionGroupHTML"].ToString() : "";
                    model.QuestionGroupTitle = !String.IsNullOrEmpty(reader["QuestionGroupTitle"].ToString()) ? reader["QuestionGroupTitle"].ToString() : "";
                    model.SaveGlobally = !String.IsNullOrEmpty(reader["SaveGlobally"].ToString()) ? reader["SaveGlobally"].ToString() : "";
                    model.CanvasCols = !String.IsNullOrEmpty(reader["CanvasCols"].ToString()) ? reader["CanvasCols"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadGlobalQuestionGroup", PROC_GLOBAL_QUESTION_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Delete Global Question Group
        /// Date : September 28, 2016
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public string deleteGlobalQuestionGroup(string questionGroupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, questionGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_GROUP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::deleteGlobalQuestionGroup", PROC_GLOBAL_QUESTION_GROUP_DELETE, ex);
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
        public string updateGlobalQuestionGroup(GlobalQuestionGroupModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersGlobalQuestionGroup(dbManager, model, false);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_GROUP_UPDATE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::updateGlobalQuestionGroup", PROC_GLOBAL_QUESTION_GROUP_UPDATE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<GlobalQuestionGroupModel> fillGlobalQuestion(string questionGroupId)
        {
            List<GlobalQuestionGroupModel> listCustomForms = new List<GlobalQuestionGroupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(questionGroupId))
                {
                    dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_QUESTION_GROUP_ID, questionGroupId);
                }

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GLOBAL_QUESTION_GROUP_FILL);
                GlobalQuestionGroupModel model = null;
                while (reader.Read())
                {
                    model = new GlobalQuestionGroupModel();
                    model.QuestionGroupId = !String.IsNullOrEmpty(reader["QuestionGroupId"].ToString()) ? reader["QuestionGroupId"].ToString() : "";
                    model.QuestionGroupName = !String.IsNullOrEmpty(reader["QuestionGroupName"].ToString()) ? reader["QuestionGroupName"].ToString() : "";
                    model.QuestionGroupHTML = !String.IsNullOrEmpty(reader["QuestionGroupHTML"].ToString()) ? reader["QuestionGroupHTML"].ToString() : "";
                    model.QuestionGroupTitle = !String.IsNullOrEmpty(reader["QuestionGroupTitle"].ToString()) ? reader["QuestionGroupTitle"].ToString() : "";
                    model.SaveGlobally = !String.IsNullOrEmpty(reader["SaveGlobally"].ToString()) ? reader["SaveGlobally"].ToString() : "";
                    model.CanvasCols = !String.IsNullOrEmpty(reader["CanvasCols"].ToString()) ? reader["CanvasCols"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::fillGlobalQuestion", PROC_GLOBAL_QUESTION_GROUP_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region Notes and Custom Forms
        public string detachCustomFormsFromNotes(string CustomFormId, string NotesId, string CustomFormDocName)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, CustomFormId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                dbManager.AddParameters(2, PARM_FILE_PATH, CustomFormDocName);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_CUSTOM_FORM_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALustomForm::detachCustomFormsFromNotes", PROC_DETACH_CUSTOM_FORM_FROM_NOTES, ex);
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
        public string attachCustomFormsWithNotes(string CustomFormId, string NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CUSTOM_FORM_ID, CustomFormId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                object formID = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ATTACH_CUSTOM_FORM_WITH_NOTES);
                return Convert.ToString(formID);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::attachCustomFormsWithNotes", PROC_ATTACH_CUSTOM_FORM_WITH_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public string DeleteCustomFormProblemLists(string ProblemListIds, long noteId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListIds);
                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CF_PROBLEMLIST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCustomForm::DeleteCustomFormProblemLists", PROC_CF_PROBLEMLIST_DELETE, ex);
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

        public string DeleteCustomFormProcedures(string procedureIds, long noteId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PROCEDURE_IDS, procedureIds);
                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CF_PROCEDURES_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALCustomForm::DeleteCustomFormProcedures", PROC_CF_PROCEDURES_DELETE, ex);
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

        public List<CustomFormModel> loadFavoriteListCustomForm(long favoriteListCustomFormId, long favoriteListId, long providerId, int? isActive, long pageNumber, long rowsPerPage)
        {
            List<CustomFormModel> listCustomForms = new List<CustomFormModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (favoriteListCustomFormId <= 0)
                    dbManager.AddParameters(0, PARM_FAVORITELIST_CF_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITELIST_CF_ID, favoriteListCustomFormId);

                if (favoriteListId <= 0)
                    dbManager.AddParameters(1, PARM_FAVORITELIST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAVORITELIST_ID, favoriteListId);

                if (isActive == null)
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, isActive);
                }

                if (providerId <= 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, providerId);

                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (pageNumber <= 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage <= 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, rowsPerPage);
                
                dbManager.AddParameters(7, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
             
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_FAVORITE_CF_SELECT);
                CustomFormModel model = null;
                while (reader.Read())
                {
                    model = new CustomFormModel();
                    model.FavoriteListCustomFormId = !String.IsNullOrEmpty(reader["FavoriteListCustomFormId"].ToString()) ? reader["FavoriteListCustomFormId"].ToString() : "";
                    model.FavoriteListId = !String.IsNullOrEmpty(reader["FavoriteListId"].ToString()) ? reader["FavoriteListId"].ToString() : "";
                    model.FormName = !String.IsNullOrEmpty(reader["FormName"].ToString()) ? reader["FormName"].ToString() : "";
                    model.CustomFormId = !String.IsNullOrEmpty(reader["CustomFormId"].ToString()) ? reader["CustomFormId"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedByName = !String.IsNullOrEmpty(reader["ModifiedByName"].ToString()) ? reader["ModifiedByName"].ToString() : "";
                    model.SpecialtyNames = !String.IsNullOrEmpty(reader["SpecialtyNames"].ToString()) ? reader["SpecialtyNames"].ToString() : "";
                    model.ProviderNames = !String.IsNullOrEmpty(reader["ProviderNames"].ToString()) ? reader["ProviderNames"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomForm::loadFavoriteListCustomForm", PROC_FAVORITE_CF_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}


