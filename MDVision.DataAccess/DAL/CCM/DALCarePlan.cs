using MDVision.DataAccess.DCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MDVision.Model.CCM.PatientHub;
using System.Data.SqlClient;
using System.Data;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Model.Clinical.Medical.CarePlan;
using MDVision.Model.Clinical.Medication;
using MDVision.Datasets;

namespace MDVision.DataAccess.DAL.CCM
{
    public class DALCarePlan
    {
        #region Constructors
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public DALCarePlan()
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

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region "Stored Procedure Names"
        //-----------------------------------------------------------------------------------------------------

        private const string PROC_CCM_CARE_PLAN_SELECT = "CCM.sp_CarePlanSelect";
        private const string PROC_CCM_CARE_PLAN_UPDATE = "CCM.sp_CarePlanUpdate";
        private const string PROC_CCM_CARE_PLAN_INSERT = "CCM.sp_CarePlanInsert";
        private const string PROC_CCM_CARE_PLAN_DELETE = "CCM.sp_CarePlanDelete";
        private const string PROC_CCM_CARE_PLAN_FILL = "CCM.sp_CarePlanFill";
        private const string PROC_CCM_CARE_PLAN_UPDATE_STATUS = "CCM.sp_CarePlanUpdateStatus";

        private const string PROC_CCM_CARE_PLAN_TEMPT_SELECT = "CCM.sp_CarePlanTemptSelect";

        #region Clinical Care Plan
        private const string PROC_CAREPLAN_SELECT = "Clinical.sp_CarePlanSelect";
        private const string PROC_CAREPLAN_INSERT = "Clinical.sp_CarePlanInsert";
        private const string PROC_CAREPLAN_UPDATE = "Clinical.sp_CarePlanUpdate";
        private const string PROC_CAREPLAN_GOAL_INSERT = "Clinical.sp_CarePlanGoalInsert";
        private const string PROC_CAREPLAN_GOAL_UPDATE = "Clinical.sp_CarePlanGoalUpdate";
        private const string PROC_CAREPLAN_GOAL_SELECT = "Clinical.sp_CarePlanGoalSelect";
        private const string PROC_CAREPLAN_GOAL_DELETE = "Clinical.sp_CarePlanGoalDelete";
        private const string PROC_CAREPLAN_HEALTHCONCERN_SELECT = "Clinical.sp_CarePlan_HealthConcernsSelect";
        private const string PROC_CAREPLAN_HEALTHCONCERN_INSERT = "Clinical.sp_CarePlan_HealthConcernsInsert";
        private const string PROC_CAREPLAN_HEALTHCONCERN_UPDATE = "Clinical.sp_CarePlan_HealthConcernsUpdate";
        private const string PROC_CAREPLAN_HEALTHCONCERN_DELETE = "Clinical.sp_CarePlan_HealthConcernsDelete";
        private const string PROC_CAREPLAN_GOALS_LOOKUP = "Clinical.sp_CarePlanGoalLookup";
        private const string PROC_MEDICATIONS_LOOKUP = "Clinical.sp_MedicationLookup";
        private const string PROC_CAREPLAN_INTERVENTION_INSERT = "Clinical.sp_CarePlan_InterventionsInsert";
        private const string PROC_CAREPLAN_INTERVENTION_UPDATE = "Clinical.sp_CarePlan_InterventionsUpdate";
        private const string PROC_CAREPLAN_INTERVENTION_SELECT = "Clinical.sp_CarePlan_InterventionsSelect";
        private const string PROC_CAREPLAN_INTERVENTION_DELETE = "Clinical.sp_CarePlan_InterventionsDelete";
        private const string PROC_CAREPLAN_INTERVENTION_LOOKUP = "Clinical.sp_CarePlanInterventionLookup";
        private const string PROC_CAREPLAN_OUTCOME_INSERT = "Clinical.sp_CarePlan_OutcomesInsert";
        private const string PROC_CAREPLAN_OUTCOME_UPDATE = "Clinical.sp_CarePlan_OutcomesUpdate";
        private const string PROC_CAREPLAN_OUTCOME_SELECT = "Clinical.sp_CarePlan_OutcomesSelect";
        private const string PROC_CAREPLAN_OUTCOME_DELETE = "Clinical.sp_CarePlan_OutcomesDelete";
        private const string PROC_CAREPLAN_ADD_TONote = "Clinical.sp_CarePlan_AttachToNote";
        private const string PROC_CAREPLAN_DETACH_FROMNOTE = "Clinical.sp_CarePlan_DetachFromNote";
        



        private const string PROC_CAREPLAN_GOAL_SELECT_CCDA = "Clinical.sp_CarePlanGoalSelect_CCDA";
        #endregion
        #endregion
        #region "Parameters"
        //-----------------------------------------------------------------------------------------------------

        private const string PARM_CAREPLAN_ID = "@CarePlanId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_ENROLLMENT_INFO_ID = "@EnrollmentInfoId";
        private const string PARM_CARE_PLAN_TEMPLATE_ID = "@CarePlanTemptId";
        private const string PARM_CARE_PLAN_HTML = "@CarePlanHTML";
        private const string PARM_CARE_PLAN_XML = "@CarePlanXML";
        private const string PARM_CARE_PLAN_NOTESID = "@NotesId";
        private const string PARM_NOTES_ID = "@NotesId";


        #region Clincal Care Plan

        private const string PARM_CAREPLAN_COMMENTS = "@Comments";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_CARETEAM_ID = "@CareTeamId";
        private const string PARM_GOAL_ID = "@GoalId";
        private const string PARM_ICD9_CODE = "@ICD9Code";
        private const string PARM_ICD9_CODE_DESCRIPTION = "@ICD9CodeDescription";
        private const string PARM_ICD10_CODE = "@ICD10Code";
        private const string PARM_ICD10_CODE_DESCRIPTION = "@ICD10CodeDescription";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_CPTCODE_ID = "@CPTCode";
        private const string PARM_CPTCode_Description = "@CPTDescription";
        private const string PARM_CPTSNOMED_ID = "@CPTSNOMEDID";
        private const string PARM_CPTSNOMED_DESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_GOAL_VALUE = "@Value";
        private const string PARM_GOAL_DATE = "@Date";
        private const string PARM_GOAL_STATUS = "@Status";
        private const string PARM_PATIENT_PRIORITY = "@PatientPriority";
        private const string PARM_PROVIDER_PRIORITY = "@ProviderPriority";

        private const string PARM_HEALTHCONCERN_ID = "@HealthConcernId";

