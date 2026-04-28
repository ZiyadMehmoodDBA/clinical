using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Payment
{
    public class DALPaymentBatch
    {

        

        #region Variable
        
        #endregion

        #region Constructors
        public DALPaymentBatch()
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

        #region PAYMENT BATCH

        #region "Stored Procedure Names"

        private const string PROC_PATIENT_PAYMENT_BATCH_INSERT = "Patient.sp_PaymentBatchInsert";
        private const string PROC_PAYMENT_PAYMENT_BATCH_SELECT = "Patient.sp_PaymentBatchSelect";
        private const string PROC_PAYMENT_PAYMENT_BATCH_SEARCH = "Patient.sp_PaymentBatchSearch";
        private const string PROC_PATIENT_PAYMENT_BATCH_UPDATE = "Patient.sp_PaymentBatchUpdate";
        private const string PROC_PATIENT_PAYMENT_BATCH_DELETE = "Patient.sp_PaymentBatchDelete";
        private const string PROC_PATIENT_PAYMENT_BATCH_LOOKUP = "Patient.sp_PaymentBatchLookup";
        private const string PROC_PAYMENT_BY_BATCH = "Billing.sp_PaymentByBatch";
        private const string PROC_INSURANCE_PAYMENT_BY_BATCH = "Billing.sp_InsurancePaymentByBatch";
        
        
        #endregion

        #region "Parameters"

        private const string PARM_PAYMENT_BATCH_ID = "@PmtBatchId";
        private const string PARM_PAYMENT_BATCH_NUMBER = "@PmtBatchNumber";
        private const string PARM_PAYMENT_BATCH_DESCRIPTION = "@Description";
        private const string PARM_PAYMENT_BATCH_PRACTICE_ID = "@PracticeId";
        private const string PARM_PAYMENT_BATCH_FACILITY_ID = "@FacilityId";
        private const string PARM_PAYMENT_BATCH_DEPOSIT_DATE = "@DepositDate";
        private const string PARM_PAYMENT_BATCH_BILLER_ID = "@BillerId";
        private const string PARM_PAYMENT_BATCH_CHECK_DATE = "@CheckDate";
        private const string PARM_PAYMENT_BATCH_PLAN_AMOUNT = "@PlanAmount";
        private const string PARM_PAYMENT_BATCH_PATIENT_AMOUNT = "@PatientAmount";
        private const string PARM_PAYMENT_BATCH_COPAYMENT = "@Copayment";
        private const string PARM_PAYMENT_BATCH_PLAN_AMOUNT_POSTED = "@PlanAmtPosted";
        private const string PARM_PAYMENT_BATCH_PATIENT_AMOUNT_POSTED = "@PatientAmtPosted";
        private const string PARM_PAYMENT_BATCH_COPAY_AMOUNT_POSTED = "@CopayAmtPosted";
        private const string PARM_PAYMENT_BATCH_ADJUSTMENT_AMOUNT = "@AdjustmentAmt";
        private const string PARM_PAYMENT_BATCH_STATUS_ID = "@BatchStatusId";
        private const string PARM_PAYMENT_BATCH_START_DATE = "@StartDate";
        private const string PARM_PAYMENT_BATCH_END_DATE = "@EndDate";
        private const string PARM_PAYMENT_BATCH_TOTAL_HOURS = "@TotalHours";
        private const string PARM_PAYMENT_BATCH_ENTERED_BY = "@BatchEnteredBy";
        private const string PARM_PAYMENT_BATCH_BATCH_CREATED_ON = "@BatchCreatedOn";
        private const string PARM_PAYMENT_BATCH_IS_ACTIVE = "@IsActive";
        private const string PARM_PAYMENT_BATCH_CREATED_BY = "@CreatedBy";
        private const string PARM_PAYMENT_BATCH_CREATED_ON = "@CreatedOn";
        private const string PARM_PAYMENT_BATCH_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_PAYMENT_BATCH_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PAYMENT_BATCH_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAYMENT_BATCH_ENTRYDATE_FROM = "@EntryDateFrom";
        private const string PARM_PAYMENT_BATCH_ENTRYDATE_TO = "@EntryDateTo";
        private const string PARM_PAYMENT_BATCH_ENTRED_BY = "@EnteredBy";
        private const string PARM_PAYMENT_CheckNo = "@CheckNo";

        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERA_ID = "@ERAId";

        private const string PARM_CHECK_NUMBER = "@CheckNumber";
        private const string PARM_PAYMENT_BATCH_URL = "@Url";
        private const string PARM_COMMENTS = "@Comments";
        


        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSPayment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(21);

            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, ds.PaymentBatch.PmtBatchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_PAYMENT_BATCH_NUMBER, ds.PaymentBatch.PmtBatchNumberColumn.ColumnName, DbType.String, ParamDirection.Output , null,50);
            }
            else
            {
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, ds.PaymentBatch.PmtBatchIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_PAYMENT_BATCH_NUMBER, ds.PaymentBatch.PmtBatchNumberColumn.ColumnName, DbType.String);
            }
            dbManager.AddParameters(2, PARM_PAYMENT_BATCH_DESCRIPTION, ds.PaymentBatch.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PAYMENT_BATCH_PRACTICE_ID, ds.PaymentBatch.PracticeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PAYMENT_BATCH_FACILITY_ID, ds.PaymentBatch.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_PAYMENT_BATCH_DEPOSIT_DATE, ds.PaymentBatch.DepositDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_PAYMENT_BATCH_BILLER_ID, ds.PaymentBatch.BillerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_PAYMENT_BATCH_CHECK_DATE, ds.PaymentBatch.CheckDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_PAYMENT_BATCH_PLAN_AMOUNT, ds.PaymentBatch.PlanAmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(9, PARM_PAYMENT_BATCH_PATIENT_AMOUNT, ds.PaymentBatch.PatientAmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(10, PARM_PAYMENT_BATCH_COPAYMENT, ds.PaymentBatch.CopaymentColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(11, PARM_PAYMENT_BATCH_STATUS_ID, ds.PaymentBatch.BatchStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARM_PAYMENT_BATCH_IS_ACTIVE, ds.PaymentBatch.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_PAYMENT_BATCH_CREATED_BY, ds.PaymentBatch.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PAYMENT_BATCH_CREATED_ON, ds.PaymentBatch.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_PAYMENT_BATCH_MODIFIED_BY, ds.PaymentBatch.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_PAYMENT_BATCH_MODIFIED_ON, ds.PaymentBatch.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_ENTITY_ID, ds.PaymentBatch.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_ERA_ID, ds.PaymentBatch.ERAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_CHECK_NUMBER, ds.PaymentBatch.CheckNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_COMMENTS, ds.PaymentBatch.CommentsColumn.ColumnName, DbType.String);
            
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSPayment InsertPaymentBatch(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSPayment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_BATCH_INSERT, ds, ds.PaymentBatch.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::InsertPaymentBatch", PROC_PATIENT_PAYMENT_BATCH_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPayment LoadPaymentBatch(Int64 BatchId, string BatchNumber, string Description, Int64 PracticeId, Int64 FacilityId, Int64 BillerId, int StatusId, string EnteredBy, DateTime? DepositDate = null, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); 
            try
            {
                if (BatchNumber == "")
                    BatchNumber = null;

                if (Description == "")
                    Description = null;

                dbManager.Open();
                dbManager.CreateParameters(16);

                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, BatchId);

                dbManager.AddParameters(1, PARM_PAYMENT_BATCH_NUMBER, BatchNumber);
                dbManager.AddParameters(2, PARM_PAYMENT_BATCH_DESCRIPTION, Description);

                if (PracticeId == 0)
                    dbManager.AddParameters(3, PARM_PAYMENT_BATCH_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PAYMENT_BATCH_PRACTICE_ID, PracticeId);

                if (FacilityId == 0)
                    dbManager.AddParameters(4, PARM_PAYMENT_BATCH_FACILITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PAYMENT_BATCH_FACILITY_ID, FacilityId);

                if (BillerId == 0)
                    dbManager.AddParameters(5, PARM_PAYMENT_BATCH_BILLER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PAYMENT_BATCH_BILLER_ID, BillerId);

               
                    dbManager.AddParameters(6, PARM_PAYMENT_BATCH_DEPOSIT_DATE, DepositDate);

                if (StatusId == 0)
                    dbManager.AddParameters(7, PARM_PAYMENT_BATCH_STATUS_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PAYMENT_BATCH_STATUS_ID, StatusId);

                if (EnteredBy == "")
                    EnteredBy = null;

                dbManager.AddParameters(8, PARM_PAYMENT_BATCH_ENTRYDATE_FROM, EntryDateFrom);
                dbManager.AddParameters(9, PARM_PAYMENT_BATCH_ENTRYDATE_TO, EntryDateTo);
                dbManager.AddParameters(10, PARM_PAYMENT_BATCH_ENTRED_BY, EnteredBy);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(13, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(13, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(14, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(14, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(15, PARM_RECORD_COUNT, ds.PaymentBatch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_PAYMENT_BATCH_SELECT, ds, ds.PaymentBatch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::LoadPaymentBatch", PROC_PAYMENT_PAYMENT_BATCH_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment PaymentBatchSearch(Int64 BatchId, string BatchNumber, string Description, Int64 PracticeId, Int64 FacilityId, Int64 BillerId, int StatusId, string EnteredBy,string checkNo, DateTime? DepositDate = null, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (BatchNumber == "")
                    BatchNumber = null;
                //PRD-26 
                if (checkNo == "")
                    checkNo = null;
                if (Description == "")
                    Description = null;

                dbManager.Open();
                dbManager.CreateParameters(17);

                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, BatchId);

                dbManager.AddParameters(1, PARM_PAYMENT_BATCH_NUMBER, BatchNumber);
                dbManager.AddParameters(2, PARM_PAYMENT_BATCH_DESCRIPTION, Description);
                

                if (PracticeId == 0)
                    dbManager.AddParameters(3, PARM_PAYMENT_BATCH_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PAYMENT_BATCH_PRACTICE_ID, PracticeId);

                if (FacilityId == 0)
                    dbManager.AddParameters(4, PARM_PAYMENT_BATCH_FACILITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PAYMENT_BATCH_FACILITY_ID, FacilityId);

                if (BillerId == 0)
                    dbManager.AddParameters(5, PARM_PAYMENT_BATCH_BILLER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PAYMENT_BATCH_BILLER_ID, BillerId);


                dbManager.AddParameters(6, PARM_PAYMENT_BATCH_DEPOSIT_DATE, DepositDate);

                if (StatusId == 0)
                    dbManager.AddParameters(7, PARM_PAYMENT_BATCH_STATUS_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PAYMENT_BATCH_STATUS_ID, StatusId);

                if (EnteredBy == "")
                    EnteredBy = null;

                dbManager.AddParameters(8, PARM_PAYMENT_BATCH_ENTRYDATE_FROM, EntryDateFrom);
                dbManager.AddParameters(9, PARM_PAYMENT_BATCH_ENTRYDATE_TO, EntryDateTo);
                dbManager.AddParameters(10, PARM_PAYMENT_BATCH_ENTRED_BY, EnteredBy);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(13, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(13, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(14, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(14, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(15, PARM_RECORD_COUNT, ds.PaymentBatch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(16, PARM_CHECK_NUMBER,checkNo);
                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_PAYMENT_BATCH_SEARCH, ds, ds.PaymentBatch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::PaymentBatchSearch", PROC_PAYMENT_PAYMENT_BATCH_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment UpdatePaymentBatch(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPayment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_BATCH_UPDATE, ds, ds.PaymentBatch.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::UpdatePaymentBatch", PROC_PATIENT_PAYMENT_BATCH_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeletePaymentBatch(Int64 BatchId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, BatchId);

                dbManager.AddParameters(1, PARM_PAYMENT_BATCH_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 256);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_BATCH_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::DeletePaymentBatch", PROC_PATIENT_PAYMENT_BATCH_DELETE, ex);
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

        #endregion

        #region PAYMENT BATCH DOCUMENT

        #region "Stored Procedure Names"

        private const string PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_INSERT = "Patient.sp_PaymentBatchDocsInsert";
        private const string PROC_PAYMENT_PAYMENT_BATCH_DOCUMENTS_SELECT = "Patient.sp_PaymentBatchDocsSelect";
        private const string PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_UPDATE = "Patient.sp_PaymentBatchDocsUpdate";
        private const string PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_DELETE = "Patient.sp_PaymentBatchDocsDelete";


        #endregion

        #region "Parameters"

        private const string PARM_PAYMENT_BATCH_DOCUMENT_ID = "@PmtBthDocId";
        private const string PARM_PAYMENT_BATCHID = "@PmtBatchId";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_CHECK_NUMBER = "@CheckNumber";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_COMMENTS = "@Comments";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_ACTIONID = "@ActionId";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_REASONID = "@ReasonId";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_FILETYPE = "@FileType";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_FILEPATH = "@FilePath";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_FILESTREAM = "@FileStream";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_PAGES = "@Pages";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_ISACTIVE = "@IsActive";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_CREATED_BY = "@CreatedBy";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_CREATED_ON = "@CreatedOn";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_IS_FILESTREAM = "@IsFileStream";
        private const string PARM_PAYMENT_BATCH_DOCUMENT_CHECK_DATE = "@CheckDate";

        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void PaymentBatchCreateParameters(IDBManager dbManager, DSPayment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(16);

            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, ds.PaymentBatchDocs.PmtBthDocIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, ds.PaymentBatchDocs.PmtBthDocIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARM_PAYMENT_BATCHID, ds.PaymentBatchDocs.PmtBatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PAYMENT_BATCH_DOCUMENT_CHECK_NUMBER, ds.PaymentBatchDocs.CheckNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PAYMENT_BATCH_DOCUMENT_COMMENTS, ds.PaymentBatchDocs.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PAYMENT_BATCH_DOCUMENT_ACTIONID, ds.PaymentBatchDocs.ActionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_PAYMENT_BATCH_DOCUMENT_REASONID, ds.PaymentBatchDocs.ReasonIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_PAYMENT_BATCH_DOCUMENT_FILETYPE, ds.PaymentBatchDocs.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PAYMENT_BATCH_DOCUMENT_FILEPATH, ds.PaymentBatchDocs.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PAYMENT_BATCH_DOCUMENT_FILESTREAM, ds.PaymentBatchDocs.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(9, PARM_PAYMENT_BATCH_DOCUMENT_PAGES, ds.PaymentBatchDocs.PagesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_PAYMENT_BATCH_DOCUMENT_ISACTIVE, ds.PaymentBatchDocs.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_PAYMENT_BATCH_CREATED_BY, ds.PaymentBatchDocs.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PAYMENT_BATCH_CREATED_ON, ds.PaymentBatchDocs.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_PAYMENT_BATCH_MODIFIED_BY, ds.PaymentBatchDocs.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PAYMENT_BATCH_MODIFIED_ON, ds.PaymentBatchDocs.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_PAYMENT_BATCH_CHECK_DATE, ds.PaymentBatchDocs.CheckDateColumn.ColumnName, DbType.DateTime);
        }

        private void PaymentBatchCreateParametersInsert(IDBManager dbManager, DSPayment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(17);

            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, ds.PaymentBatchDocs.PmtBthDocIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, ds.PaymentBatchDocs.PmtBthDocIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARM_PAYMENT_BATCHID, ds.PaymentBatchDocs.PmtBatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PAYMENT_BATCH_DOCUMENT_CHECK_NUMBER, ds.PaymentBatchDocs.CheckNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PAYMENT_BATCH_DOCUMENT_COMMENTS, ds.PaymentBatchDocs.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PAYMENT_BATCH_DOCUMENT_ACTIONID, ds.PaymentBatchDocs.ActionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_PAYMENT_BATCH_DOCUMENT_REASONID, ds.PaymentBatchDocs.ReasonIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_PAYMENT_BATCH_DOCUMENT_FILETYPE, ds.PaymentBatchDocs.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PAYMENT_BATCH_DOCUMENT_FILEPATH, ds.PaymentBatchDocs.FilePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PAYMENT_BATCH_DOCUMENT_FILESTREAM, ds.PaymentBatchDocs.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(9, PARM_PAYMENT_BATCH_DOCUMENT_PAGES, ds.PaymentBatchDocs.PagesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_PAYMENT_BATCH_DOCUMENT_ISACTIVE, ds.PaymentBatchDocs.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_PAYMENT_BATCH_CREATED_BY, ds.PaymentBatchDocs.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PAYMENT_BATCH_CREATED_ON, ds.PaymentBatchDocs.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_PAYMENT_BATCH_MODIFIED_BY, ds.PaymentBatchDocs.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PAYMENT_BATCH_MODIFIED_ON, ds.PaymentBatchDocs.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_PAYMENT_BATCH_CHECK_DATE, ds.PaymentBatchDocs.CheckDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_PAYMENT_BATCH_URL, ds.PaymentBatchDocs.UrlColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        public DSPayment InsertPaymentBatchDocument(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.PaymentBatchCreateParametersInsert(dbManager, ds, true);
                ds = (DSPayment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_INSERT, ds, ds.PaymentBatchDocs.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::InsertPaymentBatchDocument", PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment LoadPaymentBatchDocument(Int64 BatchDocId , Int64 BatchId , string isFileStream)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (BatchDocId == 0)
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, BatchDocId);

                if (BatchId == 0)
                    dbManager.AddParameters(1, PARM_PAYMENT_BATCHID, null);
                else
                    dbManager.AddParameters(1, PARM_PAYMENT_BATCHID, BatchId);

                dbManager.AddParameters(2, PARM_PAYMENT_BATCH_DOCUMENT_IS_FILESTREAM, isFileStream);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_PAYMENT_BATCH_DOCUMENTS_SELECT, ds, ds.PaymentBatchDocs.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::LoadPaymentBatchDocument", PROC_PAYMENT_PAYMENT_BATCH_DOCUMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeletePaymentBatchDocument(Int64 BatchDocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PAYMENT_BATCH_DOCUMENT_ID, BatchDocId);
                dbManager.AddParameters(1, PARM_PAYMENT_BATCH_DOCUMENT_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 256);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::DeletePaymentBatchDocument", PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_DELETE, ex);
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
        public DSPayment UpdatePaymentBatchDocument(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.PaymentBatchCreateParameters(dbManager, ds, false);
                ds = (DSPayment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_UPDATE, ds, ds.PaymentBatchDocs.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::UpdatePaymentBatchDocument", PROC_PATIENT_PAYMENT_BATCH_DOCUMENTS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #endregion

        #region Payment By Batch



        public DSPayment LoadInsurancePaymentByBatch(Int64 BatchId)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, BatchId);

                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PAYMENT_BY_BATCH, ds, ds.PaymentByBatch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::LoadInsurancePaymentByBatch", PROC_INSURANCE_PAYMENT_BY_BATCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPayment LoadPaymentByBatch(Int64 BatchId)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (BatchId == 0)
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAYMENT_BATCH_ID, BatchId);

                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_BY_BATCH, ds, ds.PaymentByBatch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPaymentBatch::LoadPaymentByBatch", PROC_PAYMENT_BY_BATCH, ex);
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
