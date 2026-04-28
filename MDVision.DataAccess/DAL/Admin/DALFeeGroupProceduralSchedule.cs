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
    public class DALFeeGroupProceduralSchedule
    {
        #region Variable
        
        #endregion
        
        #region "Stored Procedure Names"
        private const string PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_INSERT = "Provider.sp_FeeGroupProceduralScheduleInsert";
        private const string PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_UPDATE = "Provider.sp_FeeGroupProceduralScheduleUpdate";
        private const string PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_DELETE = "Provider.sp_FeeGroupProceduralScheduleDelete";
        private const string PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_SELECT = "Provider.sp_FeeGroupProceduralScheduleSelect";

        private const string PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_DELETE = "Provider.sp_FeeGroupProcModifierFeeScheduleDelete";
        private const string PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_INSERT = "Provider.sp_FeeGroupProcModifierFeeScheduleInsert";
        private const string PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_SELECT = "Provider.sp_FeeGroupProcModifierFeeScheduleSelect";
        private const string PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_UPDATE = "Provider.sp_FeeGroupProcModifierFeeScheduleUpdate";
        #endregion

        #region "Parameters"
        private const string PARM_FEE_GROUUP_PROC_ID = "@FeeGroupProcId";
        private const string PARM_FEE_GROUP_ID = "@FeeGroupId";
        private const string PARM_PLAN_FEE_LINK = "@PlanFeeLinkId";
        private const string PARM_FEE = "@Fee";
        private const string PARM_AUTHORIZATION_REQUIRED = "@AuthorizationRequired";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CPT_CODE= "@CPTCode";

     
        private const string PARM_MODIFIER_ID = "@ModifierId";
        private const string PARM_FEE_GROUP_MODIFIER_ID = "@FeeGroupProcModifierFeeSchId";
        private const string PARM_IS_REQUIRED = "@IsRequired";

        private const string PARM_EXPECTED_FEE = "@ExpectedFee";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALFeeGroupProceduralSchedule"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALFeeGroupProceduralSchedule()
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
                dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, ds.FeeGroupProceduralSchedule.FeeGroupProcIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, ds.FeeGroupProceduralSchedule.FeeGroupProcIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_FEE_GROUP_ID, ds.FeeGroupProceduralSchedule.FeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PLAN_FEE_LINK, ds.FeeGroupProceduralSchedule.PlanFeeLinkIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_CPT_CODE, ds.FeeGroupProceduralSchedule.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_FEE, ds.FeeGroupProceduralSchedule.FeeColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(5, PARM_AUTHORIZATION_REQUIRED, ds.FeeGroupProceduralSchedule.AuthorizationRequiredColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.FeeGroupProceduralSchedule.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.FeeGroupProceduralSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.FeeGroupProceduralSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.FeeGroupProceduralSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.FeeGroupProceduralSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_EXPECTED_FEE, ds.FeeGroupProceduralSchedule.ExpectedFeeColumn.ColumnName, DbType.Decimal);
        }

        /// <summary>
        /// Creates the CreateParameters_Plan_Modifier_Info.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_Plan_Modifier_Info(IDBManager dbManager, DSFeeSchedule ds, Boolean IsInsert)
        {
            //dbManager.CreateParameters(ds.Tables[ds.FeeGroupProcModifierFeeSchedule.TableName].Columns.Count);
            dbManager.CreateParameters(12);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FEE_GROUP_MODIFIER_ID, ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcModifierFeeSchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FEE_GROUP_MODIFIER_ID, ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcModifierFeeSchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_FEE_GROUUP_PROC_ID, ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_MODIFIER_ID, ds.FeeGroupProcModifierFeeSchedule.ModifierIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FEE, ds.FeeGroupProcModifierFeeSchedule.FeeColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(4, PARM_IS_REQUIRED, ds.FeeGroupProcModifierFeeSchedule.IsRequiredColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.FeeGroupProcModifierFeeSchedule.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.FeeGroupProcModifierFeeSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.FeeGroupProcModifierFeeSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.FeeGroupProcModifierFeeSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.FeeGroupProcModifierFeeSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_ERROR_MESSAGE, ds.FeeGroupProcModifierFeeSchedule.ErrorMessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_EXPECTED_FEE, ds.FeeGroupProcModifierFeeSchedule.ExpectedFeeColumn.ColumnName, DbType.Decimal);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the fee group procedural schedule.
        /// </summary>
        /// <param name="FeeGroupProcSchId">The fee group proc SCH identifier.</param>
        /// <param name="FeeGroupId">The fee group identifier.</param>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        public DSFeeSchedule LoadFeeGroupProceduralSchedule(long FeeGroupProcSchId, string FeeGroupId, string PlanFeeLinkId, string CPTCode, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
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

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (FeeGroupProcSchId <= 0)
                    dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, FeeGroupProcSchId);
                dbManager.AddParameters(1, PARM_FEE_GROUP_ID, FeeGroupId);
                dbManager.AddParameters(2, PARM_PLAN_FEE_LINK, PlanFeeLinkId);
                dbManager.AddParameters(3, PARM_CPT_CODE, CPTCode);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.FeeGroupProceduralSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_SELECT, ds, ds.FeeGroupProceduralSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::LoadFeeGroupProceduralSchedule", PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the fee group procedural schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule UpdateFeeGroupProceduralSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupProceduralSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_UPDATE, ds, ds.FeeGroupProceduralSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null )
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupProceduralSchedule.Rows[0][ds.FeeGroupProceduralSchedule.FeeGroupProcIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::UpdateFeeGroupProceduralSchedule", PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the fee group procedural schedule.
        /// </summary>
        /// <param name="FeeGroupProceduralScheduleIds">The fee group procedural schedule ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteFeeGroupProceduralSchedule(string FeeGroupProceduralScheduleId)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSFeeSchedule ds = LoadFeeGroupProceduralSchedule(Convert.ToInt64(FeeGroupProceduralScheduleId),null, null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupProceduralSchedule;
                dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, FeeGroupProceduralScheduleId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);               
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.FeeGroupProceduralSchedule.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupProceduralSchedule.Rows[0][ds.FeeGroupProceduralSchedule.FeeGroupProcIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }

                //}
                   

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::DeleteFeeGroupProceduralSchedule", PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_DELETE, ex);
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
        /// Inserts the fee group procedural schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule InsertFeeGroupProceduralSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupProceduralSchedule.GetChanges();
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_INSERT, ds, ds.FeeGroupProceduralSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupProceduralSchedule.Rows[0][ds.FeeGroupProceduralSchedule.FeeGroupProcIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::InsertFeeGroupProceduralSchedule", PROC_FEE_GROUP_PROCEDURAL_SCHEDULE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions for Fee Group Procedure Plan Info"
        public DSFeeSchedule LoadFeeGroupProceduralModifierSchedule(long FeeGroupProcId,long FeeGroupProcModifierFeeSchId)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (FeeGroupProcId <= 0)
                    dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FEE_GROUUP_PROC_ID, FeeGroupProcId);
                if (FeeGroupProcModifierFeeSchId <= 0)
                    dbManager.AddParameters(1, PARM_FEE_GROUP_MODIFIER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FEE_GROUP_MODIFIER_ID, FeeGroupProcModifierFeeSchId);
                
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_SELECT, ds, ds.FeeGroupProcModifierFeeSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::LoadFeeGroupProceduralModifierSchedule", PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFeeSchedule UpdateFeeGroupProceduralModifierSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupProcModifierFeeSchedule.GetChanges();
                this.CreateParameters_Plan_Modifier_Info(dbManager, ds, false);
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_UPDATE, ds, ds.FeeGroupProcModifierFeeSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupProcModifierFeeSchedule.Rows[0][ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcModifierFeeSchIdColumn].ToString(), ds.FeeGroupProcModifierFeeSchedule.Rows[0][ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::UpdateFeeGroupProceduralModifierSchedule", PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_UPDATE, ex);
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

        public string DeleteFeeGroupProceduralModifierSchedule(string FeeGroupProcModifierFeeSchId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFeeSchedule ds = LoadFeeGroupProceduralModifierSchedule(0, Convert.ToInt64(FeeGroupProcModifierFeeSchId));
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupProcModifierFeeSchedule;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FEE_GROUP_MODIFIER_ID, FeeGroupProcModifierFeeSchId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_DELETE);

                //if (dtTemp != null && ds.FeeGroupProcModifierFeeSchedule.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupProcModifierFeeSchedule.Rows[0][ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcModifierFeeSchIdColumn].ToString(), ds.FeeGroupProcModifierFeeSchedule.Rows[0][ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcIdColumn].ToString(), false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
            return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::DeleteFeeGroupProceduralModifierSchedule", PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_DELETE, ex);
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

        public DSFeeSchedule InsertFeeGroupProceduralModifierSchedule(DSFeeSchedule ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FeeGroupProcModifierFeeSchedule.GetChanges();
                CreateParameters_Plan_Modifier_Info(dbManager, ds, true);
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_INSERT, ds, ds.FeeGroupProcModifierFeeSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FeeGroupProcModifierFeeSchedule.Rows[0][ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcModifierFeeSchIdColumn].ToString(), ds.FeeGroupProcModifierFeeSchedule.Rows[0][ds.FeeGroupProcModifierFeeSchedule.FeeGroupProcIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFeeGroupProceduralSchedule::InsertFeeGroupProceduralModifierSchedule", PROC_GROUP_PROCEDURAL_MODIFIER_SCHEDULE_INSERT, ex);
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
