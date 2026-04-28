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

namespace MDVision.DataAccess.DAL.Schedule
{
    public class DALProviderSchedule
    {
        #region Variable
        
        #endregion
     
        #region "Stored Procedure Names"
        private const string PROC_SCHEDULE_PROVIDERSCHEDULE_INSERT = "Provider.sp_ProviderScheduleInsert";
        private const string PROC_SCHEDULE_PROVIDERSCHEDULE_UPDATE = "Provider.sp_ProviderScheduleUpdate";
        private const string PROC_SCHEDULE_PROVIDERSCHEDULE_DELETE = "Provider.sp_ProviderScheduleDelete";
        private const string PROC_SCHEDULE_PROVIDERSCHEDULE_SELECT = "Provider.sp_ProviderScheduleSelect";

        private const string PROC_SCHEDULE_REASON_DURATION = "Provider.sp_ScheduleReasonsDuration";
        #endregion

        #region "Parameters"
        private const string PARM_SCHEDULE_SCHEDULE_ID = "@ScheduleId";
        private const string PARM_SCHEDULE_PROVIDER_ID = "@ProviderId";
        private const string PARM_SCHEDULE_FACILITY_ID = "@FacilityId";
        private const string PARM_SCHEDULE_FROM_DATE = "@FromDate";
        private const string PARM_SCHEDULE_TO_DATE = "@ToDate";
        private const string PARM_SCHEDULE_FROM_TIME = "@FromTime";
        private const string PARM_SCHEDULE_TO_TIME = "@ToTime";
        private const string PARM_SCHEDULE_SLOT_MINUTES = "@SlotMinutes";
        private const string PARM_SCHEDULE_PATIENT_ALLOWED = "@PatinetAllowed";
        private const string PARM_SCHEDULE_OVERBOOKED_ALLOWED = "@OverBookedAllowed";
        private const string PARM_SCHEDULE_BLOCKTIME_FROM = "@BlockTimeFrom";
        private const string PARM_SCHEDULE_BLOCKTIME_TO = "@BlockTimeTo";
        private const string PARM_SCHEDULE_BLOCK_REASON = "@BlockReasonId";
        private const string PARM_SCHEDULE_SCHEDULE_FOR = "@SchFor";
        private const string PARM_SCHEDULE_PATTERN_EVERY = "@PatternEvery";
        private const string PARM_SCHEDULE_VALUE = "@Value";
        private const string PARM_SCHEDULE_PATTERN_DAYS = "@PatternDays";
        private const string PARM_SCHEDULE_PATTERN_WEEKS = "@PatternWeeks";
        private const string PARM_SCHEDULE_PATTERN_MONTHS = "@PatternMonths";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SCHEDULE_SCHEDULE_REASON = "@ScheduleReasonId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region Constructors
        public DALProviderSchedule()
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
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSScheduleSetup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(21);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_ID, ds.ProviderSchedule.ScheduleIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_ID, ds.ProviderSchedule.ScheduleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SCHEDULE_PROVIDER_ID, ds.ProviderSchedule.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SCHEDULE_FACILITY_ID, ds.ProviderSchedule.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_SCHEDULE_FROM_DATE, ds.ProviderSchedule.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_SCHEDULE_TO_DATE, ds.ProviderSchedule.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_SCHEDULE_FROM_TIME, ds.ProviderSchedule.FromTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SCHEDULE_TO_TIME, ds.ProviderSchedule.ToTimeColumn.ColumnName, DbType.String);
         //   dbManager.AddParameters(7, PARM_SCHEDULE_SLOT_MINUTES, ds.ProviderSchedule.SlotMinutesColumn.ColumnName, DbType.String);
         //   dbManager.AddParameters(8, PARM_SCHEDULE_PATIENT_ALLOWED, ds.ProviderSchedule.PatinetAllowedColumn.ColumnName, DbType.Int64);
         //   dbManager.AddParameters(9, PARM_SCHEDULE_OVERBOOKED_ALLOWED, ds.ProviderSchedule.OverBookedAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_SCHEDULE_BLOCKTIME_FROM, ds.ProviderSchedule.BlockTimeFromColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_SCHEDULE_BLOCKTIME_TO, ds.ProviderSchedule.BlockTimeToColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_SCHEDULE_BLOCK_REASON, ds.ProviderSchedule.BlockReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_SCHEDULE_SCHEDULE_FOR, ds.ProviderSchedule.SchForColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SCHEDULE_PATTERN_EVERY, ds.ProviderSchedule.PatternEveryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_SCHEDULE_VALUE, ds.ProviderSchedule.ValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SCHEDULE_PATTERN_DAYS, ds.ProviderSchedule.PatternDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_SCHEDULE_PATTERN_WEEKS, ds.ProviderSchedule.PatternWeeksColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_SCHEDULE_PATTERN_MONTHS, ds.ProviderSchedule.PatternMonthsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_IS_ACTIVE, ds.ProviderSchedule.isActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARM_CREATED_BY, ds.ProviderSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CREATED_ON, ds.ProviderSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_MODIFIED_BY, ds.ProviderSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_MODIFIED_ON, ds.ProviderSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
            //if (ds.ProviderSchedule.Rows[0]["ScheduleReasonId"].ToString() == "")
            //{
            //    dbManager.AddParameters(24, PARM_SCHEDULE_SCHEDULE_REASON, null, DbType.Int64);
            //}
            //else
            //{
            //    dbManager.AddParameters(24, PARM_SCHEDULE_SCHEDULE_REASON, ds.ProviderSchedule.ScheduleReasonIdColumn.ColumnName, DbType.Int64);
            //}
           
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Inserts the Provider Schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup InsertProviderSchedule(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProviderSchedule.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_PROVIDERSCHEDULE_INSERT, ds, ds.ProviderSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProviderSchedule.Rows[0][ds.ProviderSchedule.ScheduleIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderSchedule::InsertProviderSchedule", PROC_SCHEDULE_PROVIDERSCHEDULE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the provider schedule.
        /// </summary>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup LoadProviderSchedule(long ScheduleId, string ProviderId, string FacilityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
               
                if (ProviderId == "")
                    ProviderId = null;

                if (FacilityId == "")
                    FacilityId = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ScheduleId <= 0)
                    dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_ID, ScheduleId);

