using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;


namespace MDVision.DataAccess.DAL.PQRS
{
    public class DALPQRSReports
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPQRSReports()
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
        #region "Stored Procedure Names"

        private const string PROC_PQRS_INDIVIDUAL_REPORT = "Provider.Run_PQRS_Measures";// PQRS_IndividualReport";
        private const string PROC_PQRS_DATAInsert = "Provider.PQRS_DataInsert";
        private const string PROC_PQRS_GPRO_REPORT = "Reports.PQRS_GPROSubmission";
        private const string PROC_PQRS_SUMMARY_REPORT = "Reports.PQRS_SubmissionSummary";
        private const string PROC_PQRS_GET_PATIENTS_FROM_VISITS = "Provider.PQRS_GetPatientsFromVisits";
        private const string PROC_PQRS_GET_MEASURE_REASONS = "Provider.PQRS_GetMeasureReasons";

        #endregion

        #region Parameters
        private const string PARM_PROVIDER_ID = "@ProviderID";
        private const string PARM_PROVIDER_GROUP_ID = "@MeasureGroupId";
        private const string PARM_DATE_FROM = "@From";
        private const string PARM_DATE_TO = "@To";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ERROR_MeasureGroup = "@ErrorMeasureGroup";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_MEASURE_ID = "@MeasureId";
        private const string PARM_VISIT_IDS = "@VisitsID";
        #endregion

        public DSPQRS generateIndividualReport(long ProviderId, string DateFrom, string DateTo)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(3);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                
                if (string.IsNullOrEmpty(DateFrom))
                    dbManager.AddParameters(1, PARM_DATE_FROM, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_DATE_FROM, DateFrom);
                if (string.IsNullOrEmpty(DateTo))
                    dbManager.AddParameters(2, PARM_DATE_TO, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_DATE_TO, DateTo);

            //    dbManager.AddParameters(3, PARM_MEASURE_ID, 1);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PQRS_INDIVIDUAL_REPORT, ds, ds.PQRSReports.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSReports::generateIndividualReport", PROC_PQRS_INDIVIDUAL_REPORT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS generateGPROReport(long ProviderGroupId, string DateFrom, string DateTo)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(3);

                if (ProviderGroupId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, ProviderGroupId);
                if (string.IsNullOrEmpty(DateFrom))
                    dbManager.AddParameters(1, PARM_DATE_FROM, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_DATE_FROM, DateFrom);
                if (string.IsNullOrEmpty(DateTo))
                    dbManager.AddParameters(2, PARM_DATE_TO, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_DATE_TO, DateTo);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PQRS_GPRO_REPORT, ds, ds.PQRSReports.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSReports::generateGPROReport", PROC_PQRS_GPRO_REPORT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPQRS generateSummaryReport(long ProviderId, long ProviderGroupId)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);

                if (ProviderGroupId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, ProviderGroupId);
                if (ProviderId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);
               

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PQRS_SUMMARY_REPORT, ds, ds.PQRSReports.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSReports::generateSummaryReport", PROC_PQRS_SUMMARY_REPORT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS getPatientsFromVisits(string visitIds)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);

                if (string.IsNullOrEmpty(visitIds))
                    dbManager.AddParameters(0, PARM_VISIT_IDS, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_VISIT_IDS, visitIds);
                
                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PQRS_GET_PATIENTS_FROM_VISITS, ds, ds.PQRS_PatientFromVisits.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSReports::getPatientsFromVisits", PROC_PQRS_GET_PATIENTS_FROM_VISITS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS getMeasureReasons(int measureId)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);

                if (measureId<=0)
                    dbManager.AddParameters(0, PARM_MEASURE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_ID, measureId);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PQRS_GET_MEASURE_REASONS, ds, ds.PQRS_MeasureReasons.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPQRSReports::getPatientsFromVisits", PROC_PQRS_GET_MEASURE_REASONS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
