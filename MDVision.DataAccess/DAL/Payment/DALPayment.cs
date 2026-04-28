using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Billing.Payments;
using MDVision.Model.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Payment
{
    public class DALPayment
    {

        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CLAIM_SUMMARY = "Patient.ClaimSummary";
        private const string PROC_DASHBOARD_TCMPATIENTS = "System.sp_Dashboard_TCMPatients";
        private const string PROC_PATIENT_PAYMENT_INSERT = "Patient.sp_PatientPaymentsInsert";
        private const string PROC_PAYMENT_BATCH_LOOKUP = "Patient.sp_PaymentBatchLookup";

        private const string PROC_PATIENT_PAYMENT_SELECT = "Patient.sp_PatientPaymentsSelect";
        private const string PROC_PATIENT_PAYMENT_RECEIPTINFO_SELECT = "Patient.sp_PatientPaymentsReceiptInfoSelect";

        private const string PROC_CREDIT_CARD_TYPE_LOOKUP = "Patient.sp_CreditCardTypeLookup";

        private const string PROC_COPAY_BY_APPID = "Patient.sp_PatientCopayByIds";


        private const string PROC_DASHBOARD_PAYMENT = "Billing.DashboardPayments";
        private const string PROC_DASHBOARD_COPAY = "Patient.sp_DashBoardCopayment";

        private const string PROC_PATIENT_AUTO_PATYMENT_POST = "Patient.sp_AutoPaymentPost";
        private const string PROC_PATIENT_AUTO_PATYMENT_POST_NEW = "Patient.sp_AutoPaymentPost_New";
        private const string PROC_ACCOUNT_LEDGER_UPDATE = "Patient.sp_PatientPaymentUpdateComments";

        private const string PROC_IS_CHECK_POSTED = "Billing.sp_IsCheckPosted";
        private const string PROC_D_PAYMENT = "Billing.sp_D_Payments";

        private const string PROC_UNALLOCATED_COPAY_INSERT = "Billing.sp_UnAllocatedCopayInsert";
        private const string PROC_UNALLOCATED_COPAY_UPDATE = "Billing.sp_UnAllocatedCopayUpdate";
        private const string PROC_UNALLOCATED_COPAY_SELECT = "Billing.sp_UnAllocatedCopaySelect";
        private const string PROC_UNALLOCATED_COPAY_DELETE = "Billing.sp_UnAllocatedCopayDelete";
       
        

        #endregion

        #region "Parameters"

        private const string PARM_PAYMENT_ID = "@PaymentId";
        private const string PARM_CHARGE_ID = "@ChargeId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_ADVANCE_PAYMENT_ID = "@AdvPmtId";
        private const string PARM_UNALLOCATED_COPAY_ID = "@UnAllocatedCopayId";

        private const string PARM_PAYMENT_BATCH_ID = "@PmtBatchId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PAYMENT_DATE = "@PaymentDate";
        private const string PARM_PAID_AMOUNT_CR = "@PaidAmountCr";
        private const string PARM_PAID_AMOUNT_DR = "@PaidAmountDr";
        private const string PARM_PAYMENT_TYPE_ID = "@PmtTypeId";
        private const string PARM_LEDGER_ACCOUNT_ID = "@LedgerAccId";
        private const string PARM_REMIT_CODE_ID = "@RemitCodeId";
        private const string PARM_NEXT_RESPONSIBILITY_ID = "@NextResponsibilityId";
        private const string PARM_CROSS_OVER = "@CrossOver";
        private const string PARM_ICNDCN = "@ICNDCN";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_MASTER_PAYMENT_ID = "@MasterPaymentId";
        private const string PARM_CHECK_NO = "@CheckNo";
        private const string PARM_CHECK_DATE = "@CheckDate";
        private const string PARM_EXPIRY_DATE = "@ExpiryDate";
        private const string PARM_RECEIPT_DATE = "@ReceiptDate";
        private const string PARM_RECEIPT_DATE_FROM = "@ReceiptDateFrom";
        private const string PARM_RECEIPT_DATE_TO = "@ReceiptDateTo";
        private const string PARM_RECEIPT_NUMBER = "@ReceiptNumber";
        private const string PARM_CARD_TYPE_ID = "@CardTypeId";

        private const string PARM_ALLOWED = "@Allowed";
        private const string PARM_COPAY = "@Copay";
        private const string PARM_COPAY_AMOUNT = "@CopayAmount";
        private const string PARM_COPAY_STATUS = "@Status";
        private const string PARM_NEXT_RESPONSIBILITY = "@NextResponsibility";

        private const string PARM_COMMENTS = "@Comments";
        // private const string PARM_TRANSFER_AMOUNT = "@TransferAmount";

        private const string PARM_COINSURANCE = "@Coinsurance";
        private const string PARM_DEDUCTABLES = "@Deductables";
        private const string PARM_PATIENT_RESPONSIBILITY = "@PatientResponsibility";

        private const string PARM_ERADTL_ID = "@ERADtlId";
        private const string PARM_CLEARNING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_ERA_ID = "@ERAIds";

        private const string PARM_VISIT_ID = "@VisitID";
        private const string PARM_ENTITY = "@EntityId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAID_FROM = "@PaidFrom";
        private const string PARM_PAID_TO = "@PaidTo";
        private const string PARM_IS_DENIED = "@IsDenied";
        private const string PARM_PRINT_ON_PAT_STM = "@PrintOnPatStmt";
        private const string PARM_IS_RECOUPMENT = "@IsRecoupment";

        private const string PARM_PATIENT_INSURANCE_PLAN_ID = "@PatientInsurancePlanId";
        public const string PARM_PATIENT_INSURANCE_PLAN_PRIORITY = "@InsurancePlanPriority";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_BATCH_ID = "@BatchId";
        


        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_MODULE = "@Module";

        private const string PARM_ROWS_PE_PAGE = "@RowsperPage";
        private const string PARM_PAGE_N0 = "@PageNo";
        #endregion

        #region Constructors
        public DALPayment()
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

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSPayment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(41);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PAYMENT_ID, ds.PatientPayments.PaymentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PAYMENT_ID, ds.PatientPayments.PaymentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CHARGE_ID, ds.PatientPayments.ChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientPayments.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_APPOINTMENT_ID, ds.PatientPayments.AppointmentIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(4, PARM_ADVANCE_PAYMENT_ID, ds.PatientPayments.AdvPmtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_VISIT_ID, ds.PatientPayments.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_PAYMENT_BATCH_ID, ds.PatientPayments.PmtBatchIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(7, PARM_PROVIDER_ID, ds.PatientPayments.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_FACILITY_ID, ds.PatientPayments.FacilityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(9, PARM_PAYMENT_DATE, ds.PatientPayments.PaymentDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(10, PARM_PAID_AMOUNT_CR, ds.PatientPayments.PaidAmountCrColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(11, PARM_PAID_AMOUNT_DR, ds.PatientPayments.PaidAmountDrColumn.ColumnName, DbType.Double);

            dbManager.AddParameters(12, PARM_PAYMENT_TYPE_ID, ds.PatientPayments.PmtTypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_LEDGER_ACCOUNT_ID, ds.PatientPayments.LedgerAccIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(14, PARM_REMIT_CODE_ID, ds.PatientPayments.RemitCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_NEXT_RESPONSIBILITY_ID, ds.PatientPayments.NextResponsibilityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(16, PARM_CROSS_OVER, ds.PatientPayments.CrossOverColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARM_ICNDCN, ds.PatientPayments.ICNDCNColumn.ColumnName, DbType.String);

            dbManager.AddParameters(18, PARM_IS_ACTIVE, ds.PatientPayments.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(19, PARM_CREATED_BY, ds.PatientPayments.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_CREATED_ON, ds.PatientPayments.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_MODIFIED_BY, ds.PatientPayments.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_MODIFIED_ON, ds.PatientPayments.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(23, PARM_MASTER_PAYMENT_ID, ds.PatientPayments.MasterPaymentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(24, PARM_CHECK_NO, ds.PatientPayments.CheckNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CHECK_DATE, ds.PatientPayments.CheckDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_EXPIRY_DATE, ds.PatientPayments.ExpiryDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_CARD_TYPE_ID, ds.PatientPayments.CardTypeIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(28, PARM_ALLOWED, ds.PatientPayments.AllowedColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(29, PARM_COPAY, ds.PatientPayments.CopayColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(30, PARM_NEXT_RESPONSIBILITY, ds.PatientPayments.NextResponsibilityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(31, PARM_COMMENTS, ds.PatientPayments.CommentsColumn.ColumnName, DbType.String);

            //dbManager.AddParameters(32, PARM_TRANSFER_AMOUNT, ds.PatientPayments.TransferAmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(32, PARM_COINSURANCE, ds.PatientPayments.CoinsuranceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(33, PARM_DEDUCTABLES, ds.PatientPayments.DeductablesColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(34, PARM_PATIENT_RESPONSIBILITY, ds.PatientPayments.PatientResponsibilityColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(35, PARM_IS_DENIED, ds.PatientPayments.IsDeniedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(36, PARM_PRINT_ON_PAT_STM, ds.PatientPayments.PrintOnPatStmtColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(37, PARM_IS_RECOUPMENT, ds.PatientPayments.IsRecoupmentColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(38, PARM_PATIENT_INSURANCE_PLAN_ID, ds.PatientPayments.PatientInsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(39, PARM_INSURANCE_PLAN_ID, ds.PatientPayments.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(40, PARM_PATIENT_INSURANCE_PLAN_PRIORITY, ds.PatientPayments.InsurancePlanPriorityColumn.ColumnName, DbType.Int32);

        }
        #endregion

        #region "Functions Patient Payments"

        public DSPayment InsertPatientPayments(DSPayment ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSDBAudit dsDBAuditCrossOver = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.PatientPayments.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);

                ds = (DSPayment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_INSERT, ds, ds.PatientPayments.TableName);


                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientPayments.Rows[0][ds.PatientPayments.PaymentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }


                if (ds.PatientPayments.Select(ds.PatientPayments.CrossOverColumn.ColumnName + "= True").Count() > 0)
                {
                    dsDBAuditCrossOver = new DBActivityAudit().InsertDBAuditOfPaymentCrossOver(ds, dbManager);
                    dsDBAuditCrossOver.AcceptChanges();
                }

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_PAYMENT, false, "Error While inserting the Payment : " + ex.ToString());
                MDVLogger.DALErrorLog("DALPayment::InsertPatientPayments", PROC_PATIENT_PAYMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment InsertPatientPaymentsAudit(DataTable dtTemp, DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DSDBAudit dsDBAuditCrossOver = new DSDBAudit();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientPayments.Rows[0][ds.PatientPayments.PaymentIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }

                if (ds.PatientPayments.Select(ds.PatientPayments.CrossOverColumn.ColumnName + "= True").Count() > 0)
                {
                    dsDBAuditCrossOver = new DBActivityAudit().InsertDBAuditOfPaymentCrossOver(ds, dbManager);
                    dsDBAuditCrossOver.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::InsertPatientPaymentsAudit", "InsertPatientPaymentsAudit", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPayment InsertPatientPayments(DSPayment ds, IDBManager dbManager)
        {
            // IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //  dbManager.Open();
                
                CreateParameters(dbManager, ds, true);
                ds = (DSPayment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_INSERT, ds, ds.PatientPayments.TableName);
                ds.AcceptChanges();
               
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::InsertPatientPayments", PROC_PATIENT_PAYMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                //  dbManager.Dispose();
            }
        }

        public DSPayment LoadPatientPayments(long PaymentId, long AppointmentId, long VisitId, long ChargeId, string Module = "")
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Module == "")
                    Module = null;

                dbManager.Open();
                dbManager.CreateParameters(6);

                if (PaymentId <= 0)
                    dbManager.AddParameters(0, PARM_PAYMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PAYMENT_ID, PaymentId);
                if (AppointmentId <= 0)
                    dbManager.AddParameters(1, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_APPOINTMENT_ID, AppointmentId);
                if (VisitId <= 0)
                    dbManager.AddParameters(2, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_VISIT_ID, VisitId);
                if (ChargeId <= 0)
                    dbManager.AddParameters(3, PARM_CHARGE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_CHARGE_ID, ChargeId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(4, PARM_ENTITY, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY, MDVSession.Current.EntityId);
                dbManager.AddParameters(5, PARM_MODULE, Module);

                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PAYMENT_SELECT, ds, ds.PatientPayments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadPatientPayments", PROC_PATIENT_PAYMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment LoadPatientCopayByIds(long AppointmentId, long VisitId)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (AppointmentId <= 0)
                    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_APPOINTMENT_ID, AppointmentId);
                if (VisitId <= 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);
                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COPAY_BY_APPID, ds, ds.PatientCopayments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadPatientCopayByIds", PROC_COPAY_BY_APPID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment LoadDashBoardPayments(long Entity, long PageNumber, long RowsPerPage)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);

                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.DashBoardCopay.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_PAYMENT, ds, ds.DashBoardPayments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadDashBoardPayments", PROC_DASHBOARD_PAYMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment LoadDashBoardCopay(long Entity, DateTime? PaidForm, DateTime? PaidTo, int PageNumber, int RowspPage)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_PAID_FROM, PaidForm);
                dbManager.AddParameters(2, PARM_PAID_TO, PaidTo);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowspPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.DashBoardCopay.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_COPAY, ds, ds.DashBoardCopay.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadDashBoardCopay", PROC_DASHBOARD_COPAY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA AutoPaymentPost(IDBManager dbManager, long ERADtlId, string CreatedBy, DateTime CreateOn, string ModifiedBy, DateTime ModifiedOn)
        {
            DSERA ds = new DSERA();
            try
            {
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_ERADTL_ID, ERADtlId);
                dbManager.AddParameters(1, PARM_CREATED_BY, CreatedBy);
                dbManager.AddParameters(2, PARM_CREATED_ON, CreateOn);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, ModifiedOn);


                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_AUTO_PATYMENT_POST, ds, ds.ERADetail.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::AutoPaymentPost", PROC_PATIENT_AUTO_PATYMENT_POST, ex);
                throw ex;
            }
        }
        public DSERA AutoPaymentPostNew(IDBManager dbManager, long ERAId, DataTable ERADtlId, string CreatedBy, DateTime CreateOn, string ModifiedBy, DateTime ModifiedOn)
        {
            DSERA ds = new DSERA();
            try
            {
                
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_ERADTL_ID, ERADtlId);
                dbManager.AddParameters(1, PARM_ERA_ID, ERAId);
                dbManager.AddParameters(2, PARM_CREATED_BY, CreatedBy);
                dbManager.AddParameters(3, PARM_CREATED_ON, CreateOn);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(5, PARM_MODIFIED_ON, ModifiedOn);
                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_AUTO_PATYMENT_POST_NEW, ds, ds.ERADetail.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::AutoPaymentPostNew", PROC_PATIENT_AUTO_PATYMENT_POST_NEW, ex);
                throw ex;
            }
           
        }


        public DSPayment UpdatePayment(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                Int64 EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                dbManager.Open();
                dbManager.CreateParameters(11);
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientPayments.GetChanges();
                dbManager.AddParameters(0, PARM_PAYMENT_ID, ds.PatientPayments.PaymentIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_COMMENTS, ds.PatientPayments.CommentsColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_PRINT_ON_PAT_STM, ds.PatientPayments.PrintOnPatStmtColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(3, PARM_CHECK_DATE, ds.PatientPayments.CheckDateColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_CHECK_NO, ds.PatientPayments.CheckNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_EXPIRY_DATE, ds.PatientPayments.ExpiryDateColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_PAYMENT_TYPE_ID,ds.PatientPayments.PmtTypeIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(7, PARM_CARD_TYPE_ID, ds.PatientPayments.CardTypeIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(8, PARM_ENTITY, ds.PatientPayments.EntityIdColumn.ColumnName,DbType.Int64);
                dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.PatientPayments.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10,PARM_MODIFIED_ON, ds.PatientPayments.ModifiedOnColumn.ColumnName, DbType.String);
                ds = (DSPayment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ACCOUNT_LEDGER_UPDATE, ds, ds.PatientPayments.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientPayments.Rows[0][ds.PatientPayments.PaymentIdColumn].ToString(), "");
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::UpdatePatientPayments", PROC_ACCOUNT_LEDGER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string IsCheckAlreadyPosted(string checkNo, Int64 chargeId = 0)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CHECK_NO, checkNo);
                if (chargeId <= 0)
                {
                    dbManager.AddParameters(1, PARM_CHARGE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_CHARGE_ID, chargeId);
                }
                //dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_CHECK_POSTED).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::IsCheckPosted", PROC_IS_CHECK_POSTED, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                {
                    return str[1].ToString();
                }
                else
                {
                    return ex.Message;
                }
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the Payment Type.
        /// </summary>
        /// <returns>DSPaymentLookup.</returns>
        public DSPaymentLookup LookupPaymentBatch(string BatchNumber)
        {
            DSPaymentLookup ds = new DSPaymentLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (MDVSession.Current.IsAdmin)
                    dbManager.AddParameters(1, PARM_ENTITY, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_BATCH_ID, BatchNumber);
                


                ds = (DSPaymentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_BATCH_LOOKUP, ds, ds.PaymentBatch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LookupPaymentBatch", PROC_PAYMENT_BATCH_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPaymentLookup LookupCreditCardType()
        {
            DSPaymentLookup ds = new DSPaymentLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPaymentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CREDIT_CARD_TYPE_LOOKUP, ds, ds.CreditCardType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LookupCreditCardType", PROC_CREDIT_CARD_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region Dashboard Payments
        public List<DPaymentModel> LoadDashboardPayments(long Entity, long PageNumber, long RowsPerPage)
        {
            List<DPaymentModel> listModel = new List<DPaymentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);

                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(4, PARM_User_ID, MDVSession.Current.AppUserId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_D_PAYMENT);
                DPaymentModel model = null;
                while (reader.Read())
                {
                    model = new DPaymentModel();
                    model.PracticeName = Convert.ToString(reader["PracticeName"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.FacilityName = Convert.ToString(reader["FacilityName"]);
                    model.InsurancePlan = Convert.ToString(reader["InsurancePlan"]);
                    model.InsurancePaid = Convert.ToString(reader["InsurancePaid"]);
                    model.PatientPaid = Convert.ToString(reader["PatientPaid"]);
                    model.CopayPaid = Convert.ToString(reader["CopayPaid"]);
                    model.ClaimNumber = Convert.ToString(reader["ClaimNumber"]);
                    model.PatientId = Convert.ToString(reader["Patientid"]);
                    model.VisitId = Convert.ToString(reader["VisitId"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadDashboardPayments", PROC_D_PAYMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<DTCMModel> LoadDashboardTCMPatients(long patientId, long providerId,string Status, long PageNumber, long RowsPerPage)
        {
            List<DTCMModel> listModel = new List<DTCMModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_User_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY, MDVSession.Current.EntityId);

                if (patientId > 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null); 

                if (providerId > 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, providerId);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                dbManager.AddParameters(4, PARM_STATUS, Status);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_TCMPATIENTS);
                DTCMModel model = null;
                while (reader.Read())
                {
                    model = new DTCMModel();
                    model.PatientId = Convert.ToString(reader["PatientId"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.Provider = Convert.ToString(reader["PatientProviderName"]);
                    model.Insurance = Convert.ToString(reader["Insurance"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.Status = Convert.ToString(reader["Status"]);
                    model.DateOfAppointment = model.Status == "Signed" ? Convert.ToString(reader["AppointmentDate"]):"Does not apply";
                    model.DischargeDate = Convert.ToString(reader["DischargeDate"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadDashboardPayments", PROC_D_PAYMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Dashboard Copayment
        public List<DCopaymentModel> LoadDashboardCopay(long Entity, DateTime? PaidForm, DateTime? PaidTo, int PageNumber, int RowspPage)
        {
            List<DCopaymentModel> listModel = new List<DCopaymentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_PAID_FROM, PaidForm);
                dbManager.AddParameters(2, PARM_PAID_TO, PaidTo);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowspPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DASHBOARD_COPAY);
                DCopaymentModel model = null;
                while (reader.Read())
                {
                    model = new DCopaymentModel();
                    model.PatientId = Convert.ToString(reader["PatientId"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.FacilityName = Convert.ToString(reader["FacilityName"]);
                    model.Copay = Convert.ToString(reader["Copay"]);
                    model.AppointmentStatus = Convert.ToString(reader["AppointmentStatus"]);
                    model.CopayDiscount = Convert.ToString(reader["CopayDiscount"]);
                    model.CopayPaid = Convert.ToString(reader["CopayPaid"]);
                    model.VisitId = Convert.ToString(reader["VisitId"]);
                    model.AppointmentId = Convert.ToString(reader["AppointmentId"]);
                    model.FacilityId = Convert.ToString(reader["FacilityId"]);
                    model.ProviderId = Convert.ToString(reader["ProviderId"]);
                    model.ResourceId = Convert.ToString(reader["ResourceId"]);
                    model.VisitStatus = Convert.ToString(reader["VisitStatus"]);
                    model.PatientAccount = Convert.ToString(reader["PatientAccount"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadDashBoardCopay", PROC_DASHBOARD_COPAY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        #region Claim History

        public DSClaimSummary LoadClaimSummary(Int64 VisitID)
        {
            DSClaimSummary ds = new DSClaimSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            // SqlDataReader reader = null;
            try
            {
                //List<string> tableList = new List<string>();
                //tableList.Add(ds.PatientDetail.TableName);
                //tableList.Add(ds.ClaimDetail.TableName);
                //tableList.Add(ds.ICDDetail.TableName);
                //tableList.Add(ds.CPTDescription.TableName);
                //tableList.Add(ds.InsuranceDetail.TableName);
                //tableList.Add(ds.PaymentDetail.TableName);

                var tableNames = new List<string>
                {
                    ds.PatientDetail.TableName,
                    ds.ClaimDetail.TableName,
                    ds.ICDDetail.TableName,
                    ds.CPTDescription.TableName,
                    ds.InsuranceDetail.TableName,
                    ds.PaymentDetail.TableName
                };

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitID);

                ds = (DSClaimSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUMMARY, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadClaimSummary", PROC_DASHBOARD_COPAY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Unallocated Copay"
        private void createUnAllocatedCopayParameters(IDBManager dbManager, UnAllocatedCopayModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(22);
            int i = 0;
            if (IsInsert == true)
            {
                dbManager.AddParameters(i++, PARM_UNALLOCATED_COPAY_ID, model.UnAllocatedCopayId, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_UNALLOCATED_COPAY_ID, model.UnAllocatedCopayId);
            }

            if (string.IsNullOrEmpty(model.AdvPmtId))
            {
                dbManager.AddParameters(i++, PARM_ADVANCE_PAYMENT_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_ADVANCE_PAYMENT_ID, model.AdvPmtId);
            }

            if (string.IsNullOrEmpty(model.AppointmentId))
            {
                dbManager.AddParameters(i++, PARM_APPOINTMENT_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_APPOINTMENT_ID, model.AppointmentId);
            }

            if (string.IsNullOrEmpty(model.CardTypeId))
            {
                dbManager.AddParameters(i++, PARM_CARD_TYPE_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_CARD_TYPE_ID, model.CardTypeId);
            }

            if (string.IsNullOrEmpty(model.CheckDate))
            {
                dbManager.AddParameters(i++, PARM_CHECK_DATE, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_CHECK_DATE, model.CheckDate);
            }

            if (string.IsNullOrEmpty(model.CheckNo))
            {
                dbManager.AddParameters(i++, PARM_CHECK_NO, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_CHECK_NO, model.CheckNo);
            }

            if (string.IsNullOrEmpty(model.Comments))
            {
                dbManager.AddParameters(i++, PARM_COMMENTS, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_COMMENTS, model.Comments);
            }

            if (string.IsNullOrEmpty(model.CopayAmount))
            {
                dbManager.AddParameters(i++, PARM_COPAY_AMOUNT, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_COPAY_AMOUNT, model.CopayAmount);
            }


            dbManager.AddParameters(i++, PARM_CREATED_BY, model.CreatedBy);
            dbManager.AddParameters(i++, PARM_CREATED_ON, model.CreatedOn);

            if (string.IsNullOrEmpty(model.ExpiryDate))
            {
                dbManager.AddParameters(i++, PARM_EXPIRY_DATE, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_EXPIRY_DATE, model.ExpiryDate);
            }

            if (string.IsNullOrEmpty(model.FacilityId))
            {
                dbManager.AddParameters(i++, PARM_FACILITY_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_FACILITY_ID, model.FacilityId);
            }

            dbManager.AddParameters(i++, PARM_IS_DELETED, false);

            if (string.IsNullOrEmpty(model.LedgerAccId))
            {
                dbManager.AddParameters(i++, PARM_LEDGER_ACCOUNT_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_LEDGER_ACCOUNT_ID, model.LedgerAccId);
            }

            dbManager.AddParameters(i++, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, model.ModifiedOn);

            if (string.IsNullOrEmpty(model.PatientId))
            {
                dbManager.AddParameters(i++, PARM_PATIENT_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_PATIENT_ID, model.PatientId);
            }

            if (string.IsNullOrEmpty(model.PmtTypeId))
            {
                dbManager.AddParameters(i++, PARM_PAYMENT_TYPE_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_PAYMENT_TYPE_ID, model.PmtTypeId);
            }

            if (string.IsNullOrEmpty(model.ProviderId))
            {
                dbManager.AddParameters(i++, PARM_PROVIDER_ID, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_PROVIDER_ID, model.ProviderId);
            }

            if (string.IsNullOrEmpty(model.ReceiptDate))
            {
                dbManager.AddParameters(i++, PARM_RECEIPT_DATE, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_RECEIPT_DATE, model.ReceiptDate);
            }

            if (string.IsNullOrEmpty(model.ReceiptNumber))
            {
                dbManager.AddParameters(i++, PARM_RECEIPT_NUMBER, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_RECEIPT_NUMBER, model.ReceiptNumber);
            }

            if (string.IsNullOrEmpty(model.Status))
            {
                dbManager.AddParameters(i++, PARM_COPAY_STATUS, null);
            }
            else
            {
                dbManager.AddParameters(i++, PARM_COPAY_STATUS, model.Status);
            }

        }
        public UnAllocatedCopayModel SaveUnAllocatedCopay(UnAllocatedCopayModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                createUnAllocatedCopayParameters(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_UNALLOCATED_COPAY_INSERT);
                while (reader.Read())
                {
                    model.UnAllocatedCopayId = Convert.ToString(reader["UnAllocatedCopayId"]);
                    model.ReceiptNumber = Convert.ToString(reader["ReceiptNumber"]);
                }
                reader.Close();
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::SaveUnAllocatedCopay", PROC_UNALLOCATED_COPAY_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateUnAllocatedCopay(UnAllocatedCopayModel model, IDBManager dbManager)
        {
            try
            {
                SqlDataReader reader = null;
                createUnAllocatedCopayParameters(dbManager, model, false);
                var UnAllocatedCopayId = string.Empty;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_UNALLOCATED_COPAY_UPDATE);
                while (reader.Read())
                {
                    UnAllocatedCopayId = Convert.ToString(reader["UnAllocatedCopayId"]);
                }
                reader.Close();
                return UnAllocatedCopayId;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::UpdateUnAllocatedCopay", PROC_UNALLOCATED_COPAY_UPDATE, ex);
                throw ex;
            }
        }
        public string RefundUnAllocatedCopay(long UnAllocatedCopayId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string returnVal = "";
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_UNALLOCATED_COPAY_ID, UnAllocatedCopayId);
                returnVal = dbManager.ExecuteScalar(PROC_UNALLOCATED_COPAY_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::RefundUnAllocatedCopay", PROC_UNALLOCATED_COPAY_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<UnAllocatedCopayModel> LoadUnAllocatedCopay(long UnAllocatedCopayId, Int64 PatientId, Int64 ProviderId, Int64 FacilityId, Int64 AppointmentID, String ReceiptNumber, DateTime? ReceiptDateFrom, DateTime? ReceiptDateTo, Int32 PageNumber = 1, Int32 RowsPerPage = 15, bool IsDeleted = true)
        {
            List<UnAllocatedCopayModel> UnAllocatedCopayList = new List<UnAllocatedCopayModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (UnAllocatedCopayId < 1)
                    dbManager.AddParameters(PARM_UNALLOCATED_COPAY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_UNALLOCATED_COPAY_ID, UnAllocatedCopayId);

                if (PatientId < 1)
                    dbManager.AddParameters(PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);

                if (ProviderId < 1)
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);

                if (FacilityId < 1)
                    dbManager.AddParameters(PARM_FACILITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_FACILITY_ID, FacilityId);

                if (AppointmentID < 1)
                    dbManager.AddParameters(PARM_APPOINTMENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_APPOINTMENT_ID, AppointmentID);

                dbManager.AddParameters(PARM_RECEIPT_NUMBER, ReceiptNumber);

                dbManager.AddParameters(PARM_RECEIPT_DATE_FROM, ReceiptDateFrom);

                dbManager.AddParameters(PARM_RECEIPT_DATE_TO, ReceiptDateTo);

                dbManager.AddParameters(PARM_IS_DELETED, IsDeleted);

                if (PageNumber == 0)
                    dbManager.AddParameters(PARM_PAGE_N0, null);
                else
                    dbManager.AddParameters(PARM_PAGE_N0, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(PARM_ROWS_PE_PAGE, null);
                else
                    dbManager.AddParameters(PARM_ROWS_PE_PAGE, RowsPerPage);
                UnAllocatedCopayList = dbManager.ExecuteReaders<UnAllocatedCopayModel>(PROC_UNALLOCATED_COPAY_SELECT);
                return UnAllocatedCopayList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadUnAllocatedCopay", PROC_UNALLOCATED_COPAY_SELECT, ex);
                throw ex;
            }
        }

        public List<PaymentReceiptInfoModel> LoadPatientPaymentReceiptInfo(string PaymentId)
        {
            List<PaymentReceiptInfoModel> PaymentReceiptInfoList = new List<PaymentReceiptInfoModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PAYMENT_ID, PaymentId));
                using (var reader = dbManager.ExecuteReader(PROC_PATIENT_PAYMENT_RECEIPTINFO_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        PaymentReceiptInfoModel model = new PaymentReceiptInfoModel();
                        var properties = typeof(PaymentReceiptInfoModel).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        PaymentReceiptInfoList.Add(model);
                    }
                }
                return PaymentReceiptInfoList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadPatientPaymentReceiptInfo", PROC_PATIENT_PAYMENT_RECEIPTINFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Close();
            }
        }

        #endregion
    }
}
