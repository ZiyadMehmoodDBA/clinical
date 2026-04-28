using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader;
using MDVision.IEHR.EMR.Model.ReportHeader;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Reports
{
    public class ReportsSsrsDashboard
    {
        private readonly BLLReports _bllReportObj;
        private readonly BLLAdminSecurity _bllAdminSecurityObj;
        public ReportsSsrsDashboard()
        {
            _bllReportObj = new BLLReports();
            _bllAdminSecurityObj = new BLLAdminSecurity();
        }

        #region Singleton
        private static ReportsSsrsDashboard _obj;
        public static ReportsSsrsDashboard Instance()
        {
            return _obj ?? (_obj = new ReportsSsrsDashboard());
        }

        #endregion

        #region "Reports Privilage"
        private string SearchReportsPrivalege_old(string moduleId)
        {
            string username = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            try
            {
                DataTable dtuserprivlages;
                DataTable dtmodules = new DataTable();
                dtmodules.Columns.Add("ModuleId", typeof(string));
                dtmodules.Columns.Add("ModuleName", typeof(string));
                DataTable dtmodules1 = new DataTable();
                dtmodules1.Columns.Add("ModuleId", typeof(string));
                dtmodules1.Columns.Add("ModuleName", typeof(string));
                if (username.ToUpper() == "MDVISION")
                {
                    var obj = _bllAdminSecurityObj.LoadModuleForms(0, "1", "Reports");
                    var dsModule = obj.Data;
                    dtuserprivlages = dsModule.ModuleForms;
                }
                else
                {
                    dtuserprivlages = MDVSession.Current.dtUserPrivileges;
                }
                DataView view = new DataView(dtuserprivlages);
                dtmodules1 = view.ToTable(true, "ModuleId", "ModuleName");
                view.RowFilter = "ModuleName like '%Report%'";
                DataTable forreportsrights = view.ToTable(true, "ModuleId", "ModuleName", "ReportSSRSId", "FormsId", "FormName");

                dtmodules = view.ToTable(true, "ModuleId", "ModuleName");


                var response = new
                {
                    status = true,
                    REPORTSMODULE_JSON = MDVUtility.JSON_DataTable(dtmodules),
                    REPORTSRIGHT_JSON = MDVUtility.JSON_DataTable(forreportsrights),// dtuserprivlages),
                    ALLOWMODULE_JSON = MDVUtility.JSON_DataTable(dtmodules1),
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string SearchReportsPrivalege(string moduleId)
        {
            string username = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            try
            {
                DataTable dtuserprivlages;

                DataTable dtmodules = new DataTable();
                dtmodules.Columns.Add("ModuleId", typeof(string));
                dtmodules.Columns.Add("ModuleName", typeof(string));

                DataTable dtmodules1 = new DataTable();
                dtmodules1.Columns.Add("ModuleId", typeof(string));
                dtmodules1.Columns.Add("ModuleName", typeof(string));

                if (username.ToUpper() == "MDVISION")
                {
                    var obj = _bllAdminSecurityObj.LoadModuleForms(0, "1", "Reports");
                    var dsModule = obj.Data;
                    dtuserprivlages = dsModule.ModuleForms;
                }
                else
                {
                    // Poor 
                    //dtuserprivlages = MDVSession.Current.dtUserPrivileges;
                    //dtuserprivlages = new DataTable();
                    //string[] members = { "ModuleFormId", "ModuleId", "ModuleName", "FormsId", "FormName", "MFRId", "MFUId", "ReportSSRSId" };
                    //using (var reader = ObjectReader.Create(MDVSession.Current.ListUserPrivileges, members))
                    //{
                    //    dtuserprivlages.Load(reader);
                    //}

                    dtuserprivlages = MDVUtility.ConvertListToDataTable(MDVSession.Current.ListUserPrivileges.ToList());
                }

                DataView view = new DataView(dtuserprivlages);
                dtmodules1 = view.ToTable(true, "ModuleId", "ModuleName");
                view.RowFilter = "ModuleName like '%Report%'";
                DataTable forreportsrights = view.ToTable(true, "ModuleId", "ModuleName", "ReportSSRSId", "FormsId", "FormName");

                dtmodules = view.ToTable(true, "ModuleId", "ModuleName");


                var response = new
                {
                    status = true,
                    REPORTSMODULE_JSON = MDVUtility.JSON_DataTable(dtmodules),
                    REPORTSRIGHT_JSON = MDVUtility.JSON_DataTable(forreportsrights),// dtuserprivlages),
                    ALLOWMODULE_JSON = MDVUtility.JSON_DataTable(dtmodules1),
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string SearchBillingModuleForms(string ModuleId, string UserId)
        {

            try
            {

                DSModuleForm dsModuleForm = null;
                BLObject<DSModuleForm> obj;
                obj = _bllAdminSecurityObj.LoadModuleFormsByUser(MDVUtility.ToLong(ModuleId), UserId);
                dsModuleForm = obj.Data;

                if (obj.Data != null)
                {
                    if (dsModuleForm.Tables[dsModuleForm.ModuleForms.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FormCount = dsModuleForm.Tables[dsModuleForm.ModuleForms.TableName].Rows.Count,
                            AssignedModuleForm_JSON = MDVUtility.JSON_DataTable(dsModuleForm.Tables[dsModuleForm.ModuleForms.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FormCount = 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ActionCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }
        #endregion

        #region "ReportsDetails
        private string GetReports_DetailsHTML(string reportName, string reportsParamaters)
        {
            try
            {
                string reportsDetailsHtml = string.Empty;
                reportName = reportName.ToLower().Trim();
                if (reportName.Equals("Advance Payment".ToLower().Trim()))
                {
                    // ReportsDetailsHTML = BLLReportObj.LoadAdvancePaymentsReport(result);
                    reportsDetailsHtml = _bllReportObj.GetReports_DetailsHTML(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Collected Copayment".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadCollectedCopaymentReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Unallocated copayment".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadUnallocatedCopaymentReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Diagnosis Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.GetDiagnosisAnalysisReports(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Patient List".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPatientListReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Billing Inquiry by Provider".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadBillingInquiryByProviderReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Procedure Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadProcedureAnalysisReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Outstanding Balances".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadOutStandingBalancesReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Insurance Analysis Summary".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadAInsuranceAnalysisSummaryReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Insurance Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadInsuranceAnalysisReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Total Resource appointments".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadTotalResourceAppointmentReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Total Follow-Up Appointments".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadTotalFollowupAppointmentsReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Total Provider appointments".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadTotalProviderAppointmentReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Enterprise Scheduling".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadEnterpriseSchedulingReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Daily Provider Appointments".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadDailyAppointmentsProviderReport(CreateReportsParams(reportsParamaters),
                            reportName);
                    // Adding Header Footer to Report, If Selected Provider is single and that provider has any Report Header | Change Implmeneted by Azhar Shahzad on Aug08/11/016
                    string providerIds = HttpUtility.ParseQueryString(reportsParamaters).Get("ProviderId");
                    if (!string.IsNullOrEmpty(providerIds) && providerIds.Split(',').Length == 1)
                    {
                        ReportHeader_TagsSelectModel model = ReportHeaderHelper.Instance()
                            .getReportHeaderTagsHTML(-1, MDVUtility.ToInt64(providerIds), -1, "Appointment Reminder");
                        reportsDetailsHtml = model.Header + reportsDetailsHtml + model.Footer;
                    }
                }
                else if (reportName.Equals("Daily Resource Appointments".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadDailyAppointmentsResourcesReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Check in And Check out Duration".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadCheckInAndCheckOutDurationReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Revenue By Facility".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadRevenueByFacilityReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Enterprise Revenue".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadEnterpriseRevenueReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Revenue By Provider".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadRevenueByProviderReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Provider Analysis By Plan".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadProviderAnalysisByPlanReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Charges List".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadChargesListReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Claims In Collection".ToLower().Trim()))
                {
                  
                    Dictionary<string, object> ReportsParamaters = CreateReportsParams(reportsParamaters);

                    var Group1name = ReportsParamaters.Where(x => x.Key.ToLower() == "@Group1Name".ToLower()).Select(x => x.Value.ToString());
                    var Group2name = ReportsParamaters.Where(x => x.Key.ToLower() == "@Group2Name".ToLower()).Select(x => x.Value.ToString());
                    if (Group1name.FirstOrDefault().ToLower() == "none" && Group2name.FirstOrDefault().ToLower() == "none")
                        reportsDetailsHtml = _bllReportObj.LoadClaimsInCollectionReport(CreateReportsParams(reportsParamaters), reportName);
                    else if (
                        (Group1name.FirstOrDefault().ToLower() == "none" && Group2name.FirstOrDefault().ToLower() != "none")
                        | (Group1name.FirstOrDefault().ToLower() != "none" && Group2name.FirstOrDefault().ToLower() == "none")
                        )
                    {
                        string GroupByColumn = string.Empty;
                        if (Group1name.FirstOrDefault().ToLower() == "patient balance" | Group2name.FirstOrDefault().ToLower() == "patient balance")
                            GroupByColumn = "PatientBalance";
                        if (Group1name.FirstOrDefault().ToLower() == "patient name" | Group2name.FirstOrDefault().ToLower() == "patient name")
                            GroupByColumn = "Patient";
                        reportsDetailsHtml = _bllReportObj.LoadClaimsInCollectionSingleGroup(CreateReportsParams(reportsParamaters), reportName, GroupByColumn);

                    }
                    else if (Group1name.FirstOrDefault().ToLower() != "none" && Group2name.FirstOrDefault().ToLower() != "none")
                    {
                        string Group1 = string.Empty, Group2 = string.Empty;

                        if (Group1name.FirstOrDefault().ToLower() == "patient name")
                            Group1 = "Patient";
                        if (Group1name.FirstOrDefault().ToLower() == "patient balance")
                            Group1 = "PatientBalance";
                        if (Group2name.FirstOrDefault().ToLower() == "patient name")
                            Group2 = "Patient";
                        if (Group2name.FirstOrDefault().ToLower() == "patient balance")
                            Group2 = "PatientBalance";
                        reportsDetailsHtml = _bllReportObj.LoadClaimsInCollectionMutlipleGroup(CreateReportsParams(reportsParamaters), reportName, Group1, Group2);
                    }

                }

                else if (reportName.Equals("Provider Procedure Utilization".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadProviderProcedureUtilizationReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Insurance Plan AR".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadInsurancePlanARReport(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Insurance AR Plan Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadInsuranceARPlanReport(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Patient AR".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPatientARReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Payment Entries".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPaymentEntriesReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Payment Entries MPS".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPaymentEntriesReportMPS(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Enterprise AR Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadEnterpriseARAnalysisReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("AR Aging Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadARAging_AnalysisReport(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("AR Aging Analysis MPS".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadARAging_AnalysisMPSReport(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Revenue By Plan".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadRevenueByPlanReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Financial Analysis At CPT Level".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadFinancialAnalysisAtCPTLevel(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("Beginning AR Ending AR".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadBeginningAREndingAR(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Payments by Users".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPaymentByUsers(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Aging Summary Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadAgingSummaryAnalysis(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("Encounter without Claims".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadEncounterWithoutClaims(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("Charges By Users".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadChargesbyUsers(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("Beginning AR Ending AR Facility".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadBeginningAREndingARFacility(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claim Comments By User".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadClaimCommentsbyUser(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("User Activity Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadUserActivityReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Claim Submit Status".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimSUbmitStatus(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Aging Detail Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadAgingDetailAnalysis(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Daily Appointment Reminders".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadDailyAppointmentReminderReport(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("AR Reconciliation Report Detial".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadARReconciliationReportDetail(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("AR Reconciliation Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadARReconciliationReport(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Patient Overpayment".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPatientOverPayment(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Zero Paid Claim".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadZeroPaidClaim(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Claim Over Paid By Insurance".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimOverPaidByInsurance(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Claim Under Paid By Insurance".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimUnderPaidByInsurance(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Anesthesia Overlapping".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadAnesthesiaOverlapping(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Historical Aging Summary Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadZeroPaidClaim(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("Y to Y Monthly Financial Summary Analysis".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadZeroPaidClaim(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("CDS Alert Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadCDSAlertReport(CreateReportsParams(reportsParamaters),
                        reportName);
                }
                else if (reportName.Equals("Daily Copay Sheet".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadDailyCopaySheetReport(
                        CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Incorrect Balance by Voided Claims".ToLower().Trim()))
                {
                    reportsDetailsHtml =
                        _bllReportObj.LoadIncorrectBalancebyVoidedClaims(CreateReportsParams(reportsParamaters),
                            reportName);
                }
                else if (reportName.Equals("Progress Note".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadProgressNoteReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claims Never submitted to Insurance".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimsNeverSubmitedReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Allergies".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadAllergiesReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Progress Note - Amendment".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadProgressNoteAmendementReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claim Status Dashboard".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimStatusDashboardDetail(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("POS Surveys".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPOSSurveyReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Unclaimed Appointments Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadUnclaimedAppointmentsReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Problems".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadProblemsReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Procedures".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadProceduresReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Phone Encounter".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadPhoneEncounterReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Immunization".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadImmunizationReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Medications".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadMedicationsReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Vitals".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadVitalsReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("User Audit Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadUserAuditReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Appointments Vs Claim".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadAppointmentVsClaimSummary(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Appointments Vs Claim Detail".ToLower().Trim()))
                {
                    Dictionary<string, object> ReportsParamaters = CreateReportsParams(reportsParamaters);

                    var Group1name = ReportsParamaters.Where(x => x.Key.ToLower() == "@Group1Name".ToLower()).Select(x => x.Value.ToString());
                    var Group2name = ReportsParamaters.Where(x => x.Key.ToLower() == "@Group2Name".ToLower()).Select(x => x.Value.ToString());
                    if (Group1name.FirstOrDefault().ToLower() == "none" && Group2name.FirstOrDefault().ToLower() == "none")
                        reportsDetailsHtml = _bllReportObj.LoadAppointmentVsClaimDetail(CreateReportsParams(reportsParamaters), reportName);
                    else if (
                        (Group1name.FirstOrDefault().ToLower() == "none" && Group2name.FirstOrDefault().ToLower() != "none")
                        | (Group1name.FirstOrDefault().ToLower() != "none" && Group2name.FirstOrDefault().ToLower() == "none")
                        )
                    {
                        string GroupByColumn = string.Empty;
                        if (Group1name.FirstOrDefault().ToLower() == "facility")
                            GroupByColumn = "facility";
                        if (Group1name.FirstOrDefault().ToLower() == "appiontment provider")
                            GroupByColumn = "Appt Provider";
                        if (Group2name.FirstOrDefault().ToLower() == "facility")
                            GroupByColumn = "facility";
                        if (Group2name.FirstOrDefault().ToLower() == "appiontment provider")
                            GroupByColumn = "Appt Provider";
                        reportsDetailsHtml = _bllReportObj.LoadAppointmentVsClaimDetailSingleGroup(CreateReportsParams(reportsParamaters), reportName, GroupByColumn);

                    }
                    else if (Group1name.FirstOrDefault().ToLower() != "none" && Group2name.FirstOrDefault().ToLower() != "none")
                    {
                        string Group1 = string.Empty, Group2 = string.Empty;

                        if (Group1name.FirstOrDefault().ToLower() == "facility")
                            Group1 = "facility";
                        if (Group1name.FirstOrDefault().ToLower() == "appiontment provider")
                            Group1 = "Appt Provider";
                        if (Group2name.FirstOrDefault().ToLower() == "facility")
                            Group2 = "facility";
                        if (Group2name.FirstOrDefault().ToLower() == "appiontment provider")
                            Group2 = "Appt Provider";
                        reportsDetailsHtml = _bllReportObj.LoadAppointmentVsClaimDetailMutlipleGroup(CreateReportsParams(reportsParamaters), reportName, Group1, Group2);
                    }
                   
                   
                }
                else if (reportName.Equals("Patient Statement Preference".ToLower().Trim()))
                {
                    Dictionary<string, object> ReportsParamaters = CreateReportsParams(reportsParamaters);

                    var Group1name = ReportsParamaters.Where(x => x.Key.ToLower() == "@Group1Name".ToLower()).Select(x => x.Value.ToString());
                    var Group2name = ReportsParamaters.Where(x => x.Key.ToLower() == "@Group2Name".ToLower()).Select(x => x.Value.ToString());
                    if (Group1name.FirstOrDefault().ToLower() == "none" && Group2name.FirstOrDefault().ToLower() == "none")
                        reportsDetailsHtml =_bllReportObj.LoadPatientStatementNoneGroup(CreateReportsParams(reportsParamaters), reportName);
                    else if (
                        (Group1name.FirstOrDefault().ToLower() == "none" && Group2name.FirstOrDefault().ToLower() != "none")
                        | (Group1name.FirstOrDefault().ToLower() != "none" && Group2name.FirstOrDefault().ToLower() == "none")
                        )
                    {
                        string GroupByColumn = string.Empty;
                        if (Group1name.FirstOrDefault().ToLower() == "Created Date".ToLower() | Group2name.FirstOrDefault().ToLower() == "Created Date".ToLower())
                            GroupByColumn = "CreatedDate";
                        if (Group1name.FirstOrDefault().ToLower() == "Patient Statement Status".ToLower() | Group2name.FirstOrDefault().ToLower() == "Patient Statement Status".ToLower())
                            GroupByColumn = "PatientStatementStatus";
                        reportsDetailsHtml =_bllReportObj.LoadPatientStatementSingleGroup(CreateReportsParams(reportsParamaters), reportName, GroupByColumn);

                    }
                    else if (Group1name.FirstOrDefault().ToLower() != "none" && Group2name.FirstOrDefault().ToLower() != "none")
                    {
                        string Group1 = string.Empty, Group2 = string.Empty;

                        if (Group1name.FirstOrDefault().ToLower() == "Created Date".ToLower())
                            Group1 = "CreatedDate";
                        if (Group1name.FirstOrDefault().ToLower() == "Patient Statement Status".ToLower())
                            Group1 = "PatientStatementStatus";
                        if (Group2name.FirstOrDefault().ToLower() == "Created Date".ToLower())
                            Group2 = "CreatedDate";
                        if (Group2name.FirstOrDefault().ToLower() == "Patient Statement Status".ToLower())
                            Group2 = "PatientStatementStatus";
                        reportsDetailsHtml = _bllReportObj.LoadPatientStatementMutlipleGroup(CreateReportsParams(reportsParamaters), reportName, Group1, Group2);
                    }
                }
                else if (reportName.Equals("Orders_Lab".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadOrdersLab(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Orders_Radiology".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadOrdersRadiology(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Orders_Procedure".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadOrdersProcedure(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Orders_Consultation".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadOrdersConsultation(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Orders_Prescription".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadOrdersPrescription(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Results_Lab".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadResultsLab(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Results_Radiology".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadResultsRadiology(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Results_Consultation".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadResultsConsultation(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claims Under Paid by Primary Insurance".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimUnderPaidByPrimaryInsurance(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("ARO Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadAROReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("AUP Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadAUPReport(CreateReportsParams(reportsParamaters), reportName);
                }

                else if (reportName.Equals("monthly payment trend".ToLower().Trim()))
                {
                   reportsDetailsHtml = _bllReportObj.LoadMonthlyPaymentTrend(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Secondary Claims Report".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadSecondaryInsuranceClaim(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claim Follow Up".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimFollowup(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claim Submission History".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimSubmissionHistoryReport(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claim Scrubber Errors".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadClaimScrubberErrors(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("MIPS Improvement Activity".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadMIPSImprovementActivity(CreateReportsParams(reportsParamaters), reportName);
                }
                else if (reportName.Equals("Claim Notes By User".ToLower().Trim()))
                {
                    reportsDetailsHtml = string.Empty;
                    //BLLReportObj.LoadClaimCommentsbyUser(CreateReportsParams(ReportsParamaters), ReportName);
                }
                else if (reportName.Equals("Drug Code Cost".ToLower().Trim()))
                {
                    reportsDetailsHtml = _bllReportObj.LoadDrugCodeCostReport(CreateReportsParams(reportsParamaters), reportName);
                }
                var response = new
                {
                    status = true,
                    ReportsDetailsHTML = reportsDetailsHtml

                    // Message = obj.Message
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {

                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }

        private Dictionary<string, object> CreateAdvancePaymentReports(string reportsParamaters)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in HttpUtility.ParseQueryString(reportsParamaters).AllKeys)
            {
                if (!key.Equals("ReportName"))
                {
                    var currentKey = key;
                    var strings = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    var values = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    if (values == null) continue;

                    string currentValue = strings != null && string.IsNullOrEmpty(strings[0]) ? string.Empty : HttpUtility.UrlDecode(values[0]);
                    if (currentKey.Equals("PaymentDateFrom") || currentKey.Equals("PaymentDateTo") || currentKey.Equals("DOSFrom") || currentKey.Equals("DOSTo") || currentKey.Equals("VisitEntryDateFrom") || currentKey.Equals("VisitEntryDateTo") || currentKey.Equals("DatePaidFrom") || currentKey.Equals("DatePaidTo") || currentKey.Equals("EntryDateFrom") || currentKey.Equals("EntryDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("SubmitDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("AppointmentDateStart") || currentKey.Equals("AppointmentDateEnd") || currentKey.Equals("VisitDateFrom") || currentKey.Equals("VisitDateTo") || currentKey.Equals("FromRegistrationDate") || currentKey.Equals("ToRegistrationDate") || currentKey.Equals("CreateDateFrom") || currentKey.Equals("CreateDateTo") || currentKey.Equals("LastVisitFrom") || currentKey.Equals("LastVisitTo") || currentKey.Equals("DOBFrom") || currentKey.Equals("DOBTo") || currentKey.Equals("DOSStart") || currentKey.Equals("DOSEnd"))
                    {
                        if (!string.IsNullOrEmpty(currentValue) && MDVUtility.IsDate(currentValue))
                        {
                            result.Add("@" + currentKey, MDVUtility.StringToDate(currentValue));
                        }
                        else
                        {
                            result.Add("@" + currentKey, DBNull.Value);
                        }
                    }
                    else if (currentKey.Equals("Balance"))
                    {
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            result.Add("@Balance", MDVUtility.ToLong(currentValue));
                        }
                        else
                        {
                            result.Add("@Balance", DBNull.Value);
                        }
                    }
                    else
                    {
                        result.Add("@" + key, currentValue);
                    }
                }
            }
            result.Add("@EntityId", MDVSession.Current.EntityId);
            return result;
        }

        private Dictionary<string, object> CreateReportsParams(string reportsParamaters)
        {
            //if (reportsParamaters == null) throw new ArgumentNullException(nameof(reportsParamaters));
            var result = new Dictionary<string, object>();
            foreach (string key in HttpUtility.ParseQueryString(reportsParamaters).AllKeys)
            {
                if (!key.Equals("ReportName"))
                {
                    string currentKey = key;
                    var strings = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    var values = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    if (values == null) continue;
                    string currentValue = strings != null && string.IsNullOrEmpty(strings[0]) ? string.Empty : HttpUtility.UrlDecode(values[0]);
                    if (currentKey.Equals("PaymentDateFrom") || currentKey.Equals("PaymentDateTo") || currentKey.Equals("DOSFrom") || currentKey.Equals("DOSTo") || currentKey.Equals("VisitEntryDateFrom") || currentKey.Equals("VisitEntryDateTo") || currentKey.Equals("DatePaidFrom") || currentKey.Equals("DatePaidTo") || currentKey.Equals("EntryDateFrom") || currentKey.Equals("EntryDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("SubmitDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("AppointmentDateStart") || currentKey.Equals("AppointmentDateEnd") || currentKey.Equals("VisitDateFrom") || currentKey.Equals("VisitDateTo") || currentKey.Equals("FromRegistrationDate") || currentKey.Equals("ToRegistrationDate") || currentKey.Equals("CreateDateFrom") || currentKey.Equals("CreateDateTo") || currentKey.Equals("LastVisitFrom") || currentKey.Equals("LastVisitTo") || currentKey.Equals("DOBFrom") || currentKey.Equals("DOBTo") || currentKey.Equals("DOSStart") || currentKey.Equals("DOSEnd") || currentKey.Equals("ProbGivenDateFrom") || currentKey.Equals("ProbGivenDateTo"))
                    {
                        if (!string.IsNullOrEmpty(currentValue) && MDVUtility.IsDate(currentValue))
                        {
                            result.Add("@" + currentKey, Convert.ToDateTime(currentValue));
                        }
                        else
                        {
                            result.Add("@" + currentKey, DBNull.Value);
                        }
                    }
                    else if (currentKey.Equals("Balance")  || currentKey.Equals("ICDCode"))
                    {
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            result.Add("@" + currentKey,
                                currentKey.Equals("Balance")
                                    ? MDVUtility.ToDouble(currentValue)
                                    : MDVUtility.ToLong(currentValue));
                        }
                        else
                        {
                            result.Add("@" + currentKey, DBNull.Value);
                        }
                    }
                    else
                    {
                        result.Add("@" + key, currentValue);
                    }
                }
            }
            result.Add("@EntityId", MDVSession.Current.EntityId);
            return result;
        }

        private Dictionary<string, object> CreateCollectedPaymentReportsParams(string reportsParamaters)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in HttpUtility.ParseQueryString(reportsParamaters).AllKeys)
            {
                if (!key.Equals("ReportName"))
                {
                    string currentKey = key;
                    var strings = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    var values = HttpUtility.ParseQueryString(reportsParamaters).GetValues(key);
                    if (values == null) continue;
                    var currentValue = strings != null && string.IsNullOrEmpty(strings[0]) ? string.Empty : HttpUtility.UrlDecode(values[0]);
                    if (currentKey.Equals("PaymentDateFrom") || currentKey.Equals("PaymentDateTo") || currentKey.Equals("DOSFrom") || currentKey.Equals("DOSTo") || currentKey.Equals("VisitEntryDateFrom") || currentKey.Equals("VisitEntryDateTo") || currentKey.Equals("DatePaidFrom") || currentKey.Equals("DatePaidTo") || currentKey.Equals("EntryDateFrom") || currentKey.Equals("EntryDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("SubmitDateTo") || currentKey.Equals("SubmitDateFrom") || currentKey.Equals("AppointmentDateStart") || currentKey.Equals("AppointmentDateEnd") || currentKey.Equals("VisitDateFrom") || currentKey.Equals("VisitDateTo") || currentKey.Equals("FromRegistrationDate") || currentKey.Equals("ToRegistrationDate") || currentKey.Equals("CreateDateFrom") || currentKey.Equals("CreateDateTo") || currentKey.Equals("LastVisitFrom") || currentKey.Equals("LastVisitTo") || currentKey.Equals("DOBFrom") || currentKey.Equals("DOBTo") || currentKey.Equals("DOSStart") || currentKey.Equals("DOSEnd"))
                    {
                        if (!string.IsNullOrEmpty(currentValue) && MDVUtility.IsDate(currentValue))
                        {
                            result.Add("@" + currentKey, MDVUtility.StringToDate(currentValue));
                        }
                        else
                        {
                            result.Add("@" + currentKey, DBNull.Value);
                        }
                    }

                    else
                    {
                        result.Add("@" + key, currentValue);
                    }
                }
            }
            result.Add("@EntityId", MDVSession.Current.EntityId);
            return result;
        }
        #endregion
        #region "Financial Analysis Report Pdf print

        /// <summary>
        /// Returning Report PDf from session
        /// </summary>
        /// <returns></returns>
        private string ShowReport()
        {
            try
            {
                var response = new
                {
                    status = true,
                    ReportPreview = HttpContext.Current.Session["CurrentReportView"] == null ? "" : HttpContext.Current.Session["CurrentReportView"].ToString(),
                };
                return (JsonConvert.SerializeObject(response));


            }
            catch (Exception)
            {
                var response = new
                {
                    status = false,
                    ReportPreview = string.Empty,
                };
                // throw ex;
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion
        #region Service Command Handler

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_REPORTS_PRIVILEGES":
                    {
                        string moduleId = context.Request["ModuleId"];
                        string strJsonData = SearchReportsPrivalege(moduleId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SETQUERYSTRING_REPORTS":
                    {
                        context.Session["Reports_QueryString"] = context.Request.Form;
                        var response = new { status = true, };
                        context.Response.ContentType = "text/plain";
                        context.Response.Write((JsonConvert.SerializeObject(response)));
                    }
                    break;

                case "GET_REPORTS_DETAILSHTML":
                    {
                        string reportName = context.Request["ReportName"];
                        string reportsParamaters = context.Request.Form.ToString();
                        var response = GetReports_DetailsHTML(reportName, reportsParamaters);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                case "FILTER_USERROLES_USERS":
                    {
                        string users = context.Request["Users"];
                        string userRoles = context.Request["UserRoles"];
                        string isActive = context.Request["IsActive"];
                        var response = GetUserRolesUser(isActive, userRoles, users);// GetReports_DetailsHTML(ReportName, ReportsParamaters);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                case "PREVIEW_REPORTS":
                    {
                        var response = ShowReport();// GetReports_DetailsHTML(ReportName, ReportsParamaters);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                case "SEARCH_BILLING_MODULE_FORM":
                    {
                        string moduleId = context.Request["moduleId"];
                        string UserId = context.Request["UserId"];
                        var response = SearchBillingModuleForms(moduleId, UserId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                        break;
                    }
            }
        }

        public string GetUserRolesUser(string isActive, string userRoles, string users)
        {
            StringBuilder userRolesOptions = new StringBuilder();
            StringBuilder usersOptions = new StringBuilder();
            try
            {
                BLObject<DSUserLookup> objUser = _bllAdminSecurityObj.LookupUserRolesUser(isActive, users, userRoles, userRoles);
                DSUserLookup ds = objUser.Data;

                if (ds.Tables[ds.UsersRoleSelect.TableName] != null)
                {
                    var usersRoleValues = ds.Tables[ds.UsersRoleSelect.TableName].AsEnumerable()
                        .Select(row => new
                        {
                            UserRoleId = row.Field<string>("UserRoleId"),
                            UsersRole = row.Field<string>("RoleName")
                        })
                        .Distinct();
                    var userValues = ds.Tables[ds.UsersRoleSelect.TableName].AsEnumerable()
                        .Select(row => new
                        {
                            UserId = row.Field<string>("UserId"),
                            UserName = row.Field<string>("UserName")
                        })
                        .Distinct();
                    foreach (var item in usersRoleValues)
                    {
                        userRolesOptions.Append("<option value='" + item.UserRoleId + "'>" + item.UsersRole + "</option>");
                    }
                    foreach (var item in userValues)
                    {
                        usersOptions.Append("<option value='" + item.UserId + "'>" + item.UserName + "</option>");
                    }
                }

                var response = new
                {
                    status = true,
                    UserRoles = userRolesOptions.ToString(),
                    Users = usersOptions.ToString()
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion
    }
}
