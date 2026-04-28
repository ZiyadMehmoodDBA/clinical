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
using MDVision.Model.Batch.ExportCCDA;

namespace MDVision.DataAccess.DAL.Batch
{
    /// <summary>
    /// Author: Zia Mehmood
    /// Overview: Data Access Layer for Export CCDA
    /// </summary>
    public class DALExportCCDA
    {
        #region " Stored Procedure Names "

        private const string PROC_EXPORTCCDA_LOAD_PATIENTLOOKUP = "clinical.sp_ExportCDDA_PatientLookup";
        private const string PROC_EXPORTCCDA_LOAD_PROVIDERPATIENTLOOKUP = "clinical.sp_ExportCDDA_ProviderPatientLookup";
        private const string PROC_EXPORTCCDA_LOAD_NOTECOMPONENTS = "clinical.sp_ExportCDDA_NoteComponentsLookup";
        private const string PROC_EXPORTCCDA_INSERT_SCHEDULER = "Clinical.CCDASchedulerDataInsert";
        private const string PROC_EXPORTCCDA_LOAD_SCHEDULER = "[Clinical].[CCDA_SchedulerSelect]";
        private const string PROC_EXPORTCCDA_DELETE_SCHEDULER = "[Clinical].[CCDA_SchedulerDelete]";
        private const string PROC_EXPORTCCDA_ACTIVE_INACTIVE_SCHEDULER = "[Clinical].[CCDA_SchedulerActiveInActive]";
        private const string PROC_EXPORTCCDA_GET_SCHEDULED_PATIENTVISITS = "[Clinical].[Sp_GetScheduledPatientVisits]";



        #endregion



        #region " Constructors "
        public DALExportCCDA()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALExportCCDA(SharedVariable SharedVariable)
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
        private const string PARM_PATIENT_NAME = "@PatientName";
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

        private const string PARM_SCHEDULERID = "@SchedulerId";
        private const string PARM_RULETYPE = "@RuleType";
        private const string PARM_FILELOCATION = "@FileLocation";
        private const string PARM_DOSFROM = "@DOSFrom";
        private const string PARM_DOSTO = "@DOSTo";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_CREATEDBY = "@CreatedBy";
        private const string PARM_CREATEDON = "@CreatedOn";
        private const string PARM_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_MODIFIEDON = "@ModifiedOn";
        private const string PARM_SCHEDULERTIME = "@SchedulerTime";
        private const string PARM_SCHEDULERYEAR = "@SchedulerYear";
        private const string PARM_SCHEDULERMONTH = "@SchedulerMonth";
        private const string PARM_SCHEDULARWEEK = "@SchedularWeek";
        private const string PARM_SCHEDULERDAY = "@SchedulerDay";
        private const string PARM_ISPATIENTSALL = "@IsPatientsAll";
        private const string PARM_ISCOMPONENTSALL = "@IsComponentsAll";
        private const string PARM_PATIENTIDS = "@PatientIds";
        private const string PARM_COMPONENTIDS = "@ComponentIds";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_Export_Date = "@ExportDate";
        private const string PARAM_SCHEDULER_HOUR = "@SchedulerHour";
        private const string PARAM_PROVIDER_ID = "@ProviderId";


        #endregion

        #region "Methods"


        public List<ExportCCDAModel> Fill_Paitent_Lookpup(ExportCCDAModel model)
        {
            List<ExportCCDAModel> listobj = new List<ExportCCDAModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                
                SqlDataReader reader = null;
                if (model.ProviderId != "" && model.ProviderId != null)
                {
                    dbManager.AddParameters(0, PARAM_PROVIDER_ID, model.ProviderId);
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EXPORTCCDA_LOAD_PROVIDERPATIENTLOOKUP);
                }
                else {
                    dbManager.AddParameters(0, PARM_PATIENT_NAME, model.PatientName);
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EXPORTCCDA_LOAD_PATIENTLOOKUP);
                }
                
