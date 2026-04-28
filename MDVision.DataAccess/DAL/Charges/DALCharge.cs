using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Charges
{
    public class DALCharge
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PATIENT_CHARGES_SELECT = "Patient.sp_PatientChargesSelect";
        private const string PROC_PAYMENT_CHARGES_SELECT = "Patient.sp_PaymentChargesSelect";
        private const string PROC_PATIENT_VISIT_CHARGES_SELECT = "Patient.sp_VisitChargesSelect";
        private const string PROC_PATIENT_VISIT_CHARGES_SEARCH = "Patient.sp_VisitChargesSelect_Search";

        private const string PROC_PATIENT_CHARGES_INSERT = "Patient.sp_PatientChargesInsert";
        private const string PROC_PATIENT_CHARGES_UPDATE = "Patient.sp_PatientChargesUpdate";
        private const string PROC_PATIENT_CHARGES_DELETE = "Patient.sp_PatientChargesDelete";

        private const string PROC_UNCLAIMED_APP_SELECT = "Patient.sp_UnClaimedAppointmentsSelect";
        private const string PROC_UNCLAIMED_APP_SELECT_NEW = "[Patient].[sp_UnClaimedAppointmentsSelect_new]";

        private const string PROC_APPVISIT_STATUS_LOOKUP = "Patient.sp_AppVisitStatusLookup";
        private const string PROC_PATIENT_TRANSFER_CHARGES_SELECT = "Patient.sp_PatientTransferChargesSelect";
        private const string PROC_BATCH_LOOKUPS = "Patient.sp_BatchLookUps";
        private const string PROC_BILL_TO_PATIENT_CHARGES = "Patient.sp_BillToPatientCharges";
        private const string PROC_CHARGE_ENCOUNTER_TYPE_LOOKUP = "Clinical.sp_EncounterTypeLookup";

        private const string PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT = "Patient.sp_ChargesICDPointersInsert";
        private const string PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE = "Patient.sp_ChargesICDPointersUpdate";
        private const string PROC_PATIENT_CHARGES_ICD_POINTERS_DELETE = "Patient.sp_ChargesICDPointersDelete";
        private const string PROC_PATIENT_CHARGES_ICD_POINTERS_SELECT = "Patient.sp_ChargesICDPointersSelect";

        #endregion

        #region "Parameters"

        private const string PARM_CHARGECAP_ID = "@ChargeCapId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_UNITS = "@Units";
        private const string PARM_MODIFIER_1 = "@Modifier1";
        private const string PARM_MODIFIER_2 = "@Modifier2";
        private const string PARM_MODIFIER_3 = "@Modifier3";
        private const string PARM_MODIFIER_4 = "@Modifier4";
        private const string PARM_POS_CODE = "@POSCode";
        private const string PARM_EMG = "@EMG";
        private const string PARM_FEE = "@Fee";


        private const string PARM_INS_CHARGES = "@InsCharges";
        private const string PARM_PAT_CHARGES = "@PatCharges";
        private const string PARM_ALLOWED_AMT = "@AllowedAmt";
        private const string PARM_COPAY = "@Copay";

        private const string PARM_PAN = "@PAN";
        private const string PARM_NDC = "@NDC";
        private const string PARM_NDC_UNIT = "@NDCUnit";
        private const string PARM_NDC_UNIT_PRICE = "@NDCUnitPrice";
        private const string PARM_NDC_MEASURE_CODEID = "@NDCMeasurCodeId";
        private const string PARM_LINE_NOTES = "@LineNotes";


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";



        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";

        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";


        private const string PARM_CHECKIN = "@CheckIn";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_CLAIMED_NUMBER = "@ClaimNumber";

        private const string PARM_PAID = "@Paid";
        private const string PARM_CASE_MGMT_ID = "@CaseMgmtId";

        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_PATINSURANCE_ID = "@PatInsuranceId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";

        private const string PARM_APPOINTMENT_STATUS = "@AppointmentStatus";

        private const string PARM_CHARGE_STATUS = "@ChargeStatus";

        private const string PARM_EOD = "@EOD";
        private const string PARM_STATUS = "@Status";
        private const string PARM_MASTER_CHARGE = "@MasterChargeId";
        private const string PARM_HOLD_DAYS = "@HoldDays";
        private const string PARM_IS_HOLD = "@IsHold";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_CLAIM_TYPE = "@ClaimType";
        
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_CHARGE_ORDER = "@ChargeOrder";
        private const string PARM_IS_PAYMENT = "@IsPayment";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";

        private const string PARM_837_BATCH_ID = "@837BatchId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_SUBMIT_STAUT_ID = "@SubmitStatusId";
        private const string PARM_APPT_STAUT_ID = "@AppointmentStatusId";
        private const string PARM_CLAIM_STATUS = "@ClaimStatusId";


        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
   
        private const string PARM_SERVICE_DESCRIPTION = "@ServiceDescription";
        private const string PARM_EXPECTED_FEE = "@ExpectedFee";//
        private const string PARM_PARENT_CHARGE_ID = "@ParentChargeId";

        private const string PARM_LEXICODE = "@LexiCode";
        private const string PARM_LEXICODE_DESCRIPTION = "@LexiCodeDescription";

        private const string PARM_CLAIM_CREATED_BY = "@ClaimCreatedBy";
        private const string PARM_CLAIM_CREATED_FROM = "@ClaimCreatedFrom";
        private const string PARM_CLAIM_CREATED_TO = "@ClaimCreatedTo";
        private const string PARM_ENCOUNTER_SIGNOFF_DATE = "@EncounterSignOffDate";
        private const string PARM_HOLD_FROM = "@HoldFrom";

        private const string PARM_CPT_DESCRIPTION = "@CPTDescription";
        private const string PARM_RESOURCE_PROVIDER_ID = "@ResourceProviderId";
        private const string PARM_IMPORTED_DATE_FROM = "@ImportedDateFrom";
        private const string PARM_IMPORTED_DATE_TO = "@ImportedDateTo";
        private const string PARM_SUBMITTED_DATE_FROM = "@SubmittedDateFrom";
        private const string PARM_SUBMITTED_DATE_TO = "@SubmittedDateTo";
        private const string PARM_BILL_TO_PATIENT = "@BillToPatient";
        private const string PARM_INCLUDE_SECONDARY_CLAIM = "@IncludeSecondaryClaim";
        private const string PARM_LOCKED_ON = "@LockedOn";
        private const string PARM_IS_LOCKED = "@IsLocked";
        private const string PARM_IS_VNC = "@IsVNC";
        private const string PARM_VNC_VISIT_ID = "@VNCVisitId";
        private const string PARM_VNC_CHARGES_ID = "@VNCChargesId";
        private const string PARM_AUTHORIZATION_ID = "@AuthorizeId";
        private const string PARM_ICDPOINTER_1 = "@ICDPointer1";
        private const string PARM_ICDPOINTER_2 = "@ICDPointer2";
        private const string PARM_ICDPOINTER_3 = "@ICDPointer3";
        private const string PARM_ICDPOINTER_4 = "@ICDPointer4";
        private const string PARM_CLIA = "@CLIA";
        private const string PARM_INCLUDE_VOIDED_CLAIMS = "@IncludeVoidedClaims";
        private const string PARM_CLAIM_ERROR_ID = "@ClaimErroredId";
        private const string PARM_ENCOUNTER_TYPE_ID = "@EncounterTypeId";

        private const string PARM_ICD_POINTER_ID = "@ICDPointerId";
        private const string PARM_PVICD_ID = "@PVICDId";
        private const string PARM_ORDER = "@Order";
        private const string PARM_TOS_CODE = "@TOSCode";
        private const string PARM_START_TIME = "@StartTime";
        private const string PARM_END_TIME = "@EndTime";
        private const string PARM_TIME_UNITS = "@TimeUnits";
        private const string PARM_BASE_UNITS = "@BaseUnits";
        private const string PARM_RISK_UNITS = "@RiskUnits";
        private const string PARM_TOTAL_MINUTES = "@TotalMinutes";
        private const string PARM_TOTAL_UNITS = "@TotalUnits";
        private const string PARM_ROUND_BILLED_UNITS = "@RoundBilledUnitsId";
        private const string PARM_REPORT_CPT_DESC = "@ReportCPTDesc";
        private const string PARM_PATIENT_NAME = "@PatientName";
        private const string PARM_ISVOIDED_CLAIM = "@IsVoidedClaim";
        private const string PARM_DRUG_CODE_COST = "@DrugCodeCost";
        private const string PARM_NDC_DESCRIPTION = "@NDCDescription";

        #endregion

        #region Constructors
        public DALCharge()
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


        private void CreateParameters(IDBManager dbManager, DSCharge ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(54);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CHARGECAP_ID, ds.PatientCharges.ChargeCapIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CHARGECAP_ID, ds.PatientCharges.ChargeCapIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VISIT_ID, ds.PatientCharges.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DOS_FROM, ds.PatientCharges.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_DOS_TO, ds.PatientCharges.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_CPT_CODE, ds.PatientCharges.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_UNITS, ds.PatientCharges.UnitsColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(6, PARM_MODIFIER_1, ds.PatientCharges.Modifier1Column.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIER_2, ds.PatientCharges.Modifier2Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIER_3, ds.PatientCharges.Modifier3Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIER_4, ds.PatientCharges.Modifier4Column.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_POS_CODE, ds.PatientCharges.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_EMG, ds.PatientCharges.EMGColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_FEE, ds.PatientCharges.FeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(13, PARM_INS_CHARGES, ds.PatientCharges.InsChargesColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(14, PARM_PAT_CHARGES, ds.PatientCharges.PatChargesColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(15, PARM_ALLOWED_AMT, ds.PatientCharges.AllowedAmtColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(16, PARM_COPAY, ds.PatientCharges.CopayColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(17, PARM_PAN, ds.PatientCharges.PANColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_NDC, ds.PatientCharges.NDCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_NDC_UNIT, ds.PatientCharges.NDCUnitColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(20, PARM_NDC_UNIT_PRICE, ds.PatientCharges.NDCUnitPriceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(21, PARM_NDC_MEASURE_CODEID, ds.PatientCharges.NDCMeasurCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(22, PARM_LINE_NOTES, ds.PatientCharges.LineNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_IS_ACTIVE, ds.PatientCharges.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(24, PARM_CREATED_BY, ds.PatientCharges.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CREATED_ON, ds.PatientCharges.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_MODIFIED_BY, ds.PatientCharges.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_MODIFIED_ON, ds.PatientCharges.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(28, PARM_EOD, ds.PatientCharges.EODColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_STATUS, ds.PatientCharges.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_MASTER_CHARGE, ds.PatientCharges.MasterChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(31, PARM_CHARGE_ORDER, ds.PatientCharges.ChargeOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(32, PARM_EXPECTED_FEE, ds.PatientCharges.ExpectedFeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(33, PARM_SERVICE_DESCRIPTION, ds.PatientCharges.ServiceDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_PARENT_CHARGE_ID, ds.PatientCharges.ParentChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(35, PARM_AUTHORIZATION_ID, ds.PatientCharges.AuthorizeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(36, PARM_CPT_DESCRIPTION, ds.PatientCharges.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(37, PARM_ICDPOINTER_1, ds.PatientCharges.ICDPointer1Column.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_ICDPOINTER_2, ds.PatientCharges.ICDPointer2Column.ColumnName, DbType.String);
            dbManager.AddParameters(39, PARM_ICDPOINTER_3, ds.PatientCharges.ICDPointer3Column.ColumnName, DbType.String);
            dbManager.AddParameters(40, PARM_ICDPOINTER_4, ds.PatientCharges.ICDPointer4Column.ColumnName, DbType.String);
            dbManager.AddParameters(41, PARM_CLIA, ds.PatientCharges.CLIAColumn.ColumnName, DbType.String);
            dbManager.AddParameters(42, PARM_TOS_CODE, ds.PatientCharges.TOSCodeColumn.ColumnName, DbType.String);
            //Begin: July 27th, 2016: Abdur Rehman, PARAMS for Anesthesia
            dbManager.AddParameters(43, PARM_START_TIME, ds.PatientCharges.StartTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(44, PARM_END_TIME, ds.PatientCharges.EndTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(45, PARM_TIME_UNITS, ds.PatientCharges.TimeUnitsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(46, PARM_BASE_UNITS, ds.PatientCharges.BaseUnitsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(47, PARM_RISK_UNITS, ds.PatientCharges.RiskUnitsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(48, PARM_TOTAL_MINUTES, ds.PatientCharges.TotalMinutesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(49, PARM_TOTAL_UNITS, ds.PatientCharges.TotalUnitsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(50, PARM_ROUND_BILLED_UNITS, ds.PatientCharges.RoundBilledUnitsIdColumn.ColumnName, DbType.String);
            //End: July 27th, 2016: Abdur Rehman, PARAMS for Anesthesia
            dbManager.AddParameters(51, PARM_DRUG_CODE_COST, ds.PatientCharges.DrugCodeCostColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(52, PARM_REPORT_CPT_DESC, ds.PatientCharges.IsReportCPTDescColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(53, PARM_NDC_DESCRIPTION, ds.PatientCharges.NDCDescriptionColumn.ColumnName, DbType.String);
        }

        private void CreateInsertParameters(IDBManager dbManager, DSCharge ds)
        {
            dbManager.CreateInsertParameters(54);

            dbManager.AddInsertUpdateParameters(0, PARM_CHARGECAP_ID, ds.PatientCharges.ChargeCapIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_VISIT_ID, ds.PatientCharges.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_DOS_FROM, ds.PatientCharges.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_DOS_TO, ds.PatientCharges.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_CPT_CODE, ds.PatientCharges.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_UNITS, ds.PatientCharges.UnitsColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(6, PARM_MODIFIER_1, ds.PatientCharges.Modifier1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIER_2, ds.PatientCharges.Modifier2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIER_3, ds.PatientCharges.Modifier3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIER_4, ds.PatientCharges.Modifier4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_POS_CODE, ds.PatientCharges.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_EMG, ds.PatientCharges.EMGColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_FEE, ds.PatientCharges.FeeColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(13, PARM_INS_CHARGES, ds.PatientCharges.InsChargesColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(14, PARM_PAT_CHARGES, ds.PatientCharges.PatChargesColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(15, PARM_ALLOWED_AMT, ds.PatientCharges.AllowedAmtColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(16, PARM_COPAY, ds.PatientCharges.CopayColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(17, PARM_PAN, ds.PatientCharges.PANColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_NDC, ds.PatientCharges.NDCColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_NDC_UNIT, ds.PatientCharges.NDCUnitColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(20, PARM_NDC_UNIT_PRICE, ds.PatientCharges.NDCUnitPriceColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(21, PARM_NDC_MEASURE_CODEID, ds.PatientCharges.NDCMeasurCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_LINE_NOTES, ds.PatientCharges.LineNotesColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_IS_ACTIVE, ds.PatientCharges.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(24, PARM_CREATED_BY, ds.PatientCharges.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(25, PARM_CREATED_ON, ds.PatientCharges.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(26, PARM_MODIFIED_BY, ds.PatientCharges.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(27, PARM_MODIFIED_ON, ds.PatientCharges.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(28, PARM_EOD, ds.PatientCharges.EODColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_STATUS, ds.PatientCharges.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(30, PARM_MASTER_CHARGE, ds.PatientCharges.MasterChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(31, PARM_CHARGE_ORDER, ds.PatientCharges.ChargeOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(32, PARM_EXPECTED_FEE, ds.PatientCharges.ExpectedFeeColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(33, PARM_SERVICE_DESCRIPTION, ds.PatientCharges.ServiceDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(34, PARM_PARENT_CHARGE_ID, ds.PatientCharges.ParentChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(35, PARM_AUTHORIZATION_ID, ds.PatientCharges.AuthorizeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(36, PARM_CPT_DESCRIPTION, ds.PatientCharges.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(37, PARM_ICDPOINTER_1, ds.PatientCharges.ICDPointer1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(38, PARM_ICDPOINTER_2, ds.PatientCharges.ICDPointer2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(39, PARM_ICDPOINTER_3, ds.PatientCharges.ICDPointer3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(40, PARM_ICDPOINTER_4, ds.PatientCharges.ICDPointer4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(41, PARM_CLIA, ds.PatientCharges.CLIAColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(42, PARM_TOS_CODE, ds.PatientCharges.TOSCodeColumn.ColumnName, DbType.String);
            //Begin: July 27th, 2016: Abdur Rehman, PARAMS for Anesthesia
            dbManager.AddInsertUpdateParameters(43, PARM_START_TIME, ds.PatientCharges.StartTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(44, PARM_END_TIME, ds.PatientCharges.EndTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(45, PARM_TIME_UNITS, ds.PatientCharges.TimeUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(46, PARM_BASE_UNITS, ds.PatientCharges.BaseUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(47, PARM_RISK_UNITS, ds.PatientCharges.RiskUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(48, PARM_TOTAL_MINUTES, ds.PatientCharges.TotalMinutesColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(49, PARM_TOTAL_UNITS, ds.PatientCharges.TotalUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(50, PARM_ROUND_BILLED_UNITS, ds.PatientCharges.RoundBilledUnitsIdColumn.ColumnName, DbType.String);
            //End: July 27th, 2016: Abdur Rehman, PARAMS for Anesthesia
            dbManager.AddInsertUpdateParameters(51, PARM_DRUG_CODE_COST, ds.PatientCharges.DrugCodeCostColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(52, PARM_REPORT_CPT_DESC, ds.PatientCharges.IsReportCPTDescColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(53, PARM_NDC_DESCRIPTION, ds.PatientCharges.NDCDescriptionColumn.ColumnName, DbType.String);

        }

        private void CreateUpdateParameters(IDBManager dbManager, DSCharge ds)
        {
            dbManager.CreateUpdateParameters(54);

            dbManager.AddInsertUpdateParameters(0, PARM_CHARGECAP_ID, ds.PatientCharges.ChargeCapIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_VISIT_ID, ds.PatientCharges.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_DOS_FROM, ds.PatientCharges.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_DOS_TO, ds.PatientCharges.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_CPT_CODE, ds.PatientCharges.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_UNITS, ds.PatientCharges.UnitsColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(6, PARM_MODIFIER_1, ds.PatientCharges.Modifier1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIER_2, ds.PatientCharges.Modifier2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIER_3, ds.PatientCharges.Modifier3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIER_4, ds.PatientCharges.Modifier4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_POS_CODE, ds.PatientCharges.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_EMG, ds.PatientCharges.EMGColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_FEE, ds.PatientCharges.FeeColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(13, PARM_INS_CHARGES, ds.PatientCharges.InsChargesColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(14, PARM_PAT_CHARGES, ds.PatientCharges.PatChargesColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(15, PARM_ALLOWED_AMT, ds.PatientCharges.AllowedAmtColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(16, PARM_COPAY, ds.PatientCharges.CopayColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(17, PARM_PAN, ds.PatientCharges.PANColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_NDC, ds.PatientCharges.NDCColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_NDC_UNIT, ds.PatientCharges.NDCUnitColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(20, PARM_NDC_UNIT_PRICE, ds.PatientCharges.NDCUnitPriceColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(21, PARM_NDC_MEASURE_CODEID, ds.PatientCharges.NDCMeasurCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_LINE_NOTES, ds.PatientCharges.LineNotesColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_IS_ACTIVE, ds.PatientCharges.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(24, PARM_CREATED_BY, ds.PatientCharges.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(25, PARM_CREATED_ON, ds.PatientCharges.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(26, PARM_MODIFIED_BY, ds.PatientCharges.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(27, PARM_MODIFIED_ON, ds.PatientCharges.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(28, PARM_EOD, ds.PatientCharges.EODColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_STATUS, ds.PatientCharges.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(30, PARM_MASTER_CHARGE, ds.PatientCharges.MasterChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(31, PARM_CHARGE_ORDER, ds.PatientCharges.ChargeOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(32, PARM_EXPECTED_FEE, ds.PatientCharges.ExpectedFeeColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(33, PARM_SERVICE_DESCRIPTION, ds.PatientCharges.ServiceDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(34, PARM_PARENT_CHARGE_ID, ds.PatientCharges.ParentChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(35, PARM_AUTHORIZATION_ID, ds.PatientCharges.AuthorizeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(36, PARM_CPT_DESCRIPTION, ds.PatientCharges.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(37, PARM_ICDPOINTER_1, ds.PatientCharges.ICDPointer1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(38, PARM_ICDPOINTER_2, ds.PatientCharges.ICDPointer2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(39, PARM_ICDPOINTER_3, ds.PatientCharges.ICDPointer3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(40, PARM_ICDPOINTER_4, ds.PatientCharges.ICDPointer4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(41, PARM_CLIA, ds.PatientCharges.CLIAColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(42, PARM_TOS_CODE, ds.PatientCharges.TOSCodeColumn.ColumnName, DbType.String);
            //Begin: July 27th, 2016: Abdur Rehman, PARAMS for Anesthesia
            dbManager.AddInsertUpdateParameters(43, PARM_START_TIME, ds.PatientCharges.StartTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(44, PARM_END_TIME, ds.PatientCharges.EndTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(45, PARM_TIME_UNITS, ds.PatientCharges.TimeUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(46, PARM_BASE_UNITS, ds.PatientCharges.BaseUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(47, PARM_RISK_UNITS, ds.PatientCharges.RiskUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(48, PARM_TOTAL_MINUTES, ds.PatientCharges.TotalMinutesColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(49, PARM_TOTAL_UNITS, ds.PatientCharges.TotalUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(50, PARM_ROUND_BILLED_UNITS, ds.PatientCharges.RoundBilledUnitsIdColumn.ColumnName, DbType.String);
            //End: July 27th, 2016: Abdur Rehman, PARAMS for Anesthesia
            dbManager.AddInsertUpdateParameters(51, PARM_DRUG_CODE_COST, ds.PatientCharges.DrugCodeCostColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(52, PARM_REPORT_CPT_DESC, ds.PatientCharges.IsReportCPTDescColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(53, PARM_NDC_DESCRIPTION, ds.PatientCharges.NDCDescriptionColumn.ColumnName, DbType.String);
        }

        private void CreateICDPointerParameters(IDBManager dbManager, DSCharge ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICD_POINTER_ID, ds.ChargesICDPointers.ICDPointerIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICD_POINTER_ID, ds.ChargesICDPointers.ICDPointerIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PVICD_ID, ds.ChargesICDPointers.PVICDIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CHARGECAP_ID, ds.ChargesICDPointers.ChargeCapIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ORDER, ds.ChargesICDPointers.OrderColumn.ColumnName, DbType.Int32);
        }

        private void CreateICDPointerUpdateParameters(IDBManager dbManager, DSCharge ds)
        {
            dbManager.CreateInsertParameters(4);
            dbManager.AddInsertUpdateParameters(0, PARM_ICD_POINTER_ID, ds.ChargesICDPointers.ICDPointerIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_PVICD_ID, ds.ChargesICDPointers.PVICDIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CHARGECAP_ID, ds.ChargesICDPointers.ChargeCapIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_ORDER, ds.ChargesICDPointers.OrderColumn.ColumnName, DbType.Int32);
        }

        private void CreateICDPointerInsertParameters(IDBManager dbManager, DSCharge ds)
        {
            dbManager.CreateUpdateParameters(4);
            dbManager.AddInsertUpdateParameters(0, PARM_ICD_POINTER_ID, ds.ChargesICDPointers.ICDPointerIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_PVICD_ID, ds.ChargesICDPointers.PVICDIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CHARGECAP_ID, ds.ChargesICDPointers.ChargeCapIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_ORDER, ds.ChargesICDPointers.OrderColumn.ColumnName, DbType.Int32);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"




        public DSCharge InserPatientCharges(DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientCharges.GetChanges();

                CreateParameters(dbManager, ds, true);

                ds = (DSCharge)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_INSERT, ds, ds.PatientCharges.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientCharges.Rows[0][ds.PatientCharges.ChargeCapIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InserPatientCharges", PROC_PATIENT_CHARGES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge InsertPatientCharges(DSCharge ds, IDBManager dbManager)
        {
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientCharges.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCharge)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_INSERT, ds, ds.PatientCharges.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    //to handle multiple line item insertion issue with same chargecapid
                    var _capIds1 = new System.Collections.Generic.List<string>();
                    foreach (System.Data.DataRow _r in ds.PatientCharges.Rows) _capIds1.Add(_r["ChargeCapId"].ToString());
                    string ChargeCapIds = string.Join(",", _capIds1);
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ChargeCapIds);
                    dsDBAudit.AcceptChanges();


                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InsertPatientCharges", PROC_PATIENT_CHARGES_INSERT, ex);
                throw ex;
            }
        }

        public string DeletePatientCharges(DSCharge dstemp, long ChargeCapId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = dstemp.PatientCharges;

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CHARGECAP_ID, ChargeCapId);
                dbManager.AddParameters(1, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_DELETE).ToString();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ChargeCapId.ToString(), ChargeCapId.ToString());
                    dsDBAudit.AcceptChanges();
                }


                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::DeletePatientCharges", PROC_PATIENT_CHARGES_DELETE, ex);
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


        public DSCharge UpdatePatientCharges(DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientCharges.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCharge)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_UPDATE, ds, ds.PatientCharges.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientCharges.Rows[0][ds.PatientCharges.ChargeCapIdColumn].ToString());
                    // dbManager.CommitTransaction();
                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::UpdatePatientCharges", PROC_PATIENT_CHARGES_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCharge UpdatePatientCharges(DSCharge ds, IDBManager dbManager, DSCharge dstemp = null)
        {
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                // DataTable dtTemps = ds.PatientCharges.GetChanges();
                //  DSCharge dsTemp = LoadPatientCharges(0, null, null, 0, 0, null, 0, null, null, null, null, Convert.ToInt64(ds.PatientCharges.Rows[0][ds.PatientCharges.VisitIdColumn].ToString()));
                //if (dstemp == null)
                //{
                //    dstemp = LoadPatientCharges(0, null, null, 0, 0, null, 0, null, null, null, null, Convert.ToInt64(ds.PatientCharges.Rows[0][ds.PatientCharges.VisitIdColumn].ToString()));
                //}
                DataTable dtTemp = ds.PatientCharges.GetChanges();
                // DataTable dtTemp = dstemp.PatientCharges;
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCharge)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_UPDATE, ds, ds.PatientCharges.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientCharges.Rows[0][ds.PatientCharges.ChargeCapIdColumn].ToString());
                    // dbManager.CommitTransaction();
                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::UpdatePatientCharges", PROC_PATIENT_CHARGES_UPDATE, ex);
                throw ex;
            }

        }
        public DSCharge InsertUpdatePatientCharges(DSCharge ds, IDBManager dbManager)
        {
            // IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {          
                //  dbManager.Open();
              //  DSDBAudit dsDBAudit = new DSDBAudit();
               // DataTable dtTemp = ds.PatientCharges.GetChanges();
                CreateInsertParameters(dbManager, ds);
                CreateUpdateParameters(dbManager, ds);
                ds = (DSCharge)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_INSERT, PROC_PATIENT_CHARGES_UPDATE, ds, ds.PatientCharges.TableName);
                ds.AcceptChanges();


                //if (dtTemp != null)
                //{
                //    //to handle multiple line item insertion issue with same chargecapid 
                //    string ChargeCapIds = string.Join(",", ds.PatientCharges.Select(p => p.ChargeCapId.ToString()));
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ChargeCapIds);
                //    dsDBAudit.AcceptChanges();

                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InsertUpdatePatientCharges", PROC_PATIENT_CHARGES_INSERT, ex);
               // ex.Message = ex.ToString().Split('|')[0];
                var arr = ex.Message.ToString().Split('|');
                if (arr.Length > 0) {
                    throw new Exception(ex.Message.ToString().Split('|')[0]);
                }
                else
                {
                    throw ex;
                }
                
            }

        }

        public DSCharge InsertUpdatePatientChargesAuditlog(DataTable dtTemp, DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.Open();

                if (dtTemp != null)
                {
                    //to handle multiple line item insertion issue with same chargecapid
                    var _capIds2 = new System.Collections.Generic.List<string>();
                    foreach (System.Data.DataRow _r in ds.PatientCharges.Rows) _capIds2.Add(_r["ChargeCapId"].ToString());
                    string ChargeCapIds = string.Join(",", _capIds2);
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ChargeCapIds);
                    dsDBAudit.AcceptChanges();

                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InsertUpdatePatientChargesAuditlog", "DBAuditChanges", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCharge LoadPatientCharges(long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, string ChargeStatus, long PatientInsuranceId, string ClaimNumber, string Paid, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int IsPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, int? IsActive = null, bool IncludeVoidedClaims = true)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (LastName == "")
                    LastName = null;
                if (FirstName == "")
                    FirstName = null;
                if (ChargeStatus == "")
                    ChargeStatus = null;
                if (PatientAccount == "")
                    PatientAccount = null;

                if (ClaimType == "")
                    ClaimType = null;


                dbManager.Open();
                dbManager.CreateParameters(24);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, ChargeCapId);

                dbManager.AddParameters(1, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(5, PARM_STATUS_ID, ChargeStatus);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                if (ClaimNumber == "")
                    ClaimNumber = null;
                dbManager.AddParameters(9, PARM_CLAIMED_NUMBER, ClaimNumber);

                if (Paid == "")
                    dbManager.AddParameters(10, PARM_PAID, null);
                else
                    dbManager.AddParameters(10, PARM_PAID, Paid);



                if (visitId == 0)
                    dbManager.AddParameters(11, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(11, PARM_VISIT_ID, visitId);

                dbManager.AddParameters(12, PARM_IS_PAYMENT, IsPayment);
                dbManager.AddParameters(13, PARM_ACCOUNT_NUMBER, PatientAccount);
                if (_837Batchid == 0)
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, _837Batchid);

                if (PageNumber == 0)
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(17, PARM_RECORD_COUNT, ds.PatientCharges.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (CPTCode == "")
                    CPTCode = null;

                dbManager.AddParameters(18, PARM_CPT_CODE, CPTCode);

                dbManager.AddParameters(19, PARM_IS_PRIMARY, ClaimType);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(20, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(20, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(21, PARM_USER_ID, MDVSession.Current.AppUserId);


                dbManager.AddParameters(22, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(23, PARM_INCLUDE_VOIDED_CLAIMS, IncludeVoidedClaims);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_SELECT, ds, ds.PatientCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadPatientCharges", PROC_PATIENT_CHARGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge LoadPaymentCharges(string caseMgtId, long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, string ChargeStatus, long PatientInsuranceId, string ClaimNumber, bool IncludeSecondaryClaim, string Paid, string ClaimErroredId,bool isVoidedClaim, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int IsPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string PatientFullName = null)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (LastName == "")
                    LastName = null;
                if (FirstName == "")
                    FirstName = null;
                if (ChargeStatus == "")
                    ChargeStatus = null;
                if (PatientAccount == "")
                    PatientAccount = null;

                if (ClaimType == "")
                    ClaimType = null;
                if (PatientFullName == "")
                    PatientFullName = null;

                dbManager.Open();
                dbManager.CreateParameters(26);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, ChargeCapId);

                dbManager.AddParameters(1, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(5, PARM_STATUS_ID, ChargeStatus);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                if (ClaimNumber == "")
                    ClaimNumber = null;

                dbManager.AddParameters(9, PARM_CLAIMED_NUMBER, ClaimNumber);
               // dbManager.AddParameters(10, PARM_INCLUDE_SECONDARY_CLAIM, IncludeSecondaryClaim);

                if (Paid == "")
                    dbManager.AddParameters(10, PARM_PAID, null);
                else
                    dbManager.AddParameters(10, PARM_PAID, Paid);

                if (visitId == 0)
                    dbManager.AddParameters(11, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(11, PARM_VISIT_ID, visitId);

                dbManager.AddParameters(12, PARM_IS_PAYMENT, IsPayment);
                dbManager.AddParameters(13, PARM_ACCOUNT_NUMBER, PatientAccount);
                if (_837Batchid == 0)
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, _837Batchid);

                if (PageNumber == 0)
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(17, PARM_RECORD_COUNT, ds.PatientCharges.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(18, PARM_CPT_CODE, CPTCode);

                dbManager.AddParameters(19, PARM_IS_PRIMARY, ClaimType);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(20, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(20, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (caseMgtId == "")
                    dbManager.AddParameters(21, PARM_CASE_MGMT_ID, null);
                else
                    dbManager.AddParameters(21, PARM_CASE_MGMT_ID, caseMgtId);



                dbManager.AddParameters(22, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (string.IsNullOrEmpty(ClaimErroredId))
                    dbManager.AddParameters(23, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(23, PARM_CLAIM_ERROR_ID, ClaimErroredId);
                dbManager.AddParameters(24, PARM_PATIENT_NAME, PatientFullName);
                dbManager.AddParameters(25, PARM_ISVOIDED_CLAIM, isVoidedClaim);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENT_CHARGES_SELECT, ds, ds.PatientCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadPaymentCharges", PROC_PAYMENT_CHARGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge LoadPatientVisitCharges(long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, long ResourceProviderId, string ChargeStatus, long PatientInsuranceId, string ClaimNumber, string Paid, string Hold, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int IsPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string ClaimErroredId = null, int? SubmitStatusId = null, string ClaimCreatedBy = null, DateTime? ClaimCreatedFrom = null, DateTime? ClaimCreatedTo = null, DateTime? EncounterSignOffDate = null, DateTime? ImportedDateFrom = null, DateTime? ImportedDateTo = null, DateTime? SubmittedFrom = null, DateTime? SubmittedTo = null, bool IncludeSecondaryClaim = false, bool IncludeVoidedClaims = false)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (LastName == "")
                    LastName = null;
                if (FirstName == "")
                    FirstName = null;
                if (ChargeStatus == "")
                    ChargeStatus = null;
                if (PatientAccount == "")
                    PatientAccount = null;


                if (ClaimType == "")
                    ClaimType = null;

                dbManager.Open();
                dbManager.CreateParameters(35);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, ChargeCapId);

                dbManager.AddParameters(1, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(5, PARM_STATUS_ID, ChargeStatus);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                if (ClaimNumber == "")
                    ClaimNumber = null;
                dbManager.AddParameters(9, PARM_CLAIMED_NUMBER, ClaimNumber);

                if (Paid == "")
                    dbManager.AddParameters(10, PARM_PAID, null);
                else
                    dbManager.AddParameters(10, PARM_PAID, Paid);



                if (visitId == 0)
                    dbManager.AddParameters(11, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(11, PARM_VISIT_ID, visitId);

                dbManager.AddParameters(12, PARM_IS_PAYMENT, IsPayment);
                dbManager.AddParameters(13, PARM_ACCOUNT_NUMBER, PatientAccount);
                if (_837Batchid == 0)
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, _837Batchid);

                if (PageNumber == 0)
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(17, PARM_RECORD_COUNT, ds.VisitCharges.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(18, PARM_CPT_CODE, CPTCode);


                dbManager.AddParameters(19, PARM_IS_PRIMARY, ClaimType);
                if (SubmitStatusId == 0)
                    dbManager.AddParameters(20, PARM_SUBMIT_STAUT_ID, null);
                else
                    dbManager.AddParameters(20, PARM_SUBMIT_STAUT_ID, SubmitStatusId);

                if (ClaimCreatedBy == "")
                    dbManager.AddParameters(21, PARM_CLAIM_CREATED_BY, null);
                else
                    dbManager.AddParameters(21, PARM_CLAIM_CREATED_BY, ClaimCreatedBy);

                dbManager.AddParameters(22, PARM_CLAIM_CREATED_FROM, ClaimCreatedFrom);
                dbManager.AddParameters(23, PARM_CLAIM_CREATED_TO, ClaimCreatedTo);

                dbManager.AddParameters(24, PARM_ENCOUNTER_SIGNOFF_DATE, EncounterSignOffDate);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(25, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(25, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                dbManager.AddParameters(26, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ResourceProviderId == 0)
                    dbManager.AddParameters(27, PARM_RESOURCE_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(27, PARM_RESOURCE_PROVIDER_ID, ResourceProviderId);

                dbManager.AddParameters(28, PARM_IMPORTED_DATE_FROM, ImportedDateFrom);
                dbManager.AddParameters(29, PARM_IMPORTED_DATE_TO, ImportedDateTo);
                //dbManager.AddParameters(30, PARM_INCLUDE_SECONDARY_CLAIM, IncludeSecondaryClaim);
                dbManager.AddParameters(30, PARM_IS_HOLD, Hold);
                dbManager.AddParameters(31, PARM_SUBMITTED_DATE_FROM, SubmittedFrom);
                dbManager.AddParameters(32, PARM_SUBMITTED_DATE_TO, SubmittedTo);
                dbManager.AddParameters(33, PARM_INCLUDE_VOIDED_CLAIMS, IncludeVoidedClaims);

                if (ClaimErroredId == "")
                    dbManager.AddParameters(34, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(34, PARM_CLAIM_ERROR_ID, ClaimErroredId);



                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_CHARGES_SELECT, ds, ds.VisitCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadPatientVisitCharges", PROC_PATIENT_VISIT_CHARGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge PatientVisitChargesSearch(long ChargeCapId, string LastName, string FirstName, long FacilityId, long ProviderId, long ResourceProviderId, string ChargeStatus, long PatientInsuranceId, string ClaimNumber, string Paid, string Hold, DateTime? DOSFrom = null, DateTime? DOSTo = null, long visitId = 0, int IsPayment = 0, string PatientAccount = null, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, string CPTCode = null, string ClaimType = null, string ClaimErroredId = null, int? SubmitStatusId = null, string ClaimCreatedBy = null, DateTime? ClaimCreatedFrom = null, DateTime? ClaimCreatedTo = null, DateTime? EncounterSignOffDate = null, DateTime? ImportedDateFrom = null, DateTime? ImportedDateTo = null, DateTime? SubmittedFrom = null, DateTime? SubmittedTo = null, bool IncludeSecondaryClaim = false, bool IncludeVoidedClaims = false, long? EncounterTypeId = null)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (LastName == "")
                    LastName = null;
                if (FirstName == "")
                    FirstName = null;
                if (ChargeStatus == "")
                    ChargeStatus = null;
                if (PatientAccount == "")
                    PatientAccount = null;


                if (ClaimType == "")
                    ClaimType = null;

                dbManager.Open();
                dbManager.CreateParameters(36);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CHARGECAP_ID, ChargeCapId);

                dbManager.AddParameters(1, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(5, PARM_STATUS_ID, ChargeStatus);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                if (ClaimNumber == "")
                    ClaimNumber = null;
                dbManager.AddParameters(9, PARM_CLAIMED_NUMBER, ClaimNumber);

                if (Paid == "")
                    dbManager.AddParameters(10, PARM_PAID, null);
                else
                    dbManager.AddParameters(10, PARM_PAID, Paid);



                if (visitId == 0)
                    dbManager.AddParameters(11, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(11, PARM_VISIT_ID, visitId);

                dbManager.AddParameters(12, PARM_IS_PAYMENT, IsPayment);
                dbManager.AddParameters(13, PARM_ACCOUNT_NUMBER, PatientAccount);
                if (_837Batchid == 0)
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(14, PARM_837_BATCH_ID, _837Batchid);

                if (PageNumber == 0)
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(15, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(16, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(17, PARM_RECORD_COUNT, ds.VisitCharges.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(18, PARM_CPT_CODE, CPTCode);


                dbManager.AddParameters(19, PARM_CLAIM_TYPE, ClaimType);
                if (SubmitStatusId == 0)
                    dbManager.AddParameters(20, PARM_SUBMIT_STAUT_ID, null);
                else
                    dbManager.AddParameters(20, PARM_SUBMIT_STAUT_ID, SubmitStatusId);

                if (ClaimCreatedBy == "")
                    dbManager.AddParameters(21, PARM_CLAIM_CREATED_BY, null);
                else
                    dbManager.AddParameters(21, PARM_CLAIM_CREATED_BY, ClaimCreatedBy);

                dbManager.AddParameters(22, PARM_CLAIM_CREATED_FROM, ClaimCreatedFrom);
                dbManager.AddParameters(23, PARM_CLAIM_CREATED_TO, ClaimCreatedTo);

                dbManager.AddParameters(24, PARM_ENCOUNTER_SIGNOFF_DATE, EncounterSignOffDate);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(25, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(25, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                dbManager.AddParameters(26, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ResourceProviderId == 0)
                    dbManager.AddParameters(27, PARM_RESOURCE_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(27, PARM_RESOURCE_PROVIDER_ID, ResourceProviderId);

                dbManager.AddParameters(28, PARM_IMPORTED_DATE_FROM, ImportedDateFrom);
                dbManager.AddParameters(29, PARM_IMPORTED_DATE_TO, ImportedDateTo);
              //  dbManager.AddParameters(30, PARM_INCLUDE_SECONDARY_CLAIM, IncludeSecondaryClaim);
                dbManager.AddParameters(30, PARM_IS_HOLD, Hold);
                dbManager.AddParameters(31, PARM_SUBMITTED_DATE_FROM, SubmittedFrom);
                dbManager.AddParameters(32, PARM_SUBMITTED_DATE_TO, SubmittedTo);
                dbManager.AddParameters(33, PARM_INCLUDE_VOIDED_CLAIMS, IncludeVoidedClaims);

                if (ClaimErroredId == "")
                    dbManager.AddParameters(34, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(34, PARM_CLAIM_ERROR_ID, ClaimErroredId);

                if (EncounterTypeId == 0)
                    dbManager.AddParameters(35, PARM_ENCOUNTER_TYPE_ID, null);
                else
                    dbManager.AddParameters(35, PARM_ENCOUNTER_TYPE_ID, EncounterTypeId);

                

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_CHARGES_SEARCH, ds, ds.VisitCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadPatientVisitCharges", PROC_PATIENT_VISIT_CHARGES_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSNotes LookupEncounterType()
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHARGE_ENCOUNTER_TYPE_LOOKUP, ds, ds.EncounterTypeLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LookupEncounterType", PROC_CHARGE_ENCOUNTER_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge LoadPatientTransferCharges(long VisitID)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (VisitID == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitID);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_TRANSFER_CHARGES_SELECT, ds, ds.PatientCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadPatientTransferCharges", PROC_PATIENT_TRANSFER_CHARGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge LookupBatches(string Active)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;


                dbManager.Open();
                dbManager.CreateParameters(2);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, Active);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BATCH_LOOKUPS, ds, ds.BactheLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharges::LookupProvider", PROC_BATCH_LOOKUPS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCharge BillToPatientCharges(string ChargesIds, bool BillToPatient)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DateTime TotayDate = DateTime.Now;
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_CHARGECAP_ID, ChargesIds);
                dbManager.AddParameters(1, PARM_BILL_TO_PATIENT, BillToPatient);
                dbManager.AddParameters(2, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_CREATED_ON, TotayDate);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(5, PARM_MODIFIED_ON, TotayDate);
                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILL_TO_PATIENT_CHARGES, ds, ds.PatientAndInsuranceCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::BillToPatientCharges", PROC_BILL_TO_PATIENT_CHARGES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region UnClaimed Appointments


        public DSCharge LoadUnClaimedAppointments(string LastName, string FirstName, long FacilityId, long ProviderId, string Claimed, long InsurancePlanID, string Claimno, DateTime? DOSFrom = null, DateTime? DOSTo = null, string PatientAccount = null, int? SubmitStatusId = null, int PageNumber = 1, int RowspPage = 1000)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (LastName == "")
                    LastName = null;
                if (FirstName == "")
                    FirstName = null;
                if (Claimed == "")
                    Claimed = null;
                if (PatientAccount == "")
                    PatientAccount = null;
                if (Claimno == "")
                    Claimno = null;

                //string ClaimStatus = null;

                dbManager.Open();
                dbManager.CreateParameters(16);


                dbManager.AddParameters(0, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);


                dbManager.AddParameters(4, PARM_CHECKIN, Claimed);

                if (InsurancePlanID == 0)
                    dbManager.AddParameters(5, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(5, PARM_INSURANCE_PLAN_ID, InsurancePlanID);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);
                dbManager.AddParameters(8, PARM_ACCOUNT_NUMBER, PatientAccount);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (SubmitStatusId == 0)
                    dbManager.AddParameters(13, PARM_SUBMIT_STAUT_ID, null);
                else
                    dbManager.AddParameters(13, PARM_SUBMIT_STAUT_ID, SubmitStatusId);

                //dbManager.AddParameters(14, PARM_CLAIM_STATUS, ClaimStatus);
                dbManager.AddParameters(14, PARM_CLAIMED_NUMBER, Claimno);
                dbManager.AddParameters(15, PARM_RECORD_COUNT, ds.UnClaimedAppointments.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_UNCLAIMED_APP_SELECT, ds, ds.UnClaimedAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadUnClaimedAppointments", PROC_UNCLAIMED_APP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge UnClaimedAppointmentsSelect(string LastName, string FirstName, long FacilityId, long ProviderId, string Claimed, long InsurancePlanID, string Claimno, DateTime? DOSFrom = null, DateTime? DOSTo = null, string PatientAccount = null, int? SubmitStatusId = null, int? AppointmentStatusId = null, int PageNumber = 1, int RowspPage = 1000)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (LastName == "")
                    LastName = null;
                if (FirstName == "")
                    FirstName = null;
                if (Claimed == "")
                    Claimed = null;
                if (PatientAccount == "")
                    PatientAccount = null;
                if (Claimno == "")
                    Claimno = null;

                //string ClaimStatus = null;

                dbManager.Open();
                dbManager.CreateParameters(17);


                dbManager.AddParameters(0, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);


                dbManager.AddParameters(4, PARM_CHECKIN, Claimed);

                if (InsurancePlanID == 0)
                    dbManager.AddParameters(5, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(5, PARM_INSURANCE_PLAN_ID, InsurancePlanID);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);
                dbManager.AddParameters(8, PARM_ACCOUNT_NUMBER, PatientAccount);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (SubmitStatusId == 0)
                    dbManager.AddParameters(13, PARM_SUBMIT_STAUT_ID, null);
                else
                    dbManager.AddParameters(13, PARM_SUBMIT_STAUT_ID, SubmitStatusId);

                if (AppointmentStatusId == 0)
                    dbManager.AddParameters(14, PARM_APPT_STAUT_ID, null);
                else
                    dbManager.AddParameters(14, PARM_APPT_STAUT_ID, AppointmentStatusId);

                //dbManager.AddParameters(14, PARM_CLAIM_STATUS, ClaimStatus);
                dbManager.AddParameters(15, PARM_CLAIMED_NUMBER, Claimno);
                dbManager.AddParameters(16, PARM_RECORD_COUNT, ds.UnClaimedAppointments.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_UNCLAIMED_APP_SELECT_NEW, ds, ds.UnClaimedAppointments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::UnClaimedAppointmentsSelect", PROC_UNCLAIMED_APP_SELECT_NEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge LookupAppVisitStatus()
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPVISIT_STATUS_LOOKUP, ds, ds.VisitStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LookupAppVisitStatus", PROC_APPVISIT_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region " Charges ICD Pointers  "

        public DSCharge InsertUpdateChargesPointers(DSCharge ds, IDBManager dbManager)
        {
            try
            {
                CreateICDPointerInsertParameters(dbManager, ds);
                CreateICDPointerUpdateParameters(dbManager, ds);
                ds = (DSCharge)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT, PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE, ds, ds.ChargesICDPointers.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InsertUpdateChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT + PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE, ex);
                throw ex;
            }

        }

        public DSCharge InsertChargesPointers(DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateICDPointerParameters(dbManager, ds, true);
                ds = (DSCharge)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT, ds, ds.ChargesICDPointers.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InsertChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge InsertChargesPointers(DSCharge ds, IDBManager dbManager)
        {
            try
            {
                CreateICDPointerParameters(dbManager, ds, true);
                ds = (DSCharge)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT, ds, ds.ChargesICDPointers.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::InsertChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_INSERT, ex);
                throw ex;
            }
        }

        public DSCharge UpdateChargesPointers(DSCharge ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateICDPointerParameters(dbManager, ds, false);
                ds = (DSCharge)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE, ds, ds.ChargesICDPointers.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::UpdateChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCharge UpdateChargesPointers(DSCharge ds, IDBManager dbManager)
        {
            try
            {
                this.CreateICDPointerParameters(dbManager, ds, false);
                ds = (DSCharge)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE, ds, ds.ChargesICDPointers.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::UpdateChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_UPDATE, ex);
                throw ex;
            }
        }

        public string DeleteChargesPointers(long ICDPointerId, long ChargeCapId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();

                dbManager.CreateParameters(2);

                if (ICDPointerId == 0)
                    dbManager.AddParameters(0, PARM_ICD_POINTER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ICD_POINTER_ID, ICDPointerId);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(1, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CHARGECAP_ID, ChargeCapId);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::DeleteChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_DELETE, ex);
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

        public string DeleteChargesPointers(long ICDPointerId, long ChargeCapId, IDBManager dbManager)
        {
            string returnVal = "";
            try
            {
                dbManager.CreateParameters(2);
                if (ICDPointerId == 0)
                    dbManager.AddParameters(0, PARM_ICD_POINTER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ICD_POINTER_ID, ICDPointerId);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(1, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CHARGECAP_ID, ChargeCapId);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::DeleteChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        public DSCharge LoadChargesPointers(long ICDPointerId,long ChargeCapId,long VisitId)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (ICDPointerId == 0)
                    dbManager.AddParameters(0, PARM_ICD_POINTER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ICD_POINTER_ID, ICDPointerId);

                if (ChargeCapId == 0)
                    dbManager.AddParameters(1, PARM_CHARGECAP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CHARGECAP_ID, ChargeCapId);

                if (VisitId == 0)
                    dbManager.AddParameters(2, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_VISIT_ID, VisitId);

                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_CHARGES_ICD_POINTERS_SELECT, ds, ds.ChargesICDPointers.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCharge::LoadChargesPointers", PROC_PATIENT_CHARGES_ICD_POINTERS_SELECT, ex);
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
