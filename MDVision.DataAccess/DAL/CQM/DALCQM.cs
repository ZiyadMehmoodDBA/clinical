using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using EDIParser;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Common.Logging;
using System.Data.SqlClient;
using MDVision.Common.Utilities;
using Amib.Threading;
using System.Reflection;
using System.Threading;
using MDVision.Model.Batch.CQM;

namespace MDVision.DataAccess.DAL.CQM
{
    // ReSharper disable once InconsistentNaming
    public class DALCQM
    {
        public enum CQMSPs : long
        {

        }

        #region "Stored Procedure Names"
        //-----------------------------------------------------------------------------------------------------

        private const string ProcCqmSelect = "Provider.CQM_0043";
        private const string ProcCqm0028 = "Provider.CQM_0028";
        private const string ProcCqm0028A = "Provider.CQM_0028v6_A";
        private const string ProcCqm0028B = "Provider.CQM_0028v6_B";
        private const string ProcCqm0028C = "Provider.CQM_0028v6_C";
        private const string ProcCqm0022 = "Provider.CQM_0022";
        private const string ProcCqm0418 = "Provider.CQM_0418";
        private const string PROCSAMPLEDATA = "Provider.[CQM_SampleData]";
        private const string ProcCqm0419 = "Provider.CQM_0419";
        private const string ProcCqm0419_V7 = "Provider.CQM_0419V7";
        private const string ProcCqm0068 = "Provider.CQM_0068";
        private const string ProcCqm0059 = "Provider.CQM_0059";
        private const string ProcCqm0045 = "Provider.CQM_0045";
        private const string ProcCqm0075 = "Provider.CQM_0075";
        private const string ProcCqm0075_A = "Provider.CQM_0075_A";
        private const string ProcCqm0075_B = "Provider.CQM_0075_B";
        private const string ProcCqm0041 = "Provider.CQM_0041";
        private const string ProcCqm0421 = "Provider.CQM_0421";
        private const string ProcCqm0421_V6 = "Provider.CQM_0421V6";
        private const string ProcCQM50 = "Provider.CQM_cms50v3";
        private const string ProcCQM0018 = "Provider.CQM_0018";
        private const string ProcCQM0018_V6 = "Provider.CQM_0018V6";
        private const string ProcCQM0125 = "Provider.CQM_0125";
        private const string ProcCQM0123 = "Provider.CQM_0123";
        private const string ProcCQM0130 = "Provider.CQM_0130";
        private const string ProcCQM0160A = "Provider.CQM_0160_A";
        private const string ProcCQM0160B = "Provider.CQM_0160_B";
        private const string ProcCQM0160C = "Provider.CQM_0160_C";
        private const string ProcCQM0134 = "Provider.CQM_0134";
        private const string ProcCQM0139 = "Provider.CQM_0139";
        private const string ProcCQM0065 = "Provider.CQM_0065";
        private const string ProcCQM0065_V7 = "Provider.CQM_0065V7";
        private const string ProcCQM0149 = "Provider.CQM_0149";
        private const string ProcCQM22 = "Provider.CQM_CMS22v5";
        private const string ProcCQM22_V6 = "Provider.CQM_CMS22v6";
        private const string ProcCQM_0022_A = "Provider.CQM_0022_A";
        private const string ProcCQM_0022_B = "Provider.CQM_0022_B";
        private const string ParamProviderId = "@ProviderId";
        private const string ParamPatientId = "@PatientId";
        private const string ParamTIN = "@TIN";
        private const string ParamFromDate = "@FROM";
        private const string ParamToDate = "@To";
        private const string ParamCqmid = "@CQMID";
        private const string ParamProviderTypeId = "@ProviderTypeId";
        private const string ParamGender = "@Gender";
        private const string ParamAge = "@Age";
        private const string ParamAgeCondition = "@AgeCondition";
        private const string ParamEthnicityIds = "@EthnicityIds";
        private const string ParamRaceIds = "@RaceIds";
        private const string ParamAddress = "@Address";
        private const string ParamSNOMED = "@SNOMEDCode";
        private const string ParamProblems = "@Problems";
        private const string ParamInsuranceplan = "@InsuranceName";
        private const string Param_NPI = "@NPI";
        private const string Param_COMMENTS = "@Comments";
        private const string Param_MuAlertCount = "@MUAlertCount";

        private const string ProcCqmSelectPatientDataSection = "Provider.CQM_PatientDataSection";

        //private const string ProcCqm0068Select = "Provider.CQM_0068_MK";
        //private const string ProcCqm0018Select = "Provider.CQM_0018_MK";

        private const string ProcPatientFill = "Patient.sp_PatientFill_CQM";
        private const string PROC_GET_MIPS_SCORES = "";
        private const string ProcPatients_CQM = "Clinical.sp_GetPatients_CQM";
        private const string ProcCqmMeasureSection = "Provider.sp_CQM_MeasureSection";
        private const string ProcCqmReportingParameters = "Provider.sp_CQM_ReportingParameters";
        private const string ProcCqmReasonValueInsert = "Clinical.sp_cqmReasonValueInsert";
        private const string ProcCqmWithNote = "Provider.CQMWithNotes";
        private const string ProcCqmWithNoteInParallel = "bkp.[CQM_0421-newadditionwithnote]";
        private const string ProcLoadPatientBMI_CQM = "Clinical.sp_loadPatientBMI_CQM";
        private const string ProcProviderMeasure_CQM = "[Provider].[CQM_ProviderMeasure]";
        private const string ProcGroupMeasure_CQM = "[Provider].[CQM_GroupMeasure]";
        private const string ProccqmWithNote_0018_CQM = "[bkp].[CQM_0018_withnote]";
        private const string ProccqmWithNote_0022_CQM = "[bkp].[CQM_0022_withnote]";
        private const string ProccqmWithNote_0421_CQM = "[bkp].[CQM_0421_withnote]";
        private const string ProcGetPatientRecentNote_CQM = "[Clinical].[sp_GetPatientRecentNote_CQM]";
        private const string ProcProviderMeasure_VBP = "[Provider].[VBP_ProviderMeasure]";
        private const string procVBP_MeasureQuestionnairAnswers = "[Clinical].[VBP_MeasureQuestionnaireAnswers]";
        private const string ProcVBP_ScoreInsert = "[Clinical].[sp_VBP_ScoreInsert]";
        private const string ProcVBP_ScoreSelect = "[Clinical].[sp_VBP_ScoreSelect]";
        private const string ProcVBP_ScoreUpdate = "[Clinical].[sp_VBP_ScoreUpdate]";
        private const string ProcVBP_PatientPHQScoreSelect = "[Clinical].[sp_PatientPHQScoreSelect]";

        private const string PROC_PROVIDER_NPI = "Provider.sp_ProviderNpi";
        private const string PROC_LOAD_IMPROVEMENT_ACTIVITIES = "Provider.IA_ProviderMeasure";
        private const string PROC_LOAD_IMPROVEMENT_ACTIVITIES_GROUP = "Provider.IA_GroupMeasure";
        private const string ProcDepression_ScoreInsert = "[Clinical].[sp_Depression_ScoreInsert]";
        //[Provider].[CQM_ProviderMeasure]
        #region They Will be Gone

