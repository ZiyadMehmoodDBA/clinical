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
    public class DALEDIServiceHandle
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_EDI_SERVICE_INSERT = "Provider.sp_EDIServiceHandleInsert";
        private const string PROC_EDI_SERVICE_UPDATE = "Provider.sp_EDIServiceHandleUpdate";
        private const string PROC_EDI_SERVICE_DELETE = "Provider.sp_EDIServiceHandleDelete";
        private const string PROC_EDI_SERVICE_SELECT = "Provider.sp_EDIServiceHandleSelect";
        #endregion

        #region "Parameters"
        private const string PARM_EDI_SERVICE_HANDLE_ID = "@EDIServiceHandleID";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_CASE = "@Case";
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
        public DALEDIServiceHandle()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALEDIServiceHandle(SharedVariable SharedVariable)
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
        private void CreateParameters(IDBManager dbManager, DSEDI ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, ds.EDIServiceHandle.EDIServiceHandleIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, ds.EDIServiceHandle.EDIServiceHandleIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_ENTITY_ID, ds.EDIServiceHandle.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_ID, ds.EDIServiceHandle.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, ds.EDIServiceHandle.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_CASE, ds.EDIServiceHandle.CaseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MODE, ds.EDIServiceHandle.ModeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_TIME, ds.EDIServiceHandle.TimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_INTERVAL_HOURS, ds.EDIServiceHandle.IntervalHoursColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_INTERVAL_MINUTES, ds.EDIServiceHandle.IntervalMinutesColumn.ColumnName, DbType.String);

            dbManager.AddParameters(9, PARM_CREATED_BY, ds.EDIServiceHandle.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.EDIServiceHandle.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.EDIServiceHandle.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.EDIServiceHandle.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.EDIServiceHandle.IsActiveColumn.ColumnName, DbType.Byte);
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
        public DSEDI LoadEDIServiceHandle1(long eDIServiceHandleId, string entityId, string clearingHouseId, string Case, string mode, string time, string isActive, Int64 PageNumber = 1, Int64 RowsPerPage = 15)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (entityId == "")  entityId = null;
                if (clearingHouseId == "") clearingHouseId = null;
                if (Case == "") Case = null;
                if (mode == "") mode = null;
                if (time == "") time = null;
                if (isActive == "") isActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (eDIServiceHandleId <= 0)
                    dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, eDIServiceHandleId);
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
                dbManager.AddParameters(3, PARM_CASE, Case);
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

                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.EDIServiceHandle.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIServiceHandle::LoadEDIService", PROC_EDI_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="eDIServiceHandleId"></param>
        /// <param name="entityId"></param>
        /// <param name="clearingHouseId"></param>
        /// <param name="Case"></param>
        /// <param name="mode"></param>
        /// <param name="time"></param>
        /// <param name="isActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSEDI LoadEDIServiceHandle(SharedVariable SharedVariable,long eDIServiceHandleId, string entityId, string clearingHouseId, string Case, string mode, string time, string IsActive, int PageNumber = 1, int RowsPerPage = 1000 )
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Case == "")
                    Case = null;
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
                    dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, eDIServiceHandleId);

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
                dbManager.AddParameters(3, PARM_CASE, Case);
                dbManager.AddParameters(4, PARM_MODE, mode);
                dbManager.AddParameters(5, PARM_TIME, time);

                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);

                if (PageNumber <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); } 
                else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPerPage <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, null); }
                else { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPerPage); }

                

                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.EDIServiceHandle.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.EDIServiceHandle.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALQuestions::LoadEDIService", PROC_EDI_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSEDI LoadEDIServiceHandle(long eDIServiceHandleId, string entityId, string clearingHouseId, string Case, string mode, string time, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Case == "")
                    Case = null;
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
                    dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, eDIServiceHandleId);

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
                dbManager.AddParameters(3, PARM_CASE, Case);
                dbManager.AddParameters(4, PARM_MODE, mode);
                dbManager.AddParameters(5, PARM_TIME, time);

                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);

                if (PageNumber <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); }
                else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPerPage <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, null); }
                else { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPerPage); }



                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.EDIServiceHandle.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_SELECT, ds, ds.EDIServiceHandle.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::LoadEDIService", PROC_EDI_SERVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSEDI UpdateEDIServiceHandle(DSEDI ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIServiceHandle.GetChanges();
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_UPDATE, ds, ds.EDIServiceHandle.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIServiceHandle.Rows[0][ds.EDIServiceHandle.EDIServiceHandleIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIServiceHandle::UpdateEDIServiceHandle", PROC_EDI_SERVICE_UPDATE, ex);
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
        public string DeleteEDIServiceHandle(string EDIServiceHandleId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                 //DSEDI ds = LoadEDIServiceHandle(Convert.ToInt64(EDIServiceHandleId), null, null, null, null, null, null);
                 //DSDBAudit dsDBAudit = new DSDBAudit();
                 //DataTable dtTemp = ds.EDIServiceHandle;
                dbManager.AddParameters(0, PARM_EDI_SERVICE_HANDLE_ID, EDIServiceHandleId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EDI_SERVICE_DELETE);

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.EDIServiceHandle.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIServiceHandle.Rows[0][ds.EDIServiceHandle.EDIServiceHandleIDColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIServiceHandle::DeleteEDIServiceHandle", PROC_EDI_SERVICE_DELETE, ex);
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
        public DSEDI InsertEDIServiceHandle(DSEDI ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIServiceHandle.GetChanges();
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_EDI_SERVICE_INSERT, ds, ds.EDIServiceHandle.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIServiceHandle.Rows[0][ds.EDIServiceHandle.EDIServiceHandleIDColumn].ToString());
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
