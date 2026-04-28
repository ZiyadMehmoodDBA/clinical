using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALBasicFeeSchedule
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_BASIC_FEE_SCHEDULE_INSERT = "Provider.sp_BasicFeeScheduleInsert";
        private const string PROC_BASIC_FEE_SCHEDULE_UPDATE = "Provider.sp_BasicFeeScheduleUpdate";
        private const string PROC_BASIC_FEE_SCHEDULE_DELETE = "Provider.sp_BasicFeeScheduleDelete";
        private const string PROC_BASIC_FEE_SCHEDULE_SELECT = "Provider.sp_BasicFeeScheduleSelect";

        private const string PROC_MODIFIER_FEE_SCHEDULE_DELETE = "Provider.sp_BasicFeeSchModifierFeeScheduleDelete";
        private const string PROC_MODIFIER_FEE_SCHEDULE_INSERT = "Provider.sp_BasicFeeSchModifierFeeScheduleInsert";
        private const string PROC_MODIFIER_FEE_SCHEDULE_SELECT = "Provider.sp_BasicFeeSchModifierFeeScheduleSelect";
        private const string PROC_MODIFIER_FEE_SCHEDULE_UPDATE = "Provider.sp_BasicFeeSchModifierFeeScheduleUpdate";
        #endregion

        #region "Parameters"
        private const string PARM_BASIC_FEE_SCH_ID = "@BasicFeeSchId";
        private const string PARM_BASIC_FEE_GROUUP_ID = "@BasicFeeGroupId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_FEE = "@Fee";

        private const string PARM_MODIFIER_FEE_SCH_ID = "@BasicFeeSchModifierFeeSchId";
        private const string PARM_MODIFIER_ID = "@ModifierId";
        private const string PARM_IS_REQUIRED = "@IsRequired";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_EXPECTED_FEE = "@ExpectedFee";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALBasicFeeSchedule"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALBasicFeeSchedule()
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
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, ds.BasicFeeSchedule.BasicFeeSchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, ds.BasicFeeSchedule.BasicFeeSchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_BASIC_FEE_GROUUP_ID, ds.BasicFeeSchedule.BasicFeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CPT_CODE, ds.BasicFeeSchedule.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_FEE, ds.BasicFeeSchedule.FeeColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.BasicFeeSchedule.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.BasicFeeSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.BasicFeeSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.BasicFeeSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.BasicFeeSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(10, PARM_EXPECTED_FEE, ds.BasicFeeSchedule.ExpectedFeeColumn.ColumnName, DbType.Decimal);
        }

        /// <summary>
        /// Creates the parameters_ plan_ specific_ information.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_Plan_Specific_Info(IDBManager dbManager, DSFeeSchedule ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MODIFIER_FEE_SCH_ID, ds.BasicFeeSchModifierFeeSchedule.BasicFeeSchModifierFeeSchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MODIFIER_FEE_SCH_ID, ds.BasicFeeSchModifierFeeSchedule.BasicFeeSchModifierFeeSchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_BASIC_FEE_SCH_ID, ds.BasicFeeSchModifierFeeSchedule.BasicFeeSchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_MODIFIER_ID, ds.BasicFeeSchModifierFeeSchedule.ModifierIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FEE, ds.BasicFeeSchModifierFeeSchedule.FeeColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(4, PARM_IS_REQUIRED, ds.BasicFeeSchModifierFeeSchedule.IsRequiredColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.BasicFeeSchedule.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.BasicFeeSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.BasicFeeSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.BasicFeeSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.BasicFeeSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(11, PARM_EXPECTED_FEE, ds.BasicFeeSchModifierFeeSchedule.ExpectedFeeColumn.ColumnName, DbType.Decimal);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the basic fee schedule.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <param name="BasicFeeGroupId">The basic fee group identifier.</param>
        /// <param name="CPTId">The CPT identifier.</param>
        /// <returns></returns>
        public DSFeeSchedule LoadBasicFeeSchedule(long BasicFeeScheduleId, string BasicFeeGroupId, string CPTCode, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (BasicFeeGroupId == "")
                    BasicFeeGroupId = null;

                if (CPTCode == "")
                    CPTCode = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (BasicFeeScheduleId <= 0)
                    dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, BasicFeeScheduleId);
                dbManager.AddParameters(1, PARM_BASIC_FEE_GROUUP_ID, BasicFeeGroupId);
                dbManager.AddParameters(2, PARM_CPT_CODE, CPTCode);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.BasicFeeSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BASIC_FEE_SCHEDULE_SELECT, ds, ds.BasicFeeSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::LoadBasicFeeSchedule", PROC_BASIC_FEE_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the basic fee schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule UpdateBasicFeeSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BasicFeeSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BASIC_FEE_SCHEDULE_UPDATE, ds, ds.BasicFeeSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BasicFeeSchedule.Rows[0][ds.BasicFeeSchedule.BasicFeeSchIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::UpdateBasicFeeSchedule", PROC_BASIC_FEE_SCHEDULE_UPDATE, ex);
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
        /// Deletes the basic fee schedule.
        /// </summary>
        /// <param name="BasicFeeScheduleIds">The basic fee schedule ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteBasicFeeSchedule(string BasicFeeScheduleIds)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSFeeSchedule ds = LoadBasicFeeSchedule(Convert.ToInt64(BasicFeeScheduleIds), null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BasicFeeSchedule;
                dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, BasicFeeScheduleIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BASIC_FEE_SCHEDULE_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.BasicFeeGroup.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BasicFeeSchedule.Rows[0][ds.BasicFeeSchedule.BasicFeeGroupIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::DeleteBasicFeeSchedule", PROC_BASIC_FEE_SCHEDULE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString().Remove(str[1].IndexOf('.'));
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the basic fee schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule InsertBasicFeeSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BasicFeeSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BASIC_FEE_SCHEDULE_INSERT, ds, ds.BasicFeeSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BasicFeeSchedule.Rows[0][ds.BasicFeeSchedule.BasicFeeSchIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::InsertBasicFeeSchedule", PROC_BASIC_FEE_SCHEDULE_INSERT, ex);
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

        #region "Insert, delete, update and get using dataset Functions for BFS Plan Info"
        /// <summary>
        /// Loads the BFS plan.
        /// </summary>
        /// <param name="BFSId">The BFS identifier.</param>
        /// <returns></returns>
        public DSFeeSchedule LoadBFSPlan(long BFSId, long BFSPlanId)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                
                if (BFSId == 0)
                    dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BASIC_FEE_SCH_ID, BFSId);
                if (BFSPlanId == 0)
                    dbManager.AddParameters(1, PARM_MODIFIER_FEE_SCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MODIFIER_FEE_SCH_ID, BFSPlanId);
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODIFIER_FEE_SCHEDULE_SELECT, ds, ds.BasicFeeSchModifierFeeSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::LoadBFSPlan", PROC_MODIFIER_FEE_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the BFS plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule UpdateBFSPlan(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_Plan_Specific_Info(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BasicFeeSchModifierFeeSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MODIFIER_FEE_SCHEDULE_UPDATE, ds, ds.BasicFeeSchModifierFeeSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BasicFeeSchModifierFeeSchedule.Rows[0][ds.BasicFeeSchModifierFeeSchedule.BasicFeeSchModifierFeeSchIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::UpdateBFSPlan", PROC_MODIFIER_FEE_SCHEDULE_UPDATE, ex);
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
        /// Deletes the BFS plan.
        /// </summary>
        /// <param name="BFSPlanId">The BFS plan identifier.</param>
        /// <returns></returns>
        public string DeleteBFSPlan(string BFSPlanId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);          
                dbManager.AddParameters(0, PARM_MODIFIER_FEE_SCH_ID, BFSPlanId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODIFIER_FEE_SCHEDULE_DELETE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::DeleteBFSPlan", PROC_MODIFIER_FEE_SCHEDULE_DELETE, ex);
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
        /// Inserts the BFS plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule InsertBFSPlan(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters_Plan_Specific_Info(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BasicFeeSchModifierFeeSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODIFIER_FEE_SCHEDULE_INSERT, ds, ds.BasicFeeSchModifierFeeSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BasicFeeSchModifierFeeSchedule.Rows[0][ds.BasicFeeSchModifierFeeSchedule.BasicFeeSchModifierFeeSchIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBasicFeeSchedule::InsertBFSPlan", PROC_MODIFIER_FEE_SCHEDULE_INSERT, ex);
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
