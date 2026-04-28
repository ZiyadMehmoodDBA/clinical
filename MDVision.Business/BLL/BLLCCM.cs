using System;
using System.Collections.Generic;
using System.Diagnostics;
using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.CCM;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.CCM;
using MDVision.Model.CCM.CCMHub;
using MDVision.Model.CCM.PatientHub;
using MDVision.Model.Lookups;

namespace MDVision.Business.BLL
{
    public class BLLCCM
    {
        #region Constructors
        public BLLCCM()
        {
            //SharedVariable
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }

        //  private IContainer components;



        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            //  components = new System.ComponentModel.Container();
        }

        #endregion

        #region Patient Hub


        #region Goals


        public string SaveCCMEnrolledGoals(EnrolledGoals model)
        {
            try
            {
                string patientId;
                patientId = new DALPatientHub().SaveCCMEnrolledGoals(model);

                return (patientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMEnrolledGoals", ex);

                throw ex;
            }
            finally
            {

            }
        }

        public string SaveCCMEnrolledGoalsCPT(EnrolledGoalsCPT model)
        {
            try
            {
                string patientId;
                patientId = new DALPatientHub().SaveCCMEnrolledGoalsCPT(model);

                return (patientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMEnrolledGoalsCPT", ex);

                throw ex;
            }
            finally
            {

            }
        }

        public string SaveCCMEnrolledGoals_EnrolledGoalsCPT(EnrolledGoals_EnrolledGoalsCPT model)
        {
            try
            {
                string patientId;
                patientId = new DALPatientHub().SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT(model);

                return (patientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMEnrolledGoals_EnrolledGoalsCPT", ex);

                throw ex;
            }
            finally
            {

            }
        }

        #endregion

        #region Hub Static

        public BLObject<List<PatientHubStatic>> LoadCCMPatientHUBStatic(long PatientId, long EnrollmentInfoId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                //List<Model.CCM.CCMHub.PatientHubStatic> CCMEnrollmentInfoList = new List<Model.CCM.CCMHub.PatientHubStatic>();
                //CCMEnrollmentInfoList = new DALPatientHub().LoadCCMPatientHUB(PatientId, PageNumber, RowsPerPage);
                //return new BLObject<List<MDVision.Model.CCM.CCMHub.PatientHubStatic>>(CCMEnrollmentInfoList);

                List<PatientHubStatic> result = new DALPatientHub().LoadCCMPatientHUBStatic(PatientId, EnrollmentInfoId, PageNumber, RowsPerPage);
                return new BLObject<List<PatientHubStatic>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMPatientHUB", ex);
                return new BLObject<List<PatientHubStatic>>(null, ex.Message);
            }
        }

        public BLObject<List<PatientHubProblems>> LoadCCMPatientHUBProblems(long PatientId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                List<PatientHubProblems> result = new DALPatientHub().LoadCCMPatientHUBProblems(PatientId, PageNumber, RowsPerPage);
                return new BLObject<List<PatientHubProblems>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMPatientHUBProblems", ex);
                return new BLObject<List<PatientHubProblems>>(null, ex.Message);
            }
        }
        //public BLObject<List<Model.CCM.CCMHub.EnrolledRiskAssessmentTemp>> LoadCCMPatientHUBRiskAssessmentScore(long EnrollmentInfoId)
        //{
        //    try
        //    {
        //        List<Model.CCM.CCMHub.EnrolledRiskAssessmentTemp> result = new DALPatientHub().LoadCCMPatientHUBRiskAssessmentScore(EnrollmentInfoId);
        //        return new BLObject<List<Model.CCM.CCMHub.EnrolledRiskAssessmentTemp>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLCCM::LoadCCMPatientHUBProblems", ex);
        //        return new BLObject<List<Model.CCM.CCMHub.EnrolledRiskAssessmentTemp>>(null, ex.Message);
        //    }
        //}
        public BLObject<List<EnrolledRiskAssessment>> LoadCCMPatientHUBRiskAssessmentScore(long EnrollmentInfoId)
        {
            try
            {
                List<EnrolledRiskAssessment> result = new DALPatientHub().LoadCCMPatientHUBRiskAssessmentScore(EnrollmentInfoId);
                return new BLObject<List<EnrolledRiskAssessment>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMPatientHUBProblems", ex);
                return new BLObject<List<EnrolledRiskAssessment>>(null, ex.Message);
            }
        }
        public BLObject<List<ProviderCareTeam>> LoadCCMPatientHUBCareTeam(long ProviderId, long EnrollmentInfoId, long CareTeamId)
        {
            try
            {
                List<ProviderCareTeam> result = new DALPatientHub().LoadCCMPatientHUBCareTeam(ProviderId, EnrollmentInfoId, CareTeamId);
                return new BLObject<List<ProviderCareTeam>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMPatientHUBCareTeam", ex);
                return new BLObject<List<ProviderCareTeam>>(null, ex.Message);
            }
        }
        public BLObject<List<PatientHubEnrolledGoalsCPT>> LoadCCMPatientHUBGoals(long EnrollmentInfoId)
        {
            try
            {
                List<PatientHubEnrolledGoalsCPT> result = new DALPatientHub().LoadCCMPatientHUBGoals(EnrollmentInfoId);
                return new BLObject<List<PatientHubEnrolledGoalsCPT>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMPatientHUBGoals", ex);
                return new BLObject<List<PatientHubEnrolledGoalsCPT>>(null, ex.Message);
            }
        }

        public long InsertCCMPatientHUBEnrolledCareTeam(EnrolledCareTeam model)
        {
            try
            {
                long EnrolledCareTeamId = new DALPatientHub().InsertCCMPatientHUBEnrolledCareTeam(model);
                return (EnrolledCareTeamId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::InsertCCMPatientHUBCareTeam", ex);

                throw ex;
            }
            finally
            {

            }
        }

        public long InsertCCMPatientHUBRiskAssessmentTemplate(RiskAssessment model)
        {
            try
            {
                long EnrolledRiskAssessmentTempId = new DALPatientHub().InsertCCMPatientHUBRiskAssessmentTemplate(model);

                return (EnrolledRiskAssessmentTempId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::InsertCCMPatientHUBRiskAssessmentTemplate", ex);

                throw ex;
            }
            finally
            {

            }
        }
        public long InsertUpdateCCMPatientHUBRiskAssessmentScore(RiskAssessment model)
        {
            try
            {
                long EnrolledRiskAssessmentTempId = new DALPatientHub().InsertUpdateCCMPatientHUBRiskAssessmentScore(model);

                return (EnrolledRiskAssessmentTempId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::InsertUpdateCCMPatientHUBRiskAssessmentScore", ex);

                throw ex;
            }
            finally
            {

            }
        }

        public BLObject<string> DeleteChronicProb(long ChronicProblemId, long PatientId)
        {
            try
            {
                string message = new DALPatientHub().DeleteChronicProblems(ChronicProblemId, PatientId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeleteChronicProblem", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteRiskAssessmentScoreTemplate(long RiskAssessmentId)
        {
            try
            {
                string message = new DALPatientHub().DeleteRiskAssessmentScoreTemplate(RiskAssessmentId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeleteChronicProblem", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteCareTeamProviderTemplate(long ProviderId, long EnrollmentInfoId)
        {
            try
            {
                string message = new DALPatientHub().DeleteCareTeamProviderTemplate(ProviderId, EnrollmentInfoId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeleteCareTeamProviderTemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> DeletePatientHubEnrolledGoals(long EnrolledGoalsId, long EnrolledGoalsICDId)
        {
            try
            {
                string message = new DALPatientHub().DeletePatientHubEnrolledGoals(EnrolledGoalsId, EnrolledGoalsICDId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeletePatientHubEnrolledGoals", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteRiskAssessmentScoreTemplate(long EnrollmentInfoId, long RiskAssessTemptId)
        {
            try
            {
                string message = new DALPatientHub().DeleteRiskAssessmentScoreTemplate(EnrollmentInfoId, RiskAssessTemptId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeleteChronicProblem", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion

        #endregion

        #region CCM ENTROLLMENT INFO

        public BLObject<List<CCMEnrollmentInfoModel>> LoadCCMEnrollmentInfo(Int32 PatientId, Int32 ProviderId, Int32 InsurancePlanId, long UserId, string Status, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                List<CCMEnrollmentInfoModel> CCMEnrollmentInfoList = new List<CCMEnrollmentInfoModel>();
                CCMEnrollmentInfoList = new DALCCMEnrollmentInfo().LoadCCMEnrollmentInfo(PatientId, ProviderId, InsurancePlanId, UserId, Status, PageNumber, RowsPerPage);

                return new BLObject<List<CCMEnrollmentInfoModel>>(CCMEnrollmentInfoList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMEnrollmentInfo", ex);
                return new BLObject<List<CCMEnrollmentInfoModel>>(null, ex.Message);
            }
        }
        public BLObject<CCMEnrollmentInfoModel> LoadCCMEnrollmentInfoDetail(Int32 EnrollmentInfoId)
        {
            CCMEnrollmentInfoModel model;
            try
            {
                model = new DALCCMEnrollmentInfo().LoadCCMEnrollmentInfoDetail(EnrollmentInfoId);

                return new BLObject<CCMEnrollmentInfoModel>(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMEnrollmentInfoDetail", ex);
                return new BLObject<CCMEnrollmentInfoModel>(null, ex.Message);
            }
        }
        public string SaveCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            try
            {
                string patientId;
                patientId = new DALCCMEnrollmentInfo().SaveCCMEnrollmentInfo(model);

                return (patientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMEnrollmentInfoS", ex);

                throw ex;
            }
            finally
            {

            }
        }
        public string UpdateCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            try
            {
                string patientId;
                patientId = new DALCCMEnrollmentInfo().UpdateCCMEnrollmentInfo(model);

                return (patientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::UpdateCCMEnrollmentInfo", ex);

                throw ex;
            }
            finally
            {

            }
        }
        public string SaveCCMEnrollmentDecline(CCMEnrollmentInfoModel model)
        {
            try
            {
                string EnrollmentInfoId;
                EnrollmentInfoId = new DALCCMEnrollmentInfo().SaveCCMEnrollmentDecline(model);

                return EnrollmentInfoId;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMEnrollmentDecline", ex);

                throw ex;
            }
            finally
            {

            }
        }
        public string ResumeCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            try
            {
                string EnrollmentInfoId;
                EnrollmentInfoId = new DALCCMEnrollmentInfo().ResumeCCMEnrollmentInfo(model);

                return (EnrollmentInfoId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::ResumeCCMEnrollmentInfo", ex);

                throw ex;
            }
            finally
            {

            }
        }
        public string TerminationCCMEnrollmentInfo(CCMTermination model)
        {
            try
            {
                string EnrollmentInfoId;
                EnrollmentInfoId = new DALCCMEnrollmentInfo().TerminateCCMEnrollmentInfo(model);

                return (EnrollmentInfoId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::TerminationCCMEnrollmentInfo", ex);

                throw ex;
            }
            finally
            {

            }
        }
        public BLObject<List<EnrollmentInfoProgram>> loadEnrollmentInfoPrograms()
        {
            try
            {
                List<EnrollmentInfoProgram> result = new DALCCMEnrollmentInfo().loadEnrollmentInfoPrograms();
                return new BLObject<List<EnrollmentInfoProgram>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::loadEnrollmentInfoPrograms", ex);
                return new BLObject<List<EnrollmentInfoProgram>>(null, ex.Message);
            }
        }
        #endregion

        #region Program Update

        //Program Update functions will go here

        public string SaveCCMTaskTimeFromDashBoard(CCMTaskTimerModel model)
        {
            try
            {
                return new DALCCMProgramUpdate().SaveCCMTaskTimeFromDashBoard(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMTaskTime", ex);
                throw ex;
            }
            finally
            {

            }
        }

        public string SaveCCMTaskTime(TaskAmalgamatedModel model)
        {
            try
            {
                return new DALCCMProgramUpdate().SaveCCMTaskTime(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMTaskTime", ex);
                throw ex;
            }
            finally
            {

            }
        }
        public string DeleteCCMTaskTime(CCMTaskTimerModel model)
        {
            try
            {
                return new DALCCMProgramUpdate().DeleteCCMTaskTime(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeleteCCMTaskTime", ex);
                throw ex;
            }
            finally
            {

            }
        }
        public string UpdateCCMTaskTime(CCMTaskTimerModel model)
        {
            try
            {
                Convert.ToDecimal(model.TaskDuration); // :-( check valid decimal number
                return new DALCCMProgramUpdate().UpdateCCMTaskTime(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::UpdateCCMTaskTime", ex);
                throw ex;
            }
            finally
            { }
        }

        public BLObject<List<CCMTaskTimerModel>> LoadCCMTaskTimer(Int32 TaskTimerId, Int32 EnrollmentInfoId, Int32 PatientId, string Action, int SelectedMonth, Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            try
            {
                List<CCMTaskTimerModel> CCMEnrollmentInfoList = new List<CCMTaskTimerModel>();
                CCMEnrollmentInfoList = new DALCCMProgramUpdate().LoadCCMTaskTimer(TaskTimerId, EnrollmentInfoId, PatientId, Action, SelectedMonth, PageNumber, RowsPerPage);

                return new BLObject<List<CCMTaskTimerModel>>(CCMEnrollmentInfoList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMTaskTimer", ex);
                return new BLObject<List<CCMTaskTimerModel>>(null, ex.Message);
            }
        }
        public string SaveCCMCallDetails(TaskAmalgamatedModel model)
        {
            try
            {
                return new DALCCMProgramUpdate().SaveCCMCallDetails(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveCCMCallDetails", ex);
                throw ex;
            }
            finally
            {

            }
        }

        public string UpdateCCMTaskTimeDetails(TaskAmalgamatedModel model)
        {
            try
            {
                return new DALCCMProgramUpdate().UpdateCCMTaskTimeDetails(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::UpdateCCMCallDetails", ex);
                throw ex;
            }
            finally
            {

            }
        }

        public string DeleteCCMCallDetails(long callId)
        {
            try
            {
                return new DALCCMProgramUpdate().DeleteCCMCallDetails(callId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::DeleteCCMCallDetails", ex);
                throw ex;
            }
            finally
            {

            }
        }

        public BLObject<List<CCMCallDetailsModel>> LoadCCMCallDetails(Int32 CallId, Int32 EnrollmentInfoId, Int32 PatientId, string Action, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                List<CCMCallDetailsModel> CCMEnrollmentInfoList = new List<CCMCallDetailsModel>();
                CCMEnrollmentInfoList = new DALCCMProgramUpdate().LoadCCMCallDetails(CallId, EnrollmentInfoId, PatientId, Action, PageNumber, RowsPerPage);

                return new BLObject<List<CCMCallDetailsModel>>(CCMEnrollmentInfoList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMCallDetails", ex);
                return new BLObject<List<CCMCallDetailsModel>>(null, ex.Message);
            }
        }
        public BLObject<List<TaskAmalgamatedModel>> LoadCCMTaskTimerDetails(Int32 TaskTimerAmalgamatedId, Int32 EnrollmentInfoId, Int32 PatientId, string Action, Int64 month, Int64 year, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                List<TaskAmalgamatedModel> CCMEnrollmentInfoList = new List<TaskAmalgamatedModel>();
                CCMEnrollmentInfoList = new DALCCMProgramUpdate().LoadCCMTaskTimerDetails(TaskTimerAmalgamatedId, EnrollmentInfoId, PatientId, Action, month, year, PageNumber, RowsPerPage);

                return new BLObject<List<TaskAmalgamatedModel>>(CCMEnrollmentInfoList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadCCMTaskTimerDetails", ex);
                return new BLObject<List<TaskAmalgamatedModel>>(null, ex.Message);
            }
        }

        public string SaveProgramUpdate(CCMProgramUpdateModel model)
        {
            try
            {
                return new DALCCMProgramUpdate().SaveProgramUpdate(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::SaveProgramUpdate", ex);
                throw ex;
            }
            finally
            {

            }
        }


        public BLObject<List<CCMProgramUpdateModel>> LoadProgressUpdate(Int32 ProgressUpdateId, Int32 ProgressCategoryId, Int32 EnrollmentInfoId, Int32 PatientId, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            try
            {
                List<CCMProgramUpdateModel> CCMProgramUpdateList = new List<CCMProgramUpdateModel>();
                CCMProgramUpdateList = new DALCCMProgramUpdate().LoadProgressUpdate(ProgressUpdateId, ProgressCategoryId, EnrollmentInfoId, PatientId, PageNumber, RowsPerPage);

                return new BLObject<List<CCMProgramUpdateModel>>(CCMProgramUpdateList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadProgressUpdate", ex);
                return new BLObject<List<CCMProgramUpdateModel>>(null, ex.Message);
            }
        }


        public BLObject<List<CCMProgramUpdateModel>> LoadProgressUpdateDetail(Int32 EnrollmentInfoId, Int32 PatientId, Int32 ProgressMonth, Int32 ProgressYear)
        {
            try
            {
                List<CCMProgramUpdateModel> CCMProgramUpdateList = new List<CCMProgramUpdateModel>();
                CCMProgramUpdateList = new DALCCMProgramUpdate().LoadProgressUpdateDetail(EnrollmentInfoId, PatientId, ProgressMonth, ProgressYear);

                return new BLObject<List<CCMProgramUpdateModel>>(CCMProgramUpdateList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::LoadProgressUpdateDetail", ex);
                return new BLObject<List<CCMProgramUpdateModel>>(null, ex.Message);
            }
        }





        #endregion

        #region Care Plan hub
        public BLObject<List<CarePlanFillModel>> loadCarePlanList(CarePlanSearchModel model)
        {
            try
            {
                List<CarePlanFillModel> result = new DALCarePlan().loadCarePlanList(model);
                return new BLObject<List<CarePlanFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::loadCarePlanList", ex);
                return new BLObject<List<CarePlanFillModel>>(null, ex.Message);
            }
        }

        public List<CarePlanModel> fillCarePlanList(long carePlanId)
        {
            try
            {
                List<CarePlanModel> result = new DALCarePlan().fillCarePlanList(carePlanId);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::fillCarePlanList", ex);
                throw ex;
            }
        }

        public BLObject<string> updateStatusCarePlanList(long carePlanId, string isActive)
        {
            try
            {
                string message = new DALCarePlan().updateStatusCarePlanList(carePlanId, isActive);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::updateStatusCarePlanList", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> deleteCarePlan(long carePlanId)
        {
            try
            {
                string message = new DALCarePlan().deleteCarePlan(carePlanId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::deleteCarePlan", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public long saveCarePlanList(CarePlanModel model)
        {
            try
            {
                model.CarePlanId = new DALCarePlan().saveCarePlanList(model);
                return (model.CarePlanId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::saveCarePlanList", ex);
                throw ex;
            }
        }
        public long updateCarePlanList(CarePlanModel model)
        {
            try
            {
                model.CarePlanId = new DALCarePlan().updateCarePlanList(model);
                return (model.CarePlanId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::saveCarePlanList", ex);
                throw ex;
            }
        }

        public BLObject<List<CarePlanTemptModel>> loadCarePlanTemplates()
        {
            try
            {
                List<CarePlanTemptModel> result = new DALCarePlan().loadCarePlanTemplates();
                return new BLObject<List<CarePlanTemptModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::loadCarePlanTemplates", ex);
                return new BLObject<List<CarePlanTemptModel>>(null, ex.Message);
            }
        }
        //public BLObject<string> InsertTemplate(CCMTemplateModel model)
        //{
        //    try
        //    {
        //        string TemplateId;
        //        TemplateId = new DALCCM().InsertTemplate(model);
        //        return new BLObject<string>(TemplateId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdmin::InsertTemplate", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}


        public BLObject<List<ICDGroupLookupModel>> LookupICDGroup()
        {
            try
            {
                var result = new DALCCM().LookupICDGroup();
                return new BLObject<List<ICDGroupLookupModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::LookupICDGroup", ex);
                return new BLObject<List<ICDGroupLookupModel>>(null, ex.Message);
            }
        }

        public BLObject<DSCCM> FillTemplate(long TemplateId)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new DALCCM().FillTemplate(TemplateId);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::FillTemplate", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteTemplate(string TemplateId)
        {
            try
            {
                TemplateId = new DALCCM().DeleteTemplate(TemplateId);
                return new BLObject<string>(TemplateId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::DeleteTemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        //public BLObject<string> ActiveInActiveTemplate(string templateId, long isactive)
        //{
        //    try
        //    {
        //        templateId = new MDVision.DataAccess.DAL.CCM.DALCCM().ActiveInActiveTemplate(templateId, isactive);
        //        return new BLObject<string>(templateId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroup", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}

        public BLObject<string> ActiveInActiveTemplate(string templateId, long isactive)
        {
            try
            {
                templateId = new DALCCM().ActiveInActiveTemplate(templateId, isactive);
                return new BLObject<string>(templateId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSCCM> LoadSubQuestion(string QuestionId)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new DALCCM().LoadSubQuestion(QuestionId);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::FillTemplate", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> LoadSectionQuestions(string SectionId)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new DALCCM().LoadSectionQuestions(SectionId);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::FillTemplate", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> UpdateTemplate(DSCCM dsCCM)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dsCCM = new DALCCM().UpdateTemplate(dsCCM, dbManager);
                dsCCM.AcceptChanges();
                return new BLObject<DSCCM>(dsCCM);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BBLCCM::UpdateTemplates", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<DSCCM> UpdateQuestion(DSCCM dsCCM)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dsCCM = new DALCCM().UpdateQuestion(dsCCM, dbManager);
                dsCCM.AcceptChanges();
                return new BLObject<DSCCM>(dsCCM);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BBLCCM::UpdateQuestion", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<DSCCM> InsertQuestion(DSCCM dsCCM, long SectionId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM dsSectionQuestions = null;
            try
            {
                dbManager.Open();
                dsCCM = new DALCCM().InsertQuestion(dsCCM, dbManager);
                if (SectionId > 0)
                {
                    foreach (DSCCM.QuestionsRow drQuestion in dsCCM.Tables[dsCCM.Questions.TableName].Rows)
                    {
                        dsSectionQuestions = new DSCCM();
                        DSCCM.SectionQuestionsRow drq = dsSectionQuestions.SectionQuestions.NewSectionQuestionsRow();
                        drq[dsSectionQuestions.SectionQuestions.SectionIdColumn.ColumnName] = SectionId;
                        drq[dsSectionQuestions.SectionQuestions.QuestionIdColumn.ColumnName] = drQuestion[dsCCM.Questions.QuestionIdColumn.ColumnName];
                        dsSectionQuestions.SectionQuestions.AddSectionQuestionsRow(drq);
                    }
                    if (dsSectionQuestions != null && dsSectionQuestions.Tables[dsSectionQuestions.SectionQuestions.TableName].Rows.Count > 0)
                    {
                        //Section Questions
                        dsSectionQuestions = new DALCCM().InsertSectionQuestions(dsSectionQuestions, dbManager);
                    }
                }
                dsCCM.AcceptChanges();
                if (dsSectionQuestions != null)
                {
                    dsSectionQuestions.AcceptChanges();
                    dsCCM.Merge(dsSectionQuestions);
                }
                return new BLObject<DSCCM>(dsCCM);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BBLCCM::InsertQuestion", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<DSCCM> InsertTemplate(DSCCM dsCCM, DSCCM dsSection, DSCCM dsQuestions, DSCCM dsSubQuestions)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM dsTemplateSections = null;
            DSCCM dsSectionQuestions = null;
            try
            {
                dbManager.Open();
                //For Template
                dsCCM = new DALCCM().InsertTemplate(dsCCM, dbManager);
                if (dsSection.Tables[dsSection.Sections.TableName].Rows.Count > 0)
                {
                    //For Sections
                    dsSection = new DALCCM().InsertSections(dsSection, dbManager);
                }
                if (dsQuestions.Tables[dsQuestions.Questions.TableName].Rows.Count > 0)
                {
                    //For Questions
                    dsQuestions = new DALCCM().InsertQuestion(dsQuestions, dbManager);
                }
                foreach (DSCCM.TemplatesRow drTemplate in dsCCM.Templates.Rows)
                {
                    foreach (DSCCM.SectionsRow drSection in dsSection.Tables[dsSection.Sections.TableName].Rows)
                    {
                        dsTemplateSections = new DSCCM();
                        DSCCM.TemplateSectionsRow dr = dsTemplateSections.TemplateSections.NewTemplateSectionsRow();
                        dr[dsTemplateSections.TemplateSections.SectionIdColumn.ColumnName] = drSection[dsSection.Sections.SectionIdColumn.ColumnName];
                        dr[dsTemplateSections.TemplateSections.TemplateIdColumn.ColumnName] = drTemplate[dsCCM.Templates.TemplateIdColumn.ColumnName];
                        dsTemplateSections.TemplateSections.AddTemplateSectionsRow(dr);
                        foreach (DSCCM.QuestionsRow drQuestion in dsQuestions.Tables[dsQuestions.Questions.TableName].Rows)
                        {
                            if (MDVUtility.ToStr(drSection[dsSection.Sections.SectionIdForRefColumn.ColumnName]) == MDVUtility.ToStr(drQuestion[dsQuestions.Questions.SectionIdForRefColumn.ColumnName]))
                            {
                                dsSectionQuestions = new DSCCM();
                                DSCCM.SectionQuestionsRow drq = dsSectionQuestions.SectionQuestions.NewSectionQuestionsRow();
                                drq[dsSectionQuestions.SectionQuestions.SectionIdColumn.ColumnName] = drSection[dsSection.Sections.SectionIdColumn.ColumnName];
                                drq[dsSectionQuestions.SectionQuestions.QuestionIdColumn.ColumnName] = drQuestion[dsQuestions.Questions.QuestionIdColumn.ColumnName];
                                dsSectionQuestions.SectionQuestions.AddSectionQuestionsRow(drq);
                            }
                            foreach (DSCCM.SubQuestionsRow drSubQuestion in dsSubQuestions.Tables[dsSubQuestions.SubQuestions.TableName].Rows)
                            {
                                if (MDVUtility.ToStr(drSubQuestion[dsSubQuestions.SubQuestions.QuestionIdForRefColumn.ColumnName]) == MDVUtility.ToStr(drQuestion[dsQuestions.Questions.QuestionIdForRefColumn.ColumnName]))
                                {
                                    drSubQuestion[dsSubQuestions.SubQuestions.ParentQuestIdColumn.ColumnName] = drQuestion[dsQuestions.Questions.QuestionIdColumn.ColumnName];

                                }
                            }
                        }
                    }
                }
                if (dsTemplateSections != null && dsTemplateSections.Tables[dsTemplateSections.TemplateSections.TableName].Rows.Count > 0)
                {
                    //Template Sections
                    dsTemplateSections = new DALCCM().InsertTemplateSections(dsTemplateSections, dbManager);
                }
                if (dsSectionQuestions != null && dsSectionQuestions.Tables[dsSectionQuestions.SectionQuestions.TableName].Rows.Count > 0)
                {
                    //Section Questions
                    dsSectionQuestions = new DALCCM().InsertSectionQuestions(dsSectionQuestions, dbManager);
                }
                if (dsSubQuestions != null && dsSubQuestions.Tables[dsSubQuestions.SubQuestions.TableName].Rows.Count > 0)
                {
                    //Section Questions
                    dsSubQuestions = new DALCCM().InsertSubQuestions(dsSubQuestions, dbManager);
                }
                dsCCM.AcceptChanges();
                if (dsSectionQuestions != null)
                {
                    dsSectionQuestions.AcceptChanges();
                    dsCCM.Merge(dsSectionQuestions);
                }
                if (dsQuestions != null)
                {
                    dsQuestions.AcceptChanges();
                    dsCCM.Merge(dsQuestions);
                }
                if (dsSubQuestions != null)
                {
                    dsSubQuestions.AcceptChanges();
                    dsCCM.Merge(dsSubQuestions);
                }
                if (dsTemplateSections != null)
                {
                    dsTemplateSections.AcceptChanges();
                    dsCCM.Merge(dsTemplateSections);
                }

                return new BLObject<DSCCM>(dsCCM);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BBLCCM::InsertUpdateTemplates", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<DSCCM> UpdateSection(DSCCM dsCCM)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dsCCM = new DALCCM().UpdateSections(dsCCM, dbManager);
                dsCCM.AcceptChanges();

                return new BLObject<DSCCM>(dsCCM);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BBLCCM::UpdateSection", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<DSCCM> InsertSection(DSCCM dsCCM, long TemplateId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            DSCCM dsTemplateSections = null;
            try
            {
                dbManager.Open();
                //For Template
                dsCCM = new DALCCM().InsertSections(dsCCM, dbManager);

                foreach (DSCCM.SectionsRow drSection in dsCCM.Tables[dsCCM.Sections.TableName].Rows)
                {
                    dsTemplateSections = new DSCCM();
                    DSCCM.TemplateSectionsRow dr = dsTemplateSections.TemplateSections.NewTemplateSectionsRow();
                    dr[dsTemplateSections.TemplateSections.SectionIdColumn.ColumnName] = drSection[dsCCM.Sections.SectionIdColumn.ColumnName];
                    dr[dsTemplateSections.TemplateSections.TemplateIdColumn.ColumnName] = TemplateId;
                    dsTemplateSections.TemplateSections.AddTemplateSectionsRow(dr);
                }
                if (dsTemplateSections != null && dsTemplateSections.Tables[dsTemplateSections.TemplateSections.TableName].Rows.Count > 0)
                {
                    dsTemplateSections = new DALCCM().InsertTemplateSections(dsTemplateSections, dbManager);
                }
                dsCCM.AcceptChanges();
                if (dsTemplateSections != null)
                {
                    dsTemplateSections.AcceptChanges();
                    dsCCM.Merge(dsTemplateSections);
                }

                return new BLObject<DSCCM>(dsCCM);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BBLCCM::InsertSection", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<DSCCM> LoadSection(Int64 SectionId)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new DALCCM().LoadSection(SectionId);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::FillTemplate", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> LoadQuestion(Int64 QuestionId)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new DALCCM().LoadQuestion(QuestionId);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::FillTemplate", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteSection(string SectionId)
        {
            try
            {
                SectionId = new DALCCM().DeleteSection(SectionId);
                return new BLObject<string>(SectionId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::DeleteSection", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteQuestion(string QuestionId)
        {
            try
            {
                QuestionId = new DALCCM().DeleteQuestion(QuestionId);
                return new BLObject<string>(QuestionId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::DeleteQuestion", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion

        #region Health Risk Assessment

        public BLObject<List<HRAssessmentFillModel>> loadHRAssessmentList(HRAssessmentSearchModel model)
        {
            try
            {
                List<HRAssessmentFillModel> result = new DALHealthRiskAssessment().loadHRAssessmentList(model);
                return new BLObject<List<HRAssessmentFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::loadHRAssessmentList", ex);
                return new BLObject<List<HRAssessmentFillModel>>(null, ex.Message);
            }
        }

        public List<HRAssessmentModel> fillHRAssessmentList(long hRAssessmentId)
        {
            try
            {
                List<HRAssessmentModel> result = new DALHealthRiskAssessment().fillHRAssessmentList(hRAssessmentId);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::fillHRAssessmentList", ex);
                throw ex;
            }
        }

        public BLObject<string> updateStatusHRAssessmentList(long hRAssessmentId, string isActive)
        {
            try
            {
                string message = new DALHealthRiskAssessment().updateStatusHRAssessmentList(hRAssessmentId, isActive);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::updateStatusHRAssessmentList", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> deleteHRAssessment(long hRAssessmentId)
        {
            try
            {
                string message = new DALHealthRiskAssessment().deleteHRAssessment(hRAssessmentId);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::deleteHRAssessment", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public long saveHRAssessmentList(HRAssessmentModel model)
        {
            try
            {
                model.HRAssessmentId = new DALHealthRiskAssessment().saveHRAssessmentList(model);
                return (model.HRAssessmentId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::saveHRAssessmentList", ex);
                throw ex;
            }
        }
        public long updateHRAssessmentList(HRAssessmentModel model)
        {
            try
            {
                model.HRAssessmentId = new DALHealthRiskAssessment().updateHRAssessmentList(model);
                return (model.HRAssessmentId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::updateHRAssessmentList", ex);
                throw ex;
            }
        }

        public BLObject<List<HRAssessmentTemptModel>> loadHRAssessmentTemplates()
        {
            try
            {
                List<HRAssessmentTemptModel> result = new DALHealthRiskAssessment().loadHRAssessmentTemplates();
                return new BLObject<List<HRAssessmentTemptModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::loadHRAssessmentTemplates", ex);
                return new BLObject<List<HRAssessmentTemptModel>>(null, ex.Message);
            }
        }
        #endregion

        public BLObject<string> UpdateCCMQuestionOrder(string QuestionIds)
        {
            try
            {
                string message = new DALCCM().UpdateCCMQuestionOrder(QuestionIds);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCM::UpdateCCMQuestionOrder", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
    }
}