        private const string PARM_CONCERN_ICD9_CODE = "@Concerns_ICD9Code";
        private const string PARM_CONCERN_ICD9_CODE_DESCRIPTION = "@Concerns_ICD9CodeDescription";
        private const string PARM_CONCERN_ICD10_CODE = "@Concerns_ICD10Code";
        private const string PARM_CONCERN_ICD10_CODE_DESCRIPTION = "@Concerns_ICD10CodeDescription";
        private const string PARM_CONCERN_SNOMED_ID = "@Concerns_SNOMEDID";
        private const string PARM_CONCERN_SNOMED_DESCRIPTION = "@Concerns_SNOMEDDescription";
        private const string PARM_CONCERN_DATE = "@Concerns_Date";
        private const string PARM_CONCERN_STATUS = "@Concerns_Status";
        private const string PARM_CONCERN_COMMENTS = "@Concerns_Comments";

        private const string PARM_OBSERVATION_ICD9_CODE = "@Observation_ICD9Code";
        private const string PARM_OBSERVATION_ICD9_CODE_DESCRIPTION = "@Observation_ICD9CodeDescription";
        private const string PARM_OBSERVATION_ICD10_CODE = "@Observation_ICD10Code";
        private const string PARM_OBSERVATION_ICD10_CODE_DESCRIPTION = "@Observation_ICD10CodeDescription";
        private const string PARM_OBSERVATION_SNOMED_ID = "@Observation_SNOMEDID";
        private const string PARM_OBSERVATION_SNOMED_DESCRIPTION = "@Observation_SNOMEDDescription";
        private const string PARM_OBSERVATION_DATE = "@Observation_Date";
        private const string PARM_OBSERVATION_STATUS = "@Observation_Status";
        private const string PARM_OBSERVATION_COMMENTS = "@Observation_Comments";
        private const string PARM_OBSERVATION_PATIENT_PRIORITY = "@Observation_PatientPriority";
        private const string PARM_OBSERVATION_PROVIDER_PRIORITY = "@Observation_ProviderPriority";

        private const string PARM_RISK_ICD9_CODE = "@Risk_ICD9Code";
        private const string PARM_RISK_ICD9_CODE_DESCRIPTION = "@Risk_ICD9CodeDescription";
        private const string PARM_RISK_ICD10_CODE = "@Risk_ICD10Code";
        private const string PARM_RISK_ICD10_CODE_DESCRIPTION = "@Risk_ICD10CodeDescription";
        private const string PARM_RISK_SNOMED_ID = "@Risk_SNOMEDID";
        private const string PARM_RISK_SNOMED_DESCRIPTION = "@Risk_SNOMEDDescription";
        private const string PARM_RISK_DATE = "@Risk_Date";
        private const string PARM_RISK_STATUS = "@Risk_Status";
        private const string PARM_RISK_COMMENTS = "@Risk_Comments";
        private const string PARM_RISK_PATIENT_PRIORITY = "@Risk_PatientPriority";
        private const string PARM_RISK_PROVIDER_PRIORITY = "@Risk_ProviderPriority";

        private const string PARM_INTERVENTION_ID = "@InterventionId";
        private const string PARM_INTERVENTION_DATE = "@Date";
        private const string PARM_INTERVENTION_STATUS = "@Status";
        private const string PARM_INTERVENTION_COMMENTS = "@Comments";
        private const string PARM_INTERVENTION_GOALIDS = "@GoalIds";
        private const string PARM_INTERVENTION_MEDICATIONIDS = "@MedicationIds";

