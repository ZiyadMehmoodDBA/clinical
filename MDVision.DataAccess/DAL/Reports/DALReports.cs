using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Clinical.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MDVision.Model.CCM.Reports;
using MDVision.Model.Report;

namespace MDVision.DataAccess.DAL.Reports
{
    public class DALReports
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"



        private const string PROC_REPORTS_ADVANCE_PAYMENT = "Reports.sp_AdvancePayment";
        private const string PROC_REPORTS_ENTERPRISE_SCHEDULING = "Reports.sp_EnterpriseSchedulingReport";

        private const string PROC_REPORTS_COLLECTED_COPAYMENT = "Reports.sp_Copayment";
        private const string PROC_REPORTS_UNALLOCATED_COPAYMENT = "Reports.sp_UnAllocatedCopayment";
        private const string PROC_REPORTS_ENTERPRISE_REVENUE = "Reports.EnterpriseRevenue";
        private const string PROC_REPORTS_PROVIDER_ANALYSIS_BY_INSURANCEPLAN = "Reports.ProviderAnalysisByInsurancePlan";
        private const string PROC_REPORTS_PROVIDER_PROCEDURE_UTILIZATION = "Reports.ProviderProcedureUtilization";
        private const string PROC_REPORTS_REVENUE_BY_FACILITY = "Reports.RevenueByFacility";
        private const string PROC_REPORTS_REVENUE_BY_INSURANCE_PLAN = "Reports.RevenuebyInsurancePlan";
        private const string PROC_REPORTS_REVENUE_BY_PROVIDER = "Reports.RevenueByProvider";
        private const string PROC_REPORTS_SP_ARAGING_ANALYSIS = "Reports.sp_ARAgingAnalysis";
        private const string PROC_REPORTS_SP_ARAGING_ANALYSIS_MPS = "Reports.sp_ARAgingAnalysis_MPS";
        private const string PROC_REPORTS_SP_CHARGES_AND_PAYMENTS = "Reports.sp_ChargesAndPayments";
        private const string PROC_REPORTS_SP_CHARGESLIST = "Reports.sp_ChargesList";
        private const string PROC_REPORTS_SP_CHECKIN_CHECKOUT_DURATION = "Reports.sp_CheckInCheckoutduration";
        private const string PROC_REPORTS_SP_COLLECTED_REVENUE = "Reports.sp_CollectedRevenue";
        private const string PROC_REPORTS_SP_COPAYMENT = "Reports.sp_Copayment";
        private const string PROC_REPORTS_SP_COPAYMENT_BKP = "Reports.sp_Copayment_bkp";
        private const string PROC_REPORTS_SP_DAILY_APPOINTMENTS_PROVIDER = "Reports.sp_DailyAppointmentsProvider";
        private const string PROC_REPORTS_SP_DAILY_APPOINTMENTS_RESOURCE = "Reports.sp_DailyAppointmentsResource";
        private const string PROC_REPORTS_SP_DIAGNOSIS_ANALYSIS = "Reports.sp_DiagnosisAnalysis";
        private const string PROC_REPORTS_SP_ENTERPRISE_AR_ANALYSIS = "Reports.sp_EnterpriseARAnalysis";
        private const string PROC_REPORTS_SP_INSURANCE_ANALYSIS = "Reports.sp_InsuranceAnalysis";
        private const string PROC_REPORTS_SP_INSURANCE_ANALYSIS_SUMMARY = "Reports.sp_InsuranceAnalysisSummary";
        private const string PROC_REPORTS_SP_INSURANCE_PLAN_AR = "Reports.sp_InsurancePlanAR";
        private const string PROC_REPORTS_SP_PATIENT_LIST = "Reports.sp_List";
        private const string PROC_REPORTS_SP_OUTSTANDING_BALANCES = "Reports.sp_OutstandingBalances";
        private const string PROC_REPORTS_SP_PATIENT_AR = "Reports.sp_PatientAR";
        private const string PROC_REPORTS_SP_PAYMENT_ENTRIES = "Reports.sp_PaymentEntries";
        private const string PROC_REPORTS_SP_PROCEDURE_ANALYSIS = "Reports.sp_ProcedureAnalysis";
        private const string PROC_REPORTS_SP_PROVIDER_APPOINTMENT = "Reports.sp_ProviderAppointment";
        private const string PROC_REPORTS_SP_RESOURCE_APPOINTMENT = "Reports.sp_ResourceAppointment";
        private const string PROC_REPORTS_SP_FOLLOWUP_APPOINTMENTS = "Reports.sp_TtlFollowupAppoitments";
        private const string PROC_FINANCIAL_ANALYSIS_AT_CPT = "Reports.sp_FinancialAnalysisAtCpt";
        private const string PROC_BEGINING_AR_ENDING_AR = "Reports.sp_BeginningAREndingAR_NewZ";
        private const string PROC_PAYMENTS_BY_USERS = "Reports.sp_PaymentByUser";
        private const string PROC_AGING_SUMMARY_ANALYSIS = "Reports.sp_AgingSummaryAnalysis_New";
        private const string PROC_ENCOUNTER_WITHOUT_CLAIMS = "Reports.sp_EncounterWithoutClaims";
        private const string PROC_CHARGES_BY_USERS = "Reports.sp_ChargesByUser";
        private const string PROC_BEGINNING_AR_ENDING_AR_FACILITY = "Reports.sp_BeginningAREndingARFacility_NewZ";
        private const string PROC_CLAIM_COMMENT_BY_USER = "Reports.sp_ClaimCommentByUser";
        private const string PROC_USER_ACTIVITY_REPORT = "Reports.sp_UserActivityReport";
        private const string PROC_CLAIM_SUBMIT_STATUS = "Reports.ClaimSubmitStatus";
        private const string PROC_AGING_DETAIL_ANALYSIS = "Reports.sp_AgingDetailAnalysis_New";
        private const string PROC_MU1_SELECT = "Provider.MU1";
        private const string PROC_MU2_SELECT = "Provider.MU2";
        private const string PROC_MU2LATEST_SELECT = "Provider.MU2Advance";
        private const string PROC_MU3_SELECT = "Provider.MU3";
        private const string PROC_ACI_GROUP_SELECT = "Provider.sp_ACIGroup";
        private const string PROC_DAILY_APPOINTMENT_REMINDER = "Reports.DailyAppointmentReminder";
        private const string PROC_AR_RECONCILIATION_REPORT = "Reports.FindDifferenceBARandAR";
        private const string PROC_PATIENT_OVER_PAYMENT = "Reports.sp_PatientOverPayment";
        private const string PROC_ZERO_PAID_CLAIMS = "Reports.Sp_ZeroPaidClaims";
        private const string PROC_ANESTHESIA_OVERLAPPING = "Reports.sp_AnesthesiaOverlappingReport";
        private const string PROC_CDS_ALERTS_REPORT = "Reports.sp_CDSAlertReport";
        private const string PROC_DAILY_COPAY_SHEET = "Reports.Sp_DailyCopaySheet";
        private const string PROC_INCORRECT_BALANCE_BY_VOIDED_CLAIMS = "Reports.IncorrectBalancebyVoidedClaim";
        private const string PROC_PROGRESS_NOTE = "Reports.sp_ClinicalProgressNote";
        private const string PROC_PROGRESS_NOTE_AMENDED = "Reports.sp_ClinicalProgressNoteAmended";
        private const string PROC_CLAIMS_NEVER_SUBMITTED = "Reports.sp_ClaimsNeverSubmittedToInsurance";
        private const string PROC_CLAIM_SUBMIT_DASHBOARD = "Reports.sp_ClaimStatusDashboardDeatil";
        private const string PROC_UNCLAIMED_APPOINTMENT = "Reports.sp_UnClaimedAppointments";
        private const string PROC_CLAIM_OVERPAID_BY_INSURANCE = "Reports.sp_ClaimsOverPaidByInsurance";
        private const string PROC_CLAIM_UNDERPAID_BY_INSURANCE = "Reports.sp_ClaimsUnderPaidByInsurance";
        private const string PROC_PATIENT_STATEMENT_PREFERENCE = "Reports.sp_PatientStatementPreference";
        private const string PROC_CLAIMUNDER_PAIDBY_PRIMARYINSURANCE = "Reports.sp_ClaimsUnderPaidByPrimaryInsurance";
        private const string PROC_ARO_REPORT = "Reports.sp_ARO";
        private const string PROC_AUP_REPORT = "Reports.sp_AUP";
        private const string PROC_ClaimInCollectionReport_REPORT = "Reports.sp_CollectionReport";
        private const string PROC_BILLING_INQUIRY_BY_PROVIDER = "Reports.sp_BillingInquiryEmail";
        private const string PROC_FAKPI_MONTHL_PAYMENT_TREND = "Reports.FAKPI_MonthlyPaymentTrend";
        private const string PROC_CLAIMS_UNDER_SECONDARY_INSURANCE = "Reports.sp_ClaimsUnderSecondaryInsurance";
        private const string PROC_INSURANCE_AR_PLAN = "Reports.sp_InsuranceARPlan";
        private const string PROC_CLAIM_FOLLOW_UP = "Reports.sp_ClaimFollowupReport";
        private const string PROC_CLAIM_SUBMISSION_HISTORY = "Reports.sp_ClaimSubmitHistory";
        private const string PROC_CLAIM_SCRUBBER_ERRORS = "Reports.sp_ClaimScrubberErrors";
        private const string PROC_ACI_INDIVIDUAL_SELECT = "provider.sp_aci";
        private const string PROC_ACI_INDIVIDUAL_BONUS_SELECT = "provider.sp_aci_Bonus";
        private const string PROC_FAKPI_MONTHLYPAYMENTTREND_DETAIL = "Reports.FAKPI_MonthlyPaymentTrend_Detail";
        private const string PROC_MIPS_IMPROVEMENT_ACTIVITY = "Reports.sp_MIPSImprovementActivity";
        private const string PROC_DRUG_CODE_COST = "Reports.sp_DrugCodeCost";

        #region clinical reports
        private const string PROC_CLINICAL_PHONE_ENCOUNTER = "Reports.sp_ClinicalPhoneEncounter";
        private const string PROC_CLINICAL_PROGRESS_NOTE = "Reports.sp_ClinicalProgressNote";
        private const string PROC_CLINICAL_ALLERGIES = "Reports.sp_Allergies";
        private const string PROC_CLINICAL_RADIOLOGY = "Reports.sp_Order_Radiology";
        private const string PROC_CLINICAL_LAB = "Reports.sp_Order_Lab";
        private const string PROC_POS_SURVERY = "Reports.sp_POSSurveyReport";

        private const string PROC_CLINICAL_MEDICATION = "Reports.sp_Medications";
        private const string PROC_CLINICAL_VITALS = "Reports.sp_Vitals";
        private const string PROC_CLINICAL_PROBLEMS = "Reports.sp_Problems";
        private const string PROC_CLINICAL_PROCEDURES = "Reports.sp_Procedures";

        private const string PROC_CLINICAL_ORDERS_LAB = "Reports.sp_Order_Lab";
        private const string PROC_CLINICAL_ORDERS_RADIOLOGY = "Reports.sp_Order_Radiology";
        private const string PROC_CLINICAL_ORDERS_PROCEDURE = "Reports.sp_Order_Procedures";
        private const string PROC_CLINICAL_ORDERS_CONSULTATION = "Reports.sp_Order_Consultation";
        private const string PROC_CLINICAL_ORDERS_PRESECRIPTION = "Reports.sp_Order_Prescription";
        private const string PROC_CLINICAL_RESULTS_LAB = "Reports.sp_Result_Lab";
        private const string PROC_CLINICAL_RESULT_RADIOLOGY = "Reports.sp_Result_Radiology";
        private const string PROC_CLINICAL_RESULT_CONSULTATION = "Reports.sp_Result_Consultation";

