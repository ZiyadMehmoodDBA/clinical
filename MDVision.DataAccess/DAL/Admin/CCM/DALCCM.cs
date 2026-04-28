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

using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Admin.CCM
{
    public class DALCCM
    {

        #region Constructors
        public DALCCM()
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

        #region Stored Procedure names
        private const string PROC_CCMTEMPLATE_SELECT = "CCM.SP_TEMPLATESSELECT";
        private const string PROC_CCMICDGroups_SELECT = "CCM.SP_ICDGROUPSSELECT";
        //
        private const string PROC_CCMICDGroupsDetail_SELECT = "CCM.SP_ICDGROUPSDETAILSELECT";

        private const string PROC_CCMCareTeams_SELECT = "CCM.SP_CARETEAMSSELECT";

        private const string PROC_ICDGROUPS_INSERT = "CCM.ICDGROUPSINSERT";
        private const string PROC_ICDGROUPS_UPDATE = "CCM.ICDGROUPSUPDATE";
        private const string PROC_ICDGROUPSMAP_INSERT = "CCM.ICDGROUPSMAPINSERT";
        private const string PROC_ICDGROUPSMAP_DELETE = "CCM.ICDGROUPSMAPDELETE";

        #endregion

        #region Parameter Names

        private const string PARM_Template_ID = "@providerId";

        private const string PARM_ICDGroupId = "@ICDGroupId";
        private const string PARM_ICDGroupName = "@ICDGroupName";
        private const string PARM_ICDGroupDescription = "@ICDGroupDescription";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ICDGroupMapId = "@ICDGroupMapId";
        private const string PARM_ICDCodeId = "@ICDCodeId";

        #endregion

        #region Functions

        #region CCM Templates
        /// <summary>
        /// LoadCCMTemplate
        /// </summary>
        /// <param name="TemplateId"></param>
        /// <param name="TempLookupName"></param>
        /// <param name="ShortName"></param>
        /// <param name="Description"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMTemplate(int TemplateId, string TempLookupName, string ShortName, string Description, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (TemplateId == 0)
                {
                    dbManager.AddParameters(0, "@TemplateId", null);
                }
                else
                {
                    dbManager.AddParameters(0, "@TemplateId", TemplateId);
                }
                dbManager.AddParameters(1, "@TempLookupName", TempLookupName);

                dbManager.AddParameters(2, "@ShortName", ShortName);
                dbManager.AddParameters(3, "@Description", Description);
                dbManager.AddParameters(4, "@IsActive", IsActive);
                dbManager.AddParameters(5, "@PageNumber", PageNumber);
                dbManager.AddParameters(6, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(7, "@RecordCount", ds.Templates.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMTEMPLATE_SELECT, ds, ds.Templates.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMTemplate", PROC_CCMTEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region ICD Groups

        /// <summary>
        /// CreateParameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParameters(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICDGroupId, ds.ICDGroups.ICDGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICDGroupId, ds.ICDGroups.ICDGroupIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ICDGroupName, ds.ICDGroups.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ICDGroupDescription, ds.ICDGroups.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ICDGroups.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ICDGroups.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ICDGroups.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ICDGroups.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ICDGroups.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateParametersICDGroupMap(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(3);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICDGroupMapId, ds.ICDGroupMap.ICDGroupMapIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICDGroupMapId, ds.ICDGroupMap.ICDGroupMapIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ICDCodeId, ds.ICDGroupMap.ICDCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICDGroupId, ds.ICDGroupMap.ICDGroupIdColumn.ColumnName, DbType.Int64);
        }

        /// <summary>
        /// LoadCCMICDGroups
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <param name="ShortName"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMICDGroups(long ICDGroupId, string ShortName, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (ICDGroupId == 0)
                {
                    dbManager.AddParameters(0, "@ICDGroupId", null);
                }
                else
                {
                    dbManager.AddParameters(0, "@ICDGroupId", ICDGroupId);
                }
                dbManager.AddParameters(1, "@ShortName", ShortName);
                dbManager.AddParameters(2, "@IsActive", IsActive);
                dbManager.AddParameters(3, "@PageNumber", PageNumber);
                dbManager.AddParameters(4, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(5, "@RecordCount", ds.Templates.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMICDGroups_SELECT, ds, ds.ICDGroups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMICDGroups", PROC_CCMICDGroups_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCCM LoadCCMICDGroupsDetail(long ICDGroupId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ICDGroupId == 0)
                    dbManager.AddParameters(0, "@ICDGroupId", null);
                else
                    dbManager.AddParameters(0, "@ICDGroupId", ICDGroupId);

                dbManager.AddParameters(1, "@PageNumber", PageNumber);
                dbManager.AddParameters(2, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(3, "@RecordCount", ds.ICDGroupsDetailSelect.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMICDGroupsDetail_SELECT, ds, ds.ICDGroupsDetailSelect.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMICDGroupsDetail", PROC_CCMICDGroupsDetail_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// InsertICDGroup
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSCCM InsertICDGroup(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDGROUPS_INSERT, ds, ds.ICDGroups.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::InsertICDGroup", PROC_ICDGROUPS_INSERT, ex);
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

        public DSCCM UpdateICDGroup(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, false);
                ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDGROUPS_UPDATE, ds, ds.ICDGroups.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::UpdateICDGroup", PROC_ICDGROUPS_UPDATE, ex);
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

        public DSCCM InsertICDGroupMap(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersICDGroupMap(dbManager, ds, true);
                ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDGROUPSMAP_INSERT, ds, ds.ICDGroupMap.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::InsertICDGroupMap", PROC_ICDGROUPSMAP_INSERT, ex);
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

        public string DeleteICDGroupMap(string ICDGroupMapId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ICDGroupMapId, (string.IsNullOrEmpty(ICDGroupMapId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(ICDGroupMapId)));
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPSMAP_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteICDGroupMap", PROC_ICDGROUPSMAP_DELETE, ex);
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

        #region Care Teams

        /// <summary>
        /// LoadCCMCareTeams
        /// </summary>
        /// <param name="CareTeamId"></param>
        /// <param name="ShortName"></param>
        /// <param name="ProviderName"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMCareTeams(long CareTeamId, string ShortName, string ProviderName, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                if (CareTeamId == 0)
                {
                    dbManager.AddParameters(0, "@CareTeamId", null);
                }
                else
                {
                    dbManager.AddParameters(0, "@CareTeamId", CareTeamId);
                }
                dbManager.AddParameters(1, "@ShortName", ShortName);
                dbManager.AddParameters(2, "@ProviderName", ProviderName);
                dbManager.AddParameters(3, "@IsActive", IsActive);
                dbManager.AddParameters(4, "@PageNumber", PageNumber);
                dbManager.AddParameters(5, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(6, "@RecordCount", ds.Templates.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMCareTeams_SELECT, ds, ds.CareTeams.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMCareTeams", PROC_CCMCareTeams_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #endregion
    }
}