        private const string PARM_OUTCOME_ID = "@OutcomesId";
        private const string PARM_OUTCOME_DATE = "@Date";
        private const string PARM_OUTCOME_VALUE = "@Outcome";
        private const string PARM_OUTCOME_COMMENTS = "@Comments";
        private const string PARM_OUTCOME_GOALIDS = "@GoalIds";
        private const string PARM_OUTCOME_INTERVENTIONIDS = "@InterventionIds";
        #endregion
        #endregion
        #region CRUD Methods
        public List<CarePlanFillModel> loadCarePlanList(CarePlanSearchModel model)
        {
            //  List<CarePlanFillModel> listModel = new List<CarePlanFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //   SqlDataReader reader = null;
            try
            {
                //dbManager.Open();
                //dbManager.CreateParameters(4);
                List<SqlParameter> parameters = new List<SqlParameter>();
                #region paramaters
                if (model.EnrollmentInfoId <= 0)
                {
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, DBNull.Value);
                    //parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                    // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId));
                }

                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                //  parameters.Add(new SqlParameter(PARM_IS_ACTIVE, model.IsActive));

                if (model.PageNumber <= 0)
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, DBNull.Value);
                    //parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, model.PageNumber);
                    //parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, model.PageNumber));
                }
                if (model.RowsPerPage <= 0)
                {
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, DBNull.Value);
                    //parameters.Add(new SqlParameter(PARM_ROWS_PER_PAGE, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, model.RowsPerPage);
                    //parameters.Add(new SqlParameter(PARM_ROWS_PER_PAGE, model.RowsPerPage));
                }

                #endregion

                return dbManager.ExecuteReaders<CarePlanFillModel>(PROC_CCM_CARE_PLAN_SELECT);
                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_SELECT);
                //CarePlanFillModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new CarePlanFillModel();
                //    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                //    modelFill.CarePlanId = Convert.ToInt64(reader["CarePlanId"]);
                //    modelFill.RecordCount = Convert.ToInt64(reader["RecordCount"]);
                //    modelFill.Name = MDVUtility.CheckStringNull(reader["Name"]);
                //    modelFill.Description = MDVUtility.CheckStringNull(reader["Description"]);
                //    modelFill.ModifiedOn = MDVUtility.CheckDateTimeNull(reader["ModifiedOn"]);

                //    modelFill.Createdby = MDVUtility.CheckStringNull(reader["Createdby"]);
                //    modelFill.ModifiedBy = MDVUtility.CheckStringNull(reader["ModifiedBy"]);
                //    modelFill.IsActive = MDVUtility.CheckBooleanNull(reader["IsActive"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::loadCarePlanList", PROC_CCM_CARE_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                //  dbManager.Dispose();
            }
        }

        public List<CarePlanModel> fillCarePlanList(long carePlanId)
        {
            // List<CarePlanModel> listModel = new List<CarePlanModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //  SqlDataReader reader = null;
            try
            {
                //dbManager.Open();
                //dbManager.CreateParameters(1);

                // List<SqlParameter> parameters = new List<SqlParameter>();
                #region paramaters

                dbManager.AddParameters(PARM_CAREPLAN_ID, carePlanId);
                //  parameters.Add(new SqlParameter(PARM_CAREPLAN_ID, carePlanId));

                #endregion

                return dbManager.ExecuteReaders<CarePlanModel>(PROC_CCM_CARE_PLAN_FILL);
                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_FILL);
                //CarePlanModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new CarePlanModel();
                //    modelFill.CarePlanTemplateId = MDVUtility.ToInt64(reader["CarePlanTemplateId"]);
                //    modelFill.CarePlanId = MDVUtility.ToInt64(reader["CarePlanId"]);
                //    modelFill.EnrollmentInfoId = MDVUtility.ToInt64(reader["EnrollmentInfoId"]);
                //    modelFill.CarePlanHTML = MDVUtility.CheckStringNull(reader["CarePlanHTML"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::fillCarePlanList", PROC_CCM_CARE_PLAN_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public long saveCarePlanList(CarePlanModel model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                long returnValue = -1;
                dbManager.Open();
                dbManager.CreateParameters(9);

                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_CAREPLAN_ID, model.CarePlanId);
                dbManager.AddParameters(i++, PARM_CARE_PLAN_TEMPLATE_ID, model.CarePlanTemplateId);
                dbManager.AddParameters(i++, PARM_CARE_PLAN_HTML, model.CarePlanHTML);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                object carePanId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_INSERT);
                if (carePanId != null)
                {
                    returnValue = MDVUtility.ToInt64(carePanId.ToString());
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::saveCarePlanList", PROC_CCM_CARE_PLAN_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public long updateCarePlanList(CarePlanModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                long returnValue = -1;
                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_CAREPLAN_ID, model.CarePlanId);
                dbManager.AddParameters(i++, PARM_CARE_PLAN_TEMPLATE_ID, model.CarePlanTemplateId);
                dbManager.AddParameters(i++, PARM_CARE_PLAN_HTML, model.CarePlanHTML);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                object carePanId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_UPDATE);
                if (carePanId != null)
                {
                    returnValue = MDVUtility.ToInt64(carePanId.ToString());
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::updateCarePlanList", PROC_CCM_CARE_PLAN_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateStatusCarePlanList(long carePlanId, string isActive)
        {
            int returnVal = 0;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_CAREPLAN_ID, carePlanId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, isActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_UPDATE_STATUS);

                if (returnVal < 0)
                    throw new Exception("Cannot Update Status");

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::updateStatusCarePlanList", PROC_CCM_CARE_PLAN_UPDATE_STATUS, ex);
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

        public string deleteCarePlan(long carePlanId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CAREPLAN_ID, carePlanId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::deleteCarePlan", PROC_CCM_CARE_PLAN_DELETE, ex);
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

        #region Lookup
        public List<CarePlanTemptModel> loadCarePlanTemplates()
        {
            List<CarePlanTemptModel> listModel = new List<CarePlanTemptModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                // List<SqlParameter> parameters = new List<SqlParameter>();
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                #region paramaters

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(PARM_ENTITY_ID, DBNull.Value);
                // parameters.Add(new SqlParameter(PARM_ENTITY_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
                //parameters.Add(new SqlParameter(PARM_ENTITY_ID, MDVSession.Current.EntityId));
                #endregion
                return dbManager.ExecuteReaders<CarePlanTemptModel>(PROC_CCM_CARE_PLAN_TEMPT_SELECT);

                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_CARE_PLAN_TEMPT_SELECT);
                //CarePlanTemptModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new CarePlanTemptModel();
                //    modelFill.TemplateId = Convert.ToInt64(reader["TemplateId"]);
                //    modelFill.TemplateName = MDVUtility.CheckStringNull(reader["TemplateName"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::loadCarePlanList", PROC_CCM_CARE_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Clinical Care Plan

        private void createCarePlanParameters(IDBManager dbManager, CarePlanClinicalModel model, bool isInsert)
        {

            dbManager.Open();
            if (isInsert)
            {
                dbManager.CreateParameters(10);
                dbManager.AddParameters(PARM_CAREPLAN_ID, model.CarePlanId, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(8);
                dbManager.AddParameters(PARM_CAREPLAN_ID, model.CarePlanId, DbType.Int64);
            }
            dbManager.AddParameters(PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(PARM_CAREPLAN_COMMENTS, model.Comments);
            if (model.ProviderId > 0)
            {
                dbManager.AddParameters(PARM_PROVIDER_ID, model.ProviderId);
            }
            else
            {
                dbManager.AddParameters(PARM_PROVIDER_ID, null);
            }
            if (model.CareTeamId > 0)
            {
                dbManager.AddParameters(PARM_CARETEAM_ID, model.CareTeamId);
            }
            else
            {
                dbManager.AddParameters(PARM_CARETEAM_ID, null);
            }
            dbManager.AddParameters(PARM_IS_ACTIVE, true);
            if (isInsert)
            {
                dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
            }
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
        }
        private void createCarePlanGoalInsertParameters(IDBManager dbManager, CarePlanGoalsModel model)
        {
            int i = 0;
            dbManager.Open();

            dbManager.CreateParameters(22);
            dbManager.AddParameters(i++, PARM_GOAL_ID, model.GoalId, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(i++, PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(i++, PARM_ICD9_CODE, model.ICD9Code);
            dbManager.AddParameters(i++, PARM_ICD9_CODE_DESCRIPTION, model.ICD9CodeDescription);
            dbManager.AddParameters(i++, PARM_ICD10_CODE, model.ICD10Code);
            dbManager.AddParameters(i++, PARM_ICD10_CODE_DESCRIPTION, model.ICD10CodeDescription);
            dbManager.AddParameters(i++, PARM_SNOMED_ID, model.SNOMEDID);
            dbManager.AddParameters(i++, PARM_SNOMED_DESCRIPTION, model.SNOMEDDescription);
            dbManager.AddParameters(i++, PARM_CPTCODE_ID, model.CPTCode);
            dbManager.AddParameters(i++, PARM_CPTCode_Description, model.CPTDescription);
            dbManager.AddParameters(i++, PARM_CPTSNOMED_ID, model.CPTSNOMEDID);
            dbManager.AddParameters(i++, PARM_CPTSNOMED_DESCRIPTION, model.CPTSNOMEDDescription);
            dbManager.AddParameters(i++, PARM_CAREPLAN_COMMENTS, model.GoalComments);
            if (!string.IsNullOrEmpty(model.GoalValue))
            {
                dbManager.AddParameters(i++, PARM_GOAL_VALUE, model.GoalValue);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_GOAL_VALUE, null);
            }
            if (!string.IsNullOrEmpty(model.GoalDate))
            {
                dbManager.AddParameters(i++, PARM_GOAL_DATE, model.GoalDate);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_GOAL_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.GoalStatus))
            {
                dbManager.AddParameters(i++, PARM_GOAL_STATUS, model.GoalStatus);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_GOAL_STATUS, null);
            }
            if (!string.IsNullOrEmpty(model.PatientPriority))
            {
                dbManager.AddParameters(i++, PARM_PATIENT_PRIORITY, model.PatientPriority);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_PATIENT_PRIORITY, null);
            }
            if (!string.IsNullOrEmpty(model.ProviderPriority))
            {
                dbManager.AddParameters(i++, PARM_PROVIDER_PRIORITY, model.ProviderPriority);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_PROVIDER_PRIORITY, null);
            }
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
        }
        private void createCarePlanGoalUpdateParameters(IDBManager dbManager, CarePlanGoalsModel model)
        {

            dbManager.Open();

            dbManager.CreateParameters(10);
            dbManager.AddParameters(PARM_GOAL_ID, model.GoalId, DbType.Int64);
            dbManager.AddParameters(PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(PARM_CAREPLAN_COMMENTS, model.GoalComments);
            if (!string.IsNullOrEmpty(model.GoalValue))
            {
                dbManager.AddParameters(PARM_GOAL_VALUE, model.GoalValue);
            }
            else
            {
                dbManager.AddParameters(PARM_GOAL_VALUE, null);
            }
            if (!string.IsNullOrEmpty(model.GoalDate))
            {
                dbManager.AddParameters(PARM_GOAL_DATE, model.GoalDate);
            }
            else
            {
                dbManager.AddParameters(PARM_GOAL_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.GoalStatus))
            {
                dbManager.AddParameters(PARM_GOAL_STATUS, model.GoalStatus);
            }
            else
            {
                dbManager.AddParameters(PARM_GOAL_STATUS, null);
            }
            if (!string.IsNullOrEmpty(model.PatientPriority))
            {
                dbManager.AddParameters(PARM_PATIENT_PRIORITY, model.PatientPriority);
            }
            else
            {
                dbManager.AddParameters(PARM_PATIENT_PRIORITY, null);
            }
            if (!string.IsNullOrEmpty(model.ProviderPriority))
            {
                dbManager.AddParameters(PARM_PROVIDER_PRIORITY, model.ProviderPriority);
            }
            else
            {
                dbManager.AddParameters(PARM_PROVIDER_PRIORITY, null);
            }
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
        }
        private void createHealthConcernInsertParameters(IDBManager dbManager, CarePlanHealthConcernsModel model, bool isInsert)
        {
            int i = 0;
            dbManager.Open();
            dbManager.CreateParameters(36);
            if (isInsert)
            {
                dbManager.AddParameters(i++, PARM_HEALTHCONCERN_ID, model.HealthConcernsId, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_HEALTHCONCERN_ID, model.HealthConcernsId);
            }

            dbManager.AddParameters(i++, PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(i++, PARM_CONCERN_COMMENTS, model.ConcernsComments);           
            if (!string.IsNullOrEmpty(model.ConcernsDate))
            {
                dbManager.AddParameters(i++, PARM_CONCERN_DATE, model.ConcernsDate);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_CONCERN_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.ConcernsStatus))
            {
                dbManager.AddParameters(i++, PARM_CONCERN_STATUS, model.ConcernsStatus);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_CONCERN_STATUS, null);
            }
            dbManager.AddParameters(i++, PARM_OBSERVATION_COMMENTS, model.ObservationComments);            
            if (!string.IsNullOrEmpty(model.ObservationDate))
            {
                dbManager.AddParameters(i++, PARM_OBSERVATION_DATE, model.ObservationDate);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_OBSERVATION_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.ObservationPatientPriority))
            {
                dbManager.AddParameters(i++, PARM_OBSERVATION_PATIENT_PRIORITY, model.ObservationPatientPriority);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_OBSERVATION_PATIENT_PRIORITY, null);
            }
            if (!string.IsNullOrEmpty(model.ObservationProviderPriority))
            {
                dbManager.AddParameters(i++, PARM_OBSERVATION_PROVIDER_PRIORITY, model.ObservationProviderPriority);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_OBSERVATION_PROVIDER_PRIORITY, null);
            }

            dbManager.AddParameters(i++, PARM_RISK_COMMENTS, model.RiskComments);            
            if (!string.IsNullOrEmpty(model.RiskDate))
            {
                dbManager.AddParameters(i++, PARM_RISK_DATE, model.RiskDate);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_RISK_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.RiskStatus))
            {
                dbManager.AddParameters(i++, PARM_RISK_STATUS, model.RiskStatus);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_RISK_STATUS, null);
            }

            if (!string.IsNullOrEmpty(model.RiskPatientPriority))
            {
                dbManager.AddParameters(i++, PARM_RISK_PATIENT_PRIORITY, model.RiskPatientPriority);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_RISK_PATIENT_PRIORITY, null);
            }
            if (!string.IsNullOrEmpty(model.RiskProviderPriority))
            {
                dbManager.AddParameters(i++, PARM_RISK_PROVIDER_PRIORITY, model.RiskProviderPriority);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_RISK_PROVIDER_PRIORITY, null);
            }

            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);

            dbManager.AddParameters(i++, PARM_CONCERN_ICD9_CODE, model.Concerns_ICD9Code);
            dbManager.AddParameters(i++, PARM_CONCERN_ICD9_CODE_DESCRIPTION, model.Concerns_ICD9CodeDescription);
            dbManager.AddParameters(i++, PARM_CONCERN_ICD10_CODE, model.Concerns_ICD10Code);
            dbManager.AddParameters(i++, PARM_CONCERN_ICD10_CODE_DESCRIPTION, model.Concerns_ICD10CodeDescription);
            dbManager.AddParameters(i++, PARM_CONCERN_SNOMED_ID, model.Concerns_SNOMEDID);
            dbManager.AddParameters(i++, PARM_CONCERN_SNOMED_DESCRIPTION, model.Concerns_SNOMEDDescription);

            dbManager.AddParameters(i++, PARM_OBSERVATION_ICD9_CODE, model.Observation_ICD9Code);
            dbManager.AddParameters(i++, PARM_OBSERVATION_ICD9_CODE_DESCRIPTION, model.Observation_ICD9CodeDescription);
            dbManager.AddParameters(i++, PARM_OBSERVATION_ICD10_CODE, model.Observation_ICD10Code);
            dbManager.AddParameters(i++, PARM_OBSERVATION_ICD10_CODE_DESCRIPTION, model.Observation_ICD10CodeDescription);
            dbManager.AddParameters(i++, PARM_OBSERVATION_SNOMED_ID, model.Observation_SNOMEDID);
            dbManager.AddParameters(i++, PARM_OBSERVATION_SNOMED_DESCRIPTION, model.Observation_SNOMEDDescription);

            dbManager.AddParameters(i++, PARM_RISK_ICD9_CODE, model.Risk_ICD9Code);
            dbManager.AddParameters(i++, PARM_RISK_ICD9_CODE_DESCRIPTION, model.Risk_ICD9CodeDescription);
            dbManager.AddParameters(i++, PARM_RISK_ICD10_CODE, model.Risk_ICD10Code);
            dbManager.AddParameters(i++, PARM_RISK_ICD10_CODE_DESCRIPTION, model.Risk_ICD10CodeDescription);
            dbManager.AddParameters(i++, PARM_RISK_SNOMED_ID, model.Risk_SNOMEDID);
            dbManager.AddParameters(i++, PARM_RISK_SNOMED_DESCRIPTION, model.Risk_SNOMEDDescription);
        }
        private void createInterventionsInsertParameters(IDBManager dbManager, CarePlanInterventionsModel model)
        {
            int i = 0;
            dbManager.Open();

            dbManager.CreateParameters(17);
            dbManager.AddParameters(i++, PARM_INTERVENTION_ID, model.InterventionId, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(i++, PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(i++, PARM_CAREPLAN_COMMENTS, model.InterventionComments);
            if (!string.IsNullOrEmpty(model.InterventionDate))
            {
                dbManager.AddParameters(i++, PARM_INTERVENTION_DATE, model.InterventionDate);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_INTERVENTION_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.InterventionStatus))
            {
                dbManager.AddParameters(i++, PARM_INTERVENTION_STATUS, model.InterventionStatus);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_INTERVENTION_STATUS, null);
            }
            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_ICD9_CODE, model.ICD9Code);
            dbManager.AddParameters(i++, PARM_ICD9_CODE_DESCRIPTION, model.ICD9CodeDescription);
            dbManager.AddParameters(i++, PARM_ICD10_CODE, model.ICD10Code);
            dbManager.AddParameters(i++, PARM_ICD10_CODE_DESCRIPTION, model.ICD10CodeDescription);
            dbManager.AddParameters(i++, PARM_SNOMED_ID, model.SNOMEDID);
            dbManager.AddParameters(i++, PARM_SNOMED_DESCRIPTION, model.SNOMEDDescription);
            dbManager.AddParameters(i++, PARM_INTERVENTION_GOALIDS, model.GoalIds);
            dbManager.AddParameters(i++, PARM_INTERVENTION_MEDICATIONIDS, model.MedicationIds);
        }
        private void createInterventionsUpdateParameters(IDBManager dbManager, CarePlanInterventionsModel model)
        {

            dbManager.Open();

            dbManager.CreateParameters(9);
            dbManager.AddParameters(PARM_INTERVENTION_ID, model.InterventionId, DbType.Int64);
            dbManager.AddParameters(PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(PARM_INTERVENTION_COMMENTS, model.InterventionComments);
            if (!string.IsNullOrEmpty(model.InterventionDate))
            {
                dbManager.AddParameters(PARM_INTERVENTION_DATE, model.InterventionDate);
            }
            else
            {
                dbManager.AddParameters(PARM_INTERVENTION_DATE, null);
            }
            if (!string.IsNullOrEmpty(model.InterventionStatus))
            {
                dbManager.AddParameters(PARM_INTERVENTION_STATUS, model.InterventionStatus);
            }
            else
            {
                dbManager.AddParameters(PARM_INTERVENTION_STATUS, null);
            }
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_INTERVENTION_GOALIDS, model.GoalIds);
            dbManager.AddParameters(PARM_INTERVENTION_MEDICATIONIDS, model.MedicationIds);
        }
        private void createOutcomesInsertParameters(IDBManager dbManager, CarePlanOutcomesModel model)
        {
            int i = 0;
            dbManager.Open();

            dbManager.CreateParameters(17);
            dbManager.AddParameters(i++, PARM_OUTCOME_ID, model.OutcomesId, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(i++, PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(i++, PARM_OUTCOME_COMMENTS, model.OutcomeComments);
            if (!string.IsNullOrEmpty(model.OutcomeValue))
            {
                dbManager.AddParameters(i++, PARM_OUTCOME_VALUE, model.OutcomeValue);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_OUTCOME_VALUE, null);
            }
            if (!string.IsNullOrEmpty(model.OutcomeDate))
            {
                dbManager.AddParameters(i++, PARM_OUTCOME_DATE, model.OutcomeDate);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_OUTCOME_DATE, null);
            }

            dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(i++, PARM_ICD9_CODE, model.ICD9Code);
            dbManager.AddParameters(i++, PARM_ICD9_CODE_DESCRIPTION, model.ICD9CodeDescription);
            dbManager.AddParameters(i++, PARM_ICD10_CODE, model.ICD10Code);
            dbManager.AddParameters(i++, PARM_ICD10_CODE_DESCRIPTION, model.ICD10CodeDescription);
            dbManager.AddParameters(i++, PARM_SNOMED_ID, model.SNOMEDID);
            dbManager.AddParameters(i++, PARM_SNOMED_DESCRIPTION, model.SNOMEDDescription);
            dbManager.AddParameters(i++, PARM_OUTCOME_GOALIDS, model.GoalIds);
            dbManager.AddParameters(i++, PARM_OUTCOME_INTERVENTIONIDS, model.InterventionIds);
        }
        private void createOutcomesUpdateParameters(IDBManager dbManager, CarePlanOutcomesModel model)
        {

            dbManager.Open();

            dbManager.CreateParameters(9);
            dbManager.AddParameters(PARM_OUTCOME_ID, model.OutcomesId, DbType.Int64);
            dbManager.AddParameters(PARM_CAREPLAN_ID, model.CarePlanId);
            dbManager.AddParameters(PARM_OUTCOME_COMMENTS, model.OutcomeComments);
            if (!string.IsNullOrEmpty(model.OutcomeValue))
            {
                dbManager.AddParameters(PARM_OUTCOME_VALUE, model.OutcomeValue);
            }
            else
            {
                dbManager.AddParameters(PARM_OUTCOME_VALUE, null);
            }
            if (!string.IsNullOrEmpty(model.OutcomeDate))
            {
                dbManager.AddParameters(PARM_OUTCOME_DATE, model.OutcomeDate);
            }
            else
            {
                dbManager.AddParameters(PARM_OUTCOME_DATE, null);
            }

            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_OUTCOME_GOALIDS, model.GoalIds);
            dbManager.AddParameters(PARM_OUTCOME_INTERVENTIONIDS, model.InterventionIds);
        }
        public List<CarePlanClinicalModel> FillCarePlan(string CarePlanId, long patientId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                #region paramaters               

                if (MDVUtility.ToLong(CarePlanId) <= 0)
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, MDVUtility.ToLong(CarePlanId));

                }
                if (patientId <= 0)
                {
                    dbManager.AddParameters(PARM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_PATIENT_ID, patientId);
                }
                #endregion

                return dbManager.ExecuteReaders<CarePlanClinicalModel>(PROC_CAREPLAN_SELECT);

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::FillCarePlan", PROC_CAREPLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertUpdateCarePlan(CarePlanClinicalModel model)
        {
            dynamic carePlanId;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                if (MDVUtility.ToInt64(model.CarePlanId) > 0)
                {
                    List<CarePlanClinicalModel> oldModel = FillCarePlan(model.CarePlanId, model.PatientId);

                    createCarePlanParameters(dbManager, model, false);
                    carePlanId = dbManager.ExecuteScalar(PROC_CAREPLAN_UPDATE);


                    if (carePlanId > 0)
                    {
                        string auditXML = new DBActivityAudit().GetModelsDBAudit(model, oldModel[0], MDVUtility.ToStr(carePlanId), MDVUtility.ToLong(carePlanId), "CarePlanId", "Update", MDVUtility.ToStr(model.PatientId), "CarePlan");
                        if (!string.IsNullOrEmpty(auditXML))
                        {
                            new DBActivityAudit().InsertModelsDBAudit(auditXML);
                        }
                    }
                }
                else
                {
                    createCarePlanParameters(dbManager, model, true);
                    carePlanId = dbManager.ExecuteScalar(PROC_CAREPLAN_INSERT);
                }
                return MDVUtility.ToStr(carePlanId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::InsertUpdateCarePlan", PROC_CAREPLAN_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CarePlanGoalsModel> CarePlanGoalInsert(CarePlanGoalsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<CarePlanGoalsModel> listGoals = new List<CarePlanGoalsModel>();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                createCarePlanGoalInsertParameters(dbManager, model);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CAREPLAN_GOAL_INSERT);
                CarePlanGoalsModel goalsModel = null;
                while (reader.HasRows)
                {
                    reader.NextResult();

                    while (reader.Read())
                    {
                        goalsModel = new CarePlanGoalsModel();

                        goalsModel.GoalId = !String.IsNullOrEmpty(reader["GoalId"].ToString()) ? reader["GoalId"].ToString() : "";
                        listGoals.Add(goalsModel);
                    }
                }

                if (MDVUtility.ToLong(goalsModel.GoalId) > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(model, null, goalsModel.GoalId, MDVUtility.ToLong(model.CarePlanId), "GoalId", "Insert", MDVUtility.ToStr(model.PatientId), "CarePlan_Goals");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return listGoals;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::CarePlanGoalInsert", "", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string CarePlanGoalUpdate(CarePlanGoalsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            dynamic carePlanGoalId;
            try
            {
                List<CarePlanGoalsModel> oldGoal = FillCarePlanGoal(model.CarePlanId, model.GoalId, "","",MDVUtility.ToInt64(model.ProviderId));

                dbManager.Open();
                createCarePlanGoalUpdateParameters(dbManager, model);
                carePlanGoalId = dbManager.ExecuteScalar(PROC_CAREPLAN_GOAL_UPDATE);


                if (carePlanGoalId > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(model, oldGoal[0], MDVUtility.ToStr(carePlanGoalId), MDVUtility.ToLong(model.CarePlanId), "GoalId", "Update", MDVUtility.ToStr(model.PatientId), "CarePlan_Goals");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return MDVUtility.ToStr(carePlanGoalId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::CarePlanGoalUpdate", "", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteCarePlanGoal(string CarePlanId, long GoalId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<CarePlanGoalsModel> goal = FillCarePlanGoal(CarePlanId, MDVUtility.ToStr(GoalId), "","", 0);

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_GOAL_ID, GoalId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CAREPLAN_GOAL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                if (GoalId > 0 && goal.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(goal[0], null, MDVUtility.ToStr(GoalId), MDVUtility.ToLong(CarePlanId), "GoalId", "Delete", goal[0].PatientId, "CarePlan_Goals");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }


                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeletePlanGoal", PROC_CAREPLAN_GOAL_DELETE, ex);
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
        public List<CarePlanGoalsModel> FillCarePlanGoal(string CarePlanId, string GoalId,string NotesId, string action, long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                if (MDVUtility.ToLong(GoalId) <= 0)
                {
                    dbManager.AddParameters(PARM_GOAL_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_GOAL_ID, MDVUtility.ToLong(GoalId));
                }

                if (MDVUtility.ToLong(CarePlanId) <= 0)
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, MDVUtility.ToLong(CarePlanId));

                }

                if (MDVUtility.ToLong(NotesId) <= 0)
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, MDVUtility.ToLong(NotesId));

                }
                if(ProviderId == 0)
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                }
                #endregion
                List<CarePlanGoalsModel> result = dbManager.ExecuteReaders<CarePlanGoalsModel>(PROC_CAREPLAN_GOAL_SELECT);

                if (action == "View" && MDVUtility.ToLong(GoalId) > 0 && result.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(result[0], null, GoalId, MDVUtility.ToLong(CarePlanId), "GoalId", action, result[0].PatientId, "CarePlan_Goals");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::FillCarePlanGoal", PROC_CAREPLAN_GOAL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CarePlanHealthConcernsModel> InsertUpdateHealthConcern(CarePlanHealthConcernsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<CarePlanHealthConcernsModel> concern = new List<CarePlanHealthConcernsModel>();
            SqlDataReader reader = null;
            List<CarePlanHealthConcernsModel> oldConcern = null;
            try
            {
                dbManager.Open();
                if (MDVUtility.ToInt64(model.HealthConcernsId) > 0)
                {
                    oldConcern = FillCarePlanHealthConcern(model.CarePlanId, model.HealthConcernsId,"", "");

                    createHealthConcernInsertParameters(dbManager, model, false);
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CAREPLAN_HEALTHCONCERN_UPDATE);

                }
                else
                {
                    createHealthConcernInsertParameters(dbManager, model, true);
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CAREPLAN_HEALTHCONCERN_INSERT);
                }

                CarePlanHealthConcernsModel concernsModel = null;
                while (reader.HasRows)
                {
                    reader.NextResult();
                    reader.NextResult();
                    reader.NextResult();

                    while (reader.Read())
                    {
                        concernsModel = new CarePlanHealthConcernsModel();

                        concernsModel.HealthConcernsId = !String.IsNullOrEmpty(reader["HealthConcernId"].ToString()) ? reader["HealthConcernId"].ToString() : "";
                        concern.Add(concernsModel);
                    }
                }

                #region DB Audit
                string auditXML = string.Empty;
                if (MDVUtility.ToLong(model.HealthConcernsId) > 0 && oldConcern != null && oldConcern.Count > 0)
                {
                    auditXML = new DBActivityAudit().GetModelsDBAudit(model, oldConcern[0], concernsModel.HealthConcernsId, MDVUtility.ToLong(model.CarePlanId), "HealthConcernId", "Update", MDVUtility.ToStr(model.PatientId), "CarePlan_HealthConcerns");
                }
                else
                {
                    if (concernsModel != null && MDVUtility.ToLong(concernsModel.HealthConcernsId) > 0)
                    {
                        auditXML = new DBActivityAudit().GetModelsDBAudit(model, null, concernsModel.HealthConcernsId, MDVUtility.ToLong(model.CarePlanId), "HealthConcernId", "Insert", MDVUtility.ToStr(model.PatientId), "CarePlan_HealthConcerns");
                    }
                }
                if (!string.IsNullOrEmpty(auditXML))
                {
                    new DBActivityAudit().InsertModelsDBAudit(auditXML);
                }
                #endregion

                return concern;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::InsertUpdateHealthConcern", PROC_CAREPLAN_HEALTHCONCERN_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CarePlanHealthConcernsModel> FillCarePlanHealthConcern(string CarePlanId, string HealthConcernId,string NotesId, string action)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                if (MDVUtility.ToLong(HealthConcernId) <= 0)
                {
                    dbManager.AddParameters(PARM_HEALTHCONCERN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_HEALTHCONCERN_ID, MDVUtility.ToLong(HealthConcernId));
                }

                if (MDVUtility.ToLong(CarePlanId) <= 0)
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, MDVUtility.ToLong(CarePlanId));

                }
                if (MDVUtility.ToLong(NotesId) <= 0)
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, MDVUtility.ToLong(NotesId));

                }
                #endregion

                List<CarePlanHealthConcernsModel> result = dbManager.ExecuteReaders<CarePlanHealthConcernsModel>(PROC_CAREPLAN_HEALTHCONCERN_SELECT);

                if (action == "View" && MDVUtility.ToLong(HealthConcernId) > 0 && result.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(result[0], null, HealthConcernId, MDVUtility.ToLong(CarePlanId), "HealthConcernId", action, result[0].PatientId, "CarePlan_HealthConcerns");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::FillCarePlanHealthConcern", PROC_CAREPLAN_HEALTHCONCERN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteCarePlanHealthConcern(long ConcernId, long CarePlanId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<CarePlanHealthConcernsModel> concern = FillCarePlanHealthConcern(MDVUtility.ToStr(CarePlanId), MDVUtility.ToStr(ConcernId),"", "");

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_HEALTHCONCERN_ID, ConcernId);
                dbManager.AddParameters(1, PARM_CAREPLAN_ID, CarePlanId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CAREPLAN_HEALTHCONCERN_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                if (ConcernId > 0 && concern.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(concern[0], null, MDVUtility.ToStr(ConcernId), MDVUtility.ToLong(CarePlanId), "HealthConcernId", "Delete", concern[0].PatientId, "CarePlan_HealthConcerns");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteCarePlanHealthConcern", PROC_CAREPLAN_HEALTHCONCERN_DELETE, ex);
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

        public List<CarePlanGoalsModel> GetGoalsLookup(long CarePlanId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                dbManager.AddParameters(PARM_CAREPLAN_ID, CarePlanId);
                #endregion

                return dbManager.ExecuteReaders<CarePlanGoalsModel>(PROC_CAREPLAN_GOALS_LOOKUP);

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::GetGoalsLookup", PROC_CAREPLAN_GOALS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<ClinicalMedicationsModel> GetMedicationsLookup(long PatientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                #endregion

                return dbManager.ExecuteReaders<ClinicalMedicationsModel>(PROC_MEDICATIONS_LOOKUP);

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::GetMedicationsLookup", PROC_MEDICATIONS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public CarePlanInterventionsModel CarePlanInterventionInsert(CarePlanInterventionsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            CarePlanInterventionsModel intervention = null;
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                createInterventionsInsertParameters(dbManager, model);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CAREPLAN_INTERVENTION_INSERT);

                while (reader.HasRows)
                {
                    reader.NextResult();

                    while (reader.Read())
                    {
                        intervention = new CarePlanInterventionsModel();
                        intervention.InterventionId = !String.IsNullOrEmpty(reader["InterventionId"].ToString()) ? reader["InterventionId"].ToString() : "";
                    }
                }

                if (intervention != null && MDVUtility.ToLong(intervention.InterventionId) > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(model, null, intervention.InterventionId, MDVUtility.ToLong(model.CarePlanId), "InterventionId", "Insert", MDVUtility.ToStr(model.PatientId), "CarePlan_Interventions");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return intervention;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::CarePlanInterventionInsert", "", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string CarePlanInterventionUpdate(CarePlanInterventionsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            dynamic carePlanInterventionId;
            try
            {
                List<CarePlanInterventionsModel> oldIntervention = FillCarePlanInterventions(model.CarePlanId, model.InterventionId,"", "");

                dbManager.Open();
                createInterventionsUpdateParameters(dbManager, model);
                carePlanInterventionId = dbManager.ExecuteScalar(PROC_CAREPLAN_INTERVENTION_UPDATE);

                if (carePlanInterventionId > 0 && oldIntervention.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(model, oldIntervention[0], MDVUtility.ToStr(carePlanInterventionId), MDVUtility.ToLong(model.CarePlanId), "InterventionId", "Update", MDVUtility.ToStr(model.PatientId), "CarePlan_Interventions");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }
                return MDVUtility.ToStr(carePlanInterventionId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::CarePlanInterventionUpdate", "", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CarePlanInterventionsModel> FillCarePlanInterventions(string CarePlanId, string InterventionId, string NotesId, string action)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                if (MDVUtility.ToLong(InterventionId) <= 0)
                {
                    dbManager.AddParameters(PARM_INTERVENTION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_INTERVENTION_ID, MDVUtility.ToLong(InterventionId));
                }

                if (MDVUtility.ToLong(CarePlanId) <= 0)
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, MDVUtility.ToLong(CarePlanId));

                }
                if (MDVUtility.ToLong(NotesId) <= 0)
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, MDVUtility.ToLong(NotesId));

                }
                #endregion
                List<CarePlanInterventionsModel> result = dbManager.ExecuteReaders<CarePlanInterventionsModel>(PROC_CAREPLAN_INTERVENTION_SELECT);

                if (action == "View" && MDVUtility.ToLong(InterventionId) > 0 && result.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(result[0], null, InterventionId, MDVUtility.ToLong(CarePlanId), "InterventionId", action, result[0].PatientId, "CarePlan_Interventions");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::FillCarePlanHealthInterventions", PROC_CAREPLAN_INTERVENTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteCarePlanIntervention(long InterventionId, long CarePlanId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<CarePlanInterventionsModel> intervention = FillCarePlanInterventions(MDVUtility.ToStr(CarePlanId), MDVUtility.ToStr(InterventionId),"", "");

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_INTERVENTION_ID, InterventionId);
                dbManager.AddParameters(1, PARM_CAREPLAN_ID, CarePlanId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CAREPLAN_INTERVENTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                if (InterventionId > 0 && intervention.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(intervention[0], null, MDVUtility.ToStr(InterventionId), MDVUtility.ToLong(CarePlanId), "InterventionId", "Delete", intervention[0].PatientId, "CarePlan_Interventions");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteCarePlanIntervention", PROC_CAREPLAN_INTERVENTION_DELETE, ex);
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

        public CarePlanOutcomesModel CarePlanOutcomeInsert(CarePlanOutcomesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            CarePlanOutcomesModel outcome = null;
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                createOutcomesInsertParameters(dbManager, model);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CAREPLAN_OUTCOME_INSERT);

                while (reader.HasRows)
                {
                    reader.NextResult();

                    while (reader.Read())
                    {
                        outcome = new CarePlanOutcomesModel();
                        outcome.OutcomesId = !String.IsNullOrEmpty(reader["OutcomesId"].ToString()) ? reader["OutcomesId"].ToString() : "";
                    }
                }

                if (outcome != null && MDVUtility.ToLong(outcome.OutcomesId) > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(model, null, outcome.OutcomesId, MDVUtility.ToLong(model.CarePlanId), "OutcomesId", "Insert", MDVUtility.ToStr(model.PatientId), "CarePlan_Outcomes");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }
                return outcome;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::CarePlanOutcomeInsert", "", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string CarePlanOutcomeUpdate(CarePlanOutcomesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            dynamic carePlanOutcomeId;
            try
            {
                List<CarePlanOutcomesModel> oldOutcome = FillCarePlanOutcomes(model.CarePlanId, model.OutcomesId,"", "");

                dbManager.Open();
                createOutcomesUpdateParameters(dbManager, model);
                carePlanOutcomeId = dbManager.ExecuteScalar(PROC_CAREPLAN_OUTCOME_UPDATE);

                if (carePlanOutcomeId > 0 && oldOutcome.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(model, oldOutcome[0], MDVUtility.ToStr(carePlanOutcomeId), MDVUtility.ToLong(model.CarePlanId), "OutcomesId", "Update", MDVUtility.ToStr(model.PatientId), "CarePlan_Outcomes");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return MDVUtility.ToStr(carePlanOutcomeId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::CarePlanOutcomeUpdate", PROC_CAREPLAN_OUTCOME_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CarePlanOutcomesModel> FillCarePlanOutcomes(string CarePlanId, string OutcomesId, string NotesId, string action)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                if (MDVUtility.ToLong(OutcomesId) <= 0)
                {
                    dbManager.AddParameters(PARM_OUTCOME_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_OUTCOME_ID, MDVUtility.ToLong(OutcomesId));
                }

                if (MDVUtility.ToLong(CarePlanId) <= 0)
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CAREPLAN_ID, MDVUtility.ToLong(CarePlanId));

                }
                if (MDVUtility.ToLong(NotesId) <= 0)
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, MDVUtility.ToLong(NotesId));

                }
                #endregion

                List<CarePlanOutcomesModel> result = dbManager.ExecuteReaders<CarePlanOutcomesModel>(PROC_CAREPLAN_OUTCOME_SELECT);

                if (action == "View" && MDVUtility.ToLong(OutcomesId) > 0 && result.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(result[0], null, OutcomesId, MDVUtility.ToLong(CarePlanId), "OutcomesId", action, result[0].PatientId, "CarePlan_Outcomes");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::FillCarePlanOutcomes", PROC_CAREPLAN_OUTCOME_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteCarePlanOutcome(long OutcomesId, long CarePlanId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<CarePlanOutcomesModel> outcome = FillCarePlanOutcomes(MDVUtility.ToStr(CarePlanId), MDVUtility.ToStr(OutcomesId),"", "");

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_OUTCOME_ID, OutcomesId);
                dbManager.AddParameters(1, PARM_CAREPLAN_ID, CarePlanId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CAREPLAN_OUTCOME_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                if (OutcomesId > 0 && outcome.Count > 0)
                {
                    string auditXML = new DBActivityAudit().GetModelsDBAudit(outcome[0], null, MDVUtility.ToStr(OutcomesId), MDVUtility.ToLong(CarePlanId), "OutcomesId", "Delete", outcome[0].PatientId, "CarePlan_Outcomes");
                    if (!string.IsNullOrEmpty(auditXML))
                    {
                        new DBActivityAudit().InsertModelsDBAudit(auditXML);
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DeleteCarePlanOutcome", PROC_CAREPLAN_OUTCOME_DELETE, ex);
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

        public List<CarePlanInterventionsModel> GetInterventionsLookup(long CarePlanId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                #region paramaters
                dbManager.AddParameters(PARM_CAREPLAN_ID, CarePlanId);
                #endregion

                return dbManager.ExecuteReaders<CarePlanInterventionsModel>(PROC_CAREPLAN_INTERVENTION_LOOKUP);

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::GetInterventionsLookup", PROC_CAREPLAN_INTERVENTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string AddCarePlanToNote(string xml, long NotesId)
        {           
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                
                dbManager.AddParameters(PARM_CARE_PLAN_XML, xml);
                dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, NotesId);
                dbManager.ExecuteScalar(PROC_CAREPLAN_ADD_TONote);
                
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::AddCarePlanToNote", PROC_CAREPLAN_ADD_TONote, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DetachCarePlanFromoNote(string xml, long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_CARE_PLAN_XML, xml);
                dbManager.AddParameters(PARM_CARE_PLAN_NOTESID, NotesId);
                dbManager.ExecuteScalar(PROC_CAREPLAN_DETACH_FROMNOTE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::DetachCarePlanFromoNote", PROC_CAREPLAN_DETACH_FROMNOTE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public CarePlanClinicalModel GetCarePlanByPatientId(string CarePlanId, string PatientId, long ProviderId)
        {            
            try
            {
                CarePlanClinicalModel carePlanModel = null;
               List< CarePlanClinicalModel> carePlan = FillCarePlan(CarePlanId , MDVUtility.ToLong(PatientId));
                if (carePlan.Count > 0)
                {
                    CarePlanId = carePlan[0].CarePlanId;

                    carePlanModel = new CarePlanClinicalModel();
                    carePlanModel.CarePlanId = CarePlanId;
                    carePlanModel.Comments = carePlan[0].Comments;
                    carePlanModel.GoalsModelList = FillCarePlanGoal(CarePlanId, "", "", "", ProviderId);
                    carePlanModel.ConcernsModelList = FillCarePlanHealthConcern(CarePlanId, "", "", "");
                    carePlanModel.InterventionsModelList = FillCarePlanInterventions(CarePlanId, "", "", "");
                    carePlanModel.OutcomesModelList = FillCarePlanOutcomes(CarePlanId, "", "", "");
                }
               
              
                return carePlanModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::GetCarePlanByPatientId", "", ex);
                throw ex;
            }          
        }
        public List<CarePlanGoalsModel> LoadCarePlanGoal_CCDA(long PatientId, long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (NotesId > 0)
                {
                    dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                }
                else
                {
                    dbManager.AddParameters(PARM_NOTES_ID, DBNull.Value);
                }
                List<CarePlanGoalsModel> result = dbManager.ExecuteReaders<CarePlanGoalsModel>(PROC_CAREPLAN_GOAL_SELECT_CCDA);

                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCarePlan::LoadCarePlanGoal_CCDA", PROC_CAREPLAN_GOAL_SELECT_CCDA, ex);
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
