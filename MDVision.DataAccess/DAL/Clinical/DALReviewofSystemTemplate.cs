using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALReviewofSystemTemplate
    {
        #region "Variables"
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_ROS_TEMPLATE_FILL = "Clinical.sp_ROSTemplateFill";
        private const string PROC_ROS_TEMPLATE_SELECT = "Clinical.sp_ROSTemplateSelect";
        private const string PROC_ROS_TEMPLATE_INSERT = "Clinical.sp_ROSTemplateInsert";
        private const string PROC_ROS_TEMPLATE_UPDATE = "Clinical.sp_ROSTemplateUpdate";
        private const string PROC_ROS_TEMPLATE_DELETE = "Clinical.sp_ROSTemplateDelete";

        private const string PROC_ROS_TEMPLATE_SYSTEM_SELECT = "Clinical.sp_ROSTemptSystemSelect";
        private const string PROC_ROS_TEMPLATE_SYSTEM_SELECT_FOR_EDIT = "[Clinical].[sp_ROSTemptSystemSelectForEdit]";
        private const string PROC_ROS_TEMPLATE_SYSTEM_INSERT = "Clinical.sp_ROSTemptSystemInsert";
        private const string PROC_ROS_TEMPLATE_SYSTEM_UPDATE = "Clinical.sp_ROSTemptSystemUpdate";
        private const string PROC_ROS_TEMPLATE_SYSTEM_DELETE = "Clinical.sp_ROSTemptSystemDelete";

        private const string PROC_ROS_TEMPLATE_SYSTEM_CHARC_SELECT = "Clinical.sp_ROSTemptSysCharcSelect";
        private const string PROC_ROS_TEMPLATE_SYSTEM_CHARC_SELECT_FOR_EDIT = "[Clinical].[sp_ROSTemptSysCharcSelectForEdit]";
        private const string PROC_ROS_TEMPLATE_SYSTEM_CHARC_INSERT = "Clinical.sp_ROSTemptSysCharcInsert";
        private const string PROC_ROS_TEMPLATE_SYSTEM_CHARC_UPDATE = "Clinical.sp_ROSTemptSysCharcUpdate";
        private const string PROC_ROS_TEMPLATE_SYSTEM_CHARC_DELETE = "Clinical.sp_ROSTemptSysCharcDelete";

        private const string PROC_ROS_TEMPLATE_NOTE_SELECT = "Clinical.sp_ROSTemptNoteSelect";
        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_ROSSYSTEM_INFO_ID = "@ROSSystemInfoId";
        private const string PARM_SYSTEM_ID = "@ROSSystemId";
        private const string PARM_SYSTEM_CHARCID = "@ROSSystemCharacteristicsId";
        private const string PARM_ROSSYSTEM_PATIENT_ID = "@ROSSystemPatientID";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_NOTE_ID = "@NotesId";

        private const string PARM_CHARAC_NAME = "@CharacteristicsName";
        private const string PARM_SYSTEM_NAME = "@SystemName";
        private const string PARM_SORTING_ORDER = "@SortingOrder";

        private const string PARM_ROS_TEMPLATE_ID = "@ROSTemplateID";
        private const string PARM_TEMPLATE_NAME = "@TemplateName";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_IS_SPECIALTY_ALL = "@IsSpecialtyAll";
        private const string PARM_IS_PROVIDER_ALL = "@IsProviderAll";
        private const string PARM_PROVIDER_NAMES = "@ProviderNames";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_NAMES = "@SpecialtyNames";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region "Constructors"
        public DALReviewofSystemTemplate()
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
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void createParameters(IDBManager dbManager, DSClinicalReviewofSystemTemplate ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, ds.ROSTemplate.ROSTemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, ds.ROSTemplate.ROSTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_TEMPLATE_NAME, ds.ROSTemplate.TemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.ROSTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_CREATED_BY, ds.ROSTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.ROSTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.ROSTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.ROSTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(7, PARM_IS_DEFAULT, ds.ROSTemplate.IsDefaultColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_IS_SPECIALTY_ALL, ds.ROSTemplate.IsSpecialtyAllColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_IS_PROVIDER_ALL, ds.ROSTemplate.IsProviderAllColumn.ColumnName, DbType.Byte);
           // dbManager.AddParameters(10, PARM_PROVIDER_NAMES, ds.ROSTemplate.ProviderNamesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_PROVIDER_IDS, ds.ROSTemplate.ProviderIdsColumn.ColumnName, DbType.String);
         //   dbManager.AddParameters(12, PARM_SPECIALTY_NAMES, ds.ROSTemplate.SpecialtyNamesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SPECIALTY_IDS, ds.ROSTemplate.SpecialtyIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ENTITY_ID, ds.ROSTemplate.EntityIdColumn.ColumnName, DbType.Int64);
            

        }


        private void createParametersSystems(IDBManager dbManager, DSClinicalReviewofSystemTemplate ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SYSTEM_ID, ds.ROSSystem.ROSSystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SYSTEM_ID, ds.ROSSystem.ROSSystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SYSTEM_NAME, ds.ROSSystem.SystemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SORTING_ORDER, ds.ROSSystem.SortingOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_ROS_TEMPLATE_ID, ds.ROSSystem.ROSTemplateIdColumn.ColumnName, DbType.Int64);
        }

        private void createParametersSystemsCharC(IDBManager dbManager, DSClinicalReviewofSystemTemplate ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SYSTEM_CHARCID, ds.ROSSystemCharC.ROSSystemCharacteristicsIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SYSTEM_CHARCID, ds.ROSSystemCharC.ROSSystemCharacteristicsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CHARAC_NAME, ds.ROSSystemCharC.CharacteristicsNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SORTING_ORDER, ds.ROSSystemCharC.SortingOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_SYSTEM_ID, ds.ROSSystemCharC.ROSSystemIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose: To load ROS Systems Templates
        /// Date : March 01, 2016
        #region "CRUD ROS Templates"
        public DSClinicalReviewofSystemTemplate loadROSTemplates(int isActive, long templateID, long pageNumber, long rowsPerPage, long entityID)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);
                if (isActive < 0)
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
                if (templateID <= 0)
                    dbManager.AddParameters(2, PARM_ROS_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ROS_TEMPLATE_ID, templateID);
                if (rowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
                if (entityID <= 0)
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, entityID);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ROSTemplate.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SELECT, ds, ds.ROSTemplate.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadROSTemplates", PROC_ROS_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// Updates the ROSTemplate.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSClinicalReviewofSystemTemplate updateROSTemplate(DSClinicalReviewofSystemTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, false);
                ds = (DSClinicalReviewofSystemTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_UPDATE, ds, ds.ROSTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::UpdateROSTemplate", PROC_ROS_TEMPLATE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the ROSTemplate.
        /// </summary>
        /// <param name="MsgId">The ROSTemplate identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string deleteROSTemplate(string templateID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::DeleteROSTemplate", PROC_ROS_TEMPLATE_DELETE, ex);
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
        /// Inserts the ROSTemplate.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSClinicalReviewofSystemTemplate insertROSTemplate(DSClinicalReviewofSystemTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, true);
                ds = (DSClinicalReviewofSystemTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_INSERT, ds, ds.ROSTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::InsertROSTemplate", PROC_ROS_TEMPLATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
        #region "CRUD ROS Systems Templates"
        public DSClinicalReviewofSystemTemplate loadROSTemplateSystems(long templateID)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (templateID <= 0)
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);

                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_SELECT, ds, ds.ROSSystem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadROSTemplateSystems", PROC_ROS_TEMPLATE_SYSTEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystemTemplate loadROSTemplateSystemsForEdit(long templateID)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (templateID <= 0)
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);

                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_SELECT_FOR_EDIT, ds, ds.ROSSystem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadROSTemplateSystems", PROC_ROS_TEMPLATE_SYSTEM_SELECT_FOR_EDIT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystemTemplate insertROSTemplateSystems(DSClinicalReviewofSystemTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersSystems(dbManager, ds, true);
                ds = (DSClinicalReviewofSystemTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_INSERT, ds, ds.ROSSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::insertROSTemplateSystems", PROC_ROS_TEMPLATE_SYSTEM_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystemTemplate updateROSTemplateSystems(DSClinicalReviewofSystemTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersSystems(dbManager, ds, false);
                ds = (DSClinicalReviewofSystemTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_UPDATE, ds, ds.ROSSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::updateROSTemplateSystems", PROC_ROS_TEMPLATE_SYSTEM_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteROSTemplateSystems(long templateID, long systemID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);
                dbManager.AddParameters(1, PARM_SYSTEM_ID, systemID);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::deleteROSTemplateSystems", PROC_ROS_TEMPLATE_SYSTEM_DELETE, ex);
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
        #region "CRUD ROS Systems Characteristics Templates"
        public DSClinicalReviewofSystemTemplate loadROSTemplateSystemsCharc(long templateID, long ROSSystemId)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (templateID <= 0)
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);

                if (ROSSystemId <= 0)
                    dbManager.AddParameters(1, PARM_SYSTEM_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_SYSTEM_ID, ROSSystemId);

                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_CHARC_SELECT, ds, ds.ROSSystemCharC.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadROSTemplateSystemsCharc", PROC_ROS_TEMPLATE_SYSTEM_CHARC_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystemTemplate loadROSTemplateSystemsCharcForEdit(long templateID, long ROSSystemId)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (templateID <= 0)
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);

                if (ROSSystemId <= 0)
                    dbManager.AddParameters(1, PARM_SYSTEM_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_SYSTEM_ID, ROSSystemId);

                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_CHARC_SELECT_FOR_EDIT, ds, ds.ROSSystemCharC.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::loadROSTemplateSystemsCharc", PROC_ROS_TEMPLATE_SYSTEM_CHARC_SELECT_FOR_EDIT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystemTemplate insertROSTemplateSystemsCharc(DSClinicalReviewofSystemTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersSystemsCharC(dbManager, ds, true);
                ds = (DSClinicalReviewofSystemTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_CHARC_INSERT, ds, ds.ROSSystemCharC.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::insertROSTemplateSystemsCharc", PROC_ROS_TEMPLATE_SYSTEM_CHARC_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystemTemplate updateROSTemplateSystemsCharc(DSClinicalReviewofSystemTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersSystemsCharC(dbManager, ds, false);
                ds = (DSClinicalReviewofSystemTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_CHARC_UPDATE, ds, ds.ROSSystemCharC.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::updateROSTemplateSystemsCharc", PROC_ROS_TEMPLATE_SYSTEM_CHARC_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteROSTemplateSystemsCharc(long ROSSystemCharcId, long systemID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, systemID);
                dbManager.AddParameters(1, PARM_SYSTEM_CHARCID, ROSSystemCharcId);

                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_SYSTEM_CHARC_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::deleteROSTemplateSystemsCharc", PROC_ROS_TEMPLATE_SYSTEM_CHARC_DELETE, ex);
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


        #region ROS LookUP Template against NotesId and UserId
        public DSClinicalReviewofSystemTemplate lookupROSTemplates(long templateID, long UserID, long NotesId,long EntityId)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);
                if (UserID <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, UserID);
                if (templateID <=0)
                {
                    dbManager.AddParameters(1, PARM_ROS_TEMPLATE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ROS_TEMPLATE_ID, templateID);
                }

                if (NotesId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NotesId);

                if (UserID <= 0)
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);

                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_NOTE_SELECT, ds, ds.ROSTemptLookup.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::lookupROSTemplates", PROC_ROS_TEMPLATE_NOTE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public DSClinicalReviewofSystemTemplate fillROSTemplates(long templateID)
        {
            DSClinicalReviewofSystemTemplate ds = new DSClinicalReviewofSystemTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
               
                if (templateID <= 0)
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ROS_TEMPLATE_ID, templateID);


                ds = (DSClinicalReviewofSystemTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_TEMPLATE_FILL, ds, ds.ROSTemplate.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystemTemplate::fillROSTemplates", PROC_ROS_TEMPLATE_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}