                while (reader.Read())
                {
                    ExportCCDAModel model1 = new ExportCCDAModel();
                    var properties = typeof(ExportCCDAModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALExportCCDA::Fill_Paitent_Lookpup", PROC_EXPORTCCDA_LOAD_PATIENTLOOKUP, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public List<ExportCCDAModel> Get_Scheduled_PatientVisits(ExportCCDAModel model)
        {
            List<ExportCCDAModel> listobj = new List<ExportCCDAModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (model.DateFrom == "")
                {
                    model.DateFrom = null;
                }
                if (model.DateTo == "")
                {
                    model.DateTo = null;
                }
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, model.PatientId);
                dbManager.AddParameters(1, PARM_DOSFROM, model.DateFrom);
                dbManager.AddParameters(2, PARM_DOSTO, model.DateTo);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EXPORTCCDA_GET_SCHEDULED_PATIENTVISITS);
                while (reader.Read())
                {
                    ExportCCDAModel model1 = new ExportCCDAModel();
                    var properties = typeof(ExportCCDAModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALExportCCDA::Fill_Paitent_Lookpup", PROC_EXPORTCCDA_GET_SCHEDULED_PATIENTVISITS, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }

        public List<ExportCCDAModel> Fill_NoteComponent_Lookpup(ExportCCDAModel model)
        {
            List<ExportCCDAModel> listobj = new List<ExportCCDAModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();



                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EXPORTCCDA_LOAD_NOTECOMPONENTS);
                while (reader.Read())
                {
                    ExportCCDAModel model1 = new ExportCCDAModel();
                    var properties = typeof(ExportCCDAModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALExportCCDA::Fill_NoteComponent_Lookpup", PROC_EXPORTCCDA_LOAD_NOTECOMPONENTS, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }

        public ExportCCDAModel Insert_CCDA_Schedule(ExportCCDAModel model)
        {
            
            if (model.SchedulerId == "")
            {
                model.SchedulerId = null;
            }
            if (model.IsActive == "")
            {
                model.IsActive = "1";
            }
            if (model.DateFrom == "")
            {
                model.DateFrom = null;
            }
            if (model.DateTo == "")
            {
                model.DateTo = null;
            }
            if (model.ProviderId == "")
            {
                model.ProviderId = null;
            }


            IDBManager dbManager = ClientConfiguration.GetDBManager(); 
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(22);
                dbManager.AddParameters(0, PARM_SCHEDULERID,model.SchedulerId );
                dbManager.AddParameters(1, PARM_RULETYPE, model.RuleType);
                dbManager.AddParameters(2, PARM_FILELOCATION, model.FilePath);
                dbManager.AddParameters(3, PARM_DOSFROM, model.DateFrom);
                dbManager.AddParameters(4, PARM_DOSTO, model.DateTo);
                dbManager.AddParameters(5, PARM_ISACTIVE, model.IsActive);
                dbManager.AddParameters(6, PARM_CREATEDBY, model.CreatedBy);
                dbManager.AddParameters(7, PARM_CREATEDON, model.CreatedOn);
                dbManager.AddParameters(8, PARM_MODIFIEDBY, model.ModifiedBy);
                dbManager.AddParameters(9, PARM_MODIFIEDON, model.ModifiedOn);
                dbManager.AddParameters(10, PARM_SCHEDULERTIME, model.MultiTime);
                dbManager.AddParameters(11, PARM_SCHEDULERYEAR, model.Years);
                dbManager.AddParameters(12, PARM_SCHEDULERMONTH, model.Months);
                dbManager.AddParameters(13, PARM_SCHEDULARWEEK, model.Weeks);
                dbManager.AddParameters(14, PARM_SCHEDULERDAY, model.Days);
                dbManager.AddParameters(15, PARM_ISPATIENTSALL, model.IsPatientsAll);
                dbManager.AddParameters(16, PARM_ISCOMPONENTSALL, model.IsComponentsAll);
                dbManager.AddParameters(17, PARM_PATIENTIDS, model.SecPatient);
                dbManager.AddParameters(18, PARM_COMPONENTIDS, model.NoteComponents);
                dbManager.AddParameters(19, PARM_Export_Date, model.DateExport);
                dbManager.AddParameters(20, PARAM_SCHEDULER_HOUR, model.SchedulerHour);
                dbManager.AddParameters(21, PARAM_PROVIDER_ID, model.ProviderId);

                model.SchedulerId=dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EXPORTCCDA_INSERT_SCHEDULER).ToString();
               
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALExportCCDA::Insert_CCDA_Schedule", PROC_EXPORTCCDA_INSERT_SCHEDULER, ex);
                model.IsError = ex.Message;
                return model;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ExportCCDAModel> Load_CCDA_Schedule(ExportCCDAModel model)
        {
            if (model.SchedulerId == "")
            {
                model.SchedulerId = null;
            }
            if (model.IsActive == "")
            {
                model.IsActive = null;
            }
            if (model.RowsPerPage == "")
            {
                model.RowsPerPage = "15";
            }
            if (model.PageNumber == "")
            {
                model.PageNumber = "1";
            }
            List<ExportCCDAModel> listobj = new List<ExportCCDAModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_SCHEDULERID, model.SchedulerId);
                dbManager.AddParameters(1, PARM_ISACTIVE, model.IsActive);
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, model.PageNumber);
                dbManager.AddParameters(2, PARM_PAGE_NUMBER, model.RowsPerPage);
                dbManager.AddParameters(2, PARM_RECORD_COUNT, "", DbType.Int64, ParamDirection.Output);
    
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EXPORTCCDA_LOAD_SCHEDULER);
                while (reader.Read())
                {
                    ExportCCDAModel model1 = new ExportCCDAModel();
                    var properties = typeof(ExportCCDAModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALExportCCDA::Load_CCDA_Schedule", PROC_EXPORTCCDA_LOAD_SCHEDULER, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public List<ExportCCDAModel> Select_CCDA_Schedule(ExportCCDAModel model)
        {
            List<ExportCCDAModel> listobj = new List<ExportCCDAModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SCHEDULERID, model.SchedulerId);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EXPORTCCDA_LOAD_SCHEDULER);
                while (reader.Read())
                {
                    ExportCCDAModel model1 = new ExportCCDAModel();
                    var properties = typeof(ExportCCDAModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALExportCCDA::Select_CCDA_Schedule", PROC_EXPORTCCDA_LOAD_SCHEDULER, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public string Delete_CCDA_Schedule(long SchedulerId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SCHEDULERID, SchedulerId);
              
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EXPORTCCDA_DELETE_SCHEDULER).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALExportCCDA::Delete_CCDA_Schedule", PROC_EXPORTCCDA_DELETE_SCHEDULER, ex);
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
        public string ActiveInactive_CCDA_Schedule(long SchedulerId, string IsActive, string ModifiedBy)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_SCHEDULERID, SchedulerId);
                dbManager.AddParameters(1, PARM_ISACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_MODIFIEDBY, ModifiedBy);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EXPORTCCDA_ACTIVE_INACTIVE_SCHEDULER).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALExportCCDA::ActiveInactive_CCDA_Schedule", PROC_EXPORTCCDA_ACTIVE_INACTIVE_SCHEDULER, ex);
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
        #endregion

    }
}
