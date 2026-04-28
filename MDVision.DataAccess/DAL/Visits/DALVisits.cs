using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.Model.Billing.ERA;

namespace MDVision.DataAccess.DAL.Visits
{
    public class DALVisits
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PATIENT_VISITS_SELECT = "Patient.sp_PatientVisitsSelect";
        private const string PROC_PATIENT_VISITS_SEARCH = "Patient.sp_PatientVisitsSearch";
        private const string PROC_PATIENT_VISITS_DELETE = "Patient.sp_PatientVisitsDelete";
        private const string PROC_PATIENT_VISITS_INSERT = "Patient.sp_PatientVisitsInsert";
        private const string PROC_PATIENT_VISITS_UPDATE = "Patient.sp_PatientVisitsUpdate";
        private const string PROC_PATIENT_VISITS_LOOKUP = "Patient.sp_PatientVisitsLookup";
        private const string PROC_VISIT_STATUS_LOOKUP = "Patient.sp_VisitStatusLookup";
        private const string PROC_DELAY_REASON_LOOKUP = "Patient.sp_DelayReasonLookup";
        private const string PROC_CLAIM_FREQUENCY_LOOKUP = "Patient.sp_ClaimFrequencyLookup";
        private const string PROC_PATIENT_VISIT_CHARGE_STATU_UPDATE = "Patient.sp_VistAndChargeStatusUpdate";
        private const string PROC_PATIENT_VISIT_CHARGE_STATURESUBMIT_UPDATE = "Patient.sp_VistAndChargeStatusResubmitUpdate";
        private const string PROC_VISIT_VOID_RECREATE = "Patient.sp_VoidAndReCreateClaim";
        private const string PROC_CLAIM_VOID_INFO_SELECT = "Patient.sp_ClaimVoidInfoSelect";
        private const string PROC_PATIENT_VISITS_DETAIL_SELECT = "Patient.sp_PatientVisitsDetailSelect";
        private const string PROC_CLAIM_PAYMENTS_SELECT = "Patient.sp_ClaimPaymentsSelect";
        private const string PROC_CLAIM_ERRORS_DELETE = "Patient.sp_ClaimErrorsDelete";
        private const string PROC_CLAIM_ERRORS_INSERT = "Patient.sp_ClaimErrorsInsert";
        private const string PROC_CLAIM_ERRORS_SELECT = "Patient.sp_ClaimErrorsSelect";
        private const string PROC_CLAIM_ERRORS_UPDATE = "Patient.sp_ClaimErrorsUpdate";
        private const string PROC_PATIENT_CLAIM_HISTORY = "Patient.sp_PrintClaimHistory";
        private const string PROC_MULTIPLE_NOTE_COMMENTS_INSERT = "Patient.sp_MultipleNoteCommentsInsert";
        private const string PROC_VERIFY_DUPLICATE_CLAIM = "Patient.sp_VerifyDuplicateClaim";


        private const string PROC_REPORT_TYPE_CODE_LOOKUP = "System.sp_ReportTypeCodeLookup";
        private const string PROC_TRANSMISSION_CODE_LOOKUP = "System.sp_TransmissionCodeLookup";

        private const string PROC_VISIT_PLAN_LOOKUP = "Patient.sp_PatientVisitPlanLookup";

        private const string PROC_CPT_FEE_SELECT = "Billing.sp_CalculateCPTFee";
        private const string PROC_PATIENT_OUTSTANDING_VISITS_SELECT = "Patient.sp_PatientOutstandingVisitsSelect";
        private const string PROC_SUBMIT_STATUS_LOOKUP = "Patient.sp_SubmitStatusLookup";
        private const string PROC_VOIDED_STATUS_LOOKUP = "System.VoidedStatuslookup";
        private const string PROC_PATIENT_VISIT_ICD_INSERT = "Patient.sp_PatientVisitICDInsert";
        private const string PROC_PATIENT_VISIT_ICD_UPDATE = "Patient.sp_PatientVisitICDUpdate";
        private const string PROC_PATIENT_VISIT_ICD_SELECT = "Patient.sp_PatientVisitICDSelect";
        private const string PROC_PATIENT_VISIT_ICD_DELETE = "Patient.sp_PatientVisitICDDelete";
        private const string PROC_PATIENT_VISIT_ICD_BY_ID_DELETE = "Patient.sp_PatientVisitICDDeleteByPVICDId";
        private const string PROC_PATIENT_VISITS_SUBMIT_STATUS = "Patient.sp_UpdatePatientVisitsSubmitStatus";

        private const string PROC_ANES_ANESTHESIATYPE_LOOKUP = "Billing.sp_AnesthesiaTypeLookup";
        private const string PROC_ANES_SERVICETYPE_LOOKUP = "Billing.sp_AnesServiceTypeLookup";
        private const string PROC_ANES_ASA_LOOKUP = "Billing.sp_ASALookup";
        private const string PROC_CLAIM_SCRUB_HISTORY_INSERT = "Patient.sp_ClaimScrubHistoryInsert";
        private const string PROC_GET_EDI_DETAIL = "billing.sp_getEDIDetail";
        private const string PROC_GET_CLAIM_STATUS = "Billing.sp_getClaimStatus";

        #endregion

        #region "Parameters"
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_PROVIDER_ID = "@ProviderId";

        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_REF_PROVIDER_ID = "@RefProviderId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_APPOINTMENT_DATE = "@AppointmentDate";
        private const string PARM_CASE_MGMT_ID = "@CaseMgmtId";
        private const string PARM_BATCH_NUMBER = "@BatchNumber";
        private const string PARM_BATCH_ID = "@BatchId";
        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_REFERRAL_NUMBER = "@ReferralNumber";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_ADMISSION_DATE = "@AdmissionDate";
        private const string PARM_INJURY_DATE = "@InjuryDate";
        private const string PARM_ILLNESS_DATE = "@IllnessDate";
        private const string PARM_DB_AUDIT_ID = "@DBAuditId";

        private const string PARM_DISCHARGE_DATE = "@DischargeDate";
        private const string PARM_DELAY_REASON = "@DelayReason";
        private const string PARM_CLAIM_FREQUENCY = "@ClaimFrequency";
        private const string PARM_ICD_DCN = "@ICDDCN";
        private const string PARM_LMP_DATE = "@LMPDate";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_PRINT_HCFA = "@PrintHCFA";
        private const string PARM_ORDERING_PROVIDER = "@OrderingProvider";
        private const string PARM_SUPERVISING_PROVIDER = "@SupervisingProvider";
        private const string PARM_UA_WORK_FROM = "@UAWorkFrom";

        private const string PARM_UA_WORK_TO = "@UAWorkto";
        private const string PARM_VISIT_COPAYMENT = "@VisitCopayment";
        private const string PARM_IS_COPAY_PAID = "@IsCopayPaid";
        private const string PARM_SCH_REASON_ID = "@SchReasonId";
        private const string PARM_CLINICAL_TEMP_ID = "@ClinicalTempId";
        private const string PARM_IS_ACTIVE = "@IsActive";

        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_NOTE_MODIFIED_BY = "@NoteModifiedBy";
        private const string PARM_NOTE_MODIFIED_ON = "@NoteModifiedOn";
        private const string PARM_UDATE_COMMENTS_DATE = "@UpdateCommentsDate";

        private const string PARM_FROM_APPOINTMENT_DATE = "@FromAppointmentDate";
        private const string PARM_TO_APPOINTMENT_DATE = "@ToAppointmentDate";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_VISIT_STATUS_ID = "@VisitStatusId";

        private const string PARM_VISIT_STATUS = "@VisitStatus";
        private const string PARM_IS_VISIT_CHARGE = "@isVisitCharge";

