using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Clinical
{
    /// <summary>
    /// Author: Arsalan Javed
    /// Created Date: 14-02-2017
    /// Overview: Data Access Layer for new Physical Exam copy as ECW
    /// </summary>
    public class DALPhysicalExamECW
    {
        #region " Variable "

        #endregion

        #region " Stored Procedure Names "

        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_INSERT = "Clinical.sp_PESystemInsert";
        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_UPDATE = "Clinical.sp_PESystemUpdate";
        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_SELECT = "Clinical.sp_PESystemSelect";
        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_DELETE = "Clinical.sp_PESystemDelete";

        private const string PROC_PHYSICALEXAM_ECW_OBSERVATION_INSERT = "Clinical.sp_PEObservationInsert";
        private const string PROC_PHYSICALEXAM_ECW_OBSERVATION_UPDATE = "Clinical.sp_PEObservationUpdate";
        private const string PROC_PHYSICALEXAM_ECW_OBSERVATION_SELECT = "Clinical.sp_PEObservationSelect";
        private const string PROC_PHYSICALEXAM_ECW_OBSERVATION_DELETE = "Clinical.sp_PEObservationDelete";

        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_INSERT = "Clinical.sp_PESystemObservationInsert";
        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_UPDATE = "Clinical.sp_PESystemObservationUpdate";
        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_SELECT = "Clinical.sp_PESystemObservationSelect";
        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_DELETE = "Clinical.sp_PESystemObservationDelete";

        private const string PROC_PHYSICALEXAM_ECW_SYSTEM_LOOKUP = "Clinical.sp_PESystemLookup";
        private const string PROC_PHYSICALEXAM_ECW_OBSERVATION_LOOKUP = "Clinical.sp_PEObservationLookup";

        private const string PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEMSELECT = "Clinical.sp_PETemplateSystemSelect";
        private const string PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEM_DELETE = "Clinical.sp_PETemplateSystemDelete";

        private const string PROC_PHYSICALEXAM_ECW_TEMPLATESAVE = "Clinical.sp_CreateNewTemplate";
        private const string PROC_PHYSEXAMTEMPLATE_SELECTECW = "Clinical.sp_PETemplateSelect";
        private const string PROC_PHYSEXAMTEMPLATE_UPDATEACTIVEINACTIVE = "Clinical.sp_PETemplateUpdateActiveInActive";
        private const string PROC_PHYSEXAMTEMPLATE_DELETE = "Clinical.sp_PETemplateDelete";
        private const string PROC_PHYSICALEXAM_ECW_TEMP_SYS_OBSERVATION_SELECT = "Clinical.sp_PETempSysObservationSelect";
        private const string PROC_PHYSICALEXAM_ECW_TEMPLATE_SELECT = "Clinical.sp_PETemplateSelect";
        private const string PROC_PHYSICALEXAM_NOTES_OBSERVATION_INSERT = "Clinical.sp_PENotesObservation_insert";
        private const string PROC_PHYSICALEXAM_ECW_TEMP_SYSTEM_OBSERVATION_SELECT = "Clinical.sp_PETempSystemObservationSelect";
        private const string PROC_PHYSICALEXAM_ECW_TEMP_SYSTEM_OBSERVATION_NOTE_SELECT = "Clinical.sp_PETempSystemObservationNoteSelect";
        private const string PROC_PHYSICALEXAM_NOTES_OBSERVATION_DELETE = "Clinical.sp_PENotesObservationDelete";
        private const string PROC_PHYSICALEXAM_NOTES_OBSERVATION_SELECT = "Clinical.sp_PENotesObservationSelect";
        private const string PROC_PHYSICALEXAM_NOTES_SYS_OBSERS_SELECT = "clinical.sp_PENotesSystemObservationSelect";
        private const string PROC_PHYSICALEXAM_NOTES_SYS_OBSERS_DESC_UPDATE = "clinical.sp_PENotesSystemObservationDescUpdate";

        private const string PROC_GETSPECIALTYPROVIDER = "Clinical.GetSpecialtyProvider";
        private const string PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEM_INSERT = "Clinical.sp_PETemplateSystemInsert";
        private const string PROC_PHYSEXAM_TEMPLATEFOR_PROVIDERS_SELECT = "Clinical.sp_PETemplateSelectForProvider";
        private const string PROC_PHYSICALEXAM_INSERT = "Clinical.sp_PETemplateInsert";
        private const string PROC_PE_TEMPLATE_LOOKUP = "Clinical.sp_PETemplateLookup";
        private const string PROC_PHYSICALEXAM_ACTIVE_TEMP_SYS_OBSERVATION_SELECT = "Clinical.sp_PEActiveTempSysObservationSelect";
        private const string PROC_PE_PREV_NOTES_OBSERVATION_SELECT = "Clinical.sp_PEPrevNotesObservationSelect";

        #endregion

        #region " Constructors "
        public DALPhysicalExamECW()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALPhysicalExamECW(SharedVariable SharedVariable)
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

        #region " Parameters "

        private const string PARM_PE_SYSTEM_ID = "@PESystemId";
        private const string PARM_PE_OBSERVATION_ID = "@PEObservationId";
        private const string PARM_PE_SYSTEM_OBSERVATION_ID = "@PESystemObservationId";
        private const string PARM_PE_TEMPLATE_SYSTEM_ID = "@PETemplateSystemId";
        private const string PARM_NAME = "@Name";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_IS_GLOBAL = "@IsGlobal";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PR_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_TEMPLATE_ID = "@PETemplateId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_ISSELECTED = "@IsSelected";

        private const string PARM_TEMPLATE_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_TEMPLATE_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_TEMPLATE_ENTITY_ID = "@EntityId";
        private const string PARM_TEMPLATE_SYSTEM_ID = "@PETemplateSystemId";
        private const string PARM_PE_NOTES_OBSERVATION_XML = "@PENotesObservationXML";
        private const string PARM_PE_NOTES_ID = "@NotesId";
        private const string PARM_PE_NOTES_OBSERVATION_ID = "@PENotesObservationId";
        private const string PARM_PE_NOTES_DESC = "@Desc";
        private const string PARM_PE_USER_ID = "@UserId";
        private const string PE_TemplateID = "@PETemplateID";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PREV_NOTES_ID = "@PrevNotesId";
        private const string PARM_BODYPART_ID = "@BodyPartId";
        #endregion

        #region " PhysicalExamECW System "

        #region " Support Functions PhysicalExamECW System "

        private void createPhysicalExamSystemParameters(IDBManager dbManager, DSPhysicalExamECW ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, ds.PESystem.PESystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_NAME, ds.PESystem.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.PESystem.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(3, PARM_IS_GLOBAL, ds.PESystem.IsGlobalColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(4, PARM_CREATED_BY, ds.PESystem.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_CREATED_ON, ds.PESystem.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.PESystem.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.PESystem.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(8, PE_TemplateID, ds.PESystem.PETemplateIDColumn.ColumnName, DbType.String);
            }
            else
            {
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, ds.PESystem.PESystemIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_NAME, ds.PESystem.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.PESystem.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(3, PARM_IS_GLOBAL, ds.PESystem.IsGlobalColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, ds.PESystem.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_MODIFIED_ON, ds.PESystem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }

        }
        private void createPETemplateSystemParameters(IDBManager dbManager, DSPhysicalExamECW ds)
        {
            dbManager.CreateParameters(10);
            dbManager.AddParameters(0, PARM_TEMPLATE_SYSTEM_ID, ds.PETemplateSystem.PETemplateSystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_TEMPLATE_ID, ds.PETemplateSystem.PETemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PE_SYSTEM_ID, ds.PETemplateSystem.PESystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ISSELECTED, ds.PETemplateSystem.IsSelectedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PETemplateSystem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_PE_NOTES_ID, ds.PETemplateSystem.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.PETemplateSystem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.PETemplateSystem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.PETemplateSystem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.PETemplateSystem.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region " CRUD Functions PhysicalExamECW Observations "

        public DSPhysicalExamECW loadPhysicalExamSystem(long SystemId, string IsActive, string Name, long PageNumber = 1, long RowspPage = 15)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (SystemId <= 0)
                    dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, SystemId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PESystem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_SELECT, ds, ds.PESystem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamSystem", PROC_PHYSICALEXAM_ECW_SYSTEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW insertPhysicalExamSystem(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPhysicalExamSystemParameters(dbManager, ds, true);
                ds = (DSPhysicalExamECW)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_INSERT, ds, ds.PESystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::insertPhysicalExamSystem", PROC_PHYSICALEXAM_ECW_SYSTEM_INSERT, ex);
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

        public DSPhysicalExamECW updatePhysicalExamSystem(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPhysicalExamSystemParameters(dbManager, ds, false);
                ds = (DSPhysicalExamECW)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_UPDATE, ds, ds.PESystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updatePhysicalExamSystem", PROC_PHYSICALEXAM_ECW_SYSTEM_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deletePhysicalExamSystem(long SystemId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, SystemId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePhysicalExamSystem", PROC_PHYSICALEXAM_ECW_SYSTEM_DELETE, ex);
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

        #endregion

        #region " PhysicalExamECW Observation "

        #region " Support Functions PhysicalExamECW Observation "

        private void createPhysicalExamObservationParameters(IDBManager dbManager, DSPhysicalExamECW ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_PE_OBSERVATION_ID, ds.PEObservation.PEObservationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_NAME, ds.PEObservation.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.PEObservation.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(3, PARM_CREATED_BY, ds.PEObservation.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_CREATED_ON, ds.PEObservation.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.PEObservation.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.PEObservation.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
            else
            {
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_PE_OBSERVATION_ID, ds.PEObservation.PEObservationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_NAME, ds.PEObservation.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.PEObservation.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, ds.PEObservation.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, ds.PEObservation.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }

        }

        #endregion

        #region " CRUD Functions PhysicalExamECW Observations "

        public DSPhysicalExamECW loadPhysicalExamObservation(long ObservationId, string IsActive, string Name, long PageNumber = 1, long RowspPage = 15)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (ObservationId <= 0)
                    dbManager.AddParameters(0, PARM_PE_OBSERVATION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PE_OBSERVATION_ID, ObservationId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PEObservation.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_OBSERVATION_SELECT, ds, ds.PEObservation.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamObservation", PROC_PHYSICALEXAM_ECW_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW insertPhysicalExamObservation(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPhysicalExamObservationParameters(dbManager, ds, true);
                ds = (DSPhysicalExamECW)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_OBSERVATION_INSERT, ds, ds.PEObservation.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::insertPhysicalExamObservation", PROC_PHYSICALEXAM_ECW_OBSERVATION_INSERT, ex);
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

        public DSPhysicalExamECW updatePhysicalExamObservation(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPhysicalExamObservationParameters(dbManager, ds, false);
                ds = (DSPhysicalExamECW)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_OBSERVATION_UPDATE, ds, ds.PEObservation.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updatePhysicalExamObservation", PROC_PHYSICALEXAM_ECW_OBSERVATION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deletePhysicalExamObservation(long ObservationId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PE_OBSERVATION_ID, ObservationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_OBSERVATION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePhysicalExamObservation", PROC_PHYSICALEXAM_ECW_OBSERVATION_DELETE, ex);
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

        #endregion

        #region " PhycsicalExamECW System Observation "

        #region " Support Functions PhysicalExamECW System Observation "

        private void createPhysicalExamSystemObservationParameters(IDBManager dbManager, DSPhysicalExamECW ds, bool isInsert = true)
        {
            dbManager.CreateParameters(5);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_PE_SYSTEM_OBSERVATION_ID, ds.PESystemObservation.PESystemObservationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PE_SYSTEM_OBSERVATION_ID, ds.PESystemObservation.PESystemObservationIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PE_SYSTEM_ID, ds.PESystemObservation.PESystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PE_OBSERVATION_ID, ds.PESystemObservation.PEObservationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PE_TEMPLATE_SYSTEM_ID, ds.PESystemObservation.PETemplateSystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.PESystemObservation.CreatedByColumn.ColumnName, DbType.String);

        }

        public DSPhysicalExamECW loadPhysicalExamSystemObservation(long SystemId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, SystemId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_SELECT, ds, ds.PESystemObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamSystemObservation", PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW insertPhysicalExamSystemObservation(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPhysicalExamSystemObservationParameters(dbManager, ds, true);
                ds = (DSPhysicalExamECW)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_INSERT, ds, ds.PESystemObservation.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::insertPhysicalExamSystemObservation", PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_INSERT, ex);
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

        public string deletePhysicalExamSystemObservation(long SystemObservationId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PE_SYSTEM_OBSERVATION_ID, SystemObservationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePhysicalExamSystemObservation", PROC_PHYSICALEXAM_ECW_SYSTEM_OBSERVATION_DELETE, ex);
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


        #endregion


        #region " Lookup's "

        public DSPhysicalExamECWLookup lookupPESystem(string IsActive)
        {
            DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                ds = (DSPhysicalExamECWLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_LOOKUP, ds, ds.PESystemLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::lookupPESystem", PROC_PHYSICALEXAM_ECW_SYSTEM_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECWLookup lookupPEObservation(string IsActive)
        {
            DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                ds = (DSPhysicalExamECWLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_OBSERVATION_LOOKUP, ds, ds.PEObservationLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::lookupPEObservation", PROC_PHYSICALEXAM_ECW_OBSERVATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        #region PE Revamp, MK

        public DSPhysicalExamECW loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = null, int? isSelected = 1)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (templateId == 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (entityId == 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);

                //if (IsActive == null || IsActive < 0)
                //    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                //else
                //    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                if (isSelected == 0)
                    isSelected = null;

                dbManager.AddParameters(2, PARM_ISSELECTED, isSelected);

                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEMSELECT, ds, ds.PETemplateSystem.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamTemplatesECW", PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEMSELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPhysicalExamECWLookup loadPhysicalExamSystemsECW(int? IsActive = 1)
        {
            DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (IsActive == null || IsActive < 0)
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);

                ds = (DSPhysicalExamECWLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_SYSTEM_LOOKUP, ds, ds.PESystemLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamSystemsECW", PROC_PHYSICALEXAM_ECW_SYSTEM_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW loadPhysicalExamSystemObservatiosECW(long templateId, long systemId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (systemId <= 0)
                    dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PE_SYSTEM_ID, systemId);

                dbManager.AddParameters(1, "@RecordCount", ds.PEObservation.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_OBSERVATION_SELECT, ds, ds.PEObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamSystemObservatiosECW", PROC_PHYSICALEXAM_ECW_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW SavePhysicalExamSystemObservatiosECW(DSPhysicalExamECW dsPhysicalExam)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);
                dbManager.AddParameters(0, "@Name", dsPhysicalExam.PETemplateSystemObservations.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(1, "@CreatedBy", dsPhysicalExam.PETemplateSystemObservations.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@CreatedOn", dsPhysicalExam.PETemplateSystemObservations.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(3, "@ModifiedBy", dsPhysicalExam.PETemplateSystemObservations.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@ModifiedOn", dsPhysicalExam.PETemplateSystemObservations.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, "@ProviderXML", dsPhysicalExam.PETemplateSystemObservations.ProviderXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(6, "@SystemObservationXML", dsPhysicalExam.PETemplateSystemObservations.SystemObservationXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(7, "@EntityId", dsPhysicalExam.PETemplateSystemObservations.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, "@TemplatePreview", dsPhysicalExam.PETemplateSystemObservations.TemplatePreviewColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, "@TemplateId", dsPhysicalExam.PETemplateSystemObservations.PETemplateIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(10, "@SpecialtyXML", dsPhysicalExam.PETemplateSystemObservations.SpecialtyXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(11, PARM_BODYPART_ID, dsPhysicalExam.PETemplateSystemObservations.BodyPartIdColumn.ColumnName, DbType.Int64);

                dsPhysicalExam = (DSPhysicalExamECW)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMPLATESAVE, dsPhysicalExam, dsPhysicalExam.PETemplateSystemObservations.TableName);
                dsPhysicalExam.AcceptChanges();
                return dsPhysicalExam;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::SavePhysicalExamSystemObservatiosECW", PROC_PHYSICALEXAM_ECW_TEMPLATESAVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW loadPhysicalExamTempSysObservations(long PETemplateId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PETemplateId < 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, PETemplateId);

                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMP_SYS_OBSERVATION_SELECT, ds, ds.PETemplateSystemObservations.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExam", PROC_PHYSICALEXAM_ECW_TEMP_SYS_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPhysicalExamECW loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = null)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (templateId == 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (entityId == 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);

                if (IsActive == null || IsActive < 0)
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SELECTECW, ds, ds.PETemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamTemplatesECW", PROC_PHYSEXAMTEMPLATE_SELECTECW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string PETemplateIsActive(long templateId, long? IsActive = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, "@PETemplateId", templateId);
                dbManager.AddParameters(1, "@IsActive", IsActive);
                dbManager.AddParameters(2, "@ModifiedBy", Common.Utilities.MDVUtility.DecryptFrom64(Common.Shared.MDVSession.Current.AppUserName));
                var returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_UPDATEACTIVEINACTIVE).ToString();

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::PETemplateIsActive", PROC_PHYSEXAMTEMPLATE_UPDATEACTIVEINACTIVE, ex);
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

        public string deletePETemplate(long templateId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@PETemplateId", templateId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_DELETE).ToString();
                //returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPS_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePETemplate", PROC_PHYSEXAMTEMPLATE_DELETE, ex);
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
        public DSPhysicalExamECW loadPhysicalExamECW(long TemplateId, int? IsActive = 1)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (TemplateId == 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, IsActive);
                if (IsActive == null || IsActive < 0)
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMPLATE_SELECT, ds, ds.PETemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadAllPhysicalExamECW", PROC_PHYSICALEXAM_ECW_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deletePhysicalExamTemplateSystem(long PETemplateSystemId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_SYSTEM_ID, PETemplateSystemId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEM_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePhysicalExamTemplateSystem", PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEM_DELETE, ex);
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

        public DSPhysicalExamECW SavePhysicalExamNotesObservation(string xmlNotesObservation, string AppUserName)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_PE_NOTES_OBSERVATION_XML, xmlNotesObservation);
                dbManager.AddParameters(1, PARM_CREATED_BY, AppUserName);
                dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, AppUserName);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_NOTES_OBSERVATION_INSERT, ds, ds.PENotesObservation.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::SavePhysicalExamNotesObservation", PROC_PHYSICALEXAM_NOTES_OBSERVATION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPhysicalExamECW loadPETempSystemObservation(long TemplateId, long SystemId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateId);
                dbManager.AddParameters(1, PARM_PE_SYSTEM_ID, SystemId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMP_SYSTEM_OBSERVATION_SELECT, ds, ds.PESystemObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPETempSystemObservation", PROC_PHYSICALEXAM_ECW_TEMP_SYSTEM_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW loadPETempSystemObservationNote(long TemplateId, long SystemId, long? NotesId = null)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateId);
                dbManager.AddParameters(1, PARM_PE_SYSTEM_ID, SystemId);
                dbManager.AddParameters(2, PARM_PE_NOTES_ID, NotesId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMP_SYSTEM_OBSERVATION_NOTE_SELECT, ds, new List<string> {
                    ds.PESystemObservation.TableName, ds.PENotesObservation.TableName});
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPETempSystemObservationNote", PROC_PHYSICALEXAM_ECW_TEMP_SYSTEM_OBSERVATION_NOTE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string detachPhysicalExamTemplateFromNotes(long NotesId, long? PETemplateId = 0)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PE_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                if (PETemplateId == 0)
                    dbManager.AddParameters(2, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_TEMPLATE_ID, PETemplateId);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSICALEXAM_NOTES_OBSERVATION_DELETE));

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::detachPhysicalExamTemplateFromNotes", PROC_PHYSICALEXAM_NOTES_OBSERVATION_DELETE, ex);
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
        public DSPhysicalExamECW loadPETempSystemObservationForNotes(long NotesId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PE_NOTES_ID, NotesId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_NOTES_OBSERVATION_SELECT, ds, ds.PENotesObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPETempSystemObservationForNotes", PROC_PHYSICALEXAM_NOTES_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //public DSPhysicalExamECW GetSpecialtyProvider(Int64 SpecialityId)
        //{
        //    DSPhysicalExamECW ds = new DSPhysicalExamECW();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
        //        dbManager.AddParameters(0, "@SpecialtyId", SpecialityId);
        //        ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GETSPECIALTYPROVIDER, ds, ds.SpecialityProvider.TableName);

        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALPhysicalExamECW::GetSpecialtyProvider", PROC_GETSPECIALTYPROVIDER, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        public DSPhysicalExamECW loadPESystemObservationForNotes(long NotesId, long PETemplateSystemId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PE_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_TEMPLATE_SYSTEM_ID, PETemplateSystemId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_NOTES_SYS_OBSERS_SELECT, ds, ds.PENotesSystemObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPETempSystemObservationForNotes", PROC_PHYSICALEXAM_NOTES_SYS_OBSERS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updatePENotesDescription(long PENotesObservationId, string Desr)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_PE_NOTES_OBSERVATION_ID, PENotesObservationId);
                dbManager.AddParameters(1, PARM_PE_NOTES_DESC, Desr);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                var returnVal = Convert.ToString(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSICALEXAM_NOTES_SYS_OBSERS_DESC_UPDATE));
                ds.AcceptChanges();
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updatePENotesDescription", PROC_PHYSICALEXAM_NOTES_SYS_OBSERS_DESC_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPhysicalExamECW insertPETemplateSystem(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPETemplateSystemParameters(dbManager, ds);
                ds = (DSPhysicalExamECW)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEM_INSERT, ds, ds.PETemplateSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::insertPETemplateSystem", PROC_PHYSICALEXAM_ECW_TEMPLATESYSTEM_INSERT, ex);
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

        public DSPhysicalExamECW loadPhysicalExamForProvider(long providerId = 0)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PE_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (providerId <= 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, providerId);

                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAM_TEMPLATEFOR_PROVIDERS_SELECT, ds, ds.PETemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExamForProvider", PROC_PHYSEXAM_TEMPLATEFOR_PROVIDERS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamECW SavePhysicalExamForProvider(DSPhysicalExamECW ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, "@Name", ds.PETemplate.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(1, "@CreatedBy", ds.PETemplate.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@CreatedOn", ds.PETemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(3, "@ModifiedBy", ds.PETemplate.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@ModifiedOn", ds.PETemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, "@ProviderId", ds.PETemplate.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, "@IsActive", ds.PETemplate.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(7, "@EntityId", ds.PETemplate.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, "@PETemplateId", ds.PETemplate.PETemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSPhysicalExamECW)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_INSERT, ds, ds.PETemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::SavePhysicalExamForProvider", PROC_PHYSICALEXAM_INSERT, ex);
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
        public DSPhysicalExamECWLookup GetPETemplate()
        {
            DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamECWLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PE_TEMPLATE_LOOKUP, ds, ds.PETemplateLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::GetPETemplate", PROC_PE_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPhysicalExamECW LoadPhyscialExamForSOAPNote(long PETemplateId)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PETemplateId < 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, PETemplateId);
                
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_ACTIVE_TEMP_SYS_OBSERVATION_SELECT, ds, ds.PETemplateSystemObservations.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::LoadPhyscialExamForSOAPNote", PROC_PHYSICALEXAM_ACTIVE_TEMP_SYS_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam> PhysicalExamDataSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam> objList_PhysicalExam = new List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_TEMPLATE_ID, objCommonSearch.TemplateId));
                using (var reader = dbManager.ExecuteReader(PROC_PHYSICALEXAM_ACTIVE_TEMP_SYS_OBSERVATION_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        MDVision.Model.Clinical.LegacyNotes.PhysicalExam model = new MDVision.Model.Clinical.LegacyNotes.PhysicalExam();
                        var properties = typeof(MDVision.Model.Clinical.LegacyNotes.PhysicalExam).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_PhysicalExam.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::PhysicalExamDataSelect", PROC_PHYSICALEXAM_ACTIVE_TEMP_SYS_OBSERVATION_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_PhysicalExam;
        }
        public DSPhysicalExamECW SelectPhysicalExamPrevNotesObservation(long NotesId, long PrevNotesId, string Username)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_PE_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_PREV_NOTES_ID, PrevNotesId);
                dbManager.AddParameters(2, PARM_CREATED_BY, Username);
                dbManager.AddParameters(3, PARM_CREATED_ON, DateTime.Now);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PE_PREV_NOTES_OBSERVATION_SELECT, ds, ds.PENotesObservation.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::SelectPhysicalExamPrevNotesObservation", PROC_PE_PREV_NOTES_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
