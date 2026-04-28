using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALFeeGroup
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_FEE_GROUP_INSERT = "Provider.sp_FeeGroupInsert";
        private const string PROC_FEE_GROUP_UPDATE = "Provider.sp_FeeGroupUpdate";
        private const string PROC_FEE_GROUP_DELETE = "Provider.sp_FeeGroupDelete";
        private const string PROC_FEE_GROUP_SELECT = "Provider.sp_FeeGroupSelect";
        private const string PROC_FEE_GROUP_LOOKUP = "Provider.sp_FeeGroupLookup";
        #endregion

        #region "Parameters"
        private const string PARM_FEE_GROUP_ID = "@FeeGroupId";
        private const string PARM_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_NEXT_FEE_GROUP = "@NextFeeGroupId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPlanFeeLink"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALFeeGroup()
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
        private void CreateParameters(IDBManager dbManager, DSFeeSchedule ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FEE_GROUP_ID, ds.FeeGroup.FeeGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FEE_GROUP_ID, ds.FeeGroup.FeeGroupIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NAME, ds.FeeGroup.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.FeeGroup.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_END_DATE, ds.FeeGroup.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_NEXT_FEE_GROUP, ds.FeeGroup.NextFeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.FeeGroup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.FeeGroup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.FeeGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.FeeGroup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.FeeGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds.FeeGroup.EntityIdColumn.ColumnName, DbType.Int64); 
            dbManager.AddParameters(11, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the plan FeeGroup link.
        /// </summary>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="ShortName">The name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSFeeSchedule LoadFeeGroup(long FeeGroupId, string Name, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (Name == "")
                    Name = null;

                if (Description == "")
                    Description = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (FeeGroupId <= 0)
                    dbManager.AddParameters(0, PARM_FEE_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FEE_GROUP_ID, FeeGroupId);
                dbManager.AddParameters(1, PARM_NAME, Name);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.FeeGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_SELECT, ds, ds.FeeGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroup::LoadFeeGroup", PROC_FEE_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the FeeGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule UpdateFeeGroup(ref DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroup.GetChanges();
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_UPDATE, ds, ds.FeeGroup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroup.Rows[0][ds.FeeGroup.FeeGroupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroup::UpdateFeeGroup", PROC_FEE_GROUP_UPDATE, ex);
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
        /// Deletes the FeeGroup.
        /// </summary>
        /// <param name="FeeGroupId">The plan fee link identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteFeeGroup(string FeeGroupId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSFeeSchedule ds = LoadFeeGroup(Convert.ToInt64(FeeGroupId), null, null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroup;
                dbManager.AddParameters(0, PARM_FEE_GROUP_ID, FeeGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FEE_GROUP_DELETE);

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }

                //else
                //{
                //    if (dtTemp != null && ds.FeeGroup.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroup.Rows[0][ds.FeeGroup.FeeGroupIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroup::DeleteFeeGroup", PROC_FEE_GROUP_DELETE, ex);
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
        /// Inserts the plan fee link.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule InsertFeeGroup(ref DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroup.GetChanges();
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_INSERT, ds, ds.FeeGroup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroup.Rows[0][ds.FeeGroup.FeeGroupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroup::InsertFeeGroup", PROC_FEE_GROUP_INSERT, ex);
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

        public DSFeeScheduleLookup LookupFeeGroup()
        {
            DSFeeScheduleLookup ds = new DSFeeScheduleLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //dbManager.CreateParameters(1);

                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSFeeScheduleLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_LOOKUP, ds, ds.FeeGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroup::LookupFeeGroup", PROC_FEE_GROUP_LOOKUP, ex);
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