        private const string PROC_CLINICAL_IMMUNIZATION = "Reports.sp_Immunization";
        private const string PROC_CLINICAL_RESULTS_RADIOLOGY = "Reports.sp_Result_Radiology";
        private const string PROC_CLINICAL_RESULT_LAB = "Reports.sp_Result_Lab";
        private const string PROC_CLINICAL_ORDER_CONSULTATION = "Reports.sp_Order_Consultation";
        private const string PROC_CLINICAL_RESULTS_CONSULTATION = "Reports.sp_Result_Consultation";

        private const string PROC_CLINICAL_ORDERS_PROCEDURES = "Reports.sp_Order_Procedures";

        private const string PROC_CLINICAL_ORDERS_PRESCRIPTION = "Reports.sp_Order_Prescription";

        private const string PROC_CLINICAL_PHARMACY_LOOKUP = "Clinical.sp_PharmacySelect";
        private const string PROC_AUDIT_USER = "Reports.sp_UsersAuditLog";
        private const string PROC_APPOINTMENT_VS_CLAIM_SUMMARY_A = "Reports.AppointmentVsClaimSummaryAReport";
        private const string PROC_APPOINTMENT_VS_CLAIM_SUMMARY_B = "Reports.AppointmentVsClaimSummaryBReport";
        private const string PROC_APPOINTMENT_VS_CLAIM_DETAIL = "Reports.AppointmentVsClaimDetailReport";
        private const string PROC_ANTIMICROBIAL_LOOKUP = "Clinical.sp_AntimicrobialLookup";

        #endregion
        #region CCM Reports
        private const string PROC_CCM_REPORT = "Reports.sp_CCMReport";
        #endregion
        #endregion

        #region "Parameters"

        private const string PARM_PAYMENT_ID = "@PaymentId";
        private const string PARM_CHARGE_ID = "@ChargeId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";
        private const string PARM_ADVANCE_PAYMENT_ID = "@AdvPmtId";
        private const string PARM_VISIT_ID = "@VisitId";
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
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_MASTER_PAYMENT_ID = "@MasterPaymentId";
        private const string PARM_CHECK_NO = "@CheckNo";
        private const string PARM_CHECK_DATE = "@CheckDate";
        private const string PARM_EXPIRY_DATE = "@ExpiryDate";
        private const string PARM_CARD_TYPE_ID = "@CardTypeId";

        private const string PARM_ALLOWED = "@Allowed";
        private const string PARM_COPAY = "@Copay";
        private const string PARM_NEXT_RESPONSIBILITY = "@NextResponsibility";

        private const string PARM_COMMENTS = "@Comments";
        // private const string PARM_TRANSFER_AMOUNT = "@TransferAmount";

        private const string PARM_COINSURANCE = "@Coinsurance";
        private const string PARM_DEDUCTABLES = "@Deductables";
        private const string PARM_PATIENT_RESPONSIBILITY = "@PatientResponsibility";

        private const string PARM_ERADTL_ID = "@ERADtlId";
        private const string PARM_CLEARNING_HOUSE_ID = "@ClearingHouseId";


        private const string PARM_ENTITY = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAID_FROM = "@PaidFrom";
        private const string PARM_PAID_TO = "@PaidTo";

        //By KR 21 May 2016.
        private const string PARM_REPORT_TYPE = "@ReportType";
        private const string PARM_FROM_DATE = "@FROM";
        private const string PARM_TO_DATE = "@To";
        private const string PARM_PROGRAM = "@ProgramId";
        private const string PARM_MUID = "@MUID";
        private const string PARM_GROUP_ID = "@GroupId";
        private const string PARM_CLAIM_DATE_FROM = "@ClaimDateFrom";
        private const string PARM_CLAIM_DATE_TO = "@ClaimDateTo";
        private const string PARM_PROVIDER_NAME = "@ProviderName";

        //by Mu



        #region clinical reports
        private const string PARM_CREATE_DATE_FROM = "@CreateDateFrom";
        private const string PARM_CREATE_DATE_TO = "@CreateDateTo";
        private const string PARM_NOTE_STATUS = "@NoteStatus";
        private const string PARM_DURATION_FROM = "@DurationFrom";
        private const string PARM_DURATION_TO = "@DurationTo";
        private const string PARM_NOTE_TYPE = "@NoteType";

        private const string PARM_REFPROVIDER_ID = "@RefProviderId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ADMINSTERED = "@IsAdministered";
        private const string PARM_VOID_DOSE = "@VoidDose";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_PATIENT_FIRST_NAME = "@PatientFirstName";
        private const string PARM_PATIENT_LAST_NAME = "@PatientLastName";
        private const string PARM_INCLUDE_IN_ACTIVE = "@IncludeInActive";
        private const string PARM_ALLERGY_AND_OR = "@AllergyAndOr";
        private const string PARM_REACTION = "@Reaction";
        private const string PARM_ALLERGY = "@Allergy";
        private const string PARM_ALLERGY_STATUS = "@AllergyStatus";

        private const string PARM_DOSFROM = "@DOSFrom";
        private const string PARM_DOSTO = "@DOSTo";
        private const string PARM_SYSTOLIC_FROM = "@SystolicFrom";
        private const string PARM_SYSTOLIC_TO = "@SystolicTo";
        private const string PARM_TEMP_FROM = "@TempFrom";
        private const string PARM_TEMP_TO = "@TempTo";
        private const string PARM_HEIGHT_FROM = "@HeightFrom";
        private const string PARM_HEIGHT_TO = "@HeightTo";
        private const string PARM_SPO2_FROM = "@SPO2From";
        private const string PARM_SPO2_TO = "@SPO2To";
        private const string PARM_DIASTOLIC_FROM = "@DiastolicFrom";
        private const string PARM_DIASTOLIC_TO = "@DiastolicTo";
        private const string PARM_Resp_From = "@RespFrom";
        private const string PARM_RESP_TO = "@RespTo";
        private const string PARM_BMI_FROM = "@BMIFrom";
        private const string PARM_BMI_TO = "@BMITo";
        private const string PARM_PULSE_RATE_FROM = "@PulseRateFrom";
        private const string PARM_PULSE_RATE_TO = "@PulseRateTo";
        private const string PARM_WEIGHT_FROM = "@WeightFrom";

        private const string PARM_WEIGHT_TO = "@WeightTo";
        private const string PARM_BSA_FROM = "@BSAFrom";
        private const string PARM_BSA_TO = "@BSATo";

        private const string PARM_LABORATORY_IDS = "@Laboratory";
        private const string PARM_TEST_ID = "@Test";
        private const string PARM_ORDERNO = "@OrderNumber";

        private const string PARM_ORDER_STATUS = "@Status";
        private const string PARM_ORDER_DATEFROM = "@DateFrom";
        private const string PARM_ORDER_DATETO = "@DateTo";
        private const string PARM_PROCEDURE = "@Procedure";
        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";



        private const string PARM_CHRONICITY_LEVEL = "@ChronicityLevel";
        private const string PARM_PROBLEM = "@Problem";
        private const string PARM_SEVERITY = "@Severity";
        private const string PARM_PROBLEM_STATUS = "@Status";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";


        private const string PARM_MEDICATION_STATUS = "@MedicationStatus";
        private const string PARM_MEDICATION = "@Medication";
        private const string PARM_MEDICATION_AND_OR = "@MedicationAndOr";

        private const string PARM_IMMUNIZATION_ADMINISTERED_BY = "@AdministeredBy";
        private const string PARM_IMMUNIZATION_CATEGORY_ID = "@Category";
        private const string PARM_IMMUNIZATION_VACCINE_ID = "@Vaccine";
        private const string PARM_IMMUNIZATION_ROUTE_ID = "@Route";
        private const string PARM_IMMUNIZATION_SITE_ID = "@Site";
        private const string PARM_IMMUNIZATION_REACTION_ID = "@Reaction";
        private const string PARM_IMMUNIZATION_ALERT_ID = "@Alert";
        private const string PARM_IMMUNIZATION_DATE_FROM = "@DateFrom";
        private const string PARM_IMMUNIZATION_DATE_TO = "@Dateto";

        private const string PARM_RESULTNO = "@ResultNumber";

        private const string PARM_PROVIDER = "@Provider";
        private const string PARM_ASSIGNEE_PROVIDER = "@AssigneeProvider";
        private const string PARM_PROCEDURES = "@Procedures";

        private const string PARM_ASSIGNEEPROVIDER_ID = "@AssigneeProviderId";

        private const string PARM_ASSIGNEE_PROVIDER_ID = "@AssigneeProviderId";
        private const string PARM_PRESCRIPTION_AND_OR = "@AndOr";
        private const string PARM_PRESCRIPTION_PHARMACY = "@Pharmacy";
        private const string PARM_ADVANCED_SEARCH = "@IsAdvanceSearch";
        private const string PARM_PRIOVIDERID = "@ProviderID";

        private const string PARM_DOB = "@DOB";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_PROGRAM_STATUS = "@ProgramStatus";
        private const string PARM_PRACTICE_IDS = "@PracticeIds";
        private const string PARM_FACILITY_IDS = "@FacilityIds";
        private const string PARM_FROM_TIME_COMPLETED = "@FromTimeCompleted";
        private const string PARM_TO_TIME_COMPLETED = "@ToTimeCompleted";
        private const string PARM_CONSENT_STATUS = "@ConsentStatus";
        private const string PARM_NO_OF_PROBLEM_FROM = "@NoOfProblemFrom";
        private const string PARM_NO_OF_PROBLEM_TO = "@NoOfProblemTo";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_CARE_MANAGER = "@CareManager";
        private const string PARM_CARE_COORDINATOR = "@CareCoordinator";
        private const string PARM_CARE_GIVER = "@CareGiver";
        private const string PARM_SUMMARY_REPORT = "@SummaryReport";
        private const string PARM_IS_AMENDEDNOTE = "@IsAmendedNote";


        #endregion

        #endregion

        #region Constructors
        public DALReports()
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


        #region "Functions Patient Payments"



