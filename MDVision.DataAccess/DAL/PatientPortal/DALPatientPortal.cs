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
using MDVision.Model.Dashboard;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.PatientPortal
{
    public class DALPatientPortal
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PATIENTLOGIN_INSERT = "Patient.sp_PatientLoginInsert";
        private const string PROC_PATIENTLOGIN_UPDATE = "Patient.sp_PatientLoginUpdate";
        private const string PROC_PATIENTLOGIN_DELETE = "Patient.sp_PatientLoginDelete";
        private const string PROC_PATIENTLOGIN_SELECT = "Patient.sp_PatientLoginSelect";
        private const string PROC_SECURITY_QUESTION_LOOKUP = "System.sp_SecurityQuestionLookup";

        private const string PROC_PATIENTREPRESENTATIVE_INSERT = "Patient.sp_PatientRepresentativeInsert";
        private const string PROC_PATIENTREPRESENTATIVE_UPDATE = "Patient.sp_PatientRepresentativeUpdate";
        private const string PROC_PATIENTREPRESENTATIVE_DELETE = "Patient.sp_PatientRepresentativeDelete";
        private const string PROC_PATIENTREPRESENTATIVE_SELECT = "Patient.sp_PatientRepresentativeSelect";

        private const string PROC_PatientAccountRequests_Select = "Patient.sp_GetPatientAccountRequests";
        private const string PROC_PatientPortalAccountRequests_Delete = "Patient.PatientPortalAccountRequestsDelete";
        private const string PROC_ApproveBulkPatientLogin = "Patient.sp_ApproveBulkPatientLogin";
        
        #endregion

        #region "Parameters"
        private const string PARM_PATIENTLOGIN_ID = "@PatientLoginId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_PASSWORD = "@Password";
        private const string PARM_ISHEALTHRECORDPRIVILEGE = "@IsHealthRecordPrivilege";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SECURITY_QUESTION = "@SecurityQuestionId";
        private const string PARM_SECURITY_ANSWER = "@SecurityAnswer";
        private const string PARM_UNLOCK_ACCOUNT = "@UnLockAccount";
        private const string PARM_DISABLE_ACCOUNT = "@DisableAccount";
        private const string PARM_IS_FIRST_LOGIN = "@IsFirstLogin";
        private const string PARM_EMAIL = "@Email";
        private const string PARM_PATREPRESENTATIVE_ID = "@PatRepresentativeId";
        private const string PARM_REPRESENTATIVE_ID = "@RepresentativeId";
        private const string PARM_RELATION_ID = "@RelationId";
        private const string PARM_STATUS = "@RequestStatus";
        private const string PARM_PatientIds = "@PatientIds";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_IsAssessmentAndPlanTreatment = "@IsAssessmentAndPlanTreatment";
        private const string PARM_IsCareTeamMembers = "@IsCareTeamMembers";
        private const string PARM_IsDemographics = "@IsDemographics";
        private const string PARM_IsGoals = "@IsGoals";
        private const string PARM_IsHealthConcerns = "@IsHealthConcerns";
        private const string PARM_IsHistory = "@IsHistory";
        private const string PARM_IsImmunizations = "@IsImmunizations";
        private const string PARM_IsImplantableDevices = "@IsImplantableDevices";
        private const string PARM_IsLaboratoryResults = "@IsLaboratoryResults";
        private const string PARM_IsLaboratoryTest = "@IsLaboratoryTest";
        private const string PARM_IsMedications = "@IsMedications";
        private const string PARM_IsMedicationsAllergies = "@IsMedicationAllergies";
        private const string PARM_IsProblems = "@IsProblems";
        private const string PARM_IsProcedures = "@IsProcedures";
        private const string PARM_IsVitalSigns = "@IsVitalsSigns";



        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientLogin"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientPortal()
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
        private void CreateParameters(IDBManager dbManager, DSPatientPortal ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(15);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PATIENTLOGIN_ID, ds.PatientLogin.PatientLoginIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENTLOGIN_ID, ds.PatientLogin.PatientLoginIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientLogin.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_USER_NAME, ds.PatientLogin.UserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PASSWORD, ds.PatientLogin.PasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PatientLogin.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.PatientLogin.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.PatientLogin.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.PatientLogin.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.PatientLogin.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_SECURITY_QUESTION, ds.PatientLogin.SecurityQuestionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SECURITY_ANSWER, ds.PatientLogin.SecurityAnswerColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_UNLOCK_ACCOUNT, ds.PatientLogin.UnLockAccountColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_DISABLE_ACCOUNT, ds.PatientLogin.DisableAccountColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_IS_FIRST_LOGIN, ds.PatientLogin.IsFirstLoginColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(14, PARM_EMAIL, ds.PatientLogin.EmailColumn.ColumnName, DbType.String);

        }


        private void CreatePatientRepresentativeParameters(IDBManager dbManager, DSPatientPortal ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PATREPRESENTATIVE_ID, ds.PatientRepresentative.PatRepresentativeIdColumn.ColumnName, DbType.Int64);
            else
                dbManager.AddParameters(0, PARM_PATREPRESENTATIVE_ID, ds.PatientRepresentative.PatRepresentativeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_REPRESENTATIVE_ID, ds.PatientRepresentative.RepresentativeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientRepresentative.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.PatientRepresentative.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.PatientRepresentative.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.PatientRepresentative.CreatedOnColumn.ColumnName, DbType.Date);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.PatientRepresentative.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.PatientRepresentative.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_RELATION_ID, ds.PatientRepresentative.RelationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_PASSWORD, ds.PatientRepresentative.RepresentativePasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ISHEALTHRECORDPRIVILEGE, ds.PatientRepresentative.IsHealthRecordPrivilegeColumn.ColumnName, DbType.Boolean);

        }


        #endregion

        #region "Insert, delete, update and get using dataset Functions"


        #region Patient Login

        /// <summary>
        /// Updates the appointment status.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientPortal.</returns>
        public DSPatientPortal UpdatePatientLogin(DSPatientPortal ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatientPortal)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENTLOGIN_UPDATE, ds, ds.PatientLogin.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::UpdatePatientLogin", PROC_PATIENTLOGIN_UPDATE, ex);
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

        public DSPatientPortal InsertPatientLogin(DSPatientPortal ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatientPortal)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENTLOGIN_INSERT, ds, ds.PatientLogin.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::InsertPatientLogin", PROC_PATIENTLOGIN_INSERT, ex);
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

        public DSPatientPortal LoadPatientLogin(long PatientLoginId, string UserName, long PatientId)
        {
            DSPatientPortal ds = new DSPatientPortal();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (UserName == "")
                    UserName = null;

                dbManager.Open();
                dbManager.CreateParameters(3);


                if (PatientLoginId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENTLOGIN_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTLOGIN_ID, PatientLoginId);
                dbManager.AddParameters(1, PARM_USER_NAME, UserName);
                if (PatientId <= 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                ds = (DSPatientPortal)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENTLOGIN_SELECT, ds, ds.PatientLogin.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::LoadPatientLogin", PROC_PATIENTLOGIN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<PatientPortalSignupModel> LoadPatientPortalSignupReq(string Status, string ProviderId, string PageNumber, string RowsPerPage)
        {
            List<PatientPortalSignupModel> listModel = new List<PatientPortalSignupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                if (PageNumber != "" && RowsPerPage != "")
                {

                    dbManager.Open();
                    dbManager.CreateParameters(5);
                    dbManager.AddParameters(0, PARM_STATUS, Status);
                    if(ProviderId !="")
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);
                    else dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, RowsPerPage);
                    dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                }
                else
                {
                    dbManager.Open();
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_STATUS, Status);
                    if (ProviderId != "")
                        dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);
                    else dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                    dbManager.AddParameters(2, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                }

               
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PatientAccountRequests_Select);
                PatientPortalSignupModel model = null;
                while (reader.Read())
                {

                    model = new PatientPortalSignupModel();
                    model.PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? "" : Convert.ToString(reader["PatientId"]);
                    model.PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? "" : Convert.ToString(reader["PatientName"]);
                    model.AccountNumber = reader.IsDBNull(reader.GetOrdinal("AccountNumber")) ? "" : Convert.ToString(reader["AccountNumber"]);
                    model.EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? "" : Convert.ToString(reader["EmailAddress"]);
                    model.StatusId = reader.IsDBNull(reader.GetOrdinal("StatusId")) ? "" : Convert.ToString(reader["StatusId"]);
                    model.StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? "" : Convert.ToString(reader["StatusName"]);
                    model.UpdatedById = reader.IsDBNull(reader.GetOrdinal("UpdatedById")) ? "" : Convert.ToString(reader["UpdatedById"]);
                    model.UpdatedByName = reader.IsDBNull(reader.GetOrdinal("UpdatedByName")) ? "" : Convert.ToString(reader["UpdatedByName"]);
                    model.RequestDateTime = reader.IsDBNull(reader.GetOrdinal("RequestDateTime")) ? "" : Convert.ToString(reader["RequestDateTime"]);
                    model.UpdateDateTime = reader.IsDBNull(reader.GetOrdinal("UpdateDateTime")) ? "" : Convert.ToString(reader["UpdateDateTime"]);
                    model.providerId = reader.IsDBNull(reader.GetOrdinal("providerId")) ? "" : Convert.ToString(reader["providerId"]);
                    model.ProviderName = reader.IsDBNull(reader.GetOrdinal("ProviderName")) ? "" : Convert.ToString(reader["ProviderName"]);
                    model.SercurityQuestionId = reader.IsDBNull(reader.GetOrdinal("SercurityQuestionId")) ? "" : Convert.ToString(reader["SercurityQuestionId"]);
                    model.Answer = reader.IsDBNull(reader.GetOrdinal("Answer")) ? "" : Convert.ToString(reader["Answer"]);
                    model.RecordCount = reader.IsDBNull(reader.GetOrdinal("RecordCount")) ? "" : Convert.ToString(reader["RecordCount"]);
                    //   model.Status = Convert.ToString(reader["Status"]);
                    //    model.DateOfAppointment = model.Status == "Signed" ? Convert.ToString(reader["AppointmentDate"]) : "Does not apply";
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::LoadPatientPortalSignupReq", PROC_PatientAccountRequests_Select, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeletePatientPortalSignupReq(DataTable ids)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
            
                    dbManager.Open();
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_PatientIds, ids);
                dbManager.AddParameters(1, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(2, PARM_MODIFIED_ON, DateTime.Now.ToString());
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PatientPortalAccountRequests_Delete);
                reader.Read();
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::DeletePatientPortalSignupReq", PROC_PatientPortalAccountRequests_Delete, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public IList<PatientPortalSignupModel> ApprovePatientPortalSignupReq(DataTable ids, DashBoardModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(20);
                dbManager.AddParameters(0, PARM_PatientIds, ids);
                dbManager.AddParameters(1, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(2, PARM_MODIFIED_ON, DateTime.Now.ToString());
                dbManager.AddParameters(3, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(4, PARM_CREATED_ON, DateTime.Now.ToString());
                dbManager.AddParameters(5, PARM_IsAssessmentAndPlanTreatment, model.IsAssessmentAndPlanTreatment);
                dbManager.AddParameters(6, PARM_IsDemographics, model.IsDemographics);
                dbManager.AddParameters(7, PARM_IsGoals, model.IsGoals);
                dbManager.AddParameters(8, PARM_IsHealthConcerns, model.IsHealthConcerns);
                dbManager.AddParameters(9, PARM_IsHistory, model.IsHistory);
                dbManager.AddParameters(10, PARM_IsImmunizations, model.IsImmunizations);
                dbManager.AddParameters(11, PARM_IsImplantableDevices, model.IsImplantableDevices);
                dbManager.AddParameters(12, PARM_IsLaboratoryResults, model.IsLaboratoryResults);
                dbManager.AddParameters(13, PARM_IsLaboratoryTest, model.IsLaboratoryTest);
                dbManager.AddParameters(14, PARM_IsMedications, model.IsMedications);
                dbManager.AddParameters(15, PARM_IsMedicationsAllergies, model.IsMedicationsAllergies);
                dbManager.AddParameters(16, PARM_IsProblems, model.IsProblems);
                dbManager.AddParameters(17, PARM_IsProcedures, model.IsProcedures);
                dbManager.AddParameters(18, PARM_IsVitalSigns, model.IsVitalSigns);
                dbManager.AddParameters(19, PARM_IsCareTeamMembers, model.IsCareTeamMembers);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ApproveBulkPatientLogin);
                IList< PatientPortalSignupModel> ListpatModel = new List< PatientPortalSignupModel>();
                while (reader.Read())
                {
                    var patModel = new PatientPortalSignupModel();
                    patModel.FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? "" : Convert.ToString(reader["FirstName"]);
                    patModel.lastName = reader.IsDBNull(reader.GetOrdinal("lastName")) ? "" : Convert.ToString(reader["lastName"]);
                    patModel.EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? "" : Convert.ToString(reader["EmailAddress"]);
                    patModel.UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? "" : Convert.ToString(reader["UserName"]);
                    patModel.Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? "" : Convert.ToString(reader["Password"]);
                    ListpatModel.Add(patModel);
                }
           
                return ListpatModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::ApprovePatientPortalSignupReq", PROC_ApproveBulkPatientLogin, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        

        #endregion

        #region Patient Representative
        public DSPatientPortal InsertPatientRepresentative(DSPatientPortal ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePatientRepresentativeParameters(dbManager, ds, true);
                ds = (DSPatientPortal)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENTREPRESENTATIVE_INSERT, ds, ds.PatientRepresentative.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::InsertPatientRepresentative", PROC_PATIENTREPRESENTATIVE_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else if (ex.Message.Contains("UQ_PatientRepresentative_PatientId_RepresentativeId"))
                    throw new Exception("Same representative already exists");
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientPortal UpdatePatientRepresentative(DSPatientPortal ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePatientRepresentativeParameters(dbManager, ds, false);
                ds = (DSPatientPortal)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENTREPRESENTATIVE_UPDATE, ds, ds.PatientRepresentative.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::UpdatePatientRepresentative", PROC_PATIENTREPRESENTATIVE_UPDATE, ex);
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

        public DSPatientPortal LoadPatientRepresentative(long PatRepresentativeId, long PatientId)
        {
            DSPatientPortal ds = new DSPatientPortal();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatRepresentativeId == 0)
                    dbManager.AddParameters(0, PARM_PATREPRESENTATIVE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATREPRESENTATIVE_ID, PatRepresentativeId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);


                ds = (DSPatientPortal)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENTREPRESENTATIVE_SELECT, ds, ds.PatientRepresentative.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::LoadPatientRepresentative", PROC_PATIENTREPRESENTATIVE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string DeletePatRepresentative(string PatRepresentativeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATREPRESENTATIVE_ID, PatRepresentativeId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENTREPRESENTATIVE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::DeletePatRepresentative", PROC_PATIENTREPRESENTATIVE_DELETE, ex);
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


        #endregion

        #region  " Lookups "

        public DSPatientPortal LookupSecurityQuestions()
        {
            DSPatientPortal ds = new DSPatientPortal();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientPortal)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_QUESTION_LOOKUP, ds, ds.SecurityQuestionLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientPortal::LookupSecurityQuestion", PROC_SECURITY_QUESTION_LOOKUP, ex);
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
