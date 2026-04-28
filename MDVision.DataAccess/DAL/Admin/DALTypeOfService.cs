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
    public class DALTypeOfService
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_TYPE_OF_SERVICE_INSERT = "Provider.sp_TypeOfServiceInsert";
        private const string PROC_TYPE_OF_SERVICE_UPDATE = "Provider.sp_TypeOfServiceUpdate";
        private const string PROC_TYPE_OF_SERVICE_DELETE = "Provider.sp_TypeOfServiceDelete";
        private const string PROC_TYPE_OF_SERVICE_SELECT = "Provider.sp_TypeOfServiceSelect";
        private const string PROC_TYPE_OF_SERVICE_LOOKUP = "Provider.sp_TypeOfServiceLookup";

        private const string PROC_TOS_PLAN_DELETE = "Provider.sp_PlanOfTOSDelete";
        private const string PROC_TOS_PLAN_INSERT = "Provider.sp_PlanOfTOSInsert";
        private const string PROC_TOS_PLAN_SELECT = "Provider.sp_PlanOfTOSSelect";
        private const string PROC_TOS_PLAN_UPDATE = "Provider.sp_PlanOfTOSUpdate";
        #endregion

        #region "Parameters"
        private const string PARM_TOS_ID = "@TOSId";
        private const string PARM_TYPE_OF_SERVICE_CODE = "@TypeOfServiceCode";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";

        private const string PARM_PLAN_OF_TOS_ID = "@PlanOfTOSId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_PLAN_NAME = "@Name";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALTypeOfService"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALTypeOfService()
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
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TOS_ID, ds.TypeOfService.TOSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TOS_ID, ds.TypeOfService.TOSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_TYPE_OF_SERVICE_CODE, ds.TypeOfService.TypeOfServiceCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_NAME, ds.TypeOfService.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.TypeOfService.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.TypeOfService.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.TypeOfService.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.TypeOfService.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.TypeOfService.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.TypeOfService.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        /// <summary>
        /// Creates the parameters_ plan_ specific_ information.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_Plan_Specific_Info(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PLAN_OF_TOS_ID, ds.PlanOfTOS.PlanOfTOSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PLAN_OF_TOS_ID, ds.PlanOfTOS.PlanOfTOSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ID, ds.PlanOfTOS.InsurancePlanIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PLAN_NAME, ds.PlanOfTOS.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_TOS_ID, ds.PlanOfTOS.TypeOfServiceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.TypeOfService.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.TypeOfService.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.TypeOfService.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.TypeOfService.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.TypeOfService.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the type of service.
        /// </summary>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <param name="Code">The code.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSCodes LoadTypeOfService(long TypeOfServiceId, string Code, string Name, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (Code == "")
                    Code = null;

                if (Name == "")
                    Name = null;

                if (Description == "")
                    Description = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(8);

                if (TypeOfServiceId <= 0)
                    dbManager.AddParameters(0, PARM_TOS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TOS_ID, TypeOfServiceId);
                dbManager.AddParameters(1, PARM_TYPE_OF_SERVICE_CODE, Code);
                dbManager.AddParameters(2, PARM_NAME, Name);
                dbManager.AddParameters(3, PARM_DESCRIPTION, Description);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.TypeOfService.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TYPE_OF_SERVICE_SELECT, ds, ds.TypeOfService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::LoadTypeOfService", PROC_TYPE_OF_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the type of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateTypeOfService(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                 // PRD-92
                 dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.TypeOfService.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_TYPE_OF_SERVICE_UPDATE, ds, ds.TypeOfService.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.TypeOfService.Rows[0][ds.TypeOfService.TOSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::UpdateTypeOfService", PROC_TYPE_OF_SERVICE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the type of service.
        /// </summary>
        /// <param name="TypeOfServiceIds">The type of service ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteTypeOfService(string TypeOfServiceIds)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadTypeOfService(Convert.ToInt64(TypeOfServiceIds), null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.TypeOfService;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TOS_ID, TypeOfServiceIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_TYPE_OF_SERVICE_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.TypeOfService.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.TypeOfService.Rows[0][ds.TypeOfService.TOSIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::DeleteTypeOfService", PROC_TYPE_OF_SERVICE_DELETE, ex);
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
        /// Inserts the type of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertTypeOfService(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.TypeOfService.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TYPE_OF_SERVICE_INSERT, ds, ds.TypeOfService.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.TypeOfService.Rows[0][ds.TypeOfService.TOSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::InsertTypeOfService", PROC_TYPE_OF_SERVICE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCodeLookup LookupTypeOfService(string IsActive)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);

                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TYPE_OF_SERVICE_LOOKUP, ds, ds.TypeOfService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupFacilityType", PROC_TYPE_OF_SERVICE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions for TOS Plan Info"
        /// <summary>
        /// Loads the tos plan.
        /// </summary>
        /// <param name="TOSId">The tos identifier.</param>
        /// <returns></returns>
        public DSCodes LoadTOSPlan(long TOSId, long TOSPlanId)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (TOSId <= 0)
                    dbManager.AddParameters(0, PARM_TOS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TOS_ID, TOSId);
                if (TOSPlanId <= 0)
                    dbManager.AddParameters(1, PARM_PLAN_OF_TOS_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PLAN_OF_TOS_ID, TOSPlanId);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TOS_PLAN_SELECT, ds, ds.PlanOfTOS.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::LoadTOSPlan", PROC_TOS_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the tos plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateTOSPlan(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanOfTOS.GetChanges();
                this.CreateParameters_Plan_Specific_Info(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_TOS_PLAN_UPDATE, ds, ds.PlanOfTOS.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanOfTOS.Rows[0][ds.PlanOfTOS.PlanOfTOSIdColumn].ToString(), ds.PlanOfTOS.Rows[0][ds.PlanOfTOS.TypeOfServiceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::UpdateTOSPlan", PROC_TOS_PLAN_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the tos plan.
        /// </summary>
        /// <param name="TOSPlanIds">The tos plan ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteTOSPlan(string TOSPlanIds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadTOSPlan(0, Convert.ToInt64(TOSPlanIds));
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanOfTOS;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PLAN_OF_TOS_ID, TOSPlanIds);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_TOS_PLAN_DELETE);
                //if (dtTemp != null && ds.PlanOfTOS.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanOfTOS.Rows[0][ds.PlanOfTOS.PlanOfTOSIdColumn].ToString(), ds.PlanOfTOS.Rows[0][ds.PlanOfTOS.TypeOfServiceIdColumn].ToString(), false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::DeleteTOSPlan", PROC_TOS_PLAN_DELETE, ex);
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the tos plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertTOSPlan(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanOfTOS.GetChanges();
                CreateParameters_Plan_Specific_Info(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TOS_PLAN_INSERT, ds, ds.PlanOfTOS.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanOfTOS.Rows[0][ds.PlanOfTOS.PlanOfTOSIdColumn].ToString(), ds.PlanOfTOS.Rows[0][ds.PlanOfTOS.TypeOfServiceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTypeOfService::InsertTOSPlan", PROC_TOS_PLAN_INSERT, ex);
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
