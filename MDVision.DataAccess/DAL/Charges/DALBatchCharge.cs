using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Charges
{
    public class DALBatchCharge
    {
        #region Constructors
        public DALBatchCharge()
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

        #region Variable
        
        #endregion

        #region "Charge Batch"

        #region "Stored Procedure Names"

        private const string PROC_BATCHES_SELECT = "Patient.sp_BatchesSelect";
        private const string PROC_BATCHES_SELECT_NEW = "Patient.sp_BatchesSelect_new";
        private const string PROC_BATCHES_INSERT = "Patient.sp_BatchesInsert";
        private const string PROC_BATCHES_UPDATE = "Patient.sp_BatchesUpdate";
        private const string PROC_BATCHES_DELETE = "Patient.sp_BatchesDelete";

        private const string PROC_CHARGE_BATCH_STATUS_LOOKUP = "Patient.sp_ChargeBatchStatusLookup";
        private const string PROC_BATCH_CHARGE_ACTION_LOOKUP = "Patient.sp_ActionLookup";
        private const string PROC_BATCH_CHARGE_REASON_LOOKUP = "Patient.sp_ReasonLookup";

        private const string PROC_BATCH_CLAIM_SELECT = "Patient.sp_BatchClaimSelect";
        private const string PROC_BATCH_CLAIM_DETAIL = "Patient.sp_ChargeBatchClaim_Detail";

        private const string PROC_BATCH_CHARGE_SELECT = "Patient.sp_BatchChargesSelect";
        private const string PROC_BATCH_CHARGE_SELECT_DETAIL = "Patient.sp_BatchChargesSelect_Detail";
        #endregion

        #region "Parameters"

        private const string PARM_BATCH_ID = "@BatchId";
        private const string PARM_BATCH_NUMBER = "@BatchNumber";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_BILLER_ID = "@BillerId";
        private const string PARM_CLAIMS = "@Claims";
        private const string PARM_CHARGES = "@Charges";
        private const string PARM_COPAY_COLLECTED = "@CopaymentCollected";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_CLAIMS_ENTERED = "@ClaimsEntered";
        private const string PARM_CHARGES_ENTERED = "@ChargesEntered";
        private const string PARM_COPAY_POSTED = "@CopaymentPosted";
        private const string PARM_TOTAL_AMOUNT = "@TotalAmount";
        private const string PARM_BATCHSTATUS_ID = "@BatchStatusId";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_TOTAL_HOURS = "@TotalHours";
        private const string PARM_BATCH_ENTERED_BY = "@BatchEnteredBy";
        private const string PARM_BATCH_ENTERED_DATE = "@BatchEntryDate";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTRYDATE_FROM = "@EntryDateFrom";
        private const string PARM_ENTRYDATE_TO = "@EntryDateTo";
        private const string PARM_PRACTICEID = "@PracticeId";
        private const string PARM_ENTERED_BY = "@EnteredBy";

        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSBatchCharge ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(18);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BATCH_ID, ds.Batches.BatchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BATCH_ID, ds.Batches.BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_DESCRIPTION, ds.Batches.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.Batches.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.Batches.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_BILLER_ID, ds.Batches.BillerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_CLAIMS, ds.Batches.ClaimsColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_CHARGES, ds.Batches.ChargesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_COPAY_COLLECTED, ds.Batches.CopaymentCollectedColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(8, PARM_DOS_FROM, ds.Batches.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_DOS_TO, ds.Batches.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_BATCHSTATUS_ID, ds.Batches.BatchStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.Batches.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_CREATED_BY, ds.Batches.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_ON, ds.Batches.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.Batches.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.Batches.ModifiedOnColumn.ColumnName, DbType.DateTime);
            if (IsInsert == true)
                dbManager.AddParameters(16, PARM_BATCH_NUMBER, ds.Batches.BatchNumberColumn.ColumnName, DbType.String, ParamDirection.Output, null, 20);
            else
                dbManager.AddParameters(16, PARM_BATCH_NUMBER, ds.Batches.BatchNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ENTITY_ID, ds.Batches.EntityIdColumn.ColumnName, DbType.Int64);

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSBatchCharge InsertBatchCharge(DSBatchCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSBatchCharge)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BATCHES_INSERT, ds, ds.Batches.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::InsertBatchCharge", PROC_BATCHES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSBatchCharge UpdateBatchCharge(DSBatchCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSBatchCharge)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BATCHES_UPDATE, ds, ds.Batches.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::UpdateBatchCharge", PROC_BATCHES_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSBatchCharge LoadBatchCharge(long BatchId, string BatchNumber, string Description, long FacilityId, long ProviderId, long BillerId, string BatchStatusId, long practiceID, string EnteredBy, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, DateTime? DOSFrom = null, DateTime? DOSTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (BatchNumber == "")
                    BatchNumber = null;

                if (BatchStatusId == "")
                    BatchStatusId = null;

                if (Description == "")
                    Description = null;

                if (EnteredBy == "")
                    EnteredBy = null;
                dbManager.Open();
                dbManager.CreateParameters(18);

                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

                dbManager.AddParameters(1, PARM_BATCH_NUMBER, BatchNumber);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                if (BillerId == 0)
                    dbManager.AddParameters(5, PARM_BILLER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_BILLER_ID, BillerId);

                dbManager.AddParameters(6, PARM_ENTRYDATE_FROM, EntryDateFrom);
                dbManager.AddParameters(7, PARM_ENTRYDATE_TO, EntryDateTo);

                dbManager.AddParameters(8, PARM_BATCHSTATUS_ID, BatchStatusId);

                if (practiceID == 0)
                    dbManager.AddParameters(9, PARM_PRACTICEID, null);
                else
                    dbManager.AddParameters(9, PARM_PRACTICEID, practiceID);

                dbManager.AddParameters(10, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(11, PARM_DOS_TO, DOSTo);
                dbManager.AddParameters(12, PARM_ENTERED_BY, EnteredBy);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(14, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(16, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(16, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(17, PARM_RECORD_COUNT, ds.Batches.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCHES_SELECT, ds, ds.Batches.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchCharge", PROC_BATCHES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBatchCharge BatchChargeSearch(long BatchId, string BatchNumber, string Description, long FacilityId, long ProviderId, long BillerId, string BatchStatusId, long practiceID, string EnteredBy, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, DateTime? DOSFrom = null, DateTime? DOSTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (BatchNumber == "")
                    BatchNumber = null;

                if (BatchStatusId == "")
                    BatchStatusId = null;

                if (Description == "")
                    Description = null;

                if (EnteredBy == "")
                    EnteredBy = null;
                dbManager.Open();
                dbManager.CreateParameters(18);

                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

                dbManager.AddParameters(1, PARM_BATCH_NUMBER, BatchNumber);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                if (BillerId == 0)
                    dbManager.AddParameters(5, PARM_BILLER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_BILLER_ID, BillerId);

                dbManager.AddParameters(6, PARM_ENTRYDATE_FROM, EntryDateFrom);
                dbManager.AddParameters(7, PARM_ENTRYDATE_TO, EntryDateTo);

                dbManager.AddParameters(8, PARM_BATCHSTATUS_ID, BatchStatusId);

                if (practiceID == 0)
                    dbManager.AddParameters(9, PARM_PRACTICEID, null);
                else
                    dbManager.AddParameters(9, PARM_PRACTICEID, practiceID);

                dbManager.AddParameters(10, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(11, PARM_DOS_TO, DOSTo);
                dbManager.AddParameters(12, PARM_ENTERED_BY, EnteredBy);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(14, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(16, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(16, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(17, PARM_RECORD_COUNT, ds.Batches.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCHES_SELECT_NEW, ds, ds.Batches.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::BatchChargeSearch", PROC_BATCHES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteBatchCharge(long BatchId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BATCHES_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::DeleteBatchCharge", PROC_BATCHES_DELETE, ex);
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

        #region Batch Claim And Charge

        public DSBatchCharge LoadBatchClaim(long BatchNumber, long BatchId)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (BatchNumber == 0)
                    dbManager.AddParameters(0, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_NUMBER, BatchNumber);
                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_ID, BatchId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_CLAIM_SELECT, ds, ds.BatchClaimCharge.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchClaim", PROC_BATCH_CLAIM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBatchCharge LoadBatchClaimDetail(long BatchId)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                //if (BatchNumber == 0)
                //    dbManager.AddParameters(0, PARM_BATCH_NUMBER, null);
                //else
                //    dbManager.AddParameters(0, PARM_BATCH_NUMBER, BatchNumber);
                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_CLAIM_DETAIL, ds, ds.BatchClaimCharge.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchClaim", PROC_BATCH_CLAIM_DETAIL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSBatchCharge LoadBatchChargeDetail(long BatchNumber, long BatchId)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (BatchNumber == 0)
                    dbManager.AddParameters(0, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_NUMBER, BatchNumber);
                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_ID, BatchId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_CHARGE_SELECT, ds, ds.BatchClaimCharge.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchChargeDetail", PROC_BATCH_CHARGE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBatchCharge BatchChargeDetailSelect(long BatchId)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                //if (BatchNumber == 0)
                //    dbManager.AddParameters(0, PARM_BATCH_NUMBER, null);
                //else
                //    dbManager.AddParameters(0, PARM_BATCH_NUMBER, BatchNumber);
                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_CHARGE_SELECT_DETAIL, ds, ds.BatchClaimCharge.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchChargeDetail", PROC_BATCH_CHARGE_SELECT_DETAIL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #endregion

        #region "Charge Batch Document"
        #region "Stored Procedure Names"

        private const string PROC_BATCHES_DOCUMENT_INSERT = "Patient.sp_BatchDocumentsInsert";
        private const string PROC_BATCHES_DOCUMENT_SELECT = "Patient.sp_BatchDocumentsSelect";
        private const string PROC_BATCHES_DOCUMENT_UPDATE = "Patient.sp_BatchDocumentsUpdate";
        private const string PROC_BATCHES_DOCUMENT_DELETE = "Patient.sp_BatchDocumentsDelete";
        private const string PROC_BATCHES_AND_PATIENT_DOCUMENT_SELECT = "Patient.sp_BatchAndPatientDocumentsSelect";

        #endregion

        #region "Parameters"

        private const string PARM_BATCH_DOCUMENT_BATCHDOCID = "@BatchDocId";
        private const string PARM_BATCH_DOCUMENT_BATCHID = "@BatchId";
        private const string PARM_BATCH_DOCUMENT_PATIENTID = "@PatientId";
        private const string PARM_BATCH_DOCUMENT_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_BATCH_DOCUMENT_ACTIONID = "@ActionId";
        private const string PARM_BATCH_DOCUMENT_REASONID = "@ReasonId";
        private const string PARM_BATCH_DOCUMENT_COMMENTS = "@Comments";
        private const string PARM_BATCH_DOCUMENT_FILETYPE = "@FileType";
        private const string PARM_BATCH_DOCUMENT_FILEPATH = "@FilePath";
        private const string PARM_BATCH_DOCUMENT_FILESTREAM = "@FileStream";
        private const string PARM_BATCH_DOCUMENT_PAGES = "@Pages";
        private const string PARM_BATCH_DOCUMENT_ISATTACHED = "@isAttached";
        private const string PARM_BATCH_DOCUMENT_ISACTIVE = "@IsActive";
        private const string PARM_BATCH_DOCUMENT_CREATEDBY = "@CreatedBy";
        private const string PARM_BATCH_DOCUMENT_CREATEDON = "@CreatedOn";
        private const string PARM_BATCH_DOCUMENT_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_BATCH_DOCUMENT_MODIFIEDON = "@ModifiedOn";
        private const string PARM_BATCH_DOCUMENT_ISFILESTREAM = "@IsFileStream";
        private const string PARM_BATCH_DOCUMENT_ERRORMESSAGE = "@ErrorMessage";
        private const string PARM_BATCH_DOCUMENT_VISITID = "@VisitId";
        private const string PARM_BATCH_DOCUMENT_CASEID = "@CaseId";
        private const string PARM_BATCH_DOCUMENT_URL = "@Url";



        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void BatchDocumentCreateParameters(IDBManager dbManager, DSBatchCharge ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, ds.BatchDocuments.BatchDocIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, ds.BatchDocuments.BatchDocIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_BATCH_DOCUMENT_BATCHID, ds.BatchDocuments.BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_BATCH_DOCUMENT_PATIENTID, ds.BatchDocuments.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_BATCH_DOCUMENT_CLAIM_NUMBER, ds.BatchDocuments.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_BATCH_DOCUMENT_ACTIONID, ds.BatchDocuments.ActionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_BATCH_DOCUMENT_REASONID, ds.BatchDocuments.ReasonIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_BATCH_DOCUMENT_COMMENTS, ds.BatchDocuments.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_BATCH_DOCUMENT_FILETYPE, ds.BatchDocuments.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_BATCH_DOCUMENT_FILEPATH, ds.BatchDocuments.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_BATCH_DOCUMENT_FILESTREAM, ds.BatchDocuments.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(10, PARM_BATCH_DOCUMENT_PAGES, ds.BatchDocuments.PagesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_BATCH_DOCUMENT_ISATTACHED, ds.BatchDocuments.IsAttachedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_BATCH_DOCUMENT_ISACTIVE, ds.BatchDocuments.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(13, PARM_BATCH_DOCUMENT_CREATEDBY, ds.BatchDocuments.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_BATCH_DOCUMENT_CREATEDON, ds.BatchDocuments.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_BATCH_DOCUMENT_MODIFIEDBY, ds.BatchDocuments.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_BATCH_DOCUMENT_MODIFIEDON, ds.BatchDocuments.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_BATCH_DOCUMENT_VISITID, ds.BatchDocuments.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_BATCH_DOCUMENT_CASEID, ds.BatchDocuments.CaseIdColumn.ColumnName, DbType.Int64);
        }

        private void BatchDocumentCreateParametersInsert(IDBManager dbManager, DSBatchCharge ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(20);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, ds.BatchDocuments.BatchDocIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, ds.BatchDocuments.BatchDocIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_BATCH_DOCUMENT_BATCHID, ds.BatchDocuments.BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_BATCH_DOCUMENT_PATIENTID, ds.BatchDocuments.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_BATCH_DOCUMENT_CLAIM_NUMBER, ds.BatchDocuments.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_BATCH_DOCUMENT_ACTIONID, ds.BatchDocuments.ActionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_BATCH_DOCUMENT_REASONID, ds.BatchDocuments.ReasonIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_BATCH_DOCUMENT_COMMENTS, ds.BatchDocuments.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_BATCH_DOCUMENT_FILETYPE, ds.BatchDocuments.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_BATCH_DOCUMENT_FILEPATH, ds.BatchDocuments.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_BATCH_DOCUMENT_FILESTREAM, ds.BatchDocuments.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(10, PARM_BATCH_DOCUMENT_PAGES, ds.BatchDocuments.PagesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_BATCH_DOCUMENT_ISATTACHED, ds.BatchDocuments.IsAttachedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_BATCH_DOCUMENT_ISACTIVE, ds.BatchDocuments.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(13, PARM_BATCH_DOCUMENT_CREATEDBY, ds.BatchDocuments.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_BATCH_DOCUMENT_CREATEDON, ds.BatchDocuments.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_BATCH_DOCUMENT_MODIFIEDBY, ds.BatchDocuments.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_BATCH_DOCUMENT_MODIFIEDON, ds.BatchDocuments.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_BATCH_DOCUMENT_VISITID, ds.BatchDocuments.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_BATCH_DOCUMENT_CASEID, ds.BatchDocuments.CaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_BATCH_DOCUMENT_URL, ds.BatchDocuments.UrlColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSBatchCharge InsertBatchChargeDocument(DSBatchCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.BatchDocumentCreateParametersInsert(dbManager, ds, true);
                ds = (DSBatchCharge)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BATCHES_DOCUMENT_INSERT, ds, ds.BatchDocuments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::InsertBatchChargeDocument", PROC_BATCHES_DOCUMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSBatchCharge LoadBatchChargeDocument(long BatchDocId, long BatchId, string isFileStream, string VisitId = null)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                if (BatchDocId == 0)
                    dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, BatchDocId);

                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_BATCH_DOCUMENT_BATCHID, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_DOCUMENT_BATCHID, BatchId);

                dbManager.AddParameters(2, PARM_BATCH_DOCUMENT_ISFILESTREAM, isFileStream);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(4, PARM_BATCH_DOCUMENT_VISITID, VisitId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCHES_DOCUMENT_SELECT, ds, ds.BatchDocuments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchChargeDocument", PROC_BATCHES_DOCUMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBatchCharge LoadBatchChargeDocument(string VisitId)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_BATCH_DOCUMENT_VISITID, VisitId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCHES_AND_PATIENT_DOCUMENT_SELECT, ds, ds.BatchDocuments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchChargeDocument", PROC_BATCHES_DOCUMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteBatchChargeDocument(long BatchDocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BATCH_DOCUMENT_BATCHDOCID, BatchDocId);

                dbManager.AddParameters(1, PARM_BATCH_DOCUMENT_ERRORMESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BATCHES_DOCUMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";

                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_DOCUMENT_DELETE);

                //return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::DeleteBatchChargeDocument", PROC_BATCHES_DOCUMENT_DELETE, ex);
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
        public DSBatchCharge UpdateBatchChargeDocument(DSBatchCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.BatchDocumentCreateParameters(dbManager, ds, false);
                ds = (DSBatchCharge)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BATCHES_DOCUMENT_UPDATE, ds, ds.BatchDocuments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::UpdateBatchChargeDocument", PROC_BATCHES_DOCUMENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #endregion

        #region Lookup

        public DSBatchCharge LookupChargeBatchStatus()
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHARGE_BATCH_STATUS_LOOKUP, ds, ds.ChargeBatchStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LookupChargeBatchStatus", PROC_CHARGE_BATCH_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSBatchCharge LookupBatchChargeAction()
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_CHARGE_ACTION_LOOKUP, ds, ds.BatchChargeAction.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LookupBatchChargeAction", PROC_BATCH_CHARGE_ACTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBatchCharge LookupBatchChargeReason()
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_CHARGE_REASON_LOOKUP, ds, ds.BatchChargeReason.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LookupBatchChargeReason", PROC_BATCH_CHARGE_REASON_LOOKUP, ex);
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
