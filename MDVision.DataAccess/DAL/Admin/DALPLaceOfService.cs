using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALPlaceOfService
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PLACE_OF_SERVICE_INSERT = "Provider.sp_PlaceOfServiceInsert";
        private const string PROC_PLACE_OF_SERVICE_UPDATE = "Provider.sp_PlaceOfServiceUpdate";
        private const string PROC_PLACE_OF_SERVICE_DELETE = "Provider.sp_PlaceOfServiceDelete";
        private const string PROC_PLACE_OF_SERVICE_SELECT = "Provider.sp_PlaceOfServiceSelect";
        private const string PROC_PLACE_OF_SERVICE_LOOKUP = "Provider.sp_PlaceOfServiceLookup";

        private const string PROC_POS_PLAN_DELETE = "Provider.sp_PlanOfPOSDelete";
        private const string PROC_POS_PLAN_INSERT = "Provider.sp_PlanOfPOSInsert";
        private const string PROC_POS_PLAN_SELECT = "Provider.sp_PlanOfPOSSelect";
        private const string PROC_POS_PLAN_UPDATE = "Provider.sp_PlanOfPOSUpdate";
        #endregion

        #region "Parameters"
        private const string PARM_POS_ID = "@POSId";
        private const string PARM_POS_CODE = "@POSCode";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_PLAN_OF_POS_ID = "@PlanOfPOSId";
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
        /// Initializes a new instance of the <see cref="DALPLaceOfService"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALPlaceOfService()
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
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_POS_ID, ds.PlaceOfService.POSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_POS_ID, ds.PlaceOfService.POSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_POS_CODE, ds.PlaceOfService.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.PlaceOfService.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.PlaceOfService.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.PlaceOfService.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.PlaceOfService.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.PlaceOfService.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.PlaceOfService.ModifiedOnColumn.ColumnName, DbType.DateTime);
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
                dbManager.AddParameters(0, PARM_PLAN_OF_POS_ID, ds.PlanOfPOS.PlanOfPOSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PLAN_OF_POS_ID, ds.PlanOfPOS.PlanOfPOSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ID, ds.PlanOfPOS.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_POS_ID, ds.PlanOfPOS.PlaceOfServiceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PLAN_NAME, ds.PlanOfPOS.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PlanOfPOS.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.PlanOfPOS.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.PlanOfPOS.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.PlanOfPOS.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.PlanOfPOS.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the place of service.
        /// </summary>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <param name="Code">The code.</param>
        /// <returns></returns>
        public DSCodes LoadPlaceOfService(long PlaceOfServiceId, string Code, string description, string IsActive, int PageNumber, int RowsPerPage)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Code == "")
                    Code = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (PlaceOfServiceId == 0)
                    dbManager.AddParameters(0, PARM_POS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_POS_ID, PlaceOfServiceId);
                dbManager.AddParameters(1, PARM_POS_CODE, Code);
                dbManager.AddParameters(2, PARM_DESCRIPTION, description);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PlaceOfService.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);

                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLACE_OF_SERVICE_SELECT, ds, ds.PlaceOfService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::LoadPlaceOfService", PROC_PLACE_OF_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the place of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdatePlaceOfService(DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlaceOfService.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PLACE_OF_SERVICE_UPDATE, ds, ds.PlaceOfService.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlaceOfService.Rows[0][ds.PlaceOfService.POSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::UpdatePlaceOfService", PROC_PLACE_OF_SERVICE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the place of service.
        /// </summary>
        /// <param name="PlaceOfServiceIds">The place of service ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePlaceOfService(string PlaceOfServiceIds)
        {
            object returnValue;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadPlaceOfService(Convert.ToInt64(PlaceOfServiceIds), null, null, null,1,15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlaceOfService;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_POS_ID, PlaceOfServiceIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PLACE_OF_SERVICE_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.PlaceOfService.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlaceOfService.Rows[0][ds.PlaceOfService.POSIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::DeletePlaceOfService", PROC_PLACE_OF_SERVICE_DELETE, ex);
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
        /// Inserts the place of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertPlaceOfService(DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlaceOfService.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PLACE_OF_SERVICE_INSERT, ds, ds.PlaceOfService.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlaceOfService.Rows[0][ds.PlaceOfService.POSIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::InsertPlaceOfService", PROC_PLACE_OF_SERVICE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions for POS Plan Info"
        /// <summary>
        /// Loads the position plan.
        /// </summary>
        /// <param name="POSId">The position identifier.</param>
        /// <returns></returns>
        public DSCodes LoadPOSPlan(long POSId, long POSPlanId)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (POSId <= 0)
                    dbManager.AddParameters(0, PARM_POS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_POS_ID, POSId);
                if (POSPlanId <= 0)
                    dbManager.AddParameters(1, PARM_PLAN_OF_POS_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PLAN_OF_POS_ID, POSPlanId);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_POS_PLAN_SELECT, ds, ds.PlanOfPOS.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::LoadPOSPlan", PROC_POS_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the position plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdatePOSPlan(DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanOfPOS.GetChanges();
                this.CreateParameters_Plan_Specific_Info(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_POS_PLAN_UPDATE, ds, ds.PlanOfPOS.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanOfPOS.Rows[0][ds.PlanOfPOS.PlanOfPOSIdColumn].ToString(), ds.PlanOfPOS.Rows[0][ds.PlanOfPOS.PlaceOfServiceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::UpdatePOSPlan", PROC_POS_PLAN_UPDATE, ex);
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
        /// Deletes the position plan.
        /// </summary>
        /// <param name="POSPlanIds">The position plan ids.</param>
        /// <returns></returns>
        public string DeletePOSPlan(string POSPlanIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadPOSPlan(0, Convert.ToInt64(POSPlanIds));
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanOfPOS;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PLAN_OF_POS_ID, POSPlanIds);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_POS_PLAN_DELETE);
                //if (dtTemp != null && ds.PlanOfPOS.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanOfPOS.Rows[0][ds.PlanOfPOS.PlanOfPOSIdColumn].ToString(), ds.PlanOfPOS.Rows[0][ds.PlanOfPOS.PlaceOfServiceIdColumn].ToString(), false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::DeletePOSPlan", PROC_POS_PLAN_DELETE, ex);
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
        /// Inserts the position plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertPOSPlan(DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanOfPOS.GetChanges();
                CreateParameters_Plan_Specific_Info(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_POS_PLAN_INSERT, ds, ds.PlanOfPOS.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanOfPOS.Rows[0][ds.PlanOfPOS.PlanOfPOSIdColumn].ToString(), ds.PlanOfPOS.Rows[0][ds.PlanOfPOS.PlaceOfServiceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlaceOfService::InsertPOSPlan", PROC_POS_PLAN_INSERT, ex);
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

        #region Lookups
        /// <summary>
        /// Loads the place of service.
        /// </summary>
        /// <param name="POSId">The position identifier.</param>
        /// <returns></returns>
        public DSCodeLookup LookupPlaceOfService()
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLACE_OF_SERVICE_LOOKUP, ds, ds.PlaceOfService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LookupPlaceOfService", PROC_PLACE_OF_SERVICE_LOOKUP, ex);
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
