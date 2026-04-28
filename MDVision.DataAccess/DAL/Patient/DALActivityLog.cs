using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALActivityLog
    {
        #region Variable
        
        #endregion

        #region Constructors

        public DALActivityLog()
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

        #region "Stored Procedure Names"
        private const string PROC_ACTIVITY_LOG_INSERT = "Patient.sp_ActivityLogInsert";
        private const string PROC_ACTIVITY_LOG_UPDATE = "Patient.sp_ActivityLogUpdate";
        private const string PROC_ACTIVITY_LOG_DELETE = "Patient.sp_ActivityLogDelete";
        private const string PROC_ACTIVITY_LOG_SELECT = "Patient.sp_ActivityLogSelect";

        #endregion

        #region "Parameters"
        private const string PARM_LOG_ID = "@ActivityLogId";
        private const string PARM_PROFILE_NAME = "@ProfileName";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_ENTRY_DATE = "@EntryDate";
        private const string PARM_FIELD = "@Field";
        private const string PARM_ORIGINAL_VALUE = "@OriginalValue";
        private const string PARM_CURRENT_VALUE = "@CurrentValue";
        private const string PARM_ACTIONS = "@Actions";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region "Support Functions"

        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LOG_ID, ds.ActivityLog.ActivityLogIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LOG_ID, ds.ActivityLog.ActivityLogIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROFILE_NAME, ds.ActivityLog.ProfileNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_USER_ID, ds.ActivityLog.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PATIENT_ID, ds.ActivityLog.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_ENTRY_DATE, ds.ActivityLog.EntryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_FIELD, ds.ActivityLog.FieldColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ORIGINAL_VALUE, ds.ActivityLog.OriginalValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CURRENT_VALUE, ds.ActivityLog.CurrentValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ACTIONS, ds.ActivityLog.ActionsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.ActivityLog.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.ActivityLog.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.ActivityLog.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.ActivityLog.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.ActivityLog.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the Activity Log.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="LogId">The Activity Log identifier.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient LoadActivityLog(long PatientId, long ActivityLogId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ActivityLogId <= 0)
                    dbManager.AddParameters(0, PARM_LOG_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LOG_ID, ActivityLogId);

                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (MDVSession.Current.IsAdmin)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACTIVITY_LOG_SELECT, ds, ds.ActivityLog.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALActivityLog::LoadActivityLog", PROC_ACTIVITY_LOG_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the Activity Log.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient UpdateActivityLog(DSPatient ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ACTIVITY_LOG_UPDATE, ds, ds.ActivityLog.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALActivityLog::UpdateActivityLog", PROC_ACTIVITY_LOG_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Deletes the Activity Log
        /// </summary>
        /// <param name="AuthorizationId">The Activity Log identifier.</param>
        /// <returns>System.String.</returns>
        public string DeleteActivityLog(string ActivityLogId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_LOG_ID, ActivityLogId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_ACTIVITY_LOG_DELETE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALActivityLog::DeleteActivityLog", PROC_ACTIVITY_LOG_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Inserts the Activity Log.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient InsertActivityLog(DSPatient ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ACTIVITY_LOG_INSERT, ds, ds.ActivityLog.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALActivityLog::InsertActivityLog", PROC_ACTIVITY_LOG_INSERT, ex);
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
