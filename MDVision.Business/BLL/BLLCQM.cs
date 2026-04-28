using System;
using System.ComponentModel;
using System.Diagnostics;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DAL.CQM;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Common.Logging;
using System.Collections.Generic;
using Amib.Threading;
using MDVision.Common.Utilities;
using System.Reflection;
using System.Linq;
using MDVision.Model.Clinical.Notes;
using System.Globalization;
using System.Data;
using MDVision.Model.Batch.CQM;
using System.Threading.Tasks;

namespace MDVision.Business.BLL
{
    public class BLLCQM
    {

        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        /// </summary>
        public BLLCQM()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call

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

        public BLObject<DSCQM> Load_CQM(long ProviderId, string NPI, string dtpDateFrom, string dtpDateTo, string PatientId = null, long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0, bool isC1 = false)
        {
            try
            {
                string assignedMeasure = "";
                if (!string.IsNullOrEmpty(TIN))
                {
                    assignedMeasure = new DALCQM().loadGroupMeasure(TIN);
                }
                else
                {
                    assignedMeasure = new DALCQM().loadProviderMeasure(ProviderId);
                }
                if (assignedMeasure != "" && assignedMeasure != null)
                {
                    if (assignedMeasure == "0075")
                    {
                        assignedMeasure = "0075(A)";
                    }
                    else if (assignedMeasure == "0022")
                    {
                        assignedMeasure = "0022(A)";
                    }
                    else if (assignedMeasure == "0712")
                    {
                        assignedMeasure = "0712(A)";
                    }
                    cqmId = assignedMeasure;
                }

                var ds = new DALCQM().Load_CQM(ProviderId, NPI, dtpDateFrom, dtpDateTo, PatientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, MDVUtility.ToInt64(PageNumber), MDVUtility.ToInt64(rpp), eitherDetail, isC1);//providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQM", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }

        public BLObject<DSCQM> Load_ImprovementActivities(long ProviderId)
        {
            try
            {
                var ds = new DALCQM().Load_ImprovementActivities(ProviderId);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_ImprovementActivities", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> Load_ImprovementActivitiesGroup(long GroupId)
        {
            try
            {
                var ds = new DALCQM().Load_ImprovementActivitiesGroup(GroupId);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_ImprovementActivitiesGroup", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> Load_CQM_Details(long providerId, string from, string to, string PatientId = null, long reportType = 1, string cqmId = null, int eitherDetail = 0, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, string NPI = null, bool isC1 = false)
        {
            try
            {
                var ds = new DALCQM().Load_CQM_Details(providerId, from, to, PatientId, reportType, cqmId, eitherDetail, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, NPI, isC1);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQM_Details", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }

        public BLObject<DSCQM> getMIPSScore(long providerId, string measures, string measuresPerformanceRates)
        {
            try
            {
                var ds = new DALCQM().getMIPSScore(providerId, measures, measuresPerformanceRates);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::getMIPSScore", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }

        public BLObject<ProviderNPI> GetProviderNpi(string NPI, string ProviderId)
        {
            try
            {
                ProviderNPI ds = new ProviderNPI();
                ds = new DALCQM().GetProviderNpi(NPI, ProviderId);

                return new BLObject<ProviderNPI>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::GetProviderNpi", ex);
                return new BLObject<ProviderNPI>(null, ex.Message);
            }

        }

        public BLObject<DSCQM> Load_CQM_Codes(long providerId, string from, string to, string patientId = null, long reportType = 2, string cqmId = null, int eitherDetail = 0, bool isC1 = false)
        {
            try
            {
                var ds = new DALCQM().Load_CQM_Codes(providerId, from, to, patientId, reportType, cqmId, eitherDetail, isC1);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQM_Codes", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> FillPatientById(long patientId)
        {
            try
            {
                var ds = new DALCQM().FillPatient(patientId, "", "");
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::FillPatientById", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> MeasureSection(long providerId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().MeasureSection(providerId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::MeasureSection", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> ReportingParameterSection(long providerId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().ReportingParameterSection(providerId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::ReportingParameterSection", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection(long patientId, string nqfId, string loinc = "")
        {
            try
            {
                var ds = new DALCQM().PatientDataSection(patientId, nqfId, loinc);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_FamilyHx(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_FamilyHx(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_FamilyHx", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_MedicationActive(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_MedicationActive(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_MedicationActive", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_MedicationAdministered(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_MedicationAdministered(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_MedicationAdministered", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_MedicationAllergy(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_MedicationAllergy(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_MedicationAllergy", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_MedicationOrder(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_MedicationOrder(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_MedicationOrder", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_DiagnosisActiveConcernAct(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_DiagnosisActiveConcernAct(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_DiagnosisActiveConcernAct", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> PatientDataSection_ProcedureOrder(long patientId, string nqfId)
        {
            try
            {
                var ds = new DALCQM().PatientDataSection_ProcedureOrder(patientId, nqfId);
                ds.AcceptChanges();
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::PatientDataSection_ProcedureOrder", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/17/2017
        //OverView: Loads CQM with reasoning to be shown on Reports
        public BLObject<DSCQMReasoning> Load_CQMWithReasoning(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0)
        {
            try
            {
                //var ds = new DALCQM().Load_CQMWithReasoning(providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                var ds = LoadCQMReasoningParallel(providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                //var ds = LoadCQMWithNoteReasoningParallel(providerId, from, to, patientId, reportType, cqmId, eitherDetail, 0);
                return new BLObject<DSCQMReasoning>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQMWithReasoning", ex);
                return new BLObject<DSCQMReasoning>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 02/02/2017
        //OverView: Loads CQM with reasoning to be shown on Signing Note
        public BLObject<DSCQMReasoning> Load_CQMWithNoteReasoning(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0)
        {
            try
            {
                //from = "01/01/2016";
                //to = "12/31/2016";
                //var ds = new DALCQM().Load_CQMWithNoteReasoning(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId);
                var ds = LoadCQMWithNoteReasoningParallel(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId);
                return new BLObject<DSCQMReasoning>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQMWithNoteReasoning", ex);
                return new BLObject<DSCQMReasoning>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/18/2017
        //OverView: Loads Patients Data based on given Ids (Comma Separated)
        public BLObject<DSCQMReasoning> Load_Patients_CQM(string PatientIds)
        {
            try
            {
                var ds = new DALCQM().Load_Patients_CQM(PatientIds);
                return new BLObject<DSCQMReasoning>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_Patients_CQM", ex);
                return new BLObject<DSCQMReasoning>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/23/2017
        //OverView: Saves CQMReason with Value
        public BLObject<DSCQMReasoning> Insert_CQM_Reason_Value(DSCQMReasoning ds)
        {
            try
            {
                ds = new DALCQM().InsertCQMReasonValue(ds);
                return new BLObject<DSCQMReasoning>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Insert_CQM_Reason_Value", ex);
                return new BLObject<DSCQMReasoning>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 01/3/2017
        //OverView: Load BMI of Patient
        public string loadPatientBMI(long patientId, long NoteId, string ReportFromDate, string ReportToDate)
        {
            try
            {
                string strBMI = "";
                strBMI = new DALCQM().loadPatientBMI(patientId, NoteId, ReportFromDate, ReportToDate);
                return strBMI;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::loadPatientBMI", ex);
                return ex.Message;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 03/07/2017
        //OverView: Loads CQMReason for note in parrallel
        public DSCQMReasoning Load_CQMWithNoteEachReasoningParallel(Object p)
        {
            var Parameters = (ParamsAsStruct)p;
            try
            {
                var ds = new DALCQM(Parameters.Sharedobj).Load_CQMWithNoteReasoningParallel(Parameters.Sharedobj, Parameters.providerId, Parameters.from, Parameters.to, Parameters.patientId, Parameters.reportType, Parameters.cqmId, Parameters.eitherDetail, Parameters.NoteId, Parameters.CQMProcName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(Parameters.Sharedobj, "BLLCQM::Load_CQMWithNoteEachReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }
        public struct ParamsAsStruct
        {
            private long m_providerId;
            private string m_from;
            public long providerId
            {
                get { return m_providerId; }
                set { m_providerId = value; }
            }

            public string from
            {
                get { return m_from; }
                set { m_from = value; }
            }
            private string m_to;

            public string to
            {
                get { return m_to; }
                set { m_to = value; }
            }

            private string m_patientId;

            public string patientId
            {
                get { return m_patientId; }
                set { m_patientId = value; }
            }
            private long m_reportType;

            public long reportType
            {
                get { return m_reportType; }
                set { m_reportType = value; }
            }
            private string m_cqmId;

            public string cqmId
            {
                get { return m_cqmId; }
                set { m_cqmId = value; }
            }

            private int m_eitherDetail;

            public int eitherDetail
            {
                get { return m_eitherDetail; }
                set { m_eitherDetail = value; }
            }

            private long m_NoteId;

            public long NoteId
            {
                get { return m_NoteId; }
                set { m_NoteId = value; }
            }

            private SharedVariable m_Sharedobj;

            public SharedVariable Sharedobj
            {
                get { return m_Sharedobj; }
                set { m_Sharedobj = value; }
            }

            private string m_CQMProcName;

            public string CQMProcName
            {
                get { return m_CQMProcName; }
                set { m_CQMProcName = value; }
            }

            public ParamsAsStruct(long ProviderId, string From, string To, string PatientId, long ReportType, string CqmId, int EitherDetail, long noteId, string cQMProcName)
            {
                m_providerId = ProviderId;
                m_from = From;
                m_to = To;
                m_patientId = PatientId;
                m_reportType = ReportType;
                m_cqmId = CqmId;
                m_eitherDetail = EitherDetail;
                m_NoteId = noteId;
                m_Sharedobj = SharedVariable.GetSharedVariable();
                m_CQMProcName = cQMProcName;
            }

        }

        // Author:  Muhammad Arshad
        // Created Date: 03/07/2017
        //OverView: Loads CQMReason for note in parrallel
        private DSCQMReasoning LoadCQMWithNoteReasoningParallel(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0)
        {
            try
            {
                DSCQMReasoning ds = new DSCQMReasoning();
                Dictionary<String, IWorkItemResult> threadList = new Dictionary<string, IWorkItemResult>();

                SmartThreadPool threadPool = new SmartThreadPool();
                var varDALCQM = new DALCQM(SharedVariable.GetSharedVariable());
                string allMeasures = varDALCQM.loadProviderMeasure(providerId);
                string[] arrAssignedMeasures = allMeasures.Split(',').Distinct().ToArray();
                int FromYear = Convert.ToDateTime(from).Year;
                int ToYear = Convert.ToDateTime(to).Year;
                from = "01/01/" + Convert.ToString(FromYear);
                to = "12/31/" + Convert.ToString(ToYear);
                if (arrAssignedMeasures.Length > 0)
                {
                    foreach (string AssignedMeasure in arrAssignedMeasures)
                    {
                        string measure_name = AssignedMeasure;
                        if (AssignedMeasure != "" && !threadList.ContainsKey(measure_name))
                        {
                            measure_name = measure_name.Contains(")")? measure_name.Remove(measure_name.IndexOf(')'),1) : measure_name;
                            measure_name = measure_name.Contains("(") ? measure_name.Remove(measure_name.IndexOf('('), 1) : measure_name;

                            string CQMProcName = string.Empty;
                            if (measure_name.Contains("0028V6"))
                                measure_name += "_A";
                       
                            CQMProcName = "[Provider].[CQM_" + measure_name + "_withnote]";
                            ParamsAsStruct currentParams = new ParamsAsStruct(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId, CQMProcName);
                            Object objcurrentParams = (Object)currentParams;
                            var task = threadPool.QueueWorkItem(new WorkItemCallback(Load_CQMWithNoteEachReasoningParallel), objcurrentParams);
                            threadList.Add(measure_name, task);
                        }
                    }
                }

                bool success = SmartThreadPool.WaitAll(threadList.Values.ToArray());

                if (success)
                {
                    foreach (var key in threadList.Keys)
                    {
                        try
                        {
                            if (threadList[key].Result != null)
                            {
                                DSCQMReasoning dsCurrentMeasure = (DSCQMReasoning)threadList[key].Result;
                                ds.Merge(dsCurrentMeasure, true, System.Data.MissingSchemaAction.Ignore);
                            }
                        }
                        catch (Exception ex)
                        {
                            string myval = ex.Message;
                        }

                        //PropertyInfo propertyInfo = model.GetType().GetProperty(key);
                        //propertyInfo.SetValue(model, Convert.ChangeType(threadList[key].Result, propertyInfo.PropertyType), null);
                    }

                    // AttachComponentsWithNote(model)
                }

                threadPool.Shutdown();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::LoadCQMWithNoteReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 03/07/2017
        //OverView: Loads CQMReason for note in parrallel
        public DSCQMReasoning Load_CQMEachReasoningParallel(Object p)
        {
            try
            {
                var Parameters = (ParamsAsStruct)p;
                var ds = new DALCQM(Parameters.Sharedobj).Load_CQMWithReasoningParallel(Parameters.providerId, Parameters.from, Parameters.to, Parameters.patientId, Parameters.reportType, Parameters.cqmId, Parameters.eitherDetail, Parameters.CQMProcName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQMEachReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }
        // Author:  Muhammad Arshad
        // Created Date: 03/08/2017
        //OverView: Loads CQMReason for reports in parrallel
        private DSCQMReasoning LoadCQMReasoningParallel(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0)
        {
            try
            {
                DSCQMReasoning ds = new DSCQMReasoning();
                Dictionary<String, IWorkItemResult> threadList = new Dictionary<string, IWorkItemResult>();

                SmartThreadPool threadPool = new SmartThreadPool();
                var varDALCQM = new DALCQM(SharedVariable.GetSharedVariable());
                string allMeasures = varDALCQM.loadProviderMeasure(providerId);
                string[] arrAssignedMeasures = allMeasures.Split(',').Distinct().ToArray();
                if (arrAssignedMeasures.Length > 0)
                {
                    foreach (string AssignedMeasure in arrAssignedMeasures)
                    {
                        if (AssignedMeasure != "" && !threadList.ContainsKey(AssignedMeasure))
                        {
                            string CQMProcName = "[Provider].[CQM_" + AssignedMeasure + "]";
                            ParamsAsStruct currentParams = new ParamsAsStruct(providerId, from, to, patientId, reportType, cqmId, eitherDetail, 0, CQMProcName);
                            Object objcurrentParams = (Object)currentParams;
                            var task = threadPool.QueueWorkItem(new WorkItemCallback(Load_CQMEachReasoningParallel), objcurrentParams);
                            threadList.Add(AssignedMeasure, task);
                        }
                    }
                }

                bool success = SmartThreadPool.WaitAll(threadList.Values.ToArray());

                if (success)
                {
                    foreach (var key in threadList.Keys)
                    {
                        try
                        {
                            if (threadList[key].Result != null)
                            {
                                DSCQMReasoning dsCurrentMeasure = (DSCQMReasoning)threadList[key].Result;
                                ds.Merge(dsCurrentMeasure, true, System.Data.MissingSchemaAction.Ignore);
                            }
                        }
                        catch (Exception ex)
                        {
                            string myval = ex.Message;
                        }
                    }
                }

                threadPool.Shutdown();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::LoadCQMReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 14/3/2017
        //OverView: Get Recent Note of Patient
        public string getPatientRecentNote(long patientId, string strNotesId)
        {
            try
            {
                string strNoteId = "";
                strNoteId = new DALCQM().getPatientRecentNote(patientId, strNotesId);
                return strNoteId;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::getPatientRecentNote", ex);
                return ex.Message;
            }
        }

        #region VBP

        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Loads VBP with reasoning to be shown on Reports
        public BLObject<DSCQMReasoning> Load_VBPWithReasoning(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0)
        {
            try
            {
                var ds = LoadVBPReasoningParallel(providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                return new BLObject<DSCQMReasoning>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_VBPWithReasoning", ex);
                return new BLObject<DSCQMReasoning>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Loads VBP with reasoning to be shown on Signing Note
        public BLObject<DSCQMReasoning> Load_VBPWithNoteReasoning(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0)
        {
            try
            {
                //from = "01/01/2016";
                //to = "12/31/2016";
                var ds = LoadVBPWithNoteReasoningParallel(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId);
                return new BLObject<DSCQMReasoning>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_VBPWithNoteReasoning", ex);
                return new BLObject<DSCQMReasoning>(null, ex.Message);
            }
        }
        public string loadProviderVBPMeasures(long providerId)
        {
            try
            {
                string allMeasures = new DALCQM().loadProviderMeasure_VBP(providerId);
                return allMeasures;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::loadProviderVBPMeasures", ex);
                return ex.Message;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Loads VBPReason for note in parrallel
        public DSCQMReasoning Load_VBPEachReasoningParallel(Object p)
        {
            try
            {
                var Parameters = (ParamsAsStruct)p;
                var ds = new DALCQM(Parameters.Sharedobj).Load_CQMWithReasoningParallel(Parameters.providerId, Parameters.from, Parameters.to, Parameters.patientId, Parameters.reportType, Parameters.cqmId, Parameters.eitherDetail, Parameters.CQMProcName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQMEachReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }
        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Loads VBPReason for reports in parrallel
        private DSCQMReasoning LoadVBPReasoningParallel(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0)
        {
            try
            {
                DSCQMReasoning ds = new DSCQMReasoning();
                Dictionary<String, IWorkItemResult> threadList = new Dictionary<string, IWorkItemResult>();

                SmartThreadPool threadPool = new SmartThreadPool();
                var varDALCQM = new DALCQM(SharedVariable.GetSharedVariable());
                string allMeasures = varDALCQM.loadProviderMeasure_VBP(providerId);
                string[] arrAssignedMeasures = allMeasures.Split(',').Distinct().ToArray();
                if (arrAssignedMeasures.Length > 0)
                {
                    foreach (string AssignedMeasure in arrAssignedMeasures)
                    {
                        if (AssignedMeasure != "" && !threadList.ContainsKey(AssignedMeasure))
                        {
                            string CQMProcName = "[Provider].[VBP_" + AssignedMeasure + "]";
                            ParamsAsStruct currentParams = new ParamsAsStruct(providerId, from, to, patientId, reportType, cqmId, eitherDetail, 0, CQMProcName);
                            Object objcurrentParams = (Object)currentParams;
                            var task = threadPool.QueueWorkItem(new WorkItemCallback(Load_VBPEachReasoningParallel), objcurrentParams);
                            threadList.Add(AssignedMeasure, task);
                        }
                    }
                }

                bool success = SmartThreadPool.WaitAll(threadList.Values.ToArray());

                if (success)
                {
                    foreach (var key in threadList.Keys)
                    {
                        try
                        {
                            if (threadList[key].Result != null)
                            {
                                DSCQMReasoning dsCurrentMeasure = (DSCQMReasoning)threadList[key].Result;
                                ds.Merge(dsCurrentMeasure, true, System.Data.MissingSchemaAction.Ignore);
                            }
                        }
                        catch (Exception ex)
                        {
                            string myval = ex.Message;
                        }
                    }
                }

                threadPool.Shutdown();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::LoadCQMReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Loads VBPReason for note in parrallel
        public DSCQMReasoning Load_VBPWithNoteEachReasoningParallel(Object p)
        {
            try
            {
                var Parameters = (ParamsAsStruct)p;
                var ds = new DALCQM(Parameters.Sharedobj).Load_VBPWithNoteReasoningParallel(Parameters.Sharedobj, Parameters.providerId, Parameters.from, Parameters.to, Parameters.patientId, Parameters.reportType, Parameters.cqmId, Parameters.eitherDetail, Parameters.NoteId, Parameters.CQMProcName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_VBPWithNoteEachReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }
        // Author:  Muhammad Arshad
        // Created Date: 10 April 2017
        //OverView: Loads VBPReason for note in parrallel
        private DSCQMReasoning LoadVBPWithNoteReasoningParallel(long providerId, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, int eitherDetail = 0, long NoteId = 0)
        {
            try
            {
                DSCQMReasoning ds = new DSCQMReasoning();
                Dictionary<String, IWorkItemResult> threadList = new Dictionary<string, IWorkItemResult>();

                SmartThreadPool threadPool = new SmartThreadPool();
                var varDALCQM = new DALCQM(SharedVariable.GetSharedVariable());
                string allMeasures = varDALCQM.loadProviderMeasure_VBP(providerId);
                string[] arrAssignedMeasures = allMeasures.Split(',').Distinct().ToArray();
                int FromYear = Convert.ToDateTime(from).Year;
                int ToYear = Convert.ToDateTime(to).Year;
                from = "01/01/" + Convert.ToString(FromYear);
                to = "12/31/" + Convert.ToString(ToYear);
                if (arrAssignedMeasures.Length > 0)
                {
                    foreach (string AssignedMeasure in arrAssignedMeasures)
                    {
                        if (AssignedMeasure != "" && !threadList.ContainsKey(AssignedMeasure))
                        {
                            string CQMProcName = "[Provider].[VBP_" + AssignedMeasure + "_withnote]";
                            ParamsAsStruct currentParams = new ParamsAsStruct(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId, CQMProcName);
                            Object objcurrentParams = (Object)currentParams;
                            var task = threadPool.QueueWorkItem(new WorkItemCallback(Load_VBPWithNoteEachReasoningParallel), objcurrentParams);
                            threadList.Add(AssignedMeasure, task);
                        }
                    }
                }

                bool success = SmartThreadPool.WaitAll(threadList.Values.ToArray());

                if (success)
                {
                    foreach (var key in threadList.Keys)
                    {
                        try
                        {
                            if (threadList[key].Result != null)
                            {
                                DSCQMReasoning dsCurrentMeasure = (DSCQMReasoning)threadList[key].Result;
                                ds.Merge(dsCurrentMeasure, true, System.Data.MissingSchemaAction.Ignore);
                            }
                        }
                        catch (Exception ex)
                        {
                            string myval = ex.Message;
                        }

                        //PropertyInfo propertyInfo = model.GetType().GetProperty(key);
                        //propertyInfo.SetValue(model, Convert.ChangeType(threadList[key].Result, propertyInfo.PropertyType), null);
                    }

                    // AttachComponentsWithNote(model)
                }

                threadPool.Shutdown();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::LoadVBPWithNoteReasoningParallel", ex);
                return new DSCQMReasoning();
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 12 April 2017
        //OverView: Get VBP Measure Questionnaire Answers
        public DataTable loadVBPMeasureQuestionnaireAnswers(string MeasureNumber, string NotesId, string PatientId)
        {
            try
            {
                DataTable dtVBPMeasureQuestionnaire = new DataTable();
                dtVBPMeasureQuestionnaire = new DALCQM().loadVBPMeasureQuestionnaireAnswers(MeasureNumber, NotesId, PatientId);
                return dtVBPMeasureQuestionnaire;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::loadVBPMeasureQuestionnaireAnswers", ex);
                DataTable dtVBPMeasureQuestionnaire = new DataTable();
                return dtVBPMeasureQuestionnaire;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 14 April 2017
        //OverView: Saves VBP Reason with Value
        public string Insert_VBP_Reason_Value(string MeasureId, string QuestionAnswersId, Int64 ProviderId, Int64 PatientId, Int64 NotesId, string MeasureQuestionnaireId
                                          , string CPT, string Score, string IsActive, DateTime CreatedOn, string CreatedBy, DateTime ModifiedOn, string ModifiedBy)
        {
            try
            {
                string result = "";
                if (!string.IsNullOrEmpty(CPT))
                {
                    result = new DALCQM().InsertVBPReasonValue(MeasureId, QuestionAnswersId, ProviderId, PatientId, NotesId, MeasureQuestionnaireId
                                          , CPT, Score, IsActive, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy);
                }
                else if (!string.IsNullOrEmpty(QuestionAnswersId) && !string.IsNullOrEmpty(MeasureQuestionnaireId))
                {
                    result = new DALCQM().UpdateVBPReasonValue(MeasureId, QuestionAnswersId, ProviderId, PatientId, NotesId, MeasureQuestionnaireId
                                          , CPT, Score, IsActive, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy);
                }
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Insert_VBP_Reason_Value", ex);
                return ex.Message;
            }
        }
        public string Insert_VBP_Depression_Value(string MeasureId, string QuestionAnswersId, Int64 ProviderId, Int64 PatientId, Int64 NotesId, string MeasureQuestionnaireId
                                          , string IsActive, DateTime CreatedOn, string CreatedBy, DateTime ModifiedOn, string ModifiedBy, string MeasureType, string Comments)
        {
            try
            {
                string result = "";

                result = new DALCQM().InsertVBPDepressionValue(MeasureId, QuestionAnswersId, ProviderId, PatientId, NotesId, MeasureQuestionnaireId
                                      , IsActive, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, MeasureType, Comments);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Insert_VBP_Depression_Value", ex);
                return ex.Message;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 24 April 2017
        //OverView: Loads VBP Score for CurrentNote
        public DataTable loadVBPScore(Int64 NotesId, string MeasureNumber)
        {
            DataTable dtVBPScore = new DataTable();
            try
            {
                dtVBPScore = new DALCQM().loadVBPScore(NotesId, MeasureNumber);
                return dtVBPScore;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::loadVBPMeasureQuestionnaireAnswers", ex);
                return dtVBPScore;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 04 May 2017
        //OverView: Loads VBP Score for Current Patient
        public string loadPatientPHQScore_VBP(Int64 PatientId)
        {
            string PatientPHQScore = "";
            try
            {
                PatientPHQScore = new DALCQM().loadPatientPHQScore_VBP(PatientId);
                return PatientPHQScore;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::loadPatientPHQScore_VBP", ex);
                return ex.Message;
            }
        }
        #endregion

        public BLObject<DSCQM> Load_CQM_iTrackDashboard(long providerId, string from, string to, long reportType = 0, string TIN = null)
        {
            try
            {
                SharedVariable SharedVariable = new SharedVariable();
                SharedVariable.EntityId = MDVSession.Current.EntityId;
                SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                SharedVariable.UserName = MDVSession.Current.AppUserName;
                SharedVariable.ClientId = MDVSession.Current.ClientId;
                SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;
                string cqmId = "";
                string assignedMeasure = "";
                if (!string.IsNullOrEmpty(TIN))
                {
                    assignedMeasure = new DALCQM(SharedVariable).loadGroupMeasure(TIN);
                }
                else
                {
                    assignedMeasure = new DALCQM(SharedVariable).loadProviderMeasure(providerId);
                }
                if (assignedMeasure != "" && assignedMeasure != null)
                {
                    if (assignedMeasure == "0075")
                    {
                        assignedMeasure = "0075(A)";
                    }
                    else if (assignedMeasure == "0022")
                    {
                        assignedMeasure = "0022(A)";
                    }
                    else if (assignedMeasure == "0712")
                    {
                        assignedMeasure = "0712(A)";
                    }
                    cqmId = assignedMeasure;
                }
                string[] measureIds = null;
                DSCQM ds = new DSCQM();
                if (cqmId != null )
                {
                    
                    if (cqmId.IndexOf(',') < 0)
                    {
                        cqmId = cqmId + ",";
                    }
                    measureIds = cqmId.Split(',');

                    List<Task<DSCQM>> tasks = new List<Task<DSCQM>>();
                    foreach (string measureId in measureIds)
                    {
                        cqmId = measureId;

                        if (cqmId != null && cqmId != "")
                        {
                            switch (cqmId)
                            {
                                case "0043":
                                    Task<DSCQM> task0043 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0043(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                    tasks.Add(task0043);
                                    task0043.Start();
                                    break;
                                case "0028":
                                    Task<DSCQM> task0028 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(task0028);
                                    task0028.Start();
                                    break;
                                case "0028V6":
                                    Task<DSCQM> task0028V61 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(task0028V61);
                                    task0028V61.Start();
                                    //Task<DSCQM> taskProcCqm0028B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                    // tasks.Add(taskProcCqm0028B);
                                    //taskProcCqm0028B.Start();
                                    //Task<DSCQM> taskProcCqm0028C = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028C(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                    // tasks.Add(taskProcCqm0028C);
                                    //taskProcCqm0028C.Start();
                                    break;
                                case "0022(A)":
                                    Task<DSCQM> taskProcCQM_0022_A = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM_0022_A);
                                    taskProcCQM_0022_A.Start();
                                    Task<DSCQM> taskProcCQM_0022_B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM_0022_B);
                                    taskProcCQM_0022_B.Start();
                                    break;
                                case "0022(B)":
                                    Task<DSCQM> taskProcCQM_0022_A1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM_0022_A1);
                                    taskProcCQM_0022_A1.Start();
                                    Task<DSCQM> taskProcCQM_0022_B1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM_0022_B1);
                                    taskProcCQM_0022_B1.Start();
                                    break;
                                case "0068":
                                    Task<DSCQM> taskProcCqm0068 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0068(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0068);
                                    taskProcCqm0068.Start();
                                    break;
                                case "0418":
                                    Task<DSCQM> taskProcCqm0418 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0418(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0418);
                                    taskProcCqm0418.Start();
                                    break;
                                case "0419":
                                    Task<DSCQM> taskProcCqm0419 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0419(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0419);
                                    taskProcCqm0419.Start();
                                    break;
                                case "0419(V7)":
                                    Task<DSCQM> taskProcCqm0419_V7 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0419V7(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0419_V7);
                                    taskProcCqm0419_V7.Start();
                                    break;
                                case "0059":
                                    Task<DSCQM> taskProcCqm00591 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0059(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm00591);
                                    taskProcCqm00591.Start();
                                    break;
                                case "0075(A)":
                                    Task<DSCQM> taskProcCqm0075_A = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0075_A);
                                    taskProcCqm0075_A.Start();
                                    Task<DSCQM> taskProcCqm0075_B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0075_B);
                                    taskProcCqm0075_B.Start();
                                    break;
                                case "0075(B)":
                                    Task<DSCQM> taskProcCqm0075_A1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0075_A1);
                                    taskProcCqm0075_A1.Start();
                                    Task<DSCQM> taskProcCqm0075_B1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0075_B1);
                                    taskProcCqm0075_B1.Start();
                                    break;
                                case "0041":
                                    Task<DSCQM> taskProcCqm0041 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0041(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0041);
                                    taskProcCqm0041.Start();
                                    break;
                                case "0421":
                                    Task<DSCQM> taskProcCqm0421 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0421(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0421);
                                    taskProcCqm0421.Start();
                                    break;
                                case "0421(V6)":
                                    Task<DSCQM> taskProcCqm0421_V6 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0421V6(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm0421_V6);
                                    taskProcCqm0421_V6.Start();
                                    break;
                                case "0421a":
                                    Task<DSCQM> taskProcCqm04211 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0421(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCqm04211);
                                    taskProcCqm04211.Start();
                                    break;
                                case "CMS50v3":
                                    Task<DSCQM> taskProcCQM50 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM50(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM50);
                                    taskProcCQM50.Start();
                                    break;
                                case "0018":
                                    Task<DSCQM> taskProcCQM0018 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0018(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0018);
                                    taskProcCQM0018.Start();
                                    break;
                                case "0018(V6)":
                                    Task<DSCQM> taskProcCQM0018_V6 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0018V6(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0018_V6);
                                    taskProcCQM0018_V6.Start();
                                    break;
                                case "0031":
                                    Task<DSCQM> taskProcCQM0125 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0125(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0125);
                                    taskProcCQM0125.Start();
                                    break;
                                case "0056":
                                    Task<DSCQM> taskProcCQM0123 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0123(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0123);
                                    taskProcCQM0123.Start();
                                    break;
                                case "0034":
                                    Task<DSCQM> taskProcCQM0130 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0130(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0130);
                                    taskProcCQM0130.Start();
                                    break;
                                case "0062":
                                    Task<DSCQM> taskProcCQM0134 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0134(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0134);
                                    taskProcCQM0134.Start();
                                    break;
                                case "0101":
                                    Task<DSCQM> taskProcCQM0139 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0139(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0139);
                                    taskProcCQM0139.Start();
                                    break;
                                case "0065":
                                    Task<DSCQM> taskProcCQM0065 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0065(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0065);
                                    taskProcCQM0065.Start();
                                    break;
                                case "0065(v7)":
                                    Task<DSCQM> taskProcCQM0065_V7 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0065V7(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0065_V7);
                                    taskProcCQM0065_V7.Start();
                                    break;
                                case "2872":
                                    Task<DSCQM> taskProcCQM0149 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0149(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0149);
                                    taskProcCQM0149.Start();
                                    break;
                                case "CMS22v5":
                                    Task<DSCQM> taskProcCQM22 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM22(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM22);
                                    taskProcCQM22.Start();
                                    break;
                                case "CMS22v6":
                                    Task<DSCQM> taskProcCQM22_V6 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM22V6(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM22_V6);
                                    taskProcCQM22_V6.Start();
                                    break;
                                case "0712(A)":
                                    Task<DSCQM> taskProcCQM0160A = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160A);
                                    taskProcCQM0160A.Start();
                                    Task<DSCQM> taskProcCQM0160B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160B);
                                    taskProcCQM0160B.Start();
                                    Task<DSCQM> taskProcCQM0160C = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160C(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160C);
                                    taskProcCQM0160C.Start();
                                    break;
                                case "0712(B)":

                                    Task<DSCQM> taskProcCQM0160A1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160A1);
                                    taskProcCQM0160A1.Start();
                                    Task<DSCQM> taskProcCQM0160B1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160B1);
                                    taskProcCQM0160B1.Start();
                                    Task<DSCQM> taskProcCQM0160C1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160C(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160C1);
                                    taskProcCQM0160C1.Start();
                                    break;
                                case "0712(C)":
                                    Task<DSCQM> taskProcCQM0160A2 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160A(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160A2);
                                    taskProcCQM0160A2.Start();
                                    Task<DSCQM> taskProcCQM0160B2 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160B(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160B2);
                                    taskProcCQM0160B2.Start();
                                    Task<DSCQM> taskProcCQM0160C2 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160C(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                     tasks.Add(taskProcCQM0160C2);
                                    taskProcCQM0160C2.Start();
                                    break;

                                default:
                                    //Task<DSCQM> taskProcCqm04191 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0419(providerId, "", from, to, "", reportType, cqmId, TIN, 0, null, null, null, null, null, 0, null, null, 1, 1000, 0));
                                    // tasks.Add(taskProcCqm04191);
                                    //taskProcCqm04191.Start();
                                    break;
                            }
                        }
                    }
                    Task.WaitAll(tasks.ToArray());
                    foreach (var item in tasks)
                    {
                        ds.Merge(item.Result);
                    }
                }
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQM_iTrackDashboard", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
        public BLObject<DSCQM> Load_CQM_Quality(long providerId, string NPI, string from, string to, string patientId = null, long reportType = 0, string cqmId = null, string TIN = null, long ProviderTypeId = 0, string Address = null, string InsurancePlan = null, string EthnicityIds = null, string RaceIds = null, string AgeConditionText = null, long AgeInMonths = 0, string Sex = null, string Problems = null, long PageNumber = 1, long rpp = 1000, int eitherDetail = 0, bool isC1 = false)
        {
            try
            {
                SharedVariable SharedVariable = new SharedVariable();
                SharedVariable.EntityId = MDVSession.Current.EntityId;
                SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                SharedVariable.UserName = MDVSession.Current.AppUserName;
                SharedVariable.ClientId = MDVSession.Current.ClientId;
                SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;
                string assignedMeasure = "";
                if (!string.IsNullOrEmpty(TIN))
                {
                    assignedMeasure = new DALCQM(SharedVariable).loadGroupMeasure(TIN);
                }
                else
                {
                    assignedMeasure = new DALCQM(SharedVariable).loadProviderMeasure(providerId);
                }
                if (assignedMeasure != "" && assignedMeasure != null)
                {
                    if (assignedMeasure == "0075")
                    {
                        assignedMeasure = "0075(A)";
                    }
                    else if (assignedMeasure == "0022")
                    {
                        assignedMeasure = "0022(A)";
                    }
                    else if (assignedMeasure == "0712")
                    {
                        assignedMeasure = "0712(A)";
                    }
                    cqmId = assignedMeasure;
                }
                string[] measureIds = null;
                DSCQM ds = new DSCQM();
                if (cqmId != null && cqmId.IndexOf(',') > -1)
                {
                    measureIds = cqmId.Split(',');
                    List<Task<DSCQM>> tasks = new List<Task<DSCQM>>();
                    foreach (string measureId in measureIds)
                    {
                        cqmId = measureId;

                        if (cqmId != null && cqmId != "")
                        {
                            switch (cqmId)
                            {
                                case "0043":
                                    Task<DSCQM> task0043 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0043(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(task0043);
                                    task0043.Start();
                                    break;
                                case "0028":
                                    Task<DSCQM> task0028 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(task0028);
                                    task0028.Start();
                                    break;
                                case "0028V6":
                                    Task<DSCQM> task0028V61 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(task0028V61);
                                    task0028V61.Start();
                                    //Task<DSCQM> taskProcCqm0028B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    //tasks.Add(taskProcCqm0028B);
                                    //taskProcCqm0028B.Start();
                                    //Task<DSCQM> taskProcCqm0028C = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0028C(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    //tasks.Add(taskProcCqm0028C);
                                    //taskProcCqm0028C.Start();
                                    break;
                                case "0022(A)":
                                    Task<DSCQM> taskProcCQM_0022_A = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM_0022_A);
                                    taskProcCQM_0022_A.Start();
                                    Task<DSCQM> taskProcCQM_0022_B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM_0022_B);
                                    taskProcCQM_0022_B.Start();
                                    break;
                                case "0022(B)":
                                    Task<DSCQM> taskProcCQM_0022_A1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM_0022_A1);
                                    taskProcCQM_0022_A1.Start();
                                    Task<DSCQM> taskProcCQM_0022_B1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0022B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM_0022_B1);
                                    taskProcCQM_0022_B1.Start();
                                    break;
                                case "0068":
                                    Task<DSCQM> taskProcCqm0068 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0068(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0068);
                                    taskProcCqm0068.Start();
                                    break;
                                case "0418":
                                    Task<DSCQM> taskProcCqm0418 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0418(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0418);
                                    taskProcCqm0418.Start();
                                    break;
                                case "0419":
                                    Task<DSCQM> taskProcCqm0419 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0419(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0419);
                                    taskProcCqm0419.Start();
                                    break;
                                case "0419(V7)":
                                    Task<DSCQM> taskProcCqm0419_V7 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0419V7(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0419_V7);
                                    taskProcCqm0419_V7.Start();
                                    break;
                                case "0059":
                                    Task<DSCQM> taskProcCqm00591 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0059(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm00591);
                                    taskProcCqm00591.Start();
                                    break;
                                case "0075(A)":
                                    Task<DSCQM> taskProcCqm0075_A = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0075_A);
                                    taskProcCqm0075_A.Start();
                                    Task<DSCQM> taskProcCqm0075_B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0075_B);
                                    taskProcCqm0075_B.Start();
                                    break;
                                case "0075(B)":
                                    Task<DSCQM> taskProcCqm0075_A1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0075_A1);
                                    taskProcCqm0075_A1.Start();
                                    Task<DSCQM> taskProcCqm0075_B1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0075B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0075_B1);
                                    taskProcCqm0075_B1.Start();
                                    break;
                                case "0041":
                                    Task<DSCQM> taskProcCqm0041 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0041(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0041);
                                    taskProcCqm0041.Start();
                                    break;
                                case "0421":
                                    Task<DSCQM> taskProcCqm0421 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0421(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0421);
                                    taskProcCqm0421.Start();
                                    break;
                                case "0421(V6)":
                                    Task<DSCQM> taskProcCqm0421_V6 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0421V6(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm0421_V6);
                                    taskProcCqm0421_V6.Start();
                                    break;
                                case "0421a":
                                    Task<DSCQM> taskProcCqm04211 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0421(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCqm04211);
                                    taskProcCqm04211.Start();
                                    break;
                                case "CMS50v3":
                                    Task<DSCQM> taskProcCQM50 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM50(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM50);
                                    taskProcCQM50.Start();
                                    break;
                                case "0018":
                                    Task<DSCQM> taskProcCQM0018 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0018(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0018);
                                    taskProcCQM0018.Start();
                                    break;
                                case "0018(V6)":
                                    Task<DSCQM> taskProcCQM0018_V6 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0018V6(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0018_V6);
                                    taskProcCQM0018_V6.Start();
                                    break;
                                case "0031":
                                    Task<DSCQM> taskProcCQM0125 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0125(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0125);
                                    taskProcCQM0125.Start();
                                    break;
                                case "0056":
                                    Task<DSCQM> taskProcCQM0123 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0123(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0123);
                                    taskProcCQM0123.Start();
                                    break;
                                case "0034":
                                    Task<DSCQM> taskProcCQM0130 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0130(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0130);
                                    taskProcCQM0130.Start();
                                    break;
                                case "0062":
                                    Task<DSCQM> taskProcCQM0134 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0134(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0134);
                                    taskProcCQM0134.Start();
                                    break;
                                case "0101":
                                    Task<DSCQM> taskProcCQM0139 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0139(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0139);
                                    taskProcCQM0139.Start();
                                    break;
                                case "0065":
                                    Task<DSCQM> taskProcCQM0065 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0065(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0065);
                                    taskProcCQM0065.Start();
                                    break;
                                case "0065(v7)":
                                    Task<DSCQM> taskProcCQM0065_V7 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0065V7(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0065_V7);
                                    taskProcCQM0065_V7.Start();
                                    break;
                                case "2872":
                                    Task<DSCQM> taskProcCQM0149 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0149(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0149);
                                    taskProcCQM0149.Start();
                                    break;
                                case "CMS22v5":
                                    Task<DSCQM> taskProcCQM22 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM22(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM22);
                                    taskProcCQM22.Start();
                                    break;
                                case "CMS22v6":
                                    Task<DSCQM> taskProcCQM22_V6 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM22V6(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM22_V6);
                                    taskProcCQM22_V6.Start();
                                    break;
                                case "0712(A)":
                                    Task<DSCQM> taskProcCQM0160A = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160A);
                                    taskProcCQM0160A.Start();
                                    Task<DSCQM> taskProcCQM0160B = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160B);
                                    taskProcCQM0160B.Start();
                                    Task<DSCQM> taskProcCQM0160C = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160C(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160C);
                                    taskProcCQM0160C.Start();
                                    break;
                                case "0712(B)":

                                    Task<DSCQM> taskProcCQM0160A1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160A1);
                                    taskProcCQM0160A1.Start();
                                    Task<DSCQM> taskProcCQM0160B1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160B1);
                                    taskProcCQM0160B1.Start();
                                    Task<DSCQM> taskProcCQM0160C1 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160C(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160C1);
                                    taskProcCQM0160C1.Start();
                                    break;
                                case "0712(C)":

                                    Task<DSCQM> taskProcCQM0160A2 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160A(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160A2);
                                    taskProcCQM0160A2.Start();
                                    Task<DSCQM> taskProcCQM0160B2 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160B(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160B2);
                                    taskProcCQM0160B2.Start();
                                    Task<DSCQM> taskProcCQM0160C2 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_CQM0160C(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    tasks.Add(taskProcCQM0160C2);
                                    taskProcCQM0160C2.Start();
                                    break;

                                default:
                                    //Task<DSCQM> taskProcCqm04191 = new Task<DSCQM>(() => new DALCQM(SharedVariable).Load_Cqm0419(providerId, NPI, from, to, patientId, reportType, cqmId, TIN, ProviderTypeId, Address, InsurancePlan, EthnicityIds, RaceIds, AgeConditionText, AgeInMonths, Sex, Problems, PageNumber, rpp, eitherDetail));
                                    //tasks.Add(taskProcCqm04191);
                                    //taskProcCqm04191.Start();
                                    break;
                            }
                        }
                    }

                    Task.WaitAll(tasks.ToArray());
                    foreach (var item in tasks)
                    {
                        ds.Merge(item.Result);
                    }
                }
                // var //providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                return new BLObject<DSCQM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCQM::Load_CQM_iTrackDashboard", ex);
                return new BLObject<DSCQM>(null, ex.Message);
            }
        }
    }
}
