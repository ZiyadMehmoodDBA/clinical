using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using System.Data.SqlClient;
using MDVision.Model.CCM;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.CCM 
{
    public class DALCCMEnrollmentInfo
    {
        #region "Stored Procedure Names"
        private const string PROC_CCM_ENROLLMENT_INFO_DETAIL_SELECT = "CCM.sp_EnrollmentInfoDetailSelect";
        private const string PROC_CCM_ENROLLMENT_INFO_SELECT = "CCM.sp_EnrollmentInfoSelect";
        private const string PROC_CCM_ENROLLMENT_INFO_INSERT = "CCM.sp_EnrollmentInfoInsert";
        private const string PROC_CCM_ENROLLMENT_INFO_UPDATE = "CCM.sp_EnrollmentInfoUpdate";
        private const string PROC_CCM_ENROLLMENT_INFO_RESUME = "CCM.sp_EnrollmentInfoResume";
        private const string PROC_CCM_ENROLLMENT_INFO_TERMINATION = "CCM.sp_EnrollmentInfoTermination";
        private const string PROC_CCM_ENROLLMENT_DECLINE_INSERT = "CCM.sp_EnrollmentDeclineInsert";

        private const string PROC_CCM_ENROLLMENT_INFO_PROGRAM_LOOKUP = "CCM.sp_EnrollmentInfoProgramLookup";

        #endregion

        #region "Parameters"

        private const string PARM_ENROLLMENT_INFO_ID = "@EnrollmentInfoId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_Template_ID = "@providerId";
        private const string PARM_STATUS_ID = "@StatusId";

        private const string PARM_STATUS = "@Status";
        private const string PARM_REASON = "@Reason";

        private const string PARM_PROGRAM = "@Program";
        private const string PARM_PROGRAMID = "@ProgramId";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_DURATION_UNIT = "@DurationUnit";
        private const string PARM_STARTING_FROM = "@StartingFrom";
        private const string PARM_ENDING_ON = "@EndingOn";
        private const string PARM_CARE_TEAM_ID = "@CareTeamId";
        private const string PARM_CONSENT_FILE_STREAM = "@ConsentFileStream";
        private const string PARM_CONSENT_PATH = "@ConsentPath";
        private const string PARM_CONSENT_FILENAME = "@ConsentFileName";



        private const string PARM_CONSENT_DATE = "@ConsentDate";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_DECLINE_REASON = "@DeclineReason";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_IS_VERBAL = "@IsVerbal";
        private const string PARM_BINARY_DATA = "@BinaryData";
        private const string PARM_URL = "@Url";

        #endregion

        #region Constructors
        public DALCCMEnrollmentInfo()
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

        #region CCM ENTROLLMENT INFO

        private void createParameters(IDBManager dbManager, CCMEnrollmentInfoModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(20);
            int i = 0;

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);

            dbManager.AddParameters(i++, PARM_PATIENT_ID, MDVUtility.ToInt32(model.PatientId));
            dbManager.AddParameters(i++, PARM_STATUS_ID, MDVUtility.ToInt32(model.StatusId));
            dbManager.AddParameters(i++, PARM_PROGRAMID, MDVUtility.ToInt32(model.Program));
            dbManager.AddParameters(i++, PARM_DURATION, model.Duration);
            dbManager.AddParameters(i++, PARM_DURATION_UNIT, model.DurationUnit);
            dbManager.AddParameters(i++, PARM_STARTING_FROM, model.StartingFrom == "" ? null : model.StartingFrom);
            dbManager.AddParameters(i++, PARM_ENDING_ON, model.EndingOn == "" ? null : model.EndingOn);
            dbManager.AddParameters(i++, PARM_CARE_TEAM_ID, model.CareTeam);
            dbManager.AddParameters(i++, PARM_CONSENT_PATH, model.ConsentFileStream);
            dbManager.AddParameters(i++, PARM_CONSENT_FILENAME, model.ConsentFileName);
            dbManager.AddParameters(i++, PARM_CONSENT_DATE, model.ConsentDate == "" ? null : model.ConsentDate);
            dbManager.AddParameters(i++, PARM_COMMENTS, model.Comments);
            dbManager.AddParameters(i++, PARM_IS_ACTIVE, true);
            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_IS_VERBAL, MDVUtility.ToBool(model.ISVerbal));
            //dbManager.AddParameters(i++, PARM_BINARY_DATA, model.BinaryData);
            dbManager.AddParameters(i++, PARM_URL, model.Url);
        }
        public List<CCMEnrollmentInfoModel> LoadCCMEnrollmentInfo(Int32 PatientId, Int32 ProviderId, Int32 InsurancePlanId, long UserId, string StatusId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            List<CCMEnrollmentInfoModel> CCMEnrollmentInfoList = new List<CCMEnrollmentInfoModel>();

            //  SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //  List<SqlParameter> parameters = new List<SqlParameter>();

                //int i = 0;
                //dbManager.Open();
                //dbManager.CreateParameters(8);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                //  parameters.Add(new SqlParameter(PARM_PATIENT_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                //   parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                if (ProviderId == 0)
                    dbManager.AddParameters(PARM_PROVIDER_ID, null);
                //parameters.Add(new SqlParameter(PARM_PROVIDER_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                //parameters.Add(new SqlParameter(PARM_PROVIDER_ID, ProviderId));

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(PARM_INSURANCE_PLAN_ID, null);
                //parameters.Add(new SqlParameter(PARM_INSURANCE_PLAN_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_INSURANCE_PLAN_ID, InsurancePlanId);
                //parameters.Add(new SqlParameter(PARM_INSURANCE_PLAN_ID, InsurancePlanId));

                if (StatusId == "")
                    dbManager.AddParameters(PARM_STATUS_ID, null);
                //parameters.Add(new SqlParameter(PARM_STATUS_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_STATUS_ID, StatusId);
                // parameters.Add(new SqlParameter(PARM_STATUS_ID, StatusId));

                dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
                //parameters.Add(new SqlParameter(PARM_ENTITY_ID, MDVSession.Current.EntityId));
                dbManager.AddParameters(PARM_USER_ID, MDVSession.Current.AppUserId);
                // parameters.Add(new SqlParameter(PARM_USER_ID, MDVSession.Current.AppUserId));


                if (PageNumber <= 0)
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, DBNull.Value);
                    // parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                    //parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, PageNumber));
                }
                if (RowsPerPage <= 0)
                {
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, DBNull.Value);
                    //parameters.Add(new SqlParameter(PARM_ROWS_PER_PAGE, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, RowsPerPage);
                    //parameters.Add(new SqlParameter(PARM_ROWS_PER_PAGE, RowsPerPage));
                }

                // dbManager.AddParameters(i++, PARM_USER_ID, (MDVSession.Current.AppUserId));

                

                return dbManager.ExecuteReaders<CCMEnrollmentInfoModel>(PROC_CCM_ENROLLMENT_INFO_SELECT);

                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_SELECT);

                //CCMEnrollmentInfoModel model = null;
                //while (reader.Read())
                //{
                //    model = new CCMEnrollmentInfoModel();
                //    model.EnrollmentInfoId = !String.IsNullOrEmpty(reader["EnrollmentInfoId"].ToString()) ? reader["EnrollmentInfoId"].ToString() : "";
                //    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                //    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                //    model.PatientName = !String.IsNullOrEmpty(reader["PatientName"].ToString()) ? reader["PatientName"].ToString() : "";
                //    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                //    model.Problems = !String.IsNullOrEmpty(reader["Problems"].ToString()) ? reader["Problems"].ToString() : "";
                //    model.NoOfProblems = !String.IsNullOrEmpty(reader["NoOfProblems"].ToString()) ? reader["NoOfProblems"].ToString() : "";
                //    model.InsuranceName = !String.IsNullOrEmpty(reader["InsuranceName"].ToString()) ? reader["InsuranceName"].ToString() : "";
                //    model.Consent = !String.IsNullOrEmpty(reader["Consent"].ToString()) ? reader["Consent"].ToString() : "";
                //    model.TimeCompleted = !String.IsNullOrEmpty(reader["TimeCompleted"].ToString()) ? reader["TimeCompleted"].ToString() : "";
                //    model.Duration = !String.IsNullOrEmpty(reader["Duration"].ToString()) ? reader["Duration"].ToString() : "";
                //    model.Status = !String.IsNullOrEmpty(reader["Status"].ToString()) ? reader["Status"].ToString() : "";
                //    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                //    model.Program = !String.IsNullOrEmpty(reader["Program"].ToString()) ? reader["Program"].ToString() : "";
                //    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                //    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                //    CCMEnrollmentInfoList.Add(model);
                //}


                //return CCMEnrollmentInfoList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::LoadCCMEnrollmentInfo", PROC_CCM_ENROLLMENT_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }
        public CCMEnrollmentInfoModel LoadCCMEnrollmentInfoDetail(Int32 EnrollmentInfoId)
        {

            //SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // List<SqlParameter> parameters = new List<SqlParameter>();

                //int i = 0;
                //dbManager.Open();
                //dbManager.CreateParameters(1);

                dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId));



                return dbManager.ExecuteReader<CCMEnrollmentInfoModel>(PROC_CCM_ENROLLMENT_INFO_DETAIL_SELECT);

                //  reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_DETAIL_SELECT);

                //CCMEnrollmentInfoModel model = null;

                //while (reader.Read())
                //{
                //    model = new CCMEnrollmentInfoModel();
                //    model.EnrollmentInfoId = !String.IsNullOrEmpty(reader["EnrollmentInfoId"].ToString()) ? reader["EnrollmentInfoId"].ToString() : "";
                //    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                //    model.CareTeam = !String.IsNullOrEmpty(reader["CareTeam"].ToString()) ? reader["CareTeam"].ToString() : "";
                //    model.Program = !String.IsNullOrEmpty(reader["Program"].ToString()) ? reader["Program"].ToString() : "";
                //    model.Duration = !String.IsNullOrEmpty(reader["Duration"].ToString()) ? reader["Duration"].ToString() : "";
                //    model.DurationUnit = !String.IsNullOrEmpty(reader["DurationUnit"].ToString()) ? reader["DurationUnit"].ToString() : "";
                //    model.StartingFrom = !String.IsNullOrEmpty(reader["StartingFrom"].ToString()) ? reader["StartingFrom"].ToString() : "";
                //    model.EndingOn = !String.IsNullOrEmpty(reader["EndingOn"].ToString()) ? reader["EndingOn"].ToString() : "";
                //    model.ConsentDate = !String.IsNullOrEmpty(reader["ConsentDate"].ToString()) ? reader["ConsentDate"].ToString() : "";
                //    model.ConsentPath = !String.IsNullOrEmpty(reader["ConsentPath"].ToString()) ? reader["ConsentPath"].ToString() : "";
                //    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                //    model.ISVerbal = !String.IsNullOrEmpty(reader["ISVerbal"].ToString()) ? reader["ISVerbal"].ToString() : "";
                //    model.ConsentFileName = !String.IsNullOrEmpty(reader["ConsentFileName"].ToString()) ? reader["ConsentFileName"].ToString() : "";
                //}


                //return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::LoadCCMEnrollmentInfoDetail", PROC_CCM_ENROLLMENT_INFO_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }
        public string SaveCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnValue = "";
                dbManager.Open();
                createParameters(dbManager, model, true);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_INSERT);
                while (reader.Read())
                {
                    returnValue = reader["PatientId"].ToString();
                }
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::SaveCCMEnrollmentInfo", PROC_CCM_ENROLLMENT_INFO_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string UpdateCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnValue = "";
                dbManager.Open();
                createParameters(dbManager, model, false);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_UPDATE);
                while (reader.Read())
                {
                    returnValue = reader["PatientId"].ToString();
                }
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::UpdateCCMEnrollmentInfo", PROC_CCM_ENROLLMENT_INFO_UPDATE, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }


        }
        public string SaveCCMEnrollmentDecline(CCMEnrollmentInfoModel model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                dbManager.Open();
                dbManager.CreateParameters(9);

                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_PATIENT_ID, model.PatientId);
                dbManager.AddParameters(i++, PARM_STATUS_ID, model.StatusId);
                dbManager.AddParameters(i++, PARM_DECLINE_REASON, model.DeclineReason);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                var EnrollmentInfoId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_DECLINE_INSERT);

                return MDVUtility.ToStr(EnrollmentInfoId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::SaveCCMEnrollmentDecline", PROC_CCM_ENROLLMENT_DECLINE_INSERT, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public string ResumeCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                dbManager.Open();
                dbManager.CreateParameters(8);

                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_PATIENT_ID, model.PatientId);
                dbManager.AddParameters(i++, PARM_STATUS_ID, model.StatusId);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                var EnrollmentInfoId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_RESUME);

                return MDVUtility.ToStr(EnrollmentInfoId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::ResumeCCMEnrollmentInfo", PROC_CCM_ENROLLMENT_INFO_RESUME, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }


        }
        public string TerminateCCMEnrollmentInfo(Model.CCM.CCMHub.CCMTermination model)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                dbManager.Open();
                dbManager.CreateParameters(8);

                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_STATUS, model.Status);
                dbManager.AddParameters(i++, PARM_REASON, model.Reason);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                var EnrollmentInfoId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_TERMINATION);

                return MDVUtility.ToStr(EnrollmentInfoId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::TerminateCCMEnrollmentInfo", PROC_CCM_ENROLLMENT_INFO_TERMINATION, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }


        }
        public List<EnrollmentInfoProgram> loadEnrollmentInfoPrograms()
        {
            List<EnrollmentInfoProgram> listModel = new List<EnrollmentInfoProgram>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_PROGRAM_LOOKUP);
                EnrollmentInfoProgram modelFill = null;
                while (reader.Read())
                {
                    modelFill = new EnrollmentInfoProgram();
                    modelFill.ProgramId = Convert.ToInt64(reader["ProgramId"]);
                    modelFill.ProgramName = MDVUtility.CheckStringNull(reader["ProgramName"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::loadEnrollmentInfoPrograms", PROC_CCM_ENROLLMENT_INFO_PROGRAM_LOOKUP, ex);
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


