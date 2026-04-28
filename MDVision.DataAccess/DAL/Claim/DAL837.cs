using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using EDIParser;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Claim
{
    public class DAL837
    {
        #region "Variable"

        #endregion

        #region "Constructors"
        public DAL837()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DAL837(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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
        private const string PROC_BILLING_837_CMAIL_LOAD = "Billing.sp_837ClaimLoad";
        private const string PROC_BILLING_837_Header_LOAD = "Billing.sp_837HeaderLoad";
        private const string PROC_BILLING_837_SERVICE_LINE_LOAD = "Billing.837ServicesLineLoad";
        private const string PROC_BILLING_837_SERVICE_LINE_BY_VISIT_ID = "Billing.837SVDLoadByVisitId";
        private const string PROC_BILLING_837_SERVICE_LINE_BY_BATCH_ID = "Billing.837SVDLoadByBatchId";
        private const string PROC_BILLING_CLAIM_SUBMISSION_LOAD = "Billing.ClaimSubmission";
        private const string PROC_BILLING_CLAIM_ICDS = "Billing.sp_HCFAVisitICDS";

        private const string PROC_BILLING_837_NAMES_BY_VISIT_ID = "Billing.BL837NamesByVisitId";
        private const string PROC_BILLING_837_NAMES_BY_BATCH_ID = "Billing.BL837NamesByBatchId";

        private const string PROC_BILLING_837_BATCH_CLAIM_DELETE = "Billing.sp_837BatchClaimDelete";
        private const string PROC_BILLING_837_BATCH_CLAIM_UPDATE = "Billing.sp_837BatchClaimUpdate";
        private const string PROC_BILLING_837_BATCH_CLAIM_SELECT = "Billing.sp_837BatchClaimSelect";
        private const string PROC_BILLING_837_BATCH_CLAIM_INSERT = "Billing.sp_837BatchClaimInsert";
        private const string PROC_BILLING_837_BATCH_CLAIM_DELETE_BY_VISIT_IDS = "Billing.sp_837BatchClaimDeleteByVisitIds";
        

        private const string PROC_BILLING_837_BATCH_DELETE = "Billing.sp_837BatchDelete";
        private const string PROC_BILLING_837_BATCH_UPDATE = "Billing.sp_837BatchUpdate";
        private const string PROC_BILLING_837_BATCH_SELECT = "Billing.sp_837BatchSelect";
        private const string PROC_BILLING_837_BATCH_SELECT_NEW = "Billing.sp_837BatchSelect_New";
        private const string PROC_BILLING_837_BATCH_INSERT = "Billing.sp_837BatchInsert";
        private const string PROC_BILLING_837_VISIT_EXISTS = "Billing.sp_837VisitExists";
        private const string PROC_BILLING_EDI_837_Batch_INSERT = "Billing.sp_EDI837BatchInsert";
        private const string PROC_BILLING_EDI_837_Batch_DELETE = "Billing.sp_EDI837BatchDelete";
        private const string PROC_BILLING_837_BATCH_STATUS_RESUBMIT_UPDATE = "Billing.sp_837BatchStatusResubmitUpdate";



        #endregion

        #region "Parameters"
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_VISIT_IDS = "@VisitIds";
        private const string PARM_BATCH_ID = "@BatchId";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_BATCH_CLAIM_ID = "@837BatchClaimId";
        private const string PARM_837_BATCH_ID = "@837BatchId";
        private const string PARM_BATCH_SUBMIT_MODE = "@SubmitMode";
        private const string PARM_EDI_BATCH_STRING = "@EDI837String";

        private const string PARM_BATCH_CONTROL_NO = "@BatchControlNo";
        private const string PARM_SUBMITTER_NAME = "@SubmitterName";
        private const string PARM_SUBMITTER_EIN = "@SubmitterEIN";
        private const string PARM_RECEIVER_NAME = "@ReceiverName";
        private const string PARM_RECEIVER_EIN = "@ReceiverEIN";
        private const string PARM_LAST_SUBMITTED_DATE = "@LastSubmittedDate";
        private const string PARM_BATCH_STATUS = "@BatchStatus";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SUBMIT_DATE = "@SubmitDate";
        private const string PARM_SUBMIT_BY = "@SubmittedBy";
        private const string PARM_SUBMIT_TYPE = "@SubmitType";
        private const string PARM_IS_RESOLVED = "@IsResolved";
        private const string PARM_IS_COPMLETED = "@IsCompleted";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SEND_STATUS = "@SendStatus";
        private const string PARM_STATUS = "@Status";
        private const string PARM_CLIENT_ID = "@ClientId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_MARK_SUBMITTED = "@MarkSubmitted";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_VIEW_ONLY = "@ViewOnly";

        #endregion

        #region "Support Functions"
        private void Create837BatchClaimParameters(IDBManager dbManager, DS837Batch ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BATCH_CLAIM_ID, ds._837BatchClaim._837BatchClaimIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BATCH_CLAIM_ID, ds._837BatchClaim._837BatchClaimIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VISIT_ID, ds._837BatchClaim.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_837_BATCH_ID, ds._837BatchClaim._837BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_STATUS, ds._837BatchClaim.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PATIENT_INSURANCE_ID, ds._837BatchClaim.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
        }

        private void Create837BatchParameters(IDBManager dbManager, DS837Batch ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(18);

            if (IsInsert == true)
            {

                dbManager.AddParameters(0, PARM_837_BATCH_ID, ds._837Batch._837BatchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_BATCH_CONTROL_NO, ds._837Batch.BatchControlNoColumn.ColumnName, DbType.String, ParamDirection.Output, null, 20);
            }
            else
            {

                dbManager.AddParameters(0, PARM_837_BATCH_ID, ds._837Batch._837BatchIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_BATCH_CONTROL_NO, ds._837Batch.BatchControlNoColumn.ColumnName, DbType.String);
            }
            dbManager.AddParameters(2, PARM_CLEARING_HOUSE_ID, ds._837Batch.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_BATCH_STATUS, ds._837Batch.BatchStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds._837Batch.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds._837Batch.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds._837Batch.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds._837Batch.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds._837Batch.modifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_EDI_BATCH_STRING, ds._837Batch.EDI837StringColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds._837Batch.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_SUBMIT_TYPE, ds._837Batch.SubmitTypeColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(13, PARM_SEND_STATUS, ds._837Batch.SendStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CLIENT_ID, ds._837Batch.ClientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_COMMENTS, ds._837Batch.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MARK_SUBMITTED, ds._837Batch.MarkSubmittedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(17, PARM_IS_COPMLETED, ds._837Batch.IsCompletedColumn.ColumnName, DbType.Boolean);

        }

        private void CreateEDI837BatchParameters(IDBManager dbManager, DS837Batch ds)
        {
            dbManager.CreateParameters(19);


            dbManager.AddParameters(0, PARM_837_BATCH_ID, ds._837Batch._837BatchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_BATCH_CONTROL_NO, ds._837Batch.BatchControlNoColumn.ColumnName, DbType.String, ParamDirection.Output, null, 20);
            dbManager.AddParameters(2, PARM_CLEARING_HOUSE_ID, ds._837Batch.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_BATCH_STATUS, ds._837Batch.BatchStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds._837Batch.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds._837Batch.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds._837Batch.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds._837Batch.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds._837Batch.modifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_EDI_BATCH_STRING, ds._837Batch.EDI837StringColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds._837Batch.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_SUBMIT_TYPE, ds._837Batch.SubmitTypeColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_SEND_STATUS, ds._837Batch.SendStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CLIENT_ID, ds._837Batch.ClientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_COMMENTS, ds._837Batch.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MARK_SUBMITTED, ds._837Batch.MarkSubmittedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(16, PARM_IS_COPMLETED, ds._837Batch.IsCompletedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(17, PARM_VISIT_IDS, ds._837Batch.VisitIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

        }

        #endregion

        #region "837 Claim"
        public DSHCFA Load837Claim(long VisitId, long BatchId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_ID, BatchId);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CMAIL_LOAD, ds, ds._837Claim.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837Claim", PROC_BILLING_837_CMAIL_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA Load837Header(long ClearinghouseId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearinghouseId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_Header_LOAD, ds, ds._837Header.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837Header", PROC_BILLING_837_Header_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA Load837Header(SharedVariable SharedVariable,long ClearinghouseId,string EntityId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearinghouseId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);
                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_Header_LOAD, ds, ds._837Header.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::Load837Header", PROC_BILLING_837_Header_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHCFA Load837ServiceLine(long VisitId, long BatchId, bool ViewOnly = false)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_ID, BatchId);

                dbManager.AddParameters(2, PARM_VIEW_ONLY, ViewOnly);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_SERVICE_LINE_LOAD, ds, ds._837ServiceLine.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837ServiceLine", PROC_BILLING_837_SERVICE_LINE_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA Load837ServiceLineSVDByVisitId(long VisitId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_SERVICE_LINE_BY_VISIT_ID, ds, ds._837SVDServiceLine.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837ServiceLineSVDByVisitId", PROC_BILLING_837_SERVICE_LINE_BY_VISIT_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA Load837ServiceLineSVDByBatchId(long BatchId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_SERVICE_LINE_BY_BATCH_ID, ds, ds._837SVDServiceLine.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837ServiceLineSVDByBatchId", PROC_BILLING_837_SERVICE_LINE_BY_BATCH_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA Load837NamesByBatchId(long BatchId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);
                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_NAMES_BY_BATCH_ID, ds, ds._837Name.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837NamesByBatchId", PROC_BILLING_837_NAMES_BY_BATCH_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA Load837NamesByVisitId(long VisitId)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_NAMES_BY_VISIT_ID, ds, ds._837Name.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837NamesByVisitId", PROC_BILLING_837_NAMES_BY_VISIT_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA LoadClaimSubmission(string VisitIds, long BatchId, bool ViewOnly = false)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(VisitIds))
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitIds);

                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_ID, BatchId);

                dbManager.AddParameters(2, PARM_VIEW_ONLY, ViewOnly);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds._837SVDServiceLine.TableName);
                tableNames.Add(ds._837ServiceLine.TableName);
                tableNames.Add(ds._837Claim.TableName);
                tableNames.Add(ds._837Name.TableName);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_CLAIM_SUBMISSION_LOAD, ds, tableNames);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::LoadClaimSubmission", PROC_BILLING_CLAIM_SUBMISSION_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="VisitIds"></param>
        /// <param name="BatchId"></param>
        /// <param name="ViewOnly"></param>
        /// <returns></returns>
        public DSHCFA LoadClaimSubmission(SharedVariable SharedVariable,string VisitIds, long BatchId, bool ViewOnly = false)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(VisitIds))
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitIds);

                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_ID, BatchId);

                dbManager.AddParameters(2, PARM_VIEW_ONLY, ViewOnly);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds._837SVDServiceLine.TableName);
                tableNames.Add(ds._837ServiceLine.TableName);
                tableNames.Add(ds._837Claim.TableName);
                tableNames.Add(ds._837Name.TableName);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_CLAIM_SUBMISSION_LOAD, ds, tableNames);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::LoadClaimSubmission", PROC_BILLING_CLAIM_SUBMISSION_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHCFA LoadClaimICDS(string VisitIds)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitIds);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_CLAIM_ICDS, ds, ds.VisitICDs.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::LoadClaimICDS", PROC_BILLING_CLAIM_ICDS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="VisitIds"></param>
        /// <returns></returns>
        public DSHCFA LoadClaimICDS(SharedVariable SharedVariable,string VisitIds)
        {

            DSHCFA ds = new DSHCFA();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitIds);

                ds = (DSHCFA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_CLAIM_ICDS, ds, ds.VisitICDs.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::LoadClaimICDS", PROC_BILLING_CLAIM_ICDS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "837 Batch"

        public DS837Batch Load837BatchClaim(long _837BatchId)
        {
            DS837Batch ds = new DS837Batch();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_837_BATCH_ID, _837BatchId);

                ds = (DS837Batch)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_CLAIM_SELECT, ds, ds._837BatchClaim.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837BatchClaim", PROC_BILLING_837_BATCH_CLAIM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DS837Batch Insert837BatchClaim(DS837Batch ds, IDBManager dbManager)
        {
            try
            {
                Create837BatchClaimParameters(dbManager, ds, true);
                ds = (DS837Batch)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_CLAIM_INSERT, ds, ds._837BatchClaim.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Insert837BatchClaim", PROC_BILLING_837_BATCH_CLAIM_INSERT, ex);
                throw ex;
            }
        }

        public DS837Batch Update837BatchClaim(DS837Batch ds, IDBManager dbManager)
        {
            try
            {
                Create837BatchClaimParameters(dbManager, ds, false);
                ds = (DS837Batch)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_CLAIM_UPDATE, ds, ds._837BatchClaim.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Update837BatchClaim", PROC_BILLING_837_BATCH_CLAIM_UPDATE, ex);
                throw ex;
            }
        }

        public string Delete837BatchClaim(long Batch837ClaimId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BATCH_CLAIM_ID, Batch837ClaimId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_CLAIM_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Delete837BatchClaim", PROC_BILLING_837_BATCH_CLAIM_DELETE, ex);
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

        public string Delete837BatchClaimByVisitIds(SharedVariable SharedVariable, string VisitIds, long BatchId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VISIT_IDS, VisitIds);
                dbManager.AddParameters(1, PARM_837_BATCH_ID, BatchId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_CLAIM_DELETE_BY_VISIT_IDS).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::Delete837BatchClaimByVisitIds", PROC_BILLING_837_BATCH_CLAIM_DELETE_BY_VISIT_IDS, ex);
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

        public DS837Batch Load837Batch(long Batch837Id, string BatchNumber, string ClearingHouseId, DateTime? SubmitDate, long SubmittedBy, string SubmitType, string IsCompleted, string BatchStatus, int PageNumber, int RowspPage)
        {
            DS837Batch ds = new DS837Batch();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);
                if (Batch837Id == 0)
                    dbManager.AddParameters(0, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_837_BATCH_ID, Batch837Id);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (BatchNumber == "")
                    dbManager.AddParameters(2, PARM_BATCH_CONTROL_NO, null);
                else
                    dbManager.AddParameters(2, PARM_BATCH_CONTROL_NO, BatchNumber);

                if (ClearingHouseId == "")
                    dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                dbManager.AddParameters(4, PARM_SUBMIT_DATE, SubmitDate);
                if (SubmittedBy == 0)
                    dbManager.AddParameters(5, PARM_SUBMIT_BY, null);
                else
                    dbManager.AddParameters(5, PARM_SUBMIT_BY, SubmittedBy);
                if (SubmitType == "")
                    dbManager.AddParameters(6, PARM_SUBMIT_TYPE, null);
                else
                    dbManager.AddParameters(6, PARM_SUBMIT_TYPE, SubmitType);

                if (IsCompleted == "")
                    dbManager.AddParameters(7, PARM_IS_COPMLETED, null);
                else
                    dbManager.AddParameters(7, PARM_IS_COPMLETED, IsCompleted);


                if (BatchStatus == "")
                    dbManager.AddParameters(8, PARM_BATCH_STATUS, null);
                else
                    dbManager.AddParameters(8, PARM_BATCH_STATUS, BatchStatus);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds._837Batch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DS837Batch)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_SELECT, ds, ds._837Batch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Load837Batch", PROC_BILLING_837_BATCH_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DS837Batch Load837BatchSearch(long Batch837Id, string BatchNumber, string ClearingHouseId, DateTime? SubmitDate, long SubmittedBy, string SubmitType, string IsCompleted, string BatchStatus, int PageNumber, int RowspPage)
        {
            DS837Batch ds = new DS837Batch();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);
                if (Batch837Id == 0)
                    dbManager.AddParameters(0, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_837_BATCH_ID, Batch837Id);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (BatchNumber == "")
                    dbManager.AddParameters(2, PARM_BATCH_CONTROL_NO, null);
                else
                    dbManager.AddParameters(2, PARM_BATCH_CONTROL_NO, BatchNumber);

                if (ClearingHouseId == "")
                    dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                dbManager.AddParameters(4, PARM_SUBMIT_DATE, SubmitDate);
                if (SubmittedBy == 0)
                    dbManager.AddParameters(5, PARM_SUBMIT_BY, null);
                else
                    dbManager.AddParameters(5, PARM_SUBMIT_BY, SubmittedBy);
                if (SubmitType == "")
                    dbManager.AddParameters(6, PARM_SUBMIT_TYPE, null);
                else
                    dbManager.AddParameters(6, PARM_SUBMIT_TYPE, SubmitType);

                if (IsCompleted == "")
                    dbManager.AddParameters(7, PARM_IS_COPMLETED, null);
                else
                    dbManager.AddParameters(7, PARM_IS_COPMLETED, IsCompleted);


                if (BatchStatus == "")
                    dbManager.AddParameters(8, PARM_BATCH_STATUS, null);
                else
                    dbManager.AddParameters(8, PARM_BATCH_STATUS, BatchStatus);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds._837Batch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DS837Batch)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_SELECT_NEW, ds, ds._837Batch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::837BatchSearch", PROC_BILLING_837_BATCH_SELECT_NEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DS837Batch Insert837Batch(DS837Batch ds, IDBManager dbManager)
        {
            try
            {
                Create837BatchParameters(dbManager, ds, true);
                ds = (DS837Batch)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_INSERT, ds, ds._837Batch.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Insert837Batch", PROC_BILLING_837_BATCH_INSERT, ex);
                throw ex;
            }

        }

        public DS837Batch InsertEDI837Batch(SharedVariable SharedVariable,DS837Batch ds)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

            try
            {
                dbManager.Open();
                CreateEDI837BatchParameters(dbManager, ds);
                ds = (DS837Batch)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_837_Batch_INSERT, ds, ds._837Batch.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::InsertEDI837Batch", PROC_BILLING_EDI_837_Batch_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        public string DeleteEDI837Batch(long Batch837Id)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_837_BATCH_ID, Batch837Id);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_EDI_837_Batch_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::DeleteEDI837Batch", PROC_BILLING_EDI_837_Batch_DELETE, ex);
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

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="Batch837Id"></param>
        /// <returns></returns>
        public string DeleteEDI837Batch(SharedVariable SharedVariable,long Batch837Id)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_837_BATCH_ID, Batch837Id);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_EDI_837_Batch_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::DeleteEDI837Batch", PROC_BILLING_EDI_837_Batch_DELETE, ex);
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

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="VisitIds"></param>
        /// <returns></returns>
        public DS837Batch Is837BatchExists(SharedVariable SharedVariable,string VisitIds)
        {
            DS837Batch ds = new DS837Batch();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(VisitIds))
                    dbManager.AddParameters(0, PARM_VISIT_IDS, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_IDS, VisitIds);

                ds = (DS837Batch)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_VISIT_EXISTS, ds, ds._837BatchClaim.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::Load837Batch", PROC_BILLING_837_VISIT_EXISTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DS837Batch Update837Batch(DS837Batch ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                Create837BatchParameters(dbManager, ds, false);
                ds = (DS837Batch)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_UPDATE, ds, ds._837Batch.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Update837Batch", PROC_BILLING_837_BATCH_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DS837Batch Update837Batch(SharedVariable SharedVariable,DS837Batch ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

            try
            {
                dbManager.Open();
                Create837BatchParameters(dbManager, ds, false);
                ds = (DS837Batch)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_UPDATE, ds, ds._837Batch.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DAL837::Update837Batch", PROC_BILLING_837_BATCH_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public string Update837BatchStatusResubmit(int BatchId, int VisitId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (VisitId != 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);

                if (BatchId != 0)
                    dbManager.AddParameters(1, PARM_837_BATCH_ID, BatchId);
                else
                    dbManager.AddParameters(1, PARM_837_BATCH_ID, null);

                returnValue = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_STATUS_RESUBMIT_UPDATE));
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Update837BatchStatusResubmit", PROC_BILLING_837_BATCH_STATUS_RESUBMIT_UPDATE, ex);
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

        public string Dalete837Batch(long Batch837Id)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_837_BATCH_ID, Batch837Id);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL837::Dalete837Batch", PROC_BILLING_837_BATCH_DELETE, ex);
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
    }
}