        public DSReports GetReportDetail(Dictionary<string, object> ReportsParamaters, String ReportName, bool IsAppointmentClaimSummaryReprot = false)
        {
            DSReports ds = new DSReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(ReportsParamaters.Count);
                int counter = 0;
                //Adding paramaters to DBmanager.
                foreach (var item in ReportsParamaters)
                {
                    dbManager.AddParameters(counter, item.Key, string.IsNullOrEmpty(item.Value.ToString()) ? DBNull.Value : item.Value);
                    counter++;
                }
                if (ReportName.Equals("Advance Payment".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_ADVANCE_PAYMENT, ds, ds.DT_Report_AdvancePayment.TableName);
                }
                else if (ReportName.Equals("Collected Copayment".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_COLLECTED_COPAYMENT, ds, ds.DT_Reports_CollectedCopayment.TableName);
                }
                else if (ReportName.Equals("Unallocated copayment".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_UNALLOCATED_COPAYMENT, ds, ds.DT_Unallocated_Copayment.TableName);
                }
                else if (ReportName.Equals("Diagnosis Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_DIAGNOSIS_ANALYSIS, ds, ds.DT_Report_DiagnosisAnalysis.TableName);
                }
                else if (ReportName.Equals("Patient List".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_PATIENT_LIST, ds, ds.DT_Reports_PatientList.TableName);
                }
                else if (ReportName.Equals("Procedure Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_PROCEDURE_ANALYSIS, ds, ds.DT_Reports_ProcedureAnalysis.TableName);
                }
                else if (ReportName.Equals("Outstanding Balances".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_OUTSTANDING_BALANCES, ds, ds.DT_Reports_OutStandingBalances.TableName);
                }
                else if (ReportName.Equals("Insurance Analysis Summary".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_INSURANCE_ANALYSIS_SUMMARY, ds, ds.DT_Reports_InsuranceAnalysisSummary.TableName);
                }
                else if (ReportName.Equals("Insurance Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_INSURANCE_ANALYSIS, ds, ds.DT_Reports_InsuranceAnalysis.TableName);
                }
                else if (ReportName.Equals("Total Resource appointments".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_RESOURCE_APPOINTMENT, ds, ds.DT_Reports_ResourceAppointment.TableName);
                }
                else if (ReportName.Equals("Total Follow-Up Appointments".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_FOLLOWUP_APPOINTMENTS, ds, ds.DT_Ttl_Followup_Appointments.TableName);
                }
                else if (ReportName.Equals("Total Provider appointments".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_PROVIDER_APPOINTMENT, ds, ds.DT_Reports_ProviderAppointment.TableName);
                }
                else if (ReportName.Equals("Enterprise Scheduling".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_ENTERPRISE_SCHEDULING, ds, ds.DT_Report_EnterpriseScheduling.TableName);
                }
                else if (ReportName.Equals("Daily Resource Appointments".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_DAILY_APPOINTMENTS_RESOURCE, ds, ds.DT_Reports_DailyAppointmentsResource.TableName);
                }
                else if (ReportName.Equals("Daily Provider Appointments".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_DAILY_APPOINTMENTS_PROVIDER, ds, ds.DT_Reports_DailyAppointmentsProvider.TableName);
                }
                else if (ReportName.Equals("Check In And Check Out Duration".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_CHECKIN_CHECKOUT_DURATION, ds, ds.DT_Reports_CheckinCheckoutDuration.TableName);
                }
                else if (ReportName.Equals("Revenue By Facility".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_REVENUE_BY_FACILITY, ds, ds.DT_Reports_Revenue_By_Facility.TableName);
                }
                else if (ReportName.Equals("Enterprise Revenue".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_ENTERPRISE_REVENUE, ds, ds.DT_Reports_EnterpriseRevenue.TableName);
                }
                else if (ReportName.Equals("Revenue By Provider".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_REVENUE_BY_PROVIDER, ds, ds.DT_Reports_Revenue_By_Provider.TableName);
                }
                else if (ReportName.Equals("Provider Analysis By Plan".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_PROVIDER_ANALYSIS_BY_INSURANCEPLAN, ds, ds.DT_Reports_Provider_Analysis_By_InsurancePlan.TableName);
                }
                else if (ReportName.Equals("Charges List".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_CHARGESLIST, ds, ds.DT_Reports_ChargesList.TableName);
                }
                else if (ReportName.Equals("Provider Procedure Utilization".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_PROVIDER_PROCEDURE_UTILIZATION, ds, ds.DT_Reports_Provider_Procedure_Utilization.TableName);
                }
                else if (ReportName.Equals("Insurance Plan AR".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_INSURANCE_PLAN_AR, ds, ds.DT_Reports_InsurancePlanAR.TableName);
                }
                else if (ReportName.Equals("Patient AR".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_PATIENT_AR, ds, ds.DT_Reports_PatientAR.TableName);
                }
                else if (ReportName.Equals("Payment Entries".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_PAYMENT_ENTRIES, ds, ds.DT_Reports_PaymentEnteries.TableName);
                }
                else if (ReportName.Equals("Enterprise AR Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_ENTERPRISE_AR_ANALYSIS, ds, ds.DT_Reports_EnterpriseARAnalysis.TableName);
                }
                else if (ReportName.Equals("AR Aging Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_ARAGING_ANALYSIS, ds, ds.DT_Reports_ARAging_Analysis.TableName);
                }
                else if (ReportName.Equals("AR Aging Analysis MPS".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_ARAGING_ANALYSIS_MPS, ds, ds.DT_Reports_ARAging_Analysis.TableName);
                }
                else if (ReportName.Equals("Payment Entries MPS".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_SP_PAYMENT_ENTRIES, ds, ds.DT_Reports_PaymentEnteries.TableName);
                }
                else if (ReportName.Equals("Revenue By Plan".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTS_REVENUE_BY_INSURANCE_PLAN, ds, ds.DT_Reports_Revenue_By_InsurancePlan.TableName);
                }
                else if (ReportName.Equals("Financial Analysis At CPT Level".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FINANCIAL_ANALYSIS_AT_CPT, ds, ds.DT_Reports_Financial_Analysis_At_CPT.TableName);
                }
                else if (ReportName.Equals("Beginning AR Ending AR".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BEGINING_AR_ENDING_AR, ds, ds.DT_Report_Beginning_AR_Ending_AR.TableName);
                }
                else if (ReportName.Equals("Payments by Users".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PAYMENTS_BY_USERS, ds, ds.DT_Report_Payments_By_User.TableName);
                }
                else if (ReportName.Equals("Aging Summary Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AGING_SUMMARY_ANALYSIS, ds, ds.DT_Aging_Summary_Analysis.TableName);
                }
                else if (ReportName.Equals("Encounter without Claims".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ENCOUNTER_WITHOUT_CLAIMS, ds, ds.DT_Encounter_Without_Claims.TableName);
                }
                else if (ReportName.Equals("Charges By Users".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHARGES_BY_USERS, ds, ds.DT_Charges_By_Users.TableName);
                }
                else if (ReportName.Equals("Beginning AR Ending AR Facility".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BEGINNING_AR_ENDING_AR_FACILITY, ds, ds.DT_Beginning_AR_Ending_AR_Facility.TableName);
                }
                else if (ReportName.Equals("Claim Comments By User".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_COMMENT_BY_USER, ds, ds.DT_Reports_Claim_Comments_By_Users.TableName);
                }
                else if (ReportName.Equals("User Activity Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_REPORT, ds, ds.DT_User_Activity_Report.TableName);
                }
                else if (ReportName.Equals("Claim Submit Status".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUBMIT_STATUS, ds, ds.DT_Claim_Submit_Status.TableName);
                }
                else if (ReportName.Equals("Aging Detail Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AGING_DETAIL_ANALYSIS, ds, ds.DT_Aging_Detail_Analysis.TableName);
                }
                else if (ReportName.Equals("Daily Appointment Reminders".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DAILY_APPOINTMENT_REMINDER, ds, ds.DT_Daily_Appointment_Reminder.TableName);
                }
                else if (ReportName.Equals("AR Reconciliation Report Detial".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AR_RECONCILIATION_REPORT, ds, ds.DT_AR_RECONCILIATION_REPORT.TableName);
                }
                else if (ReportName.Equals("AR Reconciliation Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AR_RECONCILIATION_REPORT, ds, ds.DT_AR_RECONCILIATION_REPORT.TableName);
                }
                else if (ReportName.Equals("Patient Overpayment".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_OVER_PAYMENT, ds, ds.DT_Reports_Patient_OverPayment.TableName);
                }
                else if (ReportName.Equals("Zero Paid Claim".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ZERO_PAID_CLAIMS, ds, ds.DT_Zero_Paid_Claims.TableName);
                }
                else if (ReportName.Equals("Claim Over Paid By Insurance".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_OVERPAID_BY_INSURANCE, ds, ds.DT_Claim_OverPaid_Insurance.TableName);
                }
                else if (ReportName.Equals("Claim Under Paid By Insurance".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_UNDERPAID_BY_INSURANCE, ds, ds.DT_Claim_UnderPaid_Insurance.TableName);
                }
                else if (ReportName.Equals("Anesthesia Overlapping".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANESTHESIA_OVERLAPPING, ds, ds.DT_Anesthesia_Overlapping.TableName);
                }
                else if (ReportName.Equals("Historical Aging Summary Analysis".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ZERO_PAID_CLAIMS, ds, ds.DT_Zero_Paid_Claims.TableName);
                }
                else if (ReportName.Equals("CDS Alert Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_ALERTS_REPORT, ds, ds.DT_CDS_Alerts.TableName);
                }
                else if (ReportName.Equals("Daily Copay Sheet".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DAILY_COPAY_SHEET, ds, ds.DT_DailyCopaySheet.TableName);
                }
                else if (ReportName.Equals("Incorrect Balance by Voided Claims".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INCORRECT_BALANCE_BY_VOIDED_CLAIMS, ds, ds.DT_IncorrectBalancebyVoidedClaims.TableName);
                }
                else if (ReportName.Equals("Progress Note".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROGRESS_NOTE, ds, ds.DT_Progress_Note.TableName);
                }
                else if (ReportName.Equals("Claims Never submitted to Insurance".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIMS_NEVER_SUBMITTED, ds, ds.DT_Claims_Never_Submitted.TableName);
                }
                else if (ReportName.Equals("Allergies".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_ALLERGIES, ds, ds.DT_Allergies.TableName);
                }
                else if (ReportName.Equals("Progress Note - Amendment".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROGRESS_NOTE_AMENDED, ds, ds.DT_Progress_Note.TableName);
                }
                else if (ReportName.Equals("Claim Status Dashboard".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUBMIT_DASHBOARD, ds, ds.DT_Claim_Status_Dashboard.TableName);
                }
                else if (ReportName.Equals("POS Surveys".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_POS_SURVERY, ds, ds.DT_POS_Survey.TableName);
                }
                else if (ReportName.Equals("Unclaimed Appointments Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_UNCLAIMED_APPOINTMENT, ds, ds.DT_Unclaimed_Appointment.TableName);
                }
                else if (ReportName.Equals("Problems".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_PROBLEMS, ds, ds.DT_Problems_Report.TableName);
                }
                else if (ReportName.Equals("Procedures".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_PROCEDURES, ds, ds.DT_Procedures_Report.TableName);
                }
                else if (ReportName.Equals("Phone Encounter".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_PHONE_ENCOUNTER, ds, ds.DT_PhoneEncounter.TableName);
                }
                else if (ReportName.Equals("Immunization".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_IMMUNIZATION, ds, ds.DT_Immunization_Report.TableName);
                }
                else if (ReportName.Equals("Medications".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_MEDICATION, ds, ds.DT_Medication_Report.TableName);
                }
                else if (ReportName.Equals("Vitals".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_VITALS, ds, ds.DT_Vitals_Report.TableName);
                }
                else if (ReportName.Equals("User Audit Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AUDIT_USER, ds, ds.DT_Audit_User.TableName);
                }
                else if (ReportName.Equals("Appointments Vs Claim".ToLower().Trim()))
                {
                    if (IsAppointmentClaimSummaryReprot)
                        ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_VS_CLAIM_SUMMARY_A, ds, ds.DT_AppointmentVsClaimSummaryA.TableName);
                    else
                        ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_VS_CLAIM_SUMMARY_B, ds, ds.DT_AppointmentVsClaimSummaryB.TableName);
                }
                else if (ReportName.Equals("Appointments Vs Claim Detail".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_APPOINTMENT_VS_CLAIM_DETAIL, ds, ds.DT_AppointmentVsClaimDetail.TableName);

                }
                else if (ReportName.Equals("Orders_Lab".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_LAB, ds, ds.DT_Reports_Orders_Lab.TableName);
                }
                else if (ReportName.Equals("Orders_Radiology".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_RADIOLOGY, ds, ds.DT_Reports_Orders_Lab.TableName);
                }
                else if (ReportName.Equals("Orders_Procedure".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_PROCEDURE, ds, ds.DT_Reports_Orders_Lab.TableName);
                }
                else if (ReportName.Equals("Orders_Consultation".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_CONSULTATION, ds, ds.DT_Reports_Orders_Lab.TableName);
                }
                else if (ReportName.Equals("Orders_Prescription".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_PRESECRIPTION, ds, ds.DT_Reports_Orders_Prescription.TableName);
                }
                else if (ReportName.Equals("Results_Lab".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_RESULTS_LAB, ds, ds.DT_Results_Lab.TableName);
                }
                else if (ReportName.Equals("Results_Radiology".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_RESULT_RADIOLOGY, ds, ds.DT_Results_Lab.TableName);
                }
                else if (ReportName.Equals("Results_Consultation".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_RESULT_CONSULTATION, ds, ds.DT_Results_Lab.TableName);
                }
                else if (ReportName.Equals("Patient Statement Preference".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_PREFERENCE, ds, ds.DT_PatientStatementPreference.TableName);
                }
                else if (ReportName.Equals("Claims Under Paid by Primary Insurance".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIMUNDER_PAIDBY_PRIMARYINSURANCE, ds, ds.DT_ClaimsUnderPaidByPrimaryInsurance.TableName);
                }
                else if (ReportName.Equals("ARO Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ARO_REPORT, ds, ds.DT_ARO_Report.TableName);
                }
                else if (ReportName.Equals("AUP Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AUP_REPORT, ds, ds.DT_AUP_Report.TableName);
                }
                else if (ReportName.Equals("Claims In Collection".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ClaimInCollectionReport_REPORT, ds, ds.DT_ClaimsInCollection.TableName);
                }
                else if (ReportName.Equals("monthly payment trend".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAKPI_MONTHL_PAYMENT_TREND, ds, ds.DT_Monthly_Payment_Trend_Report.TableName);
                }
                else if (ReportName.Equals("Billing Inquiry by Provider".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_INQUIRY_BY_PROVIDER, ds, ds.DT_Biliing_Inquiry_By_Provider.TableName);
                }
                else if (ReportName.Equals("Secondary Claims Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIMS_UNDER_SECONDARY_INSURANCE, ds, ds.DT_Scecondary_Insurance_Claim.TableName);
                }
                else if (ReportName.Equals("Insurance AR Plan Report".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_AR_PLAN, ds, ds.DT_Insurance_AR_Plan.TableName);
                }
                else if (ReportName.Equals("Claim Follow Up".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_FOLLOW_UP, ds, ds.DT_Claim_Follow_Up.TableName);
                }
                else if (ReportName.Equals("Claim Submission History".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUBMISSION_HISTORY, ds, ds.DT_Claim_Submission_History_Report.TableName);
                }
                else if (ReportName.Equals("Claim Scrubber Errors".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SCRUBBER_ERRORS, ds, ds.DT_ClaimScrubberErrors.TableName); 
                }
                else if (ReportName.Equals("MIPS Improvement Activity".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MIPS_IMPROVEMENT_ACTIVITY, ds, ds.DT_MIPS_Improvement_Activity.TableName);
                }
                else if (ReportName.Equals("Drug Code Cost".ToLower().Trim()))
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DRUG_CODE_COST, ds, ds.DT_Drug_Code_Cost.TableName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPayment::LoadAdvancePaymentsReport", "Reports Sp", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        #endregion

        #region MU Report Data


        public DSReports LoadMUReportData(long providerId, long PatientId, int reportType, string fromDate, string toDate, string MUID, string rptName)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            DSReports ds = new DSReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                //DataTable dtTemp = ds.MU_AutomatedMeasure;
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(6);

                if (providerId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_REPORT_TYPE, reportType);

                if (!string.IsNullOrEmpty(fromDate))
                    dbManager.AddParameters(3, PARM_FROM_DATE, Convert.ToDateTime(fromDate));
                else
                    dbManager.AddParameters(3, PARM_FROM_DATE, DBNull.Value);
                if (!string.IsNullOrEmpty(toDate))
                    dbManager.AddParameters(4, PARM_TO_DATE, Convert.ToDateTime(toDate));
                else
                    dbManager.AddParameters(4, PARM_TO_DATE, DBNull.Value);
                if (string.IsNullOrEmpty(MUID))
                    dbManager.AddParameters(5, PARM_MUID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_MUID, MUID);

                if (rptName == "MU Stage 1 Report")
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MU1_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });
                }
                else if (rptName == "MU Stage 2 Report")
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MU2_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });
                }
                else if (rptName == "MU Stage 2 Report Latest")
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MU2LATEST_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });
                }
                else if (rptName == "MU Stage 3 Report")
                {
                    ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MU3_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });
                }

                /*if (dtTemp != null)
                {
                    if (isViewProblemList == "1" || isPrintProblemList == "1")
                    {
                        bool isViewAction = isViewProblemList == "1" ? true : false;
                        bool isPrintAcion = isPrintProblemList == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }*/
                //dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                if (rptName == "MU Stage 1 Report")
                {
                    MDVLogger.DALErrorLog("DALReports::LoadMUReportData", PROC_MU1_SELECT, ex);
                }
                else if (rptName == "MU Stage 2 Report")
                {
                    MDVLogger.DALErrorLog("DALReports::LoadMUReportData", PROC_MU2_SELECT, ex);
                }
                else if (rptName == "MU Stage 2 Report Latest")
                {
                    MDVLogger.DALErrorLog("DALReports::LoadMUReportData", PROC_MU2LATEST_SELECT, ex);
                }
                else if (rptName == "MU Stage 3 Report")
                {
                    MDVLogger.DALErrorLog("DALReports::LoadMUReportData", PROC_MU3_SELECT, ex);
                }
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReports LoadACIGroupData(long providerId, long PatientId, int reportType, string fromDate, string toDate, string MUID, string rptName,string GroupId)
        {
            DSReports ds = new DSReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (providerId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_REPORT_TYPE, reportType);

                if (!string.IsNullOrEmpty(fromDate))
                    dbManager.AddParameters(3, PARM_FROM_DATE, Convert.ToDateTime(fromDate));
                else
                    dbManager.AddParameters(3, PARM_FROM_DATE, DBNull.Value);
                if (!string.IsNullOrEmpty(toDate))
                    dbManager.AddParameters(4, PARM_TO_DATE, Convert.ToDateTime(toDate));
                else
                    dbManager.AddParameters(4, PARM_TO_DATE, DBNull.Value);
                if (string.IsNullOrEmpty(MUID))
                    dbManager.AddParameters(5, PARM_MUID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_MUID, MUID);
                if (string.IsNullOrEmpty(GroupId))
                    dbManager.AddParameters(6, PARM_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_GROUP_ID, GroupId);

                ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACI_GROUP_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });
               
                
                return ds;
            }
            catch (Exception ex)
            {
              
                    MDVLogger.DALErrorLog("DALReports::LoadACIGroupData", PROC_ACI_GROUP_SELECT, ex);
               
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReports LoadACIIndividualData(long providerId, long PatientId, int reportType, string fromDate, string toDate, string MUID, string rptName)
        {
            DSReports ds = new DSReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(6);

                if (providerId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_REPORT_TYPE, reportType);

                if (!string.IsNullOrEmpty(fromDate))
                    dbManager.AddParameters(3, PARM_FROM_DATE, Convert.ToDateTime(fromDate));
                else
                    dbManager.AddParameters(3, PARM_FROM_DATE, DBNull.Value);
                if (!string.IsNullOrEmpty(toDate))
                    dbManager.AddParameters(4, PARM_TO_DATE, Convert.ToDateTime(toDate));
                else
                    dbManager.AddParameters(4, PARM_TO_DATE, DBNull.Value);
                if (string.IsNullOrEmpty(MUID))
                    dbManager.AddParameters(5, PARM_MUID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_MUID, MUID);
                

                ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACI_INDIVIDUAL_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });


                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALReports::LoadACIIndividualData", PROC_ACI_INDIVIDUAL_SELECT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReports LoadACIGroupData1(long providerId, long PatientId, int reportType, string fromDate, string toDate, string MUID, string rptName, Int64 GroupId)
        {
            DSReports ds = new DSReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (providerId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_REPORT_TYPE, reportType);

                if (!string.IsNullOrEmpty(fromDate))
                    dbManager.AddParameters(3, PARM_FROM_DATE, Convert.ToDateTime(fromDate));
                else
                    dbManager.AddParameters(3, PARM_FROM_DATE, DBNull.Value);
                if (!string.IsNullOrEmpty(toDate))
                    dbManager.AddParameters(4, PARM_TO_DATE, Convert.ToDateTime(toDate));
                else
                    dbManager.AddParameters(4, PARM_TO_DATE, DBNull.Value);
                if (string.IsNullOrEmpty(MUID))
                    dbManager.AddParameters(5, PARM_MUID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_MUID, MUID);
                if (GroupId == 0)
                    dbManager.AddParameters(6, PARM_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_GROUP_ID, GroupId);

                ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACI_GROUP_SELECT, ds, new List<string> { ds.MU_AutomatedMeasure.TableName, ds.MU_AutomatedMeasurePatientWise.TableName });


                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALReports::LoadACIGroupData", PROC_ACI_GROUP_SELECT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSReports LoadACIIndividualData_BonusMeasures()
        {
            DSReports ds = new DSReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACI_INDIVIDUAL_BONUS_SELECT, ds, ds.MU_AutomatedMeasure.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadACIIndividualData_BonusMeasures", PROC_ACI_INDIVIDUAL_SELECT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Clinical Reports
        public List<CPhoneEncounterModel> LoadPhoneEncounterReport(string CreateDateFrom, string CreateDateTo, string NoteStatus, string DurationFrom, string DuartionTo, string ProviderId, string RefProviderId, string PracticeId)
        {
            List<CPhoneEncounterModel> listModel = new List<CPhoneEncounterModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                if (string.IsNullOrEmpty(CreateDateFrom))
                {
                    dbManager.AddParameters(0, PARM_CREATE_DATE_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_CREATE_DATE_FROM, CreateDateFrom);
                }
                if (string.IsNullOrEmpty(CreateDateTo))
                {
                    dbManager.AddParameters(1, PARM_CREATE_DATE_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_CREATE_DATE_TO, CreateDateTo);
                }
                if (string.IsNullOrEmpty(NoteStatus))
                {
                    dbManager.AddParameters(2, PARM_NOTE_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_NOTE_STATUS, NoteStatus);
                }
                if (string.IsNullOrEmpty(DurationFrom))
                {
                    dbManager.AddParameters(3, PARM_DURATION_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_DURATION_FROM, DurationFrom);
                }
                if (string.IsNullOrEmpty(DuartionTo))
                {
                    dbManager.AddParameters(4, PARM_DURATION_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_DURATION_TO, DuartionTo);
                }
                if (string.IsNullOrEmpty(ProviderId))
                {
                    dbManager.AddParameters(5, PARM_PROVIDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_PROVIDER_ID, ProviderId);
                }
                if (string.IsNullOrEmpty(RefProviderId))
                {
                    dbManager.AddParameters(6, PARM_REFPROVIDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_REFPROVIDER_ID, RefProviderId);
                }
                if (string.IsNullOrEmpty(PracticeId))
                {
                    dbManager.AddParameters(7, PARM_PRACTICE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_PRACTICE_ID, PracticeId);
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_PHONE_ENCOUNTER);
                CPhoneEncounterModel model = null;
                while (reader.Read())
                {
                    model = new CPhoneEncounterModel();
                    model.NotesId = Convert.ToInt64(reader["NotesId"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.CreatedOn = GetDateMMDDYYY(reader["CreatedOn"].ToString());
                    model.PatientId = Convert.ToInt64(reader["PatientId"]);
                    model.ProviderId = Convert.ToInt64(reader["ProviderId"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.DOB = GetDateMMDDYYY(reader["DOB"].ToString());
                    model.HomePhone = Convert.ToString(reader["HomePhoneNo"]);
                    model.NotesStatus = Convert.ToString(reader["NoteStatus"]);
                    model.NotesDuration = Convert.ToString(reader["Duration"]);
                    model.PracticeName = Convert.ToString(reader["PracticeName"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.RefProviderName = Convert.ToString(reader["ReferringProviderName"]);
                    string encounterCharge = reader["EncounterCharge"].ToString();
                    model.EncounterCharge = string.IsNullOrEmpty(encounterCharge) ? 0 : Convert.ToDouble(encounterCharge);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadPhoneEncounterReport", PROC_CLINICAL_PHONE_ENCOUNTER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<CProgressNoteReportModel> LoadProgressNotesReport(string CreateDateFrom, string CreateDateTo, string NoteStatus, long NoteType, string ProviderId, string FacilityId, string RefProviderId, string PracticeId, bool IsAmendedNote)
        {
            List<CProgressNoteReportModel> listModel = new List<CProgressNoteReportModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);
                if (string.IsNullOrEmpty(CreateDateFrom))
                {
                    dbManager.AddParameters(0, PARM_CREATE_DATE_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_CREATE_DATE_FROM, CreateDateFrom);
                }
                if (string.IsNullOrEmpty(CreateDateTo))
                {
                    dbManager.AddParameters(1, PARM_CREATE_DATE_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_CREATE_DATE_TO, CreateDateTo);
                }
                if (string.IsNullOrEmpty(NoteStatus))
                {
                    dbManager.AddParameters(2, PARM_NOTE_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_NOTE_STATUS, NoteStatus);
                }
                if (NoteType <= 0)
                {
                    dbManager.AddParameters(3, PARM_NOTE_TYPE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_NOTE_TYPE, NoteType);
                }
                if (string.IsNullOrEmpty(ProviderId))
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);
                }
                if (string.IsNullOrEmpty(RefProviderId))
                {
                    dbManager.AddParameters(5, PARM_REFPROVIDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_REFPROVIDER_ID, RefProviderId);
                }
                if (string.IsNullOrEmpty(PracticeId))
                {
                    dbManager.AddParameters(6, PARM_PRACTICE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_PRACTICE_ID, PracticeId);
                }
                if (string.IsNullOrEmpty(FacilityId))
                {
                    dbManager.AddParameters(7, PARM_FACILITY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_FACILITY_ID, FacilityId);
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (IsAmendedNote == false)
                {
                    dbManager.AddParameters(9, PARM_IS_AMENDEDNOTE, null);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_IS_AMENDEDNOTE, IsAmendedNote);
                }

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_PROGRESS_NOTE);
                CProgressNoteReportModel model = null;
                while (reader.Read())
                {
                    model = new CProgressNoteReportModel();
                    model.NotesId = Convert.ToInt64(reader["NotesId"]);
                    model.CreatedOn = GetDateMMDDYYY(reader["CreatedOn"].ToString());
                    model.PatientId = Convert.ToInt64(reader["PatientId"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.ProviderId = Convert.ToInt64(reader["ProviderId"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.DOB = GetDateMMDDYYY(reader["DOB"].ToString());
                    model.HomePhone = Convert.ToString(reader["HomePhoneNo"]);
                    model.NotesStatus = Convert.ToString(reader["NoteStatus"]);
                    model.NotesType = Convert.ToString(reader["NotesType"]);
                    model.PracticeName = Convert.ToString(reader["PracticeName"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.RefProviderName = Convert.ToString(reader["ReferringProviderName"]);
                    model.FacilityName = Convert.ToString(reader["FacilityName"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadProgressNotesReport", PROC_CLINICAL_PROGRESS_NOTE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<CAllergiesFillModel> LoadAllergiesReport(CAllergiesModel model)
        {
            List<CAllergiesFillModel> listModel = new List<CAllergiesFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }
                // if user needs all patients then sending null else only active records
                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                dbManager.AddParameters(4, PARM_ALLERGY_AND_OR, model.AllergyAND);
                if (string.IsNullOrEmpty(model.Allergy))
                {
                    dbManager.AddParameters(5, PARM_ALLERGY, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_ALLERGY, model.Allergy);
                }
                if (string.IsNullOrEmpty(model.AllergyReaction))
                {
                    dbManager.AddParameters(6, PARM_REACTION, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_REACTION, model.AllergyReaction);
                }

                if (string.IsNullOrEmpty(model.AllergyStatus))
                {
                    dbManager.AddParameters(7, PARM_ALLERGY_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_ALLERGY_STATUS, model.AllergyStatus);
                }


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                #endregion
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_ALLERGIES);
                CAllergiesFillModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CAllergiesFillModel();
                    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Allergy = MDVUtility.CheckStringNull(reader["Allergy"]);
                    modelFill.Reaction = MDVUtility.CheckStringNull(reader["Reaction"]);
                    modelFill.OnSetDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["OnSetDate"]));
                    modelFill.AllergyStatus = MDVUtility.CheckBooleanNull(reader["AllergyStatus"]) ? "Active" : "Inactive";
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadAllergiesReport", PROC_CLINICAL_ALLERGIES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<COrdersFillModel> LoadOrdersReport(COrdersModel model)
        {
            List<COrdersFillModel> listModel = new List<COrdersFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            var spName = string.Empty;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                if (model.ProviderId == 0)
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, model.ProviderId);
                }

                if (string.IsNullOrEmpty(model.LaboratoryIds))
                {
                    dbManager.AddParameters(5, PARM_LABORATORY_IDS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_LABORATORY_IDS, model.LaboratoryIds);
                }
                if (string.IsNullOrEmpty(model.CPTCode))
                {
                    dbManager.AddParameters(6, PARM_TEST_ID, null);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_TEST_ID, model.CPTCode);
                }
                if (string.IsNullOrEmpty(model.OrderNo))
                {
                    dbManager.AddParameters(7, PARM_ORDERNO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_ORDERNO, model.OrderNo);
                }

                if (string.IsNullOrEmpty(model.OrderStatus))
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, model.OrderStatus);
                }

                if (!string.IsNullOrEmpty(model.DateFrom))
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, Convert.ToDateTime(model.DateFrom));
                }
                else
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.DateTo))
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, Convert.ToDateTime(model.DateTo));
                }
                else
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, DBNull.Value);
                }
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(11, "@IsSummaryReport", model.IsSummaryReport);
                #endregion

                if (model.OrderType == "Lab")
                {
                    spName = PROC_CLINICAL_LAB;
                }
                else if (model.OrderType == "Radiology")
                {
                    spName = PROC_CLINICAL_RADIOLOGY;
                }
                if (!string.IsNullOrEmpty(spName))
                {
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, spName);
                    COrdersFillModel modelFill = null;

                    while (reader.Read())
                    {
                        modelFill = new COrdersFillModel();
                        modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                        modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                        modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                        modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                        modelFill.Test = MDVUtility.CheckStringNull(reader["Test"]);
                        modelFill.Laboratory = MDVUtility.CheckStringNull(reader["Laboratory"]);
                        modelFill.OrderNo = MDVUtility.CheckStringNull(reader["OrderNumber"]);
                        modelFill.OrderStatus = MDVUtility.CheckStringNull(reader["Status"]);
                        modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                        modelFill.PatientId = MDVUtility.CheckLongNull(reader["PatientId"]);
                        modelFill.OrderDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["Date"])) + ' ' + (reader["Time"]);
                        if (model.IsSummaryReport)
                        {
                            modelFill.InsuranceName = MDVUtility.ToStr(reader["InsuranceName"]);
                        }
                        listModel.Add(modelFill);
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadOrdersReport", spName, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<CVitalsFillModel> LoadVitalsReport(CVitalsModel model)
        {
            List<CVitalsFillModel> listModel = new List<CVitalsFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(28);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }
                //dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, model.IncludeInactivePatient);
                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                if (string.IsNullOrEmpty(model.DOSFrom))
                {
                    dbManager.AddParameters(4, PARM_DOSFROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_DOSFROM, model.DOSFrom);
                }
                if (string.IsNullOrEmpty(model.DOSTo))
                {
                    dbManager.AddParameters(5, PARM_DOSTO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_DOSTO, model.DOSTo);
                }
                if (string.IsNullOrEmpty(model.SystolicFrom))
                {
                    dbManager.AddParameters(6, PARM_SYSTOLIC_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_SYSTOLIC_FROM, model.SystolicFrom);
                }
                if (string.IsNullOrEmpty(model.SystolicTo))
                {
                    dbManager.AddParameters(7, PARM_SYSTOLIC_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_SYSTOLIC_TO, model.SystolicTo);
                }
                if (string.IsNullOrEmpty(model.TemperatureFrom))
                {
                    dbManager.AddParameters(8, PARM_TEMP_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_TEMP_FROM, model.TemperatureFrom);
                }
                if (string.IsNullOrEmpty(model.TemperatureTo))
                {
                    dbManager.AddParameters(9, PARM_TEMP_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_TEMP_TO, model.TemperatureTo);
                }
                if (string.IsNullOrEmpty(model.HeightFrom))
                {
                    dbManager.AddParameters(10, PARM_HEIGHT_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(10, PARM_HEIGHT_FROM, model.HeightFrom);
                }
                if (string.IsNullOrEmpty(model.HeightTo))
                {
                    dbManager.AddParameters(11, PARM_HEIGHT_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(11, PARM_HEIGHT_TO, model.HeightTo);
                }
                if (string.IsNullOrEmpty(model.SPO2From))
                {
                    dbManager.AddParameters(12, PARM_SPO2_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(12, PARM_SPO2_FROM, model.SPO2From);
                }
                if (string.IsNullOrEmpty(model.SPO2To))
                {
                    dbManager.AddParameters(13, PARM_SPO2_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(13, PARM_SPO2_TO, model.SPO2To);
                }
                if (string.IsNullOrEmpty(model.DiastolicFrom))
                {
                    dbManager.AddParameters(14, PARM_DIASTOLIC_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(14, PARM_DIASTOLIC_FROM, model.DiastolicFrom);
                }
                if (string.IsNullOrEmpty(model.DiastolicTo))
                {
                    dbManager.AddParameters(15, PARM_DIASTOLIC_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(15, PARM_DIASTOLIC_TO, model.DiastolicTo);
                }
                if (string.IsNullOrEmpty(model.RespirationResultFrom))
                {
                    dbManager.AddParameters(16, PARM_Resp_From, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(16, PARM_Resp_From, model.RespirationResultFrom);
                }
                if (string.IsNullOrEmpty(model.RespirationResultTo))
                {
                    dbManager.AddParameters(17, PARM_RESP_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(17, PARM_RESP_TO, model.RespirationResultTo);
                }
                if (string.IsNullOrEmpty(model.BMIFrom))
                {
                    dbManager.AddParameters(18, PARM_BMI_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(18, PARM_BMI_FROM, model.BMIFrom);
                }
                if (string.IsNullOrEmpty(model.BMITo))
                {
                    dbManager.AddParameters(19, PARM_BMI_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(19, PARM_BMI_TO, model.BMITo);
                }
                if (string.IsNullOrEmpty(model.PulseRateFrom))
                {
                    dbManager.AddParameters(20, PARM_PULSE_RATE_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(20, PARM_PULSE_RATE_FROM, model.PulseRateFrom);
                }
                if (string.IsNullOrEmpty(model.PulseRateTo))
                {
                    dbManager.AddParameters(21, PARM_PULSE_RATE_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(21, PARM_PULSE_RATE_TO, model.PulseRateTo);
                }
                if (string.IsNullOrEmpty(model.WeightFrom))
                {
                    dbManager.AddParameters(22, PARM_WEIGHT_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(22, PARM_WEIGHT_FROM, model.WeightFrom);
                }
                if (string.IsNullOrEmpty(model.WeightTo))
                {
                    dbManager.AddParameters(23, PARM_WEIGHT_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(23, PARM_WEIGHT_TO, model.WeightTo);
                }
                if (string.IsNullOrEmpty(model.BSAFrom))
                {
                    dbManager.AddParameters(24, PARM_BSA_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(24, PARM_BSA_FROM, model.BSAFrom);
                }
                if (string.IsNullOrEmpty(model.BSATo))
                {
                    dbManager.AddParameters(25, PARM_BSA_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(25, PARM_BSA_TO, model.BSATo);
                }


                ///////////////


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(26, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(26, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(27, PARM_ADVANCED_SEARCH, model.AdvancedSearch);
                #endregion
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_VITALS);
                CVitalsFillModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CVitalsFillModel();
                    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]).ToString());
                    modelFill.PatStatus = MDVUtility.CheckBooleanNull(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Systolic = MDVUtility.CheckStringNull(reader["Systolic"]);
                    modelFill.Diastolic = MDVUtility.CheckStringNull(reader["Diastolic"]);
                    modelFill.Pulse = MDVUtility.CheckStringNull(reader["Pulse"]);
                    modelFill.Temprature = MDVUtility.CheckStringNull(reader["Temp"]);
                    modelFill.Respiration = MDVUtility.CheckStringNull(reader["Resp"]);
                    modelFill.Weight = MDVUtility.CheckStringNull(reader["Weight"]);
                    modelFill.Height = MDVUtility.CheckStringNull(reader["Height"]);
                    modelFill.BSA = MDVUtility.CheckStringNull(reader["BSA"]);
                    modelFill.BMI = MDVUtility.CheckStringNull(reader["BMI"]);
                    modelFill.SPO2 = MDVUtility.CheckStringNull(reader["SPO2"]);
                    modelFill.DOS = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOS"]).ToString());
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadVitalsReport", PROC_CLINICAL_VITALS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CMedicationFillModel> LoadMedicationReport(CMedicationModel model)
        {
            List<CMedicationFillModel> listModel = new List<CMedicationFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                //dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, model.IncludeInactivePatient);
                // if user needs all patients then sending null else only active records
                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }
                dbManager.AddParameters(4, PARM_MEDICATION_AND_OR, model.MedicationAND);

                if (string.IsNullOrEmpty(model.Medication))
                {
                    dbManager.AddParameters(5, PARM_MEDICATION, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_MEDICATION, model.Medication);
                }
                if (string.IsNullOrEmpty(model.MedicationStatus))
                {
                    dbManager.AddParameters(6, PARM_MEDICATION_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_MEDICATION_STATUS, model.MedicationStatus);
                }

                if (string.IsNullOrEmpty(model.StartDate))
                {
                    dbManager.AddParameters(7, PARM_START_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_START_DATE, model.StartDate);
                }
                if (string.IsNullOrEmpty(model.ENDDate))
                {
                    dbManager.AddParameters(8, PARM_END_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_END_DATE, model.ENDDate);
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                #endregion
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_MEDICATION);
                CMedicationFillModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CMedicationFillModel();
                    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Medication = MDVUtility.CheckStringNull(reader["Medication"]);
                    modelFill.StartDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["StartDate"]));
                    modelFill.EndDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["EndDate"]));
                    modelFill.MedStatus = MDVUtility.CheckBooleanNull(reader["MedStatus"]) ? "Current" : "Past";
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadMedicationReport", PROC_CLINICAL_MEDICATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CImmunizationFillModel> LoadImmunizationReport(CImmunizationModel model)
        {
            List<CImmunizationFillModel> listModel = new List<CImmunizationFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(16);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }
                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                if (string.IsNullOrEmpty(model.Provider))
                {
                    dbManager.AddParameters(4, PARM_PRIOVIDERID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PRIOVIDERID, Convert.ToInt64(model.Provider));
                }
                if (string.IsNullOrEmpty(model.VaccineCategory))
                {
                    dbManager.AddParameters(5, PARM_IMMUNIZATION_CATEGORY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_IMMUNIZATION_CATEGORY_ID, Convert.ToInt64(model.VaccineCategory));
                }

                if (string.IsNullOrEmpty(model.Vaccine))
                {
                    dbManager.AddParameters(6, PARM_IMMUNIZATION_VACCINE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_IMMUNIZATION_VACCINE_ID, Convert.ToInt64(model.Vaccine));
                }

                if (string.IsNullOrEmpty(model.Route))
                {
                    dbManager.AddParameters(7, PARM_IMMUNIZATION_ROUTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_IMMUNIZATION_ROUTE_ID, Convert.ToInt64(model.Route));
                }
                if (string.IsNullOrEmpty(model.Site))
                {
                    dbManager.AddParameters(8, PARM_IMMUNIZATION_SITE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_IMMUNIZATION_SITE_ID, Convert.ToInt64(model.Site));
                }
                if (string.IsNullOrEmpty(model.ImmunizationReaction))
                {
                    dbManager.AddParameters(9, PARM_IMMUNIZATION_REACTION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_IMMUNIZATION_REACTION_ID, Convert.ToInt32(model.ImmunizationReaction));
                }
                if (string.IsNullOrEmpty(model.ImmunizationAlert))
                {
                    dbManager.AddParameters(10, PARM_IMMUNIZATION_ALERT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(10, PARM_IMMUNIZATION_ALERT_ID, Convert.ToInt32(model.ImmunizationAlert));
                }
                if (string.IsNullOrEmpty(model.ImmunizationFromDate))
                {
                    dbManager.AddParameters(11, PARM_IMMUNIZATION_DATE_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(11, PARM_IMMUNIZATION_DATE_FROM, Convert.ToDateTime(model.ImmunizationFromDate));
                }
                if (string.IsNullOrEmpty(model.ImmunizationToDate))
                {
                    dbManager.AddParameters(12, PARM_IMMUNIZATION_DATE_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(12, PARM_IMMUNIZATION_DATE_TO, Convert.ToDateTime(model.ImmunizationToDate));
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(13, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(14, PARM_IS_ADMINSTERED, model.IsAdministatered);
                dbManager.AddParameters(15, PARM_VOID_DOSE, model.voidDose);
                #endregion
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_IMMUNIZATION);
                CImmunizationFillModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CImmunizationFillModel();
                    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Category = MDVUtility.CheckStringNull(reader["Category"]);
                    modelFill.vaccine = MDVUtility.CheckStringNull(reader["Vaccine"]);
                    modelFill.Alert = MDVUtility.CheckStringNull(reader["Alert"]);
                    modelFill.Route = MDVUtility.CheckStringNull(reader["Route"]);
                    modelFill.Site = MDVUtility.CheckStringNull(reader["Site"]);
                    modelFill.Reaction = MDVUtility.CheckStringNull(reader["Reaction"]);
                    modelFill.AdministeredBy = MDVUtility.CheckStringNull(reader["AdministeredBy"]);
                    modelFill.AdminDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["AdminDate"]));
                    modelFill.DueDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DueDate"]));
                    modelFill.VoidDose = MDVUtility.CheckStringNull(reader["VoidDose"]);

                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadImmunizationReport", PROC_CLINICAL_IMMUNIZATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<CProblemsFillModel> LoadProblemsReport(CProblemsModel model)
        {
            List<CProblemsFillModel> listModel = new List<CProblemsFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(11);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }



                if (string.IsNullOrEmpty(model.ChronicityLevel))
                {
                    dbManager.AddParameters(4, PARM_CHRONICITY_LEVEL, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_CHRONICITY_LEVEL, model.ChronicityLevel);
                }


                if (string.IsNullOrEmpty(model.Problem))
                {
                    dbManager.AddParameters(5, PARM_PROBLEM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_PROBLEM, model.Problem);
                }


                if (string.IsNullOrEmpty(model.Severity))
                {
                    dbManager.AddParameters(6, PARM_SEVERITY, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_SEVERITY, model.Severity);
                }


                if (string.IsNullOrEmpty(model.ProblemStatus))
                {
                    dbManager.AddParameters(7, PARM_PROBLEM_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_PROBLEM_STATUS, model.ProblemStatus);
                }


                if (DateTime.MinValue == model.StartDate || model.StartDate == null)
                {
                    dbManager.AddParameters(8, PARM_START_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_START_DATE, model.StartDate);
                }


                if (DateTime.MinValue == model.EndDate || model.EndDate == null)
                {
                    dbManager.AddParameters(9, PARM_END_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_END_DATE, model.EndDate);
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(10, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                #endregion
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_PROBLEMS);
                CProblemsFillModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CProblemsFillModel();

                    modelFill.AccountNumber = reader["AccountNumber"] is DBNull ? "" : Convert.ToString(reader["AccountNumber"]);
                    modelFill.PatientName = reader["PatientName"] is DBNull ? "" : Convert.ToString(reader["PatientName"]);
                    modelFill.DOB = reader["DOB"] is DBNull ? "" : Convert.ToString(reader["DOB"]);
                    modelFill.PatStatus = reader["PatStatus"] is DBNull ? "" : Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Problem = reader["Problem"] is DBNull ? "" : Convert.ToString(reader["Problem"]);
                    modelFill.ProblemStatus = reader["PrStatus"] is DBNull ? "" : Convert.ToBoolean(reader["PrStatus"]) ? "Active" : "Inactive";
                    modelFill.ChronicityLevel = reader["ChronicityLevel"] is DBNull ? "" : Convert.ToString(reader["ChronicityLevel"]);
                    modelFill.Severity = reader["Severity"] is DBNull ? "" : Convert.ToString(reader["Severity"]);
                    modelFill.StartDate = reader["StartDate"] is DBNull ? "" : Convert.ToString(reader["StartDate"]);
                    modelFill.EndDate = reader["EndDate"] is DBNull ? "" : Convert.ToString(reader["EndDate"]);
                    modelFill.PatientId = reader["PatientId"] is DBNull ? "" : Convert.ToString(reader["PatientId"]);

                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadProblemsReport", PROC_CLINICAL_PROBLEMS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<CProceduresFillModelcs> LoadProceduresReport(CProceduresModelcs model)
        {
            List<CProceduresFillModelcs> listModel = new List<CProceduresFillModelcs>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                if (model.IncludeInactivePatient) { dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value); }
                else { dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true); }

                if (model.ProviderId == 0)
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, model.ProviderId);
                }
                if (string.IsNullOrEmpty(model.CPTCode))
                {
                    dbManager.AddParameters(5, PARM_PROCEDURE, null);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_PROCEDURE, model.CPTCode);
                }

                if (!string.IsNullOrEmpty(model.StartDate))
                {
                    dbManager.AddParameters(6, PARM_STARTDATE, Convert.ToDateTime(model.StartDate));
                }
                else
                {
                    dbManager.AddParameters(6, PARM_STARTDATE, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.EndDate))
                {
                    dbManager.AddParameters(7, PARM_ENDDATE, Convert.ToDateTime(model.EndDate));
                }
                else
                {
                    dbManager.AddParameters(7, PARM_ENDDATE, DBNull.Value);
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                #endregion

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_PROCEDURES);
                CProceduresFillModelcs modelFill = null;

                while (reader.Read())
                {
                    modelFill = new CProceduresFillModelcs();
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Procedure = MDVUtility.CheckStringNull(reader["Procedure"]);
                    modelFill.ICD = MDVUtility.CheckStringNull(reader["ICD"]);
                    modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                    modelFill.StartDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["StartDate"]));
                    modelFill.EndDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["EndDate"]));
                    modelFill.PatientId = MDVUtility.CheckLongNull(reader["PatientId"]);

                    listModel.Add(modelFill);
                }

                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadAllergiesReport", PROC_CLINICAL_ALLERGIES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<COrdersFillModel> LoadConsultationOrdersReport(COrdersModel model)
        {
            List<COrdersFillModel> listModel = new List<COrdersFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {



                dbManager.Open();
                dbManager.CreateParameters(11);
                #region paramaters

                if (string.IsNullOrEmpty(model.AccountNumber)) { dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value); }
                else { dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber); }

                if (string.IsNullOrEmpty(model.PatientFirstName)) { dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value); }
                else { dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName); }

                if (string.IsNullOrEmpty(model.PatientLastName)) { dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value); }
                else { dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName); }

                if (model.IncludeInactivePatient) { dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value); }
                else { dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true); }

