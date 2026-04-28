using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;


namespace MDVision.DataAccess.DAL.Payment
{
    public class DALAdvancePayment
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_ADVANCE_PAYMENT_SELECT = "Patient.sp_AdvancePaymentSelect";
        private const string PROC_ADVANCE_PAYMENT_SEARCH = "Patient.sp_AdvancePaymentSearch_New";
        private const string PROC_ADVANCE_PAYMENT_INSERT = "Patient.sp_AdvancePaymentInsert";
        private const string PROC_ADVANCE_PAYMENT_UPDATE = "Patient.sp_AdvancePaymentUpdate";
        private const string PROC_ADVANCE_PAYMENT_DELETE = "Patient.sp_AdvancePaymentDelete";





        #endregion

        #region "Parameters"


        private const string PARM_ADVANCE_PAYMENT_ID = "@AdvPaymentId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PAID_FROM = "@PaidFrom";
        private const string PARM_PAID_TO = "@PaidTo";
        private const string PARM_AMOUNT_PAID_CR = "@AmtPaidCr";
        private const string PARM_AMOUNT_PAID_DR = "@AmtPaidDr";
        private const string PARM_LEDGER_ACCOUNT_ID = "@LedgerAccountId";
        private const string PARM_PAYMENT_DATE = "@PaymentDate";
        private const string PARM_PAYMENT_TYPE_ID = "@PaymentTypeId";
        private const string PARM_COMMENT = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        // private const string PARM_PAYMENT_BATCH_ID = "@PaymentBatchId";
        private const string PARM_CHECK_NUMBER = "@CheckNumber";
        private const string PARM_CHECK_DATE = "@CheckDate";
        private const string PARM_EXPIRY_DATE = "@ExpiryDate";
        private const string PARM_CARD_TYPE = "@CardTypeId";
        private const string PARM_MASTER_PAYMENT_ID = "@MasterPaymentId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";







        #endregion

        #region Constructors
        public DALAdvancePayment()
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
        /// <param name="dbManager">The database manager object</param>
        /// <param name="ds">dataset of payment type</param>
        /// <param name="IsInsert">whether the parameters are being created for insert operation or not </param>
        private void CreateParameters(IDBManager dbManager, DSPayment ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ADVANCE_PAYMENT_ID, ds.AdvancePayment.AdvPaymentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ADVANCE_PAYMENT_ID, ds.AdvancePayment.AdvPaymentIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.AdvancePayment.PatientIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.AdvancePayment.FacilityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_AMOUNT_PAID_CR, ds.AdvancePayment.AmtPaidCrColumn.ColumnName, DbType.Double);

            dbManager.AddParameters(4, PARM_AMOUNT_PAID_DR, ds.AdvancePayment.AmtPaidDrColumn.ColumnName, DbType.Double);

