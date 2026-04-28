using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.PQRS
{
    public class DALPQRSexport
    {

        #region "Stored Procedure Names"
        //-----------------------------------------------------------------------------------------------------

        private const string ProcPQRSSelect = "Provider.PQRS";
        private const string ProcPQRSSelectPatientDataSection = "Provider.PQRS_PatientDataSection";

        //private const string ProcPQRS0068Select = "Provider.PQRS_0068_MK";
        //private const string ProcPQRS0018Select = "Provider.PQRS_0018_MK";

        private const string ProcPatientFill = "Patient.sp_PatientFill_CQM";

        private const string ProcPQRSMeasureSection = "Provider.sp_CQM_MeasureSection";
        private const string ProcPQRSReportingParameters = "Provider.sp_CQM_ReportingParameters";


        #region They Will be Gone

        private const string ProcPQRSPatientDataSectionFamilyHx = "Provider.CQM_PatientDataSection_FamilyHx";
        private const string ProcPQRSPatientDataSectionMedicationActive = "Provider.CQM_PatientDataSection_MedicationActive";
        private const string ProcPQRSPatientDataSectionMedicationAdministered = "Provider.CQM_PatientDataSection_MedicationAdministered";

        private const string ProcPQRSPatientDataSectionMedicationAllergy = "Provider.CQM_PatientDataSection_MedicationAllergy";
        private const string ProcPQRSPatientDataSectionMedicationOrder = "Provider.CQM_PatientDataSection_MedicationOrder";

        private const string ProcPQRSPatientDataSectionDiagnosisActiveConcernAct = "Provider.CQM_PatientDataSection_DiagnosisActiveConcernAct";
        private const string ProcPQRSPatientDataSectionProcedureOrder = "Provider.CQM_PatientDataSection_ProcedureOrder";

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

        private const string ParmPQRSid = "@PQRSID";
        private const string ParmPart = "@Part";

        private const string ParmAccountNumber = "@AccountNumber";
        private const string ParmEntityId = "@EntityId";
        private const string ParmUserId = "@UserId";
        private const string ParmIsActive = "@IsActive";

        private const string NqfId = "@NQF_ID";

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Constructors
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public DALPQRSexport()
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

        #region "Get using dataset Functions"
        public DSPQRS Load_PQRS(long providerId, string from, string to, string patientId = null, long reportType = 0, int measureID = -1, int eitherDetail = 0)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

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
                if (measureID < 0)
                {
                    dbManager.AddParameters(5, ParmPQRSid, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, ParmPQRSid, measureID);
                }

                dbManager.AddParameters(6, ParmPart, eitherDetail);

                List<string> tableNames = new List<string>
                {
                ds.PatientPQRS.TableName,
                ds.PQRS_PatientsList.TableName
                   // ds.PQRS.TableName,
                //ds.PQRS_PQRS_Details.TableName
                };

                //ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSSelect, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::Load_PQRS", ProcPQRSSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS Load_PQRS_Details(long providerId, string from, string to, string patientId = null, long reportType = 1, int measureID = -1, int eitherDetail = 0)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

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
                if (measureID < 0)
                {
                    dbManager.AddParameters(5, ParmPQRSid, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, ParmPQRSid, measureID);
                }
                dbManager.AddParameters(6, ParmPart, eitherDetail);

                // ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSSelect, ds, ds.PatientList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::Load_PQRS_Details", ProcPQRSSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS Load_PQRS_Codes(long providerId, string from, string to, string patientId = null, long reportType = 2, int measureID = -1, int eitherDetail = 0)
        {
            var ds = new DSPQRS();
            var dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

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
                if (measureID < 0)
                {
                    dbManager.AddParameters(5, ParmPQRSid, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, ParmPQRSid, measureID);
                }
                dbManager.AddParameters(6, ParmPart, eitherDetail);

                //   ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSSelect, ds, ds.PQRSCodes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::Load_PQRS_Codes", ProcPQRSSelect, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region General Functions
        public DSPQRS FillPatient(long patientId, string accountNo, string isActive)
        {
            DSPQRS ds = new DSPQRS();
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
                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPatientFill, ds, ds.PatientPQRS.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::FillPatient", ProcPatientFill, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Measure Section
        public DSPQRS MeasureSection(long providerId, string nqfId)
        {
            DSPQRS ds = new DSPQRS();
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

                //     ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSMeasureSection, ds, ds.PQRS_MeasureSection.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::MeasureSection", ProcPatientFill, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Reporting Parameters
        public DSPQRS ReportingParameterSection(long providerId, string nqfId)
        {
            DSPQRS ds = new DSPQRS();
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

                //  ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSReportingParameters, ds, ds.Providers_PQRS.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::ReportingParameterSection", ProcPatientFill, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region PatientDataSection
        public DSPQRS PatientDataSection(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                var tableNames = new List<string>
                {
                    //ds.DiagnosisActiveConcernAct.TableName,
                    //ds.FamilyHx.TableName,
                    //ds.MedicationActive.TableName,
                    //ds.MedicationAdministered.TableName,
                    //ds.MedicationAllergy.TableName,
                    //ds.MedicationOrder.TableName,
                    //ds.EncounterPerformed.TableName,
                    //ds.ProcedurePerformed.TableName,
                    //ds.ProcedureOrder.TableName,
                    //ds.PhysicalExam.TableName,
                    //ds.RiskCatagoryAssesment.TableName,
                    //ds.TobbacoUser.TableName,
                    //ds.ProviderToProvider.TableName,
                    //ds.CatagoryIII_PopulationValueSet.TableName
                };
                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSSelectPatientDataSection, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_FamilyHx", ProcPQRSPatientDataSectionFamilyHx, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_FamilyHx(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                //  ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionFamilyHx, ds, ds.FamilyHx.TableName);
                ///del
                List<string> tableNames = new List<string>
                {
                ds.PatientPQRS.TableName,
                ds.PQRS_PatientsList.TableName
                   // ds.PQRS.TableName,
                //ds.PQRS_PQRS_Details.TableName
                };
                ///

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_FamilyHx", ProcPQRSPatientDataSectionFamilyHx, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_MedicationActive(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                //  ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionMedicationActive, ds, ds.MedicationActive.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_MedicationActive", ProcPQRSPatientDataSectionMedicationActive, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_MedicationAdministered(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                //  ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionMedicationAdministered, ds, ds.MedicationAdministered.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_MedicationAdministered", ProcPQRSPatientDataSectionMedicationAdministered, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_MedicationAllergy(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                // ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionMedicationAllergy, ds, ds.MedicationAllergy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_MedicationAllergy", ProcPQRSPatientDataSectionMedicationAllergy, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_MedicationOrder(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                ///   ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionMedicationOrder, ds, ds.MedicationOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_MedicationOrder", ProcPQRSPatientDataSectionMedicationOrder, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_DiagnosisActiveConcernAct(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                //  ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionDiagnosisActiveConcernAct, ds, ds.DiagnosisActiveConcernAct.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_DiagnosisActiveConcernAct", ProcPQRSPatientDataSectionDiagnosisActiveConcernAct, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPQRS PatientDataSection_ProcedureOrder(long patientId, string nqfId = null)
        {
            var ds = new DSPQRS();
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

                // ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, ProcPQRSPatientDataSectionProcedureOrder, ds, ds.ProcedureOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSexport::PatientDataSection_ProcedureOrder", ProcPQRSPatientDataSectionProcedureOrder, ex);
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
