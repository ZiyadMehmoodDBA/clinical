using EDIParser;
using EDIParser.Professional;
using MDVision.Business.BCommon;
using MDVision.Business.NavicureService;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Admin.FollowUp;
using MDVision.DataAccess.DAL.Admin.PatientStatement;
using MDVision.DataAccess.DAL.Charges;
using MDVision.DataAccess.DAL.Claim;
using MDVision.DataAccess.DAL.FollowUp;
using MDVision.DataAccess.DAL.Patient;
using MDVision.DataAccess.DAL.PatientStatement;
using MDVision.DataAccess.DAL.Payment;
using MDVision.DataAccess.DAL.Visits;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Billing.Payments;
using MDVision.Model.Common;
using MDVision.Model.Dashboard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ServiceModel;

namespace MDVision.Business.BLL
{

    public class BLLBilling
    {
        private SharedVariable sharedVariable;
        #region Constructors
        public BLLBilling()
        {
            InitializeComponent();
        }
        public BLLBilling(SharedVariable sharedVariable)
        {

            this.sharedVariable = sharedVariable;
            InitializeComponent();

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

        #region "LedgerAccount"

        public BLObject<List<LedgerAccountModel>> LoadAllLedgerAccounts()
        {
            try
            {
                var ledgerAccounts = new DALLedgerAccount(sharedVariable).LoadAllLedgerAccounts();

                return new BLObject<List<LedgerAccountModel>>(ledgerAccounts);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadAllLedgerAccounts", ex);
                return new BLObject<List<LedgerAccountModel>>(null, ex.Message);
            }

        }
        public BLObject<DSPaymentSetup> LoadLedgerAccount(long LedgerAccountId, string ShortName, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LoadLedgerAccount(LedgerAccountId, ShortName, Description, EntityId, IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadLedgerAccount", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the LedgerAccount.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPaymentSetup> InsertLedgerAccount(ref DSPaymentSetup ds)
        {
            try
            {

                ds = new DALLedgerAccount().InsertLedgerAccount(ref ds);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertLedgerAccount", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the LedgerAccount.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        ///
        public BLObject<DSPaymentSetup> UpdateLedgerAccount(ref DSPaymentSetup ds)
        {
            try
            {

                ds = new DALLedgerAccount().UpdateLedgerAccount(ref ds);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateLedgerAccount", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the LedgerAccount.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteLedgerAccount(string UserIds)
        {
            try
            {

                UserIds = new DALLedgerAccount().DeleteLedgerAccount(UserIds);

                return new BLObject<string>(UserIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteLedgerAccount", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        //<summary>
        //Lookups the Ledger Type.
        //</summary>
        //<returns></returns>
        public BLObject<DSPaymentSetup> LookupLedgerType(string Active)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LookupLedgerType(Active);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupLedgerType", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }

        //<summary>
        //Lookups the Ledger Apply To.
        //</summary>
        //<returns></returns>
        public BLObject<DSPaymentSetup> LookupLedgerApplyTo(string Active)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LookupLedgerApplyTo(Active);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupLedgerApplyTo", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }

        //<summary>
        //Lookups the Procedure Category.
        //</summary>
        //<returns></returns>
        public BLObject<DSPaymentSetup> LookupLedgerSystemAccount(string Active)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LookupLedgerSystemAccount(Active);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupLedgerSystemAccount", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }




        #endregion

        #region "Supper Bill"

        public BLObject<DSSupperBill> LoadSupperBill(long SupperBillId, long TitleId, long PracticeId, string ShortName = "", string Description = "", string isActive = null, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSSupperBill ds = new DSSupperBill();
                ds = new DALSupperBill().LoadSupperBill(SupperBillId, TitleId, PracticeId, ShortName, Description, isActive, PageNumber, RowsPerPage);

                return new BLObject<DSSupperBill>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadSupperBill", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }
        }

        public BLObject<DSSupperBill> InsertSupperBill(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().InsertSupperBill(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertSupperBill", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<DSSupperBill> UpdateSupperBill(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().UpdateSupperBill(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateSupperBill", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteSupperBill(long SupperBillIds)
        {
            try
            {
                string SupperBill = new DALSupperBill().DeleteSupperBill(SupperBillIds);
                return new BLObject<string>(SupperBill);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteSupperBill", ex);
                return new BLObject<string>("", ex.Message);
            }
        }


        #region Supper Bill Grid Load

        public BLObject<DSSupperBill> LoadSupperBillGrid(long SupperBillId)
        {
            try
            {
                DSSupperBill dsSupperBill = new DSSupperBill();

                dsSupperBill = new DALSupperBill().LoadSupperBill(SupperBillId, 0, 0, "", "", null);

                dsSupperBill.Merge(new DALSupperBill().LoadSupperBillTitle(0, SupperBillId));
                dsSupperBill.Merge(new DALSupperBill().LoadSupperBillICD(0, SupperBillId, 0));
                dsSupperBill.Merge(new DALSupperBill().LoadSupperBillCPT(0, SupperBillId, 0));
                dsSupperBill.Merge(new DALSupperBill().LoadSupperBillModifier(0, SupperBillId, 0));


                return new BLObject<DSSupperBill>(dsSupperBill);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadSupperBillGrid", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }


        #endregion


        #region "Supper Bill Title"


        public BLObject<DSSupperBill> LoadSupperBillTitle(long TitleId, long SupperBillId)
        {
            try
            {
                DSSupperBill ds = new DSSupperBill();
                ds = new DALSupperBill().LoadSupperBillTitle(TitleId, SupperBillId);

                return new BLObject<DSSupperBill>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadSupperBillTitle", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }
        }

        public BLObject<DSSupperBill> InsertSupperBillTitle(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().InsertSupperBillTitle(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertSupperBillTitle", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<DSSupperBill> UpdateSupperBillTitle(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().UpdateSupperBillTitle(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateSupperBillTitle", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteSupperBillTitle(long TitleId)
        {
            try
            {
                string Title = new DALSupperBill().DeleteSupperBillTitle(TitleId);
                return new BLObject<string>(Title);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteSupperBillTitle", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #endregion

        #region "Supper Bill ICD"


        public BLObject<DSSupperBill> LoadSupperBillICD(long ICDId, long SupperBillId, long TitleId = 0)
        {
            try
            {
                DSSupperBill ds = new DSSupperBill();
                ds = new DALSupperBill().LoadSupperBillICD(ICDId, SupperBillId, TitleId);

                return new BLObject<DSSupperBill>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadSupperBillICD", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }
        }

        public BLObject<DSSupperBill> InsertSupperBillICD(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().InsertSupperBillICD(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertSupperBillICD", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<DSSupperBill> UpdateSupperBillICD(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().UpdateSupperBillICD(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateSupperBillICD", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteSupperBillICD(long ICDId)
        {
            try
            {
                string ICD = new DALSupperBill().DeleteSupperBillICD(ICDId);
                return new BLObject<string>(ICD);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteSupperBillICD", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #endregion

        #region "Supper Bill CPT"


        public BLObject<DSSupperBill> LoadSupperBillCPT(long CPTId, long SupperBillId, long TitleId = 0)
        {
            try
            {
                DSSupperBill ds = new DSSupperBill();
                ds = new DALSupperBill().LoadSupperBillCPT(CPTId, SupperBillId, TitleId);

                return new BLObject<DSSupperBill>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadSupperBillCPT", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }
        }

        public BLObject<DSSupperBill> InsertSupperBillCPT(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().InsertSupperBillCPT(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertSupperBillCPT", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<DSSupperBill> UpdateSupperBillCPT(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().UpdateSupperBillCPT(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateSupperBillCPT", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteSupperBillCPT(long CPTId)
        {
            try
            {
                string CPT = new DALSupperBill().DeleteSupperBillCPT(CPTId);
                return new BLObject<string>(CPT);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteSupperBillCPT", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #endregion

        #region "Supper Bill Modifier"


        public BLObject<DSSupperBill> LoadSupperBillModifier(long ModifierId, long SupperBillId, long TitleId = 0)
        {
            try
            {
                DSSupperBill ds = new DSSupperBill();
                ds = new DALSupperBill().LoadSupperBillModifier(ModifierId, SupperBillId, TitleId);

                return new BLObject<DSSupperBill>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadSupperBillModifier", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }
        }

        public BLObject<DSSupperBill> InsertSupperBillModifier(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().InsertSupperBillModifier(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertSupperBillModifier", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<DSSupperBill> UpdateSupperBillModifier(ref DSSupperBill ds)
        {
            try
            {
                ds = new DALSupperBill().UpdateSupperBillModifier(ref ds);
                return new BLObject<DSSupperBill>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateSupperBillModifier", ex);
                return new BLObject<DSSupperBill>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteSupperBillModifier(long ModifierId)
        {
            try
            {
                string Modifier = new DALSupperBill().DeleteSupperBillModifier(ModifierId);
                return new BLObject<string>(Modifier);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteSupperBillModifier", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #endregion

        #region "Lookups"

        public BLObject<DSSupperBillLookup> LookupSupperBill(long PracticeId)
        {
            try
            {
                DSSupperBillLookup ds = new DSSupperBillLookup();
                ds = new DALSupperBill().LookupSupperBill(PracticeId);
                return new BLObject<DSSupperBillLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupSupperBill", ex);
                return new BLObject<DSSupperBillLookup>(null, ex.Message);
            }
        }

        #endregion

        #endregion

        #region "Lookups"

        public BLObject<DSPaymentSetup> LookupPaymentType(string Active)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LookupPaymentType(Active);

                return new BLObject<DSPaymentSetup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupPaymentType", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }

        }

        //public BLObject<DSPaymentSetup> LookupLedgerAccountType()
        //{
        //    try
        //    {
        //        DSPaymentSetup ds = new DSPaymentSetup();
        //        ds = new DALLedgerAccount().LookupLedgerAccountType();

        //        return new BLObject<DSPaymentSetup>(ds);

        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLBilling::LookupLedgerAccountType", ex);
        //        return new BLObject<DSPaymentSetup>(null, ex.Message);
        //    }

        //}

        public BLObject<DSPaymentSetup> LookupLedgerAccount(Int64 TypeId, Int64 ApplyToId, Int64 SystemCategory = 0, Int64 IsSystem = -1)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LookupLedgerAccount(TypeId, ApplyToId, SystemCategory, IsSystem);
                return new BLObject<DSPaymentSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupLedgerAccount", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }
        }

        public BLObject<DSPaymentSetup> LookupLedgerAccountForPatientAndCopay(Int64 TypeId, Int64 SystemCategory = 0, Int64 IsSystem = -1)
        {
            try
            {
                DSPaymentSetup ds = new DSPaymentSetup();
                ds = new DALLedgerAccount().LookupLedgerAccountForPatientAndCopay(TypeId, SystemCategory, IsSystem);
                return new BLObject<DSPaymentSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupLedgerAccountForPatientAndCopay", ex);
                return new BLObject<DSPaymentSetup>(null, ex.Message);
            }
        }


        #endregion

        #region PATIENT PAYMENTS SECTION

        #region "insert Patient Payments"
        public BLObject<DSPayment> InsertPatientPayments(DSPayment ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DataTable dtTemp = ds.PatientPayments.GetChanges();
            try
            {
                dbManager.BeginTransaction();
                ds = new DALPayment().InsertPatientPayments(ds, dbManager);
                dbManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPayment::InsertPatientPayments - All transation has been rollback", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

            try
            {
                //audit log entry after transaction commit
                new DALPayment().InsertPatientPaymentsAudit(dtTemp, ds);
                return new BLObject<DSPayment>(ds); 
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::InsertPatientPayments", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
              
        }

        public BLObject<DSPayment> LoadPatientPayments(long PaymentId, long AppointmentId, long VisitId = 0, long ChargeId = 0, string Module = "")
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPayment().LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId, Module);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::LoadPatientPayments", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }

        public BLObject<DSPayment> LoadPatientCopayByIds(long AppointmentId, long VisitId)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPayment().LoadPatientCopayByIds(AppointmentId, VisitId);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::LoadPatientCopayByIds", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }

        public BLObject<DSPayment> LoadDashBoardPayments(long Entity, long PageNumber = 1, long RowsPerPage = 15)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPayment().LoadDashBoardPayments(Entity, PageNumber, RowsPerPage);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::LoadDashBoardPayments", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }
        public BLObject<DSPayment> LoadDashBoardCopay(long Entity, DateTime? PaidForm, DateTime? PaidTo, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPayment().LoadDashBoardCopay(Entity, PaidForm, PaidTo, PageNumber, RowspPage);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::LoadDashBoardCopay", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }

        public BLObject<string> IsCheckAlreadyPosted(string checkNo, Int64 chargeId = 0)
        {
            try
            {
                return new BLObject<string>(new DALPayment().IsCheckAlreadyPosted(checkNo, chargeId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::IsCheckPosted", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the payment type.
        /// </summary>
        /// <returns>BLObject&lt;DSPaymentLookup&gt;.</returns>
        public BLObject<DSPaymentLookup> LookupPaymentBatch(string BatchNumber)
        {
            try
            {
                DSPaymentLookup ds = new DSPaymentLookup();
                ds = new DALPayment().LookupPaymentBatch(BatchNumber);

                return new BLObject<DSPaymentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupPaymentBatch", ex);
                return new BLObject<DSPaymentLookup>(null, ex.Message);
            }

        }

        public BLObject<DSPaymentLookup> LookupCreditCardType()
        {
            try
            {
                DSPaymentLookup ds = new DSPaymentLookup();
                ds = new DALPayment().LookupCreditCardType();

                return new BLObject<DSPaymentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupCreditCardType", ex);
                return new BLObject<DSPaymentLookup>(null, ex.Message);
            }

        }

        public BLObject<DSPaymentLookup> LookupRemittanceCode()
        {
            try
            {
                DSPaymentLookup ds = new DSPaymentLookup();
                ds = new DALFollowUpRemittanceCode().LookupRemittanceCode();

                return new BLObject<DSPaymentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupRemittanceCode", ex);
                return new BLObject<DSPaymentLookup>(null, ex.Message);
            }

        }

        public BLObject<List<GenericLookupModel>> LookupRemittanceCodes()
        {
            try
            {
                List<GenericLookupModel> remitCodes = new List<GenericLookupModel>();
                remitCodes = new DALFollowUpRemittanceCode(sharedVariable).LookupRemittanceCodes();

                return new BLObject<List<GenericLookupModel>>(remitCodes);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupRemittanceCodes", ex);
                return new BLObject<List<GenericLookupModel>>(null, ex.Message);
            }

        }
        #endregion

        #endregion

        #region "Batch Charge"

        public BLObject<DSBatchCharge> LoadBatchCharge(long BatchId, string BatchNumber, string Description, long FacilityId, long ProviderId, long BillerId, string BatchStatusId, long PracticeID, string EnteredBy, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, DateTime? DOSFrom = null, DateTime? DOSTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LoadBatchCharge(BatchId, BatchNumber, Description, FacilityId, ProviderId, BillerId, BatchStatusId, PracticeID, EnteredBy, EntryDateFrom, EntryDateTo, DOSFrom, DOSTo, PageNumber, RowsPerPage);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchCharge", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> BatchChargeSearch(long BatchId, string BatchNumber, string Description, long FacilityId, long ProviderId, long BillerId, string BatchStatusId, long PracticeID, string EnteredBy, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, DateTime? DOSFrom = null, DateTime? DOSTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().BatchChargeSearch(BatchId, BatchNumber, Description, FacilityId, ProviderId, BillerId, BatchStatusId, PracticeID, EnteredBy, EntryDateFrom, EntryDateTo, DOSFrom, DOSTo, PageNumber, RowsPerPage);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchCharge", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> InsertBatchCharge(DSBatchCharge ds)
        {
            try
            {
                ds = new DALBatchCharge().InsertBatchCharge(ds);
                return new BLObject<DSBatchCharge>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertBatchCharge", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }
        }
        public BLObject<DSBatchCharge> UpdateBatchCharge(DSBatchCharge ds)
        {
            try
            {
                ds = new DALBatchCharge().UpdateBatchCharge(ds);
                return new BLObject<DSBatchCharge>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateBatchCharge", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }
        }

        public BLObject<DSBatchCharge> LookupChargeBatchStatus()
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LookupChargeBatchStatus();

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupChargeBatchStatus", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }


        public BLObject<DSBatchCharge> LoadBatchClaim(long BatchNumber, long BatchId)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LoadBatchClaim(BatchNumber, BatchId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchCharge", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> LoadBatchClaimDetail(long BatchId)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LoadBatchClaimDetail(BatchId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchCharge", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> LoadBatchChargeDetail(long BatchNumber, long BatchId)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LoadBatchChargeDetail(BatchNumber, BatchId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchChargeDetail", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }


        public BLObject<DSBatchCharge> BatchChargeDetailSelect(long BatchId)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().BatchChargeDetailSelect(BatchId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::BatchChargeDetailSelect", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> LookupBatchChargeAction()
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LookupBatchChargeAction();

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupBatchChargeAction", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> LookupBatchChargeReason()
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LookupBatchChargeReason();

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupBatchChargeReason", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteBatchCharge(long BatchID)
        {
            try
            {
                return new BLObject<string>(new DALBatchCharge().DeleteBatchCharge(BatchID));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteBatchCharge", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region "Batch Charge Document"

        public BLObject<DSBatchCharge> InsertBatchChargeDocument(DSBatchCharge ds)
        {
            try
            {
                ds = new DALBatchCharge().InsertBatchChargeDocument(ds);
                return new BLObject<DSBatchCharge>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertBatchChargeDocument", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }
        }
        public BLObject<DSBatchCharge> LoadBatchChargeDocument(long BatchDocId, long BatchId, string isFileStream = "0", string VisitId = null)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LoadBatchChargeDocument(BatchDocId, BatchId, isFileStream, VisitId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchChargeDocument", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        public BLObject<DSBatchCharge> SearchClaimDocuments(string VisitId)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALBatchCharge().LoadBatchChargeDocument(VisitId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadBatchChargeDocument", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }


        public BLObject<string> DeleteBatchChargeDocument(long BatchDocId)
        {
            try
            {
                return new BLObject<string>(new DALBatchCharge().DeleteBatchChargeDocument(BatchDocId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteBatchDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSBatchCharge> UpdateBatchChargeDocument(DSBatchCharge ds)
        {
            try
            {
                ds = new DALBatchCharge().UpdateBatchChargeDocument(ds);
                return new BLObject<DSBatchCharge>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateBatchChargeDocument", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }
        }

        #endregion

        #region "Charges"


        public BLObject<DSCharge> InsertPatientCharges(DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCharge dsChargePointers = new DSCharge();
            DSVisits dsVisitICD = new DSVisits();
            try
            {
                dbManager.BeginTransaction();
                ds = new DALCharge().InsertPatientCharges(ds, dbManager);
                dsVisitICD = new DALVisits().LoadPatientVisitICDs(MDVUtility.ToLong(ds.Tables[ds.PatientCharges.TableName].Rows[0][ds.PatientCharges.VisitIdColumn.ColumnName]), 0);
                // Charge Pointers
                foreach (DataRow drCharge in ds.Tables[ds.PatientCharges.TableName].Rows)
                {
                    //1- Delete this charge Previous Pointers
                    //2- Add Its new Pointers
                    long chargeCapId = MDVUtility.ToLong(drCharge[ds.PatientCharges.ChargeCapIdColumn.ColumnName]);
                    int pointer1Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer1Column.ColumnName]);
                    int pointer2Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer2Column.ColumnName]);
                    int pointer3Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer3Column.ColumnName]);
                    int pointer4Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer4Column.ColumnName]);

                    string PatientChargesPointers = new DALCharge().DeleteChargesPointers(0, chargeCapId, dbManager);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer1Index - 1, 1);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer2Index - 1, 2);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer3Index - 1, 3);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer4Index - 1, 4);
                }

                if (dsChargePointers.ChargesICDPointers.Rows.Count > 0)
                {
                    ds = new DALCharge().InsertChargesPointers(dsChargePointers, dbManager);
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return new BLObject<DSCharge>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLBilling::InsertPatientCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }
        }

        public BLObject<DSCharge> UpdatePatientCharges(DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCharge dsChargePointers = new DSCharge();
            DSVisits dsVisitICD = new DSVisits();
            try
            {
                dbManager.Open();
                ds = new DALCharge().UpdatePatientCharges(ds, dbManager);
                dsVisitICD = new DALVisits().LoadPatientVisitICDs(MDVUtility.ToLong(ds.Tables[ds.PatientCharges.TableName].Rows[0][ds.PatientCharges.VisitIdColumn.ColumnName]), 0);
                // Charge Pointers
                foreach (DataRow drCharge in ds.Tables[ds.PatientCharges.TableName].Rows)
                {
                    //1- Delete this charge Previous Pointers
                    //2- Add Its new Pointers
                    long chargeCapId = MDVUtility.ToLong(drCharge[ds.PatientCharges.ChargeCapIdColumn.ColumnName]);
                    int pointer1Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer1Column.ColumnName]);
                    int pointer2Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer2Column.ColumnName]);
                    int pointer3Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer3Column.ColumnName]);
                    int pointer4Index = MDVUtility.ToInt(drCharge[ds.PatientCharges.ICDPointer4Column.ColumnName]);

                    string PatientChargesPointers = new DALCharge().DeleteChargesPointers(0, chargeCapId, dbManager);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer1Index - 1, 1);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer2Index - 1, 2);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer3Index - 1, 3);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer4Index - 1, 4);
                }

                if (dsChargePointers.ChargesICDPointers.Rows.Count > 0)
                {
                    ds = new DALCharge().InsertChargesPointers(dsChargePointers, dbManager);
                }
                ds.AcceptChanges();
                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                MDVLogger.BLLErrorLog("BLLBilling::UpdatePatientCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ChargeCapId"></param>
        /// <returns></returns>
        public BLObject<string> DeletePatientCharges(long ChargeCapId)
        {
            try
            {
                DSCharge dstemp = new DSCharge();
                dstemp = new DALCharge().LoadPatientCharges(ChargeCapId, null, null, 0, 0, null, 0, null, null, null, null, 0);
                string PatientCharges = new DALCharge().DeletePatientCharges(dstemp, ChargeCapId);
                return new BLObject<string>(PatientCharges);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeletePatientCharges", ex);
                return new BLObject<string>("", ex.Message);
            }
        }


        public BLObject<DSCharge> LoadPatientCharges(long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, string ChargeStatus, long InsurancePlanId, string ClaimNumber, string Paid, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int isPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string IsActive = null, bool IncludeVoidedClaims = true)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadPatientCharges(ChargeCapId, LastName, FirstName, FacilityId, ProviderId, ChargeStatus, InsurancePlanId, ClaimNumber, Paid, DOSFrom, DOSTo, visitId, isPayment, PatientAccount, _837Batchid, PageNumber, RowspPage, CPTCode, ClaimType, null, IncludeVoidedClaims);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }

        public BLObject<DSCharge> LoadPaymentCharges(string caseMgtId, long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, string ChargeStatus, long InsurancePlanId, string ClaimNumber, bool IncludeSecondaryClaim, string Paid, string ClaimErroredId, bool isVoidedClaim, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int isPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string PatientFullName = null)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadPaymentCharges(caseMgtId, ChargeCapId, LastName, FirstName, FacilityId, ProviderId, ChargeStatus, InsurancePlanId, ClaimNumber, IncludeSecondaryClaim, Paid, ClaimErroredId, isVoidedClaim, DOSFrom, DOSTo, visitId, isPayment, PatientAccount, _837Batchid, PageNumber, RowspPage, CPTCode, ClaimType, PatientFullName);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPaymentCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }

        public BLObject<DSCharge> LoadPatientVisitCharges(long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, long ResourcerProviderId, string ChargeStatus, long InsurancePlanId, string ClaimNumber, string Paid, string Hold, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int isPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string ClaimErroredId = null, int? SubmitStatusId = null, string ClaimCreatedBy = null, DateTime? ClaimCreatedFrom = null, DateTime? ClaimCreatedTo = null, DateTime? EncounterSignOffDate = null, DateTime? ImpoterDateFrom = null, DateTime? ImpoterDateTo = null, DateTime? SubmittedFrom = null, DateTime? SubmittedTo = null, bool IncludeSecondaryClaim = false, bool IncludeVoidedClaims = false)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadPatientVisitCharges(ChargeCapId, LastName, FirstName, FacilityId, ProviderId, ResourcerProviderId, ChargeStatus, InsurancePlanId, ClaimNumber, Paid, Hold, DOSFrom, DOSTo, visitId, isPayment, PatientAccount, _837Batchid, PageNumber, RowspPage, CPTCode, ClaimType, ClaimErroredId, SubmitStatusId, ClaimCreatedBy, ClaimCreatedFrom, ClaimCreatedTo, EncounterSignOffDate, ImpoterDateFrom, ImpoterDateTo, SubmittedFrom, SubmittedTo, IncludeSecondaryClaim, IncludeVoidedClaims);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientVisitCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }

        public BLObject<DSCharge> PatientVisitChargesSearch(long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, long ResourcerProviderId, string ChargeStatus, long InsurancePlanId, string ClaimNumber, string Paid, string Hold, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int isPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string ClaimErroredId = null, int? SubmitStatusId = null, string ClaimCreatedBy = null, DateTime? ClaimCreatedFrom = null, DateTime? ClaimCreatedTo = null, DateTime? EncounterSignOffDate = null, DateTime? ImpoterDateFrom = null, DateTime? ImpoterDateTo = null, DateTime? SubmittedFrom = null, DateTime? SubmittedTo = null, bool IncludeSecondaryClaim = false, bool IncludeVoidedClaims = false, long? EncounterTypeId = null)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().PatientVisitChargesSearch(ChargeCapId, LastName, FirstName, FacilityId, ProviderId, ResourcerProviderId, ChargeStatus, InsurancePlanId, ClaimNumber, Paid, Hold, DOSFrom, DOSTo, visitId, isPayment, PatientAccount, _837Batchid, PageNumber, RowspPage, CPTCode, ClaimType, ClaimErroredId, SubmitStatusId, ClaimCreatedBy, ClaimCreatedFrom, ClaimCreatedTo, EncounterSignOffDate, ImpoterDateFrom, ImpoterDateTo, SubmittedFrom, SubmittedTo, IncludeSecondaryClaim, IncludeVoidedClaims, EncounterTypeId);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientVisitCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }


        public BLObject<DSCharge> LoadUnClaimedAppointments(string LastName, string FirstName, long FacilityId, long ProviderId, string Claimed, long InsurancePlanID, string Claimno, DateTime? DOSFrom = null, DateTime? DOSTo = null, string PatientAccount = null, int? SubmitStatusId = null, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadUnClaimedAppointments(LastName, FirstName, FacilityId, ProviderId, Claimed, InsurancePlanID, Claimno, DOSFrom, DOSTo, PatientAccount, SubmitStatusId, PageNumber, RowspPage);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadUnClaimedAppointments", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }

        public BLObject<DSCharge> UnClaimedAppointmentsSelect(string LastName, string FirstName, long FacilityId, long ProviderId, string Claimed, long InsurancePlanID, string Claimno, DateTime? DOSFrom = null, DateTime? DOSTo = null, string PatientAccount = null, int? SubmitStatusId = null, int? AppointmentStatusId = null, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().UnClaimedAppointmentsSelect(LastName, FirstName, FacilityId, ProviderId, Claimed, InsurancePlanID, Claimno, DOSFrom, DOSTo, PatientAccount, SubmitStatusId, AppointmentStatusId, PageNumber, RowspPage);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UnClaimedAppointmentsSelect", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }


        public BLObject<DSCharge> LookupAppVisitStatus()
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LookupAppVisitStatus();

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupAppVisitStatus", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }

        public BLObject<DSCharge> LoadPatientTransferCharges(long VisitID)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadPatientTransferCharges(VisitID);

                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientTransferCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }
        public BLObject<DSCharge> BillToPatientCharges(string ChargesIds, bool BillToPatient)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().BillToPatientCharges(ChargesIds, BillToPatient);
                return new BLObject<DSCharge>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::BillToPatientCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }
        public BLObject<DSNotes> LookupEncounterType()
        {
            try
            {
                DSNotes ds = new DSNotes();
                ds = new DALCharge().LookupEncounterType();
                return new BLObject<DSNotes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::GetEncounterType", ex);
                return new BLObject<DSNotes>(null, ex.Message);
            }
        }

        public BLObject<DSDBAudit> LoadPatientVisitsPrintInfo(string PatientId, string VisitId)
        {
            try
            {
                DSDBAudit ds = new DSDBAudit();
                ds = new DBActivityAudit().LoadPatientVisitsPrintInfo(PatientId, VisitId);
                return new BLObject<DSDBAudit>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientVisitsPrintInfo", ex);
                return new BLObject<DSDBAudit>(null, ex.Message);
            }
        }
        #endregion

        #region " Charges ICD Pointers "

        public BLObject<DSCharge> InsertChargesPointers(DSCharge ds)
        {

            try
            {

                ds = new DALCharge().InsertChargesPointers(ds);
                return new BLObject<DSCharge>(ds);
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLBilling::InsertChargesPointers", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }
        }

        public BLObject<DSCharge> UpdateChargesPointers(DSCharge ds)
        {

            try
            {

                ds = new DALCharge().UpdateChargesPointers(ds);
                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLBilling::UpdateChargesPointers", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }
        public BLObject<string> DeleteChargesPointers(long ICDPointerId, long ChargeCapId = 0)
        {
            try
            {
                string PatientChargesPointers = new DALCharge().DeleteChargesPointers(ICDPointerId, ChargeCapId);
                return new BLObject<string>(PatientChargesPointers);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteChargesPointers", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        public BLObject<DSCharge> LoadChargesPointers(long ICDPointerId, long ChargeCapId = 0, long VisitId = 0)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadChargesPointers(ICDPointerId, ChargeCapId, VisitId);
                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientCharges", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }

        public void BuildICDPointers(ref DSCharge dsChargePointers, DSVisits dsVisitICD, long chargeCapId, int pointerIndex, int Order)
        {
            if (pointerIndex >= 0)
            {
                // ICD 9 row
                DSVisits.PatientVisitICDRow[] dICD9Rows = (DSVisits.PatientVisitICDRow[])dsVisitICD.PatientVisitICD.Select(dsVisitICD.PatientVisitICD.ICDTypeColumn.ColumnName + "=9", dsVisitICD.PatientVisitICD.PVICDIdColumn.ColumnName);
                if (pointerIndex >= dICD9Rows.Length)
                {
                    throw new Exception("Save visit first, to save charge");

                }
                else
                {
                    DSCharge.ChargesICDPointersRow drPointer9 = dsChargePointers.ChargesICDPointers.NewChargesICDPointersRow();
                    drPointer9.ChargeCapId = chargeCapId;
                    DSVisits.PatientVisitICDRow drr9 = dICD9Rows[pointerIndex];
                    drPointer9.Order = Order;
                    drPointer9.PVICDId = drr9.PVICDId;
                    dsChargePointers.ChargesICDPointers.AddChargesICDPointersRow(drPointer9);
                }




                // ICD 10 row
                DSVisits.PatientVisitICDRow[] dICD10Rows = (DSVisits.PatientVisitICDRow[])dsVisitICD.PatientVisitICD.Select(dsVisitICD.PatientVisitICD.ICDTypeColumn.ColumnName + "=10", dsVisitICD.PatientVisitICD.PVICDIdColumn.ColumnName);

                if (pointerIndex >= dICD10Rows.Length)
                {
                    throw new Exception("Save visit first, to save charge");
                }
                else
                {
                    DSCharge.ChargesICDPointersRow drPointer10 = dsChargePointers.ChargesICDPointers.NewChargesICDPointersRow();
                    drPointer10.ChargeCapId = chargeCapId;
                    DSVisits.PatientVisitICDRow drr10 = dICD10Rows[pointerIndex];
                    drPointer10.Order = Order;
                    drPointer10.PVICDId = drr10.PVICDId;
                    dsChargePointers.ChargesICDPointers.AddChargesICDPointersRow(drPointer10);
                }


            }
        }

        #endregion

        //#region "Claim"
        //public BLObject<DSVisits> LoadPatientClaim(string AccountNumber, string FirstName, string LastName, long ProviderId, long BatchNumber, long FacilityId, long PracticeId, long PatientInsuranceId, DateTime? DOSForm, DateTime? DOSTo, long ClearingHouseId, long BillerId, string SecondaryVisits, int ClaimStatusId, string SubmitionMod)
        //{
        //    try
        //    {
        //        DSVisits ds = new DSVisits();
        //        ds = new DALClaim().LoadPatientClaim(AccountNumber, FirstName, LastName, ProviderId, BatchNumber, FacilityId, PracticeId, PatientInsuranceId, DOSForm, DOSTo, ClearingHouseId, BillerId, SecondaryVisits, ClaimStatusId, SubmitionMod);

        //        return new BLObject<DSVisits>(ds);

        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLBilling::LoadPatientClaim", ex);
        //        return new BLObject<DSVisits>(null, ex.Message);
        //    }

        //}



        //#endregion

        //#region "837 Batch and Claim"

        //#region "837 Batch Claim"

        //public BLObject<DS837Batch> Create837BatchClaim(List<long> VisitIds, long ClearingHouseId, bool SubmitType)
        //{
        //    DS837Batch dsBatch = new DS837Batch();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();

        //    try
        //    {
        //        //1- Create 837Batch
        //        DS837Batch._837BatchRow drBatch = dsBatch._837Batch.New_837BatchRow();
        //        drBatch.ClearingHouseId = ClearingHouseId;
        //        drBatch.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
        //        drBatch.IsActive = true;
        //        drBatch.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        drBatch.modifiedOn = DateTime.Now;
        //        drBatch.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        drBatch.CreatedOn = DateTime.Now;
        //        drBatch.SubmitType = SubmitType;
        //        drBatch.SendStatus = "I";
        //        dsBatch._837Batch.Add_837BatchRow(drBatch);

        //        //2- Begin Transaction
        //        dbManager.BeginTransaction();

        //        //3- Insert 837Batch
        //        dsBatch = new DAL837().Insert837Batch(dsBatch, dbManager);

        //        //4- Add Claims into 837BatchClaim
        //        foreach (var item in VisitIds)
        //        {
        //            DS837Batch._837BatchClaimRow drclaim = dsBatch._837BatchClaim.New_837BatchClaimRow();
        //            drclaim._837BatchId = Convert.ToInt16(dsBatch._837Batch.Rows[0][dsBatch._837Batch._837BatchIdColumn]);
        //            drclaim.VisitId = Convert.ToInt64(item);
        //            dsBatch._837BatchClaim.Add_837BatchClaimRow(drclaim);
        //        }

        //        //5- Insert 837BatchClaim
        //        dsBatch = new DAL837().Insert837BatchClaim(dsBatch, dbManager);

        //        //6- Comment Transaction
        //        dbManager.CommitTransaction();
        //        dsBatch.AcceptChanges();

        //        return new BLObject<DS837Batch>(dsBatch);
        //    }
        //    catch (Exception ex)
        //    {
        //        dsBatch.RejectChanges();
        //        dbManager.RollBackTransaction();
        //        throw ex;
        //        //MDVLogger.LogErrorMessage("BLLBilling::Create837BatchClaim", ex);
        //        //return new BLObject<DS837Batch>(null, ex.Message);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }

        //}

        //public BLObject<DSHCFA> PrintClaim(List<long> VisitIds, long ClearingHouseId, bool IsSubmit)
        //{
        //    try
        //    {
        //        DSHCFA ds = new DSHCFA();
        //        BLObject<DS837Batch> ds837Batch = new BLObject<DS837Batch>();
        //        if (IsSubmit)
        //        {
        //            //Create Batch
        //            ds837Batch = Create837BatchClaim(VisitIds, ClearingHouseId, false);

        //            if (ds837Batch.Data == null)
        //            {
        //                throw new Exception(ds837Batch.Message);
        //            }
        //        }

        //        foreach (var visitId in VisitIds)
        //        {
        //            ds = new DAL837().Load837Claim(visitId, 0);
        //            ds.Merge(new DAL837().Load837NamesByVisitId(visitId));
        //            ds.Merge(new DAL837().Load837ServiceLine(visitId, 0));
        //        }

        //        return new BLObject<DSHCFA>(GetHCFA(ds));
        //        //return this.CreateClaimXML(GetHCFA(ds));
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLBilling::PrintClaim", ex);
        //        return new BLObject<DSHCFA>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Gets the hcfa.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        /////
        //public DSHCFA GetHCFA(DSHCFA ds)
        //{
        //    //Fill Claims
        //    DSHCFA.HCFAClaimsRow drclaim = ds.HCFAClaims.NewHCFAClaimsRow();
        //    EDI837Parser Parser = new EDI837Parser(ds);
        //    DSHCFA._837NameRow Subscriber = null;
        //    DSHCFA._837NameRow OtherSubscriber = null;
        //    DSHCFA._837NameRow Patient = null;
        //    DSHCFA._837NameRow ReferringProvider = null;
        //    DSHCFA._837NameRow RenderingProvider = null;
        //    DSHCFA._837NameRow ServiceLocation = null;
        //    DSHCFA._837NameRow Provider = null;
        //    DSHCFA._837NameRow SupervisingProvider = null;
        //    DSHCFA._837ClaimRow Claim = null;

        //    #region " Name "

        //    if (ds._837Claim.Rows.Count > 0)
        //    {
        //        Claim = (DSHCFA._837ClaimRow)ds._837Claim.Rows[0];
        //    }

        //    //FILL claim from Name
        //    if (ds._837Name.Rows.Count > 0)
        //    {
        //        long VisitId = MDVUtility.ToLong(ds._837Name.Rows[0][ds._837Name.VisitIdColumn.ColumnName]);

        //        #region " Subscriber "

        //        Subscriber = Parser.GetSubscriber(VisitId);

        //        if (Subscriber != null)
        //        {
        //            string Plan = MDVUtility.ToStr(Subscriber.SBR09);
        //            switch (Plan)
        //            {
        //                case "CH":
        //                    {
        //                        drclaim.HCFA_1_A = "false";
        //                        drclaim.HCFA_1_B = "false";
        //                        drclaim.HCFA_1_C = "true";
        //                        drclaim.HCFA_1_D = "false";
        //                        drclaim.HCFA_1_E = "false";
        //                        drclaim.HCFA_1_F = "false";
        //                        drclaim.HCFA_1_G = "false";
        //                    }
        //                    break;
        //                case "MB":
        //                    {
        //                        drclaim.HCFA_1_A = "true";
        //                        drclaim.HCFA_1_B = "false";
        //                        drclaim.HCFA_1_C = "false";
        //                        drclaim.HCFA_1_D = "false";
        //                        drclaim.HCFA_1_E = "false";
        //                        drclaim.HCFA_1_F = "false";
        //                        drclaim.HCFA_1_G = "false";
        //                    }
        //                    break;
        //                case "MC":
        //                    {
        //                        drclaim.HCFA_1_A = "false";
        //                        drclaim.HCFA_1_B = "true";
        //                        drclaim.HCFA_1_C = "false";
        //                        drclaim.HCFA_1_D = "false";
        //                        drclaim.HCFA_1_E = "false";
        //                        drclaim.HCFA_1_F = "false";
        //                        drclaim.HCFA_1_G = "false";
        //                    }
        //                    break;
        //                case "VA":
        //                    {
        //                        drclaim.HCFA_1_A = "false";
        //                        drclaim.HCFA_1_B = "false";
        //                        drclaim.HCFA_1_C = "false";
        //                        drclaim.HCFA_1_D = "true";
        //                        drclaim.HCFA_1_E = "false";
        //                        drclaim.HCFA_1_F = "false";
        //                        drclaim.HCFA_1_G = "false";
        //                    }
        //                    break;
        //                case "12":
        //                case "13":
        //                case "14":
        //                case "15":
        //                case "16":
        //                case "BL":
        //                case "CI":
        //                case "HM":
        //                    {
        //                        drclaim.HCFA_1_A = "false";
        //                        drclaim.HCFA_1_B = "false";
        //                        drclaim.HCFA_1_C = "false";
        //                        drclaim.HCFA_1_D = "false";
        //                        drclaim.HCFA_1_E = "false";
        //                        drclaim.HCFA_1_F = "true";
        //                        drclaim.HCFA_1_G = "false";
        //                    }
        //                    break;
        //                default:
        //                    {
        //                        drclaim.HCFA_1_A = "false";
        //                        drclaim.HCFA_1_B = "false";
        //                        drclaim.HCFA_1_C = "false";
        //                        drclaim.HCFA_1_D = "false";
        //                        drclaim.HCFA_1_E = "false";
        //                        drclaim.HCFA_1_F = "false";
        //                        drclaim.HCFA_1_G = "true";
        //                    }
        //                    break;
        //            }




        //            drclaim.HCFA_1A = MDVUtility.ToStr(Subscriber.NM109);

        //            drclaim.HCFA_4 = MDVUtility.ToStr(Subscriber.NM103)
        //                           + " " + MDVUtility.ToStr(Subscriber.NM104)
        //                           + " " + MDVUtility.ToStr(Subscriber.NM105);

        //            string Relation = MDVUtility.ToStr(Subscriber.SBR02);
        //            switch (Relation)
        //            {
        //                case "01":
        //                    {
        //                        drclaim.HCFA_6_A = "true";
        //                        drclaim.HCFA_6_B = "false";
        //                        drclaim.HCFA_6_C = "false";
        //                        drclaim.HCFA_6_D = "false";
        //                    }
        //                    break;
        //                case "18":
        //                    {
        //                        drclaim.HCFA_6_A = "false";
        //                        drclaim.HCFA_6_B = "true";
        //                        drclaim.HCFA_6_C = "false";
        //                        drclaim.HCFA_6_D = "false";
        //                    }
        //                    break;
        //                case "19":
        //                    {
        //                        drclaim.HCFA_6_A = "false";
        //                        drclaim.HCFA_6_B = "false";
        //                        drclaim.HCFA_6_C = "true";
        //                        drclaim.HCFA_6_D = "false";
        //                    }
        //                    break;
        //                default:
        //                    {
        //                        drclaim.HCFA_6_A = "false";
        //                        drclaim.HCFA_6_B = "false";
        //                        drclaim.HCFA_6_C = "false";
        //                        drclaim.HCFA_6_D = "true";
        //                    }
        //                    break;
        //            }

        //            drclaim.HCFA_7 = MDVUtility.ToStr(Subscriber.N301);
        //            drclaim.HCFA_7_A = MDVUtility.ToStr(Subscriber.N401);
        //            drclaim.HCFA_7_B = MDVUtility.ToStr(Subscriber.N402);
        //            drclaim.HCFA_7_C = MDVUtility.ToStr(Subscriber.N403);
        //            //drclaim.HCFA_7_D = ""; Not required.

        //            drclaim.HCFA_11 = MDVUtility.ToStr(Subscriber.SBR03);

        //            drclaim.HCFA_11_A_MM = GetHcfaMonth(Subscriber.DMG02);
        //            drclaim.HCFA_11_A_DD = GetHcfaDay(Subscriber.DMG02);
        //            drclaim.HCFA_11_A_YY = GetHcfaYear(Subscriber.DMG02);

        //            drclaim.HCFA_11_A_M = false;
        //            drclaim.HCFA_11_A_F = false;
        //            if (Subscriber.DMG03.ToUpper() == "M")
        //                drclaim.HCFA_11_A_M = true;
        //            else
        //                drclaim.HCFA_11_A_F = true;

        //            drclaim.HCFA_11_C = MDVUtility.ToStr(Subscriber.SBR04);
        //            drclaim.HCFA_11_D = "false";

        //        }


        //        #endregion

        //        #region " Other Subscriber "

        //        OtherSubscriber = Parser.GetOtherSubscriber(VisitId);

        //        if (OtherSubscriber != null)
        //        {

        //            drclaim.HCFA_11_D = "true";

        //            drclaim.HCFA_9 = MDVUtility.ToStr(OtherSubscriber.NM103)
        //                                           + " " + MDVUtility.ToStr(OtherSubscriber.NM104)
        //                                           + " " + MDVUtility.ToStr(OtherSubscriber.NM105);

        //            drclaim.HCFA_9_A = MDVUtility.ToStr(OtherSubscriber.SBR03);
        //            drclaim.HCFA_9_D = MDVUtility.ToStr(OtherSubscriber.SBR04);
        //        }


        //        #endregion

        //        #region " Patient "

        //        Patient = Parser.GetPatient(VisitId);

        //        if (Patient != null)
        //        {
        //            drclaim.HCFA_2 = MDVUtility.ToStr(Patient.NM103)
        //                                          + " " + MDVUtility.ToStr(Patient.NM104)
        //                                          + " " + MDVUtility.ToStr(Patient.NM105);

        //            drclaim.HCFA_3_MM = GetHcfaMonth(Patient.DMG02);
        //            drclaim.HCFA_3_DD = GetHcfaDay(Patient.DMG02);
        //            drclaim.HCFA_3_YY = GetHcfaYear(Patient.DMG02);

        //            drclaim.HCFA_3_F = false;
        //            drclaim.HCFA_3_M = false;
        //            if (MDVUtility.ToStr(Patient.DMG03).ToUpper().Trim() == "M")
        //                drclaim.HCFA_3_M = true;
        //            else
        //                drclaim.HCFA_3_F = true;

        //            drclaim.HCFA_5 = MDVUtility.ToStr(Patient.N301);
        //            drclaim.HCFA_5_A = MDVUtility.ToStr(Patient.N401);
        //            drclaim.HCFA_5_B = MDVUtility.ToStr(Patient.N402);
        //            drclaim.HCFA_5_C = MDVUtility.ToStr(Patient.N403);
        //            //drclaim.HCFA_5_D = ""; Not required.
        //        }


        //        #endregion

        //        #region " ReferringProvider "

        //        ReferringProvider = Parser.GetReferringProvider(VisitId);

        //        if (ReferringProvider != null)
        //        {
        //            drclaim.HCFA_17 = MDVUtility.ToStr(ReferringProvider.NM103)
        //                                           + " " + MDVUtility.ToStr(ReferringProvider.NM104)
        //                                           + " " + MDVUtility.ToStr(ReferringProvider.NM105);

        //            drclaim.HCFA_17B = MDVUtility.ToStr(ReferringProvider.NM109);

        //        }
        //        else
        //        {
        //            SupervisingProvider = Parser.GetSupervisingProvider(VisitId);
        //            if (SupervisingProvider != null)
        //            {
        //                drclaim.HCFA_17 = MDVUtility.ToStr(SupervisingProvider.NM103)
        //                                         + " " + MDVUtility.ToStr(SupervisingProvider.NM104)
        //                                         + " " + MDVUtility.ToStr(SupervisingProvider.NM105);

        //                drclaim.HCFA_17B = MDVUtility.ToStr(SupervisingProvider.NM109);

        //            }
        //        }


        //        #endregion

        //        #region " RenderingProvider "

        //        RenderingProvider = Parser.GetRenderingProvider(VisitId);

        //        if (Claim != null)
        //        {
        //            if (Claim.CLM06 == "Y")
        //            {
        //                drclaim.HCFA_31_A = MDVUtility.ToStr(RenderingProvider.NM103)
        //                                   + " " + MDVUtility.ToStr(RenderingProvider.NM104)
        //                                   + " " + MDVUtility.ToStr(RenderingProvider.NM107) + "\n"
        //                                   + "SIGNATURE ON FILE";

        //                drclaim.HCFA_31_B = DateTime.Now.ToString(.DateFormat);
        //            }
        //        }

        //        #endregion

        //        #region " ServiceLocation "

        //        ServiceLocation = Parser.GetServiceLocation(VisitId);

        //        if (ServiceLocation != null)
        //        {
        //            drclaim.HCFA_32 = MDVUtility.ToStr(ServiceLocation.NM103)
        //                                        + " " + MDVUtility.ToStr(ServiceLocation.N301)
        //                                        + " " + MDVUtility.ToStr(ServiceLocation.N401)
        //                                        + " " + MDVUtility.ToStr(ServiceLocation.N402)
        //                                        + " " + MDVUtility.ToStr(ServiceLocation.N403);

        //            drclaim.HCFA_32_A = MDVUtility.ToStr(ServiceLocation.NM109);
        //        }

        //        #endregion

        //        #region " Provider "

        //        Provider = Parser.GetProvider(VisitId);

        //        if (Provider != null)
        //        {

        //            drclaim.HCFA_25_EIN = false;
        //            drclaim.HCFA_25_SSN = false;

        //            if (MDVUtility.ToStr(Provider.REF01) != "")
        //            {
        //                if (MDVUtility.ToStr(Provider.REF01) == "EI")
        //                    drclaim.HCFA_25_EIN = true;
        //                else if (MDVUtility.ToStr(Provider.REF01) == "SY")
        //                    drclaim.HCFA_25_SSN = true;

        //                drclaim.HCFA_25 = MDVUtility.ToStr(Provider.REF02);

        //            }


        //            drclaim.HCFA_33 = MDVUtility.ToStr(Provider.NM103)
        //                              + " " + MDVUtility.ToStr(Provider.NM104)
        //                              + " " + MDVUtility.ToStr(Provider.NM105)
        //                              + " " + MDVUtility.ToStr(Provider.N301)
        //                              + " " + MDVUtility.ToStr(Provider.N401)
        //                              + " " + MDVUtility.ToStr(Provider.N402)
        //                              + " " + MDVUtility.ToStr(Provider.N403);

        //            drclaim.HCFA_33_A = MDVUtility.ToStr(Provider.NM109);
        //        }

        //        #endregion
        //    }

        //    #endregion

        //    #region " Claim "

        //    //Fill claim from Claim
        //    if (Claim != null)
        //    {
        //        if (Claim.CLM11_1A == "EM")
        //            drclaim.HCFA_10_A = true;
        //        else
        //            drclaim.HCFA_10_A = false;

        //        if (Claim.CLM11_1B == "AA")
        //            drclaim.HCFA_10_B = true;
        //        else
        //            drclaim.HCFA_10_B = false;

        //        if (Claim.CLM11_1C == "OA")
        //            drclaim.HCFA_10_C = true;
        //        else
        //            drclaim.HCFA_10_C = false;

        //        drclaim.HCFA_10_B_STATE = MDVUtility.ToStr(Claim.CLM11_4);

        //        if (Claim.CLM09 == "Y")
        //            drclaim.HCFA_12_A = "SIGNATURE ON FILE";

        //        drclaim.HCFA_12_B = MDVUtility.ToStr(DateTime.Now.ToString(.DateFormat));


        //        if (Claim.CLM08 == "Y")
        //            drclaim.HCFA_13 = "SIGNATURE ON FILE";

        //        if (!string.IsNullOrEmpty(Claim.DTP03_B))
        //        {

        //            drclaim.HCFA_14_MM = GetHcfaMonth(Claim.DTP03_B);
        //            drclaim.HCFA_14_DD = GetHcfaDay(Claim.DTP03_B);
        //            drclaim.HCFA_14_YY = GetHcfaYear(Claim.DTP03_B);
        //            drclaim.HCFA_14_QUAL = Claim.DTP01_B;

        //            drclaim.HCFA_15_MM = GetHcfaMonth(Claim.DTP03_A);
        //            drclaim.HCFA_15_DD = GetHcfaDay(Claim.DTP03_A);
        //            drclaim.HCFA_15_YY = GetHcfaYear(Claim.DTP03_A);
        //            drclaim.HCFA_15_QUAL = Claim.DTP01_A;
        //        }
        //        else
        //        {
        //            drclaim.HCFA_14_MM = GetHcfaMonth(Claim.DTP03_A);
        //            drclaim.HCFA_14_DD = GetHcfaDay(Claim.DTP03_A);
        //            drclaim.HCFA_14_YY = GetHcfaYear(Claim.DTP03_A);
        //            drclaim.HCFA_14_QUAL = Claim.DTP01_A;
        //        }


        //        string[] temp = Claim.DTP03_C.Split('-');
        //        if (temp.Length > 0)
        //        {
        //            drclaim.HCFA_16_A_DD = GetHcfaDay(temp[0]);
        //            drclaim.HCFA_16_A_MM = GetHcfaMonth(temp[0]);
        //            drclaim.HCFA_16_A_YY = GetHcfaYear(temp[0]);

        //            drclaim.HCFA_16_B_DD = drclaim.HCFA_16_A_DD;
        //            drclaim.HCFA_16_B_MM = drclaim.HCFA_16_A_MM;
        //            drclaim.HCFA_16_B_YY = drclaim.HCFA_16_A_YY;
        //        }
        //        if (temp.Length > 1)
        //        {
        //            drclaim.HCFA_16_B_DD = GetHcfaDay(temp[1]);
        //            drclaim.HCFA_16_B_MM = GetHcfaMonth(temp[1]);
        //            drclaim.HCFA_16_B_YY = GetHcfaYear(temp[1]);
        //        }

        //        drclaim.HCFA_18_A_MM = GetHcfaMonth(Claim.DTP03_D);
        //        drclaim.HCFA_18_A_DD = GetHcfaDay(Claim.DTP03_D);
        //        drclaim.HCFA_18_A_YY = GetHcfaYear(Claim.DTP03_D);

        //        drclaim.HCFA_18_B_MM = GetHcfaMonth(Claim.DTP03_E);
        //        drclaim.HCFA_18_B_DD = GetHcfaDay(Claim.DTP03_E);
        //        drclaim.HCFA_18_B_YY = GetHcfaYear(Claim.DTP03_E);

        //        drclaim.HCFA_19 = Claim.NTE02;

        //        drclaim.HCFA_22_A = MDVUtility.ToStr(Claim.CLM05_3);
        //        drclaim.HCFA_22_B = MDVUtility.ToStr(Claim.REF02_PayerClmCtrlNo);
        //        drclaim.HCFA_23 = MDVUtility.ToStr(Claim.REF02_PRIOR_AUTH);
        //        drclaim.HCFA_26 = MDVUtility.ToStr(Claim.CLM01);

        //        if (Claim.CLM07.ToUpper() == "Y")
        //            drclaim.HCFA_27 = true;
        //        else
        //            drclaim.HCFA_27 = false;

        //        if (!string.IsNullOrEmpty(Claim.CLM02))
        //        {
        //            string[] fee = Claim.CLM02.Split('.');
        //            drclaim.HCFA_28_A = fee[0];

        //            if (fee.Length > 1)
        //                drclaim.HCFA_28_B = fee[1];
        //            else
        //                drclaim.HCFA_28_B = "00";

        //        }

        //    }

        //    ds.HCFAClaims.AddHCFAClaimsRow(drclaim);

        //    #endregion

        //    #region " Service Line "

        //    char icd_number = 'A';
        //    Dictionary<string, string> ICD = new Dictionary<string, string>();

        //    //Fill claim from Service Line
        //    foreach (DSHCFA._837ServiceLineRow dr in ds._837ServiceLine.Rows)
        //    {
        //        DSHCFA.HCFAChargesRow drcharges = ds.HCFACharges.NewHCFAChargesRow();

        //        #region" ICD "

        //        if (!string.IsNullOrEmpty(dr.ICDCode1))
        //        {
        //            if (!ICD.ContainsValue(dr.ICDCode1))
        //            {
        //                ICD.Add(icd_number.ToString(), dr.ICDCode1);
        //                drcharges.HCFA_24_E += icd_number.ToString();
        //                icd_number++;
        //            }
        //            else
        //            {
        //                drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode1).Key;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(dr.ICDCode2))
        //        {
        //            if (!ICD.ContainsValue(dr.ICDCode2))
        //            {
        //                ICD.Add(icd_number.ToString(), dr.ICDCode2);
        //                drcharges.HCFA_24_E += icd_number.ToString();
        //                icd_number++;
        //            }
        //            else
        //            {
        //                drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode2).Key;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(dr.ICDCode3))
        //        {
        //            if (!ICD.ContainsValue(dr.ICDCode3))
        //            {
        //                ICD.Add(icd_number.ToString(), dr.ICDCode3);
        //                drcharges.HCFA_24_E += icd_number.ToString();
        //                icd_number++;
        //            }
        //            else
        //            {
        //                drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode3).Key;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(dr.ICDCode4))
        //        {
        //            if (!ICD.ContainsValue(dr.ICDCode4))
        //            {
        //                ICD.Add(icd_number.ToString(), dr.ICDCode4);
        //                drcharges.HCFA_24_E += icd_number.ToString();
        //                icd_number++;
        //            }
        //            else
        //            {
        //                drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode4).Key;
        //            }
        //        }

        //        #endregion

        //        string[] temp = dr.DTP03.Split('-');
        //        if (temp.Length > 0)
        //        {
        //            drcharges.HCFA_24_A_From_DD = GetHcfaDay(temp[0]);
        //            drcharges.HCFA_24_A_From_MM = GetHcfaMonth(temp[0]);
        //            drcharges.HCFA_24_A_From_YY = GetHcfaYear(temp[0]);

        //            drcharges.HCFA_24_A_To_DD = drcharges.HCFA_24_A_From_DD;
        //            drcharges.HCFA_24_A_To_MM = drcharges.HCFA_24_A_From_MM;
        //            drcharges.HCFA_24_A_To_YY = drcharges.HCFA_24_A_From_YY;
        //        }
        //        if (temp.Length > 1)
        //        {
        //            drcharges.HCFA_24_A_To_DD = GetHcfaDay(temp[1]);
        //            drcharges.HCFA_24_A_To_MM = GetHcfaMonth(temp[1]);
        //            drcharges.HCFA_24_A_To_YY = GetHcfaYear(temp[1]);
        //        }




        //        if (Claim.CLM05_1 != dr.SV105)
        //            drcharges.HCFA_24_B = MDVUtility.ToStr(dr.SV105);

        //        drcharges.HCFA_24_C = MDVUtility.ToStr(dr.SV109);


        //        drcharges.HCFA_24_D_CPT = MDVUtility.ToStr(dr.SV101_2);

        //        drcharges.HCFA_24_MOD_1 = MDVUtility.ToStr(dr.SV101_3);
        //        drcharges.HCFA_24_D_MOD_2 = MDVUtility.ToStr(dr.SV101_4);
        //        drcharges.HCFA_24_D_MOD_3 = MDVUtility.ToStr(dr.SV101_5);
        //        drcharges.HCFA_24_D_MOD_4 = MDVUtility.ToStr(dr.SV101_6);

        //        // drcharges.HCFA_24_E = MDVUtility.ToStr(dr.SV107);

        //        if (!string.IsNullOrEmpty(dr.SV102))
        //        {
        //            string[] fee = dr.SV102.Split('.');
        //            drcharges.HCFA_24_F_A = fee[0];

        //            if (fee.Length > 1)
        //                drcharges.HCFA_24_F_B = fee[1];
        //            else
        //                drcharges.HCFA_24_F_B = "00";
        //        }

        //        drcharges.HCFA_24_G = MDVUtility.ToStr(dr.SV104);


        //        if (RenderingProvider != null)
        //            drcharges.HCFA_24_J = MDVUtility.ToStr(RenderingProvider.NM109);

        //        ds.HCFACharges.AddHCFAChargesRow(drcharges);
        //    }


        //    if (ICD.Count > 0)
        //    {
        //        drclaim.HCFA_21_ICD = "9";

        //        if (ICD.Count <= 12)
        //        {
        //            foreach (var item in ICD)
        //            {
        //                if (item.Key == "A")
        //                    drclaim.HCFA_21_A = item.Value;
        //                else if (item.Key == "B")
        //                    drclaim.HCFA_21_B = item.Value;
        //                else if (item.Key == "C")
        //                    drclaim.HCFA_21_C = item.Value;
        //                else if (item.Key == "D")
        //                    drclaim.HCFA_21_D = item.Value;
        //                else if (item.Key == "E")
        //                    drclaim.HCFA_21_E = item.Value;
        //                else if (item.Key == "F")
        //                    drclaim.HCFA_21_F = item.Value;
        //                else if (item.Key == "G")
        //                    drclaim.HCFA_21_G = item.Value;
        //                else if (item.Key == "H")
        //                    drclaim.HCFA_21_H = item.Value;
        //                else if (item.Key == "I")
        //                    drclaim.HCFA_21_I = item.Value;
        //                else if (item.Key == "J")
        //                    drclaim.HCFA_21_J = item.Value;
        //                else if (item.Key == "K")
        //                    drclaim.HCFA_21_K = item.Value;
        //                else if (item.Key == "L")
        //                    drclaim.HCFA_21L = item.Value;
        //            }
        //        }
        //        else if (ICD.Count > 12)
        //        {
        //            // need some more logic :p
        //        }
        //    }

        //    #endregion

        //    return ds;
        //}

        //#region " Support Function "

        //private static string GetHcfaDay(string in_date)
        //{
        //    try
        //    {
        //        return in_date.Substring(6, 2);
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //}
        //private static string GetHcfaMonth(string in_date)
        //{
        //    try
        //    {
        //        return in_date.Substring(4, 2);
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //}
        //private static string GetHcfaYear(string in_date, bool isFull = false)
        //{
        //    try
        //    {
        //        if (!isFull)
        //            return in_date.Substring(0, 2);
        //        else
        //            return in_date.Substring(0, 4);
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //}

        //#endregion

        //#endregion



        //#endregion

        #region ADVANCE PAYMENT


        /// <summary>
        ///
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSPayment> InsertAdvancePayment(DSPayment ds)
        {

            //  IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //  dbManager.BeginTransaction();

                ds = new DALAdvancePayment().InsertAdvancePayment(ds);

                //  InsertPatientPayment(ref ds, dbManager);

                // ds.AcceptChanges();
                //  dbManager.CommitTransaction();
                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                //  ds.RejectChanges();
                //   dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLBilling::InsertAdvancePayment", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }

        public BLObject<DSPayment> UpdateAdvancePayment(DSPayment ds)
        {
            try
            {

                ds = new DALAdvancePayment().UpdateAdvancePayment(ds);

                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateAdvancePayment", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AdvancePaymentId"></param>
        /// <returns></returns>
        public BLObject<string> DeleteAdvancePayment(long AdvancePaymentId)
        {
            try
            {
                string AdvancePayment = new DALAdvancePayment().DeleteAdvancePayment(AdvancePaymentId);
                return new BLObject<string>(AdvancePayment);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteAdvancePayment", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="PaymentId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="PaidFrom"></param>
        /// <param name="PaidTo"></param>
        /// <param name="PaymentType"></param>
        /// <returns></returns>
        public BLObject<DSPayment> LoadAdvancePayment(long PatientId, long PaymentId, long FacilityId, DateTime? PaidFrom, DateTime? PaidTo, long PaymentType)
        {
            try
            {
                DSPayment ds = new DSPayment();

                ds = new DALAdvancePayment().LoadAdvancePayment(PatientId, PaymentId, FacilityId, PaidFrom, PaidTo, PaymentType);

                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::LoadAdvancePayments", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }

        public BLObject<DSPayment> searchAdvancePayment(long PatientId, long FacilityId, DateTime? PaidFrom, DateTime? PaidTo, long PaymentType, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSPayment ds = new DSPayment();

                ds = new DALAdvancePayment().searchAdvancePayment(PatientId, FacilityId, PaidFrom, PaidTo, PaymentType, pageNumber, rowsPerPage);

                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::searchAdvancePayment", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }


        private void InsertPatientPayment(ref DSPayment dsPayment, IDBManager dbManager)
        {

            // DSPayment dsPatientPayment = new DSPayment();

            //DSPayment.AdvancePaymentRow advancePaymentRow=dsAdvancePayment
            // ds.Tables[ds.LedgerAccount1.TableName].Rows[0][ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName]

            //  var aa= dsAdvancePayment.Tables[dsAdvancePayment.Tables.].Rows[0]

            //[dsAdvancePayment.AdvancePayment.AdvPaymentIdColumn.ColumnName];

            DSPayment.AdvancePaymentRow advancePaymentRow = (DSPayment.AdvancePaymentRow)dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows[0];
            DSPayment.PatientPaymentsRow patientPaymentRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            //dr.AdvPmtId = MDVUtility.ToLong(dsAdvancePayment.AdvancePayment.AdvPaymentIdColumn.ColumnName);

            patientPaymentRow.PatientId = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.PatientIdColumn.ColumnName]);
            patientPaymentRow.AdvPmtId = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.AdvPaymentIdColumn.ColumnName]);
            patientPaymentRow.FacilityId = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.FacilityIdColumn.ColumnName]);
            patientPaymentRow.PaidAmountCr = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.AmtPaidDrColumn.ColumnName]);
            patientPaymentRow.PaidAmountDr = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.AmtPaidCrColumn.ColumnName]);
            patientPaymentRow.LedgerAccId = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.LedgerAccountIdColumn.ColumnName]);
            patientPaymentRow.PaymentDate = MDVUtility.ToDateTime(advancePaymentRow[dsPayment.AdvancePayment.PaymentDateColumn.ColumnName]);
            //paymentTypeId 4 is advance payment
            patientPaymentRow.PmtTypeId = 4; // MDVUtility.ToInt32(advancePaymentRow[dsPayment.AdvancePayment.PaymentTypeIdColumn.ColumnName]);
            patientPaymentRow.Comments = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.CommentsColumn.ColumnName]);
            patientPaymentRow.IsActive = true;
            patientPaymentRow.CreatedBy = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.CreatedByColumn.ColumnName]);
            patientPaymentRow.CreatedOn = MDVUtility.ToDateTime(advancePaymentRow[dsPayment.AdvancePayment.CreatedOnColumn.ColumnName]);
            patientPaymentRow.ModifiedBy = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.ModifiedByColumn.ColumnName]);
            patientPaymentRow.ModifiedOn = MDVUtility.ToDateTime(advancePaymentRow[dsPayment.AdvancePayment.ModifiedOnColumn.ColumnName]);

            if (MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.CheckNumberColumn.ColumnName]) != "")
                patientPaymentRow.CheckNo = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.CheckNumberColumn.ColumnName]);

            if (MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.CheckDateColumn.ColumnName]) != "")
                patientPaymentRow.CheckDate = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.CheckDateColumn.ColumnName]);

            if (MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.ExpiryDateColumn.ColumnName]) != "")
                patientPaymentRow.ExpiryDate = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.ExpiryDateColumn.ColumnName]);

            if (MDVUtility.ToInt32(advancePaymentRow[dsPayment.AdvancePayment.CardTypeIdColumn.ColumnName]) != 0)
                patientPaymentRow.CardTypeId = MDVUtility.ToInt32(advancePaymentRow[dsPayment.AdvancePayment.CardTypeIdColumn.ColumnName]);

            if (MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.MasterPaymentIdColumn.ColumnName]) != 0)
                patientPaymentRow.MasterPaymentId = MDVUtility.ToInt64(advancePaymentRow[dsPayment.AdvancePayment.MasterPaymentIdColumn.ColumnName]);

            //patientPaymentRow.IsRefund = MDVUtility.ToStr(advancePaymentRow[dsPayment.AdvancePayment.is.ColumnName]);

            dsPayment.PatientPayments.AddPatientPaymentsRow(patientPaymentRow);



            dsPayment = new DALPayment().InsertPatientPayments(dsPayment, dbManager);

            //ds = new DALAdvancePayment().i.InsertAdvancePayment(ds);
            //dsPayment.AcceptChanges();
        }

        #endregion

        #region PAYMENY BATCH
        public BLObject<DSPayment> InsertPaymentBatch(DSPayment ds)
        {
            try
            {
                ds = new DALPaymentBatch().InsertPaymentBatch(ds);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertPaymentBatch", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }
        public BLObject<DSPayment> LoadPaymentBatch(Int64 BatchId, string BatchNumber, string description, Int64 practiceId, Int64 FacilityId, Int64 billerId, int statusId, string EnteredBy, DateTime? DepositDate = null, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPaymentBatch().LoadPaymentBatch(BatchId, BatchNumber, description, practiceId, FacilityId, billerId, statusId, EnteredBy, DepositDate, EntryDateFrom, EntryDateTo, PageNumber, RowsPerPage);
                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPaymentBatch", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }

        public BLObject<DSPayment> PaymentBatchSearch(Int64 BatchId, string BatchNumber, string description, Int64 practiceId, Int64 FacilityId, Int64 billerId, int statusId, string EnteredBy,string checkNo, DateTime? DepositDate = null, DateTime? EntryDateFrom = null, DateTime? EntryDateTo = null, Int32 PageNumber = 1, Int32 RowsPerPage = 1000)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPaymentBatch().PaymentBatchSearch(BatchId, BatchNumber, description, practiceId, FacilityId, billerId, statusId, EnteredBy, checkNo, DepositDate, EntryDateFrom, EntryDateTo, PageNumber, RowsPerPage);
                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::PaymentBatchSearch", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }


        public BLObject<DSPayment> LoadInsurancePaymentByBatch(Int64 BatchId)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPaymentBatch().LoadInsurancePaymentByBatch(BatchId);
                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadInsurancePaymentByBatch", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }

        public BLObject<DSPayment> LoadPaymentByBatch(Int64 BatchId)
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPaymentBatch().LoadPaymentByBatch(BatchId);
                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPaymentByBatch", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }
        public BLObject<DSPayment> UpdatePaymentBatch(DSPayment ds)
        {
            try
            {
                ds = new DALPaymentBatch().UpdatePaymentBatch(ds);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdatePaymentBatch", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }
        public BLObject<string> DeletePaymentBatch(Int64 BatchID)
        {
            try
            {
                return new BLObject<string>(new DALPaymentBatch().DeletePaymentBatch(BatchID));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeletePaymentBatch", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion

        #region PAYMENY BATCH DOCUMENT

        public BLObject<DSPayment> InsertPaymentBatchDocument(DSPayment ds)
        {
            try
            {
                ds = new DALPaymentBatch().InsertPaymentBatchDocument(ds);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertPaymentBatchDocument", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }
        public BLObject<DSPayment> LoadPaymentBatchDocument(Int64 BatchDocId, Int64 BatchId, string isFileStream = "0")
        {
            try
            {
                DSPayment ds = new DSPayment();
                ds = new DALPaymentBatch().LoadPaymentBatchDocument(BatchDocId, BatchId, isFileStream);

                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPaymentBatchDocument", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }

        public BLObject<string> DeletePaymentBatchDocument(Int64 BatchDocId)
        {
            try
            {
                return new BLObject<string>(new DALPaymentBatch().DeletePaymentBatchDocument(BatchDocId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeletePaymentBatchDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSPayment> UpdatePaymentBatchDocument(DSPayment ds)
        {
            try
            {
                ds = new DALPaymentBatch().UpdatePaymentBatchDocument(ds);
                return new BLObject<DSPayment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdatePaymentBatchDocument", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }
        }

        #endregion

        #region "Get CPT FEE"
        public BLObject<DSCodes> LoadCPTFee(Int64 VisitId, string CPTCode, Int64 ProviderId, Int64 FacilityId, Int64 PracticeId, DateTime ChargeDOS, Int64 PatientInsuranceId = 0
                                , string POSCode = "", string Modifier1 = "", string Modifier2 = "", string Modifier3 = "", string Modifier4 = "")
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCPTCode().LoadCPTFee(VisitId, CPTCode, ProviderId, FacilityId, PracticeId, ChargeDOS, PatientInsuranceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadCPTFee", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion

        #region " PatientEligibility "

        public BLObject<DSPatientEligibility> LoadEDIEligibility(long EDIEligibilityId, long PatientId, long InsurancePlanId, long ProviderId, DateTime? DOSFrom, DateTime? DOSTo, string EQSevice, string LastName, string FirstName, string Status, int PageNumber = 1, int RowspPage = 1000, DateTime? EligibiltyFrom=null, DateTime? EligibiltyTo=null)
        {
            try
            {
                DSPatientEligibility ds = new DSPatientEligibility();
                ds = new DALEDIEligibility().LoadEDIEligibility(EDIEligibilityId, PatientId, InsurancePlanId, ProviderId, DOSFrom, DOSTo, EQSevice, LastName, FirstName, Status, PageNumber, RowspPage,EligibiltyFrom,EligibiltyTo);
                return new BLObject<DSPatientEligibility>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadEDIEligibility", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }
        public BLObject<DSPatientEligibility> LoadPatientEligibilityExport(long EDIEligibilityId, long PatientId, long InsurancePlanId, long ProviderId, DateTime? DOSFrom, DateTime? DOSTo, string EQSevice, string LastName, string FirstName, string Status)
        {
            try
            {
                DSPatientEligibility ds = new DSPatientEligibility();
                ds = new DALEDIEligibility().LoadPatientEligibilityExport(EDIEligibilityId, PatientId, InsurancePlanId, ProviderId, DOSFrom, DOSTo, EQSevice, LastName, FirstName, Status);
                return new BLObject<DSPatientEligibility>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadEDIEligibility", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }
        public BLObject<DSPatientEligibility> CreatePatientEligibilityBatch(long PatientId, long PatientInsuranceId, long ProviderId, DateTime DOS, string EQService, string Status)
        {
            try
            {
                DSPatientEligibility dsBatch = new DSPatientEligibility();
                DSPatientEligibility.EDIEligibilityRow drBatch = dsBatch.EDIEligibility.NewEDIEligibilityRow();

                if (PatientInsuranceId != 0)
                    drBatch.PatientInsuranceId = PatientInsuranceId;

                drBatch.PatientId = PatientId;
                drBatch.ProviderId = ProviderId;

                if (!string.IsNullOrEmpty(EQService))
                    drBatch.EQSevice = EQService;

                drBatch.Status = Status;
                drBatch.DOS = DOS;
                drBatch.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drBatch.ModifiedOn = DateTime.Now;
                drBatch.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drBatch.CreatedOn = DateTime.Now;
                drBatch.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                dsBatch.EDIEligibility.AddEDIEligibilityRow(drBatch);
                dsBatch = new DALEDIEligibility().InsertEDIEligibility(dsBatch);
                long batchId = Convert.ToInt64(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EDIEligibilityIdColumn]);
                dsBatch = new DALEDIEligibility().LoadEDIEligibility(batchId, 0, 0, 0, null, null, null, null, null, null);

                return new BLObject<DSPatientEligibility>(dsBatch);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::CreatePatientEligibilityBatch", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="PatientInsuranceId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="DOS"></param>
        /// <param name="EQService"></param>
        /// <param name="Status"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <returns></returns>
        public BLObject<DSPatientEligibility> CreatePatientEligibilityBatch(long PatientId, long PatientInsuranceId, long ProviderId, DateTime DOS, string EQService, string Status, SharedVariable SharedVariable)
        {
            try
            {
                DSPatientEligibility dsBatch = new DSPatientEligibility();
                DSPatientEligibility.EDIEligibilityRow drBatch = dsBatch.EDIEligibility.NewEDIEligibilityRow();

                if (PatientInsuranceId != 0)
                    drBatch.PatientInsuranceId = PatientInsuranceId;

                drBatch.PatientId = PatientId;
                drBatch.ProviderId = ProviderId;

                if (!string.IsNullOrEmpty(EQService))
                    drBatch.EQSevice = EQService;

                drBatch.Status = Status;
                drBatch.DOS = DOS;
                drBatch.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                drBatch.ModifiedOn = DateTime.Now;
                drBatch.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                drBatch.CreatedOn = DateTime.Now;
                drBatch.EntityId = MDVUtility.ToLong(SharedVariable.EntityId);
                dsBatch.EDIEligibility.AddEDIEligibilityRow(drBatch);
                dsBatch = new DALEDIEligibility(SharedVariable).InsertEDIEligibility(dsBatch, SharedVariable);
                long batchId = Convert.ToInt64(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EDIEligibilityIdColumn]);
                dsBatch = new DALEDIEligibility(SharedVariable).LoadEDIEligibility(batchId, 0, 0, 0, null, null, null, null, null, null, SharedVariable);

                return new BLObject<DSPatientEligibility>(dsBatch);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::CreatePatientEligibilityBatchService", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }

        public BLObject<DSPatientEligibility> PatientEligibility(long PatientId, long PatientInsuranceId, long ProviderId, DateTime DOS, string EQService = "")
        {
            DSPatientEligibility dsBatch = null;
            DS270 ds270 = new DS270();
            DS271 ds271 = new DS271();
            Int64 batchId = 0;
            string IsEligible = "false";
            BLObject<DSPatientEligibility> objBatch;

            //to only recode Parsing Exceptions.
            bool IsUpdateException = false;

            try
            {

                //1- Create PatientEligibilityBatch
                objBatch = CreatePatientEligibilityBatch(PatientId, PatientInsuranceId, ProviderId, DOS, EQService, "Initial");
                if (objBatch.Data != null)
                {
                    dsBatch = objBatch.Data;
                    batchId = Convert.ToInt64(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EDIEligibilityIdColumn]);

                    //2- Get270Header and Get270ServicesInfo
                    ds270.Merge(new DALEDIEligibility().Load270Header(batchId));
                    ds270.Merge(new DALEDIEligibility().Load270Names(batchId));

                    if (ds270.EDI270Header.Rows.Count > 0)
                    {
                        //3- Create 270String
                        IsUpdateException = true;
                        EDI270Parser parser = new EDI270Parser(ds270);
                        string Str270 = parser.Get270();
                        IsUpdateException = false;

                        //4- Update PatientEligibilityBatch with 270String
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str270Column] = Str270;
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Waiting";

                        //5- Request for Patient Eligibility
                        string Str271 = PatientEligibilitySubmit270(parser.SubmitterId, parser.SubmitterPassword, parser.URL, Str270);

                        if (!string.IsNullOrEmpty(Str271))
                        {
                            //6-a Update PatientEligibilityBatch with 271String
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str271Column] = Str271;

                            string ServiceTypeCode = MDVUtility.ToStr(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EQSeviceColumn]);

                            //7- Get Patient Eligibility Report
                            BLObject<DS271> obj = PatientEligibilityReport(0, Str271, ServiceTypeCode);
                            if (obj.Data != null)
                            {
                                ds271 = obj.Data;
                                dsBatch.Merge(ds271);

                                //Update Copay Deductible and Eligibility Status
                                if (ds271.EDI271Header.Rows.Count > 0)
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CopayColumn] = MDVUtility.ToDouble(ds271.EDI271Header.Rows[0][ds271.EDI271Header.CopayColumn]);
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.DeductibleColumn] = MDVUtility.ToDouble(ds271.EDI271Header.Rows[0][ds271.EDI271Header.DeductibleColumn]);

                                    IsEligible = (string)ds271.EDI271Header.Rows[0][ds271.EDI271Header.IsEligibleColumn];

                                }

                                //Update Eligibility for Patient
                                DALPatientInsurance ObjPatientInsurance = new DALPatientInsurance();
                                DSPatient dsPatient = ObjPatientInsurance.LoadPatientInsurance(PatientInsuranceId, 0, 0);

                                if (dsPatient != null)
                                {

                                    if (dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count > 0)
                                    {
                                        DSPatient.PatientInsuranceRow dr = (DSPatient.PatientInsuranceRow)dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[0];
                                        if (IsEligible == "Active")
                                            dr.EligibilityStatus = "Active";
                                        else if (IsEligible == "InActive")
                                            dr.EligibilityStatus = "Inactive";
                                        else
                                            dr.EligibilityStatus = "Waiting";

                                        dr.EligibilityDate = DOS;

                                        ObjPatientInsurance.UpdatePatientInsurance(dsPatient);
                                    }
                                }

                                if (IsEligible == "Active")
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Active";
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Patient eligibility checked successfully.";

                                }
                                else if (IsEligible == "InActive")
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Inactive";
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Patient eligibility checked successfully.";

                                }
                                else
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Waiting";
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Patient eligibility response get with error.";

                                }

                            }
                            else
                            {
                                IsUpdateException = true;
                                throw new Exception(obj.Message);
                            }
                        }
                        else
                        {
                            //6-b Update PatientEligibilityBatch with 271String
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str271Column] = Str271;
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Waiting";
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Can not get response from patient eligibility service.";
                        }

                        dsBatch = new DALEDIEligibility().UpdateEDIEligibility(dsBatch);
                    }
                    else
                        throw new Exception("Please setup the EDI receiver ID and submitter ID for selected clearing house.");

                }
                else
                {
                    throw new Exception(objBatch.Message);
                }

                return new BLObject<DSPatientEligibility>(dsBatch);
            }
            catch (Exception ex)
            {


                if (dsBatch != null)
                {
                    if (IsUpdateException)
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = ex.Message;

                    if (ex.ToString().Contains("timeout"))
                    {
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Request Timeout";
                        dsBatch = new DALEDIEligibility().UpdateEDIEligibility(dsBatch);

                    }
                    else
                    {
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Fail";
                        dsBatch = new DALEDIEligibility().UpdateEDIEligibility(dsBatch);
                    }

                }


                MDVLogger.BLLErrorLog("BLLBilling::PatientEligibility", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="PatientInsuranceId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="DOS"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <param name="EQService"></param>
        /// <returns></returns>
        public BLObject<DSPatientEligibility> PatientEligibility(long PatientId, long PatientInsuranceId, long ProviderId, DateTime DOS, SharedVariable SharedVariable, string EQService = "")
        {
            DSPatientEligibility dsBatch = null;
            DS270 ds270 = new DS270();
            DS271 ds271 = new DS271();
            Int64 batchId = 0;
            string IsEligible = "false";
            BLObject<DSPatientEligibility> objBatch;

            //to only recode Parsing Exceptions.
            bool IsUpdateException = false;

            try
            {

                //1- Create PatientEligibilityBatch
                objBatch = CreatePatientEligibilityBatch(PatientId, PatientInsuranceId, ProviderId, DOS, EQService, "Initial", SharedVariable);
                if (objBatch.Data != null)
                {
                    dsBatch = objBatch.Data;
                    batchId = Convert.ToInt64(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EDIEligibilityIdColumn]);

                    //2- Get270Header and Get270ServicesInfo
                    ds270.Merge(new DALEDIEligibility(SharedVariable).Load270Header(batchId, SharedVariable));
                    ds270.Merge(new DALEDIEligibility(SharedVariable).Load270Names(batchId, SharedVariable));

                    if (ds270.EDI270Header.Rows.Count > 0)
                    {
                        //3- Create 270String
                        IsUpdateException = true;
                        EDI270Parser parser = new EDI270Parser(ds270);
                        string Str270 = parser.Get270();
                        IsUpdateException = false;

                        //4- Update PatientEligibilityBatch with 270String
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str270Column] = Str270;
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Waiting";

                        //5- Request for Patient Eligibility
                        //string Str271 = PatientEligibilitySubmit270(parser.SubmitterId, parser.SubmitterPassword, parser.URL, Str270, SharedVariable);
                        string Str271 = PatientEligibilitySubmit270(parser.SubmitterId, parser.SubmitterPassword, parser.URL, Str270, SharedVariable);
                        //Task<string> EligibilityTask = null;
                        //EligibilityTask = new Task<string>(() => (string)(PatientEligibilitySubmit270(parser.SubmitterId, parser.SubmitterPassword, parser.URL, Str270, SharedVariable)));
                        //if (EligibilityTask != null) EligibilityTask.Start();
                        //if (EligibilityTask != null) EligibilityTask.Wait();
                        //string Str271 = EligibilityTask.Result;

                        if (!string.IsNullOrEmpty(Str271))
                        {
                            //6-a Update PatientEligibilityBatch with 271String
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str271Column] = Str271;

                            string ServiceTypeCode = MDVUtility.ToStr(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EQSeviceColumn]);

                            //7- Get Patient Eligibility Report
                            BLObject<DS271> obj = PatientEligibilityReport(0, SharedVariable, Str271, ServiceTypeCode);
                            if (obj.Data != null)
                            {
                                ds271 = obj.Data;
                                dsBatch.Merge(ds271);

                                //Update Copay Deductible and Eligibility Status
                                if (ds271.EDI271Header.Rows.Count > 0)
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CopayColumn] = MDVUtility.ToDouble(ds271.EDI271Header.Rows[0][ds271.EDI271Header.CopayColumn]);
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.DeductibleColumn] = MDVUtility.ToDouble(ds271.EDI271Header.Rows[0][ds271.EDI271Header.DeductibleColumn]);

                                    IsEligible = (string)ds271.EDI271Header.Rows[0][ds271.EDI271Header.IsEligibleColumn];

                                }

                                //Update Eligibility for Patient
                                DALPatientInsurance ObjPatientInsurance = new DALPatientInsurance(SharedVariable);
                                DSPatient dsPatient = ObjPatientInsurance.LoadPatientInsurance(PatientInsuranceId, 0, 0, SharedVariable);

                                if (dsPatient != null)
                                {

                                    if (dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count > 0)
                                    {
                                        DSPatient.PatientInsuranceRow dr = (DSPatient.PatientInsuranceRow)dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[0];
                                        if (IsEligible == "Active")
                                            dr.EligibilityStatus = "Active";
                                        else if (IsEligible == "InActive")
                                            dr.EligibilityStatus = "Inactive";
                                        else
                                            dr.EligibilityStatus = "Waiting";


                                        dr.EligibilityDate = DOS;

                                        dr.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        dr.ModifiedOn = DateTime.Now;

                                        ObjPatientInsurance.UpdatePatientInsurance(dsPatient, SharedVariable);
                                    }
                                }

                                if (IsEligible == "Active")
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Active";
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Patient eligibility checked successfully.";

                                }
                                else if (IsEligible == "InActive")
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Inactive";
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Patient eligibility checked successfully.";

                                }
                                else
                                {
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Waiting";
                                    dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Patient eligibility response get with error.";

                                }


                            }
                            else
                            {
                                IsUpdateException = true;
                                throw new Exception(obj.Message);
                            }
                        }
                        else
                        {
                            //6-b Update PatientEligibilityBatch with 271String
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str271Column] = Str271;
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Waiting";
                            dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = "Can not get response from patient eligibility service.";
                        }

                        dsBatch = new DALEDIEligibility(SharedVariable).UpdateEDIEligibility(dsBatch, SharedVariable);
                    }
                    else
                        throw new Exception("Please setup the EDI receiver ID and submitter ID for selected clearing house.");

                }
                else
                {
                    throw new Exception(objBatch.Message);
                }

                return new BLObject<DSPatientEligibility>(dsBatch);
            }
            catch (Exception ex)
            {
                if (dsBatch != null)
                {
                    if (IsUpdateException)
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.CommentsColumn] = ex.Message;

                    if (ex.ToString().Contains("timeout"))
                    {
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Request Timeout";
                        dsBatch = new DALEDIEligibility(SharedVariable).UpdateEDIEligibility(dsBatch, SharedVariable);
                    }
                    else
                    {
                        dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.StatusColumn] = "Fail";
                        dsBatch = new DALEDIEligibility(SharedVariable).UpdateEDIEligibility(dsBatch, SharedVariable);
                    }
                }

                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::PatientEligibilityService", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }
        public string PatientEligibilitySubmit270(string submitterIdentifier, string submitterPassword, string url, string Str270, SharedVariable SharedVariable = null)
        {
            try
            {
                SecurityHeaderType Header = new SecurityHeaderType();
                Header.submitterIdentifier = submitterIdentifier; // 4R6N1553;
                Header.submitterPassword = submitterPassword; // 0\\LhzUmi6;
                Header.originatingIdentifier = submitterIdentifier; //4R6N1553;
                Header.submissionId = new Random().Next();


                SubmitRequestType RequestType = new SubmitRequestType();
                RequestType.timeout = "60";
                RequestType.submittedAnsiVersion = "270";
                RequestType.transactionType = "E";
                RequestType.resultAnsiVersion = "5010A1";
                RequestType.processingOption = "R"; //For real time only.
                RequestType.payload = Str270;

                Uri epUri = new Uri(url);
                EndpointAddress endPoint = new EndpointAddress(epUri);
                NavicureSubmissionServiceClient Navicure = new NavicureSubmissionServiceClient("NavicureSubmissionService", endPoint);

                ResponseType ResponseType = Navicure.submitAnsiSingle(Header, RequestType);
                StatusHeaderType Status = ResponseType.statusHeader;
                if (Status.statusMessage.Equals("Success"))
                {
                    return ResponseType.payload;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (SharedVariable != null)
                    MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::PatientEligibilitySubmit270Service", ex);
                else
                    MDVLogger.BLLErrorLog("BLLBilling::PatientEligibilitySubmit270Service", ex);
                //Unable to connect service.
                //Either service URL is not setup or invalid. Please contact MDVision

                if (ex.ToString().ToLower().Contains("timeout"))
                {
                    throw new Exception("Request timeout occur");
                }
                else
                {
                    throw new Exception("Either service URL is not setup or invalid. Please contact MDVision.");
                }
            }
        }

        public BLObject<DS271> PatientEligibilityReport(long BatchId, string Str271 = "", string ServiceTypeCode = "")
        {
            DS271 dsEligibility = new DS271();
            DSPatientEligibility dsBatch = null;
            string ServiceCode = string.Empty;
            try
            {
                if (BatchId != 0)
                {
                    dsBatch = new DALEDIEligibility().LoadEDIEligibility(BatchId, 0, 0, 0, null, null, null, null, null, null);
                    if (dsBatch != null)
                    {
                        Str271 = MDVUtility.ToStr(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str271Column]);
                        ServiceCode = MDVUtility.ToStr(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EQSeviceColumn]);
                    }

                }

                if (string.IsNullOrEmpty(ServiceTypeCode))
                    ServiceTypeCode = ServiceCode;

                if (!string.IsNullOrEmpty(Str271) && !string.IsNullOrEmpty(ServiceTypeCode))
                {
                    EDI271Parser parser271 = new EDI271Parser();

                    string Str71Readable = parser271.GetHumanReadable271(Str271, ref dsEligibility, ServiceTypeCode);

                    ////Filter to contains only those benefits that have requested ServiceTypeCode
                    //DataView view = new DataView(dsEligibility.EDI271Benefits);
                    //view.RowFilter = dsEligibility.EDI271Benefits.ServiceTypeCodeColumn.ColumnName + " NOT LIKE '%" + ServiceTypeCode + "%' OR " + dsEligibility.EDI271Benefits.ServiceTypeCodeColumn.ColumnName + " is null";
                    //foreach (DataRowView datarow in view)
                    //    datarow.Delete();


                    if (dsBatch != null)
                        dsEligibility.Merge(dsBatch);

                    return new BLObject<DS271>(dsEligibility);
                }
                else
                    return new BLObject<DS271>(null, "Patient Eligibility request has no response.");
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLBilling::PatientEligibilityReport", ex);
                return new BLObject<DS271>(null, ex.Message);
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="BatchId"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <param name="Str271"></param>
        /// <param name="ServiceTypeCode"></param>
        /// <returns></returns>
        public BLObject<DS271> PatientEligibilityReport(long BatchId, SharedVariable SharedVariable, string Str271 = "", string ServiceTypeCode = "")
        {
            DS271 dsEligibility = new DS271();
            DSPatientEligibility dsBatch = null;
            string ServiceCode = string.Empty;
            try
            {
                if (BatchId != 0)
                {
                    dsBatch = new DALEDIEligibility(SharedVariable).LoadEDIEligibility(BatchId, 0, 0, 0, null, null, null, null, null, null, SharedVariable);
                    if (dsBatch != null)
                    {
                        Str271 = MDVUtility.ToStr(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.Str271Column]);
                        ServiceCode = MDVUtility.ToStr(dsBatch.EDIEligibility.Rows[0][dsBatch.EDIEligibility.EQSeviceColumn]);
                    }

                }

                if (string.IsNullOrEmpty(ServiceTypeCode))
                    ServiceTypeCode = ServiceCode;

                if (!string.IsNullOrEmpty(Str271) && !string.IsNullOrEmpty(ServiceTypeCode))
                {
                    EDI271Parser parser271 = new EDI271Parser();

                    string Str71Readable = parser271.GetHumanReadable271(Str271, ref dsEligibility, ServiceTypeCode);

                    ////Filter to contains only those benefits that have requested ServiceTypeCode
                    //DataView view = new DataView(dsEligibility.EDI271Benefits);
                    //view.RowFilter = dsEligibility.EDI271Benefits.ServiceTypeCodeColumn.ColumnName + " NOT LIKE '%" + ServiceTypeCode + "%' OR " + dsEligibility.EDI271Benefits.ServiceTypeCodeColumn.ColumnName + " is null";
                    //foreach (DataRowView datarow in view)
                    //    datarow.Delete();


                    if (dsBatch != null)
                        dsEligibility.Merge(dsBatch);

                    return new BLObject<DS271>(dsEligibility);
                }
                else
                    return new BLObject<DS271>(null, "Patient Eligibility request has no response.");
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::PatientEligibilityReportService", ex);
                return new BLObject<DS271>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteEDIEligibility(long EDIEligibilityId)
        {
            try
            {
                return new BLObject<string>(new DALEDIEligibility().DeleteEDIEligibility(EDIEligibilityId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteEDIEligibility", ex);
                return new BLObject<string>(ex.Message);
            }
        }

        public BLObject<DSPatientEligibility> LoadPatientEligibilityService(SharedVariable SharedVariable, DateTime? DOS)
        {
            try
            {
                DSPatientEligibility ds = new DSPatientEligibility();
                ds = new DALEDIEligibility(SharedVariable).LoadPatientEligibilityService(DOS, SharedVariable);
                return new BLObject<DSPatientEligibility>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::LoadPatientEligibilityService", ex);
                return new BLObject<DSPatientEligibility>(null, ex.Message);
            }
        }

        #endregion

        #region  FollowUps

        #region Patient FollowUp
        public BLObject<DSFollowUp> LoadFollowUpPatientARDetail(Int64 FollowUpARDetailID, string PatientAccount, long ProviderId, long FacilityId, string ClaimNumber, DateTime? DOSFrom, DateTime? DOSTo, long groupId, long ActionId, long ReasonId, long InsurancePlanId, string LastName, string FirstName, string suspended, long Age, string ARType = "", string LastModified="", string LastComment="", int PageNumber = 0, int RowspPage = 15)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALPatientAR().LoadFollowUpPatientARDetail(FollowUpARDetailID, PatientAccount, ProviderId, FacilityId, ClaimNumber, DOSFrom, DOSTo, groupId, ActionId, ReasonId, InsurancePlanId, LastName, FirstName, suspended, Age, ARType,LastModified,LastComment, PageNumber, RowspPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadFollowUpAR", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertFollowUpPatientARDetail(DSFollowUp ds)
        {
            try
            {
                ds = new DALPatientAR().InsertFollowUpPatientARDetail(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertFollowUpARDetail", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateFollowUpPatientARDetail(DSFollowUp ds)
        {
            try
            {
                ds = new DALPatientAR().UpdateFollowUpPatientARDetail(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateFollowUpARDetail", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteFollowUpPatientARDetail(Int64 followUpARDetailID)
        {
            try
            {
                return new BLObject<string>(new DALPatientAR().DeleteFollowUpPatientARDetail(followUpARDetailID));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteFollowUpARDetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region Insurance FollowUp
        public BLObject<DSFollowUp> LoadFollowUpInsuranceARDetail(Int64 FollowUpARDetailID, Int64 VisitId, string PatientAccount, long ProviderId, long FacilityId, string ClaimNumber, DateTime? DOSFrom, DateTime? DOSTo, long groupId, long ActionId, long ReasonId, long InsurancePlanId, string LastName, string FirstName, string suspended, long Age, long ClaimType, string ARType = "", int PageNumber = 0, int RowspPage = 15, string Module = "")
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALInsuranceAR().LoadFollowUpARDetail(FollowUpARDetailID, VisitId, PatientAccount, ProviderId, FacilityId, ClaimNumber, DOSFrom, DOSTo, groupId, ActionId, ReasonId, InsurancePlanId, LastName, FirstName, suspended, Age, ClaimType, ARType, PageNumber, RowspPage, Module);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadFollowUpAR", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        public BLObject<DSFollowUp> LoadFollowUpInsuranceARDetailSelect(Int64 FollowUpARDetailID, Int64 VisitId, string PatientAccount, long ProviderId, long FacilityId, string ClaimNumber, DateTime? DOSFrom, DateTime? DOSTo, long groupId, long ActionId, long ReasonId, long InsurancePlanId, string LastName, string FirstName, string suspended, long Age, long ClaimType,string NameInitialTo,string NameInitialFrom,double InsBalGreater, double InsBalLess, long PlanCategory, string ARType = "", int PageNumber = 0, int RowspPage = 15, string Module = "",string LastModified="",string  LastComment="")
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALInsuranceAR().LoadFollowUpARDetailSelect(FollowUpARDetailID, VisitId, PatientAccount, ProviderId, FacilityId, ClaimNumber, DOSFrom, DOSTo, groupId, ActionId, ReasonId, InsurancePlanId, LastName, FirstName, suspended, Age, ClaimType,NameInitialTo, NameInitialFrom, InsBalGreater , InsBalLess, PlanCategory, ARType, PageNumber, RowspPage, Module,LastModified,LastComment);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadFollowUpInsuranceARDetailSelect", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        public BLObject<DSFollowUp> InsertFollowUpInsuranceARDetail(DSFollowUp ds)
        {
            try
            {
                ds = new DALInsuranceAR().InsertFollowUpARDetail(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertFollowUpARDetail", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateFollowUpInsuranceARDetail(DSFollowUp ds)
        {
            try
            {
                ds = new DALInsuranceAR().UpdateFollowUpARDetail(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateFollowUpARDetail", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteFollowUpInsuranceARDetail(Int64 followUpARDetailID)
        {
            try
            {
                return new BLObject<string>(new DALInsuranceAR().DeleteFollowUpARDetail(followUpARDetailID));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteFollowUpARDetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> SplitInsuranceARClaim(Int64 VisitId, string ChargeIds)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpSplitClaim().SplitClaim(VisitId, ChargeIds));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SplitInsuranceARClaim", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Load Insurance AR Activity Logs
        /// </summary>
        public BLObject<DataSet> LoadInsuranceARActivityLogs(string ModuleName, string UserName, string ColumnKeyId, string ProfileName, string DBAuditId, string PatientId, string VisitId, DateTime? createdDate, string DBTableName)
        {
            try
            {

                DSDBAudit ds = new DSDBAudit();
                ds = new DBActivityAudit().LoadDBAudit(ModuleName, UserName, ColumnKeyId, ProfileName, DBAuditId, PatientId, VisitId, createdDate, DBTableName);

                //DSPatient ds = new DALActivityLog().LoadActivityLog(PatientId,PatientActivityLogId);
                DataSet dsResults = new DataSet();
                if (ds.DBAudit.Rows.Count > 0)
                {

                    //GroupHistory DataTable
                    DataTable dtGroupHistory = new DataTable("GroupHistory");
                    dtGroupHistory.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtGroupHistory.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                    dtGroupHistory.Columns.Add(new DataColumn("EnteredBy", typeof(string)));
                    dtGroupHistory.Columns.Add(new DataColumn("OriginalGroup", typeof(string)));
                    dtGroupHistory.Columns.Add(new DataColumn("CurrentGroup", typeof(string)));
                    dtGroupHistory.Columns.Add(new DataColumn("GroupAge", typeof(string)));
                    DataRow[] GroupHistoryRows = ds.Tables[ds.DBAudit.TableName].Select("ColumnName='ARGroupName' and DBAuditAction='update' ");
                    DateTime dtOrignal = DateTime.Now;

                    foreach (var currentRow in GroupHistoryRows)
                    {
                        if (MDVUtility.ToStr(currentRow[ds.DBAudit.OriginalValueColumn.ColumnName]) != "")
                        {
                            dtGroupHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], (Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).Date.Subtract(dtOrignal.Date)).Days);
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                        else
                        {
                            dtGroupHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], "");
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }

                    }
                    dsResults.Tables.Add(dtGroupHistory.DefaultView.ToTable(true, "ActivityLogId", "EntryDate", "EnteredBy", "OriginalGroup", "CurrentGroup", "GroupAge"));

                    //ActionHistory Data Table
                    dtOrignal = DateTime.Now;
                    DataTable dtActionHistory = new DataTable("ActionHistory");
                    dtActionHistory.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtActionHistory.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                    dtActionHistory.Columns.Add(new DataColumn("EnteredBy", typeof(string)));
                    dtActionHistory.Columns.Add(new DataColumn("OriginalAction", typeof(string)));
                    dtActionHistory.Columns.Add(new DataColumn("CurrentAction", typeof(string)));
                    dtActionHistory.Columns.Add(new DataColumn("ActionAge", typeof(string)));
                    DataRow[] ActionHistoryRows = ds.Tables[ds.DBAudit.TableName].Select("ColumnName='FollowupActionName' and DBAuditAction='update' ");
                    foreach (var currentRow in ActionHistoryRows)
                    {
                        if (MDVUtility.ToStr(currentRow[ds.DBAudit.OriginalValueColumn.ColumnName]) != "")
                        {
                            dtActionHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], (Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).Date.Subtract(dtOrignal.Date)).Days);
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                        else
                        {
                            dtActionHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], "");
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                    }
                    dsResults.Tables.Add(dtActionHistory.DefaultView.ToTable(true, "ActivityLogId", "EntryDate", "EnteredBy", "OriginalAction", "CurrentAction", "ActionAge"));

                    //ReasonHistory Data Table
                    dtOrignal = DateTime.Now;
                    DataTable dtReasonHistory = new DataTable("ReasonHistory");
                    dtReasonHistory.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtReasonHistory.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                    dtReasonHistory.Columns.Add(new DataColumn("EnteredBy", typeof(string)));
                    dtReasonHistory.Columns.Add(new DataColumn("OriginalReason", typeof(string)));
                    dtReasonHistory.Columns.Add(new DataColumn("CurrentReason", typeof(string)));
                    dtReasonHistory.Columns.Add(new DataColumn("ReasonAge", typeof(string)));
                    DataRow[] ReasonHistoryRows = ds.Tables[ds.DBAudit.TableName].Select("ColumnName='FollowupReasonName' and DBAuditAction='update' ");
                    foreach (var currentRow in ReasonHistoryRows)
                    {
                        if (MDVUtility.ToStr(currentRow[ds.DBAudit.OriginalValueColumn.ColumnName]) != "")
                        {
                            dtReasonHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], (Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).Date.Subtract(dtOrignal.Date)).Days);
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                        else
                        {
                            dtReasonHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], "");
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                    }
                    dsResults.Tables.Add(dtReasonHistory.DefaultView.ToTable(true, "ActivityLogId", "EntryDate", "EnteredBy", "OriginalReason", "CurrentReason", "ReasonAge"));

                    //RemitCodeHistory Data Table
                    dtOrignal = DateTime.Now;
                    DataTable dtRemitCodeHistory = new DataTable("RemitCodeHistory");
                    dtRemitCodeHistory.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtRemitCodeHistory.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                    dtRemitCodeHistory.Columns.Add(new DataColumn("EnteredBy", typeof(string)));
                    dtRemitCodeHistory.Columns.Add(new DataColumn("OriginalRemitCode", typeof(string)));
                    dtRemitCodeHistory.Columns.Add(new DataColumn("CurrentRemitCode", typeof(string)));
                    dtRemitCodeHistory.Columns.Add(new DataColumn("CodeAge", typeof(string)));
                    DataRow[] RemitCodeHistoryRows = ds.Tables[ds.DBAudit.TableName].Select("ColumnName='RemittanceCode' and DBAuditAction='update' ");
                    foreach (var currentRow in RemitCodeHistoryRows)
                    {
                        if (MDVUtility.ToStr(currentRow[ds.DBAudit.OriginalValueColumn.ColumnName]) != "")
                        {
                            dtRemitCodeHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], (Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).Date.Subtract(dtOrignal.Date)).Days);
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                        else
                        {
                            dtRemitCodeHistory.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.OriginalValueColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], "");
                            dtOrignal = MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);

                        }
                    }
                    dsResults.Tables.Add(dtRemitCodeHistory.DefaultView.ToTable(true, "ActivityLogId", "EntryDate", "EnteredBy", "OriginalRemitCode", "CurrentRemitCode", "CodeAge"));

                }
                return new BLObject<DataSet>(dsResults);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadInsuranceARActivityLogs", ex);
                return new BLObject<DataSet>(null, ex.Message);
            }
        }

        #endregion

        #region FollowUp Calls
        public BLObject<DSFollowUp> LoadFollowUpARCall(Int64 FollowUpARCallID, Int64 ARTypeId, Int64 ActionId, Int64 ReasonId, Int64 InsuranceARDtId, Int64 PatientARDtId, int PageNumber, int RowspPage)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARCall().LoadFollowUpARCall(FollowUpARCallID, ARTypeId, ActionId, ReasonId, InsuranceARDtId, PatientARDtId, PageNumber, RowspPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadFollowUpARCall", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertFollowUpARCall(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARCall().InsertFollowUpARCall(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertFollowUpARCall", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateFollowUpARCall(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARCall().UpdateFollowUpARCall(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateFollowUpARCall", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteFollowUpARCall(Int64 FollowUpARCallID)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARCall().DeleteFollowUpARCall(FollowUpARCallID));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteFollowUpARCall", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> LookupCallStatus()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARCall().LookupCallStatus();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupCallStatus", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        #endregion

        #endregion


        #region Patient Statement
        public BLObject<DSPatientStatement> InsertPatientStatement(SharedVariable SharedVariable, DSPatientStatement ds)
        {
            try
            {
                //SharedVariable SharedVariable = new SharedVariable();
                //SharedVariable.EntityId = MDVSession.Current.EntityId;
                //SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                //SharedVariable.UserName = MDVSession.Current.AppUserName;
                //SharedVariable.ClientId = MDVSession.Current.ClientId;
                //SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                //SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;

                ds = new DALPatientStatement(SharedVariable).InsertPatientStatement(ds);
                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::InsertPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }
        public BLObject<string> DeletePatientStatement(long PatientStatementID)
        {
            try
            {
                return new BLObject<string>(new DALPatientStatement().DeletePatientStatement(PatientStatementID));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeletePatientStatement", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPatientStatement> LoadPrintedPatientStatement(Int64 PatientStatementID, long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? DOSFrom, DateTime? DOSTo)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().LoadPrintedPatientStatement(PatientStatementID, PatientId, patientLastName, PatientFirstName, FacilityId, Age, DOSFrom, DOSTo);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPrintedPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> LoadPatientSubmittedStatement(Int64 PatientStatementID, long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? LastStatementDateFrom, DateTime? LastStatementDateTo, Int64 PatBalanceGreaterThan, Int64 PatBalanceLessThan,int ClearingHouseId=0 , int pageNumber = 1, int RecordPerPage = 15)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().LoadPatientSubmittedStatement(PatientStatementID, PatientId, patientLastName, PatientFirstName, FacilityId, Age, LastStatementDateFrom, LastStatementDateTo, PatBalanceGreaterThan, PatBalanceLessThan, ClearingHouseId, pageNumber, RecordPerPage);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientSubmittedStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> LoadPatientStatement(long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? DOSFrom, DateTime? DOSTo, string statementFormat = "")
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().LoadPatientStatement(PatientId, patientLastName, PatientFirstName, FacilityId, Age, DOSFrom, DOSTo, statementFormat);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> PatientStatementSearch(long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? DOSFrom, DateTime? DOSTo, DateTime? LastStatementDateFrom, DateTime? LastStatementDateTo, Double PatBalanceGreaterThan, Double PatBalanceLessThan, bool isIgnoreCycleDaysChecked, string statementFormat = "", int pageNumber = 1, int RecordPerPage = 15)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().PatientStatementSearch(PatientId, patientLastName, PatientFirstName, FacilityId, Age, DOSFrom, DOSTo, LastStatementDateFrom, LastStatementDateTo, PatBalanceGreaterThan, PatBalanceLessThan, isIgnoreCycleDaysChecked, statementFormat, pageNumber, RecordPerPage);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::PatientStatementSearch", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> LoadPrintSubmitPatientStatement(long PatientId, string lastName, string FirstName, long facilityId, long submittedstatementId, DateTime? DOSFrom, DateTime? DOSTo)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();
                ds = new DALPatientStatement().LoadPrintSubmitPatientStatement(PatientId, lastName, FirstName, facilityId, submittedstatementId, DOSFrom, DOSTo);
                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPrintSubmitPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }
        public BLObject<DSPatientStatement> LoadPrintPatientStatement(long PatientId, string lastName, string FirstName, long facilityId, long submittedstatementId,int Age, DateTime? DOSFrom, DateTime? DOSTo)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().LoadPrintPatientStatement(PatientId, lastName, FirstName, facilityId, submittedstatementId, Age, DOSFrom, DOSTo);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPrintPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> LoadPrintPatientStatement(SharedVariable sharedVariable, long PatientId, string lastName, string FirstName, long facilityId, long submittedstatementId, DateTime? DOSFrom, DateTime? DOSTo)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement(sharedVariable).LoadPrintPatientStatement(sharedVariable, PatientId, lastName, FirstName, facilityId, submittedstatementId, DOSFrom, DOSTo);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPrintPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }


        public string StatementSubmittedStatus(SharedVariable SharedVariable, long PatientId)
        {
            try
            {


                DSPatientStatement ds = new DSPatientStatement();
                string returnval = new DALPatientStatement(SharedVariable).StatementSubmittedStatus(PatientId);
                return returnval;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::StatementSubmittedStatus", ex);
                throw ex;
            }
        }

        public BLObject<DSPatientStatement> GetSubmittedStatementHTML(Int64 SubmittedStatementId)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();
                ds = new DALPatientStatement().GetSubmittedStatementHTML(SubmittedStatementId);
                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::StatementSubmittedStatus", ex);
                throw ex;
            }
        }


        public List<object> MakePatientStatement(List<object> items)
        {
            string firstname = "", lastname = "";
            long patientID, facilityId, submittedStatementId;
            List<object> lstStatement = new List<object>();
            try
            {

                foreach (dynamic item in items)
                {
                    float age0_31 = 0, age31_60 = 0, age61_90 = 0, age91_120 = 0, age120_onward = 0, totalBalance = 0;
                    string StatementMessage = null;
                    int nAge = 0;
                    DSPatientStatement dsPatientStatement = null;
                    BLObject<DSPatientStatement> obj = null;

                    patientID = MDVUtility.ToInt64(item.PatientId);
                    facilityId = MDVUtility.ToInt64(item.FacilityId);
                    submittedStatementId = MDVUtility.ToInt64(item.SubmittedStatementId);
                    nAge = MDVUtility.ToInt32(item.Age);

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    DateTime? DOSFrom = String.IsNullOrEmpty(MDVUtility.ToStr(item.DOSFrom)) ? (DateTime?)null : DateTime.Parse(MDVUtility.ToStr(item.DOSFrom));
                    DateTime? DOSTo = String.IsNullOrEmpty(MDVUtility.ToStr(item.DOSTo)) ? (DateTime?)null : DateTime.Parse(MDVUtility.ToStr(item.DOSTo));


                    if (submittedStatementId != 0)
                    {
                        obj = LoadPrintSubmitPatientStatement(patientID, lastname, firstname, facilityId, submittedStatementId, DOSFrom, DOSTo);
                    }
                    else
                    {
                        obj = LoadPrintPatientStatement(patientID, lastname, firstname, facilityId, submittedStatementId, nAge, DOSFrom, DOSTo);
                    }
                    dsPatientStatement = obj.Data;

                    if (dsPatientStatement.PatientStatementPrint.Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPatientStatement.PatientStatementPrint.Columns.Count; i++)
                        {
                            if (dsPatientStatement.PatientStatementPrint.Rows[0][i].ToString().Contains(","))
                            {
                                dsPatientStatement.PatientStatementPrint.Rows[0][i] = dsPatientStatement.PatientStatementPrint.Rows[0][i].ToString().Replace(',', '#');
                            }
                        }

                        dsPatientStatement.StatementHeader.ImportRow(dsPatientStatement.PatientStatementPrint.Rows[0]);
                        dsPatientStatement.StatementFooter.ImportRow(dsPatientStatement.PatientStatementPrint.Rows[0]);


                        foreach (DataRow dr in dsPatientStatement.PatientStatementPrint)
                        {
                            dsPatientStatement.StatementDetail.ImportRow(dr);
                        }

                        DataView view = new DataView(dsPatientStatement.PatientStatementPrint);
                        DataTable distinctValues = view.ToTable(true, dsPatientStatement.PatientStatementPrint.ChargeCapIdColumn.ColumnName);
                        ArrayList lststatement = new ArrayList();

                        foreach (DataRow dr in distinctValues.Rows)
                        {
                            Int32 value = MDVUtility.ToInt32(dr[dsPatientStatement.PatientStatementPrint.ChargeCapIdColumn.ColumnName]);
                            DataRow[] filterData = dsPatientStatement.StatementDetail.Select(MDVUtility.ToStr(dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName) + " = " + value);

                            int age = 0;
                            float balance = 0, charges;

                            //lststatement.Add()
                            // var LedgerEntries [];
                            object[] ChargeEntry = new object[15];

                            //for the insurance primary,secondry,teriatry insurance payment clump
                            object[] PrimaryLedgerEntriesPayment = new object[15];
                            object[] SecondryLedgerEntriesPayment = new object[15];
                            object[] TertiaryLedgerEntriesPayment = new object[15];
                            object[] SuplementryLedgerEntriesPayment = new object[15];


                            //for the insurance primary,secondry,teriatry insurance write off clump
                            object[] PrimaryLedgerEntriesWriteOff = new object[15];
                            object[] SecondryLedgerEntriesWriteOff = new object[15];
                            object[] TertiaryLedgerEntriesWriteOff = new object[15];
                            object[] SuplementryLedgerEntriesWriteOff = new object[15];


                            object[] LedgerEntriesPatPayment = new object[15];
                            object[] LedgerEntriesPatDiscount = new object[15];
                            object[] LedgerEntriesCopayPayment = new object[15];
                            object[] LedgerEntriesCopayDiscount = new object[15];

                            //for the clump of primary secondry,tertiarty entries
                            object[] PrimaryLedgerEntriesInstoInsForward = new object[15];
                            object[] SecondryLedgerEntriesInstoInsForward = new object[15];
                            object[] TertiaryLedgerEntriesInstoInsForward = new object[15];
                            object[] SuplementryLedgerEntriesInstoInsForward = new object[15];


                            object[] LedgerEntriesPatToInsForward = new object[15];
                            object[] LedgerEntriesPatForward = new object[15];

                            // dtstatement.Columns.Add(new DataColumn("PatBalance", typeof(string)));

                            //payments of different insurance
                            float PrimaryPaymentpaid = 0;
                            float PrimaryPaymentBalance = 0;

                            float SecondryPaymentpaid = 0;
                            float SecondryPaymentBalance = 0;

                            float TertiaryPaymentpaid = 0;
                            float TertiaryPaymentBalance = 0;

                            float SuplementryPaymentpaid = 0;
                            float SuplementryPaymentBalance = 0;


                            ///////////////////////

                            ////Insurance Write off /////////
                            float PrimaryWriteOffpaid = 0;
                            float PrimaryWriteOffPatBalance = 0;

                            float SecondryWriteOffpaid = 0;
                            float SecondryWriteOffPatBalance = 0;

                            float TertiaryWriteOffpaid = 0;
                            float TertiaryWriteOffPatBalance = 0;

                            float SuplementryWriteOffpaid = 0;
                            float SuplementryWriteOffPatBalance = 0;

                            //////////////////////////////
                            float Patientpaid = 0;
                            float PatientBalance = 0;

                            float PatientDiscountpaid = 0;
                            float PatientDiscountBalance = 0;

                            float Copaypaid = 0;
                            float CopaypaidBalance = 0;

                            float CopayDiscountpaid = 0;
                            float CopayDiscountBalance = 0;

                            //float InstoInsForwardPaid = 0;
                            //float InstoInsForwardBalance = 0;

                            //Insurance forward
                            float PrimaryInstoInsForwardPaid = 0;
                            float PrimaryInstoInsForwardBalance = 0;

                            float SecondryInstoInsForwardPaid = 0;
                            float SecondryInstoInsForwardBalance = 0;

                            float TertiaryInstoInsForwardPaid = 0;
                            float TertiaryInstoInsForwardBalance = 0;

                            float SuplementryInstoInsForwardPaid = 0;
                            float SuplementryInstoInsForwardBalance = 0;

                            /////////////////////////////////////





                            float InstoPatForwardPaid = 0;
                            float InstoPatForwardBalance = 0;


                            float PatForwardPaid = 0;
                            float PatForwardBalance = 0;



                            var count = 0;
                            foreach (DataRow data in filterData)
                            {
                                if (!String.IsNullOrEmpty(data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName].ToString()))
                                {
                                    charges = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName]);
                                    age = MDVUtility.ToInt(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    balance = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                    ChargeEntry[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                    ChargeEntry[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                    ChargeEntry[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                    ChargeEntry[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                    ChargeEntry[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                    ChargeEntry[5] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]);
                                    ChargeEntry[6] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                    ChargeEntry[7] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                    ChargeEntry[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                    ChargeEntry[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    ChargeEntry[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                    ChargeEntry[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                    ChargeEntry[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                    ChargeEntry[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                    ChargeEntry[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);


                                    //if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Responsibility"))
                                    //{
                                    LedgerEntriesPatForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                    LedgerEntriesPatForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                    LedgerEntriesPatForward[2] = "";
                                    LedgerEntriesPatForward[3] = "Patient Responsibility";
                                    LedgerEntriesPatForward[4] = 0.00;
                                    LedgerEntriesPatForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                    LedgerEntriesPatForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                    LedgerEntriesPatForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    LedgerEntriesPatForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                    LedgerEntriesPatForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                    LedgerEntriesPatForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                    LedgerEntriesPatForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                    LedgerEntriesPatForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                    //patient responsibilty = Patient+ copay balance balance 
                                    PatForwardPaid = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    PatForwardBalance = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    // }
                                }
                                else
                                {
                                    //for single claim scenerio add insurance to insurance check in the condition.else for three claim scenerios it will not add in the if condition.

                                    // issue in case of patient balance positive and insurance bal negative.then show incorrect amount

                                    //if (MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Copay to Ins" && MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Ins to Pat" && MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Pat to Ins" && MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Ins to Ins")
                                    //{
                                    //    balance = balance - MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                    //}
                                    //sum of same ledger entries
                                    if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Payment") && !data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Payment") && !data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Copay Payment"))
                                    {
                                        if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "1")
                                        {
                                            PrimaryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            PrimaryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            PrimaryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            PrimaryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "2")
                                        {
                                            SecondryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SecondryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            SecondryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SecondryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "3")
                                        {
                                            TertiaryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            TertiaryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            TertiaryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "4")
                                        {
                                            SuplementryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SuplementryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            TertiaryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }

                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("WriteOff"))
                                    {
                                        if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "1")
                                        {
                                            PrimaryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            PrimaryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            PrimaryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            PrimaryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "2")
                                        {
                                            SecondryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SecondryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            SecondryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SecondryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "3")
                                        {
                                            TertiaryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            TertiaryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            TertiaryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "4")
                                        {
                                            SuplementryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SuplementryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            SuplementryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SuplementryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }

                                    }
                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Payment"))
                                    {
                                        LedgerEntriesPatPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesPatPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesPatPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesPatPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesPatPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesPatPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesPatPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesPatPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesPatPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesPatPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesPatPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesPatPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesPatPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        Patientpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        PatientBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Discount"))
                                    {
                                        LedgerEntriesPatDiscount[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesPatDiscount[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesPatDiscount[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesPatDiscount[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesPatDiscount[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesPatDiscount[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesPatDiscount[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesPatDiscount[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        PatientDiscountpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        PatientDiscountBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Copay Payment"))
                                    {
                                        LedgerEntriesCopayPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesCopayPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesCopayPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesCopayPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesCopayPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesCopayPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesCopayPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesCopayPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        Copaypaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        CopaypaidBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Copay Discount"))
                                    {
                                        LedgerEntriesCopayDiscount[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesCopayDiscount[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        CopayDiscountpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        CopayDiscountBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }
                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Forward to") && !data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName].ToString().Contains("Pat to Ins"))
                                    {
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "1")
                                        {
                                            PrimaryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            PrimaryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            PrimaryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            PrimaryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "2")
                                        {
                                            SecondryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SecondryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            SecondryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SecondryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "3")
                                        {
                                            TertiaryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            TertiaryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            TertiaryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                        }
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "4")
                                        {
                                            SuplementryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SuplementryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            SuplementryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SuplementryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                    }
                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Forward to") && !data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName].ToString().Contains("Ins to Ins") && !data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Responsibility"))
                                    {
                                        LedgerEntriesPatToInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesPatToInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        InstoPatForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        InstoPatForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                    }
                                    //else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Responsibility"))
                                    //{
                                    //    LedgerEntriesPatForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                    //    LedgerEntriesPatForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                    //    LedgerEntriesPatForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                    //    LedgerEntriesPatForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                    //    LedgerEntriesPatForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                    //    LedgerEntriesPatForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                    //    LedgerEntriesPatForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                    //    LedgerEntriesPatForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                    //    PatForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                    //    PatForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    //}
                                }
                            }

                            if (ChargeEntry[0] != null)
                            {
                                lststatement.Add(ChargeEntry);
                            }
                            // primary insurance payment and write off and forward
                            if (PrimaryLedgerEntriesPayment[0] != null)
                            {
                                PrimaryLedgerEntriesPayment[6] = PrimaryPaymentpaid;
                                PrimaryLedgerEntriesPayment[7] = PrimaryPaymentBalance;
                                lststatement.Add(PrimaryLedgerEntriesPayment);
                            }

                            if (PrimaryLedgerEntriesWriteOff[0] != null)
                            {
                                PrimaryLedgerEntriesWriteOff[6] = PrimaryWriteOffpaid;
                                PrimaryLedgerEntriesWriteOff[7] = PrimaryWriteOffPatBalance;
                                lststatement.Add(PrimaryLedgerEntriesWriteOff);
                            }

                            if (PrimaryLedgerEntriesInstoInsForward[0] != null)
                            {
                                PrimaryLedgerEntriesInstoInsForward[6] = PrimaryInstoInsForwardPaid;
                                PrimaryLedgerEntriesInstoInsForward[7] = PrimaryInstoInsForwardBalance;
                                lststatement.Add(PrimaryLedgerEntriesInstoInsForward);
                            }

                            ////////////////////

                            // Secondry insurance payment and write off and forward

                            if (SecondryLedgerEntriesPayment[0] != null)
                            {
                                SecondryLedgerEntriesPayment[6] = SecondryPaymentpaid;
                                SecondryLedgerEntriesPayment[7] = SecondryPaymentBalance;
                                lststatement.Add(SecondryLedgerEntriesPayment);

                            }

                            if (SecondryLedgerEntriesWriteOff[0] != null)
                            {
                                SecondryLedgerEntriesWriteOff[6] = SecondryWriteOffpaid;
                                SecondryLedgerEntriesWriteOff[7] = SecondryWriteOffPatBalance;
                                lststatement.Add(SecondryLedgerEntriesWriteOff);

                            }

                            if (SecondryLedgerEntriesInstoInsForward[0] != null)
                            {
                                SecondryLedgerEntriesInstoInsForward[6] = SecondryInstoInsForwardPaid;
                                SecondryLedgerEntriesInstoInsForward[7] = SecondryInstoInsForwardBalance;
                                lststatement.Add(SecondryLedgerEntriesInstoInsForward);
                            }


                            /////////////////////////////////////////

                            // Teriary insurance payment and write off and forward

                            if (TertiaryLedgerEntriesPayment[0] != null)
                            {
                                TertiaryLedgerEntriesPayment[6] = TertiaryPaymentpaid;
                                TertiaryLedgerEntriesPayment[7] = TertiaryPaymentBalance;
                                lststatement.Add(TertiaryLedgerEntriesPayment);
                            }

                            if (TertiaryLedgerEntriesWriteOff[0] != null)
                            {
                                TertiaryLedgerEntriesWriteOff[6] = TertiaryWriteOffpaid;
                                TertiaryLedgerEntriesWriteOff[7] = TertiaryWriteOffPatBalance;
                                lststatement.Add(TertiaryLedgerEntriesWriteOff);
                            }


                            if (TertiaryLedgerEntriesInstoInsForward[0] != null)
                            {
                                TertiaryLedgerEntriesInstoInsForward[6] = TertiaryInstoInsForwardPaid;
                                TertiaryLedgerEntriesInstoInsForward[7] = TertiaryInstoInsForwardBalance;
                                lststatement.Add(TertiaryLedgerEntriesInstoInsForward);
                            }


                            /////////////////////////////////

                            // Suplementry insurance payment and write off and forward
                            if (SuplementryLedgerEntriesPayment[0] != null)
                            {
                                SuplementryLedgerEntriesPayment[6] = SuplementryPaymentpaid;
                                SuplementryLedgerEntriesPayment[7] = SuplementryPaymentBalance;
                                lststatement.Add(SuplementryLedgerEntriesPayment);
                            }


                            if (SuplementryLedgerEntriesWriteOff[0] != null)
                            {
                                SuplementryLedgerEntriesWriteOff[6] = SuplementryWriteOffpaid;
                                SuplementryLedgerEntriesWriteOff[7] = SuplementryWriteOffPatBalance;
                                lststatement.Add(SuplementryLedgerEntriesWriteOff);
                            }

                            if (SuplementryLedgerEntriesInstoInsForward[0] != null)
                            {
                                SuplementryLedgerEntriesInstoInsForward[6] = SuplementryInstoInsForwardPaid;
                                SuplementryLedgerEntriesInstoInsForward[7] = SuplementryInstoInsForwardBalance;
                                lststatement.Add(SuplementryLedgerEntriesInstoInsForward);
                            }

                            ///////////////////////////////////


                            if (LedgerEntriesPatPayment[0] != null)
                            {
                                LedgerEntriesPatPayment[6] = Patientpaid;
                                LedgerEntriesPatPayment[7] = PatientBalance;
                                lststatement.Add(LedgerEntriesPatPayment);
                            }
                            if (LedgerEntriesPatDiscount[0] != null)
                            {
                                LedgerEntriesPatDiscount[6] = PatientDiscountpaid;
                                LedgerEntriesPatDiscount[7] = PatientDiscountBalance;
                                lststatement.Add(LedgerEntriesPatDiscount);
                            }

                            if (LedgerEntriesCopayPayment[0] != null)
                            {
                                LedgerEntriesCopayPayment[6] = Copaypaid;
                                LedgerEntriesCopayPayment[7] = CopaypaidBalance;
                                lststatement.Add(LedgerEntriesCopayPayment);
                            }
                            if (LedgerEntriesCopayDiscount[0] != null)
                            {
                                LedgerEntriesCopayDiscount[6] = CopayDiscountpaid;
                                LedgerEntriesCopayDiscount[7] = CopayDiscountBalance;
                                lststatement.Add(LedgerEntriesCopayDiscount);
                            }


                            //////////////////////////////////

                            if (LedgerEntriesPatToInsForward[0] != null)
                            {
                                LedgerEntriesPatToInsForward[6] = InstoPatForwardPaid;
                                LedgerEntriesPatToInsForward[7] = InstoPatForwardBalance;
                                lststatement.Add(LedgerEntriesPatToInsForward);
                            }

                            if (LedgerEntriesPatForward[0] != null)
                            {
                                LedgerEntriesPatForward[6] = PatForwardPaid;
                                LedgerEntriesPatForward[7] = PatForwardBalance;
                                lststatement.Add(LedgerEntriesPatForward);
                            }


                            //LedgerEntriesPayment[count] = paid;
                            //  LedgerEntriesPayment[count] = PatBalance;
                            count++;

                            if (age >= 0 && age <= 30)
                                age0_31 += balance;
                            else if (age >= 31 && age <= 60)
                                age31_60 += balance;
                            else if (age >= 61 && age <= 90)
                                age61_90 += balance;
                            else if (age >= 91 && age <= 120)
                                age91_120 += balance;
                            else if (age >= 121)
                                age120_onward += balance;
                            totalBalance += balance;


                        }

                        if (age0_31 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.FirstMessageColumn.ColumnName]);
                        if (age31_60 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.SecondMessageColumn.ColumnName]);
                        if (age61_90 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.ThirdMessageColumn.ColumnName]);
                        if (age91_120 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.FourthMessageColumn.ColumnName]);
                        if (age120_onward > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.FifthMessageColumn.ColumnName]);

                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age0_30Column.ColumnName] = age0_31;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age31_60Column.ColumnName] = age31_60;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age61_90Column.ColumnName] = age61_90;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age91_120Column.ColumnName] = age91_120;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age121_OnwardColumn.ColumnName] = age120_onward;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter.PatBalanceColumn.ColumnName] = totalBalance;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter.StatementMessageColumn.ColumnName] = StatementMessage;




                        dic.Add(dsPatientStatement.StatementHeader.TableName, MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementHeader.TableName], "", false));
                        dic.Add(dsPatientStatement.StatementFooter.TableName, MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementFooter.TableName], "", false));

                        DataTable dtStatement = new DataTable("StatementDetail");
                        dtStatement.Columns.Add(new DataColumn("Charges", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                        dtStatement.Columns.Add(new DataColumn("FullName", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Procedure", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Description", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("LedgerType", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Paid", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("PatBalance", typeof(string)));

                        dtStatement.Columns.Add(new DataColumn("ChargeCapId", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Age", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("VisitId", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("ClaimNumber", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Units", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("ChargeProviderId", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("ChargeFacilityId", typeof(string)));

                        //for statement detail table
                        //for (int i = 0; i < lststatement.Count; i++)
                        //{
                        //    DataRow drStatement = dtStatement.NewRow();
                        //    drStatement["Charges"] = 0.0;
                        //    drStatement["Date"] = lststatement[i];
                        //    drStatement["FullName"] = lststatement[i];
                        //    drStatement["Procedure"] = lststatement[i];
                        //    drStatement["Description"] = lststatement[i];
                        //    drStatement["Paid"] = lststatement[i];
                        //}

                        //LedgerEntriesPatForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                        //        LedgerEntriesPatForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                        //        LedgerEntriesPatForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                        //        LedgerEntriesPatForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                        //        LedgerEntriesPatForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                        //        PatForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                        //        PatForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                        //    }
                        foreach (object[] itemArray in lststatement)
                        {
                            DataRow drStatement = dtStatement.NewRow();
                            drStatement["Date"] = itemArray[0];
                            drStatement["FullName"] = itemArray[1];
                            drStatement["Procedure"] = itemArray[2];
                            drStatement["Description"] = itemArray[3];
                            drStatement["Charges"] = itemArray[4];
                            drStatement["LedgerType"] = itemArray[5];
                            drStatement["Paid"] = itemArray[6];
                            drStatement["PatBalance"] = itemArray[7];

                            drStatement["ChargeCapId"] = itemArray[8];
                            drStatement["Age"] = itemArray[9];
                            drStatement["VisitId"] = itemArray[10];
                            drStatement["ClaimNumber"] = itemArray[11];
                            drStatement["Units"] = itemArray[12];
                            drStatement["ChargeProviderId"] = itemArray[13];
                            drStatement["ChargeFacilityId"] = itemArray[14];

                            dtStatement.Rows.Add(drStatement);
                        }

                        //  dic.Add(dsPatientStatement.StatementDetail.TableName, MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementDetail.TableName], "", false));
                        dic.Add(dsPatientStatement.StatementDetail.TableName, MDVUtility.JSON_DataTable(dtStatement, "", false));

                        //dic[dsPatientStatement.StatementHeader.TableName] = dic[dsPatientStatement.StatementHeader.TableName].Replace("&#39;","'");
                        //dic[dsPatientStatement.StatementFooter.TableName] = dic[dsPatientStatement.StatementFooter.TableName].Replace("&#39;", "'");
                        //dic[dsPatientStatement.StatementDetail.TableName] = dic[dsPatientStatement.StatementDetail.TableName].Replace("&#39;", "'");

                        //dic[dsPatientStatement.StatementHeader.TableName] = dic[dsPatientStatement.StatementHeader.TableName].Replace("&#233;", "é");
                        //dic[dsPatientStatement.StatementFooter.TableName] = dic[dsPatientStatement.StatementFooter.TableName].Replace("&#233;", "é");
                        //dic[dsPatientStatement.StatementDetail.TableName] = dic[dsPatientStatement.StatementDetail.TableName].Replace("&#233;", "é");
                        lstStatement.Add(dic);
                    }



                }
                return lstStatement;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::MakePatientStatement", ex);
                return lstStatement;
            }
        }

        /// <summary>
        /// Method for multithreading process 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<object> MakePatientStatement(SharedVariable sharedVariable, List<object> items)
        {
            string firstname = "", lastname = "";
            long patientID, facilityId, submittedStatementId;
            List<object> lstStatement = new List<object>();
            try
            {

                foreach (dynamic item in items)
                {
                    float age0_31 = 0, age31_60 = 0, age61_90 = 0, age91_120 = 0, age120_onward = 0, totalBalance = 0;
                    string StatementMessage = null;
                    DSPatientStatement dsPatientStatement = null;
                    BLObject<DSPatientStatement> obj = null;

                    patientID = MDVUtility.ToInt64(item.PatientId);
                    facilityId = MDVUtility.ToInt64(item.FacilityId);
                    submittedStatementId = MDVUtility.ToInt64(item.SubmittedStatementId);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    DateTime? DOSFrom = String.IsNullOrEmpty(MDVUtility.ToStr(item.DOSFrom)) ? (DateTime?)null : DateTime.Parse(MDVUtility.ToStr(item.DOSFrom));
                    DateTime? DOSTo = String.IsNullOrEmpty(MDVUtility.ToStr(item.DOSTo)) ? (DateTime?)null : DateTime.Parse(MDVUtility.ToStr(item.DOSTo));
                    obj = LoadPrintPatientStatement(sharedVariable, patientID, lastname, firstname, facilityId, submittedStatementId, DOSFrom, DOSTo);
                    dsPatientStatement = obj.Data;

                    if (dsPatientStatement.PatientStatementPrint.Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPatientStatement.PatientStatementPrint.Columns.Count; i++)
                        {
                            if (dsPatientStatement.PatientStatementPrint.Rows[0][i].ToString().Contains(","))
                            {
                                dsPatientStatement.PatientStatementPrint.Rows[0][i] = dsPatientStatement.PatientStatementPrint.Rows[0][i].ToString().Replace(',', '#');
                            }
                        }

                        dsPatientStatement.StatementHeader.ImportRow(dsPatientStatement.PatientStatementPrint.Rows[0]);
                        dsPatientStatement.StatementFooter.ImportRow(dsPatientStatement.PatientStatementPrint.Rows[0]);


                        foreach (DataRow dr in dsPatientStatement.PatientStatementPrint)
                        {
                            dsPatientStatement.StatementDetail.ImportRow(dr);
                        }

                        DataView view = new DataView(dsPatientStatement.PatientStatementPrint);
                        DataTable distinctValues = view.ToTable(true, dsPatientStatement.PatientStatementPrint.ChargeCapIdColumn.ColumnName);
                        ArrayList lststatement = new ArrayList();

                        foreach (DataRow dr in distinctValues.Rows)
                        {
                            Int32 value = MDVUtility.ToInt32(dr[dsPatientStatement.PatientStatementPrint.ChargeCapIdColumn.ColumnName]);
                            DataRow[] filterData = dsPatientStatement.StatementDetail.Select(dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName.ToString() + " = " + value);

                            int age = 0;
                            float balance = 0, charges;

                            //lststatement.Add()
                            // var LedgerEntries [];
                            object[] ChargeEntry = new object[15];

                            //for the insurance primary,secondry,teriatry insurance payment clump
                            object[] PrimaryLedgerEntriesPayment = new object[15];
                            object[] SecondryLedgerEntriesPayment = new object[15];
                            object[] TertiaryLedgerEntriesPayment = new object[15];
                            object[] SuplementryLedgerEntriesPayment = new object[15];


                            //for the insurance primary,secondry,teriatry insurance write off clump
                            object[] PrimaryLedgerEntriesWriteOff = new object[15];
                            object[] SecondryLedgerEntriesWriteOff = new object[15];
                            object[] TertiaryLedgerEntriesWriteOff = new object[15];
                            object[] SuplementryLedgerEntriesWriteOff = new object[15];


                            object[] LedgerEntriesPatPayment = new object[15];
                            object[] LedgerEntriesPatDiscount = new object[15];
                            object[] LedgerEntriesCopayPayment = new object[15];
                            object[] LedgerEntriesCopayDiscount = new object[15];

                            //for the clump of primary secondry,tertiarty entries
                            object[] PrimaryLedgerEntriesInstoInsForward = new object[15];
                            object[] SecondryLedgerEntriesInstoInsForward = new object[15];
                            object[] TertiaryLedgerEntriesInstoInsForward = new object[15];
                            object[] SuplementryLedgerEntriesInstoInsForward = new object[15];


                            object[] LedgerEntriesPatToInsForward = new object[15];
                            object[] LedgerEntriesPatForward = new object[15];

                            // dtstatement.Columns.Add(new DataColumn("PatBalance", typeof(string)));

                            //payments of different insurance
                            float PrimaryPaymentpaid = 0;
                            float PrimaryPaymentBalance = 0;

                            float SecondryPaymentpaid = 0;
                            float SecondryPaymentBalance = 0;

                            float TertiaryPaymentpaid = 0;
                            float TertiaryPaymentBalance = 0;

                            float SuplementryPaymentpaid = 0;
                            float SuplementryPaymentBalance = 0;


                            ///////////////////////

                            ////Insurance Write off /////////
                            float PrimaryWriteOffpaid = 0;
                            float PrimaryWriteOffPatBalance = 0;

                            float SecondryWriteOffpaid = 0;
                            float SecondryWriteOffPatBalance = 0;

                            float TertiaryWriteOffpaid = 0;
                            float TertiaryWriteOffPatBalance = 0;

                            float SuplementryWriteOffpaid = 0;
                            float SuplementryWriteOffPatBalance = 0;

                            //////////////////////////////
                            float Patientpaid = 0;
                            float PatientBalance = 0;

                            float PatientDiscountpaid = 0;
                            float PatientDiscountBalance = 0;

                            float Copaypaid = 0;
                            float CopaypaidBalance = 0;

                            float CopayDiscountpaid = 0;
                            float CopayDiscountBalance = 0;

                            //float InstoInsForwardPaid = 0;
                            //float InstoInsForwardBalance = 0;

                            //Insurance forward
                            float PrimaryInstoInsForwardPaid = 0;
                            float PrimaryInstoInsForwardBalance = 0;

                            float SecondryInstoInsForwardPaid = 0;
                            float SecondryInstoInsForwardBalance = 0;

                            float TertiaryInstoInsForwardPaid = 0;
                            float TertiaryInstoInsForwardBalance = 0;

                            float SuplementryInstoInsForwardPaid = 0;
                            float SuplementryInstoInsForwardBalance = 0;

                            /////////////////////////////////////





                            float InstoPatForwardPaid = 0;
                            float InstoPatForwardBalance = 0;


                            float PatForwardPaid = 0;
                            float PatForwardBalance = 0;



                            var count = 0;
                            foreach (DataRow data in filterData)
                            {
                                if (!String.IsNullOrEmpty(data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName].ToString()))
                                {
                                    charges = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName]);
                                    age = MDVUtility.ToInt(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    balance = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName]);

                                    ChargeEntry[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                    ChargeEntry[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                    ChargeEntry[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                    ChargeEntry[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                    ChargeEntry[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                    ChargeEntry[5] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]);
                                    ChargeEntry[6] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                    ChargeEntry[7] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                    ChargeEntry[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                    ChargeEntry[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    ChargeEntry[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                    ChargeEntry[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                    ChargeEntry[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                    ChargeEntry[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                    ChargeEntry[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);


                                    //if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Responsibility"))
                                    //{
                                    LedgerEntriesPatForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                    LedgerEntriesPatForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                    LedgerEntriesPatForward[2] = "";
                                    LedgerEntriesPatForward[3] = "Patient Responsibility";
                                    LedgerEntriesPatForward[4] = 0.00;
                                    LedgerEntriesPatForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                    LedgerEntriesPatForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                    LedgerEntriesPatForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    LedgerEntriesPatForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                    LedgerEntriesPatForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                    LedgerEntriesPatForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                    LedgerEntriesPatForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                    LedgerEntriesPatForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                    //patient responsibilty = Patient+ copay balance balance 
                                    PatForwardPaid = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    PatForwardBalance = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    // }
                                }
                                else
                                {
                                    //for single claim scenerio add insurance to insurance check in the condition.else for three claim scenerios it will not add in the if condition.
                                    if (MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Copay to Ins" && MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Ins to Pat" && MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Pat to Ins" && MDVUtility.ToStr(data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName]) != "Ins to Ins")
                                    {
                                        balance = balance - MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                    }
                                    //sum of same ledger entries
                                    if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Payment") && !data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Payment") && !data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Copay Payment"))
                                    {
                                        if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "1")
                                        {
                                            PrimaryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            PrimaryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            PrimaryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            PrimaryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            PrimaryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "2")
                                        {
                                            SecondryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SecondryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SecondryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SecondryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            SecondryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SecondryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "3")
                                        {
                                            TertiaryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            TertiaryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            TertiaryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            TertiaryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "4")
                                        {
                                            SuplementryLedgerEntriesPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SuplementryLedgerEntriesPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SuplementryLedgerEntriesPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            TertiaryPaymentpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryPaymentBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }

                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("WriteOff"))
                                    {
                                        if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "1")
                                        {
                                            PrimaryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            PrimaryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            PrimaryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            PrimaryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            PrimaryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "2")
                                        {
                                            SecondryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SecondryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SecondryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SecondryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            SecondryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SecondryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "3")
                                        {
                                            TertiaryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            TertiaryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            TertiaryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            TertiaryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        else if (data[dsPatientStatement.StatementDetail.PlanPriorityColumn.ColumnName].ToString() == "4")
                                        {
                                            SuplementryLedgerEntriesWriteOff[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SuplementryLedgerEntriesWriteOff[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SuplementryLedgerEntriesWriteOff[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesWriteOff[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);

                                            SuplementryWriteOffpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SuplementryWriteOffPatBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }

                                    }
                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Payment"))
                                    {
                                        LedgerEntriesPatPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesPatPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesPatPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesPatPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesPatPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesPatPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesPatPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesPatPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesPatPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesPatPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesPatPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesPatPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesPatPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        Patientpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        PatientBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Discount"))
                                    {
                                        LedgerEntriesPatDiscount[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesPatDiscount[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesPatDiscount[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesPatDiscount[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesPatDiscount[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesPatDiscount[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesPatDiscount[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesPatDiscount[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesPatDiscount[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        PatientDiscountpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        PatientDiscountBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Copay Payment"))
                                    {
                                        LedgerEntriesCopayPayment[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesCopayPayment[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesCopayPayment[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesCopayPayment[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesCopayPayment[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesCopayPayment[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesCopayPayment[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesCopayPayment[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesCopayPayment[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        Copaypaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        CopaypaidBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }

                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Copay Discount"))
                                    {
                                        LedgerEntriesCopayDiscount[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesCopayDiscount[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesCopayDiscount[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesCopayDiscount[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        CopayDiscountpaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        CopayDiscountBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    }
                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Forward to") && !data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName].ToString().Contains("Pat to Ins"))
                                    {
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "1")
                                        {
                                            PrimaryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            PrimaryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            PrimaryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            PrimaryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            PrimaryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            PrimaryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "2")
                                        {
                                            SecondryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SecondryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SecondryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SecondryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            SecondryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SecondryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "3")
                                        {
                                            TertiaryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            TertiaryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            TertiaryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            TertiaryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            TertiaryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            TertiaryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                        }
                                        if (data[dsPatientStatement.StatementDetail.NxtPlanPriorityColumn.ColumnName].ToString() == "4")
                                        {
                                            SuplementryLedgerEntriesInstoInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                            SuplementryLedgerEntriesInstoInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                            SuplementryLedgerEntriesInstoInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                            SuplementryLedgerEntriesInstoInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                            SuplementryInstoInsForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                            SuplementryInstoInsForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                        }
                                    }
                                    else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Forward to") && !data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName].ToString().Contains("Ins to Ins") && !data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Responsibility"))
                                    {
                                        LedgerEntriesPatToInsForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                        LedgerEntriesPatToInsForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                        LedgerEntriesPatToInsForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                        LedgerEntriesPatToInsForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                        InstoPatForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                        InstoPatForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);

                                    }
                                    //else if (data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName].ToString().Contains("Patient Responsibility"))
                                    //{
                                    //    LedgerEntriesPatForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                                    //    LedgerEntriesPatForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                                    //    LedgerEntriesPatForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                                    //    LedgerEntriesPatForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                                    //    LedgerEntriesPatForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                                    //    LedgerEntriesPatForward[5] = data[dsPatientStatement.StatementDetail.LedgerTypeColumn.ColumnName];

                                    //    LedgerEntriesPatForward[8] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[9] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.AgeColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[10] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[11] = data[dsPatientStatement.StatementDetail.ClaimNumberColumn.ColumnName];
                                    //    LedgerEntriesPatForward[12] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.UnitsColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[13] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeProviderIdColumn.ColumnName]);
                                    //    LedgerEntriesPatForward[14] = MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.ChargeFacilityIdColumn.ColumnName]);
                                    //    PatForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                                    //    PatForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                                    //}
                                }
                            }

                            if (ChargeEntry[0] != null)
                            {
                                lststatement.Add(ChargeEntry);
                            }
                            // primary insurance payment and write off and forward
                            if (PrimaryLedgerEntriesPayment[0] != null)
                            {
                                PrimaryLedgerEntriesPayment[6] = PrimaryPaymentpaid;
                                PrimaryLedgerEntriesPayment[7] = PrimaryPaymentBalance;
                                lststatement.Add(PrimaryLedgerEntriesPayment);
                            }

                            if (PrimaryLedgerEntriesWriteOff[0] != null)
                            {
                                PrimaryLedgerEntriesWriteOff[6] = PrimaryWriteOffpaid;
                                PrimaryLedgerEntriesWriteOff[7] = PrimaryWriteOffPatBalance;
                                lststatement.Add(PrimaryLedgerEntriesWriteOff);
                            }

                            if (PrimaryLedgerEntriesInstoInsForward[0] != null)
                            {
                                PrimaryLedgerEntriesInstoInsForward[6] = PrimaryInstoInsForwardPaid;
                                PrimaryLedgerEntriesInstoInsForward[7] = PrimaryInstoInsForwardBalance;
                                lststatement.Add(PrimaryLedgerEntriesInstoInsForward);
                            }

                            ////////////////////

                            // Secondry insurance payment and write off and forward

                            if (SecondryLedgerEntriesPayment[0] != null)
                            {
                                SecondryLedgerEntriesPayment[6] = SecondryPaymentpaid;
                                SecondryLedgerEntriesPayment[7] = SecondryPaymentBalance;
                                lststatement.Add(SecondryLedgerEntriesPayment);

                            }

                            if (SecondryLedgerEntriesWriteOff[0] != null)
                            {
                                SecondryLedgerEntriesWriteOff[6] = SecondryWriteOffpaid;
                                SecondryLedgerEntriesWriteOff[7] = SecondryWriteOffPatBalance;
                                lststatement.Add(SecondryLedgerEntriesWriteOff);

                            }

                            if (SecondryLedgerEntriesInstoInsForward[0] != null)
                            {
                                SecondryLedgerEntriesInstoInsForward[6] = SecondryInstoInsForwardPaid;
                                SecondryLedgerEntriesInstoInsForward[7] = SecondryInstoInsForwardBalance;
                                lststatement.Add(SecondryLedgerEntriesInstoInsForward);
                            }


                            /////////////////////////////////////////

                            // Teriary insurance payment and write off and forward

                            if (TertiaryLedgerEntriesPayment[0] != null)
                            {
                                TertiaryLedgerEntriesPayment[6] = TertiaryPaymentpaid;
                                TertiaryLedgerEntriesPayment[7] = TertiaryPaymentBalance;
                                lststatement.Add(TertiaryLedgerEntriesPayment);
                            }

                            if (TertiaryLedgerEntriesWriteOff[0] != null)
                            {
                                TertiaryLedgerEntriesWriteOff[6] = TertiaryWriteOffpaid;
                                TertiaryLedgerEntriesWriteOff[7] = TertiaryWriteOffPatBalance;
                                lststatement.Add(TertiaryLedgerEntriesWriteOff);
                            }


                            if (TertiaryLedgerEntriesInstoInsForward[0] != null)
                            {
                                TertiaryLedgerEntriesInstoInsForward[6] = TertiaryInstoInsForwardPaid;
                                TertiaryLedgerEntriesInstoInsForward[7] = TertiaryInstoInsForwardBalance;
                                lststatement.Add(TertiaryLedgerEntriesInstoInsForward);
                            }


                            /////////////////////////////////

                            // Suplementry insurance payment and write off and forward
                            if (SuplementryLedgerEntriesPayment[0] != null)
                            {
                                SuplementryLedgerEntriesPayment[6] = SuplementryPaymentpaid;
                                SuplementryLedgerEntriesPayment[7] = SuplementryPaymentBalance;
                                lststatement.Add(SuplementryLedgerEntriesPayment);
                            }


                            if (SuplementryLedgerEntriesWriteOff[0] != null)
                            {
                                SuplementryLedgerEntriesWriteOff[6] = SuplementryWriteOffpaid;
                                SuplementryLedgerEntriesWriteOff[7] = SuplementryWriteOffPatBalance;
                                lststatement.Add(SuplementryLedgerEntriesWriteOff);
                            }

                            if (SuplementryLedgerEntriesInstoInsForward[0] != null)
                            {
                                SuplementryLedgerEntriesInstoInsForward[6] = SuplementryInstoInsForwardPaid;
                                SuplementryLedgerEntriesInstoInsForward[7] = SuplementryInstoInsForwardBalance;
                                lststatement.Add(SuplementryLedgerEntriesInstoInsForward);
                            }

                            ///////////////////////////////////


                            if (LedgerEntriesPatPayment[0] != null)
                            {
                                LedgerEntriesPatPayment[6] = Patientpaid;
                                LedgerEntriesPatPayment[7] = PatientBalance;
                                lststatement.Add(LedgerEntriesPatPayment);
                            }
                            if (LedgerEntriesPatDiscount[0] != null)
                            {
                                LedgerEntriesPatDiscount[6] = PatientDiscountpaid;
                                LedgerEntriesPatDiscount[7] = PatientDiscountBalance;
                                lststatement.Add(LedgerEntriesPatDiscount);
                            }

                            if (LedgerEntriesCopayPayment[0] != null)
                            {
                                LedgerEntriesCopayPayment[6] = Copaypaid;
                                LedgerEntriesCopayPayment[7] = CopaypaidBalance;
                                lststatement.Add(LedgerEntriesCopayPayment);
                            }
                            if (LedgerEntriesCopayDiscount[0] != null)
                            {
                                LedgerEntriesCopayDiscount[6] = CopayDiscountpaid;
                                LedgerEntriesCopayDiscount[7] = CopayDiscountBalance;
                                lststatement.Add(LedgerEntriesCopayDiscount);
                            }


                            //////////////////////////////////

                            if (LedgerEntriesPatToInsForward[0] != null)
                            {
                                LedgerEntriesPatToInsForward[6] = InstoPatForwardPaid;
                                LedgerEntriesPatToInsForward[7] = InstoPatForwardBalance;
                                lststatement.Add(LedgerEntriesPatToInsForward);
                            }

                            if (LedgerEntriesPatForward[0] != null)
                            {
                                LedgerEntriesPatForward[6] = PatForwardPaid;
                                LedgerEntriesPatForward[7] = PatForwardBalance;
                                lststatement.Add(LedgerEntriesPatForward);
                            }


                            //LedgerEntriesPayment[count] = paid;
                            //  LedgerEntriesPayment[count] = PatBalance;
                            count++;

                            if (age >= 0 && age <= 30)
                                age0_31 += balance;
                            else if (age >= 31 && age <= 60)
                                age31_60 += balance;
                            else if (age >= 61 && age <= 90)
                                age61_90 += balance;
                            else if (age >= 91 && age <= 120)
                                age91_120 += balance;
                            else if (age >= 121)
                                age120_onward += balance;
                            totalBalance += balance;


                        }

                        if (age0_31 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.FirstMessageColumn.ColumnName]);
                        if (age31_60 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.SecondMessageColumn.ColumnName]);
                        if (age61_90 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.ThirdMessageColumn.ColumnName]);
                        if (age91_120 > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.FourthMessageColumn.ColumnName]);
                        if (age120_onward > 0)
                            StatementMessage = MDVUtility.ToStr(dsPatientStatement.PatientStatementPrint.Rows[0][dsPatientStatement.PatientStatementPrint.FifthMessageColumn.ColumnName]);

                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age0_30Column.ColumnName] = age0_31;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age31_60Column.ColumnName] = age31_60;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age61_90Column.ColumnName] = age61_90;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age91_120Column.ColumnName] = age91_120;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter._Age121_OnwardColumn.ColumnName] = age120_onward;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter.PatBalanceColumn.ColumnName] = totalBalance;
                        dsPatientStatement.StatementFooter.Rows[0][dsPatientStatement.StatementFooter.StatementMessageColumn.ColumnName] = StatementMessage;




                        dic.Add(dsPatientStatement.StatementHeader.TableName, MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementHeader.TableName], "", false));
                        dic.Add(dsPatientStatement.StatementFooter.TableName, MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementFooter.TableName], "", false));

                        DataTable dtStatement = new DataTable("StatementDetail");
                        dtStatement.Columns.Add(new DataColumn("Charges", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                        dtStatement.Columns.Add(new DataColumn("FullName", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Procedure", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Description", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("LedgerType", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Paid", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("PatBalance", typeof(string)));

                        dtStatement.Columns.Add(new DataColumn("ChargeCapId", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Age", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("VisitId", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("ClaimNumber", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("Units", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("ChargeProviderId", typeof(string)));
                        dtStatement.Columns.Add(new DataColumn("ChargeFacilityId", typeof(string)));

                        //for statement detail table
                        //for (int i = 0; i < lststatement.Count; i++)
                        //{
                        //    DataRow drStatement = dtStatement.NewRow();
                        //    drStatement["Charges"] = 0.0;
                        //    drStatement["Date"] = lststatement[i];
                        //    drStatement["FullName"] = lststatement[i];
                        //    drStatement["Procedure"] = lststatement[i];
                        //    drStatement["Description"] = lststatement[i];
                        //    drStatement["Paid"] = lststatement[i];
                        //}

                        //LedgerEntriesPatForward[0] = data[dsPatientStatement.StatementDetail.DateColumn.ColumnName];
                        //        LedgerEntriesPatForward[1] = data[dsPatientStatement.StatementDetail.FullNameColumn.ColumnName];
                        //        LedgerEntriesPatForward[2] = data[dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName];
                        //        LedgerEntriesPatForward[3] = data[dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName];
                        //        LedgerEntriesPatForward[4] = data[dsPatientStatement.StatementDetail.ChargesColumn.ColumnName];
                        //        PatForwardPaid += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);
                        //        PatForwardBalance += MDVUtility.Tofloat(data[dsPatientStatement.StatementDetail.PatBalanceColumn.ColumnName]);
                        //    }
                        foreach (object[] itemArray in lststatement)
                        {
                            DataRow drStatement = dtStatement.NewRow();
                            drStatement["Date"] = itemArray[0];
                            drStatement["FullName"] = itemArray[1];
                            drStatement["Procedure"] = itemArray[2];
                            drStatement["Description"] = itemArray[3];
                            drStatement["Charges"] = itemArray[4];
                            drStatement["LedgerType"] = itemArray[5];
                            drStatement["Paid"] = itemArray[6];
                            drStatement["PatBalance"] = itemArray[7];

                            drStatement["ChargeCapId"] = itemArray[8];
                            drStatement["Age"] = itemArray[9];
                            drStatement["VisitId"] = itemArray[10];
                            drStatement["ClaimNumber"] = itemArray[11];
                            drStatement["Units"] = itemArray[12];
                            drStatement["ChargeProviderId"] = itemArray[13];
                            drStatement["ChargeFacilityId"] = itemArray[14];

                            dtStatement.Rows.Add(drStatement);
                        }

                        //  dic.Add(dsPatientStatement.StatementDetail.TableName, MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementDetail.TableName], "", false));
                        dic.Add(dsPatientStatement.StatementDetail.TableName, MDVUtility.JSON_DataTable(dtStatement, "", false));

                        //dic[dsPatientStatement.StatementHeader.TableName] = dic[dsPatientStatement.StatementHeader.TableName].Replace("&#39;","'");
                        //dic[dsPatientStatement.StatementFooter.TableName] = dic[dsPatientStatement.StatementFooter.TableName].Replace("&#39;", "'");
                        //dic[dsPatientStatement.StatementDetail.TableName] = dic[dsPatientStatement.StatementDetail.TableName].Replace("&#39;", "'");

                        //dic[dsPatientStatement.StatementHeader.TableName] = dic[dsPatientStatement.StatementHeader.TableName].Replace("&#233;", "é");
                        //dic[dsPatientStatement.StatementFooter.TableName] = dic[dsPatientStatement.StatementFooter.TableName].Replace("&#233;", "é");
                        //dic[dsPatientStatement.StatementDetail.TableName] = dic[dsPatientStatement.StatementDetail.TableName].Replace("&#233;", "é");
                        lstStatement.Add(dic);
                    }



                }
                return lstStatement;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::MakePatientStatement", ex);
                return lstStatement;
            }
        }


        public BLObject<bool> UploadPatientStatement(long ClearingHouseId, string PatientStatementXML)
        {
            try
            {
                DSEDI dsEDI = new DSEDI();
                dsEDI = new DALClearingHouse().LoadClearingHouse(ClearingHouseId, null, null, null, null);

                string ftp = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn]);
                string userName = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn]);
                string userPassword = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn]);
                Int32 ftpPortNo = MDVUtility.ToInt32(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn]);
                string ftpHostKey = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn]);
                string FTPUploadFolder = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.IN_STATEMENTSColumn]);

                return new BLObject<bool>(true);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UploadPatientStatement", ex);
                return new BLObject<bool>(false, ex.Message);
            }
        }


        #region Submitted Statements Batch

        public BLObject<DSPatientStatement> InsertPatientStatementsBatch(SharedVariable SharedVariable, DSPatientStatement ds)
        {
            try
            {
                //SharedVariable SharedVariable = new SharedVariable();
                //SharedVariable.EntityId = MDVSession.Current.EntityId;
                //SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                //SharedVariable.UserName = MDVSession.Current.AppUserName;
                //SharedVariable.ClientId = MDVSession.Current.ClientId;
                //SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                //SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;
                ds = new DALPatientStatement(SharedVariable).InsertPatientStatementsBatch(ds);
                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::InsertPatientStatementsBatch", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> SearchPatientStatementsBatch(long BatchId, string BatchNumber, DateTime? SubmittedDate, string SubmittedBy, long? SubmittedById, string SubmitType, int ClearingHouseId, string BatchStatus, int PageNumber, int RowspPage)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().SearchPatientStatementsBatch(BatchId, BatchNumber, SubmittedDate, SubmittedBy, SubmittedById, SubmitType, ClearingHouseId, BatchStatus, PageNumber, RowspPage);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SearchPatientStatementsBatch", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> SearchPatientStatementsBatchDetail(long BatchId)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatement().SearchPatientStatementsBatchDetail(BatchId);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SearchPatientStatementsBatchDetail", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }


        public BLObject<string> ResubmitPatientStatementsBatch(SharedVariable sharedVariable, int BatchId)
        {
            try
            {
                return new BLObject<string>(new DALPatientStatement(sharedVariable).ResubmitPatientStatementsBatch(sharedVariable, BatchId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::ResubmitPatientStatementsBatch", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }



        #endregion

        #region Admin Statement Message
        public BLObject<DSPatientStatement> InsertStatementMessage(DSPatientStatement ds)
        {
            try
            {
                ds = new DALPatientStatementMessage().InsertStatementMessage(ds);

                return new BLObject<DSPatientStatement>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertPatientStatement", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }

        }

        public BLObject<DSPatientStatement> LoadStatementMessage(long statementMessageId, string shortName, string message, string isActive, int pageNumber, int RecordPerPage)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatementMessage().LoadStatementMessage(statementMessageId, shortName, message, isActive, pageNumber, RecordPerPage);

                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPayment::LoadAdvancePayments", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> UpdateStatementMessage(DSPatientStatement ds)
        {
            try
            {
                ds = new DALPatientStatementMessage().UpdateStatementMessage(ds);
                return new BLObject<DSPatientStatement>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateStatementMessage", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteStatementMessage(long StatementMessageId)
        {
            try
            {
                string StatementMessage = new DALPatientStatementMessage().DeleteStatementMessage(StatementMessageId);
                return new BLObject<string>(StatementMessage);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteStatementMessage", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #region Statement Message Lookup
        public BLObject<DSPatientStatementLookup> LookupStatementMessage()
        {
            try
            {
                DSPatientStatementLookup ds = new DSPatientStatementLookup();
                ds = new DALPatientStatementMessage().LookupStatementMessage();

                return new BLObject<DSPatientStatementLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupStatementMessage", ex);
                return new BLObject<DSPatientStatementLookup>(null, ex.Message);
            }

        }
        #endregion

        #endregion


        #region Admin Statement Group
        public BLObject<DSPatientStatement> InsertStatementGroup(DSPatientStatement ds)
        {
            try
            {
                ds = new DALPatientStatementGroup().InsertStatementGroup(ds);

                return new BLObject<DSPatientStatement>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertPatientStatementGroup", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }

        }

        public BLObject<DSPatientStatement> LoadStatementGroup(long statementGroupId, string name, Int32 cycleDays, string isActive, int pageNumber, int RecordPerPage)
        {
            try
            {
                DSPatientStatement ds = new DSPatientStatement();

                ds = new DALPatientStatementGroup().LoadStatementGroup(statementGroupId, name, cycleDays, isActive, pageNumber, RecordPerPage);


                return new BLObject<DSPatientStatement>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadStatementGroup", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }
        }

        public BLObject<DSPatientStatement> UpdateStatementGroup(DSPatientStatement ds)
        {
            try
            {
                ds = new DALPatientStatementGroup().UpdateStatementGroup(ds);
                return new BLObject<DSPatientStatement>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateStatementGroup", ex);
                return new BLObject<DSPatientStatement>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteStatementGroup(long StatementGroupId)
        {
            try
            {
                string StatementGroup = new DALPatientStatementGroup().DeleteStatementGroup(StatementGroupId);
                return new BLObject<string>(StatementGroup);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteStatementGroup", ex);
                return new BLObject<string>("", ex.Message);
            }
        }


        #region Statement Group Lookup
        public BLObject<DSPatientStatementLookup> LookupStatementGroup()
        {
            try
            {
                DSPatientStatementLookup ds = new DSPatientStatementLookup();
                ds = new DALPatientStatementGroup().LookupStatementGroup();

                return new BLObject<DSPatientStatementLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupStatementGroup", ex);
                return new BLObject<DSPatientStatementLookup>(null, ex.Message);
            }

        }
        #endregion

        #endregion

        #endregion



        #region PATIENT LEDGER

        public BLObject<DSPatientLedger> LoadPatientLedger(long patientId, long FacilityId, long ProviderId, DateTime? DOSFrom = null, DateTime? DOSTo = null, long InsurancePlanId = 0, long BilledToId = 0, long ClaimBalId = 0,bool isCollection =false,bool isOtherClaims = false,bool IsVoidedClaims =  false, bool isShowDetails =  false )
        {
            try
            {
                DSPatientLedger ds = new DSPatientLedger();
                ds = new DALPatientLedger().LoadPatientLedger(patientId, FacilityId, ProviderId, DOSFrom, DOSTo, InsurancePlanId, BilledToId, ClaimBalId, isCollection, isOtherClaims, IsVoidedClaims, isShowDetails);


                if (patientId > 0)
                {
                    ds.Merge(new DALPatientLedger().LoadPatientOutstandingBalance(patientId));
                }

                return new BLObject<DSPatientLedger>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPatientLedger", ex);
                return new BLObject<DSPatientLedger>(null, ex.Message);
            }

        }
        public BLObject<DSPatientLedger> SearchPatientPayments(long patientId, int pageNo,int  rowsPerPage)
        {
            try
            {
                DSPatientLedger ds = new DSPatientLedger();
                ds = new DALPatientLedger().SearchPatientPayments(patientId, pageNo, rowsPerPage);
                return new BLObject<DSPatientLedger>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SearchPatientPayments", ex);
                return new BLObject<DSPatientLedger>(null, ex.Message);
            }

        }

        public BLObject<DSPatientLedger> SearchReceivedPayments( long pmtId)
        {
            try
            {
                DSPatientLedger ds = new DSPatientLedger();
                ds = new DALPatientLedger().SearchReceivedPayments(pmtId);
                return new BLObject<DSPatientLedger>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SearchPatientPayments", ex);
                return new BLObject<DSPatientLedger>(null, ex.Message);
            }

        }

        public BLObject<DSPatientLedger> LoadPrintPractice(long PracticeId)
        {
            try
            {
                DSPatientLedger ds = new DSPatientLedger();
                ds = new DALPatientLedger().LoadPrintPractice(PracticeId);
                return new BLObject<DSPatientLedger>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadPrintPractice", ex);
                return new BLObject<DSPatientLedger>(null, ex.Message);
            }

        }


        public BLObject<DSPayment> UpdateLegder(DSPayment dsPayment)
        {
            try
            {

                DSPayment ds = new DALPayment().UpdatePayment(dsPayment);

                return new BLObject<DSPayment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateLegder", ex);
                return new BLObject<DSPayment>(null, ex.Message);
            }

        }

        #endregion

        #region BatcheNumbers
        public BLObject<DSCharge> LookupBatches(string Active)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LookupBatches(Active);
                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupBatches", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }
        #endregion

        #region Dashboar Payments
        public BLObject<List<DPaymentModel>> LoadDashboardPayments(long Entity, long PageNumber = 1, long RowsPerPage = 15)
        {
            try
            {

                var result = new DALPayment().LoadDashboardPayments(Entity, PageNumber, RowsPerPage);
                return new BLObject<List<DPaymentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadDashboardPayments", ex);
                return new BLObject<List<DPaymentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<DTCMModel>> LoadDashboardTCMPatients(long patientId, long providerId, string Status, long PageNumber = 1, long RowsPerPage = 15)
        {
            try
            {

                var result = new DALPayment().LoadDashboardTCMPatients(patientId, providerId, Status, PageNumber, RowsPerPage);
                return new BLObject<List<DTCMModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadDashboardTCMPatients", ex);
                return new BLObject<List<DTCMModel>>(null, ex.Message);
            }
        }
        #endregion

        #region Dashboard Copayment
        public BLObject<List<DCopaymentModel>> LoadDashboardCopay(long Entity, DateTime? PaidForm, DateTime? PaidTo, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                var result = new DALPayment().LoadDashboardCopay(Entity, PaidForm, PaidTo, PageNumber, RowspPage);
                return new BLObject<List<DCopaymentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadDashBoardCopay", ex);
                return new BLObject<List<DCopaymentModel>>(null, ex.Message);
            }
        }

        #endregion
        #region Claim summary
        public BLObject<DSClaimSummary> LoadClaimSummary(Int64 VisitId)
        {
            try
            {
                var result = new DALPayment().LoadClaimSummary(VisitId);
                return new BLObject<DSClaimSummary>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadClaimSummary", ex);
                return new BLObject<DSClaimSummary>(null, ex.Message);
            }
        }

        #endregion

        #region "Note Comments Log"
        public BLObject<DSDBAudit> LoadDBAudit_NoteComments(string PatientId, string VisitId, string DBTableName = null)
        {
            try
            {
                DSDBAudit ds = new DSDBAudit();
                ds = new DBActivityAudit().LoadDBAudit_NoteComments(PatientId, VisitId, DBTableName);
                return new BLObject<DSDBAudit>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadDBAudit_NoteComments", ex);
                return new BLObject<DSDBAudit>(null, ex.Message);
            }
        }
        #endregion

        #region "Unallocated Copay"
        public UnAllocatedCopayModel SaveUnAllocatedCopay(UnAllocatedCopayModel model)
        {
            try
            {
                return new DALPayment().SaveUnAllocatedCopay(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SaveUnAllocatedCopay", ex);
                throw ex;
            }
        }
        public string RefundUnAllocatedCopay(long UnAllocatedCopayId)
        {
            try
            {
                return new DALPayment().RefundUnAllocatedCopay(UnAllocatedCopayId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteUnAllocatedCopay", ex);
                throw ex;
            }
        }
        public BLObject<List<UnAllocatedCopayModel>> LoadUnAllocatedCopay(long UnAllocatedCopayId, Int64 PatientId, Int64 ProviderId, Int64 FacilityId, Int64 AppointmentID, String ReceiptNumber, DateTime? ReceiptDateFrom, DateTime? ReceiptDateTo, Int32 PageNumber = 1, Int32 RowsPerPage = 15, bool IsDeleted = true)
        {
            try
            {
                List<UnAllocatedCopayModel> UnAllocatedCopayList = new List<UnAllocatedCopayModel>();
                UnAllocatedCopayList = new DALPayment().LoadUnAllocatedCopay(UnAllocatedCopayId, PatientId, ProviderId, FacilityId, AppointmentID, ReceiptNumber, ReceiptDateFrom, ReceiptDateTo, PageNumber, RowsPerPage, IsDeleted);

                return new BLObject<List<UnAllocatedCopayModel>>(UnAllocatedCopayList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadUnAllocatedCopay", ex);
                return new BLObject<List<UnAllocatedCopayModel>>(null, ex.Message);
            }
        }

        public BLObject<List<PaymentReceiptInfoModel>> LoadPatientPaymentReceiptInfo(string PaymentId)
        {
            try
            {
                List<PaymentReceiptInfoModel> PaymentReceiptInfoList = new List<PaymentReceiptInfoModel>();
                PaymentReceiptInfoList = new DALPayment().LoadPatientPaymentReceiptInfo(PaymentId);

                return new BLObject<List<PaymentReceiptInfoModel>>(PaymentReceiptInfoList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::PaymentReceiptInfoModel", ex);
                return new BLObject<List<PaymentReceiptInfoModel>>(null, ex.Message);
            }
        }


        public BLObject<bool> PostUnAllocatedCopay(UnAllocatedCopayModel model, DSPayment dsPayment)
        {
            DSPayment ds = new DSPayment();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                // Transaction Begin
                dbManager.BeginTransaction();
                ds = new DALPayment().InsertPatientPayments(dsPayment, dbManager);
                if (ds.PatientPayments.Rows.Count > 0)
                {
                    model.Status = "Allocated";
                    model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.ModifiedOn = DateTime.Now.ToString();
                    string UnAllocatedCopayId = string.Empty;
                    UnAllocatedCopayId = new DALPayment().UpdateUnAllocatedCopay(model, dbManager);
                    // Transaction Commit
                    dbManager.CommitTransaction();
                    return new BLObject<bool>(true);

                }
                else
                    throw new Exception("Error occurred while UnAllocated Copay posting.");

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::PostUnAllocatedCopay", ex);
                // Transaction Roll Back
                dbManager.RollBackTransaction();
                return new BLObject<bool>(false, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
    }
}