        private const string ProcCqmPatientDataSectionFamilyHx = "Provider.CQM_PatientDataSection_FamilyHx";
        private const string ProcCqmPatientDataSectionMedicationActive = "Provider.CQM_PatientDataSection_MedicationActive";
        private const string ProcCqmPatientDataSectionMedicationAdministered = "Provider.CQM_PatientDataSection_MedicationAdministered";

        private const string ProcCqmPatientDataSectionMedicationAllergy = "Provider.CQM_PatientDataSection_MedicationAllergy";
        private const string ProcCqmPatientDataSectionMedicationOrder = "Provider.CQM_PatientDataSection_MedicationOrder";

        private const string ProcCqmPatientDataSectionDiagnosisActiveConcernAct = "Provider.CQM_PatientDataSection_DiagnosisActiveConcernAct";
        private const string ProcCqmPatientDataSectionProcedureOrder = "Provider.CQM_PatientDataSection_ProcedureOrder";

        #endregion
        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region "Parameters"
        //-----------------------------------------------------------------------------------------------------

        private const string ParmProviderId = "@ProviderID";
        private const string ParmPatientId = "@PatientId";
        private const string ParmReportType = "@ReportType";

        private const string ParmFromDate = "@FROM";
        private const string ParmToDate = "@To";

        private const string ParmCqmid = "@CQMID";
        private const string ParmPart = "@Part";

        private const string ParmAccountNumber = "@AccountNumber";
        private const string ParmEntityId = "@EntityId";
        private const string ParmUserId = "@UserId";
        private const string ParmIsActive = "@IsActive";

        private const string NqfId = "@NQF_ID";
        private const string parmPatientIds = "@PatientIds";
        private const string parmLOINC = "@Loinc";

        private const string param_cqmReasonId = "@cqmReasonId";
        private const string param_PatientId = "@PatientId";
        private const string param_NoteId = "@NoteId";
        private const string param_CreatedBy = "@CreatedBy";
        private const string param_CreatedOn = "@CreatedOn";
        private const string param_ModifiedBy = "@ModifiedBy";
        private const string param_ModifiedOn = "@ModifiedOn";
        private const string param_Systolic = "@Systolic";
        private const string param_SystolicLOINC = "@SystolicLOINC";
        private const string param_Diastolic = "@Diastolic";
        private const string param_DiastolicLOINC = "@DiastolicLOINC";
        private const string param_SNOMED = "@SNOMED";
        private const string param_CPT = "@CPT";
        private const string param_CVX = "@CVX";
        private const string param_HCPCS = "@HCPCS";
        private const string param_RXNORM = "@RXNORM";
        private const string param_LOINC = "@LOINC";
        private const string param_MeasureId = "@MeasureId";
        private const string param_ReportFromDate = "@ReportFromDate";
        private const string param_ReportToDate = "@ReportToDate";
        private const string param_ICD9CM = "@ICD9CM";
        private const string param_ICD10CM = "@ICD10CM";
        private const string param_BMI = "@BMI";
        private const string param_BMILOINC = "@BMILOINC";
        private const string param_ActionResult = "@ActionResult";
        private const string param_bSignNote = "@bSignNote";
        private const string param_ProviderId = "@ProviderId";
        private const string param_strNotesId = "@NotesId";
        private const string param_MeasureNumber = "@MeasureNumber";
        private const string param_QuestionAnswersId = "@QuestionAnswersId";
        private const string param_MeasureQuestionnaireId = "@MeasureQuestionnaireId";
        private const string param_Score = "@Score";