        private const string PARM_SUBMITTED_BY = "@SubmittedBy";
        private const string PARM_SUBMITTED_DATE = "@SubmittedDate";
        private const string PARM_ASSIGN_BENEFITS = "@AssignBenefits";
        private const string PARM_AUTO_ACCIDENT = "@AutoAccident";
        private const string PARM_STATE = "@State";
        private const string PARM_OTHER_ACCIDENT = "@OtherAccident";
        private const string PARM_IS_EMPLOYED = "@IsEmployed";

        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_837_BATCH_ID = "@837BatchId";
        private const string PARM_CLAIM_STATUS_ID = "@ClaimStatusId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_IS_CHECKOUT = "@IsCheckout";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_SUBMIT_STATUS_ID = "@SubmitStatusId";
        private const string PARM_MASTER_VISIT_ID = "@MasterVisitId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_STATUS_ID = "@StatusId";

        private const string PARM_Patient_Outstanding = "@PatientOutstanding";

        private const string PARM_REPORT_TYPE_CODE_ID = "@ReportTypeCodeId";
        private const string PARM_TRANSMISSION_CODE_ID = "@TransmissionCodeId";
        private const string PARM_CONTROL_NUMBER = "@ControlNumber";
        private const string PARM_DOCUMENT_SENT_DATE = "@DocumentSentDate";
        private const string PARM_LAST_SEEN_DATE = "@LastSeenDate";
        private const string PARM_RESOURCE_PROVIDER_ID = "@ResourceProviderId";
        private const string PARM_BILLING_PROVIDER_ID = "@BillingProviderId";
        private const string PARM_CLAIM_TYPE_ID = "@ClaimTypeId";
        private const string PARM_CLAIM_COMMENTS = "@ClaimComments";
        private const string PARM_IS_ELECTRONIC_SUBMIT = "@IsElectronicSubmit";
        private const string PARM_CHARGE_STATUS_ID = "@ChargeStatusId";
        private const string PARM_VISIT_IDS = "@VisitIds";
        private const string PARM_READY_TO_SUBMIT_ON = "@ReadyToSubmitOn";
        private const string PARM_ISSPECIALIST = "@IsSpecialist";
        private const string PARM_BILL_TO_PATIENT = "@BillToPatient";

        private const string PARM_LOCKED_ON = "@LockedOn";
        private const string PARM_IS_LOCKED = "@IsLocked";
        private const string PARM_IS_VNC = "@IsVNC";
        private const string PARM_VNC_VISIT_ID = "@VNCVisitId";
        private const string PARM_PATIENT_REFERRAL_ID = "@PatientReferralId";
        private const string PARM_IS_SPLITTED = "@IsSplitted";
        private const string PARM_SPLITTED_VISIT_ID = "@SplittedVisitId";
        private const string PARM_ISCLEANCLAIM = "@IsCleanClaim";

        private const string PARM_CLAIM_ERROR_ID = "@ErrorId";
        private const string PARM_CLAIM_ERROR_CODE = "@ErrorCode";
        private const string PARM_CLAIM_ERROR_DESCRIPTION = "@Description";
        private const string PARM_CLAIM_ERROR_ACTION = "@Action";
        private const string PARM_CLAIM_ERROR_JOB_NUMBER = "@JobNumber";
        private const string PARM_ICDCODE = "@ICDCode";
        private const string PARM_ICDCODE_DESCRIPTION = "@ICDCodeDescription";
        private const string PARM_HOLD_TILL = "@HoldTill";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_SNOME_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXICODE = "@LexiCode";
        private const string PARM_LEXICODE_DESCRIPTION = "@LexiCodeDescription";
        private const string PARM_PV_ICD_ID = "@PVICDId";
        private const string PARM_ICD_Type = "@ICDType";
        private const string PARM_PAN = "@PAN";
        private const string PARM_AUTHORIZE_ID = "@AuthorizeId";
        private const string PARM_ICD_INDEX = "@ICDIndex";
        private const string PARM_MODULE = "@Module";
        private const string PARM_IS_REPORT_NPI = "@IsReportNPI";

        private const string PARM_ANESTHESIOLOGIST_ID = "@AnesthesiologistId";
        private const string PARM_IS_ANES = "@IsAnes";
        private const string PARM_ANES_STARTTIME = "@AnesStartTime";
        private const string PARM_ANES_ENDTIME = "@AnesEndTime";
        private const string PARM_CRNA_ID = "@CRNAId";
        private const string PARM_CRNA_STARTTIME = "@CRNAStartTime";
        private const string PARM_CRNA_ENDTIME = "@CRNAEndTime";
        private const string PARM_ANES_TYPEID = "@AnesTypeId";
        private const string PARM_ASA_ID = "@ASAId";
        private const string PARM_ANES_SERVICETYPEID = "@AnesServiceTypeId";
        private const string PARM_ANES_COMMENTS = "@AnesComments";
        private const string PARM_RISK_UNITS = "@RiskUnits";

        private const string PARM_CLAIM_SCRUB_HISTORY_ID = "@ClaimScrubHistoryID";
        private const string PARM_EDI_TEXT = "@EDIText";
        private const string PARM_JOB_NUMBER = "@JobNumber";
        private const string PARM_CLAIM_SCRUB_VISIT_IDS = "@VisitID";
        private const string PARM_SERVICE_RESPONSE_TEXT = "@ServiceResponseText";

        private const string PARM_NOTE_COMMENTS = "@NoteComments";
        private const string PARM_REASON_COMMENTS = "@ReasonComments";


        private const string PARM_BOX24_IJ_SHADED = "@Box24IJShaded";
        private const string PARM_BOX24_B_SHADED = "@Box24BShaded";
        private const string PARM_IS_COLLECTIN = "@IsCollection";

        private const string PARM_SUBMIT_STATUS_USERID = "@SubmitStatusUserId";
        private const string PARM_EDITEXT = "@EDIText";
        private const string PARM_CLAIM_COMMENT_CHANGED = "@IsClaimCommentChanged";
        private const string PARM_IS_SELF_PAY = "@IsSelfPay";
        #endregion

