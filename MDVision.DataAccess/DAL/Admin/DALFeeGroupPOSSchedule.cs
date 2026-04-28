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
    public class DALFeeGroupPOSSchedule
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_FEE_GROUP_POS_SCHEDULE_INSERT = "Provider.sp_FeeGroupPOSScheduleInsert";
        private const string PROC_FEE_GROUP_POS_SCHEDULE_UPDATE = "Provider.sp_FeeGroupPOSScheduleUpdate";
        private const string PROC_FEE_GROUP_POS_SCHEDULE_DELETE = "Provider.sp_FeeGroupPOSScheduleDelete";
        private const string PROC_FEE_GROUP_POS_SCHEDULE_SELECT = "Provider.sp_FeeGroupPOSScheduleSelect";

        private const string PROC_POS_PLAN_DELETE = "Provider.sp_FeeGroupPOSModifierFeeScheduleDelete";
        private const string PROC_POS_PLAN_INSERT = "Provider.sp_FeeGroupPOSModifierFeeScheduleInsert";
        private const string PROC_POS_PLAN_SELECT = "Provider.sp_FeeGroupPOSModifierFeeScheduleSelect";
        private const string PROC_POS_PLAN_UPDATE = "Provider.sp_FeeGroupPOSModifierFeeScheduleUpdate";
        #endregion

        #region "Parameters"
        private const string PARM_FEE_GROUP_POS_ID = "@FeeGroupPOSId";
        private const string PARM_FEE_GROUP_ID = "@FeeGroupId";
        private const string PARM_PLAN_FEE_LINK = "@PlanFeeLinkId";
    
        private const string PARM_POS_ID = "@POSId";
        private const string PARM_FEE = "@Fee";
        private const string PARM_AUTHORIZATION_REQUIRED = "@AuthorizationRequired";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CPT_CODE = "@CPTCode";

        private const string PARM_POS_MODIFIER_ID = "@FeeGroupPOSModifierFeeSchId";
        private const string PARM_MODIFIER_ID = "@ModifierId";
        private const string PARM_IS_REQUIRED = "@IsRequired";

        private const string PARM_EXPECTED_FEE = "@ExpectedFee";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALFeeGroupPOSSchedule"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALFeeGroupPOSSchedule()
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
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, ds.FeeGroupPOSSchedule.FeeGroupPOSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, ds.FeeGroupPOSSchedule.FeeGroupPOSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_FEE_GROUP_ID, ds.FeeGroupPOSSchedule.FeeGroupIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PLAN_FEE_LINK, ds.FeeGroupPOSSchedule.PlanFeeLinkIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CPT_CODE, ds.FeeGroupPOSSchedule.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_POS_ID, ds.FeeGroupPOSSchedule.POSIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_FEE, ds.FeeGroupPOSSchedule.FeeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_AUTHORIZATION_REQUIRED, ds.FeeGroupPOSSchedule.AuthorizationRequiredColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.FeeGroupPOSSchedule.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.FeeGroupPOSSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.FeeGroupPOSSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.FeeGroupPOSSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.FeeGroupPOSSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_EXPECTED_FEE, ds.FeeGroupPOSSchedule.ExpectedFeeColumn.ColumnName, DbType.Decimal);
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
                dbManager.AddParameters(0, PARM_POS_MODIFIER_ID, ds.PlanPOS.FeeGroupPOSModifierFeeSchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_POS_MODIFIER_ID, ds.PlanPOS.FeeGroupPOSModifierFeeSchIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_FEE_GROUP_POS_ID, ds.PlanPOS.FeeGroupPOSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_MODIFIER_ID, ds.PlanPOS.ModifierIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FEE, ds.PlanPOS.FeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(4, PARM_IS_REQUIRED, ds.PlanPOS.IsRequiredColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.PlanPOS.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.PlanPOS.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.PlanPOS.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.PlanPOS.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.PlanPOS.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_ERROR_MESSAGE, ds.PlanPOS.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_EXPECTED_FEE, ds.PlanPOS.ExpectedFeeColumn.ColumnName, DbType.Decimal);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the fee group pos schedule.
        /// </summary>
        /// <param name="FeeGroupPOSScheduleId">The fee group position schedule identifier.</param>
        /// <param name="FeeGroupId">The fee group identifier.</param>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <param name="POSId">The pos identifier.</param>
        /// <returns></returns>
        public DSFeeSchedule LoadFeeGroupPOSSchedule(long FeeGroupPOSScheduleId, string FeeGroupId, string PlanFeeLinkId, string CPTCode, string POSId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (FeeGroupId == "")
                    FeeGroupId = null;

                if (PlanFeeLinkId == "")
                    PlanFeeLinkId = null;

                if (CPTCode == "")
                    CPTCode = null;

                if (POSId == "")
                    POSId = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(10);

                if (FeeGroupPOSScheduleId <= 0)
                    dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, FeeGroupPOSScheduleId);
                dbManager.AddParameters(1, PARM_FEE_GROUP_ID, FeeGroupId);
                dbManager.AddParameters(2, PARM_PLAN_FEE_LINK, PlanFeeLinkId);
                dbManager.AddParameters(3, PARM_CPT_CODE, CPTCode);
                dbManager.AddParameters(4, PARM_POS_ID, POSId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.FeeGroupPOSSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(9, PARM_IS_ACTIVE, IsActive);
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_POS_SCHEDULE_SELECT, ds, ds.FeeGroupPOSSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::LoadFeeGroupPOSSchedule", PROC_FEE_GROUP_POS_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the fee group position schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule UpdateFeeGroupPOSSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupPOSSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_POS_SCHEDULE_UPDATE, ds, ds.FeeGroupPOSSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupPOSSchedule.Rows[0][ds.FeeGroupPOSSchedule.FeeGroupPOSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::UpdateFeeGroupPOSSchedule", PROC_FEE_GROUP_POS_SCHEDULE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the fee group position schedule.
        /// </summary>
        /// <param name="FeeGroupPOSScheduleIds">The fee group position schedule ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteFeeGroupPOSSchedule(string LoadFeeGroupPOSScheduleId)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSFeeSchedule ds = LoadFeeGroupPOSSchedule(Convert.ToInt64(LoadFeeGroupPOSScheduleId),null, null, null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupPOSSchedule;
                dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, LoadFeeGroupPOSScheduleId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FEE_GROUP_POS_SCHEDULE_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.FeeGroupPOSSchedule.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupPOSSchedule.Rows[0][ds.FeeGroupPOSSchedule.FeeGroupPOSIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::DeleteFeeGroupPOSSchedule", PROC_FEE_GROUP_POS_SCHEDULE_DELETE, ex);
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
        /// Inserts the fee group position schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule InsertFeeGroupPOSSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupPOSSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_POS_SCHEDULE_INSERT, ds, ds.FeeGroupPOSSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupPOSSchedule.Rows[0][ds.FeeGroupPOSSchedule.FeeGroupPOSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::InsertFeeGroupPOSSchedule", PROC_FEE_GROUP_POS_SCHEDULE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions for POS Plan Info"
        public DSFeeSchedule LoadPOSPlan(long POSId, long POSPlanId)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                
                if (POSId == 0)
                    dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FEE_GROUP_POS_ID, POSId);

                if (POSPlanId == 0)
                    dbManager.AddParameters(1, PARM_POS_MODIFIER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_POS_MODIFIER_ID, POSPlanId);


                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_POS_PLAN_SELECT, ds, ds.PlanPOS.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::LoadPOSPlan", PROC_POS_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFeeSchedule UpdatePOSPlan(DSFeeSchedule ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanPOS.GetChanges();
                this.CreateParameters_Plan_Specific_Info(dbManager, ds, false);
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_POS_PLAN_UPDATE, ds, ds.PlanPOS.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanPOS.Rows[0][ds.PlanPOS.FeeGroupPOSModifierFeeSchIdColumn].ToString(), ds.PlanPOS.Rows[0][ds.PlanPOS.FeeGroupPOSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::UpdatePOSPlan", PROC_POS_PLAN_UPDATE, ex);
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

        public string DeletePOSPlan(string POSPlanIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSFeeSchedule ds = LoadPOSPlan(0, Convert.ToInt64(POSPlanIds));
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanPOS;
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_POS_MODIFIER_ID, POSPlanIds);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_POS_PLAN_DELETE);
                //if (dtTemp != null && ds.PlanPOS.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanPOS.Rows[0][ds.PlanPOS.FeeGroupPOSModifierFeeSchIdColumn].ToString(), ds.PlanPOS.Rows[0][ds.PlanPOS.FeeGroupPOSIdColumn].ToString(), false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::DeletePOSPlan", PROC_POS_PLAN_DELETE, ex);
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

        public DSFeeSchedule InsertPOSPlan(DSFeeSchedule ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanPOS.GetChanges();
                CreateParameters_Plan_Specific_Info(dbManager, ds, true);
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_POS_PLAN_INSERT, ds, ds.PlanPOS.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanPOS.Rows[0][ds.PlanPOS.FeeGroupPOSModifierFeeSchIdColumn].ToString(), ds.PlanPOS.Rows[0][ds.PlanPOS.FeeGroupPOSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupPOSSchedule::InsertPOSPlan", PROC_POS_PLAN_INSERT, ex);
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
