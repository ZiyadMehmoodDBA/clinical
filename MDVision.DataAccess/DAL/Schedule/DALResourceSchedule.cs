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
    public class DALResourceSchedule
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_RESOURCE_SCHEDULE_INSERT = "Provider.sp_ResourceScheduleInsert";
        private const string PROC_RESOURCE_SCHEDULE_UPDATE = "Provider.sp_ResourceScheduleUpdate";
        private const string PROC_RESOURCE_SCHEDULE_DELETE = "Provider.sp_ResourceScheduleDelete";
        private const string PROC_RESOURCE_SCHEDULE_SELECT = "Provider.sp_ResourceScheduleSelect";

        private const string PROC_SCHEDULE_REASON_DURATION = "Provider.sp_ScheduleReasonsDuration";
        private const string PROC_ROOMS_LOOKUP = "System.sp_RoomsLookup";
        private const string PROC_DURATION_LOOKUP = "[Clinical].[sp_DurationLookup]";
        
        #endregion

        #region "Parameters"
        private const string PARM_RESOURCE_PATIENT_ID = "@PatientId";
        private const string PARM_RESOURCE_SCHEDULE_ID = "@ResScheduleId";
        private const string PARM_SCHEDULE_RESOURCE_ID = "@ResourceId";
        private const string PARM_SCHEDULE_FACILITY_ID = "@FacilityId";
        private const string PARM_SCHEDULE_FROM_DATE = "@FromDate";
        private const string PARM_SCHEDULE_TO_DATE = "@ToDate";
        private const string PARM_SCHEDULE_FROM_TIME = "@FromTime";
        private const string PARM_SCHEDULE_TO_TIME = "@ToTime";
        private const string PARM_SCHEDULE_SLOT_MINUTES = "@SlotMinutes";
        private const string PARM_SCHEDULE_PATIENT_ALLOWED = "@PatinetAllowed";
        private const string PARM_SCHEDULE_OVERBOOKED_ALLOWED = "@OverBookedAllowed";
        private const string PARM_SCHEDULE_SCHEDULE_REASON = "@ScheduleReasonId";
        private const string PARM_SCHEDULE_BLOCKTIME_FROM = "@BlockTimeFrom";
        private const string PARM_SCHEDULE_BLOCKTIME_TO = "@BlockTimeTo";
        private const string PARM_SCHEDULE_BLOCK_REASON = "@BlockReasonId";
        private const string PARM_SCHEDULE_SCHEDULE_FOR = "@SchFor";
        private const string PARM_SCHEDULE_PATTERN_EVERY = "@PatternEvery";
        private const string PARM_SCHEDULE_VALUE = "@Value";
        private const string PARM_SCHEDULE_PATTERN_DAYS = "@PatternDays";
        private const string PARM_SCHEDULE_PATTERN_WEEKS = "@PatternWeeks";
        private const string PARM_SCHEDULE_PATTERN_MONTHS = "@PatternMonths";
        private const string PARM_IS_ACTIVE = "@isActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        #endregion

        #region Constructors
        public DALResourceSchedule()
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

        private void CreateParameters(IDBManager dbManager, DSScheduleSetup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(21);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_RESOURCE_SCHEDULE_ID, ds.ResourceSchedule.ResScheduleIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_RESOURCE_SCHEDULE_ID, ds.ResourceSchedule.ResScheduleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SCHEDULE_RESOURCE_ID, ds.ResourceSchedule.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SCHEDULE_FACILITY_ID, ds.ResourceSchedule.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_SCHEDULE_FROM_DATE, ds.ResourceSchedule.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_SCHEDULE_TO_DATE, ds.ResourceSchedule.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_SCHEDULE_FROM_TIME, ds.ResourceSchedule.FromTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SCHEDULE_TO_TIME, ds.ResourceSchedule.ToTimeColumn.ColumnName, DbType.String);
          //  dbManager.AddParameters(7, PARM_SCHEDULE_SLOT_MINUTES, ds.ResourceSchedule.SlotMinutesColumn.ColumnName, DbType.String);
          //  dbManager.AddParameters(8, PARM_SCHEDULE_PATIENT_ALLOWED, ds.ResourceSchedule.PatinetAllowedColumn.ColumnName, DbType.Int64);
          //  dbManager.AddParameters(9, PARM_SCHEDULE_OVERBOOKED_ALLOWED, ds.ResourceSchedule.OverBookedAllowedColumn.ColumnName, DbType.Byte);
            //dbManager.AddParameters(10, PARM_SCHEDULE_SCHEDULE_REASON, ds.ResourceSchedule.ScheduleReasonIdColumn.ColumnName, DbType.Int64);
            //if (Convert.ToInt64(ds.ResourceSchedule.Rows[0]["ScheduleReasonId"].ToString()) == 0)
            //{
            //    dbManager.AddParameters(10, PARM_SCHEDULE_SCHEDULE_REASON, null, DbType.Int64);
            //}
            //else
            //{
           // dbManager.AddParameters(10, PARM_SCHEDULE_SCHEDULE_REASON, ds.ResourceSchedule.ScheduleReasonIdColumn.ColumnName, DbType.Int64);
            //}
            dbManager.AddParameters(7, PARM_SCHEDULE_BLOCKTIME_FROM, ds.ResourceSchedule.BlockTimeFromColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_SCHEDULE_BLOCKTIME_TO, ds.ResourceSchedule.BlockTimeToColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_SCHEDULE_BLOCK_REASON, ds.ResourceSchedule.BlockReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_SCHEDULE_SCHEDULE_FOR, ds.ResourceSchedule.SchForColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SCHEDULE_PATTERN_EVERY, ds.ResourceSchedule.PatternEveryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_SCHEDULE_VALUE, ds.ResourceSchedule.ValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SCHEDULE_PATTERN_DAYS, ds.ResourceSchedule.PatternDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_SCHEDULE_PATTERN_WEEKS, ds.ResourceSchedule.PatternWeeksColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_SCHEDULE_PATTERN_MONTHS, ds.ResourceSchedule.PatternMonthsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_IS_ACTIVE, ds.ResourceSchedule.isActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARM_CREATED_BY, ds.ResourceSchedule.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_CREATED_ON, ds.ResourceSchedule.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_MODIFIED_BY, ds.ResourceSchedule.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_MODIFIED_ON, ds.ResourceSchedule.ModifiedOnColumn.ColumnName, DbType.DateTime);
           

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        public DSScheduleSetup InsertResourceSchedule(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ResourceSchedule.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_RESOURCE_SCHEDULE_INSERT, ds, ds.ResourceSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ResourceSchedule.Rows[0][ds.ResourceSchedule.ResScheduleIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResourceSchedule::InsertResourceSchedule", PROC_RESOURCE_SCHEDULE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSScheduleSetup LoadResourceSchedule(long ResScheduleId, string ResourceId, string FacilityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (ResourceId == "")
                    ResourceId = null;

                if (FacilityId == "")
                    FacilityId = null;

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (ResScheduleId <= 0)
                    dbManager.AddParameters(0, PARM_RESOURCE_SCHEDULE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RESOURCE_SCHEDULE_ID, ResScheduleId);

                dbManager.AddParameters(1, PARM_SCHEDULE_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(2, PARM_SCHEDULE_FACILITY_ID, FacilityId);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ResourceSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RESOURCE_SCHEDULE_SELECT, ds, ds.ResourceSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResourceSchedule::LoadResourceSchedule", PROC_RESOURCE_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteResourceSchedule(string ResScheduleId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSScheduleSetup ds = LoadResourceSchedule(Convert.ToInt64(ResScheduleId), null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ResourceSchedule;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RESOURCE_SCHEDULE_ID, ResScheduleId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_RESOURCE_SCHEDULE_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ResourceSchedule.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ResourceSchedule.Rows[0][ds.ResourceSchedule.ResScheduleIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResourceSchedule::DeleteResourceSchedule", PROC_RESOURCE_SCHEDULE_DELETE, ex);
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
        public DSScheduleSetup UpdateResourceSchedule(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ResourceSchedule.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSScheduleSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_RESOURCE_SCHEDULE_UPDATE, ds, ds.ResourceSchedule.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ResourceSchedule.Rows[0][ds.ResourceSchedule.ResScheduleIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResourceSchedule::UpdateResourceSchedule", PROC_RESOURCE_SCHEDULE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSScheduleSetup LoadScheduleSchReasonDuration(long ScheduleReasonId)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
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
                MDVLogger.DALErrorLog("DALResourceSchedule::LoadScheduleSchReasonDuration", PROC_SCHEDULE_REASON_DURATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Lookups"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DSResources LookupRooms(Int64 FacilityId)
        {
            DSResources ds = new DSResources();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (FacilityId == 0)
                {
                    dbManager.AddParameters(0, PARM_SCHEDULE_FACILITY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SCHEDULE_FACILITY_ID, FacilityId);
                }

                ds = (DSResources)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROOMS_LOOKUP, ds, ds.Rooms.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResourceSchedule::LookupRooms", PROC_ROOMS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSResources LookupDuration(int patientId)
        {
            DSResources ds = new DSResources();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (patientId<=0)
                {
                    dbManager.AddParameters(0, PARM_RESOURCE_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_RESOURCE_PATIENT_ID, patientId);
                }
                
                ds = (DSResources)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DURATION_LOOKUP, ds, ds.Duration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResourceSchedule::LookupDuration", PROC_DURATION_LOOKUP, ex);
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