                dbManager.AddParameters(1, PARM_SCHEDULE_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(2, PARM_SCHEDULE_FACILITY_ID, FacilityId);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ProviderSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_PROVIDERSCHEDULE_SELECT, ds, ds.ProviderSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderSchedule::LoadProviderSchedule", PROC_SCHEDULE_PROVIDERSCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the provider schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup UpdateProviderSchedule(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProviderSchedule.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSScheduleSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_PROVIDERSCHEDULE_UPDATE, ds, ds.ProviderSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProviderSchedule.Rows[0][ds.ProviderSchedule.ScheduleIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderSchedule::UpdateProviderSchedule", PROC_SCHEDULE_PROVIDERSCHEDULE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the provider schedule.
        /// </summary>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteProviderSchedule(string ScheduleId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                //DSScheduleSetup ds = LoadProviderSchedule(Convert.ToInt64(ScheduleId), null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProviderSchedule;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_ID, ScheduleId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SCHEDULE_PROVIDERSCHEDULE_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ProviderSchedule.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProviderSchedule.Rows[0][ds.ProviderSchedule.ScheduleIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderSchedule::DeleteProviderSchedule", PROC_SCHEDULE_PROVIDERSCHEDULE_DELETE, ex);
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

        public DSScheduleSetup LoadScheduleReasonDuration(long ScheduleReasonId)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ScheduleReasonId <= 0)
                    dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_REASON, null);
                else
                    dbManager.AddParameters(0, PARM_SCHEDULE_SCHEDULE_REASON, ScheduleReasonId);

                //dbManager.AddParameters(1, PARM_SCHEDULE_SCHEDULE_REASON, ScheduleReasonId);
                //dbManager.AddParameters(2, PARM_SCHEDULE_FACILITY_ID, FacilityId);

                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_REASON_DURATION, ds, ds.ScheduleReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderSchedule::LoadScheduleReasonDuration", PROC_SCHEDULE_REASON_DURATION, ex);
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
