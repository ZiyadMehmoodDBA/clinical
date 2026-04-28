using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Model.AuditableEvents;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Clinical
{
    /// <summary>
    /// Author: Zia Mehmood
    /// Overview: Data Access Layer for new Activity Log
    /// </summary>
    public class DALAuditbleEventsActivityLog
    {
        #region " Stored Procedure Names "

        private const string PROC_PATIENTAUDITLOG_LOAD_USER = "System.sp_PatientsAuditLog";
        private const string PROC_PATIENTAUDITLOG_LOAD_COMPONENTS = "System.PatientActivityComponents";
        private const string PROC_PATIENTAUDITLOG_LOAD_CHANGES = "System.PatientComponentsChange";
        private const string PROC_CHECKINAPPUSER_LOAD = "mobile.sp_ConfigurationSelect";

        #endregion



        #region " Constructors "
        public DALAuditbleEventsActivityLog()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALAuditbleEventsActivityLog(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }
        private IContainer components;
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Parameters
        private const string PARM_DATE_FROM = "@DateFrom";
        private const string PARM_DATE_TO = "@DateTo";
        private const string PARM_ACOCOUNT_NO = "@AccountNumber";
        private const string PARM_LASTNAME = "@LastName";
        private const string PARM_FIRSTNAME = "@FirstName";
        private const string PARM_DOB = "@DOB";
        private const string PARM_SSN = "@SSN";
        private const string PARM_PATIENT_STATUS = "@PatientStatus  ";
        private const string PARM_EMERGENCY_ACCESS = "@EmergencyAccess";
        private const string PARM_COLUMNKEY_ID = "@ColumnKeyId";
        private const string PARM_CREATED_DATE = "@CreatedDate";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARAM_PROFILENAME = "@ProfileName";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";



        #endregion

        #region "Methods"


        public List<ActivityLog> loadAcitivityLogUser(ActivityLog model)
        {
            List<ActivityLog> listobj = new List<ActivityLog>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(12);
                dbManager.AddParameters(0, PARM_ACOCOUNT_NO, model.AccountNo);
                dbManager.AddParameters(1, PARM_LASTNAME, model.LastName);
                dbManager.AddParameters(2, PARM_FIRSTNAME, model.FirstName);
                dbManager.AddParameters(3, PARM_DOB, model.DOB);
                dbManager.AddParameters(4, PARM_SSN, model.SSN);
                dbManager.AddParameters(5, PARM_PATIENT_STATUS, model.Status);
                dbManager.AddParameters(6, PARM_DATE_FROM, model.DateFrom);
                dbManager.AddParameters(7, PARM_DATE_TO, model.DateTo);
                dbManager.AddParameters(8, PARM_EMERGENCY_ACCESS, model.EmergencyAccess);
                dbManager.AddParameters(9, PARM_PAGE_NUMBER, model.PageNumber);
                dbManager.AddParameters(10, PARM_ROWS_PER_PAGE, model.RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, "", DbType.Int64, ParamDirection.Output);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENTAUDITLOG_LOAD_USER);
                while (reader.Read())
                {
                    ActivityLog model1 = new ActivityLog();
                    var properties = typeof(ActivityLog).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAuditbleEventsActivityLog::loadAcitivityLogUser", PROC_PATIENTAUDITLOG_LOAD_USER, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public List<ActivityLog> loadAcitivityLogComponents(ActivityLog model)
        {
            List<ActivityLog> listobj = new List<ActivityLog>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARAM_PROFILENAME, model.ProfileName);
                dbManager.AddParameters(1, PARM_CREATED_DATE, model.DateAndTime);
                dbManager.AddParameters(2, PARM_PATIENT_ID, model.PatientId);
                dbManager.AddParameters(3, PARM_USER_ID, model.UserId);
                dbManager.AddParameters(4, PARM_PAGE_NUMBER, model.PageNumber);
                dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, model.RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, "", DbType.Int64, ParamDirection.Output);

                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENTAUDITLOG_LOAD_COMPONENTS);
                while (reader.Read())
                {
                    ActivityLog model1 = new ActivityLog();
                    var properties = typeof(ActivityLog).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAuditbleEventsActivityLog::loadAcitivityLogComponents", PROC_PATIENTAUDITLOG_LOAD_COMPONENTS, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public List<ActivityLog> loadAcitivityLogChanges(ActivityLog model)
        {
            List<ActivityLog> listobj = new List<ActivityLog>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_COLUMNKEY_ID, model.ColumnKeyId);
                dbManager.AddParameters(1, PARAM_PROFILENAME, model.ProfileName);
                dbManager.AddParameters(2, PARM_CREATED_DATE, model.DateAndTime);
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, model.PageNumber);
                dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, model.RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, "", DbType.Int64, ParamDirection.Output);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENTAUDITLOG_LOAD_CHANGES);
                while (reader.Read())
                {
                    ActivityLog model1 = new ActivityLog();
                    var properties = typeof(ActivityLog).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAuditbleEventsActivityLog::loadAcitivityLogChanges", PROC_PATIENTAUDITLOG_LOAD_CHANGES, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public List<ActivityLog> loadCheckInAppUser(ActivityLog model)
        {
            List<ActivityLog> listobj = new List<ActivityLog>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (model.UserName == "")
                {
                    model.UserName = null;
                }
                if (model.Facility == "")
                {
                    model.Facility = null;
                }
                if (model.DeviceId == "")
                {
                    model.DeviceId = null;
                }
                if (model.Status == "")
                {
                    model.Status = null;
                }
                if (model.EntityId == "")
                {
                    model.EntityId = null;
                }
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, "@UserName", model.UserName);
                dbManager.AddParameters(1, "@Facility", model.Facility);
                dbManager.AddParameters(2, "@DeviceId", model.DeviceId);
                dbManager.AddParameters(3, "@EntityId", model.EntityId);
                dbManager.AddParameters(4, "@IsActive", model.Status);
                dbManager.AddParameters(5, "@PageNumber", model.PageNumber);
                dbManager.AddParameters(6, "@RowspPage", model.RowsPerPage);
                dbManager.AddParameters(7, "@RecordCount", "", DbType.Int64, ParamDirection.Output);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CHECKINAPPUSER_LOAD);
                while (reader.Read())
                {
                    ActivityLog model1 = new ActivityLog();
                    var properties = typeof(ActivityLog).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAuditbleEventsActivityLog::loadCheckInAppUser", PROC_CHECKINAPPUSER_LOAD, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }


        #endregion

    }
}
