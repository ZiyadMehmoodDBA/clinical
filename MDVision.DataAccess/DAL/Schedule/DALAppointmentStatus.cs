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
using MDVision.Model.Schedule;

namespace MDVision.DataAccess.DAL.Schedule
{
    public class DALAppointmentStatus
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_APPOINTMENT_STATUS_INSERT = "Provider.sp_AppointmentStatusInsert";
        private const string PROC_APPOINTMENT_STATUS_UPDATE = "Provider.sp_AppointmentStatusUpdate";
        private const string PROC_APPOINTMENT_STATUS_DELETE = "Provider.sp_AppointmentStatusDelete";
        private const string PROC_APPOINTMENT_STATUS_SELECT = "Provider.sp_AppointmentStatusSelect";
        private const string PROC_APPOINTMENT_STATUS_LOOKUP = "Provider.sp_AppointmentStatusLookup";

        private const string PROC_GET_PATIENT_FUTURE_APPOINTMENT = "Patient.sp_GetPatientFutureAppointment";
        private const string PROC_GET_PATIENT_FUTURE_APPOINTMENT_FOR_COPAY = "Patient.sp_GetPatientFutureAppointmentForCopay";
        #endregion

        #region "Parameters"
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_APPOINTMENT_NAME = "@ShortName";
        private const string PARM_APPOINTMENT_DESC = "@Description";
        private const string PARM_APPOINTMENT_COLOR = "@Color";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DALAppointmentStatus"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALAppointmentStatus()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALAppointmentStatus(SharedVariable SharedVariable)
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
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSScheduleSetup ds, Boolean IsInsert)
        {
            //dbManager.CreateParameters(ds.Tables[ds.AppointmentStatus.TableName].Columns.Count);
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_APPOINTMENT_ID, ds.AppointmentStatus.AppointmentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_APPOINTMENT_ID, ds.AppointmentStatus.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_APPOINTMENT_NAME, ds.AppointmentStatus.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_APPOINTMENT_DESC, ds.AppointmentStatus.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_APPOINTMENT_COLOR, ds.AppointmentStatus.ColorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.AppointmentStatus.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.AppointmentStatus.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.AppointmentStatus.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.AppointmentStatus.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.AppointmentStatus.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup LoadAppointmentStatus(long AppointmentId, string ShortName, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (AppointmentId <= 0)
                    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_APPOINTMENT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_APPOINTMENT_DESC, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.AppointmentStatus.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_SELECT, ds, ds.AppointmentStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::LoadAppointmentStatus", PROC_APPOINTMENT_STATUS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the appointment status.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup UpdateAppointmentStatus(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.AppointmentStatus.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSScheduleSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_UPDATE, ds, ds.AppointmentStatus.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.AppointmentStatus.Rows[0][ds.AppointmentStatus.AppointmentIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::UpdateAppointmentStatus", PROC_APPOINTMENT_STATUS_UPDATE, ex);
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
        /// Deletes the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteAppointmentStatus(string AppointmentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSScheduleSetup ds = LoadAppointmentStatus(Convert.ToInt64(AppointmentId), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.AppointmentStatus;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_APPOINTMENT_ID, AppointmentId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.AppointmentStatus.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.AppointmentStatus.Rows[0][ds.AppointmentStatus.AppointmentIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::DeleteAppointmentStatus", PROC_APPOINTMENT_STATUS_DELETE, ex);
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

        public DSScheduleSetup InsertAppointmentStatus(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.AppointmentStatus.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_INSERT, ds, ds.AppointmentStatus.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.AppointmentStatus.Rows[0][ds.AppointmentStatus.AppointmentIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::InsertAppointmentStatus", PROC_APPOINTMENT_STATUS_INSERT, ex);
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
        /// Updates the patient insurances priorities.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        public List<PatientFutureAppointment> GetPatientFutureAppointment(Int64 PatientId)
        {

            List<PatientFutureAppointment> PatientFutureAppointmentList = new List<PatientFutureAppointment>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                PatientFutureAppointmentList = dbManager.ExecuteReaders<PatientFutureAppointment>(PROC_GET_PATIENT_FUTURE_APPOINTMENT);
                return PatientFutureAppointmentList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::GetPatientFutureAppointment", PROC_GET_PATIENT_FUTURE_APPOINTMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<PatientFutureAppointment> GetPatientFutureAppointmentForCopay(Int64 PatientId, Int64 InsuranceId)
        {
            List<PatientFutureAppointment> PatientFutureAppointmentList = new List<PatientFutureAppointment>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(PARM_INSURANCE_ID, InsuranceId);
                PatientFutureAppointmentList = dbManager.ExecuteReaders<PatientFutureAppointment>(PROC_GET_PATIENT_FUTURE_APPOINTMENT_FOR_COPAY);
                return PatientFutureAppointmentList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::GetPatientFutureAppointmentForCopay", PROC_GET_PATIENT_FUTURE_APPOINTMENT_FOR_COPAY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Lookups

        public DSScheduleLookups LookupAppointmentStatus()
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_LOOKUP, ds, ds.AppointmentStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAppointmentStatus::LookupAppointmentStatus", PROC_APPOINTMENT_STATUS_LOOKUP, ex);
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
        /// <returns></returns>
        public DSScheduleLookups LookupAppointmentStatus(SharedVariable SharedVariable)
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_STATUS_LOOKUP, ds, ds.AppointmentStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALAppointmentStatus::LookupAppointmentStatus", PROC_APPOINTMENT_STATUS_LOOKUP, ex);
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