        private const string PARM_NPI = "@NPI";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_GROUP_ID = "@GroupId";
        private const string PARAM_MEASURES = "@Measures";
        private const string PARAM_MEASURES_PERFORMANCE_RATES = "@MeasuresPerformanceRates";
        private const string PARAM_MEASURES_NAME = "@MeasureName";
        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Constructors
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public DALCQM()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALCQM(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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

        #region "Get using dataset Functions"

        public DSCQM Load_CQM0043(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();

                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);

                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, tableNames1, 500);


                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM", ProcCqmSelect, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0028(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM", ProcCqmSelect, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0022A(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0022A", ProcCQM_0022_A, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0022B(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();

                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0022B", ProcCQM_0022_B, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0068(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0068, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0068", ProcCqm0068, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0418(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0418, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0418", ProcCqm0418, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0419(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0419", ProcCqm0419, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0059(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0059, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0059", ProcCqm0059, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0075A(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0075A", ProcCqm0075_A, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0075B(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0075B", ProcCqm0075_B, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0041(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0041, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0041", ProcCqm0059, ex);
                throw ex;
            }

        }
        public DSCQM Load_Cqm0421(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_Cqm0421", ProcCqm0421, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM50(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM50, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM50", ProcCQM50, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0018(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0018", ProcCQM50, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0125(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0125, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0125", ProcCQM0125, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0123(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0123, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0123", ProcCQM0125, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0130(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0130, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0130", ProcCQM0130, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0134(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0134, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0134", ProcCQM0134, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0139(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0139, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0139", ProcCQM0139, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0065(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0065", ProcCQM0065, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0149(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0149, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0149", ProcCQM0149, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0160A(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0160A", ProcCQM0160A, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0160B(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0160B", ProcCQM0160B, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM0160C(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0160C", ProcCQM0160C, ex);
                throw ex;
            }

        }
        public DSCQM Load_CQM22(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM22", ProcCQM22, ex);
                throw ex;
            }

        }

        public DSCQM Load_CQM(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0, bool isC1 = false)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();


            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);

                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);

                List<string> tableNames = new List<string>
                {
                ds.CQM.TableName,
                ds.CQM_CQM_Details.TableName
                };

                string[] measureIds = null;
                if (cqmId != null && cqmId.IndexOf(',') > -1)
                {
                    measureIds = cqmId.Split(',');
                    foreach (string measureId in measureIds)
                    {
                        cqmId = measureId;

                        if (cqmId != null && cqmId != "")
                        {
                            switch (cqmId)
                            {

                                case "0043":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, tableNames);
                                    break;
                                case "0028":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028, ds, tableNames);
                                    break;
                                case "0028V6":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028A, ds, tableNames);
                                    //ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028B, ds, tableNames);
                                    //ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028C, ds, tableNames);
                                    break;
                                case "0022(A)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, tableNames);
                                    break;
                                case "0022(B)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, tableNames);
                                    break;
                                case "0068":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0068, ds, tableNames);
                                    break;
                                case "0418":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0418, ds, tableNames);
                                    break;
                                case "0419":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, tableNames);
                                    break;
                                case "0419(V7)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419_V7, ds, tableNames);
                                    break;
                                case "0059":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0059, ds, tableNames);
                                    break;
                                case "0075(A)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, tableNames);
                                    break;
                                case "0075(B)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, tableNames);
                                    break;
                                case "0041":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0041, ds, tableNames);
                                    break;
                                case "0421":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames);
                                    break;
                                case "0421(V6)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421_V6, ds, tableNames);
                                    break;
                                case "0421a":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames);
                                    break;
                                case "CMS50v3":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM50, ds, tableNames);
                                    break;
                                case "0018":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018, ds, tableNames);
                                    break;
                                case "0018(V6)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018_V6, ds, tableNames);
                                    break;
                                case "0031":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0125, ds, tableNames);
                                    break;
                                case "0056":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0123, ds, tableNames);
                                    break;
                                case "0034":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0130, ds, tableNames);
                                    break;
                                case "0062":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0134, ds, tableNames);
                                    break;
                                case "0101":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0139, ds, tableNames);
                                    break;
                                case "0065":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065, ds, tableNames);
                                    break;
                                case "0065(v7)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065_V7, ds, tableNames);
                                    break;
                                case "2872":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0149, ds, tableNames);
                                    break;
                                case "CMS22v5":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22, ds, tableNames);
                                    break;
                                case "CMS22v6":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22_V6, ds, tableNames);
                                    break;
                                case "0712(A)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames);
                                    break;
                                case "0712(B)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames);
                                    break;
                                case "0712(C)":
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames);
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames);
                                    break;

                                default:
                                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, tableNames);
                                    break;
                            }
                        }
                        else
                        {
                            Task<DSCQM> CqmSelect = Task<DSCQM>.Factory.StartNew(() => Load_CQM0043(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0028 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0028(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            //Task<DSCQM> CQM0022A = Task<DSCQM>.Factory.StartNew(() => Load_CQM0022A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            //Task<DSCQM> CQM0022B = Task<DSCQM>.Factory.StartNew(() => Load_CQM0022B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0068 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0068(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0418 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0418(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0419 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0419(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0059 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0059(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0075A = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0075A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0075B = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0075B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0041 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0041(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> Cqm0421 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0421(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM50 = Task<DSCQM>.Factory.StartNew(() => Load_CQM50(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM165 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0018(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0125 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0125(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0123 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0123(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0130 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0130(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0134 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0134(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0139 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0139(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0065 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0065(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0149 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0149(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0160A = Task<DSCQM>.Factory.StartNew(() => Load_CQM0160A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0160B = Task<DSCQM>.Factory.StartNew(() => Load_CQM0160B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM0160C = Task<DSCQM>.Factory.StartNew(() => Load_CQM0160C(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task<DSCQM> CQM22 = Task<DSCQM>.Factory.StartNew(() => Load_CQM22(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                            Task.WaitAll(CqmSelect, CQM0028, Cqm0068, Cqm0418, Cqm0419, Cqm0059, Cqm0075A, Cqm0075B, Cqm0041, Cqm0421, CQM50, CQM165, CQM0125, CQM0123, CQM0130, CQM0134, CQM0139, CQM0065, CQM0149, CQM0160A, CQM0160B, CQM0160C, CQM22);
                            ds.Merge(CqmSelect.Result);
                            ds.Merge(CQM0028.Result);
                            ds.Merge(Cqm0068.Result);
                            //ds.Merge(CQM0022A.Result);
                            //ds.Merge(CQM0022B.Result);
                            ds.Merge(Cqm0418.Result);
                            ds.Merge(Cqm0419.Result);
                            ds.Merge(Cqm0059.Result);
                            ds.Merge(Cqm0075A.Result);
                            ds.Merge(Cqm0075B.Result);
                            ds.Merge(Cqm0041.Result);
                            ds.Merge(Cqm0421.Result);
                            ds.Merge(CQM50.Result);
                            ds.Merge(CQM0125.Result);
                            ds.Merge(CQM0123.Result);
                            ds.Merge(CQM0130.Result);
                            ds.Merge(CQM0134.Result);
                            ds.Merge(CQM0139.Result);
                            ds.Merge(CQM0065.Result);
                            ds.Merge(CQM0149.Result);
                            ds.Merge(CQM0160A.Result);
                            ds.Merge(CQM0160B.Result);
                            ds.Merge(CQM0160C.Result);
                            ds.Merge(CQM22.Result);
                            ds.Merge(CQM165.Result);


                        }
                    }
                }
                else
                {
                    if (cqmId != null && cqmId != "")
                    {
                        switch (cqmId)
                        {

                            case "0043":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, tableNames);
                                break;
                            case "0028":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028, ds, tableNames);
                                break;
                            case "0028V6":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028A, ds, tableNames);
                                //ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028B, ds, tableNames);
                                //ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028C, ds, tableNames);
                                break;
                            case "0022(A)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, tableNames);
                                break;
                            case "0022(B)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, tableNames);
                                break;
                            case "0068":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0068, ds, tableNames);
                                break;
                            case "0418":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0418, ds, tableNames);
                                break;
                            case "0419":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, tableNames);
                                break;
                            case "0059":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0059, ds, tableNames);
                                break;
                            case "0075(A)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, tableNames);
                                break;
                            case "0075(B)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, tableNames);
                                break;
                            case "0041":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0041, ds, tableNames);
                                break;
                            case "0421":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames);
                                break;
                            case "0421a":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames);
                                break;
                            case "0421(V6)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421_V6, ds, tableNames);
                                break;
                            case "CMS50v3":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM50, ds, tableNames);
                                break;
                            case "0018":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018, ds, tableNames);
                                break;
                            case "0018(V6)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018_V6, ds, tableNames);
                                break;
                            case "0031":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0125, ds, tableNames);
                                break;
                            case "0056":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0123, ds, tableNames);
                                break;
                            case "0034":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0130, ds, tableNames);
                                break;
                            case "0062":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0134, ds, tableNames);
                                break;
                            case "0101":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0139, ds, tableNames);
                                break;
                            case "0065":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065, ds, tableNames);
                                break;
                            case "0065(v7)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065_V7, ds, tableNames);
                                break;
                            case "2872":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0149, ds, tableNames);
                                break;
                            case "CMS22v5":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22, ds, tableNames);
                                break;
                            case "CMS22v6":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22_V6, ds, tableNames);
                                break;
                            case "0712(A)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames);
                                break;
                            case "0712(B)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames);
                                break;
                            case "0712(C)":
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, tableNames);
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, tableNames);
                                break;

                            default:
                                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, tableNames);
                                break;
                        }
                    }
                    else
                    {
                        Task<DSCQM> CqmSelect = Task<DSCQM>.Factory.StartNew(() => Load_CQM0043(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0028 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0028(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0022A = Task<DSCQM>.Factory.StartNew(() => Load_CQM0022A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0022B = Task<DSCQM>.Factory.StartNew(() => Load_CQM0022B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0068 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0068(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0418 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0418(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0419 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0419(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0059 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0059(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0075A = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0075A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0075B = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0075B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0041 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0041(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> Cqm0421 = Task<DSCQM>.Factory.StartNew(() => Load_Cqm0421(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM50 = Task<DSCQM>.Factory.StartNew(() => Load_CQM50(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM165 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0018(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0125 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0125(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0123 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0123(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0130 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0130(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0134 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0134(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0139 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0139(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0065 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0065(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0149 = Task<DSCQM>.Factory.StartNew(() => Load_CQM0149(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0160A = Task<DSCQM>.Factory.StartNew(() => Load_CQM0160A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0160B = Task<DSCQM>.Factory.StartNew(() => Load_CQM0160B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM0160C = Task<DSCQM>.Factory.StartNew(() => Load_CQM0160C(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task<DSCQM> CQM22 = Task<DSCQM>.Factory.StartNew(() => Load_CQM22(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                        Task.WaitAll(CqmSelect, CQM0028, CQM0022A, CQM0022B, Cqm0068, Cqm0418, Cqm0419, Cqm0059, Cqm0075A, Cqm0075B, Cqm0041, Cqm0421, CQM50, CQM165, CQM0125, CQM0123, CQM0130, CQM0134, CQM0139, CQM0065, CQM0149, CQM0160A, CQM0160B, CQM0160C, CQM22);
                        ds.Merge(CqmSelect.Result);
                        ds.Merge(CQM0028.Result);
                        ds.Merge(Cqm0068.Result);
                        ds.Merge(CQM0022A.Result);
                        ds.Merge(CQM0022B.Result);
                        ds.Merge(Cqm0418.Result);
                        ds.Merge(Cqm0419.Result);
                        ds.Merge(Cqm0059.Result);
                        ds.Merge(Cqm0075A.Result);
                        ds.Merge(Cqm0075B.Result);
                        ds.Merge(Cqm0041.Result);
                        ds.Merge(Cqm0421.Result);
                        ds.Merge(CQM50.Result);
                        ds.Merge(CQM0125.Result);
                        ds.Merge(CQM0123.Result);
                        ds.Merge(CQM0130.Result);
                        ds.Merge(CQM0134.Result);
                        ds.Merge(CQM0139.Result);
                        ds.Merge(CQM0065.Result);
                        ds.Merge(CQM0149.Result);
                        ds.Merge(CQM0160A.Result);
                        ds.Merge(CQM0160B.Result);
                        ds.Merge(CQM0160C.Result);
                        ds.Merge(CQM22.Result);
                        ds.Merge(CQM165.Result);


                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM", ProcCqmSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM Load_CQM_Details(long providerId, string from, string to, string PatientId = null, long reportType = 1, string cqmId = null, int eitherDetail = 0, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, string NPI = null, bool isC1 = false)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);

                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);

                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(PatientId) ? null : PatientId);

                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId >= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);
                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);
                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);

                //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                //dbManager.AddParameters(5, ParmPart, eitherDetail);

                //if (cqmId != "" && cqmId != null && isC1 == true)
                //{
                //    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROCSAMPLEDATA, ds, ds.CQM_PatientsList.TableName);
                //}
                //else
                //{
                switch (cqmId)
                {
                    case "0043":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0022(A)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0022(B)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0418":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0418, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0419":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0059":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0059, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0075(A)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0075(B)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0041":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0041, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0421":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0421a":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "CMS50v3":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM50, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0018":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0028":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0068":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0068, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0031":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0125, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0056":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0123, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0034":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0130, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0062":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0134, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0101":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0139, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0065":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "2872":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0149, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "CMS22v5":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0712(A)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0712(B)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0712(C)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, ds.CQM_PatientsList.TableName);
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0028(V6)A":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028A, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0028(V6)B":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028B, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0028(V6)C":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028C, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0018(V6)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018_V6, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0419(V7)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419_V7, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "CMS22v6":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22_V6, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0065(v7)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065_V7, ds, ds.CQM_PatientsList.TableName);
                        break;
                    case "0421(V6)":
                        ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421_V6, ds, ds.CQM_PatientsList.TableName);
                        break;
                }
                //}

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM_Details", ProcCqmSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM Load_CQM_Codes(long providerId, string from, string to, string patientId = null, long reportType = 2, string cqmId = null, int eitherDetail = 0, bool isC1 = false)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);

                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId);

                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else

                    dbManager.AddParameters(2, ParmReportType, reportType);

                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                //dbManager.AddParameters(6, ParmPart, eitherDetail);

                if (cqmId != "" && cqmId != null && isC1 == true)
                {
                    ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROCSAMPLEDATA, ds, ds.CQMCodes.TableName, 500);
                }
                else
                {
                    switch (cqmId)
                    {

                        case "0043":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0028":

                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0022(A)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, ds.CQMCodes.TableName, 500);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0022(B)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_A, ds, ds.CQMCodes.TableName, 500);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM_0022_B, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0068":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0068, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0418":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0418, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0419":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0059":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0059, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0075(A)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, ds.CQMCodes.TableName, 500);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0075(B)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_A, ds, ds.CQMCodes.TableName, 500);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075_B, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0041":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0041, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0421":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0421a":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "CMS50v3":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM50, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0018":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0031":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0125, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0056":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0123, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0034":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0130, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0062":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0134, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0101":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0139, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0065":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "2872":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0149, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "CMS22v5":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0712(A)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, ds.CQMCodes.TableName, 500);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, ds.CQMCodes.TableName, 500);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, ds.CQMCodes.TableName, 500);
                            break;
                        case "0712(B)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, ds.CQMCodes.TableName);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, ds.CQMCodes.TableName);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, ds.CQMCodes.TableName);
                            break;
                        case "0712(C)":
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160A, ds, ds.CQMCodes.TableName);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160B, ds, ds.CQMCodes.TableName);
                            ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0160C, ds, ds.CQMCodes.TableName);
                            break;
                    }
                }
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM_Codes", ProcCqmSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCQM Load_ImprovementActivities(long providerId)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (providerId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);
                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_IMPROVEMENT_ACTIVITIES, ds, ds.ImprovementActivities.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_ImprovementActivities", PROC_LOAD_IMPROVEMENT_ACTIVITIES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCQM Load_ImprovementActivitiesGroup(long groupId)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (groupId <= 0)
                    dbManager.AddParameters(0, PARM_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_GROUP_ID, groupId);
                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_IMPROVEMENT_ACTIVITIES_GROUP, ds, ds.ImprovementActivities.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_ImprovementActivitiesGroup", PROC_LOAD_IMPROVEMENT_ACTIVITIES_GROUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCQM getMIPSScore(long providerId, string measures, string measuresPerformanceRates)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (providerId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);
                dbManager.AddParameters(1, PARAM_MEASURES, measures);
                dbManager.AddParameters(2, PARAM_MEASURES_PERFORMANCE_RATES, measuresPerformanceRates);
              
                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_MIPS_SCORES, ds, ds.PatientsCQM.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::getMIPSScore", PROC_GET_MIPS_SCORES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region General Functions
        public DSCQM FillPatient(long patientId, string accountNo, string isActive)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (accountNo == "")
                    accountNo = null;

                if (isActive == "")
                    isActive = null;

                dbManager.Open();

                dbManager.CreateParameters(5);

                if (patientId == 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, ParmEntityId,
                String.Equals(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DefaultUser, StringComparison.CurrentCultureIgnoreCase)
                ? null
                : MDVSession.Current.EntityId);

                dbManager.AddParameters(2, ParmAccountNumber, accountNo);
                dbManager.AddParameters(3, ParmIsActive, isActive);
                dbManager.AddParameters(4, ParmUserId, MDVSession.Current.AppUserId);
                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPatientFill, ds, ds.PatientsCQM.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::FillPatient", ProcPatientFill, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public ProviderNPI GetProviderNpi(string NPI, string ProviderId)
        {
            ProviderNPI ds = new ProviderNPI();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_NPI, NPI != "null" ? NPI : null);

                dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId != "null" ? MDVUtility.ToInt64(ProviderId) : 0);

                ds = (ProviderNPI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_NPI, ds, ds.ProviderNpi.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::GetProviderNpi", PROC_PROVIDER_NPI, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Measure Section
        public DSCQM MeasureSection(long providerId, string nqfId)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmMeasureSection, ds, ds.CQM_MeasureSection.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::MeasureSection", ProcPatientFill, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Reporting Parameters
        public DSCQM ReportingParameterSection(long providerId, string nqfId)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmReportingParameters, ds, ds.Providers_CQM.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::ReportingParameterSection", ProcPatientFill, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region PatientDataSection
        public DSCQM PatientDataSection(long patientId, string nqfId = null, string loinc = "")
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);
                dbManager.AddParameters(2, parmLOINC, loinc == "" ? null : loinc);

                var tableNames = new List<string>
                {
                    ds.DiagnosisActiveConcernAct.TableName,
                    ds.FamilyHx.TableName,
                    ds.MedicationActive.TableName,
                    ds.MedicationAdministered.TableName,
                    ds.MedicationAllergy.TableName,
                    ds.MedicationOrder.TableName,
                    ds.EncounterPerformed.TableName,
                    ds.ProcedurePerformed.TableName,
                    ds.ProcedureOrder.TableName,
                    ds.PhysicalExam.TableName,
                    ds.RiskCatagoryAssesment.TableName,
                    ds.TobbacoUser.TableName,
                    ds.ProviderToProvider.TableName,
                    ds.CatagoryIII_PopulationValueSet.TableName,
                    ds.LabOrder.TableName,
                    ds.PatientToProvider.TableName,
                    ds.ProviderToProvider.TableName,
                    ds.RadiologyOrder.TableName,
                    ds.MedicationAdministered_CVX.TableName
                };
                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelectPatientDataSection, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_FamilyHx", ProcCqmSelectPatientDataSection, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_FamilyHx(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionFamilyHx, ds, ds.FamilyHx.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_FamilyHx", ProcCqmPatientDataSectionFamilyHx, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_MedicationActive(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionMedicationActive, ds, ds.MedicationActive.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_MedicationActive", ProcCqmPatientDataSectionMedicationActive, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_MedicationAdministered(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionMedicationAdministered, ds, ds.MedicationAdministered.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_MedicationAdministered", ProcCqmPatientDataSectionMedicationAdministered, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_MedicationAllergy(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionMedicationAllergy, ds, ds.MedicationAllergy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_MedicationAllergy", ProcCqmPatientDataSectionMedicationAllergy, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_MedicationOrder(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionMedicationOrder, ds, ds.MedicationOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_MedicationOrder", ProcCqmPatientDataSectionMedicationOrder, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_DiagnosisActiveConcernAct(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionDiagnosisActiveConcernAct, ds, ds.DiagnosisActiveConcernAct.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_DiagnosisActiveConcernAct", ProcCqmPatientDataSectionDiagnosisActiveConcernAct, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCQM PatientDataSection_ProcedureOrder(long patientId, string nqfId = null)
        {
            var ds = new DSCQM();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId <= 0)
                    dbManager.AddParameters(0, ParmPatientId, null);
                else
                    dbManager.AddParameters(0, ParmPatientId, patientId);

                dbManager.AddParameters(1, NqfId, nqfId == "" ? null : nqfId);

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmPatientDataSectionProcedureOrder, ds, ds.ProcedureOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::PatientDataSection_ProcedureOrder", ProcCqmPatientDataSectionProcedureOrder, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Load CQM With Reasoning

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersCQMReasonValue(IDBManager dbManager, DSCQMReasoning ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(26);

            if (IsInsert == true)
                dbManager.AddParameters(0, param_cqmReasonId, ds.CQM_ReasonValue.cqmReasonIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, param_cqmReasonId, ds.CQM_ReasonValue.cqmReasonIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, param_PatientId, ds.CQM_ReasonValue.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, param_NoteId, ds.CQM_ReasonValue.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, param_CreatedBy, ds.CQM_ReasonValue.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, param_CreatedOn, ds.CQM_ReasonValue.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, param_ModifiedBy, ds.CQM_ReasonValue.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, param_ModifiedOn, ds.CQM_ReasonValue.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, param_Systolic, ds.CQM_ReasonValue.SystolicColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, param_SystolicLOINC, ds.CQM_ReasonValue.SystolicLOINCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, param_Diastolic, ds.CQM_ReasonValue.DiastolicColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, param_DiastolicLOINC, ds.CQM_ReasonValue.DiastolicLOINCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, param_SNOMED, ds.CQM_ReasonValue.SNOMEDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, param_CPT, ds.CQM_ReasonValue.CPTColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, param_CVX, ds.CQM_ReasonValue.CVXColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, param_HCPCS, ds.CQM_ReasonValue.HCPCSColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, param_RXNORM, ds.CQM_ReasonValue.RXNORMColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, param_LOINC, ds.CQM_ReasonValue.LOINCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, param_MeasureId, ds.CQM_ReasonValue.MeasureIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, param_ReportFromDate, ds.CQM_ReasonValue.ReportFromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, param_ReportToDate, ds.CQM_ReasonValue.ReportToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, param_ICD9CM, ds.CQM_ReasonValue.ICD9CMColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, param_ICD10CM, ds.CQM_ReasonValue.ICD10CMColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, param_BMI, ds.CQM_ReasonValue.BMIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, param_BMILOINC, ds.CQM_ReasonValue.BMILOINCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, param_ActionResult, ds.CQM_ReasonValue.ActionResultColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, param_bSignNote, ds.CQM_ReasonValue.bSignNoteColumn.ColumnName, DbType.String);
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/17/2017
        //OverView: Loads CQM with reasoning to be shown either on Signing Note

        public DSCQMReasoning Load_CQMWithReasoning(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0)
        {
            DSCQMReasoning ds = new DSCQMReasoning();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);

                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId);

                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                //dbManager.AddParameters(6, ParmPart, eitherDetail);

                List<string> tableNames = new List<string>
                {
                    ds.AllMeasures.TableName,
                    ds.AllMeasuresDetail.TableName,
                    ds.AllMeasuresReasoningDetail.TableName
                };

                switch (cqmId)
                {

                    case "0043":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, tableNames);
                        break;
                    case "0028":

                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028, ds, tableNames);
                        break;
                    case "0022":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0022, ds, tableNames);
                        break;
                    case "0068":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0068, ds, tableNames);
                        break;
                    case "0418":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0418, ds, tableNames);
                        break;
                    case "0419":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419, ds, tableNames);
                        break;
                    case "0059":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0059, ds, tableNames);
                        break;
                    case "0075":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0075, ds, tableNames);
                        break;
                    case "0041":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0041, ds, tableNames);
                        break;
                    case "0421":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames);
                        break;
                    case "0421a":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421, ds, tableNames);
                        break;
                    case "CMS50v3":
                        ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM50, ds, tableNames);
                        break;
                }
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQMWithReasoning", ProcCqmSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 03/08/2017
        //OverView: Loads CQM with reasoning in parallel way

        public DSCQMReasoning Load_CQMWithReasoningParallel(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, string CQMProcName = "")
        {
            DSCQMReasoning ds = new DSCQMReasoning();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(CQMProcName))
                {
                    dbManager.Open();
                    dbManager.CreateParameters(5);

                    if (providerId <= 0)
                        dbManager.AddParameters(0, ParmProviderId, null);
                    else
                        dbManager.AddParameters(0, ParmProviderId, providerId);

                    dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId);

                    if (reportType < 0)
                        dbManager.AddParameters(2, ParmReportType, null);
                    else
                        dbManager.AddParameters(2, ParmReportType, reportType);
                    dbManager.AddParameters(3, ParmFromDate, from);
                    dbManager.AddParameters(4, ParmToDate, to);
                    //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                    //dbManager.AddParameters(5, ParmPart, eitherDetail);

                    List<string> tableNames = new List<string>
                    {
                        ds.AllMeasures.TableName,
                        ds.AllMeasuresDetail.TableName,
                        ds.AllMeasuresReasoningDetail.TableName
                    };

                    ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, CQMProcName, ds, tableNames);
                }


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQMWithReasoningParallel", CQMProcName, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // Author:  Muhammad Arshad
        // Created Date: 02/02/2017
        //OverView: Loads CQM with reasoning to be shown on Signing Note

        public DSCQMReasoning Load_CQMWithNoteReasoning(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0)
        {
            DSCQMReasoning ds = new DSCQMReasoning();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);

                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId);

                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                //dbManager.AddParameters(6, ParmPart, eitherDetail);
                if (NoteId <= 0)
                    dbManager.AddParameters(5, param_NoteId, null);
                else
                    dbManager.AddParameters(5, param_NoteId, NoteId);

                List<string> tableNames = new List<string>
                {
                    ds.AllMeasures.TableName,
                    ds.AllMeasuresDetail.TableName,
                    ds.AllMeasuresReasoningDetail.TableName
                };

                ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmWithNote, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQMWithNoteReasoning", ProcCqmWithNote, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // Author:  Muhammad Arshad
        // Created Date: 01/18/2017
        //OverView: Loads Patients Data based on given Ids (Comma Separated)

        public DSCQMReasoning Load_Patients_CQM(string PatientIds)
        {
            DSCQMReasoning ds = new DSCQMReasoning();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, parmPatientIds, string.IsNullOrEmpty(PatientIds) ? null : PatientIds);

                List<string> tableNames = new List<string>
                {
                    ds.Patients_CQM.TableName
                };

                ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPatients_CQM, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQMWithReasoning", ProcCqmSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/23/2017
        //OverView: Saves CQMReason with Value
        public DSCQMReasoning InsertCQMReasonValue(DSCQMReasoning ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersCQMReasonValue(dbManager, ds, true);
                ds = (DSCQMReasoning)dbManager.InsertDataSet(CommandType.StoredProcedure, ProcCqmReasonValueInsert, ds, ds.CQM_ReasonValue.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::InsertCQMReasonValue", ProcCqmReasonValueInsert, ex);
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
        // Author:  Muhammad Arshad
        // Created Date: 01/3/2017
        //OverView: Load BMI of Patient
        public string loadPatientBMI(long patientId, long NoteId, string ReportFromDate, string ReportToDate)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                string BMI = "";
                List<SqlParameter> parameters = new List<SqlParameter>();

                long? PatientId = patientId == 0 ? (long?)null : patientId;
                parameters.Add(new SqlParameter(param_PatientId, PatientId));
                parameters.Add(new SqlParameter(param_NoteId, NoteId));
                parameters.Add(new SqlParameter(param_ReportFromDate, ReportFromDate));
                parameters.Add(new SqlParameter(param_ReportToDate, ReportToDate));
                using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcLoadPatientBMI_CQM, parameters))
                {
                    while (reader.Read())
                    {
                        BMI = MDVUtility.CheckStringNull(reader["BMI"]);
                    }
                }

                return BMI;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadPatientBMI", ProcLoadPatientBMI_CQM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/3/2017
        //OverView: Load Assigned Measures of Provider
        public string loadProviderMeasure(long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                string ProviderMeasures = "";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (ProviderId > 0)
                {
                    parameters.Add(new SqlParameter(param_ProviderId, ProviderId));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcProviderMeasure_CQM, parameters))
                    {
                        while (reader.Read())
                        {
                            if (ProviderMeasures == "")
                            {

                            }
                            ProviderMeasures += (ProviderMeasures != "" ? "," : "") + MDVUtility.CheckStringNull(reader["Measures"]);
                        }
                    }
                }


                return ProviderMeasures;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadProviderMeasure", ProcProviderMeasure_CQM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string loadGroupMeasure(string TIN)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                string ProviderMeasures = "";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (! string.IsNullOrEmpty(TIN))
                {
                    parameters.Add(new SqlParameter("@Tin", TIN));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcGroupMeasure_CQM, parameters))
                    {
                        while (reader.Read())
                        {
                            if (ProviderMeasures == "")
                            {

                            }
                            ProviderMeasures += (ProviderMeasures != "" ? "," : "") + MDVUtility.CheckStringNull(reader["Measures"]);
                        }
                    }
                }


                return ProviderMeasures;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadGroupMeasure", ProcGroupMeasure_CQM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCQMReasoning Load_VBPWithNoteReasoningParallel(SharedVariable SharedVariable, long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0, string CQMProcName = "")
        {
            DSCQMReasoning ds = new DSCQMReasoning();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (!string.IsNullOrEmpty(CQMProcName))
                {
                    dbManager.Open();
                    dbManager.CreateParameters(6);

                    if (providerId <= 0)
                        dbManager.AddParameters(0, ParmProviderId, null);
                    else
                        dbManager.AddParameters(0, ParmProviderId, providerId);

                    dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId);

                    if (reportType < 0)
                        dbManager.AddParameters(2, ParmReportType, null);
                    else
                        dbManager.AddParameters(2, ParmReportType, reportType);
                    dbManager.AddParameters(3, ParmFromDate, from);
                    dbManager.AddParameters(4, ParmToDate, to);
                    //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                    //dbManager.AddParameters(5, ParmPart, eitherDetail);
                    if (NoteId <= 0)
                    {
                        dbManager.AddParameters(5, param_NoteId, null);
                    }
                    else
                    {
                        dbManager.AddParameters(5, param_NoteId, NoteId);
                    }

                    List<string> tableNames = new List<string>
                    {
                        ds.AllMeasures.TableName,
                        ds.AllMeasuresDetail.TableName,
                        ds.AllMeasuresReasoningDetail.TableName
                    };

                    ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, CQMProcName, ds, tableNames);

                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALCQM::Load_CQMWithNoteReasoningParallel", CQMProcName, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCQMReasoning Load_CQMWithNoteReasoningParallel(SharedVariable SharedVariable, long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0, string CQMProcName = "")
        {
            DSCQMReasoning ds = new DSCQMReasoning();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (!string.IsNullOrEmpty(CQMProcName))
                {
                    dbManager.Open();
                    dbManager.CreateParameters(6);

                    if (providerId <= 0)
                        dbManager.AddParameters(0, ParmProviderId, null);
                    else
                        dbManager.AddParameters(0, ParmProviderId, providerId);

                    dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId);

                    if (reportType < 0)
                        dbManager.AddParameters(2, ParmReportType, null);
                    else
                        dbManager.AddParameters(2, ParmReportType, reportType);
                    dbManager.AddParameters(3, ParmFromDate, from);
                    dbManager.AddParameters(4, ParmToDate, to);
                    //dbManager.AddParameters(5, ParmCqmid, string.IsNullOrEmpty(cqmId) ? null : cqmId);
                    //dbManager.AddParameters(5, ParmPart, eitherDetail);
                    if (NoteId <= 0)
                    {
                        dbManager.AddParameters(5, param_NoteId, null);
                    }
                    else
                    {
                        dbManager.AddParameters(5, param_NoteId, NoteId);
                    }

                    List<string> tableNames = new List<string>
                    {
                        ds.CQMMeasures.TableName
                    };

                    ds = (DSCQMReasoning)dbManager.ExecuteDataSet(CommandType.StoredProcedure, CQMProcName, ds, tableNames);

                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALCQM::Load_CQMWithNoteReasoningParallel", CQMProcName, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 14/3/2017
        //OverView: Load RecentNote of Patient
        public string getPatientRecentNote(long patientId, string strNotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string NotesId = "";
                if (patientId > 0 && strNotesId != "")
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter(param_PatientId, patientId));
                    parameters.Add(new SqlParameter(param_strNotesId, strNotesId));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcGetPatientRecentNote_CQM, parameters))
                    {
                        while (reader.Read())
                        {
                            NotesId = MDVUtility.CheckStringNull(reader["NotesId"]);
                        }
                    }
                }
                return NotesId;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::getPatientRecentNote", ProcGetPatientRecentNote_CQM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region VBP

        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Load Assigned VBP Measures of Provider
        public string loadProviderMeasure_VBP(long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string ProviderMeasures = "";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (ProviderId > 0)
                {
                    parameters.Add(new SqlParameter(param_ProviderId, ProviderId));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcProviderMeasure_VBP, parameters))
                    {
                        while (reader.Read())
                        {
                            ProviderMeasures += (ProviderMeasures != "" ? "," : "") + MDVUtility.CheckStringNull(reader["Measures"]);
                        }
                    }
                }
                return ProviderMeasures;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadProviderMeasure_VBP", ProcProviderMeasure_VBP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 12 April 2017
        //OverView: Load VBP Measure Questionnaire Answer
        public DataTable loadVBPMeasureQuestionnaireAnswers(string MeasureNumber, string NotesId=null, string PatientId=null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DataTable dtMeasureQuestionnairAnswer = new DataTable();
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(MeasureNumber))
                {
                    parameters.Add(new SqlParameter(param_MeasureNumber, MeasureNumber));
                    if (string.IsNullOrEmpty(NotesId))
                    {
                        parameters.Add(new SqlParameter(param_NoteId, null));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(param_NoteId, NotesId));
                    }
                    if (string.IsNullOrEmpty(PatientId))
                    {
                        parameters.Add(new SqlParameter(param_PatientId, null));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(param_PatientId,PatientId));
                    }
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(procVBP_MeasureQuestionnairAnswers, parameters))
                    {
                        dtMeasureQuestionnairAnswer.Load(reader);
                    }
                }
                return dtMeasureQuestionnairAnswer;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadVBPMeasureQuestionnaireAnswers", procVBP_MeasureQuestionnairAnswers, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 14 April 2017
        //OverView: Saves VBP Reason with Value
        public string InsertVBPReasonValue(string MeasureId, string QuestionAnswersId, Int64 ProviderId, Int64 PatientId, Int64 NotesId, string MeasureQuestionnaireId
                                          , string CPT, string Score, string IsActive, DateTime CreatedOn, string CreatedBy, DateTime ModifiedOn, string ModifiedBy)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);
                if (MeasureId == "")
                {
                    MeasureId = null;
                }
                if (!string.IsNullOrEmpty(MeasureId))
                {
                    dbManager.AddParameters(0, param_MeasureId, MeasureId);
                    dbManager.AddParameters(1, param_QuestionAnswersId, QuestionAnswersId);
                    dbManager.AddParameters(2, param_ProviderId, ProviderId);
                    dbManager.AddParameters(3, param_PatientId, PatientId);
                    dbManager.AddParameters(4, param_strNotesId, NotesId);
                    dbManager.AddParameters(5, param_MeasureQuestionnaireId, MeasureQuestionnaireId);
                    dbManager.AddParameters(6, param_CPT, CPT);
                    dbManager.AddParameters(7, param_Score, Score);
                    dbManager.AddParameters(8, ParmIsActive, IsActive);
                    dbManager.AddParameters(9, param_CreatedOn, CreatedOn);
                    dbManager.AddParameters(10, param_CreatedBy, CreatedBy);
                    dbManager.AddParameters(11, param_ModifiedOn, ModifiedOn);
                    dbManager.AddParameters(12, param_ModifiedBy, ModifiedBy);
                    dbManager.ExecuteNonQuery(CommandType.StoredProcedure, ProcVBP_ScoreInsert);
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::InsertVBPReasonValue", ProcVBP_ScoreInsert, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertVBPDepressionValue(string MeasureId, string QuestionAnswersId, Int64 ProviderId, Int64 PatientId, Int64 NotesId, string MeasureQuestionnaireId
                                          ,  string IsActive, DateTime CreatedOn, string CreatedBy, DateTime ModifiedOn, string ModifiedBy,string MeasureType,string Comments)
        {
            string MuAlertCount = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(14);
                if (MeasureId == "")
                {
                    MeasureId = null;
                }
                if (!string.IsNullOrEmpty(MeasureId))
                {
                    dbManager.AddParameters(0, param_MeasureId, MeasureId);
                    dbManager.AddParameters(1, param_QuestionAnswersId, QuestionAnswersId);
                    if(ProviderId==0)
                    { dbManager.AddParameters(2, param_ProviderId, DBNull.Value); }
                    else
                    {
                        dbManager.AddParameters(2, param_ProviderId, ProviderId);
                    }
                    
                    dbManager.AddParameters(3, param_PatientId, PatientId);
                    if(NotesId==0)
                    dbManager.AddParameters(4, param_strNotesId, null);
                    else
                    dbManager.AddParameters(4, param_strNotesId, null);

                    dbManager.AddParameters(5, param_MeasureQuestionnaireId, MeasureQuestionnaireId);
                    dbManager.AddParameters(6, ParmIsActive, IsActive);
                    dbManager.AddParameters(7, param_CreatedOn, CreatedOn);
                    dbManager.AddParameters(8, param_CreatedBy, CreatedBy);
                    dbManager.AddParameters(9, param_ModifiedOn, ModifiedOn);
                    dbManager.AddParameters(10, param_ModifiedBy, ModifiedBy);
                    dbManager.AddParameters(11, PARAM_MEASURES_NAME, MeasureType);
                    dbManager.AddParameters(12, Param_COMMENTS, Comments);
                    dbManager.AddParameters(13, Param_MuAlertCount, "", DbType.Int64, ParamDirection.Output);
                    dbManager.ExecuteNonQueryWithOutputParam(CommandType.StoredProcedure, ProcDepression_ScoreInsert);
                    if (((SqlParameter)(dbManager.Command.Parameters["@MUAlertCount"])).Value != null)
                        MuAlertCount = ((SqlParameter)(dbManager.Command.Parameters["@MUAlertCount"])).Value.ToString();
                }

                return MuAlertCount;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::InsertVBPDepressionValue", ProcDepression_ScoreInsert, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // Author:  Muhammad Arshad
        // Created Date: 24 April 2017
        //OverView: Loads VBP Score for current Note
        public DataTable loadVBPScore(Int64 NotesId, string MeasureNumber)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DataTable dtVBPScore = new DataTable();
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (NotesId > 0)
                {
                    parameters.Add(new SqlParameter(param_strNotesId, NotesId));
                    parameters.Add(new SqlParameter(param_MeasureNumber, MeasureNumber));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcVBP_ScoreSelect, parameters))
                    {
                        dtVBPScore.Load(reader);
                    }
                }

                return dtVBPScore;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadVBPScore", ProcVBP_ScoreSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 25 April 2017
        //OverView: Updates VBP Reason with Value
        public string UpdateVBPReasonValue(string MeasureId, string QuestionAnswersId, Int64 ProviderId, Int64 PatientId, Int64 NotesId, string MeasureQuestionnaireId
                                          , string CPT, string Score, string IsActive, DateTime CreatedOn, string CreatedBy, DateTime ModifiedOn, string ModifiedBy)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);
                if (MeasureId == "")
                {
                    MeasureId = null;
                }
                if (!string.IsNullOrEmpty(MeasureId))
                {
                    dbManager.AddParameters(0, param_MeasureId, MeasureId);
                    dbManager.AddParameters(1, param_QuestionAnswersId, QuestionAnswersId);
                    dbManager.AddParameters(2, param_ProviderId, ProviderId);
                    dbManager.AddParameters(3, param_PatientId, PatientId);
                    dbManager.AddParameters(4, param_strNotesId, NotesId);
                    dbManager.AddParameters(5, param_MeasureQuestionnaireId, MeasureQuestionnaireId);
                    dbManager.AddParameters(6, param_Score, Score);
                    dbManager.AddParameters(7, ParmIsActive, IsActive);
                    dbManager.AddParameters(8, param_CreatedOn, CreatedOn);
                    dbManager.AddParameters(9, param_CreatedBy, CreatedBy);
                    dbManager.AddParameters(10, param_ModifiedOn, ModifiedOn);
                    dbManager.AddParameters(11, param_ModifiedBy, ModifiedBy);
                    dbManager.ExecuteNonQuery(CommandType.StoredProcedure, ProcVBP_ScoreUpdate);
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::UpdateVBPReasonValue", ProcVBP_ScoreUpdate, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 04 May 2017
        //OverView: Load Patient PHQ Score
        public string loadPatientPHQScore_VBP(long PatientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                int intPHQ9TotalScore = 0;
                int intPHQ2TotalScore = 0;
                string resultTotalScore = "";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (PatientId > 0)
                {
                    parameters.Add(new SqlParameter(ParmPatientId, PatientId));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(ProcVBP_PatientPHQScoreSelect, parameters))
                    {
                        while (reader.Read())
                        {
                            if (MDVUtility.CheckStringNull(reader["MeasureNumber"]) == "PHQ9")
                            {
                                intPHQ9TotalScore += MDVUtility.CheckIntegerNull(reader["score"]);
                            }
                            else if (MDVUtility.CheckStringNull(reader["MeasureNumber"]) == "PHQ2")
                            {
                                intPHQ2TotalScore += MDVUtility.CheckIntegerNull(reader["score"]);
                            }
                            //ProviderMeasures += (ProviderMeasures != "" ? "," : "") + MDVUtility.CheckStringNull(reader["MeasureNumber"]) == "PHQ9" ? "PHQ-9" : "PHQ-2" + ": Total Score = " + MDVUtility.CheckStringNull(reader["score"]);
                        }
                    }
                }
                if (intPHQ9TotalScore > 0)
                {
                    resultTotalScore = "PHQ-9: Total Score = " + MDVUtility.ToStr(intPHQ9TotalScore);
                }
                else if (intPHQ2TotalScore > 0)
                {
                    resultTotalScore = "PHQ-2: Total Score = " + MDVUtility.ToStr(intPHQ2TotalScore); ;
                }
                return resultTotalScore;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::loadPatientPHQScore_VBP", ProcVBP_PatientPHQScoreSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region iTrack Dashboard KPI

        public DSCQM Load_CQM22V6(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM22_V6, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM22V6", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0065V7(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0065_V7, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0065V7", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0018V6(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCQM0018_V6, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0018V6", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0421V6(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0421_V6, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0421V6", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0419V7(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0419_V7, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0421V6", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0028A(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028A, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0028A", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0028B(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028B, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0028B", ProcCqmSelect, ex);
                throw ex;
            }
        }
        public DSCQM Load_CQM0028C(long providerId, string NPI, string from, string to, string patientId = "", long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0)
        {
            DSCQM ds = new DSCQM();
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();


                dbManager.Open();
                dbManager.CreateParameters(16);
                if (providerId <= 0)
                    dbManager.AddParameters(0, ParmProviderId, null);
                else
                    dbManager.AddParameters(0, ParmProviderId, providerId);
                dbManager.AddParameters(1, ParmPatientId, string.IsNullOrEmpty(patientId) ? null : patientId.ToString());
                if (reportType < 0)
                    dbManager.AddParameters(2, ParmReportType, null);
                else
                    dbManager.AddParameters(2, ParmReportType, reportType);
                dbManager.AddParameters(3, ParmFromDate, from);
                dbManager.AddParameters(4, ParmToDate, to);
                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(5, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(5, ParamProviderTypeId, ProviderTypeId);
                dbManager.AddParameters(6, ParamGender, string.IsNullOrEmpty(Sex) ? null : Sex);
                if (AgeInMonths <= 0)
                    dbManager.AddParameters(7, ParamAge, null);
                else
                    dbManager.AddParameters(7, ParamAge, AgeInMonths);
                dbManager.AddParameters(8, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);
                dbManager.AddParameters(9, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(10, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);

                dbManager.AddParameters(11, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);
                dbManager.AddParameters(12, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                dbManager.AddParameters(13, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);
                dbManager.AddParameters(14, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);
                dbManager.AddParameters(15, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                List<string> tableNames1 = new List<string>
                            {
                        ds.CQM.TableName,
                        ds.CQM_CQM_Details.TableName
                            };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqm0028C, ds, tableNames1, 500);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCQM::Load_CQM0028C", ProcCqmSelect, ex);
                throw ex;
            }
        }
        #endregion iTrack Dashboar KPI
    }

}


