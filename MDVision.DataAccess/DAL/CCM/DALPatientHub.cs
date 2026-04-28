using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using System.Data.SqlClient;
using MDVision.Model.CCM.CCMHub;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.CCM
{
    public class DALPatientHub
    {

        #region Constructors
        public DALPatientHub()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        //-----------------------------------------------------------------------------------------------------
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new Container();
        }

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CCM_ENROLLEDGOALS_INSERT = "CCM.sp_EnrolledGoalsInsert";
        private const string PROC_CCM_ENROLLEDGOALSCPT_INSERT = "CCM.sp_EnrolledGoalsCPTInsert";
        private const string PROC_CCM_PATIENTHUBGOALSSELECT = "CCM.sp_PatientHubGoalsSelect";


        private const string PROC_CCM_PATIENTHUBSELECT = "CCM.sp_PatientHubSelect";

        private const string PROC_CCM_PATIENTHUB_PROBLEMS_SELECT = "CCM.sp_PatientHubProblemsSelect";
        private const string PROC_CCM_PATIENTHUB_PROBLEMS_DELETE = "CCM.sp_PatientHubProblemDelete";

        private const string PROC_CCM_ENROLLEDRISKASSESSMENTTEMPSELECT = "CCM.sp_EnrolledRiskAssessmentTempSelect";
        private const string PROC_CCM_PH_ENROLLEDRISKASSESSMENTSELECT = "CCM.sp_PHEnrolledRiskAssessmentSelect";
        private const string PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERTUPDATE = "CCM.sp_RiskAssessmentInsertUpdate";
        private const string PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERT = "CCM.sp_CCMRiskAssessmentTemplateInsert";
        private const string PROC_CCM_ENROLLEDRISKASSESSMENTTEMPDELETE = "CCM.sp_PatientHubRiskAssessmentTemplateDelete";
        private const string PROC_CCM_ENROLLEDCARETEAMINSERT = "CCM.sp_EnrolledCareTeamInsert";

        private const string PROC_CCM_PATIENTHUBCARETEAMSELECT = "CCM.sp_CareTeamProviderSelect";
        private const string PROC_CCM_PATIENTHUBCARETEAMDELETE = "CCM.sp_CareTeamProviderDelete";

        private const string PROC_CCM_PATIENTHUBGOALSDELETE = "CCM.sp_PatientHubGoalsDelete";

        #endregion

        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ENROLLED_GOALS_ID = "@EnrolledGoalsId";
        private const string PARM_ENROLLMENT_INFO_ID = "@EnrollmentInfoId";
        private const string PARM_CARETEAM_ID = "@CareTeamId";
        private const string PARM_ENDSELECTIONBY = "@EndSelectionBy";
        private const string PARM_ENDSELECTIONDATE = "@EndSelectionDate";

        private const string EnrolledRiskAssesId = "@EnrolledRiskAssesId";
        private const string PARM_TEMPLATE_ID = "@TemplateId";
        private const string PARM_RISKASSESSMENTTEMP_ID = "@RiskAssessTemptId";
        private const string PARM_RISKASSESSMENT_ID = "@RiskAssessmentId";
        private const string PARM_RISKASSESSMENT_ID_ = "@RiskAssessmentId";
        private const string PARM_ENROLLEDRISKASSESSMENTTEMP_ID = "@EnrolledRiskAssessmentTempId";
        private const string PARM_ASSESSHTML = "@AssessHTML";
        private const string PARM_RISKSCORE = "@RiskScore";

        private const string PARM_ENROLLEDGOALS_CPT_ID = "@EnrolledGoalsICDId";
        private const string PARM_ENROLLEDCARETEAM_ID = "@EnrolledCareTeamId";
        private const string PARM_CHRONICPROBLEM_ID = "@ICDCodeId";
        private const string PARM_CPTCodeId_ID = "@CPTCodeId";

        private const string PARM_INSTRUCTION = "@Instruction";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_MODFIED_ON = "@ModfiedOn";

        #endregion

        #region Goals

        private void createParametersCCMEnrolledGoals(IDBManager dbManager, EnrolledGoals model, Boolean IsInsert)
        {
            dbManager.CreateParameters(6);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_ENROLLED_GOALS_ID, MDVUtility.ToStr(model.EnrolledGoalsId), DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_ENROLLED_GOALS_ID, MDVUtility.ToStr(model.EnrolledGoalsId), DbType.Int64);

            dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));

            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
        }
        private void createParametersCCMEnrolledGoalsCPT(IDBManager dbManager, EnrolledGoalsCPT model, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_ENROLLEDGOALS_CPT_ID, MDVUtility.ToStr(model.EnrolledGoalsICDId), DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_ENROLLEDGOALS_CPT_ID, MDVUtility.ToStr(model.EnrolledGoalsICDId), DbType.Int64);

            dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrolledGoalsId));
            dbManager.AddParameters(i++, PARM_CPTCodeId_ID, MDVUtility.ToInt32(model.CPTCodeId));
            dbManager.AddParameters(i++, PARM_INSTRUCTION, MDVUtility.ToInt32(model.Instruction));

        }
        private void createParametersCCMEnrolledGoals_CCMEnrolledGoalsCPT(IDBManager dbManager, EnrolledGoals_EnrolledGoalsCPT model, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_ENROLLEDGOALS_CPT_ID, MDVUtility.ToStr(model.EnrolledGoalsICDId), DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_ENROLLEDGOALS_CPT_ID, MDVUtility.ToStr(model.EnrolledGoalsICDId), DbType.Int64);

            dbManager.AddParameters(i++, PARM_ENROLLED_GOALS_ID, MDVUtility.ToInt32(model.EnrolledGoalsId));
            dbManager.AddParameters(i++, PARM_CPTCodeId_ID, MDVUtility.ToInt32(model.CPTCodeId));
            dbManager.AddParameters(i++, PARM_INSTRUCTION, MDVUtility.ToStr(model.Instruction));

            dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
        }
        private void createParametersCCMRiskAssessmentScore(IDBManager dbManager, RiskAssessment model, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_RISKASSESSMENT_ID_, MDVUtility.ToStr(model.RiskAssessmentId), DbType.Int64, ParamDirection.Output);
            else

                dbManager.AddParameters(i++, PARM_RISKASSESSMENT_ID_, MDVUtility.ToInt32(model.RiskAssessmentId));

            dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(i++, PARM_RISKASSESSMENTTEMP_ID, MDVUtility.ToInt32(model.RiskAssessTemptId));

            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_RISKSCORE, MDVUtility.Tofloat(model.RiskScore));
        }

        private void createParametersCCMRiskAssessmentScoreUpdate(IDBManager dbManager, RiskAssessment model, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, EnrolledRiskAssesId, MDVUtility.ToStr(model.EnrolledRiskAssesId), DbType.Int64, ParamDirection.Output);
            else

                dbManager.AddParameters(i++, EnrolledRiskAssesId, MDVUtility.ToInt32(model.EnrolledRiskAssesId));

            dbManager.AddParameters(i++, PARM_RISKASSESSMENT_ID_, MDVUtility.ToInt32(model.RiskAssessmentId));
            dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(i++, PARM_RISKASSESSMENTTEMP_ID, MDVUtility.ToInt32(model.RiskAssessTemptId));

            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_RISKSCORE, MDVUtility.Tofloat(model.RiskScore));
        }

        private void createParametersCCMCareTeam(IDBManager dbManager, EnrolledCareTeam model, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_ENROLLEDCARETEAM_ID, MDVUtility.ToStr(model.EnrolledCareTeamId), DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_ENROLLEDCARETEAM_ID, MDVUtility.ToStr(model.EnrolledCareTeamId), DbType.Int64);

            dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, MDVUtility.ToLong(model.EnrollmentInfoId));
            dbManager.AddParameters(i++, PARM_CARETEAM_ID, MDVUtility.ToLong(model.CareTeamId));

            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

            //dbManager.AddParameters(i++, PARM_ENDSELECTIONDATE, DateTime.Now);
            //dbManager.AddParameters(i++, PARM_ENDSELECTIONBY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
        }

        /// <summary>
        /// Only Goals
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveCCMEnrolledGoals(EnrolledGoals model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                string returnValue = "";
                dbManager.Open();
                createParametersCCMEnrolledGoals(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLEDGOALS_INSERT);
                while (reader.Read())
                {
                    returnValue = reader["PatientId"].ToString();
                }
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::SaveCCMEnrolledGoals", PROC_CCM_ENROLLEDGOALS_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Goals Map
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveCCMEnrolledGoalsCPT(EnrolledGoalsCPT model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnValue = "";
                dbManager.Open();
                createParametersCCMEnrolledGoalsCPT(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLEDGOALSCPT_INSERT);
                while (reader.Read())
                {
                    returnValue = reader["PatientId"].ToString();
                }
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::SaveCCMEnrolledGoalsCPT", PROC_CCM_ENROLLEDGOALSCPT_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// 2 in 1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT(EnrolledGoals_EnrolledGoalsCPT model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                long returnValue = -1;
                dbManager.Open();
                createParametersCCMEnrolledGoals_CCMEnrolledGoalsCPT(dbManager, model, true);
                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLEDGOALSCPT_INSERT);

                object EnrolledGoalsICDId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLEDGOALSCPT_INSERT);
                if (EnrolledGoalsICDId != null)
                {
                    returnValue = MDVUtility.ToInt64(EnrolledGoalsICDId.ToString());
                }

                //while (reader.Read())
                //{
                //    returnValue = reader["EnrolledGoalsICDId"].ToString();
                //}
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::SaveCCMEnrolledGoalsCPT", PROC_CCM_ENROLLEDGOALSCPT_INSERT, ex);

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

        public List<PatientHubStatic> LoadCCMPatientHUBStatic(long PatientId, long EnrollmentInfoId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            List<PatientHubStatic> CCMPatientHUBList = new List<PatientHubStatic>();

            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(1, PARM_ENROLLMENT_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_PATIENTHUBSELECT);

                PatientHubStatic model = null;
                while (reader.Read())
                {
                    model = new PatientHubStatic();

                    //model.PatientImage = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? reader["PatientImage"].ToString() : "";

                    string imageBase64 = "";
                    byte[] imageByteArr = reader["PatientImage"] as byte[];
                    if (imageByteArr != null)
                    {
                        imageBase64 = "data:" + reader["PatientImage"] + ";base64," + Convert.ToBase64String(reader["PatientImage"] as byte[]);
                    }

                    if (!String.IsNullOrEmpty(imageBase64))
                        model.PatientImage = imageBase64;
                    else
                        model.PatientImage = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? reader["PatientImage"].ToString() : "";


                    model.PatientName = !String.IsNullOrEmpty(reader["PatientName"].ToString()) ? reader["PatientName"].ToString() : "";
                    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.Gender = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    model.Age = !String.IsNullOrEmpty(reader["Age"].ToString()) ? reader["Age"].ToString() : "";
                    model.DateOfBirth = !String.IsNullOrEmpty(reader["DateOfBirth"].ToString()) ? reader["DateOfBirth"].ToString() : "";
                    model.EmergencyContact = !String.IsNullOrEmpty(reader["EmergencyContact"].ToString()) ? reader["EmergencyContact"].ToString() : "";
                    model.HomePhone = !String.IsNullOrEmpty(reader["HomePhone"].ToString()) ? reader["HomePhone"].ToString() : "";
                    model.Relation = !String.IsNullOrEmpty(reader["Relation"].ToString()) ? reader["Relation"].ToString() : "";
                    model.Address1 = !String.IsNullOrEmpty(reader["Address1"].ToString()) ? reader["Address1"].ToString() : "";
                    model.City = !String.IsNullOrEmpty(reader["City"].ToString()) ? reader["City"].ToString() : "";
                    model.State = !String.IsNullOrEmpty(reader["State"].ToString()) ? reader["State"].ToString() : "";
                    model.ZIPCode = !String.IsNullOrEmpty(reader["ZIPCode"].ToString()) ? reader["ZIPCode"].ToString() : "";
                    model.Patientphone = !String.IsNullOrEmpty(reader["Patientphone"].ToString()) ? reader["Patientphone"].ToString() : "";
                    model.NextAppointment = !String.IsNullOrEmpty(reader["NextAppointment"].ToString()) ? reader["NextAppointment"].ToString() : "";
                    model.LastAppointment = !String.IsNullOrEmpty(reader["LastAppointment"].ToString()) ? reader["LastAppointment"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.StatusId = !String.IsNullOrEmpty(reader["StatusId"].ToString()) ? reader["StatusId"].ToString() : "";
                    model.CCMStatus = !String.IsNullOrEmpty(reader["CCMStatus"].ToString()) ? reader["CCMStatus"].ToString() : "";
                    model.Reason = !String.IsNullOrEmpty(reader["Reason"].ToString()) ? reader["Reason"].ToString() : "";
                    model.EnrollmentDate = !String.IsNullOrEmpty(reader["EnrollmentDate"].ToString()) ? reader["EnrollmentDate"].ToString() : "";
                    model.Program = !String.IsNullOrEmpty(reader["Program"].ToString()) ? reader["Program"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";

                    CCMPatientHUBList.Add(model);
                }


                return CCMPatientHUBList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUB", PROC_CCM_PATIENTHUBSELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<PatientHubStatic> LoadCCMPatientHUBStatic1(long PatientId, long EnrollmentInfoId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            //   List<PatientHubStatic> CCMPatientHUBList = new List<PatientHubStatic>();

            //SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                //   List<SqlParameter> parameters = new List<SqlParameter>();
                //dbManager.Open();
                //dbManager.CreateParameters(2);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                //   parameters.Add(new SqlParameter(PARM_PATIENT_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                //  parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, null);
                // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                //  parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId));


                return dbManager.ExecuteReaders<PatientHubStatic>(PROC_CCM_PATIENTHUBSELECT);

                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_PATIENTHUBSELECT);

                //PatientHubStatic model = null;
                //while (reader.Read())
                //{
                //    model = new PatientHubStatic();

                //    //model.PatientImage = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? reader["PatientImage"].ToString() : "";

                //    string imageBase64 = "";
                //    byte[] imageByteArr = reader["PatientImage"] as byte[];
                //    if (imageByteArr != null)
                //    {
                //        imageBase64 = "data:" + reader["PatientImage"] + ";base64," + Convert.ToBase64String(reader["PatientImage"] as byte[]);
                //    }

                //    if (!String.IsNullOrEmpty(imageBase64))
                //        model.PatientImage = imageBase64;
                //    else
                //        model.PatientImage = !String.IsNullOrEmpty(reader["PatientImage"].ToString()) ? reader["PatientImage"].ToString() : "";


                //    model.PatientName = !String.IsNullOrEmpty(reader["PatientName"].ToString()) ? reader["PatientName"].ToString() : "";
                //    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                //    model.Gender = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                //    model.Age = !String.IsNullOrEmpty(reader["Age"].ToString()) ? reader["Age"].ToString() : "";
                //    model.DateOfBirth = !String.IsNullOrEmpty(reader["DateOfBirth"].ToString()) ? reader["DateOfBirth"].ToString() : "";
                //    model.EmergencyContact = !String.IsNullOrEmpty(reader["EmergencyContact"].ToString()) ? reader["EmergencyContact"].ToString() : "";
                //    model.HomePhone = !String.IsNullOrEmpty(reader["HomePhone"].ToString()) ? reader["HomePhone"].ToString() : "";
                //    model.Relation = !String.IsNullOrEmpty(reader["Relation"].ToString()) ? reader["Relation"].ToString() : "";
                //    model.Address1 = !String.IsNullOrEmpty(reader["Address1"].ToString()) ? reader["Address1"].ToString() : "";
                //    model.City = !String.IsNullOrEmpty(reader["City"].ToString()) ? reader["City"].ToString() : "";
                //    model.State = !String.IsNullOrEmpty(reader["State"].ToString()) ? reader["State"].ToString() : "";
                //    model.ZIPCode = !String.IsNullOrEmpty(reader["ZIPCode"].ToString()) ? reader["ZIPCode"].ToString() : "";
                //    model.Patientphone = !String.IsNullOrEmpty(reader["Patientphone"].ToString()) ? reader["Patientphone"].ToString() : "";
                //    model.NextAppointment = !String.IsNullOrEmpty(reader["NextAppointment"].ToString()) ? reader["NextAppointment"].ToString() : "";
                //    model.LastAppointment = !String.IsNullOrEmpty(reader["LastAppointment"].ToString()) ? reader["LastAppointment"].ToString() : "";
                //    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                //    model.StatusId = !String.IsNullOrEmpty(reader["StatusId"].ToString()) ? reader["StatusId"].ToString() : "";
                //    model.CCMStatus = !String.IsNullOrEmpty(reader["CCMStatus"].ToString()) ? reader["CCMStatus"].ToString() : "";
                //    model.Reason = !String.IsNullOrEmpty(reader["Reason"].ToString()) ? reader["Reason"].ToString() : "";
                //    model.EnrollmentDate = !String.IsNullOrEmpty(reader["EnrollmentDate"].ToString()) ? reader["EnrollmentDate"].ToString() : "";
                //    model.Program = !String.IsNullOrEmpty(reader["Program"].ToString()) ? reader["Program"].ToString() : "";
                //    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";

                //    CCMPatientHUBList.Add(model);
                //}


                //return CCMPatientHUBList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUB", PROC_CCM_PATIENTHUBSELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }
        public List<PatientHubProblems> LoadCCMPatientHUBProblems(long PatientId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            //List<PatientHubProblems> CCMPatientHUBProblemsList = new List<PatientHubProblems>();

            //SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //  List<SqlParameter> parameters = new List<SqlParameter>();
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                //  parameters.Add(new SqlParameter(PARM_PATIENT_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                // parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                return dbManager.ExecuteReaders<PatientHubProblems>(PROC_CCM_PATIENTHUB_PROBLEMS_SELECT);

                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_PATIENTHUB_PROBLEMS_SELECT);

                //PatientHubProblems model = null;
                //while (reader.Read())
                //{
                //    model = new PatientHubProblems();
                //    model.Id = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                //    model.ICD10 = !String.IsNullOrEmpty(reader["ICD10"].ToString()) ? reader["ICD10"].ToString() : "";
                //    model.ICD10_Description = !String.IsNullOrEmpty(reader["ICD10_Description"].ToString()) ? reader["ICD10_Description"].ToString() : "";
                //    CCMPatientHUBProblemsList.Add(model);
                //}
                //return CCMPatientHUBProblemsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUBProblems", PROC_CCM_PATIENTHUB_PROBLEMS_SELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }
        //public List<EnrolledRiskAssessmentTemp> LoadCCMPatientHUBRiskAssessmentScore(long EnrollmentInfoId)
        //{
        //    List<EnrolledRiskAssessmentTemp> CCMEnrolledRiskAssessmentTempList = new List<EnrolledRiskAssessmentTemp>();

        //    SqlDataReader reader = null;
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
        //        if (EnrollmentInfoId == 0)
        //            dbManager.AddParameters(0, PARM_ENROLLMENT_INFO_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);

        //        reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLEDRISKASSESSMENTTEMPSELECT);

        //        EnrolledRiskAssessmentTemp model = null;
        //        while (reader.Read())
        //        {
        //            model = new EnrolledRiskAssessmentTemp();
        //            model.EnrolledRiskAssessmentTempId = !String.IsNullOrEmpty(reader["EnrolledRiskAssessmentTempId"].ToString()) ? reader["EnrolledRiskAssessmentTempId"].ToString() : "";
        //            model.EnrollmentInfoId = !String.IsNullOrEmpty(reader["EnrollmentInfoId"].ToString()) ? reader["EnrollmentInfoId"].ToString() : "";
        //            model.TemplateId = !String.IsNullOrEmpty(reader["TemplateId"].ToString()) ? reader["TemplateId"].ToString() : "";
        //            model.AssessHTML = !String.IsNullOrEmpty(reader["AssessHTML"].ToString()) ? reader["AssessHTML"].ToString() : "";
        //            model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
        //            model.CreatedOn = MDVUtility.CheckDateTimeNull(reader["CreatedOn"]);
        //            model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
        //            model.ModifiedOn = MDVUtility.CheckDateTimeNull(reader["ModfiedOn"]);
        //            CCMEnrolledRiskAssessmentTempList.Add(model);
        //        }
        //        return CCMEnrolledRiskAssessmentTempList;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUBRiskAssessmentScore", PROC_CCM_ENROLLEDRISKASSESSMENTTEMPSELECT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //            reader.Close();
        //        dbManager.Dispose();
        //    }
        //}

        public List<EnrolledRiskAssessment> LoadCCMPatientHUBRiskAssessmentScore(long EnrollmentInfoId)
        {
            //List<EnrolledRiskAssessment> CCMEnrolledRiskAssessmentTempList = new List<EnrolledRiskAssessment>();

            //SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // List<SqlParameter> parameters = new List<SqlParameter>();
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, null);
                // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId));


                return dbManager.ExecuteReaders<EnrolledRiskAssessment>(PROC_CCM_PH_ENROLLEDRISKASSESSMENTSELECT);


                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_PH_ENROLLEDRISKASSESSMENTSELECT);

                //EnrolledRiskAssessment model = null;
                //while (reader.Read())
                //{
                //    model = new EnrolledRiskAssessment();
                //    model.EnrollmentInfoId = !String.IsNullOrEmpty(reader["EnrollmentInfoId"].ToString()) ? reader["EnrollmentInfoId"].ToString() : "";
                //    model.RiskAssessmentId = !String.IsNullOrEmpty(reader["RiskAssessmentId"].ToString()) ? reader["RiskAssessmentId"].ToString() : "";
                //    model.TemplateId = !String.IsNullOrEmpty(reader["TemplateId"].ToString()) ? reader["TemplateId"].ToString() : "";
                //    model.TemplateDescription = !String.IsNullOrEmpty(reader["TemplateDescription"].ToString()) ? reader["TemplateDescription"].ToString() : "";
                //    model.RiskScore = !String.IsNullOrEmpty(reader["RiskScore"].ToString()) ? reader["RiskScore"].ToString() : "";
                //    CCMEnrolledRiskAssessmentTempList.Add(model);
                //}
                //return CCMEnrolledRiskAssessmentTempList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUBRiskAssessmentScore", PROC_CCM_PH_ENROLLEDRISKASSESSMENTSELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }
        public List<ProviderCareTeam> LoadCCMPatientHUBCareTeam(long ProviderId, long EnrollmentInfoId, long CareTeamId)
        {
            //   List<ProviderCareTeam> CCMPatientHUBList = new List<ProviderCareTeam>();

            // SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //  List<SqlParameter> parameters = new List<SqlParameter>();
                //dbManager.Open();
                //dbManager.CreateParameters(2);
                //if (ProviderId == 0)
                //    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                //else
                if (ProviderId <= 0)
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                    // parameters.Add(new SqlParameter(PARM_PROVIDER_ID, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                    // parameters.Add(new SqlParameter(PARM_PROVIDER_ID, ProviderId));
                }
                if (EnrollmentInfoId <= 0)
                {
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, DBNull.Value);
                    // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                    //parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId));
                }
                if (CareTeamId <= 0)
                {
                    dbManager.AddParameters(PARM_CARETEAM_ID, DBNull.Value);                    
                }
                else
                {
                    dbManager.AddParameters(PARM_CARETEAM_ID, CareTeamId);                    
                }
                return dbManager.ExecuteReaders<ProviderCareTeam>(PROC_CCM_PATIENTHUBCARETEAMSELECT);

                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_PATIENTHUBCARETEAMSELECT);

                //ProviderCareTeam model = null;
                //while (reader.Read())
                //{
                //    model = new ProviderCareTeam();
                //    model.CareTeamId = !String.IsNullOrEmpty(reader["CareTeamId"].ToString()) ? reader["CareTeamId"].ToString() : "";
                //    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                //    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                //    model.PCPName = !String.IsNullOrEmpty(reader["PCPName"].ToString()) ? reader["PCPName"].ToString() : "";
                //    model.CareManagerName = !String.IsNullOrEmpty(reader["CareManagerName"].ToString()) ? reader["CareManagerName"].ToString() : "";
                //    model.CareCoordinatorName = !String.IsNullOrEmpty(reader["CareCoordinatorName"].ToString()) ? reader["CareCoordinatorName"].ToString() : "";
                //    model.CareGiverName = !String.IsNullOrEmpty(reader["CareGiverName"].ToString()) ? reader["CareGiverName"].ToString() : "";
                //    model.LastUpdated = !String.IsNullOrEmpty(reader["LastUpdated"].ToString()) ? reader["LastUpdated"].ToString() : "";
                //    model.Specialty = !String.IsNullOrEmpty(reader["Specialty"].ToString()) ? reader["Specialty"].ToString() : "";
                //    model.ProviderPhone = !String.IsNullOrEmpty(reader["ProviderPhone"].ToString()) ? reader["ProviderPhone"].ToString() : "";
                //    model.CareManagerPhone = !String.IsNullOrEmpty(reader["CareManagerPhone"].ToString()) ? reader["CareManagerPhone"].ToString() : "";
                //    model.CareCoordinatorPhone = !String.IsNullOrEmpty(reader["CareCoordinatorPhone"].ToString()) ? reader["CareCoordinatorPhone"].ToString() : "";
                //    model.CareGiverPhone = !String.IsNullOrEmpty(reader["CareGiverPhone"].ToString()) ? reader["CareGiverPhone"].ToString() : "";
                //    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";

                //    CCMPatientHUBList.Add(model);
                //}
                //return CCMPatientHUBList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUBCareTeam", PROC_CCM_PATIENTHUBCARETEAMSELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }

        public List<PatientHubEnrolledGoalsCPT> LoadCCMPatientHUBGoals(long EnrollmentInfoId)
        {
            //List<PatientHubEnrolledGoalsCPT> CCMPatientHUBList = new List<PatientHubEnrolledGoalsCPT>();

            //SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //  List<SqlParameter> parameters = new List<SqlParameter>();
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                //  parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId));
                dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);


                return dbManager.ExecuteReaders<PatientHubEnrolledGoalsCPT>(PROC_CCM_PATIENTHUBGOALSSELECT);
                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_PATIENTHUBGOALSSELECT);

                //PatientHubEnrolledGoalsCPT model = null;
                //while (reader.Read())
                //{
                //    model = new PatientHubEnrolledGoalsCPT();
                //    model.EnrolledGoalsICDId = !String.IsNullOrEmpty(reader["EnrolledGoalsICDId"].ToString()) ? reader["EnrolledGoalsICDId"].ToString() : "";
                //    model.EnrolledGoalsId = !String.IsNullOrEmpty(reader["EnrolledGoalsId"].ToString()) ? reader["EnrolledGoalsId"].ToString() : "";
                //    model.EnrollmentInfoId = !String.IsNullOrEmpty(reader["EnrollmentInfoId"].ToString()) ? reader["EnrollmentInfoId"].ToString() : "";
                //    model.CPTpkID = !String.IsNullOrEmpty(reader["CPTpkID"].ToString()) ? reader["CPTpkID"].ToString() : "";
                //    model.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                //    model.CPTDescription = !String.IsNullOrEmpty(reader["CPTDescription"].ToString()) ? reader["CPTDescription"].ToString() : "";
                //    model.SNOMEDCode = !String.IsNullOrEmpty(reader["SNOMEDCode"].ToString()) ? reader["SNOMEDCode"].ToString() : "";
                //    model.SNOMEDDescription = !String.IsNullOrEmpty(reader["SNOMEDDescription"].ToString()) ? reader["SNOMEDDescription"].ToString() : "";
                //    model.Instruction = !String.IsNullOrEmpty(reader["Instruction"].ToString()) ? reader["Instruction"].ToString() : "";
                //    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                //    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                //    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                //    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                //    CCMPatientHUBList.Add(model);
                //}
                //return CCMPatientHUBList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::LoadCCMPatientHUBGoals", PROC_CCM_PATIENTHUBGOALSSELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }

        public long InsertCCMPatientHUBEnrolledCareTeam(EnrolledCareTeam model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                long returnValue = -1;
                dbManager.Open();
                createParametersCCMCareTeam(dbManager, model, true);

                object EnrolledCareTeamId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLEDCARETEAMINSERT);
                if (EnrolledCareTeamId != null)
                {
                    returnValue = MDVUtility.ToInt64(EnrolledCareTeamId.ToString());
                }
                return MDVUtility.ToInt64(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::InsertCCMPatientHUBCareTeam", PROC_CCM_ENROLLEDCARETEAMINSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public long InsertCCMPatientHUBRiskAssessmentTemplate(RiskAssessment model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                long returnValue = -1;
                dbManager.Open();
                createParametersCCMRiskAssessmentScore(dbManager, model, true);

                object RiskAssessmentId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERT); // dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERTUPDATE);
                if (RiskAssessmentId != null)
                {
                    returnValue = MDVUtility.ToInt64(RiskAssessmentId.ToString());
                }
                return MDVUtility.ToInt64(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::InsertCCMPatientHUBRiskAssessmentTemplate", PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public long InsertUpdateCCMPatientHUBRiskAssessmentScore(RiskAssessment model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                long returnValue = -1;
                dbManager.Open();
                createParametersCCMRiskAssessmentScoreUpdate(dbManager, model, false);

                object RiskAssessmentId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERTUPDATE); // dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERTUPDATE);
                if (RiskAssessmentId != null)
                {
                    returnValue = MDVUtility.ToInt64(RiskAssessmentId.ToString());
                }
                return MDVUtility.ToInt64(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientHub::InsertUpdateCCMPatientHUBRiskAssessmentScore", PROC_CCM_ENROLLEDRISKASSESSMENTTEMPINSERTUPDATE, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public string DeleteRiskAssessmentScoreTemplate(long EnrollmentInfoId, long RiskAssessTemptId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CHRONICPROBLEM_ID, EnrollmentInfoId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, RiskAssessTemptId);

                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_PATIENTHUB_PROBLEMS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteChronicProblem", PROC_CCM_PATIENTHUB_PROBLEMS_DELETE, ex);
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

        public string DeleteChronicProblems(long ChronicProblemId, long patientID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CHRONICPROBLEM_ID, ChronicProblemId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientID);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_PATIENTHUB_PROBLEMS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteChronicProblem", PROC_CCM_PATIENTHUB_PROBLEMS_DELETE, ex);
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

        public string DeleteRiskAssessmentScoreTemplate(long RiskAssessmentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_RISKASSESSMENT_ID, RiskAssessmentId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLEDRISKASSESSMENTTEMPDELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteRiskAssessmentScoreTemplate", PROC_CCM_ENROLLEDRISKASSESSMENTTEMPDELETE, ex);
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

        public string DeleteCareTeamProviderTemplate(long ProviderId, long EnrollmentInfoId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(1, PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_PATIENTHUBCARETEAMDELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteCareTeamProviderTemplate", PROC_CCM_PATIENTHUBCARETEAMDELETE, ex);
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
        public string DeletePatientHubEnrolledGoals(long EnrolledGoalsId, long EnrolledGoalsICDId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENROLLED_GOALS_ID, EnrolledGoalsId);
                dbManager.AddParameters(1, PARM_ENROLLEDGOALS_CPT_ID, EnrolledGoalsICDId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_PATIENTHUBGOALSDELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeletePatientHubEnrolledGoals", PROC_CCM_PATIENTHUBGOALSDELETE, ex);
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
    }
}
