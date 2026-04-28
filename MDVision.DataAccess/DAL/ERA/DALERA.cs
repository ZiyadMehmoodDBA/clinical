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

namespace MDVision.DataAccess.DAL.ERA
{
    public class DALERA
    {
         

        #region "Stored Procedure Names"
        private const string PROC_ERA_INSERT = "Billing.sp_ERAInsert";
        private const string PROC_ERA_UPDATE = "Billing.sp_ERAUpdate";
        private const string PROC_ERA_DELETE = "Billing.sp_ERADelete";
        private const string PROC_ERA_SELECT = "Billing.sp_ERASelect";
        private const string PROC_ERA_SEARCH = "Billing.sp_ERASearch";
        private const string PROC_ERA_LINK_CHARGES = "Billing.sp_LinkERACharges";
        private const string PROC_ERA_STATUS_LOOKUP = "Billing.sp_ERAStatusLookup";
        private const string PROC_ERA_PAYEE_LOOKUP = "Billing.sp_ERAPayeeLookup";
        private const string PROC_ERA_LINK_CHARGE = "Billing.sp_LinkERACharge";
        private const string PROC_ERA_VOIDED_CLAIMS = "Billing.sp_ERAVoidedClaims";

        private const string PROC_ERA_DETAIL_INSERT = "Billing.sp_ERADetailInsert";
        private const string PROC_ERA_DETAIL_UPDATE = "Billing.sp_ERADetailUpdate";
        private const string PROC_ERA_DETAIL_DELETE = "Billing.sp_ERADetailDelete";
        private const string PROC_ERA_DETAIL_SELECT = "Billing.sp_ERADetailSelect";
        private const string PROC_ERA_LINKED_HISTORY = "Billing.sp_ERALinkedHistory";


        private const string PROC_ERA_CLAIM_ADJUSTMENT_CODE_SELECT = "Billing.sp_ERAClaimAdjustmentCodeSelect";
        private const string PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT = "Billing.sp_ERAClaimAdjustmentCodeInsert";
        private const string PROC_ERA_CLAIM_ADJUSTMENT_CODE_UPDATE = "Billing.sp_ERAClaimAdjustmentCodeUpdate";
        private const string PROC_ERA_CLAIM_ADJUSTMENT_CODE_DELETE = "Billing.sp_ERAClaimAdjustmentCodeDelete";

        private const string PROC_ERA_PROVIDER_ADJUSTMENT_SELECT = "Billing.sp_ERAProviderAdjustmentSelect";
        private const string PROC_ERA_PROVIDER_ADJUSTMENT_INSERT = "Billing.sp_ERAProviderAdjustmentInsert";
        private const string PROC_ERA_PROVIDER_ADJUSTMENT_UPDATE = "Billing.sp_ERAProviderAdjustmentUpdate";
        private const string PROC_ERA_PROVIDER_ADJUSTMENT_DELETE = "Billing.sp_ERAProviderAdjustmentDelete";


        private const string PROC_BILLING_ERA_ELECTRONIC_EOB = "Billing.sp_ElectronicEOB";
        private const string PROC_BILLING_ERA_LOGGER_INSERT = "System.sp_ERAActivityStepsInsert";

        #endregion