                if (model.ProviderId == 0) { dbManager.AddParameters(4, PARM_PROVIDER_ID, null); }
                else { dbManager.AddParameters(4, PARM_PROVIDER_ID, model.ProviderId); }

                if (model.AssigneeProviderId == 0) { dbManager.AddParameters(5, PARM_ASSIGNEE_PROVIDER_ID, DBNull.Value); }
                else { dbManager.AddParameters(5, PARM_ASSIGNEE_PROVIDER_ID, model.AssigneeProviderId); }

                if (string.IsNullOrEmpty(model.Procedure)) { dbManager.AddParameters(6, PARM_PROCEDURES, null); }
                else { dbManager.AddParameters(6, PARM_PROCEDURES, model.Procedure); }

                if (string.IsNullOrEmpty(model.OrderNo)) { dbManager.AddParameters(7, PARM_ORDERNO, DBNull.Value); }
                else { dbManager.AddParameters(7, PARM_ORDERNO, model.OrderNo); }

                if (string.IsNullOrEmpty(model.OrderStatus)) { dbManager.AddParameters(8, PARM_ORDER_STATUS, DBNull.Value); }
                else { dbManager.AddParameters(8, PARM_ORDER_STATUS, model.OrderStatus); }

                if (!string.IsNullOrEmpty(model.DateFrom)) { dbManager.AddParameters(9, PARM_ORDER_DATEFROM, Convert.ToDateTime(model.DateFrom)); }
                else { dbManager.AddParameters(9, PARM_ORDER_DATEFROM, DBNull.Value); }

