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


namespace MDVision.DataAccess.DAL.Admin
{
    public class DALPatientEligibilityService
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_EDI_SERVICE_INSERT = "Provider.sp_EDIPatientEligibilityServiceInsert";
        private const string PROC_EDI_SERVICE_UPDATE = "Provider.sp_EDIPatientEligibilityServiceUpdate";
        private const string PROC_EDI_SERVICE_DELETE = "Provider.sp_EDIPatientEligibilityServiceDelete";
        private const string PROC_EDI_SERVICE_SELECT = "Provider.sp_EDIPatientEligibilityServiceSelect";
        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ELIGIBILITY_SERVICE_ID = "@PatientEligibilityServiceID";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_SCHEDULE_DAYS = "@ScheduleDays";
        private const string PARM_MODE = "@Mode";
        private const string PARM_TIME = "@Time";
        private const string PARM_INTERVAL_HOURS = "@IntervalHours";
        private const string PARM_INTERVAL_MINUTES = "@IntervalMinutes";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_IS_ACTIVE = "@IsActive";
        //private const string PARM_PAGE_NUMBER = "@PageNumber";
        //private const string PARM_ROWSP_PAGE = "@RowspPage";
        //private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_CLEARING_HOUSE_NAME = "@ClearingHouseName";
        private const string PARM_ENTITY_NAME = "@EntityName";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALClearingHouse"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientEligibilityService()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALPatientEligibilityService(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

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
        private void CreateParameters(IDBManager dbManager, DSPatientEligibilityService ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, ds.PatientEligibilityService.PatientEligibilityServiceIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, ds.PatientEligibilityService.PatientEligibilityServiceIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_ENTITY_ID, ds.PatientEligibilityService.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.PatientEligibilityService.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, ds.PatientEligibilityService.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_SCHEDULE_DAYS, ds.PatientEligibilityService.ScheduleDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MODE, ds.PatientEligibilityService.ModeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_TIME, ds.PatientEligibilityService.TimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_INTERVAL_HOURS, ds.PatientEligibilityService.IntervalHoursColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_INTERVAL_MINUTES, ds.PatientEligibilityService.IntervalMinutesColumn.ColumnName, DbType.String);

            dbManager.AddParameters(9, PARM_CREATED_BY, ds.PatientEligibilityService.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.PatientEligibilityService.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.PatientEligibilityService.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.PatientEligibilityService.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.PatientEligibilityService.IsActiveColumn.ColumnName, DbType.Byte);
            //dbManager.AddParameters(14, PARM_ENTITY_NAME, ds.EDIServiceHandle.EntityNameColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(15, PARM_CLEARING_HOUSE_NAME, ds.EDIServiceHandle.ClearingHouseNameColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(14, PARM_CLEARING_HOUSE_NAME, ds.EDIServiceHandle.ClearingHouseNameColumn.ColumnName, DbType.Byte);
            //dbManager.AddParameters(15, PARM_ENTITY_NAME, ds.EDIServiceHandle.EntityNameColumn.ColumnName, DbType.Byte);

            // Commented Parameters 14 and 15 for Issue number PMS-2811
            //Abdur Rehman - Nov 30th, 2015

            //dbManager.AddParameters(14, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// LoadEDIService
        /// </summary>
        /// <param name="eDIServiceHandleId"></param>
        /// <param name="entityId"></param>
        /// <param name="clearingHouseId"></param>
        /// <param name="Case"></param>
        /// <param name="mode"></param>
        /// <param name="time"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public DSPatientEligibilityService LoadPatientEligibilityService(long eDIServiceHandleId, string entityId, string clearingHouseId, string Case, string mode, string time, string isActive, Int64 PageNumber = 1, Int64 RowsPerPage = 15)
        {
            DSPatientEligibilityService ds = new DSPatientEligibilityService();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (entityId == "") entityId = null;
                if (clearingHouseId == "") clearingHouseId = null;
                if (Case == "") Case = null;
                if (mode == "") mode = null;
                if (time == "") time = null;
                if (isActive == "") isActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (eDIServiceHandleId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, eDIServiceHandleId);
                if (entityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
                }
                dbManager.AddParameters(2, PARM_CLEARING_HOUSE_ID, clearingHouseId);
                dbManager.AddParameters(3, PARM_SCHEDULE_DAYS, Case);
                dbManager.AddParameters(4, PARM_MODE, mode);
                dbManager.AddParameters(5, PARM_TIME, time);

                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWS_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWS_PAGE, RowsPerPage);
                //dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.EDIServiceHandle.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatientEligibilityService)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.PatientEligibilityService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEligibilityService::LoadPatientEligibilityService", PROC_EDI_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        ///// <summary>
        ///// This Method is for Windows service to pass SharedVabiables directly
        ///// </summary>
        ///// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        ///// <param name="eDIServiceHandleId"></param>
        ///// <param name="entityId"></param>
        ///// <param name="clearingHouseId"></param>
        ///// <param name="Case"></param>
        ///// <param name="mode"></param>
        ///// <param name="time"></param>
        ///// <param name="isActive"></param>
        ///// <param name="PageNumber"></param>
        ///// <param name="RowsPerPage"></param>
        ///// <returns></returns>
        //public DSEDI LoadEDIServiceHandle(SharedVariable SharedVariable, long eDIServiceHandleId, string entityId, string clearingHouseId, string Case, string mode, string time, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    DSEDI ds = new DSEDI();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        if (Case == "")
        //            Case = null;
        //        if (mode == "")
        //            mode = null;
        //        if (time == "")
        //            time = null;
        //        if (IsActive == "")
        //            IsActive = null;
        //        if (clearingHouseId == "")
        //            clearingHouseId = null;
        //        if (entityId == "")
        //            entityId = null;


