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
    public class DALCQMTest
    {
        #region "Stored Procedures"
        private const string ProcCqmSelect = "Provider.sp_CQMFilter";
        #endregion

        #region "Parameters"
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

        #endregion

        #region Constructors
        public DALCQMTest()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALCQMTest(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new Container();
        }
        #endregion

        public DSCQM Load_CQM(long ProviderId, string NPI, string dtpDateFrom, string dtpDateTo, long PatientId = 0, long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, long GenderId = 0, string Problems = null, long PageNumber = 1, long rpp = 1000)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, ParamProviderId, null);
                else
                    dbManager.AddParameters(0, ParamProviderId, ProviderId);

                dbManager.AddParameters(1, Param_NPI, !string.IsNullOrEmpty(NPI) ? NPI : null);

                if (PatientId <= 0)
                    dbManager.AddParameters(2, ParamPatientId, null);
                else
                    dbManager.AddParameters(2, ParamPatientId, PatientId);

                dbManager.AddParameters(3, ParamTIN, !string.IsNullOrEmpty(TIN) ? TIN : null);

                if (ProviderTypeId <= 0)
                    dbManager.AddParameters(4, ParamProviderTypeId, null);
                else
                    dbManager.AddParameters(4, ParamProviderTypeId, ProviderTypeId);

                dbManager.AddParameters(5, ParamAddress, !string.IsNullOrEmpty(Address) ? Address : null);

                //if (InsurancePlanId <= 0)
                //    dbManager.AddParameters(6, ParamInsuranceplanId, null);
                //else
                    dbManager.AddParameters(6, ParamInsuranceplan, !string.IsNullOrEmpty(InsurancePlan) ? InsurancePlan : null);

                dbManager.AddParameters(7, ParamEthnicityIds, !string.IsNullOrEmpty(EthnicityIds) ? EthnicityIds : null);
                dbManager.AddParameters(8, ParamRaceIds, !string.IsNullOrEmpty(RaceIds) ? RaceIds : null);
                dbManager.AddParameters(9, ParamAgeCondition, !string.IsNullOrEmpty(AgeConditionText) ? AgeConditionText : null);

                if (AgeInMonths <= 0)
                    dbManager.AddParameters(10, ParamAge, null);
                else
                    dbManager.AddParameters(10, ParamAge, AgeInMonths);

                if (GenderId <= 0)
                    dbManager.AddParameters(11, ParamGender, null);
                else
                    dbManager.AddParameters(11, ParamGender, GenderId);

                dbManager.AddParameters(12, ParamProblems, !string.IsNullOrEmpty(Problems) ? Problems : null);
                //dbManager.AddParameters(13, ParamSNOMED, !string.IsNullOrEmpty(SnomedCode) ? SnomedCode : null);

                List<string> tableNames = new List<string>
                {
                ds.CQM.TableName,
                ds.CQM_CQM_Details.TableName
                };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, tableNames);
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

        public DSCQM Load_CQMTest(CQMProblemSearch model, long PageNumber = 1, long rpp = 1000)
        {
            DSCQM ds = new DSCQM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);

                dbManager.AddParameters(0, ParamProviderId, !string.IsNullOrEmpty(model.ProviderId) ? model.ProviderId : null);
                dbManager.AddParameters(1, Param_NPI, !string.IsNullOrEmpty(model.NPI) ? model.NPI : null);
                dbManager.AddParameters(2, ParamPatientId, !string.IsNullOrEmpty(model.PatientId) ? model.PatientId : null);
                dbManager.AddParameters(3, ParamTIN, !string.IsNullOrEmpty(model.TIN) ? model.TIN : null);
                dbManager.AddParameters(4, ParamProviderTypeId, !string.IsNullOrEmpty(model.ProviderTypeId) ? model.ProviderTypeId : null);
                dbManager.AddParameters(5, ParamAddress, !string.IsNullOrEmpty(model.Address) ? model.Address : null);
                dbManager.AddParameters(6, ParamInsuranceplan, !string.IsNullOrEmpty(model.InsurancePlan) ? model.InsurancePlan : null);
                dbManager.AddParameters(7, ParamEthnicityIds, !string.IsNullOrEmpty(model.EthnicityIds) ? model.EthnicityIds : null);
                dbManager.AddParameters(8, ParamRaceIds, !string.IsNullOrEmpty(model.RaceIds) ? model.RaceIds : null);
                dbManager.AddParameters(9, ParamAgeCondition, !string.IsNullOrEmpty(model.AgeCondition_text) ? model.AgeCondition_text : null);
                dbManager.AddParameters(10, ParamAge, !string.IsNullOrEmpty(model.Age) ? model.Age : null);
                dbManager.AddParameters(11, ParamGender, !string.IsNullOrEmpty(model.Sex) ? model.Sex : null);
                dbManager.AddParameters(12, ParamProblems, !string.IsNullOrEmpty(model.ProblemListXML) ? model.ProblemListXML : null);

                List<string> tableNames = new List<string>
                {
                ds.CQM.TableName,
                ds.CQM_CQM_Details.TableName
                };

                ds = (DSCQM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcCqmSelect, ds, tableNames);
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
    }
}