            dbManager.AddParameters(5, PARM_LEDGER_ACCOUNT_ID, ds.AdvancePayment.LedgerAccountIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(6, PARM_PAYMENT_DATE, ds.AdvancePayment.PaymentDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(7, PARM_PAYMENT_TYPE_ID, ds.AdvancePayment.PaymentTypeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(8, PARM_COMMENT, ds.AdvancePayment.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.AdvancePayment.IsActiveColumn.ColumnName, DbType.Byte);

            dbManager.AddParameters(10, PARM_CREATED_BY, ds.AdvancePayment.CreatedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(11, PARM_CREATED_ON, ds.AdvancePayment.CreatedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.AdvancePayment.ModifiedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.AdvancePayment.ModifiedOnColumn.ColumnName, DbType.DateTime);

            //   dbManager.AddParameters(14, PARM_PAYMENT_BATCH_ID, ds.AdvancePayment.PaymentBatchIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(14, PARM_CHECK_NUMBER, ds.AdvancePayment.CheckNumberColumn.ColumnName, DbType.String);

            dbManager.AddParameters(15, PARM_CHECK_DATE, ds.AdvancePayment.CheckDateColumn.ColumnName, DbType.String);

            dbManager.AddParameters(16, PARM_EXPIRY_DATE, ds.AdvancePayment.ExpiryDateColumn.ColumnName, DbType.String);

            dbManager.AddParameters(17, PARM_CARD_TYPE, ds.AdvancePayment.CardTypeIdColumn.ColumnName, DbType.String);

            dbManager.AddParameters(18, PARM_MASTER_PAYMENT_ID, ds.AdvancePayment.MasterPaymentIdColumn.ColumnName, DbType.Int64);


        }
        #endregion


        #region "Functions Patient Advance Payments"


        public DSPayment UpdateAdvancePayment(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPayment)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ADVANCE_PAYMENT_UPDATE, ds, ds.AdvancePayment.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvancePayment::UpdateAdvancePayment", PROC_ADVANCE_PAYMENT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AdvancePaymentId"></param>
        /// <returns></returns>
        public string DeleteAdvancePayment(long AdvancePaymentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ADVANCE_PAYMENT_ID, AdvancePaymentId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ADVANCE_PAYMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvancePayment::DeleteAdvancePayment", PROC_ADVANCE_PAYMENT_DELETE, ex);
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

        public DSPayment InsertAdvancePayment(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPayment)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ADVANCE_PAYMENT_INSERT, ds, ds.AdvancePayment.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::InsertAdvancePayment", PROC_ADVANCE_PAYMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                 dbManager.Dispose();
            }
        }


        public DSPayment LoadAdvancePayment(long PatientId, long PaymentId, long FacilityId, DateTime? PaidFrom, DateTime? PaidTo, long PaymentTypeId)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (PaymentId <= 0)
                    dbManager.AddParameters(0, PARM_ADVANCE_PAYMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ADVANCE_PAYMENT_ID, PaymentId);



                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);


                if (FacilityId <= 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);



                //if (PaymentId <= 0)
                //    dbManager.AddParameters(3, PARM_PAID_FROM, null);
                //else
                dbManager.AddParameters(3, PARM_PAID_FROM, PaidFrom);


                //if (PaymentId <= 0)
                //    dbManager.AddParameters(4, PARM_PAID_TO, null);
                //else
                dbManager.AddParameters(4, PARM_PAID_TO, PaidTo);



                if (PaymentTypeId <= 0)
                    dbManager.AddParameters(5, PARM_PAYMENT_TYPE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PAYMENT_TYPE_ID, PaymentTypeId);

                dbManager.AddParameters(6, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));



                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ADVANCE_PAYMENT_SELECT, ds, ds.AdvancePayment.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvancePayment::LoadAdvancePayments", PROC_ADVANCE_PAYMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPayment searchAdvancePayment(long PatientId, long FacilityId, DateTime? PaidFrom, DateTime? PaidTo, long PaymentTypeId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (FacilityId <= 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);

                dbManager.AddParameters(2, PARM_PAID_FROM, PaidFrom);
                dbManager.AddParameters(3, PARM_PAID_TO, PaidTo);
                if (PaymentTypeId <= 0)
                    dbManager.AddParameters(4, PARM_PAYMENT_TYPE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PAYMENT_TYPE_ID, PaymentTypeId);

                dbManager.AddParameters(5, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                if (pageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.AdvancePaymentSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);



                ds = (DSPayment)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ADVANCE_PAYMENT_SEARCH, ds, ds.AdvancePaymentSearch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvancePayment::searchAdvancePayment", PROC_ADVANCE_PAYMENT_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        #endregion

        #region "Lookups"
        ///// <summary>
        ///// Lookups the Payment Type.
        ///// </summary>
        ///// <returns>DSPaymentLookup.</returns>
        //public DSPaymentLookup LookupPaymentBatch()
        //{
        //    DSPaymentLookup ds = new DSPaymentLookup();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSPaymentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_BATCH_LOOKUP, ds, ds.PaymentBatch.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALPayment::LookupPaymentBatch", PROC_PAYMENT_BATCH_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        #endregion


    }
}
