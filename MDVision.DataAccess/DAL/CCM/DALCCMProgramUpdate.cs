using System.ComponentModel;
using System.Diagnostics;
using MDVision.DataAccess.DCommon;
using MDVision.Model.CCM.CCMHub;
using System;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using System.Data;
using MDVision.Common.Logging;
using System.Collections.Generic;

namespace MDVision.DataAccess.DAL.CCM
{
    public class DALCCMProgramUpdate
    {
        #region "Stored Procedure Names"
        private const string PROC_CCM_TASK_TIMER_INSERT = "CCM.sp_TaskTimerInsert";
        private const string PROC_CCM_TASK_TIMER_SELECT = "CCM.sp_TaskTimerSelect";
        private const string PROC_CCM_TASK_TIMER_DELETE = "CCM.sp_TaskTimerDelete";
        private const string PROC_CCM_TASK_TIMER_UPDATE = "CCM.sp_TaskTimerUpdate";

        private const string PROC_CCM_CALL_DETAIL_INSERT = "CCM.sp_CallDetailsInsert";
        private const string PROC_CCM_CALL_DETAIL_SELECT = "CCM.sp_CallDetailsSelect";
        private const string PROC_CCM_CALL_DETAIL_LOOKUP = "CCM.sp_CallDetailLookup";
        private const string PROC_CCM_CALL_DETAIL_UPDATE = "CCM.sp_CallDetailsUpdate";
        private const string PROC_CCM_CALL_DETAIL_DELETE = "CCM.sp_CallDetailsDelete";


        private const string PROC_CCM_TASK_TIMER_AMALGAMATED_INSERT = "CCM.sp_TaskTimerAmalgamatedInsert";
        private const string PROC_CCM_TASK_TIMER_AMALGAMATED_UPDATE = "CCM.sp_TaskTimerAmalgamatedUpdate";

        private const string PROC_CCM_ENROLLEDCARETEAMSLOOKUP = "CCM.sp_EnrolledCareTeamsLookup";

        private const string PROC_CCM_PROGRESS_UPDATE_INSERT = "CCM.sp_ProgressUpdatesInsert";
        private const string PROC_CCM_PROGRESS_UPDATE_SELECT = "CCM.sp_ProgressUpdateSelect";
        private const string PROC_CCM_PROGRESS_UPDATE_DETAIL_SELECT = "CCM.sp_ProgressUpdateDetailSelect";
        private const string PROC_CCM_TASKTIMER_DETAIL_SELECT = "CCM.sp_TaskTimerDetailsSelect";

        #endregion

        #region "Parameters"

        #region Task timer parameters

        private const string PARM_TASK_TIMER_ID = "@TaskTimerId";
        private const string PARM_TASK_TIMER_AMALGAMATED_ID = "@TaskTimerAmalgamatedId";
        private const string PARM_ENROLLMENT_INFO_ID = "@EnrollmentInfoId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_TASK_REASON = "@TaskReason";
        private const string PARM_TASK_REASONID = "@ReasonId";
        private const string PARM_TASK_DATE = "@TaskDate";
        private const string PARM_TASK_TIME = "@TaskTime";
        private const string PARM_TASK_DURATION = "@TaskDuration";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        #endregion

        #region CALL DETAIL PARAMETERS
        private const string PARM_CALL_ID = "@CallId";
        private const string PARM_CARETEAM_ID = "@CareTeamId";
        private const string PARM_CALLER_ID = "@CallerId";
        private const string PARM_CALLER_TYPE = "@CallerType";

        private const string PARM_RECEIVER_NAME = "@ReceiverName";
        private const string PARM_CALL_DATE = "@CallDate";
        private const string PARM_CALL_TIME = "@CallTime";
        private const string PARM_CALL_DURATION = "@Duration";
        private const string PARM_CALL_DURATION_UNITS = "@Units";
        private const string PARM_CALL_REASON = "@CallReason";
        private const string PARM_ACTION = "@Action";
        #endregion

        #region Progress Update Parameter

