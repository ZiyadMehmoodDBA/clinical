/*
 * Author: Muhammad Irfan
 * Date: 10/12/2015
 * Created for face sheet in EMR Clinical module to handle CRUD functions
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Model.FaceSheet;
using System.Data.SqlClient;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.Model.Common;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALFaceSheet
    {
        #region Variable

        #endregion

        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Defines Stored procedures of facesheet
         */
        #region "Stored Procedure Names"

        private const string PROC_FACESHEET_INSERT = "Clinical.sp_FaceSheetInsert";
        // private const string PROC_FACESHEET_UPDATE = "Clinical.sp_FaceSheetUpdate";
        private const string PROC_FACESHEET_UPDATE = "Clinical.sp_FS_Update";
        private const string PROC_FACESHEET_DELETE = "Clinical.sp_FaceSheetDelete";
        private const string PROC_FACESHEET_SELECT = "Clinical.sp_FaceSheetSelect";

        #endregion

        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Defines Parameters for facesheet
         */
        #region "Parameters"

        private const string PARM_FACESHEET_ID = "@FaceSheetId";
        private const string PARM_COMP_IDS = "@ComponentId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_COMPONENT_NAME = "@ComponentName";
        private const string PARM_COMPONENT_ORDER = "@ComponentOrder";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_SORT_ORDER = "@SortOrder";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PATIENT_ID = "@PatientId";


        private const string PROC_ALLERGY_SELECT = "Clinical.sp_FS_AllergySelect";
        private const string PROC_PROBLEM_LIST_SELECT = "Clinical.sp_FS_ProblemListSelect";
        private const string PROC_VITALS_SELECT = "Clinical.sp_FS_VitalSignsSelect";
        private const string PROC_NOTES_SELECT = "Clinical.sp_FS_NotesSelect";
        private const string PROC_DASHBOARD_VISIT_SELECT = "Clinical.sp_FS_DashboardVisitSelect";
        private const string PROC_HISOTRY_SELECT = "Clinical.sp_FS_HistorySummaryForSoapSelect";
        private const string PROC_LAB_RESULT_SELECT = "Clinical.sp_FS_LabOrderResultDetailSelect";
        private const string PROC_LAB_ORDER_SELECT = "Clinical.sp_FS_LabOrderSelect";
        private const string PROC_PROCEDURE_ORDER_SELECT = "Clinical.sp_FS_ProcedureOrderSelect";
        private const string PROC_RADIOLOGY_ORDER_SELECT = "Clinical.sp_FS_RadiologyOrderSelect";
        private const string PROC_MEDICATIONS_SELECT = "Clinical.sp_FS_MedicationSelect";
        private const string PROC_REFERRALS_SELECT = "Clinical.sp_FS_ReferralsSelect";
        private const string PROC_IMMUNIZATION_SELECT = "Clinical.sp_FS_VaccineFaceSheetSelect";
        private const string PROC_COMPLAINTS_SELECT = "Clinical.sp_FS_FaceSheetComplaintSelect";
        private const string PROC_PATIENT_DOCUMENT_SELECT = "Clinical.sp_FS_FaceSheetPatientDocumentSelect";
        private const string PROC_IMPLANTABLE_DEVICE_SELECT = "Clinical.Sp_implantabledeviceFSselect";

        #endregion

        #region Constructors
        public DALFaceSheet()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALFaceSheet(SharedVariable SharedVariable)
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

        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Supproting functions for facesheet
         */
        #region "Support Functions Face Sheet"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void createParameters(IDBManager dbManager, DSFaceSheet ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FACESHEET_ID, ds.FaceSheet.FaceSheetIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FACESHEET_ID, ds.FaceSheet.FaceSheetIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(1, PARM_USER_ID, ds.FaceSheet.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_COMPONENT_NAME, ds.FaceSheet.ComponentNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_COMPONENT_ORDER, ds.FaceSheet.ComponentOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.FaceSheet.RecordCountColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_SORT_ORDER, ds.FaceSheet.SortOrderColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.FaceSheet.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.FaceSheet.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.FaceSheet.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.FaceSheet.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.FaceSheet.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Add/Update/Delete/Load functions
         */
        #region "Face Sheet"
        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Insert function for facesheet DAL
         */
        public DSFaceSheet insertFaceSheet(DSFaceSheet ds)
        {
            // DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createParameters(dbManager, ds, true);
                ds = (DSFaceSheet)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FACESHEET_INSERT, ds, ds.FaceSheet.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFaceSheet::insertFaceSheet", PROC_FACESHEET_INSERT, ex);
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
        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Update function for facesheet DAL
         */
        public string updateFaceSheet(String compIDs)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (compIDs == "")
                    dbManager.AddParameters(0, PARM_COMP_IDS, null);
                else
                    dbManager.AddParameters(0, PARM_COMP_IDS, compIDs);

                int result = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_FACESHEET_UPDATE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFaceSheet::updateFaceSheet", PROC_FACESHEET_UPDATE, ex);
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
        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Delete function for facesheet DAL
         */
        public string deleteFaceSheet(string faceSheetId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FACESHEET_ID, faceSheetId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FACESHEET_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFaceSheet::deleteFaceSheet", PROC_FACESHEET_DELETE, ex);
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
        /*
         * Author: Muhammad Irfan
         * Date: 10/12/2015
         * Overview: Load function for facesheet DAL
         */
        public List<FaceSheetOrderModel> loadFaceSheet(SharedVariable SharedVariable,long faceSheetId, long userId, long PatientId, string isViewFacesheet = "", string isPrintFacesheet = "")
        {
            List<FaceSheetOrderModel> listFSOrder = new List<FaceSheetOrderModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_FACESHEET_ID, faceSheetId > 0 ? faceSheetId : (long?)null));
                parameters.Add(new SqlParameter(PARM_USER_ID, userId > 0 ? userId : (long?)null));
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId > 0 ? PatientId : (long?)null));

                using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(PROC_FACESHEET_SELECT, parameters))
                {
                    FaceSheetOrderModel model = null;
                    while (reader.Read())
                    {
                        model = new FaceSheetOrderModel();
                        model.FaceSheetId = !String.IsNullOrEmpty(reader["FaceSheetId"].ToString()) ? reader["FaceSheetId"].ToString() : "";
                        model.UserId = !String.IsNullOrEmpty(reader["UserId"].ToString()) ? reader["UserId"].ToString() : "";
                        model.ComponentName = !String.IsNullOrEmpty(reader["ComponentName"].ToString()) ? reader["ComponentName"].ToString() : "";
                        model.ComponentOrder = !String.IsNullOrEmpty(reader["ComponentOrder"].ToString()) ? reader["ComponentOrder"].ToString() : "";
                        model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";

                        listFSOrder.Add(model);
                    }

                }

                return listFSOrder;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::loadFaceSheet", PROC_FACESHEET_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFaceSheet loadFaceSheet_DS(long faceSheetId, long userId, long PatientId, string isViewFacesheet = "", string isPrintFacesheet = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSFaceSheet ds = new DSFaceSheet();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (faceSheetId == 0)
                    dbManager.AddParameters(0, PARM_FACESHEET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FACESHEET_ID, faceSheetId);
                if (userId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, userId);
                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                ds = (DSFaceSheet)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACESHEET_SELECT, ds, ds.FaceSheet.TableName);

                //foreach(DSFaceSheet.FaceSheetRow dr in ds.FaceSheet){
                //    dr["PatientId"] = Patient;
                //}
                //DSFaceSheet dsDBaudit;
                //foreach(DSFaceSheet.FaceSheetRow drFaceSheetRow in ds.FaceSheet)
                //{
                //    DSFaceSheet.FaceSheetRow FaceSheetRow = ds.FaceSheet.NewFaceSheetRow();
                //    FaceSheetRow.FaceSheetId=drFaceSheetRow[ds.];
                //    FaceSheetRow.UserId=drFaceSheetRow.UserId;
                //    FaceSheetRow.ComponentName=drFaceSheetRow.ComponentName;
                //    FaceSheetRow.ComponentOrder=drFaceSheetRow.ComponentOrder;
                //    FaceSheetRow.SortOrder=drFaceSheetRow.SortOrder;
                //    FaceSheetRow.IsActive=drFaceSheetRow.IsActive;
                //    FaceSheetRow.CreatedBy=drFaceSheetRow.CreatedBy;
                //    FaceSheetRow.CreatedOn=drFaceSheetRow.CreatedOn;
                //    FaceSheetRow.ModifiedBy=drFaceSheetRow.ModifiedBy;
                //    FaceSheetRow.ModifiedOn=drFaceSheetRow.ModifiedOn;
                //    FaceSheetRow.PatientId=Patient;
                //    dsDBaudit.FaceSheet.AddFaceSheetRow(FaceSheetRow.cop);
                //}

                //dsDBaudit.Tables.Add(dataTableFromDataSetY.Copy()); ;
                DataTable dtTemp = ds.FaceSheet;
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    if (isViewFacesheet == "1" || isPrintFacesheet == "1")
                    {
                        bool isViewAction = isViewFacesheet == "1" ? true : false;
                        bool isPrintAcion = isPrintFacesheet == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FaceSheet.Rows[0][ds.FaceSheet.FaceSheetIdColumn].ToString(), null, ds.FaceSheet.Rows[0][ds.FaceSheet.FaceSheetIdColumn].ToString(), isViewAction, isPrintAcion);
                        dsDBAudit.AcceptChanges();
                    }
                }

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALFaceSheet::loadFaceSheet", PROC_FACESHEET_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<FSAllergyModel> loadFaceSheetAllergies(SharedVariable SharedVariable,long patientId)
        {
            List<FSAllergyModel> listAllergies = new List<FSAllergyModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ALLERGY_SELECT);

                FSAllergyModel model = null;
                while (reader.Read())
                {
                    model = new FSAllergyModel();
                    model.Allergen = !String.IsNullOrEmpty(reader["Allergen"].ToString()) ? reader["Allergen"].ToString() : "";
                    model.Reaction = !String.IsNullOrEmpty(reader["Reaction"].ToString()) ? reader["Reaction"].ToString() : "";
                    model.Severity = !String.IsNullOrEmpty(reader["OnsetDate"].ToString()) ? reader["OnsetDate"].ToString() : "";

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::loadAllergies", PROC_ALLERGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSProblemListModel> loadFaceSheetProblemList(SharedVariable SharedVariable,long patientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

            try
            {
                dbManager.Open();

                List<FSProblemListModel> listProblemList = new List<FSProblemListModel>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                long? PatientId =  patientId == 0 ? (long?)null : patientId;
                parameters.Add(new SqlParameter(PARM_PATIENT_ID,PatientId));

                using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(PROC_PROBLEM_LIST_SELECT, parameters))
                {
                    FSProblemListModel model = null;

                    while (reader.Read())
                    {
                        model = new FSProblemListModel();

                        model.ProblemName = MDVUtility.CheckStringNull(reader["ProblemName"]);
                        model.ChronicityLevel = MDVUtility.CheckStringNull(reader["ChronicityLevel"]);
                        model.IsActive = MDVUtility.CheckStringNull(reader["IsActive"]);
                        model.InActiveChkBoxValue = MDVUtility.CheckStringNull(reader["InActiveChkBoxValue"]);

                        listProblemList.Add(model);
                    }
                }

                return listProblemList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::ProblemList", PROC_PROBLEM_LIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<FSVitalsModel> loadFaceSheetVitals(SharedVariable SharedVariable,long patientId)
        {
            List<FSVitalsModel> listVitals = new List<FSVitalsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_VITALS_SELECT);

                FSVitalsModel model = null;
                while (reader.Read())
                {
                    model = new FSVitalsModel();
                    model.Systolic = !String.IsNullOrEmpty(reader["Systolic"].ToString()) ? reader["Systolic"].ToString() : "";
                    model.Diastolic = !String.IsNullOrEmpty(reader["Diastolic"].ToString()) ? reader["Diastolic"].ToString() : "";
                    model.TemperatureResult = !String.IsNullOrEmpty(reader["TemperatureResult"].ToString()) ? reader["TemperatureResult"].ToString() : "";
                    model.BMI = !String.IsNullOrEmpty(reader["BMI"].ToString()) ? reader["BMI"].ToString() : "";
                    model.VitalSignDate = !String.IsNullOrEmpty(reader["VitalSignDate"].ToString()) ? reader["VitalSignDate"].ToString() : "";
                    listVitals.Add(model);
                }

                return listVitals;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFaceSheet::Vitals", PROC_VITALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSNotesModel> loadFaceSheetNotes(SharedVariable SharedVariable,long patientId)
        {
            List<FSNotesModel> listNotes = new List<FSNotesModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTES_SELECT);

                FSNotesModel model = null;
                while (reader.Read())
                {
                    model = new FSNotesModel();
                    model.VisitReason = !String.IsNullOrEmpty(reader["VisitReason"].ToString()) ? reader["VisitReason"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.VisitDate = !String.IsNullOrEmpty(reader["VisitDate"].ToString()) ? reader["VisitDate"].ToString() : "";

                    listNotes.Add(model);
                }

                return listNotes;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::Notes", PROC_NOTES_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSAppointmentModel> loadFaceSheetAppointments(SharedVariable SharedVariable,long patientId)
        {
            List<FSAppointmentModel> listAppointments = new List<FSAppointmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_VISIT_SELECT);

                FSAppointmentModel model = null;
                while (reader.Read())
                {
                    model = new FSAppointmentModel();
                    model.SchReason = !String.IsNullOrEmpty(reader["SchReason"].ToString()) ? reader["SchReason"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.ScheduleDate = !String.IsNullOrEmpty(reader["ScheduleDate"].ToString()) ? reader["ScheduleDate"].ToString() : "";
                    model.ScheduleTimeFrom = !String.IsNullOrEmpty(reader["ScheduleTimeFrom"].ToString()) ? reader["ScheduleTimeFrom"].ToString() : "";

                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.PatientName = !String.IsNullOrEmpty(reader["PatientName"].ToString()) ? reader["PatientName"].ToString() : "";
                  
                    model.Gender = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.DOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? reader["DOB"].ToString() : "";
                    model.CellNo = !String.IsNullOrEmpty(reader["CellNo"].ToString()) ? reader["CellNo"].ToString() : "";
                    model.EmailAddress = !String.IsNullOrEmpty(reader["EmailAddress"].ToString()) ? reader["EmailAddress"].ToString() : "";
                    model.InsurancePlanName = !String.IsNullOrEmpty(reader["InsurancePlanName"].ToString()) ? reader["InsurancePlanName"].ToString() : "";
                    model.Duration = !String.IsNullOrEmpty(reader["Duration"].ToString()) ? reader["Duration"].ToString() : "";
                    model.AppointmentId = !String.IsNullOrEmpty(reader["AppointmentId"].ToString()) ? reader["AppointmentId"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.PatientType = !String.IsNullOrEmpty(reader["PatientType"].ToString()) ? reader["PatientType"].ToString() : "";
                    model.ReminderResponseDelivery = !String.IsNullOrEmpty(reader["ReminderResponseDelivery"].ToString()) ? reader["ReminderResponseDelivery"].ToString() : "";
                    model.ReminderResponseMessage = !String.IsNullOrEmpty(reader["ReminderResponseMessage"].ToString()) ? reader["ReminderResponseMessage"].ToString() : "";
                    model.ReminderStatus = !String.IsNullOrEmpty(reader["ReminderStatus"].ToString()) ? reader["ReminderStatus"].ToString() : "";
                    model.copayment = !String.IsNullOrEmpty(reader["copayment"].ToString()) ? reader["copayment"].ToString() : "";
                    model.AppointmentStatus = !String.IsNullOrEmpty(reader["AppointmentStatus"].ToString()) ? reader["AppointmentStatus"].ToString() : "";
                  
                    model.PatientVisitType = !String.IsNullOrEmpty(reader["PatientVisitType"].ToString()) ? reader["PatientVisitType"].ToString() : "";
                    model.VisitTypeColor = !String.IsNullOrEmpty(reader["VisitTypeColor"].ToString()) ? reader["VisitTypeColor"].ToString() : "";
                    model.FacilityName = !String.IsNullOrEmpty(reader["FacilityName"].ToString()) ? reader["FacilityName"].ToString() : "";
                    model.CancellationReason = !String.IsNullOrEmpty(reader["CancellationReason"].ToString()) ? reader["CancellationReason"].ToString() : "";
                    listAppointments.Add(model);
                }

                return listAppointments;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::Appointments", PROC_DASHBOARD_VISIT_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSHistoryModel> loadFaceSheetHistory(SharedVariable SharedVariable,long patientId)
        {
            List<FSHistoryModel> listHistory = new List<FSHistoryModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HISOTRY_SELECT);

                FSHistoryModel model = null;
                while (reader.Read())
                {
                    model = new FSHistoryModel();
                    model.SocialHxSoapText = !String.IsNullOrEmpty(reader["SocialHxSoapText"].ToString()) ? reader["SocialHxSoapText"].ToString() : "";
                    model.BirthHxSoapText = !String.IsNullOrEmpty(reader["BirthHxSoapText"].ToString()) ? reader["BirthHxSoapText"].ToString() : "";
                    model.MedicalHxSoapText = !String.IsNullOrEmpty(reader["MedicalHxSoapText"].ToString()) ? reader["MedicalHxSoapText"].ToString() : "";
                    model.SurgicalHxSoapText = !String.IsNullOrEmpty(reader["SurgicalHxSoapText"].ToString()) ? reader["SurgicalHxSoapText"].ToString() : "";
                    model.FamilyHxSoapText = !String.IsNullOrEmpty(reader["FamilyHxSoapText"].ToString()) ? reader["FamilyHxSoapText"].ToString() : "";
                    model.HospitalizationHxSoapText = !String.IsNullOrEmpty(reader["HospitalizationHxSoapText"].ToString()) ? reader["HospitalizationHxSoapText"].ToString() : "";
                    model.SocPsyandBehaviorHxSoapText = !String.IsNullOrEmpty(reader["SocPsyandBehaviorHxSoapText"].ToString()) ? reader["SocPsyandBehaviorHxSoapText"].ToString() : "";
                    listHistory.Add(model);
                }

                return listHistory;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::History", PROC_HISOTRY_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSLabResultsModel> loadFaceSheetLabResult(SharedVariable SharedVariable,long patientId)
        {
            List<FSLabResultsModel> listLabResult = new List<FSLabResultsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LAB_RESULT_SELECT);

                FSLabResultsModel model = null;
                while (reader.Read())
                {
                    model = new FSLabResultsModel();
                   // model.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                   // model.CPTCodeDescription = !String.IsNullOrEmpty(reader["CPTCodeDescription"].ToString()) ? reader["CPTCodeDescription"].ToString() : "";

                    string str = reader["ResultDetail"].ToString();
                    string[] strArray;
                    strArray = str.Split(',');
                    model.CPTCodeDescription = strArray[0];
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";

                    listLabResult.Add(model);
                }

                return listLabResult;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::LabResult", PROC_LAB_RESULT_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSLabOrdersModel> loadFaceSheetLabOrder(SharedVariable SharedVariable, long patientId)
        {
            List<FSLabOrdersModel> listLabOrder = new List<FSLabOrdersModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LAB_ORDER_SELECT);

                FSLabOrdersModel model = null;
                while (reader.Read())
                {
                    model = new FSLabOrdersModel();
                    model.Test = !String.IsNullOrEmpty(reader["Test"].ToString()) ? reader["Test"].ToString() : "";
                    model.OrderDate = !String.IsNullOrEmpty(reader["OrderDate"].ToString()) ? reader["OrderDate"].ToString() : "";

                    listLabOrder.Add(model);
                }

                return listLabOrder;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::LabOrder", PROC_LAB_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSProcedureOrderModel> loadFaceSheetProcedureOrder(SharedVariable SharedVariable,long patientId)
        {
            List<FSProcedureOrderModel> listProcedureOrder = new List<FSProcedureOrderModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_SELECT);

                FSProcedureOrderModel model = null;
                while (reader.Read())
                {
                    model = new FSProcedureOrderModel();
                    model.Procedures = !String.IsNullOrEmpty(reader["Procedures"].ToString()) ? reader["Procedures"].ToString() : "";
                    model.OrderDate = !String.IsNullOrEmpty(reader["OrderDate"].ToString()) ? reader["OrderDate"].ToString() : "";

                    listProcedureOrder.Add(model);
                }

                return listProcedureOrder;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::ProcedureOrder", PROC_PROCEDURE_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSRadiologyOrdersModel> loadFaceSheetRadiologyOrder(SharedVariable SharedVariable,long patientId)
        {
            List<FSRadiologyOrdersModel> listRadiologyOrder = new List<FSRadiologyOrdersModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_SELECT);

                FSRadiologyOrdersModel model = null;
                while (reader.Read())
                {
                    model = new FSRadiologyOrdersModel();
                    model.Test = !String.IsNullOrEmpty(reader["Test"].ToString()) ? reader["Test"].ToString() : "";
                    model.OrderDate = !String.IsNullOrEmpty(reader["OrderDate"].ToString()) ? reader["OrderDate"].ToString() : "";

                    listRadiologyOrder.Add(model);
                }

                return listRadiologyOrder;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::RadiologyOrder", PROC_RADIOLOGY_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSMedicationsModel> loadFaceSheetMedications(SharedVariable SharedVariable,long patientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                List<FSMedicationsModel> listMedications = new List<FSMedicationsModel>();
                dbManager.Open();

                long? PatientId = patientId == 0 ? (long?)null : patientId;
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                using(SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(PROC_MEDICATIONS_SELECT, parameters))
                {
                    FSMedicationsModel model = null;
                    while (reader.Read())
                    {
                        model = new FSMedicationsModel();

                        model.MedicationName = !String.IsNullOrEmpty(reader["MedicationName"].ToString()) ? reader["MedicationName"].ToString() : "";
                        model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";

                        listMedications.Add(model);
                    }
                }

                return listMedications;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::Medications", PROC_MEDICATIONS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<FSReferralsModel> loadFaceSheetReferrals(SharedVariable SharedVariable,long patientId)
        {
            List<FSReferralsModel> listReferrals = new List<FSReferralsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REFERRALS_SELECT);

                FSReferralsModel model = null;
                while (reader.Read())
                {
                    model = new FSReferralsModel();
                    model.Procedures = !String.IsNullOrEmpty(reader["Procedures"].ToString()) ? reader["Procedures"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";

                    listReferrals.Add(model);
                }

                return listReferrals;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::Referrals", PROC_REFERRALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSImmunizationsModel> loadFaceSheetImmunization(SharedVariable SharedVariable,long patientId)
        {
            List<FSImmunizationsModel> listImmunization = new List<FSImmunizationsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMMUNIZATION_SELECT);

                FSImmunizationsModel model = null;
                while (reader.Read())
                {
                    model = new FSImmunizationsModel();
                    model.VaccineName = !String.IsNullOrEmpty(reader["VaccineName"].ToString()) ? reader["VaccineName"].ToString() : "";
                    model.Type = !String.IsNullOrEmpty(reader["Type"].ToString()) ? reader["Type"].ToString() : "";
                    model.AdministrationDate = !String.IsNullOrEmpty(reader["AdministrationDate"].ToString()) ? reader["AdministrationDate"].ToString() : "";

                    listImmunization.Add(model);
                }

                return listImmunization;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::Immunization", PROC_IMMUNIZATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSComplaintsModel> loadFaceSheetComplaints(SharedVariable SharedVariable,long patientId)
        {
            List<FSComplaintsModel> listComplaints = new List<FSComplaintsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_COMPLAINTS_SELECT);

                FSComplaintsModel model = null;
                while (reader.Read())
                {
                    model = new FSComplaintsModel();
                    model.ComplaintDescription = !String.IsNullOrEmpty(reader["ComplaintDescription"].ToString()) ? reader["ComplaintDescription"].ToString() : "";
                    model.VisitDate = !String.IsNullOrEmpty(reader["VisitDate"].ToString()) ? reader["VisitDate"].ToString() : "";
                    model.IsChiefComplaint = !String.IsNullOrEmpty(reader["IsChiefComplaint"].ToString()) ? reader["IsChiefComplaint"].ToString() : "";
                    listComplaints.Add(model);
                }

                return listComplaints;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::Complaints", PROC_COMPLAINTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSPatientDocumentModel> loadFaceSheetPatientDocument(SharedVariable SharedVariable,long patientId)
        {
            List<FSPatientDocumentModel> listPatientDocument = new List<FSPatientDocumentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_SELECT);

                FSPatientDocumentModel model = null;
                while (reader.Read())
                {
                    model = new FSPatientDocumentModel();
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.UploadDate = !String.IsNullOrEmpty(reader["UploadDate"].ToString()) ? reader["UploadDate"].ToString() : "";
                    listPatientDocument.Add(model);
                }

                return listPatientDocument;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::loadFaceSheetPatientDocument", PROC_COMPLAINTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSImplantableDevicesModel> loadFaceSheetImplantableDevices(SharedVariable SharedVariable,long patientId)
        {
            List<FSImplantableDevicesModel> listImplantableDevices = new List<FSImplantableDevicesModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICE_SELECT);

                FSImplantableDevicesModel model = null;
                while (reader.Read())
                {
                    model = new FSImplantableDevicesModel();
                    model.GMDNPName = !String.IsNullOrEmpty(reader["DeviceName"].ToString()) ? reader["DeviceName"].ToString() : "";
                    model.DI = !String.IsNullOrEmpty(reader["DeviceId"].ToString()) ? reader["DeviceId"].ToString() : "";
                    model.Status = !String.IsNullOrEmpty(reader["Status"].ToString()) ? reader["Status"].ToString() : "";
                    model.Expiration_Date = !String.IsNullOrEmpty(reader["Expiry"].ToString()) ? reader["Expiry"].ToString() : "";
                    listImplantableDevices.Add(model);
                }

                return listImplantableDevices;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALFaceSheet::loadFaceSheetImplantableDevices", PROC_IMPLANTABLE_DEVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<FSImplantableDevicesModel> loadFaceSheetImplantableDevices(long patientId)
        {
            List<FSImplantableDevicesModel> listImplantableDevices = new List<FSImplantableDevicesModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICE_SELECT);

                FSImplantableDevicesModel model = null;
                while (reader.Read())
                {
                    model = new FSImplantableDevicesModel();
                    model.GMDNPName = !String.IsNullOrEmpty(reader["DeviceName"].ToString()) ? reader["DeviceName"].ToString() : "";
                    model.DI = !String.IsNullOrEmpty(reader["DeviceId"].ToString()) ? reader["DeviceId"].ToString() : "";
                    model.Status = !String.IsNullOrEmpty(reader["Status"].ToString()) ? reader["Status"].ToString() : "";
                    model.Expiration_Date = !String.IsNullOrEmpty(reader["Expiry"].ToString()) ? reader["Expiry"].ToString() : "";
                    listImplantableDevices.Add(model);
                }

                return listImplantableDevices;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFaceSheet::loadFaceSheetImplantableDevices", PROC_IMPLANTABLE_DEVICE_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }


        #endregion
    }
}
