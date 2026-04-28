using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;


namespace MDVision.DataAccess.DAL.Admin
{
    public class DALRevenueCode

    { 
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_REVENUE_CODE_INSERT = "Provider.sp_RevenueCodeInsert";
        private const string PROC_REVENUE_CODE_UPDATE = "Provider.sp_RevenueCodeUpdate";
        private const string PROC_REVENUE_CODE_DELETE = "Provider.sp_RevenueCodeDelete";
        private const string PROC_REVENUE_CODE_SELECT = "Provider.sp_RevenueCodeSelect";
        private const string PROC_REVENUE_CODE_LOOKUP = "Provider.sp_RevenueCodeLookup";

        private const string PROC_REVENUE_CODE_PLAN_DELETE = "Provider.sp_RevenuePlanDelete";
        private const string PROC_REVENUE_CODE_PLAN_INSERT = "Provider.sp_RevenuePlanInsert";
        private const string PROC_REVENUE_CODE_PLAN_SELECT = "Provider.sp_RevenuePlanSelect";
        private const string PROC_REVENUE_CODE_PLAN_UPDATE = "Provider.sp_RevenuePlanUpdate";
        #endregion

        #region "Parameters"
        private const string PARM_REVENUE_CODE_ID = "@RevenueCodeId";
        private const string PARM_REVENUE_CODE = "@RevenueCode";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_REVENUE_PLAN_ID = "@RevenuePlanId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion
    
        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the <see cref="DALRevenueCode"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALRevenueCode()
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
        private void CreateParameters(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);
            //dbManager.CreateParameters(ds.Tables[ds.ProcedureCategory.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, ds.RevenueCode.RevenueCodeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, ds.RevenueCode.RevenueCodeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_REVENUE_CODE, ds.RevenueCode.RevenueCodeColumn.ColumnName, DbType.String);
           // dbManager.AddParameters(2, PARM_NAME, ds.RevenueCode.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.RevenueCode.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.RevenueCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.RevenueCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.RevenueCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.RevenueCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.RevenueCode.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ENTITY_ID, ds.RevenueCode.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            
        }

        /// <summary>
        /// Creates the parameters_ plan_ specific_ information.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_Plan_Specific_Info(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REVENUE_PLAN_ID, ds.RevenuePlan.RevenuePlanIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REVENUE_PLAN_ID, ds.RevenuePlan.RevenuePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ID, ds.RevenuePlan.InsurancePlanIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_REVENUE_CODE_ID, ds.RevenuePlan.RevenueCodeIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_NAME, ds.RevenuePlan.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the revenue code.
        /// </summary>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <param name="RevenueCode">The revenue code.</param>
        /// <param name="Description">The description.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public DSCodes LoadRevenueCode(long RevenueCodeId, string RevenueCode, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (RevenueCode == "")
                    RevenueCode = null;

                if (Description == "")
                    Description = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;
                
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (RevenueCodeId <= 0)
                    dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, RevenueCodeId);

                dbManager.AddParameters(1, PARM_REVENUE_CODE, RevenueCode);
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
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.RevenueCode.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_SELECT, ds, ds.RevenueCode.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::LoadRevenueCode", PROC_REVENUE_CODE_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        /// <summary>
        /// Updates the revenue code.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        public DSCodes UpdateRevenueCode(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RevenueCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_UPDATE, ds, ds.RevenueCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.RevenueCode.Rows[0][ds.RevenueCode.RevenueCodeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::UpdateRevenueCode", PROC_REVENUE_CODE_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        /// <summary>
        /// Deletes the revenue code.
        /// </summary>
        /// <param name="RevenueCodeIds">The revenue code ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteRevenueCode(string RevenueCodeIds)
        {
            string returnValue = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadRevenueCode(Convert.ToInt64(RevenueCodeIds), null, null, null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RevenueCode;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, RevenueCodeIds);
               
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVENUE_CODE_DELETE).ToString();
                if (returnValue != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.RevenueCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.RevenueCode.Rows[0][ds.RevenueCode.RevenueCodeIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::DeleteRevenueCode", PROC_REVENUE_CODE_DELETE, ex);
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
        /// Inserts the revenue code.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertRevenueCode(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RevenueCode.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_INSERT, ds, ds.RevenueCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.RevenueCode.Rows[0][ds.RevenueCode.RevenueCodeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::InsertRevenueCode", PROC_REVENUE_CODE_INSERT, ex);
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
        /// Lookups the revenue code.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupRevenueCode()
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_LOOKUP, ds, ds.RevenueCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::LookupRevenueCode", PROC_REVENUE_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "use for transaction with dataset"
       
        #endregion
        #endregion

        #region "Insert, delete, update and get using dataset Functions for Revenue Code Info"
        /// <summary>
        /// Loads the revenue code plan.
        /// </summary>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <returns></returns>
        public DSCodes LoadRevenueCodePlan(long RevenueCodeId,long RevenueCodePlanId)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (RevenueCodeId <= 0)
                    dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REVENUE_CODE_ID, RevenueCodeId);
                if (RevenueCodePlanId <= 0)
                    dbManager.AddParameters(1, PARM_REVENUE_PLAN_ID, null);
                else
                    dbManager.AddParameters(1, PARM_REVENUE_PLAN_ID, RevenueCodePlanId);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_PLAN_SELECT, ds, ds.RevenuePlan.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::LoadRevenueCodePlan", PROC_REVENUE_CODE_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the revenue code plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateRevenueCodePlan(DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RevenuePlan.GetChanges();
                this.CreateParameters_Plan_Specific_Info(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_PLAN_UPDATE, ds, ds.RevenuePlan.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.RevenuePlan.Rows[0][ds.RevenuePlan.RevenuePlanIdColumn].ToString(), ds.RevenuePlan.Rows[0][ds.RevenuePlan.RevenueCodeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::UpdateRevenueCodePlan", PROC_REVENUE_CODE_PLAN_UPDATE, ex);
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
        /// Deletes the revenue code plan.
        /// </summary>
        /// <param name="RevenueCodePlanId">The revenue code plan identifier.</param>
        /// <returns></returns>
        public string DeleteRevenueCodePlan(string RevenueCodePlanId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadRevenueCodePlan(Convert.ToInt64(RevenueCodePlanId), 0);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RevenuePlan;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_REVENUE_PLAN_ID, RevenueCodePlanId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVENUE_CODE_PLAN_DELETE);
                //if (dtTemp != null && ds.RevenuePlan.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.RevenuePlan.Rows[0][ds.RevenuePlan.RevenuePlanIdColumn].ToString(), ds.RevenuePlan.Rows[0][ds.RevenuePlan.RevenueCodeIdColumn].ToString(), false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
            return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::DeleteRevenueCodePlan", PROC_REVENUE_CODE_PLAN_DELETE, ex);
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
        /// Inserts the revenue code plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertRevenueCodePlan(DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RevenuePlan.GetChanges();
                CreateParameters_Plan_Specific_Info(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REVENUE_CODE_PLAN_INSERT, ds, ds.RevenuePlan.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.RevenuePlan.Rows[0][ds.RevenuePlan.RevenuePlanIdColumn].ToString(), ds.RevenuePlan.Rows[0][ds.RevenuePlan.RevenueCodeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::InsertRevenueCodePlan", PROC_REVENUE_CODE_PLAN_INSERT, ex);
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