                if (!string.IsNullOrEmpty(model.DateTo)) { dbManager.AddParameters(10, PARM_ORDER_DATETO, Convert.ToDateTime(model.DateTo)); }
                else { dbManager.AddParameters(10, PARM_ORDER_DATETO, DBNull.Value); }

                #endregion


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_ORDER_CONSULTATION);
                COrdersFillModel modelFill = null;

                while (reader.Read())
                {

                    modelFill = new COrdersFillModel();
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Procedure = MDVUtility.CheckStringNull(reader["Procedures"]);
                    //  modelFill.Laboratory = MDVUtility.CheckStringNull(reader["Laboratory"]);
                    modelFill.OrderNo = MDVUtility.CheckStringNull(reader["OrderNumber"]);
                    modelFill.OrderStatus = MDVUtility.CheckStringNull(reader["Status"]);
                    modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                    modelFill.AssingneeProvider = MDVUtility.CheckStringNull(reader["AssigneeProvider"]);
                    modelFill.OrderDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["Date"])) + ' ' + (reader["Time"]);

                    //if (model.OrderType == "Lab")
                    //{
                    //    modelFill.OrderDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["Date"])) + ' ' + (reader["Time"]); 
                    //}
                    //else
                    //{ 
                    //modelFill.OrderDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["OrderDateTime"]));
                    //}
                    listModel.Add(modelFill);
                }

                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadConsultationOrdersReport", PROC_CLINICAL_ORDER_CONSULTATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CResultsFillModel> LoadResultsReport(CResultsModel model)
        {
            List<CResultsFillModel> listModel = new List<CResultsFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                #region Consultation

                //if (model.ResultType.ToLower() == "consultation")
                //{
                //    dbManager.Open();
                //    dbManager.CreateParameters(11);
                //    int x=0;
                //    #region Paramaters
                //    if (string.IsNullOrEmpty(model.AccountNumber)) { dbManager.AddParameters(x++, PARM_ACCOUNT_NUMBER, DBNull.Value); }
                //    else { dbManager.AddParameters(x++, PARM_ACCOUNT_NUMBER, model.AccountNumber); }

                //    if (string.IsNullOrEmpty(model.PatientFirstName)) { dbManager.AddParameters(x++, PARM_PATIENT_FIRST_NAME, DBNull.Value); }
                //    else { dbManager.AddParameters(x++, PARM_PATIENT_FIRST_NAME, model.PatientFirstName); }

                //    if (string.IsNullOrEmpty(model.PatientLastName)) { dbManager.AddParameters(x++, PARM_PATIENT_LAST_NAME, DBNull.Value); }
                //    else { dbManager.AddParameters(x++, PARM_PATIENT_LAST_NAME, model.PatientLastName); }

                //    if (model.IncludeInactivePatient) { dbManager.AddParameters(x++, PARM_INCLUDE_IN_ACTIVE, DBNull.Value); }
                //    else { dbManager.AddParameters(x++, PARM_INCLUDE_IN_ACTIVE, true); }

                //    if (model.ProviderId == 0) { dbManager.AddParameters(x++, "@Provider", null); }
                //    else { dbManager.AddParameters(x++, "@Provider", model.ProviderId); }

                //    if (model.AssigneeProvider>0 && model.AssigneeProvider!=null) { dbManager.AddParameters(x++, PARM_ASSIGNEE_PROVIDER, DBNull.Value); }
                //    else { dbManager.AddParameters(x++, PARM_ASSIGNEE_PROVIDER, model.AssigneeProvider); }

                //    if (string.IsNullOrEmpty(model.Procedures)) { dbManager.AddParameters(x++, PARM_PROCEDURES, null); }
                //    else { dbManager.AddParameters(x++, PARM_PROCEDURES, model.Procedures); }

                //    //if (string.IsNullOrEmpty(model.ResultNo)) { dbManager.AddParameters(7, PARM_RESULTNO, DBNull.Value); }
                //    //else { dbManager.AddParameters(7, PARM_RESULTNO, model.ResultNo); }

                //    if (string.IsNullOrEmpty(model.ResultStatus)) { dbManager.AddParameters(x++, PARM_ORDER_STATUS, DBNull.Value); }
                //    else { dbManager.AddParameters(x++, PARM_ORDER_STATUS, model.ResultStatus); }

                //    if (!string.IsNullOrEmpty(model.DateFrom)) { dbManager.AddParameters(x++, PARM_ORDER_DATEFROM, Convert.ToDateTime(model.DateFrom)); }
                //    else { dbManager.AddParameters(x++, PARM_ORDER_DATEFROM, DBNull.Value); }

                //    if (!string.IsNullOrEmpty(model.DateTo)) { dbManager.AddParameters(x++, PARM_ORDER_DATETO, Convert.ToDateTime(model.DateTo)); }
                //    else { dbManager.AddParameters(x++, PARM_ORDER_DATETO, DBNull.Value); }

                //    #endregion

                //    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_RESULTS_CONSULTATION);
                //    CResultsFillModel modelFill = null;

                //    while (reader.Read())
                //    {
                //        modelFill = new CResultsFillModel();
                //        modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                //        modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                //        modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                //        modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                //        modelFill.Procedures = MDVUtility.CheckStringNull(reader["Procedures"]);
                //        //   modelFill.Test = MDVUtility.CheckStringNull(reader["Test"]);
                //        //  modelFill.Laboratory = MDVUtility.CheckStringNull(reader["Laboratory"]);
                //        modelFill.ResultNo = MDVUtility.CheckStringNull(reader["ResultNumber"]);
                //        modelFill.ResultStatus = MDVUtility.CheckStringNull(reader["Status"]);
                //        modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                //        //----------------------------   modelFill.PatientId = MDVUtility.CheckLongNull(reader["PatientId"]);
                //        //modelFill.ResultDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["Date"])) + ' ' + (reader["Time"]);

                //        //////////string timeStr = string.Empty;
                //        //////////string dateStr = MDVUtility.CheckDateTimeNull((reader["ObservationDate"]));
                //        //////////if (!string.IsNullOrEmpty(dateStr))
                //        //////////{
                //        //////////    timeStr = Convert.ToDateTime(dateStr).ToString("hh:mm tt");
                //        //////////}
                //        //////////modelFill.ObservationDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["ObservationDate"])) + ' ' + timeStr;

                //        listModel.Add(modelFill);
                //    }
                //    return listModel;
                //}

                #endregion

                dbManager.Open();
                dbManager.CreateParameters(12);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                if (model.ProviderId == 0)
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, model.ProviderId);
                }

                if (string.IsNullOrEmpty(model.LabId))
                {
                    dbManager.AddParameters(5, PARM_LABORATORY_IDS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_LABORATORY_IDS, model.LabId);
                }
                if (string.IsNullOrEmpty(model.LabTest))
                {
                    dbManager.AddParameters(6, PARM_TEST_ID, null);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_TEST_ID, model.LabTest);
                }
                if (string.IsNullOrEmpty(model.ResultNo))
                {
                    dbManager.AddParameters(7, PARM_RESULTNO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_RESULTNO, model.ResultNo);
                }

                if (string.IsNullOrEmpty(model.ResultStatus))
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, model.ResultStatus);
                }

                if (!string.IsNullOrEmpty(model.DateFrom))
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, Convert.ToDateTime(model.DateFrom));
                }
                else
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.DateTo))
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, Convert.ToDateTime(model.DateTo));
                }
                else
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, DBNull.Value);
                }
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                #endregion
                var spName = string.Empty;

                if (model.ResultType == "Radiology")
                {
                    spName = PROC_CLINICAL_RESULTS_RADIOLOGY;
                }
                else if (model.ResultType == "Lab")
                {
                    spName = PROC_CLINICAL_RESULT_LAB;
                }


                if (!string.IsNullOrEmpty(spName))
                {
                    reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, spName);
                    CResultsFillModel modelFill = null;

                    while (reader.Read())
                    {
                        modelFill = new CResultsFillModel();
                        modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                        modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                        modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                        modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                        modelFill.Test = MDVUtility.CheckStringNull(reader["Test"]);
                        modelFill.Laboratory = MDVUtility.CheckStringNull(reader["Laboratory"]);
                        modelFill.ResultNo = MDVUtility.CheckStringNull(reader["ResultNumber"]);
                        modelFill.ResultStatus = MDVUtility.CheckStringNull(reader["Status"]);
                        modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                        modelFill.PatientId = MDVUtility.CheckLongNull(reader["PatientId"]);
                        //modelFill.ResultDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["Date"])) + ' ' + (reader["Time"]);

                        string timeStr = string.Empty;
                        string dateStr = MDVUtility.CheckDateTimeNull((reader["ObservationDate"]));
                        if (!string.IsNullOrEmpty(dateStr))
                        {
                            timeStr = Convert.ToDateTime(dateStr).ToString("hh:mm tt");
                        }
                        modelFill.ObservationDate = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["ObservationDate"])) + ' ' + timeStr;

                        listModel.Add(modelFill);
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadResultsReport", PROC_CLINICAL_ALLERGIES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<COrdersFillModel> LoadProcedureOrdersReport(COrdersModel model)
        {
            List<COrdersFillModel> listModel = new List<COrdersFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(11);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                if (model.ProviderId == 0)
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, model.ProviderId);
                }

                if (model.AssigneeProviderId == 0)
                {
                    dbManager.AddParameters(5, PARM_ASSIGNEEPROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_ASSIGNEEPROVIDER_ID, model.AssigneeProviderId);
                }

                if (string.IsNullOrEmpty(model.Procedure))
                {
                    dbManager.AddParameters(6, PARM_PROCEDURES, null);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_PROCEDURES, model.Procedure);
                }

                if (string.IsNullOrEmpty(model.OrderNo))
                {
                    dbManager.AddParameters(7, PARM_ORDERNO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_ORDERNO, model.OrderNo);
                }

                if (string.IsNullOrEmpty(model.OrderStatus))
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, model.OrderStatus);
                }

                if (!string.IsNullOrEmpty(model.DateFrom))
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, Convert.ToDateTime(model.DateFrom));
                }
                else
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.DateTo))
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, Convert.ToDateTime(model.DateTo));
                }
                else
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, DBNull.Value);
                }

                #endregion

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_PROCEDURES);
                COrdersFillModel modelFill = null;

                while (reader.Read())
                {
                    modelFill = new COrdersFillModel();
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Procedure = MDVUtility.CheckStringNull(reader["Procedures"]);
                    modelFill.OrderNo = MDVUtility.CheckStringNull(reader["OrderNumber"]);
                    modelFill.OrderStatus = MDVUtility.CheckStringNull(reader["Status"]);
                    modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                    modelFill.AssingneeProvider = MDVUtility.CheckStringNull(reader["AssigneeProvider"]);
                    modelFill.PatientId = MDVUtility.CheckLongNull(reader["PatientId"]);

                    modelFill.OrderDateTime = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["Date"])) + ' ' + (reader["Time"]);

                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadProcedureOrdersReport", PROC_CLINICAL_ORDERS_PROCEDURES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CPrescriptionOrderFillModel> LoadPrescriptionOrdersReport(CPrescriptionOrderModel model)
        {
            List<CPrescriptionOrderFillModel> listModel = new List<CPrescriptionOrderFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(12);
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }

                if (model.IncludeInactivePatient)
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_INCLUDE_IN_ACTIVE, true);
                }

                if (model.ProviderId == 0)
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, model.ProviderId);
                }

                if (string.IsNullOrEmpty(model.Medication))
                {
                    dbManager.AddParameters(5, PARM_MEDICATION, null);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_MEDICATION, model.Medication);
                }

                if (string.IsNullOrEmpty(model.Pharmacy))
                {
                    dbManager.AddParameters(6, PARM_PRESCRIPTION_PHARMACY, null);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_PRESCRIPTION_PHARMACY, model.Pharmacy);
                }

                if (string.IsNullOrEmpty(model.OrderNo))
                {
                    dbManager.AddParameters(7, PARM_ORDERNO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_ORDERNO, model.OrderNo);
                }

                if (string.IsNullOrEmpty(model.PrescriptionStatus))
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_ORDER_STATUS, model.PrescriptionStatus);
                }

                if (!string.IsNullOrEmpty(model.DateFrom))
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, Convert.ToDateTime(model.DateFrom));
                }
                else
                {
                    dbManager.AddParameters(9, PARM_ORDER_DATEFROM, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.DateTo))
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, Convert.ToDateTime(model.DateTo));
                }
                else
                {
                    dbManager.AddParameters(10, PARM_ORDER_DATETO, DBNull.Value);
                }
                dbManager.AddParameters(11, PARM_PRESCRIPTION_AND_OR, model.MedicationAND);
                #endregion

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_ORDERS_PRESCRIPTION);
                CPrescriptionOrderFillModel modelFill = null;

                while (reader.Read())
                {
                    modelFill = new CPrescriptionOrderFillModel();
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.PatStatus = Convert.ToBoolean(reader["PatStatus"]) ? "Active" : "Inactive";
                    modelFill.Medication = MDVUtility.CheckStringNull(reader["Medication"]);
                    modelFill.Pharmacy = MDVUtility.CheckStringNull(reader["Pharmacy"]);
                    modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                    modelFill.PrescriptionStatus = MDVUtility.CheckStringNull(reader["Status"]);
                    modelFill.Refill = MDVUtility.CheckStringNull(reader["Refill(s)"]);
                    modelFill.PatientId = MDVUtility.CheckLongNull(reader["PatientId"]);
                    modelFill.PrescribedOn = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["PrescribedOn"]));

                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::LoadPrescriptionOrdersReport", PROC_CLINICAL_ORDERS_PRESCRIPTION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region Lookups
        public DSMedicationLookup getPharmacyLookup()
        {
            DSMedicationLookup ds = new DSMedicationLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicationLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_PHARMACY_LOOKUP, ds, ds.Pharmacy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::getPharmacyLookup", PROC_CLINICAL_PHARMACY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrderLookup LookupAntimicrobials()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANTIMICROBIAL_LOOKUP, ds, ds.Antimicrobial.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LookupAntimicrobials", PROC_ANTIMICROBIAL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #endregion

        #region CCM Reports
        public List<CCM_ReportFillModel> Load_CCM_Report(CCM_ReportSearchModel model)
        {
            List<CCM_ReportFillModel> listModel = new List<CCM_ReportFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(24);
                int i = 0;
                #region paramaters
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    dbManager.AddParameters(i++, PARM_ACCOUNT_NUMBER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_ACCOUNT_NUMBER, model.AccountNumber);
                }
                if (string.IsNullOrEmpty(model.PatientFirstName))
                {
                    dbManager.AddParameters(i++, PARM_PATIENT_FIRST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);
                }
                if (string.IsNullOrEmpty(model.PatientLastName))
                {
                    dbManager.AddParameters(i++, PARM_PATIENT_LAST_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PATIENT_LAST_NAME, model.PatientLastName);
                }
                // if user needs all patients then sending null else only active records

                if (string.IsNullOrEmpty(model.DOB))
                {
                    dbManager.AddParameters(i++, PARM_DOB, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_DOB, model.DOB);
                }

                if (string.IsNullOrEmpty(model.Gender))
                {
                    dbManager.AddParameters(i++, PARM_GENDER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_GENDER, model.Gender);
                }
                if (string.IsNullOrEmpty(model.ProgramStatus))
                {
                    dbManager.AddParameters(i++, PARM_PROGRAM_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PROGRAM_STATUS, model.ProgramStatus);
                }
                if (string.IsNullOrEmpty(model.PracticeIds))
                {
                    dbManager.AddParameters(i++, PARM_PRACTICE_IDS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PRACTICE_IDS, model.PracticeIds);
                }
                if (string.IsNullOrEmpty(model.FacilityIds))
                {
                    dbManager.AddParameters(i++, PARM_FACILITY_IDS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_FACILITY_IDS, model.FacilityIds);
                }
                if (string.IsNullOrEmpty(model.fromTimeCompleted))
                {

                    if (string.IsNullOrEmpty(model.ToTimeCompleted))
                    {
                        dbManager.AddParameters(i++, PARM_FROM_TIME_COMPLETED, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(i++, PARM_FROM_TIME_COMPLETED, model.ToTimeCompleted);
                    }
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_FROM_TIME_COMPLETED, model.fromTimeCompleted);
                }
                if (string.IsNullOrEmpty(model.ToTimeCompleted))
                {
                    if (string.IsNullOrEmpty(model.fromTimeCompleted))
                    {
                        dbManager.AddParameters(i++, PARM_TO_TIME_COMPLETED, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(i++, PARM_TO_TIME_COMPLETED, model.fromTimeCompleted);
                    }
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_TO_TIME_COMPLETED, model.ToTimeCompleted);
                }
                if (string.IsNullOrEmpty(model.ConsentStatus))
                {
                    dbManager.AddParameters(i++, PARM_CONSENT_STATUS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_CONSENT_STATUS, model.ConsentStatus);
                }
                if (string.IsNullOrEmpty(model.NoOfProblemFrom))
                {
                    if (string.IsNullOrEmpty(model.NoOfProblemTo))
                    {
                        dbManager.AddParameters(i++, PARM_NO_OF_PROBLEM_FROM, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(i++, PARM_NO_OF_PROBLEM_FROM, model.NoOfProblemTo);
                    }

                }
                else
                {
                    dbManager.AddParameters(i++, PARM_NO_OF_PROBLEM_FROM, model.NoOfProblemFrom);
                }
                if (string.IsNullOrEmpty(model.NoOfProblemTo))
                {
                    if (string.IsNullOrEmpty(model.NoOfProblemFrom))
                    {
                        dbManager.AddParameters(i++, PARM_NO_OF_PROBLEM_TO, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(i++, PARM_NO_OF_PROBLEM_TO, model.NoOfProblemFrom);
                    }
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_NO_OF_PROBLEM_TO, model.NoOfProblemTo);
                }

                if (string.IsNullOrEmpty(model.ProblemNameValue))
                {
                    dbManager.AddParameters(i++, PARM_PROBLEM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PROBLEM, model.ProblemNameValue);
                }
                if (string.IsNullOrEmpty(model.ProviderIds))
                {
                    dbManager.AddParameters(i++, PARM_PROVIDER_IDS, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PROVIDER_IDS, model.ProviderIds);
                }
                if (string.IsNullOrEmpty(model.CareManager))
                {
                    dbManager.AddParameters(i++, PARM_CARE_MANAGER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_CARE_MANAGER, model.CareManager);
                }
                if (string.IsNullOrEmpty(model.CareCoordinator))
                {
                    dbManager.AddParameters(i++, PARM_CARE_COORDINATOR, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_CARE_COORDINATOR, model.CareCoordinator);
                }
                if (string.IsNullOrEmpty(model.CareGiver))
                {
                    dbManager.AddParameters(i++, PARM_CARE_GIVER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_CARE_GIVER, model.CareGiver);
                }
                if (string.IsNullOrEmpty(model.FromDate))
                {
                    dbManager.AddParameters(i++, PARM_FROM_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_FROM_DATE, model.FromDate);
                }
                if (string.IsNullOrEmpty(model.ToDate))
                {
                    dbManager.AddParameters(i++, PARM_TO_DATE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_TO_DATE, model.ToDate);
                }
                if (string.IsNullOrEmpty(model.ProgramType))
                {
                    dbManager.AddParameters(i++, PARM_PROGRAM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(i++, PARM_PROGRAM, model.ProgramType);
                }

                dbManager.AddParameters(i++, PARM_SUMMARY_REPORT, model.SummaryReport);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(i++, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(i++, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(i++, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(i++, PARM_USER_ID, MDVSession.Current.AppUserId);
                #endregion
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_REPORT);
                CCM_ReportFillModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new CCM_ReportFillModel();
                    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                    modelFill.PracticeName = MDVUtility.CheckStringNull(reader["PracticeName"]);
                    modelFill.FacilityName = MDVUtility.CheckStringNull(reader["FacilityName"]);
                    modelFill.AccountNumber = MDVUtility.CheckStringNull(reader["AccountNumber"]);
                    modelFill.PatientName = MDVUtility.CheckStringNull(reader["PatientName"]);
                    modelFill.DOB = GetDateMMDDYYY(MDVUtility.CheckDateTimeNull(reader["DOB"]));
                    modelFill.Gender = MDVUtility.CheckStringNull(reader["Gender"]);
                    modelFill.ProgramStatus = MDVUtility.CheckStringNull(reader["ProgramStatus"]);
                    modelFill.ConsentStatus = MDVUtility.CheckStringNull(reader["ConsentStatus"]);
                    modelFill.TimeCompleted = MDVUtility.CheckStringNull(reader["TimeCompleted"]);
                    modelFill.ChronicConditionsCount = MDVUtility.CheckStringNull(reader["ChronicConditionsCount"]);
                    modelFill.Problems = MDVUtility.CheckStringNull(reader["Problems"]);
                    modelFill.Provider = MDVUtility.CheckStringNull(reader["Provider"]);
                    modelFill.CareManager = MDVUtility.CheckStringNull(reader["CareManager"]);
                    modelFill.CareCoordinator = MDVUtility.CheckStringNull(reader["CareCoordinator"]);
                    modelFill.CareGiver = MDVUtility.CheckStringNull(reader["CareGiver"]);
                    modelFill.ProgramType = MDVUtility.CheckStringNull(reader["ProgramStatus"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReports::Load_CCM_Report", PROC_CCM_REPORT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region helpers
        public static string GetDateMMDDYYY(string dt)
        {
            if (dt == null || dt == "")
                return "";

            string part1;
            string part2;
            string part3;

            dt = dt.Contains(" ") ? dt.Substring(0, dt.IndexOf(" ")) : dt;
            dt = dt.Trim();

            part1 = LPad(dt.Substring(0, dt.IndexOf("/")), "0", 2);
            part2 = LPad(dt.Substring(dt.IndexOf("/") + 1, 2).Replace("/", ""), "0", 2);
            part3 = LPad(dt.Substring(dt.LastIndexOf("/") + 1), "0", 4);


            if (Convert.ToInt32(part1) > 12) //it's a dd/mm/yyyy
                return part2 + "/" + part1 + "/" + part3;
            else
                return part1 + "/" + part2 + "/" + part3;

        }
        public static string LPad(string str, string fillChar, int digits)
        {
            if (str.Length <= digits)
            {
                for (int i = 0; i < digits - str.Length; i++)
                    str = fillChar + str;

                //do nothing
            }

            return str;
        }

        #endregion
        #region Finacial Analystics
        /// <summary>
        /// Load Monthly Kpi Detail for specfic provider
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <param name="ProviderName"></param>
        /// <param name="ClaimDateFrom"></param>
        /// <param name="ClaimDateTo"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        public List<ProviderMonthlyPayment> LoadMonthlyPaymentTrendDetail(string ProviderId, string ProviderName, string ClaimDateFrom, string ClaimDateTo, string EntityId)
        {
            List<ProviderMonthlyPayment> MonthlyPaymentTrendList = new List<ProviderMonthlyPayment>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (string.IsNullOrEmpty(ProviderId))
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);

                if (string.IsNullOrEmpty(ProviderName))
                    dbManager.AddParameters(PARM_PROVIDER_NAME, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROVIDER_NAME, ProviderName);
                dbManager.AddParameters(PARM_CLAIM_DATE_FROM, ClaimDateFrom);
                dbManager.AddParameters(PARM_CLAIM_DATE_TO, ClaimDateTo);
                dbManager.AddParameters(PARM_ENTITY, EntityId);
                MonthlyPaymentTrendList = dbManager.ExecuteReaders<ProviderMonthlyPayment>(PROC_FAKPI_MONTHLYPAYMENTTREND_DETAIL);
                return MonthlyPaymentTrendList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReport::LoadMonthlyPaymentTrendDetail", PROC_FAKPI_MONTHLYPAYMENTTREND_DETAIL, ex);
                throw ex;
            }

        }


        #endregion
    }
}