        private const string PARM_PROGRESS_UPDATE_ID = "@ProgressUpdateId";
        private const string PARM_PROGRESS_CATEGORY_ID = "@ProgressCategoryId";
        private const string PARM_PROGRESS_VALUE = "@Value";
        private const string PARM_GOAL_IMP_TO_PATIENT = "@GoalsImpToPatient";
        private const string PARM_TARGETS = "@Targets";
        private const string PARM_POTENTIAL_BARRIERS = "@PotentialBarriers";
        private const string PARM_EXPECTED_OUTCOMES = "@ExpectedOutcomes";
        private const string PARM_GOALS_ACHIEVED = "@GoalsAchieved";
        private const string PARM_PROGRESS_REDUCING_BARRIERS = "@ProgressReducingBarrriers";
        private const string PARM_PROGRESS_TOWARDS_EXPECTED_OUTCOMES = "@ProgressTowardsExpectedOutcomes";
        private const string PARM_OTHER_INFORMATION = "@OtherInformation";

        private const string PARM_CREATED_TIME = "@CreatedTime";
        private const string PARM_PROGRESSMONTH = "@ProgressMonth";
        private const string PARM_PROGRESSYEAR = "@ProgressYear";


        #endregion

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PROGRESS_MONTH = "@ProgressMonth";
        private const string PARM_PROGRESS_YEAR = "@ProgressYear";
        private const string PARM_SELECTED_MONTH = "@SelectedMonth";


        private const string PARM_TASKTIMERAMALGAMATED_ID = "@TaskTimerAmalgamatedId";
        private const string PARM_TASKTIMERAMALGAMATED_MONTH = "@Month";
        private const string PARM_TASKTIMERAMALGAMATED_YEAR = "@Year";

        #endregion

        #region Constructors
        public DALCCMProgramUpdate()
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

        private void createTaskTimerParameters(IDBManager dbManager, TaskAmalgamatedModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
            {
                dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_ID, model.TaskTimerAmalgamatedId, DbType.Int64, ParamDirection.Output);
            }
            else
                dbManager.AddParameters(PARM_TASK_TIMER_AMALGAMATED_ID, model.TaskTimerAmalgamatedId, DbType.Int64);

            dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(PARM_PATIENT_ID, MDVUtility.ToInt32(model.PatientId));
            dbManager.AddParameters(PARM_TASK_REASON, model.TaskReason);
            if (!string.IsNullOrEmpty(model.ReasonId))
            {
                dbManager.AddParameters(PARM_TASK_REASONID, model.ReasonId);
            }
            else
            {
                dbManager.AddParameters(PARM_TASK_REASONID, DBNull.Value);
            }

            dbManager.AddParameters(PARM_CALLER_ID, model.Caller);
            dbManager.AddParameters(PARM_CALLER_TYPE, model.CallerType);
            dbManager.AddParameters(PARM_CARETEAM_ID, model.CareTeamId);

            dbManager.AddParameters(PARM_TASK_DATE, model.TaskDate);
            dbManager.AddParameters(PARM_TASK_TIME, model.TaskTime);
            dbManager.AddParameters(PARM_TASK_DURATION, model.TaskDuration);
            dbManager.AddParameters(PARM_RECEIVER_NAME, model.ReceiverName);
            dbManager.AddParameters(PARM_COMMENTS, model.Comments);


            dbManager.AddParameters(PARM_IS_ACTIVE, true);
            dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
        }

        private void createTaskTimerParametersForDashBoard(IDBManager dbManager, CCMTaskTimerModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_TASK_TIMER_ID, model.TaskTimerId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_TASK_TIMER_ID, model.TaskTimerId, DbType.Int64);

            dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(PARM_PATIENT_ID, MDVUtility.ToInt32(model.PatientId));
            dbManager.AddParameters(PARM_TASK_REASON, model.TaskReason);
            dbManager.AddParameters(PARM_COMMENTS, model.Comments);
            dbManager.AddParameters(PARM_TASK_DATE, model.TaskDate);
            dbManager.AddParameters(PARM_TASK_TIME, model.TaskTime);
            dbManager.AddParameters(PARM_TASK_DURATION, model.TaskDuration);
            dbManager.AddParameters(PARM_IS_ACTIVE, true);
            dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
        }
        private void createCallDetailsParameters(IDBManager dbManager, TaskAmalgamatedModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
            {
                dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_ID, model.TaskTimerAmalgamatedId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            }
            else
                dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_ID, model.TaskTimerAmalgamatedId);

            dbManager.AddParameters(PARM_CARETEAM_ID, MDVUtility.ToInt32(model.CareTeamId));
            dbManager.AddParameters(PARM_RECEIVER_NAME, MDVUtility.ToStr(model.ReceiverName));

            dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(PARM_PATIENT_ID, MDVUtility.ToInt32(model.PatientId));

            dbManager.AddParameters(PARM_TASK_DATE, model.TaskDate);
            dbManager.AddParameters(PARM_TASK_TIME, model.TaskTime);
            dbManager.AddParameters(PARM_TASK_DURATION, model.TaskDuration);         
            dbManager.AddParameters(PARM_CALL_DURATION_UNITS, model.DurationUnit);

            dbManager.AddParameters(PARM_TASK_REASON, model.TaskReason);
            dbManager.AddParameters(PARM_TASK_REASONID, model.ReasonId);
            
            dbManager.AddParameters(PARM_COMMENTS, model.Comments);

            dbManager.AddParameters(PARM_IS_ACTIVE, true);
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

            dbManager.AddParameters(PARM_CALLER_ID, MDVUtility.ToInt32(model.Caller));
            dbManager.AddParameters(PARM_CALLER_TYPE, model.CallerType);


        }
        private void createProgramUpdateParameters(IDBManager dbManager, CCMProgramUpdateModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_PROGRESS_UPDATE_ID, model.ProgressUpdateId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_PROGRESS_UPDATE_ID, model.ProgressUpdateId, DbType.Int64);

            dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, MDVUtility.ToInt32(model.EnrollmentInfoId));
            dbManager.AddParameters(PARM_PATIENT_ID, MDVUtility.ToInt32(model.PatientId));
            dbManager.AddParameters(PARM_GOAL_IMP_TO_PATIENT, model.GoalsImpToPatient);
            dbManager.AddParameters(PARM_TARGETS, model.Targets);
            dbManager.AddParameters(PARM_POTENTIAL_BARRIERS, model.PotentialBarriers);
            dbManager.AddParameters(PARM_EXPECTED_OUTCOMES, model.ExpectedOutcomes);
            dbManager.AddParameters(PARM_GOALS_ACHIEVED, model.GoalsAchieved);
            dbManager.AddParameters(PARM_PROGRESS_REDUCING_BARRIERS, model.ProgressReducingBarrriers);
            dbManager.AddParameters(PARM_PROGRESS_TOWARDS_EXPECTED_OUTCOMES, model.ProgressTowardsExpectedOutcomes);
            dbManager.AddParameters(PARM_OTHER_INFORMATION, model.OtherInformation);
            dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_CREATED_TIME, DateTime.Now.TimeOfDay);
            dbManager.AddParameters(PARM_PROGRESSMONTH, model.ProgressMonth);
            dbManager.AddParameters(PARM_PROGRESSYEAR, model.ProgressYear);

        }

        public string SaveCCMTaskTimeFromDashBoard(CCMTaskTimerModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createTaskTimerParametersForDashBoard(dbManager, model, true);
                var TaskTimerId = dbManager.ExecuteScalar(PROC_CCM_TASK_TIMER_INSERT);

                return MDVUtility.ToStr(TaskTimerId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::SaveCCMEnrollmentInfo", PROC_CCM_TASK_TIMER_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string SaveCCMTaskTime(TaskAmalgamatedModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createTaskTimerParameters(dbManager, model, true);
                var TaskTimerId = dbManager.ExecuteScalar(PROC_CCM_TASK_TIMER_AMALGAMATED_INSERT);
                return MDVUtility.ToStr(TaskTimerId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::SaveCCMEnrollmentInfo", PROC_CCM_TASK_TIMER_AMALGAMATED_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateCCMTaskTimeDetails(TaskAmalgamatedModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createTaskTimerParameters(dbManager, model, false);
                var TaskTimerId = dbManager.ExecuteScalar(PROC_CCM_TASK_TIMER_AMALGAMATED_UPDATE);

                return MDVUtility.ToStr(TaskTimerId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::UpdateCCMTaskTimeDetails", PROC_CCM_TASK_TIMER_AMALGAMATED_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteCCMTaskTime(CCMTaskTimerModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string returnVal = "";
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_TASK_TIMER_ID, model.TaskTimerId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 255);
                returnVal = dbManager.ExecuteScalar(PROC_CCM_TASK_TIMER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::SaveCCMEnrollmentInfo", PROC_CCM_TASK_TIMER_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateCCMTaskTime(CCMTaskTimerModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string returnVal = "";
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_TASK_TIMER_ID, model.TaskTimerId);
                dbManager.AddParameters(PARM_TASK_REASON, model.TaskReason);
                dbManager.AddParameters(PARM_TASK_DURATION, model.TaskDuration);
                dbManager.AddParameters(PARM_COMMENTS, model.Comments);
                dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                returnVal = dbManager.ExecuteScalar(PROC_CCM_TASK_TIMER_UPDATE).ToString();

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::SaveCCMEnrollmentInfo", PROC_CCM_TASK_TIMER_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CCMTaskTimerModel> LoadCCMTaskTimer(Int32 TaskTimerId, Int32 EnrollmentInfoId, Int32 PatientId, string Action, int SelectedMonth, Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            List<CCMTaskTimerModel> CCMEnrollmentInfoList = new List<CCMTaskTimerModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (TaskTimerId == 0)
                    dbManager.AddParameters(PARM_TASK_TIMER_AMALGAMATED_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_TASK_TIMER_AMALGAMATED_ID, TaskTimerId);
                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(PARM_ACTION, Action);
                dbManager.AddParameters(PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(PARM_SELECTED_MONTH, SelectedMonth);


                return dbManager.ExecuteReaders<CCMTaskTimerModel>(PROC_CCM_TASK_TIMER_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::LoadCCMEnrollmentInfo", PROC_CCM_TASK_TIMER_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public string SaveCCMCallDetails(TaskAmalgamatedModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createCallDetailsParameters(dbManager, model, true);
                var TaskTimerId = dbManager.ExecuteScalar(PROC_CCM_CALL_DETAIL_INSERT);

                return MDVUtility.ToStr(TaskTimerId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::SaveCCMCallDetails", PROC_CCM_CALL_DETAIL_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        
        public string DeleteCCMCallDetails(long callId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_CALL_ID, callId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, DBNull.Value, DbType.Object, ParamDirection.Output, 255);
                var dbCallId = dbManager.ExecuteScalar(PROC_CCM_CALL_DETAIL_DELETE);
                return MDVUtility.ToStr(dbCallId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::DeleteCCMCallDetails", PROC_CCM_CALL_DETAIL_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CCMCallDetailsModel> LoadCCMCallDetails(Int32 CallId, Int32 EnrollmentInfoId, Int32 PatientId, string Action, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            List<CCMCallDetailsModel> CCMCallDetailsList = new List<CCMCallDetailsModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (CallId == 0)
                {
                    dbManager.AddParameters(PARM_TASK_TIMER_AMALGAMATED_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_TASK_TIMER_AMALGAMATED_ID, CallId);
                }
                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(PARM_ACTION, Action);

                CCMCallDetailsList = dbManager.ExecuteReaders<CCMCallDetailsModel>(PROC_CCM_CALL_DETAIL_SELECT);
                return CCMCallDetailsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::LoadCCMCallDetails", PROC_CCM_CALL_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public List<TaskAmalgamatedModel> LoadCCMTaskTimerDetails(Int32 TaskTimerAmalgamatedId, Int32 EnrollmentInfoId, Int32 PatientId, string Action, Int64 month, Int64 year, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            List<TaskAmalgamatedModel> CCMCallDetailsList = new List<TaskAmalgamatedModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (TaskTimerAmalgamatedId == 0)
                    dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_ID, TaskTimerAmalgamatedId);

                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);

                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(PARM_ACTION, Action);

                if (month == 0)
                    dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_MONTH, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_MONTH, month);

                if (year == 0)
                    dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_YEAR, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_TASKTIMERAMALGAMATED_YEAR, year);

                CCMCallDetailsList = dbManager.ExecuteReaders<TaskAmalgamatedModel>(PROC_CCM_TASKTIMER_DETAIL_SELECT);
                return CCMCallDetailsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::LoadCCMTaskTimerDetails", PROC_CCM_TASKTIMER_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<CallDetailLookupModel> CallDetailsLookup(Int64 EnrollmentInfoId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                return dbManager.ExecuteReaders<CallDetailLookupModel>(PROC_CCM_CALL_DETAIL_LOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgrmauPdate::CallDetailsLookup", PROC_CCM_CALL_DETAIL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<CareTeamLookupModel> PatientCareTeamLookup(Int64 PatientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                return dbManager.ExecuteReaders<CareTeamLookupModel>(PROC_CCM_ENROLLEDCARETEAMSLOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgrmauPdate::CareTeamLookup", PROC_CCM_ENROLLEDCARETEAMSLOOKUP, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public string SaveProgramUpdate(CCMProgramUpdateModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createProgramUpdateParameters(dbManager, model, true);
                var ProgressUpdateId = dbManager.ExecuteScalar(PROC_CCM_PROGRESS_UPDATE_INSERT);

                return MDVUtility.ToStr(ProgressUpdateId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMProgramUpdate::SaveProgramUpdate", PROC_CCM_PROGRESS_UPDATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CCMProgramUpdateModel> LoadProgressUpdate(Int32 ProgressUpdateId, Int32 ProgressCategoryId, Int32 EnrollmentInfoId, Int32 PatientId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            List<CCMProgramUpdateModel> CCMProgressUpdateList = new List<CCMProgramUpdateModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ProgressUpdateId == 0)
                    dbManager.AddParameters(PARM_PROGRESS_UPDATE_ID, null);
                else
                    dbManager.AddParameters(PARM_PROGRESS_UPDATE_ID, ProgressUpdateId);

                if (ProgressCategoryId == 0)
                    dbManager.AddParameters(PARM_PROGRESS_CATEGORY_ID, null);
                else
                    dbManager.AddParameters(PARM_PROGRESS_CATEGORY_ID, ProgressCategoryId);

                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, null);
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                return dbManager.ExecuteReaders<CCMProgramUpdateModel>(PROC_CCM_PROGRESS_UPDATE_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::LoadProgressUpdate", PROC_CCM_PROGRESS_UPDATE_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<CCMProgramUpdateModel> LoadProgressUpdateDetail(Int32 EnrollmentInfoId, Int32 PatientId, Int32 ProgressMonth, Int32 ProgressYear)
        {


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (EnrollmentInfoId == 0)
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, null);
                else
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (ProgressMonth == 0)
                    dbManager.AddParameters(PARM_PROGRESS_MONTH, null);
                else
                    dbManager.AddParameters(PARM_PROGRESS_MONTH, ProgressMonth);
                if (ProgressYear == 0)
                    dbManager.AddParameters(PARM_PROGRESS_YEAR, null);
                else
                    dbManager.AddParameters(PARM_PROGRESS_YEAR, ProgressYear);

                return dbManager.ExecuteReaders<CCMProgramUpdateModel>(PROC_CCM_PROGRESS_UPDATE_DETAIL_SELECT);

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCMEnrollmentInfo::LoadProgressUpdateDetail", PROC_CCM_PROGRESS_UPDATE_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
            }


        }
    }
}