        #region "Parameters"
        private const string PARM_ERA_ID = "@ERAId";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_EDI_REPORT_ID = "@EDIReportId";
        private const string PARM_EDI_STATUS_ID = "@ERAStatusId";
        private const string PARM_CHECK_NO = "@CheckNo";
        private const string PARM_CHECK_AMOUNT = "@CheckAmount";
        private const string PARM_CHECK_DATE = "@CheckDate";
        private const string PARM_CHECK_DEPOSIT_DATE = "@CheckDepositDate";
        private const string PARM_PRAYER_NAME = "@PayerName";
        private const string PARM_PRAYER_ADDRESS = "@PayerAddress";
        private const string PARM_PRAYER_CITY = "@PayerCity";
        private const string PARM_PRAYER_STATE = "@PayerState";
        private const string PARM_PRAYER_ZIP_CODE = "@PayerZipCode";
        private const string PARM_PAYEE_NAME = "@PayeeName";
        private const string PARM_PAYEE_ID = "@ERAPayeeId";
        private const string PARM_PAYEE_ADDRESS = "@PayeeAddress";
        private const string PARM_PAYEE_CITY = "@PayeeCity";
        private const string PARM_PAYEE_STATE = "@PayeeState";
        private const string PARM_PAYEE_ZIP_CODE = "@PayeeZipCode";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_CLAIM_PAYER_ID = "@ClaimPayerId";
        private const string PARM_FROM_DATE = "@FromEntryDate";
        private const string PARM_TO_DATE = "@ToEntryDate";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_MODIFIERS = "@Modifiers";
        private const string PARM_ERA_CHARGE_ID = "@ERAChargeId";
        private const string PARM_ERA_CLAIM_NO = "@ERAClaimNo";
        private const string PARM_ERA_PAYMENT_INSURANCE_ID = "@PaymentInsureanceId";
        private const string PARM_REMIT_CODE = "@RemitCode";
        private const string PARM_SUBSCRIBER_FIRST_NAME = "@SubscriberFirstName";
        private const string PARM_SUBSCRIBER_LAST_NAME = "@SubscriberLastName";
        private const string PARM_NEXT_RESPONSIBILITY_ID = "@NextResponsibilityId";



        private const string PARM_ERA_DETAIL_ID = "@ERADtlId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_PAT_FIRST_NAME = "@PatFirstName";
        private const string PARM_PAT_LAST_NAME = "@PatLastName";
        private const string PARM_PAT_DOB = "@PatDOB";
        private const string PARM_ERA_CLAIM_NUMBER = "@ERAClaimNumber";
        private const string PARM_CHARGE_ID = "@ChargeId";
        private const string PARM_ERA_CHARGE_NO = "@ERAChargeNumber";
        private const string PARM_SUBSCRIBER_ID = "@SubscriberId";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_IS_LINK_CHARGE = "@IsLinkCharge";

        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_MODIFIER_CODE = "@ModifierCode";
        private const string PARM_UNITS_BILLED = "@UnitsBilled";
        private const string PARM_POS = "@POS";
        private const string PARM_PROCESS_AS = "@ProcessAs";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_ICN = "@ICN";
        private const string PARM_SEC_INS = "@SecondaryInsurance";
        private const string PARM_SEC_SUBSC_ID = "@SecondarySubscriberId";
        private const string PARM_CROSSED_OVER = "@IsCrossedOver";
        private const string PARM_VISIT_DATE = "@VisitDate";
        private const string PARM_POST_STATUS = "@PostStatus";



        private const string PARM_CHARGE_AMT = "@ChargedAmount";
        private const string PARM_ALLOWED_AMT = "@AllowedAmount";
        private const string PARM_PAID_AMOUNT = "@PaidAmount";
        private const string PARM_CO_INS_AMT = "@CoInsuranceAmount";
        private const string PARM_DEDUCTABLE_AMT = "@DeductableAmount";
        private const string PARM_COPAYMENT = "@Copayment";
        private const string PARM_LATE_FILLING_CHRG = "@LateFilingCharges";
        private const string PARM_INTEREST_AMT = "@InterestAmount";
        private const string PARM_WRITE_OFF = "@WriteOff";
        private const string PARM_PATIENT_RESPONSIBILITY = "@PatientResponsibility";
        private const string PARM_NEXT_RESPONSIBILITY = "@NextResponsibility";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_MI = "@MI";
        private const string PARM_ERA_CLAIM_ADJUSTMENT_ID = "@ERAClaimAdjId";
        private const string PARM_ERA_CLAIM_ADJUSTMENT_GROUP_CODE = "@AdjGroupCode";
        private const string PARM_ERA_CLAIM_ADJUSTMENT_GROUP_DESCRIPTION = "@AdjGroupDescription";
        private const string PARM_ERA_CLAIM_ADJUSTMENT_REASON_CODE = "@AdjReasonCode";
        private const string PARM_ERA_CLAIM_ADJUSTMENT_REASON_DESCRIPTION = "@AdjReasonDescription";
        private const string PARM_ERA_CLAIM_ADJUSTMENT_AMOUNT = "@Amount";

        private const string PARM_ERA_PROVIDER_ADJUSTMENT_ID = "@ERAProviderAdjId";
        private const string PARM_ERA_REASON_CODE = "@ReasonCode";
        private const string PARM_ERA_IDENTIFIER = "@Identifier";
        private const string PARM_ERA_DESCRIPTION = "@Description";


        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_USER_ID = "@Userid";