        //        dbManager.Open();
        //        dbManager.CreateParameters(10);
        //        if (eDIServiceHandleId <= 0)
        //            dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, eDIServiceHandleId);

        //        if (entityId == null)
        //        {
        //            if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
        //                dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
        //            else
        //                dbManager.AddParameters(1, PARM_ENTITY_ID, SharedVariable.EntityId);
        //        }
        //        else
        //        {
        //            dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
        //        }
        //        dbManager.AddParameters(2, PARM_CLEARING_HOUSE_ID, clearingHouseId);
        //        dbManager.AddParameters(3, PARM_CASE, Case);
        //        dbManager.AddParameters(4, PARM_MODE, mode);
        //        dbManager.AddParameters(5, PARM_TIME, time);

        //        dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);

        //        if (PageNumber <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); }
        //        else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber); }
        //        if (RowsPerPage <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, null); }
        //        else { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPerPage); }



        //        dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.EDIServiceHandle.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
        //        ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.EDIServiceHandle.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALQuestions::LoadEDIService", PROC_EDI_SERVICE_SELECT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        public DSPatientEligibilityService LoadPatientEligibilityService(long eDIServiceHandleId, string entityId, string clearingHouseId, string scheduleDays, string mode, string time, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSPatientEligibilityService ds = new DSPatientEligibilityService();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (scheduleDays == "")
                    scheduleDays = null;
                if (mode == "")
                    mode = null;
                if (time == "")
                    time = null;
                if (IsActive == "")
                    IsActive = null;
                if (clearingHouseId == "")
                    clearingHouseId = null;
                if (entityId == "")
                    entityId = null;


                dbManager.Open();
                dbManager.CreateParameters(10);
                if (eDIServiceHandleId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, eDIServiceHandleId);

                if (entityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
                }
                dbManager.AddParameters(2, PARM_CLEARING_HOUSE_ID, clearingHouseId);
                dbManager.AddParameters(3, PARM_SCHEDULE_DAYS, scheduleDays);
                dbManager.AddParameters(4, PARM_MODE, mode);
                dbManager.AddParameters(5, PARM_TIME, time);

                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);

                if (PageNumber <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); }
                else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPerPage <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, null); }
                else { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPerPage); }



                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.PatientEligibilityService.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSPatientEligibilityService)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.PatientEligibilityService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEligibilityService::LoadPatientEligibilityService", PROC_EDI_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatientEligibilityService LoadPatientEligibilityService(SharedVariable SharedVariable, long eDIServiceHandleId, string entityId, string clearingHouseId, string scheduleDays, string mode, string time, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSPatientEligibilityService ds = new DSPatientEligibilityService();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (scheduleDays == "")
                    scheduleDays = null;
                if (mode == "")
                    mode = null;
                if (time == "")
                    time = null;
                if (IsActive == "")
                    IsActive = null;
                if (clearingHouseId == "")
                    clearingHouseId = null;
                if (entityId == "")
                    entityId = null;


                dbManager.Open();
                dbManager.CreateParameters(10);
                if (eDIServiceHandleId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, eDIServiceHandleId);

                if (entityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, SharedVariable.EntityId);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);
                }
                dbManager.AddParameters(2, PARM_CLEARING_HOUSE_ID, clearingHouseId);
                dbManager.AddParameters(3, PARM_SCHEDULE_DAYS, scheduleDays);
                dbManager.AddParameters(4, PARM_MODE, mode);
                dbManager.AddParameters(5, PARM_TIME, time);

                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);

                if (PageNumber <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); }
                else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPerPage <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, null); }
                else { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPerPage); }



                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.PatientEligibilityService.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSPatientEligibilityService)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.PatientEligibilityService.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALPatientEligibilityService::LoadPatientEligibilityService", PROC_EDI_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientEligibilityService UpdatePatientEligibilityService(DSPatientEligibilityService ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientEligibilityService.GetChanges();
                ds = (DSPatientEligibilityService)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_UPDATE, ds, ds.PatientEligibilityService.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PatientEligibilityService.Rows[0][ds.PatientEligibilityService.PatientEligibilityServiceIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEligibilityService::UpdatePatientEligibilityService", PROC_EDI_SERVICE_UPDATE, ex);
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
        /// DeleteEDIServiceHandle
        /// </summary>
        /// <param name="EDIServiceHandleId"></param>
        /// <returns></returns>
        public string DeletePatientEligibilityService(string EDIServiceHandleId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSPatientEligibilityService ds = LoadPatientEligibilityService(Convert.ToInt64(EDIServiceHandleId), null, null, null, null,null,null,1,1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientEligibilityService;
                dbManager.AddParameters(0, PARM_PATIENT_ELIGIBILITY_SERVICE_ID, EDIServiceHandleId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EDI_SERVICE_DELETE);

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.PatientEligibilityService.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PatientEligibilityService.Rows[0][ds.PatientEligibilityService.ClearingHouseIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEligibilityService::DeletePatientEligibilityService", PROC_EDI_SERVICE_DELETE, ex);
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
        /// InsertEDIServiceHandle
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSPatientEligibilityService InsertPatientEligibilityService(DSPatientEligibilityService ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientEligibilityService.GetChanges();
                ds = (DSPatientEligibilityService)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_INSERT, ds, ds.PatientEligibilityService.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PatientEligibilityService.Rows[0][ds.PatientEligibilityService.PatientEligibilityServiceIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIServiceHandle::InsertEDIServiceHandle", PROC_EDI_SERVICE_INSERT, ex);
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