        #region Constructors
        public DALVisits()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALVisits(SharedVariable SharedVariable)
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

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSVisits ds, Boolean IsInsert)
        {
            if (IsInsert == true)
            {
                dbManager.CreateParameters(86);
                dbManager.AddParameters(0, PARM_VISIT_ID, ds.PatientVisits.VisitIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(85);
                dbManager.AddParameters(0, PARM_VISIT_ID, ds.PatientVisits.VisitIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARM_APPOINTMENT_ID, ds.PatientVisits.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatientVisits.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.PatientVisits.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_FACILITY_ID, ds.PatientVisits.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_REF_PROVIDER_ID, ds.PatientVisits.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_PATIENT_INSURANCE_ID, ds.PatientVisits.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_APPOINTMENT_DATE, ds.PatientVisits.AppointmentDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_CASE_MGMT_ID, ds.PatientVisits.CaseMgmtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_BATCH_ID, ds.PatientVisits.BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_CLAIM_NUMBER, ds.PatientVisits.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_REFERRAL_NUMBER, ds.PatientVisits.ReferralNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_DOS_FROM, ds.PatientVisits.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_DOS_TO, ds.PatientVisits.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_ADMISSION_DATE, ds.PatientVisits.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_INJURY_DATE, ds.PatientVisits.InjuryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_ILLNESS_DATE, ds.PatientVisits.IllnessDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_DISCHARGE_DATE, ds.PatientVisits.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_DELAY_REASON, ds.PatientVisits.DelayReasonColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(19, PARM_CLAIM_FREQUENCY, ds.PatientVisits.ClaimFrequencyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ICD_DCN, ds.PatientVisits.ICDDCNColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_LMP_DATE, ds.PatientVisits.LMPDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_COMMENTS, ds.PatientVisits.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_PRINT_HCFA, ds.PatientVisits.PrintHCFAColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(24, PARM_ORDERING_PROVIDER, ds.PatientVisits.OrderingProviderColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(25, PARM_SUPERVISING_PROVIDER, ds.PatientVisits.SupervisingProviderColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_UA_WORK_FROM, ds.PatientVisits.UAWorkFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(27, PARM_UA_WORK_TO, ds.PatientVisits.UAWorktoColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(28, PARM_VISIT_COPAYMENT, ds.PatientVisits.VisitCopaymentColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(29, PARM_IS_COPAY_PAID, ds.PatientVisits.IsCopayPaidColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(30, PARM_SCH_REASON_ID, ds.PatientVisits.SchReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(31, PARM_CLINICAL_TEMP_ID, ds.PatientVisits.ClinicalTempIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(32, PARM_IS_ACTIVE, ds.PatientVisits.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, PARM_CREATED_BY, ds.PatientVisits.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_CREATED_ON, ds.PatientVisits.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_MODIFIED_BY, ds.PatientVisits.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_MODIFIED_ON, ds.PatientVisits.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(37, PARM_VISIT_STATUS, ds.PatientVisits.VisitStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_SUBMITTED_BY, ds.PatientVisits.SubmittedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(39, PARM_SUBMITTED_DATE, ds.PatientVisits.SubmittedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(40, PARM_ASSIGN_BENEFITS, ds.PatientVisits.AssignBenefitsColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(41, PARM_AUTO_ACCIDENT, ds.PatientVisits.AutoAccidentColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(42, PARM_STATE, ds.PatientVisits.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(43, PARM_OTHER_ACCIDENT, ds.PatientVisits.OtherAccidentColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(44, PARM_IS_EMPLOYED, ds.PatientVisits.IsEmployedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(45, PARM_CLAIM_STATUS_ID, ds.PatientVisits.ClaimStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(46, PARM_SUBMIT_STATUS_ID, ds.PatientVisits.SubmitStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(47, PARM_MASTER_VISIT_ID, ds.PatientVisits.MasterVisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(48, PARM_REPORT_TYPE_CODE_ID, ds.PatientVisits.ReportTypeCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(49, PARM_TRANSMISSION_CODE_ID, ds.PatientVisits.TransmissionCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(50, PARM_CONTROL_NUMBER, ds.PatientVisits.ControlNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(51, PARM_DOCUMENT_SENT_DATE, ds.PatientVisits.DocumentSentDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(52, PARM_LAST_SEEN_DATE, ds.PatientVisits.LastSeenDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(53, PARM_RESOURCE_PROVIDER_ID, ds.PatientVisits.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(54, PARM_BILLING_PROVIDER_ID, ds.PatientVisits.BillingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(55, PARM_CLAIM_TYPE_ID, ds.PatientVisits.ClaimTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(56, PARM_CLAIM_COMMENTS, ds.PatientVisits.ClaimCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(57, PARM_IS_ELECTRONIC_SUBMIT, ds.PatientVisits.IsElectronicSubmitColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(58, PARM_READY_TO_SUBMIT_ON, ds.PatientVisits.ReadyToSubmitOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(59, PARM_ISSPECIALIST, ds.PatientVisits.IsSpecialistColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(60, PARM_BILL_TO_PATIENT, ds.PatientVisits.BillToPatientColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(61, PARM_PATIENT_REFERRAL_ID, ds.PatientVisits.PatientReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(62, PARM_IS_SPLITTED, ds.PatientVisits.IsSplittedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(63, PARM_SPLITTED_VISIT_ID, ds.PatientVisits.SplittedVisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(64, PARM_HOLD_TILL, ds.PatientVisits.HoldTillColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(65, PARM_PAN, ds.PatientVisits.PANColumn.ColumnName, DbType.String);
            dbManager.AddParameters(66, PARM_AUTHORIZE_ID, ds.PatientVisits.AuthorizeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(67, PARM_IS_REPORT_NPI, ds.PatientVisits.IsReportNPIColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(68, PARM_ISCLEANCLAIM, ds.PatientVisits.IsCleanClaimColumn.ColumnName, DbType.Boolean);

            dbManager.AddParameters(69, PARM_ANESTHESIOLOGIST_ID, ds.PatientVisits.AnesthesiologistIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(70, PARM_IS_ANES, ds.PatientVisits.IsAnesColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(71, PARM_ANES_STARTTIME, ds.PatientVisits.AnesStartTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(72, PARM_ANES_ENDTIME, ds.PatientVisits.AnesEndTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(73, PARM_CRNA_ID, ds.PatientVisits.CRNAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(74, PARM_CRNA_STARTTIME, ds.PatientVisits.CRNAStartTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(75, PARM_CRNA_ENDTIME, ds.PatientVisits.CRNAEndTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(76, PARM_ANES_TYPEID, ds.PatientVisits.AnesTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(77, PARM_ASA_ID, ds.PatientVisits.ASAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(78, PARM_ANES_SERVICETYPEID, ds.PatientVisits.AnesServiceTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(79, PARM_ANES_COMMENTS, ds.PatientVisits.AnesCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(80, PARM_RISK_UNITS, ds.PatientVisits.RiskUnitsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(81, PARM_NOTE_COMMENTS, ds.PatientVisits.NoteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(82, PARM_REASON_COMMENTS, ds.PatientVisits.ReasonCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(83, PARM_BOX24_IJ_SHADED, ds.PatientVisits.Box24IJShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(84, PARM_BOX24_B_SHADED, ds.PatientVisits.Box24BShadedColumn.ColumnName, DbType.Int64);
            if (IsInsert == true)
            {
                ds.Tables["PatientVisits"].Rows[0]["selfpay"] = string.IsNullOrEmpty(ds.Tables["PatientVisits"].Rows[0]["selfpay"].ToString()) == true ? "False" : ds.Tables["PatientVisits"].Rows[0]["selfpay"].ToString();
                dbManager.AddParameters(85, PARM_IS_SELF_PAY, ds.PatientVisits.selfpayColumn.ColumnName, DbType.Boolean);
            }
        }

        private void CreateInsertParameters(IDBManager dbManager, DSVisits ds)
        {
            dbManager.CreateInsertParameters(90);

            dbManager.AddInsertUpdateParameters(0, PARM_VISIT_ID, ds.PatientVisits.VisitIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_APPOINTMENT_ID, ds.PatientVisits.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PATIENT_ID, ds.PatientVisits.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_PROVIDER_ID, ds.PatientVisits.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_FACILITY_ID, ds.PatientVisits.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_REF_PROVIDER_ID, ds.PatientVisits.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_PATIENT_INSURANCE_ID, ds.PatientVisits.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(7, PARM_APPOINTMENT_DATE, ds.PatientVisits.AppointmentDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_CASE_MGMT_ID, ds.PatientVisits.CaseMgmtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_BATCH_ID, ds.PatientVisits.BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_CLAIM_NUMBER, ds.PatientVisits.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_REFERRAL_NUMBER, ds.PatientVisits.ReferralNumberColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_DOS_FROM, ds.PatientVisits.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(13, PARM_DOS_TO, ds.PatientVisits.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_ADMISSION_DATE, ds.PatientVisits.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_INJURY_DATE, ds.PatientVisits.InjuryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_ILLNESS_DATE, ds.PatientVisits.IllnessDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_DISCHARGE_DATE, ds.PatientVisits.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_DELAY_REASON, ds.PatientVisits.DelayReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_CLAIM_FREQUENCY, ds.PatientVisits.ClaimFrequencyColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_ICD_DCN, ds.PatientVisits.ICDDCNColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_LMP_DATE, ds.PatientVisits.LMPDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(22, PARM_COMMENTS, ds.PatientVisits.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_PRINT_HCFA, ds.PatientVisits.PrintHCFAColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(24, PARM_ORDERING_PROVIDER, ds.PatientVisits.OrderingProviderColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(25, PARM_SUPERVISING_PROVIDER, ds.PatientVisits.SupervisingProviderColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(26, PARM_UA_WORK_FROM, ds.PatientVisits.UAWorkFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(27, PARM_UA_WORK_TO, ds.PatientVisits.UAWorktoColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(28, PARM_VISIT_COPAYMENT, ds.PatientVisits.VisitCopaymentColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(29, PARM_IS_COPAY_PAID, ds.PatientVisits.IsCopayPaidColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(30, PARM_SCH_REASON_ID, ds.PatientVisits.SchReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(31, PARM_CLINICAL_TEMP_ID, ds.PatientVisits.ClinicalTempIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(32, PARM_IS_ACTIVE, ds.PatientVisits.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(33, PARM_CREATED_BY, ds.PatientVisits.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(34, PARM_CREATED_ON, ds.PatientVisits.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(35, PARM_MODIFIED_BY, ds.PatientVisits.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(36, PARM_MODIFIED_ON, ds.PatientVisits.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(37, PARM_VISIT_STATUS, ds.PatientVisits.VisitStatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(38, PARM_SUBMITTED_BY, ds.PatientVisits.SubmittedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(39, PARM_SUBMITTED_DATE, ds.PatientVisits.SubmittedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(40, PARM_ASSIGN_BENEFITS, ds.PatientVisits.AssignBenefitsColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(41, PARM_AUTO_ACCIDENT, ds.PatientVisits.AutoAccidentColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(42, PARM_STATE, ds.PatientVisits.StateColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(43, PARM_OTHER_ACCIDENT, ds.PatientVisits.OtherAccidentColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(44, PARM_IS_EMPLOYED, ds.PatientVisits.IsEmployedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(45, PARM_CLAIM_STATUS_ID, ds.PatientVisits.ClaimStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(46, PARM_SUBMIT_STATUS_ID, ds.PatientVisits.SubmitStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(47, PARM_MASTER_VISIT_ID, ds.PatientVisits.MasterVisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(48, PARM_REPORT_TYPE_CODE_ID, ds.PatientVisits.ReportTypeCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(49, PARM_TRANSMISSION_CODE_ID, ds.PatientVisits.TransmissionCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(50, PARM_CONTROL_NUMBER, ds.PatientVisits.ControlNumberColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(51, PARM_DOCUMENT_SENT_DATE, ds.PatientVisits.DocumentSentDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(52, PARM_LAST_SEEN_DATE, ds.PatientVisits.LastSeenDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(53, PARM_RESOURCE_PROVIDER_ID, ds.PatientVisits.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(54, PARM_BILLING_PROVIDER_ID, ds.PatientVisits.BillingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(55, PARM_CLAIM_TYPE_ID, ds.PatientVisits.ClaimTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(56, PARM_CLAIM_COMMENTS, ds.PatientVisits.ClaimCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(57, PARM_IS_ELECTRONIC_SUBMIT, ds.PatientVisits.IsElectronicSubmitColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(58, PARM_READY_TO_SUBMIT_ON, ds.PatientVisits.ReadyToSubmitOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(59, PARM_ISSPECIALIST, ds.PatientVisits.IsSpecialistColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(60, PARM_BILL_TO_PATIENT, ds.PatientVisits.BillToPatientColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(61, PARM_PATIENT_REFERRAL_ID, ds.PatientVisits.PatientReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(62, PARM_IS_SPLITTED, ds.PatientVisits.IsSplittedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(63, PARM_SPLITTED_VISIT_ID, ds.PatientVisits.SplittedVisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(64, PARM_HOLD_TILL, ds.PatientVisits.HoldTillColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(65, PARM_PAN, ds.PatientVisits.PANColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(66, PARM_AUTHORIZE_ID, ds.PatientVisits.AuthorizeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(67, PARM_IS_REPORT_NPI, ds.PatientVisits.IsReportNPIColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(68, PARM_ISCLEANCLAIM, ds.PatientVisits.IsCleanClaimColumn.ColumnName, DbType.Boolean);

            dbManager.AddInsertUpdateParameters(69, PARM_ANESTHESIOLOGIST_ID, ds.PatientVisits.AnesthesiologistIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(70, PARM_IS_ANES, ds.PatientVisits.IsAnesColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(71, PARM_ANES_STARTTIME, ds.PatientVisits.AnesStartTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(72, PARM_ANES_ENDTIME, ds.PatientVisits.AnesEndTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(73, PARM_CRNA_ID, ds.PatientVisits.CRNAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(74, PARM_CRNA_STARTTIME, ds.PatientVisits.CRNAStartTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(75, PARM_CRNA_ENDTIME, ds.PatientVisits.CRNAEndTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(76, PARM_ANES_TYPEID, ds.PatientVisits.AnesTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(77, PARM_ASA_ID, ds.PatientVisits.ASAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(78, PARM_ANES_SERVICETYPEID, ds.PatientVisits.AnesServiceTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(79, PARM_ANES_COMMENTS, ds.PatientVisits.AnesCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(80, PARM_RISK_UNITS, ds.PatientVisits.RiskUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(81, PARM_NOTE_COMMENTS, ds.PatientVisits.NoteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(82, PARM_REASON_COMMENTS, ds.PatientVisits.ReasonCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(83, PARM_NOTE_MODIFIED_BY, ds.PatientVisits.NoteModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(84, PARM_NOTE_MODIFIED_ON, ds.PatientVisits.NoteModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(85, PARM_BOX24_IJ_SHADED, ds.PatientVisits.Box24IJShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(86, PARM_BOX24_B_SHADED, ds.PatientVisits.Box24BShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(87, PARM_IS_COLLECTIN, ds.PatientVisits.IsCollectionColumn.ColumnName, DbType.Byte);

            string submitStatusId = ds.Tables["PatientVisits"].Rows[0]["SubmitStatusId"].ToString();
            int SubmitsatId = MDVUtility.ToInt32(submitStatusId);
            if (SubmitsatId == 27 || SubmitsatId == 28)
            {
                Int64 AppUserID= MDVSession.Current.AppUserId;
                ds.Tables["PatientVisits"].Rows[0]["SubmitStatusUserId"] = AppUserID;
                dbManager.AddInsertUpdateParameters(88, PARM_SUBMIT_STATUS_USERID, ds.PatientVisits.SubmitStatusUserIdColumn.ColumnName, DbType.Int64);
            }
            else {              
                ds.Tables["PatientVisits"].Rows[0]["SubmitStatusUserId"] = null;
                dbManager.AddInsertUpdateParameters(88, PARM_SUBMIT_STATUS_USERID, ds.PatientVisits.SubmitStatusUserIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(89, PARM_IS_SELF_PAY, ds.PatientVisits.selfpayColumn.ColumnName, DbType.Boolean);
        }

        private void CreateUpdateParameters(IDBManager dbManager, DSVisits ds)
        {
            dbManager.CreateUpdateParameters(90);

            dbManager.AddInsertUpdateParameters(0, PARM_VISIT_ID, ds.PatientVisits.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_APPOINTMENT_ID, ds.PatientVisits.AppointmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PATIENT_ID, ds.PatientVisits.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_PROVIDER_ID, ds.PatientVisits.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_FACILITY_ID, ds.PatientVisits.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_REF_PROVIDER_ID, ds.PatientVisits.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_PATIENT_INSURANCE_ID, ds.PatientVisits.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(7, PARM_APPOINTMENT_DATE, ds.PatientVisits.AppointmentDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_CASE_MGMT_ID, ds.PatientVisits.CaseMgmtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_BATCH_ID, ds.PatientVisits.BatchIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_CLAIM_NUMBER, ds.PatientVisits.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_REFERRAL_NUMBER, ds.PatientVisits.ReferralNumberColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_DOS_FROM, ds.PatientVisits.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(13, PARM_DOS_TO, ds.PatientVisits.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_ADMISSION_DATE, ds.PatientVisits.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_INJURY_DATE, ds.PatientVisits.InjuryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_ILLNESS_DATE, ds.PatientVisits.IllnessDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_DISCHARGE_DATE, ds.PatientVisits.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_DELAY_REASON, ds.PatientVisits.DelayReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_CLAIM_FREQUENCY, ds.PatientVisits.ClaimFrequencyColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_ICD_DCN, ds.PatientVisits.ICDDCNColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_LMP_DATE, ds.PatientVisits.LMPDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(22, PARM_COMMENTS, ds.PatientVisits.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_PRINT_HCFA, ds.PatientVisits.PrintHCFAColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(24, PARM_ORDERING_PROVIDER, ds.PatientVisits.OrderingProviderColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(25, PARM_SUPERVISING_PROVIDER, ds.PatientVisits.SupervisingProviderColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(26, PARM_UA_WORK_FROM, ds.PatientVisits.UAWorkFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(27, PARM_UA_WORK_TO, ds.PatientVisits.UAWorktoColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(28, PARM_VISIT_COPAYMENT, ds.PatientVisits.VisitCopaymentColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(29, PARM_IS_COPAY_PAID, ds.PatientVisits.IsCopayPaidColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(30, PARM_SCH_REASON_ID, ds.PatientVisits.SchReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(31, PARM_CLINICAL_TEMP_ID, ds.PatientVisits.ClinicalTempIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(32, PARM_IS_ACTIVE, ds.PatientVisits.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(33, PARM_CREATED_BY, ds.PatientVisits.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(34, PARM_CREATED_ON, ds.PatientVisits.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(35, PARM_MODIFIED_BY, ds.PatientVisits.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(36, PARM_MODIFIED_ON, ds.PatientVisits.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(37, PARM_VISIT_STATUS, ds.PatientVisits.VisitStatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(38, PARM_SUBMITTED_BY, ds.PatientVisits.SubmittedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(39, PARM_SUBMITTED_DATE, ds.PatientVisits.SubmittedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(40, PARM_ASSIGN_BENEFITS, ds.PatientVisits.AssignBenefitsColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(41, PARM_AUTO_ACCIDENT, ds.PatientVisits.AutoAccidentColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(42, PARM_STATE, ds.PatientVisits.StateColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(43, PARM_OTHER_ACCIDENT, ds.PatientVisits.OtherAccidentColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(44, PARM_IS_EMPLOYED, ds.PatientVisits.IsEmployedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(45, PARM_CLAIM_STATUS_ID, ds.PatientVisits.ClaimStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(46, PARM_SUBMIT_STATUS_ID, ds.PatientVisits.SubmitStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(47, PARM_MASTER_VISIT_ID, ds.PatientVisits.MasterVisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(48, PARM_REPORT_TYPE_CODE_ID, ds.PatientVisits.ReportTypeCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(49, PARM_TRANSMISSION_CODE_ID, ds.PatientVisits.TransmissionCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(50, PARM_CONTROL_NUMBER, ds.PatientVisits.ControlNumberColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(51, PARM_DOCUMENT_SENT_DATE, ds.PatientVisits.DocumentSentDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(52, PARM_LAST_SEEN_DATE, ds.PatientVisits.LastSeenDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(53, PARM_RESOURCE_PROVIDER_ID, ds.PatientVisits.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(54, PARM_BILLING_PROVIDER_ID, ds.PatientVisits.BillingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(55, PARM_CLAIM_TYPE_ID, ds.PatientVisits.ClaimTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(56, PARM_CLAIM_COMMENTS, ds.PatientVisits.ClaimCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(57, PARM_IS_ELECTRONIC_SUBMIT, ds.PatientVisits.IsElectronicSubmitColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(58, PARM_READY_TO_SUBMIT_ON, ds.PatientVisits.ReadyToSubmitOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(59, PARM_ISSPECIALIST, ds.PatientVisits.IsSpecialistColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(60, PARM_BILL_TO_PATIENT, ds.PatientVisits.BillToPatientColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(61, PARM_PATIENT_REFERRAL_ID, ds.PatientVisits.PatientReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(62, PARM_IS_SPLITTED, ds.PatientVisits.IsSplittedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(63, PARM_SPLITTED_VISIT_ID, ds.PatientVisits.SplittedVisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(64, PARM_HOLD_TILL, ds.PatientVisits.HoldTillColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(65, PARM_PAN, ds.PatientVisits.PANColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(66, PARM_AUTHORIZE_ID, ds.PatientVisits.AuthorizeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(67, PARM_IS_REPORT_NPI, ds.PatientVisits.IsReportNPIColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(68, PARM_ISCLEANCLAIM, ds.PatientVisits.IsCleanClaimColumn.ColumnName, DbType.Boolean);

            dbManager.AddInsertUpdateParameters(69, PARM_ANESTHESIOLOGIST_ID, ds.PatientVisits.AnesthesiologistIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(70, PARM_IS_ANES, ds.PatientVisits.IsAnesColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(71, PARM_ANES_STARTTIME, ds.PatientVisits.AnesStartTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(72, PARM_ANES_ENDTIME, ds.PatientVisits.AnesEndTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(73, PARM_CRNA_ID, ds.PatientVisits.CRNAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(74, PARM_CRNA_STARTTIME, ds.PatientVisits.CRNAStartTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(75, PARM_CRNA_ENDTIME, ds.PatientVisits.CRNAEndTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(76, PARM_ANES_TYPEID, ds.PatientVisits.AnesTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(77, PARM_ASA_ID, ds.PatientVisits.ASAIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(78, PARM_ANES_SERVICETYPEID, ds.PatientVisits.AnesServiceTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(79, PARM_ANES_COMMENTS, ds.PatientVisits.AnesCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(80, PARM_RISK_UNITS, ds.PatientVisits.RiskUnitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(81, PARM_NOTE_COMMENTS, ds.PatientVisits.NoteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(82, PARM_REASON_COMMENTS, ds.PatientVisits.ReasonCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(83, PARM_NOTE_MODIFIED_BY, ds.PatientVisits.NoteModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(84, PARM_NOTE_MODIFIED_ON, ds.PatientVisits.NoteModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(85, PARM_BOX24_IJ_SHADED, ds.PatientVisits.Box24IJShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(86, PARM_BOX24_B_SHADED, ds.PatientVisits.Box24BShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(87, PARM_IS_COLLECTIN, ds.PatientVisits.IsCollectionColumn.ColumnName, DbType.Byte);
            string submitStatusId = ds.Tables["PatientVisits"].Rows[0]["SubmitStatusId"].ToString();
            int SubmitsatId = MDVUtility.ToInt32(submitStatusId);
            if (SubmitsatId == 27 || SubmitsatId == 28)
            {
                Int64 AppUserID = MDVSession.Current.AppUserId;
                ds.Tables["PatientVisits"].Rows[0]["SubmitStatusUserId"] = AppUserID;
                dbManager.AddInsertUpdateParameters(88, PARM_SUBMIT_STATUS_USERID, ds.PatientVisits.SubmitStatusUserIdColumn.ColumnName, DbType.Int64);
            }
            else
            {
                ds.Tables["PatientVisits"].Rows[0]["SubmitStatusUserId"] = null;
                dbManager.AddInsertUpdateParameters(88, PARM_SUBMIT_STATUS_USERID, ds.PatientVisits.SubmitStatusUserIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(89, PARM_CLAIM_COMMENT_CHANGED, ds.PatientVisits.ClaimCommentChangedColumn.ColumnName, DbType.Byte);
        }

        private void CreateClaimErrorParameters(IDBManager dbManager, DSVisits ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(6);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CLAIM_ERROR_ID, ds.ClaimErrors.ErrorIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CLAIM_ERROR_ID, ds.ClaimErrors.ErrorIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VISIT_ID, ds.ClaimErrors.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CLAIM_ERROR_CODE, ds.ClaimErrors.ErrorCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CLAIM_ERROR_DESCRIPTION, ds.ClaimErrors.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CLAIM_ERROR_ACTION, ds.ClaimErrors.ActionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CLAIM_ERROR_JOB_NUMBER, ds.ClaimErrors.JobNumberColumn.ColumnName, DbType.String);
        }

        private void CreateClaimScrubHistoryParameters(IDBManager dbManager, DSVisits ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CLAIM_SCRUB_HISTORY_ID, ds.ClaimScrubHistory.ClaimScrubHistoryIDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CLAIM_SCRUB_HISTORY_ID, ds.ClaimScrubHistory.ClaimScrubHistoryIDColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(1, PARM_EDI_TEXT, ds.ClaimScrubHistory.EDITextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_JOB_NUMBER, ds.ClaimScrubHistory.JobNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CLAIM_SCRUB_VISIT_IDS, ds.ClaimScrubHistory.VisitIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SERVICE_RESPONSE_TEXT, ds.ClaimScrubHistory.ServiceResponseTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ClaimScrubHistory.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.ClaimScrubHistory.CreatedByColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the Patient Visits.
        /// </summary>
        /// <param name="VisitId">The Visit identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSVisits LoadPatientVisits(long VisitId, long PatientId, long ProviderId, long FacilityId, DateTime? fAppDate, DateTime? toAppDate, string ClaimNum, string IsCheckout, string IsActive, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 1000, Int64 CaseMgmtId = 0, string Module = "")
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(16);
                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (ProviderId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(4, PARM_FROM_APPOINTMENT_DATE, fAppDate);
                dbManager.AddParameters(5, PARM_TO_APPOINTMENT_DATE, toAppDate);
                if (ClaimNum == "")
                    ClaimNum = null;
                dbManager.AddParameters(6, PARM_CLAIM_NUMBER, ClaimNum);
                if (IsCheckout == "")
                    IsCheckout = null;
                dbManager.AddParameters(7, PARM_IS_CHECKOUT, IsCheckout);
                if (IsActive == "")
                    IsActive = null;
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);

                if (_837Batchid == 0)
                    dbManager.AddParameters(9, PARM_837_BATCH_ID, null);
                else
                    dbManager.AddParameters(9, PARM_837_BATCH_ID, _837Batchid);
                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.PatientVisits.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                if (CaseMgmtId == 0)
                    dbManager.AddParameters(14, PARM_CASE_MGMT_ID, null);
                else
                    dbManager.AddParameters(14, PARM_CASE_MGMT_ID, CaseMgmtId);


                dbManager.AddParameters(15, PARM_USER_ID, MDVSession.Current.AppUserId);

                // dbManager.AddParameters(16, PARM_MODULE, Module);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_SELECT, ds, ds.PatientVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadPatientVisits", PROC_PATIENT_VISITS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisits searchPatientVisits(long VisitId, long PatientId, long ProviderId, long FacilityId, DateTime? fAppDate, DateTime? toAppDate, string ClaimNum, string IsCheckout, string IsActive,  int PageNumber = 1, int RowspPage = 1000)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(14);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (ProviderId == 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);
                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(3, PARM_FROM_APPOINTMENT_DATE, fAppDate);
                dbManager.AddParameters(4, PARM_TO_APPOINTMENT_DATE, toAppDate);
                if (ClaimNum == "")
                    ClaimNum = null;
                dbManager.AddParameters(5, PARM_CLAIM_NUMBER, ClaimNum);
                if (IsCheckout == "")
                    IsCheckout = null;
                dbManager.AddParameters(6, PARM_IS_CHECKOUT, IsCheckout);
                if (IsActive == "")
                    IsActive = null;
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);


                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.PatientVisitsSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(12, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (VisitId == 0)
                    dbManager.AddParameters(13, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(13, PARM_VISIT_ID, VisitId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_SEARCH, ds, ds.PatientVisitsSearch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::searchPatientVisits", PROC_PATIENT_VISITS_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the Patient Outstanding Visits.
        /// </summary>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="PatientOutstanding">The PatientOutstanding.</param>
        /// <returns></returns>
        public DSVisits LoadPatientOutstandingVisits(long PatientId, string PatientOutstanding = "")
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PatientOutstanding == "")
                    PatientOutstanding = null;
                dbManager.AddParameters(3, PARM_Patient_Outstanding, PatientOutstanding);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_OUTSTANDING_VISITS_SELECT, ds, ds.PatientVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadPatientOutstandingVisits", PROC_PATIENT_OUTSTANDING_VISITS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the Visit Plans.
        /// </summary>
        /// <param name="VisitId">The Visit identifier.</param>
        /// <returns></returns>
        public DSVisitLookup LoadVisitPlan(Int64 VisitId)
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VISIT_PLAN_LOOKUP, ds, ds.PatientVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadVisitPlan", PROC_VISIT_PLAN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Patient Visits.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSVisits InsertPatientVisits(DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = ds.PatientVisits.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSVisits)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_INSERT, ds, ds.PatientVisits.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertPatientVisits", PROC_PATIENT_VISITS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }



        }



        public DSVisits InsertPatientVisitsWithTrans(DSVisits ds, IDBManager dbManager)
        {
            //  IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //DataTable dtTemp = ds.PatientVisits.GetChanges();
                // dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSVisits)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_INSERT, ds, ds.PatientVisits.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertPatientVisits", PROC_PATIENT_VISITS_INSERT, ex);
                throw ex;
            }
           
        }

        public DSVisits InsertPatientVisitsAuditlog(DataTable dtTemp, DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.Open();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertPatientVisits", PROC_PATIENT_VISITS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSVisits InsertPatientVisits(DSVisits ds, IDBManager dbManager)
        {
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientVisits.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSVisits)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_INSERT, ds, ds.PatientVisits.TableName);
                ds.AcceptChanges();

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertPatientVisits", PROC_PATIENT_VISITS_INSERT, ex);
                throw ex;
            }
        }

      



        /// <summary>
        /// Inserts the Patient Visits.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSVisits InsertUpdatePatientVisits(DSVisits ds, IDBManager dbManager)
        {
            // IDBManager dbManager = ClientConfiguration.GetDBManager();
           // DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //dbManager.Open();
                //this.CreateInsertParameters(dbManager, ds);
                //this.CreateUpdateParameters(dbManager, ds);
                //ds = (DSVisits)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_INSERT, PROC_PATIENT_VISITS_UPDATE, ds, ds.PatientVisits.TableName);
                ////ds.AcceptChanges();
                //return ds;



                // DataTable dtTemp = ds.PatientVisits.GetChanges();
                //dbManager.Open();
                // dbManager.BeginTransaction();
                if (MDVUtility.ToInt64(ds.Tables["PatientVisits"].Rows[0]["VisitId"]) < 0)
                    this.CreateInsertParameters(dbManager, ds);
                else
                    this.CreateUpdateParameters(dbManager, ds);
                ds = (DSVisits)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_INSERT, PROC_PATIENT_VISITS_UPDATE, ds, ds.PatientVisits.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                //    // dbManager.CommitTransaction();
                //    dsDBAudit.AcceptChanges();
                //}

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertUpdatePatientVisits", PROC_PATIENT_VISITS_INSERT, ex);
                throw ex;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        public DSVisits InsertUpdatePatientVisitsAuditLog(DataTable dtTemp,DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                    // dbManager.CommitTransaction();
                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertUpdatePatientVisitsAuditLog", "DBAuditChanges", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string SaveMultipleNoteComments(string VisitIds, string ModifiedBy, DateTime ModifiedOn, string NoteComments = null, string NoteModifiedBy = null, DateTime? NoteModifiedOn = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string result = string.Empty;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_VISIT_IDS, VisitIds);
                dbManager.AddParameters(1, PARM_NOTE_COMMENTS, NoteComments);
                dbManager.AddParameters(2, PARM_NOTE_MODIFIED_BY, NoteModifiedBy);
                dbManager.AddParameters(3, PARM_NOTE_MODIFIED_ON, NoteModifiedOn);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(5, PARM_MODIFIED_ON, ModifiedOn);
                dbManager.AddParameters(6, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                result = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MULTIPLE_NOTE_COMMENTS_INSERT).ToString();
                if (result != "")
                    throw new Exception(result);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::SaveMultipleNoteComments", PROC_MULTIPLE_NOTE_COMMENTS_INSERT, ex);
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
        /// Updates the Patient Visits
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSVisits UpdatePatientVisits(DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientVisits.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSVisits)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_UPDATE, ds, ds.PatientVisits.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::UpdatePatientVisits", PROC_PATIENT_VISITS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSVisits UpdatePatientVisits(DSVisits ds, IDBManager dbManager)
        {
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientVisits.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSVisits)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_UPDATE, ds, ds.PatientVisits.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisits.Rows[0][ds.PatientVisits.VisitIdColumn].ToString());
                    // dbManager.CommitTransaction();
                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::UpdatePatientVisits", PROC_PATIENT_VISITS_UPDATE, ex);
                throw ex;
            }


        }

        public string PrintClaimHistory(string VisitId,string PatientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_USER_NAME, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(4, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(5, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_CLAIM_HISTORY).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::PrintClaimHistory", PROC_PATIENT_CLAIM_HISTORY, ex);
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
        public string UpdatePatientVisitsAndChargesStatusUpdate(long BatchId, long StatusId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_837_BATCH_ID, BatchId);
                dbManager.AddParameters(1, PARM_STATUS_ID, StatusId);
                dbManager.AddParameters(2, PARM_USER_NAME, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_VISIT_CHARGE_STATU_UPDATE).ToString();

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::UpdatePatientVisitsAndChargesStatusUpdate", PROC_PATIENT_VISIT_CHARGE_STATU_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdatePatientVisitsAndChargesStatusUpdate(SharedVariable SharedVariable,long BatchId, long StatusId,string EntityId,string UserName)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_837_BATCH_ID, BatchId);
                dbManager.AddParameters(1, PARM_STATUS_ID, StatusId);
                dbManager.AddParameters(2, PARM_USER_NAME, ClientConfiguration.DecryptFrom64(UserName));
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                dbManager.AddParameters(4, PARM_ENTITY_ID, EntityId);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_VISIT_CHARGE_STATU_UPDATE).ToString();

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALVisits::UpdatePatientVisitsAndChargesStatusUpdate", PROC_PATIENT_VISIT_CHARGE_STATU_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdateResubmitStatusOfVisitsAndCharges(long BatchId, long StatusId, string VisitIds, int VisitStatusId, int ChargeStatusId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);

                dbManager.AddParameters(0, PARM_837_BATCH_ID, BatchId);
                dbManager.AddParameters(1, PARM_STATUS_ID, StatusId);
                dbManager.AddParameters(2, PARM_USER_NAME, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(5, PARM_VISIT_IDS, VisitIds);
                dbManager.AddParameters(6, PARM_VISIT_STATUS_ID, VisitStatusId);
                dbManager.AddParameters(7, PARM_CHARGE_STATUS_ID, ChargeStatusId);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_VISIT_CHARGE_STATURESUBMIT_UPDATE).ToString();

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::UpdateResubmitStatusOfVisitsAndCharges", PROC_PATIENT_VISIT_CHARGE_STATURESUBMIT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Added By Azeem Raza Tayyab on 29-Apr-2016 to update SubmitStatus of multiple claims at once.
        /// </summary>
        /// <param name="VisitIds"></param>
        /// <param name="SubmitStatusId"></param>
        /// <returns></returns>
        public string UpdateVisitsSubmitStatus(string VisitIds, int SubmitStatusId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                dbManager.AddParameters(0, PARM_VISIT_IDS, VisitIds);
                dbManager.AddParameters(1, PARM_SUBMIT_STATUS_ID, SubmitStatusId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(3, PARM_USER_NAME, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
               
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_VISITS_SUBMIT_STATUS).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::UpdateVisitsSubmitStatus", PROC_PATIENT_VISITS_SUBMIT_STATUS, ex);
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
        /// Deletes the Visit.
        /// </summary>
        /// <param name="VisitId">The Visit identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteVisit(string VisitId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_VISITS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::DeleteVisit", PROC_PATIENT_VISITS_DELETE, ex);
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

        public string LoadCPTFee(Int64 VisitId, string CPTCode)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (CPTCode == "")
                    CPTCode = null;

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_CPT_CODE, CPTCode);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CPT_FEE_SELECT).ToString();

                return returnVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadCPTFee", PROC_CPT_FEE_SELECT, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string VoidAndReCreateVisit(string VisitId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(5, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VISIT_VOID_RECREATE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::VoidAndReCreateVisit", PROC_VISIT_VOID_RECREATE, ex);
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

        public DSVisits LoadClaimVoidInfo(long VisitId)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_VOID_INFO_SELECT, ds, ds.ClaimVoidInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadClaimVoidInfo", PROC_CLAIM_VOID_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisits LoadPatientVistsDetails(long VisitId)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_DETAIL_SELECT, ds, ds.PatientVisitsDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadPatientVistsDetails", PROC_PATIENT_VISITS_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSVisits LoadClaimPayments(long VisitId)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_PAYMENTS_SELECT, ds, ds.ClaimPayments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadClaimPayments", PROC_CLAIM_PAYMENTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region " Claim Errors "

        public DSVisits LoadClaimErrors(long ErrorId, long VisitId)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ErrorId == 0)
                    dbManager.AddParameters(0, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CLAIM_ERROR_ID, ErrorId);

                if (VisitId == 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_ERRORS_SELECT, ds, ds.ClaimErrors.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadClaimErrors", PROC_CLAIM_ERRORS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSVisits InsertClaimScrubHistory(DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateClaimScrubHistoryParameters(dbManager, ds, true);
                ds = (DSVisits)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLAIM_SCRUB_HISTORY_INSERT, ds, ds.ClaimScrubHistory.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertClaimScrubHistory", PROC_CLAIM_SCRUB_HISTORY_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSVisits InsertClaimErrors(DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateClaimErrorParameters(dbManager, ds, true);
                ds = (DSVisits)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLAIM_ERRORS_INSERT, ds, ds.ClaimErrors.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertClaimErrors", PROC_CLAIM_ERRORS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisits UpdateClaimErrors(DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateClaimErrorParameters(dbManager, ds, false);
                ds = (DSVisits)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CLAIM_ERRORS_UPDATE, ds, ds.ClaimErrors.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::UpdateClaimErrors", PROC_CLAIM_ERRORS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteClaimErrors(long ErrorId, long VisitId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (ErrorId == 0)
                    dbManager.AddParameters(0, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CLAIM_ERROR_ID, ErrorId);

                if (VisitId == 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);

                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLAIM_ERRORS_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::DeleteClaimError", PROC_CLAIM_ERRORS_DELETE, ex);
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
        //Start PRD-635 TahreeMalik         to verify claim exists in system with same Provider, Facility and DOS or not
        public DSVisits VerifyDuplicateClaim(Int64 PatientId, Int64 ProviderId, Int64 FacilityId, DateTime DOSFrom, DateTime DOSTo, string ClaimNumber = null)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (ProviderId == 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                if (FacilityId == 0)
                    dbManager.AddParameters(2, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);

                dbManager.AddParameters(3, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(4, PARM_DOS_TO, DOSTo);
                
                if(string.IsNullOrEmpty(ClaimNumber))
                    dbManager.AddParameters(5, PARM_CLAIM_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_CLAIM_NUMBER, ClaimNumber);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VERIFY_DUPLICATE_CLAIM, ds, ds.DuplicateClaim.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::VerifyDuplicateClaim", PROC_VERIFY_DUPLICATE_CLAIM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //End PRD-635 TahreeMalik         to verify claim exists in system with same Provider, Facility and DOS or not
        #endregion

        #region "Lookups"

        public DSVisitLookup LookupPatientVisits(Int64 PatientId, string ClaimNumber, Boolean isVisitCharge)
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (ClaimNumber == "")
                    ClaimNumber = null;
                dbManager.AddParameters(1, PARM_CLAIM_NUMBER, ClaimNumber);

                if (isVisitCharge == false)
                    dbManager.AddParameters(2, PARM_IS_VISIT_CHARGE, null);
                else
                    dbManager.AddParameters(2, PARM_IS_VISIT_CHARGE, isVisitCharge);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISITS_LOOKUP, ds, ds.PatientVisits.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupPatientVisits", PROC_PATIENT_VISITS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupVisitsDelayReason()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DELAY_REASON_LOOKUP, ds, ds.DelayReason.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupVisitsDelayReason", PROC_DELAY_REASON_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupVisitsClaimFrequency()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_FREQUENCY_LOOKUP, ds, ds.ClaimFrequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupVisitsClaimFrequency", PROC_CLAIM_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupReportTypeCode()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORT_TYPE_CODE_LOOKUP, ds, ds.ReportTypeCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupReportTypeCode", PROC_REPORT_TYPE_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSVisitLookup LookupTransmissionCode()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TRANSMISSION_CODE_LOOKUP, ds, ds.TransmissionCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupTransmissionCode", PROC_TRANSMISSION_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSVisitLookup LookupVisitStatus()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VISIT_STATUS_LOOKUP, ds, ds.VisitStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupVisitStatus", PROC_VISIT_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSVisitLookup LookupSubmitStatus(string IsActive)
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);

                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUBMIT_STATUS_LOOKUP, ds, ds.SubmitStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupSubmitStatus", PROC_SUBMIT_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupVoidedStatus(string IsActive)
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(0);

                //if (string.IsNullOrEmpty(IsActive))
                //    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                //else
                //    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);

                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VOIDED_STATUS_LOOKUP, ds, ds.DT_Voided_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupSubmitStatus", PROC_SUBMIT_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupAnesthesiaType()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANES_ANESTHESIATYPE_LOOKUP, ds, ds.AnesthesiaType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupAnesthesiaType", PROC_ANES_ANESTHESIATYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupAnesServiceType()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANES_SERVICETYPE_LOOKUP, ds, ds.AnesServiceType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupAnesServiceType", PROC_ANES_SERVICETYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisitLookup LookupAnesthesiaASA()
        {
            DSVisitLookup ds = new DSVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANES_ASA_LOOKUP, ds, ds.ASA.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LookupAnesthesiaASA", PROC_ANES_ASA_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<STC> getEDIDetail(string claimNumber)
        {
            List<STC> EDIDetailList = new List<STC>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_CLAIM_NUMBER, claimNumber);
                EDIDetailList = dbManager.ExecuteReaders<STC>(PROC_GET_EDI_DETAIL);
                return EDIDetailList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::getEDIDetail", PROC_GET_EDI_DETAIL, ex);
                throw ex;
            }
            finally
            {
            }
        }

        

            public string getClaimStatus(string claimNumber)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_CLAIM_NUMBER, claimNumber);
                var result = dbManager.ExecuteScalar(PROC_GET_CLAIM_STATUS).ToString();
                if (result != "-1" && result != "1" && result != "0")
                {
                    throw new Exception(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALEDIReports::getClaimStatus", PROC_GET_CLAIM_STATUS, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Patient Visits ICD
        /// <summary>
        /// Inserts the Patient Visits.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSVisits InsertUpdatePatientVisitsICD(DSVisits ds, IDBManager dbManager)
        {
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientVisitICD.GetChanges();
                this.CreateInsertPatientVisitsICDParameters(dbManager, ds);
                this.CreateUpdatePatientVisitsICDParameters(dbManager, ds);
                ds = (DSVisits)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_ICD_INSERT, PROC_PATIENT_VISIT_ICD_UPDATE, ds, ds.PatientVisitICD.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisitICD.Rows[ds.PatientVisitICD.Rows.Count - 1][ds.PatientVisitICD.VisitIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertUpdatePatientVisitsICD", PROC_PATIENT_VISIT_ICD_INSERT, ex);
                throw ex;
            }

        }

        public DSVisits InsertUpdatePatientVisitsICDWithTran(DSVisits ds, IDBManager dbManager)
        {
            try
            {
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientVisitICD.GetChanges();
                this.CreateInsertPatientVisitsICDParameters(dbManager, ds);
                this.CreateUpdatePatientVisitsICDParameters(dbManager, ds);
                ds = (DSVisits)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_ICD_INSERT, PROC_PATIENT_VISIT_ICD_UPDATE, ds, ds.PatientVisitICD.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisitICD.Rows[ds.PatientVisitICD.Rows.Count - 1][ds.PatientVisitICD.VisitIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertUpdatePatientVisitsICD", PROC_PATIENT_VISIT_ICD_INSERT, ex);
                throw ex;
            }

        }
        public DSVisits InsertUpdatePatientVisitsICDAuditLog(DataTable dtTemp,DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
               
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientVisitICD.Rows[ds.PatientVisitICD.Rows.Count - 1][ds.PatientVisitICD.VisitIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::InsertUpdatePatientVisitsICDAuditLog", "DBAuditChanges", ex);
                throw ex;
            }
            finally {
                dbManager.Dispose();
            }

        }

        private void CreateInsertPatientVisitsICDParameters(IDBManager dbManager, DSVisits ds)
        {
            dbManager.CreateInsertParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_PV_ICD_ID, ds.PatientVisitICD.PVICDIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_VISIT_ID, ds.PatientVisitICD.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD_Type, ds.PatientVisitICD.ICDTypeColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_ICDCODE, ds.PatientVisitICD.ICDCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICDCODE_DESCRIPTION, ds.PatientVisitICD.ICDCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_SNOMEDID, ds.PatientVisitICD.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOME_DESCRIPTION, ds.PatientVisitICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_LEXICODE, ds.PatientVisitICD.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXICODE_DESCRIPTION, ds.PatientVisitICD.LexiCodeDescriptionColumn.ColumnName, DbType.String);
        }

        private void CreateUpdatePatientVisitsICDParameters(IDBManager dbManager, DSVisits ds)
        {
            dbManager.CreateUpdateParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_PV_ICD_ID, ds.PatientVisitICD.PVICDIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_VISIT_ID, ds.PatientVisitICD.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD_Type, ds.PatientVisitICD.ICDTypeColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_ICDCODE, ds.PatientVisitICD.ICDCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICDCODE_DESCRIPTION, ds.PatientVisitICD.ICDCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_SNOMEDID, ds.PatientVisitICD.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOME_DESCRIPTION, ds.PatientVisitICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_LEXICODE, ds.PatientVisitICD.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXICODE_DESCRIPTION, ds.PatientVisitICD.LexiCodeDescriptionColumn.ColumnName, DbType.String);
        }
        /// <summary>
        /// Load ICDs for Patient Visits
        /// </summary>
        /// <param name="VisitId"></param>
        /// <param name="ICDType"></param>
        /// <returns></returns>
        public DSVisits LoadPatientVisitICDs(long VisitId, Int32 ICDType)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                if (ICDType == 0)
                    dbManager.AddParameters(1, PARM_ICD_Type, null);
                else
                    dbManager.AddParameters(1, PARM_ICD_Type, ICDType);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_ICD_SELECT, ds, ds.PatientVisitICD.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadPatientVisitICDs", PROC_PATIENT_VISIT_ICD_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCharge DeleteVisitICD(long VisitId, int ICDIndex)
        {
            DSCharge ds = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(1, PARM_ICD_INDEX, ICDIndex);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(4, PARM_USER_NAME, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                ds = (DSCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_ICD_DELETE, ds, ds.PatientCharges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::DeleteVisitICD", PROC_PATIENT_VISIT_ICD_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public bool DeleteVisitICDByPVICDID(long PVICDId)
        {
            int res = 0;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PV_ICD_ID, PVICDId);
                res = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_VISIT_ICD_BY_ID_DELETE);
                return res > 0;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::DeleteVisitICDByPVICDID", PROC_PATIENT_VISIT_ICD_BY_ID_DELETE, ex);
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