        private const string PARM_LINKED_By = "@LinkedBy";
        private const string PARM_LINKED_DATE = "@LinkedDate";

        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_MODULE = "@module";
        private const string PARM_ERA_LOGGER_XML = "@XMLData";

        #endregion

        #region Constructors
        public DALERA()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
            
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALERA(SharedVariable SharedVariable)
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

        #region "Support Functions ERA"
        private void CreateERAParameters(IDBManager dbManager, DSERA ds, Boolean IsInsert)
        {
            //dbManager.CreateParameters(ds.Tables[ds.Era.TableName].Columns.Count);
            dbManager.CreateParameters(27);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERA_ID, ds.ERA.ERAIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERA_ID, ds.ERA.ERAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, ds.ERA.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.ERA.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.ERA.ProviderIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(4, PARM_EDI_REPORT_ID, ds.ERA.EDIReportIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_EDI_STATUS_ID, ds.ERA.ERAStatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_CHECK_NO, ds.ERA.CheckNoColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_CHECK_AMOUNT, ds.ERA.CheckAmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(8, PARM_CHECK_DATE, ds.ERA.CheckDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_CHECK_DEPOSIT_DATE, ds.ERA.CheckDepositDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(10, PARM_PRAYER_NAME, ds.ERA.PayerNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_PRAYER_ADDRESS, ds.ERA.PayerAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PRAYER_CITY, ds.ERA.PayerCityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(13, PARM_PRAYER_STATE, ds.ERA.PayerStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PRAYER_ZIP_CODE, ds.ERA.PayerZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_PAYEE_NAME, ds.ERA.PayeeNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(16, PARM_PAYEE_ADDRESS, ds.ERA.PayeeAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_PAYEE_CITY, ds.ERA.PayeeCityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(18, PARM_PAYEE_STATE, ds.ERA.PayeeStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_PAYEE_ZIP_CODE, ds.ERA.PayeeZipCodeColumn.ColumnName, DbType.String);

            dbManager.AddParameters(20, PARM_IS_ACTIVE, ds.ERA.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(21, PARM_CREATED_BY, ds.ERA.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_ON, ds.ERA.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_MODIFIED_BY, ds.ERA.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_MODIFIED_ON, ds.ERA.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_STATUS, ds.ERA.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_CLAIM_PAYER_ID, ds.ERA.ClaimPayerIdColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "Support Functions ERA Detail"
        private void CreateERADetailParameters(IDBManager dbManager, DSERA ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(45);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ds.ERADetail.ERADtlIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ds.ERADetail.ERADtlIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ERA_ID, ds.ERADetail.ERAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CLAIM_NUMBER, ds.ERADetail.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PAT_FIRST_NAME, ds.ERADetail.PatFirstNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(4, PARM_PAT_LAST_NAME, ds.ERADetail.PatLastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_PAT_DOB, ds.ERADetail.PatDOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_ERA_CLAIM_NUMBER, ds.ERADetail.ERAClaimNumberColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_CHARGE_ID, ds.ERADetail.ChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_ERA_CHARGE_NO, ds.ERADetail.ERAChargeNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_SUBSCRIBER_ID, ds.ERADetail.SubscriberIdColumn.ColumnName, DbType.String);

            dbManager.AddParameters(10, PARM_DOS_FROM, ds.ERADetail.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_DOS_TO, ds.ERADetail.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_CPT_CODE, ds.ERADetail.CPTCodeColumn.ColumnName, DbType.String);

            dbManager.AddParameters(13, PARM_MODIFIER_CODE, ds.ERADetail.ModifierCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_UNITS_BILLED, ds.ERADetail.UnitsBilledColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(15, PARM_POS, ds.ERADetail.POSColumn.ColumnName, DbType.String);

            dbManager.AddParameters(16, PARM_PROCESS_AS, ds.ERADetail.ProcessAsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ICN, ds.ERADetail.ICNColumn.ColumnName, DbType.String);

            dbManager.AddParameters(18, PARM_SEC_INS, ds.ERADetail.SecondaryInsuranceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_SEC_SUBSC_ID, ds.ERADetail.SecondarySubscriberIdColumn.ColumnName, DbType.String);

            dbManager.AddParameters(20, PARM_CROSSED_OVER, ds.ERADetail.IsCrossedOverColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(21, PARM_CHARGE_AMT, ds.ERADetail.ChargedAmountColumn.ColumnName, DbType.Decimal);

            dbManager.AddParameters(22, PARM_ALLOWED_AMT, ds.ERADetail.AllowedAmountColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(23, PARM_PAID_AMOUNT, ds.ERADetail.PaidAmountColumn.ColumnName, DbType.Decimal);

            dbManager.AddParameters(24, PARM_CO_INS_AMT, ds.ERADetail.CoInsuranceAmountColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(25, PARM_DEDUCTABLE_AMT, ds.ERADetail.DeductableAmountColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(26, PARM_COPAYMENT, ds.ERADetail.CopaymentColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(27, PARM_LATE_FILLING_CHRG, ds.ERADetail.LateFilingChargesColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(28, PARM_INTEREST_AMT, ds.ERADetail.InterestAmountColumn.ColumnName, DbType.Decimal);

            dbManager.AddParameters(29, PARM_IS_ACTIVE, ds.ERADetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(30, PARM_CREATED_BY, ds.ERADetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_CREATED_ON, ds.ERADetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(32, PARM_MODIFIED_BY, ds.ERADetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_MODIFIED_ON, ds.ERADetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(34, PARM_VISIT_DATE, ds.ERADetail.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_WRITE_OFF, ds.ERADetail.WriteOffColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(36, PARM_PATIENT_RESPONSIBILITY, ds.ERADetail.PatientResponsibilityColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(37, PARM_POST_STATUS, ds.ERADetail.PostStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_REMIT_CODE, ds.ERADetail.RemitCodeColumn.ColumnName, DbType.String);

            dbManager.AddParameters(39, PARM_NEXT_RESPONSIBILITY, ds.ERADetail.NextResponsibilityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(40, PARM_PATIENT_INSURANCE_ID, ds.ERADetail.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(41, PARM_MI, ds.ERADetail.MIColumn.ColumnName, DbType.String);

            dbManager.AddParameters(42, PARM_SUBSCRIBER_FIRST_NAME, ds.ERADetail.SubFirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(43, PARM_SUBSCRIBER_LAST_NAME, ds.ERADetail.SubLastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(44, PARM_NEXT_RESPONSIBILITY_ID, ds.ERADetail.NextResponsibilityIdColumn.ColumnName, DbType.String);
        }

        private void CreateERAClaimAdjusmentCodeParameters(IDBManager dbManager, DSERA ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERA_CLAIM_ADJUSTMENT_ID, ds.ERAClaimAdjustmentCode.ERAClaimAdjIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERA_CLAIM_ADJUSTMENT_ID, ds.ERAClaimAdjustmentCode.ERAClaimAdjIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ERA_CLAIM_ADJUSTMENT_GROUP_CODE, ds.ERAClaimAdjustmentCode.AdjGroupCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ERA_CLAIM_ADJUSTMENT_REASON_CODE, ds.ERAClaimAdjustmentCode.AdjReasonCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ERA_CLAIM_ADJUSTMENT_AMOUNT, ds.ERAClaimAdjustmentCode.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ERA_DETAIL_ID, ds.ERAClaimAdjustmentCode.ERADtlIdColumn.ColumnName, DbType.Int64);
        }

        private void CreateERAProviderAdjusmentParameters(IDBManager dbManager, DSERA ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(6);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERA_PROVIDER_ADJUSTMENT_ID, ds.ERAProviderAdjustments.ERAProviderAdjIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERA_PROVIDER_ADJUSTMENT_ID, ds.ERAProviderAdjustments.ERAProviderAdjIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ERA_ID, ds.ERAProviderAdjustments.ERAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ERA_REASON_CODE, ds.ERAProviderAdjustments.ReasonCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ERA_IDENTIFIER, ds.ERAProviderAdjustments.IdentifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ERA_CLAIM_ADJUSTMENT_AMOUNT, ds.ERAProviderAdjustments.AmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(5, PARM_ERA_DESCRIPTION, ds.ERAProviderAdjustments.DescriptionColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "ERA Functions"

        #region "Insert, delete, update and get using dataset Functions"

        public DSERA LoadERA(long ERAId, long ClearingHouseId, string CheckNo, long FacilityId, long PracticeId, string PayerName, int PayeeName, string Status, string FromEntryDate, string ToEntryDate, Int32 PageNumber = 1, Int32 RowsPerPage = 1000, string Module = "")
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Module == "")
                    Module = null;

                if (CheckNo == "")
                    CheckNo = null;

                if (PayerName == "")
                    PayerName = null;

                if (FromEntryDate == "")
                    FromEntryDate = null;

                if (ToEntryDate == "")
                    ToEntryDate = null;

                dbManager.Open();
                dbManager.CreateParameters(16);

                if (ClearingHouseId == 0)
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearingHouseId);

                dbManager.AddParameters(1, PARM_CHECK_NO, CheckNo);

                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                if (PracticeId == 0)
                    dbManager.AddParameters(3, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PRACTICE_ID, PracticeId);

                if (string.IsNullOrEmpty(PayerName))
                    dbManager.AddParameters(4, PARM_PRAYER_NAME, null);
                else
                    dbManager.AddParameters(4, PARM_PRAYER_NAME, PayerName);

                if (PayeeName == 0)
                    dbManager.AddParameters(5, PARM_PAYEE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PAYEE_ID, PayeeName);


                if (string.IsNullOrEmpty(Status))
                    dbManager.AddParameters(6, PARM_STATUS, null);
                else
                    dbManager.AddParameters(6, PARM_STATUS, Status);

                dbManager.AddParameters(7, PARM_FROM_DATE, FromEntryDate);
                dbManager.AddParameters(8, PARM_TO_DATE, ToEntryDate);
                if (ERAId == 0)
                    dbManager.AddParameters(9, PARM_ERA_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ERA_ID, ERAId);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.ERA.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(14, PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(15, PARM_MODULE, Module);

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_SELECT, ds, ds.ERA.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LoadERA", PROC_ERA_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSERA ERASearch(long ERAId, long ClearingHouseId, string CheckNo, long FacilityId, long PracticeId, string PayerName, int PayeeName, string Status, string FromEntryDate, string ToEntryDate, Int32 PageNumber = 1, Int32 RowsPerPage = 1000, string Module = "",string CheckAmount="")
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Module == "")
                    Module = null;

                if (CheckNo == "")
                    CheckNo = null;

                if (PayerName == "")
                    PayerName = null;

                if (FromEntryDate == "")
                    FromEntryDate = null;

                if (ToEntryDate == "")
                    ToEntryDate = null;

                dbManager.Open();
                //PRD-281 by:MAHMAD
                dbManager.CreateParameters(17);
                //PRD-281 by:MAHMAD
                if (ClearingHouseId == 0)
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearingHouseId);

                dbManager.AddParameters(1, PARM_CHECK_NO, CheckNo);

                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                if (PracticeId == 0)
                    dbManager.AddParameters(3, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PRACTICE_ID, PracticeId);

                if (string.IsNullOrEmpty(PayerName))
                    dbManager.AddParameters(4, PARM_PRAYER_NAME, null);
                else
                    dbManager.AddParameters(4, PARM_PRAYER_NAME, PayerName);

                if (PayeeName == 0)
                    dbManager.AddParameters(5, PARM_PAYEE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PAYEE_ID, PayeeName);


                if (string.IsNullOrEmpty(Status))
                    dbManager.AddParameters(6, PARM_STATUS, null);
                else
                    dbManager.AddParameters(6, PARM_STATUS, Status);

                dbManager.AddParameters(7, PARM_FROM_DATE, FromEntryDate);
                dbManager.AddParameters(8, PARM_TO_DATE, ToEntryDate);
                if (ERAId == 0)
                    dbManager.AddParameters(9, PARM_ERA_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ERA_ID, ERAId);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.ERA.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(14, PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(15, PARM_MODULE, Module);
                //PRD-281 by:MAHMAD
                if (string.IsNullOrEmpty(CheckAmount))
                    dbManager.AddParameters(16, PARM_CHECK_AMOUNT, null);
                else
                    dbManager.AddParameters(16, PARM_CHECK_AMOUNT,CheckAmount);
                //PRD-281 by:MAHMAD
                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_SEARCH, ds, ds.ERA.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::ERASearch", PROC_ERA_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public DSERA UpdateERA(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateERAParameters(dbManager, ds, false);
                ds = (DSERA)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ERA_UPDATE, ds, ds.ERA.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::UpdateERA", PROC_ERA_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteERA(string ERAId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ERA_ID, ERAId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERA_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::DeleteERA", PROC_ERA_DELETE, ex);
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
        public DSERA InsertERA(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERAParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_INSERT, ds, ds.ERA.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERA", PROC_ERA_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERA(DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERAParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_INSERT, ds, ds.ERA.TableName);
                ds.AcceptChanges();
                if (ds.ERA.Rows[0][ds.ERA.ERAIdColumn].ToString() == "-2")
                {
                    throw new Exception(ds.ERA.Rows[0][ds.ERA.DuplicateChkMesgColumn].ToString());
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERA", PROC_ERA_INSERT, ex);
                throw ex;
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        public DSERA InsertERA(SharedVariable SharedVariable,DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERAParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_INSERT, ds, ds.ERA.TableName);
                ds.AcceptChanges();
                if (ds.ERA.Rows[0][ds.ERA.ERAIdColumn].ToString() == "-2")
                {
                    throw new Exception(ds.ERA.Rows[0][ds.ERA.DuplicateChkMesgColumn].ToString());
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALERA::InsertERA", PROC_ERA_INSERT, ex);
                throw ex;
            }

        }

        public DSERALookup LookupERAStatus()
        {
            DSERALookup ds = new DSERALookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSERALookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_STATUS_LOOKUP, ds, ds.ERAStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LookupERAStatus", PROC_ERA_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERALookup LookupERAPayee()
        {
            DSERALookup ds = new DSERALookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSERALookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_PAYEE_LOOKUP, ds, ds.ERAPayeeLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LookupERAPayee", PROC_ERA_PAYEE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA LoadERADetail(long ERADtlId, long ERAId, long VisitId)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ERADtlId == 0)
                    dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ERADtlId);

                if (ERAId == 0)
                    dbManager.AddParameters(1, PARM_ERA_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ERA_ID, ERAId);

                //if (VisitId == 0)
                //    dbManager.AddParameters(3, PARM_VISIT_ID, null);
                //else
                //    dbManager.AddParameters(3, PARM_VISIT_ID, VisitId);

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_DETAIL_SELECT, ds, ds.ERADetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LoadERA", PROC_ERA_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSERA LoadElectronicEOB(long ChargeId, long visitId)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ChargeId == 0)
                    dbManager.AddParameters(0, PARM_CHARGE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CHARGE_ID, ChargeId);
                if (visitId == 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, visitId);


                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_ERA_ELECTRONIC_EOB, ds, ds.ElectronicEOB.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LoadElectronicEOB", PROC_BILLING_ERA_ELECTRONIC_EOB, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #endregion


        public DSERA FillERALinkedHistory(long ERADtlID)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (ERADtlID == 0)
                    dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ERADtlID);

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_LINKED_HISTORY, ds, ds.ERALinkedHistory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::FillERALinkedHistory", PROC_ERA_LINKED_HISTORY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region  ERA Charge Detail
        public DSERA LoadERADetail(long ERADtID, long ERAID, string Module = "")
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Module == "")
                    Module = null;
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (ERADtID == 0)
                    dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ERADtID);

                if (ERAID == 0)
                    dbManager.AddParameters(1, PARM_ERA_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ERA_ID, ERAID);
                dbManager.AddParameters(2, PARM_MODULE, Module);
                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_DETAIL_SELECT, ds, ds.ERADetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LoadERADetail", PROC_ERA_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERADetail(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERADetailParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_DETAIL_INSERT, ds, ds.ERADetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERADetail", PROC_ERA_DETAIL_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERADetail(DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERADetailParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_DETAIL_INSERT, ds, ds.ERADetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERADetail", PROC_ERA_DETAIL_INSERT, ex);
                throw ex;
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        public DSERA InsertERADetail(SharedVariable SharedVariable, DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERADetailParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_DETAIL_INSERT, ds, ds.ERADetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALERA::InsertERADetail", PROC_ERA_DETAIL_INSERT, ex);
                throw ex;
            }

        }

        public DSERA UpdateERADetail(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERADetailParameters(dbManager, ds, false);
                ds = (DSERA)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ERA_DETAIL_UPDATE, ds, ds.ERADetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::UpdateERADetail", PROC_ERA_DETAIL_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string LinkERADetail(long ERADetailId, long ChargeId, bool IsLink, long PaymentInsuranceId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ERADetailId);
                dbManager.AddParameters(1, PARM_CHARGE_ID, ChargeId);
                dbManager.AddParameters(2, PARM_IS_LINK_CHARGE, IsLink);

                if (PaymentInsuranceId > 0)
                    dbManager.AddParameters(3, PARM_ERA_PAYMENT_INSURANCE_ID, PaymentInsuranceId);
                else
                    dbManager.AddParameters(3, PARM_ERA_PAYMENT_INSURANCE_ID, null);


                dbManager.AddParameters(4, PARM_LINKED_By, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(5, PARM_LINKED_DATE, DateTime.Now);

                dbManager.AddParameters(6, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERA_LINK_CHARGE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LinkERADetail", PROC_ERA_LINK_CHARGE, ex);
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

        public List<ERAVoidedClaims> IsVoidedClaimExist(Int64 ERAID,Int64 VisitId)
        {
            List<ERAVoidedClaims> ERAVoidedClaimsList = new List<ERAVoidedClaims>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_ERA_ID, ERAID);
                dbManager.AddParameters(PARM_VISIT_ID, VisitId);
                ERAVoidedClaimsList = dbManager.ExecuteReaders<ERAVoidedClaims>(PROC_ERA_VOIDED_CLAIMS);
                return ERAVoidedClaimsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::IsVoidedClaimExist", PROC_ERA_VOIDED_CLAIMS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public string DeleteERADetail(string ERADetailId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ERA_DETAIL_ID, ERADetailId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERA_DETAIL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::DeleteERADetail", PROC_ERA_DETAIL_DELETE, ex);
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

        public DSERA LinkERACharges(string FirstName, string LastName, string CPTCode, string Modifiers, long ERAChargeId, string ERAClaimNo, IDBManager dbManager, DateTime? DOSFrom = null)
        {
            DSERA ds = new DSERA();
            try
            {
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, PARM_FIRST_NAME, FirstName == "" ? null : FirstName);

                dbManager.AddParameters(1, PARM_LAST_NAME, LastName == "" ? null : LastName);

                dbManager.AddParameters(2, PARM_DOS_FROM, DOSFrom);

                dbManager.AddParameters(3, PARM_CPT_CODE, CPTCode == "" ? null : CPTCode);

                dbManager.AddParameters(4, PARM_MODIFIERS, Modifiers == "" ? null : Modifiers);


                if (ERAChargeId == 0)
                    dbManager.AddParameters(5, PARM_ERA_CHARGE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_ERA_CHARGE_ID, ERAChargeId);

                dbManager.AddParameters(6, PARM_ERA_CLAIM_NO, ERAClaimNo == "" ? null : ERAClaimNo);

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_LINK_CHARGES, ds, ds.LinkERACharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LinkERACharges", PROC_ERA_LINK_CHARGES, ex);
                throw ex;
            }
        }

        #endregion

        #region " ERA Claim AdjustmentCode "

        public DSERA LoadERAClaimAdjustmentCode(long ERAClaimAdjId, long ERADtID)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ERAClaimAdjId == 0)
                    dbManager.AddParameters(0, PARM_ERA_CLAIM_ADJUSTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERA_CLAIM_ADJUSTMENT_ID, ERAClaimAdjId);

                if (ERADtID == 0)
                    dbManager.AddParameters(1, PARM_ERA_DETAIL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ERA_DETAIL_ID, ERADtID);
                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_CLAIM_ADJUSTMENT_CODE_SELECT, ds, ds.ERAClaimAdjustmentCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LoadERAClaimAdjustmentCode", PROC_ERA_CLAIM_ADJUSTMENT_CODE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERAClaimAdjustmentCode(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERAClaimAdjusmentCodeParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT, ds, ds.ERAClaimAdjustmentCode.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERAClaimAdjustmentCode", PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERAClaimAdjustmentCode(DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERAClaimAdjusmentCodeParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT, ds, ds.ERAClaimAdjustmentCode.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERAClaimAdjustmentCode", PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT, ex);
                throw ex;
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        public DSERA InsertERAClaimAdjustmentCode(SharedVariable SharedVariable,DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERAClaimAdjusmentCodeParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT, ds, ds.ERAClaimAdjustmentCode.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALERA::InsertERAClaimAdjustmentCode", PROC_ERA_CLAIM_ADJUSTMENT_CODE_INSERT, ex);
                throw ex;
            }

        }

        public DSERA UpdateERAClaimAdjustmentCode(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERAClaimAdjusmentCodeParameters(dbManager, ds, false);
                ds = (DSERA)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ERA_CLAIM_ADJUSTMENT_CODE_UPDATE, ds, ds.ERAClaimAdjustmentCode.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::UpdateERAClaimAdjustmentCode", PROC_ERA_CLAIM_ADJUSTMENT_CODE_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteERAClaimAdjustmentCode(string ERAClaimAdjustmentCodeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ERA_CLAIM_ADJUSTMENT_ID, ERAClaimAdjustmentCodeId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERA_CLAIM_ADJUSTMENT_CODE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::DeleteERAClaimAdjustmentCode", PROC_ERA_CLAIM_ADJUSTMENT_CODE_DELETE, ex);
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


        #region " ERA Provider Adjustments "

        public DSERA LoadERAProviderAdjustments(long ERAProviderAdjId, long ERAId)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ERAProviderAdjId == 0)
                    dbManager.AddParameters(0, PARM_ERA_PROVIDER_ADJUSTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERA_PROVIDER_ADJUSTMENT_ID, ERAProviderAdjId);

                if (ERAId == 0)
                    dbManager.AddParameters(1, PARM_ERA_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ERA_ID, ERAId);

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERA_PROVIDER_ADJUSTMENT_SELECT, ds, ds.ERAProviderAdjustments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::LoadERAProviderAdjustments", PROC_ERA_PROVIDER_ADJUSTMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERAProviderAdjustments(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERAProviderAdjusmentParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_PROVIDER_ADJUSTMENT_INSERT, ds, ds.ERAProviderAdjustments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERAProviderAdjustments", PROC_ERA_PROVIDER_ADJUSTMENT_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERA InsertERAProviderAdjustments(DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERAProviderAdjusmentParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_PROVIDER_ADJUSTMENT_INSERT, ds, ds.ERAProviderAdjustments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERAProviderAdjustments", PROC_ERA_PROVIDER_ADJUSTMENT_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        public DSERA InsertERAProviderAdjustments(SharedVariable SharedVariable,DSERA ds, IDBManager dbManager)
        {
            try
            {
                CreateERAProviderAdjusmentParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERA_PROVIDER_ADJUSTMENT_INSERT, ds, ds.ERAProviderAdjustments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALERA::InsertERAProviderAdjustments", PROC_ERA_PROVIDER_ADJUSTMENT_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
        }

        public DSERA UpdateERAProviderAdjustments(DSERA ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateERAProviderAdjusmentParameters(dbManager, ds, false);
                ds = (DSERA)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ERA_PROVIDER_ADJUSTMENT_UPDATE, ds, ds.ERAProviderAdjustments.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::UpdateERAProviderAdjustments", PROC_ERA_PROVIDER_ADJUSTMENT_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteERAProviderAdjustment(string ERAProviderAdjId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ERA_PROVIDER_ADJUSTMENT_ID, ERAProviderAdjId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERA_PROVIDER_ADJUSTMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::DeleteERAProviderAdjustment", PROC_ERA_PROVIDER_ADJUSTMENT_DELETE, ex);
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

        #region ERA Logging
        public string InsertERALog(SharedVariable SharedVariable,string xmlofERALogger)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ERA_LOGGER_XML, xmlofERALogger);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_ERA_LOGGER_INSERT));

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable,"DALERA::InsertERALog", PROC_BILLING_ERA_LOGGER_INSERT, ex);
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

        public string InsertERALog(string xmlofERALogger)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ERA_LOGGER_XML, xmlofERALogger);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_ERA_LOGGER_INSERT));

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERA::InsertERALog", PROC_BILLING_ERA_LOGGER_INSERT, ex);
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
