var Moduleids;
var urlpath;
var ModulesList;
var inactiveFacility = [];
var inactiveProvider = [];
var RefProviderId = [];
var RefProviderIdFrom = [];
var RefProviderSimple = [];
var VisitType = [];
var PCP = [];
var RefProviderName = [];

ReportsSSRSDashboard = {
    FromDate: '',
    ToDate: '',
    bIsFirstLoad: true,
    params: [],
    MUdata_Patient: [],
    totalResultResponse: {},
    //Report Id used for selected Report
    ReportId: "",
    // This id used to set the Report Id in Report Modules Tree
    SelectedReportID: "",
    //Report Name variable used to store the loaded report Name
    ReportName: "",
    arrReasoning: [],
    IsHeaderFareez: true,
    findHeader: null,
    getHeaderRow: null,
    headingRow: null,
    RowCount: 0,
    Locations: [],

    //variables for Reports Controls
    
    //Patients Report
    PatientList: "CreateDateFrom,CreateDateTo,LastVisitFrom,LastVisitTo,DOBFrom,DOBTo,Practice,Facility,Provider,RefProvider,IsActive,ZipCode,IncompleteDemo",
    AdvancePayment: "AccountNumber,PatientFirstName,PatientLastName,PaymentDateFrom,PaymentDateTo,AdvanceBalance,Practice,Facility",
    outstandingBalances: "VisitDateFrom,VisitDateTo,AccountNumber,PatientFirstName,PatientLastName,PatientBalance,DOSStartDays,DOSEndDays,Practice,Facility,Provider",
    Copayment: "AccountNumber,PatientLastName,PatientFirstName,CopaymentStatus,VisitDateFrom,VisitDateTo,DatePaidFrom,DatePaidTo,InsurancePlan,PaymentType,LedgerAccountCopay,Practice,Facility,Provider,IncludeUnLinkCopay",
    UnallocatedCopayment: "AccountNumber,PatientLastName,PatientFirstName,VisitDateFrom,VisitDateTo,InsurancePlan,Practice,Facility,Provider,PaidDate",
    DiagnosisAnalysis: "AccountNumber,PatientFirstName,PatientLastName,ICDCode,Practice,Facility,Provider,DOSStart,DOSEnd",
    ProcedureAnalysis: "AccountNumber,PatientFirstName,PatientLastName,CPT,Practice,Facility,Provider",
    InsuranceAnalysis: "Insurance,InsurancePlan,VisitDateFrom,VisitDateTo,FromRegistrationDate,ToRegistrationDate,PlanCategory,Practice,Facility,Provider",
    InsuranceAnalysisSummary: "Insurance,InsurancePlan,PlanCategory,Practice,Facility,Provider",
    //Scheduler
    DailyAppointmentsResource: "Resource,AppointmentDateStart,AppointmentDateEnd,AppointmentTimeStart,AppointmentTimeEnd,PatientType,visittype,Facility,Practice",
    DailyAppointmentsProvider: "Provider,AppointmentDateStart,AppointmentDateEnd,AppointmentTimeStart,AppointmentTimeEnd,PatientType,visittype,Facility,Practice,Apptstatus",
    TotalProviderAppointmentByReason: "AppointmentDateStart,AppointmentDateEnd,Provider,PatientType,visittype,Facility,Practice",
    TotalResourceAppointmentByReason: "AppointmentDateStart,AppointmentDateEnd,Resource,PatientType,visittype,Facility,Practice",
    CheckInandCheckOutDuration: "AppointmentDateStart,AppointmentDateEnd,Provider,PatientType,visittype,Facility,Practice",
    EnterPriseSchedulingReport: "Facility,Provider,Resource,RefProvider,spacing,Status,InsurancePlan,AppointmentDateStart,AppointmentDateEnd",
    DailyAppointmentReminder: "Provider,AccountNumber,AppointmentDateStart,AppointmentDateEnd,AppointmentTimeStart,AppointmentTimeEnd,ApptReminderType,Deliverystatus,ResponseStatus,Apptstatus",
    UnClaimedAppointment: "AccountNumber,PatientLastName,PatientFirstName,Facility,Provider,InsurancePlan,DOSFrom,DOSTo,SubmitStatus,Apptstatus,ClaimNumber",
    TtlFollowupApt: "StartDate,ENDDate,Provider,Facility,Practice",
    //-------------------------------New Reports   

    ProviderAnalysisByInsurancePlan: "Practice,Facility,Specialty,Provider,InsurancePlan,InsurancePlanCategory,IsPrimary,DOSFrom,DOSTo,VisitEntryDateFrom,VisitEntryDateTo",
    ProviderProcedureUtilization: "Practice,Facility,Provider,CPTCodeDescription,CPTCodeFrom,CPTCodeTo,DOSFrom,DOSTo,VisitEntryDateFrom,VisitEntryDateTo",
    RevenuebyFacility: "Practice,Facility,Specialty,InsurancePlan,DOSFrom,DOSTo,VisitEntryDateFrom,VisitEntryDateTo,AssignedUserId",
    RevenueByProvider: "Provider,Specialty,InsurancePlan,DOSFrom,DOSTo,VisitEntryDateFrom,VisitEntryDateTo,AssignedUserId",
    RevenuebyInsurancePlan: "Practice,Facility,Provider,InsurancePlan,InsurancePlanCategory,DOSFrom,DOSTo,VisitEntryDateFrom,VisitEntryDateTo",
    ChargesList: "Practice,Facility,Provider,InsurancePlan,InsurancePlanCategory,CPTMulti,ProcedureCategory,ICDMulti,IsPrimary,DOSFrom,DOSTo,VisitEntryDateFrom,VisitEntryDateTo,SubmitDateFrom,SubmitDateTo,EnteredBy,RefProvider,VoidedStatus,ClaimStatus,SubmitStatus",
    EnterpriseRevenue: "Practice,Facility,Provider,InsurancePlan",
    //------
    EnterpriseARAnalysis: "Practice,Facility,Provider,FromYearMonth,ToYearMonth,InsurancePlan",
    ARAgingAnalysis: "Practice,Facility,Provider,InsurancePlan",//Claim Number, DOSFrom and DOSTo Removed on Request by Waqas after discusion with Salman Bhai, Changee: Abdur Rehman Latif
    InsurancePlanAR: "Practice,Facility,Provider,FromYearMonth,ToYearMonth,InsurancePlan",
    InsuranceARPlan: "Practice,Facility,Provider,InsurancePlan,DOSFrom,DOSTo,ClaimDateFrom,ClaimDateTo,ClaimSubmitDateFrom,ClaimSubmitDateTo,SubmitStatus",
    PatientAR: "Practice,Facility,Provider,FromYearMonth,ToYearMonth,InsurancePlan",
    PaymentEntries: "Practice,Facility,Provider,InsurancePlan,AccountNumber,ClaimNumber,CPTMulti,PaymentType,ApplyTo,SystemCategory,LedgerAccount,DatePaidFrom,DatePaidTo,CheckNumber,AmountSmaller,AmountGreater,LedgerType,EntryDateFrom,EntryDateTo,EnteredBy,DOSFrom,DOSTo,IncludeUnLinkCopay",
    UserActivityReport: "Practice,Facility,Provider,User,SecurityRole,TransactionDateFrom,TransactionDateTo",
    FinancialAnalysisAtCPT: "FilterChargesBy,DOSStart,DOSEnd,Facility,AppointmentProvider,Provider,CPTMulti,InsurancePlan,RefProvider,Group1,Group2,Group3,Group4,Subtotal1,Subtotal2,Subtotal3,Subtotal4",  // Temporary Checkin : Abdur Rehman
    BeginningAREndingAR: "Provider,ClaimDateFrom,ClaimDateTo,DOSFrom,DOSTo",
    PaymentByUsers: "Facility,ResourceProvider,User,RenderingProvider,AppointmentProvider,PostingDateFrom,PostingDateTo,ClaimNumber",
    AgingSummaryAnalysis: "AgeReportBy,AgingBucketsToDisplay,BalanceOnReport,BalanceFrom,BalanceTo,Facility,InsurancePlan,Provider,AppointmentProvider,ResourceProvider,PatientLastName,PatientFirstName,AgeReportGroup1,AgeReportGroup2,AgeReportGroup3,AgeReportGroup4,Subtotal1,Subtotal2,Subtotal3,Subtotal4",
    EncounterWithoutClaims: "AppointmentDateFrom,AppointmentDateTo,Provider,ResourceProvider,Facility,AgeReportGroup1,AgeReportGroup2,AgeReportGroup3,AgeReportGroup4,Subtotal1,Subtotal2,Subtotal3,Subtotal4",
    ChargesbyUsers: "ReportType,CreatedCUFrom,CreatedCUTo,Facility,Provider,ResourceProvider,CreatedBy",
    BeginningAREndingARFacility: "ResourceProvider,Facility,ClaimDateFrom,ClaimDateTo,RenderingProvider",
    Claimcommentsbyuser: "CommentsEntryDateFrom,CommentsEntryDateTo,User,ClaimNumber,GroupBy",
    ClaimNotesbyuser: "NotesModifiedDateFrom,NotesModifiedDateTo,User,ClaimNumber",
    AnesthesiaOverlapping: "Facility,Anesthesiologist,CRNA,DOSFrom,DOSTo,ClaimNumber,ClaimDate,AnesTimeStart,AnesTimeEnd",
    HistoricalAgingSummaryAnalysis: "AgingasofDate,AgeReportBy,AgingBucketsToDisplayHistorical,Facility,InsurancePlan,Provider,SuprvisingProvider,AppointmentProvider,ResourceProvider,PatientLastName,PatientFirstName,HistoryGroup1,HistoryGroup2,HistoryGroup3,HistoryGroup4,Subtotal1,Subtotal2,Subtotal3,Subtotal4",
    ClaimSubmitStatus: "Facility,Provider,ResourceProvider,AccountNumber,PatientLastName,PatientFirstName,DOSFrom,DOSTo,ClaimNumber,InsurancePlan,SubmitStatus,User,CreatedCUFrom,CreatedCUTo,ClaimStatus",
    MUStage1Report: "Provider,FromDate,ToDate,QuarterlyReport,Quarter,Year",//kr
    MUStage2Report: "Provider,FromDate,ToDate,QuarterlyReport,Quarter,Year",//kr
    MUStage2ReportLatest: "Provider,FromDate,ToDate,QuarterlyReport,Quarter,Year",//kr
    MUStage3Report: "Provider,FromDate,ToDate,QuarterlyReport,Quarter,Year",//kr
    PhoneEncounter: "CreateDateFrom,CreateDateTo,NotesStatus,NotesDuration,Provider,RefProvider,Practice",
    ProgressNote: "CreateDateFrom,CreateDateTo,NotesStatus,NoteType,Provider,RefProvider,Practice,Facility,AmendedNote",
    ProgressNoteAmended: "CreateDateFrom,CreateDateTo,NotesStatus,NoteType,Provider,RefProvider,Practice,Facility,AmendmentDateFrom,AmendmentDateTo,AmendmentForBilling",
    BeginningAREndingARAndARAgingDifference: "Provider,ReportType",
    ZeroPaidClaim: "Practice,Facility,Provider,AgingGroup,AccountNumber,PatientLastName,PatientFirstName,InsurancePlan,SubscriberId,DOSFrom,DOSTo,ClaimNumber,VisitEntryDateFrom,VisitEntryDateTo,AdjustmentGroupCode,AdjustmentReasonCode,User",
    FinancialSummaryAnalysis: "ResourceProvider,Facility,ClaimDateFrom,ClaimDateTo,FinancialSummaryGroupBy",
    DailyCopaySheet: "Provider,Facility,AppointmentDate",
    CDSAlertsReport: "AccountNumber,PatientLastName,PatientFirstName,GenderList,RuleType,DOBirth,Practice,Facility,CDSStatus,CDSFrom,CDSTo",
    ClaimStatusDashboard: "SubmitStatus,DOSStart,DOSEnd",
    IncorrectBalanceVoidedClaims: "RunReportBy,DOSStart,DOSEnd",
    ClaimsNeversubmittedtoInsurance: "Provider,Facility,ResourceProvider,InsurancePlan,CreatedCUFrom,CreatedCUTo,ChgCreatedFrom,ChgCreatedTo,DOSFrom,DOSTo,ClaimStatus,CreatedBy",
    ClaimsFollowup: "Provider,Facility,InsurancePlanCategory,InsurancePlan,ClaimNumber,AgingBucketsToDisplay,DOSFrom,DOSTo,FollowupGroup,FollowupAction,FollowupReason,GroupByClaimFollowup",
    ClaimsSubmissionHistory: "Provider,Facility,InsurancePlan,ClaimNumber,CreatedCUFrom,CreatedCUTo,DOSFrom,DOSTo",
    ClaimsScrubberError: "Provider,Facility,InsurancePlan,ClaimNumber,DOSFrom,DOSTo,ClaimDateFrom,ClaimDateTo",
    DrugCodeCostReport: "Practice,Provider,Facility,User,AccountNumber,PatientFirstName,PatientLastName,ClaimNumber,PayorType,Payor,DOSFrom,DOSTo,PostingDateFrom,PostingDateTo,DrugCPTMulti,DrugCodeAmountGreater,DrugCodeAmountSmaller",

    //Referral: "Referrals",
    ReferralsIncoming: "Referrals,ClinicalAccountNumber,ClinicalCategory,ClinicalName,ClinicalVisitType,ClinicalReferalFromIncoming,ClinicalReferalToIncoming,ClinicalReferalFacilityFrom,ClinicalReferalFacilityTo,ClinicalReferalSpecialityFrom,ClinicalReferalSpecialityTo,ClinicalStatus,InsurancePlan,ReferalDateFrom,ReferalDateTo",
    ReferralOutgoing: "Referrals,ClinicalAccountNumber,ClinicalCategory,ClinicalName,ClinicalVisitType,ClinicalReferalFrom,ClinicalReferalTo,ClinicalReferalFacilityFrom,ClinicalReferalFacilityTo,ClinicalReferalSpecialityFrom,ClinicalReferalSpecialityTo,ClinicalStatus,InsurancePlan,ReferalDateFrom,ReferalDateTo",
    PatientOverPayment: "Provider,ClaimNumber,AccountNumber,PatientFirstName,PatientLastName",

    ClinicalAllergies: "AccountNumber,ClinicalPatientFirstName,PatientLastName,Allergy,AllergyReaction,AllergyStatus,PatStatus",
    ClinicalAppointmentReminders: "",
    ClinicalImmunization: "AccountNumber,ClinicalPatientFirstName,PatientLastName,ProviderSignleSelect,VaccineCategory,Vaccine,Route,Site,ImmunizationReaction,ImmunizationAlert,ImmunizationFromDate,ImmunizationToDate,PatStatus,IsAdministatered,voidDose,ImmunizationHints",

    ClinicalMedications: "AccountNumber,ClinicalPatientFirstName,PatientLastName,Medication,MedicationStatus,StartDate,ENDDate,PatStatus",
    ClinicalProcedures: "AccountNumber,ClinicalPatientFirstName,PatientLastName,ClinicalProvider,Procedure,StartDate,ENDDate,PatStatus",
    ClinicalProblems: "AccountNumber,ClinicalPatientFirstName,PatientLastName,ProblemChronicity,Problems,ProblemSeverity,ProblemStatus,ProbDateFrom,ProbDateTo,ProbGivenDateFrom,ProbGivenDateTo,PatStatus",
    ClinicalOrders: "OrderTabs,LabTab,RadiologyTab,ProcedureTab,ConsultationOrderTab,PrescriptionTab,ChkSummaryReport",

    ClinicalResults: "ResultTabs,labResult",
    ClinicalVitals: "AccountNumber,ClinicalPatientFirstName,PatientLastName,DOSFrom,clearfix,DOSTo,PatStatus,VitalsAdvControls",

    RadiologyOrder: "OrderTabs,RadiologyTab",
    LabOrder: "OrderTabs,LabTab,ProcedureTab",
    OrderType: '',
    ResultType: '',
    ReferralType: 'OutGoing',
    //RadiologyOrder: "OrderTabs,RadiologyTab,RadiologyTabDiv,ClinicalRadAccountNumber,RadPatientFirstName,RadPatientLastName,RadProvider,ClinicalRadLab,CPT,RadOrderNo,ClinicalRadStatus,RadDateFrom,RadDateTo",
    CCMReport: "AccountNumber,PatientFirstName,PatientLastName,DOB,Gender,ProgramStatus,Practice,Facility,AdvanceSearchCCM,From,To,clearfix",
    AdvanceSearchCCM: "TimeCompleted,ConsentStatus,NoOfProblem,Problems,Provider,CareManager,CareCoordinator,CareGiver,ProgramType",
    POSSurvey: "StartDate,ENDDate,Spacerline,GenderList,PatientStatement,PatStatus,visittype,Apptstatus,Ethnicity,Race,Provider,InsurancePlan,Facility,RefProvider,PCP,PatientLastName,PatientFirstName,Spacerline,ChkFieldsPOSSurvey",
    UserAudit: "username,ActivityDateFrom,ActivityDateTo,SecurityEntityGroup,AuditSecurityRole",
    AppointmentClaim: "ReportType,Provider,AppointmentDateFrom,AppointmentDateTo,Facility,AppointmentClaimNotes,Apptstatus,Practice,AppointmentClaimGroup1,AppointmentClaimGroup2,CheckIn",
    ClaimOverPaidByInsurance: "Practice,Provider,Facility,InsurancePlan,DOSFrom,DOSTo,ClaimDateFrom,ClaimDateTo",
    ClaimUnderPaidByInsurance: "Practice,Provider,Facility,InsurancePlan,DOSFrom,DOSTo,ClaimDateFrom,ClaimDateTo",

    ClaimsInCollection: "Practice,Facility,Provider,AccountNumber,PatientLastName,PatientFirstName,DOBFrom,DOBTo,DOSFrom,DOSTo,ClaimDateFrom,ClaimDateTo,PatBalGreater,PatBalLess,CollectionInGroup1,CollectionInGroup2,includeZeroBalanceClaims",

    PatientStatementPreference: "Practice,Facility,Provider,AccountNumber,PatientLastName,PatientFirstName,CreateDateFrom,CreateDateTo,SubmitedPtStatementDateFrom,SubmitedPtStatementDateTo,PatientStatementStatus,PatientStatementGroup1,PatientStatementGroup2",
    VoidAndReCreateClaim: "VoidAndReCreateClaimFilter,DOSStart,DOSEnd,Facility,Provider,ClaimNumber,VoidAndReCreateClaimGroup1,VoidAndReCreateClaimGroup2",
    AUPReport: "FacilityOIDMU3,FacilityMU3,FacilityType,LocationMU3,AntimicrobialMU3,StartDate,ENDDate",
    AROReport: "FacilityOIDMU3,FacilityMU3,FacilityType,LocationMU3,AccountNumber,SpecimenMU3,OrganismMU3,AntimicrobialBySpecimentAndOrganismMU3,StartDate,ENDDate",
    ClaimUnderPaidByPrimaryInsurance: "Practice,Provider,Facility,InsurancePlan,DOSFrom,DOSTo,ClaimDateFrom,ClaimDateTo,ClaimSubmissionDateFrom,ClaimSubmissionDateTo,CheckDateFrom,CheckDateTo",
    MonthlyPaymentTrend: "Provider,FinancialKPIClaimDateFrom,FinancialKPIClaimDateTo",
    BillingInquiryByProvider: "Practice,Facility,Provider,AccountNumber,PatientLastName,PatientFirstName,InquiryDateFrom,InquiryDateTo,PatBalFrom,PatBalTo,InquirySentBy",
    SecondaryInsuranceClaims: "Practice,Provider,Facility,PrimaryInsurance,SecondaryInsurance,DOSFrom,DOSTo,ClaimNumber,IsCrossover",
    MonthlyScoreboard: "Provider,Facility,CurrentYearMonth,CurrentToYearMonth",

    //iTrack Reports
    MIPSImprovementActivity: "Provider,ImprovementActivity,PatientLastNameCom,PatientFirstNameCom,QuarterlyReport,Quarter,Year,FromDate,ToDate",

    Load: function (params) {
        //Clearing the Object properties

        ReportsSSRSDashboard.params = params;
        if (ReportsSSRSDashboard.bIsFirstLoad) {
            ReportsSSRSDashboard.bIsFirstLoad = false;
            var self = "";
            if (ReportsSSRSDashboard.params["PanelID"] != "pnlReportsSSRSDashboard") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + " #pnlReportsSSRSDashboard")
            }
            else
                self = $('#' + ReportsSSRSDashboard.params["PanelID"]);
            // self.loadDropDowns(true);

            //var dataActive = "IsActive=1";
            //self.find('.LoadFirst').loadDropDowns(true, dataActive, ReportsSSRSDashboard.params["PanelID"]);
            //var dataLedgerAccount = "IsActive=&ID=" + -1 + "&ID2=" + -1;
            //self.find('.LoadThird').loadDropDowns(true, dataLedgerAccount, ReportsSSRSDashboard.params["PanelID"]);
            //var dataAllUsers = "IsActive=&ID=1";
            //self.find('.LoadSecond').loadDropDowns(true, dataAllUsers, ReportsSSRSDashboard.params["PanelID"]);
            //var data = "IsActive=";
            //self.find('.LoadZero').loadDropDowns(true, data, ReportsSSRSDashboard.params["PanelID"]);
            ReportsSSRSDashboard.SearchReportsPriviliges(null).done(function (response) {
                if (response.status != false) {
                    ReportsSSRSDashboard.BindModules(response);
                    ReportsSSRSDashboard.ReportTree(response);
                    ReportsSSRSDashboard.DocumentReadyFunction(true);
                }
            });

            //hiding all controls of Form Group which will be used to clone furthur on basises of Report selected
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' .form-group ').children().hide();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Quarter').addClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Year').addClass('disableAll');
            ReportsSSRSDashboard.validateReport();
            $('#pnlReportsSSRSDashboard #reportPanel').toggleClass('reportToggle');
            $('#pnlReportsSSRSDashboard #mainpanel').toggleClass('reportMainPanel');
        }
        $($(document.getElementById('ReportViewIframe').contentWindow.document).find('body')).on('click', function (ev) {
            $('html').trigger('click');
        });
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport').hide();
        //$('#' + ReportsSSRSDashboard.params["PanelID"] + ' .RadiologyTab').hide();
        //$('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ulOrderTabsItems').hide();
    },
    emptyParamsReports: function () {
        ReportsSSRSDashboard.OrderType = '';
        ReportsSSRSDashboard.ResultType = '';
        ReportsSSRSDashboard.ReportName = '';
        ReportsSSRSDashboard.SelectedReportID = '';
        ReportsSSRSDashboard.MUdata_Patient = [];
        ReportsSSRSDashboard.totalResultResponse = [];
    },
    DocumentReadyFunction: function (firstLoad) {
        $(document).ready(function () {

            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtSSProvider').on('change', function () {

                if ($(this).val() != '' && $(this).val() != null) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtSSProviderGroup').val('');
                }
            });

            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtSSProviderGroup').on('change', function () {

                if ($(this).val() != '' && $(this).val() != null) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtSSProvider').val('');
                }
            });


            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').change('value', function (event) {
                //Begin: Abdur Rehman Latif, May 25th, 2016, BUG PMS 5147
                //$('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                //End: Abdur Rehman Latif, May 25th, 2016, BUG PMS 5147
            });


            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').on("select_node.jstree", function (e, data) {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").hide();
                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").hide();
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport').hide();
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #btnExportXML').hide();
                var FormName = data.node.text;

                if (FormName.toLowerCase().trim() == "Value Based Program".toLowerCase().trim()) {
                    $('#pnlReportsSSRSDashboard #ReportViewIframe').hide();
                } else {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                    $('#pnlReportsSSRSDashboard #ReportViewIframe').show();
                }
                if (FormName.toLowerCase().trim() == "PQRS Registry Report".toLowerCase().trim() || FormName.toLowerCase().trim() == "physician quality reporting system (pqrs)".toLowerCase().trim() || FormName.indexOf("PQRS") > -1) {
                    FormName = "Physician Quality Reporting System (PQRS)";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #DivWellComePanel').hide();
                    $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
                    $('#pnlReportsSSRSDashboard #ReportParamaters').html('').hide();
                    $('#MuStageReportView').hide();
                    ReportsSSRSDashboard.ReportName = FormName;
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport').show();
                    ReportsSSRSDashboard.showPQRSselectReport(true);

                    self = $('#' + ReportsSSRSDashboard.params["PanelID"]);
                    // self.loadDropDowns(true);
                    var data = "IsActive=";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport').find('.LoadPQRSReportddl').loadDropDowns(true, data, ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport').done(function () {
                        //Loading Modules and Creating Report Tree
                        var ProviderHTML = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport #txtMeasureProvider').html();
                        var ProviderGroupHTML = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport #txtGPROProviderGroup').html();
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport #txtSSProvider').html(ProviderHTML);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport #txtSSProviderGroup').html(ProviderGroupHTML);
                    });
                    var ReportsControlDiv = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport');

                    utility.ValidateFromToDate("frmSSRSReports #PQRSRegistryReport", 'IndividualReportFromDate', 'IndividualReportToDate', false);
                    utility.ValidateFromToDate("frmSSRSReports #PQRSRegistryReport", 'GPROFromDate', 'GPROToDate', false);
                    //ReportsSSRSDashboard.ValidatePQRSReport();

                }
                if (FormName.toLowerCase().trim() == "CQM".toLowerCase().trim()) {
                    FormName = "Clinical Quality Measures (CQM)";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #DivWellComePanel').hide();
                    $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
                    $('#pnlReportsSSRSDashboard #ReportParamaters').html('').hide();
                    $('#MuStageReportView').hide();
                    ReportsSSRSDashboard.ReportName = FormName;
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport').show();
                    ReportsSSRSDashboard.showCQMIndividualReporting();
                    //ReportsSSRSDashboard.showPQRSselectReport(true);

                    self = $('#' + ReportsSSRSDashboard.params["PanelID"]);
                    // self.loadDropDowns(true);
                    var data = "IsActive=";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport').find('.LoadPQRSReportddl').loadDropDowns(true, data, ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport').done(function () {
                        //Loading Modules and Creating Report Tree
                        var ProviderHTML = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport #txtMeasureProvider').html();
                        var ProviderGroupHTML = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport #txtGPROProviderGroup').html();
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport #txtSSProvider').html(ProviderHTML);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport #txtSSProviderGroup').html(ProviderGroupHTML);
                    });
                    var ReportsControlDiv = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport');

                    utility.ValidateFromToDate("frmSSRSReports #CQMIndividualReport", 'IndividualReportFromDate', 'IndividualReportToDate', true);
                    utility.ValidateFromToDate("frmSSRSReports #CQMIndividualReport", 'GPROFromDate', 'GPROToDate', false);
                    //ReportsSSRSDashboard.ValidatePQRSReport();

                }
                if (FormName.toLowerCase().trim() == "Value Based Program".toLowerCase().trim()) {
                    FormName = "Value Based Program (VBP)";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #DivWellComePanel').hide();
                    $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
                    $('#pnlReportsSSRSDashboard #ReportParamaters').html('').hide();
                    $('#MuStageReportView').hide();
                    ReportsSSRSDashboard.ReportName = FormName;
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport').show();
                    ReportsSSRSDashboard.showVBPIndividualReporting();
                    //ReportsSSRSDashboard.showPQRSselectReport(true);

                    self = $('#' + ReportsSSRSDashboard.params["PanelID"]);
                    // self.loadDropDowns(true);
                    var data = "IsActive=";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport').find('.LoadPQRSReportddl').loadDropDowns(true, data, ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport').done(function () {
                        //Loading Modules and Creating Report Tree
                        var ProviderHTML = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport #txtMeasureProvider').html();
                        var ProviderGroupHTML = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport #txtGPROProviderGroup').html();
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport #txtSSProvider').html(ProviderHTML);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport #txtSSProviderGroup').html(ProviderGroupHTML);
                    });
                    var ReportsControlDiv = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport');

                    utility.ValidateFromToDate("frmSSRSReports #VBPIndividualReport", 'IndividualReportFromDate', 'IndividualReportToDate', true);
                    utility.ValidateFromToDate("frmSSRSReports #VBPIndividualReport", 'GPROFromDate', 'GPROToDate', false);
                    //ReportsSSRSDashboard.ValidatePQRSReport();

                }
                if (FormName.toLowerCase().trim() != "physician quality reporting system (pqrs)".toLowerCase().trim() && FormName.toLowerCase().trim() != "PQRS Registry Report".toLowerCase().trim()) {// || FormName.indexOf("PQRS") < -1) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport').hide();
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PhoneEncounterView').hide();
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ProgressNoteView").hide();
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").hide();
                    //preveting anchor tag to click again so when a report is loading the second call should be stopped
                    if ($(this).find('a').attr('disabled')) {
                        e.preventDefault();
                        e.stopImmediatePropagation();

                        //resolving Chrome browser issue for clicking class change in Report tree
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').removeClass("jstree-clicked");
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('#' + ReportsSSRSDashboard.SelectedReportID).find("a").addClass("jstree-clicked");

                    } else {
                        //setting selected Report ID
                        ReportsSSRSDashboard.SelectedReportID = data.node.id
                        //disabling Report  tree during report load
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').attr('disabled', true).css("cursor", "wait");
                        var parent = data.instance.get_path('#' + data.node.id);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #DivWellComePanel').hide();
                        var SelectedModule = jQuery.map(ModulesList, function (obj) {
                            if (obj.FormsId === data.node.id)
                                return obj; // or return obj.name, whatever.
                        });

                        var isParamatersAppend = true;
                        //ReportsSSRSDashboard.RunReport();
                        if (SelectedModule.length > 0 && SelectedModule[0].ReportSSRSId != '') {
                            //set the report path for financial Analysis to No group at default, Azhar siyal & Abdur Rehman
                            var reportpath = parent[0];
                            //if (FormName.toLowerCase().trim() == "Financial Analysis At CPT Level".toLowerCase().trim()) {
                            //    var FinancialReportId = ReportsSSRSDashboard.FinancialAnalysisURL(SelectedModule[0].ReportSSRSId);
                            //    //  urlpath = 'Controls/Reports/ReportViewer.aspx?reportpath=' + FinancialReportId;//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');

                            //    reportpath = parent[0] + '/' + FinancialReportId;
                            //} else {
                            // urlpath = null;
                            reportpath = parent[0] + '/' + SelectedModule[0].ReportSSRSId; //+ parent[1];
                            // }
                            $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
                            $('#MuStageReportView').hide();


                            ReportsSSRSDashboard.ReportName = FormName;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);

                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSRegistryReport').hide();
                            //$('#' + ReportsSSRSDashboard.params["PanelID"] + ' .RadiologyTab').hide();
                            //$('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ulOrderTabsItems').hide();

                            if (FormName.toLowerCase().trim() == "Advance Payment".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.AdvancePayment);
                            } else if (FormName.toLowerCase().trim() == "Patient List".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PatientList);
                                ReportsSSRSDashboard.LoadRefProviderSimple();
                            } else if (FormName.toLowerCase().trim() == "Procedure Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ProcedureAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Outstanding Balances".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.outstandingBalances);
                            } else if (FormName.toLowerCase().trim() == "Insurance Analysis Summary".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.InsuranceAnalysisSummary);
                            } else if (FormName.toLowerCase().trim() == "Insurance Analysis ".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.InsuranceAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Diagnosis Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.DiagnosisAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Collected Copayment".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.Copayment);
                            } else if (FormName.toLowerCase().trim() == "Unallocated copayment".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.UnallocatedCopayment);
                            }
                                //kr
                            else if (FormName.toLowerCase().trim() == "MU Stage 1 Report".toLowerCase().trim()) {

                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MUStage1Report);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Quarter').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Year').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .Provider select').removeAttr('multiple');
                            } else if (FormName.toLowerCase().trim() == "MU Stage 2 Report".toLowerCase().trim()) {

                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MUStage2Report);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Quarter').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Year').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .Provider select').removeAttr('multiple');
                            }
                            else if (FormName.toLowerCase().trim() == "MU Stage 2 Report Latest".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MUStage2ReportLatest);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Quarter').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Year').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .Provider select').removeAttr('multiple');
                            } else if (FormName.toLowerCase().trim() == "MU Stage 3 Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MUStage3Report);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Quarter').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Year').addClass('disableAll');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .Provider select').removeAttr('multiple');
                            }
                                //Clinical Reports start
                            else if (FormName.toLowerCase().trim() == "Progress Note".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ProgressNote);
                                ReportsSSRSDashboard.LoadRefProviderSimple();
                            }
                            else if (FormName.toLowerCase().trim() == "Progress Note - Amendment".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ProgressNoteAmended);
                                ReportsSSRSDashboard.LoadRefProviderSimple();
                            }
                            else if (FormName.toLowerCase().trim() == "Phone Encounter".toLowerCase().trim()) {
                                if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ddlDuration option').length == 0) {
                                    //CacheManager.BindDropDownsByEntityID('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ddlDuration', 'GetPhoneEncounterDuration', false, -1);
                                }
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PhoneEncounter);
                                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ddlDuration').multiselect('destroy');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ddlDuration').multiselect({
                                    includeSelectAllOption: true,
                                    enableFiltering: true,
                                    enableCaseInsensitiveFiltering: true,
                                    onDropdownShow: function (event) {
                                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ddlDuration').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                                    },
                                });
                                ReportsSSRSDashboard.LoadRefProviderSimple();

                            }
                            else if (FormName.toLowerCase().trim() == "AUP".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.AUPReport);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #btnExportXML').show();
                                ReportsSSRSDashboard.params.ReportType = "AUPReport";
                            } else if (FormName.toLowerCase().trim() == "ARO".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.AROReport);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #btnExportXML').show();
                                ReportsSSRSDashboard.params.ReportType = "AROReport";
                            }
                                //Clinical Reports End
                                //Schedular Reports
                            else if (FormName.toLowerCase().trim() == "Total Resource appointments ".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.TotalResourceAppointmentByReason);
                            } else if (FormName.toLowerCase().trim() == "Total Provider appointments".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.TotalProviderAppointmentByReason);
                            } else if (FormName.toLowerCase().trim() == "Enterprise Scheduling".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.EnterPriseSchedulingReport);
                                ReportsSSRSDashboard.LoadRefProviderSimple();
                            } else if (FormName.toLowerCase().trim() == "Daily Resource Appointments".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.DailyAppointmentsResource);
                            } else if (FormName.toLowerCase().trim() == "Daily Provider Appointments".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.DailyAppointmentsProvider);
                            } else if (FormName.toLowerCase().trim() == "Check In And Check Out Duration".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.CheckInandCheckOutDuration);
                            } else if (FormName.toLowerCase().trim() == "Total Follow-Up Appointments".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.TtlFollowupApt);
                            } else if (FormName.toLowerCase().trim() == "Revenue By Facility".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.RevenuebyFacility);
                            } else if (FormName.toLowerCase().trim() == "Enterprise Revenue".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.EnterpriseRevenue);
                            } else if (FormName.toLowerCase().trim() == "Revenue By Provider".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.RevenueByProvider);
                            } else if (FormName.toLowerCase().trim() == "Provider Analysis By Plan".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ProviderAnalysisByInsurancePlan);
                            } else if (FormName.toLowerCase().trim() == "Charges List".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ChargesList);
                                ReportsSSRSDashboard.LoadICDMultiSelect();
                                ReportsSSRSDashboard.LoadCPTMultiSelect();
                            } else if (FormName.toLowerCase().trim() == "Provider Procedure Utilization".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ProviderProcedureUtilization);
                            } else if (FormName.toLowerCase().trim() == "Insurance Plan AR".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.InsurancePlanAR);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtInsurancePlan').prepend("<option value='self' id='self'>Self</option>");
                            } else if (FormName.toLowerCase().trim() == "Patient AR".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PatientAR);
                            }
                            else if (FormName.toLowerCase().trim() == "Payment Entries".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PaymentEntries);
                                ReportsSSRSDashboard.LoadCPTMultiSelect();
                            } else if (FormName.toLowerCase().trim() == "Enterprise AR Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.EnterpriseARAnalysis);
                            } else if (FormName.toLowerCase().trim() == "AR Aging Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ARAgingAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Revenue By Plan".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.RevenuebyInsurancePlan);
                            } else if (FormName.toLowerCase().trim() == "User Activity Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.UserActivityReport);
                            } else if (FormName.toLowerCase().trim() == "Financial Analysis At CPT Level".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.FinancialAnalysisAtCPT);
                            } else if (FormName.toLowerCase().trim() == "Beginning AR Ending AR".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.BeginningAREndingAR);
                            } else if (FormName.toLowerCase().trim() == "Payments by Users".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PaymentByUsers);
                                ReportsSSRSDashboard.BindClaimNumber();
                            } else if (FormName.toLowerCase().trim() == "Aging Summary Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.AgingSummaryAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Encounter without Claims".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.EncounterWithoutClaims);
                            } else if (FormName.toLowerCase().trim() == "Charges By Users".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ChargesbyUsers);
                            } else if (FormName.toLowerCase().trim() == "Beginning AR Ending AR Facility".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.BeginningAREndingARFacility);
                            } else if (FormName.toLowerCase().trim() == "Claim Comments By User".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.Claimcommentsbyuser);
                            } else if (FormName.toLowerCase().trim() == "Claim Notes By User".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimNotesbyuser);
                            } else if (FormName.toLowerCase().trim() == "Claim Submit Status".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimSubmitStatus);
                            } else if (FormName.toLowerCase().trim() == "Daily Appointment Reminders".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.DailyAppointmentReminder);
                            } else if (FormName.toLowerCase().trim() == "AR Reconciliation Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.BeginningAREndingARAndARAgingDifference);
                            } else if (FormName.toLowerCase().trim() == "Referrals".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ReferralOutgoing);
                                ReportsSSRSDashboard.BindPatientAccount();
                                ReportsSSRSDashboard.LoadRefProvider();
                                ReportsSSRSDashboard.LoadRefProviderFrom();
                            } else if (FormName.toLowerCase().trim() == "Patient Overpayment".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PatientOverPayment);
                            } else if (FormName.toLowerCase().trim() == "Zero Paid Claim".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ZeroPaidClaim);
                            }
                            else if (FormName.toLowerCase().trim() == "Claim Over Paid By Insurance".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimOverPaidByInsurance);
                            }
                            else if (FormName.toLowerCase().trim() == "Claim Under Paid By Insurance".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimUnderPaidByInsurance);
                            }

                            else if (FormName.toLowerCase().trim() == "Claims In Collection".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimsInCollection);
                            }

                            else if (FormName.toLowerCase().trim() == "Anesthesia Overlapping".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.AnesthesiaOverlapping);
                            } else if (FormName.toLowerCase().trim() == "Historical Aging Summary Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.HistoricalAgingSummaryAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Y to Y Monthly Financial Summary Analysis".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.FinancialSummaryAnalysis);
                            } else if (FormName.toLowerCase().trim() == "CDS Alert Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.CDSAlertsReport);
                            } else if (FormName.toLowerCase().trim() == "Daily Copay Sheet".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.DailyCopaySheet);
                            } else if (FormName.toLowerCase().trim() == "Claim Status Dashboard".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimStatusDashboard);
                            } else if (FormName.toLowerCase().trim() == "Incorrect Balance by Voided Claims".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.IncorrectBalanceVoidedClaims);
                            } else if (FormName.toLowerCase().trim() == "Claims Never submitted to Insurance".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimsNeversubmittedtoInsurance);
                            } else if (FormName.toLowerCase().trim() == "Unclaimed Appointments Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.UnClaimedAppointment);
                            } else if (FormName.toLowerCase().trim() == "Claims Under Paid by Primary Insurance".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimUnderPaidByPrimaryInsurance);
                            } else if (FormName.toLowerCase().trim() == "Billing Inquiry by Provider".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.BillingInquiryByProvider);
                            }
                            else if (FormName.toLowerCase().trim() == "Monthly Payment Trend".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MonthlyPaymentTrend);
                            } else if (FormName.toLowerCase().trim() == "AR Aging Analysis MPS".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ARAgingAnalysis);
                            } else if (FormName.toLowerCase().trim() == "Patient List MPS".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PatientList);
                            } else if (FormName.toLowerCase().trim() == "Payment Entries MPS".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PaymentEntries);
                                ReportsSSRSDashboard.LoadCPTMultiSelect();
                            } else if (FormName.toLowerCase().trim() == "Insurance AR Plan Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.InsuranceARPlan);
                            } else if (FormName.toLowerCase().trim() == "Monthly Scoreboard".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MonthlyScoreboard);
                            } else if (FormName.toLowerCase().trim() == "Claim Follow Up".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimsFollowup);
                            } else if (FormName.toLowerCase().trim() == "Claim Submission History".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimsSubmissionHistory);
                            } else if (FormName.toLowerCase().trim() == "Claim Scrubber Errors".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClaimsScrubberError);
                            } else if (FormName.toLowerCase().trim() == "MIPS Improvement Activity".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.MIPSImprovementActivity);
                            }
                                //Clinical Reports  Started

                            else if (FormName.toLowerCase().trim() == "Allergies".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalAllergies);
                                ReportsSSRSDashboard.fillAllergiesDropdown();
                                ReportsSSRSDashboard.fillReactionsDropdown();
                            }
                            else if (FormName.toLowerCase().trim() == "POS Surveys".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.POSSurvey);
                                //$("#ReportParamaters #btn-Run").addClass('hidden')
                                //$("#ReportParamaters #btn-Export").removeClass('hidden')
                                //ReportsSSRSDashboard.fillAllergiesDropdown();
                                //ReportsSSRSDashboard.fillReactionsDropdown();
                                ReportsSSRSDashboard.LoadRefProviderSimple();
                                ReportsSSRSDashboard.LoadPCP();
                            }

                            else if (FormName.toLowerCase().trim() == "Appointment Reminders".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalAppointmentReminders);
                            }
                            else if (FormName.toLowerCase().trim() == "Immunization".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalImmunization);
                                ReportsSSRSDashboard.fillImmunizationDropdown();
                            } else if (FormName.toLowerCase().trim() == "Medications".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalMedications);
                                ReportsSSRSDashboard.fillMedicationDropdown();
                            } else if (FormName.toLowerCase().trim() == "Procedures".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalProcedures);
                                ReportsSSRSDashboard.LoadProviderAutocomplete('#ReportParamaters #txtProvider', '#ReportParamaters #hfClinicalProvider');
                                //var PanelDiv = '#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters';
                                //if ($(PanelDiv + '  .tab-pane.active' + ' #txtProvider').length > 0) {
                                //    ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtProvider', PanelDiv + '  .tab-pane.active' + ' #hfProvider');
                                //}   
                                ReportsSSRSDashboard.fillProceduresDropdown();
                            }


                            else if (FormName.toLowerCase().trim() == "Problems".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalProblems);
                                //ReportsSSRSDashboard.fillProblemsDropdown();
                                ReportsSSRSDashboard.fillChronicityDropdown();
                                ReportsSSRSDashboard.fillSeverityDropdown();
                                ReportsSSRSDashboard.fillProblemsDropdown($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters input#txtProblems'), $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #hdnProblemName'));
                            } else if (FormName.toLowerCase().trim() == "Orders".toLowerCase().trim()) {
                                if (ReportsSSRSDashboard.OrderType != 'Lab') {
                                    ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalOrders);

                                    ReportsSSRSDashboard.LoadLabs('ReportParamaters #txtLab', ReportsSSRSDashboard.params.PanelID).done(function () {

                                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtLab').multiselect('destroy');
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtLab').multiselect({
                                            includeSelectAllOption: true,
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true,
                                            onDropdownShow: function (event) {
                                                $('#' + ReportsSSRSDashboard.params.PanelID + ' #txtLab').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                                            },
                                        });
                                    });

                                    ReportsSSRSDashboard.SelectOrdersTab('Lab');
                                } else {
                                    isParamatersAppend = false;
                                }

                            } else if (FormName.toLowerCase().trim() == "Results".toLowerCase().trim()) {
                                if (ReportsSSRSDashboard.ResultType != 'Lab') {
                                    ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalResults);
                                    ReportsSSRSDashboard.LoadLabs('ReportParamaters #LabResultDiv #txtLab', ReportsSSRSDashboard.params.PanelID).done(function () {

                                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #LabResultDiv #txtLab').multiselect('destroy');
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabResultDiv #txtLab').multiselect({
                                            includeSelectAllOption: true,
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true,
                                            onDropdownShow: function (event) {
                                                $('#' + ReportsSSRSDashboard.params.PanelID + ' #txtLab').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                                            },
                                        });
                                    });
                                    ReportsSSRSDashboard.SelectResultTab('Lab');
                                } else {
                                    isParamatersAppend = false;
                                }
                            } else if (FormName.toLowerCase().trim() == "Vitals".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ClinicalVitals);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #VitalsAdvControls').hide();
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').append('<div class="clearfix"></div><div class="col-sm-4 mt-lg" id="Vitalslabel"><a href="javascript:void(0);" onclick="ReportsSSRSDashboard.showHideAdvControls(this,\'VitalsAdvControls\');" isShow="0">Advanced Search</a></div>');
                            }

                                //Clinical Reports ClinicalAllergies Ended
                            else if (FormName.toLowerCase().trim() == "CCM Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.CCMReport);
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ddlGender').prepend('<option value="" id="All" selected>- Select -</option>');
                                ReportsSSRSDashboard.LoadUsersAutocomplete('#ReportParamaters #txtCareCoordinator', '#ReportParamaters #hdnCareCoordinator');
                                ReportsSSRSDashboard.LoadUsersAutocomplete('#ReportParamaters #txtCareManager', '#ReportParamaters #hdnCareManager');
                                ReportsSSRSDashboard.LoadUsersAutocomplete('#ReportParamaters #txtCareGiver', '#ReportParamaters #hdnCareGiver');
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AdvanceSearchCCM').hide();


                                $('<div class="col-sm-4 col-md-3 pull-right mt-lg" id="CCMlabel"><a href="javascript:void(0);" class="pull-right" onclick="ReportsSSRSDashboard.showHideAdvControls(this,\'AdvanceSearchCCM\');" isShow="0">Advanced Search</a></div>').insertAfter('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .To');

                                for (var i = 0; i < ReportsSSRSDashboard.AdvanceSearchCCM.split(',').length; i++) {
                                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AdvanceSearchCCM').append($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .' + ReportsSSRSDashboard.AdvanceSearchCCM.split(',')[i]).clone());
                                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AdvanceSearchCCM .' + ReportsSSRSDashboard.AdvanceSearchCCM.split(',')[i]).show();
                                }
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .Provider [type=checkbox]').remove();
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .Provider label:nth-child(2)').remove();
                            }
                            else if (FormName.toLowerCase().trim() == "User Audit Report".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.UserAudit);

                            }
                            else if (FormName.toLowerCase().trim() == "Appointments Vs Claim".toLowerCase().trim()) {

                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.AppointmentClaim);
                            }
                            else if (FormName.toLowerCase().trim() == "Patient Statement Preference".toLowerCase().trim()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.PatientStatementPreference);
                            }
                            else if (FormName.toLowerCase().trim() == 'Void And Recreate Claims'.toLowerCase()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.VoidAndReCreateClaim);
                            }
                            else if (FormName.toLowerCase().trim() == 'Secondary Claims Report'.toLowerCase()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.SecondaryInsuranceClaims);
                            }
                            else if (FormName.toLowerCase().trim() == 'Drug Code Cost'.toLowerCase()) {
                                ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.DrugCodeCostReport);
                            }
                            else {
                                $('#DivWellComePanel').show();
                            }
                            var objdef = jQuery.Deferred();
                            var dataLoadzero = "IsActive=";
                            ReportsSSRSDashboard.populateSpecimen();
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadZero').loadDropDowns(true, dataLoadzero, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {

                                ReportsSSRSDashboard.saveAllLocations();
                                var dataActive = "IsActive=1";
                                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtFacilityMU3').trigger("change")
                                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadFirst').loadDropDowns(true, dataActive, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                                    var dataLedgerAccount = "IsActive=&ID=" + -1 + "&ID2=" + -1;
                                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadThird').loadDropDowns(true, dataLedgerAccount, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                                        var dataAllUsers = "IsActive=&ID=1";
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadSecond').loadDropDowns(true, dataAllUsers, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                                            objdef.resolve();
                                            objdef.promise();
                                        });
                                    });
                                });
                            });
                            objdef.then(function () {
                                if (FormName.toLowerCase().trim() != "Results".toLowerCase().trim()) {
                                    ReportsSSRSDashboard.ResultType = '';
                                }
                                if (FormName.toLowerCase().trim() != "Orders".toLowerCase().trim()) {
                                    ReportsSSRSDashboard.OrderType = '';
                                }
                                if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').html() != '') {

                                    if (isParamatersAppend) {
                                        setTimeout(function () {
                                            $('#BackgroundLoader').show();
                                        }, 10)
                                        ReportsSSRSDashboard.ShowHideDiv(FormName);
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').append($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .ActionsButtons').clone().show());
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').show();

                                        var ReportsControlDiv = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                        for (var i = 0; i < ReportsControlDiv.find('.datepickerStart').length; i++) {
                                            utility.ValidateFromToDate("frmSSRSReports #ReportParamaters", ReportsControlDiv.find('.datepickerStart')[i].id, ReportsControlDiv.find('.datepickerEnd')[i].id, true);
                                        }
                                        for (var i = 0; i < ReportsControlDiv.find('.datepickerMonthViewStart').length; i++) {
                                            utility.ValidateMonthViewFromToDate("frmSSRSReports #ReportParamaters", ReportsControlDiv.find('.datepickerMonthViewStart')[i].id, ReportsControlDiv.find('.datepickerMonthViewEnd')[i].id, true);
                                        }

                                        ReportsControlDiv.find('.datepickerClaim').each(function (i, item) {
                                            utility.CreateDatePicker('frmSSRSReports #ReportParamaters #' + $(item).attr('Id'),
                                                  function (ev) {
                                                      //on-change callback method 
                                                  }, false);
                                        });
                                        ReportsControlDiv.find('.datepicker').each(function (i, item) {
                                            utility.CreateDatePicker('frmSSRSReports #ReportParamaters #' + $(item).attr('Id'),
                                                  function (ev) {
                                                      //on-change callback method 
                                                  }, false);
                                        });

                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtPractice option').prop('selected', true);
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility option').prop('selected', true);
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider option').prop('selected', true);
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtSecondaryInsurance option').prop('selected', true);
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #LedgerType option[value=""]').remove();
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ApplyTo option[value=""]').remove();
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #SystemCategory option[value=""]').remove();
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #LedgerAccount option[value=""]').remove();

                                        if (FormName.toLowerCase().trim() == "Claim Scrubber Errors".toLowerCase().trim()) {
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtInsurancePlan option').prop('selected', true);
                                        }
                                        //if (FormName.toLowerCase().trim() != "Aging Summary Analysis".toLowerCase().trim()) {
                                        //    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txAppointmenttProvider option').prop('selected', true);
                                        //}
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AgingBucketsToDisplay option:first').prop('selected', true);
                                        if (FormName.toLowerCase().trim() == "Beginning AR Ending AR Facility".toLowerCase().trim()) {
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtResourceProvider option').prop('selected', true);
                                        }
                                        if (FormName.toLowerCase().trim() == "Appointments Vs Claim".toLowerCase().trim()) {
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #DDLAppointmentClaimNotes option').prop('selected', true);

                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ReportType').val(0);
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #Apptstatus option').prop('selected', true);
                                        }
                                        if (FormName.toLowerCase().trim() == "Claim Under Paid By Insurance".toLowerCase().trim()
                                            || FormName.toLowerCase().trim() == "Claim Over Paid By Insurance".toLowerCase().trim()
                                            || FormName.toLowerCase().trim() == "Secondary Claims Report".toLowerCase().trim()
                                            ) {
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtInsurancePlan option').prop('selected', true);
                                        }
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters select').multiselect({
                                            includeSelectAllOption: true,
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true
                                        });
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .datetimepicker').timepicker({
                                            defaultTime: false,
                                            minuteStep: 5
                                        });

                                    }
                                    ReportsSSRSDashboard.ReportId = reportpath;
                                    ReportsSSRSDashboard.ReportName = data.node.text
                                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').attr('disabled', false).css("cursor", "");
                                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null); //Report load source null, Azhar siyal & Abdur Rehman

                                    if (FormName.toLowerCase().trim() == "Financial Analysis At CPT Level".toLowerCase().trim() || FormName.toLowerCase().trim() == "Revenue By Facility".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Revenue By Provider".toLowerCase().trim() || FormName.toLowerCase().trim() == "Claim Comments By User".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Aging Summary Analysis".toLowerCase().trim() || FormName.toLowerCase().trim() == "Charges By Users".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Patient List".toLowerCase().trim() || FormName.toLowerCase().trim() == "Encounter without Claims".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Phone Encounter".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Referrals".toLowerCase().trim() || FormName.toLowerCase().trim() == "Historical Aging Summary Analysis".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Y to Y Monthly Financial Summary Analysis".toLowerCase().trim() || FormName.toLowerCase().trim() == "Claim Status Dashboard".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Void And Recreate Claims".toLowerCase().trim() || FormName.toLowerCase().trim() == "Patient List MPS".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "POS Surveys".toLowerCase().trim() || FormName.toLowerCase().trim() == "Monthly Scoreboard".toLowerCase().trim()
                                     || FormName.toLowerCase().trim() == "Claim Notes By User".toLowerCase().trim() || FormName.toLowerCase().trim() == "Collected Copayment".toLowerCase().trim()) {
                                        $("#ReportParamaters #btn-print").hide();
                                    }
                                    else {
                                        $("#ReportParamaters #btn-print").show();
                                    }
                                    if (FormName.toLowerCase().trim() == "Payment Entries".toLowerCase().trim()) {
                                        $("#pnlReportsSSRSDashboard #hfCPTCodeMulti").text("");
                                    }
                                    inactiveFacility = [];
                                    inactiveProvider = [];
                                    if (FormName.toLowerCase().trim() != "Procedures".toLowerCase().trim() && FormName.toLowerCase().trim() != "Results".toLowerCase().trim() && FormName.toLowerCase().trim() != "Orders".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.ActiveInactiveFacility();
                                        ReportsSSRSDashboard.ActiveInactiveProvider();
                                    }
                                    if (FormName.toLowerCase().trim() == "POS Surveys".toLowerCase().trim()) {
                                        $("#ReportParamaters #btn-Reset").removeClass('hidden')
                                    } else {
                                        $("#ReportParamaters #btn-Reset").addClass('hidden')
                                    }
                                    if (FormName.toLowerCase().trim() == "Beginning AR Ending AR".toLowerCase().trim() || FormName.toLowerCase().trim() == "Beginning AR Ending AR Facility".toLowerCase().trim() || FormName.toLowerCase().trim() == "AR Reconciliation Report".toLowerCase().trim() || FormName.toLowerCase().trim() == "Y to Y Monthly Financial Summary Analysis".toLowerCase().trim() || FormName.toLowerCase().trim() == "Claim Under Paid By Insurance".toLowerCase().trim() || FormName.toLowerCase().trim() == "Claim Over Paid By Insurance".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.SetDefaultClaimDate();
                                    }
                                    if (FormName.toLowerCase().trim() == "Beginning AR Ending AR".toLowerCase().trim() || FormName.toLowerCase().trim() == "Claim Under Paid By Insurance".toLowerCase().trim() || FormName.toLowerCase().trim() == "Claim Over Paid By Insurance".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.SetDefaultDOS();
                                    }
                                    if (FormName.toLowerCase().trim() == "Problems".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.SetProblemDefaultDOS();
                                    }
                                    if (FormName.toLowerCase().trim() == "User Audit Report".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.SetDefaultAuditDates();
                                    }
                                    if (FormName.toLowerCase().trim() == "Daily Copay Sheet".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.SetDefaultAppointmentDate();
                                    }
                                    if (FormName.toLowerCase().trim() == "Financial Analysis At CPT Level".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.FilterbyChargesVal();
                                        ReportsSSRSDashboard.LoadCPTMultiSelect();
                                    }
                                    if (FormName.toLowerCase().trim() == "Drug Code Cost".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.LoadDrugCPTMultiSelect();
                                    }
                                    if (FormName.toLowerCase().trim() == "Patient List".toLowerCase().trim() || FormName.toLowerCase().trim() == "Enterprise Scheduling".toLowerCase().trim() || FormName.toLowerCase().trim() == "charges list".toLowerCase().trim() || FormName.toLowerCase().trim() == "Financial Analysis At CPT Level".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.LoadRefProviderSimple();
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProvider').text("");
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProviderName').text("");
                                    }
                                    else if (FormName.toLowerCase().trim() == "Aging Summary Analysis".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.AgeReportByVal();
                                    }
                                    else if (FormName.toLowerCase().trim() == "Historical Aging Summary Analysis".toLowerCase().trim()) {
                                        ReportsSSRSDashboard.BindGroupsHistory($("#ReportParamaters #HistoryGroup1")[0], "HistoryGroup2");
                                    }
                                    else if (FormName.toLowerCase().trim() == "Appointments Vs Claim".toLowerCase().trim()) {
                                        // Hide Group Filter in Summary Case
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " .AppointmentClaimGroup1").hide();
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " .AppointmentClaimGroup2").hide();
                                        // Default Date Set
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " #AppointmentDateFrom").datepicker("setDate", new Date());
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " #AppointmentDateTo").datepicker("setDate", new Date());
                                        // Bind Event For Grouping Detail Report.
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " #ReportType").bind("change", function () {
                                            if ($(this).val() == 1) {
                                                $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " .AppointmentClaimGroup1").show();
                                                $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " .AppointmentClaimGroup2").show();
                                            } else {
                                                $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " .AppointmentClaimGroup1").hide();
                                                $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " .AppointmentClaimGroup2").hide();

                                            }
                                        });
                                    }
                                    else if (FormName.toLowerCase().trim() == "Patient Statement Preference".toLowerCase()) {
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #CreateDateFrom').datepicker("setDate", new Date());
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #CreateDateTo').datepicker("setDate", new Date());
                                    }
                                    else if (FormName.toLowerCase().trim() == "Monthly Payment Trend".toLowerCase()) {
                                        ReportsSSRSDashboard.SetDefaultMonthlyTrendPaymentDates();
                                    }
                                    else if (FormName.toLowerCase().trim() == "Payments by Users".toLowerCase().trim()) {
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " #ReportType > option:eq(1)").remove();
                                        $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " #ReportType").multiselect('rebuild');
                                    }
                                    ReportsSSRSDashboard.GroupOptionSelection(FormName);


                                    if (FormName.toLowerCase().trim() == "Revenue By Provider".toLowerCase().trim() || FormName.toLowerCase().trim() == "Revenue By Facility".toLowerCase().trim()) {
                                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AssignedUserId').val(globalAppdata.AppUserId);
                                    }
                                    setTimeout(function () {
                                        $('#BackgroundLoader').hide();
                                    }, 300);

                                } else {
                                    ReportsSSRSDashboard.ShowDefaultInformation(FormName);
                                }

                            });
                        } else {
                            ReportsSSRSDashboard.ShowDefaultInformation(FormName);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('#' + ReportsSSRSDashboard.SelectedReportID).find("a").prev().first().click();
                        }
                    }

                    //BackgroundLoaderShow(false);
                    //alert('stop');
                    //setTimeout(function () {
                    //    $('#BackgroundLoader').hide();
                    //}, 300)
                }

            });

            $('#' + ReportsSSRSDashboard.params["PanelID"] + " .search-input").keyup(function () {
                var searchString = $(this).val();
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').jstree('search', searchString);
            });


        });
        if (firstLoad) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Modules').change(function () {
                var moduleid = this.value;
                if (moduleid == '')
                    moduleid = 'All';
                if (moduleid != "") {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').jstree("destroy");
                    ReportsSSRSDashboard.ReportTree1(moduleid);
                    ReportsSSRSDashboard.DocumentReadyFunction();
                }
                else {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').jstree("destroy");
                    ReportsSSRSDashboard.ReportTree(null);
                }
            });
        }
    },

    ShowDefaultInformation: function (FormName) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #DivWellComePanel').show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').hide();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
        BackgroundLoaderShow(false);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').attr('disabled', false).css("cursor", "");
    },

    BindModules: function (response) {
        var Module_Detail = JSON.parse(response.REPORTSMODULE_JSON);
        Module_Detail = Module_Detail.sort(function (a, b) {
            if (a.ModuleName < b.ModuleName) return -1;
            if (a.ModuleName > b.ModuleName) return 1;
            return 0;
        });
        var modulename;
        var $select = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Modules');
        $select.find('option').remove();
        $select.append('<option value="">All</option>');

        $.each(Module_Detail, function (i, item) {
            modulename = item.ModuleName;
            if (modulename.indexOf('Reports') > -1) {
                $select.append('<option value=' + item.ModuleId + '>' + item.ModuleName + '</option>');
                Moduleids += item.ModuleId + ',';
            }
        });
        Moduleids = Moduleids.replace(undefined, '');
    },

    BindPatientAccount: function () {
        var Ctrl = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AccountNum');
        var func = function () { return utility.GetPatientArray(Ctrl.val()) };
        var hfCtrl = $("#" + ReportsSSRSDashboard.params["PanelID"] + " #hfAccNumber");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabReports';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, PatientName, FirstName, LastName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfAccNumber').val(PatientId);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #AccountNum').val(AccountNo);
        $("#" + ReportsSSRSDashboard.params["PanelID"] + " #PatientName").val(LastName + ", " + FirstName);

        UnloadActionPan(ReportsSSRSDashboard.params["TabID"]);
        //utility.InsertRecentPatient(PatientId);
    },


    BindModulesWithModuleID: function (response, ModuleID) {
        var Module_Detail = JSON.parse(response.REPORTSMODULE_JSON);
        var modulename;
        var $select = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Modules');
        $select.find('option').remove();
        $select.append('<option value="">All</option>');
        var ModuleList = jQuery.grep(Module_Detail, function (obj) {
            return obj.ModuleId === ModuleID;
        });
        $.each(ModuleList, function (i, item) {
            modulename = item.ModuleName;
            if (modulename.indexOf('Reports') > -1) {
                $select.append('<option value=' + item.ModuleId + '>' + item.ModuleName + '</option>');
                Moduleids += item.ModuleId + ',';
            }
        });
        Moduleids = Moduleids.replace(undefined, '');
    },

    GroupOptionSelection: function (FormName) {

        if (FormName.toLowerCase().trim() == "Encounter without Claims".toLowerCase().trim()) {
            $('#ReportParamaters').find('.GroupingFilterAgeReport').each(function (index, elmDiv) {
                $(this).find("select option:contains('Insurance Plan')").remove();
                $(this).find("select option:contains('Rendering Provider')").remove();
                $(this).find("select option:contains('CPT')").remove();
                $(this).find('select').multiselect('refresh');
                $(this).find('select').multiselect('rebuild');
            });
        }
        else if (FormName.toLowerCase().trim() == "Aging Summary Analysis".toLowerCase().trim()) {
            $('#ReportParamaters').find('.GroupingFilterAgeReport').each(function (index, elmDiv) {
                $(this).find("select option:contains('CPT')").remove();
                $(this).find("select option:contains('Visit type')").remove();
                $(this).find('select').multiselect('refresh');
                $(this).find('select').multiselect('rebuild');
            });
        }
    },


    FilterbyChargesVal: function () {
        $('#ReportParamaters #FilterChargesBy').change(function () {
            $('#ReportParamaters').find('.GroupingFilter').find('select option:first').prop("selected", "selected");
            $('#ReportParamaters').find('.GroupingFilter').find("input[type='checkbox']").prop('disabled', true);
            $('#ReportParamaters').find('.GroupingFilter').find("input[type='checkbox']").prop('checked', false);
            $('#ReportParamaters #Group1').parent().find('~ .GroupingFilter').each(function (index, elmDiv) {
                if ($(this).hasClass('disableAll') == false) {
                    $(this).addClass('disableAll');
                }
            });
        });
        if ($('#ReportParamaters #FilterChargesBy').val() == '1') {
            $('#ReportParamaters').find('.GroupingFilter').each(function (index, elmDiv) {
                $(this).find("select option:contains('Rendering Provider')").prop('disabled', true);
                $(this).find("select option:contains('Appiontment Provider')").prop('disabled', false);
                $(this).find('select').multiselect('refresh');
            });
        }
        else {
            $('#ReportParamaters').find('.GroupingFilter').each(function (index, elmDiv) {
                $(this).find("select option:contains('Appiontment Provider')").prop('disabled', true);
                $(this).find("select option:contains('Rendering Provider')").prop('disabled', false);
                $(this).find('select').multiselect('refresh');
            });
        }
    },

    AgeReportByVal: function () {
        $('#ReportParamaters #AgeReportBy').change(function () {
            $('#ReportParamaters').find('.GroupingFilterAgeReport').find('select option:first').prop("selected", "selected");
            $('#ReportParamaters').find('.GroupingFilterAgeReport').find("input[type='checkbox']").prop('disabled', true);
            $('#ReportParamaters').find('.GroupingFilterAgeReport').find("input[type='checkbox']").prop('checked', false);
            $('#ReportParamaters #AgeReportGroup1').parent().find('~ .GroupingFilterAgeReport').each(function (index, elmDiv) {
                if ($(this).hasClass('disableAll') == false) {
                    $(this).addClass('disableAll');
                }
            });
        });
        if ($('#ReportParamaters #AgeReportBy').val() == '1') {
            $('#ReportParamaters').find('.GroupingFilterAgeReport').each(function (index, elmDiv) {
                $(this).find("select option:contains('Rendering Provider')").prop('disabled', false);
                $(this).find("select option:contains('Appiontment Provider')").prop('disabled', false);
                $(this).find('select').multiselect('refresh');
            });
        }
        else {
            $('#ReportParamaters').find('.GroupingFilterAgeReport').each(function (index, elmDiv) {
                $(this).find("select option:contains('Appiontment Provider')").prop('disabled', false);
                $(this).find("select option:contains('Rendering Provider')").prop('disabled', false);
                $(this).find('select').multiselect('refresh');
            });
        }
    },
    BindGroups: function (control, nextcontrol) {
        var selected = $("#ReportParamaters #" + control.id).val();
        if (selected == 0) {
            $(control).parent().find('~ .GroupingFilter').each(function (index, elmDiv) {
                $(this).find('select option:first').prop("selected", "selected");
                $(this).find("input:checkbox").prop('disabled', true);
                $(this).find("input:checkbox").prop('checked', false);
                $(this).find('select').multiselect('refresh');

            });

        }
        var SelectedOptions = [];
        var NonSelectedOptions = [];
        $('#ReportParamaters .GroupingFilter').each(function (index, Objelement) {
            if ($(Objelement).find('select').hasClass('disableAll') == false) {
                $(Objelement).find('select option:selected').each(function (a, item) {
                    if (item.value != 0) {
                        SelectedOptions.push(item.value);
                    }
                });
                $(Objelement).find('select option:not(:selected)').each(function (a, item) {
                    if (item.value != 0) {
                        var index = NonSelectedOptions.indexOf(item.value);
                        if (index == -1) {
                            NonSelectedOptions.push(item.value);
                        }
                    }
                });
            }
        });
        $.each(NonSelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .GroupingFilter').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');
                var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                input.prop('disabled', false);
                input.parent('li').removeClass('disableAll')
                ReportsSSRSDashboard.FilterbyChargesVal();
            });
        });
        $.each(SelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .GroupingFilter').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');

                if ($(CurrentDDl).attr('id') == control.id && selected == elementSelected) {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', false);
                    input.parent('li').removeClass('disableAll')
                } else {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disableAll');
                }
            });
        });

        if (selected == 0) {
            $(control).parent().find('~ .GroupingFilter').each(function (index, elmDiv) {
                if ($(this).hasClass('disableAll') == false) {
                    $(this).addClass('disableAll');
                }
            });

        } else {
            $("#ReportParamaters #" + nextcontrol).parent().removeClass('disableAll');
        }
        if ($('#ReportParamaters #' + control.id).val() == 0) {
            $('#ReportParamaters #Subtotal' + control.id).prop('disabled', true);
            $('#ReportParamaters #Subtotal' + control.id).prop('checked', false);
        }
        else {
            $('#ReportParamaters #Subtotal' + control.id).prop('disabled', false);
        }
    },

    BindGroupsAppointmentClaim: function (control, nextcontrol, currentGroupName) {
        var selected = $("#ReportParamaters #" + control.id).val();
        if (selected == "") {
            $(control).parent().find('~ .GroupingFilter').each(function (index, elmDiv) {
                $(this).find('select option:first').prop("selected", "selected");
                $(this).find("input:checkbox").prop('disabled', true);
                $(this).find("input:checkbox").prop('checked', false);
                $(this).find('select').multiselect('refresh');

            });
        }
        var SelectedOptions = [];
        var NonSelectedOptions = [];
        $('#ReportParamaters .AppointmentClaimGroupingFilter, .PatientStatementGroupFilter, .VoidAndReCreateClaimGroupingFilter, .CollectionInGroupFilter').each(function (index, Objelement) {
            if ($(Objelement).find('select').hasClass('disableAll') == false) {
                $(Objelement).find('select option:selected').each(function (a, item) {
                    if (item.value != "") {
                        SelectedOptions.push(item.value);
                    }
                });
                $(Objelement).find('select option:not(:selected)').each(function (a, item) {
                    if (item.value != "") {
                        var index = NonSelectedOptions.indexOf(item.value);
                        if (index == -1) {
                            NonSelectedOptions.push(item.value);
                        }
                    }
                });
            }
        });
        $.each(NonSelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .AppointmentClaimGroupingFilter, .PatientStatementGroupFilter, .VoidAndReCreateClaimGroupingFilter, .CollectionInGroupFilter').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');
                var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                input.prop('disabled', false);
                input.parent('li').removeClass('disableAll')
                ReportsSSRSDashboard.FilterbyChargesVal();
            });
        });
        $.each(SelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .AppointmentClaimGroupingFilter, .PatientStatementGroupFilter,.VoidAndReCreateClaimGroupingFilter, .CollectionInGroupFilter').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');

                if ($(CurrentDDl).attr('id') == control.id && selected == elementSelected) {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', false);
                    input.parent('li').removeClass('disableAll')
                } else {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disableAll');
                }
            });
        });
        if (currentGroupName) {
            if (selected == 0 || selected == "") {
                //$(control).parent().find('~ .' + multiGroupName).each(function (index, elmDiv) {
                //    if ($(this).hasClass('disableAll') == false) {
                //        $(this).addClass('disableAll');
                //    }
                //});

            } else {
                $("#ReportParamaters #" + nextcontrol).parent().removeClass('disableAll');
            }
            if ($('#ReportParamaters #' + control.id).val() == 0 || $('#ReportParamaters #' + control.id).val() == "") {
                $('#ReportParamaters #Subtotal' + control.id).prop('disabled', true);
                $('#ReportParamaters #Subtotal' + control.id).prop('checked', false);
            }
            else {
                $('#ReportParamaters #Subtotal' + control.id).prop('disabled', false);
            }
        }

    },
    BindGroupsAgeReport: function (control, nextcontrol) {
        var selected = $("#ReportParamaters #" + control.id).val();
        if (selected == 0) {
            $(control).parent().find('~ .GroupingFilterAgeReport').each(function (index, elmDiv) {
                $(this).find('select option:first').prop("selected", "selected");
                $(this).find("input:checkbox").prop('disabled', true);
                $(this).find("input:checkbox").prop('checked', false);
                $(this).find('select').multiselect('refresh');

            });

        }
        var SelectedOptions = [];
        var NonSelectedOptions = [];
        $('#ReportParamaters .GroupingFilterAgeReport').each(function (index, Objelement) {
            if ($(Objelement).find('select').hasClass('disableAll') == false) {
                $(Objelement).find('select option:selected').each(function (a, item) {
                    if (item.value != 0) {
                        SelectedOptions.push(item.value);
                    }
                });
                $(Objelement).find('select option:not(:selected)').each(function (a, item) {
                    if (item.value != 0) {
                        var index = NonSelectedOptions.indexOf(item.value);
                        if (index == -1) {
                            NonSelectedOptions.push(item.value);
                        }
                    }
                });
            }
        });
        $.each(NonSelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .GroupingFilterAgeReport').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');
                var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                input.prop('disabled', false);
                input.parent('li').removeClass('disableAll')
                ReportsSSRSDashboard.AgeReportByVal();
            });
        });
        $.each(SelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .GroupingFilterAgeReport').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');

                if ($(CurrentDDl).attr('id') == control.id && selected == elementSelected) {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', false);
                    input.parent('li').removeClass('disableAll')
                } else {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disableAll');
                }
            });
        });

        if (selected == 0) {
            $(control).parent().find('~ .GroupingFilterAgeReport').each(function (index, elmDiv) {
                if ($(this).hasClass('disableAll') == false) {
                    $(this).addClass('disableAll');
                }
            });

        } else {
            $("#ReportParamaters #" + nextcontrol).parent().removeClass('disableAll');
        }
        if ($('#ReportParamaters #' + control.id).val() == 0) {
            $('#ReportParamaters #Subtotal' + control.id).prop('disabled', true);
            $('#ReportParamaters #Subtotal' + control.id).prop('checked', false);
        }
        else {
            $('#ReportParamaters #Subtotal' + control.id).prop('disabled', false);
        }
    },

    BindGroupsHistory: function (control, nextcontrol) {
        var selected = $("#ReportParamaters #" + control.id).val();
        if (selected == 0) {
            $(control).parent().find('~ .GroupingFilterHistory').each(function (index, elmDiv) {
                $(this).find('select option:first').prop("selected", "selected");
                $(this).find("input:checkbox").prop('disabled', true);
                $(this).find("input:checkbox").prop('checked', false);
                $(this).find('select').multiselect('refresh');

            });

        }
        var SelectedOptions = [];
        var NonSelectedOptions = [];
        $('#ReportParamaters .GroupingFilterHistory').each(function (index, Objelement) {
            if ($(Objelement).find('select').hasClass('disableAll') == false) {
                $(Objelement).find('select option:selected').each(function (a, item) {
                    if (item.value != 0) {
                        SelectedOptions.push(item.value);
                    }
                });
                $(Objelement).find('select option:not(:selected)').each(function (a, item) {
                    if (item.value != 0) {
                        var index = NonSelectedOptions.indexOf(item.value);
                        if (index == -1) {
                            NonSelectedOptions.push(item.value);
                        }
                    }
                });
            }
        });
        $.each(NonSelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .GroupingFilterHistory').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');
                var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                input.prop('disabled', false);
                input.parent('li').removeClass('disableAll')
                ReportsSSRSDashboard.HistoryByVal();
            });
        });
        $.each(SelectedOptions, function (index, elementSelected) {
            $('#ReportParamaters .GroupingFilterHistory').each(function (index, obj) {
                var CurrentDDl = $(obj).find('select');

                if ($(CurrentDDl).attr('id') == control.id && selected == elementSelected) {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', false);
                    input.parent('li').removeClass('disableAll')
                } else {
                    var input = $(obj).find('.multiselect-container').find('input[value="' + elementSelected + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disableAll');
                }
            });
        });

        if (selected == 0) {
            $(control).parent().find('~ .GroupingFilterHistory').each(function (index, elmDiv) {
                if ($(this).hasClass('disableAll') == false) {
                    $(this).addClass('disableAll');
                }
            });

        } else {
            $("#ReportParamaters #" + nextcontrol).parent().removeClass('disableAll');
        }
        if ($('#ReportParamaters #' + control.id).val() == 0) {
            $('#ReportParamaters #Subtotal' + control.id).prop('disabled', true);
            $('#ReportParamaters #Subtotal' + control.id).prop('checked', false);
        }
        else {
            $('#ReportParamaters #Subtotal' + control.id).prop('disabled', false);
        }
    },

    HistoryByVal: function () {
        $('#ReportParamaters #AgeReportBy').change(function () {
            $('#ReportParamaters').find('.GroupingFilterHistory').find('select option:first').prop("selected", "selected");
            $('#ReportParamaters').find('.GroupingFilterHistory').find("input[type='checkbox']").prop('disabled', true);
            $('#ReportParamaters').find('.GroupingFilterHistory').find("input[type='checkbox']").prop('checked', false);
            $('#ReportParamaters #HistoryGroup1').parent().find('~ .GroupingFilterHistory').each(function (index, elmDiv) {
                if ($(this).hasClass('disableAll') == false) {
                    $(this).addClass('disableAll');
                }
            });
        });
        if ($('#ReportParamaters #AgeReportBy').val() == '1') {
            $('#ReportParamaters').find('.GroupingFilterHistory').each(function (index, elmDiv) {
                $(this).find("select option:contains('Rendering Provider')").prop('disabled', false);
                $(this).find("select option:contains('Appiontment Provider')").prop('disabled', false);
                $(this).find('select').multiselect('refresh');
            });
        }
        else {
            $('#ReportParamaters').find('.GroupingFilterHistory').each(function (index, elmDiv) {
                $(this).find("select option:contains('Appiontment Provider')").prop('disabled', false);
                $(this).find("select option:contains('Rendering Provider')").prop('disabled', false);
                $(this).find('select').multiselect('refresh');
            });
        }
    },

    ReportTree: function (response) {
        if (response.status != false) {
            var pair = [];
            var arrmod = [];
            var arrforms = [];
            var userrights = JSON.parse(response.REPORTSRIGHT_JSON);
            ModulesList = userrights;

            //for (var i = 0; i < userrights.length; i++) {
            //    if (userrights[i].ModuleName.indexOf('Reports') > -1) {

            //    }


            //}
            for (var i = 0; i < userrights.length; i++) {
                var isPQRSReport = false;
                if (userrights[i].ModuleName.indexOf('Report') > -1) {

                    if (jQuery.inArray(userrights[i].ModuleId, arrmod) == -1) {

                        pair.push({ "id": userrights[i].ModuleId, "parent": "#", "text": userrights[i].ModuleName, 'icon': 'fa  fa-folder blue' });
                    }
                    arrmod.push(userrights[i].ModuleId);


                    if (jQuery.inArray(userrights[i].FormsId, arrforms) == -1 && userrights[i].ReportSSRSId != null && userrights[i].ReportSSRSId != "") {

                        if (userrights[i].FormName.indexOf('PQRS') > -1) {
                            userrights[i].FormName = 'PQRS Registry Report';
                            var isAlreadyPQRSpush = $.grep(pair, function (item) { return (item.text == 'PQRS Registry Report') });
                            if (isAlreadyPQRSpush == null || isAlreadyPQRSpush.length == 0) {
                                pair.push({ "id": userrights[i].FormsId, "parent": userrights[i].ModuleId, "text": userrights[i].FormName, 'icon': 'fa fa-bar-chart blue' });
                            }
                        } else {
                            pair.push({ "id": userrights[i].FormsId, "parent": userrights[i].ModuleId, "text": userrights[i].FormName, 'icon': 'fa fa-bar-chart blue' });
                        }


                    }
                    arrforms.push(userrights[i].FormsId);


                }
            }

            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').jstree({
                'core': {
                    'check_callback': true,
                    'data': pair
                },
                "search": {
                    "case_insensitive": true,
                    "show_only_matches": true,
                    "fuzzy": false,
                },
                "root": {
                    "icon": "/Content/Blue/images/tree_icon.png"//,
                    // "valid_children": ["file"]
                },
                'sort': function (a, b) {
                    a1 = this.get_node(a);
                    b1 = this.get_node(b);
                    if (a1.icon == b1.icon) {
                        return (a1.text > b1.text) ? 1 : -1;
                    } else {
                        return (a1.icon > b1.icon) ? 1 : -1;
                    }
                },

                "plugins": ["search", 'types', "sort"]
            });
        }

    },

    ReportTree1: function (moduleid) {
        ReportsSSRSDashboard.SearchReportsPriviliges(null).done(function (response) {
            if (response.status != false) {
                var pair = [];
                var arrmod = [];
                var arrforms = [];
                var Module_Detail = JSON.parse(response.REPORTSRIGHT_JSON);
                //filtering Modules List
                var ModulesList = jQuery.grep(Module_Detail, function (obj) {
                    return obj.ModuleId === moduleid || moduleid == 'All';
                });

                for (var i = 0; i < ModulesList.length; i++) {
                    if (ModulesList[i].ModuleName.indexOf('Report') > -1) {
                        if (jQuery.inArray(ModulesList[i].ModuleId, arrmod) == -1) {
                            if (ModulesList[i].ModuleId == moduleid || moduleid == 'All') {
                                pair.push({ "id": ModulesList[i].ModuleId, "parent": "#", "text": ModulesList[i].ModuleName, 'icon': 'fa  fa-folder blue' });
                            }
                        }
                        arrmod.push(ModulesList[i].ModuleId);
                    }
                }

                for (var i = 0; i < ModulesList.length; i++) {
                    if (ModulesList[i].ModuleName.indexOf('Report') > -1) {
                        if (jQuery.inArray(ModulesList[i].FormsId, arrforms) == -1 && ModulesList[i].ReportSSRSId != null && ModulesList[i].ReportSSRSId != "") {
                            if (ModulesList[i].ModuleId == moduleid || moduleid == 'All') {
                                var isAlreadyPQRSpush = $.grep(pair, function (item) { return (item.text == 'PQRS Registry Report') });;
                                if (ModulesList[i].FormName.indexOf('PQRS') > -1) {
                                    ModulesList[i].FormName = 'PQRS Registry Report';
                                    if (isAlreadyPQRSpush == null || isAlreadyPQRSpush.length == 0) {
                                        pair.push({ "id": ModulesList[i].FormsId, "parent": ModulesList[i].ModuleId, "text": ModulesList[i].FormName, 'icon': 'fa fa-bar-chart blue' });
                                    }
                                } else {
                                    pair.push({ "id": ModulesList[i].FormsId, "parent": ModulesList[i].ModuleId, "text": ModulesList[i].FormName, 'icon': 'fa fa-bar-chart blue' });
                                }

                            }
                        }
                        arrforms.push(ModulesList[i].FormsId);
                    }
                }
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').jstree({
                    'core': {
                        'check_callback': true,
                        'data': pair
                    },
                    "search": {

                        "case_insensitive": true,
                        "show_only_matches": true,
                        "fuzzy": false,
                    },
                    "root": {
                        "icon": "/Content/Blue/images/tree_icon.png"//,
                        // "valid_children": ["file"]
                    },
                    'sort': function (a, b) {
                        a1 = this.get_node(a);
                        b1 = this.get_node(b);
                        if (a1.icon == b1.icon) {
                            return (a1.text > b1.text) ? 1 : -1;
                        } else {
                            return (a1.icon > b1.icon) ? 1 : -1;
                        }
                    },

                    "plugins": ["search", 'types', "sort"]
                });
            }
        });
    },

    ResetParameter: function (Parameters) {
        if (ReportsSSRSDashboard.ReportName.toLowerCase().trim() == "POS Surveys".toLowerCase().trim()) {
            ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.POSSurvey);
        }

        setTimeout(function () {
            $('#BackgroundLoader').show();
        }, 10)
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').append($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .ActionsButtons').clone().show());
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').show();
        var objdef = jQuery.Deferred();
        var dataLoadzero = "IsActive=";
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadZero').loadDropDowns(true, dataLoadzero, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
            var dataActive = "IsActive=1";
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadFirst').loadDropDowns(true, dataActive, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                var dataLedgerAccount = "IsActive=&ID=" + -1 + "&ID2=" + -1;
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadThird').loadDropDowns(true, dataLedgerAccount, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                    var dataAllUsers = "IsActive=&ID=1";
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadSecond').loadDropDowns(true, dataAllUsers, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                        objdef.resolve();
                        objdef.promise();
                    });
                });
            });
        });
        objdef.then(function () {
            var ReportsControlDiv = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
            for (var i = 0; i < ReportsControlDiv.find('.datepickerStart').length; i++) {
                utility.ValidateFromToDate("frmSSRSReports #ReportParamaters", ReportsControlDiv.find('.datepickerStart')[i].id, ReportsControlDiv.find('.datepickerEnd')[i].id, true);
            }
            for (var i = 0; i < ReportsControlDiv.find('.datepickerMonthViewStart').length; i++) {
                utility.ValidateMonthViewFromToDate("frmSSRSReports #ReportParamaters", ReportsControlDiv.find('.datepickerMonthViewStart')[i].id, ReportsControlDiv.find('.datepickerMonthViewEnd')[i].id, true);
            }

            ReportsControlDiv.find('.datepickerClaim').each(function (i, item) {
                utility.CreateDatePicker('frmSSRSReports #ReportParamaters #' + $(item).attr('Id'),
                      function (ev) {
                          //on-change callback method 
                      }, false);
            });
            ReportsControlDiv.find('.datepicker').each(function (i, item) {
                utility.CreateDatePicker('frmSSRSReports #ReportParamaters #' + $(item).attr('Id'),
                      function (ev) {
                          //on-change callback method 
                      }, false);
            });

            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtPractice option').prop('selected', true);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility option').prop('selected', true);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider option').prop('selected', true);

            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters select').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .datetimepicker').timepicker({
                defaultTime: false,
                minuteStep: 5//,
            });

            //ReportsSSRSDashboard.ReportId = reportpath;
            //ReportsSSRSDashboard.ReportName = data.node.text
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').attr('disabled', false).css("cursor", "");
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null); //Report load source null, Azhar siyal & Abdur Rehman


            $("#ReportParamaters #btn-Reset").removeClass('hidden')
            $("#ReportParamaters #btn-print").hide();


            ReportsSSRSDashboard.LoadRefProviderSimple();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProvider').text("");
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProviderName').text("");

            setTimeout(function () {
                $('#BackgroundLoader').hide();
            }, 300);
        });
    },

    BindReport: function (FormName, url) {

        if (FormName == "" || FormName == "undefined" || typeof FormName == "undefined") {
            FormName = ReportsSSRSDashboard.ReportName
        }
        AppPrivileges.GetFormPrivileges(FormName, "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (FormName.toLowerCase().trim() == "Allergies".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateAllergiesReport();
                //} else
                //if (FormName.toLowerCase().trim() == "Vitals".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateVitalsReport();
                //}
                //    else if (FormName.toLowerCase().trim() == "Medications".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateMedicationsReport();
                //}
                //else 
                //if (FormName.toLowerCase().trim() == "Orders".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateOrdersReport();
                //}
                //else if (FormName.toLowerCase().trim() == "Procedures".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateProceduresReport();
                //}
                //else if (FormName.toLowerCase().trim() == "POS Surveys".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generatePOSSurveyReport();
                //}
                //else if (FormName.toLowerCase().trim() == "Progress Note".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateProgressNoteReport();
                //}
                //else if (FormName.toLowerCase().trim() == "Immunization".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateImmunizationReport();
                //}
                //else if (FormName.toLowerCase().trim() == "Phone Encounter".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generatePhoneEncounterReport();
                //}
                //Start   || 31 August, 2016 || Talha Tanweer  || 
                //else if (FormName.toLowerCase().trim() == "Problems".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateProblemsReport();
                //}
                //End   || 31 August, 2016 || Talha Tanweer  || 
                //else 
                //if (FormName.toLowerCase().trim() == "Results".toLowerCase().trim()) {
                //    ReportsSSRSDashboard.generateResultsReport();
                //}
                //else 
                if (FormName.toLowerCase().trim() == "CCM Report".toLowerCase().trim()) {
                    ReportsSSRSDashboard.generateCCmReport();
                }

                else
                    if (FormName == "MU Stage 1 Report" || FormName == "MU Stage 2 Report") {
                        ReportsSSRSDashboard.generateMuStageReport();
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').hide();
                    }
                    else if (FormName == "MU Stage 2 Report Latest" || FormName == "MU Stage 3 Report") {
                        ReportsSSRSDashboard.generateMuStageReportNumerator();
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').hide();
                    }
                    else {
                        $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
                        $('#MuStageReportView').hide();
                        var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                        if (QueryString) {
                            if (FormName.toLowerCase().trim() == "Insurance Plan AR".toLowerCase().trim()) {
                                QueryString += "&Self=" + $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtInsurancePlan option[value=self]').is(":checked");
                            }
                            if (url == "" || url == "undefined" || typeof url == "undefined") {
                                if (FormName.toLowerCase().trim() == "Financial Analysis At CPT Level".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.FinancialAnalysisURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                else if (FormName.toLowerCase().trim() == "Aging Summary Analysis".toLowerCase().trim() || FormName.toLowerCase().trim() == "Encounter without Claims".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.AgingSummaryAnalysisURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                else if (FormName.toLowerCase().trim() == "Historical Aging Summary Analysis".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.AgingSummaryAnalysisHistoricalURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                else if (FormName.toLowerCase().trim() == "Claim Comments By User".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.ClaimCommentsByUserURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                //else if (FormName.toLowerCase().trim() == "Claim Notes By User".toLowerCase().trim()) {
                                //    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.ClaimNotesByUserURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                //}
                                else if (FormName.toLowerCase().trim() == "AR Reconciliation Report".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.BeginingAREndinARDifferenceURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                else if (FormName.toLowerCase().trim() == "Charges By Users".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.ChargesByUserURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                else if (FormName.toLowerCase().trim() == "Y to Y Monthly Financial Summary Analysis".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.FinancialSummaryAnalysisURL(ReportsSSRSDashboard.ReportId);//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }
                                else if (FormName.toLowerCase().trim() == "Orders".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.OrdersReportURL(ReportsSSRSDashboard.ReportId);
                                    if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listLabOrder") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabOrderDiv');
                                    } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listRadiologyOrder") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyOrder');
                                    } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listProcedureOrder") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProcedureOrderDiv');
                                    } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listConsultationOrder") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationOrderDiv');
                                    } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listPrescriptionOrder") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #PrescriptionOrderDiv');
                                    }
                                }
                                else if (FormName.toLowerCase().trim() == "Results".toLowerCase().trim()) {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.ResultsReportURL(ReportsSSRSDashboard.ReportId);
                                    if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listLabResult") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabResultDiv');
                                    } else if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listRadiologyResult") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyResult');
                                    } else if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listConsultationResult") {
                                        QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationResultDiv');
                                    }
                                }
                                else if (FormName.toLowerCase().trim() == "Appointments Vs Claim".toLowerCase().trim()) {
                                    // for summary report
                                    if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ReportType option:selected').val() == 0)
                                    { url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.ReportId; }
                                        // detail report
                                    else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ReportType option:selected').val() == 1) {

                                        if (  // when there is no Group selected from both dropdown
                                           ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup1 option:selected').val() == ""
                                            &&
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup2 option:selected').val() == ""
                                            )
                                          )
                                            url = 'Controls/Reports/ReportViewer.aspx?reportpath=Charges and Payments Reports/AppointmentsClaimeDetail';

                                        else if (   // when only one option is selected from both group dropdown
                                             ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup1 option:selected').val() != ""
                                             &&
                                             $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup2 option:selected').val() == ""
                                             ) ||
                                             ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup2 option:selected').val() != ""
                                             &&
                                             $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup1 option:selected').val() == ""
                                             )
                                         )
                                            url = 'Controls/Reports/ReportViewer.aspx?reportpath=Charges and Payments Reports/AppointmentsClaimeDetailGroup1';
                                        else if ( // when  multiple option is selected from both group dropdown
                                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup1 option:selected').val() != ""
                                             &&
                                             $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #AppointmentClaimGroup2 option:selected').val() != ""
                                             )
                                            url = 'Controls/Reports/ReportViewer.aspx?reportpath=Charges and Payments Reports/AppointmentsClaimeDetailGroup2';


                                    }
                                }
                                else {
                                    url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.ReportId;//+ "&" + QueryString;// CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                                }

                            }
                            ReportsSSRSDashboard.SetQueryString(QueryString, FormName).done(function () {
                                urlpath = url;
                                ReportsSSRSDashboard.RunReport();
                            });
                        }
                    }
            }
            else {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').attr('disabled', false).css("cursor", "");;
                utility.DisplayMessages(strMessage, 2);
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
            }
        });

    },

    ImmunizationMandatoryFields: function (obj) {
        if (ReportsSSRSDashboard.ReportName == "Immunization") {
            if ($('#ReportParamaters #ImmunizationAlert').val() == "") {
                $("#ReportParamaters #SpnProviderSingleSelect").removeClass("hidden");
                $("#ReportParamaters #SpnAccountNumber").addClass("hidden");
            } else {
                $("#ReportParamaters #SpnAccountNumber").removeClass("hidden");
                $("#ReportParamaters #SpnProviderSingleSelect").addClass("hidden");
                $("#ReportParamaters #txtSignleSelectProvider").val(0);
                $("#ReportParamaters #txtSignleSelectProvider").multiselect('refresh');
            }
        }
    },

    ImmunizationProviderChange: function (obj) {
        if (ReportsSSRSDashboard.ReportName == "Immunization") {
            if ($('#ReportParamaters #txtSignleSelectProvider').val() == "0") {
                $("#ReportParamaters #SpnAccountNumber").removeClass("hidden");
                $("#ReportParamaters #SpnProviderSingleSelect").addClass("hidden");
            } else {
                $("#ReportParamaters #SpnProviderSingleSelect").removeClass("hidden");
                $("#ReportParamaters #SpnAccountNumber").addClass("hidden");
                $('#ReportParamaters #ImmunizationAlert').val(0);
                $('#ReportParamaters #ImmunizationAlert').multiselect('refresh');
            }
        }
    },

    FinancialAnalysisURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group4"]').val() > 0) {
            return ReportId;
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group3"]').val() > 0) {
            return ReportId + 'Group3';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group2"]').val() > 0) {
            return ReportId + 'Group2';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group1"]').val() > 0) {
            return ReportId + 'Group1';
        }
        else {
            return ReportId + 'LevelNoGroup';
        }
    },

    AgingSummaryAnalysisURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterAgeReport').find('select[id="AgeReportGroup4"]').val() > 0) {
            return ReportId + 'Group4';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterAgeReport').find('select[id="AgeReportGroup3"]').val() > 0) {
            return ReportId + 'Group3';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterAgeReport').find('select[id="AgeReportGroup2"]').val() > 0) {
            return ReportId + 'Group2';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterAgeReport').find('select[id="AgeReportGroup1"]').val() > 0) {
            return ReportId + 'Group1';
        }
        else {
            return ReportId;
        }
    },

    AgingSummaryAnalysisHistoricalURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterHistory').find('select[id="HistoryGroup4"]').val() > 0) {
            return ReportId + 'Group4';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterHistory').find('select[id="HistoryGroup3"]').val() > 0) {
            return ReportId + 'Group3';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterHistory').find('select[id="HistoryGroup2"]').val() > 0) {
            return ReportId + 'Group2';
        }
        else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilterHistory').find('select[id="HistoryGroup1"]').val() > 0) {
            return ReportId + 'Group1';
        }
        else {
            return ReportId;
        }
    },
    ClaimCommentsByUserURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupBy').find('select[id="GroupBy"]').val() == '1') {
            return ReportId + 'Grp';
        }
        else {
            return ReportId + 'dt';
        }
    },

    ClaimNotesByUserURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupBy').find('select[id="GroupBy"]').val() == '1') {
            return ReportId;
        }
        else {
            return ReportId + 'Date';
        }
    },

    BeginingAREndinARDifferenceURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.ReportType').find('select[id="ReportType"]').val() == '0') {
            return ReportId + 'Summary';
        }
        else {
            return ReportId;
        }
    },

    FinancialSummaryAnalysisURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.FinancialSummaryGroupBy').find('select[id="FinancialSummaryGroupBy"]').val() == '0') {
            return ReportId + 'Provider';
        }
        else {
            return ReportId;
        }
    },

    OrdersReportURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listLabOrder').hasClass('active')) {
            return ReportId + '_Lab';
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listRadiologyOrder').hasClass('active')) {
            return ReportId + '_Radiology';
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listProcedureOrder').hasClass('active')) {
            return ReportId + '_Procedure';
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listConsultationOrder').hasClass('active')) {
            return ReportId + '_Consultation';
        }
        else {
            return ReportId + '_Prescription';
        }
    },

    ResultsReportURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listLabResult').hasClass('active')) {
            return ReportId + '_Lab';
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listRadiologyResult').hasClass('active')) {
            return ReportId + '_Radiology';
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listConsultationResult').hasClass('active')) {
            return ReportId + '_Consultation';
        }
    },

    ChargesByUserURL: function (ReportId) {
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.ReportType').find('select[id="ReportType"]').val() == '1') {
            return ReportId + 'Dtl';
        }
        else {
            return ReportId + '';
        }
    },
    SetDefaultClaimDate: function () {
        $('#ReportParamaters #ClaimDateFrom').datepicker("setDate", new Date());
        $('#ReportParamaters #ClaimDateTo').datepicker("setDate", new Date());
    },

    SetDefaultDOS: function () {
        $('#ReportParamaters #DOSFrom').datepicker("setDate", new Date());
        $('#ReportParamaters #DOSTo').datepicker("setDate", new Date());
    },

    SetProblemDefaultDOS: function () {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        $('#ReportParamaters #ProbGivenDateFrom').datepicker("setDate", new Date(y, m, 1));
        $('#ReportParamaters #ProbGivenDateTo').datepicker("setDate", new Date());
    },


    SetDefaultAppointmentDate: function () {
        $('#ReportParamaters #AppointmentDate').datepicker("setDate", new Date());
        //$('#ReportParamaters #DOSTo').datepicker("setDate", new Date());
    },
    SetDefaultAuditDates: function () {
        $('#ReportParamaters #ActivityDateFrom').datepicker("setDate", new Date());
        $('#ReportParamaters #ActivityDateTo').datepicker("setDate", new Date());
    },
    SetDefaultMonthlyTrendPaymentDates: function () {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .datepickerMonthViewStart').datepicker("setDate", new Date(new Date().getFullYear(), 0, 1));
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .datepickerMonthViewEnd').datepicker("setDate", new Date(new Date().getFullYear(), new Date().getMonth(), 1));
    },
    GroupingParamString: function (ReportId) {
        var GroupingString = [];

        GroupingString.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group1"]').val())
        GroupingString.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group2"]').val())
        GroupingString.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group3"]').val())
        GroupingString.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.GroupingFilter').find('select[id="Group4"]').val())

        GroupingString.toString();
    },

    appendParamaters: function (FromControls) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' [type=hidden]').val('');
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' .form-group ').children().hide();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' .form-group ').find('[type=text]').val('');
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').children().remove();//.html('');
        for (var i = 0; i < FromControls.split(',').length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').append($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .' + FromControls.split(',')[i]).clone());
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .' + FromControls.split(',')[i]).show();
        }
    },

    RunReport: function () {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #DivWellComePanel').css("display", "none");
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', urlpath);

        var iframe = document.getElementById('ReportViewIframe');
        iframe.addEventListener('load', function () {
            var iframeBody = $(document.getElementById('ReportViewIframe').contentWindow.document).find("body");
            $(iframeBody).on('click', function (ev) {
                $('html').trigger('click');
            });
            iframeBody.find("#RptViewer_ReportViewer").find("#RptViewer_ctl05").css({ "position": "fixed", "top": "0", "width": "100%", "z-index": "102" });
            iframeBody.find("#RptViewer_ReportViewer").find("#RptViewer_ctl09").css("padding-top", "25px");
        });
        ReportsSSRSDashboard.IsHeaderFareez = true;
        ReportsSSRSDashboard.findHeader = null;
        ReportsSSRSDashboard.getHeaderRow = null;
        ReportsSSRSDashboard.headingRow = null;
        ReportsSSRSDashboard.RowCount = 0;
    },
    FreezHeader: function () {
        if (ReportsSSRSDashboard.ReportName.toLowerCase().trim() != "Financial Analysis At CPT Level".toLowerCase().trim()) {
            var MultiRowHeader;
            var iframeBody = $(document.getElementById('ReportViewIframe').contentWindow.document).contents().find("body");
            iframeBody.find(".headerRow").remove();
            iframeBody.find(".NewRow1").remove();
            // find target row in DOM 
            var getRow = iframeBody.find("#RptViewer_ReportViewer").find("#RptViewer_ctl09").find("#VisibleReportContentRptViewer_ctl09").children().children().children().children().children().children().children().children().eq("1");
            // get row to which we append in div
            var rowToAppend = $(getRow).children().children().children().children().children().children().children().children().last();
            var findRowHirarchy = $(rowToAppend).parent().html();
            if (findRowHirarchy.startsWith("<tbody>")) {
                var getRow = iframeBody.find("#RptViewer_ReportViewer").find("#RptViewer_ctl09").find("#VisibleReportContentRptViewer_ctl09").children().children().children().children().children().children().children().children().eq("0");
                var rowToAppend = $(getRow).children().children().children().children().children().children().children().children().last();
            }
            // when default page is being render
            if (ReportsSSRSDashboard.IsHeaderFareez == true || ReportsSSRSDashboard.RowCount < 2 || $(iframeBody).find("#RptViewer_ctl05_ctl00_CurrentPage").val() == 1) {
                ReportsSSRSDashboard.findHeader = $(rowToAppend).last();       // find header row
                ReportsSSRSDashboard.IsHeaderFareez = false;
                // find header and create Clone
                if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "User Audit Report".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis MPS".toLocaleLowerCase().trim()) {
                    var getMultiRowHeader = $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[1]);
                    MultiRowHeader = $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[1]);

                    ReportsSSRSDashboard.getHeaderRow = $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[2]);  // get header from nested table
                    ReportsSSRSDashboard.headingRow = $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[2]); // Create copy of header
                    // create css of copy header same as origional header
                    $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[1]).find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(MultiRowHeader).find("td").eq(k).css("width", getwidth) });
                    $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[2]).find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", getwidth) });
                    $(getMultiRowHeader).hide();


                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Zero Paid Claim".toLocaleLowerCase().trim()) {
                    var getMultiRowHeader = $(ReportsSSRSDashboard.findHeader).find("table:eq(0)").find("tr:eq(1)");
                    MultiRowHeader = $(ReportsSSRSDashboard.findHeader).find("table:eq(0)").find("tr:eq(1)").clone();
                    ReportsSSRSDashboard.getHeaderRow = $(ReportsSSRSDashboard.findHeader).find("table:eq(0)").find("tr:eq(2)");  // get header from nested table
                    ReportsSSRSDashboard.headingRow = $(ReportsSSRSDashboard.findHeader).find("table:eq(0)").find("tr:eq(2)").clone(); // Create copy of header
                    // create css of copy header same as origional header
                    $(ReportsSSRSDashboard.findHeader).find("table:eq(0)").find("tr:eq(1)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(MultiRowHeader).find("td").eq(k).css("width", getwidth) });
                    $(ReportsSSRSDashboard.findHeader).find("table:eq(0)").find("tr:eq(2)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", getwidth) });
                    $(getMultiRowHeader).hide();
                }
                else {
                    ReportsSSRSDashboard.getHeaderRow = $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[1]);  // get header from nested table
                    ReportsSSRSDashboard.headingRow = $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[1]); // Create copy of header
                    // create css of copy header same as origional header
                    $($(ReportsSSRSDashboard.findHeader).children().children().children().children()[1]).find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", getwidth) });
                }
                $(ReportsSSRSDashboard.getHeaderRow).hide();  // hide origional header
                $(getRow).children().children().children().children().children().children().children().children().last().hide();  // hide actual table 
                // append header copy and data custom created div 
                if ($(ReportsSSRSDashboard.headingRow).html()) {
                    if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "User Audit Report".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Zero Paid Claim".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis MPS".toLocaleLowerCase().trim()) { $(getRow).children().children().children().children().children().children().after("<div class='headerRow'><table cellpadding='0' cellspacing='0'><tr><td width='100%'><div class='headerCell' style='padding-right: 16px;'><table  class='headerTable' cellpadding='0' cellspacing='0' style='border-collapse: collapse;width:100%;'><tr>" + $(MultiRowHeader).html() + "</tr>" + $(ReportsSSRSDashboard.headingRow).html() + "</table></div></td></tr><tr><td><div class='NewRow1' style='max-height:390px;overflow-y:auto;overflow-x:hidden'><table cellpadding='0' cellspacing='0'><tbody>" + $(rowToAppend).html() + "</tbody></table></div></td></tr></table></div>"); }
                    else { $(getRow).children().children().children().children().children().children().after("<div class='headerRow'><table cellpadding='0' cellspacing='0'><tr><td width='100%'><div class='headerCell' style='padding-right: 16px;'><table class='headerTable' cellpadding='0' cellspacing='0' style='border-collapse: collapse;width:100%;'>" + $(ReportsSSRSDashboard.headingRow).html() + "</table></div></td></tr><tr><td><div class='NewRow1' style='max-height:390px;overflow-y:auto;overflow-x:hidden'><table cellpadding='0' cellspacing='0'><tbody>" + $(rowToAppend).html() + "</tbody></table></div></td></tr></table></div>"); }

                    if (ReportsSSRSDashboard.ReportName.toLowerCase().trim() == 'Zero Paid Claim'.toLowerCase()) {
                        iframeBody.find(".NewRow1").find("table:eq(1)").find("tr:eq(3)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); iframeBody.find(".headerCell").find("table").find("tr:eq(1)").find("td").eq(k).css("width", getwidth) });

                    }
                    if (ReportsSSRSDashboard.ReportName.toLowerCase().trim() == 'Monthly Payment Trend'.toLowerCase()) {
                        iframeBody.find(".NewRow1").find("table:eq(0)").find("tr:eq(0)").find("td:eq(0)").removeAttr("style");
                    }
                    if (iframeBody.find(".NewRow1").length > 0) {
                        if (iframeBody.find(".NewRow1").outerHeight() < 390) {
                            iframeBody.find(".headerCell").removeAttr("style");
                        }
                    }
                }
            }
            else {
                rowToAppend = $(getRow).children().children().children().children().children().last().clone();  // get DOM of second to onword Page 
                var IsHeaderFind = false;
                var IsHeaderCopied = false;
                var CopyMultiHeaderRow;
                if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Zero Paid Claim".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis MPS".toLocaleLowerCase().trim()) {
                    // get first row
                    CopyMultiHeaderRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(0)").find("table:eq(0)").find("tr:eq(1)");
                    $(getRow).children().children().children().children().children().children().children().find("tr:eq(0)").find("table:eq(0)").find("tr:eq(1)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(CopyMultiHeaderRow).find("td").eq(k).css("width", getwidth) });
                    $(getRow).children().children().children().children().children().children().children().find("tr:eq(0)").find("table:eq(0)").find("tr:eq(1)").hide();
                    // get 2nd row
                    ReportsSSRSDashboard.headingRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(0)").find("table:eq(0)").find("tr:eq(2)");
                    $(getRow).children().children().children().children().children().children().children().find("tr:eq(0)").find("table:eq(0)").find("tr:eq(2)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", getwidth) });
                    $(getRow).children().children().children().children().children().children().children().find("tr:eq(0)").find("table:eq(0)").find("tr:eq(2)").hide();
                    if (CopyMultiHeaderRow.length == 0) {
                        // get first row
                        CopyMultiHeaderRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(3)");
                        $(getRow).children().children().children().children().children().children().children().find("tr:eq(3)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(CopyMultiHeaderRow).find("td").eq(k).css("width", getwidth) });
                        $(getRow).children().children().children().children().children().children().children().find("tr:eq(3)").hide();
                        // get 2nd row
                        ReportsSSRSDashboard.headingRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(4)");
                        $(getRow).children().children().children().children().children().children().children().find("tr:eq(4)").find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", getwidth) });
                        $(getRow).children().children().children().children().children().children().children().find("tr:eq(4)").hide();
                    }
                    IsHeaderCopied = true;
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "historical aging summary analysis".toLocaleLowerCase().trim()) {
                    var group1 = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #HistoryGroup1').val();
                    var group2 = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #HistoryGroup2').val();
                    var group3 = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #HistoryGroup3').val();
                    var group4 = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #HistoryGroup4').val();
                    if ((group1 && group2 && group3 && group4 && parseInt(group1) > 0 && parseInt(group2) == 0 && parseInt(group3) == 0 && parseInt(group4) == 0)

                        ) {
                        ReportsSSRSDashboard.headingRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(2)");
                        $(getRow).children().children().children().children().children().children().children().find("tr:eq(2)").hide();
                    }
                    else if ((group2 && group3 && parseInt(group2) > 0 && parseInt(group3) == 0)
                        || (group3 && group4 && parseInt(group3) > 0 && parseInt(group4) == 0)
                        || (group4 && parseInt(group4) > 0)
                    ) {
                        ReportsSSRSDashboard.headingRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(1)");
                        if ($(ReportsSSRSDashboard.headingRow).attr("height") == "0") {
                            ReportsSSRSDashboard.headingRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(2)");
                            $(getRow).children().children().children().children().children().children().children().find("tr:eq(2)").hide();
                        }
                        else { $(getRow).children().children().children().children().children().children().children().find("tr:eq(1)").hide(); }
                    }
                    IsHeaderCopied = true;
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "insurance ar plan report".toLocaleLowerCase().trim()) {
                    ReportsSSRSDashboard.headingRow = $(getRow).children().children().children().children().children().children().children().find("tr:eq(1)");
                    $(getRow).children().children().children().children().children().children().children().find("tr:eq(1)").hide();
                    IsHeaderCopied = true;
                }
                else {
                    $(getRow).children().children().children().children().children().children().children().find("tr").each(function (key, value) {
                        if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "User Audit Report".toLocaleLowerCase().trim()) {

                            if (key == 1) { //  to skip top row for multiHeader Reports
                                IsHeaderFind = true;
                                CopyMultiHeaderRow = $(this).clone();
                                $(this).parent().find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); $(CopyMultiHeaderRow).find("td").eq(k).css("width", getwidth) });
                                $(this).hide();
                            }
                        }
                        if (!IsHeaderFind) {
                            $(this).find("td").each(function () {
                                var getBackGroundColor = $(this).css("background-color");  // get header row background color
                                if (getBackGroundColor == "rgb(70, 138, 234)") {
                                    if (!IsHeaderCopied) {
                                        ReportsSSRSDashboard.getHeaderRow = $(this).parent();  // copy  header row
                                        ReportsSSRSDashboard.headingRow = $(this).parent();

                                        var findTdWidth = '0px';
                                        // get default width for header first td   
                                        $(getRow).children().children().children().children().children().last().find("tr").each(function (k, v) {
                                            if (k == 0) {
                                                $(this).find("td").each(function (k1, v1) {
                                                    if (k1 == 0) {
                                                        if ($($(this)[0]).children().length == 0) { findTdWidth = $(this).css("width"); }  // get width only when td has no children
                                                    }
                                                    return false;
                                                });

                                            }
                                            return false;
                                        });
                                        // create css of copy header same as origional header
                                        $(this).parent().find("td").each(function (k, v) { var getwidth = $(this).outerWidth(); if (k == 0) { $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", findTdWidth) } else { $(ReportsSSRSDashboard.headingRow).find("td").eq(k).css("width", getwidth) } });
                                        $(this).parent().hide(); // hide origional header
                                        IsHeaderFind = true;
                                        IsHeaderCopied = true;
                                        return false;
                                    }
                                }
                            });
                            if (IsHeaderFind) {
                                return false;
                            }
                        }
                        // to skip top row
                        if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "User Audit Report".toLocaleLowerCase().trim()
                            || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Zero Paid Claim".toLocaleLowerCase().trim()
                            ) {
                            if (IsHeaderFind) {
                                IsHeaderFind = false;
                            }
                        }
                    });
                }
                $(getRow).children().children().children().children().children().last().hide();// hide actual table 
                if (IsHeaderCopied)
                    // append header copy and data custom created div 
                {

                    if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "User Audit Report".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Zero Paid Claim".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis MPS".toLocaleLowerCase().trim()) {
                        $(getRow).children().children().children().children().children().after("<div class='headerRow'><table cellpadding='0' cellspacing='0'><tr><td width='100%'><div class='headerCell' style='padding-right: 16px;'><table class='headerTable' cellpadding='0' cellspacing='0' style='border-collapse: collapse;width:100%;'><tr>" + $(CopyMultiHeaderRow).html() + "</tr>" + $(ReportsSSRSDashboard.headingRow).html() + "</table></div></td></tr><tr><td><div class='NewRow1' style='max-height:390px;overflow-y:auto;overflow-x:hidden'><table cellpadding='0' cellspacing='0' class='reportdata'><tbody>" + $(rowToAppend).html() + "</tbody></table></div></td></tr></table></div>");
                    }
                    else { $(getRow).children().children().children().children().children().after("<div class='headerRow'><table cellpadding='0' cellspacing='0'><tr><td width='100%'><div class='headerCell' style='padding-right: 16px;'><table class='headerTable' cellpadding='0' cellspacing='0' style='border-collapse: collapse;width:100%;'>" + $(ReportsSSRSDashboard.headingRow).html() + "</table></div></td></tr><tr><td><div class='NewRow1' style='max-height:390px;overflow-y:auto;overflow-x:hidden'><table cellpadding='0' cellspacing='0' class='reportdata'><tbody>" + $(rowToAppend).html() + "</tbody></table></div></td></tr></table></div>"); }

                }
                else {
                    $(getRow).children().children().children().children().children().after("<div class='headerRow'><table cellpadding='0' cellspacing='0'><tr><td><div class='NewRow1' style='max-height:390px;overflow-y:auto;overflow-x:hidden'><table cellpadding='0' cellspacing='0' class='reportdata'><tbody>" + $(rowToAppend).html() + "</tbody></table></div></td></tr></table></div>");
                }
                if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Appointments Vs Claim".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "AR Aging Analysis".toLocaleLowerCase().trim()) {
                    if (IsHeaderFind) {

                        iframeBody.find(".headerTable").removeAttr("style");
                        iframeBody.find(".headerTable").css("border-collapse", "collapse");
                        iframeBody.find(".headerCell").removeAttr("style");
                        var bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[1]).find("table").width();
                        if (bodyWidth == null) {
                            bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[0]).find("table").width();
                        }
                        bodyWidth = bodyWidth + 3;
                        iframeBody.find(".NewRow1").css("width", bodyWidth);
                        iframeBody.find(".headerTable").css("width", bodyWidth);
                    }
                }
                else if (ReportsSSRSDashboard.ReportName.toLowerCase().trim() == 'Void And Recreate Claims'.toLowerCase() || ReportsSSRSDashboard.ReportName.trim() == "Claims In Collection".trim()) {
                    if (IsHeaderFind) {

                        iframeBody.find(".headerTable").removeAttr("style");
                        iframeBody.find(".headerTable").css("border-collapse", "collapse");
                        iframeBody.find(".headerCell").removeAttr("style");
                        var bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[1]).find("table").width(); //get tbody width
                        if (bodyWidth == null) {
                            bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[0]).find("table").width();
                        }
                        iframeBody.find(".NewRow1").css("width", bodyWidth + 30);
                        iframeBody.find(".headerTable").css("width", bodyWidth + 13);
                    }
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Encounter without Claims".toLocaleLowerCase().trim()) {
                    iframeBody.find(".headerCell").removeAttr("style");
                    iframeBody.find(".headerCell").css("padding-right", "22px");
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Payment Entries".toLocaleLowerCase().trim()) {
                    iframeBody.find(".headerCell").removeAttr("style");
                    iframeBody.find(".headerCell").css("padding-right", "20px");
                    iframeBody.find(".headerCell").find("table:eq(0) > tbody >tr:eq(0) > td:eq(0) ").css("width", "0px");
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Patient Statement Preference".toLocaleLowerCase().trim()) {
                    iframeBody.find(".headerCell").removeAttr("style");
                    iframeBody.find(".headerCell").css("padding-right", "132px");
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Monthly Payment Trend".toLocaleLowerCase().trim()) {
                    iframeBody.find(".headerTable").removeAttr("style");
                    iframeBody.find(".headerTable").css("border-collapse", "collapse");
                    iframeBody.find(".headerCell").removeAttr("style");
                    var bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[1]).find("table").width(); //get tbody width
                    if (bodyWidth == null) {
                        bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[0]).find("table").width();
                    }
                    iframeBody.find(".NewRow1").css("width", bodyWidth + 73);
                    iframeBody.find(".headerTable").css("width", bodyWidth + 60);

                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Patient AR".toLocaleLowerCase().trim() || ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "Claim Follow Up".toLocaleLowerCase().trim()) {
                    iframeBody.find(".headerTable").removeAttr("style");
                    iframeBody.find(".headerTable").css("border-collapse", "collapse");
                    iframeBody.find(".headerCell").removeAttr("style");
                    var bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[1]).find("table").width(); //get tbody width
                    if (bodyWidth == null) {
                        bodyWidth = $(iframeBody.find(".NewRow1").find("table:eq(1)").find("tr")[0]).find("table").width();
                    }
                    iframeBody.find(".NewRow1").css("width", bodyWidth);


                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "historical aging summary analysis".toLocaleLowerCase().trim()) {
                    iframeBody.find(".headerTable").removeAttr("style");
                    if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "historical aging summary analysis".toLocaleLowerCase().trim())
                    { iframeBody.find(".headerTable tbody tr > td:first").remove(); }
                }
                else if (ReportsSSRSDashboard.ReportName.toLocaleLowerCase().trim() == "insurance ar plan report".toLocaleLowerCase().trim()) {
                    var bodyWidth = $(iframeBody.find(".NewRow1")).width();
                    iframeBody.find(".headerCell").css("width", bodyWidth);

                    iframeBody.find(".NewRow1").find("table:eq(1) > tbody > tr:eq(2) td").each(function (k, v) {
                        if (k <15) {
                            var tdwidth = $(this).css("width");
                            $(ReportsSSRSDashboard.headingRow).children().eq(k).css("min-width", tdwidth);
                        }
                    });
                }
                if (iframeBody.find(".NewRow1").length > 0) {
                    if (iframeBody.find(".NewRow1").outerHeight() < 390) {
                        iframeBody.find(".headerCell").removeAttr("style");
                    }
                }
            }
            ReportsSSRSDashboard.RowCount++;
        }
    },

    SearchReportsPriviliges: function (ModuleId) {
        var data = "ModuleId=" + ModuleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "SEARCH_REPORTS_PRIVILEGES");
    },

    AdminSearchReportsPriviliges: function (ModuleId) {
        var data = "ModuleId=" + ModuleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "SEARCH_ADMINREPORTS_PRIVILEGES");
    },

    BindAutoComplete: function (RefCntrl, RefHiddenCntrl) {
        var entityId = globalAppdata["SeletedEntityId"];
        //if ($('#' + ReportsSSRSDashboard.params["PanelID"] + "  #ReportParamaters input#txtCPTCode").length > 0) {
        //    if (globalAppdata['IMO_ID'] == "") {
        //        //CacheManager.BindAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #' + RefCntrl, 'GetCPTCode', true, '#' + ReportsSSRSDashboard.params["PanelID"] + ' #' + RefHiddenCntrl, entityId);
        //    }
        //    else {
        //        utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCode', entityId, true, '', 'CPT', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        //    }
        //}
        //if ($('#' + ReportsSSRSDashboard.params["PanelID"] + "  #ReportParamaters input#txtICD").length > 0) {
        //    if (globalAppdata['IMO_ID'] == "") {
        //        //CacheManager.BindAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtICD', 'GetICDCode', true, '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCode', entityId, true);
        //    }
        //    else {
        //        utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtICD', 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCode', entityId, true, '', 'ICD', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        //    }
        //}

        if (RefCntrl == "txtICD") {
            utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtICD', 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCode', entityId, true, '', 'ICD', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        }
        else if (RefCntrl == "txtCPTCode") {
            utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCode', entityId, true, '', 'CPT', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        }
        else if (RefCntrl == "CPTCodeFrom") {
            utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #CPTCodeFrom', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeFrom', entityId, true, '', 'CPT', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        }
        else if (RefCntrl == "CPTCodeTo") {
            utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #CPTCodeTo', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeTo', entityId, true, '', 'CPT', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        }
        else if (RefCntrl == "txtCPTCodeRadiology") {
            utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtCPTCodeRadiology', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeRadiology', entityId, true, '', 'CPT', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        }
        else {
            utility.BindIMOAutoCompleteText('#' + RefCntrl, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + RefHiddenCntrl, entityId, true, '', 'CPT', true, ReportsSSRSDashboard.params.TabID, RefCntrl, true);
        }


        //var entityId = globalAppdata["SeletedEntityId"];
        //if ($('#' + ReportsSSRSDashboard.params["PanelID"] + "  #ReportParamaters input#" + RefCntrl).length > 0) {

        //    utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #' + RefCntrl, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #' + RefHiddenCntrl, entityId, true, -1, "CPT", true, "ReportsSSRSDashboard", null, false);

        //    //if (globalAppdata['IMO_ID'] == "") {
        //    //    CacheManager.BindAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #' + RefCntrl, 'GetCPTCode', true, '#' + ReportsSSRSDashboard.params["PanelID"] + ' #' + RefHiddenCntrl, entityId);
        //    //}
        //    //else {
        //    //    utility.BindAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #' + RefCntrl, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #' + RefHiddenCntrl, entityId);
        //    //}
        //}
        //if ($('#' + ReportsSSRSDashboard.params["PanelID"] + "  #ReportParamaters input#txtICD").length > 0) {

        //    utility.BindIMOAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtICD', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCode', entityId, true, -1, "ICD", true, "ReportsSSRSDashboard", null, false);


        //        //if (globalAppdata['IMO_ID'] == "") {
        //        //    CacheManager.BindAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtICD', 'GetICDCode', true, '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCode', entityId, true);
        //        //}
        //        //else {
        //        //    utility.BindAutoCompleteText('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtICD', 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCode', entityId, true);
        //        //}
        //}
    },

    ClearHiddenField: function (Cntrl, HiddenCntrlId) {

        if ($(Cntrl).val() == "") {
            $("#pnlReportsSSRSDashboard #" + HiddenCntrlId).val("");
        }
    },

    POSSurveyCheckboxes: function (obj) {
        if ($(obj).prop('id') == "chkFieldsAll" && $(obj).prop('checked') == true) {
            $('#ReportParamaters').find('.ChkFieldsPOSSurvey').find('input[type=checkbox]').prop('checked', true);
        } else if ($(obj).prop('id') == "chkFieldsAll" && $(obj).prop('checked') == false) {
            $('#ReportParamaters').find('.ChkFieldsPOSSurvey').find('input[type=checkbox]').prop('checked', false);
        } else if ($(obj).prop('id') != "chkFieldsAll" && $(obj).prop('checked') == false) {
            $('#ReportParamaters').find('.ChkFieldsPOSSurvey').find('input[id=chkFieldsAll]').prop('checked', false);
        }
    },

    //--------------ICD------------------
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {
            controlToLoad = "Admin_ICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_CPTCode";
        }

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            LoadActionPan(controlToLoad, params);
        }

    },
    //--------------END ICD------------------
    //-------------Resources----------------
    OpenResource: function () {
        var params = [];
        params["ResourceId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_Resources', params);
    },
    //-----------End Resources

    //------------------Insurance Plan----------------
    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_InsurancePlan', params);
    },
    //end Insurance Plan
    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmSSRSReports";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_Provider', params);
    },

    OpenOrderProvider: function (RefCtrl, RefHiddenCtrl) {
        var params = [];
        params["IsOptional"] = true;
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmSSRSReports";
        params["ParentCtrl"] = "mstrTabReports";
        params["RefCtrlHidden"] = RefHiddenCtrl;
        params["RefCtrl"] = RefCtrl;
        params["ProviderId"] = "-1";
        //params["EntityId"] = globalAppdata["SeletedEntityId"];
        LoadActionPan('Admin_Provider', params);
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmSSRSReports";
        params["FacilityId"] = "-1";
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_Facility', params);
    },


    // -------------- End Facility -----------------

    // -------------- CPT Code -----------------
    OpenICDDetail: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        params["RefCtrl"] = "txtICD";
        params["RefHiddenCtrl"] = "hfICDCode";
        params["EntityId"] = globalAppdata["SeletedEntityId"];
        LoadActionPan('Admin_IMOICD', params);
    },

    OpenCPTCode: function (RefCtrl, RefHiddenCtrl) {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        params["RefHiddenCtrl"] = RefHiddenCtrl;

        params["RefCtrl"] = RefCtrl;
        params["EntityId"] = globalAppdata["SeletedEntityId"];
        LoadActionPan('Admin_IMOCPT', params);
    },
    // -------------- Ref Provider -----------------
    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #txtRefProvider').val(RefProviderName);
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #hfRefProvider').val(RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_Practice', params);
    },

    PrintReport: function () {
        //ReportNameParam = ReportsSSRSDashboard.ReportName;
        var params = [];
        var ReportName = ReportsSSRSDashboard.ReportName.toLowerCase().split(' ').join('');
        // PMS-972, Abdur rehman latif. Report doesnot show data in PDF veiw so changed it back to the normal print screen that we are using for other reports
        if (ReportName == "appointmentreminders" || ReportName == "ccmreport") { //ReportName == "dailyappointmentreminders" ||  || ReportName == "problems"|| ReportName == "procedures"|| ReportName == "vitals"  || ReportName == "immunization" || ReportName == "medications" || ReportName == "orders" || ReportName == "results"
            // PMS-972, Abdur rehman latif. Report doesnot show data in PDF veiw so changed it back to the normal print screen that we are using for other reports
            $('#clinicalKenduGrid .k-grid-header').css('padding-right', '0px');
            ReportsSSRSDashboard.getPrintPDF();
        } else {
            if (ReportName == "financialanalysisatcptlevel" || ReportName == "revenuebyfacility" || ReportName == "revenuebyprovider") {
                params["PreviewPdf"] = true;
            } else {
                params["PreviewPdf"] = false;
            }
            if (ReportName == "mustage1report" || ReportName == "mustage2report" || ReportName == "mustage2reportlatest" || ReportName == "mustage3report") {
                params["ReportPrintHTML"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #MuStageReportView').html();
            }
            if (ReportName == "appointmentsvsclaim" && $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters' + " #ReportType").val() == 1) {

                params["ReportName"] = "Appointments Vs Claim Detail";
            }
            else {
                params["ReportName"] = ReportsSSRSDashboard.ReportName;
            }
            params["ParentCtrl"] = "mstrTabReports";
            LoadActionPan('ReportsSSRSPrintView', params);
        }
    },

    SetQueryString: function (QueryString, FormName) {

        var data = QueryString;//"QueryString=" + QueryString + "&FormName=" + FormName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "SETQUERYSTRING_REPORTS");
    },

    CreateQuery1: function (DivID) {

        //var self = $(DivID);
        //var ContolsKeys = "ProviderId,AppointmentProviderId,ResourceProviderId,RenderingProviderId,FacilityId,PracticeId,RefProviderId,InsurancePlanId,ResourceId,CPTCode,ICDCode,PlanType,SpecialtyId,PlanCategoryId,ProcedureCategoryId,PaymentTypeId,ApplyToId,SystemCategoryId,LedgerAccountId,LedgerTypeId,CPTCodeFrom,CPTCodeTo,UserId,SecurityRoleId,Subtotal1,Subtotal2,Subtotal3,Subtotal4,DOSStart,DOSEnd,AgingBucketsToDisplay,EnteredBy"; // Temporary Checkin : Abdur Rehman 
        //var ContolsValues = "hfProvider,hfAppointmentProvider,hfResourceProvider,hfRenderingProvider,hfFacility,hfPractice,txtRefProvider,txtInsurancePlan,hfResource,txtCPTCode,txtICD,ddlPlanCategory,Specialty,ddlInsurancePlanCategory,ProcedureCategory,PaymentType,ApplyTo,SystemCategory,LedgerAccount,LedgerType,CPTCodeFrom,CPTCodeTo,User,SecurityRole,SubtotalGroup1,SubtotalGroup2,SubtotalGroup3,SubtotalGroup4,DOSStartDays,DOSEndDays,AgingBucketsToDisplay,EnteredBy";
        //var ControlsDiv = "txtProvider,txAppointmenttProvider,txtResourceProvider,txtRenderingProvider,txtFacility,txtPractice,txtRefProvider,txtInsurancePlan,txtResource,txtCPTCode,txtICD,ddlPlanCategory,Specialty,ddlInsurancePlanCategory,ProcedureCategory,PaymentType,ApplyTo,SystemCategory,LedgerAccount,LedgerType,CPTCodeFrom,CPTCodeTo,User,SecurityRole,SubtotalGroup1,SubtotalGroup2,SubtotalGroup3,SubtotalGroup4,DOSStartDays,DOSEndDays,AgingBucketsToDisplay,EnteredBy";
        ////Practice txtPractice hfPractice
        ////Provider txtProvider hfProvider
        ////Facility txtFacility hfFacility
        ////Ref Provider txtRefProvider hfRefProvider
        ////Insurance plan hfInsurancePlan
        //var kvpairs = [];
        //var isMultiselected = true;
        //var isValidDate = true;
        //self.find('[type=hidden],[type=text],[type=number], textarea').each(function () {
        //    var CurrentID = this.id;

        //    if (CurrentID != "" && $.data($(this).get(0), 'datepicker') != null && $(this).val().length < 7 && $(this).val() != '') {
        //        if (($(this).hasClass("datepickerMonthViewEnd") || $(this).hasClass("datepickerMonthViewStart")) && $(this).val().length < 7) {
        //            isValidDate = false;
        //        } else if ($(this).val().length < 10) {
        //            isValidDate = false;
        //        }
        //    } else if (CurrentID != "") {
        //        var date_format = 'dd/mm/yyyy';
        //        //set default Date Formate
        //        if (globalAppdata['DateFormat'])
        //            date_format = globalAppdata['DateFormat'];
        //        date_format = date_format.replace('yyyy', 'yy');
        //        var SelectedId = "";
        //        for (var i = 0; i < ControlsDiv.split(',').length; i++) {
        //            if (ControlsDiv.split(',')[i] === CurrentID) {
        //                SelectedId = ContolsKeys.split(',')[i]
        //                break;
        //            }
        //        }
        //        if (SelectedId != "") {
        //            if ($.data($(this).get(0), 'datepicker') != null && $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val() != '') {
        //                //For datepicker up to month view, We need the last day of to date field so that we can good date
        //                var selectedDate = $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).datepicker('getDate');
        //                if ($(this).hasClass("datepickerMonthViewEnd")) {
        //                    var date = selectedDate, y = selectedDate.getFullYear(), m = selectedDate.getMonth();
        //                    selectedDate = new Date(y, m + 1, 0);
        //                }
        //                kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($.datepicker.formatDate(date_format, selectedDate)));
        //            } else {
        //                if (ContolsValues.split(',')[i].indexOf('hf') > -1) {
        //                    kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val()));
        //                } else
        //                    kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + ContolsValues.split(',')[i]).val()));
        //            }

        //        } else {
        //            if ($.data($(this).get(0), 'datepicker') != null && this.value != '') {
        //                //For datepicker up to month view, We need the last day of to date field so that we can good date
        //                var selectedDate = $(this).datepicker('getDate');
        //                if ($(this).hasClass("datepickerMonthViewEnd")) {
        //                    var date = selectedDate, y = selectedDate.getFullYear(), m = selectedDate.getMonth();
        //                    selectedDate = new Date(y, m + 1, 0);
        //                }
        //                kvpairs.push(encodeURIComponent(this.id) + "=" + encodeURIComponent($.datepicker.formatDate(date_format, selectedDate)));
        //            } else {
        //                if (CurrentID == "ClaimDateFrom" && this.value == "") {
        //                    setTimeout(function () {
        //                        utility.DisplayMessages("Claim Date From is not selected", 2);
        //                    }, 500);
        //                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
        //                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
        //                }
        //                if (CurrentID == "ClaimDateTo" && this.value == "") {
        //                    setTimeout(function () {
        //                        utility.DisplayMessages("Claim Date To is not selected", 2);
        //                    }, 500);
        //                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
        //                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
        //                }
        //                kvpairs.push(encodeURIComponent(this.id) + "=" + encodeURIComponent(this.value));
        //            }
        //        }
        //    }
        //});
        //self.find('[type=checkbox][id], [type=radio][id]').each(function () {
        //    var CurrentID = this.id;
        //    if (CurrentID != "" && CurrentID.indexOf("chkInclude") < 0) {
        //        var SelectedId = "";
        //        for (var i = 0; i < ControlsDiv.split(',').length; i++) {
        //            if (ControlsDiv.split(',')[i] === CurrentID) {
        //                SelectedId = ContolsKeys.split(',')[i]
        //                break;
        //            }
        //        }
        //        if (SelectedId != "") {
        //            kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent((this.checked) ? true : false));
        //        } else {
        //            kvpairs.push(encodeURIComponent(this.id) + "=" + encodeURIComponent((this.checked) ? true : false));
        //        }
        //    }
        //});
        //self.find('select').each(function () {
        //    var CurrentID = this.id;
        //    if (CurrentID != "") {
        //        var SelectedId = "";
        //        for (var i = 0; i < ControlsDiv.split(',').length; i++) {
        //            if (ControlsDiv.split(',')[i] === CurrentID) {
        //                SelectedId = ContolsKeys.split(',')[i]
        //                break;
        //            }
        //        }
        //        if (SelectedId != "") {
        //            var Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected').map(function (a, item) { return item.value; }).get().join();
        //            if (CurrentID == 'txtInsurancePlan') {
        //                Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option[value!=self]:selected').map(function (a, item) { return item.value; }).get().join();
        //            }
        //            if (Selectedvalues == "" && (SelectedId == "PracticeId" || SelectedId == "FacilityId" || SelectedId == "ProviderId") && $(this).hasClass("multiselect")) {
        //                setTimeout(function () { utility.DisplayMessages(SelectedId.slice(0, -2) + " is not selected", 2); }, 500);
        //                isMultiselected = false;
        //                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
        //                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId.slice(0, -2) + " is not selected");
        //            }
        //            if (Selectedvalues == "" && (SelectedId == "AgingBucketsToDisplay") && $(this).hasClass("multiselect")) {
        //                setTimeout(function () { utility.DisplayMessages(SelectedId + " is not selected", 2); }, 500);
        //                isMultiselected = false;
        //                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
        //                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId + " is not selected");
        //            }
        //            kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent(Selectedvalues));
        //        } else {
        //            if (CurrentID == "IsActive" || CurrentID == "FilterChargesBy") {
        //                kvpairs.push(encodeURIComponent(CurrentID) + "=" + encodeURIComponent((typeof $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected') == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected').val() == "") ? "" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected').val() == "1" ? true : false)));
        //            } else {
        //                kvpairs.push(encodeURIComponent(CurrentID) + "=" + encodeURIComponent(typeof $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected') == 'undefined' ? "" : $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected').map(function (a, item) { return item.value; }).get().join()));
        //            }
        //        }
        //    }
        //});
        //if (isMultiselected && isValidDate) {
        //    var queryStrings = kvpairs.join("&");
        //    $("#" + ReportsSSRSPrintView.params["PanelID"] + " #btn-print").attr('disabled', false);
        //    return queryStrings;
        //} else {
        //    $("#" + ReportsSSRSPrintView.params["PanelID"] + " #btn-print").attr('disabled', true);
        //    return false;
        //}

    },

    CreateQuery: function (DivID) {

        var self = $(DivID);
        var ContolsKeys = "chkIsAmendmentForBilling,CPTCode,CPTCode,CPTCode,CPTCode,txtDrugCPTCodeMulti,ICDCode,ICDCode,PlanType,ApplyToId,SystemCategoryId,LedgerAccountId,LedgerTypeId,CPTCodeFrom,CPTCodeTo,Subtotal1,Subtotal2,Subtotal3,Subtotal4,DOSStart,DOSEnd,AgingBucketsToDisplay,ReferalToId,ReferalFormId,RefProviderId,AssignedUserId,PCPId,SystolicFrom,SystolicTo,TempFrom,TempTo,HeightFrom,HeightTo,SPO2From,SPO2To,DiastolicFrom,DiastolicTo,RespFrom,RespTo,BMIFrom,BMITo,PulseRateFrom,PulseRateTo,WeightFrom,WeightTo,BSAFrom,BSATo,Test,OrderNumber,CPTCodeProdcedure,OrderNumber,OrderNumber,OrderNumber,IncludeUnLinkCopay";
        var ContolsValues = "chkIsAmendmentForBilling,txtCPTCode,hfCPTCodeRadiology,txtCPTCodeConsultation,hfCPTCodeMulti,hfDrugCPTCodeMulti,hfICDCodeMultiSelect,txtICD,ddlPlanCategory,ApplyTo,SystemCategory,LedgerAccount,LedgerType,CPTCodeFrom,CPTCodeTo,SubtotalGroup1,SubtotalGroup2,SubtotalGroup3,SubtotalGroup4,DOSStartDays,DOSEndDays,AgingBucketsToDisplay,hfReferalToId,hfReferalFromId,hfRefProvider,AssignedUserId,hfPCP,txtSystolicFrom,txtSystolicTo,txtTemperatureFrom,txtTemperatureTo,txtHeightFrom,txtHeightTo,txtSPO2From,txtSPO2To,txtDiastolicFrom,txtDiastolicTo,txtRespirationResultFrom,txtRespirationResultTo,txtBMIFrom,txtBMITo,txtPulseRateFrom,txtPulseRateTo,txtWeight,txtWeightTo,txtBSAFrom,txtBSATo,txtLoincCode,RadOrderNo,txtCPTCodeProcedure,RadiologyOrderNo,ProcedureOrderNo,ConsultationOrderNo,IncludeUnLinkCopay";
        var ControlsDiv = "chkIsAmendmentForBilling,txtCPTCode,txtCPTCodeRadiology,txtCPTCodeConsultation,txtCPTCodeMulti,txtDrugCPTCodeMulti,txtMultiSelectICD,txtICD,ddlPlanCategory,ApplyTo,SystemCategory,LedgerAccount,LedgerType,CPTCodeFrom,CPTCodeTo,SubtotalGroup1,SubtotalGroup2,SubtotalGroup3,SubtotalGroup4,DOSStartDays,DOSEndDays,AgingBucketsToDisplay,txtReferalTo,txtReferalForm,txtRefProvider,AssignedUserId,txtPCP,txtSystolicFrom,txtSystolicTo,txtTemperatureFrom,txtTemperatureTo,txtHeightFrom,txtHeightTo,txtSPO2From,txtSPO2To,txtDiastolicFrom,txtDiastolicTo,txtRespirationResultFrom,txtRespirationResultTo,txtBMIFrom,txtBMITo,txtPulseRateFrom,txtPulseRateTo,txtWeight,txtWeightTo,txtBSAFrom,txtBSATo,txtLoincCode,RadOrderNo,txtCPTCodeProcedure,RadiologyOrderNo,ProcedureOrderNo,ConsultationOrderNo,IncludeUnLinkCopay";

        var kvpairs = [];
        var isMultiselected = true;
        var isValidDate = true;
        self.find('[type=hidden],[type=text],[type=number], textarea').each(function () {
            var CurrentID = this.id;

            if (CurrentID != "" && $.data($(this).get(0), 'datepicker') != null && $(this).val().length < 7 && $(this).val() != '') {
                if (($(this).hasClass("datepickerMonthViewEnd") || $(this).hasClass("datepickerMonthViewStart")) && $(this).val().length < 7) {
                    isValidDate = false;
                } else if ($(this).val().length < 10) {
                    isValidDate = false;
                }
            } else if (CurrentID != "") {
                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                date_format = date_format.replace('yyyy', 'yy');
                var SelectedId = "";
                for (var i = 0; i < ControlsDiv.split(',').length; i++) {
                    if (ControlsDiv.split(',')[i] === CurrentID) {
                        SelectedId = ContolsKeys.split(',')[i]
                        break;
                    }
                }
                if (SelectedId != "") {
                    if ($.data($(this).get(0), 'datepicker') != null && $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val() != '') {
                        //For datepicker up to month view, We need the last day of to date field so that we can good date
                        var selectedDate = $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).datepicker('getDate');
                        if ($(this).hasClass("datepickerMonthViewEnd")) {
                            var date = selectedDate, y = selectedDate.getFullYear(), m = selectedDate.getMonth();
                            selectedDate = new Date(y, m + 1, 0);
                        }
                        kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($.datepicker.formatDate(date_format, selectedDate)));
                    } else {
                        if (ContolsValues.split(',')[i].indexOf('hf') > -1) {
                            if (ContolsValues.split(',')[i] == 'hfReferalToId' || ContolsValues.split(',')[i] == 'hfReferalFromId' || ContolsValues.split(',')[i] == 'hfCPTCodeMulti' || ContolsValues.split(',')[i] == 'hfRefProvider' || ContolsValues.split(',')[i] == 'hfPCP' || ContolsValues.split(',')[i] == 'hfDrugCPTCodeMulti') {
                                kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).text()));
                                if (ContolsValues.split(',')[i] == 'hfRefProvider') {
                                    kvpairs.push(encodeURIComponent("txtRefProvider") + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #hfRefProviderName').text()));
                                } else if (ContolsValues.split(',')[i] == 'hfPCP') {
                                    kvpairs.push(encodeURIComponent("txtPCP") + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #hfPCPName').text()));
                                }
                            }
                            else {
                                if (ContolsValues.split(',')[i] == "hfCPTCodeRadiology") {
                                    kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val().split('-')[0].trim()));
                                } else {
                                    kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val()));
                                }
                            }
                        } else
                            kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + ContolsValues.split(',')[i]).val()));
                    }

                } else {
                    if ($.data($(this).get(0), 'datepicker') != null && this.value != '') {
                        //For datepicker up to month view, We need the last day of to date field so that we can good date
                        var selectedDate = $(this).datepicker('getDate');
                        if ($(this).hasClass("datepickerMonthViewEnd")) {
                            var date = selectedDate, y = selectedDate.getFullYear(), m = selectedDate.getMonth();
                            selectedDate = new Date(y, m + 1, 0);
                        }
                        kvpairs.push(encodeURIComponent(this.id) + "=" + encodeURIComponent($.datepicker.formatDate(date_format, selectedDate)));
                    } else {
                        if (CurrentID == "StartDate" && this.value == "") {
                            setTimeout(function () {
                                utility.DisplayMessages("Start Date is not selected", 2);
                            }, 500);
                            isValidDate = false;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
                        }
                        if (CurrentID == "EndDate" && this.value == "") {
                            setTimeout(function () {
                                utility.DisplayMessages("End Date is not selected", 2);
                            }, 500);
                            isValidDate = false;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
                        }
                        if (CurrentID == "ProbGivenDateFrom" && this.value == "") {
                            utility.DisplayMessages("From Date is not selected", 2);
                            isValidDate = false;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
                        }
                        if (CurrentID == "ProbGivenDateTo" && this.value == "") {
                            utility.DisplayMessages("To Date is not selected", 2);
                            isValidDate = false;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
                        }
                        kvpairs.push(encodeURIComponent(this.id) + "=" + encodeURIComponent(this.value));
                    }
                }
            }
        });
        self.find('[type=checkbox][id], [type=radio][id]').each(function () {
            var CurrentID = this.id;
            if (CurrentID != "" && CurrentID.indexOf("chkInclude") < 0) {
                var SelectedId = "";
                for (var i = 0; i < ControlsDiv.split(',').length; i++) {
                    if (ControlsDiv.split(',')[i] === CurrentID) {
                        SelectedId = ContolsKeys.split(',')[i]
                        break;
                    }
                }
                if (SelectedId != "") {
                    kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent((this.checked) ? true : false));
                } else {
                    kvpairs.push(encodeURIComponent(this.id) + "=" + encodeURIComponent((this.checked) ? true : false));
                }
            }
        });
        self.find('select').each(function (index, element) {

            var CurrentID = this.id;
            if (CurrentID != "") {
                var SelectedId = "";
                var SelectedNames = [];
                SelectedId = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).attr('customname');
                var SelectedClass = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).attr('customval');
                if (SelectedId != "" && SelectedId != undefined) {

                    var Selectedvalues;
                    //if ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters .' + SelectedClass + ' .multiselect-all').hasClass('active')) {
                    //    Selectedvalues = "all";
                    //}
                    //else {
                    //if (SelectedId == "ProviderId" || SelectedId == "AppointmentProviderId" || SelectedId == "ResourceProviderId" || SelectedId == "RenderingProviderId" || SelectedId == "FacilityId" || SelectedId == "PracticeId" || SelectedId == "RefProviderId" || SelectedId == "InsurancePlanId" || SelectedId == "ResourceId" || SelectedId == "SpecialtyId" || SelectedId == "PlanCategoryId" || SelectedId == "ProcedureCategoryId" || SelectedId == "PaymentTypeId" || SelectedId == "UserId" || SelectedId == "SecurityRoleId" || SelectedId == "EnteredBy") {
                    //    Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected').map(function (a, item) { return item.value; }).get().join();
                    // }
                    // else {
                    if (SelectedId == "IsActive" || SelectedId == "FilterChargesBy" || SelectedId == "ProblemStatus") {
                        Selectedvalues = ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "") ? "" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "1" ? true : false);
                        if (SelectedId == "IsActive" || SelectedId == "ProblemStatus") {
                            SelectedNames = ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "") ? "All" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "1" ? "Active" : "In Active");
                        } else {
                            SelectedNames = ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "") ? "All" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "1" ? "Service Date" : "Claim Date");
                        }
                    }
                    else {
                        if (CurrentID == 'txtInsurancePlan') {
                            Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option[value!=self]:selected').map(function (a, item) { return item.value; }).get().join();
                        } else {
                            Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val();//$('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + SelectedId + ' option:selected').map(function (a, item) { return item.value; }).get().join();
                        }

                        if ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).find('option:selected').length == $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).find('option').length) {
                            SelectedNames = 'All';
                        } else {
                            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).find('option:selected').each(function () {
                                //if (SelectedId == "ProviderId" || SelectedId == "RenderingProviderId" || SelectedId == "ResourceProviderId" || SelectedId == "AppointmentProviderId") {
                                //SelectedNames.push("'" + $(this).text() + "'");
                                //} else {
                                if ($(this).text() != "-select-") {
                                    SelectedNames.push($(this).text());
                                }
                                //}
                            })
                        }
                        if (Selectedvalues == undefined) {
                            Selectedvalues = "";
                        }
                        if (SelectedNames == "'- Select -'") {
                            SelectedNames = "";
                        }
                        Selectedvalues = $.isArray(Selectedvalues) ? Selectedvalues.join() : Selectedvalues;
                        SelectedNames = $.isArray(SelectedNames) ? SelectedNames.join() : SelectedNames;
                    }
                    //}

                    if (Selectedvalues == "" && (SelectedId == "PracticeId" || SelectedId == "FacilityId") && $(this).hasClass("multiselect")) {
                        setTimeout(function () { utility.DisplayMessages(SelectedId.slice(0, -2) + " is not selected", 2); }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId.slice(0, -2) + " is not selected");
                    }
                    if (Selectedvalues == "" && (SelectedId == "ProviderId") && $(this).hasClass("multiselect") && (ReportsSSRSDashboard.ReportName != "Immunization")) {
                        setTimeout(function () { utility.DisplayMessages(SelectedId.slice(0, -2) + " is not selected", 2); }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId.slice(0, -2) + " is not selected");
                    } else if (ReportsSSRSDashboard.ReportName == "Immunization") {
                        var Error = "Provider or Account Number is not selected";
                        var AlertValue = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #ImmunizationAlert').val();
                        var Accountnumber = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #AccountNumber').val();
                        if (Selectedvalues == "" && (SelectedId == "ProviderId") && $(this).hasClass("multiselect") && AlertValue == "") {
                            setTimeout(function () {
                                utility.DisplayMessages("Provider is not selected", 2);
                            }, 500);
                            isMultiselected = false;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html("Provider or Account Number is not selected");
                        } else if ((SelectedId == "ProviderId") && (AlertValue != "" && AlertValue != null && AlertValue != undefined) && Accountnumber == "") {
                            setTimeout(function () {
                                utility.DisplayMessages("Account Number is not selected", 2);
                            }, 500);
                            isMultiselected = false;
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html("Provider or Account Number is not selected");
                        }
                    }
                    if (Selectedvalues == "" && (SelectedId == "AgingBucketsToDisplay") && $(this).hasClass("multiselect")) {
                        setTimeout(function () { utility.DisplayMessages(SelectedId + " is not selected", 2); }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId + " is not selected");
                    } else {
                        kvpairs.push(encodeURIComponent(SelectedId) + "=" + encodeURIComponent(Selectedvalues));
                        kvpairs.push(encodeURIComponent(SelectedClass + "Name") + "=" + encodeURIComponent(SelectedNames));
                    }
                }
            }
        });
        if (isMultiselected && isValidDate) {
            var queryStrings = kvpairs.join("&");
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #btn-print").attr('disabled', false);
            return queryStrings;
        } else {
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #btn-print").attr('disabled', true);
            return false;
        }

    },

    CreateReportPrint: function () {
        //get the ReportViewer Id


        ReportsSSRSDashboard.GetReportBody().done(function (response) {
            if (response.status != false) {
                // Generating a copy of the report in a new window
                var docType = '<!doctype html>';
                var docCnt = "<h4 class='text-center' style='word-wrap: break-word;white-space: pre-wrap;font-style: normal;font-family: Verdana;font-size: 12pt;font-weight: 400;color: #000;direction: ltr;text-align: left;'>Report Heading</h4>" + response.ReportsDetailsHTML;
                var docHead = '<head> <link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" /></head>';
                var winAttr = "location=yes, statusbar=no, directories=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=720, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                var newWin = window.open("", "_blank", winAttr);
                writeDoc = newWin.document;
                writeDoc.open();
                writeDoc.write(docType + '<html>' + docHead + '<body onload="">' + docCnt + '</body></html>');
                writeDoc.close();
                newWin.focus();
                // uncomment to autoclose the preview window when printing is confirmed or canceled.
                //  
                // setTimeout(function () { newWin.close(); }, 100);
            }
        });


    },

    GetReportBody: function (FormName) {
        if (FormName == null || FormName == "" || FormName == "undefined" || typeof FormName == "undefined") {
            FormName = ReportsSSRSDashboard.ReportName;
        }
        var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
        if (QueryString) {
            if (FormName.toLowerCase().trim() == "Insurance Plan AR".toLowerCase().trim()) {
                QueryString += "&Self=" + $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtInsurancePlan option[value=self]').is(":checked");
            }

            var data = "ReportName=" + FormName.trim() + "&" + QueryString;
            // serach parameter , class name, command name of class 
            return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "GET_REPORTS_DETAILSHTML");
        }
    },


    GetUsersByRoles: function () {
        if (ReportsSSRSDashboard.ReportName.toLowerCase().trim() == "User Activity Report".toLowerCase().trim()) {
            var SelectedUsers = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #User option:selected').map(function (a, item) { return item.value; }).get().join();
            var SelectedUserRoles = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #SecurityRole option:selected').map(function (a, item) { return item.value; }).get().join();
            if (SelectedUserRoles != '' && SelectedUsers != '') {
                return false;
            } else if (SelectedUserRoles != '' && SelectedUsers == '') {
                ReportsSSRSDashboard.SearchUserAndUserRoles(SelectedUserRoles, SelectedUsers, null).done(function (response) {
                    if (response.status) {
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #User').html(response.Users);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #User').multiselect('rebuild');
                    }
                });
            } else if (SelectedUserRoles == '' && SelectedUsers != '') {
                ReportsSSRSDashboard.SearchUserAndUserRoles(SelectedUserRoles, SelectedUsers, null).done(function (response) {
                    if (response.status) {
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #SecurityRole').html(response.UserRoles);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #SecurityRole').multiselect('rebuild');
                    }
                });
            } else {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #User').html($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .User').html());
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #SecurityRole').html($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .SecurityRole').html());
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #SecurityRole').multiselect('rebuild');
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #User').multiselect('rebuild');
            }
        }
    },

    GetVisitTypeByPatientType: function (obj) {
        if (VisitType.length == 0) {
            $.each($("#ReportParamaters #ddlVisitType option"), function (i, item) {
                VisitType.push(item);
            });
        }
        if ($(obj).val() == "1") {
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType option").remove();
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType").append(VisitType);
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType option[refvalue=2]").remove();
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType").multiselect("rebuild");
        } else if ($(obj).val() == "2") {
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType option").remove();
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType").append(VisitType);
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType option[refvalue=1]").remove();
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType").multiselect("rebuild");
        } else {
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType option").remove();
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType").append(VisitType);
            $("#" + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #ddlVisitType").multiselect("rebuild");
        }
    },

    SearchUserAndUserRoles: function (UserRoles, Users, IsActive) {
        if (typeof IsActive == "undefiend" || IsActive == null || IsActive == '') {
            IsActive = "";
        }
        var data = "Users=" + Users + "&UserRoles=" + UserRoles + "&IsActive" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "FILTER_USERROLES_USERS");
    },
    iframeClick: function () {

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe');
    },

    ActiveInactiveFacility: function () {
        inactiveFacility.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').find('option'));
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #chkIncludeActiveFacilities').is(":checked") == true) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').find('option').remove();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').append(inactiveFacility[0]);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').multiselect('rebuild');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').find('select').multiselect('refresh');
            inactiveFacility = [];
        } else {
            //inactiveFacility.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').find('option'));
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').find('option[IsActive=False]').remove();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').multiselect('rebuild');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtFacility').find('select').multiselect('refresh');
        }
    },

    ActiveInactiveProvider: function () {
        if (inactiveProvider.length == 0) {
            inactiveProvider.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('option'));
        }
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #chkIncludeActiveProvider').is(":checked") == true) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('option').remove();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').append(inactiveProvider[0]);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').multiselect('rebuild');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('select').multiselect('refresh');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').multiselect('selectAll', true);
        } else {
            //inactiveProvider.push($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('option'));
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('option').remove();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').append(inactiveProvider[0]);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('option[IsActive=False]').remove();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').multiselect('rebuild');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').find('select').multiselect('refresh');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #txtProvider').multiselect('selectAll', true);
        }
    },

    LoadRefProvider: function (obj) {

        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtReferalTo').kendoMultiSelect({
            dataValueField: "ReferringProviderId",
            dataTextField: "FirstName",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var objData = {};
                        objData.RefProName = $('#ReportParamaters #txtReferalTo').data("kendoMultiSelect")._prev;
                        var data = JSON.stringify(objData);
                        MDVisionService.APIService(data, "Admin", "SearchRefProviderKendoMultiSelect").done(function (response) {
                            var RefProvider = { data: [] };
                            var resposeData = jQuery.parseJSON(response);
                            var RefProv = jQuery.parseJSON(resposeData.ResponseModel);
                            if (RefProv.ReferringProviderCount != 0) {
                                RefProvider.data = jQuery.parseJSON(RefProv.ReferringProviderLoad_JSON);
                            }
                            e.success(RefProvider);
                        })
                    }
                }
            }),
            minLength: 3,
            separator: ", ",
            select: function (e) {
                var dataItem = this.dataSource.view()[e.item.index()];
                RefProviderId.push(dataItem.ReferringProviderId);
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalToId').text("");
                for (i = 0; i < RefProviderId.length; i++) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalToId').append(RefProviderId[i] + ',');
                }
            },
            change: function (e) {
                RefProviderId = e.sender._old;
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalToId').text("");
                for (i = 0; i < RefProviderId.length; i++) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalToId').append(RefProviderId[i] + ',');
                }
            },
        });
        // });
    },


    LoadRefProviderFrom: function (obj) {
        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        var crudServiceBaseUrl = "api/Admin/SearchRefProviderKendoMultiSelect";
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtReferalForm').kendoMultiSelect({
            dataValueField: "ReferringProviderId",
            dataTextField: "FirstName",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var objData = {};
                        objData.RefProName = $('#ReportParamaters #txtReferalForm').data("kendoMultiSelect")._prev;
                        var data = JSON.stringify(objData);
                        MDVisionService.APIService(data, "Admin", "SearchRefProviderKendoMultiSelect").done(function (response) {
                            var RefProvider = { data: [] };
                            var resposeData = jQuery.parseJSON(response);
                            var RefProv = jQuery.parseJSON(resposeData.ResponseModel);
                            if (RefProv.ReferringProviderCount != 0) {
                                RefProvider.data = jQuery.parseJSON(RefProv.ReferringProviderLoad_JSON);
                            }
                            e.success(RefProvider);
                        })
                    }
                }
            }),
            minLength: 3,
            separator: ", ",
            select: ReportsSSRSDashboard.onSelectFrom,
            change: ReportsSSRSDashboard.onChangeFrom,
        });
        //});
    },
    onSelectFrom: function (e) {
        var dataItem = this.dataSource.view()[e.item.index()];
        RefProviderIdFrom.push(dataItem.Value);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalFromId').text("");
        for (i = 0; i < RefProviderIdFrom.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalFromId').append(RefProviderIdFrom[i] + ',');
        }
    },

    onChangeFrom: function (e) {
        RefProviderIdFrom = e.sender._old;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalFromId').text("");
        for (i = 0; i < RefProviderIdFrom.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalFromId').append(RefProviderIdFrom[i] + ',');
        }
    },

    LoadRefProviderSimple: function (obj) {

        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        var crudServiceBaseUrl = "api/Admin/SearchRefProviderKendoMultiSelect";

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtRefProvider').kendoMultiSelect({
            dataValueField: "ReferringProviderId",
            dataTextField: "FirstName",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var objData = {};
                        objData.RefProName = $('#ReportParamaters #txtRefProvider').data("kendoMultiSelect")._prev;
                        var data = JSON.stringify(objData);
                        MDVisionService.APIService(data, "Admin", "SearchRefProviderKendoMultiSelect").done(function (response) {
                            var RefProvider = { data: [] };
                            var resposeData = jQuery.parseJSON(response);
                            var RefProv = jQuery.parseJSON(resposeData.ResponseModel);
                            if (RefProv.status) {
                                if (RefProv.ReferringProviderCount != 0) {
                                    RefProvider.data = jQuery.parseJSON(RefProv.ReferringProviderLoad_JSON);
                                }
                            }
                            e.success(RefProvider);
                        })
                    }
                }
            }),
            minLength: 3,
            separator: ", ",
            select: ReportsSSRSDashboard.onSelectSimple,
            change: ReportsSSRSDashboard.onChangeSimple,
        });
        // });
    },
    onSelectSimple: function (e) {
        var dataItem = this.dataSource.view()[e.item.index()];
        //RefProviderSimple.push(dataItem.Value);
        //RefProviderName.push(dataItem.FirstName);
        RefProviderSimple.push({ 'RefProvId': dataItem.ReferringProviderId, 'RefProvName': dataItem.FirstName });
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProvider').text("");
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProviderName').text("");
        for (i = 0; i < RefProviderSimple.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProvider').append(RefProviderSimple[i].RefProvId + ',');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProviderName').append(RefProviderSimple[i].RefProvName + ',');
        }
    },

    onChangeSimple: function (e) {
        //RefProviderSimple = e.sender._old;
        RefProviderName = [];
        for (i = 0; i < e.sender._old.length; i++) {
            if (RefProviderSimple[i].RefProvId = e.sender._old[i]) {
                RefProviderName.push(RefProviderSimple[i]);
            }
        }
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProvider').text("");
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProviderName').text("");
        for (i = 0; i < RefProviderName.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProvider').append(RefProviderName[i].RefProvId + ',');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfRefProviderName').append(RefProviderName[i].RefProvName + ',');
        }
        RefProviderSimple = [];
        RefProviderSimple = RefProviderName;
    },

    LoadPCP: function (obj) {

        //CacheManager.BindCodes('GetPCPs', false).done(function (result) {

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtPCP').kendoMultiSelect({
            dataValueField: "ReferringProviderId",
            dataTextField: "FirstName",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var objData = {};
                        objData.RefProName = $('#ReportParamaters #txtPCP').data("kendoMultiSelect")._prev;
                        var data = JSON.stringify(objData);
                        MDVisionService.APIService(data, "Admin", "SearchRefProviderKendoMultiSelect").done(function (response) {
                            var PCP = { data: [] };
                            var resposeData = jQuery.parseJSON(response);
                            var RefProv = jQuery.parseJSON(resposeData.ResponseModel);
                            if (RefProv.status) {
                                if (RefProv.ReferringProviderCount != 0) {
                                    PCP.data = jQuery.parseJSON(RefProv.ReferringProviderLoad_JSON);
                                }
                            }
                            e.success(PCP);
                        })
                    }
                }
            }),
            minLength: 3,
            separator: ", ",
            select: ReportsSSRSDashboard.onSelect,
            change: ReportsSSRSDashboard.onChange,
        });
        // });
    },
    onSelect: function (e) {
        var dataItem = this.dataSource.view()[e.item.index()];
        //PCP.push(dataItem.Value);
        //PCPName.push(dataItem.FirstName);
        PCP.push({ 'RefProvId': dataItem.ReferringProviderId, 'RefProvName': dataItem.FirstName });
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCP').text("");
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCPName').text("");
        for (i = 0; i < PCP.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCP').append(PCP[i].RefProvId + ',');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCPName').append(PCP[i].RefProvName + ',');
        }
    },

    onChange: function (e) {
        //PCP = e.sender._old;
        PCPName = [];
        for (i = 0; i < e.sender._old.length; i++) {
            if (PCP[i].RefProvId = e.sender._old[i]) {
                PCPName.push(PCP[i]);
            }
        }
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCP').text("");
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCPName').text("");
        for (i = 0; i < PCPName.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCP').append(PCPName[i].RefProvId + ',');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfPCPName').append(PCPName[i].RefProvName + ',');
        }
        PCP = [];
        PCP = PCPName;
    },

    LoadCPTMultiSelect: function (obj) {

        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtCPTCodeMulti').kendoMultiSelect({
            autoClose: false,
            dataValueField: "id",
            dataTextField: "value",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var cptCode = $('#ReportParamaters #txtCPTCodeMulti').data("kendoMultiSelect")._prev;
                        var data = "text=" + cptCode + "&entityID=" + globalAppdata["SeletedEntityId"] + "&iscode=CPT" + "&isMDVision=true";
                        MDVisionService.defaultService(data, "COMMON_IMO_CODE", "GET_IMO_CPTCODE").done(function (result) {
                            var CPTData = { data: [] };
                            $.each(result, function (j, item) {
                                if (item.RefValue) {
                                    var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                    var _ConcatinatedString = item.Name;

                                    // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                    if (_ConcatinatedString.indexOf("!") >= 0) {

                                        LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                        CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                        ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                        SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                        _LexiCode = LexiCode;

                                        _CPT = CPT.split("+")[0];
                                        _CPTDescription = CPT.split("+")[1];
                                        _ICD = ICD.split("+")[0];
                                        _ICDDescription = ICD.split("+")[1];
                                        _SNOMED = SNOMED.split("+")[0];
                                        _SNOMEDDescription = SNOMED.split("+")[1];
                                    }
                                    else {
                                        CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                        _CPT = CPT.split("-")[0].trim();
                                        _CPTDescription = CPT.split("-")[1].trim();
                                    }

                                    var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                    if (_CPT == "") {
                                        CPTData.data.push({ id: item.Value, value: _CPTDescription, RefValue: item.RefValue });
                                    }
                                    else {
                                        CPTData.data.push({ id: item.Value, value: _CPT, RefValue: item.RefValue });
                                    }
                                }

                            });

                            e.success(CPTData);
                        })
                    }
                }
            }),
            separator: ", ",
            select: function (e) {
                var dataItem = this.dataSource.view()[e.item.index()];
                RefProviderSimple.push(dataItem.id);
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').text("");
                var CPTMultiText = "";
                for (i = 0; i < RefProviderSimple.length; i++) {
                    CPTMultiText += RefProviderSimple[i] + ',';
                }
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').append(CPTMultiText);
            },
            change: function (e) {
                RefProviderSimple = e.sender._old;
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').text("");
                for (i = 0; i < RefProviderSimple.length; i++) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').append(RefProviderSimple[i] + ',');
                }
            },
            open: function (e) {
                if (this.input.val().length == 0) {
                    e.preventDefault();
                }
            },
        });
        // });
    },
    onSelectCPT: function (e) {
        var dataItem = this.dataSource.view()[e.item.index()];
        RefProviderSimple.push(dataItem.id);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').text("");
        var CPTMultiText = "";
        for (i = 0; i < RefProviderSimple.length; i++) {
            CPTMultiText += RefProviderSimple[i] + ',';
        }
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').append(CPTMultiText);
    },

    onChangeCPT: function (e) {
        RefProviderSimple = e.sender._old;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').text("");
        for (i = 0; i < RefProviderSimple.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfCPTCodeMulti').append(RefProviderSimple[i] + ',');
        }
    },
    LoadDrugCPTMultiSelect: function (obj) {

        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtDrugCPTCodeMulti').kendoMultiSelect({
            autoClose: false,
            dataValueField: "value",
            dataTextField: "value",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var cptCode = $('#ReportParamaters #txtDrugCPTCodeMulti').data("kendoMultiSelect")._prev;
                        var data = "IsActive=true";
                        MDVisionService.lookups("GetDrugCodeCost", true, data).done(function (result) {
                            var CPTData = { data: [] };
                            $.each(JSON.parse(result.GetDrugCodeCost), function (j, item) {
                                CPTData.data.push({ id: item.Value, value: item.Name });
                            });

                            e.success(CPTData);
                        })
                    }
                }
            }),
            separator: ", ",
            select: function (e) {
                var dataItem = this.dataSource.view()[e.item.index()];
                RefProviderSimple.push(dataItem.id);
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').text("");
                var CPTMultiText = "";
                for (i = 0; i < RefProviderSimple.length; i++) {
                    CPTMultiText += RefProviderSimple[i] + ',';
                }
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').append(CPTMultiText);
            },
            change: function (e) {
                RefProviderSimple = e.sender._old;
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').text("");
                for (i = 0; i < RefProviderSimple.length; i++) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').append(RefProviderSimple[i] + ',');
                }
            },
            open: function (e) {
                if (this.input.val().length == 0) {
                    e.preventDefault();
                }
            },
        });
        // });
    },
    onSelectCPT: function (e) {
        var dataItem = this.dataSource.view()[e.item.index()];
        RefProviderSimple.push(dataItem.id);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').text("");
        for (i = 0; i < RefProviderSimple.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').append(RefProviderSimple[i] + ',');
        }
    },

    onChangeCPT: function (e) {
        RefProviderSimple = e.sender._old;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').text("");
        for (i = 0; i < RefProviderSimple.length; i++) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfDrugCPTCodeMulti').append(RefProviderSimple[i] + ',');
        }
    },


    LoadICDMultiSelect: function (obj) {

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtMultiSelectICD').kendoMultiSelect({
            dataValueField: "id",
            dataTextField: "value",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var icdCode = $('#ReportParamaters #txtMultiSelectICD').data("kendoMultiSelect")._prev;
                        var data = "text=" + icdCode + "&entityID=" + globalAppdata["SeletedEntityId"] + "&iscode=ICD" + "&isMDVision=true";
                        MDVisionService.defaultService(data, "COMMON_IMO_CODE", "GET_IMO_ICDCODE").done(function (result) {
                            var ICDData = { data: [] };
                            $.each(result, function (j, item) {
                                if (item.RefValue) {

                                    var LexiCode = "", ICD9 = "", ICD10 = "", SNOMED = "", _ICD9 = "", _ICD9Description = "", _ICD10 = "",
                                       _ICD10Description = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                    var _ConcatinatedString = item.Name;

                                    LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                    ICD9 = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                    ICD10 = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                    SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                    _LexiCode = LexiCode;

                                    _ICD9 = ICD9.split("+")[0];
                                    _ICD9Description = ICD9.split("+")[1];
                                    _ICD10 = ICD10.split("+")[0];
                                    _ICD10Description = ICD10.split("+")[1];
                                    _SNOMED = SNOMED.split("+")[0];
                                    _SNOMEDDescription = SNOMED.split("+")[1];

                                    var duMulti = _LexiCode + "*" + ICD9 + "$" + ICD10 + "~" + SNOMED;

                                    var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                    _ICD10 = _ICD10 != "" ? _ICD10 : "NoICD10";
                                    _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";

                                    if (isIMO == "imo") {
                                        if (utility.IsShowICD10) {
                                            ICDData.data.push({ id: item.Value, value: _ICD10, RefValue: item.RefValue, RefName: duMulti });
                                        }

                                        else {
                                            ICDData.data.push({ id: item.Value, value: _ICD10, RefValue: item.RefValue, RefName: duMulti });
                                        }
                                    }
                                    else {

                                        ICDData.data.push({ id: item.Value, value: _ICD10, RefValue: item.RefValue, RefName: duMulti });
                                    }

                                }

                            });
                            e.success(ICDData);
                        })
                    }
                }
            }),
            separator: ", ",
            select: ReportsSSRSDashboard.onSelectICD,
            change: ReportsSSRSDashboard.onChangeICD,
            open: function (e) {
                if (this.input.val().length == 0) {
                    e.preventDefault();
                }
            },
        });
    },
    onSelectICD: function (e) {
        var dataItem = this.dataSource.view()[e.item.index()];
        RefProviderSimple.push(dataItem.id);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCodeMultiSelect').val("");
        var ICDMultiText = "";
        for (i = 0; i < RefProviderSimple.length; i++) {
            ICDMultiText += RefProviderSimple[i] + ',';
        }
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCodeMultiSelect').val(ICDMultiText);

    },

    onChangeICD: function (e) {
        RefProviderSimple = e.sender._old;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCodeMultiSelect').val("");
        var ICDMultiText = "";
        for (i = 0; i < RefProviderSimple.length; i++) {
            ICDMultiText += RefProviderSimple[i] + ',';
        }
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfICDCodeMultiSelect').val(ICDMultiText);
    },
    //Srart By khaleel Ur Rehman
    // 21 May 2016
    //Functions to extraction MU report data.
    generateMuStageReport: function () {
        if (ReportsSSRSDashboard.validateFieldsBeforeRunReport()) {
            $('#MuStageReportView').show();
            $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').show();
            ReportsSSRSDashboard.generateMuStageReport_DBCall().done(function (response) {
                response = JSON.parse(response);
                ReportsSSRSDashboard.totalResultResponse = response;
                if (response.status != false) {
                    var data = JSON.parse(response.MU_JSON);
                    ReportsSSRSDashboard.MUdata_Patient = JSON.parse(response.MU_JSON_PatientWise);

                    if (data.length > 0) {
                        var passedCount = 0;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #tblMUData > tbody').html("");
                        $.each(data, function (i, item) {
                            var $row = $('<tr/>');
                            $row.attr("id", "gvMU2_row" + item.ID);
                            $row.attr("onclick", "utility.SelectGridRow($('#gvMU2_row" + item.ID + "'))");
                            var TargetRequired = Number(item.RequiredTarget);
                            var Targetmet = Number(item.PerfromanceRate1);
                            var ProgressClass = "";
                            var labelClass = "";
                            var circleClass = "";
                            if (TargetRequired <= Targetmet) {
                                success = "progress-bar-success";
                            } else if (TargetRequired > Targetmet) {
                                ProgressClass = "progress-bar-danger";
                            }
                            if (TargetRequired <= Targetmet) {
                                circleClass = "success";
                                labelClass = "green";
                            } else if (TargetRequired > Targetmet) {
                                circleClass = "danger";
                                labelClass = "red";
                            }
                            var PatientCount = 0;
                            if (ReportsSSRSDashboard.MUdata_Patient != null && ReportsSSRSDashboard.MUdata_Patient.length > 0) {
                                var ListPatient = [];
                                $.each(ReportsSSRSDashboard.MUdata_Patient, function (i, dataItem) {
                                    if (dataItem.ID == item.ID && dataItem.Numerator == "0") {
                                        ListPatient.push(dataItem);
                                    }
                                });
                                PatientCount = ListPatient.length;

                            }
                            var disableButtonClass = "";
                            if (PatientCount == 0) {
                                disableButtonClass = " disableAll";
                            }

                            $row.append('<td style=display:none>' + item.ID + '</td>' + '<td style="min-width:345px; max-width:345px;width:345px;">' + item.Measure + '</td>'
                               + '<td style="min-width:80px; max-width:80px;width:80px;">' + item.Numerator + '/' + item.Denominator + '</td>'
                                + '<td style="min-width:46px; max-width:46px;width:46px;">' + Math.round(item.ReportingRate2) + '</td>' //RequiredTarget
                           + '<td style="min-width:83px; max-width:83px;width:83px;">' + item.RequiredTarget + ' %</td>'
                          + '<td style="min-width:128px; max-width:128px;width:128px;">' + '<div class="progress-bar-circle ' + circleClass + '" data-percent="' + Math.round(item.PerfromanceRate1) + '" data-line="2" data-size="50" ></div><label id="performanceRateCount" style="display:none" class="' + labelClass + '">' + Math.round(item.PerfromanceRate1) + ' %</label></td>'
                          + '<td>' + '<button type="button" onclick="ReportsSSRSDashboard.OpenMUStage1(' + item.ID + ')" class="btn btn-sm ' + disableButtonClass + '">PATIENTS (' + PatientCount + ')</button>' + '<label id="patCount" style="display:none">' + PatientCount + '</label></td>');


                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #tblMUData > tbody').last().append($row);

                            if (item.IsObjectCompleted == "True") {
                                passedCount++;
                            }
                        });
                        var dataPercent = (passedCount / data.length) * 100;
                        $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('<h4> Passed ' + passedCount + ' of ' + data.length + ' Objectives</h4><div class="progress mb-none mt-xs mb-xs">' +
                                                            '<div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="' + passedCount + '" aria-valuemin="0" aria-valuemax="' + data.length + '" style="width: ' + dataPercent + '%;min-width:0px !important;"></div>' +
                                                        '</div>');
                        ReportsSSRSDashboard.progressCircle('#pnlReportsSSRSDashboard #MUStagePassedProgressBar ');
                        ReportsSSRSDashboard.progressCircle('#pnlReportsSSRSDashboard #tblMUData ');
                    }
                }
            });
        } else {
            $('#MuStageReportView').hide();
            $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
            utility.DisplayMessages('Please select From date and To date or select a quarter.', 3);
        }
    },

    generateMuStageReportNumerator: function () {
        if (ReportsSSRSDashboard.validateFieldsBeforeRunReport()) {
            $('#MuStageReportView').show();
            $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').show();
            ReportsSSRSDashboard.generateMuStageReport_DBCall().done(function (response) {
                response = JSON.parse(response);
                ReportsSSRSDashboard.totalResultResponse = response;
                if (response.status != false) {
                    var data = JSON.parse(response.MU_JSON);
                    ReportsSSRSDashboard.MUdata_Patient = JSON.parse(response.MU_JSON_PatientWise);

                    if (data.length > 0) {
                        var passedCount = 0;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #tblMUData > tbody').html("");
                        $.each(data, function (i, item) {
                            var $row = $('<tr/>');
                            $row.attr("id", "gvMU2_row" + item.ID);
                            $row.attr("onclick", "utility.SelectGridRow($('#gvMU2_row" + item.ID + "'))");
                            var TargetRequired = Number(item.RequiredTarget);
                            var Targetmet = Number(item.PerfromanceRate1);
                            var ProgressClass = "";
                            var labelClass = "";
                            var circleClass = "";
                            if (TargetRequired <= Targetmet) {
                                success = "progress-bar-success";
                            } else if (TargetRequired > Targetmet) {
                                ProgressClass = "progress-bar-danger";
                            }
                            if (TargetRequired <= Targetmet) {
                                circleClass = "success";
                                labelClass = "green";
                            } else if (TargetRequired > Targetmet) {
                                circleClass = "danger";
                                labelClass = "red";
                            }
                            var PatientCount = 0;
                            if (ReportsSSRSDashboard.MUdata_Patient != null && ReportsSSRSDashboard.MUdata_Patient.length > 0) {
                                var ListPatient = [];
                                $.each(ReportsSSRSDashboard.MUdata_Patient, function (i, dataItem) {
                                    if (dataItem.ID == item.ID/* && dataItem.Numerator == "1"*/) {
                                        ListPatient.push(dataItem);
                                    }
                                });
                                PatientCount = ListPatient.length;

                            }
                            var disableButtonClass = "";
                            if (PatientCount == 0) {
                                disableButtonClass = " disableAll";
                            }

                            $row.append('<td style=display:none>' + item.ID + '</td>' + '<td style="min-width:345px; max-width:345px;width:345px;">' + item.Measure + '</td>'
                               + '<td style="min-width:80px; max-width:80px;width:80px;">' + item.Numerator + '/' + item.Denominator + '</td>'
                                + '<td style="min-width:46px; max-width:46px;width:46px;">' + Math.round(item.ReportingRate2) + '</td>' //RequiredTarget
                           + '<td style="min-width:83px; max-width:83px;width:83px;">' + item.RequiredTarget + ' %</td>'
                          + '<td style="min-width:128px; max-width:128px;width:128px;">' + '<div class="progress-bar-circle ' + circleClass + '" data-percent="' + Math.round(item.PerfromanceRate1) + '" data-line="2" data-size="50" ></div><label id="performanceRateCount" style="display:none" class="' + labelClass + '">' + Math.round(item.PerfromanceRate1) + ' %</label></td>'
                          + '<td>' + '<button type="button" onclick="ReportsSSRSDashboard.OpenMUStage1(' + item.ID + ')" class="btn btn-sm ' + disableButtonClass + '">PATIENTS (' + PatientCount + ')</button>' + '<label id="patCount" style="display:none">' + PatientCount + '</label></td>');


                            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #tblMUData > tbody').last().append($row);

                            if (item.IsObjectCompleted == "True") {
                                passedCount++;
                            }
                        });
                        var dataPercent = (passedCount / data.length) * 100;
                        $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('<h4> Passed ' + passedCount + ' of ' + data.length + ' Objectives</h4><div class="progress mb-none mt-xs mb-xs">' +
                                                            '<div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="' + passedCount + '" aria-valuemin="0" aria-valuemax="' + data.length + '" style="width: ' + dataPercent + '%;min-width:0px !important;"></div>' +
                                                        '</div>');
                        ReportsSSRSDashboard.progressCircle('#pnlReportsSSRSDashboard #MUStagePassedProgressBar ');
                        ReportsSSRSDashboard.progressCircle('#pnlReportsSSRSDashboard #tblMUData ');
                    }
                }
            });
        } else {
            $('#MuStageReportView').hide();
            $('#pnlReportsSSRSDashboard #MUStagePassedProgressBar').html('').hide();
            utility.DisplayMessages('Please select From date and To date or select a quarter.', 3);
        }
    },
    //Srart By khaleel Ur Rehman
    // 21 May 2016
    //Functions to extraction MU report data.
    OpenMUStage1: function (ID) {
        var params = [];
        params["ParentCtrl"] = "mstrTabReports";
        params["FromAdmin"] = 0;
        params["ID"] = ID;
        params["rptName"] = ReportsSSRSDashboard.ReportName;
        params["resultSet"] = ReportsSSRSDashboard.MUdata_Patient;
        LoadActionPan("MUStage1", params);
    },
    //Srart By khaleel Ur Rehman
    // 21 May 2016
    //Functions to extraction MU report data.
    generateMuStageReport_DBCall: function () {
        ReportsSSRSDashboard.calculateFromDateToDate();
        var objData = {};
        objData["Provider"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtProvider').val();

        objData["FromDate"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #QuarterlyReport').is(':checked') ? ReportsSSRSDashboard.FromDate : $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #FromDate').val();
        objData["ToDate"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #QuarterlyReport').is(':checked') ? ReportsSSRSDashboard.ToDate : $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ToDate').val();
        //objData["ToDate"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ToDate').val();
        objData["ReportName"] = ReportsSSRSDashboard.ReportName;
        objData["commandType"] = "SEARCH_MUStageReport1";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "MUReport");
    },
    //Srart By khaleel Ur Rehman
    // 21 May 2016
    //Functions to enable/ disable fields.
    checkQuarterly: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #FromDate').addClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #FromDate').val('');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ToDate').val('');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Quarter').removeClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Year').removeClass('disableAll');

        } else {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #FromDate').removeClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ddlQuarter').addClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ddlYear').addClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Quarter').addClass('disableAll');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Year').addClass('disableAll');
        }
    },
    //Srart By khaleel Ur Rehman
    // 21 May 2016
    //Functions to calculate FromDate and ToDate.
    calculateFromDateToDate: function () {
        var from = '';
        var to = '';
        var year = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ddlYear').val();
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ddlQuarter').val() == 1) {
            from = '01/01/' + year;
            to = '03/31/' + year;
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ddlQuarter').val() == 2) {
            from = '04/01/' + year;
            to = '06/30/' + year;
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ddlQuarter').val() == 3) {
            from = '07/01/' + year;
            to = '09/30/' + year;
        } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ddlQuarter').val() == 4) {
            from = '10/01/' + year;
            to = '12/31/' + year;
        }

        /*var dsplitFrom = from.split("/");
        var dsplitTo = to.split("/");
        ReportsSSRSDashboard.FromDate = new Date(dsplitFrom[0], dsplitFrom[1], dsplitFrom[2]);
        ReportsSSRSDashboard.ToDate = new Date(dsplitTo[0], dsplitTo[1], dsplitTo[2]);*/

        ReportsSSRSDashboard.FromDate = from;
        ReportsSSRSDashboard.ToDate = to;
    },
    //Srart By khaleel Ur Rehman
    // 21 May 2016
    //Functions to VAliadte FromDate and ToDate.
    validateFieldsBeforeRunReport: function () {
        var retVal = true;
        if (($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #FromDate').val() == ''
            || $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ToDate').val() == '')
            && $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #QuarterlyReport').is(':checked') == false) {
            retVal = false;
        }
        return retVal;
    },
    //End By khaleel Ur Rehman
    progressCircle: function (id) {
        var el = id + ' .progress-bar-circle'; //document.getElementsByClassName('progress-bar-circle'); // get canvas
        $(el).each(function (count) {
            var options = {
                percent: this.getAttribute('data-percent') || 0,
                size: this.getAttribute('data-size') || 100,
                lineWidth: this.getAttribute('data-line') || 100,
                rotate: this.getAttribute('data-rotate') || 0,
                primaryClr: "#0088cc",
                dangerClr: '#d2322d',
                successClr: '#47a447',
                defaultClr: '#555555',
                remainCircleClor: '#efefef'
            }
            $(this).html('<canvas id="' + count + 'MUReport"></canvas>');
            var canvasID = $(this).children('canvas').attr("id");
            var canvas = document.getElementById(canvasID);
            var span = document.createElement('span');
            span.textContent = options.percent + '%';
            if (typeof (G_vmlCanvasManager) !== 'undefined') {
                G_vmlCanvasManager.initElement(canvas);
            }

            var ctx = canvas.getContext('2d');
            canvas.width = canvas.height = options.size;

            this.appendChild(span);
            this.appendChild(canvas);

            //settings
            $(this).height(options.size);
            $(this).width(options.size);
            //settings for span
            $(this).children("span").css("line-height", options.size + "px");
            $(this).children("span").width(options.size);

            ctx.translate(options.size / 2, options.size / 2); // change center
            ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg
            //imd = ctx.getImageData(0, 0, 240, 240);
            var radius = (options.size - options.lineWidth) / 2;
            radius = radius < 0 ? 1 : radius;
            var drawCircle = function (color, lineWidth, percent) {
                percent = Math.min(Math.max(0, percent || 1), 1);
                ctx.beginPath();
                ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
                ctx.strokeStyle = color;
                ctx.lineCap = 'square'; // butt, round or square
                ctx.lineWidth = lineWidth
                ctx.stroke();
            };
            drawCircle(options.remainCircleClor, options.lineWidth, 100 / 100);
            //circle themes color
            if (options.percent === "0") {
                return;
            }
            else if ($(this).hasClass("success") == true) {
                drawCircle(options.successClr, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("danger") == true) {
                drawCircle(options.dangerClr, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("primary") == true) {
                drawCircle(options.primaryClr, options.lineWidth, options.percent / 100);
            }
            else {
                drawCircle(options.defaultClr, options.lineWidth, options.percent / 100);
            }

        });//each function
    },
    iframeHeight: function () {
        var iframe = $("#ReportViewIframe");
        var iframeHTML = $($('#ReportViewIframe').contents().find('html'));
        var _height = iframeHTML.height();
        if (iframeHTML.height() <= 650) {
            iframe.attr("height", _height);
        }
        else {
            iframeHTML.attr("height", 650);
        }
    },

    //***********************************************************************
    //****************** PQRS Registry Report Functions *********************
    //***********************************************************************
    ValidatePQRSReport: function () {

        $('#' + ReportsSSRSDashboard.params.PanelID + ' #frmSSRSReports').bootstrapValidator('destroy');
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #frmSSRSReports')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  ProviderId: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ReportFromDate: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ReportToDate: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  GPROProviderGroup: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }

              }
          }).on('error.form.bv', function (e) {
              // Prevent form submission

          })
       .on('success.form.bv', function (e) {
           if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport #PQRSIndividualReporting .form-group ').is(':visible')) {
               ReportsSSRSDashboard.generateIndividualReport();

           } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport #PQRSGPROSubmission .form-group ').is(':visible')) {
               ReportsSSRSDashboard.generateGPROReport();

           }
           else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport #PQRSSubmissionSummaryReport .form-group ').is(':visible')) {
               ReportsSSRSDashboard.generateSSReport();

           }
       });
    },
    showIndividualReporting: function () {
        ReportsSSRSDashboard.showPQRSselectReport(false);
        ReportsSSRSDashboard.ReportName = "Physician Quality Reporting System (PQRS) - Individual Reporting";
        FormName = ReportsSSRSDashboard.ReportName;

        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport').hide();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSIndividualReporting').show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Individualkdhtml').removeClass('hidden');
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSIndividualReporting .form-group > div').show();

    },

    showCQMIndividualReporting: function () {
        ReportsSSRSDashboard.showPQRSselectReport(false);
        ReportsSSRSDashboard.ReportName = "Clinical Quality Measures (CQM)";
        FormName = ReportsSSRSDashboard.ReportName;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport').show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualkdhtml').removeClass('hidden');
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualReport .form-group > div').show();

    },
    showVBPIndividualReporting: function () {
        ReportsSSRSDashboard.showPQRSselectReport(false);
        ReportsSSRSDashboard.ReportName = "Value Based Program (VBP)";
        FormName = ReportsSSRSDashboard.ReportName;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport').show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualkdhtml').removeClass('hidden');
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport .form-group > div').show();

    },
    showGPROSubmission: function () {
        ReportsSSRSDashboard.showPQRSselectReport(false);
        ReportsSSRSDashboard.ReportName = "Physician Quality Reporting System (PQRS) - GPRO Submission";
        FormName = ReportsSSRSDashboard.ReportName;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport').hide();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #kdhtml').addClass('hidden')
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSGPROSubmission').show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #GPROkdhtml').removeClass('hidden');
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSGPROSubmission .form-group > div').show();
    },
    showSubmissionSummaryReport: function () {
        ReportsSSRSDashboard.showPQRSselectReport(false);
        ReportsSSRSDashboard.ReportName = "Physician Quality Reporting System (PQRS) - Submission Summary Report";
        FormName = ReportsSSRSDashboard.ReportName;
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #heading').text(FormName);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport').hide();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSSubmissionSummaryReport').show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSSubmissionSummaryReport .form-group > div').show();
    },
    showPQRSselectReport: function (isShow) {
        if (isShow) {
            //  $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Summarykdhtml').addClass('hidden');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #GPROkdhtml').addClass('hidden');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Individualkdhtml').addClass('hidden');
            // $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #GPROChart').empty();
            //  $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #myChart').empty();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport').show();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSGPROSubmission').hide();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSSubmissionSummaryReport').hide();
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSIndividualReporting').hide();


        } else {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport').hide();

        }
    },
    ShowHideDiv: function (FormName) {
        if (FormName.toLowerCase().trim() == "Value Based Program (VBP)".toLowerCase().trim()) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport').show();
        } else {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualReport').hide();
        }

    },
    showCQMSselectReport: function (isShow) {
        if (isShow) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualkdhtml').addClass('hidden');

        } else {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #PQRSselectReport').hide();

        }
    },
    generateIndividualReport: function () {
        // $("#" + ReportsSSRSDashboard.params.PanelID + " form #PQRSIndividualReporting").bootstrapValidator('revalidateField', 'ProviderId');
        var hasError = false;
        //$('#' + ReportsSSRSDashboard.params.PanelID + ' form #PQRSIndividualReporting .form-group input,#' + ReportsSSRSDashboard.params.PanelID + ' form #PQRSIndividualReporting .form-group select').each(function () {
        //    if ($(this).val() == null || $(this).val() == "") {
        //        $(this).closest('div.col-sm-4').addClass('has-error');
        //        $(this).closest('div.col-sm-4').css('color', '#cc2724');
        //        $(this).closest('div.input-group').addClass('has-error');
        //        hasError = true;
        //    } else {
        //        $(this).closest('div.col-sm-4').find('label.control-label').removeClass('has-error');
        //        $(this).closest('div.col-sm-4').find('label.control-label').css('color', '');
        //        $(this).closest('div.input-group').removeClass('has-error');
        //    }
        //    console.log(this)
        //});
        if (!hasError) {
            var self = $('#' + ReportsSSRSDashboard.params.PanelID + ' form #PQRSIndividualReporting');
            var myJSON = self.getMyJSONByName();
            ReportsSSRSDashboard.generateIndividualReport_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Individualkdhtml').removeClass('hidden');
                    ReportsSSRSDashboard.generateHTMLIndiReport(response);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #myChart').empty();
                    ReportsSSRSDashboard.KPICharts(response);
                }
                else {
                    ReportsSSRSDashboard.generateHTMLIndiReport(response);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #myChart').empty();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    generateCQMIndividualReport: function () {
        var hasError = false;
        if (!hasError) {
            var self = $('#' + ReportsSSRSDashboard.params.PanelID + ' form #CQMIndividualReport');
            var myJSON = self.getMyJSONByName();
            var parseJSON = JSON.parse(myJSON);
            var ProviderId = parseJSON["ProviderId"];
            var ProviderName = parseJSON["ProviderId_text"];
            ReportsSSRSDashboard.generateCQMIndividualReport_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #CQMIndividualkdhtml').removeClass('hidden');
                    ReportsSSRSDashboard.generateHTMLCQMIndiReport(response, ProviderId, ProviderName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' div#CQMIndividualkdhtml #myCQMChart').empty();
                    ReportsSSRSDashboard.KPIChartsCQM(null, response);
                }
                else {
                    ReportsSSRSDashboard.generateHTMLCQMIndiReport(response, ProviderId, ProviderName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' div#CQMIndividualkdhtml #myCQMChart').empty();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    generateVBPIndividualReport: function () {
        var hasError = false;
        if (!hasError) {
            var self = $('#' + ReportsSSRSDashboard.params.PanelID + ' form #VBPIndividualReport');
            var myJSON = self.getMyJSONByName();
            var parseJSON = JSON.parse(myJSON);
            var ProviderId = parseJSON["ProviderId"];
            var ProviderName = parseJSON["ProviderId_text"];
            ReportsSSRSDashboard.generateVBPIndividualReport_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #VBPIndividualkdhtml').removeClass('hidden');
                    ReportsSSRSDashboard.generateHTMLVBPIndiReport(response, ProviderId, ProviderName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' div#VBPIndividualkdhtml #myVBPChart').empty();
                    ReportsSSRSDashboard.KPIChartsVBP(null, response);
                }
                else {
                    ReportsSSRSDashboard.generateHTMLVBPIndiReport(response, ProviderId, ProviderName);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' div#VBPIndividualkdhtml #myVBPChart').empty();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    generateSSReport: function () { },
    printSSReport: function () { },

    generateGPROQRDA1Report: function () { },
    generateGPROQRDA3Report: function () { },
    generateIndiviQRDA1Report: function () {
        var self = $('#' + ReportsSSRSDashboard.params.PanelID + ' form #PQRSIndividualReporting');
        var myJSON = self.getMyJSONByName();
        console.log(myJSON);
        if ($("#PQRSIndividualReporting #dgvIndividualReport tbody tr input:checked").length > 0) {


            ReportsSSRSDashboard.generateIndiviQRDA1Report_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var zip = new JSZip();
                    for (var i = 0; i < response.DownloadFile.length; i++) {
                        zip.file(response.DownloadFile[i].Key + ".xml", response.DownloadFile[i].Value, { base64: true });
                    }

                    zip.generateAsync({ type: "blob" }).then(function (content) {
                        saveAs(content, response.FileName);
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            utility.DisplayMessages("No Record Found to Export.", 3);
        }
    },
    generateIndiviQRDA3Report: function () { },

    //HTML BINDING
    generateHTMLIndiReport: function (response) {

        $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSIndividualReporting #dgvIndividualReport tbody').html('');
        if (response.pqrsReportCount > 0) {
            var pqrsReportList_JSON = JSON.parse(response.pqrsReportList_JSON);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSIndividualReporting #lblProviderId').text(response.ProviderName);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSIndividualReporting #lblProivderNPI').text(response.NPI);
            $.each(pqrsReportList_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.measureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.measureId);
                $row.attr("measureId", item.measureId);
                var reportingRateCSS = "";
                var performanceRateCSS = "";
                var pr = parseFloat(item.performanceRate);
                if (pr < 50) {
                    performanceRateCSS = 'style ="color:red"';
                }
                else {
                    performanceRateCSS = 'style ="color:LimeGreen"';
                }
                var rr = parseFloat(item.reportingRate);
                if (rr < 50) {
                    reportingRateCSS = 'style ="color:red"';
                }
                else {
                    reportingRateCSS = 'style ="color:LimeGreen"';
                }
                var nonComplaintFunction = "";
                if (item.nonCompliantPatients != null && item.nonCompliantPatients > 0 && item.NonCompliantVisitsList != null && item.NonCompliantVisitsList != '') {
                    nonComplaintFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.measureId + "\",\"" + item.NonCompliantVisitsList + "\");'";
                    nonComplaintFunction = '<td ' + nonComplaintFunction + '><a href="javascript:void(0);">' + item.nonCompliantPatients + '</a></td>'
                } else {
                    nonComplaintFunction = '<td>' + item.nonCompliantPatients + '</td>';
                }

                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.measureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.measureNumber + '</td>' +
                    '<td>' + item.measureName + '</td>' +
                    '<td>' + item.denuminator + '</td>' +
                   '<td>' + item.numerator + '</td>' +
                   '<td ' + reportingRateCSS + '>' + parseFloat(item.reportingRate).toFixed(2) + ' %' + '</td>' +
                   '<td ' + performanceRateCSS + '>' + parseFloat(item.performanceRate).toFixed(2) + ' %' + '</td>' +
                   '<td>' + item.exclusion + '</td>' +
                   nonComplaintFunction +
                   '<td>' + item.performanceMet + '</td>' +
                   '<td>' + item.performanceNotMet + '</td>');
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSIndividualReporting #dgvIndividualReport tbody').last().append($row);

            });
        }
    },

    generateHTMLCQMIndiReport: function (response, ProviderId, ProviderName) {

        $('#' + ReportsSSRSDashboard.params.PanelID + ' #CQMIndividualReport #dgvIndividualReport tbody').html('');
        if (response.AllMeasuresCount > 0) {
            var pqrsReportList_JSON = JSON.parse(response.AllMeasuresLoad_JSON);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #CQMIndividualReport #lblProviderId').text(' ' + ProviderName);//.text(response.ProviderName);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #CQMIndividualReport #lblProivderNPI').hide();//.text(response.NPI);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #CQMIndividualReport #lblProviderNPIHeading').hide();
            $.each(pqrsReportList_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.CQMID + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.CQMID);
                $row.attr("measureId", item.CQMID);
                var reportingRateCSS = "";
                var performanceRateCSS = "";
                var pr = parseFloat(item.PerfromanceRate1);
                if (pr < 50) {
                    performanceRateCSS = 'style ="color:red"';
                }
                else {
                    performanceRateCSS = 'style ="color:LimeGreen"';
                }
                var rr = parseFloat(item.ReportingRate1);
                if (rr < 50) {
                    reportingRateCSS = 'style ="color:red"';
                }
                else {
                    reportingRateCSS = 'style ="color:LimeGreen"';
                }
                var currentMeasureId = item.CQMID;

                var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                    return a.MeasureId == currentMeasureId;
                });

                var arrInitialPopulationPatients = [];
                var arrInitialPopulationPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.InitialPopulation == 1 && a.CQMID == item.CQMID) {
                        arrInitialPopulationPatients.push(a.PatientID);
                    }
                    return (a.InitialPopulation == 1 && a.CQMID == item.CQMID);
                });
                var InitialPopulationPatientIds = $.unique(arrInitialPopulationPatients).join(",");
                var InitialPopulationFunction = "";
                if (arrInitialPopulationPatients.length > 0) {
                    //InitialPopulationFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + JSON.stringify(arrNonCompliantPatients) + "\");'";
                    InitialPopulationFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + InitialPopulationPatientIds + "\",\"" + ProviderId + "\",\"" + "1" + "\,\"" + item.MeasureNumber + " - " + item.Measure + "\");'";
                    InitialPopulationFunction = '<td ' + InitialPopulationFunction + '><a href="javascript:void(0);">' + parseInt(InitialPopulationPatientIds.split(",").length) + '</a></td>'
                } else {
                    InitialPopulationFunction = '<td>' + parseInt(0) + '</td>';
                }

                var arrPerfMetPatients = [];
                var arrPerfMetPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.Numerator == 1) {
                        arrPerfMetPatients.push(a.PatientID);
                    }
                    return a.Numerator == 1;
                });
                var PerfMetPatientIds = $.unique(arrPerfMetPatients).join(",");
                var PerMetFunction = "";
                if (arrPerfMetPatients.length > 0) {
                    //PerMetFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + JSON.stringify(arrNonCompliantPatients) + "\");'";
                    PerMetFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PerfMetPatientIds + "\",\"" + ProviderId + "\",\"" + "1" + "\,\"" + item.MeasureNumber + " - " + item.Measure + "\");'";
                    PerMetFunction = '<td ' + PerMetFunction + '><a href="javascript:void(0);">' + parseInt(PerfMetPatientIds.split(",").length) + '</a></td>'
                } else {
                    PerMetFunction = '<td>' + parseInt(0) + '</td>';
                }

                var arrPerfNotMetPatients = [];
                var arrPerfNotMetPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.Optional1 == 1) {
                        arrPerfNotMetPatients.push(a.PatientID);
                    }
                    return a.Optional1 == 1;
                });

                var PerfNotMetPatientIds = $.unique(arrPerfNotMetPatients).join(",");

                var arrDenominatorExclusionPatients = [];
                var arrDenominatorExclusionPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.DenominatorExclusion == 1) {
                        arrDenominatorExclusionPatients.push(a.PatientID);
                    }
                    return a.DenominatorExclusion == 1;
                });

                var DenominatorExclusionPatientIds = $.unique(arrDenominatorExclusionPatients).join(",");

                var arrDenominatorExceptionPatients = [];
                var arrDenominatorExceptionPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.DenominatorException == 1) {
                        arrDenominatorExceptionPatients.push(a.PatientID);
                    }
                    return a.DenominatorException == 1;
                });

                var DenominatorExceptionPatientIds = $.unique(arrDenominatorExceptionPatients).join(",");

                ReportsSSRSDashboard.arrReasoning[currentMeasureId] = JSON.stringify(arrNonCompliantPatients);
                //arrReasoning.push(arrNonCompliantPatients);
                ////$.each(arrNonCompliantPatients, function (i, item) {
                ////    arrReasoning.push(item);
                ////});
                var strReasoning = arrNonCompliantPatients.join(",");
                var arrPatient = [];
                $.each(arrNonCompliantPatients, function (i, item) {
                    arrPatient.push(item.Patientid);
                });

                var PatientIds = $.unique(arrPatient).join(",");

                var nonComplaintFunction = "";
                if (arrNonCompliantPatients.length > 0) {
                    //nonComplaintFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + JSON.stringify(arrNonCompliantPatients) + "\");'";
                    nonComplaintFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + ProviderId + "\",\"" + "0" + "\",\"" + item.MeasureNumber + " - " + item.Measure + "\");'";
                    nonComplaintFunction = '<td  ' + nonComplaintFunction + '><a href="javascript:void(0);">' + parseInt(item.Optional1) + '</a></td>'
                } else {
                    nonComplaintFunction = '<td >' + parseInt(item.Optional1) + '</td>';
                }

                //nonComplaintFunction = '<td>' + parseInt(item.Optional1) + '</td>';

                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.CQMID + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    //'<td class="hiddden">' + item.MeasureNumber + '</td>' +
                    '<td>' + item.MeasureNumber + '</td>' +
                    '<td>' + item.Measure + '</td>' +
                    '<td>' + parseInt(item.InitialPopulation) + '</td>' +
                    '<td>' + parseInt(item.Denominator) + '</td>' +
                    '<td>' + parseInt(item.Numerator) + '</td>' +
                    '<td>' + parseInt(item.DenominatorExclusion) + '</td>' +

                   '<td class="hidden">' + parseInt(item.NumeratorExclusion) + '</td>' +
                   '<td>' + parseInt(item.DenominatorException) + '</td>' +
                   '<td ' + performanceRateCSS + '>' + parseInt(item.PerfromanceRate1) + ' %' + '</td>' +
                   '<td class="hidden" ' + performanceRateCSS + '>' + parseInt(item.PerfromanceRate2) + ' %' + '</td>' +
                   '<td class="hidden" ' + reportingRateCSS + '>' + parseInt(item.ReportingRate1) + ' %' + '</td>' +
                   '<td class="hidden"' + reportingRateCSS + '>' + parseInt(item.ReportingRate2) + ' %' + '</td>'
                   + nonComplaintFunction
                   //+ '<td class="hiddden">' + parseInt(item.Numerator) + '</td>' +
                   //'<td class="hiddden">' + parseInt(item.Optional1) + '</td>'
                   );
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #CQMIndividualReport #dgvIndividualReport tbody').last().append($row);

            });
        }
    },

    generateHTMLVBPIndiReport: function (response, ProviderId, ProviderName) {
        if (ReportsSSRSDashboard.params.PHQ2SelectedPatientId != null) {
            ReportsSSRSDashboard.params.PHQ2SelectedPatientId = null;
            ReportsSSRSDashboard.params.PHQ2SelectedNoteId = null;
        }
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #VBPIndividualReport #dgvIndividualReport tbody').html('');
        if (response.AllMeasuresCount > 0) {
            var pqrsReportList_JSON = JSON.parse(response.AllMeasuresLoad_JSON);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VBPIndividualReport #lblProviderId').text(' ' + ProviderName);//.text(response.ProviderName);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VBPIndividualReport #lblProivderNPI').hide();//.text(response.NPI);
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VBPIndividualReport #lblProviderNPIHeading').hide();
            $.each(pqrsReportList_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.CQMID + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.CQMID);
                $row.attr("measureId", item.CQMID);
                var reportingRateCSS = "";
                var performanceRateCSS = "";
                var pr = parseFloat(item.PerfromanceRate1);
                if (pr < 50) {
                    performanceRateCSS = 'style ="color:red"';
                }
                else {
                    performanceRateCSS = 'style ="color:LimeGreen"';
                }
                var rr = parseFloat(item.ReportingRate1);
                if (rr < 50) {
                    reportingRateCSS = 'style ="color:red"';
                }
                else {
                    reportingRateCSS = 'style ="color:LimeGreen"';
                }
                var currentMeasureId = item.ID;

                var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                    return a.MeasureId == currentMeasureId;
                });

                var arrInitialPopulationPatients = [];
                var arrInitialPopulationPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.InitialPopulation == 1 && a.CQMID == item.CQMID) {
                        arrInitialPopulationPatients.push(a.PatientID);
                    }
                    return (a.InitialPopulation == 1 && a.CQMID == item.CQMID);
                });
                var InitialPopulationPatientIds = $.unique(arrInitialPopulationPatients).join(",");
                var InitialPopulationFunction = "";
                if (arrInitialPopulationPatients.length > 0) {
                    //InitialPopulationFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + JSON.stringify(arrNonCompliantPatients) + "\");'";
                    InitialPopulationFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + InitialPopulationPatientIds + "\",\"" + ProviderId + "\",\"" + "1" + "\,\"" + item.MeasureNumber + " - " + item.Measure + "\");'";
                    InitialPopulationFunction = '<td ' + InitialPopulationFunction + '><a href="javascript:void(0);">' + parseInt(InitialPopulationPatientIds.split(",").length) + '</a></td>'
                } else {
                    InitialPopulationFunction = '<td>' + parseInt(0) + '</td>';
                }

                var arrPerfMetPatients = [];
                var arrPerfMetPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.Numerator == 1) {
                        arrPerfMetPatients.push(a.PatientID);
                    }
                    return a.Numerator == 1;
                });
                var PerfMetPatientIds = $.unique(arrPerfMetPatients).join(",");
                var PerMetFunction = "";
                if (arrPerfMetPatients.length > 0) {
                    //PerMetFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + JSON.stringify(arrNonCompliantPatients) + "\");'";
                    PerMetFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PerfMetPatientIds + "\",\"" + ProviderId + "\",\"" + "1" + "\,\"" + item.MeasureNumber + " - " + item.Measure + "\");'";
                    PerMetFunction = '<td ' + PerMetFunction + '><a href="javascript:void(0);">' + parseInt(PerfMetPatientIds.split(",").length) + '</a></td>'
                } else {
                    PerMetFunction = '<td>' + parseInt(0) + '</td>';
                }

                var arrPerfNotMetPatients = [];
                var arrPerfNotMetPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.Optional1 == 1) {
                        arrPerfNotMetPatients.push(a.PatientID);
                    }
                    return a.Optional1 == 1;
                });

                var PerfNotMetPatientIds = $.unique(arrPerfNotMetPatients).join(",");

                var arrDenominatorExclusionPatients = [];
                var arrDenominatorExclusionPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.DenominatorExclusion == 1) {
                        arrDenominatorExclusionPatients.push(a.PatientID);
                    }
                    return a.DenominatorExclusion == 1;
                });

                var DenominatorExclusionPatientIds = $.unique(arrDenominatorExclusionPatients).join(",");

                var arrDenominatorExceptionPatients = [];
                var arrDenominatorExceptionPatients1 = $.grep(JSON.parse(response.AllMeasuresDetailLoad_JSON), function (a) {
                    if (a.DenominatorException == 1) {
                        arrDenominatorExceptionPatients.push(a.PatientID);
                    }
                    return a.DenominatorException == 1;
                });

                var DenominatorExceptionPatientIds = $.unique(arrDenominatorExceptionPatients).join(",");

                ReportsSSRSDashboard.arrReasoning[currentMeasureId] = JSON.stringify(arrNonCompliantPatients);
                //arrReasoning.push(arrNonCompliantPatients);
                ////$.each(arrNonCompliantPatients, function (i, item) {
                ////    arrReasoning.push(item);
                ////});
                var strReasoning = arrNonCompliantPatients.join(",");
                var arrPatient = [];
                $.each(arrNonCompliantPatients, function (i, item) {
                    arrPatient.push(item.Patientid);
                });

                var PatientIds = $.unique(arrPatient).join(",");

                var nonComplaintFunction = "";
                if (arrNonCompliantPatients.length > 0) {
                    //nonComplaintFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.CQMID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + JSON.stringify(arrNonCompliantPatients) + "\");'";
                    nonComplaintFunction = "onclick='ReportsSSRSDashboard.openPatientList(\"" + item.ID + "\",\"" + "" + "\",\"" + PatientIds + "\",\"" + ProviderId + "\",\"" + "0" + "\",\"" + item.ID + " - " + item.Measure + "\",\"" + "VBP" + "\");'";
                    nonComplaintFunction = '<td  ' + nonComplaintFunction + '><a href="javascript:void(0);">' + parseInt(item.Optional1) + '</a></td>'
                } else {
                    nonComplaintFunction = '<td >' + parseInt(item.Optional1) + '</td>';
                }

                //nonComplaintFunction = '<td>' + parseInt(item.Optional1) + '</td>';

                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.ID + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    //'<td class="hiddden">' + item.MeasureNumber + '</td>' +
                    '<td>' + (item.MeasureNumber == "" ? item.ID : item.MeasureNumber) + '</td>' +
                    '<td>' + item.Measure + '</td>' +
                    '<td>' + parseInt(item.InitialPopulation) + '</td>' +
                    '<td>' + parseInt(item.Denominator) + '</td>' +
                    '<td>' + parseInt(item.Numerator) + '</td>' +
                    '<td>' + parseInt(item.DenominatorExclusion) + '</td>' +

                   '<td class="hidden">' + parseInt(item.NumeratorExclusion) + '</td>' +
                   '<td>' + parseInt(item.DenominatorException) + '</td>' +
                   '<td ' + performanceRateCSS + '>' + parseInt(item.PerfromanceRate1) + ' %' + '</td>' +
                   '<td class="hidden" ' + performanceRateCSS + '>' + parseInt(item.PerfromanceRate2) + ' %' + '</td>' +
                   '<td class="hidden" ' + reportingRateCSS + '>' + parseInt(item.ReportingRate1) + ' %' + '</td>' +
                   '<td class="hidden"' + reportingRateCSS + '>' + parseInt(item.ReportingRate2) + ' %' + '</td>'
                   + nonComplaintFunction
                   //+ '<td class="hiddden">' + parseInt(item.Numerator) + '</td>' +
                   //'<td class="hiddden">' + parseInt(item.Optional1) + '</td>'
                   );
                var TableBody = $('#' + ReportsSSRSDashboard.params.PanelID + ' #VBPIndividualReport #dgvIndividualReport tbody').last();
                if (item.ID == "PHQ2") {
                    TableBody.prepend($row);
                }
                else {
                    TableBody.append($row);
                }
            });
        }
    },
    generateGPROReport: function (response) {

        $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSGPROSubmission #dgvGPROReport tbody').html('');
        if (response.gproReportCount > 0) {
            var gproReportList_JSON = JSON.parse(response.gproReportList_JSON);
            // $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSGPROSubmission #lblProviderId').text(response.ProviderName);
            // $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSGPROSubmission #lblProivderNPI').text(response.NPI);
            $.each(gproReportList_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.measureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.measureId);
                $row.attr("measureId", item.measureId);
                var reportingRateCSS = "";
                var performanceRateCSS = "";
                var pr = parseFloat(item.performanceRate);
                if (pr < 50) {
                    performanceRateCSS = 'style ="color:red"';
                }
                else {
                    performanceRateCSS = 'style ="color:LimeGreen"';
                }
                var rr = parseFloat(item.reportingRate);
                if (rr < 50) {
                    reportingRateCSS = 'style ="color:red"';
                }
                else {
                    reportingRateCSS = 'style ="color:LimeGreen"';
                }

                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.measureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.measureId + '</td>' +
                    '<td>' + item.measureName + '</td>' +
                    '<td>' + item.denuminator + '</td>' +
                   '<td>' + item.numerator + '</td>' +
                   '<td ' + reportingRateCSS + '>' + parseFloat(item.reportingRate).toFixed(2) + ' %' + '</td>' +
                   '<td ' + performanceRateCSS + '>' + parseFloat(item.performanceRate).toFixed(2) + ' %' + '</td>' +
                   '<td>' + item.exclusion + '</td>' +
                   '<td>' + item.nonCompliantPatients + '</td>' +
                   '<td>' + item.performanceMet + '</td>' +
                   '<td>' + item.performanceNotMet + '</td>');
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSGPROSubmission #dgvGPROReport tbody').last().append($row);

            });
        }
    },

    generateSummaryReport: function (response) {

        $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSSubmissionSummaryReport #dgvSSReport tbody').html('');
        if (response.summaryReportCount > 0) {
            var summaryReportList_JSON = JSON.parse(response.summaryReportList_JSON);
            // $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSGPROSubmission #lblProviderId').text(response.ProviderName);
            // $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSGPROSubmission #lblProivderNPI').text(response.NPI);
            $.each(summaryReportList_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.measureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.measureId);
                $row.attr("measureId", item.measureId);
                var reportingRateCSS = "";
                var performanceRateCSS = "";
                var pr = parseFloat(item.performanceRate);
                if (pr < 50) {
                    performanceRateCSS = 'style ="color:red"';
                }
                else {
                    performanceRateCSS = 'style ="color:LimeGreen"';
                }
                var rr = parseFloat(item.reportingRate);
                if (rr < 50) {
                    reportingRateCSS = 'style ="color:red"';
                }
                else {
                    reportingRateCSS = 'style ="color:LimeGreen"';
                }

                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.measureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.measureId + '</td>' +
                    '<td>' + item.measureName + '</td>' +
                    '<td>' + item.numerator + '</td>' +
                    '<td>' + item.numerator + '</td>' +
                    '<td>' + item.denuminator + '</td>' +
                    '<td>' + item.performanceMet + '</td>' +
                    '<td>' + item.performanceNotMet + '</td>' +
                '<td>' + item.exclusion + '</td>' +
                '<td ' + reportingRateCSS + '>' + parseFloat(item.reportingRate).toFixed(2) + ' %' + '</td>' +
                '<td ' + performanceRateCSS + '>' + parseFloat(item.performanceRate).toFixed(2) + ' %' + '</td>');

                $('#' + ReportsSSRSDashboard.params.PanelID + ' #PQRSSubmissionSummaryReport #dgvSSReport tbody').last().append($row);

            });
        }
    },


    //Back End Calls for PQRS Registry Report
    generateIndividualReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["commandType"] = "GENERATE_INDIVIDUAL_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSReports");
    },

    //Back End Calls for CQM Individual Report
    generateCQMIndividualReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        //objData["commandType"] = "GENERATE_INDIVIDUAL_REPORT";
        //var data = JSON.stringify(objData);
        //return MDVisionService.APIService(data, "PQRS", "PQRSReports");

        //var objData = {};
        ////if (NotesData != null) {
        ////    objData = JSON.parse(NotesData);
        ////}
        objData["from"] = objData["ReportFromDate"];
        ReportsSSRSDashboard.params.ReportFromDate = objData["ReportFromDate"];
        objData["to"] = objData["ReportToDate"];
        ReportsSSRSDashboard.params.ReportToDate = objData["ReportToDate"];
        objData["reportType"] = "0";
        objData["cqmId"] = "";
        objData["eitherDetail"] = "0";
        objData["PatientId"] = "";
        //objData["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        objData["commandType"] = "load_cqm_with_reasoning";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    generateVBPIndividualReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["from"] = objData["ReportFromDate"];
        ReportsSSRSDashboard.params.ReportFromDate = objData["ReportFromDate"];
        objData["to"] = objData["ReportToDate"];
        ReportsSSRSDashboard.params.ReportToDate = objData["ReportToDate"];
        objData["reportType"] = "0";
        objData["VBPId"] = "";
        objData["eitherDetail"] = "0";
        objData["PatientId"] = "";
        //objData["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        objData["commandType"] = "load_VBP_with_reasoning";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    generateSSReport_DbCall: function () { },


    generateGPROQRDA1Report_DbCall: function () { },
    generateGPROQRDA3Report_DbCall: function () { },
    generateIndiviQRDA1Report_DbCall: function (clinicalPQRSMeasureDetailData, cqmid, patientId, providerId, dateFrom, dateTo) {
        var objData = JSON.parse(clinicalPQRSMeasureDetailData);
        objData["MeasureId"] = -1;
        objData["commandType"] = "generate_qrda1_report";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRS_QRDA1");

    },
    generateIndiviQRDA3Report_DbCall: function () { },
    //Author: Ahmad Raza
    //Function Name: KPIChart
    //Date: 16-08-2016
    //Description: Creating KPI Chart on PQRSReport Dashboard with dummy data
    KPIChart: function () {
        var myJSON = [];
        for (var i = 0; i < length; i++) {
            var jsonArray = { month: 'Test ' + i, a: i + 200, b: i + 300, c: i + 50 };
            var temp = jsonArray;
            myJSON.push(temp);
        }

        var chart = Morris.Bar({
            xLabelMargin: 10,
            xLabelAngle: 45,
            element: 'myChart',
            data: myJSON,
            hoverCallback: function (index, options, content) {
                return (content);
            },
            xkey: 'month',
            ykeys: ['a', 'b', 'c'],
            labels: ['Data Missing', 'Pref Met', 'Pref.not.Met'],
            preUnits: ['$'],
            barColors: function (row, series, type) {

                if (series.label == "Data Missing") return "#FF0000";
                if (series.label == "Pref Met") return "#007F00";
                if (series.label == "Pref.not.Met") return "#ffff66";

            },
        });
        $('#myChart').resize(function () {
            //   if ($("#ctrlPanDashBoard").css("display") != "none") {
            chart.redraw();
            //   }
        });

    },

    //Author: Ahmad Raza
    //Function Name: KPICharts
    //Date: 19-08-2016
    //Description: Creating KPI Chart on PQRSReport Dashboard
    KPICharts: function (response, CQMAllMeasuresResponse) {

        var KPIlist = null;
        if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0)
            KPIlist = JSON.parse(CQMAllMeasuresResponse.AllMeasuresLoad_JSON);
        else if (response != null)
            KPIlist = JSON.parse(response.pqrsReportList_JSON);
        var myJSON = [];

        var totala = 0;
        var totalb = 0;
        var totalc = 0;
        $.each(KPIlist, function (i, item) {

            var tempa = parseFloat(totala);
            var tempb = parseFloat(totalb);
            var tempc = parseFloat(totalc);
            if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0) {
                tempa += parseFloat(item.Optional1);
                tempb += parseFloat(item.Numerator);// + parseFloat(item.NumeratorExclusion);
                tempc += parseFloat(item.Optional1);
            }
            else {
                tempa += parseFloat(item.nonCompliantPatients);
                tempb += parseFloat(item.performanceMet);
                tempc += parseFloat(item.performanceNotMet);
            }



            totala = tempa;
            totalb = tempb;
            totalc = tempc;
        });
        var totalarray = { Age: 'Status', a: totala, b: totalb, c: totalc };
        var tempobj = totalarray;
        myJSON.push(tempobj);
        var gridId = 'myChart';
        if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0) {
            gridId = 'myCQMChart';
        }

        var individualReportChart = Morris.Bar({
            xLabelMargin: 10,
            xLabelAngle: 45,
            element: gridId,
            data: myJSON,
            hoverCallback: function (index, options, content) {
                return (content);
            },
            xkey: 'Age',
            ykeys: ['a', 'b', 'c'],
            labels: ['Data Missing', 'Perf Met', 'Perf.Not.Met'],
            preUnits: [''],
            barColors: function (row, series, type) {

                if (series.label == "Data Missing") return "#FF0000";
                if (series.label == "Perf Met") return "#007F00";
                if (series.label == "Perf.Not.Met") return "#ffff66";

                $("#CQMIndividualkdhtml div.morris-hover.morris-default-style").css("left", "98%");
            },
        });

        //$('#myChart').resize(function () {
        //    if ($("#ctrlPanDashBoard").css("display") != "none") {
        //        individualReportChart.redraw();
        //    }
        //});
        //if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0) {
        //    $("#CQMIndividualkdhtml div.morris-hover.morris-default-style").css("left", "100%");
        //}


    },

    //Author: Muhammad Arshad
    //Function Name: KPICharts for CQM
    //Date: 16-02-2017
    //Description: Creating KPI Chart on CQM Report Dashboard
    KPIChartsCQM: function (response, CQMAllMeasuresResponse) {

        var KPIlist = null;
        if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0)
            KPIlist = JSON.parse(CQMAllMeasuresResponse.AllMeasuresLoad_JSON);
        else if (response != null)
            KPIlist = JSON.parse(response.pqrsReportList_JSON);
        var myJSON = [];

        var totala = 0;
        var totalb = 0;
        var totalc = 0;
        $.each(KPIlist, function (i, item) {

            var tempa = parseFloat(totala);
            var tempb = parseFloat(totalb);
            var tempc = parseFloat(totalc);
            if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0) {
                tempa += parseFloat(item.Optional1);
                tempb += parseFloat(item.Numerator);// + parseFloat(item.NumeratorExclusion);
                tempc += parseFloat(item.Optional1);
            }
            else {
                tempa += parseFloat(item.nonCompliantPatients);
                tempb += parseFloat(item.performanceMet);
                tempc += parseFloat(item.performanceNotMet);
            }



            totala = tempa;
            totalb = tempb;
            totalc = tempc;
        });
        var totalarray = { Age: 'Data Missing', a: totala };
        var tempobj = totalarray;
        myJSON.push(tempobj);
        var totalarrayPerfMet = { Age: 'Perf Met', a: totalb };
        myJSON.push(totalarrayPerfMet);
        var totalarrayPerfNotMet = { Age: 'Perf Not Met', a: totalc };
        myJSON.push(totalarrayPerfNotMet);
        var gridId = 'myChart';
        if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0) {
            gridId = 'myCQMChart';
        }

        var individualReportChart = Morris.Bar({
            xLabelMargin: 10,
            xLabelAngle: 45,
            element: gridId,
            data: myJSON,
            gridIntegers: true,
            ymin: 0,
            hoverCallback: function (index, options, content) {
                content = content.replace('#ffff66', '#007F00');
                if (content.indexOf('Perf Met') > -1) {
                    content = content.replace('Data Missing:', '');
                }
                else if (content.indexOf('Perf Not Met') > -1) {
                    content = content.replace('Data Missing:', '');
                }
                else if (content.indexOf('Data Missing') > -1) {
                    content = content.replace('Data Missing:', '');
                }
                return (content);
            },
            xkey: 'Age',
            ykeys: ['a'],
            labels: ['Data Missing', 'Perf Met', 'Perf Not Met'],
            preUnits: [''],
            barColors: function (row, series, type) {
                //$("#CQMIndividualkdhtml div.morris-hover.morris-default-style").css("left", "98%");
                if (row.label == "Data Missing") return "#FF0000";
                if (row.label == "Perf Met") return "#007F00";
                if (row.label == "Perf Not Met") return "#ffff66";


            },
        });

        //$('#myChart').resize(function () {
        //    if ($("#ctrlPanDashBoard").css("display") != "none") {
        //        individualReportChart.redraw();
        //    }
        //});
        //if (CQMAllMeasuresResponse.AllMeasuresLoad_JSON != null && CQMAllMeasuresResponse.AllMeasuresCount > 0) {
        //    $("#CQMIndividualkdhtml div.morris-hover.morris-default-style").css("left", "100%");
        //}


    },

    //Author: Muhammad Arshad
    //Function Name: KPICharts for VBP
    //Date: 10-04-2017
    //Description: Creating KPI Chart on VBP Report Dashboard
    KPIChartsVBP: function (response, VBPAllMeasuresResponse) {
        var KPIlist = null;
        if (VBPAllMeasuresResponse.AllMeasuresLoad_JSON != null && VBPAllMeasuresResponse.AllMeasuresCount > 0)
            KPIlist = JSON.parse(VBPAllMeasuresResponse.AllMeasuresLoad_JSON);
        else if (response != null)
            KPIlist = JSON.parse(response.pqrsReportList_JSON);
        var myJSON = [];

        var totala = 0;
        var totalb = 0;
        var totalc = 0;
        $.each(KPIlist, function (i, item) {
            var tempa = parseFloat(totala);
            var tempb = parseFloat(totalb);
            var tempc = parseFloat(totalc);
            if (VBPAllMeasuresResponse.AllMeasuresLoad_JSON != null && VBPAllMeasuresResponse.AllMeasuresCount > 0) {
                tempa += parseFloat(item.Optional1);
                tempb += parseFloat(item.Numerator);// + parseFloat(item.NumeratorExclusion);
                tempc += parseFloat(item.Optional1);
            }
            else {
                tempa += parseFloat(item.nonCompliantPatients);
                tempb += parseFloat(item.performanceMet);
                tempc += parseFloat(item.performanceNotMet);
            }
            totala = tempa;
            totalb = tempb;
            totalc = tempc;
        });
        var totalarray = { Age: 'Data Missing', a: totala };
        var tempobj = totalarray;
        myJSON.push(tempobj);
        var totalarrayPerfMet = { Age: 'Perf Met', a: totalb };
        myJSON.push(totalarrayPerfMet);
        var totalarrayPerfNotMet = { Age: 'Perf Not Met', a: totalc };
        myJSON.push(totalarrayPerfNotMet);
        var gridId = 'myChart';
        if (VBPAllMeasuresResponse.AllMeasuresLoad_JSON != null && VBPAllMeasuresResponse.AllMeasuresCount > 0) {
            gridId = 'myVBPChart';
        }

        var individualReportChart = Morris.Bar({
            xLabelMargin: 10,
            xLabelAngle: 45,
            element: gridId,
            data: myJSON,
            gridIntegers: true,
            ymin: 0,
            hoverCallback: function (index, options, content) {
                content = content.replace('#ffff66', '#007F00');
                if (content.indexOf('Perf Met') > -1) {
                    content = content.replace('Data Missing:', '');
                }
                else if (content.indexOf('Perf Not Met') > -1) {
                    content = content.replace('Data Missing:', '');
                }
                else if (content.indexOf('Data Missing') > -1) {
                    content = content.replace('Data Missing:', '');
                }
                return (content);
            },
            xkey: 'Age',
            ykeys: ['a'],
            labels: ['Data Missing', 'Perf Met', 'Perf Not Met'],
            preUnits: [''],
            barColors: function (row, series, type) {
                //$("#VBPIndividualkdhtml div.morris-hover.morris-default-style").css("left", "98%");
                if (row.label == "Data Missing") return "#FF0000";
                if (row.label == "Perf Met") return "#007F00";
                if (row.label == "Perf Not Met") return "#ffff66";
            },
        });
    },
    //Author: Ahmad Raza
    //Function Name: GPROCharts
    //Date: 22-08-2016
    //Description: Creating KPI Chart on PQRSReport Dashboard
    GPROCharts: function (response) {

        var KPIlist = JSON.parse(response.gproReportList_JSON);
        var myJSON = [];

        var totala = 0;
        var totalb = 0;
        var totalc = 0;
        $.each(KPIlist, function (i, item) {

            var tempa = parseFloat(totala);
            var tempb = parseFloat(totalb);
            var tempc = parseFloat(totalc);

            tempa += parseFloat(item.nonCompliantPatients);
            tempb += parseFloat(item.performanceMet);
            tempc += parseFloat(item.performanceNotMet);

            totala = tempa;
            totalb = tempb;
            totalc = tempc;

        });
        var totalarray = { Age: 'Status', a: totala, b: totalb, c: totalc };
        var tempobj = totalarray;
        myJSON.push(tempobj);

        var gproReportChart = Morris.Bar({
            xLabelMargin: 10,
            xLabelAngle: 45,
            yLabelMargin: 10,
            yLabelAngle: 45,
            element: 'GPROChart',
            data: myJSON,
            hoverCallback: function (index, options, content) {
                return (content);
            },
            xkey: 'Age',
            label: 'Age',
            ykeys: ['a', 'b', 'c'],
            labels: ['Data Missing', 'Pref Met', 'Pref.Not.Met'],
            preUnits: [''],
            barColors: function (row, series, type) {

                if (series.label == "Data Missing") return "#FF0000";
                if (series.label == "Pref Met") return "#007F00";
                if (series.label == "Pref.Not.Met") return "#ffff66";

            },
        });
        //$('#myChart').resize(function () {
        //    if ($("#ctrlPanDashBoard").css("display") != "none") {
        //        individualReportChart.redraw();
        //    }
        //});


    },


    //Author: Ahmad Raza
    //Function Name: openPatientList
    //Date: 16-08-2016
    //Description: Opening Patient List from PQRS Report Dashboard
    openPatientList: function (measureId, VisitIds, PatientIds, ProviderId, OnlyShowPatients, MeasureFullName, ReportType) {

        var params = [];
        params["mode"] = "Add";
        params["measureId"] = measureId;
        params["MeasureFullName"] = MeasureFullName;
        params["ReportType"] = ReportType;
        if (PatientIds != null && PatientIds != "") {
            params["PatientIds"] = PatientIds;
        }
        else {
            params["PatientIds"] = "";
        }
        params["ProviderId"] = ProviderId;
        var arrcurrentMeasureReasoning = JSON.parse(ReportsSSRSDashboard.arrReasoning[measureId]);

        if (OnlyShowPatients != "1" && arrcurrentMeasureReasoning != null && arrcurrentMeasureReasoning.length > 0) {
            params["arrcurrentMeasureReasoning"] = arrcurrentMeasureReasoning;
            params["OnlyShowPatients"] = 0;
        }
        else {
            params["OnlyShowPatients"] = 1;
            params["arrcurrentMeasureReasoning"] = "";
        }
        params["FromParentCtrl"] = "ReportsSSRSDashboard";
        params["VisitIds"] = VisitIds;
        params["PatientId"] = ReportsSSRSDashboard.params.PatientId;
        params["FromAdmin"] = 0;
        params["ReportFromDate"] = ReportsSSRSDashboard.params.ReportFromDate;
        params["ReportToDate"] = ReportsSSRSDashboard.params.ReportToDate;
        //./ params["ParentCtrl"] = "ReportsSSRSDashboard";
        LoadActionPan('PQRS_Patient_List', params);


    },

    generateGPROSubmissionReport: function () {
        var hasError = false;
        if (!hasError) {
            var self = $('#' + ReportsSSRSDashboard.params.PanelID + ' form #PQRSGPROSubmission');
            var myJSON = self.getMyJSONByName();
            ReportsSSRSDashboard.generateGPROReport_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #GPROkdhtml').removeClass('hidden');
                    ReportsSSRSDashboard.generateGPROReport(response);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #GPROChart').empty();
                    ReportsSSRSDashboard.GPROCharts(response);
                }
                else {
                    ReportsSSRSDashboard.generateGPROReport(response);
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #GPROChart').empty();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    generateGPROReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["commandType"] = "GENERATE_GPRO_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSReports");
    },
    generateSubmissionSummaryReport: function () {

        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtSSProviderGroup').val() != '' || $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtSSProvider').val() != '') {

            var hasError = false;
            if (!hasError) {
                var self = $('#' + ReportsSSRSDashboard.params.PanelID + ' form #PQRSSubmissionSummaryReport');
                var myJSON = self.getMyJSONByName();
                ReportsSSRSDashboard.generateSummaryReport_DbCall(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #Summarykdhtml').removeClass('hidden');

                        ReportsSSRSDashboard.generateSummaryReport(response);

                    }
                    else {
                        ReportsSSRSDashboard.generateSummaryReport(response);
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else {
            utility.DisplayMessages("Please Select Provider or Provider Group", 3);
        }
    },
    generateSummaryReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["commandType"] = "GENERATE_SUMMARY_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSReports");
    },

    //Author: Ahmad Raza
    //Function Name:validateReport
    //Date: 23-08-2016
    //Description: Field validation of PQRS Reports
    validateReport: function () {
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #frmSSRSReports')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ProviderId: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ProviderGroupId: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }

               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {

                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    case 'btngenerateindividualreport':
                        ReportsSSRSDashboard.generateIndividualReport();
                        break;
                    case 'btngenerategproreport':
                        ReportsSSRSDashboard.generateGPROSubmissionReport();
                        break;
                    case 'btngenerateCQMindividualreport':
                        ReportsSSRSDashboard.generateCQMIndividualReport();
                        break;
                    case 'btngenerateVBPindividualreport':
                        ReportsSSRSDashboard.generateVBPIndividualReport();
                        break;
                }

            }
            e.type = "";
        });

    },
    //***********************************************************************
    //****************** END PQRS Registry Report Functions *****************
    //***********************************************************************


    //***********************************************************************
    //****************** START CLINICAL Report Functions *****************
    //***********************************************************************
    showHideAdvControls: function (cntrl, ParentDiv) {
        if ($(cntrl).attr('isShow') == "0") {
            $(cntrl).attr('isShow', "1");
            $(cntrl).text("Normal Search");
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #' + ParentDiv).show();
        } else {
            $(cntrl).attr('isShow', "0");
            $(cntrl).text("Advanced Search");
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #' + ParentDiv).hide();
        }
    },
    //medication
    generateMedicationsReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();
        ReportsSSRSDashboard.generateMedicationseReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.MedicationseLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    MedicationseLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [
             { title: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' }
        , { field: "PatientName", title: "Patient Name", width: "auto" },
                                { field: "DOB", title: "DOB", width: "130px" },
                                { field: "PatStatus", title: "Pt. Status", width: "auto" },
                                { field: "Medication", title: "Medication", width: "auto" },
                                { field: "MedStatus", title: "Med. Status", width: "auto" },
                                { field: "StartDate", title: "Start Date", width: "auto" },
                                { field: "EndDate", title: "End Date", width: "auto" }
        ];
        if (response.medicationsCount > 0) {
            var medicationseJSONData = JSON.parse(response.medicationsList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                MedStatus: { type: "string" },
                Medication: { type: "string" },
                StartDate: { type: "string" },
                EndDate: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(medicationseJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },

    generateMedicationseReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["MedicationAND"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters [name=MedicationAND]:checked').val() == "1" ? true : false;
        objData["commandType"] = "LOAD_MEDICATIONS_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },
    //vitals
    generateVitalsReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();
        ReportsSSRSDashboard.generateVitalseReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.VitalseLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    VitalseLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [
             { title: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' }
        , { field: "PatientName", title: "Patient Name", width: "auto" },
                                { field: "DOB", title: "DOB", width: "130px" },
                                { field: "PatStatus", title: "Pt. Status", width: "auto" },
                                { field: "Systolic", title: "<span>Systolic <br/> mmHg</span>", width: "auto" },
                                { field: "Diastolic", title: "<span>Diastolic <br/> mmHg</span>", width: "auto" },
                                { field: "Pulse", title: "<span>Pulse <br/> bpm</span>", width: "auto" },
                                { field: "Temprature", title: "<span>Temp. <br/> F</span>", width: "auto" },
                                { field: "Respiration", title: "<span>Resp. <br/>  rpm</span>", width: "auto" },
                                //{ field: "Weight", title: "<span>Weight <br/> lbs</span>", width: "auto" },
                                { field: "Height", title: "<span>Height <br/> Inches</span>", width: "auto" },
                                { field: "BSA", title: "<span>BSA <br/> m<sup>2</sup></span>", width: "auto" },
                                { field: "Weight", title: "<span>Weight <br/> lbs</span>", width: "auto" },
                                { field: "BMI", title: "<span>BMI <br/>  kg/m<sup>2</sup></span>", width: "auto" },
                                { field: "SPO2", title: "<span>SPO2 <br/> %</span>", width: "auto" },
            { field: "DOS", title: "DOS", width: "auto" },
        ];
        if (response.vitalsCount > 0) {
            var VitalseJSONData = JSON.parse(response.vitalsList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Systolic: { type: "string" },
                Diastolic: { type: "string" },
                Pulse: { type: "string" },
                Temprature: { type: "string" },
                Respiration: { type: "string" },
                Weight: { type: "string" },
                Height: { type: "string" },
                BSA: { type: "string" },
                BMI: { type: "string" },
                SPO2: { type: "string" },
                DOS: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(VitalseJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },

    generateVitalseReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        var isAdvancedSearch = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Vitalslabel > a').attr('isShow');
        if (isAdvancedSearch == "0") {
            objData["AdvancedSearch"] = false;
        }
        else {
            objData["AdvancedSearch"] = true;
        }

        objData["commandType"] = "LOAD_VITALS_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },
    //allergies
    generateAllergiesReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();

        ReportsSSRSDashboard.generateAllergieseReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                ReportsSSRSDashboard.AllergieseLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    AllergieseLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [
        {
            title: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>'
        }
        , {
            field: "PatientName", title: "Patient Name", width: "auto"
        },
            {
                field: "DOB", title: "DOB", width: "auto"
            },
            {
                field: "PatStatus", title: "Pt. Status", width: "auto"
            },
                {
                    field: "Allergy", title: "Allergy", width: "auto"
                },
            {
                field: "Reaction", title: "Reaction (s)", width: "auto"
            },
        {
            field: "OnSetDate", title: "Onset Date", width: "auto"
        },
            {
                field: "AllergyStatus", title: "Allergy Status", width: "auto"
            }
        ];
        if (response.allergiesCount > 0) {
            var AllergieseJSONData = JSON.parse(response.allergiesList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Allergy: { type: "string" },
                Reaction: { type: "string" },
                OnSetDate: { type: "string" },
                AllergyStatus: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(AllergieseJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },
    generateAllergieseReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["Allergy_text"] = "";
        objData["Allergy_RefValue"] = "";
        objData["AllergyReaction_text"] = "";
        objData["AllergyStatus_text"] = "";
        objData["AllergyAND"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters [name=AllergyAND]:checked').val() == "1" ? true : false;
        objData["commandType"] = "LOAD_ALLERGIES_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },
    generateImmunizationReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();
        var ParseJSONData = JSON.parse(myJSON);
        if (ParseJSONData.Provider != "") {
            ReportsSSRSDashboard.generateImmunizationReport_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    ReportsSSRSDashboard.ImmunizationLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProviderDivReports #txtProvider').focus();
            utility.DisplayMessages("Provider is not selected", 2);
        }
    },

    ImmunizationLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [

                      { title: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                      { field: "PatientName", title: "Patient Name", width: "auto" },
                      { field: "DOB", title: "DOB", width: "auto" },
                      { field: "PatStatus", title: "Pt. Status", width: "auto" },
                      { field: "Category", title: "Category", width: "auto" },
                      { field: "vaccine", title: "Vaccine", width: "auto" },
                      { field: "Alert", title: "Alert", width: "auto", template: '#=ReportsSSRSDashboard.MakeAlertForImmunizationReport(data)#' },
                      { field: "Route", title: "Route", width: "auto" },
                      { field: "Site", title: "Site", width: "auto" },
                      { field: "Reaction", title: "Reaction", width: "auto" },
                      { field: "AdministeredBy", title: "Administered By", width: "auto" },
                      { field: "AdminDate", title: "Admin Date", width: "auto" },
                      { field: "DueDate", title: "Due Date", width: "auto" }

        ];
        if (response.immunizationCount > 0) {
            var ImmunizationJSONData = JSON.parse(response.immunizationList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Category: { type: "string" },
                vaccine: { type: "string" },
                Alert: { type: "string" },
                Route: { type: "string" },
                Site: { type: "string" },
                Reaction: { type: "string" },
                AdministeredBy: { type: "string" },
                AdminDate: { type: "string" },
                DueDate: { type: "string" },
                PatientId: { type: "number" },
                VoidDose: { type: "string" }
            };
            KenduReports.inializeKenduGridForImmunization(ImmunizationJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGridForImmunization([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },
    MakeAlertForImmunizationReport: function (data) {
        if (data.Alert == "Normal") {
            return '<span class="text-success">' + data.Alert + '</span>';
        }
        else if (data.Alert == "Due") {
            return '<span class="text-warning">' + data.Alert + '</span>';
        }
        else if (data.Alert == "Overdue") {
            return '<span class="text-danger">' + data.Alert + '</span>';
        }
        else {
            return data.Alert;
        }
    },

    generateImmunizationReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["commandType"] = "LOAD_IMMUNIZATION_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },
    //---------CCM Report----//
    generateCCmReport: function () {
        var FromDate = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #From').val();
        var ToDate = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #To').val();
        if (FromDate == "" && ToDate == "") {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #From').focus();
            utility.DisplayMessages("Please select start/end date", 2);
            return false;
        } else if (FromDate == "") {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #From').focus();
            utility.DisplayMessages("Please select start date", 2);
            return false;
        } else if (ToDate == "") {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #To').focus();
            utility.DisplayMessages("Please select End date", 2);
            return false;
        }
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();
        ReportsSSRSDashboard.generateCcmReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.ccmReportLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ccmReportLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [
            { field: "PracticeName", title: "Practice", width: "65px" },
            { field: "FacilityName", title: "Facility", width: "60px" },
             { title: "AccountNumber", title: "Account Number", width: "120px", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' }
        , { field: "PatientName", title: "Patient Name", width: "100px" },
                                { field: "DOB", title: "DOB", width: "80px" },
                                { field: "Gender", title: "Gender", width: "60px" },
                                { field: "ProgramStatus", title: "Program Status", width: "110px", headerAttributes: { style: "white-space: normal" } },
                                { field: "ConsentStatus", title: "Consent Status", width: "110px", headerAttributes: { style: "white-space: normal" } },
                                { field: "TimeCompleted", title: "Time Completed", width: "110px", headerAttributes: { style: "white-space: normal" } },
                                { field: "ChronicConditionsCount", title: "No. of Chronic conditions", width: "110px", headerAttributes: { style: "white-space: normal" } },
                                { field: "Problems", title: "Problem(s)", width: "90px" },
                                { field: "Provider", title: "Provider", width: "90px" },
                                { field: "CareManager", title: "Care Manager", width: "100px" },
                                { field: "CareCoordinator", title: "Care Coordinator", width: "120px" },
                                { field: "CareGiver", title: "Care Giver", width: "100px" },
                                { field: "ProgramType", title: "Program Type", width: "110px" }

        ];
        if (response.ccmCount > 0) {
            var ccmList_JSONData = JSON.parse(response.ccmList_JSON);

            var fieldRows = {
                PatientId: { type: "number" },
                PracticeName: { type: "string" },
                FacilityName: { type: "string" },
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                Gender: { type: "string" },
                ProgramStatus: { type: "string" },
                ConsentStatus: { type: "string" },
                TimeCompleted: { type: "string" },

                ChronicConditionsCount: { type: "string" },
                Problems: { type: "string" },
                Provider: { type: "string" },
                CareManager: { type: "string" },
                CareCoordinator: { type: "string" },
                CareGiver: { type: "string" },
                ProgramType: { type: "string" },
            };
            KenduReports.inializeKenduGrid(ccmList_JSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },

    generateCcmReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["commandType"] = "LOAD_CCM_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "CCM_Reports");
    },
    //------CCM Report Functions end-----//
    getPrintPDF: function (selector, Name) {
        //------------------------------------------------------   KenduReports.getPDF(selector, Name);

        ReportsSSRSDashboard.getHeaderFooterInfo().done(function (response) {
            response = JSON.parse(response);

            var insideHTML = ' <script type="x/kendo-template" id="page-template">' +
                   ' <div class="page-template">' +
                   '     <div class="blueBorderPrint" style="position:absolute;left:20px; right: 20px;font-size: 90%;height: 50px;top: 15px; padding-bottom:5px;">' +
                   '         <img id="ClinicalReportsHeaderLogo" src="content/images/SHS-nav-logo-small-100.png" class="img-responsive" style="max-width: 100px;height:100%;">' +
                   '     </div>' +
                   '     <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">' +
                   '         <div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>' +
                   '         <div id="ClinicalReportsFooterText" class="blueBgPrint" style=" float:left; width:100%;padding:2px 5px 0 5px;height:22px;">' +
                   '             Generated by: <span id="ClinicalReportsFooter">MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page #:pageNum# of #:totalPages#</span>' +
                   '     </div>' +
                   '     </div>' +
                   ' </div>' +
                '</script>';
            var PageTemp = $(insideHTML);

            var HeaderLogo = response.HeaderLogo;
            var FooterText = response.FooterText;

            if (HeaderLogo !== null && HeaderLogo !== "") {
                var CustomLogo = '<img id="ClinicalReportsHeaderLogo" src="' + HeaderLogo + '   " class="img-responsive" style="width: 100px;">';
                $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', HeaderLogo);
                $(insideHTML).html(PageTemp);
            }
            else {
                var defaultLogo = 'content/images/SHS-nav-logo-small-100.png';
                $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', defaultLogo);
                $(insideHTML).html(PageTemp);
            }


            // ----- footer 

            var marginScale = "2cm";
            if (FooterText !== null && FooterText !== "") {
                var PageTemp = $(insideHTML);
                $(PageTemp).find('#ClinicalReportsFooter').text(FooterText);
                $(insideHTML).html(PageTemp);
            }
            else {
                $(PageTemp).find('#ClinicalReportsFooter').text('MDVISION PM EMR');
                $(insideHTML).html(PageTemp);
            }

            var ReportScale = 0.6;
            var reportordername = "Orders";
            if (ReportsSSRSDashboard.ReportName == "Orders") {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ulOrderTabsItems li').each(function () {
                    if ($(this).prop("class") == "active") {
                        reportordername = ReportsSSRSDashboard.ReportName + " - " + $(this).text();
                    }

                });
                ReportScale = 0.7;
                marginScale = {
                    left: "3mm",
                    top: "20mm",
                    right: "3mm",
                    bottom: "10mm"
                };
            }
            if (ReportsSSRSDashboard.ReportName == "Procedures" || ReportsSSRSDashboard.ReportName == "Problems") {
                ReportScale = 0.32;
            }
            if (ReportsSSRSDashboard.ReportName == "Results") {
                ReportScale = 0.35;
            }
            //var marginScale = "2cm";
            if (ReportsSSRSDashboard.ReportName == "CCM Report") {
                ReportScale = $(document.getElementById('clinicalKenduGrid'))[0].scrollWidth / (3 * 1000);
                marginScale = {
                    left: "5mm",
                    top: "10mm",
                    right: "5mm",
                    bottom: "10mm"
                };
            }
            // ----- end footer 
            kendo.ui.progress($("body"), true);
            //   $("#clinicalKenduGrid").css('width', '21cm');
            // --------------------------------------------- start Download functionality--------------------------------------------------------------
            kendo.drawing.drawDOM("#clinicalKenduGrid", {
                landscape: true,
                paperSize: "A4",
                margin: marginScale,
                repeatHeaders: true,
                template: $(insideHTML).html(),
                scale: ReportScale
            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    kendo.ui.progress($("body"), false);
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    if (ReportsSSRSDashboard.ReportName == "Orders") {
                        params["ReportName"] = reportordername;
                    } else {
                        params["ReportName"] = ReportsSSRSDashboard.ReportName;
                    }

                    params["ParentCtrl"] = "mstrTabReports";
                    LoadActionPan('ReportsSSRSPrintView', params);
                    setTimeout(function () {
                        $('#clinicalKenduGrid .k-grid-header').css('padding-right', '17px');

                    }, 800);
                });
            });

            $(" #clinicalKenduGrid table td").css("word-wrap", "break-word;");
            $(" #clinicalKenduGrid table th").css("word-wrap", "break-word;");
            // ------------------------------------- End Download functionality--------------------------------------
        });
    },

    getPDF: function (selector, Name) {
        //------------------------------------------------------   KenduReports.getPDF(selector, Name);
        if (Name == null) {
            Name = ReportsSSRSDashboard.ReportName;
        }
        ReportsSSRSDashboard.getHeaderFooterInfo().done(function (response) {
            response = JSON.parse(response);
            var HeaderLogo = response.HeaderLogo;
            var FooterText = response.FooterText;
            var insideHTML = ' <script type="x/kendo-template" id="page-template">' +
                   ' <div class="page-template">' +
                   '     <div class="blueBorderPrint" style="position:absolute;left:20px; right: 20px;font-size: 90%;height: 50px;top: 15px; padding-bottom:5px;">' +
                   '         <img id="ClinicalReportsHeaderLogo" src="content/images/SHS-nav-logo-small-100.png" class="img-responsive" style="max-width: 100px;height:100%;">' +
                   '     </div>' +
                   '     <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">' +
                   '         <div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>' +
                   '         <div id="ClinicalReportsFooterText" class="blueBgPrint" style=" float:left; width:100%;padding:2px 5px 0 5px;height:22px;">' +
                   '             Generated by: <span id="ClinicalReportsFooter">MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page #:pageNum# of #:totalPages#</span>' +
                   '     </div>' +
                   '     </div>' +
                   ' </div>' +
                '</script>';
            var PageTemp = $(insideHTML);

            var HeaderLogo = response.HeaderLogo;
            var FooterText = response.FooterText;

            if (HeaderLogo !== null && HeaderLogo !== "") {
                var CustomLogo = '<img id="ClinicalReportsHeaderLogo" src="' + HeaderLogo + '   " class="img-responsive" style="width: 100px;">';
                $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', HeaderLogo);
                $(insideHTML).html(PageTemp);
            }
            else {
                var defaultLogo = 'content/images/SHS-nav-logo-small-100.png';
                $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', defaultLogo);
                $(insideHTML).html(PageTemp);
            }

            // ----- footer 
            if (FooterText !== null && FooterText !== "") {

                var insideHTML = $("#page-template ").html();
                var PageTemp = $(insideHTML);
                $(PageTemp).find('#ClinicalReportsFooter').text(FooterText);
                $(insideHTML).html(PageTemp);
            }
            else {
                $(PageTemp).find('#ClinicalReportsFooter').text('MDVISION PM EMR');
                $(insideHTML).html(PageTemp);
            }

            var ReportScale = 0.6;
            var ReportName = Name;
            if (ReportsSSRSDashboard.ReportName == "Orders") {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ulOrderTabsItems li').each(function () {
                    if ($(this).prop("class") == "active") {
                        ReportName = ReportsSSRSDashboard.ReportName + " - " + $(this).text();
                    }
                });
                ReportScale = 0.30;
            }
            if (ReportsSSRSDashboard.ReportName == "Procedures" || ReportsSSRSDashboard.ReportName == "Problems") {
                ReportScale = 0.30;
            }
            if (ReportsSSRSDashboard.ReportName == "Problems" || ReportsSSRSDashboard.ReportName == "Results" || ReportsSSRSDashboard.ReportName == "Progress Note") {
                ReportScale = 0.35;
            }
            if (ReportsSSRSDashboard.ReportName == "Results") {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ulRersultTabsItems li').each(function () {
                    if ($(this).prop("class") == "active") {
                        ReportName = ReportsSSRSDashboard.ReportName + " - " + $(this).text();
                    }

                });
            }
            var marginScale = "2cm";
            if (ReportsSSRSDashboard.ReportName == "CCM Report") {
                ReportScale = $(document.getElementById('clinicalKenduGrid'))[0].scrollWidth / (4 * 1000);
                marginScale = {
                    left: "5mm",
                    top: "10mm",
                    right: "5mm",
                    bottom: "10mm"
                };
            }
            // ----- end footer 
            kendo.ui.progress($("body"), true);
            // --------------------------------------------- start Download functionality--------------------------------------------------------------
            kendo.drawing.drawDOM($("#clinicalKenduGrid"), {
                landscape: true,
                paperSize: "A4",
                margin: marginScale,
                repeatHeaders: true,
                template: $(insideHTML).html(),
                scale: ReportScale
            }).then(function (group) {
                // Render the result as a PDF file
                return kendo.drawing.exportPDF(group);

            }).done(function (data) {
                // Save the PDF file
                kendo.saveAs({
                    dataURI: data,
                    fileName: ReportName + ".pdf"
                });
                $('#clinicalKenduGrid .k-grid-header').css('padding-right', '0px');
                //  kendo.drawing.pdf.saveAs(group, ReportName + ".pdf");
                setTimeout(function () {
                    $('#clinicalKenduGrid .k-grid-header').css('padding-right', '17px');
                    kendo.ui.progress($("body"), false);
                }, 500);
            });
            $(" #clinicalKenduGrid table td").css("word-wrap", "break-word;");
            $(" #clinicalKenduGrid table th").css("word-wrap", "break-word;");
            // ------------------------------------- End Download functionality--------------------------------------
        });
    },

    // Start || 6 September, 2016 || Talha Tanweer || 
    getHeaderFooterInfo: function () {

        var objData = {};
        objData["commandType"] = "get_report_header_footer_clinical_reports";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },
    // End   || 6 September, 2016 || Talha Tanweer || 


    SelectOrdersTab: function (Type) {
        var PanelDiv = '#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters';
        if (ReportsSSRSDashboard.OrderType != Type) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").hide();

            var TabId = "";
            ReportsSSRSDashboard.OrderType = Type;
            $(PanelDiv + ' .ClinicalOrdersTab').hide();
            $(PanelDiv + ' .tab-pane').removeClass("active");
            if (ReportsSSRSDashboard.OrderType == "Lab") {
                TabId = PanelDiv + " #LabOrderDiv";
                if (!$(TabId).hasClass("active")) {
                    $(TabId).addClass("active");
                    $(PanelDiv + " .LabTab").css('display', 'block');
                }
            }
            else if (ReportsSSRSDashboard.OrderType == "Radiology") {
                TabId = PanelDiv + " #RadiologyOrder";
                if (!$(TabId).hasClass("active")) {
                    $(TabId).addClass("active");
                    $(PanelDiv + " .RadiologyTab").css('display', 'block');
                }

                ReportsSSRSDashboard.LoadLabs('ReportParamaters #RadiologyOrder #txtLab', ReportsSSRSDashboard.params.PanelID).done(function () {

                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #RadiologyOrder #txtLab').multiselect('destroy');
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyOrder #txtLab').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        onDropdownShow: function (event) {
                            $('#' + ReportsSSRSDashboard.params.PanelID + ' #txtLab').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                        },
                    });
                });

                ReportsSSRSDashboard.LoadLabs('ReportParamaters #RadiologyOrder #txtLabRadiology', ReportsSSRSDashboard.params.PanelID).done(function () {

                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #RadiologyOrder #txtLabRadiology').multiselect('destroy');
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyOrder #txtLabRadiology').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        onDropdownShow: function (event) {
                            $('#' + ReportsSSRSDashboard.params.PanelID + ' #txtLab').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                        },
                    });
                });
                ReportsSSRSDashboard.BindFacilityTo();
            }
            else if (ReportsSSRSDashboard.OrderType == "Procedure") {
                TabId = PanelDiv + " #ProcedureOrderDiv";
                if (!$(TabId).hasClass("active")) {
                    $(TabId).addClass("active");
                    $(PanelDiv + " .ProcedureTab ").css('display', 'block');
                }
            }

            else if (ReportsSSRSDashboard.OrderType == "Consultation") {
                TabId = PanelDiv + " #ConsultationOrderDiv";
                if (!$(TabId).hasClass("active")) {
                    $(TabId).addClass("active");
                    $(PanelDiv + " .ConsultationOrderTab").css('display', 'block');
                }

            }
            else if (ReportsSSRSDashboard.OrderType == "Prescription") {
                TabId = PanelDiv + " #PrescriptionOrderDiv";
                if (!$(TabId).hasClass("active")) {
                    $(TabId).addClass("active");
                    $(PanelDiv + " .PrescriptionTab ").css('display', 'block');
                    ReportsSSRSDashboard.fillMedicationDropdown();
                    ReportsSSRSDashboard.LoadPharmacyAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtPharmacy', PanelDiv + '  .tab-pane.active' + ' #hfPharmacy');
                }
            }

            if ($(PanelDiv + '  .tab-pane.active' + ' #txtProvider').length > 0) {
                ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtProvider', PanelDiv + '  .tab-pane.active' + ' #hfProviderOrder');
            }
            if ($(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProvider').length > 0) {
                ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProvider', PanelDiv + '  .tab-pane.active' + ' #hfAssigneeProviderOrder');
            }

            // Start || 5 October, 2016 || Talha Tanweer ||  
            if ($(PanelDiv + '  .tab-pane.active' + ' #hfAssigneeProviderOrderConsultation').length > 0) {
                ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProviderOrderConsultation', PanelDiv + '  .tab-pane.active' + ' #hfAssigneeProviderOrderConsultation');
            } // End || 5 October, 2016 || Talha Tanweer || 
        }





        // Start || 1 October, 2016 || Talha Tanweer ||

        // Start || 1 October, 2016 || Talha Tanweer || 

        return true;
    },

    LoadProviderAutocomplete: function (Ctrl, hfCtrl) {
        $(Ctrl).html('');
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            utility.BindKendoAutoComplete($(Ctrl), 1, "value", "contains", Providers, null, $(hfCtrl));
        });
    },
    LoadUsersAutocomplete: function (cntrl, hfUser) {
        //$(cntrl).html('');
        //CacheManager.BindCodes('GetUsers', false).done(function (result) {
        //    var Users = JSON.parse(result['GetUsers']);
        //    Users = Users.filter(function (el) {
        //        return el.Value !== "";
        //    });
        //    $(cntrl).autocomplete({
        //        autoFocus: true,
        //        source: Users, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $(hfUser).val(ui.item.id); // add the selected id                       
        //            }, 100);
        //        }
        //    });
        //    // $(cntrl).autocomplete("option", "appendTo", "#frmdemographicDetail");
        //});
    },


    SelectResultTab: function (Type) {
        if (ReportsSSRSDashboard.ResultType != Type) {
            $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").hide();

            ReportsSSRSDashboard.ResultType = Type;
            var PanelDiv = '#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters';
            $(PanelDiv + '  .labResult').hide();
            $(PanelDiv + '  .tab-pane').removeClass("active");
            if (ReportsSSRSDashboard.ResultType == "Lab") {
                if (!$(PanelDiv + " #LabResultDiv").hasClass("active")) {
                    $(PanelDiv + " #LabResultDiv").addClass("active");
                    $(PanelDiv + " .LabResultTab").css('display', 'block');
                }
                ReportsSSRSDashboard.BindLabTestText();
            }
            else if (ReportsSSRSDashboard.ResultType == "Radiology") {
                if (!$(PanelDiv + " #RadiologyResult").hasClass("active")) {
                    $(PanelDiv + " #RadiologyResult").addClass("active");
                    $(PanelDiv + " .RadiologyResultTab").css('display', 'block');
                }
                ReportsSSRSDashboard.LoadLabs('ReportParamaters #RadiologyResult #txtLabRadiology', ReportsSSRSDashboard.params.PanelID).done(function () {

                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #RadiologyResult #txtLabRadiology').multiselect('destroy');
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyResult #txtLabRadiology').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        onDropdownShow: function (event) {
                            $('#' + ReportsSSRSDashboard.params.PanelID + ' #txtLabRadiology').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                        },
                    });
                });
                ReportsSSRSDashboard.BindRadiologyLabTestText();
            }
            else if (ReportsSSRSDashboard.ResultType == "Procedure") {
                if (!$(PanelDiv + " #ProcedureResultDiv").hasClass("active")) {
                    $(PanelDiv + " #ProcedureResultDiv").addClass("active");
                    $(PanelDiv + " .ProcedureResultTab").css('display', 'block');
                }
            }
            else if (ReportsSSRSDashboard.ResultType == "Consultation") {
                if (!$(PanelDiv + " #ConsultationResultDiv").hasClass("active")) {
                    $(PanelDiv + " #ConsultationResultDiv").addClass("active");
                    $(PanelDiv + " .ConsultationResultTab").css('display', 'block');
                }
            }
            if ($(PanelDiv + '  .tab-pane.active' + ' #txtProvider').length > 0) {
                ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtProvider', PanelDiv + '  .tab-pane.active' + ' #hfProviderResult');
            }
            if ($(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProvider').length > 0) {
                ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProvider', PanelDiv + '  .tab-pane.active' + ' #hfAssigneeProviderResult');
            }

            //if ($(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProviderOrderConsultation').length > 0) {
            //    ReportsSSRSDashboard.LoadProviderAutocomplete(PanelDiv + '  .tab-pane.active' + ' #txtAssigneeProviderOrderConsultation', PanelDiv + '  .tab-pane.active' + ' #hfAssigneeProviderOrderConsultation');
            //}

        }
        return true;
    },

    SelectReferralsTab: function (Type) {

        ReportsSSRSDashboard.ReferralType = Type;
        if (ReportsSSRSDashboard.ReferralType == "Incoming") {
            ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ReferralsIncoming);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listIncoming').addClass('active');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listOutgoing').removeClass('active');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .ClinicalCategory').find('option[value=1]').prop('selected', true);
        }
        else if (ReportsSSRSDashboard.ReferralType == "Outgoing") {
            ReportsSSRSDashboard.appendParamaters(ReportsSSRSDashboard.ReferralOutgoing);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listOutgoing').addClass('active');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listIncoming').removeClass('active');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters .ClinicalCategory').find('option[value=2]').prop('selected', true)
        }
        var RefProviderId = [];
        var RefProviderIdFrom = [];
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalToId').text("");
        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfReferalFromId').text("");
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').html() != '') {
            setTimeout(function () {
                $('#BackgroundLoader').show();
            }, 10)
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').append($('#' + ReportsSSRSDashboard.params["PanelID"] + ' .ActionsButtons').clone().show());
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').show();
            var objdef = jQuery.Deferred();
            var dataLoadzero = "IsActive=";
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadZero').loadDropDowns(true, dataLoadzero, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                var dataActive = "IsActive=1";
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadFirst').loadDropDowns(true, dataActive, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                    var dataLedgerAccount = "IsActive=&ID=" + -1 + "&ID2=" + -1;
                    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadThird').loadDropDowns(true, dataLedgerAccount, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                        var dataAllUsers = "IsActive=&ID=1";
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').find('.LoadSecond').loadDropDowns(true, dataAllUsers, ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').done(function () {
                            objdef.resolve();
                            objdef.promise();
                        });
                    });
                });
            });
            ReportsSSRSDashboard.BindPatientAccount();
            ReportsSSRSDashboard.LoadRefProvider();
            ReportsSSRSDashboard.LoadRefProviderFrom();
            objdef.then(function () {

                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters select').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true
                });

                var ReportsControlDiv = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
                for (var i = 0; i < ReportsControlDiv.find('.datepickerStart').length; i++) {
                    utility.ValidateFromToDate("frmSSRSReports #ReportParamaters", ReportsControlDiv.find('.datepickerStart')[i].id, ReportsControlDiv.find('.datepickerEnd')[i].id, true);
                }
                for (var i = 0; i < ReportsControlDiv.find('.datepickerMonthViewStart').length; i++) {
                    utility.ValidateMonthViewFromToDate("frmSSRSReports #ReportParamaters", ReportsControlDiv.find('.datepickerMonthViewStart')[i].id, ReportsControlDiv.find('.datepickerMonthViewEnd')[i].id, true);
                }

                ReportsControlDiv.find('.datepickerClaim').each(function (i, item) {
                    utility.CreateDatePicker('frmSSRSReports #ReportParamaters #' + $(item).attr('Id'),
                          function (ev) {
                              //on-change callback method 
                          }, false);
                });
                ReportsControlDiv.find('.datepicker').each(function (i, item) {
                    utility.CreateDatePicker('frmSSRSReports #ReportParamaters #' + $(item).attr('Id'),
                          function (ev) {
                              //on-change callback method 
                          }, false);
                });

                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #btn-print").hide();
                ReportsSSRSDashboard.ReportId = "Clinical Reports/Referrals";
                ReportsSSRSDashboard.ReportName = "Referrals";
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #treeBasic').find('a').attr('disabled', false).css("cursor", "");
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null); //Report load source null, Azhar siyal & Abdur Rehman
                //BackgroundLoaderShow(false);
                // }
                setTimeout(function () {
                    $('#BackgroundLoader').hide();
                }, 300);
            });
        } else {
            ReportsSSRSDashboard.ShowDefaultInformation(FormName);
        }

        // return true;
    },

    BindClaimNumber: function () {
        var Ctrl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #ClaimNumber');
        var func = function () { return ReportsSSRSDashboard.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func);
    },

    GetClaimNumberArray: function (ClaimNumber) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        ReportsSSRSDashboard.LoadClaimNumers(ClaimNumber).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber, PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                }
            }
            dfd.resolve(AllClaimsVisits);
        });
        return dfd.promise();
    },

    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },

    generateOrdersReport: function () {
        var myJSON = '';

        if (ReportsSSRSDashboard.OrderType == "Lab") {
            myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabOrderDiv').getMyJSONByName();

        }
        else if (ReportsSSRSDashboard.OrderType == "Radiology") {
            myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyOrder').getMyJSONByName();
        }
            // Start || 1 October, 2016 || Talha Tanweer || EMR-748
        else if (ReportsSSRSDashboard.OrderType == "Consultation") {
            myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationOrderDiv').getMyJSONByName();
            ReportsSSRSDashboard.ConsultationOrderLoad(myJSON);
            return;
        }
        else if (ReportsSSRSDashboard.OrderType == "Procedure") {
            myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProcedureOrderDiv').getMyJSONByName();
        }
            // End   || 1 October, 2016 || Talha Tanweer || EMR-748
            //var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();
        else if (ReportsSSRSDashboard.OrderType == "Prescription") {
            myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #PrescriptionOrderDiv').getMyJSONByName();
            ReportsSSRSDashboard.PrescriptionOrderLoad(myJSON);
            return;
        }
        ReportsSSRSDashboard.generateOrdersReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (ReportsSSRSDashboard.OrderType == "Procedure") {
                    ReportsSSRSDashboard.ProcedureOrdersLoad(response);
                }
                else {
                    ReportsSSRSDashboard.OrdersLoad(response);
                }

            }
            else {


                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    GetLabOrderTestsGrid: function (data) {
        var testsList = "";
        if (data.Test != "") {
            testsList += '<ul style="list-style-type:none">';
            var Tests = data.Test.split(';');
            if (Tests.length > 1) {
                $.each(Tests, function (i, item) {
                    if (i == (Tests.length - 1)) {
                        testsList += '<li>' + item + '</li>';
                    }
                    else {
                        testsList += '<li style="border-bottom: 1px solid #ddd;">' + item + '</li>';
                    }
                });
            }
            else if (Tests.length == 1) {
                testsList += '<li>' + Tests[0] + '</li>';
            }
            testsList += '</ul>';
        }
        return testsList;
    },
    generateOrdersReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["IsSummaryReport"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #IsSummaryReport').prop('checked');
        //objData["AccountNumber"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfAccNumber').val();
        //objData["ProviderId"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfProvider').val() == "" ? 0 : $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfProvider').val();
        objData["ProviderId"] = objData["ProviderId"] == "" ? 0 : objData["ProviderId"];
        if (ReportsSSRSDashboard.OrderType == "Procedure") {
            objData["AssigneeProviderId"] = objData["AssigneeProviderId"] == "" ? 0 : objData["AssigneeProviderId"];
        }
        objData["OrderType"] = ReportsSSRSDashboard.OrderType;
        objData["commandType"] = "LOAD_ORDER_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },

    OrdersLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #IsSummaryReport').prop('checked')) {
            var ColumnHeaders = [

                                    { field: "AccountNumber", title: "Account Number", width: "80px", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                                    { field: "PatientName", title: "Patient Name", width: "70px" },
                                    { field: "DOB", title: "DOB", width: "50px" },
                                    { field: "Provider", title: "Provider", width: "70px" },
                                    { field: "Tests", title: "Test", width: "100px", template: '#=ReportsSSRSDashboard.GetLabOrderTestsGrid(data)#' },
                                    { field: "Laboratory", title: "Laboratory", width: "80px" },
                                    { field: "OrderNo", title: "Order Number", width: "50px" },
                                    { field: "OrderStatus", title: "Status", width: "70px" },
                                    { field: "OrderDateTime", title: "Date & Time", width: "70px" },
                                    { field: "InsuranceName", title: "Insurance Name", width: "80px" }
            ];
            if (response.ordersCount > 0) {
                var ordersJSONData = JSON.parse(response.ordersList_JSON);

                var fieldRows = {
                    AccountNumber: { type: "string" },
                    PatientName: { type: "string" },
                    DOB: { type: "string" },
                    Provider: { type: "string" },
                    Test: { type: "string" },
                    Laboratory: { type: "string" },
                    OrderNo: { type: "string" },
                    OrderStatus: { type: "string" },
                    OrderDateTime: { type: "string" },
                    PatientId: { type: "number" },
                    InsuranceName: { type: "string" }
                };
                KenduReports.inializeKenduGrid(ordersJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
            } else {
                KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
            }
        }
        else {
            var ColumnHeaders = [

                                { field: "AccountNumber", title: "Account Number", width: "80px", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                                { field: "PatientName", title: "Patient Name", width: "70px" },
                                { field: "DOB", title: "DOB", width: "50px" },
                                { field: "PatStatus", title: "Pt. Status", width: "50px" },
                                { field: "Test", title: "Test", width: "70px" },
                                { field: "Laboratory", title: "Laboratory", width: "80px" },
                                { field: "OrderNo", title: "Order Number", width: "50px" },
                                { field: "OrderStatus", title: "Status", width: "70px" },
                                { field: "Provider", title: "Provider", width: "70px" },
                                { field: "OrderDateTime", title: "Date & Time", width: "70px" }
            ];
            if (response.ordersCount > 0) {
                var ordersJSONData = JSON.parse(response.ordersList_JSON);

                var fieldRows = {
                    AccountNumber: { type: "string" },
                    PatientName: { type: "string" },
                    DOB: { type: "string" },
                    PatStatus: { type: "string" },
                    Test: { type: "string" },
                    Laboratory: { type: "string" },
                    OrderNo: { type: "string" },
                    OrderStatus: { type: "string" },
                    Provider: { type: "string" },
                    OrderDateTime: { type: "string" },
                    PatientId: { type: "number" }
                };
                KenduReports.inializeKenduGrid(ordersJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
            } else {
                KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
            }
        }
    },


    // End || 1 October, 2016 || Talha Tanweer || EMR-748

    // Start || 1 October, 2016 || Talha Tanweer || EMR-748
    ConsultationOrderLoad: function (myJSON) {

        ReportsSSRSDashboard.generateConsultationOrdersReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                ReportsSSRSDashboard.ConsultationOrdersGridLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


    },


    generateConsultationOrdersReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        var hiddenfieldProviderOrderConsultation = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfAssigneeProviderOrderConsultation')[1];

        if ($(hiddenfieldProviderOrderConsultation).length > 0 && $(hiddenfieldProviderOrderConsultation).val() != "") {

            objData["AssigneeProviderId"] = $(hiddenfieldProviderOrderConsultation).val();
        }
        else {
            objData["AssigneeProviderId"] = 0;
        }


        //objData["AccountNumber"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfAccNumber').val();
        objData["ProviderId"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfProvider').val() == "" ? 0 : $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfProvider').val();
        //   objData["AssigneeProviderId"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfAssigneeProvider_OrderConsultation').val() == "" ? 0 : $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfAssigneeProvider_OrderConsultation').val();
        //  objData["AssigneeProviderId"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #txtAssigneeProvider')
        objData["OrderType"] = ReportsSSRSDashboard.OrderType;
        objData["Procedure"] = objData["CPTCode"];
        objData["commandType"] = "LOAD_CONSULTATION_ORDER_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },

    ConsultationOrdersGridLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [

                            { field: "AccountNumber", title: "Account Number", width: 80, template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                            { field: "PatientName", title: "Patient Name", width: 80 },
                            { field: "DOB", title: "DOB", width: 50 },
                            { field: "PatStatus", title: "Pt. Status", width: 70 },
                            { field: "Procedure", title: "Procedure", width: 70 },
                            { field: "Laboratory", title: "Laboratory", width: 70 },
                            { field: "OrderNo", title: "Order Number", width: 70 },
                            { field: "OrderStatus", title: "Status", width: 70 },
                            { field: "Provider", title: "Provider", width: "auto" },
                            { field: "AssingneeProvider", title: "AssigneeProvider", width: "auto" },
                            { field: "OrderDateTime", title: "Date & Time", width: "auto" }
        ];
        if (response.ordersCount > 0) {
            var ordersJSONData = JSON.parse(response.ordersList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Procedures: { type: "string" },
                Laboratory: { type: "string" },
                OrderNo: { type: "string" },
                OrderStatus: { type: "string" },
                Provider: { type: "string" },
                AssigneeProvider: { type: "string" },
                OrderDateTime: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(ordersJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },


    // End || 1 October, 2016 || Talha Tanweer || EMR-748



    // -----------------Results---------------
    generateResultsReport: function () {
        var PanelDiv = '#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters';
        if (ReportsSSRSDashboard.ResultType == "Lab") {
            PanelDiv += " .LabResultTab";
        }
        else if (ReportsSSRSDashboard.ResultType == "Radiology") {
            PanelDiv += " .RadiologyResultTab";
        }
        else if (ReportsSSRSDashboard.ResultType == "Procedure") {
            PanelDiv += " .ProcedureResultTab";
        }
        else if (ReportsSSRSDashboard.ResultType == "Consultation") {
            PanelDiv += " .ConsultationResultTab";
        }

        var myJSON = $(PanelDiv).getMyJSONByName();

        ReportsSSRSDashboard.generateResultsReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.ResultsLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    generateResultsReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["ProviderId"] = objData["ProviderId"] == "" ? 0 : objData["ProviderId"];
        objData["AssigneeProvider"] = objData["AssigneeProvider"] == "" ? 0 : objData["AssigneeProvider"];
        objData["ResultType"] = ReportsSSRSDashboard.ResultType;
        objData["commandType"] = "LOAD_RESULT_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },

    ResultsLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();

        if (response.resultsCount > 0) {
            var resultsJSONData = JSON.parse(response.resultsList_JSON);
            var ColumnHeaders = [

                                    { field: "AccountNumber", title: "Account Number", width: 70, template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                                    { field: "PatientName", title: "Patient Name", width: 80 },
                                    { field: "DOB", title: "DOB", width: 50 },
                                    { field: "PatStatus", title: "Pt. Status", width: 60 },
                                    { field: "Test", title: "Test", width: 100 },
                                    { field: "Laboratory", title: "Laboratory", width: 80 },
                                    { field: "ResultNo", title: "Result Number", width: 70 },
                                    { field: "ResultStatus", title: "Status", width: 80 },
                                    { field: "Provider", title: "Provider", width: 70 },
                                    { field: "ObservationDate", title: "Date & Time", width: 80 }
            ];
            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Test: { type: "string" },
                Laboratory: { type: "string" },
                ResultNo: { type: "string" },
                ResultStatus: { type: "string" },
                Provider: { type: "string" },
                ObservationDate: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(resultsJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], [], {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },
    //-------------------results end----------------------------
    generateProceduresReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();

        ReportsSSRSDashboard.generateProceduresReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                ReportsSSRSDashboard.ProceduresLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    generateProceduresReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["ProviderId"] = objData["ProviderId"] == "" ? 0 : objData["ProviderId"];//$('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfProvider').val() == "" ? 0 : $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #hfProvider').val();
        objData["commandType"] = "LOAD_PROCEDURES_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },


    ProceduresLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();

        if (response.proceduresCount > 0) {
            var proceduresJSONData = JSON.parse(response.proceduresList_JSON);
            var ColumnHeaders = [

                                    { field: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                                    { field: "PatientName", title: "Patient Name", width: "auto" },
                                    { field: "DOB", title: "DOB", width: "auto" },
                                    { field: "PatStatus", title: "Pt. Status", width: "auto" },
                                    { field: "Procedure", title: "Procedure", width: "auto" },
                                    { field: "ICD", title: "ICD (Diagnosis)", width: "auto" },
                                    { field: "Provider", title: "Provider", width: "auto" },
                                    { field: "StartDate", title: "Start Date", width: "auto" },
                                    { field: "EndDate", title: "End Date", width: "auto" }

            ];
            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Procedure: { type: "string" },
                ICD: { type: "string" },
                Provider: { type: "string" },
                StartDate: { type: "string" },
                EndDate: { type: "string" },
                PatientId: { type: "number" }

            };
            KenduReports.inializeKenduGrid(proceduresJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], [], {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },

    bindAutoCompleteProcedure: function (element, refCtrlId) {
        var hiddenCrtl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        utility.BindCPTCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, ReportsSSRSDashboard.params.TabID, hiddenCrtl, true);
    },
    bindAutoCompleteCPTProcedure: function (element, refCtrlId) {
        var hiddenCrtl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCodeProcedure'));
        utility.BindCPTCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, ReportsSSRSDashboard.params.TabID, hiddenCrtl, true);
    },
    bindLoincAutoComplete: function (element, refCtrlId) {

        var hiddenCrtl = $(element);
        //  utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_LabOrder", null, true);
        EMRUtility.BindLOINCCodes(hiddenCrtl, "ReportsSSRSDashboard", null, '', '1');
    },
    /////////////
    generateProgressNoteReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();

        ReportsSSRSDashboard.generateProgressNoteReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.ProgressNoteLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ProgressNoteLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ProgressNoteView").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ProgressNoteView #tblProgressNote tbody").find("tr").remove();
        if (response.progressNoteCount > 0) {
            var progressNoteJSONData = JSON.parse(response.progressNoteList_JSON);
            $.each(progressNoteJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'))");
                $row.attr("id", "gvNotes_row" + item.NotesId);
                $row.attr("NotesId", item.NotesId);
                var AmendmentData = "";
                var AmendmentNotesOpen = "ReportsSSRSDashboard.AmendmentNotesOpen(" + item.NotesId + ");"
                if ($('#' + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #chkAmendedNote").prop("checked") == true) {
                    AmendmentData = '<a title="Amendment" href="javascript:void(0);" onclick="' + AmendmentNotesOpen + '">Details</a>';
                }
                var NotesPreview = "Clinical_Notes.NotesPreview('" + item.NotesId + "','mstrTabReports','" + item.PatientId + "','" + item.ProviderId + "');";
                var PatientPreview = "ReportsSSRSDashboard.PatientDemographics('" + item.PatientId + "',event);";
                $row.append('<td><a title="View Note" href="javascript:void(0);" onclick="' + NotesPreview + '"> ' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</a></td><td>' +
                    '<a title="View Patient" href="javascript:void(0);" onclick="' + PatientPreview + '"> ' + item.AccountNumber +
                    '</a></td><td>' + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td>'
                    + item.HomePhone + '</td>' +
                    '<td>' + item.NotesStatus + '</td>' +
                    '<td>' + item.NotesType + '</td>' +
                    '<td>' + item.PracticeName + '</td>' +
                    '<td>' + item.FacilityName + '</td>' +
                    '<td>' + item.ProviderName + '</td><td>' + item.RefProviderName + '</td>' +
                    '<td>' + AmendmentData + '</td>');
                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #tblProgressNote tbody").last().append($row);
            });
        } else {
            var $row = $('<tr/>');
            $row.attr("class", 'text-center');
            $row.append('<td colspan="11">No Record Found</td>')
            $('#' + ReportsSSRSDashboard.params["PanelID"] + " #tblProgressNote tbody").last().append($row);
        }
    },
    AmendmentNotesOpen: function (NotesId) {
        var params = [];
        params["ParentCtrl"] = "mstrTabReports";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["NotesId"] = NotesId;
        LoadActionPan("Clinical_NotesAmendment", params);
    },
    AmendedNoteChange: function (obj) {
        if ($(obj).prop("checked") == true) {
            if (!$('#' + ReportsSSRSDashboard.params["PanelID"] + " #ddlNoteStatus").parent().find("button").hasClass('disableAll')) {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ddlNoteStatus").parent().find("button").addClass('disableAll');
            }
            $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters  #ddlNoteStatus option[value='2']").prop('selected', true);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ddlNoteStatus').multiselect('rebuild');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ddlNoteStatus').find('select').multiselect('refresh');
        }
        else {
            if ($('#' + ReportsSSRSDashboard.params["PanelID"] + " #ddlNoteStatus").parent().find("button").hasClass('disableAll')) {
                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ddlNoteStatus").parent().find("button").removeClass('disableAll');
            }
            $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters  #ddlNoteStatus option[value='']").prop('selected', true);
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ddlNoteStatus').multiselect('rebuild');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ddlNoteStatus').find('select').multiselect('refresh');
        }
    },
    generateProgressNoteReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["ProviderIds_text"] = "";
        objData["ProviderIds_RefValue"] = "";
        objData["PracticeIds_RefValue"] = "";
        objData["PracticeIds_text"] = "";
        objData["FacilityIds_RefValue"] = "";
        objData["FacilityIds_text"] = "";
        objData["NoteStatus"] = objData["NoteStatus_text"] == "All" ? "" : objData["NoteStatus_text"];
        objData["NoteType"] = (objData.NoteType == "" || objData.NoteType == null) ? -1 : objData.NoteType;
        objData["commandType"] = "LOAD_PROGRESSNOTE_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },

    generatePhoneEncounterReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();

        ReportsSSRSDashboard.generatePhoneEncounterReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.PhoneEncounterLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    generatePhoneEncounterReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["ProviderIds_text"] = "";
        objData["ProviderIds_RefValue"] = "";
        objData["PracticeIds_RefValue"] = "";
        objData["PracticeIds_text"] = "";
        objData["NotesDuration"] = objData["NotesDuration"] != '' ? objData["NotesDuration_text"] : "";
        objData["NotesDuration_text"] = "";
        objData["NoteStatus"] = objData["NoteStatus_text"] == "All" ? "" : objData["NoteStatus_text"];
        objData["commandType"] = "LOAD_PHONEENCOUNTER_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },

    isNumberKey: function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode < 48 || charCode > 57) {
            return false;
        } else {
            return true;
        }
    },

    UpdateCounters: function (duration, charges) {
        var strTotalMinutes = ReportsSSRSDashboard.getCallDuration(duration);
        var Minutes = ReportsSSRSDashboard.secondsTimeSpanToHMS(strTotalMinutes);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #lblTotalCalls").text(1);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #lblTotalMinutes").text(Minutes);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #lblTotalCharges").text(charges);

    },


    PhoneEncounterLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #tblPhoneEncounter tbody").find("tr").remove();
        var TotalCalls = 0, TotalMinutes = 0, TotalCharges = 0;
        if (response.phoneEncounterCount > 0) {
            var phoneEncounterJSONData = JSON.parse(response.phoneEncounterList_JSON);
            TotalCalls = response.phoneEncounterCount;
            $.each(phoneEncounterJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'));ReportsSSRSDashboard.UpdateCounters('" + item.NotesDuration + "'," + item.EncounterCharge + ");");
                $row.attr("id", "gvNotes_row" + item.NotesId);
                $row.attr("NotesId", item.NotesId);

                var NotesPreview = "Clinical_Notes.NotesPreview('" + item.NotesId + "','mstrTabReports','" + item.PatientId + "','" + item.ProviderId + "');"
                var PatientPreview = "ReportsSSRSDashboard.PatientDemographics('" + item.PatientId + "',event);";
                $row.append('<td><a title="View Note" href="javascript:void(0);" onclick="' + NotesPreview + '"> ' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</a></td><td>' +
                    '<a title="View Patient" href="javascript:void(0);" onclick="' + PatientPreview + '"> ' + item.AccountNumber +
                    '</a></td><td>' + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td>'
                    + item.HomePhone + '</td>' +
                    '<td>' + item.NotesStatus + '</td>' +
                    '<td>' + item.NotesDuration + '</td>' +
                    '<td>' + item.PracticeName + '</td>' +
                    '<td>' + item.ProviderName + '</td><td>' + item.RefProviderName + '</td>');
                TotalCharges += item.EncounterCharge;
                TotalMinutes += ReportsSSRSDashboard.getCallDuration(item.NotesDuration);
                $('#' + ReportsSSRSDashboard.params["PanelID"] + " #tblPhoneEncounter tbody").last().append($row);
            });

        } else {
            var $row = $('<tr/>');
            $row.attr("class", 'text-center');
            $row.append('<td colspan="10">No Record Found</td>')
            $('#' + ReportsSSRSDashboard.params["PanelID"] + " #tblPhoneEncounter tbody").last().append($row);
        }
        var Minutes = ReportsSSRSDashboard.secondsTimeSpanToHMS(TotalMinutes);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #lblTotalCalls").text(TotalCalls);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #lblTotalMinutes").text(Minutes);
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #PhoneEncounterView #lblTotalCharges").text(TotalCharges);
    },
    getCallDuration: function (callDuration) {
        var TotalDuration = 0;
        var Hours = ((isNaN(parseInt(callDuration.split(':')[0])) ? 0 : parseInt(callDuration.split(':')[0])) * 60 * 60);
        var Minutes = ((isNaN(parseInt(callDuration.split(':')[1])) ? 0 : parseInt(callDuration.split(':')[1])) * 60);
        var Seconds = (isNaN(parseInt(callDuration.split(':')[2])) ? 0 : parseInt(callDuration.split(':')[2]));

        TotalDuration = Hours + Minutes + Seconds;
        //RecordDuration = TotalDuration / 60;
        return TotalDuration;
        //if (callDuration == null) {
        //    return 0;
        //} else {
        //    if (callDuration.indexOf("30") > -1) {
        //        return 30;
        //    } else if (callDuration.indexOf("20") > -1) {
        //        return 20;
        //    } else if (callDuration.indexOf("10") > -1) {
        //        return 10;
        //    } else {
        //        return 30;
        //    }
        //}
    },

    secondsTimeSpanToHMS: function (s) {
        var h = Math.floor(s / 3600); //Get whole hours
        s -= h * 3600;
        var m = Math.floor(s / 60); //Get remaining minutes
        s -= m * 60;
        return h + ":" + (m < 10 ? '0' + m : m) + ":" + (s < 10 ? '0' + s : s);
    },
    PatientDemographics: function (patientid, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "mstrTabReports";
                LoadActionPan('demographicDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    //***********************************************************************
    //****************** END CLINICAL Report Functions *****************
    //*********************************************************************** 

    //Supportive functions clinical
    fillAllergiesDropdown: function () {
        ReportsSSRSDashboard.fillAllergiesDropdown_Dbcall().done(function (response) {

            var response = JSON.parse(response);
            if (response.AllergiesCount > 0) {
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Allergy').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #AllergyAND').prop("checked", true);
                $.each(JSON.parse(response.AllergiesList), function (i, item) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #Allergy').append(
                        $('<option/>', {
                            value: item.AllergyId,
                            html: item.Allergen,
                        })
                    );
                });

                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Allergy').multiselect('destroy');

                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Allergy').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #Allergy').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },

                });
            }
        });
    },

    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {
            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                Clinical_ProblemLists.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                Clinical_ProblemLists.LastSctBaseSearch = "";
            }
        }
        else {
            Clinical_ProblemLists.LastSctBaseSearch = "";
        }

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_ProblemLists", null, false);
    },
    fillReactionsDropdown: function () {
        ReportsSSRSDashboard.fillReactionsDropdown_Dbcall().done(function (response) {

            var response = JSON.parse(response);
            if (response.ReactionCount > 0) {
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #AllergyReaction').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #AllergyReaction').append('<option value="" id="All" selected>- Select -</option>');
                $.each(JSON.parse(response.ReactionList), function (i, item) {
                    if (item.Reaction != '' && item.Reaction != null) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #AllergyReaction').append(
                            $('<option/>', {
                                value: item.ReactionId,
                                html: item.Reaction,
                            })
                        );
                    }
                });
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #AllergyReaction').multiselect('destroy');
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AllergyReaction').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #AllergyReaction').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },
                });
            }
        });
    },


    fillAllergiesDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "FILL_ALLERGIES_DROPDOWN_FORREPORTS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },

    fillReactionsDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "FILL_REACTIONS_DROPDOWN_FORREPORTS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },

    fillMedicationDropdown: function () {
        ReportsSSRSDashboard.fillMedicationDropdown_Dbcall().done(function (response) {

            var response = JSON.parse(response);
            if (response.medicationCount > 0) {
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Medication').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #MedicationAND').prop("checked", true);
                $.each(JSON.parse(response.medicationList_JSON), function (i, item) {
                    if (item.MedicationName != '' && item.MedicationName != null) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #Medication').append(
                            $('<option/>', {
                                value: item.MedicationID,
                                html: item.MedicationName,
                            })
                        );
                    }
                });


                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Medication').multiselect('destroy');

                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Medication').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #Medication').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },

                });
            }
        });
    },
    fillMedicationDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "LOOKUP_MEDICATIONS_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },
    //-----------------------------------------------------------------------------------
    //------------------------------ Start Problems Report ------------------------------
    //-----------------------------------------------------------------------------------
    fillProblemsDropdown: function (Ctrl, hfCtrl) {
        var valid = false;
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.description && obj.description.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.description;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.description);
            $(hfCtrl).val(dataItem.id);
            $("#" + ReportsSSRSDashboard.params.PanelID + " #ICD10Description").val("");
            $("#" + ReportsSSRSDashboard.params.PanelID + " #ICD10Code").val("")
        }
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 2,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        ReportsSSRSDashboard.GetProblemsArray(Ctrl.val()).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });


    },

    GetProblemsArray: function (ShortName) {
        var allProblems = [];
        var dfd = new $.Deferred();
        if (ShortName.length > 2) {
            Batch_PatientList.loadProblems_DBCall(ShortName).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ProblemCount > 0) {
                        var Problems = JSON.parse(response.ProblemLoad_JSON);

                        $.each(Problems, function (i, item) {
                            var completeDescription = (item.Code) ? (item.Code + " - " + item.Description) : (item.Description)
                            allProblems.push({ id: item.ProblemListId, value: completeDescription, description: item.Description });
                        });
                    }
                }

                dfd.resolve(allProblems);
            });
        }
        else {
            dfd.resolve(allProblems);
        }

        return dfd.promise();

    },


    //-----------------------------------------------------------------------------------
    //------------------------------ Start Procedures Report ------------------------------
    //-----------------------------------------------------------------------------------
    fillProceduresDropdown: function (cntrl, hdnField) {
        var valid = false;

        var Ctrl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters input#txtCPTCodeProcedure');
        var hfCtrl = "";
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.description && obj.description.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.description;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.description);
            $(hfCtrl).val(dataItem.id);
        }
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 2,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        ReportsSSRSDashboard.GetProceduresArray(Ctrl.val()).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },

    GetProceduresArray: function (ShortName) {
        var allProcedures = [];
        var dfd = new $.Deferred();
        if (ShortName.length > 2) {
            entityID = globalAppdata["SeletedEntityId"];
            let isMDVision = true;
            var data = "text=" + ShortName + "&entityID=" + entityID + "&iscode=CPT&isMDVision=" + isMDVision;
            var LocalOrIMO = "";
            MDVisionService.defaultService(data, "COMMON_IMO_CODE", "GET_IMO_CPTCODE").done(function (result) {

                $.each(result, function (j, item) {
                    item.Name = utility.decodeHtml(item.Name);
                    if (item.Name.toLowerCase() != "- select -") {

                        var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                        var _ConcatinatedString = item.Name;

                        // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                        if (_ConcatinatedString.indexOf("!") >= 0) {

                            LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                            CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                            ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                            SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                            _LexiCode = LexiCode;

                            _CPT = CPT.split("+")[0];
                            _CPTDescription = CPT.split("+")[1];
                            _ICD = ICD.split("+")[0];
                            _ICDDescription = ICD.split("+")[1];
                            _SNOMED = SNOMED.split("+")[0];
                            _SNOMEDDescription = SNOMED.split("+")[1];
                        }
                        else {
                            CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                            _CPT = CPT.split("-")[0].trim();
                            _CPTDescription = CPT.split("-")[1].trim();
                        }

                        var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                        // In IMO case Build Four Column Header, Else Two Columns

                        _CPTDescription = _CPTDescription.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');

                        var duMulti = _LexiCode + "*" + CPT + "$" + ICD + "~" + SNOMED;
                        _ICD = _ICD != "" ? _ICD : "NoICD";
                        _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                        allProcedures.push({ id: item.Value, value: _CPT + " - " + _CPTDescription, description: _CPTDescription });

                    }
                })
                dfd.resolve(allProcedures);
            })
        }
        else {
            dfd.resolve(allProcedures);
        }

        return dfd.promise();

    },

    //Start || 30 August, 2016 || Talha Tanweer || fillProblemsDropdown


    //Start || 30 August, 2016 || Talha Tanweer || fillProblemsDropdown_Dbcall
    fillProblemsDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "FILL_PROBLEMS_DROPDOWN_FORREPORTS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },



    //Start || 31 August, 2016 || Talha Tanweer || 
    generateProblemsReport: function () {
        var myJSON = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters').getMyJSONByName();

        ReportsSSRSDashboard.generateProblemsReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                ReportsSSRSDashboard.ProblemsLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Start || 31 August, 2016 || Talha Tanweer || 
    ProblemsLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();

        var ColumnHeaders = [
                              {
                                  title: "AccountNumber", title: "Account Number", width: 70,
                                  template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>'
                              },
                                          // { field: "AccountNumber", title: "Account Number", width: "auto" },
                                  { field: "PatientName", title: "Patient Name", width: 80 },
                                  { field: "DOB", title: "DOB", width: 80 },
                                  { field: "PatStatus", title: "Pt. Status", width: 50 },
                                  { field: "Problem", title: "Problem", width: 100 },
                                  { field: "ProblemStatus", title: "Problem Status", width: 50 },
                                  { field: "ChronicityLevel", title: "Chronicity Level", width: 80 },
                                  { field: "Severity", title: "Severity", width: 80 },
                                  { field: "StartDate", title: "StartDate", width: 80 },
                                  { field: "EndDate", title: "End Date", width: 80 }

        ];

        if (response.ProblemsCount > 0) {
            var ProblemsJSONData = JSON.parse(response.ProblemsList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Problem: { type: "string" },
                ProblemStatus: { type: "string" },
                ChronicityLevel: { type: "string" },
                Severity: { type: "string" },
                StartDate: { type: "string" },
                EndDate: { type: "string" }
            };
            //   ReportsSSRSDashboard.inializeKenduGrid(ProblemsJSONData, ColumnHeaders, fieldRows);

            KenduReports.inializeKenduGrid(ProblemsJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");


        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },

    //Start || 31 August, 2016 || Talha Tanweer || 
    generateProblemsReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        //objData["Allergy_text"] = "";
        //objData["Allergy_RefValue"] = "";
        //objData["AllergyReaction_text"] = "";
        //objData["AllergyStatus_text"] = "";

        objData["StartDate"] = (objData["StartDate"] == "") ? "1/1/0001 12:00:00 AM" : objData["StartDate"];
        objData["EndDate"] = (objData["EndDate"] == "") ? "1/1/0001 12:00:00 AM" : objData["EndDate"];
        objData["Problem"] = objData["ProblemName"];
        objData["commandType"] = "LOAD_Problems_REPORT";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },


    //-----------------------------------------------------------------------------------
    //------------------------------ End   Problems Report ------------------------------
    //-----------------------------------------------------------------------------------




    fillImmunizationDropdown: function () {
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').multiselect('disable');

        ReportsSSRSDashboard.fillImmunizationDropdown_Dbcall().done(function (response) {

            var self = "";
            if (ReportsSSRSDashboard.params["PanelID"] != "pnlReportsSSRSDashboard") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + " #pnlReportsSSRSDashboard")
            }
            else
                self = $('#' + ReportsSSRSDashboard.params["PanelID"]);



            $('#' + ReportsSSRSDashboard.params.PanelID + ' #AdministeredBy').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #AdministeredBy').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #AdministeredBy').append('<option value="" id="All" selected>- Select -</option>');
            $.each(self.find('#EnteredBy option'), function (i, item) {

                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #AdministeredBy').append(item);
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #AdministeredBy').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AdministeredBy').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #AdministeredBy').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });





            var response = JSON.parse(response);




            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VaccineCategory').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VaccineCategory').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VaccineCategory').append('<option value="" id="All" selected>- Select -</option>');
            $.each(JSON.parse(response.CategoryList), function (i, item) {
                if (item.Name != '' && item.Name != null) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #VaccineCategory').append(
                        $('<option/>', {
                            value: item.Id,
                            html: item.Name,
                        })
                    );
                }
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #VaccineCategory').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #VaccineCategory').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #VaccineCategory').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });
            //$('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').empty();
            //$('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').empty();
            //$('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').append('<option value="" id="All" selected>- Select -</option>');
            //$.each(JSON.parse(response.), function (i, item) {
            //    if (item.Name != '' && item.Name != null) {
            //        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').append(
            //            $('<option/>', {
            //                value: item.Id,
            //                html: item.Name,
            //            })
            //        );
            //    }
            //});


            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Route').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Route').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Route').append('<option value="" id="All" selected>- Select -</option>');
            $.each(JSON.parse(response.RouteList), function (i, item) {
                if (item.Name != '' && item.Name != null) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Route').append(
                        $('<option/>', {
                            value: item.Id,
                            html: item.Name,
                        })
                    );
                }
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Route').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Route').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #Route').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });



            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Site').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Site').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Site').append('<option value="" id="All" selected>- Select -</option>');
            $.each(JSON.parse(response.SiteList), function (i, item) {
                if (item.Name != '' && item.Name != null) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Site').append(
                        $('<option/>', {
                            value: item.Id,
                            html: item.Name,
                        })
                    );
                }
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #Site').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Site').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #Site').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });



            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationReaction').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationReaction').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationReaction').append('<option value="" id="All" selected>- Select -</option>');
            $.each(JSON.parse(response.ReactionList), function (i, item) {
                if (item.Name != '' && item.Name != null) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #ImmunizationReaction').append(
                        $('<option/>', {
                            value: item.Id,
                            html: item.Name,
                        })
                    );
                }
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationReaction').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ImmunizationReaction').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationReaction').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });




            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationAlert').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationAlert').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationAlert').append('<option value="" id="All" selected>- Select -</option>');
            $.each(JSON.parse(response.AlertsList), function (i, item) {
                if (item.Name != '' && item.Name != null) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #ImmunizationAlert').append(
                        $('<option/>', {
                            value: item.Id,
                            html: item.Name,
                        })
                    );
                }
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationAlert').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ImmunizationAlert').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #ImmunizationAlert').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });

        });
    },
    fillImmunizationDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "FILL_IMMUNIZATION_DROPDOWN_FORREPORTS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    VaccineCategoryChange: function (categoryId) {
        if (categoryId != "") {
            ReportsSSRSDashboard.fillVaccineDropdown_Dbcall(categoryId).done(function (response) {
                response = JSON.parse(response);
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').multiselect('enable');
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').append('<option value="" id="All" selected>- Select -</option>');
                var selectedId = "";
                $.each(JSON.parse(response.VaccineList), function (i, item) {
                    if (i == 0) {
                        selectedId = item.Id;
                    }

                    if (item.Name != '' && item.Name != null) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').append(
                            $('<option/>', {
                                value: item.Id,
                                html: item.Name,
                            })
                        );
                    }
                });
                //select default Vaccine

                $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').multiselect('destroy');
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Vaccine').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },
                });

                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Vaccine').val(selectedId)
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Vaccine').multiselect("refresh");
                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Vaccine').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },
                });

            });
        }
        else {
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').empty();
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').append('<option value="" id="All" selected>- Select -</option>');

            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').multiselect('destroy');
            $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #Vaccine').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $('#' + ReportsSSRSDashboard.params.PanelID + ' #Vaccine').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });
            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #Vaccine').multiselect('disable');
        }

    },
    fillVaccineDropdown_Dbcall: function (categoryId) {
        var objData = new Object();
        objData["CategoryID"] = categoryId;
        objData["commandType"] = "FILL_VACCINE_DROPDOWN_FORREPORTS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },

    ProcedureOrdersLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [

                                { field: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                                { field: "PatientName", title: "Patient Name", width: "auto" },
                                { field: "DOB", title: "DOB", width: "auto" },
                                { field: "PatStatus", title: "Pt. Status", width: "auto" },
                                { field: "Procedure", title: "Procedure", width: "auto" },
                                { field: "OrderNo", title: "Order Number", width: "auto" },
                                { field: "OrderStatus", title: "Status", width: "auto" },
                                { field: "Provider", title: "Provider", width: "auto" },
                                { field: "AssingneeProvider", title: "Assignee Provider", width: "auto" },
                                { field: "OrderDateTime", title: "Date & Time", width: "auto" }
        ];
        if (response.ordersCount > 0) {
            var ordersJSONData = JSON.parse(response.ordersList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Procedure: { type: "string" },
                OrderNo: { type: "string" },
                OrderStatus: { type: "string" },
                Provider: { type: "string" },
                AssingneeProvider: { type: "string" },
                OrderDateTime: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(ordersJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },


    LoadLabs: function (ddlId, controlPanelID) {

        return Clinical_Lab.searchLab(null, 0, 1, 5000).done(function (response) {
            //Populate Distinct Values in typeArray
            response = JSON.parse(response);
            var NameArray = new Array();
            var labIds = new Array();
            var data = JSON.parse(response.ClinicalLab_JSON);
            $.each(data, function (i, row) {
                if (jQuery.inArray(row.Name, NameArray) === -1) {
                    NameArray.push(row.Name);
                    labIds.push(row.LabId);
                }
            });
            var ddType = $('#' + controlPanelID + " #" + ddlId);
            ddType.empty();
            ddType.append($("<option />").val("").text('-Select-'));
            if (NameArray.length > 0) {
                $.each(NameArray, function (index, Name) {
                    ddType.append($("<option />").val(labIds[index]).text(Name));
                });
            }
        });
    },

    PrescriptionOrderLoad: function (myJSON) {
        ReportsSSRSDashboard.generatePrescriptionReport_DbCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReportsSSRSDashboard.PrescriptionLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    generatePrescriptionReport_DbCall: function (searchData) {
        var objData = JSON.parse(searchData);
        objData["ProviderId"] = objData["ProviderId"] == "" ? 0 : objData["ProviderId"];
        objData["MedicationAND"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters [name=MedicationAND]:checked').val() == "1" ? true : false;
        objData["commandType"] = "LOAD_PRESCRIPTIONORDER_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ClinicalReports");
    },

    PrescriptionLoad: function (response) {
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #btn-downloadpdf").show();
        $('#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid").show();
        var ColumnHeaders = [

                                { field: "AccountNumber", title: "Account Number", width: "auto", template: '<a href="javascript:void(0);" title="View Patient"  onClick="OpenPatientDemographics(#= PatientId#)"> #=AccountNumber#</a>' },
                                { field: "PatientName", title: "Patient Name", width: "auto" },
                                { field: "DOB", title: "DOB", width: "auto" },
                                { field: "PatStatus", title: "Pt. Status", width: "auto" },
                                { field: "Medication", title: "Medication", width: "auto" },
                                { field: "PrescriptionStatus", title: "Status", width: "auto" },
                                { field: "Pharmacy", title: "Pharmacy", width: "auto" },
                                { field: "Refill", title: "Refill(s)", width: "auto" },
                                { field: "Provider", title: "Provider", width: "auto" },
                                { field: "PrescribedOn", title: "Prescribed On", width: "auto" }
        ];
        if (response.ordersCount > 0) {
            var ordersJSONData = JSON.parse(response.ordersList_JSON);

            var fieldRows = {
                AccountNumber: { type: "string" },
                PatientName: { type: "string" },
                DOB: { type: "string" },
                PatStatus: { type: "string" },
                Medication: { type: "string" },
                PrescriptionStatus: { type: "string" },
                Pharmacy: { type: "string" },
                Refill: { type: "string" },
                Provider: { type: "string" },
                PrescribedOn: { type: "string" },
                PatientId: { type: "number" }
            };
            KenduReports.inializeKenduGrid(ordersJSONData, ColumnHeaders, fieldRows, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        } else {
            KenduReports.inializeKenduGrid([], ColumnHeaders, {}, '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid");
        }
    },

    LoadPharmacyAutocomplete: function (cntrl, hfPharmacy) {
        $(cntrl).html('');
        CacheManager.BindCodes('GetPharmacy', false).done(function (result) {
            utility.BindKendoAutoComplete($(cntrl), 1, "value", "contains", Pharmacy, null, $(hfPharmacy));
        });
    },

    fillChronicityDropdown: function () {
        ReportsSSRSDashboard.fillChronicityDropdown_Dbcall().done(function (response) {

            var response = JSON.parse(response);
            if (response.ChronicityCount > 0) {
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemChronicity').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemChronicity').append('<option value="" id="All" selected>- Select -</option>');
                $.each(JSON.parse(response.ChronicityList), function (i, item) {
                    if (item.ShortName != '' && item.ShortName != null) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemChronicity').append(
                            $('<option/>', {
                                value: item.ChronicityLevelId,
                                html: item.ShortName,
                            })
                        );
                    }
                });


                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemChronicity').multiselect('destroy');

                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProblemChronicity').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemChronicity').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },

                });
            }
        });
    },
    fillChronicityDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "fill_chronicity_dropdown_forreports";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    fillSeverityDropdown: function () {
        ReportsSSRSDashboard.fillSeverityDropdown_Dbcall().done(function (response) {

            var response = JSON.parse(response);
            if (response.severityCount > 0) {
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemSeverity').empty();
                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemSeverity').append('<option value="" id="All" selected>- Select -</option>');
                $.each(JSON.parse(response.SeverityList), function (i, item) {
                    if (item.ShortName != '' && item.ShortName != null) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemSeverity').append(
                            $('<option/>', {
                                value: item.SeverityTypeId,
                                html: item.ShortName,
                            })
                        );
                    }
                });


                $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemSeverity').multiselect('destroy');

                $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProblemSeverity').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ProblemSeverity').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },

                });
            }
        });
    },

    fillSeverityDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "fill_severity_dropdown_forreports";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    BindLabTestText: function (PanelID, control, hfField) {
        var Ctrl = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtLabTest');
        var func = function () { return ReportsSSRSDashboard.GetLabTestTextArray(Ctrl.val()) };
        var hfCtrl = $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #LabTest");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },

    GetLabTestTextArray: function (ShortName) {
        var allLabTests = [];
        var dfd = new $.Deferred();
        if (ShortName != null && ShortName.length > 2) {
            Clinical_LabOrderView.loadLabTests_DBCall(ShortName).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.labTestCount > 0) {
                        var LabTests = JSON.parse(response.labTestList_JSON);
                        $.each(LabTests, function (i, item) {
                            allLabTests.push({ id: item.LOINC, value: item.LOINCDescription, });

                        });
                    }
                }

                dfd.resolve(allLabTests);
            });
        }
        else {
            dfd.resolve(allLabTests);
        }

        return dfd.promise();

    },

    BindRadiologyLabTestText: function () {
        var Ctrl = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtRadLabTest');
        var func = function () { return ReportsSSRSDashboard.GetRadiologyLabTestTextArray(Ctrl.val()) };
        var hfCtrl = $('#' + ReportsSSRSDashboard.params["PanelID"] + " #ReportParamaters #LabTest");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },

    GetRadiologyLabTestTextArray: function (ShortName) {
        var allRadiologyLabTests = [];
        var dfd = new $.Deferred();
        if (ShortName != null && ShortName.length > 2) {
            Clinical_RadiologyOrderView.loadRadiologyLabTests_DBCall(ShortName).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.radiologyTestCount > 0) {
                        var RadiologyLabTests = JSON.parse(response.radiologyTestList_JSON);

                        $.each(RadiologyLabTests, function (i, item) {
                            allRadiologyLabTests.push({ id: item.LOINC, value: item.LOINCDescription, });

                        });
                    }
                }

                dfd.resolve(allRadiologyLabTests);
            });
        }
        else {
            dfd.resolve(allRadiologyLabTests);
        }

        return dfd.promise();

    },


    BindFacilityTo: function () {
        var Ctrl = $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtFacilityTo');
        var func = function () { return ReportsSSRSDashboard.GetFacilityArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #hfFacilityTo');
        var onChange = function () { ReportsSSRSDashboard.removeFacility($("#" + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtFacilityTo')) };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, null, onChange);
    },
    GetFacilityArrayByName: function (ShortName, IsAccountWithFullName, IsGetAll) {
        var AllPatients = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (ShortName != null && ShortName.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Patient_Referrals_Outgoing_Detail.LoadActiveFacilitiesByName(ShortName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllPatients.push({ id: item.FacilityId, value: item.Description });


                        });
                    }
                }

                dfd.resolve(AllPatients);
            });
        }
        else {
            dfd.resolve(AllPatients);
        }

        return dfd.promise();

    },
    removeFacility: function (ctrl) {
        if ($(ctrl).val() == "") {
            $("#" + ReportsSSRSDashboard.params["PanelID"] + ' #hfFacilityTo').val("");
        }

    },
    OpenFacilityTo: function () {
        //if ($("#" + ReportsSSRSDashboard.params.PanelID + " #txtFacilityTo").hasClass('ui-autocomplete-input')) {
        //    $("#" + ReportsSSRSDashboard.params.PanelID + " #txtFacilityTo").autocomplete("destroy");
        //}
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmSSRSReports";
        params["FacilityTo"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "ReportParamaters #RadiologyOrder #txtFacilityTo";
        params["RefHiddenIdCtrl"] = "ReportParamaters #RadiologyOrder #hfFacilityTo";
        params["LoadAllFacility"] = "True";
        params["ParentCtrl"] = "mstrTabReports";
        LoadActionPan('Admin_Facility', params);
    },
    saveAllLocations: function () {
        ReportsSSRSDashboard.Locations = [];
        ReportsSSRSDashboard.Locations = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #LocationDivReports').find("select option").map(function () {
            return {
                "Id": this.value,
                "Location": this.text,
                "facilityTypeId": $(this).attr("refvalue")
            }
        });
        $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtLocation').empty();
    },
    getLocationByFacilityType: function (facilityTypeId) {
        var locations = $.grep(ReportsSSRSDashboard.Locations, function (v) {
            return v.facilityTypeId == facilityTypeId;
        });
        return locations;
    },
    facilityTypeChange: function () {
        var locationCtrl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtLocation');
        var facilityTypeId = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtFacilityType').val();

        locationCtrl.empty();
        $.each(ReportsSSRSDashboard.getLocationByFacilityType(facilityTypeId), function (i, item) {
            locationCtrl.append($('<option />', { value: item.Id, html: item.Location, refvalue: item.facilityTypeId }));
        });
        locationCtrl.multiselect("refresh");
        locationCtrl.multiselect("rebuild");
    },

    facilityMU3Change: function () {
        var OrgIdCtrl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtFacilityOIDMU3');
        var FacilityMu3Ctrl = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtFacilityMU3');

        OrgIdCtrl.text($(FacilityMu3Ctrl).find("option:selected").attr("ExName"))
    },

    populateSpecimen: function () {

        var ddlSpecimen = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtSpecimen');


        //FIXME {Ali Awan}
        data = { StrID: "87187", ID: "31", StrID2: "Prefer", StrID3: "Specimen66010" };

        return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
            if (results["GetSpecimen"] != null) {
                results = JSON.parse(JSON.parse(results["GetSpecimen"]).data);
            }
            if (results) {

                if (ddlSpecimen != null) {

                    ddlSpecimen.empty();
                    $.each(results, function (j, result) {
                        ddlSpecimen.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                    });
                    ddlSpecimen.multiselect("refresh");
                    ddlSpecimen.multiselect("rebuild");
                }
            }
        });
    },
    populateAntimicrobial: function () {


        var ddlSpecimen = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtSpecimen');
        var ddlOrganism = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtOrganism');

        var ddlAntimicrobials = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #txtAntimicrobialBySpecimentAndOrganism');

        if (!$(ddlSpecimen).val() && !$(ddlOrganism).val()) {
            ddlAntimicrobials.empty();

            return;
        }
        data = { 'ID': $(ddlSpecimen).val(), 'ID2': $(ddlOrganism).val() };

        MDVisionService.lookups('GetAntimicrobialBySpecimentAndOrganism', true, data).done(function (results) {
            if (results["GetAntimicrobialBySpecimentAndOrganism"] != null) {
                results = JSON.parse(results["GetAntimicrobialBySpecimentAndOrganism"]);
            }
            if (results) {

                if (ddlAntimicrobials != null) {

                    ddlAntimicrobials.empty();
                    $.each(results, function (j, result) {
                        ddlAntimicrobials.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                    });
                    ddlAntimicrobials.multiselect("refresh");
                    ddlAntimicrobials.multiselect("rebuild");
                }
            }
        });
    },
    downloadXML: function () {
        if (ReportsSSRSDashboard.params.ReportType) {
            var param = new Object();
            var Components = [];

            //param["FacilityOID"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtFacilityOIDMU3').text()
            //param["Facility"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtFacilityMU3').val();
            //param["FacilityType"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtFacilityType').val();
            //param["Location"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtLocation').val();
            //param["StartDate"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #StartDate').val();
            //param["EndDate"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #EndDate').val();
            if (ReportsSSRSDashboard.params.ReportType == "AUPReport") {
                //param["AccountNumber"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AccountNumber').val();
                //param["Specimen"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtSpecimen').val();
                //param["Organism"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtOrganism').val();
                //param["AntimicrobialBySpecimentAndOrganism"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtAntimicrobialBySpecimentAndOrganism').val();
                Components.push({
                    componentId: -1,
                    componentName: "AUPReport",
                });
            }
            else {
                //  param["Antimicrobial"] = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #txtAntimicrobialMU3').val();
                Components.push({
                    componentId: -1,
                    componentName: "AROReport",
                });
            }
            var dfd = new $.Deferred();
            var AllPatients = [];
            var PatientId = 0;
            var AccountNo = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #AccountNumber').val()
            if (AccountNo) {
                utility.GetPatientArray(AccountNo, 0).done(function (response) {
                    response.filter(function (obj) {
                        if (obj.AccountNumber == AccountNo)
                            PatientId = obj.id;
                    });
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }

            $.when(dfd).then(function () {
                param["FromAdmin"] = "0";
                param["UserId"] = globalAppdata['AppUserId'];
                param["PatientId"] = PatientId;
                param["ParentCtrl"] = "ReportsSSRSDashboard";
                param["ProviderId"] = 0;
                param["Template"] = ReportsSSRSDashboard.params.ReportType;
                param["Components"] = Components;
                param["NoteId"] = 0;
                param["commandType"] = ReportsSSRSDashboard.params.ReportType;
                param["IsConfidential"] = false;
                var FormName = ReportsSSRSDashboard.ReportName;

                var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');

                param["QueryString"] = "ReportName=" + FormName.trim() + "&" + QueryString;
                data = JSON.stringify(param);
                MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                    var responseDetail = response = JSON.parse(response);
                    if (response.status != false) {
                        response.data = JSON.parse(response.data);
                        $("#" + ReportsSSRSDashboard.params.PanelID + " #frmSSRSReports #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                        param["XMLData"] = response.data.xmlData;
                        param["Encryption"] = false;
                        param["IncludeHashCode"] = false;
                        param["Password"] = "";
                        param["commandType"] = "DOWNLOAD";
                        param["SummaryType"] = "1"; // 1 for clinical Summary

                        data = JSON.stringify(param);
                        MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var zip = new JSZip();
                                var xml = zip.folder("XML");
                                xml.file("XMLData.xml", response.XMLByte, { base64: true });
                                var html = zip.folder("HTML");
                                html.file("htmlData.html", response.HTMLByte, { base64: true });
                                zip.generateAsync({ type: "blob" })
                                .then(function (content) {
                                    saveAs(content, ReportsSSRSDashboard.params.ReportType + ".zip");
                                });
                                utility.DisplayMessages("Report data Downloaded Successfully.", 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            });

        }
    },
    OpenProblemListScreen: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabReports";
        params["RefCtrl"] = "txtProblems";
        params["Parent"] = ReportsSSRSDashboard.params.PanelID;
        HiddenCtrl = 'hdnProblemDescription';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },
    VitalReportValidation: function (VitalName, FromCtrl, ToCtrl) {
        var FromValue = $("#" + ReportsSSRSDashboard.params.PanelID + " #ReportParamaters #VitalsAdvControls #" + FromCtrl).val();
        var ToValue = $("#" + ReportsSSRSDashboard.params.PanelID + " #ReportParamaters #VitalsAdvControls #" + ToCtrl).val();
        if ((FromValue && ToValue) && (parseInt(ToValue) < parseInt(FromValue))) {
            utility.DisplayMessages(VitalName + " To Should be Greater than " + VitalName + " From", 4);
            $("#" + ReportsSSRSDashboard.params.PanelID + "  #ReportParamaters #VitalsAdvControls #" + ToCtrl).val("");
        }
    },
}

