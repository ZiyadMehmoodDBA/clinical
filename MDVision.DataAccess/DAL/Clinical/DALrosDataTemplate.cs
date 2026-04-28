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
    public class DALrosDataTemplate
    {
        #region "Variables"
        
        #endregion
        #region "Constructors"
        public DALrosDataTemplate()
        {
            InitializeComponent();
            DataAccess.DCommon.ClientConfiguration.SetClientObject();
           
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
        private const string PROC_ROS_DATA_TEMPLATE_SELECT = "Clinical.sp_ROSDataTemplateSelect";
        private const string PROC_ROS_DATA_TEMPLATE_INSERT = "Clinical.sp_ROSDataTemplateInsert";
        private const string PROC_ROS_DATA_TEMPLATE_UPDATE = "Clinical.sp_ROSDataTemplateUpdate";
        private const string PROC_ROS_DATA_TEMPLATE_DELETE = "Clinical.sp_ROSDataTemplateDelete";

        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_SELECT = "Clinical.sp_ROSTemptSystemSelect";
        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_INSERT = "Clinical.sp_ROSTemptSystemInsert";
        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_UPDATE = "Clinical.sp_ROSTemptSystemUpdate";
        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_DELETE = "Clinical.sp_ROSTemptSystemDelete";

        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_CHARC_SELECT = "Clinical.sp_ROSTemptSysCharcSelect";
        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_CHARC_INSERT = "Clinical.sp_ROSTemptSysCharcInsert";
        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_CHARC_UPDATE = "Clinical.sp_ROSTemptSysCharcUpdate";
        private const string PROC_ROS_DATA_TEMPLATE_SYSTEM_CHARC_DELETE = "Clinical.sp_ROSTemptSysCharcDelete";

        private const string PROC_ROS_DATA_TEMPLATE_NOTE_SELECT = "Clinical.sp_ROSDataTemptNoteSelect";


        //****** save update
        private const string PROC_ROS_DATA_TEMP_INFO_SELECT = "Clinical.sp_ROSDataTempInfoSelect";
        private const string PROC_ROS_DATA_TEMP_INFO_DELETE = "Clinical.sp_ROSDataTempInfoDelete";
        private const string PROC_ROS_DATA_TEMP_INFO_UPDATE = "Clinical.sp_ROSDataTempInfoUpdate";
        private const string PROC_ROS_DATA_TEMP_INFO_INSERT = "Clinical.sp_ROSDataTempInfoInsert";

        private const string PROC_ROS_DATA_SYSTEM_SELECT = "Clinical.sp_ROSDataSystemSelect";
        private const string PROC_ROS_DATA_SYSTEM_INSERT = "Clinical.sp_ROSDataSystemInsert";
        private const string PROC_ROS_DATA_SYSTEM_UPDATE = "Clinical.sp_ROSDataSystemUpdate";
        private const string PROC_ROS_DATA_SYSTEM_DELETE = "Clinical.sp_ROSDataSystemDelete";

        private const string PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_SELECT = "Clinical.sp_ROSDataSystemCharcSelect";
        private const string PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_UPDATE = "Clinical.sp_ROSDataSystemCharcUpdate";
        private const string PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_INSERT = "Clinical.sp_ROSDataSystemCharcInsert";

        private const string PROC_ROS_DATA_CHARACTERISTICS_DETAILS_SELECT = "Clinical.sp_ROSDataCharcDetailSelect";
        private const string PROC_ROS_DATA_CHARACTERISTICS_DETAILS_INSERT = "Clinical.sp_ROSDataCharcDetailsInsert";
        private const string PROC_ROS_DATA_CHARACTERISTICS_DETAILS_UPDATE = "Clinical.sp_ROSDataCharcDetailsUpdate";
        private const string PROC_ROS_DATA_TEMPLATE_SAVEAS_INSERT = "Clinical.sp_ROSDataTemplateSaveAsInsert";
        private const string PROC_ROS_DATA_TEMPLATE_SAVEAS = "Clinical.sp_ROSDataTemplateSaveAs";
        private const string PROC_ROS_DATA_SYSTEM_RESET = "Clinical.sp_ROSDataSystemReset";
        private const string PROC_ROS_DATA_SYSTEM_CHARC_RESET = "Clinical.sp_ROSDataTempSysCharacReset";

        private const string PROC_ROS_DATA_TEMPLATE_SELECT_FOR_PROVIDER = "Clinical.sp_ROSDataTemplateSelectForProvider";

        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_ROS_DATA_TEMP_INFO_ID = "@ROSDataTempInfoId";
        private const string PARM_ROS_SYSTEM_INFO_ID = "@ROSSystemInfoId";
        private const string PARM_DATA_SYSTEM_ID = "@ROSDataSystemId";

        private const string PARM_DATA_SYSTEM_CHARCID = "@ROSDataSystemCharcID";//"@ROSDataSystemCharacteristicsId";


        private const string PARM_DATA_SYSTEM_CHARC_DETAILSID = "@ROSDataCharcDetailId";

        private const string PARM_ROSSYSTEM_CHARACTERISTICSID = "@ROSSystemCharacteristicsId";
        private const string PARM_ISPOSITIVE = "@IsPositive";
        private const string PARM_PREVIOUSHISTORY = "@PreviousHistory";
        private const string PARM_ROSCHARACTERISTICSDETAIL_STATUSID = "@ROSCharacteristicsDetailStatusId";
        private const string PARM_ONSET = "@Onset";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_ROSCHARACTERISTICSDETAIL_DURATIONID = "@ROSCharacteristicsDetailDurationId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_PATTERNID = "@ROSCharacteristicsDetailPatternId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_SEVERITYID = "@ROSCharacteristicsDetailSeverityId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_COURSEID = "@ROSCharacteristicsDetailCourseId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_RADIATIONID = "@ROSCharacteristicsDetailRadiationId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_FREQUENCYID = "@ROSCharacteristicsDetailFrequencyId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_CONTEXTID = "@ROSCharacteristicsDetailContextId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_CHARACTERCSZID = "@ROSCharacteristicsDetailCharacterCSZId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_AGGRAVEDBYID = "@ROSCharacteristicsDetailAggravedById";
        private const string PARM_ROSCHARACTERISTICSDETAIL_RELIEVEDBYID = "@ROSCharacteristicsDetailRelievedById";
        private const string PARM_LOCATION = "@Location";
        private const string PARM_PRECIPITATEDBY = "@PrecipitatedBY";
        private const string PARM_ASSOCIATEDWITH = "@AssociatedWith";
        private const string PARM_REMOVE_SYSCHARC_DETAILS = "@RemoveSystemCharcDetails";
        private const string PARM_REMOVE_SYSTEMDETAILS = "@removeSystemDetails";

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_SYSTEM_ID = "@ROSSystemId";
        private const string PARM_NOTE_ID = "@NotesId";

        private const string PARM_CHARAC_NAME = "@CharacteristicsName";
        private const string PARM_SYSTEM_NAME = "@SystemName";
        private const string PARM_SORTING_ORDER = "@SortingOrder";

        private const string PARM_ROS_DATA_TEMPLATE_ID = "@ROSDataTemplateID";

        private const string PARM_ROS_TEMPLATE_ID = "@ROSTemplateID";
        private const string PARM_DATA_TEMPLATE_NAME = "@DataTemplateName";
        private const string PARM_TEMPLATE_NAME = "@TemplateName";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_IS_SPECIALTY_ALL = "@IsSpecialtyAll";
        private const string PARM_IS_PROVIDER_ALL = "@IsProviderAll";
        private const string PARM_PROVIDER_NAMES = "@ProviderNames";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_NAMES = "@SpecialtyNames";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ISNORMAL = "@IsNormal";
        private const string PARM_ROSSYSTEM_DATE = "@ROSSystemDate";


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_COPYFROM_ROS_DATA_TEMPLATEID = "@CopyFromROSDataTemplateId";
        private const string PARM_IS_TEMPLATE_CHANGED = "@IsTemplateChanged";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void createParameters(IDBManager dbManager, DSROSDataTemplate ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, ds.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, ds.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(1, PARM_ROS_TEMPLATE_ID, ds.ROSDataTemplate.ROSTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DATA_TEMPLATE_NAME, ds.ROSDataTemplate.DataTemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ROSDataTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ROSDataTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ROSDataTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ROSDataTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ROSDataTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ENTITY_ID, ds.ROSDataTemplate.EntityIdColumn.ColumnName, DbType.Int64);


        }

        private void createParametersROSDataTempInfo(IDBManager dbManager, DSROSDataTemplate ds, Boolean isInsert)
        {
            dbManager.CreateParameters(8);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMP_INFO_ID, ds.ROSDataTempInfo.ROSDataTempInfoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMP_INFO_ID, ds.ROSDataTempInfo.ROSDataTempInfoIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_DESCRIPTION, ds.ROSDataTempInfo.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ROSSYSTEM_DATE, ds.ROSDataTempInfo.ROSSystemDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_ISNORMAL, ds.ROSDataTempInfo.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.ROSDataTempInfo.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ROS_DATA_TEMPLATE_ID, ds.ROSDataTempInfo.ROSDataTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_ROS_TEMPLATE_ID, ds.ROSDataTempInfo.ROSTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_IS_TEMPLATE_CHANGED, ds.ROSDataTempInfo.IsTemplateChangedColumn.ColumnName, DbType.Byte);
        }

        private void createParametersROSDataSystem(IDBManager dbManager, DSROSDataTemplate ds, Boolean isInsert)
        {
            dbManager.CreateParameters(5);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_ID, ds.ROSDataSystem.ROSDataSystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_ID, ds.ROSDataSystem.ROSDataSystemIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SYSTEM_ID, ds.ROSDataSystem.ROSSystemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ROSDataSystem.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ISNORMAL, ds.ROSDataSystem.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_ROS_DATA_TEMP_INFO_ID, ds.ROSDataSystem.ROSDataTempInfoIdColumn.ColumnName, DbType.Int64);

        }

        private void createParametersForROSDataSystemCharacteristics(IDBManager dbManager, DSROSDataTemplate ds, Boolean isInsert)
        {
            dbManager.CreateParameters(5);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, ds.ROSDataSystemCharc.ROSDataSystemCharcIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, ds.ROSDataSystemCharc.ROSDataSystemCharcIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_DATA_SYSTEM_ID, ds.ROSDataSystemCharc.ROSDataSystemIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ROSSYSTEM_CHARACTERISTICSID, ds.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.ROSDataSystemCharc.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ISPOSITIVE, ds.ROSDataSystemCharc.IsPositiveColumn.ColumnName, DbType.Boolean);
        }

        private void createParametersForROSCharacteristicsDetails(IDBManager dbManager, DSROSDataTemplate ds, Boolean isInsert)
        {
            dbManager.CreateParameters(19);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARC_DETAILSID, ds.ROSDataCharcDetail.ROSDataCharcDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARC_DETAILSID, ds.ROSDataCharcDetail.ROSDataCharcDetailIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_DATA_SYSTEM_CHARCID, ds.ROSDataCharcDetail.ROSDataSystemCharcIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PREVIOUSHISTORY, ds.ROSDataCharcDetail.PreviousHistoryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ROSCHARACTERISTICSDETAIL_STATUSID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_ONSET, ds.ROSDataCharcDetail.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DURATION, ds.ROSDataCharcDetail.DurationColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(6, PARM_ROSCHARACTERISTICSDETAIL_DURATIONID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailDurationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_ROSCHARACTERISTICSDETAIL_PATTERNID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailPatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_ROSCHARACTERISTICSDETAIL_SEVERITYID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailSeverityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_ROSCHARACTERISTICSDETAIL_COURSEID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailCourseIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_ROSCHARACTERISTICSDETAIL_RADIATIONID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailRadiationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_ROSCHARACTERISTICSDETAIL_FREQUENCYID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailFrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARM_ROSCHARACTERISTICSDETAIL_CONTEXTID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailContextIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_ROSCHARACTERISTICSDETAIL_CHARACTERCSZID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailCharacterCSZIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(14, PARM_ROSCHARACTERISTICSDETAIL_AGGRAVEDBYID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailAggravedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_ROSCHARACTERISTICSDETAIL_RELIEVEDBYID, ds.ROSDataCharcDetail.ROSCharacteristicsDetailRelievedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(16, PARM_LOCATION, ds.ROSDataCharcDetail.LocationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_PRECIPITATEDBY, ds.ROSDataCharcDetail.PrecipitatedBYColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ASSOCIATEDWITH, ds.ROSDataCharcDetail.AssociatedWithColumn.ColumnName, DbType.String);
            // dbManager.AddParameters(24, PARM_SOAPTEXT, "", DbType.String);
        }

        private void createParametersSaveAs(IDBManager dbManager, DSROSDataTemplate ds, Boolean IsInsert, long ROSSystemInfo)
        {
            int paramcount = 13;
            if (ROSSystemInfo > 0)
            {
                paramcount++;
            }
            dbManager.CreateParameters(paramcount);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, ds.ROSDataTemplateSaveAs.ROSDataTemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, ds.ROSDataTemplateSaveAs.ROSDataTemplateIdColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(1, PARM_ROS_TEMPLATE_ID, ds.ROSDataTemplateSaveAs.ROSTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DATA_TEMPLATE_NAME, ds.ROSDataTemplateSaveAs.DataTemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_COMMENTS, ds.ROSDataTemplateSaveAs.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ROSSYSTEM_DATE, ds.ROSDataTemplateSaveAs.ROSSystemDateColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.ROSDataTemplateSaveAs.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.ROSDataTemplateSaveAs.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.ROSDataTemplateSaveAs.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.ROSDataTemplateSaveAs.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.ROSDataTemplateSaveAs.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds.ROSDataTemplateSaveAs.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_COPYFROM_ROS_DATA_TEMPLATEID, ds.ROSDataTemplateSaveAs.CopyFromROSDataTemplateIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ROS_DATA_TEMP_INFO_ID, ds.ROSDataTemplateSaveAs.ROSDataTempInfoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            if (ROSSystemInfo > 0)
            {
                dbManager.AddParameters(13, PARM_ROS_SYSTEM_INFO_ID, ds.ROSDataTemplateSaveAs.ROSSystemInfoIdColumn.ColumnName.ToString(), DbType.Int64);
            }
        }

        #endregion
        #region saveAs
        public DSROSDataTemplate saveAsROSDataTemplate(DSROSDataTemplate ds, long ROSSystemInfo)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersSaveAs(dbManager, ds, true, ROSSystemInfo);
                if (ROSSystemInfo > 0)
                {
                    ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_SAVEAS, ds, ds.ROSDataTemplateSaveAs.TableName);
                }
                else
                {
                    ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_SAVEAS_INSERT, ds, ds.ROSDataTemplateSaveAs.TableName);
                }

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::saveAsROSDataTemplate", PROC_ROS_DATA_TEMPLATE_SAVEAS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        #region "CRUD ROS Templates"
        public DSROSDataTemplate loadROSDataTemplates(int? isActive, long templateID, long rosDataTemplateID, long pageNumber, long rowsPerPage, long entityID)
        {
            DSROSDataTemplate ds = new DSROSDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);
                if (isActive == null)
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);
                }

                if (templateID <= 0)
                    dbManager.AddParameters(1, PARM_ROS_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROS_TEMPLATE_ID, templateID);
                if (rosDataTemplateID <= 0)
                    dbManager.AddParameters(2, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ROS_DATA_TEMPLATE_ID, rosDataTemplateID);
                if (pageNumber <= 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage <= 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, rowsPerPage);
                if (entityID <= 0)
                    dbManager.AddParameters(5, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(5, PARM_ENTITY_ID, entityID);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ROSDataTemplate.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSROSDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_SELECT, ds, ds.ROSDataTemplate.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::loadROSDataTemplates", PROC_ROS_DATA_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSROSDataTemplate updateROSDataTemplate(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, false);
                ds = (DSROSDataTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_UPDATE, ds, ds.ROSDataTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::updateROSDataTemplate", PROC_ROS_DATA_TEMPLATE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSROSDataTemplate insertROSDataTemplate(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, true);
                ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_INSERT, ds, ds.ROSDataTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::insertROSDataTemplate", PROC_ROS_DATA_TEMPLATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string deleteROSDataTemplate(string rosDataTemplateID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, rosDataTemplateID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::deleteROSDataTemplate", PROC_ROS_DATA_TEMPLATE_DELETE, ex);
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
        #region "ROS Data Template Info Insert/Update/Delete/Select"
        public DSROSDataTemplate loadROSDataTempInfo(long ROSDataTempInfoId, long ROSDataTemplateId)
        {
            DSROSDataTemplate ds = new DSROSDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);

                if (ROSDataTempInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROS_DATA_TEMP_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROS_DATA_TEMP_INFO_ID, ROSDataTempInfoId);

                ds = (DSROSDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMP_INFO_SELECT, ds, ds.ROSDataTempInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::loadROSDataTempInfo", PROC_ROS_DATA_TEMP_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for Insert ROs SystemInfo.
        /// Date : March 01, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSROSDataTemplate insertROSDataTempInfo(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSDataTempInfo(dbManager, ds, true);
                ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMP_INFO_INSERT, ds, ds.ROSDataTempInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::insertROSDataTempInfo", PROC_ROS_DATA_TEMP_INFO_INSERT, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for update Ros Sytem Info.
        /// Date : March 01, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSROSDataTemplate updateROSDataTempInfo(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSDataTempInfo(dbManager, ds, false);
                ds = (DSROSDataTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMP_INFO_UPDATE, ds, ds.ROSDataTempInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::updateROSDataTempInfo", PROC_ROS_DATA_TEMP_INFO_UPDATE, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for delete ROS System info.
        /// Date : March 01, 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public string deleteROSDataTempInfo(long ROSDataTempInfoId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROS_DATA_TEMP_INFO_ID, ROSDataTempInfoId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_DATA_TEMP_INFO_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::deleteROSDataTempInfo", PROC_ROS_DATA_TEMP_INFO_DELETE, ex);
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

        public DSROSDataTemplate loadROSDataSystem(long userId, long ROSDataSystemId, long ROSDataTempInfoId)
        {
            DSROSDataTemplate ds = new DSROSDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);
                if (ROSDataSystemId <= 0)
                    dbManager.AddParameters(1, PARM_DATA_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_DATA_SYSTEM_ID, ROSDataSystemId);

                if (ROSDataTempInfoId <= 0)
                    dbManager.AddParameters(2, PARM_ROS_DATA_TEMP_INFO_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ROS_DATA_TEMP_INFO_ID, ROSDataTempInfoId);

                ds = (DSROSDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_SELECT, ds, ds.ROSDataSystem.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::loadROSDataSystem", PROC_ROS_DATA_SYSTEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSROSDataTemplate insertROSDataSystem(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSDataSystem(dbManager, ds, true);
                ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_INSERT, ds, ds.ROSDataSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::insertROSDataSystem", PROC_ROS_DATA_SYSTEM_INSERT, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for update Ros Sytem Info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSROSDataTemplate updateROSDataSystem(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSDataSystem(dbManager, ds, false);
                ds = (DSROSDataTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_UPDATE, ds, ds.ROSDataSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::updateROSSystemPatient", PROC_ROS_DATA_SYSTEM_UPDATE, ex);
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


        #region ROS SystemPatient Characteristics AND Detail (Select, Insert, Update, Ddelete)
        /// <summary>
        ///  Author: Khaleel Ur Rehman.
        // Purpose : function to load ROSSystem Patient Characteristics.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="rOSSystemPatientCharacteristicsId"></param>
        /// <returns></returns>
        public DSROSDataTemplate loadROSDataSystemCharacteristics(long ROSDataTemplateId, long rOSROSDataSystemCharacteristicsId, long ROSDataSystemID)
        {
            DSROSDataTemplate ds = new DSROSDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (rOSROSDataSystemCharacteristicsId <= 0)
                    dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, rOSROSDataSystemCharacteristicsId);
                if (ROSDataSystemID <= 0)
                    dbManager.AddParameters(1, PARM_DATA_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_DATA_SYSTEM_ID, ROSDataSystemID);
                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(1, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);
                ds = (DSROSDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_SELECT, ds, ds.ROSDataSystemCharc.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::loadROSDataSystemCharacteristics", PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSROSDataTemplate updateROSDataSystemCharacteristics(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSDataSystemCharacteristics(dbManager, ds, false);
                ds = (DSROSDataTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_UPDATE, ds, ds.ROSDataSystemCharc.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::updateROSDataSystemCharacteristics", PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_UPDATE, ex);
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

        public DSROSDataTemplate insertROSDataSystemCharacteristics(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSDataSystemCharacteristics(dbManager, ds, true);
                ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_INSERT, ds, ds.ROSDataSystemCharc.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::insertROSDataSystemCharacteristics", PROC_ROS_DATA_SYSTEM_CHARACTERISTICS_INSERT, ex);
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
        #region details
        public DSROSDataTemplate loadROSDataSystemCharacDetails(long rOSSystemPatientCharacteristicsID, long rOSCharacteristicsDetailsId)
        {
            DSROSDataTemplate ds = new DSROSDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);



                if (rOSSystemPatientCharacteristicsID <= 0)
                    dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, rOSSystemPatientCharacteristicsID);

                if (rOSCharacteristicsDetailsId <= 0)
                    dbManager.AddParameters(1, PARM_DATA_SYSTEM_CHARC_DETAILSID, null);
                else
                    dbManager.AddParameters(1, PARM_DATA_SYSTEM_CHARC_DETAILSID, rOSCharacteristicsDetailsId);
                ds = (DSROSDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_CHARACTERISTICS_DETAILS_SELECT, ds, ds.ROSDataCharcDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::loadROSDataSystemCharacDetails", PROC_ROS_DATA_CHARACTERISTICS_DETAILS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSROSDataTemplate insertROSDataCharcDetail(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSCharacteristicsDetails(dbManager, ds, true);
                ds = (DSROSDataTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_CHARACTERISTICS_DETAILS_INSERT, ds, ds.ROSDataCharcDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::insertROSDataCharcDetail", PROC_ROS_DATA_CHARACTERISTICS_DETAILS_INSERT, ex);
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

        public DSROSDataTemplate updateROSDataCharcDetail(DSROSDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSCharacteristicsDetails(dbManager, ds, false);
                ds = (DSROSDataTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_CHARACTERISTICS_DETAILS_UPDATE, ds, ds.ROSDataCharcDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::updateROSDataCharcDetail", PROC_ROS_DATA_CHARACTERISTICS_DETAILS_UPDATE, ex);
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

        public string ROSDataSystemReset(long ROSDataSystemID, bool removeSystemDetails)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = ROSDataSystemID.ToString();
                //DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (ROSDataSystemID <= 0)
                {
                    dbManager.AddParameters(0, PARM_DATA_SYSTEM_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_DATA_SYSTEM_ID, ROSDataSystemID);
                }
                //

                dbManager.AddParameters(1, PARM_REMOVE_SYSTEMDETAILS, removeSystemDetails);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_RESET).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::ROSDataSystemReset", PROC_ROS_DATA_SYSTEM_RESET, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region "Delete Characteristics Details Info"
        public string deleteRosDataCharacteristicsDetails(long rOSDataSystemCharcID, bool removeSystemCharcDetails)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_DATA_SYSTEM_CHARCID, rOSDataSystemCharcID);
                dbManager.AddParameters(1, PARM_REMOVE_SYSCHARC_DETAILS, removeSystemCharcDetails);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_DATA_SYSTEM_CHARC_RESET).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::deleteRosDataCharacteristicsDetails", PROC_ROS_DATA_SYSTEM_CHARC_RESET, ex);
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


        #region "ROS Data template Provider base"

        public DSROSDataTemplate loadROSDataTemplateForProvider(long providerId = 0)
        {
            DSROSDataTemplate ds = new DSROSDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (providerId <= 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, providerId);

                ds = (DSROSDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_SELECT_FOR_PROVIDER, ds, ds.ROSDataTemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALrosDataTemplate::loadROSDataTemplateForProvider", PROC_ROS_DATA_TEMPLATE_SELECT_FOR_PROVIDER, ex);
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
