using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Clinical.HPI;
using MDVision.Model.Clinical.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALHPI
    {
        #region "Constructors"

        private IContainer components;
        public DALHPI()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALHPI(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region "Stored Procedures Names"
        private const string PROC_HPITEMPLATE_SELECT = "Clinical.sp_HPITemplateSelect";
        private const string PROC_HPITEMPLATE_SAVE = "Clinical.sp_CreateHPINewTemplate";
        private const string PROC_HPI_SYMPTOMS_LOOKUP = "Clinical.sp_HPISymptomsLookup";
        private const string PROC_HPI_SYMFINDINGS_SELECT = "Clinical.HPITempSymFindingsSelect";
        private const string PROC_HPI_SYMPTOMFINDINGS_SELECT = "Clinical.sp_HPITempSymptomFindingSelect";
        private const string PROC_HPI_SYMPTOMFINDINGSDETAIL_SELECT = "Clinical.sp_HPISymptomsDetailSelect";
        private const string PROC_HPI_TEMPLATEFOR_PROVIDERS_SELECT = "Clinical.sp_HPITemplateSelectForProvider";
        private const string PROC_HPI_NOTES_FINDINGS_SELECT = "Clinical.sp_HPINotesFindingSelect";
        private const string PROC_HPI_TEMPLATESYPMTOM_DELETE = "Clinical.sp_HPITemplateSymptomDelete";
        private const string PROC_HPI_SYMPTOM_FINDING_DELETE = "Clinical.sp_HPISymptomFindingDelete";
        private const string PROC_HPI_SYMPTOM_FINDING_NOTE_SELECT = "Clinical.sp_HPITempSymptomFindingNoteSelect";
        private const string PROC_HPI_TEMPLATE_SYMPTOMS_SELECT = "Clinical.sp_HPITemplateSymptomSelect";
        private const string PROC_HPI_NOTES_FINDINGS_SAVE = "Clinical.sp_HPINotesFinding_insert";
        private const string PROC_HPI_NOTES_SYMPTOM_FINDING_DESC_UPDATE = "Clinical.sp_HPINotesSymptomFindingDescUpdate";

        private const string PROC_HPI_FINDINGS_SELECT = "Clinical.sp_HPIFindingsSelect";
        private const string PROC_HPI_FINDINGS_DELETE = "Clinical.sp_HPIFindingsDelete";
        private const string PROC_HPI_FINDINGS_INSERT = "Clinical.sp_HPIFindingsInsert";
        private const string PROC_HPI_FINDINGS_UPDATE = "Clinical.sp_HPIFindingsUpdate";

        private const string PROC_HPI_SYMPTOMS_SELECT = "Clinical.sp_HPISymptomsSelect";
        private const string PROC_HPI_SYMPTOMS_DELETE = "Clinical.sp_HPISymptomsDelete";
        private const string PROC_HPI_SYMPTOMS_INSERT = "Clinical.sp_HPISymptomsInsert";
        private const string PROC_HPI_SYMPTOMS_UPDATE = "Clinical.sp_HPISymptomsUpdate";

        private const string PROC_HPI_SYMPTOM_FINDINGS_SELECT = "Clinical.sp_HPISymptomFindingsSelect";
        private const string PROC_HPI_SYMPTOM_FINDINGS_DELETE = "Clinical.sp_HPISymptomFindingsDelete";
        private const string PROC_HPI_SYMPTOM_FINDINGS_INSERT = "Clinical.sp_HPISymptomFindingsInsert";

        private const string PROC_HPI_FINDINGS_LOOKUP = "Clinical.sp_HPIFindingsLookup";
        private const string PROC_HPI_TEMPLATE_LOOKUP = "Clinical.sp_HPITemplateLookup";
        private const string PROC_IS_HPI_COMPLAINT = "Clinical.sp_IsHPIComplaint";
        private const string PROC_HPITEMPLATE_DELETE = "Clinical.sp_HPITemplateDelete";
        private const string PROC_HPITEMPLATE_UPDATEACTIVEINACTIVE = "Clinical.sp_HPITemplateUpdateActiveInActive";

        private const string PROC_HPITEMPLATE_SYMPTOMS_INSERT = "Clinical.sp_HPITemplateSymptomInsert";


        #endregion

        #region "Parameters"
        private const string PARM_HPI_TEMPLATEID = "@HPITemplateId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_TEMPLATE_NAME = "@Name";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PROVIDER_XML = "@ProviderXML";
        private const string PARM_SYMPTOMFINDING_XML = "@SystemObservationXML";
        private const string PARM_TEMPLATE_PREVIEW = "@TemplatePreview";
        private const string PARM_SPECIALTY_XML = "@SpecialtyXML";
        private const string PARM_HPI_SYMPTOMID = "@HPISymptomId";
        private const string PARM_HPI_TEMPLATE_ID = "@TemplateId";
        private const string PARM_HPI_SYMPTOMDETAIL_ID = "@HPISymptomsDetailId";
        private const string PARM_HPI_TEMPLATESYMPTOM_ID = "@HPITemplateSymptomsId";
        private const string PARM_PROVIDER_ID = "@providerId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_HPI_SYMPTOMFINDING_ID = "@HPISymptomFindingId";
        private const string PARM_NOTES_ID = "@notesId";
        private const string PARM_ISSELECTED = "@isselected";
        private const string PARM_NOTES_FINDINGSXML = "@HPINotesFindingXML";

        private const string PARM_HPI_FINDINGS_ID = "@HPIFindingsId";
        private const string PARM_HPI_SYMPTOMS_ID = "@HPISymptomsId";
        private const string PARM_HPI_SYMPTOM_FINDINGS_ID = "@HPISymptomFindingsId";
        private const string PARM_HPI_NOTES_FINDING_ID = "@HPINotesFindingId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PR_PAGE = "@RowspPage";
        private const string PARM_NAME = "@Name";
        private const string PARM_IS_GLOBAL = "@IsGlobal";
        private const string PARM_Comments = "@Comments";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SYMPTOM_ORDER = "@SymptomOrder";
        private const string PARM_FINDING_ORDER = "@FindingOrder";


        #endregion

        #region "Support Functions"
        private void createHPITemplateParameters(IDBManager dbManager, HPITemplateModel model)
        {
            dbManager.CreateParameters(12);
            dbManager.AddParameters(0, PARM_TEMPLATE_NAME, model.Name);
            dbManager.AddParameters(1, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(3, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_PROVIDER_XML, model.ProviderXML);
            dbManager.AddParameters(6, PARM_SYMPTOMFINDING_XML, model.SymptomFindingXML);
            dbManager.AddParameters(7, PARM_ENTITY_ID, model.EntityId);
            dbManager.AddParameters(8, PARM_TEMPLATE_PREVIEW, model.TemplatePreview);
            dbManager.AddParameters(9, PARM_HPI_TEMPLATE_ID, MDVUtility.ToInt64(model.HPITemplateId));
            dbManager.AddParameters(10, PARM_SPECIALTY_XML, model.SpecialtyXML);
            dbManager.AddParameters(11, PARM_Comments, model.Comments);
        }

        private void createHPItemplateSymptomsParameters(IDBManager dbManager, HPITemplateSymptomsModel model)
        {
            dbManager.CreateParameters(11);
            dbManager.AddParameters(0, PARM_HPI_TEMPLATESYMPTOM_ID, model.HPITemplateSymptomId, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_HPI_TEMPLATEID, MDVUtility.ToInt64(model.HPITemplateId));
            dbManager.AddParameters(2, PARM_HPI_SYMPTOMID, MDVUtility.ToInt64(model.HPISymptomId));
            dbManager.AddParameters(3, PARM_ISSELECTED, model.IsSelected);
            dbManager.AddParameters(4, PARM_NOTES_ID, model.NotesId);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(6, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(7, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(9, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(10, PARM_SYMPTOM_ORDER, model.SymptomOrder);

        }
        #endregion

        #region "CRUD"

        #region " Lookup's "

        public List<HPIFindingsModel> LookupHPIFindings(string IsActive)
        {
            List<HPIFindingsModel> listHPIFindings = new List<HPIFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_FINDINGS_LOOKUP);
                HPIFindingsModel model = null;
                while (reader.Read())
                {
                    model = new HPIFindingsModel();
                    model.HPIFindingsId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";

                    listHPIFindings.Add(model);
                }
                return listHPIFindings;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LookupHPIFindings", PROC_HPI_FINDINGS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public List<HPITemplateModel> LookupHPITemplate()
        {
            List<HPITemplateModel> listHPITemplate = new List<HPITemplateModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_TEMPLATE_LOOKUP);
                HPITemplateModel model = null;
                while (reader.Read())
                {
                    model = new HPITemplateModel();
                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    listHPITemplate.Add(model);
                }
                return listHPITemplate;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LookupHPITemplate", PROC_HPI_TEMPLATE_LOOKUP, ex);
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

        #region " HPI Finding "
        public List<HPIFindingsModel> LoadHPIFindings(long HPIFindingsId, string IsActive, string Name, long PageNumber = 1, long RowspPage = 15)
        {
            List<HPIFindingsModel> listHPIFindings = new List<HPIFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);

                if (HPIFindingsId <= 0)
                    dbManager.AddParameters(0, PARM_HPI_FINDINGS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_FINDINGS_ID, HPIFindingsId);

                if (string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);

                if (string.IsNullOrEmpty(Name))
                    dbManager.AddParameters(2, PARM_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PR_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PR_PAGE, RowspPage);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_FINDINGS_SELECT);
                HPIFindingsModel model = null;
                while (reader.Read())
                {
                    model = new HPIFindingsModel();
                    model.HPIFindingsId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";

                    listHPIFindings.Add(model);
                }
                return listHPIFindings;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LoadHPIFindings", PROC_HPI_FINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string DeleteHPIFindings(long HPIFindingsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HPI_FINDINGS_ID, HPIFindingsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPI_FINDINGS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::DeleteHPIFindings", PROC_HPI_FINDINGS_DELETE, ex);
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
        private void createHPIFindingsParameters(IDBManager dbManager, HPIFindingsModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
            {
                int i = 0;
                dbManager.CreateParameters(7);
                dbManager.AddParameters(i++, PARM_HPI_FINDINGS_ID, model.HPIFindingsId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(i++, PARM_NAME, model.Name);
                dbManager.AddParameters(i++, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(i++, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, model.ModifiedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_HPI_FINDINGS_ID, model.HPIFindingsId);
                dbManager.AddParameters(PARM_NAME, model.Name);
                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(PARM_MODIFIED_ON, model.ModifiedOn);
            }
        }
        public string SaveHPIFindings(HPIFindingsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                createHPIFindingsParameters(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_FINDINGS_INSERT);
                while (reader.Read())
                {
                    model.HPIFindingsId = Convert.ToString(reader["HPIFindingsId"]);
                }
                return model.HPIFindingsId;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::SaveHPIFindings", PROC_HPI_FINDINGS_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string UpdateHPIFindings(HPIFindingsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createHPIFindingsParameters(dbManager, model, false);
                string returnVal = Convert.ToString(dbManager.ExecuteScalar(PROC_HPI_FINDINGS_UPDATE));
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::UpdateHPIFindings", PROC_HPI_FINDINGS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region " HPI Symptom "
        public List<HPISymptomsModel> LoadHPISymptoms(long HPISymptomsId, string IsActive, string Name, long PageNumber = 1, long RowspPage = 15)
        {
            List<HPISymptomsModel> listHPISymptoms = new List<HPISymptomsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);

                if (HPISymptomsId <= 0)
                    dbManager.AddParameters(0, PARM_HPI_SYMPTOMS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_SYMPTOMS_ID, HPISymptomsId);

                if (string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);

                if (string.IsNullOrEmpty(Name))
                    dbManager.AddParameters(2, PARM_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_NAME, Name);

                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PR_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PR_PAGE, RowspPage);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOMS_SELECT);
                HPISymptomsModel model = null;
                while (reader.Read())
                {
                    model = new HPISymptomsModel();
                    model.HPISymptomsId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.IsGlobal = !String.IsNullOrEmpty(reader["IsGlobal"].ToString()) ? reader["IsGlobal"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";

                    listHPISymptoms.Add(model);
                }
                return listHPISymptoms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LoadHPISymptoms", PROC_HPI_SYMPTOMS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string DeleteHPISymptoms(long HPISymptomsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HPI_SYMPTOMS_ID, HPISymptomsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPI_SYMPTOMS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::DeleteHPISymptoms", PROC_HPI_SYMPTOMS_DELETE, ex);
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
        private void createHPISymptomsParameters(IDBManager dbManager, HPISymptomsModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
            {
                int i = 0;
                dbManager.CreateParameters(10);
                dbManager.AddParameters(i++, PARM_HPI_SYMPTOMS_ID, model.HPISymptomsId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(i++, PARM_NAME, model.Name);
                dbManager.AddParameters(i++, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(i++, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(i++, PARM_IS_GLOBAL, model.IsGlobal);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, model.ModifiedOn);
                if (MDVUtility.ToInt64(model.HPITemplateId) == 0)
                {
                    dbManager.AddParameters(i++, PARM_HPI_TEMPLATEID, null);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_HPI_TEMPLATEID, MDVUtility.ToInt64(model.HPITemplateId));
                }
                dbManager.AddParameters(i++,PARM_SYMPTOM_ORDER, model.SymptomOrder);

            }
            else
            {
                dbManager.AddParameters(PARM_HPI_SYMPTOMS_ID, model.HPISymptomsId);
                dbManager.AddParameters(PARM_NAME, model.Name);
                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(PARM_IS_GLOBAL, model.IsGlobal);
                dbManager.AddParameters(PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(PARM_MODIFIED_ON, model.ModifiedOn);
            }
        }
        public List<HPISymptomsModel> SaveHPISymptoms(HPISymptomsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<HPISymptomsModel> templateModel = new List<HPISymptomsModel>();

            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                createHPISymptomsParameters(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOMS_INSERT);
                HPISymptomsModel symptom = new HPISymptomsModel();
                while (reader.Read())
                {
                    symptom.HPISymptomsId = Convert.ToString(reader["HPISymptomsId"]);
                    symptom.HPITemplateSymptomId = Convert.ToString(reader["HPITemplateSymptomId"]);
                    templateModel.Add(symptom);
                }
                return templateModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::SaveHPISymptoms", PROC_HPI_SYMPTOMS_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string UpdateHPISymptoms(HPISymptomsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createHPISymptomsParameters(dbManager, model, false);
                string returnVal = Convert.ToString(dbManager.ExecuteScalar(PROC_HPI_SYMPTOMS_UPDATE));
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::UpdateHPISymptoms", PROC_HPI_SYMPTOMS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region " HPI Symptom Finding "
        public List<HPIFindingsModel> LoadHPISymptomFinding(long HPISymptomsId)
        {
            List<HPIFindingsModel> listHPIFindings = new List<HPIFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_HPI_SYMPTOMS_ID, HPISymptomsId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOM_FINDINGS_SELECT);
                HPIFindingsModel model = null;
                while (reader.Read())
                {
                    model = new HPIFindingsModel();
                    model.HPIFindingsId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    model.HPISymptomFindingsId = !String.IsNullOrEmpty(reader["HPISymptomFindingsId"].ToString()) ? reader["HPISymptomFindingsId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.FindingOrder = !String.IsNullOrEmpty(reader["FindingOrder"].ToString()) ? reader["FindingOrder"].ToString() : "";

                    listHPIFindings.Add(model);
                }
                return listHPIFindings;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LoadHPISymptomFinding", PROC_HPI_SYMPTOM_FINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string DeleteHPISymptomFindings(long HPISymptomFindingsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HPI_SYMPTOM_FINDINGS_ID, HPISymptomFindingsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPI_SYMPTOM_FINDINGS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::DeleteHPISymptomFindings", PROC_HPI_SYMPTOM_FINDINGS_DELETE, ex);
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
        public List<HPISymptomFindingModel> SaveHPISymptomFinding(HPISymptomFindingModel model)
        {
            List<HPISymptomFindingModel> listModel = new List<HPISymptomFindingModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                List<string> HPIFindingsIds = model.HPIFindingsIds.Split(',').ToList().Select(s => s).ToList();
                string HPISymptomsId = model.HPISymptomsId;
                dbManager.Open();

                foreach (var HPIFindingsId in HPIFindingsIds)
                {
                    if (!string.IsNullOrEmpty(HPIFindingsId))
                    {
                        dbManager.CreateParameters(5);
                        dbManager.AddParameters(0, PARM_HPI_SYMPTOM_FINDINGS_ID, model.HPISymptomFindingsId, DbType.Int64, ParamDirection.Output);
                        dbManager.AddParameters(1, PARM_HPI_SYMPTOMS_ID, HPISymptomsId);
                        dbManager.AddParameters(2, PARM_HPI_FINDINGS_ID, HPIFindingsId);
                        dbManager.AddParameters(3, PARM_HPI_TEMPLATESYMPTOM_ID, MDVUtility.ToInt64(model.HPITemplateSymptomId));
                        if (MDVUtility.ToInt(model.FindingOrder) <= 0)
                        {
                            dbManager.AddParameters(4, PARM_FINDING_ORDER, null);
                        }
                        else
                        {
                            dbManager.AddParameters(4, PARM_FINDING_ORDER, MDVUtility.ToInt(model.FindingOrder));
                        }


                        reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOM_FINDINGS_INSERT);
                        while (reader.Read())
                        {
                            model = new HPISymptomFindingModel();
                            model.HPISymptomFindingsId = Convert.ToString(reader["HPISymptomFindingsId"]);
                            model.HPIFindingsId = HPIFindingsId;
                            listModel.Add(model);
                        }
                        reader.Close();
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::SaveHPISymptomFinding", PROC_HPI_SYMPTOM_FINDINGS_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        #endregion
        public List<HPITemplateModel> loadHPITemplates(long hpiTemplateId, int entityId, int? isActive)
        {
            List<HPITemplateModel> listHPITemplate = new List<HPITemplateModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);

                if (hpiTemplateId == 0)
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, hpiTemplateId);

                if (isActive == null || isActive < 0)
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, isActive);

                if (entityId == 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);

                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPITEMPLATE_SELECT);
                HPITemplateModel model = null;
                while (reader.Read())
                {
                    model = new HPITemplateModel();
                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.ProviderIds = !String.IsNullOrEmpty(reader["ProviderIds"].ToString()) ? reader["ProviderIds"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.TemplatePreview = !String.IsNullOrEmpty(reader["TemplatePreview"].ToString()) ? reader["TemplatePreview"].ToString() : "";

                    listHPITemplate.Add(model);
                }
                return listHPITemplate;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::loadHPITemplates", PROC_HPITEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string insertupdateHPITemplate(HPITemplateModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createHPITemplateParameters(dbManager, model);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPITEMPLATE_SAVE));
                if (retunVal == "Template Name already exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::insertupdateHPITemplate", PROC_HPITEMPLATE_SAVE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<HPITemplateModel> fillHPITemplate(long hpiTemplateId)
        {
            List<HPITemplateModel> listHPISymFindings = new List<HPITemplateModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (hpiTemplateId == 0)
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, hpiTemplateId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMFINDINGS_SELECT);
                HPITemplateModel model = null;
                List<HPITemplateSymptomsModel> symptoms = new List<HPITemplateSymptomsModel>();
                List<HPITemplateSymptomFindingsModel> findings = new List<HPITemplateSymptomFindingsModel>();

                while (reader.Read())
                {
                    model = new HPITemplateModel();
                    HPITemplateSymptomsModel sympotmsModel = new HPITemplateSymptomsModel();
                    HPITemplateSymptomFindingsModel findingsModel = new HPITemplateSymptomFindingsModel();

                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["TemplateName"].ToString()) ? reader["TemplateName"].ToString() : "";
                    model.SpecialtyIds = !String.IsNullOrEmpty(reader["SpecialtyIds"].ToString()) ? reader["SpecialtyIds"].ToString() : "";
                    model.ProviderIds = !String.IsNullOrEmpty(reader["ProviderIds"].ToString()) ? reader["ProviderIds"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.TemplatePreview = !String.IsNullOrEmpty(reader["TemplatePreview"].ToString()) ? reader["TemplatePreview"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";

                    sympotmsModel.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    sympotmsModel.TemplateName = !String.IsNullOrEmpty(reader["TemplateName"].ToString()) ? reader["TemplateName"].ToString() : "";
                    sympotmsModel.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    sympotmsModel.SymptomName = !String.IsNullOrEmpty(reader["SystemName"].ToString()) ? reader["SystemName"].ToString() : "";
                    sympotmsModel.IsSelectedSymptom = !String.IsNullOrEmpty(reader["IsSelectedSymptom"].ToString()) ? reader["IsSelectedSymptom"].ToString() : "";
                    sympotmsModel.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    sympotmsModel.HPISymptomsAnswersId = !String.IsNullOrEmpty(reader["HPISymptomsAnswersId"].ToString()) ? reader["HPISymptomsAnswersId"].ToString() : "";
                    sympotmsModel.SymptomOrder = !String.IsNullOrEmpty(reader["SymptomOrder"].ToString()) ? reader["SymptomOrder"].ToString() : "";
                    symptoms.Add(sympotmsModel);

                    findingsModel.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    findingsModel.SymptomName = !String.IsNullOrEmpty(reader["SystemName"].ToString()) ? reader["SystemName"].ToString() : "";
                    findingsModel.HPIFindingId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    findingsModel.FindingName = !String.IsNullOrEmpty(reader["ObservationName"].ToString()) ? reader["ObservationName"].ToString() : "";
                    findingsModel.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    findingsModel.HPISymptomsDetailId = !String.IsNullOrEmpty(reader["HPISymptomsDetailId"].ToString()) ? reader["HPISymptomsDetailId"].ToString() : "";
                    findingsModel.HPISymptoms_LocationIds = !String.IsNullOrEmpty(reader["HPISymptoms_LocationIds"].ToString()) ? reader["HPISymptoms_LocationIds"].ToString() : "";
                    findingsModel.HPISymptoms_RadiationId = !String.IsNullOrEmpty(reader["HPISymptoms_RadiationId"].ToString()) ? reader["HPISymptoms_RadiationId"].ToString() : "";
                    findingsModel.HPISymptoms_QualityId = !String.IsNullOrEmpty(reader["HPISymptoms_QualityId"].ToString()) ? reader["HPISymptoms_QualityId"].ToString() : "";
                    findingsModel.HPISymptoms_SeverityId = !String.IsNullOrEmpty(reader["HPISymptoms_SeverityId"].ToString()) ? reader["HPISymptoms_SeverityId"].ToString() : "";
                    findingsModel.Onset = !String.IsNullOrEmpty(reader["Onset"].ToString()) ? reader["Onset"].ToString() : "";
                    findingsModel.AssociatedWith = !String.IsNullOrEmpty(reader["AssociatedWith"].ToString()) ? reader["AssociatedWith"].ToString() : "";
                    findingsModel.HPISymptoms_AggravatedById = !String.IsNullOrEmpty(reader["HPISymptoms_AggravatedById"].ToString()) ? reader["HPISymptoms_AggravatedById"].ToString() : "";
                    findingsModel.HPISymptoms_RelievedById = !String.IsNullOrEmpty(reader["HPISymptoms_RelievedById"].ToString()) ? reader["HPISymptoms_RelievedById"].ToString() : "";
                    findingsModel.HPITemplateSympFindingId = !String.IsNullOrEmpty(reader["HPITemplateSympFindingId"].ToString()) ? reader["HPITemplateSympFindingId"].ToString() : "";
                    findingsModel.IsSymptomsDetail = !String.IsNullOrEmpty(reader["IsSymptomsDetail"].ToString()) ? MDVUtility.ToBool(reader["IsSymptomsDetail"]) : false;
                    findingsModel.FindingOrder = !String.IsNullOrEmpty(reader["FindingOrder"].ToString()) ? reader["FindingOrder"].ToString() : "";
                    findings.Add(findingsModel);

                    // listHPISymFindings.Add(model);
                }

                model.SymptomData = new List<HPITemplateSymptomsModel>(symptoms);
                model.FindingsData = new List<HPITemplateSymptomFindingsModel>(findings);

                listHPISymFindings.Add(model);

                return listHPISymFindings;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::fillHPITemplate", PROC_HPI_SYMFINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<HPISymptomsLookupModel> lookupHPISymptoms(int? isActive)
        {
            List<HPISymptomsLookupModel> listHPISymptoms = new List<HPISymptomsLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (isActive == null || isActive < 0)
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOMS_LOOKUP);
                HPISymptomsLookupModel model = null;
                while (reader.Read())
                {
                    model = new HPISymptomsLookupModel();
                    model.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.IsGlobal = !String.IsNullOrEmpty(reader["IsGlobal"].ToString()) ? reader["IsGlobal"].ToString() : "";

                    listHPISymptoms.Add(model);
                }
                return listHPISymptoms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::lookupHPISymptoms", PROC_HPI_SYMPTOMS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<HPITemplateSymptomFindingsModel> LoadHPISymptomsFindings(long HPITemplateId, long SymptomsId)
        {
            List<HPITemplateSymptomFindingsModel> listHPISymptoms = new List<HPITemplateSymptomFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (HPITemplateId == 0)
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, HPITemplateId);

                if (SymptomsId == 0)
                    dbManager.AddParameters(1, PARM_HPI_SYMPTOMID, null);
                else
                    dbManager.AddParameters(1, PARM_HPI_SYMPTOMID, SymptomsId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOMFINDINGS_SELECT);
                HPITemplateSymptomFindingsModel model = null;
                while (reader.Read())
                {
                    model = new HPITemplateSymptomFindingsModel();
                    model.FindingName = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.HPIFindingId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    model.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.FindingOrder = !String.IsNullOrEmpty(reader["FindingOrder"].ToString()) ? reader["FindingOrder"].ToString() : "";

                    listHPISymptoms.Add(model);
                }
                return listHPISymptoms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LoadHPISymptomsFindings", PROC_HPI_SYMPTOMFINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<HPITemplateSymptomFindingsModel> LoadHPISymptomsFindingsDetail(long HPISymptomDetail, long HPITemplateSymptomsId)
        {
            List<HPITemplateSymptomFindingsModel> listHPISymptoms = new List<HPITemplateSymptomFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (HPISymptomDetail == 0)
                    dbManager.AddParameters(0, PARM_HPI_SYMPTOMDETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_SYMPTOMDETAIL_ID, HPISymptomDetail);

                if (HPITemplateSymptomsId == 0)
                    dbManager.AddParameters(1, PARM_HPI_TEMPLATESYMPTOM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_HPI_TEMPLATESYMPTOM_ID, HPITemplateSymptomsId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOMFINDINGSDETAIL_SELECT);
                HPITemplateSymptomFindingsModel findingsModel = null;
                while (reader.Read())
                {
                    findingsModel = new HPITemplateSymptomFindingsModel();
                    findingsModel.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    findingsModel.SymptomName = !String.IsNullOrEmpty(reader["SystemName"].ToString()) ? reader["SystemName"].ToString() : "";
                    findingsModel.HPIFindingId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    findingsModel.FindingName = !String.IsNullOrEmpty(reader["ObservationName"].ToString()) ? reader["ObservationName"].ToString() : "";
                    findingsModel.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    findingsModel.HPISymptomsDetailId = !String.IsNullOrEmpty(reader["HPISymptomsDetailId"].ToString()) ? reader["HPISymptomsDetailId"].ToString() : "";
                    findingsModel.HPISymptoms_LocationIds = !String.IsNullOrEmpty(reader["HPISymptoms_LocationIds"].ToString()) ? reader["HPISymptoms_LocationIds"].ToString() : "";
                    findingsModel.HPISymptoms_RadiationId = !String.IsNullOrEmpty(reader["HPISymptoms_RadiationId"].ToString()) ? reader["HPISymptoms_RadiationId"].ToString() : "";
                    findingsModel.HPISymptoms_QualityId = !String.IsNullOrEmpty(reader["HPISymptoms_QualityId"].ToString()) ? reader["HPISymptoms_QualityId"].ToString() : "";
                    findingsModel.HPISymptoms_SeverityId = !String.IsNullOrEmpty(reader["HPISymptoms_SeverityId"].ToString()) ? reader["HPISymptoms_SeverityId"].ToString() : "";
                    findingsModel.Onset = !String.IsNullOrEmpty(reader["Onset"].ToString()) ? reader["Onset"].ToString() : "";
                    findingsModel.AssociatedWith = !String.IsNullOrEmpty(reader["AssociatedWith"].ToString()) ? reader["AssociatedWith"].ToString() : "";
                    findingsModel.HPISymptoms_AggravatedById = !String.IsNullOrEmpty(reader["HPISymptoms_AggravatedById"].ToString()) ? reader["HPISymptoms_AggravatedById"].ToString() : "";
                    findingsModel.HPISymptoms_RelievedById = !String.IsNullOrEmpty(reader["HPISymptoms_RelievedById"].ToString()) ? reader["HPISymptoms_RelievedById"].ToString() : "";
                    findingsModel.HPITemplateSympFindingId = !String.IsNullOrEmpty(reader["HPITemplateSympFindingId"].ToString()) ? reader["HPITemplateSympFindingId"].ToString() : "";

                    listHPISymptoms.Add(findingsModel);
                }
                return listHPISymptoms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::LoadHPISymptomsFindingsDetail", PROC_HPI_SYMPTOMFINDINGSDETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<HPITemplateModel> loadHPIForProvider(long providerId = 0)
        {
            List<HPITemplateModel> listHPITemplates = new List<HPITemplateModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (providerId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, providerId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_TEMPLATEFOR_PROVIDERS_SELECT);
                HPITemplateModel model = null;
                while (reader.Read())
                {
                    model = new HPITemplateModel();

                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";

                    listHPITemplates.Add(model);
                }
                return listHPITemplates;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::loadHPIForProvider", PROC_HPI_TEMPLATEFOR_PROVIDERS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<HPINotesFindings> loadHPITempSymptomFindingsForNotes(long notesId)
        {
            List<HPINotesFindings> listHPINotesFindings = new List<HPINotesFindings>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_NOTES_ID, notesId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_NOTES_FINDINGS_SELECT);
                HPINotesFindings model = null;
                while (reader.Read())
                {
                    model = new HPINotesFindings();

                    model.HPINotesFindingsId = !String.IsNullOrEmpty(reader["HPINotesFindingId"].ToString()) ? reader["HPINotesFindingId"].ToString() : "";
                    model.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    model.SymptomDescription = !String.IsNullOrEmpty(reader["SymptomDescription"].ToString()) ? reader["SymptomDescription"].ToString() : "";
                    model.NotesId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.TemplateName = !String.IsNullOrEmpty(reader["TemplateName"].ToString()) ? reader["TemplateName"].ToString() : "";
                    model.SymptomName = !String.IsNullOrEmpty(reader["SymptomName"].ToString()) ? reader["SymptomName"].ToString() : "";
                    model.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    model.HPISymptomsAnswersId = !String.IsNullOrEmpty(reader["HPISymptomsAnswersId"].ToString()) ? reader["HPISymptomsAnswersId"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.SymptomOrder = !String.IsNullOrEmpty(reader["SymptomOrder"].ToString()) ? reader["SymptomOrder"].ToString() : "";
                    listHPINotesFindings.Add(model);
                }
                return listHPINotesFindings;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::loadHPITempSymptomFindingsForNotes", PROC_HPI_NOTES_FINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteHPITemplateSymptom(long HPITemplateSymptomId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HPI_TEMPLATESYMPTOM_ID, HPITemplateSymptomId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "");

                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPI_TEMPLATESYPMTOM_DELETE));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::deleteHPITemplateSymptom", PROC_HPI_TEMPLATESYPMTOM_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteHPISymptomFinding(long symptomFindingId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_HPI_SYMPTOMFINDING_ID, symptomFindingId);

                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPI_SYMPTOM_FINDING_DELETE));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::deleteHPISymptomFinding", PROC_HPI_SYMPTOM_FINDING_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<HPITemplateModel> loadHPITempSymptomFindingNote(long TemplateId, long SystemId, long? NotesId = null)
        {
            List<HPITemplateModel> listHPINotesFindings = new List<HPITemplateModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, TemplateId);
                dbManager.AddParameters(1, PARM_HPI_SYMPTOMID, SystemId);
                dbManager.AddParameters(2, PARM_NOTES_ID, NotesId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_SYMPTOM_FINDING_NOTE_SELECT);
                HPITemplateModel modelTemplate = new HPITemplateModel();
                List<HPINotesFindings> notesFindingsList = new List<HPINotesFindings>();
                List<HPITemplateSymptomFindingsModel> findingdata = new List<HPITemplateSymptomFindingsModel>();

                HPINotesFindings notesData = null;
                HPITemplateSymptomFindingsModel findings = null;

                while (reader.Read())
                {

                    findings = new HPITemplateSymptomFindingsModel();

                    findings.HPISymptomFindingsId = !String.IsNullOrEmpty(reader["HPISymptomFindingsId"].ToString()) ? reader["HPISymptomFindingsId"].ToString() : "";
                    findings.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    findings.HPIFindingId = !String.IsNullOrEmpty(reader["HPIFindingsId"].ToString()) ? reader["HPIFindingsId"].ToString() : "";
                    findings.FindingName = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    findings.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    findings.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    findings.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    findings.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    findings.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    findings.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    findings.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    findings.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    findings.IsFindingSelected = !String.IsNullOrEmpty(reader["IsFindingSelected"].ToString()) ? reader["IsFindingSelected"].ToString() : "";
                    findings.TemplatePreview = !String.IsNullOrEmpty(reader["TemplatePreview"].ToString()) ? reader["TemplatePreview"].ToString() : "";

                    findings.HPISymptomsDetailId = !String.IsNullOrEmpty(reader["HPISymptomsDetailId"].ToString()) ? reader["HPISymptomsDetailId"].ToString() : "";
                    findings.HPISymptoms_LocationIds = !String.IsNullOrEmpty(reader["HPISymptoms_LocationIds"].ToString()) ? reader["HPISymptoms_LocationIds"].ToString() : "";
                    findings.HPISymptoms_RadiationId = !String.IsNullOrEmpty(reader["HPISymptoms_RadiationId"].ToString()) ? reader["HPISymptoms_RadiationId"].ToString() : "";
                    findings.HPISymptoms_QualityId = !String.IsNullOrEmpty(reader["HPISymptoms_QualityId"].ToString()) ? reader["HPISymptoms_QualityId"].ToString() : "";
                    findings.HPISymptoms_SeverityId = !String.IsNullOrEmpty(reader["HPISymptoms_SeverityId"].ToString()) ? reader["HPISymptoms_SeverityId"].ToString() : "";
                    findings.Onset = !String.IsNullOrEmpty(reader["Onset"].ToString()) ? reader["Onset"].ToString() : "";
                    findings.AssociatedWith = !String.IsNullOrEmpty(reader["AssociatedWith"].ToString()) ? reader["AssociatedWith"].ToString() : "";
                    findings.HPISymptoms_AggravatedById = !String.IsNullOrEmpty(reader["HPISymptoms_AggravatedById"].ToString()) ? reader["HPISymptoms_AggravatedById"].ToString() : "";
                    findings.HPISymptoms_RelievedById = !String.IsNullOrEmpty(reader["HPISymptoms_RelievedById"].ToString()) ? reader["HPISymptoms_RelievedById"].ToString() : "";
                    findings.HPITemplateSympFindingId = !String.IsNullOrEmpty(reader["HPITempSymFindingsId"].ToString()) ? reader["HPITempSymFindingsId"].ToString() : "";
                    findings.IsSymptomsDetail = !String.IsNullOrEmpty(reader["IsSymptomsDetail"].ToString()) ? MDVUtility.ToBool(reader["IsSymptomsDetail"]) : false;
                    findings.FindingOrder = !String.IsNullOrEmpty(reader["FindingOrder"].ToString()) ? reader["FindingOrder"].ToString() : "";

                    findingdata.Add(findings);
                }
                modelTemplate.FindingsData = new List<HPITemplateSymptomFindingsModel>(findingdata);
                reader.NextResult();
                while (reader.Read())
                {
                    notesData = new HPINotesFindings();
                    notesData.HPINotesFindingsId = !String.IsNullOrEmpty(reader["HPINotesFindingId"].ToString()) ? reader["HPINotesFindingId"].ToString() : "";
                    notesData.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    notesData.Desc = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    notesData.NotesId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    notesData.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    notesData.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    notesData.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    notesData.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    notesData.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    notesFindingsList.Add(notesData);
                }

                modelTemplate.NotesFindingsData = new List<HPINotesFindings>(notesFindingsList);
                listHPINotesFindings.Add(modelTemplate);

                return listHPINotesFindings;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::loadHPITempSymptomFindingNote", PROC_HPI_SYMPTOM_FINDING_NOTE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<HPITemplateSymptomsModel> loadHPITemplateSymptoms(long templateId, long entityId, int? IsActive = 1, int? isSelected = 1)
        {
            List<HPITemplateSymptomsModel> listTemplateSymptoms = new List<HPITemplateSymptomsModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);

                if (templateId == 0)
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, null);
                else
                    dbManager.AddParameters(0, PARM_HPI_TEMPLATEID, templateId);

                if (isSelected == 0)
                    isSelected = null;
                dbManager.AddParameters(1, PARM_ISSELECTED, isSelected);

                if (entityId == 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_TEMPLATE_SYMPTOMS_SELECT);

                HPITemplateSymptomsModel model = null;

                while (reader.Read())
                {
                    model = new HPITemplateSymptomsModel();

                    model.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    model.TemplateName = !String.IsNullOrEmpty(reader["TemplateName"].ToString()) ? reader["TemplateName"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["Providerid"].ToString()) ? reader["Providerid"].ToString() : "";
                    model.SymptomName = !String.IsNullOrEmpty(reader["SymptomName"].ToString()) ? reader["SymptomName"].ToString() : "";
                    model.IsGlobal = !String.IsNullOrEmpty(reader["isGlobal"].ToString()) ? reader["isGlobal"].ToString() : "";
                    model.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    model.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.SymptomDescription = !String.IsNullOrEmpty(reader["SystemDescription"].ToString()) ? reader["SystemDescription"].ToString() : "";
                    model.HPISymptomsAnswersId = !String.IsNullOrEmpty(reader["HPISymptomsAnswersId"].ToString()) ? reader["HPISymptomsAnswersId"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.SymptomOrder = !String.IsNullOrEmpty(reader["SymptomOrder"].ToString()) ? reader["SymptomOrder"].ToString() : "";
                    listTemplateSymptoms.Add(model);
                }

                return listTemplateSymptoms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::loadHPITemplateSymptoms", PROC_HPI_TEMPLATE_SYMPTOMS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<HPINotesFindings> saveHPIComplaintNotesFinding(string xmlNotesFindings,string AppUserName)
        {
            List<HPINotesFindings> listHPISymFindings = new List<HPINotesFindings>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_CREATED_BY, AppUserName);
                dbManager.AddParameters(1, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, AppUserName);
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_NOTES_FINDINGSXML, xmlNotesFindings);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HPI_NOTES_FINDINGS_SAVE);
                HPINotesFindings model = null;

                while (reader.Read())
                {
                    model = new HPINotesFindings();

                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.TemplateName = !String.IsNullOrEmpty(reader["TemplateName"].ToString()) ? reader["TemplateName"].ToString() : "";
                    model.NotesId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    model.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.SymptomName = !String.IsNullOrEmpty(reader["SymptomName"].ToString()) ? reader["SymptomName"].ToString() : "";
                    model.Desc = !String.IsNullOrEmpty(reader["SymptomDescription"].ToString()) ? reader["SymptomDescription"].ToString() : "";
                    model.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    model.HPINotesFindingsId = !String.IsNullOrEmpty(reader["HPINotesFindingId"].ToString()) ? reader["HPINotesFindingId"].ToString() : "";
                    model.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    model.SymptomDescription = !String.IsNullOrEmpty(reader["SymptomDescription"].ToString()) ? reader["SymptomDescription"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    listHPISymFindings.Add(model);

                }
                return listHPISymFindings;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::saveHPIComplaintNotesFinding", PROC_HPI_NOTES_FINDINGS_SAVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string isHPIComplaint(long notesId)
        {
            List<HPINotesFindings> listHPISymFindings = new List<HPINotesFindings>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                string retunVal = "";
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTES_ID, notesId);


                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_HPI_COMPLAINT));
                return retunVal;


                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IS_HPI_COMPLAINT);
                //HPINotesFindings model = null;

                //while (reader.Read())
                //{
                //    model = new HPINotesFindings();

                //    model.IsHPIComplaint = !String.IsNullOrEmpty(reader["IsHPIComplaint"].ToString()) ? reader["IsHPIComplaint"].ToString() : "";                  
                //    listHPISymFindings.Add(model);

                //}
                //return listHPISymFindings;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::isHPIComplaint", PROC_IS_HPI_COMPLAINT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteHPITemplate(long templateId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HPI_TEMPLATE_ID, templateId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPITEMPLATE_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::deleteHPITemplate", PROC_HPITEMPLATE_DELETE, ex);
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

        public string HPITemplateIsActive(long templateId, long? IsActive = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_HPI_TEMPLATE_ID, templateId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                var returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_HPITEMPLATE_UPDATEACTIVEINACTIVE).ToString();

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::HPITemplateIsActive", PROC_HPITEMPLATE_UPDATEACTIVEINACTIVE, ex);
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

        public string associateHPISymptomAndTemplate(HPITemplateSymptomsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createHPItemplateSymptomsParameters(dbManager, model);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HPITEMPLATE_SYMPTOMS_INSERT));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::associateHPISymptomAndTemplate", PROC_HPITEMPLATE_SYMPTOMS_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateHPINotesSymptomFindingDesc(long HPINotesFindingId, string Desc)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, PARM_HPI_NOTES_FINDING_ID, HPINotesFindingId);
                dbManager.AddParameters(1, PARM_DESCRIPTION, Desc);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                var returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_HPI_NOTES_SYMPTOM_FINDING_DESC_UPDATE).ToString();

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::UpdateHPINotesSymptomFindingDesc", PROC_HPI_NOTES_SYMPTOM_FINDING_DESC_UPDATE, ex);
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
