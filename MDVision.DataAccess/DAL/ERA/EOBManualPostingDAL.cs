using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;
using MDVision.Model.Billing.ERA;
using System.Data.SqlClient;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.ERA
{
   public class EOBManualPostingDAL
    {


        #region "Stored Procedure Names"
        private const string PROC_EOB_MANUAL_POSTING_INSERT = "[Patient].[sp_InsertEobManualPosting]";
        private const string PROC_EOB_MANUAL_POSTING_UPDATE = "[Patient].[sp_UpdateEobManualPosting]";
        private const string PROC_EOB_MANUAL_POSTING_EXIST_OR_NOT = "[Patient].[sp_EobManualPostingExistORNot]";
        private const string PROC_EOB_MANUAL_CHARGES_SELECT= "[Patient].[sp_EOBManualPostingChargesSelect]";
        private const string PROC_EOB_MANUAL_POTING_PAYMENT_INSERT = "[Patient].[sp_EOBManualPostingDetailInsert]";
        private const string PROC_EOB_MANUAL_POTING_PAYMENT_UPDATE = "[Patient].[sp_EOBManualPostingDetailUpdate]";
        private const string PROC_EOB_MANUAL_POTING_PAYMENT_DETAIL = "[Patient].[sp_EOBManualPostingDetailSelect]";
        private const string PROC_EOB_MANUAL_POTING_SEARCH = "[Patient].[sp_EOBManualPostingSearch]";
        private const string PROC_POST_EOB_MANUAL_PAYMENT_INSERT = "[Patient].[sp_ManualEOBPaymentsInsert]";
        private const string PROC_EOB_MANUAL_DETAIL_DELETE = "[Patient].[sp_EOBManualPostingDetlDelete]";
        private const string PROC_EOB_MANUAL_DELETE = "[Patient].[sp_EOBManualPostingDelete]";
        private const string PROC_EOB_MANUAL_POSTING_DOCUMENT = "[Patient].[sp_EOBManualPostingDocument]";
        #endregion

        #region "Parameters"
        private const string PARM_INSURANCE_PAYMENT_DETAIL_ID = "@InsurancePaymentDetailId";
        private const string PARM_PAYER_NAME = "@PayerName";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_CHECK_NO = "@CheckNo";
        private const string PARM_CHECK_DATE = "@CheckDate";
        private const string PARM_CHECK_DEPOSIT_DATE = "@CheckDepositDate";
        private const string PARM_CHECK_AMOUNT = "@CheckAmount";
        private const string PARM_POSTED_STATUS_ID = "@PostedStatusId";
       
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_POSTED_AMOUNT = "@PostedAmount";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_Payment_DTL = "@EOBManualPostingdetail";
        private const string PARM_EOB_DETAIL_ID = "@EOBDtlId";
        private const string PARM_BILL_AMOUNT= "@BilledAmount";

        private const string PARM_EXPECTED_AMOUNT = "@ExpectedAmount";
        private const string PARM_INS_CHARGE = "@InsCharges";
        private const string PARM_ALLOWED_AMOUNT = "@AllowedAmount";
        private const string PARM_PAID_AMOUNT = "@PaidAmoount";
        private const string PARM_WRITE_OFF_AMOUNT = "@WriteOffAmount";
        private const string PARM_DEDUCABLE = "@Deducables";
        private const string PARM_CO_INS_AMOUNT = "@CoInsAmount";
        private const string PARM_EOB_COPAY = "@EOBCopay";
        private const string PARM_PATIENT_RESP = "@PatientResp";
        private const string PARM_RESPONSIBILITY = "@NextResponsibility";

        private const string PARM_RESPONSIBILITY_ID = "@NextResponsibilityId";
        private const string PARM_ADJUSTMENT_GROUP_CODE = "@AdjustmentGroupCodeId";
        private const string PARM_ADJUSTMENT_REASON_CODE = "@AdjustmentReasonCodeId";
        private const string PARM_REMARK_CODE = "@RemarkCode";
        private const string PARM_FROM_CHECK_DATE = "@FromCheckDate";
        private const string PARM_TO_CHECK_DATE = "@ToCheckDate";
        private const string PARM_FROM_ENTRY_DATE = "@FromEntryDate";
        private const string PARM_TO_ENTRY_DATE = "@ToEntryDate";
        private const string PARM_TO_MANUAL_POSTING_ID = "@EOBManualPostingId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region Constructors
        public EOBManualPostingDAL()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public EOBManualPostingDAL(SharedVariable SharedVariable)
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

        #region "Support Functions EOB Manual Posting"
        private void CreateEobDetailParameters(IDBManager dbManager, EOBManualPaymentPost modal)
        {
            
            dbManager.CreateParameters(18);
            dbManager.AddParameters(0, PARM_EOB_DETAIL_ID, modal.Id, DbType.Int64);
            dbManager.AddParameters(1, PARM_BILL_AMOUNT, modal.BilledAmount, DbType.Decimal);
            dbManager.AddParameters(2, PARM_EXPECTED_AMOUNT, modal.ExpectedAmount, DbType.Decimal);
            dbManager.AddParameters(3, PARM_INS_CHARGE, modal.InsCharges, DbType.Decimal);
            dbManager.AddParameters(4, PARM_ALLOWED_AMOUNT, modal.AllowedAmount, DbType.Decimal);
            dbManager.AddParameters(5, PARM_PAID_AMOUNT, modal.PaidAmount, DbType.Decimal);
            dbManager.AddParameters(6, PARM_WRITE_OFF_AMOUNT, modal.WriteOffAmount, DbType.Decimal);
            dbManager.AddParameters(7, PARM_DEDUCABLE, modal.Deducables, DbType.Decimal);
            dbManager.AddParameters(8, PARM_CO_INS_AMOUNT, modal.CoInsAmount, DbType.Decimal);
            dbManager.AddParameters(9, PARM_EOB_COPAY, modal.EOBCopay, DbType.Decimal);
            dbManager.AddParameters(10, PARM_PATIENT_RESP, modal.PatientResp, DbType.Decimal);
            dbManager.AddParameters(11, PARM_RESPONSIBILITY, modal.NextResponsibilityId_text, DbType.String);
            dbManager.AddParameters(12, PARM_RESPONSIBILITY_ID, modal.NextResponsibilityId, DbType.Int64);
            dbManager.AddParameters(13, PARM_ADJUSTMENT_GROUP_CODE, modal.AdjustmentGroupCodeId, DbType.Int64);
            dbManager.AddParameters(14, PARM_ADJUSTMENT_REASON_CODE, modal.AdjustmentReasonCodeId, DbType.Int64);
            dbManager.AddParameters(15, PARM_REMARK_CODE, modal.RemarkCode, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_BY, modal.ModifiedBy, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIED_ON, modal.ModifiedOn, DbType.DateTime);  
        }
        #endregion
        #region EOB Manual Posting
        public List<InsurancePaymentDetail> LoadInsurancePaymentSearch(InsurancePaymentDetail modal)
        {
            List<InsurancePaymentDetail> InsuranceDetail = new List<InsurancePaymentDetail>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
               
                if (string.IsNullOrEmpty(modal.PayerName))
                {
                    dbManager.AddParameters(0, PARM_PAYER_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PAYER_NAME, modal.PayerName);
                }
                if(string.IsNullOrEmpty(modal.CheckNo))
                {
                    dbManager.AddParameters(1, PARM_CHECK_NO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_CHECK_NO, modal.CheckNo);
                }
                if (modal.PostedStatusId == 0) { dbManager.AddParameters(2, PARM_POSTED_STATUS_ID, DBNull.Value); }
                else { 
                dbManager.AddParameters(2, PARM_POSTED_STATUS_ID, modal.PostedStatusId);
                }
                if (string.IsNullOrEmpty(modal.FromCheckDate))
                {
                    dbManager.AddParameters(3, PARM_FROM_CHECK_DATE,DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_FROM_CHECK_DATE, modal.FromCheckDate);
                }
                
                if (string.IsNullOrEmpty(modal.CheckDate))
                {       dbManager.AddParameters(4, PARM_TO_CHECK_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_TO_CHECK_DATE, modal.ToCheckDate);
                }

                if (string.IsNullOrEmpty(modal.FromEntryDate))
                { dbManager.AddParameters(5, PARM_FROM_ENTRY_DATE, DBNull.Value); }
                else
                { dbManager.AddParameters(5, PARM_FROM_ENTRY_DATE, modal.FromEntryDate); }

                if (string.IsNullOrEmpty(modal.ToEntryDate))
                {        dbManager.AddParameters(6, PARM_TO_ENTRY_DATE, DBNull.Value);
                }
                else { dbManager.AddParameters(6, PARM_TO_ENTRY_DATE, modal.ToEntryDate); }
                if (modal.CheckAmount == 0)
                { dbManager.AddParameters(7, PARM_CHECK_AMOUNT, DBNull.Value); }
                else
                {
                    dbManager.AddParameters(7, PARM_CHECK_AMOUNT, modal.CheckAmount);
                }
                if (modal.Id == 0)
                {
                    dbManager.AddParameters(8, PARM_TO_MANUAL_POSTING_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_TO_MANUAL_POSTING_ID, modal.Id);
                }
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EOB_MANUAL_POTING_SEARCH);
                InsurancePaymentDetail model = null;
                while (reader.Read())
                {
                    model = new InsurancePaymentDetail();
                    model.Id= MDVUtility.ToInt64(reader["Id"].ToString());
                    model.CheckAmount = MDVUtility.ToDecimal(reader["CheckAmount"].ToString());
                    model.StatusName = MDVUtility.ToStr(reader["StatusName"].ToString());
                    model.PayerName = MDVUtility.ToStr(reader["PayerName"].ToString());
                    model.CheckNo = MDVUtility.ToStr(reader["CheckNo"].ToString());
                    model.PostedAmount = MDVUtility.ToDecimal(reader["PostedAmount"].ToString());
                    model.CheckDate = MDVUtility.ToStr(reader["CheckDate"].ToString());
                    model.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"].ToString());
                    model.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"].ToString());
                    model.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"].ToString());
                    model.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"].ToString());
                    model.PostedStatusId = MDVUtility.ToInt(reader["PostedStatusId"].ToString());
                    model.CheckDepositDate = MDVUtility.ToStr(reader["CheckDepositDate"]);
                    InsuranceDetail.Add(model);
                }

                return InsuranceDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::LoadInsurancePaymentSearch", PROC_EOB_MANUAL_POTING_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public InsurancePaymentDetail InsertEOBManualPosting(InsurancePaymentDetail model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(15);
                dbManager.AddParameters(0, PARM_INSURANCE_PAYMENT_DETAIL_ID, model.Id.ToString(), DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_PAYER_NAME, model.PayerName);
                dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, model.InsurancePlanId);
                dbManager.AddParameters(3, PARM_FACILITY_ID, string.IsNullOrEmpty(model.FacilityId) ? (object)DBNull.Value : model.FacilityId);
                dbManager.AddParameters(4, PARM_CHECK_NO, string.IsNullOrEmpty(model.CheckNo) ? (object)DBNull.Value : model.CheckNo);
                dbManager.AddParameters(5, PARM_CHECK_DATE, string.IsNullOrEmpty(model.CheckDate) ? (object)DBNull.Value : model.CheckDate);
                dbManager.AddParameters(6, PARM_CHECK_DEPOSIT_DATE, string.IsNullOrEmpty(model.CheckDepositDate) ? (object)DBNull.Value : model.CheckDepositDate);
                dbManager.AddParameters(7, PARM_CHECK_AMOUNT, model.CheckAmount);
                dbManager.AddParameters(8, PARM_POSTED_STATUS_ID, model.PostedStatusId);
                dbManager.AddParameters(9, PARM_POSTED_AMOUNT, model.PostedAmount);
                dbManager.AddParameters(10, PARM_ISACTIVE, model.IsActive);
                dbManager.AddParameters(11, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(12, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(13, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(14, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.ExecuteNonQueryWithOutputParam(CommandType.StoredProcedure, PROC_EOB_MANUAL_POSTING_INSERT);
                if (((SqlParameter)(dbManager.Command.Parameters[PARM_INSURANCE_PAYMENT_DETAIL_ID])).Value != null)
                    model.Id = MDVUtility.ToInt32( ((SqlParameter)(dbManager.Command.Parameters[PARM_INSURANCE_PAYMENT_DETAIL_ID])).Value.ToString());
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::InsertEOBManualPosting", PROC_EOB_MANUAL_POSTING_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public InsurancePaymentDetail UpdateEOBManualPosting(InsurancePaymentDetail model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);
                dbManager.AddParameters(0, PARM_TO_MANUAL_POSTING_ID, model.Id);
                dbManager.AddParameters(1, PARM_PAYER_NAME, model.PayerName);
                dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, model.InsurancePlanId);
                dbManager.AddParameters(3, PARM_FACILITY_ID, string.IsNullOrEmpty(model.FacilityId) ? (object)DBNull.Value : model.FacilityId);
                dbManager.AddParameters(4, PARM_CHECK_NO, string.IsNullOrEmpty(model.CheckNo) ? (object)DBNull.Value : model.CheckNo);
                dbManager.AddParameters(5, PARM_CHECK_DATE, string.IsNullOrEmpty(model.CheckDate) ? (object)DBNull.Value : model.CheckDate);
                dbManager.AddParameters(6, PARM_CHECK_DEPOSIT_DATE, string.IsNullOrEmpty(model.CheckDepositDate) ? (object)DBNull.Value : model.CheckDepositDate);
                dbManager.AddParameters(7, PARM_CHECK_AMOUNT, model.CheckAmount);
                dbManager.AddParameters(8, PARM_POSTED_STATUS_ID, model.PostedStatusId);
                dbManager.AddParameters(9, PARM_POSTED_AMOUNT, model.PostedAmount);
                dbManager.AddParameters(10, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(11, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_EOB_MANUAL_POSTING_UPDATE);
                
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::UpdateEOBManualPosting", PROC_EOB_MANUAL_POSTING_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertEOBManualPostingDetail(DataTable dtpaymentlist)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_Payment_DTL, dtpaymentlist);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_EOB_MANUAL_POTING_PAYMENT_INSERT);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::InsertEOBManualPostingDetail", PROC_EOB_MANUAL_POTING_PAYMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdateEOBManualPostingDetail(EOBManualPaymentPost modal)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(18);
                dbManager.AddParameters(0, PARM_EOB_DETAIL_ID, modal.Id);
                dbManager.AddParameters(1, PARM_BILL_AMOUNT, modal.BilledAmount);
                dbManager.AddParameters(2, PARM_EXPECTED_AMOUNT, modal.ExpectedAmount);
                dbManager.AddParameters(3, PARM_INS_CHARGE, modal.InsCharges);
                dbManager.AddParameters(4, PARM_ALLOWED_AMOUNT, modal.AllowedAmount);
                dbManager.AddParameters(5, PARM_PAID_AMOUNT, modal.PaidAmount);
                dbManager.AddParameters(6, PARM_WRITE_OFF_AMOUNT, modal.WriteOffAmount);
                dbManager.AddParameters(7, PARM_DEDUCABLE, modal.Deducables);
                dbManager.AddParameters(8, PARM_CO_INS_AMOUNT, modal.CoInsAmount);
                dbManager.AddParameters(9, PARM_EOB_COPAY, modal.EOBCopay);
                dbManager.AddParameters(10, PARM_PATIENT_RESP, modal.PatientResp);
                dbManager.AddParameters(11, PARM_RESPONSIBILITY, modal.NextResponsibilityId_text);
                dbManager.AddParameters(12, PARM_RESPONSIBILITY_ID, modal.NextResponsibilityId);
                dbManager.AddParameters(13, PARM_ADJUSTMENT_GROUP_CODE, modal.AdjustmentGroupCodeId);
                dbManager.AddParameters(14, PARM_ADJUSTMENT_REASON_CODE, modal.AdjustmentReasonCodeId);
                dbManager.AddParameters(15, PARM_REMARK_CODE, modal.RemarkCode);
                dbManager.AddParameters(16, PARM_MODIFIED_BY, modal.ModifiedBy);
                dbManager.AddParameters(17, PARM_MODIFIED_ON, modal.ModifiedOn);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_EOB_MANUAL_POTING_PAYMENT_UPDATE);
                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::UpdateEOBManualPostingDetail", PROC_EOB_MANUAL_POTING_PAYMENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string EobManualPostingExistORNot(long VisitId,long InsurancePaymentDetailId)
        {
            string returnVal = "";

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0,PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_INSURANCE_PAYMENT_DETAIL_ID, InsurancePaymentDetailId);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EOB_MANUAL_POSTING_EXIST_OR_NOT));
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::EobManualPostingExistORNot", PROC_EOB_MANUAL_POSTING_EXIST_OR_NOT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
       
        public List<EOBManualChargeLoad> LoadPatientCharges(long VisitId)
        {
            List<EOBManualChargeLoad> ChargeList = new List<EOBManualChargeLoad>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EOB_MANUAL_CHARGES_SELECT);
                EOBManualChargeLoad model = null;
                while (reader.Read())
                {
                    model = new EOBManualChargeLoad();
                    model.VisitId = MDVUtility.ToInt32(reader["VisitId"].ToString());
                    model.ChargeCapId = MDVUtility.ToInt32(reader["ChargeCapId"].ToString());
                    model.DOSFrom = MDVUtility.ToStr(reader["DOSFrom"].ToString());
                    model.CPTCode = MDVUtility.ToStr(reader["CPTCode"].ToString());
                    model.CPTDescription = MDVUtility.ToStr(reader["CPTDescription"].ToString()); 
                    model.Fee = MDVUtility.ToDecimal(reader["Fee"].ToString());
                    model.Billed = MDVUtility.ToDecimal(reader["Billed"].ToString());
                    model.InsCharges = MDVUtility.ToDecimal(reader["InsCharges"].ToString()); 
                    model.PatCharges = MDVUtility.ToDecimal(reader["PatCharges"].ToString());
                    model.Copay = MDVUtility.ToDecimal(reader["Copay"].ToString());
                    model.AllowedAmt = MDVUtility.ToDecimal(reader["AllowedAmt"].ToString());
                    model.PatientInsuranceId = MDVUtility.ToStr(reader["PatientInsuranceId"].ToString());
                    model.PatientId = MDVUtility.ToInt32(reader["PatientId"].ToString());
                    model.VisitInsuranceId = MDVUtility.ToStr(reader["VisitInsuranceId"].ToString());
                    ChargeList.Add(model);
                }

                return ChargeList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::LoadPatientCharges", PROC_EOB_MANUAL_CHARGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public long LoadEOBManualPostingDocument(long EOBId)
        {
            long PatDocId = 0;
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_TO_MANUAL_POSTING_ID, EOBId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EOB_MANUAL_POSTING_DOCUMENT);
                while (reader.Read())
                {
                    PatDocId = MDVUtility.ToInt64(reader["PatDocId"]);
                }

                return PatDocId;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::LoadEOBManualPostingDocument", PROC_EOB_MANUAL_POSTING_DOCUMENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<EOBManualPaymentPost> LoadEOBPaymentDetail(long VisitId=0,long EOBPostingId=0, long EOBDetlId=0)
        {
            List<EOBManualPaymentPost> PaymentList = new List<EOBManualPaymentPost>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (VisitId == 0)
                { dbManager.AddParameters(0, PARM_VISIT_ID, DBNull.Value); }
                else
                {
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                }
                if (EOBPostingId == 0)
                { dbManager.AddParameters(1, PARM_TO_MANUAL_POSTING_ID, DBNull.Value); }
                else
                {
                    dbManager.AddParameters(1, PARM_TO_MANUAL_POSTING_ID, EOBPostingId);
                }
                if (EOBDetlId == 0)
                { dbManager.AddParameters(2, PARM_EOB_DETAIL_ID, DBNull.Value); }
                else
                {
                    dbManager.AddParameters(2, PARM_EOB_DETAIL_ID, EOBDetlId);
                }
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EOB_MANUAL_POTING_PAYMENT_DETAIL);
                EOBManualPaymentPost model = null;
                while (reader.Read())
                {
                    model = new EOBManualPaymentPost();
                    model.Id= MDVUtility.ToStr(reader["Id"].ToString());
                    model.VisitId = MDVUtility.ToStr(reader["VisitId"].ToString());
                    model.ChargeCapId = MDVUtility.ToStr(reader["ChargeId"].ToString());
                    model.DateOfService = MDVUtility.ToStr(reader["DateOfService"].ToString());
                    model.CPTCode = MDVUtility.ToStr(reader["CPTCode"].ToString());
                    model.InsurancePlanId_text = MDVUtility.ToStr(reader["PlanName"].ToString());
                    model.PatientId = MDVUtility.ToStr(reader["PatientId"].ToString());
                    model.PatientName = MDVUtility.ToStr(reader["PatName"].ToString());
                    model.AccountNumber = MDVUtility.ToStr(reader["AccountNumber"].ToString());
                    model.ClaimNumber = MDVUtility.ToStr(reader["ClaimNumber"].ToString());
                    model.BilledAmount = MDVUtility.ToStr(reader["BilledAmount"].ToString());
                    model.ExpectedAmount = MDVUtility.ToStr(reader["ExpectedAmount"].ToString());
                    model.InsCharges = MDVUtility.ToStr(reader["InsCharges"].ToString());
                    model.AllowedAmount = MDVUtility.ToStr(reader["AllowedAmount"].ToString());
                    model.PaidAmount = MDVUtility.ToStr(reader["PaidAmount"].ToString());
                    model.WriteOffAmount = MDVUtility.ToStr(reader["WriteOffAmount"].ToString());
                    model.Deducables = MDVUtility.ToStr(reader["Deducables"].ToString());
                    model.CoInsAmount = MDVUtility.ToStr(reader["CoInsAmount"].ToString());
                    model.EOBCopay = MDVUtility.ToStr(reader["EOBCopay"].ToString());
                    model.ChargeCopay = MDVUtility.ToStr(reader["CopayCharge"].ToString());
                    model.PatientResp = MDVUtility.ToStr(reader["PatientResp"].ToString());
                    model.NextResponsibilityId_text = MDVUtility.ToStr(reader["NextResponsibility"].ToString());
                    model.NextResponsibilityId = MDVUtility.ToStr(reader["NextResponsibilityId"].ToString());
                    model.AdjustmentGroupCodeId = MDVUtility.ToStr(reader["AdjustmentGroupCodeId"].ToString());
                    model.AdjustmentReasonCodeId = MDVUtility.ToStr(reader["AdjustmentReasonCodeId"].ToString());
                    model.RemarkCode = MDVUtility.ToStr(reader["RemarkCode"].ToString());
                    model.CrossOver = MDVUtility.ToStr(reader["CrossedOver"].ToString()).ToLower() == "false" ? "No" : "Yes";
                    model.Posted = MDVUtility.ToStr(reader["IsPosted"].ToString()).ToLower()=="false"?"No":"Yes";
                    model.Comments = MDVUtility.ToStr(reader["Comments"].ToString());
                    model.IsActive = MDVUtility.ToStr(reader["IsActive"].ToString());
                    model.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"].ToString());
                    model.CreatedOn = MDVUtility.ToStr(reader["CreatedOn"].ToString());
                    model.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"].ToString());
                    model.ModifiedOn = MDVUtility.ToStr(reader["ModifiedOn"].ToString());
                    model.EOBManualPostingId = MDVUtility.ToStr(reader["EOBManualPostingId"].ToString());
                    model.VisitInsuranceId= MDVUtility.ToStr(reader["VisitInsuranceId"].ToString());
                    model.AdjustmentGroupCodeId_text = MDVUtility.ToStr(reader["AdjustmentGroupCodeId_text"].ToString());
                    model.AdjustmentReasonCodeId_text = MDVUtility.ToStr(reader["AdjustmentReasonCodeId_text"].ToString());
                    model.RemarkCode_text = MDVUtility.ToStr(reader["RemarkCode_text"].ToString());
                    model.CheckNumber = MDVUtility.ToStr(reader["CheckNo"].ToString());
                    PaymentList.Add(model);
                }

                return PaymentList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::LoadEOBPaymentDetail", PROC_EOB_MANUAL_POTING_PAYMENT_DETAIL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteEOBManualPostingDetail(long EOBDetlId =0)
        {
            string errorMessge = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
               
                dbManager.AddParameters(0, PARM_EOB_DETAIL_ID, EOBDetlId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.Int64, ParamDirection.Output);
                dbManager.ExecuteNonQueryWithOutputParam(CommandType.StoredProcedure, PROC_EOB_MANUAL_DETAIL_DELETE);
                if (((SqlParameter)(dbManager.Command.Parameters[PARM_ERROR_MESSAGE])).Value != null)
                    errorMessge = MDVUtility.ToStr(((SqlParameter)(dbManager.Command.Parameters[PARM_ERROR_MESSAGE])).Value.ToString());
                return errorMessge;
              

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::DeleteEOBManualPostingDetail", PROC_EOB_MANUAL_DETAIL_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteEOBManualPosting(long EOBPostingId = 0)
        {
            string errorMessge = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_TO_MANUAL_POSTING_ID, EOBPostingId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.Int64, ParamDirection.Output);
               
                dbManager.ExecuteNonQueryWithOutputParam(CommandType.StoredProcedure, PROC_EOB_MANUAL_DELETE);
                if (((SqlParameter)(dbManager.Command.Parameters[PARM_ERROR_MESSAGE])).Value != null)
                    errorMessge = MDVUtility.ToStr(((SqlParameter)(dbManager.Command.Parameters[PARM_ERROR_MESSAGE])).Value.ToString());
                return errorMessge;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::DeleteEOBManualPosting", PROC_EOB_MANUAL_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string PostEOBManualPosting(long EOBPostingId= 0, long EOBDetlId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(7);
                if (EOBPostingId == 0)
                {
                    dbManager.AddParameters(0, PARM_TO_MANUAL_POSTING_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_TO_MANUAL_POSTING_ID, EOBPostingId);
                }
                if (EOBDetlId==0)
                {
                    dbManager.AddParameters(1, PARM_EOB_DETAIL_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_EOB_DETAIL_ID, EOBDetlId);
                }
                dbManager.AddParameters(2, PARM_MODIFIED_BY,MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_ISACTIVE,true);
                dbManager.AddParameters(5, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(6, PARM_CREATED_ON, DateTime.Now);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_POST_EOB_MANUAL_PAYMENT_INSERT);
                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("EOBManualPostingDAL::PostEOBManualPosting", PROC_POST_EOB_MANUAL_PAYMENT_INSERT, ex);
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
